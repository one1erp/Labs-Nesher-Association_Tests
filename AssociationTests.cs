using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Linq;
using Association_Tests.Prompts;
using Common;
using DAL;
using LSSERVICEPROVIDERLib;
using One1.Controls;
using Telerik.WinControls;
using Telerik.WinControls.UI;
using Validators;
using XmlService;
using System.Data.Objects.DataClasses;
using Telerik.WinControls.Data;

namespace Association_Tests
{
    public partial class AssociationTests : UserControl
    {
        #region  fields

        private RadContextMenu CustomcontextMenu;
        private LabInfo _currentLab;
        private List<AliquotWrapper> _aliquotWrappers;
        private Sdg _currentSdg;
        private List<AliquotWrapper> _itemsToAdd;
        private List<Aliquot> _itemsToCancel;
        private List<TestTemplateEx> _testTemplateExs;
        private bool afterSave;
        private DataTable dataTable;
        private INautilusServiceProvider serviceProvider;
        private int staticColumnsCount;
        public IDataLayer dal
        {
            get;
            set;
        }
        public event Func<Sdg> sdgChanged1;

        private List<PhraseEntry> categories;
        //List<TestTemplateEx> listTestTemplateEx;
        bool isTestTemplateListLoaded = false;

        #endregion

        #region Ctor


        public AssociationTests()
        {
            InitializeComponent();

        }

        #endregion

        #region Initilaize



        internal void Init(Sdg CurrentSdg, INautilusUser _ntlsUser, INautilusServiceProvider sp)
        {
            try
            {


                //   SetNewContextMenu();

                dal = new DataLayer();

                dal.Connect();

                _currentSdg = dal.GetSdgByName(CurrentSdg.Name);

                _currentLab = _currentSdg.LabInfo;

                if (_currentLab == null)
                {
                    MessageBox.Show("לא הוגדרה מעבדה להזמנה");
                    ParentForm.Close();
                    return;
                }

                serviceProvider = sp;

                var currentOperator = GetcurrentOperator(); //for debug
                if (currentOperator != null) //todo:זמני בגלל בעיה של היוזר שחוזר ריק אחרי הדפסה שניה
                {
                    EnableButtons(currentOperator.Name == "lims_sys");
                }


                //מביא את הבדיקות לחיוב לפי מעבדה
                //   _testTemplateExs = dal.GetTestTemplatesForPriceList();
                _testTemplateExs = dal.GetTestTemplatesForPriceListIcludeWorkflow().OrderBy(x=>x.Name).ToList();
                _testTemplateExs.Sort((x, y) => x.Name.CompareTo(y.Name));


                _testTemplateExs = _testTemplateExs.Where(
                    tt => tt.RelevantLabs != null && tt.RelevantLabs.Contains(_currentLab.LabLetter)).ToList();



                var testTemplateWithoutWF = _testTemplateExs.Where(x => x.AliquotWorkflow == null);

                _sdgPriorityPhrase = dal.GetPhraseByName("Sdg Priority");
                // group סינון נוסף לפי ה 
                //ספי ביקש להוריד
                //הסינון היחיד יהיה לפי current lab letter
                //_testTemplateExs = _testTemplateExs.Where(templateEx => currentOperator.OperatorGroups
                //                                                            .Any(
                //                                                                operatorGroup =>
                //                                                                templateEx.AliquotWorkflow != null
                //                                                                  & templateEx.GroupId == operatorGroup.GroupId //בדיקה של ה WF group                                                                               
                //                                                               )).ToList();

                categories = dal.GetPhraseByName("Test Template Category").PhraseEntries.OrderBy(x=>x.ORDER_NUMBER).ToList();
                comboBoxCategory.DataSource = categories;
                comboBoxCategory.DisplayMember = "PhraseName";

                if (_testTemplateExs.Count < 1)
                {
                    MessageBox.Show("לא נמצאו בדיקות בקבוצת ההרשאה המתאימה. ");
                    ParentForm.Close();
                    return;
                }
                else
                {

                    //מציג הודעה למשתמש אם לא הוגדר WORKFLOW לבדיקה
                    CheckIfHasWorkflow(testTemplateWithoutWF);

                }

                InitilaizeGui();
                changeVisibleColumns();

                SortDescriptor descriptor = new SortDescriptor();
                descriptor.PropertyName = "Title";
                descriptor.Direction = ListSortDirection.Ascending;
                gridAssociationTests.SortDescriptors.Add(descriptor);

                ((GridTableElement)this.gridAssociationTests.TableElement).AlternatingRowColor = Color.LightGray;

            }
            catch (Exception exception)
            {
                Logger.WriteLogFile(exception);
                One1.Controls.CustomMessageBox.Show("Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ParentForm.Close();

            }
        }

        private void CheckIfHasWorkflow(IEnumerable<TestTemplateEx> testTemplateWithoutWF)
        {
            if (testTemplateWithoutWF.Count() < 1)
                return;
            string text = null;

            if (testTemplateWithoutWF.Count() == 1)
            {
                text = string.Format("לבדיקה {0}  לא הוגדר  workflow .היא לא תוצג במסך",
                    testTemplateWithoutWF.First().Name);
            }
            else if (testTemplateWithoutWF.Count() > 1)
            {
                string testNames = null;
                var c = testTemplateWithoutWF.Count();
                int i = 0;
                foreach (var test in testTemplateWithoutWF)
                {
                    testNames += test.Name;
                    i++;
                    if (i != c)
                    {
                        testNames += ", ";
                    }
                }

                text = string.Format("לבדיקות {0} לא הוגדרו  Workflows .הם לא יוצגו במסך", testNames);

            }
            CustomMessageBox.Show(text);

        }

        private List<AliquotTemplate> aliquotTemplates;

        private void InitilaizeGui()
        {
            _aliquotWrappers = new List<AliquotWrapper>();
            foreach (TestTemplateEx templateEx in _testTemplateExs)
            {
                WorkflowNode wn = templateEx.Workflow.WorkflowNodes.Where(w => w.ParentId == null).FirstOrDefault();
                AliquotTemplate at = dal.GetAliquotTemplateByWorkfloeNode(wn);

                List<Sample> samples = GetRelevantSamples();
                if (samples.Count > 0)
                {
                    foreach (Sample sample in samples.OrderBy(x => x.SampleId))
                    {
                        _aliquotWrappers.Add(new AliquotWrapper(at)
                                             {
                                                 SampleNameKey = sample.Name,
                                                 WorkFlowNameKey = templateEx.Workflow.Name,
                                                 TestTemplateExNameKey = templateEx.Name
                                             });
                    }
                }
                else
                {
                    CustomMessageBox.Show("לא נמצאו דוגמאות שניתנות לעריכה.");
                    ParentForm.Close();
                    return;
                }
            }

            CreateGrid();

            CreateDataTable();


        }

        private DAL.Operator GetcurrentOperator()
        {
            NautilusUser user = Utils.GetNautilusUser(serviceProvider);
            return dal.GetOPeratorByName(user.GetOperatorName());
        }

        private void EnableButtons(bool b)
        {
            btnSaveToLab.Enabled = b;
            btnSaveToClient.Enabled = true; // b;//Sefi changes to always true
            btnDeleteXmlToClient.Enabled = true; // b;//Sefi changes to always true
            btnDeleteXmlForLab.Enabled = b;
        }

        #endregion

        #region Create data

        /// <summary>
        /// Build grid
        /// </summary>
        private void CreateGrid()
        {
            //UNDONE insted of  _testTemplateExs.First().Name use _testTemplateExs.First().U_SHORT_NAME, find where to do it in the code.

            //UNDONE to the same this but with the manager diaplay

            //UNDONE sql query: update U_TEST_TEMPLATE_EX_USER set u_short_name=ALIQUOT_USER.U_SHORT_NAME WHERE  ALIQUOT_USER.U_TEST_TEMPLATE_EXTENDED = U_TEST_TEMPLATE_EX_USER.U_TEST_TEMPLATE_EX_ID;


            CreateStaticColumns();
            CreateDynamicColumns();
        }

        private void CreateStaticColumns()
        {

            gridAssociationTests.Columns.Clear();

            //3 עמודות קבועות
            gridAssociationTests.Columns.Add("מס", "מס", "מס");
            gridAssociationTests.Columns.Add("מספר פנימי", "מספר פנימי", "מספר פנימי");
            gridAssociationTests.Columns.Add("תיאור", "תיאור", "תיאור");

            staticColumnsCount = gridAssociationTests.Columns.Count;
        }

        private void CreateDynamicColumns()
        {

            TestTemplateEx tt = null;
            try
            {


                int index = staticColumnsCount;
                PhraseEntry specificCategory = (PhraseEntry)comboBoxCategory.SelectedValue;
                foreach (TestTemplateEx testTemplateEx in _testTemplateExs)
                {

                    tt = testTemplateEx;
                    var checkBoxColumn = new GridViewCheckBoxColumn(testTemplateEx.Name,
                        testTemplateEx.Name);
                    checkBoxColumn.TextImageRelation = TextImageRelation.ImageAboveText;
                    checkBoxColumn.HeaderText = testTemplateEx.Name; //.HeadLineEnglish;


                    checkBoxColumn.WrapText = true;

                    //Set header image
                    checkBoxColumn.HeaderImage = Resource1.AliquotV;


                    gridAssociationTests.Columns.Insert(index++, checkBoxColumn);


                }

                //Set properties to static columns
                for (int i = 0; i < staticColumnsCount; i++)
                {
                    gridAssociationTests.Columns[i].ReadOnly = true;
                    gridAssociationTests.Columns[i].IsPinned = true;
                    gridAssociationTests.Columns[i].AllowReorder = false;
                    gridAssociationTests.Columns[i].PinPosition = PinnedColumnPosition.Left;
                }
                //Set columns width
                foreach (GridViewDataColumn column in gridAssociationTests.Columns)
                {
                    column.MinWidth = 100;
                }

            }
            catch (Exception e)
            {
                Logger.WriteLogFile(tt.Name + " " + tt.HeadLineHebrew, false);
                throw;
            }
        }

        /// <summary>
        /// call a function that determines which columns of the 'gridAssociationTests' table will be visible according to the relevent category from 'comboBoxCategory'
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBoxCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            changeVisibleColumns();
        }

