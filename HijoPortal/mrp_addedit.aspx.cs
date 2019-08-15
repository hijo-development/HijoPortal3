﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using DevExpress.Web;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HijoPortal.classes;
using System.Web.UI.HtmlControls;

namespace HijoPortal
{
    public partial class mrp_addedit : System.Web.UI.Page
    {
        ArrayList ArrDirectMat = new ArrayList();
        ArrayList ArrOpex = new ArrayList();
        ArrayList ArrManPower = new ArrayList();
        ArrayList ArrCapex = new ArrayList();
        ArrayList ArrRevenue = new ArrayList();
        private static int mrp_key = 0, wrkflwln = 0, iStatusKey = 0;
        private static string docnumber = "", entitycode = "", buCode = "", sExpenseCode = "";
        private static bool bindDM = true, bindOpex = true, bindManPower = true, bindCapex = true, bindRevenue = true;
        private static DateTime dateCreated;

        protected void Page_Load(object sender, EventArgs e)
        {
            CheckCreatorKey();

            if (!Page.IsPostBack)
            {

                DirectMaterialsRoundPanel.Font.Bold = true;
                OpexRoundPanel.Font.Bold = true;
                ManpowerRoundPanel.Font.Bold = true;
                CapexRoundPanel.Font.Bold = true;
                RevenueRoundPanel.Font.Bold = true;

                DirectMaterialsRoundPanel.Collapsed = true;
                OpexRoundPanel.Collapsed = true;
                ManpowerRoundPanel.Collapsed = true;
                CapexRoundPanel.Collapsed = true;
                RevenueRoundPanel.Collapsed = true;

                ASPxPageControl1.Font.Bold = true;
                ASPxPageControl1.Font.Size = 12;

                //ScriptManager.RegisterStartupScript(this.Page, typeof(string), "Resize", "changeWidth.resizeWidth();", true);

                //docnumber = Request.Params["DocNum"].ToString();  //Session["DocNumber"].ToString();
                docnumber = Session["mrp_docNum"].ToString();
                wrkflwln = Convert.ToInt32(Session["mrp_wrkLine"]);
                //wrkflwln = Convert.ToInt32(Request.Params["WrkFlwLn"].ToString());

                if (wrkflwln == 0)
                {
                    Submit.Text = "Submit";
                }
                else
                {
                    Submit.Text = "Confirm & Submit";
                }

                //ASPxHiddenField hidWorkflowLine = Page.FindControl("WorkFlowLine") as ASPxHiddenField;
                //hidWorkflowLine["hidden_value"] = wrkflwln.ToString();

                //WorkFlowLine["hidden_value"] = wrkflwln.ToString();

                //if (wrkflwln == 1)
                //{
                //    MRPList.Visible = false;
                //} else
                //{
                //    MRPList.Visible = true;
                //}

                Load_MRP(docnumber);
            }


            if (bindDM)
                BindDirectMaterials(Get_Docnumber());
            else
                bindDM = true;

            if (bindOpex)
                BindOPEX(Get_Docnumber());
            else
                bindOpex = true;

            if (bindManPower)
                BindManPower(Get_Docnumber());
            else
                bindManPower = true;

            if (bindCapex)
                BindCAPEX(Get_Docnumber());
            else
                bindCapex = true;

            if (bindRevenue)
                BindRevenue(Get_Docnumber());
            else
                bindRevenue = true;

        }

        private void Load_MRP(string docnumber)
        {
            ASPxHiddenField hidStatusKey = Page.FindControl("StatusKey") as ASPxHiddenField;

            DataTable dt = new DataTable();
            SqlCommand cmd = null;
            SqlDataAdapter adp;

            DataTable dt1 = new DataTable();
            SqlCommand cmd1 = null;
            SqlDataAdapter adp1;

            ArrDirectMat.Clear();
            ArrOpex.Clear();
            ArrManPower.Clear();
            ArrCapex.Clear();
            ArrRevenue.Clear();

            string qry = "";

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

            //SqlCommand cmd = new SqlCommand(query, conn);
            //SqlDataReader reader = cmd.ExecuteReader();
            //while (reader.Read())
            cmd = new SqlCommand(query);
            cmd.Connection = conn;
            adp = new SqlDataAdapter(cmd);
            adp.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    mrp_key = Convert.ToInt32(row["PK"].ToString());
                    dateCreated = Convert.ToDateTime(row["DateCreated"]);
                    entitycode = row["EntityCode"].ToString();
                    DocNum.Text = row["DocNumber"].ToString();
                    DateCreated.Text = row["DateCreated"].ToString();
                    EntityCode.Text = row["EntityCodeDesc"].ToString();
                    BUCode.Text = row["BUCodeDesc"].ToString();
                    Month.Text = MRPClass.Month_Name(Int32.Parse(row["MRPMonth"].ToString()));
                    Year.Text = row["MRPYear"].ToString();
                    Status.Text = row["StatusName"].ToString();
                    iStatusKey = Convert.ToInt32(row["StatusKey"]);
                    firstname = row["Firstname"].ToString();
                    lastname = row["Lastname"].ToString();

                    entitycode = row["EntityCode"].ToString();
                    Entity.Text = row["EntityCode"].ToString();
                    BU.Text = row["BUCode"].ToString();
                    buCode = row["BUCode"].ToString();

                    if (Convert.ToInt32(row["StatusKey"]) == 1)
                    {
                        CurrentWorkFlowTxt.Text = "0";
                    }
                    else if (Convert.ToInt32(row["StatusKey"]) == 2)
                    {
                        qry = "SELECT dbo.tbl_MRP_List_Workflow.Line, dbo.tbl_System_Approval_Position.PositionName FROM dbo.tbl_MRP_List_Workflow LEFT OUTER JOIN dbo.tbl_System_Approval_Position ON dbo.tbl_MRP_List_Workflow.PositionNameKey = dbo.tbl_System_Approval_Position.PK WHERE(dbo.tbl_MRP_List_Workflow.MasterKey = " + row["PK"] + ") AND(dbo.tbl_MRP_List_Workflow.Status = 0) AND(dbo.tbl_MRP_List_Workflow.Visible = 1)";
                        cmd1 = new SqlCommand(qry);
                        cmd1.Connection = conn;
                        adp1 = new SqlDataAdapter(cmd1);
                        adp1.Fill(dt1);
                        if (dt1.Rows.Count > 0)
                        {
                            foreach (DataRow row1 in dt1.Rows)
                            {
                                CurrentWorkFlowTxt.Text = row1["Line"].ToString();
                            }
                        }
                        dt1.Clear();
                    }
                    else if (Convert.ToInt32(row["StatusKey"]) == 3)
                    {
                        qry = "SELECT dbo.tbl_MRP_List_Workflow.Line, dbo.tbl_System_Approval_Position.PositionName FROM dbo.tbl_MRP_List_Approval LEFT OUTER JOIN dbo.tbl_System_Approval_Position ON dbo.tbl_MRP_List_Approval.PositionNameKey = dbo.tbl_System_Approval_Position.PK WHERE(dbo.tbl_MRP_List_Approval.MasterKey = " + row["PK"] + ") AND(dbo.tbl_MRP_List_Approval.Status = 0) AND(dbo.tbl_MRP_List_Approval.Visible = 1)";
                        cmd1 = new SqlCommand(qry);
                        cmd1.Connection = conn;
                        adp1 = new SqlDataAdapter(cmd1);
                        adp1.Fill(dt1);
                        if (dt1.Rows.Count > 0)
                        {
                            foreach (DataRow row1 in dt1.Rows)
                            {
                                CurrentWorkFlowTxt.Text = row1["Line"].ToString();
                            }
                        }
                        dt1.Clear();
                    }
                    else
                    {
                        CurrentWorkFlowTxt.Text = "5";
                    }
                }
            }
            dt.Clear();
            conn.Close();

            //WorkFlowLineLbl.Text = wrkflwln.ToString();
            WorkFlowLineTxt.Text = wrkflwln.ToString();
            //StatusKeyLbl.Text = iStatusKey.ToString();
            StatusKeyTxt.Text = iStatusKey.ToString();

            WorkFlowLineStatusTxt.Text = "0";

            WorkFlowLineStatusTxt.Text = MRPClass.MRP_Line_Status(mrp_key, wrkflwln).ToString();

            Creator.Text = EncryptionClass.Decrypt(firstname) + " " + EncryptionClass.Decrypt(lastname);

            DirectMaterialsRoundPanel.HeaderText = "[" + DocNum.Text.ToString().Trim() + "] " + Constants.DM_string();
            OpexRoundPanel.HeaderText = "[" + DocNum.Text.ToString().Trim() + "] " + Constants.OP_string();
            ManpowerRoundPanel.HeaderText = "[" + DocNum.Text.ToString().Trim() + "] " + Constants.MAN_string();
            CapexRoundPanel.HeaderText = "[" + DocNum.Text.ToString().Trim() + "] " + Constants.CA_string();
            RevenueRoundPanel.HeaderText = "[" + DocNum.Text.ToString().Trim() + "] " + Constants.REV_string();

            //ASPxPageControl pageControl = grid.FindEditFormTemplateControl("RevenuePageControl") as ASPxPageControl;
            ASPxHiddenField hfwrkLine = ASPxPageControl1.FindControl("ASPxHiddenFieldDMWrkFlwLn") as ASPxHiddenField;
            ASPxHiddenField hfstatKey = ASPxPageControl1.FindControl("ASPxHiddenFieldDMStatusKey") as ASPxHiddenField;
            hfwrkLine["hidden_value"] = wrkflwln.ToString();
            hfstatKey["hidden_value"] = iStatusKey.ToString();


