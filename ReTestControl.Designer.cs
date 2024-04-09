namespace Association_Tests
{
    partial class ReTestControl
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
            this.reTestPanel = new Telerik.WinControls.UI.RadGroupBox();
            this.lblSum = new Telerik.WinControls.UI.RadLabel();
            this.lblRetestToday = new Telerik.WinControls.UI.RadLabel();
            this.lblCreatedToday = new Telerik.WinControls.UI.RadLabel();
            this.radLabel3 = new Telerik.WinControls.UI.RadLabel();
            this.radLabel2 = new Telerik.WinControls.UI.RadLabel();
            this.radLabel1 = new Telerik.WinControls.UI.RadLabel();
            ((System.ComponentModel.ISupportInitialize)(this.reTestPanel)).BeginInit();
            this.reTestPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lblSum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblRetestToday)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCreatedToday)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).BeginInit();
            this.SuspendLayout();
            // 
            // reTestPanel
            // 
            this.reTestPanel.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this.reTestPanel.Controls.Add(this.lblSum);
            this.reTestPanel.Controls.Add(this.lblRetestToday);
            this.reTestPanel.Controls.Add(this.lblCreatedToday);
            this.reTestPanel.Controls.Add(this.radLabel3);
            this.reTestPanel.Controls.Add(this.radLabel2);
            this.reTestPanel.Controls.Add(this.radLabel1);
            this.reTestPanel.HeaderText = "מעבדה";
            this.reTestPanel.Location = new System.Drawing.Point(3, 3);
            this.reTestPanel.Name = "reTestPanel";
            this.reTestPanel.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            // 
            // 
            // 
            this.reTestPanel.RootElement.Padding = new System.Windows.Forms.Padding(2, 18, 2, 2);
            this.reTestPanel.Size = new System.Drawing.Size(294, 90);
            this.reTestPanel.TabIndex = 0;
            this.reTestPanel.Text = "מעבדה";
            this.reTestPanel.Click += new System.EventHandler(this.reTestPanel_Click);
            // 
            // lblSum
            // 
            this.lblSum.AutoSize = true;
            this.lblSum.Location = new System.Drawing.Point(5, 69);
            this.lblSum.Name = "lblSum";
            this.lblSum.Size = new System.Drawing.Size(2, 2);
            this.lblSum.TabIndex = 5;
            this.lblSum.TextAlignment = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblRetestToday
            // 
            this.lblRetestToday.AutoSize = true;
            this.lblRetestToday.Location = new System.Drawing.Point(5, 45);
            this.lblRetestToday.Name = "lblRetestToday";
            this.lblRetestToday.Size = new System.Drawing.Size(2, 2);
            this.lblRetestToday.TabIndex = 4;
            this.lblRetestToday.TextAlignment = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblCreatedToday
            // 
            this.lblCreatedToday.AutoSize = true;
            this.lblCreatedToday.Location = new System.Drawing.Point(5, 21);
            this.lblCreatedToday.Name = "lblCreatedToday";
            this.lblCreatedToday.Size = new System.Drawing.Size(2, 2);
            this.lblCreatedToday.TabIndex = 3;
            this.lblCreatedToday.TextAlignment = System.Drawing.ContentAlignment.TopRight;
            // 
            // radLabel3
            // 
            this.radLabel3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.radLabel3.AutoSize = true;
            this.radLabel3.Location = new System.Drawing.Point(257, 69);
            this.radLabel3.Name = "radLabel3";
            this.radLabel3.Size = new System.Drawing.Size(32, 18);
            this.radLabel3.TabIndex = 2;
            this.radLabel3.Text = "סה\"כ";
            this.radLabel3.TextAlignment = System.Drawing.ContentAlignment.TopRight;
            // 
            // radLabel2
            // 
            this.radLabel2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.radLabel2.AutoSize = true;
            this.radLabel2.Location = new System.Drawing.Point(145, 45);
            this.radLabel2.Name = "radLabel2";
            this.radLabel2.Size = new System.Drawing.Size(144, 18);
            this.radLabel2.TabIndex = 1;
            this.radLabel2.Text = "בדיקות חוזרות שנוצרו היום:";
            this.radLabel2.TextAlignment = System.Drawing.ContentAlignment.TopRight;
            // 
            // radLabel1
            // 
            this.radLabel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.radLabel1.AutoSize = true;
            this.radLabel1.Location = new System.Drawing.Point(182, 21);
            this.radLabel1.Name = "radLabel1";
            this.radLabel1.Size = new System.Drawing.Size(107, 18);
            this.radLabel1.TabIndex = 0;
            this.radLabel1.Text = "בדיקות שנוצרו היום:";
            this.radLabel1.TextAlignment = System.Drawing.ContentAlignment.TopRight;
            // 
            // ReTestControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.reTestPanel);
            this.Name = "ReTestControl";
            this.Size = new System.Drawing.Size(301, 95);
            ((System.ComponentModel.ISupportInitialize)(this.reTestPanel)).EndInit();
            this.reTestPanel.ResumeLayout(false);
            this.reTestPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lblSum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblRetestToday)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblCreatedToday)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.RadGroupBox reTestPanel;
        private Telerik.WinControls.UI.RadLabel lblSum;
        private Telerik.WinControls.UI.RadLabel lblRetestToday;
        private Telerik.WinControls.UI.RadLabel lblCreatedToday;
        private Telerik.WinControls.UI.RadLabel radLabel3;
        private Telerik.WinControls.UI.RadLabel radLabel2;
        private Telerik.WinControls.UI.RadLabel radLabel1;

    }
}