        private void changeVisibleColumns()
        {
            PhraseEntry specificCategory = (PhraseEntry)comboBoxCategory.SelectedValue;

            if (!isTestTemplateListLoaded)
            {
                //listTestTemplateEx = dal.GetAll<TestTemplateEx>().ToList();
                isTestTemplateListLoaded = true;
            }
            
            TestTemplateEx testTemplateEx;
            int forLoop = gridAssociationTests.Columns.Count;

            for (int j = staticColumnsCount; j < forLoop; j++)
            {
                string s = gridAssociationTests.Columns[j].Name;
                testTemplateEx = _testTemplateExs.Find(x => x.Name == s);
                //TestTemplateEx testTemplateEx = (TestTemplateEx)dal.GetAll<TestTemplateEx>().Where(x => x.Name == s).FirstOrDefault();
                if (testTemplateEx.U_CATEGORY == specificCategory.PhraseName || specificCategory.PhraseName == "None")
                {
                    gridAssociationTests.Columns[j].MinWidth = 100;
                    gridAssociationTests.Columns[j].MaxWidth = 100;
                    gridAssociationTests.Columns[j].Width = 100;
                    if (!gridAssociationTests.Columns[j].IsVisible)
                    {
                        gridAssociationTests.Columns[j].VisibleInColumnChooser = true;
                    }
                }
                else
                {
                    gridAssociationTests.Columns[j].MinWidth = 1;
                    gridAssociationTests.Columns[j].MaxWidth = 1;
                    gridAssociationTests.Columns[j].Width = 1;
                    if (!gridAssociationTests.Columns[j].IsVisible)
                    {
                        gridAssociationTests.Columns[j].VisibleInColumnChooser = false;
                    }
                }
            }


            this.BringToFront();

        }


