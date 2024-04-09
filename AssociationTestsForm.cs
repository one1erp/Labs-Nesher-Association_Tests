using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DAL;
using LSSERVICEPROVIDERLib;

namespace Association_Tests
{
    public partial class AssociationTestsForm : Form
    {
        private AssociationTests _associationTest;



        public AssociationTestsForm(Sdg CurrentSdg, INautilusUser _ntlsUser, INautilusServiceProvider ServiceProvider)
        {

           
          //  this.Size = new Size(1000, 700);

            this.Closed += AssociationTestsForm_Closed;
            _associationTest = new AssociationTests();
            _associationTest.Cursor = Cursors.Default;
            this.Controls.Add(_associationTest);
            _associationTest.Dock = DockStyle.Fill;
            if (WindowState == FormWindowState.Normal)
            {
                WindowState = FormWindowState.Maximized;
            }
            _associationTest.Init(CurrentSdg, _ntlsUser, ServiceProvider);
        }

        void AssociationTestsForm_Closed(object sender, EventArgs e)
        {
            if (_associationTest.dal != null) _associationTest.dal.Close();
            _associationTest.dal = null;
        }
    }
}
