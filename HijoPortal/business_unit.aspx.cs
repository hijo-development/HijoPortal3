using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HijoPortal.classes;
using DevExpress.Web;

namespace HijoPortal
{
    public partial class business_unit : System.Web.UI.Page
    {
        private static bool bindBUSSUList = true;
        private static bool bindCreatorList = true;
        private static string sEntCode = "";
        private static string sBUCode = "";

        private static string sBUSSUMasterKey = "";
        private static string sBUSSUCreatorKey = "";
        private static string sBUSSUCreatorStatusKey = "";
        private static string sBUSSULeadKey = "";

        private static int iBUSSUMasterKey = 0;
        private static int iBUSSUCreatorKey = 0;
        private static int iBUSSUCreatorStatusKey = 0;
        private static int iBUSSULeadKey = 0;

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

        protected void FocusThisRowGrid(ASPxGridView grid, int keyVal)
        {
            grid.FocusedRowIndex = grid.FindVisibleIndexByKeyValue(keyVal);
        }

        private void BindBUSSUList()
        {
            //MRPClass.PrintString("MRP is bind");
            DataTable dtRecord = GlobalClass.BUSSUListTable();
            grdBUSSU.DataSource = dtRecord;
            grdBUSSU.KeyFieldName = "PK";
            grdBUSSU.DataBind();

        }

        private void BindBUSSUListCreator(int iMasterKey)
        {
            DataTable dtRecord = GlobalClass.BUSSUListCreatorTable(iMasterKey);
            grdCreator.DataSource = dtRecord;
            grdCreator.KeyFieldName = "PK";
            grdCreator.DataBind();
        }

        protected void EntityCode_Init(object sender, EventArgs e)
        {
            DataTable dtRecord = GlobalClass.EntityTable();
            ASPxComboBox combo = sender as ASPxComboBox;
            combo.DataSource = dtRecord;
            ListBoxColumn l_ValueField = new ListBoxColumn();
            l_ValueField.FieldName = "ID";
            l_ValueField.Caption = "CODE";
            l_ValueField.Width = 30;
            combo.Columns.Add(l_ValueField);

            ListBoxColumn l_TextField = new ListBoxColumn();
            l_TextField.FieldName = "NAME";
            combo.Columns.Add(l_TextField);

            combo.ValueField = "ID";
            combo.TextField = "NAME";
            combo.DataBind();

            GridViewEditFormTemplateContainer container = combo.NamingContainer.NamingContainer as GridViewEditFormTemplateContainer;
            //MRPClass.PrintString("exp:" + !container.Grid.IsNewRowEditing);
            if (!container.Grid.IsNewRowEditing)
            {
                combo.Value = DataBinder.Eval(container.DataItem, "EntityCode").ToString();
            }
        }

        protected void BUCode_Init(object sender, EventArgs e)
        {
            DataTable dtRecord = GlobalClass.EntBUSSUTable(sEntCode);
            ASPxComboBox combo = sender as ASPxComboBox;
            combo.DataSource = dtRecord;

            ListBoxColumn l_ValueField = new ListBoxColumn();
            l_ValueField.FieldName = "ID";
            l_ValueField.Caption = "CODE";
            l_ValueField.Width = 30;
            combo.Columns.Add(l_ValueField);

            ListBoxColumn l_TextField = new ListBoxColumn();
            l_TextField.FieldName = "NAME";
            l_TextField.Caption = "NAME";
            combo.Columns.Add(l_TextField);

            combo.ValueField = "ID";
            combo.TextField = "NAME";
            combo.DataBind();

            combo.Value = sBUCode;
        }

        protected void Creator_Init(object sender, EventArgs e)
        {
            DataTable dtRecord = AccountClass.UserListTable();
            ASPxComboBox combo = sender as ASPxComboBox;
            combo.DataSource = dtRecord;
            ListBoxColumn l_ValueField = new ListBoxColumn();
            l_ValueField.FieldName = "ID";
            l_ValueField.Caption = "CODE";
            l_ValueField.Width = 0;
            combo.Columns.Add(l_ValueField);

            ListBoxColumn l_TextField = new ListBoxColumn();
            l_TextField.FieldName = "NAME";
            combo.Columns.Add(l_TextField);

            combo.ValueField = "ID";
            combo.TextField = "NAME";
            combo.DataBind();

            combo.Value = sBUSSUCreatorKey.ToString();
        }