        private void CreateDataTable()
        {
            dataTable = new DataTable();
            //Add static columns from grid  to data table
            for (int j = 0; j < staticColumnsCount; j++)
            {
                var col = new DataColumn(gridAssociationTests.Columns[j].Name, Type.GetType("System.String"));
                dataTable.Columns.Add(col);
            }
            //Add dynamic columns to data table
            for (int j = staticColumnsCount; j < gridAssociationTests.Columns.Count; j++)
            {

                dataTable.Columns.Add(new DataColumn(gridAssociationTests.Columns[j].Name,
                    Type.GetType(" System.Boolean")));
            }
            int count = 0;

            List<Sample> samples = GetRelevantSamples();


            foreach (Sample sample in samples.OrderBy(x => x.SampleId))
            {
                int cell = 0;
                count++;
                var array = new object[gridAssociationTests.Columns.Count];
                for (int i = 0; i < array.Length; i++)
                {
                    array[i] = false;
                }
                array[cell] = count;
                cell++;
                array[cell] = sample.Name;
                cell++;
                array[cell] = sample.Description;

                List<Aliquot> aliqouts =
                    sample.Aliqouts.Where(
                        aliquot => aliquot.Status != "X" && aliquot.Status != "R" && aliquot.Parent.Count == 0).ToList();
                foreach (Aliquot aliqout in aliqouts)
                {
                    // var a = aliqout.Children;
                    //var ab = aliqout.Parent;

                    cell = staticColumnsCount;
                    for (int i = staticColumnsCount; i < gridAssociationTests.Columns.Count; i++)
                    {
                        string fullName = gridAssociationTests.Columns[i].FieldName;

                        if (fullName == aliqout.TestTemplateEx.Name)
                        {
                            var aw = GetAliquotWrapperByKeys(aliqout.Sample.Name, aliqout.TestTemplateEx.Name);
                            aw.OldValue =
                                true;
                            aw.HasReTest = aliqout.Retest == "T";

                            array[cell] = true;
                            cell++;
                            break;
                        }
                        cell++;
                    }

                }

                dataTable.Rows.Add(array);
            }

            gridAssociationTests.DataSource = dataTable;

            dataTable.AcceptChanges();
            LoadLayoutXml();

            InitRetestControl();


        }

        private void InitRetestControl()
        {
            reTestControl1.Init(dal, _currentLab);
        }

        #endregion

        #region Events

        #region buttons  Events

        private void ButtonExitClick(object sender, EventArgs e)
        {
            if (dtChange(dataTable) || dal.HasChanges())
            {
                DialogResult dr = MessageBox.Show("?אתה עלול לאבד נתונים,האם ברצונך להמשיך", "Nautilus",
                    MessageBoxButtons.YesNo);
                if (dr == DialogResult.Yes)
                {
                    ParentForm.Close();
                    Dispose();
                }
            }
            else
            {
                ParentForm.Close();
                Dispose();
            }
        }


