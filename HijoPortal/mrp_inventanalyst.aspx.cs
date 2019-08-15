﻿using DevExpress.Web;
using HijoPortal.classes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HijoPortal
{
    public partial class mrp_inventanalyst : System.Web.UI.Page
    {
        private static int mrp_key = 0, wrkflwln = 0, iStatusKey = 0;
        private static string docnumber = "", entitycode = "", buCode = "";
        private static bool bindDM = true, bindOpex = true, bindManPower = true, bindCapex = true;
        private static DateTime dateCreated;

        protected void Page_Load(object sender, EventArgs e)
        {
            CheckCreatorKey();
            if (!Page.IsPostBack)
            {
                docnumber = Session["mrp_docNum"].ToString();
                wrkflwln = Convert.ToInt32(Session["mrp_wrkLine"]);

                //bool isAllowed = false;


                //if (MRPClass.PreviewRights(Convert.ToInt32(Session["CreatorKey"]), docnumber, wrkflwln) == false)
                //{
                //    MRPAccessRightsMsg.Text = "Acces Denied!";
                //    MRPAccessRights.HeaderText = "Access Denied";
                //    MRPAccessRights.ShowOnPageLoad = true;
                //}

                //if (GlobalClass.IsSuperAdmin(Convert.ToInt32(Session["CreatorKey"])))
                //{
                //    isAllowed = true;
                //} else
                //{
                //    isAllowed = GlobalClass.IsAllowed(Convert.ToInt32(Session["CreatorKey"]), "MOPInventoryAnalyst", DateTime.Now);
                //}
                    
                if (MRPClass.PreviewRights(Convert.ToInt32(Session["CreatorKey"]), docnumber, wrkflwln) == false)
                {
                    //Response.Redirect("home.aspx");
                    MRPAccessRightsMsg.Text = "Acces Denied!";
                    MRPAccessRights.HeaderText = "Access Denied";
                    MRPAccessRights.ShowOnPageLoad = true;
                }
                else
                {
                    DirectMaterialsRoundPanel.Font.Bold = true;
                    OpexRoundPanel.Font.Bold = true;
                    ManpowerRoundPanel.Font.Bold = true;
                    CapexRoundPanel.Font.Bold = true;

                    DirectMaterialsRoundPanel.Collapsed = true;
                    OpexRoundPanel.Collapsed = true;
                    ManpowerRoundPanel.Collapsed = true;
                    CapexRoundPanel.Collapsed = true;

                    ASPxPageControl1.Font.Bold = true;
                    ASPxPageControl1.Font.Size = 12;

                    //Rsize
                    //ScriptManager.RegisterStartupScript(this.Page, typeof(string), "Resize", "changeWidth.resizeWidth();", true);
                    //Session["mrp_docNum"] = docnumber.ToString();
                    //Session["mrp_wrkLine"] = wrkflwln.ToString();
                    //docnumber = Request.Params["DocNum"].ToString();
                    //wrkflwln = Convert.ToInt32(Request.Params["WrkFlwLn"].ToString());
                    
                    if (wrkflwln == 2)
                    {
                        mrpHead.InnerText = "M O P  Details (Inventory Analyst)";
                    }
                    //if (wrkflwln == 4)
                    if (wrkflwln == 3)
                    {
                        mrpHead.InnerText = "M O P  Deliberation";
                    }

                    //if (wrkflwln == 0)
                    if (wrkflwln == 2)
                    {
                        Submit.Text = "Submit for Deliberation";
                    }
                    else if (wrkflwln == 3)
                    {
                        Submit.Text = "Submit for Approval";
                    }

                    Load_MRP(docnumber);

                    DirectMaterialsRoundPanel.HeaderText = "[" + DocNum.Text.ToString().Trim() + "] " + Constants.DM_string();
                    OpexRoundPanel.HeaderText = "[" + DocNum.Text.ToString().Trim() + "] " + Constants.OP_string();
                    ManpowerRoundPanel.HeaderText = "[" + DocNum.Text.ToString().Trim() + "] " + Constants.MAN_string();
                    CapexRoundPanel.HeaderText = "[" + DocNum.Text.ToString().Trim() + "] " + Constants.CA_string();

                    DirectMaterialsRoundPanel.Font.Bold = true;
                    OpexRoundPanel.Font.Bold = true;
                    ManpowerRoundPanel.Font.Bold = true;
                    CapexRoundPanel.Font.Bold = true;

                    DirectMaterialsRoundPanel.Collapsed = true;
                    OpexRoundPanel.Collapsed = true;
                    ManpowerRoundPanel.Collapsed = true;
                    CapexRoundPanel.Collapsed = true;

                    ASPxPageControl1.Font.Bold = true;
                    ASPxPageControl1.Font.Size = 12;
                }


            }

            //MRPClass.PrintString(bindDM.ToString());
            //MRPClass.PrintString(DMGrid.ViewStateMode.ToString());
            if (bindDM) BindDirectMaterials(docnumber); else bindDM = true;
            if (bindOpex) BindOpex(docnumber); else bindOpex = true;
            if (bindManPower) BindManPower(docnumber); else bindManPower = true;
            if (bindCapex) BindCapex(docnumber); else bindCapex = true;
        }

        private void Load_MRP(string docnumber)
        {
            ASPxHiddenField hidStatusKey = Page.FindControl("StatusKey") as ASPxHiddenField;

            string query = "SELECT tbl_MRP_List.*, " +
                           " vw_AXEntityTable.NAME AS EntityCodeDesc, " +
                           " vw_AXOperatingUnitTable.NAME AS BUCodeDesc, " +
                           " tbl_MRP_Status.StatusName, tbl_Users.Lastname, " +
                           " tbl_Users.Firstname, tbl_MRP_List.EntityCode, " +
                           " tbl_MRP_List.BUCode " +
                           " FROM tbl_MRP_List LEFT OUTER JOIN tbl_Users ON tbl_MRP_List.CreatorKey = tbl_Users.PK " +
                           " LEFT OUTER JOIN vw_AXOperatingUnitTable ON tbl_MRP_List.BUCode = vw_AXOperatingUnitTable.OMOPERATINGUNITNUMBER " +
                           " LEFT OUTER JOIN tbl_MRP_Status ON tbl_MRP_List.StatusKey = tbl_MRP_Status.PK " +
                           " LEFT OUTER JOIN vw_AXEntityTable ON tbl_MRP_List.EntityCode = vw_AXEntityTable.ID " +
                           " WHERE dbo.tbl_MRP_List.DocNumber = '" + docnumber + "' " +
                           " ORDER BY dbo.tbl_MRP_List.DocNumber DESC";
            SqlConnection conn = new SqlConnection(GlobalClass.SQLConnString());
            conn.Open();

            string firstname = "", lastname = "";

            SqlCommand cmd = new SqlCommand(query, conn);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                mrp_key = Convert.ToInt32(reader["PK"].ToString());
                entitycode = reader["EntityCode"].ToString();
                DocNum.Text = reader["DocNumber"].ToString();
                DateCreated.Text = reader["DateCreated"].ToString();
                dateCreated = Convert.ToDateTime(reader["DateCreated"]);
                EntityCode.Text = reader["EntityCodeDesc"].ToString();
                BUCode.Text = reader["BUCodeDesc"].ToString();
                Month.Text = MRPClass.Month_Name(Int32.Parse(reader["MRPMonth"].ToString()));
                Year.Text = reader["MRPYear"].ToString();
                Status.Text = reader["StatusName"].ToString();
                iStatusKey = Convert.ToInt32(reader["StatusKey"]);
                firstname = reader["Firstname"].ToString();
                lastname = reader["Lastname"].ToString();

                entitycode = reader["EntityCode"].ToString();
                Entity.Text = reader["EntityCode"].ToString();
                BU.Text = reader["BUCode"].ToString();
                buCode = reader["BUCode"].ToString();

                Creator.Text = EncryptionClass.Decrypt(firstname) + " " + EncryptionClass.Decrypt(lastname);

            }
            reader.Close();

            iStatusKey = MRPClass.MRP_Line_Status(mrp_key, wrkflwln);

            WorkFlowLineLbl.Text = wrkflwln.ToString();
            WorkFlowLineTxt.Text = wrkflwln.ToString();
            StatusKeyLbl.Text = iStatusKey.ToString();
            StatusKeyTxt.Text = iStatusKey.ToString();

            Creator.Text = EncryptionClass.Decrypt(firstname) + " " + EncryptionClass.Decrypt(lastname);

            DirectMaterialsRoundPanel.HeaderText = "[" + DocNum.Text.ToString().Trim() + "] Direct Materials";
            OpexRoundPanel.HeaderText = "[" + DocNum.Text.ToString().Trim() + "] Operational Expense";
            ManpowerRoundPanel.HeaderText = "[" + DocNum.Text.ToString().Trim() + "] Man Power";
            CapexRoundPanel.HeaderText = "[" + DocNum.Text.ToString().Trim() + "] Capital Expenditure";

            //ASPxPageControl pageControl = grid.FindEditFormTemplateControl("RevenuePageControl") as ASPxPageControl;
            ASPxHiddenField hfwrkLine = ASPxPageControl1.FindControl("ASPxHiddenFieldDMWrkFlwLnInventAnal") as ASPxHiddenField;
            ASPxHiddenField hfstatKey = ASPxPageControl1.FindControl("ASPxHiddenFieldDMStatusKeyInventAnal") as ASPxHiddenField;
            hfwrkLine["hidden_value"] = wrkflwln.ToString();
            hfstatKey["hidden_value"] = iStatusKey.ToString();


        }

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

        private void BindDirectMaterials(string DOC_NUMBER)
        {
            //MRPClass.PrintString(DOC_NUMBER);
            DataTable dtRecord = MRPClass.MRPInvent_Direct_Materials(DOC_NUMBER, entitycode);
            DMGrid.DataSource = dtRecord;
            DMGrid.KeyFieldName = "PK";
            DMGrid.DataBind();
        }

        private void BindOpex(string DOC_NUMBER)
        {
            DataTable dtRecord = MRPClass.MRPInvent_OPEX(DOC_NUMBER, entitycode);
            OpGrid.DataSource = dtRecord;
            OpGrid.KeyFieldName = "PK";
            OpGrid.DataBind();
        }

        private void BindManPower(string DOC_NUMBER)
        {
            DataTable dtRecord = MRPClass.MRPInvent_ManPower(DOC_NUMBER, entitycode);
            ManPoGrid.DataSource = dtRecord;
            ManPoGrid.KeyFieldName = "PK";
            ManPoGrid.DataBind();
        }


        private void BindCapex(string DOC_NUMBER)
        {
            DataTable dtRecord = MRPClass.MRPInvent_CAPEX(DOC_NUMBER, entitycode);
            CapGrid.DataSource = dtRecord;
            CapGrid.KeyFieldName = "PK";
            CapGrid.DataBind();
        }

        protected void DMGrid_BeforeGetCallbackResult(object sender, EventArgs e)
        {
            ASPxGridView grid = sender as ASPxGridView;
            MRPClass.SetBehaviorGrid(grid);
        }

        protected void OpGrid_BeforeGetCallbackResult(object sender, EventArgs e)
        {
            ASPxGridView grid = sender as ASPxGridView;
            MRPClass.SetBehaviorGrid(grid);
        }

        protected void ManPoGrid_BeforeGetCallbackResult(object sender, EventArgs e)
        {
            ASPxGridView grid = sender as ASPxGridView;
            MRPClass.SetBehaviorGrid(grid);
        }

        protected void CapGrid_BeforeGetCallbackResult(object sender, EventArgs e)
        {
            ASPxGridView grid = sender as ASPxGridView;
            MRPClass.SetBehaviorGrid(grid);
        }

        protected void DMGrid_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            bindDM = false;
        }

        protected void DMGrid_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            ASPxGridView grid = sender as ASPxGridView;

            ASPxTextBox qty = grid.FindEditRowCellTemplateControl((GridViewDataColumn)grid.Columns["EdittedQty"], "InvEdittedQty") as ASPxTextBox;
            ASPxTextBox cost = grid.FindEditRowCellTemplateControl((GridViewDataColumn)grid.Columns["EdittedCost"], "InvEdittedCost") as ASPxTextBox;
            ASPxTextBox total = grid.FindEditRowCellTemplateControl((GridViewDataColumn)grid.Columns["EdittiedTotalCost"], "InvEdittiedTotalCost") as ASPxTextBox;

            string PK = e.Keys[0].ToString();
            Double qty_float = 0, cost_float = 0, total_float = 0;
            if (qty.Value != null)
            {
                qty_float = Convert.ToDouble(qty.Value.ToString());
            }
            if (cost.Value != null)
            {
                cost_float = Convert.ToDouble(cost.Value.ToString());
            }
            if (total.Value != null)
            {
                total_float = Convert.ToDouble(total.Value.ToString());
            }

            //Double qty_float = Convert.ToDouble(qty.Value.ToString());
            //Double cost_float = Convert.ToDouble(cost.Value.ToString());
            //Double total_float = Convert.ToDouble(total.Value.ToString());

            SqlConnection conn = new SqlConnection(GlobalClass.SQLConnString());
            conn.Open();

            //string update = "UPDATE " + MRPClass.DirectMatTable() + 
            //                " SET [EdittedQty] = @QTY, [EdittedCost] = @COST, [EdittiedTotalCost] = @TOTAL, " +
            //                " [ApprovedQty] = @QTY, [ApprovedCost] = @COST, [ApprovedTotalCost] = @TOTAL " +
            //                " WHERE [PK] = @PK";
            //SqlCommand cmd = new SqlCommand(update, conn);
            //cmd.Parameters.AddWithValue("@PK", PK);
            //cmd.Parameters.AddWithValue("@QTY", qty_float);
            //cmd.Parameters.AddWithValue("@COST", cost_float);
            //cmd.Parameters.AddWithValue("@TOTAL", total_float);
            //cmd.CommandType = CommandType.Text;
            //int result = cmd.ExecuteNonQuery();

            int result = QuerySPClass.InsertUpdateDirectMaterials(wrkflwln, 2, Convert.ToInt32(PK), docnumber, 1, "", "", "", "", "", "", "", qty_float, cost_float, total_float);
            if (result > 0)
            {
                MRPClass.UpdateLastModified(conn, docnumber);
                string remarks = MRPClass.directmaterials_logs + "-" + MRPClass.edit_logs;
                MRPClass.AddLogsMOPList(conn, mrp_key, remarks);
            }

            conn.Close();

            e.Cancel = true;
            grid.CancelEdit();
            bindDM = true;
            BindDirectMaterials(docnumber);
        }



        protected void OpGrid_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            bindOpex = false;
        }

        protected void OpGrid_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            ASPxGridView grid = sender as ASPxGridView;

            ASPxTextBox qty = grid.FindEditRowCellTemplateControl((GridViewDataColumn)grid.Columns["EdittedQty"], "InvEdittedQtyOp") as ASPxTextBox;
            ASPxTextBox cost = grid.FindEditRowCellTemplateControl((GridViewDataColumn)grid.Columns["EdittedCost"], "InvEdittedCostOp") as ASPxTextBox;
            ASPxTextBox total = grid.FindEditRowCellTemplateControl((GridViewDataColumn)grid.Columns["EdittedTotalCost"], "InvEdittiedTotalCostOp") as ASPxTextBox;

            string PK = e.Keys[0].ToString();
            Double qty_float = 0, cost_float = 0, total_float = 0;
            if (qty.Value != null)
            {
                qty_float = Convert.ToDouble(qty.Value.ToString());
            }
            if (cost.Value != null)
            {
                cost_float = Convert.ToDouble(cost.Value.ToString());
            }
            if (total.Value != null)
            {
                total_float = Convert.ToDouble(total.Value.ToString());
            }

            SqlConnection conn = new SqlConnection(GlobalClass.SQLConnString());
            conn.Open();

            //string update = "UPDATE " + MRPClass.OpexTable() + 
            //                " SET [EdittedQty] = @QTY, [EdittedCost] = @COST, [EdittedTotalCost] = @TOTAL, " +
            //                " [ApprovedQty] = @QTY, [ApprovedCost] = @COST, [ApprovedTotalCost] = @TOTAL " +
            //                " WHERE [PK] = @PK";
            //SqlCommand cmd = new SqlCommand(update, conn);
            //cmd.Parameters.AddWithValue("@PK", PK);
            //cmd.Parameters.AddWithValue("@QTY", qty_float);
            //cmd.Parameters.AddWithValue("@COST", cost_float);
            //cmd.Parameters.AddWithValue("@TOTAL", total_float);
            //cmd.CommandType = CommandType.Text;
            //int result = cmd.ExecuteNonQuery();
            int result = QuerySPClass.InsertUpdateOperatingExpense(wrkflwln, 2, Convert.ToInt32(PK), docnumber, 2, "", "", "", "", "", "", "", qty_float, cost_float, total_float);
            if (result > 0)
            {
                MRPClass.UpdateLastModified(conn, docnumber);
                string remarks = MRPClass.opex_logs + "-" + MRPClass.edit_logs;
                MRPClass.AddLogsMOPList(conn, mrp_key, remarks);
            }

            conn.Close();

            e.Cancel = true;
            grid.CancelEdit();
            bindOpex = true;
            BindOpex(docnumber);

        }



        protected void ManPoGrid_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            bindManPower = false;
        }



        protected void ManPoGrid_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            ASPxGridView grid = sender as ASPxGridView;

            ASPxTextBox qty = grid.FindEditRowCellTemplateControl((GridViewDataColumn)grid.Columns["EdittedQty"], "InvEdittedQtyManPo") as ASPxTextBox;
            ASPxTextBox cost = grid.FindEditRowCellTemplateControl((GridViewDataColumn)grid.Columns["EdittedCost"], "InvEdittedCostManPo") as ASPxTextBox;
            ASPxTextBox total = grid.FindEditRowCellTemplateControl((GridViewDataColumn)grid.Columns["EdittiedTotalCost"], "InvEdittiedTotalCostManPo") as ASPxTextBox;

            string PK = e.Keys[0].ToString();
            Double qty_float = 0, cost_float = 0, total_float = 0;
            if (qty.Value != null)
            {
                qty_float = Convert.ToDouble(qty.Value.ToString());
            }
            if (cost.Value != null)
            {
                cost_float = Convert.ToDouble(cost.Value.ToString());
            }
            if (total.Value != null)
            {
                total_float = Convert.ToDouble(total.Value.ToString());
            }

            //Double qty_float = Convert.ToDouble(qty.Value.ToString());
            //Double cost_float = Convert.ToDouble(cost.Value.ToString());
            //Double total_float = Convert.ToDouble(total.Value.ToString());

            SqlConnection conn = new SqlConnection(GlobalClass.SQLConnString());
            conn.Open();

            //string update = "UPDATE " + MRPClass.ManPowerTable() + 
            //                " SET [EdittedQty] = @QTY, [EdittedCost] = @COST, [EdittiedTotalCost] = @TOTAL, " +
            //                " [ApprovedQty] = @QTY, [ApprovedCost] = @COST, [ApprovedTotalCost] = @TOTAL " + 
            //                " WHERE [PK] = @PK";
            //SqlCommand cmd = new SqlCommand(update, conn);
            //cmd.Parameters.AddWithValue("@PK", PK);
            //cmd.Parameters.AddWithValue("@QTY", qty_float);
            //cmd.Parameters.AddWithValue("@COST", cost_float);
            //cmd.Parameters.AddWithValue("@TOTAL", total_float);
            //cmd.CommandType = CommandType.Text;
            //int result = cmd.ExecuteNonQuery();
            int result = QuerySPClass.InsertUpdateManPower(wrkflwln, 2, Convert.ToInt32(PK), docnumber, 3, "", "", 0, "", "", qty_float, cost_float, total_float);
            if (result > 0)
            {
                MRPClass.UpdateLastModified(conn, docnumber);
                string remarks = MRPClass.manpower_logs + "-" + MRPClass.edit_logs;
                MRPClass.AddLogsMOPList(conn, mrp_key, remarks);
            }

            conn.Close();

            e.Cancel = true;
            grid.CancelEdit();
            bindManPower = true;
            BindManPower(docnumber);
        }

        protected void CapGrid_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            bindCapex = true;
        }

        protected void CapGrid_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            ASPxGridView grid = sender as ASPxGridView;

            ASPxTextBox qty = grid.FindEditRowCellTemplateControl((GridViewDataColumn)grid.Columns["EdittedQty"], "InvEdittedQtyCapex") as ASPxTextBox;
            ASPxTextBox cost = grid.FindEditRowCellTemplateControl((GridViewDataColumn)grid.Columns["EdittedCost"], "InvEdittedCostCapex") as ASPxTextBox;
            ASPxTextBox total = grid.FindEditRowCellTemplateControl((GridViewDataColumn)grid.Columns["EdittiedTotalCost"], "InvEdittiedTotalCostCapex") as ASPxTextBox;

            string PK = e.Keys[0].ToString();
            Double qty_float = 0, cost_float = 0, total_float = 0;
            if (qty.Value != null)
            {
                qty_float = Convert.ToDouble(qty.Value.ToString());
            }
            if (cost.Value != null)
            {
                cost_float = Convert.ToDouble(cost.Value.ToString());
            }
            if (total.Value != null)
            {
                total_float = Convert.ToDouble(total.Value.ToString());
            }

            //Double qty_float = Convert.ToDouble(qty.Value.ToString());
            //Double cost_float = Convert.ToDouble(cost.Value.ToString());
            //Double total_float = Convert.ToDouble(total.Value.ToString());

            SqlConnection conn = new SqlConnection(GlobalClass.SQLConnString());
            conn.Open();

            //string update = "UPDATE " + MRPClass.CapexTable() + 
            //                " SET [EdittedQty] = @QTY, [EdittedCost] = @COST, [EdittiedTotalCost] = @TOTAL, " +
            //                " [ApprovedQty] = @QTY, [ApprovedCost] = @COST, [ApprovedTotalCost] = @TOTAL " +
            //                " WHERE [PK] = @PK";
            //SqlCommand cmd = new SqlCommand(update, conn);
            //cmd.Parameters.AddWithValue("@PK", PK);
            //cmd.Parameters.AddWithValue("@QTY", qty_float);
            //cmd.Parameters.AddWithValue("@COST", cost_float);
            //cmd.Parameters.AddWithValue("@TOTAL", total_float);
            //cmd.CommandType = CommandType.Text;
            //int result = cmd.ExecuteNonQuery();
            int result = QuerySPClass.InsertUpdateCapitalExpenditures(wrkflwln, 2, Convert.ToInt32(PK), docnumber, 4, "", "", "", "", qty_float, cost_float, total_float);
            if (result > 0)
            {
                MRPClass.UpdateLastModified(conn, docnumber);
                string remarks = MRPClass.capex_logs + "-" + MRPClass.edit_logs;
                MRPClass.AddLogsMOPList(conn, mrp_key, remarks);
            }

            conn.Close();

            e.Cancel = true;
            grid.CancelEdit();
            bindCapex = true;
            BindCapex(docnumber);

        }

        protected void DMGrid_CancelRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            BindDirectMaterials(docnumber);
        }

        protected void OpGrid_CancelRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            BindOpex(docnumber);
        }

        protected void ManPoGrid_CancelRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            BindManPower(docnumber);
        }

        protected void CapGrid_CancelRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            BindCapex(docnumber);
        }

        protected void MRPList_Click(object sender, EventArgs e)
        {
            Response.Redirect("mrp_listinventoryanalyst.aspx");
        }

        protected void Submit_Click(object sender, EventArgs e)
        {
            CheckCreatorKey();

            iStatusKey = MRPClass.MRP_Line_Status(mrp_key, wrkflwln);

            //if (wrkflwln == 0)
            //{
            //    if (iStatusKey == 1)
            //    {

            //        //MRPClass.Submit_MRP(docnumber.ToString(), mrp_key, wrkflwln + 1, entitycode, buCode, Convert.ToInt32(Session["CreatorKey"]));
            //        PopupSubmit.ShowOnPageLoad = false;
            //        ScriptManager.RegisterStartupScript(this.Page, typeof(string), "Resize", "changeWidth.resizeWidth();", true);

            //        MRPSubmitClass.MRP_Submit(docnumber.ToString(), mrp_key, dateCreated, wrkflwln, entitycode, buCode, Convert.ToInt32(Session["CreatorKey"]));
            //        Load_MRP(docnumber);

            //        BindDirectMaterials(docnumber);
            //        BindOpex(docnumber);
            //        BindManPower(docnumber);
            //        BindCapex(docnumber);

            //        MRPNotificationMessage.Text = "Successfully Submitted";
            //        MRPNotify.HeaderText = "Info";
            //        MRPNotify.ShowOnPageLoad = true;

            //    }
            //    else
            //    {

            //        ScriptManager.RegisterStartupScript(this.Page, typeof(string), "Resize", "changeWidth.resizeWidth();", true);

            //        MRPNotificationMessage.Text = "Document already submitted to BU / SSU Lead for review.";
            //        MRPNotify.HeaderText = "Alert";
            //        MRPNotify.ShowOnPageLoad = true;
            //        //MRPNotify.
            //    }
            //}
            //else
            //{
                if (MRPClass.MRP_Line_Status(mrp_key, wrkflwln) == 0)
                {
                    //MRPClass.Submit_MRP(docnumber.ToString(), mrp_key, wrkflwln + 1, entitycode, buCode, Convert.ToInt32(Session["CreatorKey"]));
                    PopupSubmit.ShowOnPageLoad = false;
                    //ScriptManager.RegisterStartupScript(this.Page, typeof(string), "Resize", "changeWidth.resizeWidth();", true);

                    MRPSubmitClass.MRP_Submit(docnumber.ToString(), mrp_key, dateCreated, wrkflwln, entitycode, buCode, Convert.ToInt32(Session["CreatorKey"]));

                    Load_MRP(docnumber);
                    BindDirectMaterials(docnumber);
                    BindOpex(docnumber);
                    BindManPower(docnumber);
                    BindCapex(docnumber);
                    //BindRevenue(docnumber);

                    MRPNotificationMessage.Text = "Successfully Submitted";
                    MRPNotify.HeaderText = "Info";
                    MRPNotify.ShowOnPageLoad = true;
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(string), "Resize", "changeWidth.resizeWidth();", true);

                    MRPNotificationMessage.Text = "Document already submitted to Inventory Analyst for review.";
                    MRPNotify.HeaderText = "Alert";
                    MRPNotify.ShowOnPageLoad = true;
                }

            //}
        }

        protected void Preview_Click(object sender, EventArgs e)
        {
            //Response.Redirect("mrp_preview.aspx?DocNum=" + docnumber.ToString() + "&WrkFlwLn=" + wrkflwln.ToString());
            //Response.Redirect("mrp_preview_inventanalyst.aspx?DocNum=" + docnumber.ToString() + "&WrkFlwLn=" + wrkflwln.ToString());
            Response.Redirect("mrp_preview_inventanalyst.aspx?DocNum=" + docnumber.ToString() + "&WrkFlwLn=" + wrkflwln.ToString());
            //mrp_preview_inventanalyst.aspx
        }

        protected void DMGrid_DataBound(object sender, EventArgs e)
        {
            ASPxGridView grid = sender as ASPxGridView;
            MRPClass.VisibilityRevDesc(grid, entitycode);
        }

        protected void OpGrid_DataBound(object sender, EventArgs e)
        {
            ASPxGridView grid = sender as ASPxGridView;
            MRPClass.VisibilityRevDesc(grid, entitycode);
        }

        protected void ManPoGrid_DataBound(object sender, EventArgs e)
        {
            ASPxGridView grid = sender as ASPxGridView;
            MRPClass.VisibilityRevDesc(grid, entitycode);
        }

        protected void CapGrid_DataBound(object sender, EventArgs e)
        {
            ASPxGridView grid = sender as ASPxGridView;
            MRPClass.VisibilityRevDesc(grid, entitycode);
        }
        protected void RightsOK_Click(object sender, EventArgs e)
        {
            Response.Redirect("default.aspx");
        }
    }
}