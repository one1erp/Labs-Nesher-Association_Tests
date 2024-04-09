using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using System.Diagnostics;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using Common;
using DAL;

namespace Association_Tests
{
    public partial class ReTestControl : UserControl
    {
        public ReTestControl()
        {
            InitializeComponent();






        }

        private void reTestPanel_Click(object sender, EventArgs e)
        {

        }

        private static int HOUR = 15;
        internal void Init(IDataLayer dal, LabInfo lab)
        {
            if (lab.GroupID == null) return;

            var now = DateTime.Now;


            var yesterday = now.AddDays(-1);

            DateTime d = new DateTime(yesterday.Year, yesterday.Month, yesterday.Day, HOUR, 00, 00);

            long g = lab.GroupID.Value;



            var samples = from sample in dal.GetSampleByGroup(g)
                          where sample.CREATED_ON > d
                          select sample;

            int retestCount = 0;
            int notretestCount = 0;
            foreach (Sample sample in samples)
            {
                if (sample.Aliqouts.Any(x => x.Retest == "T"))
                {
                    ++retestCount;
                }
                else
                {
               //     ++notretestCount;
                }
            }
            double sum = 1.1;
            notretestCount = samples.Count();
            lblCreatedToday.Text = notretestCount.ToString();
            lblRetestToday.Text = retestCount.ToString();
            if (notretestCount > 0 && retestCount > 0)
            {
                sum = (retestCount * 100) / notretestCount;
                Math.Round(sum, 2);
                lblSum.Text = sum + "%";
            }

            reTestPanel.Text = lab.Name;

        }

    }
}

