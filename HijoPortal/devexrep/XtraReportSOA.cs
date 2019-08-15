using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace HijoPortal.devexrep
{
    public partial class XtraReportSOA : DevExpress.XtraReports.UI.XtraReport
    {
        public XtraReportSOA()
        {
            
            InitializeComponent();
            //xrPictureBox1.ImageUrl = "~/images/hls.jpg";
            //xrPictureBoxLogo.ImageUrl = @"~/images/hls.jpg";
        }

        private void PageFooter_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {

        }
    }
}