        protected void CreatorStatus_Init(object sender, EventArgs e)
        {
            DataTable dtRecord = AccountClass.UserStatusTable();
            ASPxComboBox combo = sender as ASPxComboBox;
            combo.DataSource = dtRecord;

            ListBoxColumn l_ValueField = new ListBoxColumn();
            l_ValueField.FieldName = "ID";
            l_ValueField.Caption = "CODE";
            l_ValueField.Width = 0;
            combo.Columns.Add(l_ValueField);

            ListBoxColumn l_TextField = new ListBoxColumn();
            l_TextField.FieldName = "NAME";
            l_TextField.Caption = "STATUS";
            combo.Columns.Add(l_TextField);

            combo.ValueField = "ID";
            combo.TextField = "NAME";
            combo.DataBind();

            combo.Value = sBUSSUCreatorStatusKey.ToString();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            CheckCreatorKey();

            if (!Page.IsPostBack)
            {
                //if (GlobalClass.IsAdmin(Convert.ToInt32(Session["CreatorKey"])) == false)
                if (GlobalClass.IsAdmin(Convert.ToInt32(Session["CreatorKey"])) == false && (GlobalClass.IsSuperAdmin(Convert.ToInt32(Session["CreatorKey"]))) == false)
                {
                    Response.Redirect("home.aspx");
                }
                //else
                //{
                //    ScriptManager.RegisterStartupScript(this.Page, typeof(string), "Resize", "changeWidth.resizeWidth();", true);
                //}

                if (bindBUSSUList)
                {
                    BindBUSSUList();
                }
                else
                {
                    bindBUSSUList = true;
                }

                if (bindCreatorList)
                {
                    int iMaster = 0;
                    if (grdBUSSU.VisibleRowCount > 0)
                    {
                        iMaster = Convert.ToInt32(grdBUSSU.GetRowValues(grdBUSSU.FocusedRowIndex, "PK").ToString());
                    }
                    BindBUSSUListCreator(iMaster);
                }
                else
                {
                    bindCreatorList = true;
                }
            }
        }

        protected void BUSSUCallBackPanel_Callback(object sender, CallbackEventArgsBase e)
        {
            ASPxPageControl pageControl = grdBUSSU.FindEditFormTemplateControl("BUHeadPageControl") as ASPxPageControl;
            ASPxComboBox entCode = pageControl.FindControl("EntityCode") as ASPxComboBox;

            ASPxCallbackPanel callBackPanel = pageControl.FindControl("BUSSUCallBackPanel") as ASPxCallbackPanel;
            ASPxComboBox buCode = callBackPanel.FindControl("BUCode") as ASPxComboBox;

            buCode.Value = "";
            buCode.Text = "";

            DataTable dtRecord = GlobalClass.EntBUSSUTable(entCode.Value.ToString());
            buCode.DataSource = dtRecord;

            buCode.TextField = "NAME";
            buCode.ValueField = "ID";
            buCode.TextFormatString = "{1}";
            buCode.DataBind();
        }

        protected void grdBUSSU_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            bindBUSSUList = false;
            sEntCode = "";
            sBUCode = "";
        }

        protected void grdBUSSU_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            ASPxGridView grid = sender as ASPxGridView;
            ASPxPageControl pageControl = grid.FindEditFormTemplateControl("BUSSUPageControl") as ASPxPageControl;
            //ASPxDateEdit effectDate = pageControl.FindControl("EffectDate") as ASPxDateEdit;
            ASPxComboBox entCode = pageControl.FindControl("EntityCode") as ASPxComboBox;
            ASPxCallbackPanel callBackPanel = pageControl.FindControl("BUSSUCallBackPanel") as ASPxCallbackPanel;
            ASPxComboBox buCode = callBackPanel.FindControl("BUCode") as ASPxComboBox;

            SqlConnection conn = new SqlConnection(GlobalClass.SQLConnString());
            conn.Open();

            //string sCtrlNum = GlobalClass.GetControl_DocNum("BU_Dept_Head", Convert.ToDateTime(effectDate.Value.ToString()));
            string sLastModified = DateTime.Now.ToString();

            string sbuCode = "";
            if (buCode.Value != null)
            {
                sbuCode = buCode.Value.ToString();
            }

            string insert = "INSERT INTO tbl_System_BusinessUnit (EntityCode, BUDeptCode, LastModified) VALUES (@EntityCode, @BUDeptCode, @LastModified)";

            SqlCommand cmd = new SqlCommand(insert, conn);
            cmd.Parameters.AddWithValue("@EntityCode", entCode.Value.ToString());
            cmd.Parameters.AddWithValue("@BUDeptCode", sbuCode.ToString());
            cmd.Parameters.AddWithValue("@LastModified", sLastModified);
            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();

            e.Cancel = true;
            grid.CancelEdit();
            BindBUSSUList();

