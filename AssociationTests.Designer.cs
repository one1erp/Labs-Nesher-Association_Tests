using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DAL;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace Association_Tests
{
    partial class AssociationTests
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
      
      

       

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.gridAssociationTests = new Telerik.WinControls.UI.RadGridView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.reTestControl1 = new Association_Tests.ReTestControl();
            this.buttonPrint = new System.Windows.Forms.Button();
            this.radButtonExit = new Telerik.WinControls.UI.RadButton();
            this.btnSaveTests = new Telerik.WinControls.UI.RadButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBoxCategory = new System.Windows.Forms.ComboBox();
            this.btnDeleteXmlToClient = new Telerik.WinControls.UI.RadButton();
            this.btnDeleteXmlForLab = new Telerik.WinControls.UI.RadButton();
            this.promptPanel = new Telerik.WinControls.UI.RadScrollablePanel();
            this.btnSaveToLab = new Telerik.WinControls.UI.RadButton();
            this.btnSaveToClient = new Telerik.WinControls.UI.RadButton();
            this.lblHeader = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.gridAssociationTests)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridAssociationTests.MasterTemplate)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonExit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnSaveTests)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnDeleteXmlToClient)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnDeleteXmlForLab)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.promptPanel)).BeginInit();
            this.promptPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnSaveToLab)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnSaveToClient)).BeginInit();
            this.SuspendLayout();
            // 
            // gridAssociationTests
            // 
            this.gridAssociationTests.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridAssociationTests.AutoSizeRows = true;
            this.gridAssociationTests.Location = new System.Drawing.Point(19, 73);
            // 
            // gridAssociationTests
            // 
            this.gridAssociationTests.MasterTemplate.AllowAddNewRow = false;
            this.gridAssociationTests.MasterTemplate.AllowDragToGroup = false;
            this.gridAssociationTests.MasterTemplate.AutoGenerateColumns = false;
            this.gridAssociationTests.MasterTemplate.EnableAlternatingRowColor = true;
            this.gridAssociationTests.MasterTemplate.EnableGrouping = false;
            this.gridAssociationTests.MasterTemplate.EnableSorting = false;
            this.gridAssociationTests.MasterTemplate.MultiSelect = true;
            this.gridAssociationTests.MasterTemplate.SelectionMode = Telerik.WinControls.UI.GridViewSelectionMode.CellSelect;
            this.gridAssociationTests.Name = "gridAssociationTests";
            this.gridAssociationTests.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            // 
            // 
            // 
            this.gridAssociationTests.RootElement.TextOrientation = System.Windows.Forms.Orientation.Horizontal;
            this.gridAssociationTests.Size = new System.Drawing.Size(1919, 536);
            this.gridAssociationTests.TabIndex = 3;
            this.gridAssociationTests.CellFormatting += new Telerik.WinControls.UI.CellFormattingEventHandler(this.radGridView1_CellFormatting_1);
            this.gridAssociationTests.CellBeginEdit += new Telerik.WinControls.UI.GridViewCellCancelEventHandler(this.gridAssociationTests_CellBeginEdit);
            this.gridAssociationTests.SelectionChanging += new System.ComponentModel.CancelEventHandler(this.gridAssociationTests_SelectionChanging);
            this.gridAssociationTests.CellValueChanged += new Telerik.WinControls.UI.GridViewCellEventHandler(this.gridAssociationTests_CellValueChanged);
            this.gridAssociationTests.ContextMenuOpening += new Telerik.WinControls.UI.ContextMenuOpeningEventHandler(this.gridAssociationTests_ContextMenuOpening);
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.reTestControl1);
            this.panel1.Controls.Add(this.buttonPrint);
            this.panel1.Controls.Add(this.radButtonExit);
            this.panel1.Controls.Add(this.btnSaveTests);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1993, 1647);
            this.panel1.TabIndex = 5;
            this.panel1.Resize += new System.EventHandler(this.panel1_Resize);
            // 
            // reTestControl1
            // 
            this.reTestControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.reTestControl1.Location = new System.Drawing.Point(40, 1548);
            this.reTestControl1.Name = "reTestControl1";
            this.reTestControl1.Size = new System.Drawing.Size(302, 98);
            this.reTestControl1.TabIndex = 38;
            // 
            // buttonPrint
            // 
            this.buttonPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonPrint.Location = new System.Drawing.Point(1576, 1577);
            this.buttonPrint.Margin = new System.Windows.Forms.Padding(2);
            this.buttonPrint.Name = "buttonPrint";
            this.buttonPrint.Size = new System.Drawing.Size(116, 24);
            this.buttonPrint.TabIndex = 0;
            this.buttonPrint.Text = "הדפסת מדבקות";
            this.buttonPrint.UseVisualStyleBackColor = true;
            this.buttonPrint.Click += new System.EventHandler(this.buttonPrint_Click);
            // 
            // radButtonExit
            // 
            this.radButtonExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.radButtonExit.Location = new System.Drawing.Point(1842, 1577);
            this.radButtonExit.Name = "radButtonExit";
            this.radButtonExit.Size = new System.Drawing.Size(110, 24);
            this.radButtonExit.TabIndex = 34;
            this.radButtonExit.Text = "יציאה";
            this.radButtonExit.Click += new System.EventHandler(this.ButtonExitClick);
            // 
            // btnSaveTests
            // 
            this.btnSaveTests.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSaveTests.Location = new System.Drawing.Point(1712, 1577);
            this.btnSaveTests.Name = "btnSaveTests";
            this.btnSaveTests.Size = new System.Drawing.Size(110, 24);
            this.btnSaveTests.TabIndex = 9;
            this.btnSaveTests.Text = "שמירה";
            this.btnSaveTests.Click += new System.EventHandler(this.ButtonSaveTests_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.comboBoxCategory);
            this.groupBox1.Controls.Add(this.gridAssociationTests);
            this.groupBox1.Controls.Add(this.btnDeleteXmlToClient);
            this.groupBox1.Controls.Add(this.btnDeleteXmlForLab);
            this.groupBox1.Controls.Add(this.promptPanel);
            this.groupBox1.Controls.Add(this.btnSaveToLab);
            this.groupBox1.Controls.Add(this.btnSaveToClient);
            this.groupBox1.Location = new System.Drawing.Point(20, 52);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1953, 1503);
            this.groupBox1.TabIndex = 35;
            this.groupBox1.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 13F);
            this.label1.Location = new System.Drawing.Point(776, 43);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 25);
            this.label1.TabIndex = 38;
            this.label1.Text = "קטגוריה:";
            // 
            // comboBoxCategory
            // 
            this.comboBoxCategory.FormattingEnabled = true;
            this.comboBoxCategory.Location = new System.Drawing.Point(636, 46);
            this.comboBoxCategory.Name = "comboBoxCategory";
            this.comboBoxCategory.Size = new System.Drawing.Size(121, 21);
            this.comboBoxCategory.TabIndex = 37;
            this.comboBoxCategory.SelectedIndexChanged += new System.EventHandler(this.comboBoxCategory_SelectedIndexChanged);
            // 
            // btnDeleteXmlToClient
            // 
            this.btnDeleteXmlToClient.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDeleteXmlToClient.Enabled = false;
            this.btnDeleteXmlToClient.Location = new System.Drawing.Point(1659, 43);
            this.btnDeleteXmlToClient.Name = "btnDeleteXmlToClient";
            this.btnDeleteXmlToClient.Size = new System.Drawing.Size(143, 24);
            this.btnDeleteXmlToClient.TabIndex = 36;
            this.btnDeleteXmlToClient.Text = "מחיקת XML ללקוח";
            this.btnDeleteXmlToClient.Click += new System.EventHandler(this.btnDeleteXmlToClient_Click);
            // 
            // btnDeleteXmlForLab
            // 
            this.btnDeleteXmlForLab.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDeleteXmlForLab.Enabled = false;
            this.btnDeleteXmlForLab.Location = new System.Drawing.Point(1010, 43);
            this.btnDeleteXmlForLab.Name = "btnDeleteXmlForLab";
            this.btnDeleteXmlForLab.Size = new System.Drawing.Size(143, 24);
            this.btnDeleteXmlForLab.TabIndex = 9;
            this.btnDeleteXmlForLab.Text = "מחיקת XML למעבדה";
            this.btnDeleteXmlForLab.Click += new System.EventHandler(this.btnDeleteXmlToLab_Click);
            // 
            // promptPanel
            // 
            this.promptPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.promptPanel.Font = new System.Drawing.Font("Segoe UI", 44F);
            this.promptPanel.Location = new System.Drawing.Point(19, 620);
            this.promptPanel.Name = "promptPanel";
            // 
            // 
            // 
            this.promptPanel.RootElement.Padding = new System.Windows.Forms.Padding(1);
            this.promptPanel.Size = new System.Drawing.Size(1919, 870);
            this.promptPanel.TabIndex = 35;
            this.promptPanel.Tag = "";
            this.promptPanel.Text = "Prompts";
            ((Telerik.WinControls.UI.RadScrollablePanelElement)(this.promptPanel.GetChildAt(0))).Padding = new System.Windows.Forms.Padding(1);
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.promptPanel.GetChildAt(0).GetChildAt(0))).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            // 
            // btnSaveToLab
            // 
            this.btnSaveToLab.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSaveToLab.Enabled = false;
            this.btnSaveToLab.Location = new System.Drawing.Point(1180, 43);
            this.btnSaveToLab.Name = "btnSaveToLab";
            this.btnSaveToLab.Size = new System.Drawing.Size(143, 24);
            this.btnSaveToLab.TabIndex = 8;
            this.btnSaveToLab.Text = "שמירת שדות למעבדה ";
            this.btnSaveToLab.Click += new System.EventHandler(this.btnSaveToLab_Click);
            // 
            // btnSaveToClient
            // 
            this.btnSaveToClient.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSaveToClient.Enabled = false;
            this.btnSaveToClient.Location = new System.Drawing.Point(1828, 43);
            this.btnSaveToClient.Name = "btnSaveToClient";
            this.btnSaveToClient.Size = new System.Drawing.Size(110, 24);
            this.btnSaveToClient.TabIndex = 7;
            this.btnSaveToClient.Text = "שמירת שדות ללקוח";
            this.btnSaveToClient.Click += new System.EventHandler(this.btnSaveToClient_click);
            // 
            // lblHeader
            // 
            this.lblHeader.AutoSize = true;
            this.lblHeader.Font = new System.Drawing.Font("David", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.lblHeader.ForeColor = System.Drawing.Color.Blue;
            this.lblHeader.Location = new System.Drawing.Point(434, 16);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblHeader.Size = new System.Drawing.Size(153, 21);
            this.lblHeader.TabIndex = 33;
            this.lblHeader.Text = "מסך שיוך בדיקות";
            // 
            // AssociationTests
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblHeader);
            this.Controls.Add(this.panel1);
            this.Name = "AssociationTests";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.Size = new System.Drawing.Size(1993, 1647);
            ((System.ComponentModel.ISupportInitialize)(this.gridAssociationTests.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridAssociationTests)).EndInit();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radButtonExit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnSaveTests)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnDeleteXmlToClient)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnDeleteXmlForLab)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.promptPanel)).EndInit();
            this.promptPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.btnSaveToLab)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnSaveToClient)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

     

        #endregion

        private Telerik.WinControls.UI.RadGridView gridAssociationTests;
        private Panel panel1;
        private RadButton btnSaveToClient;
        private RadButton btnSaveToLab;
        private RadButton btnSaveTests;
        private RadButton radButtonExit;
        private Label lblHeader;
        private GroupBox groupBox1;
        private RadScrollablePanel promptPanel;
        private RadButton btnDeleteXmlForLab;
        private RadButton btnDeleteXmlToClient;
        private Button buttonPrint;
        private ReTestControl reTestControl1;
        private Label label1;
        private ComboBox comboBoxCategory;
    }
}
