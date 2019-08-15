using DevExpress.Web;
using HijoPortal.classes;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
//using CrystalDecisions.CrystalReports.Engine;
//using CrystalDecisions.Shared;
using System.Windows.Forms;

namespace HijoPortal
{
    public partial class hlsSOA_Add : System.Web.UI.Page
    {
        private void CheckCreatorKey()
        {
            if (Session["CreatorKey"] == null)
            {
                if (Page.IsCallback)
                    ASPxWebControl.RedirectOnCallback(MRPClass.DefaultPage());
                else
                    Response.Redirect("default.aspx");
                return;
            }
        }

        //private void LoadSOADetails(string sSOANum)
        //{

        //}

        private void LoadAddSOA(string sTrans, string sYear, string sWeekNum, string sCustCode, string sSOANum, string sUserKey)
        {
            DataTable dt = HLSSOA.Customer_Info(sCustCode);
            if (dt.Rows.Count> 0)
            {
                foreach (DataRow row in dt.Rows )
                {
                    txtCustCode.Text = row["CustCode"].ToString();
                    txtCustTIN.Text = row["CustTIN"].ToString();
                    txtCustomer.Text = row["CustName"].ToString();
                    txtCustomerAdd.Text = row["CustAdd"].ToString();
                    txtAttention.Text = row["CustAtt"].ToString();
                    txtAttentionNum.Text = row["CustAttNum"].ToString();
                }
            }
            dt.Clear();

            DataTable dt1 = HLSSOA.SOA_Footer(sSOANum, sTrans);
            if (dt1.Rows.Count > 0)
            {
                foreach (DataRow row in dt1.Rows)
                {
                    txtDate.Text = row["SOADate"].ToString();
                    txtYear.Text = row["Year"].ToString();
                    txtWeekNum.Text = row["WeekNum"].ToString();
                    txtSOANumber.Text = row["SOANum"].ToString();
                    txtBillingInvoice.Text = row["InvNum"].ToString();
                    txtBillingDate.Text = row["InvDate"].ToString();
                    txtRemarks.Text = row["Remarks"].ToString();
                    txtPreparedBy.Text = row["PreparedBy"].ToString();
                    txtPreparedByPost.Text = row["PreparedByPost"].ToString();
                    txtCheckedBy.Text = row["CheckedBy"].ToString();
                    txtCheckedByPost.Text = row["CheckedByPost"].ToString();
                    txtApprovedBy.Text = row["ApprovedBy"].ToString();
                    txtApprovedByPost.Text = row["ApprovedByPost"].ToString();
                }
            }
            dt1.Clear();

            HLSWaybill.DataSource = HLSSOA.HLSSOA_Details(sSOANum, sUserKey, sCustCode, sYear, sWeekNum);
            HLSWaybill.KeyFieldName = "Waybill";
            HLSWaybill.DataBind();

            switch (sTrans)
            {
                case "Add":
                    {
                        
                        Page.Title = "HLS - SOA (Add)";
                        HLSSOATitle.InnerText = "HLS - Statement Of Account (Add)";
                        txtDate.Text = DateTime.Now.ToString("MM/dd/yyyy");
                        txtYear.Text = sYear;
                        txtWeekNum.Text = sWeekNum;
                        txtRemarks.Text = "This is to bill the trucking services rendered for (Commodity: Fresh Bananas) Wk" + sWeekNum + ", details hereunder as follows:";
                        txtDate.ReadOnly = true; 
                        txtYear.ReadOnly = true;
                        txtWeekNum.ReadOnly = true;
                        txtCustomer.ReadOnly = true;
                        txtCustomerAdd.ReadOnly = true;
                        txtSOANumber.ReadOnly = false;
                        txtBillingInvoice.ReadOnly = true;
                        txtBillingDate.ReadOnly = true;
                        txtAttention.ReadOnly = false;
                        txtAttentionNum.ReadOnly = false;
                        txtRemarks.ReadOnly = false;
                        txtPreparedBy.ReadOnly = false;
                        txtPreparedByPost.ReadOnly = false;
                        txtCheckedBy.ReadOnly = false;
                        txtCheckedByPost.ReadOnly = false;
                        txtApprovedBy.ReadOnly = false;
                        txtApprovedByPost.ReadOnly = false;
                        BtnSaveSOA.Text = "S A V E";
                        //BtnSaveSOA.Enabled = true;
                        btnPrintInv.Visible = false;

                        HLSWaybillList.DataSource = HLSSOA.SOAWaybill_for_Save(sUserKey);
                        HLSWaybillList.KeyFieldName = "Waybill";
                        HLSWaybillList.DataBind();
                        

                        break;
                    }
                case "View":
                    {
                        Page.Title = "HLS - SOA (View)";
                        HLSSOATitle.InnerText = "HLS - Statement Of Account (View)";
                        txtDate.ReadOnly = true;
                        txtYear.ReadOnly = true;
                        txtWeekNum.ReadOnly = true;
                        txtCustomer.ReadOnly = true;
                        txtCustomerAdd.ReadOnly = true;
                        txtSOANumber.ReadOnly = true;
                        txtBillingInvoice.ReadOnly = true;
                        txtBillingDate.ReadOnly = true;
                        txtAttention.ReadOnly = true;
                        txtAttentionNum.ReadOnly = true;
                        txtRemarks.ReadOnly = true;
                        txtPreparedBy.ReadOnly = true;
                        txtPreparedByPost.ReadOnly = true;
                        txtCheckedBy.ReadOnly = true;
                        txtCheckedByPost.ReadOnly = true;
                        txtApprovedBy.ReadOnly = true;
                        txtApprovedByPost.ReadOnly = true;
                        BtnSaveSOA.Text = "Print S O A";
                        btnPrintInv.Visible = true;
                        //BtnSaveSOA.Enabled = false;

                        //HLSWaybillList.DataSource = HLSSOA.SOAWaybill_for_Save(sUserKey);
                        //HLSWaybillList.KeyFieldName = "Waybill";
                        //HLSWaybillList.DataBind();

                        break;
                    }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            
            CheckCreatorKey();
            if (!Page.IsPostBack)
            {
                LoadAddSOA(Session["HLS_Trans"].ToString(), Session["HLS_Add_Year"].ToString(), Session["HLS_Add_WeekNum"].ToString(), Session["HLS_Add_CustCode"].ToString(), Session["HLS_Add_SOANum"].ToString(), Session["CreatorKey"].ToString());
            }
        }

        protected void BtnBackSOA_Click(object sender, EventArgs e)
        {
            Response.Redirect("hlsSOA.aspx");
        }

        protected void BtnSaveSOA_Click(object sender, EventArgs e)
        {
            if (BtnSaveSOA.Text == "S A V E")
            {
                HLSSOA.Save_HLSSOA(txtSOANumber.Text, Convert.ToDateTime(txtDate.Text), Convert.ToInt32(txtYear.Text), Convert.ToInt32(txtWeekNum.Text), txtCustCode.Text, txtRemarks.Text, txtPreparedBy.Text, txtPreparedByPost.Text, txtCheckedBy.Text, txtCheckedByPost.Text, txtApprovedBy.Text, txtApprovedByPost.Text, HLSWaybillList, txtAttention.Text, txtAttentionNum.Text);

                if (HLSSOA.Saving_Error == "")
                {
                    Session["HLS_Trans"] = "View";
                    Session["HLS_Add_Year"] = txtYear.Text;
                    Session["HLS_Add_WeekNum"] = txtWeekNum.Text;
                    Session["HLS_Add_CustCode"] = txtCustCode.Text;
                    Session["HLS_Add_SOANum"] = txtSOANumber.Text;
                    Response.Redirect("hlsSOA_Add.aspx");
                }

                //Session["HLS_Add_Year"] = HLSSOAList.GetRowValues(HLSSOAList.FocusedRowIndex, "Year").ToString();
                //Session["HLS_Add_WeekNum"] = HLSSOAList.GetRowValues(HLSSOAList.FocusedRowIndex, "WeekNum").ToString();
                //Session["HLS_Add_CustCode"] = HLSSOAList.GetRowValues(HLSSOAList.FocusedRowIndex, "CustCode").ToString();
                //Session["HLS_Add_SOANum"] = HLSSOAList.GetRowValues(HLSSOAList.FocusedRowIndex, "SOANum").ToString();
                //Response.Redirect("hlsSOA_Add.aspx");
                //Response.RedirectLocation = "hlsSOA_Add.aspx";

                //Response.Redirect("hlsSOA.aspx");
            }
            if (BtnSaveSOA.Text == "Print S O A")
            {
                //ModalPopupExtenderLoading.Hide();

                HLSSOA.Print_HLS_SOA(Convert.ToInt32(Session["CreatorKey"]), HLSWaybill, txtSOANumber.Text, txtDate.Text, txtCustomer.Text, txtCustomerAdd.Text, txtAttention.Text, txtAttentionNum.Text, txtRemarks.Text, txtPreparedBy.Text, txtPreparedByPost.Text, txtCheckedBy.Text, txtCheckedByPost.Text, txtApprovedBy.Text, txtApprovedByPost.Text);

                Session["SOA_PrintKey"] = HLSSOA.hlsSOAPrintKey.ToString();
                Session["DocumentViewer"] = "HLS_SOA";

                //View_HLS_SOA(HLSSOA.hlsSOAPrintKey);

                //SqlConnection cn = new SqlConnection(GlobalClass.SQLConnString());

                //cn.Open();
                //string query = "SELECT * FROM tbl_HLS_StatementOfAccountReport WHERE (PK = " + HLSSOA.hlsSOAPrintKey + ")";
                //SqlDataAdapter sqlAd = new SqlDataAdapter(query, cn);
                //cn.Close();

                //datasets.DataSetSOA hlsSOA = new datasets.DataSetSOA();
                //sqlAd.Fill(hlsSOA, "tbl_HLS_StatementOfAccountReport");

                //ReportDocument myRpt = new ReportDocument();
                //myRpt.Load(Server.MapPath("~/crystalrep/crystalRep_SOA.rpt"));
                //myRpt.SetDataSource(hlsSOA);
                ////myRpt.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, "hlsSOA");

                //PrintDialog dialog1 = new PrintDialog();
                //dialog1.AllowSomePages = true;
                //dialog1.AllowPrintToFile = false;

                //if (dialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                //{
                //    int copies = dialog1.PrinterSettings.Copies;
                //    int fromPage = dialog1.PrinterSettings.FromPage;
                //    int toPage = dialog1.PrinterSettings.ToPage;
                //    bool collate = dialog1.PrinterSettings.Collate;

                //    myRpt.PrintOptions.PrinterName = dialog1.PrinterSettings.PrinterName;
                //    myRpt.PrintToPrinter(copies, collate, fromPage, toPage);
                //}

                //myRpt.Dispose();
                //dialog1.Dispose();


                //myRpt.PrintToPrinter(1, false, 0, 1);

                Response.Redirect("hlsSOA_Print.aspx");
                //Response.Redirect("hlsSOA_PrintCrys.aspx");

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

            cn.Close();

            datasets.DataSetSOA hlsSOA = new datasets.DataSetSOA();
            sqlAdSOA.Fill(hlsSOA, "tbl_HLS_StatementOfAccountReport");
            sqlAdSOADet.Fill(hlsSOA, "tbl_HLS_StatementOfAccountReport_Det");

            //XtraReport myRpt = new XtraReport();
            //myRpt.DataSource = hlsSOA;

            //ASPxWebDocumentViewer1.OpenReport();

            //XtraReport1 myRpt = new XtraReport1();
            //myRpt.DataSource = hlsSOA;
        }

        protected void btnPrintInv_Click(object sender, EventArgs e)
        {
            if (txtBillingInvoice.Text != "" && txtBillingDate.Text != "")
            {
            //    txtPrintBillInvDate.Value = DateTime.Now.ToString("MM/dd/yyyy");
            //    txtPrintBillInv.Text = "";
            //    PopUpControlPrintInv.HeaderText = "Print Billing Invoice";
            //    PopUpControlPrintInv.ShowOnPageLoad = true;
            //    txtPrintBillInv.Focus();
            //} else
            //{
                HLSSOA.Print_HLS_BillInv(Convert.ToInt32(Session["CreatorKey"]), txtBillingDate.Text, txtCustCode.Text, txtCustomer.Text, txtCustomerAdd.Text, txtCustTIN.Text, txtYear.Text, txtWeekNum.Text, txtPreparedBy.Text, txtApprovedBy.Text, txtSOANumber.Text);

                Session["BillInv_PrintKey"] = HLSSOA.hlsBillInvPrintKey.ToString();
                Session["DocumentViewer"] = "HLS_BillInv";
                Response.Redirect("hlsSOA_Print.aspx");
            }
        }

        protected void BtnPrintInvOK_Click(object sender, EventArgs e)
        {
            SqlConnection cn = new SqlConnection(GlobalClass.SQLConnString());
            SqlCommand cmdIns = null;
            DataTable dt = new DataTable();
            SqlCommand cmd = null;
            SqlDataAdapter adp;
            string qry = "";

            cn.Open();
            qry = "SELECT tbl_HLS_StatementOfAccount.* FROM tbl_HLS_StatementOfAccount WHERE (BillingInvoiceNum = '" + txtPrintBillInv.Text + "')";
            cmd = new SqlCommand(qry);
            cmd.Connection = cn;
            adp = new SqlDataAdapter(cmd);
            adp.Fill(dt);
            if (dt.Rows.Count == 0)
            {
                cmdIns = new SqlCommand("UPDATE tbl_HLS_StatementOfAccount SET BillingInvoiceNum = @BillingInvoiceNum, BillingInvoiceDate = @BillingInvoiceDate WHERE (SOANum = @SOANum)", cn);
                cmdIns.Parameters.AddWithValue("@SOANum", txtSOANumber.Text);
                cmdIns.Parameters.AddWithValue("@BillingInvoiceNum", txtPrintBillInv.Text);
                cmdIns.Parameters.AddWithValue("@BillingInvoiceDate", txtPrintBillInvDate.Text);
                cmdIns.ExecuteNonQuery();

                HLSSOA.Print_HLS_BillInv(Convert.ToInt32(Session["CreatorKey"]), txtPrintBillInvDate.Text, txtCustCode.Text, txtCustomer.Text, txtCustomerAdd.Text, txtCustTIN.Text, txtYear.Text, txtWeekNum.Text, txtPreparedBy.Text, txtApprovedBy.Text, txtSOANumber.Text);

                Session["BillInv_PrintKey"] = HLSSOA.hlsBillInvPrintKey.ToString();
                Session["DocumentViewer"] = "HLS_BillInv";
                Response.Redirect("hlsSOA_Print.aspx");

            } else
            {
                txtError.Text = "Found duplicate invoice number.";
                PopUpControlError.HeaderText = "Error . . .";
                PopUpControlError.ShowOnPageLoad = true;
            }
            dt.Clear();

            cn.Close();
        }
    }
}