            if (Session["mrp_creator"].ToString() != Session["CreatorKey"].ToString())
            {
                if (GlobalClass.IsSuperAdmin(Convert.ToInt32(Session["CreatorKey"])) == false)
                {
                    if (GlobalClass.IsAllowed(Convert.ToInt32(Session["CreatorKey"]), "MOPBULead", dateCreated, entitycode, buCode) == false)
                    {
                        Response.Redirect("mrp_list.aspx");
                    }
                }

            }



        }

        private void CheckCreatorKey()
        {

            if (Session["CreatorKey"] == null)
            {
                if (Page.IsCallback)
                    ASPxWebControl.RedirectOnCallback(MRPClass.DefaultPage());
                else
                    Response.Redirect("default.aspx");
                MRPClass.PrintString("something is wrong");
                return;
            }

            //entitycode = "", buCode = ""
            //MRPClass.PrintString(Session["mrp_creator"].ToString() + "==" + Session["CreatorKey"].ToString());
            //if (Session["mrp_creator"].ToString() != Session["CreatorKey"].ToString())
            //{
            //    Response.Redirect("mrp_list.aspx");
            //}
        }

        private string Get_Docnumber()
        {
            string str = "";
            if (Session["mrp_docNum"] != null)
                str = Session["mrp_docNum"].ToString();
            else
            {
                if (Page.IsCallback)
                    ASPxWebControl.RedirectOnCallback("mrp_list.aspx");
                else
                    Response.Redirect("mrp_list.aspx");
            }
            return str;
        }

        private void BindDirectMaterials(string DOC_NUMBER)
        {
            DataTable dtRecord = MRPClass.MRP_Direct_Materials(DOC_NUMBER, entitycode);
            DirectMaterialsGrid.DataSource = dtRecord;
            DirectMaterialsGrid.KeyFieldName = "PK";
            DirectMaterialsGrid.DataBind();
        }

        private void BindOPEX(string DOC_NUMBER)
        {
            DataTable dtRecord = MRPClass.MRP_OPEX(DOC_NUMBER, entitycode);
            OPEXGrid.DataSource = dtRecord;
            OPEXGrid.KeyFieldName = "PK";
            OPEXGrid.DataBind();
        }
        private void BindManPower(string DOC_NUMBER)
        {
            DataTable dtRecord = MRPClass.MRP_ManPower(DOC_NUMBER, entitycode);
            ManPowerGrid.DataSource = dtRecord;
            ManPowerGrid.KeyFieldName = "PK";
            ManPowerGrid.DataBind();
        }

        private void BindCAPEX(string DOC_NUMBER)
        {
            DataTable dtRecord = MRPClass.MRP_CAPEX(DOC_NUMBER, entitycode);
            CAPEXGrid.DataSource = dtRecord;
            CAPEXGrid.KeyFieldName = "PK";
            CAPEXGrid.DataBind();
        }

        private void BindRevenue(string DOC_NUMBER)
        {
            DataTable dtRecord = MRPClass.MRP_Revenue(DOC_NUMBER, entitycode);
            RevenueGrid.DataSource = dtRecord;
            RevenueGrid.KeyFieldName = "PK";
            RevenueGrid.DataBind();
        }


        protected void FocusThisRowGrid(ASPxGridView grid, int keyVal)
        {
            grid.FocusedRowIndex = grid.FindVisibleIndexByKeyValue(keyVal);
        }


        protected void ActivityCode_Init(object sender, EventArgs e)
        {
            ASPxComboBox combo = sender as ASPxComboBox;
            combo.DataSource = MRPClass.ActivityCodeTable();
            combo.ItemStyle.Wrap = DevExpress.Utils.DefaultBoolean.True;

            ListBoxColumn l_ValueField = new ListBoxColumn();
            l_ValueField.FieldName = "VALUE";
            l_ValueField.Caption = "Code";
            l_ValueField.Width = 50;
            combo.Columns.Add(l_ValueField);

            ListBoxColumn l_TextField = new ListBoxColumn();
            l_TextField.FieldName = "DESCRIPTION";
            l_TextField.Width = 300;
            combo.Columns.Add(l_TextField);

            combo.ValueField = "VALUE";
            combo.TextField = "DESCRIPTION";
            combo.DataBind();

            GridViewEditFormTemplateContainer container = combo.NamingContainer.NamingContainer as GridViewEditFormTemplateContainer;
            if (!container.Grid.IsNewRowEditing)
            {
                combo.Value = DataBinder.Eval(container.DataItem, "ActivityCode").ToString();
            }
            else
            {
                HttpCookie cookie_value = null;
                HttpCookie cookie_text = null;
                HttpCookie cookie_value_act = null;
                HttpCookie cookie_text_act = null;
                switch (combo.ClientInstanceName)
                {
                    case "ActivityCodeDirect":
                        cookie_value = Request.Cookies["dmvalue"];
                        cookie_text = Request.Cookies["dmtext"];
                        if (cookie_value != null && cookie_text != null)
                        {
                            object val = cookie_value.Value;
                            object text = cookie_text.Value;
                            combo.Value = val;
                            combo.Text = text.ToString();
                        }
                        break;
                    case "ActivityCodeMAN":
                        cookie_value_act = Request.Cookies["manvalue"];
                        cookie_text_act = Request.Cookies["mantext"];
                        if (cookie_value_act != null && cookie_text_act != null)
                        {
                            object valAct = cookie_value_act.Value;
                            object textAct = cookie_text_act.Value;
                            combo.Value = valAct;
                            combo.Text = textAct.ToString();
                        }
                        break;
                }
            }
        }

        protected void UOM_Init(object sender, EventArgs e)
        {
            ASPxComboBox combo = sender as ASPxComboBox;
            combo.DataSource = MRPClass.UOMTable();
            combo.ItemStyle.Wrap = DevExpress.Utils.DefaultBoolean.True;

            ListBoxColumn l_value = new ListBoxColumn();
            l_value.FieldName = "SYMBOL";
            combo.Columns.Add(l_value);

            ListBoxColumn l_text = new ListBoxColumn();
            l_text.FieldName = "description";
            combo.Columns.Add(l_text);

            combo.ValueField = "SYMBOL";
            combo.TextField = "description";
            combo.DataBind();
            combo.TextFormatString = "{0}";

            GridViewEditFormTemplateContainer container = ((ASPxComboBox)sender).NamingContainer.NamingContainer as GridViewEditFormTemplateContainer;
            if (!container.Grid.IsNewRowEditing)
            {
                combo.Value = DataBinder.Eval(container.DataItem, "UOM").ToString();
                //combo.Text = DataBinder.Eval(container.DataItem, "ExpenseCodeName").ToString();
            }

        }

        protected void ExpenseCode_Init(object sender, EventArgs e)
        {
            ASPxComboBox combo = sender as ASPxComboBox;
            bool isOPEXCombobox = combo.ClientInstanceName == "ExpenseCodeOPEX";
            int isOPEX = -1;
            if (isOPEXCombobox)
                isOPEX = 1;
            else
                isOPEX = 0;

            combo.DataSource = MRPClass.ExpenseCodeTable(isOPEX);
            combo.ItemStyle.Wrap = DevExpress.Utils.DefaultBoolean.True;

            ListBoxColumn l_value = new ListBoxColumn();
            l_value.FieldName = "MAINACCOUNTID";
            combo.Columns.Add(l_value);

            ListBoxColumn l_text = new ListBoxColumn();
            l_text.FieldName = "NAME";
            l_text.Width = 300;
            combo.Columns.Add(l_text);

            ListBoxColumn l_text2 = new ListBoxColumn();
            l_text2.FieldName = "isItem";
            l_text2.Width = 0;
            combo.Columns.Add(l_text2);

            ListBoxColumn l_text3 = new ListBoxColumn();
            l_text3.FieldName = "isProdCategory";
            l_text3.Width = 0;
            combo.Columns.Add(l_text3);

            combo.ValueField = "MAINACCOUNTID";
            combo.TextField = "NAME";
            combo.DataBind();
            combo.TextFormatString = "{1}";

            GridViewEditFormTemplateContainer container = ((ASPxComboBox)sender).NamingContainer.NamingContainer as GridViewEditFormTemplateContainer;
            if (!container.Grid.IsNewRowEditing)
            {
                combo.Value = DataBinder.Eval(container.DataItem, "ExpenseCode").ToString();
                combo.Text = DataBinder.Eval(container.DataItem, "ExpenseCodeName").ToString();
            }
            else
            {

                //!IMPORTANT... cookie for expense direct materials
                HttpCookie cookie_value = null;
                HttpCookie cookie_text = null;
                HttpCookie cookie_isItem = null;
                HttpCookie cookie_isProdCat = null;

                if (isOPEXCombobox)
                {
                    cookie_value = Request.Cookies["opvalue"];
                    cookie_text = Request.Cookies["optext"];
                    cookie_isItem = Request.Cookies["opisItem"];
                    cookie_isProdCat = Request.Cookies["opisProdCat"];
                    if (cookie_value != null && cookie_text != null)
                    {
                        object val = cookie_value.Value;
                        object text = cookie_text.Value;
                        combo.Value = val;
                        combo.Text = text.ToString();
                    }
                }
                else
                {
                    cookie_value = Request.Cookies["dm_exp_value"];
                    cookie_text = Request.Cookies["dm_exp_text"];
                    if (cookie_value != null && cookie_text != null)
                    {
                        object val = cookie_value.Value;
                        object text = cookie_text.Value;
                        combo.Value = val;
                        combo.Text = text.ToString();
                    }
                }

            }
        }

        protected void OperatingUnit_Init(object sender, EventArgs e)
        {
            ASPxComboBox combo = sender as ASPxComboBox;
            combo.DataSource = MRPClass.OperatingUnitTable(entitycode);
            combo.ItemStyle.Wrap = DevExpress.Utils.DefaultBoolean.True;

            ListBoxColumn l_value = new ListBoxColumn();
            l_value.FieldName = "VALUE";
            combo.Columns.Add(l_value);

            ListBoxColumn l_text = new ListBoxColumn();
            l_text.FieldName = "DESCRIPTION";
            combo.Columns.Add(l_text);

            combo.ValueField = "VALUE";
            combo.TextField = "DESCRIPTION";
            combo.DataBind();
            combo.TextFormatString = "{1}";

            GridViewEditFormTemplateContainer container = ((ASPxComboBox)sender).NamingContainer.NamingContainer as GridViewEditFormTemplateContainer;
            if (!container.Grid.IsNewRowEditing)
            {
                combo.Value = DataBinder.Eval(container.DataItem, "VALUE").ToString();
                combo.Text = DataBinder.Eval(container.DataItem, "RevDesc").ToString();
            }
            else
            {
                HttpCookie cookie_value = null;
                HttpCookie cookie_text = null;
                switch (combo.ClientInstanceName)
                {
                    case "OperatingUnit"://direct material
                        cookie_value = Request.Cookies["dm_operating_value"];
                        cookie_text = Request.Cookies["dm_operating_text"];
                        break;
                    case "OperatingUnitOP":
                        cookie_value = Request.Cookies["op_operating_value"];
                        cookie_text = Request.Cookies["op_operating_text"];
                        break;
                    case "OperatingUnitMAN":
                        cookie_value = Request.Cookies["man_operating_value"];
                        cookie_text = Request.Cookies["man_operating_text"];
                        break;
                    case "OperatingUnitCA":
                        cookie_value = Request.Cookies["ca_operating_value"];
                        cookie_text = Request.Cookies["ca_operating_text"];
                        break;
                    case "OperatingUnitREV":
                        cookie_value = Request.Cookies["rev_operating_value"];
                        cookie_text = Request.Cookies["rev_operating_text"];
                        break;
                }

                if (cookie_value != null && cookie_text != null)
                {
                    object val = cookie_value.Value;
                    object text = cookie_text.Value;
                    combo.Value = val;
                    combo.Text = text.ToString();
                }

            }
        }

        protected void ManPowerTypeKey_Init(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection(GlobalClass.SQLConnString());
            conn.Open();
            string query = "SELECT [ManPowerTypeDesc] FROM [dbo].[tbl_System_ManPowerType]";
            SqlCommand cmd = new SqlCommand(query, conn);
            SqlDataReader reader = cmd.ExecuteReader();

            ASPxComboBox combo = sender as ASPxComboBox;
            while (reader.Read())
            {
                combo.Items.Add(reader[0].ToString());
            }
            reader.Close();
            conn.Close();
        }

        protected void DirectMaterialsGrid_BeforeGetCallbackResult(object sender, EventArgs e)
        {
            ASPxGridView grid = sender as ASPxGridView;
            MRPClass.SetBehaviorGrid(grid);

            ASPxPageControl pageControl = grid.FindEditFormTemplateControl("DirectPageControl") as ASPxPageControl;
            if (pageControl != null)
            {
                ASPxHiddenField entityhidden = pageControl.FindControl("entityhidden") as ASPxHiddenField;
                if (entitycode != MRPClass.train_entity)
                {
                    HtmlControl div1 = (HtmlControl)pageControl.FindControl("OperatingUnit_label");
                    div1.Style.Add("display", "none");
                    div1.Style.Add("display", "none");
                    HtmlControl div2 = (HtmlControl)pageControl.FindControl("OperatingUnit_combo");
                    div2.Style.Add("display", "none");
                    div2.Style.Add("display", "none");

                    entityhidden["hidden_value"] = "not display";
                    //MRPClass.PrintString(".....................trial");
                }
                else
                {
                    entityhidden["hidden_value"] = "display";
                }
            }
        }

        protected void DirectMaterialsGrid_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            CheckCreatorKey();
            //docnumber = Session["mrp_docNum"].ToString();
            bindDM = false;
        }

        protected void DirectMaterialsGrid_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            CheckCreatorKey();
            docnumber = Session["mrp_docNum"].ToString();

            ASPxGridView grid = sender as ASPxGridView;
            ASPxPageControl pageControl = grid.FindEditFormTemplateControl("DirectPageControl") as ASPxPageControl;

            ASPxComboBox opunit = pageControl.FindControl("OperatingUnit") as ASPxComboBox;
            ASPxComboBox actCode = pageControl.FindControl("ActivityCode") as ASPxComboBox;
            ASPxTextBox itemCode = pageControl.FindControl("ItemCode") as ASPxTextBox;
            ASPxTextBox itemDesc = pageControl.FindControl("ItemDescription") as ASPxTextBox;

            ASPxComboBox expcode = pageControl.FindControl("ExpenseCode") as ASPxComboBox;
            ASPxTextBox itemDesc2 = pageControl.FindControl("ItemDescriptionAddl") as ASPxTextBox;
            ASPxComboBox uom = pageControl.FindControl("UOM") as ASPxComboBox;
            ASPxTextBox cost = pageControl.FindControl("Cost") as ASPxTextBox;
            ASPxTextBox qty = pageControl.FindControl("Qty") as ASPxTextBox;
            ASPxTextBox totalcost = pageControl.FindControl("TotalCost") as ASPxTextBox;

            SqlConnection conn = new SqlConnection(GlobalClass.SQLConnString());
            conn.Open();

            string operating_unit = "";
            if (opunit.Value != null)
                operating_unit = opunit.Value.ToString();

            string activity_code = "";
            if (actCode.Value != null)
                activity_code = actCode.Value.ToString();

            string desc_two = "";
            if (itemDesc2.Value != null)
                desc_two = itemDesc2.Value.ToString();

            string exp_code = "";
            if (expcode.Value != null)
                exp_code = expcode.Value.ToString();

            Double qty_float = 0, cost_float = 0, total_float = 0;
            if (qty.Value != null)
            {
                qty_float = Convert.ToDouble(qty.Value.ToString());
            }
            if (cost.Value != null)
            {
                cost_float = Convert.ToDouble(cost.Value.ToString());
            }
            if (totalcost.Value != null)
            {
                total_float = Convert.ToDouble(totalcost.Value.ToString());
            }

            //string insert = "INSERT INTO " + MRPClass.DirectMatTable() +
            //                " ([HeaderDocNum], [ActivityCode], [ItemCode], [ItemDescription], [UOM], " +
            //                " [Cost], [Qty], [TotalCost], [OprUnit], " +
            //                " [EdittedQty], [EdittedCost], [EdittiedTotalCost], " +
            //                " [ApprovedQty], [ApprovedCost], [ApprovedTotalCost], [ItemDescriptionAddl], [ExpenseCode]) " +
            //                " VALUES (@HeaderDocNum, @ActivityCode, @ItemCode, @ItemDesc, @UOM, " +
            //                " @Cost, @Qty, @TotalCost, @OprUnit, " +
            //                " @Qty, @Cost, @TotalCost, @Qty, @Cost, @TotalCost, @itemDesc2, @ExpenseCode)";

            //SqlCommand cmd = new SqlCommand(insert, conn);
            //cmd.Parameters.AddWithValue("@HeaderDocNum", docnumber);
            //cmd.Parameters.AddWithValue("@OprUnit", operating_unit);
            //cmd.Parameters.AddWithValue("@ActivityCode", activity_code);
            //cmd.Parameters.AddWithValue("@ItemCode", itemCode.Value.ToString());
            //cmd.Parameters.AddWithValue("@ItemDesc", GlobalClass.FormatSQL(itemDesc.Value.ToString()));
            //cmd.Parameters.AddWithValue("@itemDesc2", GlobalClass.FormatSQL(desc_two));
            //cmd.Parameters.AddWithValue("@UOM", uom.Value.ToString());
            //cmd.Parameters.AddWithValue("@Cost", Convert.ToDouble(cost.Value.ToString()));
            //cmd.Parameters.AddWithValue("@Qty", Convert.ToDouble(qty.Value.ToString()));
            //cmd.Parameters.AddWithValue("@TotalCost", Convert.ToDouble(totalcost.Value.ToString()));
            //cmd.Parameters.AddWithValue("@ExpenseCode", exp_code);
            //cmd.CommandType = CommandType.Text;

            //SqlCommand cmd = new SqlCommand("sp_InsertUpdateDM", conn);
            //cmd.CommandType = CommandType.StoredProcedure;
            //cmd.Parameters.AddWithValue("@ModuleType", wrkflwln);
            //cmd.Parameters.AddWithValue("@TransType", 1);
            //cmd.Parameters.AddWithValue("@PK", 0);
            //cmd.Parameters.AddWithValue("@HeaderDocNum", docnumber);
            //cmd.Parameters.AddWithValue("@TableIdentifier", 1);
            //cmd.Parameters.AddWithValue("@ExpenseCode", exp_code);
            //cmd.Parameters.AddWithValue("@ActivityCode", activity_code);
            //cmd.Parameters.AddWithValue("@OprUnit", operating_unit);
            //cmd.Parameters.AddWithValue("@ItemCode", itemCode.Value.ToString());
            //cmd.Parameters.AddWithValue("@ItemDescription", GlobalClass.FormatSQL(itemDesc.Value.ToString()));
            //cmd.Parameters.AddWithValue("@ItemDescriptionAddl", GlobalClass.FormatSQL(desc_two));
            //cmd.Parameters.AddWithValue("@UOM", uom.Value.ToString());
            //cmd.Parameters.AddWithValue("@Qty", Convert.ToDouble(qty.Value.ToString()));
            //cmd.Parameters.AddWithValue("@Cost", Convert.ToDouble(cost.Value.ToString()));
            //cmd.Parameters.AddWithValue("@TotalCost", Convert.ToDouble(totalcost.Value.ToString()));
            //int result = cmd.ExecuteNonQuery();
            int result = QuerySPClass.InsertUpdateDirectMaterials(wrkflwln, 1, 0, docnumber, 1, exp_code, activity_code, operating_unit, itemCode.Value.ToString(), GlobalClass.FormatSQL(itemDesc.Value.ToString()), GlobalClass.FormatSQL(desc_two), uom.Value.ToString(), qty_float, cost_float, total_float);
            if (result > 0)
            {
                string remarks = MRPClass.directmaterials_logs + "-" + MRPClass.add_logs;
                MRPClass.UpdateLastModified(conn, docnumber);
                MRPClass.AddLogsMOPList(conn, mrp_key, remarks);
            }

            int pk_latest = QuerySPClass.MRP_Details_Latest_PK(docnumber, 1);
            //string query_pk = "SELECT TOP 1 [PK] FROM " + MRPClass.DirectMatTable() + " WHERE ([HeaderDocNum] = '" + docnumber + "') ORDER BY [PK] DESC";
            //SqlCommand comm = new SqlCommand(query_pk, conn);
            //SqlDataReader r = comm.ExecuteReader();
            //while (r.Read())
            //{
            //    pk_latest = Convert.ToInt32(r[0].ToString());
            //}



            e.Cancel = true;
            grid.CancelEdit();

            string str = Get_Docnumber();
            BindDirectMaterials(str);

            if (pk_latest > 0)
                FocusThisRowGrid(grid, pk_latest);

        }

        protected void DirectMaterialsGrid_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            CheckCreatorKey();
            //docnumber = Session["mrp_docNum"].ToString();
            bindDM = false;
        }

        protected void DirectMaterialsGrid_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            CheckCreatorKey();
            docnumber = Session["mrp_docNum"].ToString();
            ASPxGridView grid = sender as ASPxGridView;
            ASPxPageControl pageControl = grid.FindEditFormTemplateControl("DirectPageControl") as ASPxPageControl;

            ASPxComboBox opunit = pageControl.FindControl("OperatingUnit") as ASPxComboBox;
            ASPxComboBox actCode = pageControl.FindControl("ActivityCode") as ASPxComboBox;
            ASPxTextBox itemCode = pageControl.FindControl("ItemCode") as ASPxTextBox;
            ASPxTextBox itemDesc = pageControl.FindControl("ItemDescription") as ASPxTextBox;
            ASPxTextBox itemDesc2 = pageControl.FindControl("ItemDescriptionAddl") as ASPxTextBox;
            ASPxComboBox uom = pageControl.FindControl("UOM") as ASPxComboBox;
            ASPxTextBox cost = pageControl.FindControl("Cost") as ASPxTextBox;
            ASPxTextBox qty = pageControl.FindControl("Qty") as ASPxTextBox;
            ASPxTextBox totalcost = pageControl.FindControl("TotalCost") as ASPxTextBox;

            ASPxComboBox expcode = pageControl.FindControl("ExpenseCode") as ASPxComboBox;

            SqlConnection conn = new SqlConnection(GlobalClass.SQLConnString());
            conn.Open();

            string PK = e.Keys[0].ToString();


            //string activity_code = "";
            //if (actCode.Value != null)
            //    activity_code = actCode.Value.ToString();

            string activity_code = "";
            if (actCode.Value != null)
                //activity_code = MRPClass.ActivityCodeDESCRIPTION(actCode.Value.ToString());
                activity_code = actCode.Value.ToString();

            string operating_unit = "";
            if (opunit.Value != null)
                operating_unit = opunit.Value.ToString();

            string desc_two = "";
            if (itemDesc2.Value != null)
                desc_two = itemDesc2.Value.ToString();

            string exp_code = "";
            if (expcode.Value != null)
                exp_code = expcode.Value.ToString();

            Double qty_float = 0, cost_float = 0, total_float = 0;
            if (qty.Value != null)
            {
                qty_float = Convert.ToDouble(qty.Value.ToString());
            }
            if (cost.Value != null)
            {
                cost_float = Convert.ToDouble(cost.Value.ToString());
            }
            if (totalcost.Value != null)
            {
                total_float = Convert.ToDouble(totalcost.Value.ToString());
            }

            //string update_MRP = "UPDATE " + MRPClass.DirectMatTable() +
            //                    " SET [ActivityCode] = @ActivityCode, [ItemCode] = @ItemCode , " +
            //                    " [ItemDescription] = @ItemDescription, [UOM]= @UOM, " +
            //                    " [Cost] = @Cost, [Qty] = @Qty, [TotalCost] = @TotalCost, [OprUnit] = @OprUnit, " +
            //                    " [EdittedQty] = @Qty, [EdittedCost] = @Cost, [EdittiedTotalCost] = @TotalCost, " +
            //                    " [ApprovedQty] = @Qty, [ApprovedCost] = @Cost, [ApprovedTotalCost] = @TotalCost, [ItemDescriptionAddl] = @itemDesc2, [ExpenseCode] = @ExpenseCode " +
            //                    " WHERE [PK] = @PK";

            //SqlCommand cmd = new SqlCommand(update_MRP, conn);
            //cmd.Parameters.AddWithValue("@PK", PK);
            //cmd.Parameters.AddWithValue("@OprUnit", operating_unit);
            //cmd.Parameters.AddWithValue("@ActivityCode", activity_code);
            //cmd.Parameters.AddWithValue("@ItemCode", itemCode.Value.ToString());
            //cmd.Parameters.AddWithValue("@ItemDescription", GlobalClass.FormatSQL(itemDesc.Value.ToString()));
            //cmd.Parameters.AddWithValue("@itemDesc2", GlobalClass.FormatSQL(desc_two));
            //cmd.Parameters.AddWithValue("@UOM", uom.Value.ToString());
            //cmd.Parameters.AddWithValue("@Cost", Convert.ToDouble(cost.Value.ToString()));
            //cmd.Parameters.AddWithValue("@Qty", Convert.ToDouble(qty.Value.ToString()));
            //cmd.Parameters.AddWithValue("@TotalCost", Convert.ToDouble(totalcost.Value.ToString()));
            //cmd.Parameters.AddWithValue("@ExpenseCode", exp_code);

            //MRPClass.PrintString(activity_code);

            //SqlCommand cmd = new SqlCommand("sp_InsertUpdateDM", conn);
            //cmd.CommandType = CommandType.StoredProcedure;
            //cmd.Parameters.AddWithValue("@ModuleType", wrkflwln);
            //cmd.Parameters.AddWithValue("@TransType", 2);
            //cmd.Parameters.AddWithValue("@PK", PK);
            //cmd.Parameters.AddWithValue("@HeaderDocNum", docnumber);
            //cmd.Parameters.AddWithValue("@TableIdentifier", 1);
            //cmd.Parameters.AddWithValue("@ExpenseCode", exp_code);
            //cmd.Parameters.AddWithValue("@ActivityCode", activity_code);
            //cmd.Parameters.AddWithValue("@OprUnit", operating_unit);
            //cmd.Parameters.AddWithValue("@ItemCode", itemCode.Value.ToString());
            //cmd.Parameters.AddWithValue("@ItemDescription", GlobalClass.FormatSQL(itemDesc.Value.ToString()));
            //cmd.Parameters.AddWithValue("@ItemDescriptionAddl", GlobalClass.FormatSQL(desc_two));
            //cmd.Parameters.AddWithValue("@UOM", uom.Value.ToString());
            //cmd.Parameters.AddWithValue("@Qty", Convert.ToDouble(qty.Value.ToString()));
            //cmd.Parameters.AddWithValue("@Cost", Convert.ToDouble(cost.Value.ToString()));
            //cmd.Parameters.AddWithValue("@TotalCost", Convert.ToDouble(totalcost.Value.ToString()));
            //int result = cmd.ExecuteNonQuery();
            int result = QuerySPClass.InsertUpdateDirectMaterials(wrkflwln, 2, Convert.ToInt32(PK), docnumber, 1, exp_code, activity_code, operating_unit, itemCode.Value.ToString(), GlobalClass.FormatSQL(itemDesc.Value.ToString()), GlobalClass.FormatSQL(desc_two), uom.Value.ToString(), qty_float, cost_float, total_float);
            if (result > 0)
            {
                MRPClass.UpdateLastModified(conn, docnumber);
                string remarks = MRPClass.directmaterials_logs + "-" + MRPClass.edit_logs;
                MRPClass.AddLogsMOPList(conn, mrp_key, remarks);
            }

            conn.Close();

            string str = Get_Docnumber();
            BindDirectMaterials(str);

            e.Cancel = true;
            grid.CancelEdit();
        }



        protected void DirectMaterialsGrid_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            CheckCreatorKey();
            //docnumber = Session["mrp_docNum"].ToString();
            SqlConnection conn = new SqlConnection(GlobalClass.SQLConnString());
            conn.Open();

            string PK = e.Keys[0].ToString();
            bool Exist = MRPClass.CheckLogsExist(MRPClass.MaterialsTableLogs(), PK);
            if (Exist)//if the material has logs
            {
                conn.Close();
                e.Cancel = true;
            }
            else
            {
                string delete = "DELETE FROM " + MRPClass.DirectMatTable() + " WHERE [PK] ='" + PK + "'";
                SqlCommand cmd = new SqlCommand(delete, conn);
                int result = cmd.ExecuteNonQuery();
                if (result > 0)
                {
                    MRPClass.UpdateLastModified(conn, docnumber);

                    string remarks = MRPClass.directmaterials_logs + "-" + MRPClass.delete_logs;
                    MRPClass.AddLogsMOPList(conn, mrp_key, remarks);
                }
                conn.Close();

                string str = Get_Docnumber();
                BindDirectMaterials(str);

                e.Cancel = true;
            }
        }

        protected void OPEXGrid_BeforeGetCallbackResult(object sender, EventArgs e)
        {
            ASPxGridView grid = sender as ASPxGridView;
            MRPClass.SetBehaviorGrid(grid);

            ASPxPageControl pageControl = grid.FindEditFormTemplateControl("OPEXPageControl") as ASPxPageControl;
            if (pageControl != null)
            {
                ASPxHiddenField entityhidden = pageControl.FindControl("entityhidden") as ASPxHiddenField;
                if (entitycode != MRPClass.train_entity)
                {
                    HtmlControl div1 = (HtmlControl)pageControl.FindControl("OperatingUnit_label");
                    div1.Style.Add("display", "none");
                    HtmlControl div2 = (HtmlControl)pageControl.FindControl("OperatingUnit_combo");
                    div2.Style.Add("display", "none");

                    entityhidden["hidden_value"] = "not display";
                }
                else
                {
                    entityhidden["hidden_value"] = "display";
                }
            }
        }
        protected void OPEXGrid_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            CheckCreatorKey();
            //docnumber = Session["mrp_docNum"].ToString();
            bindCapex = false;
        }

        protected void OPEXGrid_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            CheckCreatorKey();
            docnumber = Session["mrp_docNum"].ToString();

            ASPxGridView grid = sender as ASPxGridView;
            ASPxPageControl pageControl = grid.FindEditFormTemplateControl("OPEXPageControl") as ASPxPageControl;

            ASPxCallbackPanel cp = pageControl.FindControl("ProcCatOPEXCallback") as ASPxCallbackPanel;
            ASPxComboBox ProCatCode = cp.FindControl("ProcCatOPEX") as ASPxComboBox;

            ASPxComboBox opunit = pageControl.FindControl("OperatingUnit") as ASPxComboBox;
            ASPxComboBox experseCode = pageControl.FindControl("ExpenseCode") as ASPxComboBox;
            ASPxTextBox itemCode = pageControl.FindControl("ItemCode") as ASPxTextBox;
            ASPxTextBox itemDesc = pageControl.FindControl("Description") as ASPxTextBox;
            ASPxTextBox itemDesc2 = pageControl.FindControl("DescriptionAddl") as ASPxTextBox;
            ASPxComboBox uom = pageControl.FindControl("UOM") as ASPxComboBox;
            ASPxTextBox cost = pageControl.FindControl("Cost") as ASPxTextBox;
            ASPxTextBox qty = pageControl.FindControl("Qty") as ASPxTextBox;
            ASPxTextBox totalcost = pageControl.FindControl("TotalCost") as ASPxTextBox;

            SqlConnection conn = new SqlConnection(GlobalClass.SQLConnString());
            conn.Open();

            //string insert = "INSERT INTO " + MRPClass.OpexTable() + " ([HeaderDocNum], [ExpenseCode], [ItemCode], [Description], [UOM], [Cost], [Qty], [TotalCost], [OprUnit], [EdittedQty], [EdittedCost], [EdittedTotalCost], [ApprovedQty], [ApprovedCost], [ApprovedTotalCost],  [DescriptionAddl], [ProcCat]) VALUES (@HeaderDocNum, @ExpenseCode, @ItemCode, @Description, @UOM, @Cost, @Qty, @TotalCost, @OprUnit, @Qty, @Cost, @TotalCost, @Qty, @Cost, @TotalCost, @itemDesc2, @ProcCat)";

            string code = "";
            if (itemCode.Value != null) code = itemCode.Value.ToString();

            string operating_unit = "";
            if (opunit.Value != null)
                operating_unit = opunit.Value.ToString();

            string desc_two = "";
            if (itemDesc2.Value != null)
                desc_two = itemDesc2.Value.ToString();

            string prod_cat = "";
            MRPClass.PrintString((ProCatCode.Value != null).ToString());
            if (ProCatCode.Value != null)
                prod_cat = ProCatCode.Value.ToString();

            Double qty_float = 0, cost_float = 0, total_float = 0;
            if (qty.Value != null)
            {
                qty_float = Convert.ToDouble(qty.Value.ToString());
            }
            if (cost.Value != null)
            {
                cost_float = Convert.ToDouble(cost.Value.ToString());
            }
            if (totalcost.Value != null)
            {
                total_float = Convert.ToDouble(totalcost.Value.ToString());
            }

            //MRPClass.PrintString(prod_cat);

            //SqlCommand cmd = new SqlCommand(insert, conn);
            //cmd.Parameters.AddWithValue("@HeaderDocNum", docnumber);
            //cmd.Parameters.AddWithValue("@OprUnit", operating_unit);
            //cmd.Parameters.AddWithValue("@ExpenseCode", experseCode.Value.ToString());
            //cmd.Parameters.AddWithValue("@ItemCode", code);
            //cmd.Parameters.AddWithValue("@Description", GlobalClass.FormatSQL(itemDesc.Value.ToString()));
            //cmd.Parameters.AddWithValue("@itemDesc2", GlobalClass.FormatSQL(desc_two));
            //cmd.Parameters.AddWithValue("@UOM", uom.Value.ToString());
            //cmd.Parameters.AddWithValue("@Cost", Convert.ToDouble(cost.Value.ToString()));
            //cmd.Parameters.AddWithValue("@Qty", Convert.ToDouble(qty.Value.ToString()));
            //cmd.Parameters.AddWithValue("@TotalCost", Convert.ToDouble(totalcost.Value.ToString()));
            //cmd.Parameters.AddWithValue("@ProcCat", prod_cat);
            //cmd.CommandType = CommandType.Text;
            //int result = cmd.ExecuteNonQuery();
            int result = QuerySPClass.InsertUpdateOperatingExpense(wrkflwln, 1, 0, docnumber, 2, experseCode.Value.ToString(), operating_unit, prod_cat, code, GlobalClass.FormatSQL(itemDesc.Value.ToString()), GlobalClass.FormatSQL(desc_two), uom.Value.ToString(), qty_float, cost_float, total_float);
            if (result > 0)
            {
                MRPClass.UpdateLastModified(conn, docnumber);
                string remarks = MRPClass.opex_logs + "-" + MRPClass.add_logs;
                MRPClass.AddLogsMOPList(conn, mrp_key, remarks);
            }

            //e.Cancel = true;
            //grid.CancelEdit();
            //BindOPEX(docnumber);
            int pk_latest = QuerySPClass.MRP_Details_Latest_PK(docnumber, 2);
            //string query_pk = "SELECT TOP 1 [PK] FROM " + MRPClass.OpexTable() + " WHERE ([HeaderDocNum] = '" + docnumber + "') ORDER BY [PK] DESC";
            //SqlCommand comm1 = new SqlCommand(query_pk, conn);
            //SqlDataReader r = comm1.ExecuteReader();
            //while (r.Read())
            //{
            //    pk_latest = Convert.ToInt32(r[0].ToString());
            //}

            e.Cancel = true;
            grid.CancelEdit();

            string str = Get_Docnumber();
            BindOPEX(str);

            if (pk_latest > 0)
                FocusThisRowGrid(grid, pk_latest);
        }

        protected void OPEXGrid_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            CheckCreatorKey();
            //docnumber = Session["mrp_docNum"].ToString();
            SqlConnection conn = new SqlConnection(GlobalClass.SQLConnString());
            conn.Open();

            string PK = e.Keys[0].ToString();
            bool Exist = MRPClass.CheckLogsExist(MRPClass.OpexTableLogs(), PK);
            if (Exist)//if the material has logs
            {
                conn.Close();
                e.Cancel = true;
            }
            else
            {
                string delete = "DELETE FROM " + MRPClass.OpexTable() + " WHERE [PK] ='" + PK + "'";
                SqlCommand cmd = new SqlCommand(delete, conn);
                int result = cmd.ExecuteNonQuery();

                if (result > 0)
                {
                    MRPClass.UpdateLastModified(conn, docnumber);
                    string remarks = MRPClass.opex_logs + "-" + MRPClass.delete_logs;
                    MRPClass.AddLogsMOPList(conn, mrp_key, remarks);
                }

                conn.Close();
                string str = Get_Docnumber();
                BindOPEX(str);
                e.Cancel = true;
            }
        }

        protected void OPEXGrid_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            CheckCreatorKey();
            //docnumber = Session["mrp_docNum"].ToString();
            bindCapex = false;
        }

        protected void OPEXGrid_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            CheckCreatorKey();
            docnumber = Session["mrp_docNum"].ToString();

            ASPxGridView grid = sender as ASPxGridView;
            ASPxPageControl pageControl = grid.FindEditFormTemplateControl("OPEXPageControl") as ASPxPageControl;

            ASPxCallbackPanel cp = pageControl.FindControl("ProcCatOPEXCallback") as ASPxCallbackPanel;
            ASPxComboBox ProCatCode = cp.FindControl("ProcCatOPEX") as ASPxComboBox;

            ASPxComboBox opunit = pageControl.FindControl("OperatingUnit") as ASPxComboBox;
            ASPxComboBox expenseCode = pageControl.FindControl("ExpenseCode") as ASPxComboBox;
            ASPxTextBox itemCode = pageControl.FindControl("ItemCode") as ASPxTextBox;
            ASPxTextBox itemDesc = pageControl.FindControl("Description") as ASPxTextBox;
            ASPxTextBox itemDesc2 = pageControl.FindControl("DescriptionAddl") as ASPxTextBox;
            ASPxComboBox uom = pageControl.FindControl("UOM") as ASPxComboBox;
            ASPxTextBox cost = pageControl.FindControl("Cost") as ASPxTextBox;
            ASPxTextBox qty = pageControl.FindControl("Qty") as ASPxTextBox;
            ASPxTextBox totalcost = pageControl.FindControl("TotalCost") as ASPxTextBox;


            SqlConnection conn = new SqlConnection(GlobalClass.SQLConnString());
            conn.Open();

            string PK = e.Keys[0].ToString();

            //string update_MRP = "UPDATE " + MRPClass.OpexTable() + " SET [ExpenseCode] = @ExpenseCode, [ItemCode] = @ItemCode , [Description] = @Description, [UOM]= @UOM, [Cost] = @Cost, [Qty] = @Qty, [TotalCost] = @TotalCost, [OprUnit] = @OprUnit, [EdittedQty] = @Qty, [EdittedCost] = @Cost, [EdittedTotalCost] = @TotalCost, [ApprovedQty] = @Qty, [ApprovedCost] = @Cost, [ApprovedTotalCost] = @TotalCost, [DescriptionAddl] = @itemDesc2, [ProcCat] = @ProcCat WHERE [PK] = @PK";

            string code = "";
            if (itemCode.Value != null) code = itemCode.Value.ToString();

            string operating_unit = "";
            if (opunit.Value != null)
                operating_unit = opunit.Value.ToString();

            string desc_two = "";
            if (itemDesc2.Value != null)
                desc_two = itemDesc2.Value.ToString();

            string prod_cat = "";
            //MRPClass.PrintString((ProCatCode != null).ToString());
            if (ProCatCode != null)
                if (ProCatCode.Value != null)
                    prod_cat = ProCatCode.Value.ToString();

            Double qty_float = 0, cost_float = 0, total_float = 0;
            if (qty.Value != null)
            {
                qty_float = Convert.ToDouble(qty.Value.ToString());
            }
            if (cost.Value != null)
            {
                cost_float = Convert.ToDouble(cost.Value.ToString());
            }
            if (totalcost.Value != null)
            {
                total_float = Convert.ToDouble(totalcost.Value.ToString());
            }

            //SqlCommand cmd = new SqlCommand(update_MRP, conn);
            //cmd.Parameters.AddWithValue("@PK", PK);
            //cmd.Parameters.AddWithValue("@OprUnit", operating_unit);
            //cmd.Parameters.AddWithValue("@ExpenseCode", expenseCode.Value.ToString());
            //cmd.Parameters.AddWithValue("@ItemCode", code);
            //cmd.Parameters.AddWithValue("@Description", GlobalClass.FormatSQL(itemDesc.Value.ToString()));
            //cmd.Parameters.AddWithValue("@itemDesc2", GlobalClass.FormatSQL(desc_two));
            //cmd.Parameters.AddWithValue("@UOM", uom.Value.ToString());
            //cmd.Parameters.AddWithValue("@Cost", Convert.ToDouble(cost.Value.ToString()));
            //cmd.Parameters.AddWithValue("@Qty", Convert.ToDouble(qty.Value.ToString()));
            //cmd.Parameters.AddWithValue("@TotalCost", Convert.ToDouble(totalcost.Value.ToString()));
            //cmd.Parameters.AddWithValue("@ProcCat", prod_cat);
            //cmd.CommandType = CommandType.Text;
            //int result = cmd.ExecuteNonQuery();
            int result = QuerySPClass.InsertUpdateOperatingExpense(wrkflwln, 2, Convert.ToInt32(PK), docnumber, 2, expenseCode.Value.ToString(), operating_unit, prod_cat, code, GlobalClass.FormatSQL(itemDesc.Value.ToString()), GlobalClass.FormatSQL(desc_two), uom.Value.ToString(), qty_float, cost_float, total_float);
            if (result > 0)
            {
                MRPClass.UpdateLastModified(conn, docnumber);
                string remarks = MRPClass.opex_logs + "-" + MRPClass.edit_logs;
                MRPClass.AddLogsMOPList(conn, mrp_key, remarks);
            }

            conn.Close();

            string str = Get_Docnumber();
            BindOPEX(str);

            e.Cancel = true;
            grid.CancelEdit();
        }
        protected void ManPowerGrid_BeforeGetCallbackResult(object sender, EventArgs e)
        {
            ASPxGridView grid = sender as ASPxGridView;
            MRPClass.SetBehaviorGrid(grid);

            ASPxPageControl pageControl = grid.FindEditFormTemplateControl("ManPowerPageControl") as ASPxPageControl;
            if (pageControl != null)
            {
                ASPxHiddenField entityhidden = pageControl.FindControl("entityhidden") as ASPxHiddenField;
                if (entitycode != MRPClass.train_entity)
                {
                    HtmlControl div1 = (HtmlControl)pageControl.FindControl("OperatingUnit_label");
                    div1.Style.Add("display", "none");
                    HtmlControl div2 = (HtmlControl)pageControl.FindControl("OperatingUnit_combo");
                    div2.Style.Add("display", "none");

                    entityhidden["hidden_value"] = "not display";
                }
                else
                {
                    entityhidden["hidden_value"] = "display";
                }
            }


        }

        protected void ManPowerGrid_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            CheckCreatorKey();
            //docnumber = Session["mrp_docNum"].ToString();
            bindManPower = false;
        }

        protected void ManPowerGrid_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            CheckCreatorKey();
            docnumber = Session["mrp_docNum"].ToString();

            ASPxGridView grid = sender as ASPxGridView;
            ASPxPageControl pageControl = grid.FindEditFormTemplateControl("ManPowerPageControl") as ASPxPageControl;

            ASPxComboBox opunit = pageControl.FindControl("OperatingUnit") as ASPxComboBox;
            ASPxComboBox actCode = pageControl.FindControl("ActivityCode") as ASPxComboBox;
            ASPxComboBox type = pageControl.FindControl("ManPowerTypeKeyName") as ASPxComboBox;
            ASPxTextBox itemDesc = pageControl.FindControl("Description") as ASPxTextBox;
            ASPxComboBox uom = pageControl.FindControl("UOM") as ASPxComboBox;
            ASPxTextBox cost = pageControl.FindControl("Cost") as ASPxTextBox;
            ASPxTextBox qty = pageControl.FindControl("Qty") as ASPxTextBox;
            ASPxTextBox totalcost = pageControl.FindControl("TotalCost") as ASPxTextBox;

            SqlConnection conn = new SqlConnection(GlobalClass.SQLConnString());
            conn.Open();

            int manpower_type_pk = 0;
            string query = "SELECT [PK] FROM [dbo].[tbl_System_ManPowerType] WHERE [ManPowerTypeDesc] = '" + type.Value.ToString() + "'";
            SqlCommand comm = new SqlCommand(query, conn);
            SqlDataReader reader = comm.ExecuteReader();
            while (reader.Read())
            {
                manpower_type_pk = Convert.ToInt32(reader[0].ToString());
            }
            reader.Close();

            string operating_unit = "";
            if (opunit.Value != null)
                operating_unit = opunit.Value.ToString();

            Double qty_float = 0, cost_float = 0, total_float = 0;
            if (qty.Value != null)
            {
                qty_float = Convert.ToDouble(qty.Value.ToString());
            }
            if (cost.Value != null)
            {
                cost_float = Convert.ToDouble(cost.Value.ToString());
            }
            if (totalcost.Value != null)
            {
                total_float = Convert.ToDouble(totalcost.Value.ToString());
            }

            //string insert = "INSERT INTO " + MRPClass.ManPowerTable() +
            //                " ([HeaderDocNum], [ActivityCode], [ManPowerTypeKey], " +
            //                " [UOM], [Cost], [Qty], [TotalCost], [OprUnit], " +
            //                " [EdittedQty], [EdittedCost], [EdittiedTotalCost], " +
            //                " [ApprovedQty], [ApprovedCost], [ApprovedTotalCost]) " +
            //                " VALUES (@HeaderDocNum, @ActivityCode, @ManPowerTypeKey, " +
            //                " @UOM, @Cost, @Qty, @TotalCost, @OprUnit, " +
            //                " @Qty, @Cost, @TotalCost, @Qty, @Cost, @TotalCost)";

            //SqlCommand cmd = new SqlCommand(insert, conn);
            //cmd.Parameters.AddWithValue("@HeaderDocNum", docnumber);
            //cmd.Parameters.AddWithValue("@OprUnit", operating_unit);
            //cmd.Parameters.AddWithValue("@ActivityCode", actCode.Value.ToString());
            //cmd.Parameters.AddWithValue("@ManPowerTypeKey", manpower_type_pk);
            //cmd.Parameters.AddWithValue("@Description", GlobalClass.FormatSQL(itemDesc.Value.ToString()));
            //cmd.Parameters.AddWithValue("@UOM", uom.Value.ToString());
            //cmd.Parameters.AddWithValue("@Cost", Convert.ToDouble(cost.Value.ToString()));
            //cmd.Parameters.AddWithValue("@Qty", Convert.ToDouble(qty.Value.ToString()));
            //cmd.Parameters.AddWithValue("@TotalCost", Convert.ToDouble(totalcost.Value.ToString()));
            //cmd.CommandType = CommandType.Text;
            //int result = cmd.ExecuteNonQuery();

            int result = QuerySPClass.InsertUpdateManPower(wrkflwln, 1, 0, docnumber, 3, actCode.Value.ToString(), operating_unit, manpower_type_pk, GlobalClass.FormatSQL(itemDesc.Value.ToString()), uom.Value.ToString(), qty_float, cost_float, total_float);
            if (result > 0)
            {
                MRPClass.UpdateLastModified(conn, docnumber);
                string remarks = MRPClass.manpower_logs + "-" + MRPClass.add_logs;
                MRPClass.AddLogsMOPList(conn, mrp_key, remarks);
            }

            //e.Cancel = true;
            //grid.CancelEdit();
            //BindManPower(docnumber);
            int pk_latest = QuerySPClass.MRP_Details_Latest_PK(docnumber, 3);
            //string query_pk = "SELECT TOP 1 [PK] FROM " + MRPClass.ManPowerTable() + " WHERE ([HeaderDocNum] = '" + docnumber + "') ORDER BY [PK] DESC";
            //SqlCommand comm1 = new SqlCommand(query_pk, conn);
            //SqlDataReader r = comm1.ExecuteReader();
            //while (r.Read())
            //{
            //    pk_latest = Convert.ToInt32(r[0].ToString());
            //}

            e.Cancel = true;
            grid.CancelEdit();
            string str = Get_Docnumber();
            BindManPower(str);

            if (pk_latest > 0)
                FocusThisRowGrid(grid, pk_latest);
        }

        protected void ManPowerGrid_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            CheckCreatorKey();
            //docnumber = Session["mrp_docNum"].ToString();
            SqlConnection conn = new SqlConnection(GlobalClass.SQLConnString());
            conn.Open();

            string PK = e.Keys[0].ToString();
            bool Exist = MRPClass.CheckLogsExist(MRPClass.ManpowerTableLogs(), PK);
            if (Exist)//if the material has logs
            {
                conn.Close();
                e.Cancel = true;
            }
            else
            {
                string delete = "DELETE FROM " + MRPClass.ManPowerTable() + " WHERE [PK] ='" + PK + "'";
                SqlCommand cmd = new SqlCommand(delete, conn);
                int result = cmd.ExecuteNonQuery();

                if (result > 0)
                {
                    MRPClass.UpdateLastModified(conn, docnumber);
                    string remarks = MRPClass.manpower_logs + "-" + MRPClass.delete_logs;
                    MRPClass.AddLogsMOPList(conn, mrp_key, remarks);
                }

                conn.Close();
                string str = Get_Docnumber();
                BindManPower(str);
                e.Cancel = true;
            }
        }

        protected void ManPowerGrid_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            CheckCreatorKey();
            //docnumber = Session["mrp_docNum"].ToString();
            bindManPower = false;
        }

        protected void ManPowerGrid_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            CheckCreatorKey();
            docnumber = Session["mrp_docNum"].ToString();

            ASPxGridView grid = sender as ASPxGridView;
            ASPxPageControl pageControl = grid.FindEditFormTemplateControl("ManPowerPageControl") as ASPxPageControl;

            ASPxComboBox opunit = pageControl.FindControl("OperatingUnit") as ASPxComboBox;
            ASPxComboBox actCode = pageControl.FindControl("ActivityCode") as ASPxComboBox;
            ASPxComboBox type = pageControl.FindControl("ManPowerTypeKeyName") as ASPxComboBox;
            ASPxTextBox itemDesc = pageControl.FindControl("Description") as ASPxTextBox;
            ASPxComboBox uom = pageControl.FindControl("UOM") as ASPxComboBox;
            ASPxTextBox cost = pageControl.FindControl("Cost") as ASPxTextBox;
            ASPxTextBox qty = pageControl.FindControl("Qty") as ASPxTextBox;
            ASPxTextBox totalcost = pageControl.FindControl("TotalCost") as ASPxTextBox;

            SqlConnection conn = new SqlConnection(GlobalClass.SQLConnString());
            conn.Open();

            string actcodeVal = MRPClass.ActivityCodeDESCRIPTION(actCode.Value.ToString());
            string PK = e.Keys[0].ToString();

            int manpower_type_pk = 0;
            string query = "SELECT [PK] FROM [dbo].[tbl_System_ManPowerType] WHERE [ManPowerTypeDesc] = '" + type.Value.ToString() + "'";
            SqlCommand comm = new SqlCommand(query, conn);
            SqlDataReader reader = comm.ExecuteReader();
            while (reader.Read())
            {
                manpower_type_pk = Convert.ToInt32(reader[0].ToString());
            }
            reader.Close();

            string operating_unit = "";
            if (opunit.Value != null)
                operating_unit = opunit.Value.ToString();

            Double qty_float = 0, cost_float = 0, total_float = 0;
            if (qty.Value != null)
            {
                qty_float = Convert.ToDouble(qty.Value.ToString());
            }
            if (cost.Value != null)
            {
                cost_float = Convert.ToDouble(cost.Value.ToString());
            }
            if (totalcost.Value != null)
            {
                total_float = Convert.ToDouble(totalcost.Value.ToString());
            }

            //string update_MRP = "UPDATE " + MRPClass.ManPowerTable() +
            //                    " SET [ActivityCode] = @ActivityCode, [ManPowerTypeKey] = @ManPowerTypeKey, " +
            //                    " [UOM]= @UOM, " +
            //                    " [Cost] = @Cost, [Qty] = @Qty, [TotalCost] = @TotalCost, [OprUnit] = @OprUnit, " +
            //                    " [EdittedQty] = @Qty, [EdittedCost] = @Cost, [EdittiedTotalCost] = @TotalCost, " +
            //                    " [ApprovedQty] = @Qty, [ApprovedCost] = @Cost, [ApprovedTotalCost] = @TotalCost " +
            //                    " WHERE [PK] = @PK";

            //SqlCommand cmd = new SqlCommand(update_MRP, conn);
            //cmd.Parameters.AddWithValue("@PK", PK);
            //cmd.Parameters.AddWithValue("@OprUnit", operating_unit);
            //cmd.Parameters.AddWithValue("@ActivityCode", actcodeVal);
            //cmd.Parameters.AddWithValue("@ManPowerTypeKey", manpower_type_pk);
            //cmd.Parameters.AddWithValue("@Description", GlobalClass.FormatSQL(itemDesc.Value.ToString()));
            //cmd.Parameters.AddWithValue("@UOM", uom.Value.ToString());
            //cmd.Parameters.AddWithValue("@Cost", Convert.ToDouble(cost.Value.ToString()));
            //cmd.Parameters.AddWithValue("@Qty", Convert.ToDouble(qty.Value.ToString()));
            //cmd.Parameters.AddWithValue("@TotalCost", Convert.ToDouble(totalcost.Value.ToString()));
            //cmd.CommandType = CommandType.Text;
            //int result = cmd.ExecuteNonQuery();

            int result = QuerySPClass.InsertUpdateManPower(wrkflwln, 2, Convert.ToInt32(PK), docnumber, 3, actcodeVal, operating_unit, manpower_type_pk, GlobalClass.FormatSQL(itemDesc.Value.ToString()), uom.Value.ToString(), qty_float, cost_float, total_float);
            if (result > 0)
            {
                MRPClass.UpdateLastModified(conn, docnumber);
                string remarks = MRPClass.manpower_logs + "-" + MRPClass.edit_logs;
                MRPClass.AddLogsMOPList(conn, mrp_key, remarks);
            }

            conn.Close();

            string str = Get_Docnumber();
            BindManPower(str);
            e.Cancel = true;
            grid.CancelEdit();
        }

        protected void CAPEXGrid_BeforeGetCallbackResult(object sender, EventArgs e)
        {
            ASPxGridView grid = sender as ASPxGridView;
            MRPClass.SetBehaviorGrid(grid);

            ASPxPageControl pageControl = grid.FindEditFormTemplateControl("CAPEXPageControl") as ASPxPageControl;
            if (pageControl != null)
            {
                ASPxHiddenField entityhidden = pageControl.FindControl("entityhidden") as ASPxHiddenField;
                if (entitycode != MRPClass.train_entity)
                {
                    HtmlControl div1 = (HtmlControl)pageControl.FindControl("OperatingUnit_label");
                    div1.Style.Add("display", "none");
                    HtmlControl div2 = (HtmlControl)pageControl.FindControl("OperatingUnit_combo");
                    div2.Style.Add("display", "none");

                    entityhidden["hidden_value"] = "not display";
                }
                else
                {
                    entityhidden["hidden_value"] = "display";
                }
            }
        }

        protected void CAPEXGrid_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            CheckCreatorKey();
            //docnumber = Session["mrp_docNum"].ToString();
            bindCapex = false;
        }

        protected void CAPEXGrid_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            CheckCreatorKey();
            docnumber = Session["mrp_docNum"].ToString();

            ASPxGridView grid = sender as ASPxGridView;
            ASPxPageControl pageControl = grid.FindEditFormTemplateControl("CAPEXPageControl") as ASPxPageControl;

            ASPxComboBox opunit = pageControl.FindControl("OperatingUnit") as ASPxComboBox;
            ASPxComboBox prodcat = pageControl.FindControl("ProdCat") as ASPxComboBox;
            ASPxTextBox itemDesc = pageControl.FindControl("Description") as ASPxTextBox;
            ASPxComboBox uom = pageControl.FindControl("UOM") as ASPxComboBox;
            ASPxTextBox cost = pageControl.FindControl("Cost") as ASPxTextBox;
            ASPxTextBox qty = pageControl.FindControl("Qty") as ASPxTextBox;
            ASPxTextBox totalcost = pageControl.FindControl("TotalCost") as ASPxTextBox;

            SqlConnection conn = new SqlConnection(GlobalClass.SQLConnString());
            conn.Open();

            string operating_unit = "";
            if (opunit.Value != null)
                operating_unit = opunit.Value.ToString();

            Double qty_float = 0, cost_float = 0, total_float = 0;
            if (qty.Value != null)
            {
                qty_float = Convert.ToDouble(qty.Value.ToString());
            }
            if (cost.Value != null)
            {
                cost_float = Convert.ToDouble(cost.Value.ToString());
            }
            if (totalcost.Value != null)
            {
                total_float = Convert.ToDouble(totalcost.Value.ToString());
            }

            //MRPClass.PrintString(prodcat.Value.ToString());

            //string insert = "INSERT INTO " + MRPClass.CapexTable() +
            //                " ([HeaderDocNum],[ProdCat], [Description], [UOM], " +
            //                " [Cost], [Qty], [TotalCost], [OprUnit], " +
            //                " [EdittedQty], [EdittedCost], [EdittiedTotalCost], " +
            //                " [ApprovedQty], [ApprovedCost], [ApprovedTotalCost]) " +
            //                " VALUES (@HeaderDocNum,@ProdCat, @Description, @UOM, " +
            //                " @Cost, @Qty, @TotalCost, @OprUnit, " +
            //                " @Qty, @Cost, @TotalCost, " +
            //                " @Qty, @Cost, @TotalCost)";

            //SqlCommand cmd = new SqlCommand(insert, conn);
            //cmd.Parameters.AddWithValue("@HeaderDocNum", docnumber);
            //cmd.Parameters.AddWithValue("@ProdCat", prodcat.Value.ToString());
            //cmd.Parameters.AddWithValue("@OprUnit", operating_unit);
            //cmd.Parameters.AddWithValue("@Description", GlobalClass.FormatSQL(itemDesc.Value.ToString()));
            //cmd.Parameters.AddWithValue("@UOM", uom.Value.ToString());
            //cmd.Parameters.AddWithValue("@Cost", Convert.ToDouble(cost.Value.ToString()));
            //cmd.Parameters.AddWithValue("@Qty", Convert.ToDouble(qty.Value.ToString()));
            //cmd.Parameters.AddWithValue("@TotalCost", Convert.ToDouble(totalcost.Value.ToString()));
            //cmd.CommandType = CommandType.Text;
            //int result = cmd.ExecuteNonQuery();
            int result = QuerySPClass.InsertUpdateCapitalExpenditures(wrkflwln, 1, 0, docnumber, 4, operating_unit, prodcat.Value.ToString(), GlobalClass.FormatSQL(itemDesc.Value.ToString()), uom.Value.ToString(), qty_float, cost_float, total_float);
            if (result > 0)
            {
                MRPClass.UpdateLastModified(conn, docnumber);
                string remarks = MRPClass.capex_logs + "-" + MRPClass.add_logs;
                MRPClass.AddLogsMOPList(conn, mrp_key, remarks);
            }

            //e.Cancel = true;
            //grid.CancelEdit();
            //BindCAPEX(docnumber);
            int pk_latest = QuerySPClass.MRP_Details_Latest_PK(docnumber, 4);
            //string query_pk = "SELECT TOP 1 [PK] FROM " + MRPClass.CapexTable() + " WHERE ([HeaderDocNum] = '" + docnumber + "') ORDER BY [PK] DESC";
            //SqlCommand comm = new SqlCommand(query_pk, conn);
            //SqlDataReader r = comm.ExecuteReader();
            //while (r.Read())
            //{
            //    pk_latest = Convert.ToInt32(r[0].ToString());
            //}

            e.Cancel = true;
            grid.CancelEdit();

            string str = Get_Docnumber();
            BindCAPEX(str);

            if (pk_latest > 0)
                FocusThisRowGrid(grid, pk_latest);
        }

        protected void CAPEXGrid_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {

            CheckCreatorKey();
            //docnumber = Session["mrp_docNum"].ToString();

            SqlConnection conn = new SqlConnection(GlobalClass.SQLConnString());
            conn.Open();

            string PK = e.Keys[0].ToString();
            bool Exist = MRPClass.CheckLogsExist(MRPClass.CapexTableLogs(), PK);
            if (Exist)//if the material has logs
            {
                conn.Close();
                e.Cancel = true;
            }
            else
            {
                string delete = "DELETE FROM " + MRPClass.CapexTable() + " WHERE [PK] ='" + PK + "'";
                SqlCommand cmd = new SqlCommand(delete, conn);
                int result = cmd.ExecuteNonQuery();

                if (result > 0)
                {
                    MRPClass.UpdateLastModified(conn, docnumber);
                    string remarks = MRPClass.capex_logs + "-" + MRPClass.delete_logs;
                    MRPClass.AddLogsMOPList(conn, mrp_key, remarks);
                }

                conn.Close();
                string str = Get_Docnumber();
                BindCAPEX(str);
                e.Cancel = true;
            }
        }

        protected void CAPEXGrid_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            CheckCreatorKey();
            //docnumber = Session["mrp_docNum"].ToString();
            bindCapex = false;
        }

        protected void CAPEXGrid_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            CheckCreatorKey();
            docnumber = Session["mrp_docNum"].ToString();

            ASPxGridView grid = sender as ASPxGridView;
            ASPxPageControl pageControl = grid.FindEditFormTemplateControl("CAPEXPageControl") as ASPxPageControl;

            ASPxComboBox opunit = pageControl.FindControl("OperatingUnit") as ASPxComboBox;
            ASPxComboBox prodcat = pageControl.FindControl("ProdCat") as ASPxComboBox;
            ASPxTextBox itemDesc = pageControl.FindControl("Description") as ASPxTextBox;
            ASPxComboBox uom = pageControl.FindControl("UOM") as ASPxComboBox;
            ASPxTextBox cost = pageControl.FindControl("Cost") as ASPxTextBox;
            ASPxTextBox qty = pageControl.FindControl("Qty") as ASPxTextBox;
            ASPxTextBox totalcost = pageControl.FindControl("TotalCost") as ASPxTextBox;

            SqlConnection conn = new SqlConnection(GlobalClass.SQLConnString());
            conn.Open();

            string PK = e.Keys[0].ToString();

            string operating_unit = "";
            if (opunit.Value != null)
                operating_unit = opunit.Value.ToString();
            Double qty_float = 0, cost_float = 0, total_float = 0;
            if (qty.Value != null)
            {
                qty_float = Convert.ToDouble(qty.Value.ToString());
            }
            if (cost.Value != null)
            {
                cost_float = Convert.ToDouble(cost.Value.ToString());
            }
            if (totalcost.Value != null)
            {
                total_float = Convert.ToDouble(totalcost.Value.ToString());
            }

            //string update_MRP = "UPDATE " + MRPClass.CapexTable() +
            //                    " SET [Description] = @Description, [ProdCat] = @ProdCat, [UOM]= @UOM, [OprUnit] = @OprUnit, " +
            //                    " [Cost] = @Cost, [Qty] = @Qty, [TotalCost] = @TotalCost, " +
            //                    " [EdittedQty] = @Qty, [EdittedCost] = @Cost, [EdittiedTotalCost] = @TotalCost, " +
            //                    " [ApprovedQty] = @Qty, [ApprovedCost] = @Cost, [ApprovedTotalCost] = @TotalCost " +
            //                    " WHERE [PK] = @PK";

            //SqlCommand cmd = new SqlCommand(update_MRP, conn);
            //cmd.Parameters.AddWithValue("@PK", PK);
            //cmd.Parameters.AddWithValue("@OprUnit", operating_unit);
            //cmd.Parameters.AddWithValue("@ProdCat", prodcat.Value.ToString());
            //cmd.Parameters.AddWithValue("@Description", GlobalClass.FormatSQL(itemDesc.Value.ToString()));
            //cmd.Parameters.AddWithValue("@UOM", uom.Value.ToString());
            //cmd.Parameters.AddWithValue("@Cost", Convert.ToDouble(cost.Value.ToString()));
            //cmd.Parameters.AddWithValue("@Qty", Convert.ToDouble(qty.Value.ToString()));
            //cmd.Parameters.AddWithValue("@TotalCost", Convert.ToDouble(totalcost.Value.ToString()));
            //cmd.CommandType = CommandType.Text;
            //int result = cmd.ExecuteNonQuery();
            int result = QuerySPClass.InsertUpdateCapitalExpenditures(wrkflwln, 2, Convert.ToInt32(PK), docnumber, 4, operating_unit, prodcat.Value.ToString(), GlobalClass.FormatSQL(itemDesc.Value.ToString()), uom.Value.ToString(), qty_float, cost_float, total_float);
            if (result > 0)
            {
                MRPClass.UpdateLastModified(conn, docnumber);
                string remarks = MRPClass.capex_logs + "-" + MRPClass.edit_logs;
                MRPClass.AddLogsMOPList(conn, mrp_key, remarks);
            }

            conn.Close();

            string str = Get_Docnumber();
            BindCAPEX(str);

            e.Cancel = true;
            grid.CancelEdit();
        }

        protected void listbox_Callback(object sender, CallbackEventArgsBase e)
        {
            ASPxPageControl pageControl = DirectMaterialsGrid.FindEditFormTemplateControl("DirectPageControl") as ASPxPageControl;
            ASPxListBox listbox = pageControl.FindControl("listbox") as ASPxListBox;
            listbox.Visible = true;
            listbox.DataSource = MRPClass.AXInventTable(e.Parameter, entitycode);

            //ListBoxColumn l_value = new ListBoxColumn();
            //l_value.FieldName = "ITEMID";
            //listbox.Columns.Add(l_value);

            //ListBoxColumn l_text = new ListBoxColumn();
            //l_text.FieldName = "NAMEALIAS";
            //l_text.Width = 300;
            //listbox.Columns.Add(l_text);

            //ListBoxColumn l_text2 = new ListBoxColumn();
            //l_text2.FieldName = "UOM";
            //l_text2.Width = 150;
            //listbox.Columns.Add(l_text2);

            listbox.ValueField = "ITEMID";
            listbox.TextField = "NAMEALIAS";
            listbox.DataBind();
        }



        protected void listboxOPEX_Callback(object sender, CallbackEventArgsBase e)
        {
            ASPxPageControl pageControl = OPEXGrid.FindEditFormTemplateControl("OPEXPageControl") as ASPxPageControl;
            ASPxListBox listbox = pageControl.FindControl("listboxOPEX") as ASPxListBox;
            ASPxComboBox expCode = pageControl.FindControl("ExpenseCode") as ASPxComboBox;
            listbox.Visible = true;
            listbox.DataSource = MRPClass.AXInventTable(e.Parameter, entitycode);

            listbox.TextField = "NAMEALIAS";
            listbox.DataBind();

            //ListBoxColumn l_value = new ListBoxColumn();
            //l_value.FieldName = "ITEMID";
            //listbox.Columns.Add(l_value);

            //ListBoxColumn l_text = new ListBoxColumn();
            //l_text.FieldName = "NAMEALIAS";
            ////l_text.Width = 300;
            //listbox.Columns.Add(l_text);

            //ListBoxColumn l_text2 = new ListBoxColumn();
            //l_text2.FieldName = "UOM";
            ////l_text2.Width = 150;
            //listbox.Columns.Add(l_text2);

            //listbox.ValueField = "ITEMID";
            //listbox.TextField = "NAMEALIAS";
            ////listbox.Columns.Add();
            //listbox.DataBind();
        }



        protected void RevenueGrid_BeforeGetCallbackResult(object sender, EventArgs e)
        {
            ASPxGridView grid = sender as ASPxGridView;
            MRPClass.SetBehaviorGrid(grid);

            ASPxPageControl pageControl = grid.FindEditFormTemplateControl("RevenuePageControl") as ASPxPageControl;
            if (pageControl != null)
            {
                ASPxHiddenField entityhidden = pageControl.FindControl("entityhidden") as ASPxHiddenField;
                if (entitycode != MRPClass.train_entity)
                {
                    HtmlControl div1 = (HtmlControl)pageControl.FindControl("OperatingUnit_label");
                    div1.Style.Add("display", "none");
                    HtmlControl div2 = (HtmlControl)pageControl.FindControl("OperatingUnit_combo");
                    div2.Style.Add("display", "none");

                    entityhidden["hidden_value"] = "not display";
                }
                else
                {
                    entityhidden["hidden_value"] = "display";
                }
            }
        }

        protected void RevenueGrid_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            CheckCreatorKey();
            //docnumber = Session["mrp_docNum"].ToString();
            bindRevenue = false;
        }

        protected void MRPList_Click(object sender, EventArgs e)
        {
            Response.Redirect("mrp_list.aspx");
        }

        protected void Preview_Click(object sender, EventArgs e)
        {

            if (iStatusKey == 4)
            {
                Session["mrp_docNum"] = docnumber.ToString();
                Session["mrp_source"] = "1";
                Response.Redirect("mrp_preview_approve.aspx?DocNum=" + docnumber.ToString() + "&Source=1");
            }
            else if (iStatusKey == 3)
            {
                Session["mrp_docNum"] = docnumber.ToString();
                Session["mrp_source"] = "1";
                Response.Redirect("mrp_preview_approve.aspx?DocNum=" + docnumber.ToString() + "&Source=1");
            }
            else if (iStatusKey == 2)
            {
                if (Convert.ToInt32(CurrentWorkFlowTxt.Text.ToString()) == 0 || Convert.ToInt32(CurrentWorkFlowTxt.Text.ToString()) == 1)
                {
                    Session["mrp_docNum"] = docnumber.ToString();
                    Session["mrp_wrkLine"] = wrkflwln.ToString();
                    Response.Redirect("mrp_preview.aspx?DocNum=" + docnumber.ToString() + "&WrkFlwLn=" + wrkflwln.ToString());
                }
                else
                {
                    Session["mrp_docNum"] = docnumber.ToString();
                    Session["mrp_source"] = "1";
                    Response.Redirect("mrp_preview_approve.aspx?DocNum=" + docnumber.ToString() + "&Source=1");
                }
            }
            else
            {
                Session["mrp_docNum"] = docnumber.ToString();
                Session["mrp_wrkLine"] = wrkflwln.ToString();
                Response.Redirect("mrp_preview.aspx?DocNum=" + docnumber.ToString() + "&WrkFlwLn=" + wrkflwln.ToString());
            }
            //Response.RedirectLocation = "mrp_preview.aspx?DocNum=" + docnumber.ToString();

        }



        protected void RevenueGrid_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            CheckCreatorKey();
            docnumber = Session["mrp_docNum"].ToString();

            ASPxGridView grid = sender as ASPxGridView;
            ASPxPageControl pageControl = grid.FindEditFormTemplateControl("RevenuePageControl") as ASPxPageControl;

            ASPxComboBox opunit = pageControl.FindControl("OperatingUnit") as ASPxComboBox;
            ASPxTextBox product = pageControl.FindControl("ProductName") as ASPxTextBox;
            ASPxTextBox farm = pageControl.FindControl("FarmName") as ASPxTextBox;
            ASPxTextBox prize = pageControl.FindControl("Prize") as ASPxTextBox;
            ASPxTextBox volume = pageControl.FindControl("Volume") as ASPxTextBox;
            ASPxTextBox totalprize = pageControl.FindControl("TotalPrize") as ASPxTextBox;
            ASPxComboBox uom = pageControl.FindControl("UOM") as ASPxComboBox;

            SqlConnection conn = new SqlConnection(GlobalClass.SQLConnString());
            conn.Open();

            string operating_unit = "", product_val = "", farm_val = "";
            if (opunit.Value != null)
            {
                operating_unit = opunit.Value.ToString();
            }

            if (product.Value != null)
            {
                product_val = product.Value.ToString();
            }

            string uom_val = (uom.Value != null) ? uom.Value.ToString() : "";

            //if (farm.Value != null)
            //{
            //    farm_val = farm.Value.ToString();
            //}

            Double prize_float = 0, volume_float = 0, total_float = 0;
            if (prize.Value != null)
            {
                prize_float = Convert.ToDouble(prize.Value.ToString());
            }
            if (volume.Value != null)
            {
                volume_float = Convert.ToDouble(volume.Value.ToString());
            }
            if (totalprize.Value != null)
            {
                total_float = Convert.ToDouble(totalprize.Value.ToString());
            }

            //string insert = "INSERT INTO " + MRPClass.RevenueTable() + " ([HeaderDocNum], [ProductName], [FarmName], [Prize], [Volume], [TotalPrize], [OprUnit]) VALUES (@HeaderDocNum, @ProductName, @FarmName, @Prize, @Volume, @TotalPrize, @OprUnit)";

            //string insert = "INSERT INTO " + MRPClass.RevenueTable() + " ([HeaderDocNum], [ProductName], [Prize], [Volume], [TotalPrize], [OprUnit]) VALUES (@HeaderDocNum, @ProductName, @Prize, @Volume, @TotalPrize, @OprUnit)";


            //SqlCommand cmd = new SqlCommand(insert, conn);
            //cmd.Parameters.AddWithValue("@HeaderDocNum", docnumber);
            //cmd.Parameters.AddWithValue("@OprUnit", operating_unit);
            //cmd.Parameters.AddWithValue("@ProductName", GlobalClass.FormatSQL(product.Value.ToString()));
            ////cmd.Parameters.AddWithValue("@FarmName", farm.Value.ToString());
            //cmd.Parameters.AddWithValue("@Prize", Convert.ToDouble(prize.Value.ToString()));
            //cmd.Parameters.AddWithValue("@Volume", Convert.ToDouble(volume.Value.ToString()));
            //cmd.Parameters.AddWithValue("@TotalPrize", Convert.ToDouble(totalprize.Value.ToString()));
            //cmd.CommandType = CommandType.Text;
            //int result = cmd.ExecuteNonQuery();

            int result = QuerySPClass.InsertUpdateRevenueAssumptions(1, 0, docnumber, operating_unit, GlobalClass.FormatSQL(product_val.ToString()), GlobalClass.FormatSQL(farm_val.ToString()), prize_float, volume_float, total_float, uom_val);
            if (result > 0)
            {
                MRPClass.UpdateLastModified(conn, docnumber);
                string remarks = MRPClass.revenueassumption_logs + "-" + MRPClass.add_logs;
                MRPClass.AddLogsMOPList(conn, mrp_key, remarks);
            }

            //e.Cancel = true;
            //grid.CancelEdit();
            //BindRevenue(docnumber);
            int pk_latest = QuerySPClass.MRP_Details_Latest_PK(docnumber, 5);
            //string query_pk = "SELECT TOP 1 [PK] FROM " + MRPClass.RevenueTable() + " WHERE ([HeaderDocNum] = '" + docnumber + "') ORDER BY [PK] DESC";
            //SqlCommand comm1 = new SqlCommand(query_pk, conn);
            //SqlDataReader r = comm1.ExecuteReader();
            //while (r.Read())
            //{
            //    pk_latest = Convert.ToInt32(r[0].ToString());
            //}

            e.Cancel = true;
            grid.CancelEdit();
            BindRevenue(docnumber);

            if (pk_latest > 0)
                FocusThisRowGrid(grid, pk_latest);
        }

        protected void RevenueGrid_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            CheckCreatorKey();
            //docnumber = Session["mrp_docNum"].ToString();

            SqlConnection conn = new SqlConnection(GlobalClass.SQLConnString());
            conn.Open();

            string PK = e.Keys[0].ToString();
            bool Exist = MRPClass.CheckLogsExist(MRPClass.RevenueTableLogs(), PK);
            if (Exist)//if the material has logs
            {
                conn.Close();
                e.Cancel = true;
            }
            else
            {
                string delete = "DELETE FROM " + MRPClass.RevenueTable() + " WHERE [PK] ='" + PK + "'";
                SqlCommand cmd = new SqlCommand(delete, conn);
                int result = cmd.ExecuteNonQuery();
                if (result > 0)
                {
                    MRPClass.UpdateLastModified(conn, docnumber);
                    string remarks = MRPClass.revenueassumption_logs + "-" + MRPClass.delete_logs;
                    MRPClass.AddLogsMOPList(conn, mrp_key, remarks);
                }
                conn.Close();
                BindRevenue(docnumber);
                e.Cancel = true;
            }
        }

        protected void DirectMaterialsGrid_CancelRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            string str = Get_Docnumber();
            BindDirectMaterials(str);
        }

        protected void OPEXGrid_CancelRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            string str = Get_Docnumber();
            BindOPEX(str);
        }

        protected void ManPowerGrid_CancelRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            string str = Get_Docnumber();
            BindManPower(str);
        }

        protected void CAPEXGrid_CancelRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            string str = Get_Docnumber();
            BindCAPEX(str);
        }

        protected void RevenueGrid_CancelRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            BindRevenue(docnumber);
        }

        protected void ProdCat_Init(object sender, EventArgs e)
        {
            ASPxComboBox combo = sender as ASPxComboBox;
            //combo.DataSource = MRPClass.ProCategoryTable_WithoutAll();
            combo.DataSource = MRPClass.ProCategoryTableWithType(entitycode, "FixedAsset");

            ListBoxColumn lv = new ListBoxColumn();
            lv.FieldName = "NAME";
            lv.Caption = "Code";
            lv.Width = 80;
            combo.Columns.Add(lv);

            ListBoxColumn lt = new ListBoxColumn();
            lt.FieldName = "DESCRIPTION";
            lt.Caption = "Description";
            lt.Width = 300;
            combo.Columns.Add(lt);

            combo.ValueField = "NAME";
            combo.TextField = "DESCRIPTION";
            combo.DataBind();
            combo.TextFormatString = "{1}";

            GridViewEditFormTemplateContainer container = combo.NamingContainer.NamingContainer as GridViewEditFormTemplateContainer;
            if (!container.Grid.IsNewRowEditing)
            {
                combo.Value = DataBinder.Eval(container.DataItem, "ProdCode").ToString();
                combo.Text = DataBinder.Eval(container.DataItem, "ProdCat").ToString();
            }
            else
            {
                HttpCookie cookie_value = null;
                HttpCookie cookie_text = null;

                cookie_value = Request.Cookies["caproductvalue"];
                cookie_text = Request.Cookies["caproducttext"];
                if (cookie_value != null && cookie_text != null)
                {
                    object valueCA = cookie_value.Value;
                    object text = cookie_text.Value;

                    combo.Text = text.ToString();
                    combo.Value = valueCA;


                    MRPClass.PrintString(valueCA.ToString());
                    MRPClass.PrintString(text.ToString());
                }

                //MRPClass.PrintString(combo.Value.ToString());
            }
        }

        protected void ProcCatOPEXCallback_Callback(object sender, CallbackEventArgsBase e)
        {
            ASPxPageControl pageControl = OPEXGrid.FindEditFormTemplateControl("OPEXPageControl") as ASPxPageControl;
            ASPxComboBox expCode = pageControl.FindControl("ExpenseCode") as ASPxComboBox;
            ASPxCallbackPanel callBackPanel = pageControl.FindControl("ProcCatOPEXCallback") as ASPxCallbackPanel;
            ASPxComboBox proCat = callBackPanel.FindControl("ProcCatOPEX") as ASPxComboBox;

            proCat.Value = "";
            proCat.Text = "";

            DataTable dtRecord = MRPClass.ProCategoryTableWithType(entitycode, "Expense", expCode.Value.ToString());
            proCat.DataSource = dtRecord;

            proCat.TextField = "DESCRIPTION";
            proCat.ValueField = "NAME";
            proCat.TextFormatString = "{1}";
            proCat.DataBind();
        }

        protected void OPEXGrid_HtmlEditFormCreated(object sender, ASPxGridViewEditFormEventArgs e)
        {
            //ScriptManager.RegisterStartupScript(this.Page, typeof(string), "Cookies", "AddEditDisplay.cookiesCondition();", true);
            ASPxGridView grid = sender as ASPxGridView;

            ASPxPageControl pageControl = grid.FindEditFormTemplateControl("OPEXPageControl") as ASPxPageControl;
            ASPxTextBox Description = pageControl.FindControl("Description") as ASPxTextBox;
            ASPxTextBox ItemCode = pageControl.FindControl("ItemCode") as ASPxTextBox;
            HtmlControl div1 = (HtmlControl)pageControl.FindControl("div1");
            HtmlControl div2 = (HtmlControl)pageControl.FindControl("div2");
            HtmlControl CA_prodcombo_div = (HtmlControl)pageControl.FindControl("CA_prodcombo_div");
            HtmlControl CA_prodlabel_div = (HtmlControl)pageControl.FindControl("CA_prodlabel_div");

            if (grid.IsNewRowEditing)
            {
                HttpCookie cookie_isItem = null;
                HttpCookie cookie_isProdCat = null;

                cookie_isItem = Request.Cookies["opisItem"];
                cookie_isProdCat = Request.Cookies["opisProdCat"];
                if (cookie_isItem != null && cookie_isProdCat != null)
                {
                    MRPClass.PrintString(cookie_isItem.Value.ToString());
                    //div1 and div2 is ItemCode Combobox
                    switch (cookie_isItem.Value)
                    {
                        case "0":
                            div1.Style.Add("display", "none");
                            div2.Style.Add("display", "none");

                            Description.Text = "";
                            //old function change June 4, 2019 (See Documentation)
                            //Description.ReadOnly = false;

                            ItemCode.Text = "";
                            break;
                        case "1":
                            div1.Style.Add("display", "block");
                            div2.Style.Add("display", "block");

                            Description.Text = "";
                            //old function change June 4, 2019 (See Documentation)
                            //Description.ReadOnly = true;
                            ItemCode.Text = "";
                            break;
                    }

                    switch (cookie_isProdCat.Value)
                    {
                        case "0"://hide product category combobox
                                 //document.getElementById("CA_prodcombo_div").style.display = "none";
                                 //document.getElementById("CA_prodlabel_div").style.display = "none";

                            CA_prodcombo_div.Style.Add("display", "none");
                            CA_prodlabel_div.Style.Add("display", "none");
                            break;
                        case "1"://show product category combobox
                            CA_prodcombo_div.Style.Add("display", "block");
                            CA_prodlabel_div.Style.Add("display", "block");
                            break;
                    }
                }
            }
            else if (grid.IsEditing)
            {
                string isItem = grid.GetRowValues(grid.EditingRowVisibleIndex, new string[] { "isItem" }).ToString();
                string isProdCategory = grid.GetRowValues(grid.EditingRowVisibleIndex, new string[] { "isProdCategory" }).ToString();
                //string isItem = OPEXGrid.GetRowValuesByKeyValue(e.EditingKeyValue, "isItem").ToString();
                //string isProdCategory = OPEXGrid.GetRowValuesByKeyValue(e.EditingKeyValue, "isProdCategory").ToString();

                switch (isItem)
                {
                    case "0":
                        div1.Style.Add("display", "none");
                        div2.Style.Add("display", "none");

                        Description.Text = "";
                        //old function change June 4, 2019 (See Documentation)
                        //Description.ReadOnly = false;
                        ItemCode.Text = "";
                        break;
                    case "1":
                        div1.Style.Add("display", "block");
                        div2.Style.Add("display", "block");

                        Description.Text = "";
                        //old function change June 4, 2019 (See Documentation)
                        //Description.ReadOnly = true;
                        ItemCode.Text = "";
                        break;
                }

                switch (isProdCategory)
                {
                    case "0"://hide product category combobox
                             //document.getElementById("CA_prodcombo_div").style.display = "none";
                             //document.getElementById("CA_prodlabel_div").style.display = "none";

                        CA_prodcombo_div.Style.Add("display", "none");
                        CA_prodlabel_div.Style.Add("display", "none");
                        break;
                    case "1"://show product category combobox
                        CA_prodcombo_div.Style.Add("display", "block");
                        CA_prodlabel_div.Style.Add("display", "block");
                        break;
                }
            }

            HtmlControl oprLbl = (HtmlControl)pageControl.FindControl("OperatingUnit_label");
            HtmlControl oprCmbo = (HtmlControl)pageControl.FindControl("OperatingUnit_combo");

            if (entitycode == Constants.TRAIN_CODE())
            {
                oprLbl.Style.Add("display", "block");
                oprCmbo.Style.Add("display", "block");
            }
            else
            {
                oprLbl.Style.Add("display", "none");
                oprCmbo.Style.Add("display", "none");
            }
        }

        protected void ASPxPageControl1_Load(object sender, EventArgs e)
        {
            ASPxPageControl pagecontrol = sender as ASPxPageControl;
            pagecontrol.TabPages[0].Text = Constants.MOP_string();
            pagecontrol.TabPages[1].Text = Constants.REV_string();
            if (entitycode == "0303")
                pagecontrol.TabPages[1].ClientVisible = false;
            else
                pagecontrol.TabPages[1].ClientVisible = true;
        }

        protected void DirectMaterialsGrid_HtmlEditFormCreated(object sender, ASPxGridViewEditFormEventArgs e)
        {
            ASPxGridView grid = sender as ASPxGridView;
            ASPxPageControl pageControl = grid.FindEditFormTemplateControl("DirectPageControl") as ASPxPageControl;

            HtmlControl oprLbl = (HtmlControl)pageControl.FindControl("OperatingUnit_label");
            HtmlControl oprCmbo = (HtmlControl)pageControl.FindControl("OperatingUnit_combo");

            if (entitycode == Constants.TRAIN_CODE())
            {
                oprLbl.Style.Add("display", "block");
                oprCmbo.Style.Add("display", "block");
            }
            else
            {
                oprLbl.Style.Add("display", "none");
                oprCmbo.Style.Add("display", "none");
            }
        }

        protected void ManPowerGrid_HtmlEditFormCreated(object sender, ASPxGridViewEditFormEventArgs e)
        {
            ASPxGridView grid = sender as ASPxGridView;
            ASPxPageControl pageControl = grid.FindEditFormTemplateControl("ManPowerPageControl") as ASPxPageControl;

            HtmlControl oprLbl = (HtmlControl)pageControl.FindControl("OperatingUnit_label");
            HtmlControl oprCmbo = (HtmlControl)pageControl.FindControl("OperatingUnit_combo");

            if (entitycode == Constants.TRAIN_CODE())
            {
                oprLbl.Style.Add("display", "block");
                oprCmbo.Style.Add("display", "block");
            }
            else
            {
                oprLbl.Style.Add("display", "none");
                oprCmbo.Style.Add("display", "none");
            }
        }

        protected void CAPEXGrid_HtmlEditFormCreated(object sender, ASPxGridViewEditFormEventArgs e)
        {
            ASPxGridView grid = sender as ASPxGridView;
            ASPxPageControl pageControl = grid.FindEditFormTemplateControl("CAPEXPageControl") as ASPxPageControl;

            HtmlControl oprLbl = (HtmlControl)pageControl.FindControl("OperatingUnit_label");
            HtmlControl oprCmbo = (HtmlControl)pageControl.FindControl("OperatingUnit_combo");

            if (entitycode == Constants.TRAIN_CODE())
            {
                oprLbl.Style.Add("display", "block");
                oprCmbo.Style.Add("display", "block");
            }
            else
            {
                oprLbl.Style.Add("display", "none");
                oprCmbo.Style.Add("display", "none");
            }
        }

        protected void ProcCatOPEX_Init(object sender, EventArgs e)
        {
            ASPxComboBox combo = sender as ASPxComboBox;
            //combo.DataSource = MRPClass.ProCategoryTable_WithoutAll();
            //combo.DataSource = MRPClass.ProCategoryTableWithType(entitycode, "Expense");

            HttpCookie cookie_value = null;
            string expcode = "";
            cookie_value = Request.Cookies["opvalue"];
            if (cookie_value != null)
            {
                expcode = cookie_value.Value;

            }

            combo.DataSource = MRPClass.ProCategoryTableWithType(entitycode, "Expense", expcode);

            ListBoxColumn lv = new ListBoxColumn();
            lv.FieldName = "NAME";
            lv.Caption = "Code";
            lv.Width = 80;
            combo.Columns.Add(lv);

            ListBoxColumn lt = new ListBoxColumn();
            lt.FieldName = "DESCRIPTION";
            lt.Caption = "Description";
            lt.Width = 300;
            combo.Columns.Add(lt);

            combo.ValueField = "NAME";
            combo.TextField = "DESCRIPTION";
            combo.DataBind();
            combo.TextFormatString = "{1}";

            if (combo != null)
            {
                //CALLBACK COMBOBOX
                GridViewEditFormTemplateContainer container = combo.NamingContainer.NamingContainer.NamingContainer as GridViewEditFormTemplateContainer;
                if (!container.Grid.IsNewRowEditing)
                {
                    combo.Value = DataBinder.Eval(container.DataItem, "ProcCatCode").ToString();
                    combo.Text = DataBinder.Eval(container.DataItem, "ProcCatName").ToString();
                }
                else
                {
                    HttpCookie cookie_text = null;

                    cookie_value = Request.Cookies["opproductvalue"];
                    cookie_text = Request.Cookies["opproducttext"];
                    if (cookie_value != null && cookie_text != null)
                    {
                        object val = cookie_value.Value;
                        object text = cookie_text.Value;
                        combo.Value = val;
                        combo.Text = text.ToString();
                    }
                }
            }
        }

        protected void RevenueGrid_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            CheckCreatorKey();
            //docnumber = Session["mrp_docNum"].ToString();
            bindRevenue = false;
        }

        protected void RevenueGrid_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            CheckCreatorKey();
            docnumber = Session["mrp_docNum"].ToString();

            ASPxGridView grid = sender as ASPxGridView;
            ASPxPageControl pageControl = grid.FindEditFormTemplateControl("RevenuePageControl") as ASPxPageControl;

            ASPxComboBox opunit = pageControl.FindControl("OperatingUnit") as ASPxComboBox;
            ASPxTextBox product = pageControl.FindControl("ProductName") as ASPxTextBox;
            //ASPxTextBox farm = pageControl.FindControl("FarmName") as ASPxTextBox;
            ASPxTextBox prize = pageControl.FindControl("Prize") as ASPxTextBox;
            ASPxTextBox volume = pageControl.FindControl("Volume") as ASPxTextBox;
            ASPxTextBox totalprize = pageControl.FindControl("TotalPrize") as ASPxTextBox;
            ASPxComboBox uom = pageControl.FindControl("UOM") as ASPxComboBox;

            SqlConnection conn = new SqlConnection(GlobalClass.SQLConnString());
            conn.Open();

            string PK = e.Keys[0].ToString();

            string operating_unit = "";
            if (opunit.Value != null)
                operating_unit = opunit.Value.ToString();

            Double prize_float = 0, volume_float = 0, total_float = 0;
            if (prize.Value != null)
            {
                prize_float = Convert.ToDouble(prize.Value.ToString());
            }
            if (volume.Value != null)
            {
                volume_float = Convert.ToDouble(volume.Value.ToString());
            }
            if (totalprize.Value != null)
            {
                total_float = Convert.ToDouble(totalprize.Value.ToString());
            }

            string uom_val = (uom.Value != null) ? uom.Value.ToString() : "";
            //string farm_val = (farm.Value != null) ? farm.Value.ToString() : "";

            System.Diagnostics.Debug.WriteLine(uom_val);

            //string update_MRP = "UPDATE " + MRPClass.RevenueTable() + " SET [ProductName] = @ProductName [Prize] = @Prize, [Volume] = @Volume, [TotalPrize] = @TotalPrize, [OprUnit] = @OprUnit WHERE [PK] = @PK";

            //SqlCommand cmd = new SqlCommand(update_MRP, conn);
            //cmd.Parameters.AddWithValue("@PK", PK);
            //cmd.Parameters.AddWithValue("@OprUnit", operating_unit);
            //cmd.Parameters.AddWithValue("@ProductName", GlobalClass.FormatSQL(product.Value.ToString()));
            ////cmd.Parameters.AddWithValue("@FarmName", farm.Value.ToString());
            //cmd.Parameters.AddWithValue("@Prize", Convert.ToDouble(prize.Value.ToString()));
            //cmd.Parameters.AddWithValue("@Volume", Convert.ToDouble(volume.Value.ToString()));
            //cmd.Parameters.AddWithValue("@TotalPrize", Convert.ToDouble(totalprize.Value.ToString()));
            //cmd.CommandType = CommandType.Text;
            //int result = cmd.ExecuteNonQuery();
            int result = QuerySPClass.InsertUpdateRevenueAssumptions(2, Convert.ToInt32(PK), docnumber, operating_unit, GlobalClass.FormatSQL(product.Value.ToString()), GlobalClass.FormatSQL(""), prize_float, volume_float, total_float, uom_val);
            if (result > 0)
            {
                MRPClass.UpdateLastModified(conn, docnumber);
                string remarks = MRPClass.revenueassumption_logs + "-" + MRPClass.edit_logs;
                MRPClass.AddLogsMOPList(conn, mrp_key, remarks);
            }

            conn.Close();

            BindRevenue(docnumber);
            e.Cancel = true;
            grid.CancelEdit();
        }



        protected void DirectMaterialsGrid_DataBound(object sender, EventArgs e)
        {
            ASPxGridView grid = sender as ASPxGridView;
            MRPClass.VisibilityRevDesc(grid, entitycode);
        }

        protected void OPEXGrid_DataBound(object sender, EventArgs e)
        {
            ASPxGridView grid = sender as ASPxGridView;
            MRPClass.VisibilityRevDesc(grid, entitycode);
        }

        protected void ManPowerGrid_DataBound(object sender, EventArgs e)
        {
            ASPxGridView grid = sender as ASPxGridView;
            MRPClass.VisibilityRevDesc(grid, entitycode);
        }

        protected void CAPEXGrid_DataBound(object sender, EventArgs e)
        {
            ASPxGridView grid = sender as ASPxGridView;
            MRPClass.VisibilityRevDesc(grid, entitycode);
        }

        protected void RevenueGrid_DataBound(object sender, EventArgs e)
        {
            ASPxGridView grid = sender as ASPxGridView;
            MRPClass.VisibilityRevDesc(grid, entitycode);
        }

        protected void Submit_Click(object sender, EventArgs e)
        {
            if (wrkflwln == 0)
            {
                if (iStatusKey == 1)
                {

                    //MRPClass.Submit_MRP(docnumber.ToString(), mrp_key, wrkflwln + 1, entitycode, buCode, Convert.ToInt32(Session["CreatorKey"]));
                    PopupSubmit.ShowOnPageLoad = false;
                    ScriptManager.RegisterStartupScript(this.Page, typeof(string), "Resize", "changeWidth.resizeWidth();", true);

                    MRPSubmitClass.MRP_Submit(docnumber.ToString(), mrp_key, dateCreated, wrkflwln, entitycode, buCode, Convert.ToInt32(Session["CreatorKey"]));

                    Load_MRP(docnumber);
                    BindDirectMaterials(docnumber);
                    BindOPEX(docnumber);
                    BindManPower(docnumber);
                    BindCAPEX(docnumber);
                    BindRevenue(docnumber);

                    ModalPopupExtenderLoading.Hide();

                    if (MRPSubmitClass.SubmitError == "")
                    {
                        MRPNotify.HeaderText = "Info";
                        MRPNotificationMessage.Text = "Successfully Submitted!";
                        MRPNotify.ShowOnPageLoad = true;
                    }
                    else
                    {
                        MRPNotify.HeaderText = "Error";
                        MRPNotificationMessage.Text = MRPSubmitClass.SubmitError.ToString();
                        MRPNotify.ShowOnPageLoad = true;
                    }

                }
                else
                {

                    ScriptManager.RegisterStartupScript(this.Page, typeof(string), "Resize", "changeWidth.resizeWidth();", true);

                    MRPNotificationMessage.Text = "Document already submitted to BU / SSU Lead for review.";
                    MRPNotify.HeaderText = "Alert";
                    MRPNotify.ShowOnPageLoad = true;
                    //MRPNotify.
                }
            }
            else
            {
                if (MRPClass.MRP_Line_Status(mrp_key, wrkflwln) == 0)
                {
                    bool isAllowed = false;
                    if (GlobalClass.IsSuperAdmin(Convert.ToInt32(Session["CreatorKey"])))
                    {
                        isAllowed = true;
                    }
                    else
                    {
                        isAllowed = GlobalClass.IsAllowed(Convert.ToInt32(Session["CreatorKey"]), "MOPBULead", dateCreated, entitycode, buCode);
                    }

                    if (isAllowed == true)
                    {
                        //MRPClass.Submit_MRP(docnumber.ToString(), mrp_key, wrkflwln + 1, entitycode, buCode, Convert.ToInt32(Session["CreatorKey"]));
                        PopupSubmit.ShowOnPageLoad = false;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(string), "Resize", "changeWidth.resizeWidth();", true);
                        MRPSubmitClass.MRP_Submit(docnumber.ToString(), mrp_key, dateCreated, wrkflwln, entitycode, buCode, Convert.ToInt32(Session["CreatorKey"]));
                        Load_MRP(docnumber);
                        BindDirectMaterials(docnumber);
                        BindOPEX(docnumber);
                        BindManPower(docnumber);
                        BindCAPEX(docnumber);
                        BindRevenue(docnumber);

                        ModalPopupExtenderLoading.Hide();

                        if (MRPSubmitClass.SubmitError == "")
                        {
                            MRPNotify.HeaderText = "Info";
                            MRPNotificationMessage.Text = "Successfully Submitted!";
                            MRPNotify.ShowOnPageLoad = true;
                        }
                        else
                        {
                            MRPNotify.HeaderText = "Error";
                            MRPNotificationMessage.Text = MRPSubmitClass.SubmitError.ToString();
                            MRPNotify.ShowOnPageLoad = true;
                        }
                    }
                    else
                    {
                        MRPNotificationMessage.Text = "You have no permission to perform this command!" + Environment.NewLine + "Access Denied!";
                        MRPNotificationMessage.ForeColor = System.Drawing.Color.Red;
                        MRPNotify.HeaderText = "Info";
                        MRPNotify.ShowOnPageLoad = true;
                    }

                }
                else
                {
                    //ScriptManager.RegisterStartupScript(this.Page, typeof(string), "Resize", "changeWidth.resizeWidth();", true);

                    MRPNotificationMessage.Text = "Document already submitted to Inventory Analyst for review.";
                    MRPNotify.HeaderText = "Alert";
                    MRPNotify.ShowOnPageLoad = true;
                }

            }

        }
    }
}