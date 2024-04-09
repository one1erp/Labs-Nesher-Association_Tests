using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DAL;
using One1.Controls;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace Association_Tests.Prompts
{
    class PromptPhrase : BasePrompt
    {
    
       private readonly RadDropDownList dropDownList;
        private AliquotTemplateField field;
        public PhraseHeader PhraseHeader { get; set; }
        public bool DisplayDescription { get; set; }
        public PromptPhrase()
        {
            dropDownList = new RadDropDownList();
            dropDownList.Popup.AutoSize = true;
            dropDownList.AutoSize = true;
            dropDownList.SelectedValueChanged += new System.EventHandler(dropDownList_SelectedValueChanged);

        }



        void dropDownList_SelectedValueChanged(object sender, System.EventArgs e)
        {
            //    Value = dropDownList.Text;
        }

        public override RadControl Control
        {
            get { return dropDownList; }
        }

        public override void MarkedAsMandatory()
        {
            dropDownList.DropDownListElement.TextBox.BackColor = MandatoryColor;

        }


        public override void SetDefaultText(AliquotTemplateField field)
        {

            dropDownList.SelectedValue = field.DefaultText;

        }

        public override void SetValue(string value)
        {
            dropDownList.SelectedValue = value;

        }

        public override string GetValue()
        {


            if (dropDownList.SelectedItem == null) return null;
            if (DisplayDescription)
            {
                if (string.IsNullOrEmpty(dropDownList.SelectedItem.ToString()))
                    return null;
                return PhraseHeader.PhraseEntries.Where(x => x.PhraseDescription == dropDownList.SelectedItem.ToString()).FirstOrDefault().PhraseName;
            }
            else
            {
                return dropDownList.SelectedItem.ToString();
            }


        }
    }
}