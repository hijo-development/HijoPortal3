using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
//using DevExpress.XtraReports.Web;
using DevExpress.XtraReports.UI;
using HijoPortal.classes;
using HijoPortal.devexrep;

namespace HijoPortal
{
    public partial class hlsSOA_Print : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //var cachedReportSource = new CachedReportSourceWeb(new XtraReport1());
            //ASPxWebDocumentViewer1.OpenReport(cachedReportSource);
            if (!Page.IsPostBack)
            {
                //View_HLS_SOA(Convert.ToInt32(Session["SOA_PrintKey"]));
                switch (Session["DocumentViewer"])
                {
                    case "HLS_SOA":
                        {
                            View_HLS_SOA(Convert.ToInt32(Session["SOA_PrintKey"]));
                            XtraReportSOA reportSOA = new XtraReportSOA();
                            reportSOA.Parameters["PK"].Value = Convert.ToInt32(Session["SOA_PrintKey"]);
                            ASPxWebDocumentViewer1.OpenReport(reportSOA);
                            //ASPxWebDocumentViewer1.ReportSourceId = "HijoPortal.devexrep.XtraReportSOA";
                            break;
                        }
                    case "HLS_BillInv":
                        {
                            View_HLS_BillInv(Convert.ToInt32(Session["BillInv_PrintKey"]));
                            XtraReportBillInv reportBillInv = new XtraReportBillInv();
                            reportBillInv.Parameters["PK"].Value = Convert.ToInt32(Session["BillInv_PrintKey"]);
                            ASPxWebDocumentViewer1.OpenReport(reportBillInv);
                            //ASPxWebDocumentViewer1.ReportSourceId = "HijoPortal.devexrep.XtraReportBillInv";
                            break;
                        }
                }
            }                
        }
        
        private void View_HLS_SOA(int iSOAkey)
        {
            SqlConnection cn = new SqlConnection(GlobalClass.SQLConnString());
            string query = "";
            cn.Open();
            query = "SELECT * FROM tbl_HLS_StatementOfAccountReport WHERE (PK = " + iSOAkey + ")";
            SqlDataAdapter sqlAdSOA = new SqlDataAdapter(query, cn);
            query = "SELECT * FROM tbl_HLS_StatementOfAccountReport_Det WHERE (MasterKey = " + iSOAkey + ")";
            SqlDataAdapter sqlAdSOADet = new SqlDataAdapter(query, cn);
            query = "SELECT * FROM tbl_HLS_StatementOfAccountReport_Sub WHERE (MasterKey = " + iSOAkey + ")";
            SqlDataAdapter sqlAdSOASub = new SqlDataAdapter(query, cn);
            cn.Close();

            datasets.DataSetSOA hlsSOA = new datasets.DataSetSOA();
            sqlAdSOA.Fill(hlsSOA, "tbl_HLS_StatementOfAccountReport");
            sqlAdSOADet.Fill(hlsSOA, "tbl_HLS_StatementOfAccountReport_Det");
            datasets.DataSetSOASub hlsSOASub = new datasets.DataSetSOASub();
            sqlAdSOASub.Fill(hlsSOASub, "tbl_HLS_StatementOfAccountReport_Sub");
        }

        private void View_HLS_BillInv(int iBillInvKey)
        {
            SqlConnection cn = new SqlConnection(GlobalClass.SQLConnString());
            string query = "";
            cn.Open();
            query = "SELECT * FROM tbl_HLS_StatementOfAccountReportInv WHERE (PK = " + iBillInvKey + ")";
            SqlDataAdapter sqlAdBillInv = new SqlDataAdapter(query, cn);
            query = "SELECT * FROM tbl_HLS_StatementOfAccountReportInv_Det WHERE (MasterKey = " + iBillInvKey + ")";
            SqlDataAdapter sqlAdBillInvDet = new SqlDataAdapter(query, cn);
            cn.Close();

            datasets.DataSetBillInv hlsSOA = new datasets.DataSetBillInv();
            sqlAdBillInv.Fill(hlsSOA, "tbl_HLS_StatementOfAccountReportInv");
            sqlAdBillInvDet.Fill(hlsSOA, "tbl_HLS_StatementOfAccountReportInv_Det");
        }
    }
}