            int pk_latest = 0;
            string query_pk = "SELECT TOP 1 [PK] FROM tbl_System_BusinessUnit ORDER BY [PK] DESC";
            SqlCommand comm = new SqlCommand(query_pk, conn);
            SqlDataReader r = comm.ExecuteReader();
            while (r.Read())
            {
                pk_latest = Convert.ToInt32(r[0].ToString());
            }
            conn.Close();
            if (pk_latest > 0)
                FocusThisRowGrid(grid, pk_latest);
        }

        protected void grdBUSSU_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            bindBUSSUList = false;
            sEntCode = grdBUSSU.GetRowValues(grdBUSSU.FocusedRowIndex, "EntityCode").ToString();
            sBUCode = grdBUSSU.GetRowValues(grdBUSSU.FocusedRowIndex, "BUDeptCode").ToString();
        }

        protected void grdBUSSU_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            ASPxGridView grid = sender as ASPxGridView;
            ASPxPageControl pageControl = grid.FindEditFormTemplateControl("BUSSUPageControl") as ASPxPageControl;
            ASPxComboBox entCode = pageControl.FindControl("EntityCode") as ASPxComboBox;
            ASPxCallbackPanel callBackPanel = pageControl.FindControl("BUSSUCallBackPanel") as ASPxCallbackPanel;
            ASPxComboBox buCode = callBackPanel.FindControl("BUCode") as ASPxComboBox;

            SqlConnection conn = new SqlConnection(GlobalClass.SQLConnString());
            conn.Open();

            string PK = e.Keys[0].ToString();
            string sLastModified = DateTime.Now.ToString();
            string sbuCode = "";
            if (buCode.Value != null)
            {
                sbuCode = buCode.Value.ToString();
            }
            string update_MRP = "UPDATE tbl_System_BusinessUnit SET [EntityCode] = @EntCode, [BUDeptCode] = @BUCode, [LastModified] = @LastModified WHERE [PK] = @PK";

            SqlCommand cmd = new SqlCommand(update_MRP, conn);
            cmd.Parameters.AddWithValue("@PK", PK);
            cmd.Parameters.AddWithValue("@EntCode", entCode.Value.ToString());
            cmd.Parameters.AddWithValue("@BUCode", sbuCode.ToString());
            cmd.Parameters.AddWithValue("@LastModified", sLastModified);
            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();

            conn.Close();

            BindBUSSUList();
            e.Cancel = true;
            grid.CancelEdit();
        }

        protected void grdBUSSU_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            SqlConnection conn = new SqlConnection(GlobalClass.SQLConnString());
            conn.Open();

            string PK = e.Keys[0].ToString();
            string delete = "DELETE FROM tbl_System_BusinessUnit WHERE [PK] ='" + PK + "'";
            SqlCommand cmd = new SqlCommand(delete, conn);
            cmd.ExecuteNonQuery();
            conn.Close();
            BindBUSSUList();
            e.Cancel = true;
            conn.Close();
        }

        protected void grdBUSSU_BeforeGetCallbackResult(object sender, EventArgs e)
        {
            ASPxGridView grid = sender as ASPxGridView;
            if (grid.IsEditing || grid.IsNewRowEditing)
            {
                grid.SettingsBehavior.AllowSort = false;
                grid.SettingsBehavior.AllowAutoFilter = false;
                grid.SettingsBehavior.AllowHeaderFilter = false;
            }
            else
            {
                grid.SettingsBehavior.AllowSort = true;
                grid.SettingsBehavior.AllowAutoFilter = true;
                grid.SettingsBehavior.AllowHeaderFilter = true;
            }
        }

        protected void grdCreator_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            ASPxGridView grid = sender as ASPxGridView;

            //MRPClass.PrintString(e.Parameters.ToString());

            if (e.Parameters == "AddNew")
            {
                if (grdBUSSU.VisibleRowCount == 0) { return; }
                bool parseInt = int.TryParse(grdBUSSU.GetRowValues(grdBUSSU.FocusedRowIndex, "PK").ToString(), out iBUSSUMasterKey);
                if (parseInt == false) { return; }
                if (iBUSSUMasterKey == 0) { return; }

                bindCreatorList = false;
                sBUSSUCreatorKey = "";

                grid.AddNewRow();
            }

            if (e.Parameters == "Creator")
            {
                int iMaster = 0;
                if (grdBUSSU.VisibleRowCount > 0)
                {
                    iMaster = Convert.ToInt32(grdBUSSU.GetRowValues(grdBUSSU.FocusedRowIndex, "PK").ToString());
                }
                grid.CancelEdit();

                MRPClass.PrintString(iMaster.ToString());

                BindBUSSUListCreator(iMaster);
            }
        }

        protected void grdCreator_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            sBUSSUMasterKey = "";
            sBUSSUMasterKey = grdBUSSU.GetRowValues(grdBUSSU.FocusedRowIndex, "PK").ToString();
            if (sBUSSUMasterKey == "") { return; }

            grdBUSSU.Enabled = false;
            bindCreatorList = false;
            iBUSSUMasterKey = Convert.ToInt32(sBUSSUMasterKey);
            sBUSSUCreatorKey = "";
            sBUSSUCreatorStatusKey = "";
            
        }

        protected void grdCreator_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            ASPxGridView grid = sender as ASPxGridView;
            ASPxHiddenField masterKey = grid.FindEditRowCellTemplateControl((GridViewDataColumn)grid.Columns["MasterKey"], "ASPxCreatorMasterKeyHiddenField") as ASPxHiddenField;
            ASPxDateEdit effectDate = grid.FindEditRowCellTemplateControl((GridViewDataColumn)grid.Columns["EffectDate"], "CreatorEffectDate") as ASPxDateEdit;
            ASPxComboBox sCreator = grid.FindEditRowCellTemplateControl((GridViewDataColumn)grid.Columns["UserCompleteName"], "Creator") as ASPxComboBox;
            ASPxComboBox sCreatorStat = grid.FindEditRowCellTemplateControl((GridViewDataColumn)grid.Columns["StatusDesc"], "CreatorStatus") as ASPxComboBox;

            SqlConnection conn = new SqlConnection(GlobalClass.SQLConnString());
            conn.Open();

            string sLastModified = DateTime.Now.ToString();

            string insert = "INSERT INTO tbl_System_BusinessUnit_Creator (MasterKey, UserKey, EffectDate, StatusKey) VALUES (@MasterKey, @UserKey, @EffectDate, @StatusKey)";

            SqlCommand cmd = new SqlCommand(insert, conn);
            cmd.Parameters.AddWithValue("@MasterKey", iBUSSUMasterKey.ToString());
            cmd.Parameters.AddWithValue("@UserKey", sCreator.Value.ToString());
            cmd.Parameters.AddWithValue("@EffectDate", effectDate.Value.ToString());
            cmd.Parameters.AddWithValue("@StatusKey", sCreatorStat.Value.ToString());
            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();

            string updateMaster = "UPDATE tbl_System_BusinessUnit SET [LastModified] = @LastModified WHERE ([PK] = @PK)";

            SqlCommand cmd1 = new SqlCommand(updateMaster, conn);
            cmd1.Parameters.AddWithValue("@PK", iBUSSUMasterKey.ToString());
            cmd1.Parameters.AddWithValue("@LastModified", sLastModified.ToString());
            cmd1.CommandType = CommandType.Text;
            cmd1.ExecuteNonQuery();

            e.Cancel = true;
            grid.CancelEdit();
            //BindSCMProcOff_Details(Convert.ToInt32(sMasterKey.Value));
            //BindSCMProcOff_Details(iProcOffMaster);

            BindBUSSUListCreator(iBUSSUMasterKey);

            grdBUSSU.Enabled = true;

            int pk_latest = 0;
            string query_pk = "SELECT TOP 1 [PK] FROM tbl_System_BusinessUnit_Creator ORDER BY [PK] DESC";
            SqlCommand comm = new SqlCommand(query_pk, conn);
            SqlDataReader r = comm.ExecuteReader();
            while (r.Read())
            {
                pk_latest = Convert.ToInt32(r[0].ToString());
            }
            conn.Close();

            if (pk_latest > 0)
            {
                FocusThisRowGrid(grid, pk_latest);
            }
        }

        protected void grdCreator_BeforeGetCallbackResult(object sender, EventArgs e)
        {
            ASPxGridView grid = sender as ASPxGridView;
            MRPClass.SetBehaviorGrid(grid);
        }

        protected void grdCreator_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            sBUSSUMasterKey = "";
            sBUSSUMasterKey = grdBUSSU.GetRowValues(grdBUSSU.FocusedRowIndex, "PK").ToString();
            if (sBUSSUMasterKey == "") { return; }

            bindCreatorList = false;
            iBUSSUMasterKey = Convert.ToInt32(sBUSSUMasterKey);
            sBUSSUCreatorKey = grdCreator.GetRowValues(grdCreator.FocusedRowIndex, "UserKey").ToString();
            sBUSSUCreatorStatusKey = grdCreator.GetRowValues(grdCreator.FocusedRowIndex, "StatusKey").ToString();
        }
    }
}