        private void btnSaveToClient_click(object sender, EventArgs e)
        {
            if (_currentSdg != null && _currentSdg.Client != null)
            {
                var clientId = _currentSdg.Client.ClientId;
                AddXml("CLIENT_ASSOCIATION_TESTS", clientId, true);
            }
            else
            {
                CustomMessageBox.Show("שמירת שדות למעבדה נכשלה,אנא פנה לתמיכה", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }

        }

        private string SaveTableOrder()
        {
            string orderColumns = "";
            for (int i = staticColumnsCount; i < gridAssociationTests.ColumnCount; i++)
            {
                if (gridAssociationTests.Columns[i].IsVisible)
                {
                    orderColumns +=
                        _testTemplateExs.First(x => x.Name == gridAssociationTests.Columns[i].FieldName).TestTemplateExId + ",";
                }
            }
            return orderColumns;


        }

        private void btnSaveToLab_Click(object sender, EventArgs e)
        {
            if (_currentLab != null)
            {

                AddXml("U_LABS_INFO", _currentLab.LabInfoId, false);
            }
            else
            {
                CustomMessageBox.Show("שמירת שדות למעבדה נכשלה,אנא פנה לתמיכה", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }


        }

        private void btnDeleteXmlToLab_Click(object sender, EventArgs e)
        {

            DeleteXml("U_LABS_INFO", _currentLab.LabInfoId, false);




        }

        private void btnDeleteXmlToClient_Click(object sender, EventArgs e)
        {
            DeleteXml("CLIENT_ASSOCIATION_TESTS", _currentSdg.Client.ClientId, true);


        }

        private void AddXml(string tableName, long recordId, bool byClient)
        {
            var xmlStoragedal = new DataLayer();
            try
            {
                //for (int i = staticColumnsCount; i < gridAssociationTests.ColumnCount; i++)
                //{
                //    gridAssociationTests.Columns[i].FieldName = gridAssociationTests.Columns[i].Name;
                //}

                xmlStoragedal.Connect();
                var newLayoutXml = UiHelperMethods.ConvertGridToByteArrray(gridAssociationTests);
                var xs = xmlStoragedal.GetXmlStorage(tableName, recordId, (long)_currentSdg.LabInfoId);
                if (xs == null)
                {
                    xs = new XmlStorage();
                    xs.TableName = tableName;
                    xs.EntityId = recordId;
                    xs.XmlData = newLayoutXml;
                    xs.LAB_ID = _currentSdg.LabInfoId;
                    xmlStoragedal.AddXmlStorage(xs);
                }
                else
                {
                    xs.XmlData = newLayoutXml;
                }
                var columnsOrder = SaveTableOrder();
                UpdateTableAssociationOrder(xmlStoragedal, recordId, columnsOrder, byClient);

                xmlStoragedal.SaveChanges();

                //for (int i = staticColumnsCount; i < gridAssociationTests.ColumnCount; i++)
                //{
                //    gridAssociationTests.Columns[i].Name = gridAssociationTests.Columns[i].FieldName;
                //}

            }
            catch (Exception ex)
            {
                CustomMessageBox.Show("הוספת XML נכשלה");
                Logger.WriteLogFile(ex);
            }
            finally
            {
                xmlStoragedal.Close();
            }

        }

        private void DeleteXml(string tableName, long recordId, bool byClient)
        {
            var xmlStoragedal = new DataLayer();
            try
            {

                xmlStoragedal.Connect();
                XmlStorage xs = xmlStoragedal.GetXmlStorage(tableName, recordId, (long)_currentSdg.LabInfoId);
                if (xs != null)
                {
                    xs.XmlData = null;
                    UpdateTableAssociationOrder(xmlStoragedal, recordId, "", byClient);

                    xmlStoragedal.SaveChanges();
                }

            }
            catch (Exception e)
            {

                CustomMessageBox.Show("מחיקת XML נכשלה");
                Logger.WriteLogFile(e);
            }
            finally
            {
                xmlStoragedal.Close();
            }
        }

        private void UpdateTableAssociationOrder(DataLayer dal, long recordId, string columnsOrder, bool byClient)
        {


            if (byClient)
            {
                var client = dal.GetClientByID(recordId);
                client.TableAssociationOrder = columnsOrder;
            }
            else //per lab
            {
                var lab = dal.GetLabs().FirstOrDefault(x => x.LabInfoId == recordId);
                lab.TableAssociationOrder = columnsOrder;
            }

        }

        #endregion

        private void panel1_Resize(object sender, EventArgs e)
        {
            panel1.Location = new Point(Width / 2 - panel1.Width / 2, panel1.Location.Y);
            lblHeader.Location = new Point(Width / 2 - lblHeader.Width / 2, lblHeader.Location.Y);
        }

        #region grid Events

        private void radGridView1_CellFormatting_1(object sender, CellFormattingEventArgs e)
        {

            //עיצוב שונה לעמודות הקבועות
            if (e.CellElement.ColumnInfo.Name == "מס" || e.CellElement.ColumnInfo.Name == "מספר פנימי" ||
                e.CellElement.ColumnInfo.Name == "תיאור")
            {
                e.CellElement.DrawFill = true;
                e.CellElement.NumberOfColors = 1;
                e.CellElement.BackColor = Color.Bisque;
                e.CellElement.IsPinned = true;

            }
            else if (e.CellElement.IsPinned)
            {
                if (e.CellElement.RowIndex % 2 == 0)
                {
                    e.CellElement.BackColor = ColorTranslator.FromHtml("#74b9ff");
                }
                else
                {
                    e.CellElement.BackColor = ColorTranslator.FromHtml("#0984e3");
                }
                e.CellElement.BorderColor = Color.Black;
                e.CellElement.BorderColor2 = Color.Black;
                e.CellElement.BorderColor3 = Color.Black;
                e.CellElement.BorderColor4 = Color.Black;
            }
            else
            {
                //בדיקה אם יש שדות RETEST
                string sampleName = e.CellElement.RowInfo.Cells["מספר פנימי"].Value.ToString();
                string workflowName = e.CellElement.ColumnInfo.FieldName;
                var aw = GetAliquotWrapperByKeys(sampleName, workflowName);
                if (aw != null && (aw.HasReTest || aw.AddedRetest) && (bool)e.CellElement.Value)
                {
                    SetCellValue(e.CellElement, ReTestColor);
                    return;
                }


                if (!afterSave)
                {
                    ResetCellValue(e.CellElement);


                }
                else //אם היה נסיון לשמור יסומנו שדות החובה באדום
                {


                    AliquotWrapper wrapper =
                        _itemsToAdd.FirstOrDefault(
                            x => x.SampleNameKey == sampleName && x.TestTemplateExNameKey == workflowName);


                    if (wrapper != null)
                    {



                        bool requiredField = //האם יש שדה חובה שלא הוכנס אליו ערך
                            wrapper.Prompts.Any(
                                x => x.IsMandatory && string.IsNullOrEmpty(x.GetValue()));
                        //TODO:לבדוק האם זה עובד גם על פרייז


                        if (requiredField)
                        {
                            SetCellValue(e.CellElement, MandatoryColor);
                            //סימון באדום של השדה חובה
                            //         e.CellElement.DrawFill = true;
                            //       e.CellElement.NumberOfColors = 1;
                            //     e.CellElement.BackColor = MandatoryColor;
                        }
                        else if (wrapper.HasReTest || wrapper.AddedRetest)
                        {
                            e.CellElement.DrawFill = true;
                            e.CellElement.NumberOfColors = 1;
                            e.CellElement.BackColor = ReTestColor;

                        }
                        else
                        {
                            //Reset cell element value
                            ResetCellValue(e.CellElement);
                        }
                    }
                    else
                    {
                        //Reset cell element value
                        ResetCellValue(e.CellElement);
                    }
                }
            }
        }

        private static void ResetCellValue(GridCellElement e)
        {
            e.ResetValue(LightVisualElement.DrawFillProperty, ValueResetFlags.Local);
            e.ResetValue(LightVisualElement.NumberOfColorsProperty, ValueResetFlags.Local);
            e.ResetValue(LightVisualElement.BackColorProperty, ValueResetFlags.Local);
        }
        private static void
            SetCellValue(GridCellElement e, Color color)
        {
            e.DrawFill = true;
            e.NumberOfColors = 1;
            e.BackColor = color;
        }

        private void gridAssociationTests_SelectionChanging(object sender, CancelEventArgs e)
        {

            promptPanel.Controls.Clear();
            GridDataCellElement cell = gridAssociationTests.CurrentCell;
            if (cell != null)
            {
                int rowIndex = cell.RowInfo.Index;
                int colIndex = cell.ColumnInfo.Index;
                CreatePrompts(rowIndex, colIndex, true);
            }
        }

        private const int diffY = 35;
        private const int diffX = 200;
        private const int constY = 20;






        private void CreatePrompts(int rowIndex, int colIndex, bool setOnPanel)
        {



            string sn = gridAssociationTests.Rows[rowIndex].Cells[1].Value.ToString();
            string ttexn = gridAssociationTests.Columns[colIndex].FieldName;

            AliquotWrapper wrapper = GetAliquotWrapperByKeys(sn, ttexn);
            if (wrapper == null)
                return;
            if (wrapper.OldValue) //אם הבדיקה קיימת כבר במערכת לא יוצגו השדות חובה
            {
                //  promptPanel.PanelContainer.Enabled = false;//ביינתים לא להעלות
                return;
            }


            AliquotTemplate aliquotTemplate = wrapper.AliquotTemplate;
            if (wrapper.Prompts.Count <= 0)
            {
                int i = 0;
                IOrderedEnumerable<AliquotTemplateField> fields =
                    aliquotTemplate.AliquotTemplateFields.OrderBy(x => x.OrderNumber);

                foreach (AliquotTemplateField field in fields)
                {
                    if (field.Displayed == "F" && field.Prompted == "F")
                    {
                        continue; //not display
                    }


                    BasePrompt prompt = null;

                    switch (field.SchemaFieldPropmpt.PromptType) //יצירת פקד לפי סוג השדה
                    {
                        case "E":
                            prompt = new PromptEntity();
                            SetEntityData(prompt, field);
                            break;
                        case "D":
                            prompt = new PromptDateTime();
                            break;
                        case "T":
                            prompt = new PromptText();
                            break;
                        case "B":
                            prompt = new PromptCheckBox();
                            break;
                        case "N":
                            prompt = new PromptsNum();
                            break;
                        case "P":
                            prompt = new PromptPhrase();
                            SetPhraseData(prompt, field);
                            break;
                        default:
                            return;
                    }
                    if (prompt != null)
                    {
                        if (setOnPanel)
                            CreateLabel(constY, diffY, i, field.PrompText);

                        prompt.PromptText = field.PrompText;
                        prompt.Prompted = field.Prompted == "T";
                        prompt.Displayed = field.Displayed == "T";
                        prompt.IsMandatory = field.Mandatory == "T";
                        if (!prompt.Prompted && prompt.Displayed)
                            prompt.Control.Enabled = false;
                        wrapper.Prompts.Add(prompt);
                        prompt.Control.Name = field.SchemaField.DataBaseName;
                        prompt.Control.Location = new Point(promptPanel.Location.X + diffX, (i * diffY) + constY);
                        if (prompt.Displayed && !prompt.Prompted)
                            prompt.Control.Enabled = false;
                        //Add control to panel
                        if (setOnPanel)
                            promptPanel.Controls.Add(prompt.Control);
                        //Set value to control
                        prompt.SetDefaultText(field);
                        prompt.DataBaseName = field.SchemaField.DataBaseName;
                        i++;
                    }
                }
            }
            else
            {
                int i = 0;
                foreach (BasePrompt prompt in wrapper.Prompts)
                {
                    if (setOnPanel)
                    {
                        CreateLabel(constY, diffY, i, prompt.PromptText);
                        prompt.Control.Location = new Point(promptPanel.Location.X + diffX, (i * diffY) + constY);
                        promptPanel.Controls.Add(prompt.Control);
                        i++;
                    }
                }
            }
        }

        private void gridAssociationTests_ContextMenuOpening(object sender, ContextMenuOpeningEventArgs e)
        {
            try
            {


                var cell = e.ContextMenuProvider as GridDataCellElement;
                if (cell != null)
                {

                    CustomcontextMenu = new RadContextMenu();
                    var markAllRow = new RadMenuItem();
                    markAllRow.Text = "סמן את כל השורה";
                    markAllRow.Click += markAllRow_Click;

                    var markAllColumn = new RadMenuItem();
                    markAllColumn.Text = "סמן את כל העמודה";
                    markAllColumn.Click += markAllColumn_Click;

                    var separator = new RadMenuSeparatorItem();
                    CustomcontextMenu.Items.Add(separator);
                    CustomcontextMenu.Items.Add(markAllRow);
                    CustomcontextMenu.Items.Add(markAllColumn);
                    var currentCell = gridAssociationTests.CurrentCell;
                    if (currentCell != null && ((bool)currentCell.Value))
                    {
                        var aw = GetAliquotWrapperByKeys(currentCell.RowInfo.Cells[1].Value.ToString(),
                            currentCell.ColumnInfo.Name);
                        if (!aw.HasReTest)
                        {
                            var reTest = new RadMenuItem();
                            if (aw.AddedRetest)
                            {
                                reTest.Text = "Cancel Retest";

                            }
                            else
                            {
                                reTest.Text = "ReTest";

                            }
                            reTest.Click += reTest_Click;
                            CustomcontextMenu.Items.Add(reTest);
                        }
                    }
                    e.ContextMenu = CustomcontextMenu.DropDown;
                }
                else
                {
                    //ביטול של התפריט שנפתח מקצה השורה.
                    var rowContext = e.ContextMenuProvider as GridRowHeaderCellElement;
                    if (rowContext != null)
                        e.ContextMenu = null;
                }
            }
            catch (Exception ex)
            {
                e.ContextMenu = null;

            }
        }

        private void SetNewContextMenu()
        {



        }

        private void reTest_Click(object sender, EventArgs e)
        {
            string sn = gridAssociationTests.CurrentCell.RowInfo.Cells[1].Value.ToString();
            string ttexn = gridAssociationTests.CurrentCell.ColumnInfo.Name;

            var aw = GetAliquotWrapperByKeys(sn, ttexn);
            aw.AddedRetest = !aw.AddedRetest;

            SetReTestColor(aw);
        }

        private void markAllRow_Click(object sender, EventArgs e)
        {
            int index = gridAssociationTests.CurrentRow.Index;
            for (int i = staticColumnsCount; i < gridAssociationTests.Columns.Count; i++)
            {
                GridViewCellInfo cell = gridAssociationTests.Rows[index].Cells[i];
                if (cell != null && cell.ColumnInfo.IsVisible)
                {

                    cell.Value = true;

                }
            }
        }

        private void markAllColumn_Click(object sender, EventArgs e)
        {
            //Gets specified cell indexes
            int currentColumnIndex = gridAssociationTests.CurrentCell.ColumnIndex;
            int currentRowIndex = gridAssociationTests.CurrentCell.RowIndex;

            gridAssociationTests.Rows[currentRowIndex].Cells[currentColumnIndex].Value = true;


            //Gets data abuot specified cell
            string sampleName = gridAssociationTests.Rows[currentRowIndex].Cells[1].Value.ToString();
            string testName = gridAssociationTests.Columns[currentColumnIndex].FieldName;
            AliquotWrapper currenwrapper = GetAliquotWrapperByKeys(sampleName, testName);

            //Copy data from specified cell to other cells in this column
            for (int i = 0; i < gridAssociationTests.Rows.Count; i++)
            {
                if (currentRowIndex != i)//don't do  this for current row
                {
                    //checked cell
                    gridAssociationTests.Rows[i].Cells[currentColumnIndex].Value = true;
                    //get cell index
                    var rowIndex = gridAssociationTests.Rows[i].Index;
                    //Create prompt to populate it data
                    CreatePrompts(rowIndex, currentColumnIndex, false);

                    //Get aliquot wrapper for this cell
                    string sn = gridAssociationTests.Rows[rowIndex].Cells[1].Value.ToString();
                    string ttexn = gridAssociationTests.Columns[currentColumnIndex].FieldName;
                    AliquotWrapper newWrapper = GetAliquotWrapperByKeys(sn, ttexn);


                    //Copy data to this cell
                    foreach (var prompt in currenwrapper.Prompts)
                    {
                        var p = newWrapper.Prompts.FirstOrDefault(x => x.DataBaseName == prompt.DataBaseName);
                        if (p != null)
                            p.SetValue(prompt.GetValue());

                    }


                }
            }

        }


        private void gridAssociationTests_CellBeginEdit(object sender, GridViewCellCancelEventArgs e)
        {
            //ניתן לשנות ערך רק לבדיקות לא קיימות.
            int rowIndex = e.RowIndex
                ;
            int colIndex = e.ColumnIndex;
            //Get sample name
            string sn = gridAssociationTests.Rows[rowIndex].Cells[1].Value.ToString();
            //get workflow name
            string ttexn = gridAssociationTests.Columns[colIndex].FieldName;
            AliquotWrapper wrapper = GetAliquotWrapperByKeys(sn, ttexn);
            if (wrapper.OldValue)
                e.Cancel = true;


        }

        #endregion

        #endregion

        #region Save tetst methods

        private Dictionary<bool, int> failedOrSuccessDic;
        private Color ReTestColor = Color.Purple;
        private Color MandatoryColor = Color.Red;
        private PhraseHeader _sdgPriorityPhrase;

        private void ButtonSaveTests_Click(object sender, EventArgs e)
        {
            afterSave = true;
            //For added tests
            _itemsToAdd = new List<AliquotWrapper>();
            //For canceled test
            _itemsToCancel = new List<Aliquot>();

            foreach (GridViewRowInfo row in gridAssociationTests.Rows)
            {
                for (int i = staticColumnsCount; i < gridAssociationTests.ColumnCount; i++)
                {
                    //Get sample name
                    string sampleName = row.Cells[1].Value.ToString() ?? null;


                    //get workflow name
                    string ttex = gridAssociationTests.Columns[i].Name;
                    AliquotWrapper wrapper = GetAliquotWrapperByKeys(sampleName, ttex);
                    if (wrapper == null)
                    {
                        MessageBox.Show("אנא פנה לתמיכה");
                        return;
                    }

                    wrapper.NewValue = (bool)row.Cells[i].Value;

                    if (wrapper.OldValue != wrapper.NewValue || wrapper.AddedRetest) //added test or added retest
                    {
                        if (wrapper.NewValue)
                        {
                            _itemsToAdd.Add(wrapper);
                        }
                        else
                        {
                            //Cancel aliquot
                            Sample sample =
                                (from item in _currentSdg.Samples
                                 where item.Name == sampleName
                                 select item).
                                    FirstOrDefault();
                            IEnumerable<Aliquot> aliquots = sample.Aliqouts.Where(x => x.Status != "X");

                            try
                            {
                                Aliquot aliq = (from aliquot in aliquots
                                                where
                                                    aliquot.TestTemplateEx.Name == ttex &&
                                                    aliquot.Status != "X"
                                                select aliquot).SingleOrDefault();

                                _itemsToCancel.Add(aliq);
                            }
                            catch (Exception exception)
                            {
                                CustomMessageBox.Show(
                                    "שויכה יותר מבדיקה  אחת עם אותו WORKFLOW אנא פנה לתמיכה  ");
                                throw exception;
                            }
                        }
                    }
                }
            }
            bool isValid = CheckMandatory();
            if (isValid)
            {
                Cancel();

                int count = _itemsToAdd.Count;
                if (count > 0)
                {
                    Login();

                    string message = string.Format("נוספו {0} בדיקות למערכת", failedOrSuccessDic[true]);
                    if (failedOrSuccessDic[false] > 0)
                    {
                        string failedMessage = string.Format("נכשלה הכנסת {0} בדיקות", failedOrSuccessDic[false]);
                        message += failedMessage;
                    }
                    buttonPrint.Enabled = true;

                    CustomMessageBox.Show(message, MessageBoxButtons.OK, MessageBoxIcon.Information);

                    //XML זמני : לא הצלחתי להביא את מה שנוצר מה 
                    //Dal ללא פתיחה מחודשת של ה  
                    dal.Close();
                    dal = new DataLayer();
                    dal.Connect();

                    _currentSdg = dal.GetSdgByName(_currentSdg.Name);

                }
                else
                {
                    CustomMessageBox.Show("לא נוספו בדיקות.");
                }


                InitilaizeGui();
                changeVisibleColumns();
            }
            else
            {
                CustomMessageBox.Show("אנא מלא שדות חובה!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                Focus();


                //Running an event that makes the red cells
                gridAssociationTests.Rows[0].Cells[0].BeginEdit();
                gridAssociationTests.Rows[0].Cells[0].EndEdit();
            }
        }

        /// <summary>
        /// ביטול בדיקות
        /// </summary>
        private void Cancel()
        {
            _itemsToCancel.ForEach(x => x.Status = "X");
            dal.SaveChanges();
            if (_itemsToCancel.Count > 0)
                MessageBox.Show(string.Format("בוטלו {0} בדיקות", _itemsToCancel.Count));
            _itemsToCancel.Clear();
        }

        private void Login()
        {
            failedOrSuccessDic = new Dictionary<bool, int>(); //בכדי לדעת כמה בדיקות הוזנו וכמה נכשלו
            failedOrSuccessDic.Add(true, 0);
            failedOrSuccessDic.Add(false, 0);
            foreach (AliquotWrapper wrapper in _itemsToAdd)
            {
                if (wrapper.OldValue != wrapper.NewValue)//if added test
                    failedOrSuccessDic[LoginAliquot(wrapper, false)]++;
                if (wrapper.AddedRetest)//if added retest
                    failedOrSuccessDic[LoginAliquot(wrapper, true)]++; //מוסיף בדיקה נוספת   
            }
            _itemsToAdd.Clear();
        }

        /// <summary>
        /// Login aliquot by xml processor
        /// </summary>
        /// <param name="aliquotWrapper"></param>
        /// <returns>If login success</returns>
        private bool LoginAliquot(AliquotWrapper aliquotWrapper, bool reTest)
        {
            var loginxml = new LoginXmlHandler(serviceProvider, "LOGIN ALIQUOT " + aliquotWrapper.WorkFlowNameKey);
            loginxml.CreateLoginChildXml("SAMPLE", aliquotWrapper.SampleNameKey, "ALIQUOT",
                aliquotWrapper.WorkFlowNameKey, FindBy.Name);

            //Add properties to login
            foreach (var field in GetFields(aliquotWrapper))
            {
                loginxml.AddProperties(field.Key, field.Value);
            }

            //For Environment lab
            if (_currentSdg.U_PRIORITY != null)
            {
                AddPriorityProperty(loginxml);
            }

            loginxml.AddProperties("U_TEST_TEMPLATE_EXTENDED", aliquotWrapper.TestTemplateExNameKey);
            loginxml.AddProperties("GROUP_ID", _currentSdg.LimsGroup.Name);
            loginxml.AddProperties("U_RETEST", reTest ? "T" : "F");
            if (reTest)
            {
                loginxml.AddProperties("U_CHARGE", "F");
            }



            // wfName.TestTemplateExes.First().Name);
            bool success = loginxml.ProcssXml();
            if (!success)
            {
                //Write error to log                
                Logger.WriteLogFile(loginxml.ErrorResponse, true);
            }
            return success;
        }

        private void AddPriorityProperty(LoginXmlHandler loginxml)
        {
            if (_sdgPriorityPhrase != null)
            {
                var pe = _sdgPriorityPhrase.PhraseEntries.FirstOrDefault(x => x.PhraseName == _currentSdg.U_PRIORITY);
                if (pe != null)
                    loginxml.AddProperties("PRIORITY", pe.PHRASE_INFO);
            }
        }


        /// <summary>
        /// Get all properties to login with aliquot
        /// </summary>
        /// <param name="aliquotWrapper"></param>
        /// <returns>Dictionary with property name and property value</returns>
        private Dictionary<string, string> GetFields(AliquotWrapper aliquotWrapper)
        {
            var dic = new Dictionary<string, string>();

            foreach (BasePrompt prompt in aliquotWrapper.Prompts)
            {

                var value = prompt.GetValue();
                if (value != null)
                {
                    bool b;
                    if (bool.TryParse(value, out b)) //if is boolean value 
                    {
                        //convert to nautilus boolean
                        value = ValidateItem.ConvertToNautilsuBoolean(Convert.ToBoolean(value)).ToString();
                    }
                    //Add to login xml
                    dic.Add(prompt.DataBaseName, value);
                }
            }
            return dic;
        }

        /// <summary>
        /// Checks mandatory fields
        /// </summary>
        /// <returns>True if all fields are entries</returns>
        private bool CheckMandatory()
        {
            foreach (AliquotWrapper aliquotWrapper in _itemsToAdd)
            {
                foreach (BasePrompt prompt in aliquotWrapper.Prompts)
                {

                    if (prompt.IsMandatory)
                    {
                        var v = prompt.GetValue();
                        if (string.IsNullOrEmpty(v))
                        {
                            return false;
                        }
                    }
                }
            }


            return true;
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Create label with prompt text
        /// </summary>
        /// <param name="constY"></param>
        /// <param name="diffY"></param>
        /// <param name="i"></param>
        /// <param name="PrompText"></param>
        private void CreateLabel(int constY, int diffY, int i, string PrompText)
        {
            //create label
            var label = new RadLabel();

            label.RightToLeft = RightToLeft.Inherit;
            label.Text = " : " + PrompText;
            label.Location = new Point(promptPanel.Location.X, (i * diffY) + 5 + constY);
            promptPanel.Controls.Add(label);
        }
        /// <summary>
        /// Populate drop down with data
        /// </summary>
        /// <param name="prompt"></param>
        /// <param name="field"></param>
        private void SetEntityData(BasePrompt prompt, AliquotTemplateField field)
        {
            if (prompt == null || field == null)
                return;
            var comboBoxColumn = (RadDropDownList)prompt.Control;
            comboBoxColumn.BindingContext = new BindingContext();
            Font f = comboBoxColumn.Font;
            string tableName = field.SchemaFieldPropmpt.SCHEMA_ENTITY.SCHEMA_TABLE.DATABASE_NAME;
            var details = dal.GetObjDetailses(tableName, null);
            comboBoxColumn.Items.Add("");
            List<Size> sizes = new List<Size>();

            if (details != null)
                foreach (ObjDetails obj in details)
                {
                    comboBoxColumn.Items.Add(obj.Name);
                    sizes.Add(TextRenderer.MeasureText(obj.Name, f));


                }
            var w = sizes.Max(x => x.Width) + 40;
            if (w < 100)
                w = 100;
            comboBoxColumn.Size = new Size(w, 100);

        }
        /// <summary>
        /// Populate drop down with data
        /// </summary>
        /// <param name="prompt"></param>
        /// <param name="field"></param>
        private void SetPhraseData(BasePrompt prompt, AliquotTemplateField field)
        {
            if (prompt == null || field == null)
                return;
            var comboBoxColumn = (RadDropDownList)prompt.Control;
            comboBoxColumn.BindingContext = new BindingContext();

            //בדיקה של ההגדרות בנאוטילוס האם יוצג 
            //phrase name or phrase descriptiob
            PromptPhrase pf = prompt as PromptPhrase;
            if (field.SchemaFieldPropmpt.InfoNum1 == 0)
                pf.DisplayDescription = true;


            pf.PhraseHeader = field.SchemaField.PhraseHeader;
            System.Drawing.Font f = comboBoxColumn.Font;
            List<Size> sizes = new List<Size>();
            IEnumerable<string> data;
            if (pf.DisplayDescription)
            {
                data = pf.PhraseHeader.PhraseEntries.Select(x => x.PhraseDescription);
            }
            else
            {
                data = pf.PhraseHeader.PhraseEntries.Select(x => x.PhraseName);
            }
            comboBoxColumn.Items.Add("");

            foreach (string phrase in data)
            {
                comboBoxColumn.Items.Add(phrase);
                sizes.Add(TextRenderer.MeasureText(phrase, f));
            }

            //Set combo box width
            var w = sizes.Max(x => x.Width) + 40;
            if (w < 100)
                w = 100;
            comboBoxColumn.Size = new Size(w, 100);
        }

        /// <summary>
        /// Get aliquot wrapper by keys
        /// </summary>
        /// <param name="sampleNameKey">sample name</param>
        /// <param name="WorkFlowNameKey">workflow name</param>
        /// <returns></returns>
        private AliquotWrapper GetAliquotWrapperByKeys(string sampleNameKey, string ttexNameKey)
        {
            AliquotWrapper aliquotWrapper = _aliquotWrappers.Where(
                x => x.SampleNameKey == sampleNameKey && x.TestTemplateExNameKey == ttexNameKey).FirstOrDefault();
            //if (aliquotWrapper == null)
            //{
            //    MessageBox.Show("Aliquot wrapper is null")
            //}

            return aliquotWrapper;
        }

        /// <summary>
        /// Check if has changes
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private bool dtChange(DataTable dt)
        {
            if (dt != null)
            {
                DataTable d3 = dt.GetChanges();

                if (d3 == null)
                {
                    return false;
                }
            }
            else
            {
                return false;
            }

            return true;
        }


        private void LoadLayoutXml()
        {
            //for (int j = staticColumnsCount; j < gridAssociationTests.Columns.Count; j++)
            //{

            //    string s = gridAssociationTests.Columns[j].HeaderText;
            //    TestTemplateEx ex = (TestTemplateEx)dal.GetAll<TestTemplateEx>().Where(x => x.U_SHORT_NAME == s).FirstOrDefault();
            //    //gridAssociationTests.Columns[j].HeaderText = ex.;
            //    gridAssociationTests.Columns[j].FieldName = ex.U_SHORT_NAME;
            //}

            var storage4Lab = GetCurrentXmlStorage4Lab();
            var hasXmlLab = storage4Lab != null && storage4Lab.XmlData != null;

            XElement dataXml;
            using (var stream = new MemoryStream())
            {
                gridAssociationTests.SaveLayout(stream);
                stream.Position = 0;
                var sr = new StreamReader(stream);
                dataXml = XElement.Load(sr);
            }



            string viewXmlStr;
            XElement viewXml = null;

            XmlStorage storage4Client = dal.GetXmlStorage("CLIENT_ASSOCIATION_TESTS", _currentSdg.Client.ClientId, (long)_currentSdg.LabInfoId);
            if (storage4Client != null && storage4Client.XmlData != null)
            {
                btnSaveToClient.ButtonElement.BorderElement.ForeColor = Color.Blue;
                if (hasXmlLab)
                    btnSaveToLab.ButtonElement.BorderElement.ForeColor = Color.Blue;
                var sr = new MemoryStream(storage4Client.XmlData);
                viewXml = XElement.Load(sr);
            }
            else
            {

                if (hasXmlLab)
                {
                    var sr = new MemoryStream(storage4Lab.XmlData);
                    viewXml = XElement.Load(sr);
                }
            }
            if (viewXml != null)
            {
                CompreTwoXml(viewXml, dataXml);

                //LoadImages();

            }

        }

        private XmlStorage GetCurrentXmlStorage4Lab()
        {

            var xmlStoragedal = new DataLayer();
            xmlStoragedal.Connect();
            var xs = xmlStoragedal.GetXmlStorage("U_LABS_INFO", _currentLab.LabInfoId, _currentLab.LabInfoId);
            xmlStoragedal.Close();

            return xs;
        }

        private void LoadImages()
        {


            foreach (GridViewDataColumn column in gridAssociationTests.Columns)
            {
                string s = column.HeaderText;
                column.TextImageRelation = TextImageRelation.ImageAboveText;

                column.HeaderText = s;

                column.WrapText = true;

                column.HeaderImage = Resource1.AliquotV;

            }

        }

        private void SetReTestColor(AliquotWrapper aw)
        {


            if (aw != null && (aw.HasReTest || aw.AddedRetest) && (bool)gridAssociationTests.CurrentCell.Value)
            {

                gridAssociationTests.CurrentCell.DrawFill = true;
                gridAssociationTests.CurrentCell.NumberOfColors = 1;
                gridAssociationTests.CurrentCell.BackColor = Color.Purple;

            }
            else
            {
                ResetCellValue(gridAssociationTests.CurrentCell);

            }
        }
        /// <summary>
        /// מביא רק את הדגימות שאליהם יהיה ניתן להוסיף בדיקות
        /// ורק הם יוצגו בטבלה
        /// </summary>
        /// <returns></returns>
        private List<Sample> GetRelevantSamples()
        {
            return _currentSdg.Samples.Where(s => s.Status == "C" || s.Status == "V" || s.Status == "P").OrderBy(s => s.SampleId).ToList();
        }


        private void CompreTwoXml(XElement viewXml, XElement dataXml)
        {
            //dataXml.Save(@"C:\Users\eilamd\Desktop\nesher projects\Association_Tests\image\befordataXml.xml");
            //viewXml.Save(@"C:\Users\eilamd\Desktop\nesher projects\Association_Tests\image\beforviewXml.xml");

            //ניגש לעמודות של האקסאמאל של התצוגה
            IEnumerable<XElement> viewXmlCha =
                from el in viewXml.Elements().Elements().Elements()
                select el;
            //ניגש לעמודות של האקסאמל של המידע
            IEnumerable<XElement> dataXmlCha =
                from el in dataXml.Elements().Elements().Elements()
                select el;
            var le1 = new List<XElement>();
            //לולאה שמורידה את העמודות המיותרות מהתצוגה
            foreach (XElement el in viewXmlCha)
            {
                bool delete = true;
                //צוברת את העמודות להסרה בתוך ליסט
                foreach (XElement xElement in dataXmlCha)
                {
                    XAttribute xAttribute = el.Attribute("FieldName");
                    XAttribute attribute = xElement.Attribute("FieldName");
                    if (attribute != null && (xAttribute != null && xAttribute.ToString() == attribute.ToString()))
                    {
                        delete = false;
                    }
                }
                if (delete)
                {
                    le1.Add(el);
                }
            }
            //מסירה אותם מהתצוגה
            foreach (XElement xElement in le1)
            {
                xElement.Remove();
                //                viewXml.Elements().Elements().FirstOrDefault().Remove
            }
            var le = new List<XElement>();
            //לולאה שמוסיפה עמודות לתצוגה לפי המידע הרלוונטי
            foreach (XElement el in dataXmlCha)
            {
                bool insert = true;
                //צובר לתוך רשימה את העמודות הרלוונטיות
                foreach (XElement xElement in viewXmlCha)
                {
                    XAttribute xAttribute = el.Attribute("FieldName");
                    XAttribute attribute = xElement.Attribute("FieldName");
                    if (attribute != null && (xAttribute != null && xAttribute.ToString() == attribute.ToString()))
                    {
                        insert = false;
                    }
                }
                if (insert)
                {
                    le.Add(el);
                }
            }
            //מוסיף מתוך הרשימה אל האקסאמל
            foreach (XElement xElement in le)
            {
                XElement firstOrDefault = viewXml.Elements().Elements().FirstOrDefault();
                if (firstOrDefault != null)
                    firstOrDefault.Add(xElement);
            }
            //            viewXml.Save(@"C:\Users\eilamd\Desktop\nesher projects\Association_Tests\image\viewXml.xml");
            //            dataXml.Save(@"C:\Users\eilamd\Desktop\nesher projects\Association_Tests\image\dataXml.xml");
            UiHelperMethods.LoadGridLayout(viewXml.ToString(), gridAssociationTests);
        }

        #endregion


        private void buttonPrint_Click(object sender, EventArgs e)
        {
            try
            {

                var xmlHen = new FireEventXmlHandler(serviceProvider);
                xmlHen.CreateFireEventXml("SDG", _currentSdg.SdgId, "הדפסת מדבקות");
                bool secses = xmlHen.ProcssXmlWithOutResponse();
                if (!secses)
                {
                    Logger.WriteLogFile(xmlHen.ErrorResponse, true);
                }
            }
            catch (Exception e1)
            {
                Logger.WriteLogFile(e1);
                One1.Controls.CustomMessageBox.Show("ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private void gridAssociationTests_CellValueChanged(object sender, GridViewCellEventArgs e)
        {

            var x = sender as GridViewCellInfo;
            if (x != null)
            {
                promptPanel.Controls.Clear();
                var row = x.RowInfo.Index;
                var col = x.ColumnInfo.Index;
                CreatePrompts(row, col, false);
            }

        }




    }
}