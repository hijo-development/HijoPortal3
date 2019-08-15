﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using DevExpress.Web;
using System.Net.NetworkInformation;
using System.Text;
using System.Collections;


namespace HijoPortal.classes
{
    public class MRPClass
    {
        public const string capex_logs = "CAPEX", opex_logs = "OPEX", directmaterials_logs = "DIRECTMATERIALS", manpower_logs = "MANPOWER", revenueassumption_logs = "REVENUEASSUMPTION", add_logs = "ADD", edit_logs = "EDIT", delete_logs = "DELETE", train_entity = "0101";

        public const string successfully_submitted = "Successfully Submitted";
        public const string successfully_approved = "Successfully Approved";

        static double
            capex_total_amount = 0,
            ca_edited_total = 0,
            ca_approved_total = 0,
            opex_total_amount = 0,
            op_edited_total = 0,
            op_approved_total = 0,
            materials_total_amount = 0,
            mat_edited_total = 0,
            mat_approved_total = 0,
            manpower_total_amount = 0,
            man_edited_total = 0,
            man_approved_total = 0,
            revenue_total_amount = 0,
            prev_summary = 0;


        public static string Insert_MRP(string sMonth, string sYear, int iCreator, string entCode, string buCode, int iCopyPreMOP, int iPreMonth, int iPreYear, ASPxPopupControl wrnPopup, ASPxLabel wrnText, ASPxPopupControl PopUpControl)
        {
            string iRes = "0||0";

            DateTime DATE_CREATED = DateTime.Now;
            SqlConnection cn = new SqlConnection(GlobalClass.SQLConnString());
            DataTable dt = new DataTable();
            SqlCommand cmd = null;
            SqlDataAdapter adp;

            DataTable dt1 = new DataTable();
            SqlCommand cmd1 = null;
            SqlDataAdapter adp1;

            string qry = "";

            SqlCommand cmdIn = null;
            SqlCommand cmdUp = null;

            string month = sMonth;
            string year = sYear;
            int monthIndex = Convert.ToDateTime("01-" + month + "-2011").Month;
            int yearInteger = Convert.ToInt32(year);

            string BU_CODE = buCode;
            string CREATOR_KEY = iCreator.ToString();
            string ENTITY_CODE = entCode;

            string monthStringTwoDigits = DateTime.Now.ToString("MM");
            string yearStringTwoDigits = DateTime.Now.ToString("yy");

            string DocumentPrefix = "", DocumentNum = "", STATUS_NAME = "";
            int STATUS_KEY = 1, DataFlowKey = 0, AppFlowKey = 0;
            int MRP_MONTH = monthIndex;
            int MRP_YEAR = yearInteger;

            cn.Open();

            qry = "SELECT * FROM [dbo].[tbl_MRP_List] WHERE ([MRPMonth] = " + MRP_MONTH + ") AND ([MRPYear] = " + MRP_YEAR + ") AND ([EntityCode] = '"+ ENTITY_CODE + "') AND ([BUCode] = '" + BU_CODE + "')";
            cmd = new SqlCommand(qry);
            cmd.Connection = cn;
            adp = new SqlDataAdapter(cmd);
            adp.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                wrnPopup.HeaderText = "Error";
                wrnText.Text = "Month and Year Already Exist";
                wrnPopup.ShowOnPageLoad = true;
                PopUpControl.ShowOnPageLoad = true;
                goto Ret_Res;
            }
            dt.Clear();


            qry = "SELECT [DocumentPrefix],[DocumentNum] FROM [dbo].[tbl_DocumentNumber] WHERE [DocumentPrefix] = 'MRP'";
            cmd = new SqlCommand(qry);
            cmd.Connection = cn;
            adp = new SqlDataAdapter(cmd);
            adp.Fill(dt);
            if (dt.Rows.Count>0)
            {
                foreach(DataRow row in dt.Rows)
                {
                    DocumentPrefix = row["DocumentPrefix"].ToString();
                    DocumentNum = row["DocumentNum"].ToString();
                }
            }
            dt.Clear();

            int doc_num = Int32.Parse(DocumentNum) + 1;

            
            string DOC_NUMBER = (doc_num).ToString("00000#");
            DOC_NUMBER = ENTITY_CODE + "-" + monthStringTwoDigits + yearStringTwoDigits + DocumentPrefix + "-" + DOC_NUMBER;

            qry= "SELECT TOP (1) [PK] FROM [dbo].[tbl_System_MOP_DataFlow] WHERE [EffectDate] <= '" + DATE_CREATED + "' ORDER BY EffectDate DESC";
            cmd = new SqlCommand(qry);
            cmd.Connection = cn;
            adp = new SqlDataAdapter(cmd);
            adp.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    DataFlowKey = Convert.ToInt32(row["PK"]);
                }
            } else
            {
                wrnPopup.HeaderText = "Error";
                wrnPopup.ShowOnPageLoad = true;
                wrnText.Text = "No Dataflow Setup !";
                PopUpControl.ShowOnPageLoad = true;
                goto Ret_Res;
            }
            dt.Clear();

            qry = "SELECT TOP (1) [PK] FROM [dbo].[tbl_System_Approval] WHERE [EffectDate] <= '" + DATE_CREATED + "' ORDER BY EffectDate DESC";
            cmd = new SqlCommand(qry);
            cmd.Connection = cn;
            adp = new SqlDataAdapter(cmd);
            adp.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    AppFlowKey = Convert.ToInt32(row["PK"]);
                }
            }
            else
            {
                wrnPopup.HeaderText = "Error";
                wrnPopup.ShowOnPageLoad = true;
                wrnText.Text = "No Approval Setup !";
                PopUpControl.ShowOnPageLoad = true;
                goto Ret_Res;
            }
            dt.Clear();

            //Insert MRP
            //qry = "INSERT INTO [dbo].[tbl_MRP_List] ([DocNumber], [DateCreated], [EntityCode], [BUCode], [MRPMonth], [MRPYear], [StatusKey], [CreatorKey], [LastModified]) VALUES (@DocNumber, @DateCreated, @EntityCode, @BUCode, @Month, @Year, @StatusKey, @CreatorKey, @LastModified)";

            //cmdIn = new SqlCommand(qry, cn);
            //cmdIn.Parameters.AddWithValue("@DocNumber", DOC_NUMBER);
            //cmdIn.Parameters.AddWithValue("@DateCreated", DATE_CREATED);
            //cmdIn.Parameters.AddWithValue("@EntityCode", ENTITY_CODE);
            //cmdIn.Parameters.AddWithValue("@BUCode", BU_CODE);
            //cmdIn.Parameters.AddWithValue("@Month", MRP_MONTH);
            //cmdIn.Parameters.AddWithValue("@Year", MRP_YEAR);
            //cmdIn.Parameters.AddWithValue("@StatusKey", STATUS_KEY);
            //cmdIn.Parameters.AddWithValue("@CreatorKey", CREATOR_KEY);
            //cmdIn.Parameters.AddWithValue("@LastModified", DATE_CREATED.ToString("MM/dd/yyyy hh:mm:ss tt"));
            //cmdIn.CommandType = CommandType.Text;
            //cmdIn.ExecuteNonQuery();

            QuerySPClass.InsertMRPList(DOC_NUMBER, DATE_CREATED, ENTITY_CODE, BU_CODE, MRP_MONTH, MRP_YEAR, STATUS_KEY, Convert.ToInt32(CREATOR_KEY), DATE_CREATED.ToString("MM/dd/yyyy hh:mm:ss tt"));

            //Get MRP Key
            int MRPKey = 0;
            qry = "SELECT [PK] FROM [dbo].[tbl_MRP_List] WHERE ([DocNumber] = '" + DOC_NUMBER + "')";
            cmd = new SqlCommand(qry);
            cmd.Connection = cn;
            adp = new SqlDataAdapter(cmd);
            adp.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    MRPKey = Convert.ToInt32(row["PK"]);
                }
            }
            dt.Clear();

            if (MRPKey != 0)
            {
                //Add Workflow
                qry = "SELECT dbo.tbl_System_MOP_DataFlow_Details.Line, dbo.tbl_System_MOP_DataFlow_Details.PositionNameKey, dbo.tbl_System_Approval_Position.SQLQuery FROM dbo.tbl_System_MOP_DataFlow_Details LEFT OUTER JOIN dbo.tbl_System_Approval_Position ON dbo.tbl_System_MOP_DataFlow_Details.PositionNameKey = dbo.tbl_System_Approval_Position.PK WHERE(dbo.tbl_System_MOP_DataFlow_Details.MasterKey = " + DataFlowKey + ") AND (dbo.tbl_System_Approval_Position.AfterApproved = 0) ORDER BY dbo.tbl_System_MOP_DataFlow_Details.Line";
                cmd = new SqlCommand(qry);
                cmd.Connection = cn;
                adp = new SqlDataAdapter(cmd);
                adp.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        int userKey = 0, status = 0, visible = 0;
                        if (row["SQLQuery"].ToString().Trim() != "")
                        {
                            qry = row["SQLQuery"].ToString() + " '" + ENTITY_CODE + "', '" + BU_CODE + "', '" + DATE_CREATED + "'";
                            cmd1 = new SqlCommand(qry);
                            cmd1.Connection = cn;
                            adp1 = new SqlDataAdapter(cmd1);
                            adp1.Fill(dt1);
                            if (dt1.Rows.Count > 0)
                            {
                                foreach (DataRow row1 in dt1.Rows)
                                {
                                    userKey = Convert.ToInt32(row1["UserKey"]);
                                }
                            }
                            dt1.Clear();
                        }

                        qry = "INSERT INTO tbl_MRP_List_Workflow ([MasterKey], [Line], [PositionNameKey], [UserKey], [Status], [Visible]) VALUES (@MasterKey, @Line, @PositionNameKey, @UserKey, @Status, @Visible)";
                        cmdIn = new SqlCommand(qry, cn);
                        cmdIn.Parameters.AddWithValue("@MasterKey", MRPKey);
                        cmdIn.Parameters.AddWithValue("@Line", Convert.ToInt32(row["Line"]));
                        cmdIn.Parameters.AddWithValue("@PositionNameKey", Convert.ToInt32(row["PositionNameKey"]));
                        cmdIn.Parameters.AddWithValue("@UserKey", userKey);
                        cmdIn.Parameters.AddWithValue("@Status", status);
                        cmdIn.Parameters.AddWithValue("@Visible", visible);
                        cmdIn.CommandType = CommandType.Text;
                        cmdIn.ExecuteNonQuery();
                    }
                }
                dt.Clear();

                //Add Approval
                qry = "SELECT TOP (100) PERCENT dbo.tbl_System_Approval_Details.Line, dbo.tbl_System_Approval_Details.PositionNameKey, dbo.tbl_System_Approval_Position.SQLQuery FROM dbo.tbl_System_Approval_Details LEFT OUTER JOIN dbo.tbl_System_Approval_Position ON dbo.tbl_System_Approval_Details.PositionNameKey = dbo.tbl_System_Approval_Position.PK LEFT OUTER JOIN dbo.tbl_System_Approval ON dbo.tbl_System_Approval_Details.MasterKey = dbo.tbl_System_Approval.PK WHERE(dbo.tbl_System_Approval_Details.MasterKey = " + AppFlowKey + ") AND (dbo.tbl_System_Approval_Position.AfterApproved = 0) ORDER BY dbo.tbl_System_Approval_Details.Line";
                cmd = new SqlCommand(qry);
                cmd.Connection = cn;
                adp = new SqlDataAdapter(cmd);
                adp.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        int userKey = 0, status = 0, visible = 0;
                        if (row["SQLQuery"].ToString().Trim() != "")
                        {
                            qry = row["SQLQuery"].ToString() + " '" + ENTITY_CODE + "', '" + BU_CODE + "', '" + DATE_CREATED + "'";
                            cmd1 = new SqlCommand(qry);
                            cmd1.Connection = cn;
                            adp1 = new SqlDataAdapter(cmd1);
                            adp1.Fill(dt1);
                            if (dt1.Rows.Count > 0)
                            {
                                foreach (DataRow row1 in dt1.Rows)
                                {
                                    userKey = Convert.ToInt32(row1["UserKey"]);
                                }
                            }
                            dt1.Clear();

                            qry = "INSERT INTO tbl_MRP_List_Approval ([MasterKey], [Line], [PositionNameKey], [UserKey], [Status], [Visible]) VALUES (@MasterKey, @Line, @PositionNameKey, @UserKey, @Status, @Visible)";
                            cmdIn = new SqlCommand(qry, cn);
                            cmdIn.Parameters.AddWithValue("@MasterKey", MRPKey);
                            cmdIn.Parameters.AddWithValue("@Line", Convert.ToInt32(row["Line"]));
                            cmdIn.Parameters.AddWithValue("@PositionNameKey", Convert.ToInt32(row["PositionNameKey"]));
                            cmdIn.Parameters.AddWithValue("@UserKey", userKey);
                            cmdIn.Parameters.AddWithValue("@Status", status);
                            cmdIn.Parameters.AddWithValue("@Visible", visible);
                            cmdIn.CommandType = CommandType.Text;
                            cmdIn.ExecuteNonQuery();
                        }
                    }
                }
                dt.Clear();

                string remarks = "MRP-ADD";
                MRPClass.AddLogsMOPList(cn, MRPKey, remarks);

                // Check if BU Lead = true then bypass creator 
                bool isBULead = false;
                isBULead = GlobalClass.IsAllowed(iCreator, "MOPBULead", DATE_CREATED, ENTITY_CODE, BU_CODE);
                if (isBULead)
                {
                    //Update Workflow
                    qry = "UPDATE tbl_MRP_List_Workflow SET Visible = 1, Status = 0 WHERE (MasterKey = " + MRPKey + ") AND (Line = 1)";
                    cmdUp = new SqlCommand(qry, cn);
                    cmdUp.ExecuteNonQuery();
                }
    
                //Update Document Number
                qry = "UPDATE [dbo].[tbl_DocumentNumber] SET [DocumentNum] = '" + doc_num + "' WHERE [DocumentPrefix] = 'MRP'";
                cmdUp = new SqlCommand(qry, cn);
                cmdUp.ExecuteNonQuery();

                //Copy previous MOP
                if (iCopyPreMOP == 1)
                {
                    string preMRPDocNum = "";
                    qry = "SELECT [DocNumber] FROM [dbo].[tbl_MRP_List] WHERE ([MRPMonth] = " + iPreMonth + ") AND ([MRPYear] = " + iPreYear + ") AND ([EntityCode] = '" + ENTITY_CODE + "') AND ([BUCode] = '" + BU_CODE + "')";
                    cmd = new SqlCommand(qry);
                    cmd.Connection = cn;
                    adp = new SqlDataAdapter(cmd);
                    adp.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            preMRPDocNum = row["DocNumber"].ToString();
                        }
                    }
                    dt.Clear();

                    if (preMRPDocNum != "")
                    {
                        // Copy Direct Materials
                        qry = "SELECT * FROM [dbo].[tbl_MRP_List_DirectMaterials] WHERE ([HeaderDocNum] = '" + preMRPDocNum + "')";
                        cmd = new SqlCommand(qry);
                        cmd.Connection = cn;
                        adp = new SqlDataAdapter(cmd);
                        adp.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow row in dt.Rows)
                            {
                                //qry = "INSERT INTO tbl_MRP_List_DirectMaterials (HeaderDocNum, TableIdentifier, ExpenseCode, ActivityCode, OprUnit, ItemCode, ItemDescription, ItemDescriptionAddl, UOM, Cost, Qty, TotalCost, EdittedQty, EdittedCost, EdittiedTotalCost) VALUES ('" + DOC_NUMBER + "', "+ Convert.ToInt32(row["TableIdentifier"]) + ", '" + row["ExpenseCode"].ToString() + "', '" + row["ActivityCode"].ToString() +"', '"+ row["OprUnit"].ToString() +"', '"+ row["ItemCode"].ToString() +"', '"+ GlobalClass.FormatSQL(row["ItemDescription"].ToString()) +"', '"+ GlobalClass.FormatSQL(row["ItemDescriptionAddl"].ToString()) +"', '"+ row["UOM"].ToString() +"', "+ Convert.ToDouble(row["Cost"]) +", "+ Convert.ToDouble(row["Qty"]) +", "+ Convert.ToDouble(row["TotalCost"]) + ", " + Convert.ToDouble(row["Qty"]) + ", " + Convert.ToDouble(row["Cost"]) + ", " + Convert.ToDouble(row["TotalCost"]) + ")";
                                //cmdIn = new SqlCommand(qry, cn);
                                //cmdIn.CommandType = CommandType.Text;
                                //cmdIn.ExecuteNonQuery();

                                int result = QuerySPClass.InsertUpdateDirectMaterials(0, 1, 0, DOC_NUMBER, Convert.ToInt32(row["TableIdentifier"]), row["ExpenseCode"].ToString(), row["ActivityCode"].ToString(), row["OprUnit"].ToString(), row["ItemCode"].ToString(), GlobalClass.FormatSQL(row["ItemDescription"].ToString()), GlobalClass.FormatSQL(row["ItemDescriptionAddl"].ToString()), row["UOM"].ToString(), Convert.ToDouble(row["Qty"]), Convert.ToDouble(row["Cost"]), Convert.ToDouble(row["TotalCost"]));

                                if (result > 0)
                                {
                                    MRPClass.UpdateLastModified(cn, DOC_NUMBER);
                                    string mrp_remarks = MRPClass.directmaterials_logs + "-" + MRPClass.add_logs;
                                    MRPClass.AddLogsMOPList(cn, MRPKey, mrp_remarks);
                                }
                            }
                        }
                        dt.Clear();

                        // Copy OPEX
                        qry = "SELECT * FROM [dbo].[tbl_MRP_List_OPEX] WHERE ([HeaderDocNum] = '" + preMRPDocNum + "')";
                        cmd = new SqlCommand(qry);
                        cmd.Connection = cn;
                        adp = new SqlDataAdapter(cmd);
                        adp.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow row in dt.Rows)
                            {
                                //qry = "INSERT INTO tbl_MRP_List_OPEX (HeaderDocNum, TableIdentifier, ExpenseCode, OprUnit, ProcCat, ItemCode, Description, DescriptionAddl, UOM, Cost, Qty, TotalCost, EdittedQty, EdittedCost, EdittedTotalCost) VALUES ('" + DOC_NUMBER + "', " + Convert.ToInt32(row["TableIdentifier"]) + ", '" + row["ExpenseCode"].ToString() + "', '" + row["OprUnit"].ToString() + "', '"+ row["ProcCat"].ToString() +"', '" + row["ItemCode"].ToString() + "', '" + GlobalClass.FormatSQL(row["Description"].ToString()) + "', '" + GlobalClass.FormatSQL(row["DescriptionAddl"].ToString()) + "', '" + row["UOM"].ToString() + "', " + Convert.ToDouble(row["Cost"]) + ", " + Convert.ToDouble(row["Qty"]) + ", " + Convert.ToDouble(row["TotalCost"]) + ", " + Convert.ToDouble(row["Qty"]) + ", " + Convert.ToDouble(row["Cost"]) + ", " + Convert.ToDouble(row["TotalCost"]) + ")";
                                //cmdIn = new SqlCommand(qry, cn);
                                //cmdIn.CommandType = CommandType.Text;
                                //cmdIn.ExecuteNonQuery();
                                int result = QuerySPClass.InsertUpdateOperatingExpense(0, 1, 0, DOC_NUMBER, Convert.ToInt32(row["TableIdentifier"]), row["ExpenseCode"].ToString(), row["OprUnit"].ToString(), row["ProcCat"].ToString(), row["ItemCode"].ToString(), GlobalClass.FormatSQL(row["Description"].ToString()), GlobalClass.FormatSQL(row["DescriptionAddl"].ToString()), row["UOM"].ToString(), Convert.ToDouble(row["Qty"]), Convert.ToDouble(row["Cost"]), Convert.ToDouble(row["TotalCost"]));
                                if (result > 0)
                                {
                                    MRPClass.UpdateLastModified(cn, DOC_NUMBER);
                                    string mrp_remarks = MRPClass.opex_logs + "-" + MRPClass.add_logs;
                                    MRPClass.AddLogsMOPList(cn, MRPKey, mrp_remarks);
                                }
                            }
                        }
                        dt.Clear();

                        // Copy Manpower
                        qry = "SELECT * FROM [dbo].[tbl_MRP_List_ManPower] WHERE ([HeaderDocNum] = '" + preMRPDocNum + "')";
                        cmd = new SqlCommand(qry);
                        cmd.Connection = cn;
                        adp = new SqlDataAdapter(cmd);
                        adp.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow row in dt.Rows)
                            {
                                //qry = "INSERT INTO tbl_MRP_List_ManPower (HeaderDocNum, TableIdentifier, ActivityCode, ManPowerTypeKey, OprUnit, Description, UOM, Cost, Qty, TotalCost, EdittedQty, EdittedCost, EdittiedTotalCost) VALUES ('" + DOC_NUMBER + "', " + Convert.ToInt32(row["TableIdentifier"]) + ", '" + row["ActivityCode"].ToString() + "', " + Convert.ToInt32(row["ManPowerTypeKey"]) + ", '" + row["OprUnit"].ToString() + "', '" + GlobalClass.FormatSQL(row["Description"].ToString()) + "', '" + row["UOM"].ToString() + "', " + Convert.ToDouble(row["Cost"]) + ", " + Convert.ToDouble(row["Qty"]) + ", " + Convert.ToDouble(row["TotalCost"]) + ", " + Convert.ToDouble(row["Qty"]) + ", " + Convert.ToDouble(row["Cost"]) + ", " + Convert.ToDouble(row["TotalCost"]) + ")";
                                //cmdIn = new SqlCommand(qry, cn);
                                //cmdIn.CommandType = CommandType.Text;
                                //cmdIn.ExecuteNonQuery();
                                int result = QuerySPClass.InsertUpdateManPower(0, 1, 0, DOC_NUMBER, Convert.ToInt32(row["TableIdentifier"]), row["ActivityCode"].ToString(), row["OprUnit"].ToString(), Convert.ToInt32(row["ManPowerTypeKey"]), GlobalClass.FormatSQL(row["Description"].ToString()), row["UOM"].ToString(), Convert.ToDouble(row["Qty"]), Convert.ToDouble(row["Cost"]), Convert.ToDouble(row["TotalCost"]));
                                if (result > 0)
                                {
                                    MRPClass.UpdateLastModified(cn, DOC_NUMBER);
                                    string mrp_remarks = MRPClass.manpower_logs + "-" + MRPClass.add_logs;
                                    MRPClass.AddLogsMOPList(cn, MRPKey, mrp_remarks);
                                }
                            }
                        }
                        dt.Clear();

                        //Copy CAPEX
                        qry = "SELECT * FROM [dbo].[tbl_MRP_List_CAPEX] WHERE ([HeaderDocNum] = '" + preMRPDocNum + "')";
                        cmd = new SqlCommand(qry);
                        cmd.Connection = cn;
                        adp = new SqlDataAdapter(cmd);
                        adp.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow row in dt.Rows)
                            {
                                //qry = "INSERT INTO tbl_MRP_List_CAPEX (HeaderDocNum, TableIdentifier, OprUnit, ProdCat, Description, UOM, Cost, Qty, TotalCost, EdittedQty, EdittedCost, EdittiedTotalCost) VALUES ('" + DOC_NUMBER + "', " + Convert.ToInt32(row["TableIdentifier"]) + ", '" + row["OprUnit"].ToString() + "', '"+ row["ProdCat"].ToString() +"', '" + GlobalClass.FormatSQL(row["Description"].ToString()) + "', '" + row["UOM"].ToString() + "', " + Convert.ToDouble(row["Cost"]) + ", " + Convert.ToDouble(row["Qty"]) + ", " + Convert.ToDouble(row["TotalCost"]) + ", " + Convert.ToDouble(row["Qty"]) + ", " + Convert.ToDouble(row["Cost"]) + ", " + Convert.ToDouble(row["TotalCost"]) + ")";
                                //cmdIn = new SqlCommand(qry, cn);
                                //cmdIn.CommandType = CommandType.Text;
                                //cmdIn.ExecuteNonQuery();
                                int result = QuerySPClass.InsertUpdateCapitalExpenditures(0, 1, 0, DOC_NUMBER, Convert.ToInt32(row["TableIdentifier"]), row["OprUnit"].ToString(), row["ProdCat"].ToString(), GlobalClass.FormatSQL(row["Description"].ToString()), row["UOM"].ToString(), Convert.ToDouble(row["Qty"]), Convert.ToDouble(row["Cost"]), Convert.ToDouble(row["TotalCost"]));
                                if (result > 0)
                                {
                                    MRPClass.UpdateLastModified(cn, DOC_NUMBER);
                                    string mrp_remarks = MRPClass.capex_logs + "-" + MRPClass.add_logs;
                                    MRPClass.AddLogsMOPList(cn, MRPKey, mrp_remarks);
                                }
                            }
                        }
                        dt.Clear();

                        //Copy Revenue Assumption
                        qry = "SELECT * FROM [dbo].[tbl_MRP_List_RevenueAssumptions] WHERE ([HeaderDocNum] = '" + preMRPDocNum + "')";
                        cmd = new SqlCommand(qry);
                        cmd.Connection = cn;
                        adp = new SqlDataAdapter(cmd);
                        adp.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow row in dt.Rows)
                            {
                                //qry = "INSERT INTO tbl_MRP_List_RevenueAssumptions (HeaderDocNum, OprUnit, ProductName, FarmName, Prize, Volume, TotalPrize) VALUES ('" + DOC_NUMBER + "', '" + row["OprUnit"].ToString() + "', '" + GlobalClass.FormatSQL(row["ProductName"].ToString()) + "', '" + GlobalClass.FormatSQL(row["FarmName"].ToString()) + "', " + Convert.ToDouble(row["Prize"]) + ", " + Convert.ToDouble(row["Volume"]) + ", " + Convert.ToDouble(row["TotalPrize"]) + ")";
                                //cmdIn = new SqlCommand(qry, cn);
                                //cmdIn.CommandType = CommandType.Text;
                                //cmdIn.ExecuteNonQuery();
                                int result = QuerySPClass.InsertUpdateRevenueAssumptions(1, 0, DOC_NUMBER, row["OprUnit"].ToString(), GlobalClass.FormatSQL(row["ProductName"].ToString()), GlobalClass.FormatSQL(row["FarmName"].ToString()), Convert.ToDouble(row["Prize"]), Convert.ToDouble(row["Volume"]), Convert.ToDouble(row["TotalPrize"]), row["UOM"].ToString());
                                if (result > 0)
                                {
                                    MRPClass.UpdateLastModified(cn, DOC_NUMBER);
                                    string mrp_remarks = MRPClass.revenueassumption_logs + "-" + MRPClass.add_logs;
                                    MRPClass.AddLogsMOPList(cn, MRPKey, mrp_remarks);
                                }
                            }
                        }
                        dt.Clear();
                    }
                }
 
                iRes = "1|" + DOC_NUMBER + "|" + MRPKey.ToString();
            }
            
            cn.Close();

            Ret_Res:
            return iRes;
        }

        public static string Month_Name(int iMonth)
        {
            DateTime now = DateTime.Now;
            string sMonth = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(iMonth);
            return sMonth;
        }        

        public static DataTable Master_MRP_List(int ViewAll, string EntityCode, string BUCode)
        {
            DataTable dtTable = new DataTable();
            SqlConnection cn = new SqlConnection(GlobalClass.SQLConnString());
            DataTable dt = new DataTable();
            SqlCommand cmd = null;
            SqlDataAdapter adp;

            DataTable dt1 = new DataTable();
            SqlCommand cmd1 = null;
            SqlDataAdapter adp1;

            string qry = "";

            cn.Open();
            if (dtTable.Columns.Count == 0)
            {
                //Columns for AspxGridview
                dtTable.Columns.Add("PK", typeof(string));
                dtTable.Columns.Add("DocNumber", typeof(string));
                dtTable.Columns.Add("DateCreated", typeof(string));
                dtTable.Columns.Add("EntityCode", typeof(string));
                dtTable.Columns.Add("EntityCodeDesc", typeof(string));
                dtTable.Columns.Add("BUCode", typeof(string));
                dtTable.Columns.Add("BUCodeDesc", typeof(string));
                dtTable.Columns.Add("MRPMonth", typeof(Int32));
                dtTable.Columns.Add("MRPMonthDesc", typeof(string));
                dtTable.Columns.Add("MRPYear", typeof(Int32));
                dtTable.Columns.Add("Amount", typeof(string));
                dtTable.Columns.Add("Creator", typeof(string));
                dtTable.Columns.Add("StatusKey", typeof(string));
                dtTable.Columns.Add("StatusKeyDesc", typeof(string));
                dtTable.Columns.Add("WorkflowStatusLine", typeof(string));
                dtTable.Columns.Add("WorkflowStatus", typeof(string));
                dtTable.Columns.Add("CreatorKey", typeof(Int32));
                dtTable.Columns.Add("LastModified", typeof(string));
            }

            //string query = "SELECT tbl_MRP_List.*, tbl_MRP_Status.StatusName FROM tbl_MRP_List LEFT OUTER JOIN tbl_MRP_Status ON tbl_MRP_List.StatusKey = tbl_MRP_Status.PK";
            //string query = "SELECT TOP (100) PERCENT dbo.tbl_MRP_List.PK, dbo.tbl_MRP_List.DocNumber, " +
            //               " dbo.tbl_MRP_List.DateCreated, dbo.tbl_MRP_List.EntityCode, dbo.vw_AXEntityTable.NAME AS EntityCodeDesc, " +
            //               " dbo.tbl_MRP_List.BUCode, dbo.vw_AXOperatingUnitTable.NAME AS BUCodeDesc, dbo.tbl_MRP_List.MRPMonth, " +
            //               " dbo.tbl_MRP_List.MRPYear, dbo.tbl_MRP_List.StatusKey, dbo.tbl_MRP_Status.StatusName, " +
            //               " dbo.tbl_MRP_List.CreatorKey, dbo.tbl_MRP_List.LastModified " +
            //               " FROM  dbo.tbl_MRP_List LEFT OUTER JOIN " +
            //               " dbo.vw_AXOperatingUnitTable ON dbo.tbl_MRP_List.BUCode = dbo.vw_AXOperatingUnitTable.OMOPERATINGUNITNUMBER LEFT OUTER JOIN " +
            //               " dbo.tbl_MRP_Status ON dbo.tbl_MRP_List.StatusKey = dbo.tbl_MRP_Status.PK LEFT OUTER JOIN " +
            //               " dbo.vw_AXEntityTable ON dbo.tbl_MRP_List.EntityCode = dbo.vw_AXEntityTable.ID " +
            //               " ORDER BY dbo.tbl_MRP_List.DocNumber DESC";
            string query = "sp_MRPList " + ViewAll + ", '" + EntityCode + "', '" + BUCode + "'";
            cmd = new SqlCommand(query);
            cmd.Connection = cn;
            adp = new SqlDataAdapter(cmd);
            adp.Fill(dt);
            //double dummy = 12.2;
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    DataRow dtRow = dtTable.NewRow();
                    dtRow["PK"] = row["PK"].ToString();
                    dtRow["DocNumber"] = row["DocNumber"].ToString();
                    dtRow["DateCreated"] = Convert.ToDateTime(row["DateCreated"]).ToString("MM/dd/yyyy");
                    dtRow["EntityCode"] = row["EntityCode"].ToString();
                    dtRow["EntityCodeDesc"] = row["EntityCodeDesc"].ToString();
                    dtRow["BUCode"] = row["BUCode"].ToString();
                    dtRow["BUCodeDesc"] = row["BUCodeDesc"].ToString();
                    dtRow["MRPMonth"] = Convert.ToInt32(row["MRPMonth"]);
                    dtRow["MRPMonthDesc"] = Month_Name(Convert.ToInt32(row["MRPMonth"]));
                    dtRow["MRPYear"] = Convert.ToInt32(row["MRPYear"]);
                    
                    dtRow["Creator"] = EncryptionClass.Decrypt(row["FirstName"].ToString()) + " " + EncryptionClass.Decrypt(row["LastName"].ToString());
                    string docnum = row["DocNumber"].ToString();
                    double amount = Convert.ToDouble(row["TotalCost"]);

                    //string query_1 = "SELECT SUM(TotalCost) AS Total FROM " + DirectMatTable() + " WHERE(HeaderDocNum = '" + docnum + "')GROUP BY HeaderDocNum";
                    //SqlCommand com = new SqlCommand(query_1, cn);
                    //SqlDataReader reader = com.ExecuteReader();
                    //while (reader.Read())
                    //    amount += Convert.ToDouble(reader[0].ToString());
                    //reader.Close();

                    //string query_2 = "SELECT SUM(TotalCost) AS Total FROM " + OpexTable() + " WHERE(HeaderDocNum = '" + docnum + "')GROUP BY HeaderDocNum";
                    //com = new SqlCommand(query_2, cn);
                    //reader = com.ExecuteReader();
                    //while (reader.Read())
                    //    amount += Convert.ToDouble(reader[0].ToString());
                    //reader.Close();

                    //string query_3 = "SELECT SUM(TotalCost) AS Total FROM " + ManPowerTable() + " WHERE(HeaderDocNum = '" + docnum + "')GROUP BY HeaderDocNum";
                    //com = new SqlCommand(query_3, cn);
                    //reader = com.ExecuteReader();
                    //while (reader.Read())
                    //    amount += Convert.ToDouble(reader[0].ToString());
                    //reader.Close();

                    //string query_4 = "SELECT SUM(TotalCost) AS Total FROM " + CapexTable() + " WHERE(HeaderDocNum = '" + docnum + "')GROUP BY HeaderDocNum";
                    //com = new SqlCommand(query_4, cn);
                    //reader = com.ExecuteReader();
                    //while (reader.Read())
                    //    amount += Convert.ToDouble(reader[0].ToString());
                    //reader.Close();

                    dtRow["Amount"] = String.Format("{0:n}", amount);
                    dtRow["StatusKey"] = row["StatusKey"].ToString();
                    dtRow["StatusKeyDesc"] = row["StatusName"].ToString();
                    dtRow["CreatorKey"] = Convert.ToInt32(row["CreatorKey"]);
                    dtRow["LastModified"] = row["LastModified"].ToString();

                    if (Convert.ToInt32(row["StatusKey"]) == 1)
                    {
                        dtRow["WorkflowStatusLine"] = "0";
                        dtRow["WorkflowStatus"] = "At Source";
                    }
                    else if (Convert.ToInt32(row["StatusKey"]) == 2)
                    {
                        qry = "SELECT dbo.tbl_MRP_List_Workflow.Line, dbo.tbl_System_Approval_Position.PositionName " +
                              " FROM dbo.tbl_MRP_List_Workflow LEFT OUTER JOIN " +
                              " dbo.tbl_System_Approval_Position ON dbo.tbl_MRP_List_Workflow.PositionNameKey = dbo.tbl_System_Approval_Position.PK " +
                              " WHERE(dbo.tbl_MRP_List_Workflow.MasterKey = " + row["PK"] + ") " +
                              " AND(dbo.tbl_MRP_List_Workflow.Status = 0) " +
                              " AND(dbo.tbl_MRP_List_Workflow.Visible = 1)";
                        cmd1 = new SqlCommand(qry);
                        cmd1.Connection = cn;
                        adp1 = new SqlDataAdapter(cmd1);
                        adp1.Fill(dt1);
                        if (dt1.Rows.Count > 0)
                        {
                            foreach (DataRow row1 in dt1.Rows)
                            {
                                dtRow["WorkflowStatusLine"] = row1["Line"].ToString();
                                dtRow["WorkflowStatus"] = row1["PositionName"].ToString();
                            }
                        }
                        dt1.Clear();
                    }
                    else if (Convert.ToInt32(row["StatusKey"]) == 3)
                    {
                        qry = "SELECT TOP (1) dbo.tbl_MRP_List_Approval.Line, dbo.tbl_System_Approval_Position.PositionName FROM dbo.tbl_MRP_List_Approval LEFT OUTER JOIN dbo.tbl_System_Approval_Position ON dbo.tbl_MRP_List_Approval.PositionNameKey = dbo.tbl_System_Approval_Position.PK WHERE(dbo.tbl_MRP_List_Approval.MasterKey = " + row["PK"] + ") AND(dbo.tbl_MRP_List_Approval.Status = 0) AND(dbo.tbl_MRP_List_Approval.Visible = 1) ORDER BY dbo.tbl_MRP_List_Approval.Line ";
                        cmd1 = new SqlCommand(qry);
                        cmd1.Connection = cn;
                        adp1 = new SqlDataAdapter(cmd1);
                        adp1.Fill(dt1);
                        if (dt1.Rows.Count > 0)
                        {
                            foreach (DataRow row1 in dt1.Rows)
                            {
                                dtRow["WorkflowStatusLine"] = row1["Line"].ToString();
                                dtRow["WorkflowStatus"] = row1["PositionName"].ToString();
                            }
                        }
                        dt1.Clear();
                    }
                    else
                    {
                        dtRow["WorkflowStatusLine"] = "5";
                        dtRow["WorkflowStatus"] = "Procurement Officer";
                    }
                    dtTable.Rows.Add(dtRow);
                }
            }
            dt.Clear();
            cn.Close();
            return dtTable;
        }

        public static DataTable Master_MRP_List_DOCNUM()
        {
            DataTable dtTable = new DataTable();
            SqlConnection cn = new SqlConnection(GlobalClass.SQLConnString());
            DataTable dt = new DataTable();
            SqlCommand cmd = null;
            SqlDataAdapter adp;

            cn.Open();
            if (dtTable.Columns.Count == 0)
            {
                //Columns for AspxGridview
                dtTable.Columns.Add("DocNumber", typeof(string));
                dtTable.Columns.Add("EntityCode", typeof(string));
                dtTable.Columns.Add("BUCode", typeof(string));
                dtTable.Columns.Add("MRPMonth", typeof(string));
            }

            string query = "SELECT tbl_MRP_List.DocNumber, tbl_MRP_List.MRPMonth, tbl_MRP_List.MRPYear, vw_AXOperatingUnitTable.NAME, vw_AXEntityTable.NAME AS BU FROM tbl_MRP_List LEFT OUTER JOIN vw_AXOperatingUnitTable ON tbl_MRP_List.BUCode = vw_AXOperatingUnitTable.OMOPERATINGUNITNUMBER LEFT OUTER JOIN vw_AXEntityTable ON tbl_MRP_List.EntityCode = vw_AXEntityTable.ID WHERE (tbl_MRP_List.StatusKey = 4) ORDER BY DocNumber DESC";

            cmd = new SqlCommand(query);
            cmd.Connection = cn;
            adp = new SqlDataAdapter(cmd);
            adp.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    DataRow dtRow = dtTable.NewRow();
                    dtRow["DocNumber"] = row["DocNumber"].ToString();
                    dtRow["EntityCode"] = row["NAME"].ToString();
                    dtRow["BUCode"] = row["BU"].ToString();
                    dtRow["MRPMonth"] = DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(Convert.ToInt32(row["MRPMonth"])) + "-" + row["MRPYear"].ToString();
                    dtTable.Rows.Add(dtRow);
                }
            }
            dt.Clear();
            cn.Close();
            return dtTable;
        }

        public static DataTable MRP_Direct_Materials(string DOC_NUMBER, string entity)
        {
            DataTable dtTable = new DataTable();
            SqlConnection cn = new SqlConnection(GlobalClass.SQLConnString());
            DataTable dt = new DataTable();
            SqlCommand cmd = null;
            SqlDataAdapter adp;
            materials_total_amount = 0;

            cn.Open();
            if (dtTable.Columns.Count == 0)
            {
                //Columns for AspxGridview
                dtTable.Columns.Add("PK", typeof(string));
                dtTable.Columns.Add("HeaderDocNum", typeof(string));
                dtTable.Columns.Add("ActivityCode", typeof(string));
                dtTable.Columns.Add("ItemCode", typeof(string));
                dtTable.Columns.Add("ItemDescription", typeof(string));
                dtTable.Columns.Add("ItemDescriptionAddl", typeof(string));
                dtTable.Columns.Add("UOM", typeof(string));
                dtTable.Columns.Add("Cost", typeof(string));
                dtTable.Columns.Add("Qty", typeof(string));
                dtTable.Columns.Add("TotalCost", typeof(string));
                dtTable.Columns.Add("VALUE", typeof(string));
                dtTable.Columns.Add("RevDesc", typeof(string));
                dtTable.Columns.Add("ExpenseCode", typeof(string));
                dtTable.Columns.Add("ExpenseCodeName", typeof(string));
                //dtTable.Columns.Add("WrkLine", typeof(string));
                //dtTable.Columns.Add("StatusKey", typeof(string));
            }

            //string query_1 = "SELECT DISTINCT tbl_MRP_List_DirectMaterials.*, vw_AXFindimActivity.DESCRIPTION, vw_AXFindimBananaRevenue.VALUE, vw_AXFindimBananaRevenue.DESCRIPTION AS RevDesc, tbl_MRP_List.EntityCode FROM   tbl_MRP_List_DirectMaterials LEFT OUTER JOIN vw_AXFindimActivity ON tbl_MRP_List_DirectMaterials.ActivityCode = vw_AXFindimActivity.VALUE LEFT OUTER JOIN tbl_MRP_List ON tbl_MRP_List_DirectMaterials.HeaderDocNum = tbl_MRP_List.DocNumber LEFT OUTER JOIN vw_AXFindimBananaRevenue ON tbl_MRP_List.EntityCode = vw_AXFindimBananaRevenue.Entity AND tbl_MRP_List_DirectMaterials.OprUnit = vw_AXFindimBananaRevenue.VALUE WHERE tbl_MRP_List_DirectMaterials.HeaderDocNum = '" + DOC_NUMBER + "'";

            string query_1 = "SELECT DISTINCT dbo.tbl_MRP_List_DirectMaterials.PK, dbo.tbl_MRP_List_DirectMaterials.HeaderDocNum, dbo.tbl_MRP_List_DirectMaterials.TableIdentifier, dbo.tbl_MRP_List_DirectMaterials.ExpenseCode, dbo.tbl_MRP_List_DirectMaterials.ActivityCode, dbo.tbl_MRP_List_DirectMaterials.OprUnit, dbo.tbl_MRP_List_DirectMaterials.ItemCode, dbo.tbl_MRP_List_DirectMaterials.ItemDescription, dbo.tbl_MRP_List_DirectMaterials.ItemDescriptionAddl, dbo.tbl_MRP_List_DirectMaterials.UOM, dbo.tbl_MRP_List_DirectMaterials.Cost, dbo.tbl_MRP_List_DirectMaterials.Qty, dbo.tbl_MRP_List_DirectMaterials.TotalCost, dbo.tbl_MRP_List_DirectMaterials.EdittedQty, dbo.tbl_MRP_List_DirectMaterials.EdittedCost, dbo.tbl_MRP_List_DirectMaterials.EdittiedTotalCost, dbo.tbl_MRP_List_DirectMaterials.ApprovedQty, dbo.tbl_MRP_List_DirectMaterials.ApprovedCost, dbo.tbl_MRP_List_DirectMaterials.ApprovedTotalCost, dbo.tbl_MRP_List_DirectMaterials.QtyPO, dbo.tbl_MRP_List_DirectMaterials.AvailForPO, dbo.vw_AXFindimActivity.DESCRIPTION, dbo.vw_AXFindimBananaRevenue.VALUE, dbo.vw_AXFindimBananaRevenue.DESCRIPTION AS RevDesc, dbo.tbl_MRP_List.EntityCode, dbo.vw_AXExpenseAccount.NAME AS ExpenseCodeName FROM   dbo.tbl_MRP_List_DirectMaterials LEFT OUTER JOIN dbo.tbl_MRP_List ON dbo.tbl_MRP_List_DirectMaterials.HeaderDocNum = dbo.tbl_MRP_List.DocNumber LEFT OUTER JOIN dbo.vw_AXFindimActivity ON dbo.tbl_MRP_List_DirectMaterials.ActivityCode = dbo.vw_AXFindimActivity.VALUE LEFT OUTER JOIN dbo.vw_AXFindimBananaRevenue ON dbo.tbl_MRP_List.EntityCode = dbo.vw_AXFindimBananaRevenue.Entity AND dbo.tbl_MRP_List_DirectMaterials.OprUnit = dbo.vw_AXFindimBananaRevenue.VALUE LEFT OUTER JOIN dbo.vw_AXExpenseAccount ON dbo.tbl_MRP_List_DirectMaterials.ExpenseCode = dbo.vw_AXExpenseAccount.MAINACCOUNTID WHERE tbl_MRP_List_DirectMaterials.HeaderDocNum = '" + DOC_NUMBER + "'";

            //string query_2 = "SELECT DISTINCT tbl_MRP_List_DirectMaterials.*, vw_AXFindimActivity.DESCRIPTION FROM   tbl_MRP_List_DirectMaterials LEFT OUTER JOIN vw_AXFindimActivity ON tbl_MRP_List_DirectMaterials.ActivityCode = vw_AXFindimActivity.VALUE WHERE tbl_MRP_List_DirectMaterials.HeaderDocNum = '" + DOC_NUMBER + "'";

            //if (entity == train_entity) cmd = new SqlCommand(query_1);
            //else cmd = new SqlCommand(query_2);

            cmd = new SqlCommand(query_1);

            cmd.Connection = cn;
            adp = new SqlDataAdapter(cmd);
            adp.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    DataRow dtRow = dtTable.NewRow();
                    dtRow["PK"] = row["PK"].ToString();
                    dtRow["HeaderDocNum"] = row["HeaderDocNum"].ToString();
                    dtRow["ActivityCode"] = row["DESCRIPTION"].ToString();
                    dtRow["ExpenseCode"] = row["ExpenseCode"].ToString();
                    dtRow["ExpenseCodeName"] = row["ExpenseCodeName"].ToString();
                    dtRow["ItemCode"] = row["ItemCode"].ToString();
                    dtRow["ItemDescription"] = row["ItemDescription"].ToString();
                    dtRow["ItemDescriptionAddl"] = row["ItemDescriptionAddl"].ToString();
                    dtRow["UOM"] = row["UOM"].ToString();
                    dtRow["Cost"] = Convert.ToDouble(row["Cost"]).ToString("N");
                    dtRow["Qty"] = Convert.ToDouble(row["Qty"]).ToString("N");
                    dtRow["TotalCost"] = Convert.ToDouble(row["TotalCost"]).ToString("N");

                    if (entity == "0101")
                    {
                        dtRow["VALUE"] = row["VALUE"].ToString();
                        dtRow["RevDesc"] = row["RevDesc"].ToString();
                    }
                    else
                    {
                        dtRow["VALUE"] = "";
                        dtRow["RevDesc"] = "";
                    }
                    //dtRow["WrkLine"] = WrkLine.ToString();
                    //dtRow["StatusKey"] = StatusKey.ToString();

                    dtTable.Rows.Add(dtRow);

                    //PrintString(row["Cost"].ToString());
                    materials_total_amount += Convert.ToDouble(row["TotalCost"]);
                }
            }
            dt.Clear();
            cn.Close();
            return dtTable;
        }

        public static DataTable MRPInvent_Direct_Materials(string DOC_NUMBER, string entitycode)
        {
            DataTable dtTable = new DataTable();
            SqlConnection cn = new SqlConnection(GlobalClass.SQLConnString());
            DataTable dt = new DataTable();
            SqlCommand cmd = null;
            SqlDataAdapter adp;
            materials_total_amount = 0;

            cn.Open();
            if (dtTable.Columns.Count == 0)
            {
                //Columns for AspxGridview
                dtTable.Columns.Add("PK", typeof(string));
                dtTable.Columns.Add("HeaderDocNum", typeof(string));
                dtTable.Columns.Add("ActivityCode", typeof(string));
                dtTable.Columns.Add("ItemCode", typeof(string));
                dtTable.Columns.Add("ItemDescription", typeof(string));
                dtTable.Columns.Add("UOM", typeof(string));
                dtTable.Columns.Add("Cost", typeof(string));
                dtTable.Columns.Add("Qty", typeof(Double));
                dtTable.Columns.Add("TotalCost", typeof(string));
                dtTable.Columns.Add("EdittedCost", typeof(string));
                dtTable.Columns.Add("EdittedQty", typeof(Double));
                dtTable.Columns.Add("EdittiedTotalCost", typeof(string));
                dtTable.Columns.Add("RevDesc", typeof(string));
            }

            //string query_1 = "SELECT DISTINCT tbl_MRP_List_DirectMaterials.*, vw_AXFindimActivity.DESCRIPTION, vw_AXFindimBananaRevenue.DESCRIPTION AS RevDesc FROM tbl_MRP_List_DirectMaterials LEFT OUTER JOIN vw_AXFindimActivity ON tbl_MRP_List_DirectMaterials.ActivityCode = vw_AXFindimActivity.VALUE LEFT OUTER JOIN vw_AXFindimBananaRevenue ON tbl_MRP_List_DirectMaterials.OprUnit = vw_AXFindimBananaRevenue.VALUE WHERE tbl_MRP_List_DirectMaterials.HeaderDocNum = '" + DOC_NUMBER + "'";

            //string query_2 = "SELECT DISTINCT tbl_MRP_List_DirectMaterials.*, vw_AXFindimActivity.DESCRIPTION FROM   tbl_MRP_List_DirectMaterials LEFT OUTER JOIN vw_AXFindimActivity ON tbl_MRP_List_DirectMaterials.ActivityCode = vw_AXFindimActivity.VALUE WHERE tbl_MRP_List_DirectMaterials.HeaderDocNum = '" + DOC_NUMBER + "'";

            string query_1 = "SELECT DISTINCT dbo.tbl_MRP_List_DirectMaterials.PK, dbo.tbl_MRP_List_DirectMaterials.HeaderDocNum, dbo.tbl_MRP_List_DirectMaterials.TableIdentifier, dbo.tbl_MRP_List_DirectMaterials.ExpenseCode, dbo.tbl_MRP_List_DirectMaterials.ActivityCode, dbo.tbl_MRP_List_DirectMaterials.OprUnit, dbo.tbl_MRP_List_DirectMaterials.ItemCode, dbo.tbl_MRP_List_DirectMaterials.ItemDescription, dbo.tbl_MRP_List_DirectMaterials.ItemDescriptionAddl, dbo.tbl_MRP_List_DirectMaterials.UOM, dbo.tbl_MRP_List_DirectMaterials.Cost, dbo.tbl_MRP_List_DirectMaterials.Qty, dbo.tbl_MRP_List_DirectMaterials.TotalCost, dbo.tbl_MRP_List_DirectMaterials.EdittedQty, dbo.tbl_MRP_List_DirectMaterials.EdittedCost, dbo.tbl_MRP_List_DirectMaterials.EdittiedTotalCost, dbo.tbl_MRP_List_DirectMaterials.ApprovedQty, dbo.tbl_MRP_List_DirectMaterials.ApprovedCost, dbo.tbl_MRP_List_DirectMaterials.ApprovedTotalCost, dbo.tbl_MRP_List_DirectMaterials.QtyPO, dbo.tbl_MRP_List_DirectMaterials.AvailForPO, dbo.vw_AXFindimActivity.DESCRIPTION, dbo.vw_AXFindimBananaRevenue.DESCRIPTION AS RevDesc FROM  dbo.tbl_MRP_List_DirectMaterials LEFT OUTER JOIN dbo.vw_AXFindimBananaRevenue ON dbo.tbl_MRP_List_DirectMaterials.OprUnit = dbo.vw_AXFindimBananaRevenue.VALUE LEFT OUTER JOIN dbo.vw_AXFindimActivity ON dbo.tbl_MRP_List_DirectMaterials.ActivityCode = dbo.vw_AXFindimActivity.VALUE WHERE(dbo.tbl_MRP_List_DirectMaterials.HeaderDocNum = '" + DOC_NUMBER + "')";

            string query_2 = "SELECT DISTINCT dbo.tbl_MRP_List_DirectMaterials.PK, dbo.tbl_MRP_List_DirectMaterials.HeaderDocNum, dbo.tbl_MRP_List_DirectMaterials.TableIdentifier, dbo.tbl_MRP_List_DirectMaterials.ExpenseCode, dbo.tbl_MRP_List_DirectMaterials.ActivityCode, dbo.tbl_MRP_List_DirectMaterials.OprUnit, dbo.tbl_MRP_List_DirectMaterials.ItemCode, dbo.tbl_MRP_List_DirectMaterials.ItemDescription, dbo.tbl_MRP_List_DirectMaterials.ItemDescriptionAddl, dbo.tbl_MRP_List_DirectMaterials.UOM, dbo.tbl_MRP_List_DirectMaterials.Cost, dbo.tbl_MRP_List_DirectMaterials.Qty, dbo.tbl_MRP_List_DirectMaterials.TotalCost, dbo.tbl_MRP_List_DirectMaterials.EdittedQty, dbo.tbl_MRP_List_DirectMaterials.EdittedCost, dbo.tbl_MRP_List_DirectMaterials.EdittiedTotalCost, dbo.tbl_MRP_List_DirectMaterials.ApprovedQty, dbo.tbl_MRP_List_DirectMaterials.ApprovedCost, dbo.tbl_MRP_List_DirectMaterials.ApprovedTotalCost, dbo.tbl_MRP_List_DirectMaterials.QtyPO, dbo.tbl_MRP_List_DirectMaterials.AvailForPO, dbo.vw_AXFindimActivity.DESCRIPTION FROM dbo.tbl_MRP_List_DirectMaterials LEFT OUTER JOIN  dbo.vw_AXFindimActivity ON dbo.tbl_MRP_List_DirectMaterials.ActivityCode = dbo.vw_AXFindimActivity.VALUE WHERE(dbo.tbl_MRP_List_DirectMaterials.HeaderDocNum = '" + DOC_NUMBER + "')";
            bool execute = entitycode == train_entity;
            if (execute)
                cmd = new SqlCommand(query_1);
            else
                cmd = new SqlCommand(query_2);


            cmd.Connection = cn;
            adp = new SqlDataAdapter(cmd);
            adp.Fill(dt);

            //PrintString(dt.Rows.Count.ToString());

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    DataRow dtRow = dtTable.NewRow();
                    dtRow["PK"] = row["PK"].ToString();
                    dtRow["HeaderDocNum"] = row["HeaderDocNum"].ToString();
                    dtRow["ActivityCode"] = row["DESCRIPTION"].ToString();
                    dtRow["ItemCode"] = row["ItemCode"].ToString();

                    string desc = row["ItemDescriptionAddl"].ToString();
                    if (!string.IsNullOrEmpty(desc))
                        dtRow["ItemDescription"] = row["ItemDescription"].ToString() + " (" + desc + ")";
                    else
                        dtRow["ItemDescription"] = row["ItemDescription"].ToString();

                    dtRow["UOM"] = row["UOM"].ToString();
                    dtRow["Cost"] = Convert.ToDouble(row["Cost"]).ToString("N");
                    dtRow["Qty"] = Convert.ToDouble(row["Qty"]);
                    dtRow["TotalCost"] = Convert.ToDouble(row["TotalCost"]).ToString("N");
                    dtRow["EdittedQty"] = Convert.ToDouble(row["EdittedQty"]);
                    dtRow["EdittedCost"] = Convert.ToDouble(row["EdittedCost"]).ToString("N");
                    dtRow["EdittiedTotalCost"] = Convert.ToDouble(row["EdittiedTotalCost"]).ToString("N");

                    if (execute)
                        dtRow["RevDesc"] = row["RevDesc"].ToString();
                    else
                        dtRow["RevDesc"] = "";

                    dtTable.Rows.Add(dtRow);
                    materials_total_amount += Convert.ToDouble(row["TotalCost"]);
                }
            }
            dt.Clear();
            cn.Close();
            return dtTable;
        }

        public static DataTable MRPApproval_Direct_Materials(string DOC_NUMBER, string entitycode)
        {
            DataTable dtTable = new DataTable();
            SqlConnection cn = new SqlConnection(GlobalClass.SQLConnString());
            DataTable dt = new DataTable();
            SqlCommand cmd = null;
            SqlDataAdapter adp;
            materials_total_amount = 0;
            mat_edited_total = 0;
            mat_approved_total = 0;

            cn.Open();
            if (dtTable.Columns.Count == 0)
            {
                //Columns for AspxGridview
                dtTable.Columns.Add("PK", typeof(string));
                dtTable.Columns.Add("HeaderDocNum", typeof(string));
                dtTable.Columns.Add("ActivityCode", typeof(string));
                dtTable.Columns.Add("ItemCode", typeof(string));
                dtTable.Columns.Add("ItemDescription", typeof(string));
                dtTable.Columns.Add("UOM", typeof(string));
                dtTable.Columns.Add("Cost", typeof(string));
                dtTable.Columns.Add("Qty", typeof(Double));
                dtTable.Columns.Add("TotalCost", typeof(string));
                dtTable.Columns.Add("EdittedCost", typeof(string));
                dtTable.Columns.Add("EdittedQty", typeof(Double));
                dtTable.Columns.Add("EdittiedTotalCost", typeof(string));
                dtTable.Columns.Add("ApprovedCost", typeof(string));
                dtTable.Columns.Add("ApprovedQty", typeof(Double));
                dtTable.Columns.Add("ApprovedTotalCost", typeof(string));
                dtTable.Columns.Add("RevDesc", typeof(string));
            }

            string query_1 = "SELECT DISTINCT tbl_MRP_List_DirectMaterials.*, vw_AXFindimActivity.DESCRIPTION, vw_AXFindimBananaRevenue.DESCRIPTION AS RevDesc FROM tbl_MRP_List_DirectMaterials LEFT OUTER JOIN vw_AXFindimActivity ON tbl_MRP_List_DirectMaterials.ActivityCode = vw_AXFindimActivity.VALUE LEFT OUTER JOIN vw_AXFindimBananaRevenue ON tbl_MRP_List_DirectMaterials.OprUnit = vw_AXFindimBananaRevenue.VALUE WHERE tbl_MRP_List_DirectMaterials.HeaderDocNum = '" + DOC_NUMBER + "'";

            string query_2 = "SELECT DISTINCT tbl_MRP_List_DirectMaterials.*, vw_AXFindimActivity.DESCRIPTION FROM  tbl_MRP_List_DirectMaterials LEFT OUTER JOIN vw_AXFindimActivity ON tbl_MRP_List_DirectMaterials.ActivityCode = vw_AXFindimActivity.VALUE WHERE tbl_MRP_List_DirectMaterials.HeaderDocNum = '" + DOC_NUMBER + "'";

            bool exec = entitycode == train_entity;
            if (exec)
                cmd = new SqlCommand(query_1);
            else
                cmd = new SqlCommand(query_2);

            cmd.Connection = cn;
            adp = new SqlDataAdapter(cmd);
            adp.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    DataRow dtRow = dtTable.NewRow();
                    dtRow["PK"] = row["PK"].ToString();
                    dtRow["HeaderDocNum"] = row["HeaderDocNum"].ToString();
                    dtRow["ActivityCode"] = row["DESCRIPTION"].ToString();
                    dtRow["ItemCode"] = row["ItemCode"].ToString();

                    string desc = row["ItemDescriptionAddl"].ToString();
                    if (!string.IsNullOrEmpty(desc))
                        dtRow["ItemDescription"] = row["ItemDescription"].ToString() + " (" + desc + ")";
                    else
                        dtRow["ItemDescription"] = row["ItemDescription"].ToString();

                    dtRow["UOM"] = row["UOM"].ToString();
                    dtRow["Cost"] = Convert.ToDouble(row["Cost"]).ToString("N");
                    dtRow["Qty"] = Convert.ToDouble(row["Qty"]);
                    dtRow["TotalCost"] = Convert.ToDouble(row["TotalCost"]).ToString("N");
                    dtRow["EdittedQty"] = Convert.ToDouble(row["EdittedQty"]);
                    dtRow["EdittedCost"] = Convert.ToDouble(row["EdittedCost"]).ToString("N");
                    dtRow["EdittiedTotalCost"] = Convert.ToDouble(row["EdittiedTotalCost"]).ToString("N");
                    dtRow["ApprovedQty"] = Convert.ToDouble(row["ApprovedQty"]);
                    dtRow["ApprovedCost"] = Convert.ToDouble(row["ApprovedCost"]).ToString("N");
                    dtRow["ApprovedTotalCost"] = Convert.ToDouble(row["ApprovedTotalCost"]).ToString("N");

                    if (exec)
                        dtRow["RevDesc"] = row["RevDesc"].ToString();
                    else
                        dtRow["RevDesc"] = "";


                    dtTable.Rows.Add(dtRow);
                    materials_total_amount += Convert.ToDouble(row["TotalCost"]);
                    mat_edited_total += Convert.ToDouble(row["EdittiedTotalCost"]);
                    mat_approved_total += Convert.ToDouble(row["ApprovedTotalCost"]);
                }
            }
            dt.Clear();
            cn.Close();
            return dtTable;
        }

        public static DataTable MRP_OPEX(string DOC_NUMBER, string entitycode)
        {
            DataTable dtTable = new DataTable();
            SqlConnection cn = new SqlConnection(GlobalClass.SQLConnString());
            DataTable dt = new DataTable();
            SqlCommand cmd = null;
            SqlDataAdapter adp;
            opex_total_amount = 0;

            cn.Open();
            if (dtTable.Columns.Count == 0)
            {
                //Columns for AspxGridview
                dtTable.Columns.Add("PK", typeof(string));
                dtTable.Columns.Add("HeaderDocNum", typeof(string));
                dtTable.Columns.Add("ExpenseCodeName", typeof(string));
                dtTable.Columns.Add("ExpenseCode", typeof(string));
                dtTable.Columns.Add("ProcCatCode", typeof(string));
                dtTable.Columns.Add("ProcCatName", typeof(string));
                dtTable.Columns.Add("isItem", typeof(string));
                dtTable.Columns.Add("ItemCode", typeof(string));
                dtTable.Columns.Add("Description", typeof(string));
                dtTable.Columns.Add("DescriptionAddl", typeof(string));
                dtTable.Columns.Add("UOM", typeof(string));
                dtTable.Columns.Add("Cost", typeof(string));
                dtTable.Columns.Add("Qty", typeof(string));
                dtTable.Columns.Add("TotalCost", typeof(string));
                dtTable.Columns.Add("VALUE", typeof(string));
                dtTable.Columns.Add("RevDesc", typeof(string));
                dtTable.Columns.Add("isProdCategory", typeof(string));
            }

            string query_1 = "SELECT DISTINCT dbo.tbl_MRP_List_OPEX.PK, dbo.tbl_MRP_List_OPEX.HeaderDocNum, dbo.tbl_MRP_List_OPEX.TableIdentifier, dbo.tbl_MRP_List_OPEX.ExpenseCode, dbo.tbl_MRP_List_OPEX.OprUnit, dbo.tbl_MRP_List_OPEX.ProcCat, dbo.tbl_MRP_List_OPEX.ItemCode, dbo.tbl_MRP_List_OPEX.Description, dbo.tbl_MRP_List_OPEX.DescriptionAddl, dbo.tbl_MRP_List_OPEX.UOM, dbo.tbl_MRP_List_OPEX.Cost, dbo.tbl_MRP_List_OPEX.Qty, dbo.tbl_MRP_List_OPEX.TotalCost, dbo.tbl_MRP_List_OPEX.EdittedQty, dbo.tbl_MRP_List_OPEX.EdittedCost, dbo.tbl_MRP_List_OPEX.EdittedTotalCost, dbo.tbl_MRP_List_OPEX.ApprovedQty, dbo.tbl_MRP_List_OPEX.ApprovedCost, dbo.tbl_MRP_List_OPEX.ApprovedTotalCost, dbo.tbl_MRP_List_OPEX.QtyPO, dbo.tbl_MRP_List_OPEX.AvailForPO, dbo.vw_AXExpenseAccount.NAME, dbo.vw_AXExpenseAccount.isItem, ISNULL(dbo.vw_AXFindimBananaRevenue.VALUE, '') AS VALUE, ISNULL(dbo.vw_AXFindimBananaRevenue.DESCRIPTION, '') AS RevDesc, ISNULL((SELECT DESCRIPTION AS ProcCatName FROM dbo.vw_AXProdCategory WHERE(NAME = dbo.tbl_MRP_List_OPEX.ProcCat) AND(mainaccount = dbo.tbl_MRP_List_OPEX.ExpenseCode) AND(dataareaid = dbo.tbl_MRP_List.EntityCode) AND(LedgerType = 'Expense')), '') AS ProcCatName, dbo.vw_AXExpenseAccount.isProdCategory FROM   dbo.tbl_MRP_List_OPEX LEFT OUTER JOIN dbo.vw_AXExpenseAccount ON dbo.tbl_MRP_List_OPEX.ExpenseCode = dbo.vw_AXExpenseAccount.MAINACCOUNTID LEFT OUTER JOIN dbo.vw_AXFindimBananaRevenue ON dbo.tbl_MRP_List_OPEX.OprUnit = dbo.vw_AXFindimBananaRevenue.VALUE LEFT OUTER JOIN dbo.tbl_MRP_List ON dbo.tbl_MRP_List_OPEX.HeaderDocNum = dbo.tbl_MRP_List.DocNumber WHERE(dbo.tbl_MRP_List_OPEX.HeaderDocNum = '" + DOC_NUMBER + "')";

            //string query_1 = "SELECT dbo.tbl_MRP_List_OPEX.PK, dbo.tbl_MRP_List_OPEX.HeaderDocNum, dbo.tbl_MRP_List_OPEX.TableIdentifier, dbo.tbl_MRP_List_OPEX.ExpenseCode, dbo.tbl_MRP_List_OPEX.OprUnit, dbo.tbl_MRP_List_OPEX.ProcCat, dbo.tbl_MRP_List_OPEX.ItemCode, dbo.tbl_MRP_List_OPEX.Description, dbo.tbl_MRP_List_OPEX.DescriptionAddl, dbo.tbl_MRP_List_OPEX.UOM, dbo.tbl_MRP_List_OPEX.Cost, dbo.tbl_MRP_List_OPEX.Qty, dbo.tbl_MRP_List_OPEX.TotalCost, dbo.tbl_MRP_List_OPEX.EdittedQty, dbo.tbl_MRP_List_OPEX.EdittedCost, dbo.tbl_MRP_List_OPEX.EdittedTotalCost, dbo.tbl_MRP_List_OPEX.ApprovedQty, dbo.tbl_MRP_List_OPEX.ApprovedCost, dbo.tbl_MRP_List_OPEX.ApprovedTotalCost, dbo.tbl_MRP_List_OPEX.QtyPO, dbo.tbl_MRP_List_OPEX.AvailForPO, dbo.vw_AXExpenseAccount.NAME, dbo.vw_AXExpenseAccount.isItem, ISNULL(dbo.vw_AXFindimBananaRevenue.VALUE, '') AS VALUE, ISNULL(dbo.vw_AXFindimBananaRevenue.DESCRIPTION, '') AS RevDesc, ISNULL((SELECT DESCRIPTION AS ProcCatName FROM    dbo.vw_AXProdCategory WHERE(NAME = dbo.tbl_MRP_List_OPEX.ProcCat) AND(mainaccount = dbo.tbl_MRP_List_OPEX.ExpenseCode) AND(dataareaid = EntityCode) AND(LedgerType = 'Expense')), '') AS ProcCatName FROM dbo.tbl_MRP_List_OPEX LEFT OUTER JOIN dbo.vw_AXExpenseAccount ON dbo.tbl_MRP_List_OPEX.ExpenseCode = dbo.vw_AXExpenseAccount.MAINACCOUNTID LEFT OUTER JOIN dbo.vw_AXFindimBananaRevenue ON dbo.tbl_MRP_List_OPEX.OprUnit = dbo.vw_AXFindimBananaRevenue.VALUE LEFT OUTER JOIN dbo.tbl_MRP_List ON dbo.tbl_MRP_List_OPEX.HeaderDocNum = dbo.tbl_MRP_List.DocNumber WHERE(dbo.tbl_MRP_List_OPEX.HeaderDocNum = '" + DOC_NUMBER + "')";

            //string query_2 = "SELECT tbl_MRP_List_OPEX.*, vw_AXExpenseAccount.NAME, vw_AXExpenseAccount.isItem FROM   tbl_MRP_List_OPEX LEFT OUTER JOIN vw_AXExpenseAccount ON tbl_MRP_List_OPEX.ExpenseCode = vw_AXExpenseAccount.MAINACCOUNTID WHERE [HeaderDocNum] = '" + DOC_NUMBER + "'";

            //if (entitycode == train_entity)
            cmd = new SqlCommand(query_1);
            //else
            //    cmd = new SqlCommand(query_2);

            cmd.Connection = cn;
            adp = new SqlDataAdapter(cmd);
            adp.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    DataRow dtRow = dtTable.NewRow();
                    dtRow["PK"] = row["PK"].ToString();
                    dtRow["HeaderDocNum"] = row["HeaderDocNum"].ToString();
                    dtRow["ExpenseCode"] = row["ExpenseCode"].ToString();
                    dtRow["ExpenseCodeName"] = row["NAME"].ToString();

                    dtRow["ProcCatCode"] = row["ProcCat"].ToString();
                    dtRow["ProcCatName"] = row["ProcCatName"].ToString();

                    dtRow["isItem"] = row["isItem"].ToString();
                    dtRow["isProdCategory"] = row["isProdCategory"].ToString();
                    dtRow["ItemCode"] = row["ItemCode"].ToString();
                    dtRow["Description"] = row["Description"].ToString();
                    dtRow["DescriptionAddl"] = row["DescriptionAddl"].ToString();
                    dtRow["UOM"] = row["UOM"].ToString();
                    dtRow["Cost"] = Convert.ToDouble(row["Cost"]).ToString("N");
                    dtRow["Qty"] = Convert.ToDouble(row["Qty"]).ToString("N");
                    dtRow["TotalCost"] = Convert.ToDouble(row["TotalCost"]).ToString("N");

                    //if (entitycode == train_entity)
                    //{
                    dtRow["VALUE"] = row["VALUE"].ToString();
                    dtRow["RevDesc"] = row["RevDesc"].ToString();
                    //}
                    //else
                    //{
                    //    dtRow["VALUE"] = "";
                    //    dtRow["RevDesc"] = "";
                    //}

                    dtTable.Rows.Add(dtRow);

                    opex_total_amount += Convert.ToDouble(row["TotalCost"]);
                }
            }
            dt.Clear();
            cn.Close();
            return dtTable;
        }

        public static DataTable MRPInvent_OPEX(string DOC_NUMBER, string entitycode)
        {
            DataTable dtTable = new DataTable();
            SqlConnection cn = new SqlConnection(GlobalClass.SQLConnString());
            DataTable dt = new DataTable();
            SqlCommand cmd = null;
            SqlDataAdapter adp;
            opex_total_amount = 0;

            cn.Open();
            if (dtTable.Columns.Count == 0)
            {
                //Columns for AspxGridview
                dtTable.Columns.Add("PK", typeof(string));
                dtTable.Columns.Add("HeaderDocNum", typeof(string));
                dtTable.Columns.Add("ExpenseCodeName", typeof(string));
                dtTable.Columns.Add("ItemCode", typeof(string));
                dtTable.Columns.Add("Description", typeof(string));
                dtTable.Columns.Add("UOM", typeof(string));
                dtTable.Columns.Add("Cost", typeof(string));
                dtTable.Columns.Add("Qty", typeof(Double));
                dtTable.Columns.Add("TotalCost", typeof(string));
                dtTable.Columns.Add("EdittedCost", typeof(string));
                dtTable.Columns.Add("EdittedQty", typeof(Double));
                dtTable.Columns.Add("EdittedTotalCost", typeof(string));
                dtTable.Columns.Add("RevDesc", typeof(string));
            }

            //string query_1 = "SELECT vw_AXExpenseAccount.NAME, tbl_MRP_List_OPEX.*, vw_AXFindimBananaRevenue.DESCRIPTION AS RevDesc FROM tbl_MRP_List_OPEX LEFT OUTER JOIN vw_AXExpenseAccount ON tbl_MRP_List_OPEX.ExpenseCode = vw_AXExpenseAccount.MAINACCOUNTID LEFT OUTER JOIN vw_AXFindimBananaRevenue ON tbl_MRP_List_OPEX.OprUnit = vw_AXFindimBananaRevenue.VALUE WHERE [HeaderDocNum] = '" + DOC_NUMBER + "'";

            //string query_2 = "SELECT tbl_MRP_List_OPEX.*, vw_AXExpenseAccount.NAME FROM   tbl_MRP_List_OPEX LEFT OUTER JOIN vw_AXExpenseAccount ON tbl_MRP_List_OPEX.ExpenseCode = vw_AXExpenseAccount.MAINACCOUNTID WHERE [HeaderDocNum] = '" + DOC_NUMBER + "'";

            string query_1 = "SELECT dbo.vw_AXExpenseAccount.NAME, dbo.tbl_MRP_List_OPEX.PK, dbo.tbl_MRP_List_OPEX.HeaderDocNum, dbo.tbl_MRP_List_OPEX.TableIdentifier, dbo.tbl_MRP_List_OPEX.ExpenseCode, dbo.tbl_MRP_List_OPEX.OprUnit, dbo.tbl_MRP_List_OPEX.ProcCat, dbo.tbl_MRP_List_OPEX.ItemCode, dbo.tbl_MRP_List_OPEX.Description, dbo.tbl_MRP_List_OPEX.DescriptionAddl, dbo.tbl_MRP_List_OPEX.UOM, dbo.tbl_MRP_List_OPEX.Cost, dbo.tbl_MRP_List_OPEX.Qty, dbo.tbl_MRP_List_OPEX.TotalCost, dbo.tbl_MRP_List_OPEX.EdittedQty, dbo.tbl_MRP_List_OPEX.EdittedCost, dbo.tbl_MRP_List_OPEX.EdittedTotalCost, dbo.tbl_MRP_List_OPEX.ApprovedQty, dbo.tbl_MRP_List_OPEX.ApprovedCost, dbo.tbl_MRP_List_OPEX.ApprovedTotalCost, dbo.tbl_MRP_List_OPEX.QtyPO, dbo.tbl_MRP_List_OPEX.AvailForPO, dbo.vw_AXFindimBananaRevenue.DESCRIPTION AS RevDesc FROM  dbo.tbl_MRP_List_OPEX LEFT OUTER JOIN dbo.vw_AXExpenseAccount ON dbo.tbl_MRP_List_OPEX.ExpenseCode = dbo.vw_AXExpenseAccount.MAINACCOUNTID LEFT OUTER JOIN dbo.vw_AXFindimBananaRevenue ON dbo.tbl_MRP_List_OPEX.OprUnit = dbo.vw_AXFindimBananaRevenue.VALUE WHERE(dbo.tbl_MRP_List_OPEX.HeaderDocNum = '" + DOC_NUMBER + "')";

            string query_2 = "SELECT dbo.tbl_MRP_List_OPEX.PK, dbo.tbl_MRP_List_OPEX.HeaderDocNum, dbo.tbl_MRP_List_OPEX.TableIdentifier, dbo.tbl_MRP_List_OPEX.ExpenseCode, dbo.tbl_MRP_List_OPEX.OprUnit, dbo.tbl_MRP_List_OPEX.ProcCat, dbo.tbl_MRP_List_OPEX.ItemCode, dbo.tbl_MRP_List_OPEX.Description, dbo.tbl_MRP_List_OPEX.DescriptionAddl, dbo.tbl_MRP_List_OPEX.UOM, dbo.tbl_MRP_List_OPEX.Cost, dbo.tbl_MRP_List_OPEX.Qty, dbo.tbl_MRP_List_OPEX.TotalCost, dbo.tbl_MRP_List_OPEX.EdittedQty, dbo.tbl_MRP_List_OPEX.EdittedCost, dbo.tbl_MRP_List_OPEX.EdittedTotalCost, dbo.tbl_MRP_List_OPEX.ApprovedQty, dbo.tbl_MRP_List_OPEX.ApprovedCost, dbo.tbl_MRP_List_OPEX.ApprovedTotalCost, dbo.tbl_MRP_List_OPEX.QtyPO, dbo.tbl_MRP_List_OPEX.AvailForPO, dbo.vw_AXExpenseAccount.NAME FROM  dbo.tbl_MRP_List_OPEX LEFT OUTER JOIN dbo.vw_AXExpenseAccount ON dbo.tbl_MRP_List_OPEX.ExpenseCode = dbo.vw_AXExpenseAccount.MAINACCOUNTID WHERE(dbo.tbl_MRP_List_OPEX.HeaderDocNum = '" + DOC_NUMBER + "')";

            bool exec = entitycode == train_entity;
            if (exec)
                cmd = new SqlCommand(query_1);
            else
                cmd = new SqlCommand(query_2);

            cmd.Connection = cn;
            adp = new SqlDataAdapter(cmd);
            adp.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    DataRow dtRow = dtTable.NewRow();
                    dtRow["PK"] = row["PK"].ToString();
                    dtRow["HeaderDocNum"] = row["HeaderDocNum"].ToString();
                    dtRow["ExpenseCodeName"] = row["NAME"].ToString();
                    dtRow["ItemCode"] = row["ItemCode"].ToString();

                    string desc = row["DescriptionAddl"].ToString();
                    if (!string.IsNullOrEmpty(desc))
                        dtRow["Description"] = row["Description"].ToString() + " (" + desc + ")";
                    else
                        dtRow["Description"] = row["Description"].ToString();

                    dtRow["UOM"] = row["UOM"].ToString();
                    dtRow["Cost"] = Convert.ToDouble(row["Cost"]).ToString("N");
                    dtRow["Qty"] = Convert.ToDouble(row["Qty"]);
                    dtRow["TotalCost"] = Convert.ToDouble(row["TotalCost"]).ToString("N");
                    dtRow["EdittedCost"] = Convert.ToDouble(row["EdittedCost"]).ToString("N");
                    dtRow["EdittedQty"] = Convert.ToDouble(row["EdittedQty"]);
                    dtRow["EdittedTotalCost"] = Convert.ToDouble(row["EdittedTotalCost"]).ToString("N");

                    if (exec)
                        dtRow["RevDesc"] = row["RevDesc"].ToString();
                    else
                        dtRow["RevDesc"] = "";

                    dtTable.Rows.Add(dtRow);

                    opex_total_amount += Convert.ToDouble(row["TotalCost"]);
                }
            }
            dt.Clear();
            cn.Close();
            return dtTable;
        }

        public static DataTable MRPApproval_OPEX(string DOC_NUMBER, string entitycode)
        {
            DataTable dtTable = new DataTable();
            SqlConnection cn = new SqlConnection(GlobalClass.SQLConnString());
            DataTable dt = new DataTable();
            SqlCommand cmd = null;
            SqlDataAdapter adp;
            opex_total_amount = 0;
            op_approved_total = 0;
            op_edited_total = 0;

            cn.Open();
            if (dtTable.Columns.Count == 0)
            {
                //Columns for AspxGridview
                dtTable.Columns.Add("PK", typeof(string));
                dtTable.Columns.Add("HeaderDocNum", typeof(string));
                dtTable.Columns.Add("ExpenseCodeName", typeof(string));
                dtTable.Columns.Add("ItemCode", typeof(string));
                dtTable.Columns.Add("Description", typeof(string));
                dtTable.Columns.Add("UOM", typeof(string));
                dtTable.Columns.Add("Cost", typeof(string));
                dtTable.Columns.Add("Qty", typeof(Double));
                dtTable.Columns.Add("TotalCost", typeof(string));
                dtTable.Columns.Add("EdittedCost", typeof(string));
                dtTable.Columns.Add("EdittedQty", typeof(Double));
                dtTable.Columns.Add("EdittedTotalCost", typeof(string));
                dtTable.Columns.Add("ApprovedCost", typeof(string));
                dtTable.Columns.Add("ApprovedQty", typeof(Double));
                dtTable.Columns.Add("ApprovedTotalCost", typeof(string));
                dtTable.Columns.Add("RevDesc", typeof(string));
            }

            string query_1 = "SELECT tbl_MRP_List_OPEX.*, vw_AXExpenseAccount.NAME, vw_AXFindimBananaRevenue.DESCRIPTION AS RevDesc FROM tbl_MRP_List_OPEX LEFT OUTER JOIN vw_AXExpenseAccount ON tbl_MRP_List_OPEX.ExpenseCode = vw_AXExpenseAccount.MAINACCOUNTID LEFT OUTER JOIN vw_AXFindimBananaRevenue ON tbl_MRP_List_OPEX.OprUnit = vw_AXFindimBananaRevenue.VALUE WHERE [HeaderDocNum] = '" + DOC_NUMBER + "'";

            string query_2 = "SELECT tbl_MRP_List_OPEX.*, vw_AXExpenseAccount.NAME FROM   tbl_MRP_List_OPEX LEFT OUTER JOIN vw_AXExpenseAccount ON tbl_MRP_List_OPEX.ExpenseCode = vw_AXExpenseAccount.MAINACCOUNTID WHERE [HeaderDocNum] = '" + DOC_NUMBER + "'";

            bool exec = entitycode == train_entity;
            if (exec)
                cmd = new SqlCommand(query_1);
            else
                cmd = new SqlCommand(query_2);

            cmd.Connection = cn;
            adp = new SqlDataAdapter(cmd);
            adp.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    DataRow dtRow = dtTable.NewRow();
                    dtRow["PK"] = row["PK"].ToString();
                    dtRow["HeaderDocNum"] = row["HeaderDocNum"].ToString();
                    dtRow["ExpenseCodeName"] = row["NAME"].ToString();
                    dtRow["ItemCode"] = row["ItemCode"].ToString();

                    string desc = row["DescriptionAddl"].ToString();
                    if (!string.IsNullOrEmpty(desc))
                        dtRow["Description"] = row["Description"].ToString() + " (" + desc + ")";
                    else
                        dtRow["Description"] = row["Description"].ToString();

                    dtRow["UOM"] = row["UOM"].ToString();
                    dtRow["Cost"] = Convert.ToDouble(row["Cost"]).ToString("N");
                    dtRow["Qty"] = Convert.ToDouble(row["Qty"]);
                    dtRow["TotalCost"] = Convert.ToDouble(row["TotalCost"]).ToString("N");
                    dtRow["EdittedCost"] = Convert.ToDouble(row["EdittedCost"]).ToString("N");
                    dtRow["EdittedQty"] = Convert.ToDouble(row["EdittedQty"]);
                    dtRow["EdittedTotalCost"] = Convert.ToDouble(row["EdittedTotalCost"]).ToString("N");
                    dtRow["ApprovedQty"] = Convert.ToDouble(row["ApprovedQty"]);
                    dtRow["ApprovedCost"] = Convert.ToDouble(row["ApprovedCost"]).ToString("N");
                    dtRow["ApprovedTotalCost"] = Convert.ToDouble(row["ApprovedTotalCost"]).ToString("N");

                    if (exec)
                        dtRow["RevDesc"] = row["RevDesc"].ToString();
                    else
                        dtRow["RevDesc"] = "";

                    dtTable.Rows.Add(dtRow);
                    opex_total_amount += Convert.ToDouble(row["TotalCost"]);
                    op_edited_total += Convert.ToDouble(row["EdittedTotalCost"]);
                    op_approved_total += Convert.ToDouble(row["ApprovedTotalCost"]);
                }
            }
            dt.Clear();
            cn.Close();
            return dtTable;
        }

        public static DataTable MRP_ManPower(string DOC_NUMBER, string entitycode)
        {
            DataTable dtTable = new DataTable();
            SqlConnection cn = new SqlConnection(GlobalClass.SQLConnString());
            DataTable dt = new DataTable();
            SqlCommand cmd = null;
            SqlDataAdapter adp;
            manpower_total_amount = 0;

            cn.Open();
            if (dtTable.Columns.Count == 0)
            {
                //Columns for AspxGridview
                dtTable.Columns.Add("PK", typeof(string));
                dtTable.Columns.Add("HeaderDocNum", typeof(string));
                dtTable.Columns.Add("ActivityCode", typeof(string));
                dtTable.Columns.Add("ManPowerTypeKey", typeof(Int32));
                dtTable.Columns.Add("ManPowerTypeKeyName", typeof(string));
                dtTable.Columns.Add("Description", typeof(string));
                dtTable.Columns.Add("UOM", typeof(string));
                dtTable.Columns.Add("Cost", typeof(string));
                dtTable.Columns.Add("Qty", typeof(string));
                dtTable.Columns.Add("TotalCost", typeof(string));
                dtTable.Columns.Add("VALUE", typeof(string));
                dtTable.Columns.Add("RevDesc", typeof(string));
            }

            string query_1 = "SELECT DISTINCT tbl_MRP_List_ManPower.*, tbl_System_ManPowerType.ManPowerTypeDesc, vw_AXFindimActivity.DESCRIPTION AS AC_Desc, vw_AXFindimBananaRevenue.VALUE, vw_AXFindimBananaRevenue.DESCRIPTION AS RevDesc FROM tbl_MRP_List_ManPower LEFT OUTER JOIN tbl_System_ManPowerType ON tbl_MRP_List_ManPower.ManPowerTypeKey = tbl_System_ManPowerType.PK LEFT OUTER JOIN vw_AXFindimActivity ON tbl_MRP_List_ManPower.ActivityCode = vw_AXFindimActivity.VALUE LEFT OUTER JOIN vw_AXFindimBananaRevenue ON tbl_MRP_List_ManPower.OprUnit = vw_AXFindimBananaRevenue.VALUE WHERE [HeaderDocNum] = '" + DOC_NUMBER + "'";

            string query_2 = "SELECT tbl_MRP_List_ManPower.*, tbl_System_ManPowerType.ManPowerTypeDesc, vw_AXFindimActivity.DESCRIPTION as AC_Desc FROM tbl_MRP_List_ManPower LEFT OUTER JOIN tbl_System_ManPowerType ON tbl_MRP_List_ManPower.ManPowerTypeKey = tbl_System_ManPowerType.PK LEFT OUTER JOIN vw_AXFindimActivity ON tbl_MRP_List_ManPower.ActivityCode = vw_AXFindimActivity.VALUE WHERE [HeaderDocNum] = '" + DOC_NUMBER + "'";
            //string query = "SELECT tbl_MRP_List_ManPower.*, tbl_System_ManPowerType.ManPowerTypeDesc FROM tbl_MRP_List_ManPower LEFT OUTER JOIN tbl_System_ManPowerType ON tbl_MRP_List_ManPower.ManPowerTypeKey = tbl_System_ManPowerType.PK WHERE [HeaderDocNum] = '" + DOC_NUMBER + "'";

            if (entitycode == train_entity)
                cmd = new SqlCommand(query_1);
            else
                cmd = new SqlCommand(query_2);

            cmd.Connection = cn;
            adp = new SqlDataAdapter(cmd);
            adp.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    DataRow dtRow = dtTable.NewRow();
                    dtRow["PK"] = row["PK"].ToString();
                    dtRow["HeaderDocNum"] = row["HeaderDocNum"].ToString();
                    dtRow["ActivityCode"] = row["AC_Desc"].ToString();
                    dtRow["ManPowerTypeKey"] = Convert.ToInt32(row["ManPowerTypeKey"]);
                    dtRow["ManPowerTypeKeyName"] = row["ManPowerTypeDesc"].ToString();
                    dtRow["Description"] = row["Description"].ToString();
                    dtRow["UOM"] = row["UOM"].ToString();
                    dtRow["Cost"] = Convert.ToDouble(row["Cost"]).ToString("N");
                    dtRow["Qty"] = Convert.ToDouble(row["Qty"]).ToString("N");
                    dtRow["TotalCost"] = Convert.ToDouble(row["TotalCost"]).ToString("N");
                    if (entitycode == train_entity)
                    {
                        dtRow["VALUE"] = row["VALUE"].ToString();
                        dtRow["RevDesc"] = row["RevDesc"].ToString();
                    }
                    else
                    {
                        dtRow["VALUE"] = "";
                        dtRow["RevDesc"] = "";
                    }

                    dtTable.Rows.Add(dtRow);
                    manpower_total_amount += Convert.ToDouble(row["TotalCost"]);
                }
            }
            dt.Clear();
            cn.Close();
            return dtTable;
        }


        public static DataTable MRPInvent_ManPower(string DOC_NUMBER, string entitycode)
        {
            DataTable dtTable = new DataTable();
            SqlConnection cn = new SqlConnection(GlobalClass.SQLConnString());
            DataTable dt = new DataTable();
            SqlCommand cmd = null;
            SqlDataAdapter adp;
            manpower_total_amount = 0;

            cn.Open();
            if (dtTable.Columns.Count == 0)
            {
                //Columns for AspxGridview
                dtTable.Columns.Add("PK", typeof(string));
                dtTable.Columns.Add("HeaderDocNum", typeof(string));
                dtTable.Columns.Add("ActivityCode", typeof(string));
                dtTable.Columns.Add("ManPowerTypeKey", typeof(Int32));
                dtTable.Columns.Add("ManPowerTypeKeyName", typeof(string));
                dtTable.Columns.Add("Description", typeof(string));
                dtTable.Columns.Add("UOM", typeof(string));
                dtTable.Columns.Add("Cost", typeof(string));
                dtTable.Columns.Add("Qty", typeof(Double));
                dtTable.Columns.Add("TotalCost", typeof(string));
                dtTable.Columns.Add("EdittedCost", typeof(string));
                dtTable.Columns.Add("EdittedQty", typeof(Double));
                dtTable.Columns.Add("EdittiedTotalCost", typeof(string));
                dtTable.Columns.Add("RevDesc", typeof(string));
            }

            //string query_1 = "SELECT tbl_MRP_List_ManPower.*, tbl_System_ManPowerType.ManPowerTypeDesc, vw_AXFindimActivity.DESCRIPTION AS AC_Desc, vw_AXFindimBananaRevenue.DESCRIPTION AS RevDesc FROM tbl_MRP_List_ManPower LEFT OUTER JOIN tbl_System_ManPowerType ON tbl_MRP_List_ManPower.ManPowerTypeKey = tbl_System_ManPowerType.PK LEFT OUTER JOIN vw_AXFindimActivity ON tbl_MRP_List_ManPower.ActivityCode = vw_AXFindimActivity.VALUE LEFT OUTER JOIN vw_AXFindimBananaRevenue ON tbl_MRP_List_ManPower.OprUnit = vw_AXFindimBananaRevenue.VALUE WHERE [HeaderDocNum] = '" + DOC_NUMBER + "'";

            //string query_2 = "SELECT tbl_MRP_List_ManPower.*, tbl_System_ManPowerType.ManPowerTypeDesc, vw_AXFindimActivity.DESCRIPTION as AC_Desc FROM tbl_MRP_List_ManPower LEFT OUTER JOIN tbl_System_ManPowerType ON tbl_MRP_List_ManPower.ManPowerTypeKey = tbl_System_ManPowerType.PK LEFT OUTER JOIN vw_AXFindimActivity ON tbl_MRP_List_ManPower.ActivityCode = vw_AXFindimActivity.VALUE WHERE [HeaderDocNum] = '" + DOC_NUMBER + "'";

            string query_1 = "SELECT dbo.tbl_MRP_List_ManPower.PK, dbo.tbl_MRP_List_ManPower.HeaderDocNum, dbo.tbl_MRP_List_ManPower.TableIdentifier, dbo.tbl_MRP_List_ManPower.ActivityCode, dbo.tbl_MRP_List_ManPower.OprUnit, dbo.tbl_MRP_List_ManPower.ManPowerTypeKey, dbo.tbl_MRP_List_ManPower.Description, dbo.tbl_MRP_List_ManPower.UOM, dbo.tbl_MRP_List_ManPower.Cost, dbo.tbl_MRP_List_ManPower.Qty, dbo.tbl_MRP_List_ManPower.TotalCost, dbo.tbl_MRP_List_ManPower.EdittedQty, dbo.tbl_MRP_List_ManPower.EdittedCost, dbo.tbl_MRP_List_ManPower.EdittiedTotalCost, dbo.tbl_MRP_List_ManPower.ApprovedQty, dbo.tbl_MRP_List_ManPower.ApprovedCost, dbo.tbl_MRP_List_ManPower.ApprovedTotalCost,         dbo.tbl_System_ManPowerType.ManPowerTypeDesc, dbo.vw_AXFindimActivity.DESCRIPTION AS AC_Desc, dbo.vw_AXFindimBananaRevenue.DESCRIPTION AS RevDesc FROM  dbo.tbl_MRP_List_ManPower LEFT OUTER JOIN dbo.vw_AXFindimBananaRevenue ON dbo.tbl_MRP_List_ManPower.OprUnit = dbo.vw_AXFindimBananaRevenue.VALUE LEFT OUTER JOIN dbo.tbl_System_ManPowerType ON dbo.tbl_MRP_List_ManPower.ManPowerTypeKey = dbo.tbl_System_ManPowerType.PK LEFT OUTER JOIN dbo.vw_AXFindimActivity ON dbo.tbl_MRP_List_ManPower.ActivityCode = dbo.vw_AXFindimActivity.VALUE WHERE(dbo.tbl_MRP_List_ManPower.HeaderDocNum = '" + DOC_NUMBER + "')";

            string query_2 = "SELECT dbo.tbl_MRP_List_ManPower.PK, dbo.tbl_MRP_List_ManPower.HeaderDocNum, dbo.tbl_MRP_List_ManPower.TableIdentifier, dbo.tbl_MRP_List_ManPower.ActivityCode, dbo.tbl_MRP_List_ManPower.OprUnit, dbo.tbl_MRP_List_ManPower.ManPowerTypeKey, dbo.tbl_MRP_List_ManPower.Description, dbo.tbl_MRP_List_ManPower.UOM, dbo.tbl_MRP_List_ManPower.Cost, dbo.tbl_MRP_List_ManPower.Qty, dbo.tbl_MRP_List_ManPower.TotalCost, dbo.tbl_MRP_List_ManPower.EdittedQty, dbo.tbl_MRP_List_ManPower.EdittedCost, dbo.tbl_MRP_List_ManPower.EdittiedTotalCost, dbo.tbl_MRP_List_ManPower.ApprovedQty, dbo.tbl_MRP_List_ManPower.ApprovedCost, dbo.tbl_MRP_List_ManPower.ApprovedTotalCost, dbo.tbl_System_ManPowerType.ManPowerTypeDesc, dbo.vw_AXFindimActivity.DESCRIPTION AS AC_Desc FROM dbo.tbl_MRP_List_ManPower LEFT OUTER JOIN dbo.tbl_System_ManPowerType ON dbo.tbl_MRP_List_ManPower.ManPowerTypeKey = dbo.tbl_System_ManPowerType.PK LEFT OUTER JOIN dbo.vw_AXFindimActivity ON dbo.tbl_MRP_List_ManPower.ActivityCode = dbo.vw_AXFindimActivity.VALUE WHERE(dbo.tbl_MRP_List_ManPower.HeaderDocNum = '" + DOC_NUMBER + "')";

            bool exec = entitycode == train_entity;
            if (exec)
                cmd = new SqlCommand(query_1);
            else
                cmd = new SqlCommand(query_2);

            cmd.Connection = cn;
            adp = new SqlDataAdapter(cmd);
            adp.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    DataRow dtRow = dtTable.NewRow();
                    dtRow["PK"] = row["PK"].ToString();
                    dtRow["HeaderDocNum"] = row["HeaderDocNum"].ToString();
                    dtRow["ActivityCode"] = row["AC_Desc"].ToString();
                    dtRow["ManPowerTypeKey"] = Convert.ToInt32(row["ManPowerTypeKey"]);
                    dtRow["ManPowerTypeKeyName"] = row["ManPowerTypeDesc"].ToString();
                    dtRow["Description"] = row["Description"].ToString();
                    dtRow["UOM"] = row["UOM"].ToString();
                    dtRow["Cost"] = Convert.ToDouble(row["Cost"]).ToString("N");
                    dtRow["Qty"] = Convert.ToDouble(row["Qty"]);
                    dtRow["TotalCost"] = Convert.ToDouble(row["TotalCost"]).ToString("N");
                    dtRow["EdittedCost"] = Convert.ToDouble(row["EdittedCost"]).ToString("N");
                    dtRow["EdittedQty"] = Convert.ToDouble(row["EdittedQty"]);
                    dtRow["EdittiedTotalCost"] = Convert.ToDouble(row["EdittiedTotalCost"]).ToString("N");

                    if (exec)
                        dtRow["RevDesc"] = row["RevDesc"].ToString();
                    else
                        dtRow["RevDesc"] = "";

                    dtTable.Rows.Add(dtRow);

                    manpower_total_amount += Convert.ToDouble(row["TotalCost"]);
                }
            }
            dt.Clear();
            cn.Close();
            return dtTable;
        }

        public static DataTable MRPApproval_ManPower(string DOC_NUMBER, string entitycode)
        {
            DataTable dtTable = new DataTable();
            SqlConnection cn = new SqlConnection(GlobalClass.SQLConnString());
            DataTable dt = new DataTable();
            SqlCommand cmd = null;
            SqlDataAdapter adp;
            manpower_total_amount = 0;
            man_edited_total = 0;
            man_approved_total = 0;

            cn.Open();
            if (dtTable.Columns.Count == 0)
            {
                //Columns for AspxGridview
                dtTable.Columns.Add("PK", typeof(string));
                dtTable.Columns.Add("HeaderDocNum", typeof(string));
                dtTable.Columns.Add("ActivityCode", typeof(string));
                dtTable.Columns.Add("ManPowerTypeKey", typeof(Int32));
                dtTable.Columns.Add("ManPowerTypeKeyName", typeof(string));
                dtTable.Columns.Add("Description", typeof(string));
                dtTable.Columns.Add("UOM", typeof(string));
                dtTable.Columns.Add("Cost", typeof(string));
                dtTable.Columns.Add("Qty", typeof(Double));
                dtTable.Columns.Add("TotalCost", typeof(string));
                dtTable.Columns.Add("EdittedCost", typeof(string));
                dtTable.Columns.Add("EdittedQty", typeof(Double));
                dtTable.Columns.Add("EdittiedTotalCost", typeof(string));
                dtTable.Columns.Add("ApprovedCost", typeof(string));
                dtTable.Columns.Add("ApprovedQty", typeof(Double));
                dtTable.Columns.Add("ApprovedTotalCost", typeof(string));
                dtTable.Columns.Add("RevDesc", typeof(string));
            }

            string query_1 = "SELECT tbl_MRP_List_ManPower.*, tbl_System_ManPowerType.ManPowerTypeDesc, vw_AXFindimActivity.DESCRIPTION AS AC_Desc, vw_AXFindimBananaRevenue.DESCRIPTION AS RevDesc FROM tbl_MRP_List_ManPower LEFT OUTER JOIN tbl_System_ManPowerType ON tbl_MRP_List_ManPower.ManPowerTypeKey = tbl_System_ManPowerType.PK LEFT OUTER JOIN vw_AXFindimActivity ON tbl_MRP_List_ManPower.ActivityCode = vw_AXFindimActivity.VALUE LEFT OUTER JOIN vw_AXFindimBananaRevenue ON tbl_MRP_List_ManPower.OprUnit = vw_AXFindimBananaRevenue.VALUE WHERE [HeaderDocNum] = '" + DOC_NUMBER + "'";

            string query_2 = "SELECT tbl_MRP_List_ManPower.*, tbl_System_ManPowerType.ManPowerTypeDesc, vw_AXFindimActivity.DESCRIPTION as AC_Desc FROM tbl_MRP_List_ManPower LEFT OUTER JOIN tbl_System_ManPowerType ON tbl_MRP_List_ManPower.ManPowerTypeKey = tbl_System_ManPowerType.PK LEFT OUTER JOIN vw_AXFindimActivity ON tbl_MRP_List_ManPower.ActivityCode = vw_AXFindimActivity.VALUE WHERE [HeaderDocNum] = '" + DOC_NUMBER + "'";

            bool exec = entitycode == train_entity;
            if (exec)
                cmd = new SqlCommand(query_1);
            else
                cmd = new SqlCommand(query_2);

            cmd.Connection = cn;
            adp = new SqlDataAdapter(cmd);
            adp.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    DataRow dtRow = dtTable.NewRow();
                    dtRow["PK"] = row["PK"].ToString();
                    dtRow["HeaderDocNum"] = row["HeaderDocNum"].ToString();
                    dtRow["ActivityCode"] = row["AC_Desc"].ToString();
                    dtRow["ManPowerTypeKey"] = Convert.ToInt32(row["ManPowerTypeKey"]);
                    dtRow["ManPowerTypeKeyName"] = row["ManPowerTypeDesc"].ToString();
                    dtRow["Description"] = row["Description"].ToString();
                    dtRow["UOM"] = row["UOM"].ToString();
                    dtRow["Cost"] = Convert.ToDouble(row["Cost"]).ToString("N");
                    dtRow["Qty"] = Convert.ToDouble(row["Qty"]);
                    dtRow["TotalCost"] = Convert.ToDouble(row["TotalCost"]).ToString("N");
                    dtRow["EdittedCost"] = Convert.ToDouble(row["EdittedCost"]).ToString("N");
                    dtRow["EdittedQty"] = Convert.ToDouble(row["EdittedQty"]);
                    dtRow["EdittiedTotalCost"] = Convert.ToDouble(row["EdittiedTotalCost"]).ToString("N");
                    dtRow["ApprovedQty"] = Convert.ToDouble(row["ApprovedQty"]);
                    dtRow["ApprovedCost"] = Convert.ToDouble(row["ApprovedCost"]).ToString("N");
                    dtRow["ApprovedTotalCost"] = Convert.ToDouble(row["ApprovedTotalCost"]).ToString("N");

                    if (exec)
                        dtRow["RevDesc"] = row["RevDesc"].ToString();
                    else
                        dtRow["RevDesc"] = "";

                    dtTable.Rows.Add(dtRow);

                    manpower_total_amount += Convert.ToDouble(row["TotalCost"]);
                    man_edited_total += Convert.ToDouble(row["EdittiedTotalCost"]);
                    man_approved_total += Convert.ToDouble(row["ApprovedTotalCost"]);
                }
            }
            dt.Clear();
            cn.Close();
            return dtTable;
        }

        public static DataTable MRP_Revenue(string DOC_NUMBER, string entitycode)
        {
            DataTable dtTable = new DataTable();
            SqlConnection cn = new SqlConnection(GlobalClass.SQLConnString());
            DataTable dt = new DataTable();
            SqlCommand cmd = null;
            SqlDataAdapter adp;
            revenue_total_amount = 0;

            cn.Open();
            if (dtTable.Columns.Count == 0)
            {
                //Columns for AspxGridview
                dtTable.Columns.Add("PK", typeof(string));
                dtTable.Columns.Add("HeaderDocNum", typeof(string));
                dtTable.Columns.Add("ProductName", typeof(string));
                dtTable.Columns.Add("FarmName", typeof(string));
                dtTable.Columns.Add("Prize", typeof(string));
                dtTable.Columns.Add("UOM", typeof(string));
                dtTable.Columns.Add("Volume", typeof(string));
                dtTable.Columns.Add("TotalPrize", typeof(string));
                dtTable.Columns.Add("VALUE", typeof(string));
                dtTable.Columns.Add("RevDesc", typeof(string));
            }

            string query_1 = "SELECT tbl_MRP_List_RevenueAssumptions.*, vw_AXFindimBananaRevenue.VALUE, vw_AXFindimBananaRevenue.DESCRIPTION as RevDesc FROM tbl_MRP_List_RevenueAssumptions LEFT OUTER JOIN vw_AXFindimBananaRevenue ON tbl_MRP_List_RevenueAssumptions.OprUnit = vw_AXFindimBananaRevenue.VALUE WHERE [HeaderDocNum] = '" + DOC_NUMBER + "'";

            string query_2 = "SELECT * FROM [dbo].[tbl_MRP_List_RevenueAssumptions] WHERE [HeaderDocNum] = '" + DOC_NUMBER + "'";

            bool execute = entitycode == train_entity;
            if (execute)
                cmd = new SqlCommand(query_1);
            else
                cmd = new SqlCommand(query_2);

            cmd.Connection = cn;
            adp = new SqlDataAdapter(cmd);
            adp.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    DataRow dtRow = dtTable.NewRow();
                    dtRow["PK"] = row["PK"].ToString();
                    dtRow["HeaderDocNum"] = row["HeaderDocNum"].ToString();
                    dtRow["ProductName"] = row["ProductName"].ToString();
                    dtRow["FarmName"] = row["FarmName"].ToString();
                    dtRow["UOM"] = row["UOM"].ToString();
                    dtRow["Prize"] = Convert.ToDouble(row["Prize"]).ToString("N");
                    dtRow["Volume"] = Convert.ToDouble(row["Volume"]).ToString("N");
                    dtRow["TotalPrize"] = Convert.ToDouble(row["TotalPrize"]).ToString("N");
                    if (execute)
                    {
                        dtRow["VALUE"] = row["VALUE"].ToString();
                        dtRow["RevDesc"] = row["RevDesc"].ToString();
                    }
                    else
                    {
                        dtRow["VALUE"] = "";
                        dtRow["RevDesc"] = "";
                    }

                    dtTable.Rows.Add(dtRow);

                    revenue_total_amount += Convert.ToDouble(row["TotalPrize"]);
                }
            }
            dt.Clear();
            cn.Close();
            return dtTable;
        }

        public static DataTable MRP_CAPEX(string DOC_NUMBER, string entitycode)
        {
            DataTable dtTable = new DataTable();
            SqlConnection cn = new SqlConnection(GlobalClass.SQLConnString());
            DataTable dt = new DataTable();
            SqlCommand cmd = null;
            SqlDataAdapter adp;
            capex_total_amount = 0;

            cn.Open();
            if (dtTable.Columns.Count == 0)
            {
                //Columns for AspxGridview
                dtTable.Columns.Add("PK", typeof(string));
                dtTable.Columns.Add("HeaderDocNum", typeof(string));
                dtTable.Columns.Add("Description", typeof(string));
                dtTable.Columns.Add("ProdCode", typeof(string));
                dtTable.Columns.Add("ProdCat", typeof(string));
                dtTable.Columns.Add("UOM", typeof(string));
                dtTable.Columns.Add("Cost", typeof(string));
                dtTable.Columns.Add("Qty", typeof(string));
                dtTable.Columns.Add("TotalCost", typeof(string));
                dtTable.Columns.Add("VALUE", typeof(string));
                dtTable.Columns.Add("RevDesc", typeof(string));
            }

            string query_1 = "SELECT DISTINCT dbo.tbl_MRP_List_CAPEX.*, dbo.vw_AXFindimBananaRevenue.VALUE, dbo.vw_AXFindimBananaRevenue.DESCRIPTION AS RevDesc, dbo.vw_AXProdCategory.DESCRIPTION AS ProdDesc FROM dbo.tbl_MRP_List_CAPEX LEFT OUTER JOIN dbo.vw_AXProdCategory ON dbo.tbl_MRP_List_CAPEX.ProdCat = dbo.vw_AXProdCategory.NAME LEFT OUTER JOIN dbo.vw_AXFindimBananaRevenue ON dbo.tbl_MRP_List_CAPEX.OprUnit = dbo.vw_AXFindimBananaRevenue.VALUE WHERE [HeaderDocNum] = '" + DOC_NUMBER + "'";

            string query_2 = "SELECT DISTINCT dbo.vw_AXProdCategory.DESCRIPTION AS ProdDesc, dbo.tbl_MRP_List_CAPEX.* FROM dbo.tbl_MRP_List_CAPEX LEFT OUTER JOIN dbo.vw_AXProdCategory ON dbo.tbl_MRP_List_CAPEX.ProdCat = dbo.vw_AXProdCategory.NAME WHERE [HeaderDocNum] = '" + DOC_NUMBER + "'";

            bool execute = entitycode == train_entity;
            if (execute)
                cmd = new SqlCommand(query_1);
            else
                cmd = new SqlCommand(query_2);

            cmd.Connection = cn;
            adp = new SqlDataAdapter(cmd);
            adp.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    DataRow dtRow = dtTable.NewRow();
                    dtRow["PK"] = row["PK"].ToString();
                    dtRow["HeaderDocNum"] = row["HeaderDocNum"].ToString();
                    dtRow["Description"] = row["Description"].ToString();
                    dtRow["ProdCode"] = row["ProdCat"].ToString();
                    dtRow["ProdCat"] = row["ProdDesc"].ToString();
                    dtRow["UOM"] = row["UOM"].ToString();
                    dtRow["Cost"] = Convert.ToDouble(row["Cost"]).ToString("N");
                    dtRow["Qty"] = Convert.ToDouble(row["Qty"]).ToString("N");
                    dtRow["TotalCost"] = Convert.ToDouble(row["TotalCost"]).ToString("N");

                    if (execute)
                    {
                        dtRow["VALUE"] = row["VALUE"].ToString();
                        dtRow["RevDesc"] = row["RevDesc"].ToString();
                    }
                    else
                    {
                        dtRow["VALUE"] = "";
                        dtRow["RevDesc"] = "";
                    }

                    dtTable.Rows.Add(dtRow);

                    //for Preview Page
                    capex_total_amount += Convert.ToDouble(row["TotalCost"]);
                }
            }
            dt.Clear();
            cn.Close();
            return dtTable;
        }

        public static DataTable MRPInvent_CAPEX(string DOC_NUMBER, string entitycode)
        {
            DataTable dtTable = new DataTable();
            SqlConnection cn = new SqlConnection(GlobalClass.SQLConnString());
            DataTable dt = new DataTable();
            SqlCommand cmd = null;
            SqlDataAdapter adp;
            capex_total_amount = 0;

            cn.Open();
            if (dtTable.Columns.Count == 0)
            {
                //Columns for AspxGridview
                dtTable.Columns.Add("PK", typeof(string));
                dtTable.Columns.Add("HeaderDocNum", typeof(string));
                dtTable.Columns.Add("ProdDesc", typeof(string));
                dtTable.Columns.Add("Description", typeof(string));
                dtTable.Columns.Add("UOM", typeof(string));
                dtTable.Columns.Add("Cost", typeof(string));
                dtTable.Columns.Add("Qty", typeof(Double));
                dtTable.Columns.Add("TotalCost", typeof(string));
                dtTable.Columns.Add("EdittedCost", typeof(string));
                dtTable.Columns.Add("EdittedQty", typeof(Double));
                dtTable.Columns.Add("EdittiedTotalCost", typeof(string));
                dtTable.Columns.Add("RevDesc", typeof(string));

            }

            //string query_1 = "SELECT DISTINCT dbo.tbl_MRP_List_CAPEX.PK, dbo.vw_AXFindimBananaRevenue.VALUE, dbo.vw_AXFindimBananaRevenue.DESCRIPTION AS RevDesc, dbo.vw_AXProdCategory.DESCRIPTION AS ProdDesc, dbo.tbl_MRP_List_CAPEX.HeaderDocNum, dbo.tbl_MRP_List_CAPEX.TableIdentifier, dbo.tbl_MRP_List_CAPEX.OprUnit, dbo.tbl_MRP_List_CAPEX.ProdCat, dbo.tbl_MRP_List_CAPEX.Description, dbo.tbl_MRP_List_CAPEX.UOM, dbo.tbl_MRP_List_CAPEX.Cost, dbo.tbl_MRP_List_CAPEX.Qty, dbo.tbl_MRP_List_CAPEX.TotalCost, dbo.tbl_MRP_List_CAPEX.CIPSIPNumber, dbo.tbl_MRP_List_CAPEX.EdittedQty, dbo.tbl_MRP_List_CAPEX.EdittedCost, dbo.tbl_MRP_List_CAPEX.EdittiedTotalCost, dbo.tbl_MRP_List_CAPEX.ApprovedQty, dbo.tbl_MRP_List_CAPEX.ApprovedCost, dbo.tbl_MRP_List_CAPEX.ApprovedTotalCost, dbo.tbl_MRP_List_CAPEX.QtyPO, dbo.tbl_MRP_List_CAPEX.AvailForPO FROM   dbo.tbl_MRP_List_CAPEX LEFT OUTER JOIN dbo.vw_AXFindimBananaRevenue ON dbo.tbl_MRP_List_CAPEX.OprUnit = dbo.vw_AXFindimBananaRevenue.VALUE LEFT OUTER JOIN dbo.vw_AXProdCategory ON dbo.tbl_MRP_List_CAPEX.ProdCat = dbo.vw_AXProdCategory.NAME WHERE [HeaderDocNum] = '" + DOC_NUMBER + "'";

            string query_1 = "SELECT DISTINCT dbo.tbl_MRP_List_CAPEX.PK, ISNULL(dbo.vw_AXFindimBananaRevenue.VALUE, '') AS VALUE, ISNULL(dbo.vw_AXFindimBananaRevenue.DESCRIPTION, '') AS RevDesc, dbo.vw_AXProdCategory.DESCRIPTION AS ProdDesc, dbo.tbl_MRP_List_CAPEX.HeaderDocNum, dbo.tbl_MRP_List_CAPEX.TableIdentifier, dbo.tbl_MRP_List_CAPEX.OprUnit, dbo.tbl_MRP_List_CAPEX.ProdCat, dbo.tbl_MRP_List_CAPEX.Description, dbo.tbl_MRP_List_CAPEX.UOM, dbo.tbl_MRP_List_CAPEX.Cost, dbo.tbl_MRP_List_CAPEX.Qty, dbo.tbl_MRP_List_CAPEX.TotalCost, dbo.tbl_MRP_List_CAPEX.CIPSIPNumber, dbo.tbl_MRP_List_CAPEX.EdittedQty, dbo.tbl_MRP_List_CAPEX.EdittedCost, dbo.tbl_MRP_List_CAPEX.EdittiedTotalCost, dbo.tbl_MRP_List_CAPEX.ApprovedQty, dbo.tbl_MRP_List_CAPEX.ApprovedCost, dbo.tbl_MRP_List_CAPEX.ApprovedTotalCost, dbo.tbl_MRP_List_CAPEX.QtyPO, dbo.tbl_MRP_List_CAPEX.AvailForPO FROM  dbo.tbl_MRP_List_CAPEX LEFT OUTER JOIN dbo.vw_AXFindimBananaRevenue ON dbo.tbl_MRP_List_CAPEX.OprUnit = dbo.vw_AXFindimBananaRevenue.VALUE LEFT OUTER JOIN dbo.vw_AXProdCategory ON dbo.tbl_MRP_List_CAPEX.ProdCat = dbo.vw_AXProdCategory.NAME WHERE(dbo.tbl_MRP_List_CAPEX.HeaderDocNum = '" + DOC_NUMBER + "')";

            //string query_2 = "SELECT DISTINCT dbo.vw_AXProdCategory.DESCRIPTION AS ProdDesc, dbo.tbl_MRP_List_CAPEX.* FROM dbo.tbl_MRP_List_CAPEX LEFT OUTER JOIN dbo.vw_AXProdCategory ON dbo.tbl_MRP_List_CAPEX.ProdCat = dbo.vw_AXProdCategory.NAME WHERE [HeaderDocNum] = '" + DOC_NUMBER + "'";

            string query_2 = "SELECT DISTINCT dbo.vw_AXProdCategory.DESCRIPTION AS ProdDesc, dbo.tbl_MRP_List_CAPEX.PK, dbo.tbl_MRP_List_CAPEX.HeaderDocNum, dbo.tbl_MRP_List_CAPEX.TableIdentifier, dbo.tbl_MRP_List_CAPEX.OprUnit, dbo.tbl_MRP_List_CAPEX.ProdCat, dbo.tbl_MRP_List_CAPEX.Description, dbo.tbl_MRP_List_CAPEX.UOM, dbo.tbl_MRP_List_CAPEX.Cost, dbo.tbl_MRP_List_CAPEX.Qty, dbo.tbl_MRP_List_CAPEX.TotalCost, dbo.tbl_MRP_List_CAPEX.CIPSIPNumber, dbo.tbl_MRP_List_CAPEX.EdittedQty, dbo.tbl_MRP_List_CAPEX.EdittedCost, dbo.tbl_MRP_List_CAPEX.EdittiedTotalCost, dbo.tbl_MRP_List_CAPEX.ApprovedQty, dbo.tbl_MRP_List_CAPEX.ApprovedCost, dbo.tbl_MRP_List_CAPEX.ApprovedTotalCost, dbo.tbl_MRP_List_CAPEX.QtyPO, dbo.tbl_MRP_List_CAPEX.AvailForPO FROM  dbo.tbl_MRP_List_CAPEX LEFT OUTER JOIN dbo.vw_AXProdCategory ON dbo.tbl_MRP_List_CAPEX.ProdCat = dbo.vw_AXProdCategory.NAME WHERE(dbo.tbl_MRP_List_CAPEX.HeaderDocNum = '" + DOC_NUMBER + "')";

            bool exec = entitycode == train_entity;
            if (exec)
                cmd = new SqlCommand(query_1);
            else
                cmd = new SqlCommand(query_2);

            cmd.Connection = cn;
            adp = new SqlDataAdapter(cmd);
            adp.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    DataRow dtRow = dtTable.NewRow();
                    dtRow["PK"] = row["PK"].ToString();
                    dtRow["HeaderDocNum"] = row["HeaderDocNum"].ToString();
                    dtRow["ProdDesc"] = row["ProdDesc"].ToString();
                    dtRow["Description"] = row["Description"].ToString();
                    dtRow["UOM"] = row["UOM"].ToString();
                    dtRow["Cost"] = Convert.ToDouble(row["Cost"]).ToString("N");
                    dtRow["Qty"] = Convert.ToDouble(row["Qty"]);
                    dtRow["TotalCost"] = Convert.ToDouble(row["TotalCost"]).ToString("N");
                    dtRow["EdittedCost"] = Convert.ToDouble(row["EdittedCost"]).ToString("N");
                    dtRow["EdittedQty"] = Convert.ToDouble(row["EdittedQty"]);
                    dtRow["EdittiedTotalCost"] = Convert.ToDouble(row["EdittiedTotalCost"]).ToString("N");

                    if (exec)
                        dtRow["RevDesc"] = row["RevDesc"].ToString();
                    else
                        dtRow["RevDesc"] = "";

                    dtTable.Rows.Add(dtRow);

                    //for Preview Page
                    capex_total_amount += Convert.ToDouble(row["TotalCost"]);

                }
            }
            dt.Clear();
            cn.Close();
            return dtTable;
        }

        public static DataTable MRPApproval_CAPEX(string DOC_NUMBER, string entitycode)
        {
            DataTable dtTable = new DataTable();
            SqlConnection cn = new SqlConnection(GlobalClass.SQLConnString());
            DataTable dt = new DataTable();
            SqlCommand cmd = null;
            SqlDataAdapter adp;
            capex_total_amount = 0;
            ca_edited_total = 0;
            ca_approved_total = 0;

            cn.Open();
            if (dtTable.Columns.Count == 0)
            {
                //Columns for AspxGridview
                dtTable.Columns.Add("PK", typeof(string));
                dtTable.Columns.Add("HeaderDocNum", typeof(string));
                dtTable.Columns.Add("Description", typeof(string));
                dtTable.Columns.Add("UOM", typeof(string));
                dtTable.Columns.Add("Cost", typeof(string));
                dtTable.Columns.Add("Qty", typeof(Double));
                dtTable.Columns.Add("TotalCost", typeof(string));
                dtTable.Columns.Add("EdittedCost", typeof(string));
                dtTable.Columns.Add("EdittedQty", typeof(Double));
                dtTable.Columns.Add("EdittiedTotalCost", typeof(string));
                dtTable.Columns.Add("ApprovedCost", typeof(string));
                dtTable.Columns.Add("ApprovedQty", typeof(Double));
                dtTable.Columns.Add("ApprovedTotalCost", typeof(string));
                dtTable.Columns.Add("RevDesc", typeof(string));
            }

            //string query_1 = "SELECT tbl_MRP_List_CAPEX.*, vw_AXFindimBananaRevenue.DESCRIPTION AS RevDesc FROM tbl_MRP_List_CAPEX LEFT OUTER JOIN vw_AXFindimBananaRevenue ON tbl_MRP_List_CAPEX.OprUnit = vw_AXFindimBananaRevenue.VALUE WHERE [HeaderDocNum] = '" + DOC_NUMBER + "'";
            string query_1 = "SELECT tbl_MRP_List_CAPEX.*, vw_AXFindimBananaRevenue.DESCRIPTION AS RevDesc FROM tbl_MRP_List_CAPEX LEFT OUTER JOIN vw_AXFindimBananaRevenue ON tbl_MRP_List_CAPEX.OprUnit = vw_AXFindimBananaRevenue.VALUE WHERE [HeaderDocNum] = '" + DOC_NUMBER + "'";
            
            string query_2 = "SELECT * FROM [dbo].[tbl_MRP_List_CAPEX] WHERE [HeaderDocNum] = '" + DOC_NUMBER + "'";

            bool exec = entitycode == train_entity;
            if (exec)
                cmd = new SqlCommand(query_1);
            else
                cmd = new SqlCommand(query_2);

            cmd.Connection = cn;
            adp = new SqlDataAdapter(cmd);
            adp.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    DataRow dtRow = dtTable.NewRow();
                    dtRow["PK"] = row["PK"].ToString();
                    dtRow["HeaderDocNum"] = row["HeaderDocNum"].ToString();
                    dtRow["Description"] = row["Description"].ToString();
                    dtRow["UOM"] = row["UOM"].ToString();
                    dtRow["Cost"] = Convert.ToDouble(row["Cost"]).ToString("N");
                    dtRow["Qty"] = Convert.ToDouble(row["Qty"]);
                    dtRow["TotalCost"] = Convert.ToDouble(row["TotalCost"]).ToString("N");
                    dtRow["EdittedCost"] = Convert.ToDouble(row["EdittedCost"]).ToString("N");
                    dtRow["EdittedQty"] = Convert.ToDouble(row["EdittedQty"]);
                    dtRow["EdittiedTotalCost"] = Convert.ToDouble(row["EdittiedTotalCost"]).ToString("N");
                    dtRow["ApprovedQty"] = Convert.ToDouble(row["ApprovedQty"]);
                    dtRow["ApprovedCost"] = Convert.ToDouble(row["ApprovedCost"]).ToString("N");
                    dtRow["ApprovedTotalCost"] = Convert.ToDouble(row["ApprovedTotalCost"]).ToString("N");

                    if (exec)
                        dtRow["RevDesc"] = row["RevDesc"].ToString();
                    else
                        dtRow["RevDesc"] = "";

                    dtTable.Rows.Add(dtRow);

                    //for Preview Page
                    capex_total_amount += Convert.ToDouble(row["TotalCost"]);
                    ca_edited_total += Convert.ToDouble(row["EdittiedTotalCost"]);
                    ca_approved_total += Convert.ToDouble(row["ApprovedTotalCost"]);

                }
            }
            dt.Clear();
            cn.Close();
            return dtTable;
        }


        public static DataTable PO_Table()
        {
            DataTable dtTable = new DataTable();
            SqlConnection cn = new SqlConnection(GlobalClass.SQLConnString());
            DataTable dt = new DataTable();
            SqlCommand cmd = null;
            SqlDataAdapter adp;
            capex_total_amount = 0;

            cn.Open();
            if (dtTable.Columns.Count == 0)
            {
                //Columns for AspxGridview
                dtTable.Columns.Add("PK", typeof(string));
                dtTable.Columns.Add("PONumber", typeof(string));
                dtTable.Columns.Add("MRPNumber", typeof(string));
                dtTable.Columns.Add("DateCreated", typeof(string));
                dtTable.Columns.Add("CreatorKey", typeof(string));
                dtTable.Columns.Add("CreatorName", typeof(string));
                dtTable.Columns.Add("ExpectedDate", typeof(string));
                dtTable.Columns.Add("VendorCode", typeof(string));
            }

            //string query = "SELECT * FROM [dbo].[tbl_POCreation]";
            string query = "SELECT tbl_POCreation.PK, tbl_POCreation.PONumber, tbl_POCreation.MRPNumber, tbl_POCreation.DateCreated, tbl_POCreation.CreatorKey, tbl_POCreation.ExpectedDate, tbl_POCreation.VendorCode, tbl_Users.Firstname, tbl_Users.Lastname FROM tbl_POCreation LEFT OUTER JOIN tbl_Users ON tbl_POCreation.CreatorKey = tbl_Users.PK";

            cmd = new SqlCommand(query);
            cmd.Connection = cn;
            adp = new SqlDataAdapter(cmd);
            adp.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    DataRow dtRow = dtTable.NewRow();
                    dtRow["PK"] = row["PK"].ToString();
                    dtRow["PONumber"] = row["PONumber"].ToString();
                    dtRow["MRPNumber"] = row["MRPNumber"].ToString();
                    dtRow["DateCreated"] = Convert.ToDateTime(row["DateCreated"]).ToString("MM/dd/yyyy");
                    dtRow["CreatorKey"] = row["CreatorKey"].ToString();
                    dtRow["CreatorName"] = EncryptionClass.Decrypt(row["Firstname"].ToString()) + " " + EncryptionClass.Decrypt(row["Lastname"].ToString());
                    //dtRow["ExpectedDate"] = row["ExpectedDate"].ToString();

                    if (!DBNull.Value.Equals(row["ExpectedDate"]))
                    {
                        dtRow["ExpectedDate"] = Convert.ToDateTime(row["ExpectedDate"]).ToString("MM/dd/yyyy");
                    }
                    else
                    {
                        dtRow["ExpectedDate"] = "";
                    }
                    dtRow["VendorCode"] = row["VendorCode"].ToString();
                    dtTable.Rows.Add(dtRow);
                }
            }
            dt.Clear();
            cn.Close();
            return dtTable;
        }


        public static DataTable MRP_List()
        {
            DataTable dtTable = new DataTable();
            SqlConnection cn = new SqlConnection(GlobalClass.SQLConnString());
            DataTable dt = new DataTable();
            SqlCommand cmd = null;
            SqlDataAdapter adp;

            cn.Open();

            if (dtTable.Columns.Count == 0)
            {
                //Columns for AspxGridview
                dtTable.Columns.Add("PK", typeof(string));
                dtTable.Columns.Add("DocNumber", typeof(string));
                dtTable.Columns.Add("DateCreated", typeof(string));
                dtTable.Columns.Add("MRPType", typeof(string));
                dtTable.Columns.Add("MRPTypeDesc", typeof(string));
                dtTable.Columns.Add("MRPMonth", typeof(Int32));
                dtTable.Columns.Add("MRPMonthDesc", typeof(string));
                dtTable.Columns.Add("MRPYear", typeof(Int32));
                dtTable.Columns.Add("StatusKey", typeof(string));
                dtTable.Columns.Add("StatusKeyDesc", typeof(string));
                dtTable.Columns.Add("EntityCode", typeof(string));
                dtTable.Columns.Add("BUSSUCode", typeof(string));
                dtTable.Columns.Add("CreatorKey", typeof(Int32));
                dtTable.Columns.Add("LastModified", typeof(string));
            }

            string qry = "SELECT dbo.tbl_MRP.PK, dbo.tbl_MRP.DocNumber, dbo.tbl_MRP.DateCreated, dbo.tbl_MRP.MRPType, " +
                         " dbo.tbl_MRP_Type.MRPType AS MRPTypeDesc, dbo.tbl_MRP.MRPMonth, dbo.tbl_MRP.MRPYear, " +
                         " dbo.tbl_MRP.StatusKey, " +
                         " dbo.tbl_MRP_Status.StatusName, dbo.tbl_MRP.LastModified, dbo.tbl_MRP.EntityCode, " +
                         " dbo.tbl_MRP.BUSSUCode, dbo.tbl_MRP.CreatorKey " +
                         " FROM   dbo.tbl_MRP LEFT OUTER JOIN " +
                         " dbo.tbl_MRP_Type ON dbo.tbl_MRP.MRPType = dbo.tbl_MRP_Type.PK LEFT OUTER JOIN " +
                         " dbo.tbl_MRP_Status ON dbo.tbl_MRP.StatusKey = dbo.tbl_MRP_Status.PK LEFT OUTER JOIN " +
                         " dbo.tbl_Users ON dbo.tbl_MRP.CreatorKey = dbo.tbl_Users.PK";

            cmd = new SqlCommand(qry);
            cmd.Connection = cn;
            adp = new SqlDataAdapter(cmd);
            adp.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    DataRow dtRow = dtTable.NewRow();
                    dtRow["PK"] = row["PK"].ToString();
                    dtRow["DocNumber"] = row["DocNumber"].ToString();
                    dtRow["DateCreated"] = Convert.ToDateTime(row["DateCreated"]).ToString("MM/dd/yyyy");
                    dtRow["MRPType"] = row["MRPType"].ToString();
                    dtRow["MRPTypeDesc"] = row["MRPTypeDesc"].ToString();
                    dtRow["MRPMonth"] = Convert.ToInt32(row["MRPMonth"]);
                    dtRow["MRPMonthDesc"] = Month_Name(Convert.ToInt32(row["MRPMonth"]));
                    dtRow["MRPYear"] = Convert.ToInt32(row["MRPYear"]);
                    dtRow["StatusKey"] = row["StatusKey"].ToString();
                    dtRow["StatusKeyDesc"] = row["StatusName"].ToString();
                    dtRow["EntityCode"] = row["EntityCode"].ToString();
                    dtRow["BUSSUCode"] = row["BUSSUCode"].ToString();
                    dtRow["CreatorKey"] = Convert.ToInt32(row["CreatorKey"]);
                    dtRow["LastModified"] = row["LastModified"].ToString();
                    dtTable.Rows.Add(dtRow);
                }
            }
            dt.Clear();
            cn.Close();
            //DataTable table = new DataTable("Players");
            //table.Columns.Add(new DataColumn("Size", typeof(int)));
            //table.Columns.Add(new DataColumn("Sex", typeof(char)));

            return dtTable;
        }

        //public static DataTable AXInventTable(string str, string EntCode, string mainaccoundid)
        public static DataTable AXInventTable(string str, string EntCode)
        {

            DataTable dtTable = new DataTable();

            if (string.IsNullOrEmpty(str)) return dtTable;

            SqlConnection cn = new SqlConnection(GlobalClass.SQLConnString());
            DataTable dt = new DataTable();
            SqlCommand cmd = null;
            SqlDataAdapter adp;

            DataTable dt1 = new DataTable();
            SqlCommand cmd1 = null;
            SqlDataAdapter adp1;

            cn.Open();


            if (dtTable.Columns.Count == 0)
            {
                //Columns for AspxGridview
                dtTable.Columns.Add("ITEMID", typeof(string));
                dtTable.Columns.Add("NAMEALIAS", typeof(string));
                dtTable.Columns.Add("UOM", typeof(string));
                dtTable.Columns.Add("LastCost", typeof(string));
            }

            //string MAINACCOUNT = "";
            //if (mainaccoundid == "empty")
            //    MAINACCOUNT = "MAINACCOUNT";
            //else
            //    MAINACCOUNT = "'" +mainaccoundid + "'";

            //string qry = "SELECT [ITEMID],[NAMEALIAS], [UNITID] " +
            //              " FROM [dbo].[vw_AXInventTable] " +
            //              " WHERE [NAMEALIAS] LIKE '%" + str + "%'" +
            //              " AND DATAAREAID = '" + EntCode + "' AND MAINACCOUNT = " + MAINACCOUNT;

            string qry = "SELECT DISTINCT [ITEMID],[NAMEALIAS], [UNITID] " +
                          " FROM [dbo].[vw_AXInventTable] " +
                          " WHERE [NAMEALIAS] LIKE '%" + str + "%' AND DATAAREAID = '" + EntCode + "'";

            cmd = new SqlCommand(qry);
            cmd.Connection = cn;
            adp = new SqlDataAdapter(cmd);
            adp.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    DataRow dtRow = dtTable.NewRow();
                    dtRow["ITEMID"] = row["ITEMID"].ToString();
                    dtRow["NAMEALIAS"] = row["NAMEALIAS"].ToString();
                    dtRow["UOM"] = row["UNITID"].ToString();

                    string qry1 = "SELECT lastprice " +
                                  " FROM dbo.vw_AXInventTablePrice " +
                                  " WHERE(DATAAREAID = '" + EntCode + "') " +
                                  " AND(ITEMID = '" + row["ITEMID"].ToString() + "')";
                    cmd1 = new SqlCommand(qry1);
                    cmd1.Connection = cn;
                    adp1 = new SqlDataAdapter(cmd1);
                    adp1.Fill(dt1);
                    if (dt1.Rows.Count > 0)
                    {
                        foreach (DataRow row1 in dt1.Rows)
                        {
                            dtRow["LastCost"] = Convert.ToDouble(row1["lastprice"]).ToString("#,##0.00");
                        }
                    }
                    else
                    {
                        dtRow["LastCost"] = "0";
                    }
                    dt1.Clear();
                    dtTable.Rows.Add(dtRow);
                }
            }
            dt.Clear();
            cn.Close();

            return dtTable;
        }

        public static DataTable ExpenseCodeTable(int isOPEX)
        {

            DataTable dtTable = new DataTable();

            SqlConnection cn = new SqlConnection(GlobalClass.SQLConnString());
            DataTable dt = new DataTable();
            SqlCommand cmd = null;
            SqlDataAdapter adp;

            cn.Open();

            if (dtTable.Columns.Count == 0)
            {
                //Columns for AspxGridview
                dtTable.Columns.Add("MAINACCOUNTID", typeof(string));
                dtTable.Columns.Add("NAME", typeof(string));
                dtTable.Columns.Add("isItem", typeof(int));
                dtTable.Columns.Add("isProdCategory", typeof(int));
            }

            string qry = "SELECT * FROM [dbo].[vw_AXExpenseAccount] WHERE isOPEX = '" + isOPEX + "'";

            cmd = new SqlCommand(qry);
            cmd.Connection = cn;
            adp = new SqlDataAdapter(cmd);
            adp.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    DataRow dtRow = dtTable.NewRow();
                    dtRow["MAINACCOUNTID"] = row["MAINACCOUNTID"].ToString();
                    dtRow["NAME"] = row["NAME"].ToString();
                    dtRow["isItem"] = Convert.ToInt32(row["isItem"].ToString());
                    dtRow["isProdCategory"] = Convert.ToInt32(row["isProdCategory"].ToString());
                    dtTable.Rows.Add(dtRow);
                }
            }
            dt.Clear();
            cn.Close();

            return dtTable;
        }

        public static DataTable ActivityCodeTable()
        {

            DataTable dtTable = new DataTable();

            SqlConnection cn = new SqlConnection(GlobalClass.SQLConnString());
            DataTable dt = new DataTable();
            SqlCommand cmd = null;
            SqlDataAdapter adp;

            cn.Open();

            if (dtTable.Columns.Count == 0)
            {
                //Columns for AspxGridview
                dtTable.Columns.Add("VALUE", typeof(string));
                dtTable.Columns.Add("DESCRIPTION", typeof(string));
            }

            string qry = "SELECT * FROM [dbo].[vw_AXFindimActivity]";

            cmd = new SqlCommand(qry);
            cmd.Connection = cn;
            adp = new SqlDataAdapter(cmd);
            adp.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    DataRow dtRow = dtTable.NewRow();
                    dtRow["VALUE"] = row["VALUE"].ToString();
                    dtRow["DESCRIPTION"] = row["DESCRIPTION"].ToString();
                    dtTable.Rows.Add(dtRow);
                }
            }
            dt.Clear();
            cn.Close();

            return dtTable;
        }

        public static DataTable UOMTable()
        {
            DataTable dtTable = new DataTable();

            SqlConnection cn = new SqlConnection(GlobalClass.SQLConnString());
            DataTable dt = new DataTable();
            SqlCommand cmd = null;
            SqlDataAdapter adp;

            cn.Open();

            if (dtTable.Columns.Count == 0)
            {
                //Columns for AspxGridview
                dtTable.Columns.Add("SYMBOL", typeof(string));
                dtTable.Columns.Add("description", typeof(string));
            }

            string qry = "SELECT * FROM [dbo].[vw_AXUnitOfMeasure]";

            cmd = new SqlCommand(qry);
            cmd.Connection = cn;
            adp = new SqlDataAdapter(cmd);
            adp.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    DataRow dtRow = dtTable.NewRow();
                    dtRow["SYMBOL"] = row["SYMBOL"].ToString();
                    dtRow["description"] = row["description"].ToString();
                    dtTable.Rows.Add(dtRow);
                }
            }
            dt.Clear();
            cn.Close();

            return dtTable;
        }

        public static string ActivityCodeDESCRIPTION(string code)
        {
            string query = "SELECT VALUE FROM [dbo].[vw_AXFindimActivity] where DESCRIPTION = '" + code + "'";

            string actcodeVal = "";
            SqlConnection conn = new SqlConnection(GlobalClass.SQLConnString());
            conn.Open();
            SqlCommand queryCMD = new SqlCommand(query, conn);
            SqlDataReader reader = queryCMD.ExecuteReader();
            while (reader.Read())
            {
                actcodeVal = reader[0].ToString();
            }
            reader.Close();
            conn.Close();
            return actcodeVal;
        }

        public static DataTable MRPItems(string DocNumber, string mrp_type)
        {
            DataTable dtTable = new DataTable();
            SqlConnection conn = new SqlConnection(GlobalClass.SQLConnString());
            DataTable dt = new DataTable();
            SqlCommand cmd = null;
            SqlDataAdapter adp;

            conn.Open();

            if (dtTable.Columns.Count == 0)
            {
                dtTable.Columns.Add("PK", typeof(Int32));
                dtTable.Columns.Add("ExpenseCode", typeof(string));
                dtTable.Columns.Add("ActivityCode", typeof(string));
                dtTable.Columns.Add("ItemCode", typeof(string));
                dtTable.Columns.Add("ManPowerTypeKey", typeof(string));
                dtTable.Columns.Add("ManPowerTypeDesc", typeof(string));
                dtTable.Columns.Add("ItemDescription", typeof(string));
                dtTable.Columns.Add("UOM", typeof(string));
                dtTable.Columns.Add("ItemQty", typeof(float));
                dtTable.Columns.Add("HeadCount", typeof(Int32));
                dtTable.Columns.Add("ItemCost", typeof(float));
                dtTable.Columns.Add("ItemTotalCost", typeof(float));
                dtTable.Columns.Add("Purpose", typeof(string));
                dtTable.Columns.Add("DateRequired", typeof(DateTime));
            }

            string query = "";

            if (mrp_type == "MANPOWER")
            {
                query = "SELECT tbl_MRP_Items.PK, tbl_MRP_Items.HeaderDocNum, tbl_MRP_Items.ExpenseCode, tbl_MRP_Items.ActivityCode, tbl_MRP_Items.ItemCode, tbl_MRP_Items.ItemDescription, tbl_MRP_Items.UOM, tbl_MRP_Items.ItemQty, tbl_MRP_Items.ItemCost, tbl_MRP_Items.ItemTotalCost, tbl_MRP_Items.DateRequired, tbl_MRP_Items.HeadCount, tbl_MRP_Items.Purpose, tbl_System_ManPowerType.ManPowerTypeDesc, tbl_System_ManPowerType.PK AS ManPowerTypeDesc, tbl_MRP_Items.ManPowerTypeKey " +
                              "FROM   tbl_MRP_Items LEFT OUTER JOIN tbl_System_ManPowerType ON tbl_MRP_Items.ManPowerTypeKey = tbl_System_ManPowerType.PK WHERE [HeaderDocNum] = '" + DocNumber + "'";
            }
            else
            {
                query = "SELECT * FROM [dbo].[tbl_MRP_Items] WHERE [HeaderDocNum] = '" + DocNumber + "'";
            }

            cmd = new SqlCommand(query);
            cmd.Connection = conn;
            adp = new SqlDataAdapter(cmd);
            adp.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    DataRow dtRow = dtTable.NewRow();
                    dtRow["PK"] = row["PK"].ToString();
                    dtRow["ExpenseCode"] = row["ExpenseCode"].ToString();
                    dtRow["ActivityCode"] = row["ActivityCode"].ToString();
                    dtRow["ItemCode"] = row["ItemCode"].ToString();
                    dtRow["ManPowerTypeKey"] = row["ManPowerTypeKey"].ToString();
                    if (mrp_type == "MANPOWER")
                        dtRow["ManPowerTypeDesc"] = row["ManPowerTypeDesc"].ToString();
                    dtRow["ItemDescription"] = row["ItemDescription"].ToString();
                    dtRow["UOM"] = row["UOM"].ToString();
                    dtRow["ItemQty"] = Convert.ToDouble(row["ItemQty"]);
                    dtRow["HeadCount"] = Convert.ToInt32(row["HeadCount"]);
                    dtRow["ItemCost"] = Convert.ToDouble(row["ItemCost"]);
                    dtRow["ItemTotalCost"] = Convert.ToDouble(row["ItemTotalCost"]);
                    dtRow["Purpose"] = row["Purpose"].ToString();
                    dtRow["DateRequired"] = Convert.ToDateTime(row["DateRequired"]);
                    dtTable.Rows.Add(dtRow);
                }
            }
            dt.Clear();
            conn.Close();

            return dtTable;
        }

        public static DataTable RevAssumpTable()
        {
            DataTable dtTable = new DataTable();
            SqlConnection conn = new SqlConnection(GlobalClass.SQLConnString());
            DataTable dt = new DataTable();
            SqlCommand cmd = null;
            SqlDataAdapter adp;

            conn.Open();

            if (dtTable.Columns.Count == 0)
            {
                dtTable.Columns.Add("PK", typeof(Int32));
                dtTable.Columns.Add("Ctrl", typeof(string));
                dtTable.Columns.Add("MRPMonth", typeof(Int32));
                dtTable.Columns.Add("MRPMonthName", typeof(string));
                dtTable.Columns.Add("MRPYear", typeof(Int32));
                dtTable.Columns.Add("LastModified", typeof(DateTime));
            }

            string query = "SELECT * FROM [dbo].[tbl_MRP_RevenueAssumptions]";
            cmd = new SqlCommand(query);
            cmd.Connection = conn;
            adp = new SqlDataAdapter(cmd);
            adp.Fill(dt);

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    DataRow dtRow = dtTable.NewRow();
                    dtRow["PK"] = row["PK"].ToString();
                    dtRow["Ctrl"] = row["Ctrl"].ToString();
                    dtRow["MRPMonth"] = Convert.ToInt32(row["MRPMonth"]);
                    dtRow["MRPMonthName"] = Month_Name(Convert.ToInt32(row["MRPMonth"]));
                    dtRow["MRPYear"] = Convert.ToInt32(row["MRPYear"]);
                    dtRow["LastModified"] = Convert.ToDateTime(row["LastModified"]).ToString("MM/dd/yyyy hh:mm:ss tt");
                    dtTable.Rows.Add(dtRow);

                }
            }
            dt.Clear();
            conn.Close();

            return dtTable;
        }

        public static DataTable RevDetailsTable(int pk)
        {
            DataTable dtTable = new DataTable();
            SqlConnection conn = new SqlConnection(GlobalClass.SQLConnString());
            DataTable dt = new DataTable();
            SqlCommand cmd = null;
            SqlDataAdapter adp;

            conn.Open();

            if (dtTable.Columns.Count == 0)
            {
                dtTable.Columns.Add("MasterKey", typeof(Int32));
                dtTable.Columns.Add("Line", typeof(Int32));
                dtTable.Columns.Add("Product", typeof(string));
                dtTable.Columns.Add("Farm", typeof(string));
                dtTable.Columns.Add("Volume", typeof(Double));
                dtTable.Columns.Add("Price", typeof(Double));
                dtTable.Columns.Add("Amount", typeof(Double));
            }

            string query = "SELECT * FROM [dbo].[tbl_MRP_RevenueAssumptions_Details] WHERE [MasterKey] = '" + pk + "'";
            cmd = new SqlCommand(query);
            cmd.Connection = conn;
            adp = new SqlDataAdapter(cmd);
            adp.Fill(dt);

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    DataRow dtRow = dtTable.NewRow();
                    dtRow["MasterKey"] = row["MasterKey"].ToString();
                    dtRow["Line"] = Convert.ToInt32(row["Line"]);
                    dtRow["Product"] = row["Product"].ToString();
                    dtRow["Farm"] = row["Farm"].ToString();
                    dtRow["Volume"] = Convert.ToDouble(row["Volume"]);
                    dtRow["Price"] = Convert.ToDouble(row["Price"]);
                    dtRow["Amount"] = Convert.ToDouble(row["Amount"]);
                    dtTable.Rows.Add(dtRow);
                }
            }
            dt.Clear();
            conn.Close();

            return dtTable;
        }

        public static DataTable POAddEdit_Table(string mrp_number, string type)
        {

            if (type != "ITEMGROUPID")
            {
                type = "'" + type + "'";
            }
            DataTable dtTable = new DataTable();
            SqlConnection cn = new SqlConnection(GlobalClass.SQLConnString());
            DataTable dt = new DataTable();
            SqlCommand cmd = null;
            SqlDataAdapter adp;
            capex_total_amount = 0;

            cn.Open();
            if (dtTable.Columns.Count == 0)
            {
                //Columns for AspxGridview
                dtTable.Columns.Add("PK", typeof(string));
                dtTable.Columns.Add("TableIdentifier", typeof(string));
                dtTable.Columns.Add("MRPCategory", typeof(string));
                dtTable.Columns.Add("Item", typeof(string));
                dtTable.Columns.Add("Qty", typeof(Double));
                dtTable.Columns.Add("UOM", typeof(string));
                dtTable.Columns.Add("Cost", typeof(string));
                dtTable.Columns.Add("TotalCost", typeof(string));
                dtTable.Columns.Add("POQty", typeof(Double));
                dtTable.Columns.Add("POCost", typeof(string));
                dtTable.Columns.Add("POTotalCost", typeof(string));
                dtTable.Columns.Add("TaxGroup", typeof(string));
                dtTable.Columns.Add("TaxItemGroup", typeof(string));
            }

            //string query = "SELECT * FROM [dbo].[tbl_POCreation]";
            //string query = "SELECT tbl_POCreation.PK, tbl_POCreation.PONumber, tbl_POCreation.MRPNumber, tbl_POCreation.DateCreated, tbl_POCreation.CreatorKey, tbl_POCreation.ExpectedDate, tbl_POCreation.VendorCode, tbl_Users.Firstname, tbl_Users.Lastname FROM tbl_POCreation LEFT OUTER JOIN tbl_Users ON tbl_POCreation.CreatorKey = tbl_Users.PK";

            string query = "SELECT tbl_MRP_List_DirectMaterials.PK, tbl_MRP_List_DirectMaterials.TableIdentifier, tbl_MRP_List_DirectMaterials.ItemCode, tbl_MRP_List_DirectMaterials.ItemDescription, tbl_MRP_List_DirectMaterials.Qty, tbl_MRP_List_DirectMaterials.UOM, tbl_MRP_List_DirectMaterials.Cost, tbl_MRP_List_DirectMaterials.TotalCost, vw_AXInventTable.ITEMGROUPID FROM tbl_MRP_List_DirectMaterials LEFT OUTER JOIN vw_AXInventTable ON tbl_MRP_List_DirectMaterials.ItemCode = vw_AXInventTable.ITEMID " +
                "WHERE tbl_MRP_List_DirectMaterials.HeaderDocNum = '" + mrp_number + "' AND ITEMGROUPID = " + type + " AND tbl_MRP_List_DirectMaterials.QtyPO = '0' " +
                "UNION SELECT tbl_MRP_List_OPEX.PK, tbl_MRP_List_OPEX.TableIdentifier, tbl_MRP_List_OPEX.ItemCode, tbl_MRP_List_OPEX.Description, tbl_MRP_List_OPEX.Qty, tbl_MRP_List_OPEX.UOM, tbl_MRP_List_OPEX.Cost, tbl_MRP_List_OPEX.TotalCost, vw_AXInventTable.ITEMGROUPID FROM tbl_MRP_List_OPEX LEFT OUTER JOIN vw_AXInventTable ON tbl_MRP_List_OPEX.ItemCode = vw_AXInventTable.ITEMID " +
                "WHERE tbl_MRP_List_OPEX.HeaderDocNum = '" + mrp_number + "' AND ITEMGROUPID = " + type + " AND tbl_MRP_List_OPEX.QtyPO = '0'";
            cmd = new SqlCommand(query);
            cmd.Connection = cn;
            adp = new SqlDataAdapter(cmd);
            adp.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    DataRow dtRow = dtTable.NewRow();
                    dtRow["PK"] = row["PK"].ToString();
                    dtRow["TableIdentifier"] = row["TableIdentifier"].ToString();
                    dtRow["MRPCategory"] = row["ITEMGROUPID"].ToString();
                    dtRow["Item"] = row["ItemCode"].ToString() + "/" + row["ItemDescription"].ToString();
                    dtRow["Qty"] = row["Qty"].ToString();
                    dtRow["UOM"] = row["UOM"].ToString();
                    dtRow["Cost"] = Convert.ToDouble(row["Cost"]).ToString("N");
                    dtRow["TotalCost"] = Convert.ToDouble(row["TotalCost"]).ToString("N");
                    dtRow["POQty"] = row["Qty"].ToString();
                    dtRow["POCost"] = Convert.ToDouble(row["Cost"]).ToString("N");
                    dtRow["POTotalCost"] = Convert.ToDouble(row["TotalCost"]).ToString("N");
                    dtRow["TaxGroup"] = "";
                    dtRow["TaxItemGroup"] = "";
                    dtTable.Rows.Add(dtRow);
                }
            }
            dt.Clear();
            cn.Close();
            return dtTable;
        }

        public static DataTable ProCategoryTable()
        {

            DataTable dtTable = new DataTable();

            SqlConnection cn = new SqlConnection(GlobalClass.SQLConnString());
            DataTable dt = new DataTable();
            SqlCommand cmd = null;
            SqlDataAdapter adp;

            cn.Open();

            if (dtTable.Columns.Count == 0)
            {
                //Columns for AspxGridview
                dtTable.Columns.Add("NAME", typeof(string));
                dtTable.Columns.Add("DESCRIPTION", typeof(string));
            }

            string qry = "SELECT [NAME],[DESCRIPTION] FROM [dbo].[vw_AXProdCategory] ORDER BY NAME ASC";

            cmd = new SqlCommand(qry);
            cmd.Connection = cn;
            adp = new SqlDataAdapter(cmd);
            adp.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                dtTable.Rows.Add("ALL", "ALL");
                foreach (DataRow row in dt.Rows)
                {
                    DataRow dtRow = dtTable.NewRow();
                    dtRow["NAME"] = row["NAME"].ToString();
                    dtRow["DESCRIPTION"] = row["DESCRIPTION"].ToString();
                    dtTable.Rows.Add(dtRow);
                }
            }
            dt.Clear();
            cn.Close();

            return dtTable;
        }

        public static DataTable ProCategoryTable_WithoutAll()
        {

            DataTable dtTable = new DataTable();

            SqlConnection cn = new SqlConnection(GlobalClass.SQLConnString());
            DataTable dt = new DataTable();
            SqlCommand cmd = null;
            SqlDataAdapter adp;

            cn.Open();

            if (dtTable.Columns.Count == 0)
            {
                //Columns for AspxGridview
                dtTable.Columns.Add("NAME", typeof(string));
                dtTable.Columns.Add("DESCRIPTION", typeof(string));
            }

            string qry = "SELECT DISTINCT [NAME],[DESCRIPTION] FROM [dbo].[vw_AXProdCategory] ORDER BY NAME ASC";

            cmd = new SqlCommand(qry);
            cmd.Connection = cn;
            adp = new SqlDataAdapter(cmd);
            adp.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    DataRow dtRow = dtTable.NewRow();
                    dtRow["NAME"] = row["NAME"].ToString();
                    dtRow["DESCRIPTION"] = row["DESCRIPTION"].ToString();
                    dtTable.Rows.Add(dtRow);
                }
            }
            dt.Clear();
            cn.Close();

            return dtTable;
        }

        public static DataTable ProCategoryTable_Filter_SelectItemPO (int Year, int Month, string docnum)
        {
            DataTable dtTable = new DataTable();
            SqlConnection cn = new SqlConnection(GlobalClass.SQLConnString());
            DataTable dt = new DataTable();
            SqlCommand cmd = null;
            SqlDataAdapter adp;
            string query = "";
            cn.Open();

            if (dtTable.Columns.Count == 0)
            {
                //Columns for AspxGridview
                dtTable.Columns.Add("NAME", typeof(string));
                dtTable.Columns.Add("DESCRIPTION", typeof(string));
            }

            query = "sp_SelectItemforPO_ProCat " + Year + ", " + Month + ", '" + docnum + "'";
            cmd = new SqlCommand(query);
            cmd.Connection = cn;
            adp = new SqlDataAdapter(cmd);
            adp.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    DataRow dtRow = dtTable.NewRow();
                    dtRow["NAME"] = row["ItemCatCode"].ToString();
                    dtRow["DESCRIPTION"] = row["ItemCat"].ToString();
                    dtTable.Rows.Add(dtRow);
                }
            }
            dt.Clear();

            cn.Close();

            //DataTable uniqdtTable = RemoveDuplicateRows(dtTable, "NAME", "DESCRIPTION");

            return dtTable;
            //return uniqdtTable;
        }

        public static DataTable ProCategoryTable_Filter(string docnum, string entCode)
        {

            DataTable dtTable = new DataTable();
            SqlConnection cn = new SqlConnection(GlobalClass.SQLConnString());
            DataTable dt = new DataTable();
            SqlCommand cmd = null;
            SqlDataAdapter adp;
            string query = "";
            cn.Open();

            if (dtTable.Columns.Count == 0)
            {
                //Columns for AspxGridview
                dtTable.Columns.Add("NAME", typeof(string));
                dtTable.Columns.Add("DESCRIPTION", typeof(string));
            }

            //string qry = "SELECT [NAME],[DESCRIPTION] FROM [dbo].[vw_AXProdCategory] ORDER BY NAME ASC";
            query = "SELECT DISTINCT dbo.vw_AXProdCategory.NAME, dbo.vw_AXProdCategory.DESCRIPTION FROM dbo.vw_AXInventTable LEFT OUTER JOIN dbo.vw_AXProdCategory ON dbo.vw_AXInventTable.ITEMGROUPID = dbo.vw_AXProdCategory.NAME LEFT OUTER JOIN dbo.tbl_MRP_List_DirectMaterials ON dbo.vw_AXInventTable.ITEMID = dbo.tbl_MRP_List_DirectMaterials.ItemCode WHERE HeaderDocNum = '" + docnum + "' AND (dbo.tbl_MRP_List_DirectMaterials.AvailForPO > 0) GROUP BY dbo.vw_AXProdCategory.NAME, dbo.vw_AXProdCategory.DESCRIPTION";

            cmd = new SqlCommand(query);
            cmd.Connection = cn;
            adp = new SqlDataAdapter(cmd);
            adp.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    DataRow dtRow = dtTable.NewRow();
                    dtRow["NAME"] = row["NAME"].ToString();
                    dtRow["DESCRIPTION"] = row["DESCRIPTION"].ToString();
                    dtTable.Rows.Add(dtRow);
                }
            }
            dt.Clear();

            //OPEX
            //query = "SELECT DISTINCT dbo.vw_AXProdCategory.NAME, dbo.vw_AXProdCategory.DESCRIPTION FROM dbo.tbl_MRP_List_OPEX LEFT OUTER JOIN dbo.vw_AXInventTable ON dbo.tbl_MRP_List_OPEX.ItemCode = dbo.vw_AXInventTable.ITEMID LEFT OUTER JOIN dbo.vw_AXProdCategory ON dbo.vw_AXInventTable.ITEMGROUPID = dbo.vw_AXProdCategory.NAME WHERE HeaderDocNum = '" + docnum + "' AND (dbo.tbl_MRP_List_OPEX.AvailForPO > 0) GROUP BY dbo.vw_AXProdCategory.NAME, dbo.vw_AXProdCategory.DESCRIPTION";

            query = "SELECT DISTINCT (CASE RTRIM(LTRIM(dbo.tbl_MRP_List_OPEX.ItemCode)) WHEN '' THEN dbo.tbl_MRP_List_OPEX.ProcCat ELSE (SELECT ITEMGROUPID FROM  dbo.vw_AXInventTable WHERE(ITEMID = RTRIM(LTRIM(dbo.tbl_MRP_List_OPEX.ItemCode))) AND(DATAAREAID = '" + entCode + "')) END) AS ProcurementCat, ISNULL((SELECT DESCRIPTION FROM dbo.vw_AXProdCategory WHERE(NAME = (CASE RTRIM(LTRIM(dbo.tbl_MRP_List_OPEX.ItemCode)) WHEN '' THEN dbo.tbl_MRP_List_OPEX.ProcCat ELSE (SELECT ITEMGROUPID FROM dbo.vw_AXInventTable WHERE(ITEMID = RTRIM(LTRIM (dbo.tbl_MRP_List_OPEX.ItemCode))) AND(DATAAREAID = '" + entCode + "')) END)) AND(dataareaid = N'0000')), '') AS ProcurementCatName FROM dbo.tbl_MRP_List_OPEX LEFT OUTER JOIN dbo.vw_AXInventTable ON dbo.tbl_MRP_List_OPEX.ItemCode = dbo.vw_AXInventTable.ITEMID WHERE(dbo.tbl_MRP_List_OPEX.HeaderDocNum = '" + docnum + "') AND(dbo.tbl_MRP_List_OPEX.AvailForPO > 0) AND((CASE RTRIM(LTRIM(dbo.tbl_MRP_List_OPEX.ItemCode)) WHEN '' THEN dbo.tbl_MRP_List_OPEX.ProcCat ELSE (SELECT ITEMGROUPID FROM  dbo.vw_AXInventTable WHERE(ITEMID = RTRIM(LTRIM(dbo.tbl_MRP_List_OPEX.ItemCode))) AND(DATAAREAID = '" + entCode + "')) END) <> '')";
            cmd = new SqlCommand(query);
            cmd.Connection = cn;
            adp = new SqlDataAdapter(cmd);
            adp.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    DataRow dtRow = dtTable.NewRow();
                    dtRow["NAME"] = row["ProcurementCat"].ToString();
                    dtRow["DESCRIPTION"] = row["ProcurementCatName"].ToString();
                    dtTable.Rows.Add(dtRow);
                }
            }
            dt.Clear();

            //CAPEX
            query = "SELECT DISTINCT dbo.vw_AXProdCategory.NAME, dbo.vw_AXProdCategory.DESCRIPTION FROM dbo.tbl_MRP_List_CAPEX LEFT OUTER JOIN dbo.vw_AXProdCategory ON dbo.tbl_MRP_List_CAPEX.ProdCat = dbo.vw_AXProdCategory.NAME CROSS JOIN dbo.vw_AXInventTable WHERE HeaderDocNum = '" + docnum + "' AND (dbo.tbl_MRP_List_CAPEX.AvailForPO > 0) GROUP BY dbo.vw_AXProdCategory.NAME, dbo.vw_AXProdCategory.DESCRIPTION";

            cmd = new SqlCommand(query);
            cmd.Connection = cn;
            adp = new SqlDataAdapter(cmd);
            adp.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    DataRow dtRow = dtTable.NewRow();
                    dtRow["NAME"] = row["NAME"].ToString();
                    dtRow["DESCRIPTION"] = row["DESCRIPTION"].ToString();
                    dtTable.Rows.Add(dtRow);
                }
            }
            dt.Clear();

            DataTable uniqdtTable = RemoveDuplicateRows(dtTable, "NAME", "DESCRIPTION");

            cn.Close();

            return uniqdtTable;
        }

        public static DataTable ProCategoryTableWithType(string entCode, string sType, string sExpenseCode = "")
        {

            DataTable dtTable = new DataTable();

            SqlConnection cn = new SqlConnection(GlobalClass.SQLConnString());
            DataTable dt = new DataTable();
            SqlCommand cmd = null;
            SqlDataAdapter adp;
            string qry = "";
            cn.Open();

            if (dtTable.Columns.Count == 0)
            {
                //Columns for AspxGridview
                dtTable.Columns.Add("NAME", typeof(string));
                dtTable.Columns.Add("DESCRIPTION", typeof(string));
            }

            if (sExpenseCode == "")
            {
                qry = "SELECT [NAME],[DESCRIPTION] FROM [dbo].[vw_AXProdCategory] WHERE ([dataareaid] = '" + entCode + "') AND ([LedgerType] ='" + sType + "') AND ([isallowedProcCat] = 1) ORDER BY NAME ASC";
            }
            else
            {
                qry = "SELECT [NAME],[DESCRIPTION] FROM [dbo].[vw_AXProdCategory] WHERE ([dataareaid] = '" + entCode + "') AND ([LedgerType] ='" + sType + "') AND ([mainaccount] = '" + sExpenseCode + "') AND ([isallowedProcCat] = 1) ORDER BY NAME ASC";
            }


            cmd = new SqlCommand(qry);
            cmd.Connection = cn;
            adp = new SqlDataAdapter(cmd);
            adp.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    DataRow dtRow = dtTable.NewRow();
                    dtRow["NAME"] = row["NAME"].ToString();
                    dtRow["DESCRIPTION"] = row["DESCRIPTION"].ToString();
                    dtTable.Rows.Add(dtRow);
                }
            }
            dt.Clear();
            cn.Close();

            return dtTable;
        }
        public static DataTable RemoveDuplicateRows(DataTable table, string DistinctColumn, string DistinctColumn2)
        {
            try
            {
                ArrayList UniqueRecords = new ArrayList();
                ArrayList DuplicateRecords = new ArrayList();

                // Check if records is already added to UniqueRecords otherwise,
                // Add the records to DuplicateRecords
                foreach (DataRow dRow in table.Rows)
                {
                    if (UniqueRecords.Contains(dRow[DistinctColumn]))
                        DuplicateRecords.Add(dRow);
                    else
                        UniqueRecords.Add(dRow[DistinctColumn]);

                    if (UniqueRecords.Contains(dRow[DistinctColumn2]))
                        DuplicateRecords.Add(dRow);
                    else
                        UniqueRecords.Add(dRow[DistinctColumn2]);
                }

                // Remove duplicate rows from DataTable added to DuplicateRecords
                foreach (DataRow dRow in DuplicateRecords)
                {
                    table.Rows.Remove(dRow);
                }

                // Return the clean DataTable which contains unique records.
                return table;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public static DataTable OperatingUnitTable(string entity)
        {
            DataTable dtTable = new DataTable();

            SqlConnection cn = new SqlConnection(GlobalClass.SQLConnString());
            DataTable dt = new DataTable();
            SqlCommand cmd = null;
            SqlDataAdapter adp;

            cn.Open();

            if (dtTable.Columns.Count == 0)
            {
                //Columns for AspxGridview
                dtTable.Columns.Add("VALUE", typeof(string));
                dtTable.Columns.Add("DESCRIPTION", typeof(string));
            }

            string qry = "SELECT [VALUE], [DESCRIPTION] FROM [dbo].[vw_AXFindimBananaRevenue] WHERE [Entity] ='" + entity + "'";

            cmd = new SqlCommand(qry);
            cmd.Connection = cn;
            adp = new SqlDataAdapter(cmd);
            adp.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    DataRow dtRow = dtTable.NewRow();
                    dtRow["VALUE"] = row["VALUE"].ToString();
                    dtRow["DESCRIPTION"] = row["DESCRIPTION"].ToString();
                    dtTable.Rows.Add(dtRow);
                }
            }
            dt.Clear();
            cn.Close();

            return dtTable;
        }

        public static void SqlSuccess(int result)
        {
            if (result > 0)
                PrintString("Updated");
            else
                PrintString("Failed Update");
        }

        public static string MOPTableName()
        {
            return "[dbo].[tbl_MRP_List]";
        }

        public static string DirectMatTable()
        {
            return "[dbo].[tbl_MRP_List_DirectMaterials]";
        }

        public static string OpexTable()
        {
            return "[dbo].[tbl_MRP_List_OPEX]";
        }

        public static string ManPowerTable()
        {
            return "[dbo].[tbl_MRP_List_ManPower]";
        }

        public static string CapexTable()
        {
            return "[dbo].[tbl_MRP_List_CAPEX]";
        }

        public static string RevenueTable()
        {
            return "[dbo].[tbl_MRP_List_RevenueAssumptions]";
        }

        public static void PrintString(string str)
        {
            System.Diagnostics.Debug.WriteLine(">>>" + str + "<<<");
        }

        public static string MaterialsTableLogs()
        {
            return "[dbo].[tbl_MRP_List_DirectMaterials_Logs]";
        }

        public static string OpexTableLogs()
        {
            return "[dbo].[tbl_MRP_List_OPEX_Logs]";
        }

        public static string ManpowerTableLogs()
        {
            return "[dbo].[tbl_MRP_List_ManPower_Logs]";
        }

        public static string CapexTableLogs()
        {
            return "[dbo].[tbl_MRP_List_CAPEX_Logs]";
        }

        public static string RevenueTableLogs()
        {
            return "[dbo].[tbl_MRP_List_RevenueAssumptions_Logs]";
        }

        public static string POTableName()
        {
            return "[dbo].[tbl_POCreation]";
        }

        public static string POCreationTableName()
        {
            return "[dbo].[tbl_POCreation_Details]";
        }

        public static string DocNumberTableName()
        {
            return "[dbo].[tbl_DocumentNumber]";
        }

        public static string DefaultPage()
        {
            return "default.aspx";
        }

        public static double capex_total()
        {
            return capex_total_amount;
        }

        public static double opex_total()
        {
            return opex_total_amount;
        }
        public static double materials_total()
        {
            return materials_total_amount;
        }

        public static double manpower_total()
        {
            return manpower_total_amount;
        }

        public static double revenue_total()
        {
            return revenue_total_amount;
        }

        public static double material_edited_total()
        {
            return mat_edited_total;
        }

        public static double capex_edited_total()
        {
            return ca_edited_total;
        }

        public static double opex_edited_total()
        {
            return op_edited_total;
        }
        public static double manpower_edited_total()
        {
            return man_edited_total;
        }

        public static double material_approved_total()
        {
            return mat_approved_total;
        }

        public static double capex_approved_total()
        {
            return ca_approved_total;
        }

        public static double opex_approved_total()
        {
            return op_approved_total;
        }
        public static double manpower_approved_total()
        {
            return man_approved_total;
        }

        public static bool CheckLogsExist(string table, string PK)
        {
            SqlConnection conn = new SqlConnection(GlobalClass.SQLConnString());
            conn.Open();

            string query = "SELECT COUNT(*) FROM " + table + " where MasterKey = '" + PK + "'";
            SqlCommand cmd = new SqlCommand(query, conn);
            int count = Convert.ToInt32(cmd.ExecuteScalar());
            conn.Close();

            if (count > 0)//if the material has logs
                return true;
            else
                return false;
        }

        public static void UpdateLastModified(SqlConnection conn, string docnumber)
        {
            string update_last_modified = "UPDATE [dbo].[tbl_MRP_List] SET [LastModified] = '" + DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss tt") + "' WHERE [DocNumber] = '" + docnumber + "'";
            SqlCommand comm = new SqlCommand(update_last_modified, conn);
            comm.ExecuteNonQuery();
        }

        public static void AddLogsMOPList(SqlConnection conn, int MRPKey, string Remarks)
        {
            String PhysicalAdd = NetworkInterface.GetAllNetworkInterfaces()
                            .Where(nic => nic.OperationalStatus == OperationalStatus.Up && nic.NetworkInterfaceType != NetworkInterfaceType.Loopback)
                            .Select(nic => nic.GetPhysicalAddress().ToString())
                            .FirstOrDefault();

            string ComputerUN = Environment.UserName;

            string add_logs = "INSERT INTO [dbo].[tbl_MRP_List_ModifiedLogs] ([MRPKey], [DateModified],[PhysicalAdd], [ComputerUN], [Remarks]) VALUES (@MRPKey, @DateModified, @PhysicalAdd, @ComputerUN, @Remarks)";
            SqlCommand comm = new SqlCommand(add_logs, conn);
            comm.Parameters.AddWithValue("@MRPKey", MRPKey);
            comm.Parameters.AddWithValue("@DateModified", DateTime.Now.ToString());
            comm.Parameters.AddWithValue("@PhysicalAdd", PhysicalAdd);
            comm.Parameters.AddWithValue("@ComputerUN", ComputerUN);
            comm.Parameters.AddWithValue("@Remarks", Remarks);
            comm.ExecuteNonQuery();
        }

        public static void SetBehaviorGrid(ASPxGridView grid)
        {
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

        public static void VisibilityRevDesc(ASPxGridView grid, string entitycode)
        {
            if (entitycode == Constants.TRAIN_CODE())
                grid.Columns["RevDesc"].Visible = true;
            else
                grid.Columns["RevDesc"].Visible = false;
        }

        public static int MRP_Line_Status(int masterKey, int line)
        {
            //tbl_MRP_List_Workflow
            int mrpLineStat = 0;
            string qry = "";
            SqlConnection conn = new SqlConnection(GlobalClass.SQLConnString());
            SqlCommand cmd = null;
            SqlDataAdapter adp;
            DataTable dtable = new DataTable();

            conn.Open();
            if (line == 0)
            {
                qry = "SELECT StatusKey " +
                      " FROM tbl_MRP_List " +
                      " WHERE (PK = " + masterKey + ") ";
                cmd = new SqlCommand(qry);
                cmd.Connection = conn;
                adp = new SqlDataAdapter(cmd);
                adp.Fill(dtable);
                if (dtable.Rows.Count > 0)
                {
                    foreach (DataRow row in dtable.Rows)
                    {
                        if (Convert.ToInt32(row["StatusKey"]) == 1)
                        {
                            mrpLineStat = 0;
                        }
                        else
                        {
                            mrpLineStat = 1;
                        }
                        //mrpLineStat = Convert.ToInt32(row["Status"]);
                    }
                }
                dtable.Clear();
            }
            else
            {
                qry = "SELECT Status " +
                      " FROM tbl_MRP_List_Workflow " +
                      " WHERE (MasterKey = " + masterKey + ") " +
                      " AND (Line = " + line + ")";
                cmd = new SqlCommand(qry);
                cmd.Connection = conn;
                adp = new SqlDataAdapter(cmd);
                adp.Fill(dtable);
                if (dtable.Rows.Count > 0)
                {
                    foreach (DataRow row in dtable.Rows)
                    {
                        mrpLineStat = Convert.ToInt32(row["Status"]);
                    }
                }
                dtable.Clear();
            }

            conn.Close();
            return mrpLineStat;
        }

        public static void Approve_MRP(string docNum, int MRPKey, int AprvLine)
        {
            SqlConnection conn = new SqlConnection(GlobalClass.SQLConnString());
            string qry = "", sEmail = "", sEmailCR = "", sMailSubject = "", sGreetings = "", sSubjectCR = "", sGreetingsCR = "";
            var sbBody = new StringBuilder();
            var sbBodyCR = new StringBuilder();
            SqlCommand cmdIns = null;
            SqlCommand cmdUp = null;
            SqlCommand cmd = null;
            SqlDataAdapter adp;
            DataTable dtable = new DataTable();

            SqlCommand cmd1 = null;
            SqlDataAdapter adp1;
            DataTable dtable1 = new DataTable();

            SqlCommand cmd2 = null;
            SqlDataAdapter adp2;
            DataTable dtable2 = new DataTable();

            conn.Open();
            qry = "SELECT dbo.tbl_System_Approval_Position.SQLQuery, ISNULL(tbl_Users_1.Email, '') AS Email, " +
                  " tbl_Users_1.Lastname, tbl_Users_1.Gender, dbo.tbl_MRP_List_Approval.UserKey, " +
                  " dbo.tbl_MRP_List_Approval.PositionNameKey, dbo.tbl_Users.Lastname AS CreatorLName, " +
                  " dbo.tbl_Users.Email AS CreatorEmail, dbo.tbl_Users.Gender AS CreatorGender, " +
                  " dbo.tbl_System_Approval_Position.PositionName " +
                  " FROM dbo.tbl_Users RIGHT OUTER JOIN " +
                  " dbo.tbl_MRP_List ON dbo.tbl_Users.PK = dbo.tbl_MRP_List.CreatorKey RIGHT OUTER JOIN " +
                  " dbo.tbl_MRP_List_Approval ON dbo.tbl_MRP_List.PK = dbo.tbl_MRP_List_Approval.MasterKey LEFT OUTER JOIN " +
                  " dbo.tbl_Users AS tbl_Users_1 ON dbo.tbl_MRP_List_Approval.UserKey = tbl_Users_1.PK LEFT OUTER JOIN " +
                  " dbo.tbl_System_Approval_Position ON dbo.tbl_MRP_List_Approval.PositionNameKey = dbo.tbl_System_Approval_Position.PK " +
                  " WHERE(dbo.tbl_MRP_List_Approval.Line = " + AprvLine + ") " +
                  " AND(dbo.tbl_MRP_List_Approval.MasterKey = " + MRPKey + ")";
            cmd = new SqlCommand(qry);
            cmd.Connection = conn;
            adp = new SqlDataAdapter(cmd);
            adp.Fill(dtable);
            if (dtable.Rows.Count > 0)
            {
                foreach (DataRow row in dtable.Rows)
                {
                    sEmail = EncryptionClass.Decrypt(row["Email"].ToString());
                    sEmailCR = EncryptionClass.Decrypt(row["CreatorEmail"].ToString());

                    int AprvLineB4 = AprvLine - 1;

                    if (Convert.ToInt32(row["CreatorGender"]) == 1)
                    {
                        sGreetingsCR = "Dear Mr. " + EncryptionClass.Decrypt(row["CreatorLName"].ToString());
                    }
                    else
                    {
                        sGreetingsCR = "Dear Ms. " + EncryptionClass.Decrypt(row["CreatorLName"].ToString());
                    }

                    sSubjectCR = "MOP DocNum " + docNum.ToString() + " status";

                    if (GlobalClass.IsEmailValid(sEmail) == true)
                    {
                        if (Convert.ToInt32(row["Gender"]) == 1)
                        {
                            sGreetings = "Dear Mr. " + EncryptionClass.Decrypt(row["Lastname"].ToString());
                        }
                        else
                        {
                            sGreetings = "Dear Ms. " + EncryptionClass.Decrypt(row["Lastname"].ToString());
                        }
                        sMailSubject = "MOP DocNum " + docNum.ToString() + " is waiting for your approval";

                        //Update Workflow B4
                        qry = "UPDATE tbl_MRP_List_Approval " +
                               " SET Visible = 0, " +
                               " Status = 1 " +
                               " WHERE (MasterKey = " + MRPKey + ") " +
                               " AND (Line = " + AprvLineB4 + ")";
                        cmdUp = new SqlCommand(qry, conn);
                        cmdUp.ExecuteNonQuery();

                        //Update Workflow
                        qry = "UPDATE tbl_MRP_List_Approval " +
                               " SET Visible = 1 " +
                               " WHERE (MasterKey = " + MRPKey + ") " +
                               " AND (Line = " + AprvLine + ")";
                        cmdUp = new SqlCommand(qry, conn);
                        cmdUp.ExecuteNonQuery();

                        sbBody.Append("<!DOCTYPE html>");
                        sbBody.Append("<html>");
                        sbBody.Append("<head>");
                        sbBody.Append("</head>");
                        sbBody.Append("<body>");
                        sbBody.Append("<p style='font-family:Tahoma; font-size: 12px;'>" + sGreetings + ",</p>");
                        sbBody.Append("<p style='font-family:Tahoma; font-size: 12px;'>MOP Document # " + docNum.ToString() + " is waiting for your approval.</p>");
                        sbBody.Append("<p style='font-family:Tahoma; font-size: 10px;font-style:italic;'>***This is a system-generated message. please do not reply to this email.***</p>");
                        sbBody.Append("<p style='font-family:Tahoma; font-size: 10px;'>DISCLAIMER: This email is confidential and intended solely for the use of the individual to whom it is addressed. If you are not the intended recipient, be advised that you have received this email in error and that any use, dissemination, forwarding, printing or copying of this email is strictly prohibited. If you have received this email in error please notify the sender or email info@hijoresources.net, telephone number (082) 282-3662.</p>");
                        sbBody.Append("</body>");
                        sbBody.Append("</html>");

                        bool msgSend = GlobalClass.IsMailSent(sEmail, sMailSubject, sbBody.ToString());

                        if (msgSend == true)
                        {
                            //Insert to User Assigned
                            qry = "SELECT tbl_Users_Assigned.* " +
                                  " FROM tbl_Users_Assigned " +
                                  " WHERE (UserKey = " + Convert.ToInt32(row["UserKey"]) + ") " +
                                  " AND (MRPKey = " + MRPKey + ") " +
                                  " AND (WorkFlowLine = " + AprvLine + ") " +
                                  " AND (WorkFlowType = 2)";
                            cmd1 = new SqlCommand(qry);
                            cmd1.Connection = conn;
                            adp1 = new SqlDataAdapter(cmd1);
                            adp1.Fill(dtable1);
                            if (dtable1.Rows.Count == 0)
                            {
                                qry = "INSERT INTO tbl_Users_Assigned " +
                                      " (UserKey, PositionNameKey, MRPKey, WorkFlowLine, WorkFlowType) " +
                                      " VALUES(" + Convert.ToInt32(row["UserKey"]) + ", " +
                                      " " + Convert.ToInt32(row["PositionNameKey"]) + ", " +
                                      " " + MRPKey + ", " + AprvLine + ", 2)";
                                cmdIns = new SqlCommand(qry, conn);
                                cmdIns.ExecuteNonQuery();
                            }
                            dtable1.Clear();
                        }

                        // Send Email to Creator
                        sbBodyCR.Append("<!DOCTYPE html>");
                        sbBodyCR.Append("<html>");
                        sbBodyCR.Append("<head>");
                        sbBodyCR.Append("</head>");
                        sbBodyCR.Append("<body>");
                        sbBodyCR.Append("<p style='font-family:Tahoma; font-size: 12px;'>" + sGreetingsCR + ",</p>");
                        sbBodyCR.Append("<p style='font-family:Tahoma; font-size: 12px;'>MOP Document # " + docNum.ToString() + " has been submitted to " + row["PositionName"].ToString() + " for approval.</p>");
                        sbBodyCR.Append("<p style='font-family:Tahoma; font-size: 10px;font-style:italic;'>***This is a system-generated message. please do not reply to this email.***</p>");
                        sbBodyCR.Append("<p style='font-family:Tahoma; font-size: 10px;'>DISCLAIMER: This email is confidential and intended solely for the use of the individual to whom it is addressed. If you are not the intended recipient, be advised that you have received this email in error and that any use, dissemination, forwarding, printing or copying of this email is strictly prohibited. If you have received this email in error please notify the sender or email info@hijoresources.net, telephone number (082) 282-3662.</p>");
                        sbBodyCR.Append("</body>");
                        sbBodyCR.Append("</html>");

                        bool msgSendToCreator = GlobalClass.IsMailSent(sEmailCR, sSubjectCR, sbBodyCR.ToString());
                    }
                }
            }
            else
            {
                // back to workflow
                qry = "SELECT dbo.tbl_MRP_List.CreatorKey, dbo.tbl_Users.Lastname, " +
                      " dbo.tbl_Users.Gender, dbo.tbl_Users.Email " +
                      " FROM dbo.tbl_MRP_List LEFT OUTER JOIN " +
                      " dbo.tbl_Users ON dbo.tbl_MRP_List.CreatorKey = dbo.tbl_Users.PK " +
                      " WHERE(dbo.tbl_MRP_List.PK = " + MRPKey + ")";
                cmd1 = new SqlCommand(qry);
                cmd1.Connection = conn;
                adp1 = new SqlDataAdapter(cmd1);
                adp1.Fill(dtable1);
                if (dtable1.Rows.Count > 0)
                {
                    foreach (DataRow row in dtable1.Rows)
                    {
                        sEmailCR = EncryptionClass.Decrypt(row["Email"].ToString());
                        if (Convert.ToInt32(row["Gender"]) == 1)
                        {
                            sGreetingsCR = "Dear Mr. " + EncryptionClass.Decrypt(row["Lastname"].ToString());
                        }
                        else
                        {
                            sGreetingsCR = "Dear Ms. " + EncryptionClass.Decrypt(row["Lastname"].ToString());
                        }
                        sSubjectCR = "MOP DocNum " + docNum.ToString() + " status";
                        // Send Email to Creator
                        sbBodyCR.Append("<!DOCTYPE html>");
                        sbBodyCR.Append("<html>");
                        sbBodyCR.Append("<head>");
                        sbBodyCR.Append("</head>");
                        sbBodyCR.Append("<body>");
                        sbBodyCR.Append("<p style='font-family:Tahoma; font-size: 12px;'>" + sGreetingsCR + ",</p>");
                        sbBodyCR.Append("<p style='font-family:Tahoma; font-size: 12px;'>MOP Document # " + docNum.ToString() + " has been approved.</p>");
                        sbBodyCR.Append("<p style='font-family:Tahoma; font-size: 10px;font-style:italic;'>***This is a system-generated message. please do not reply to this email.***</p>");
                        sbBodyCR.Append("<p style='font-family:Tahoma; font-size: 10px;'>DISCLAIMER: This email is confidential and intended solely for the use of the individual to whom it is addressed. If you are not the intended recipient, be advised that you have received this email in error and that any use, dissemination, forwarding, printing or copying of this email is strictly prohibited. If you have received this email in error please notify the sender or email info@hijoresources.net, telephone number (082) 282-3662.</p>");
                        sbBodyCR.Append("</body>");
                        sbBodyCR.Append("</html>");

                        bool msgSendToCreator = GlobalClass.IsMailSent(sEmailCR, sSubjectCR, sbBodyCR.ToString());

                    }
                }
                dtable1.Clear();

                //Update MOP Status
                qry = "UPDATE tbl_MRP_List SET StatusKey = 4 WHERE (PK = " + MRPKey + ")";
                cmdUp = new SqlCommand(qry, conn);
                cmdUp.ExecuteNonQuery();

                //Update Workflow B4
                qry = "UPDATE tbl_MRP_List_Workflow " +
                       " SET Visible = 0, " +
                       " Status = 1 " +
                       " WHERE (MasterKey = " + MRPKey + ") " +
                       " AND (Line = 5)";
                cmdUp = new SqlCommand(qry, conn);
                cmdUp.ExecuteNonQuery();

                //if have CAPEX
                qry = "SELECT tbl_MRP_List_CAPEX.* " +
                      " FROM tbl_MRP_List_CAPEX " +
                      " WHERE (HeaderDocNum = '" + docNum.ToString() + "')";
                cmd1 = new SqlCommand(qry);
                cmd1.Connection = conn;
                adp1 = new SqlDataAdapter(cmd1);
                adp1.Fill(dtable1);
                if (dtable1.Rows.Count > 0)
                {
                    qry = "UPDATE tbl_MRP_List_Workflow " +
                          " SET Visible = 1 " +
                          " WHERE (MasterKey = " + MRPKey + ") " +
                          " AND (Line = 6)";
                    cmdUp = new SqlCommand(qry, conn);
                    cmdUp.ExecuteNonQuery();
                }
                else
                {
                    qry = "UPDATE tbl_MRP_List_Workflow " +
                          " SET Visible = 0, " +
                          " Status = 1 " +
                          " WHERE (MasterKey = " + MRPKey + ") " +
                          " AND (Line = 6)";
                    cmdUp = new SqlCommand(qry, conn);
                    cmdUp.ExecuteNonQuery();

                    qry = "UPDATE tbl_MRP_List_Workflow " +
                          " SET Visible = 1 " +
                          " WHERE (MasterKey = " + MRPKey + ") " +
                          " AND (Line = 7)";
                    cmdUp = new SqlCommand(qry, conn);
                    cmdUp.ExecuteNonQuery();
                }
                dtable1.Clear();
            }
            dtable.Clear();
            conn.Close();
        }

        public static void Submit_MRP(string docNum, int MRPKey, int WorkFlowLine, string EntCode, string BuCode, int usrKey)
        {
            SqlConnection conn = new SqlConnection(GlobalClass.SQLConnString());
            string qry = "", sEmail = "", sEmailCR = "", sMailSubject = "", sGreetings = "", sSubjectCR = "", sGreetingsCR = "";
            SqlCommand cmdIns = null;
            SqlCommand cmdUp = null;
            SqlCommand cmd = null;
            SqlDataAdapter adp;
            DataTable dtable = new DataTable();

            SqlCommand cmd1 = null;
            SqlDataAdapter adp1;
            DataTable dtable1 = new DataTable();

            conn.Open();
            qry = "SELECT dbo.tbl_System_Approval_Position.SQLQuery, ISNULL(tbl_Users_1.Email, '') AS Email, " +
                  " tbl_Users_1.Lastname, tbl_Users_1.Gender, dbo.tbl_MRP_List_Workflow.UserKey, " +
                  " dbo.tbl_MRP_List_Workflow.PositionNameKey, dbo.tbl_Users.Lastname AS CreatorLName, " +
                  " dbo.tbl_Users.Email AS CreatorEmail, dbo.tbl_Users.Gender AS CreatorGender, " +
                  " dbo.tbl_System_Approval_Position.PositionName " +
                  " FROM dbo.tbl_Users RIGHT OUTER JOIN " +
                  " dbo.tbl_MRP_List ON dbo.tbl_Users.PK = dbo.tbl_MRP_List.CreatorKey RIGHT OUTER JOIN " +
                  " dbo.tbl_MRP_List_Workflow ON dbo.tbl_MRP_List.PK = dbo.tbl_MRP_List_Workflow.MasterKey LEFT OUTER JOIN " +
                  " dbo.tbl_Users AS tbl_Users_1 ON dbo.tbl_MRP_List_Workflow.UserKey = tbl_Users_1.PK LEFT OUTER JOIN " +
                  " dbo.tbl_System_Approval_Position ON dbo.tbl_MRP_List_Workflow.PositionNameKey = dbo.tbl_System_Approval_Position.PK " +
                  " WHERE(dbo.tbl_MRP_List_Workflow.Line = " + WorkFlowLine + ") " +
                  " AND(dbo.tbl_MRP_List_Workflow.MasterKey = " + MRPKey + ")";
            cmd = new SqlCommand(qry);
            cmd.Connection = conn;
            adp = new SqlDataAdapter(cmd);
            adp.Fill(dtable);
            if (dtable.Rows.Count > 0)
            {
                foreach (DataRow row in dtable.Rows)
                {
                    sEmail = "";
                    if (row["Email"].ToString().Trim() != "")
                    {
                        sEmail = EncryptionClass.Decrypt(row["Email"].ToString());
                    }

                    sEmailCR = EncryptionClass.Decrypt(row["CreatorEmail"].ToString());
                    var sbBody = new StringBuilder();
                    var sbBodyCR = new StringBuilder();
                    int wrkflwlnB4 = WorkFlowLine - 1;

                    if (Convert.ToInt32(row["CreatorGender"]) == 1)
                    {
                        sGreetingsCR = "Dear Mr. " + EncryptionClass.Decrypt(row["CreatorLName"].ToString());
                    }
                    else
                    {
                        sGreetingsCR = "Dear Ms. " + EncryptionClass.Decrypt(row["CreatorLName"].ToString());
                    }

                    sSubjectCR = "MOP DocNum " + docNum.ToString() + " status";

                    if (GlobalClass.IsEmailValid(sEmail) == true)
                    {
                        //send email to approver
                        if (WorkFlowLine == 4)
                        {
                            sMailSubject = "MOP DocNum " + docNum.ToString() + " is waiting for deliberation";
                        }
                        else
                        {
                            sMailSubject = "MOP DocNum " + docNum.ToString() + " is waiting for your review and approval";
                        }

                        if (Convert.ToInt32(row["Gender"]) == 1)
                        {
                            sGreetings = "Dear Mr. " + EncryptionClass.Decrypt(row["Lastname"].ToString());
                        }
                        else
                        {
                            sGreetings = "Dear Ms. " + EncryptionClass.Decrypt(row["Lastname"].ToString());
                        }

                        //Update Workflow B4
                        qry = "UPDATE tbl_MRP_List_Workflow " +
                               " SET Visible = 0, " +
                               " Status = 1 " +
                               " WHERE (MasterKey = " + MRPKey + ") " +
                               " AND (Line = " + wrkflwlnB4 + ")";
                        cmdUp = new SqlCommand(qry, conn);
                        cmdUp.ExecuteNonQuery();

                        //Update Workflow
                        qry = "UPDATE tbl_MRP_List_Workflow " +
                               " SET Visible = 1 " +
                               " WHERE (MasterKey = " + MRPKey + ") " +
                               " AND (Line = " + WorkFlowLine + ")";
                        cmdUp = new SqlCommand(qry, conn);
                        cmdUp.ExecuteNonQuery();

                        //Update MOP Status
                        qry = "UPDATE tbl_MRP_List SET StatusKey = 2 WHERE (PK = " + MRPKey + ")";
                        cmdUp = new SqlCommand(qry, conn);
                        cmdUp.ExecuteNonQuery();

                        sbBody.Append("<!DOCTYPE html>");
                        sbBody.Append("<html>");
                        sbBody.Append("<head>");
                        sbBody.Append("</head>");
                        sbBody.Append("<body>");
                        sbBody.Append("<p style='font-family:Tahoma; font-size: 12px;'>" + sGreetings + ",</p>");
                        sbBody.Append("<p style='font-family:Tahoma; font-size: 12px;'>MOP Document # " + docNum.ToString() + " is waiting for your review/approval.</p>");
                        sbBody.Append("<p style='font-family:Tahoma; font-size: 10px;font-style:italic;'>***This is a system-generated message. please do not reply to this email.***</p>");
                        sbBody.Append("<p style='font-family:Tahoma; font-size: 10px;'>DISCLAIMER: This email is confidential and intended solely for the use of the individual to whom it is addressed. If you are not the intended recipient, be advised that you have received this email in error and that any use, dissemination, forwarding, printing or copying of this email is strictly prohibited. If you have received this email in error please notify the sender or email info@hijoresources.net, telephone number (082) 282-3662.</p>");
                        sbBody.Append("</body>");
                        sbBody.Append("</html>");

                        bool msgSend = GlobalClass.IsMailSent(sEmail, sMailSubject, sbBody.ToString());

                        if (msgSend == true)
                        {
                            if (WorkFlowLine == 1 || WorkFlowLine == 2 || WorkFlowLine == 3)
                            {
                                //Insert to User Assigned
                                qry = "SELECT tbl_Users_Assigned.* " +
                                  " FROM tbl_Users_Assigned " +
                                  " WHERE (UserKey = " + Convert.ToInt32(row["UserKey"]) + ") " +
                                  " AND (MRPKey = " + MRPKey + ") " +
                                  " AND (WorkFlowLine = " + WorkFlowLine + ") " +
                                  " AND (WorkFlowType = 1)";
                                cmd1 = new SqlCommand(qry);
                                cmd1.Connection = conn;
                                adp1 = new SqlDataAdapter(cmd1);
                                adp1.Fill(dtable1);
                                if (dtable1.Rows.Count == 0)
                                {
                                    qry = "INSERT INTO tbl_Users_Assigned " +
                                          " (UserKey, PositionNameKey, MRPKey, WorkFlowLine, WorkFlowType) " +
                                          " VALUES(" + Convert.ToInt32(row["UserKey"]) + ", " +
                                          " " + Convert.ToInt32(row["PositionNameKey"]) + ", " +
                                          " " + MRPKey + ", " + WorkFlowLine + ", 1)";
                                    cmdIns = new SqlCommand(qry, conn);
                                    cmdIns.ExecuteNonQuery();
                                }
                                dtable1.Clear();
                            }
                        }

                        // Send Email to Creator
                        sbBodyCR.Append("<!DOCTYPE html>");
                        sbBodyCR.Append("<html>");
                        sbBodyCR.Append("<head>");
                        sbBodyCR.Append("</head>");
                        sbBodyCR.Append("<body>");
                        sbBodyCR.Append("<p style='font-family:Tahoma; font-size: 12px;'>" + sGreetingsCR + ",</p>");
                        if (WorkFlowLine == 4)
                        {
                            sbBodyCR.Append("<p style='font-family:Tahoma; font-size: 12px;'>MOP Document # " + docNum.ToString() + " has been submitted for deliberation.</p>");
                        }
                        else
                        {
                            sbBodyCR.Append("<p style='font-family:Tahoma; font-size: 12px;'>MOP Document # " + docNum.ToString() + " has been submitted to " + row["PositionName"].ToString() + " for review/approval.</p>");
                        }
                        sbBodyCR.Append("<p style='font-family:Tahoma; font-size: 10px;font-style:italic;'>***This is a system-generated message. please do not reply to this email.***</p>");
                        sbBodyCR.Append("<p style='font-family:Tahoma; font-size: 10px;'>DISCLAIMER: This email is confidential and intended solely for the use of the individual to whom it is addressed. If you are not the intended recipient, be advised that you have received this email in error and that any use, dissemination, forwarding, printing or copying of this email is strictly prohibited. If you have received this email in error please notify the sender or email info@hijoresources.net, telephone number (082) 282-3662.</p>");
                        sbBodyCR.Append("</body>");
                        sbBodyCR.Append("</html>");


                    }
                    else
                    {
                        if (WorkFlowLine == 5)
                        {

                            //Update MOP Status
                            qry = "UPDATE tbl_MRP_List SET StatusKey = 3 WHERE (PK = " + MRPKey + ")";
                            cmdUp = new SqlCommand(qry, conn);
                            cmdUp.ExecuteNonQuery();

                            // Send Email to Creator
                            sbBodyCR.Append("<!DOCTYPE html>");
                            sbBodyCR.Append("<html>");
                            sbBodyCR.Append("<head>");
                            sbBodyCR.Append("</head>");
                            sbBodyCR.Append("<body>");
                            sbBodyCR.Append("<p style='font-family:Tahoma; font-size: 12px;'>" + sGreetingsCR + ",</p>");
                            sbBodyCR.Append("<p style='font-family:Tahoma; font-size: 12px;'>MOP Document # " + docNum.ToString() + " has been submitted for approval.</p>");
                            sbBodyCR.Append("<p style='font-family:Tahoma; font-size: 10px;font-style:italic;'>***This is a system-generated message. please do not reply to this email.***</p>");
                            sbBodyCR.Append("<p style='font-family:Tahoma; font-size: 10px;'>DISCLAIMER: This email is confidential and intended solely for the use of the individual to whom it is addressed. If you are not the intended recipient, be advised that you have received this email in error and that any use, dissemination, forwarding, printing or copying of this email is strictly prohibited. If you have received this email in error please notify the sender or email info@hijoresources.net, telephone number (082) 282-3662.</p>");
                            sbBodyCR.Append("</body>");
                            sbBodyCR.Append("</html>");
                        }
                    }

                    bool msgSendToCreator = GlobalClass.IsMailSent(sEmailCR, sSubjectCR, sbBodyCR.ToString());

                    //Update user assigned to me
                    qry = "UPDATE tbl_Users_Assigned " +
                           " SET Attended = 1 " +
                           " WHERE (MRPKey = " + MRPKey + ") " +
                           " AND (WorkFlowLine = " + wrkflwlnB4 + ") " +
                           " AND (UserKey = " + usrKey + ") " +
                           " AND (WorkFlowType = 1)";
                    cmdUp = new SqlCommand(qry, conn);
                    cmdUp.ExecuteNonQuery();

                    //to approver
                    if (WorkFlowLine == 5)
                    {
                        //Update approver workflow
                        qry = "UPDATE tbl_MRP_List_Approval " +
                               " SET Visible = 1 " +
                               " WHERE (MasterKey = " + MRPKey + ") " +
                               " AND (Line = 1)";
                        cmdUp = new SqlCommand(qry, conn);
                        cmdUp.ExecuteNonQuery();
                    }

                }
            }
            dtable.Clear();
            conn.Close();
        }

        public static DataTable MRP_InventoryAnalyst_Edit()
        {
            DataTable dtTable = new DataTable();
            SqlCommand cmd = null;
            SqlDataAdapter adp;
            DataTable dtable = new DataTable();

            if (dtTable.Columns.Count == 0)
            {
                dtTable.Columns.Add("PK", typeof(string));
                dtTable.Columns.Add("DocNumber", typeof(string));
                dtTable.Columns.Add("DateCreated", typeof(string));
                dtTable.Columns.Add("EntityCodeDesc", typeof(string));
                dtTable.Columns.Add("BUCodeDesc", typeof(string));
                dtTable.Columns.Add("MRPMonthDesc", typeof(string));
                dtTable.Columns.Add("MRPYear", typeof(string));
            }
            dtTable.Clear();

            SqlConnection conn = new SqlConnection(GlobalClass.SQLConnString());
            conn.Open();
            string qry = "SELECT dbo.tbl_MRP_List.DocNumber, dbo.vw_AXEntityTable.NAME AS Entity, dbo.vw_AXOperatingUnitTable.NAME AS Department, dbo.tbl_MRP_List_Workflow.MasterKey, dbo.tbl_MRP_List.MRPMonth, dbo.tbl_MRP_List.MRPYear, dbo.tbl_MRP_List.DateCreated FROM dbo.tbl_MRP_List_Workflow LEFT OUTER JOIN dbo.tbl_MRP_List ON dbo.tbl_MRP_List_Workflow.MasterKey = dbo.tbl_MRP_List.PK LEFT OUTER JOIN dbo.vw_AXEntityTable ON dbo.tbl_MRP_List.EntityCode = dbo.vw_AXEntityTable.ID LEFT OUTER JOIN dbo.vw_AXOperatingUnitTable ON dbo.tbl_MRP_List.BUCode = dbo.vw_AXOperatingUnitTable.OMOPERATINGUNITNUMBER LEFT OUTER JOIN dbo.tbl_MRP_Status ON dbo.tbl_MRP_List.StatusKey = dbo.tbl_MRP_Status.PK WHERE(dbo.tbl_MRP_List_Workflow.Line = 2) AND(dbo.tbl_MRP_List_Workflow.Status = 0)  AND(dbo.tbl_MRP_List_Workflow.Visible = 1)";
            cmd = new SqlCommand(qry);
            cmd.Connection = conn;
            adp = new SqlDataAdapter(cmd);
            adp.Fill(dtable);
            if (dtable.Rows.Count > 0)
            {
                foreach (DataRow row in dtable.Rows)
                {
                    DataRow rowAdd = dtTable.NewRow();
                    rowAdd["PK"] = row["MasterKey"].ToString();
                    rowAdd["DocNumber"] = row["DocNumber"].ToString();
                    rowAdd["DateCreated"] = Convert.ToDateTime(row["DateCreated"]).ToString("MM/dd/yyyy");
                    rowAdd["EntityCodeDesc"] = row["Entity"].ToString();
                    rowAdd["BUCodeDesc"] = row["Department"].ToString();
                    rowAdd["MRPMonthDesc"] = Month_Name(Convert.ToInt32(row["MRPMonth"]));
                    rowAdd["MRPYear"] = row["MRPYear"].ToString();
                    dtTable.Rows.Add(rowAdd);
                }
            }
            dtable.Clear();
            conn.Close();

            return dtTable;
        }

        public static DataTable MRP_Work_Assigned_To_Me(int usrkey)
        {
            DataTable dtTable = new DataTable();
            SqlCommand cmd = null;
            SqlDataAdapter adp;
            DataTable dtable = new DataTable();

            if (dtTable.Columns.Count == 0)
            {
                dtTable.Columns.Add("PK", typeof(string));
                dtTable.Columns.Add("DocNumber", typeof(string));
                dtTable.Columns.Add("DateCreated", typeof(string));
                dtTable.Columns.Add("EntityCodeDesc", typeof(string));
                dtTable.Columns.Add("BUCodeDesc", typeof(string));
                dtTable.Columns.Add("MRPMonthDesc", typeof(string));
                dtTable.Columns.Add("MRPYear", typeof(string));
                dtTable.Columns.Add("LevelLine", typeof(string));
                dtTable.Columns.Add("LevelPosition", typeof(string));
                dtTable.Columns.Add("Status", typeof(string));
                dtTable.Columns.Add("WorkflowType", typeof(string));
                dtTable.Columns.Add("CreatorKey", typeof(string));
            }
            dtTable.Clear();

            SqlConnection conn = new SqlConnection(GlobalClass.SQLConnString());
            conn.Open();
            //string qry = "SELECT dbo.tbl_Users_Assigned.MRPKey, dbo.tbl_MRP_List.DocNumber, " +
            //             " dbo.tbl_MRP_List.DateCreated, dbo.vw_AXEntityTable.NAME AS Entity, " +
            //             " dbo.vw_AXOperatingUnitTable.NAME AS Dept, dbo.tbl_MRP_List.MRPMonth, " +
            //             " dbo.tbl_MRP_List.MRPYear, dbo.tbl_Users_Assigned.WorkFlowLine, " +
            //             " dbo.tbl_Users_Assigned.PositionNameKey, dbo.tbl_System_Approval_Position.PositionName, " +
            //             " dbo.tbl_MRP_List.StatusKey, dbo.tbl_MRP_Status.StatusName, " +
            //             " dbo.tbl_MRP_List_Workflow.Visible, dbo.tbl_MRP_List_Workflow.Status, " +
            //             " dbo.tbl_Users_Assigned.WorkFlowType " +
            //             " FROM  dbo.tbl_MRP_List_Workflow RIGHT OUTER JOIN " +
            //             " dbo.tbl_MRP_List ON dbo.tbl_MRP_List_Workflow.MasterKey = dbo.tbl_MRP_List.PK RIGHT OUTER JOIN " +
            //             " dbo.tbl_Users_Assigned ON dbo.tbl_MRP_List_Workflow.Line = dbo.tbl_Users_Assigned.WorkFlowLine AND dbo.tbl_MRP_List.PK = dbo.tbl_Users_Assigned.MRPKey LEFT OUTER JOIN " +
            //             " dbo.tbl_System_Approval_Position ON dbo.tbl_Users_Assigned.PositionNameKey = dbo.tbl_System_Approval_Position.PK LEFT OUTER JOIN " +
            //             " dbo.tbl_MRP_Status ON dbo.tbl_MRP_List.StatusKey = dbo.tbl_MRP_Status.PK LEFT OUTER JOIN " +
            //             " dbo.vw_AXEntityTable ON dbo.tbl_MRP_List.EntityCode = dbo.vw_AXEntityTable.ID LEFT OUTER JOIN " +
            //             " dbo.vw_AXOperatingUnitTable ON dbo.tbl_MRP_List.BUCode = dbo.vw_AXOperatingUnitTable.OMOPERATINGUNITNUMBER " +
            //             " WHERE(dbo.tbl_MRP_List_Workflow.Visible = 1) " +
            //             " AND(dbo.tbl_MRP_List_Workflow.Status = 0)" +
            //             " AND (dbo.tbl_Users_Assigned.UserKey = " + usrkey + ")";
            string qry = "SELECT dbo.tbl_Users_Assigned.MRPKey, dbo.tbl_MRP_List.DocNumber, dbo.tbl_MRP_List.CreatorKey, dbo.tbl_MRP_List.DateCreated, dbo.vw_AXEntityTable.NAME AS Entity, " +
                         " dbo.vw_AXOperatingUnitTable.NAME AS Dept, dbo.tbl_MRP_List.MRPMonth, dbo.tbl_MRP_List.MRPYear, dbo.tbl_Users_Assigned.WorkFlowLine, " +
                         " dbo.tbl_Users_Assigned.PositionNameKey, dbo.tbl_System_Approval_Position.PositionName, dbo.tbl_MRP_List.StatusKey, dbo.tbl_MRP_Status.StatusName, " +
                         " dbo.tbl_Users_Assigned.WorkFlowType, (CASE dbo.tbl_Users_Assigned.WorkFlowType WHEN 1 THEN (SELECT Status FROM  dbo.tbl_MRP_List_Workflow " +
                         " WHERE(MasterKey = dbo.tbl_Users_Assigned.MRPKey) AND(Line = dbo.tbl_Users_Assigned.WorkFlowLine)) ELSE (SELECT Status FROM  dbo.tbl_MRP_List_Approval " +
                         " WHERE(MasterKey = dbo.tbl_Users_Assigned.MRPKey) AND(Line = dbo.tbl_Users_Assigned.WorkFlowLine)) END) AS Status, (CASE dbo.tbl_Users_Assigned.WorkFlowType WHEN 1 THEN " +
                         " (SELECT Visible FROM  dbo.tbl_MRP_List_Workflow WHERE(MasterKey = dbo.tbl_Users_Assigned.MRPKey) AND(Line = dbo.tbl_Users_Assigned.WorkFlowLine)) ELSE " +
                         " (SELECT Visible FROM  dbo.tbl_MRP_List_Approval WHERE(MasterKey = dbo.tbl_Users_Assigned.MRPKey) AND(Line = dbo.tbl_Users_Assigned.WorkFlowLine)) END) AS Visible " +
                         " FROM dbo.tbl_MRP_List RIGHT OUTER JOIN " +
                         " dbo.tbl_Users_Assigned ON dbo.tbl_MRP_List.PK = dbo.tbl_Users_Assigned.MRPKey LEFT OUTER JOIN " +
                         " dbo.tbl_System_Approval_Position ON dbo.tbl_Users_Assigned.PositionNameKey = dbo.tbl_System_Approval_Position.PK LEFT OUTER JOIN " +
                         " dbo.tbl_MRP_Status ON dbo.tbl_MRP_List.StatusKey = dbo.tbl_MRP_Status.PK LEFT OUTER JOIN " +
                         " dbo.vw_AXEntityTable ON dbo.tbl_MRP_List.EntityCode = dbo.vw_AXEntityTable.ID LEFT OUTER JOIN " +
                         " dbo.vw_AXOperatingUnitTable ON dbo.tbl_MRP_List.BUCode = dbo.vw_AXOperatingUnitTable.OMOPERATINGUNITNUMBER " +
                         " WHERE(dbo.tbl_Users_Assigned.UserKey = " + usrkey + ") " +
                         " AND ((CASE dbo.tbl_Users_Assigned.WorkFlowType WHEN 1 THEN (SELECT Visible FROM  dbo.tbl_MRP_List_Workflow " +
                         " WHERE(MasterKey = dbo.tbl_Users_Assigned.MRPKey) AND(Line = dbo.tbl_Users_Assigned.WorkFlowLine)) ELSE " +
                         " (SELECT Visible FROM  dbo.tbl_MRP_List_Approval WHERE(MasterKey = dbo.tbl_Users_Assigned.MRPKey) AND(Line = dbo.tbl_Users_Assigned.WorkFlowLine)) END) = 1) " +
                         " AND((CASE dbo.tbl_Users_Assigned.WorkFlowType WHEN 1 THEN (SELECT Status FROM  dbo.tbl_MRP_List_Workflow WHERE(MasterKey = dbo.tbl_Users_Assigned.MRPKey) " +
                         " AND(Line = dbo.tbl_Users_Assigned.WorkFlowLine)) ELSE (SELECT Status FROM  dbo.tbl_MRP_List_Approval WHERE(MasterKey = dbo.tbl_Users_Assigned.MRPKey) AND(Line = dbo.tbl_Users_Assigned.WorkFlowLine)) END) = 0)";
            cmd = new SqlCommand(qry);
            cmd.Connection = conn;
            adp = new SqlDataAdapter(cmd);
            adp.Fill(dtable);
            if (dtable.Rows.Count > 0)
            {
                foreach (DataRow row in dtable.Rows)
                {
                    DataRow rowAdd = dtTable.NewRow();
                    rowAdd["PK"] = row["MRPKey"].ToString();
                    rowAdd["DocNumber"] = row["DocNumber"].ToString();
                    rowAdd["DateCreated"] = Convert.ToDateTime(row["DateCreated"]).ToString("MM/dd/yyyy");
                    rowAdd["EntityCodeDesc"] = row["Entity"].ToString();
                    rowAdd["BUCodeDesc"] = row["Dept"].ToString();
                    rowAdd["MRPMonthDesc"] = Month_Name(Convert.ToInt32(row["MRPMonth"]));
                    rowAdd["MRPYear"] = row["MRPYear"].ToString();
                    rowAdd["LevelLine"] = row["WorkFlowLine"].ToString();
                    rowAdd["LevelPosition"] = row["PositionName"].ToString();
                    rowAdd["Status"] = row["StatusName"].ToString();
                    rowAdd["WorkflowType"] = row["WorkFlowType"].ToString();
                    rowAdd["CreatorKey"] = row["CreatorKey"].ToString();
                    dtTable.Rows.Add(rowAdd);
                }
            }
            conn.Close();
            return dtTable;
        }

        public static DataTable MRP_ListBudget()
        {
            DataTable dtTable = new DataTable();
            SqlCommand cmd = null;
            SqlDataAdapter adp;
            DataTable dtable = new DataTable();

            if (dtTable.Columns.Count == 0)
            {
                dtTable.Columns.Add("PK", typeof(string));
                dtTable.Columns.Add("DocNumber", typeof(string));
                dtTable.Columns.Add("DateCreated", typeof(string));
                dtTable.Columns.Add("EntityCodeDesc", typeof(string));
                dtTable.Columns.Add("BUCodeDesc", typeof(string));
                dtTable.Columns.Add("MRPMonthDesc", typeof(string));
                dtTable.Columns.Add("MRPYear", typeof(string));
                dtTable.Columns.Add("WorkLine", typeof(string));
            }
            dtTable.Clear();

            SqlConnection conn = new SqlConnection(GlobalClass.SQLConnString());
            conn.Open();
            //string qry = "SELECT dbo.tbl_MRP_List.DocNumber, dbo.vw_AXEntityTable.NAME AS Entity, " +
            //             " dbo.vw_AXOperatingUnitTable.NAME AS Department, dbo.tbl_MRP_List_Workflow.MasterKey, " +
            //             " dbo.tbl_MRP_List.MRPMonth, dbo.tbl_MRP_List.MRPYear, dbo.tbl_MRP_List.DateCreated " +
            //             " FROM dbo.tbl_MRP_List_Workflow LEFT OUTER JOIN " +
            //             " dbo.tbl_MRP_List ON dbo.tbl_MRP_List_Workflow.MasterKey = dbo.tbl_MRP_List.PK LEFT OUTER JOIN " +
            //             " dbo.vw_AXEntityTable ON dbo.tbl_MRP_List.EntityCode = dbo.vw_AXEntityTable.ID LEFT OUTER JOIN " +
            //             " dbo.vw_AXOperatingUnitTable ON dbo.tbl_MRP_List.BUCode = dbo.vw_AXOperatingUnitTable.OMOPERATINGUNITNUMBER LEFT OUTER JOIN " +
            //             " dbo.tbl_MRP_Status ON dbo.tbl_MRP_List.StatusKey = dbo.tbl_MRP_Status.PK " +
            //             " WHERE(dbo.tbl_MRP_List_Workflow.Line = 3) AND(dbo.tbl_MRP_List_Workflow.Status = 0)";

            string qry = "SELECT dbo.tbl_MRP_List.DocNumber, dbo.vw_AXEntityTable.NAME AS Entity, " +
                         " dbo.vw_AXOperatingUnitTable.NAME AS Department, dbo.tbl_MRP_List_Workflow.MasterKey, " +
                         " dbo.tbl_MRP_List.MRPMonth, dbo.tbl_MRP_List.MRPYear, dbo.tbl_MRP_List.DateCreated, dbo.tbl_MRP_List_Workflow.Line " +
                         " FROM dbo.tbl_MRP_List_Workflow LEFT OUTER JOIN " +
                         " dbo.tbl_MRP_List ON dbo.tbl_MRP_List_Workflow.MasterKey = dbo.tbl_MRP_List.PK LEFT OUTER JOIN " +
                         " dbo.vw_AXEntityTable ON dbo.tbl_MRP_List.EntityCode = dbo.vw_AXEntityTable.ID LEFT OUTER JOIN " +
                         " dbo.vw_AXOperatingUnitTable ON dbo.tbl_MRP_List.BUCode = dbo.vw_AXOperatingUnitTable.OMOPERATINGUNITNUMBER LEFT OUTER JOIN " +
                         " dbo.tbl_MRP_Status ON dbo.tbl_MRP_List.StatusKey = dbo.tbl_MRP_Status.PK " +
                         " WHERE(dbo.tbl_MRP_List_Workflow.Line = 3) " +
                         " AND(dbo.tbl_MRP_List_Workflow.Status = 0) " +
                         " AND(dbo.tbl_MRP_List_Workflow.Visible = 1)";

            cmd = new SqlCommand(qry);
            cmd.Connection = conn;
            adp = new SqlDataAdapter(cmd);
            adp.Fill(dtable);
            if (dtable.Rows.Count > 0)
            {
                foreach (DataRow row in dtable.Rows)
                {
                    DataRow rowAdd = dtTable.NewRow();
                    rowAdd["PK"] = row["MasterKey"].ToString();
                    rowAdd["DocNumber"] = row["DocNumber"].ToString();
                    rowAdd["DateCreated"] = Convert.ToDateTime(row["DateCreated"]).ToString("MM/dd/yyyy");
                    rowAdd["EntityCodeDesc"] = row["Entity"].ToString();
                    rowAdd["BUCodeDesc"] = row["Department"].ToString();
                    rowAdd["MRPMonthDesc"] = Month_Name(Convert.ToInt32(row["MRPMonth"]));
                    rowAdd["MRPYear"] = row["MRPYear"].ToString();
                    rowAdd["WorkLine"] = row["Line"].ToString();
                    dtTable.Rows.Add(rowAdd);
                }
            }
            dtable.Clear();
            conn.Close();

            return dtTable;
        }

        public static DataTable MRP_ListForApproval()
        {
            DataTable dtTable = new DataTable();
            SqlCommand cmd = null;
            SqlDataAdapter adp;
            DataTable dtable = new DataTable();

            if (dtTable.Columns.Count == 0)
            {
                dtTable.Columns.Add("PK", typeof(string));
                dtTable.Columns.Add("DocNumber", typeof(string));
                dtTable.Columns.Add("DateCreated", typeof(string));
                dtTable.Columns.Add("EntityCodeDesc", typeof(string));
                dtTable.Columns.Add("BUCodeDesc", typeof(string));
                dtTable.Columns.Add("MRPMonthDesc", typeof(string));
                dtTable.Columns.Add("MRPYear", typeof(string));
                dtTable.Columns.Add("WorkLine", typeof(string));
            }
            dtTable.Clear();

            SqlConnection conn = new SqlConnection(GlobalClass.SQLConnString());
            conn.Open();
            string qry = "SELECT dbo.tbl_MRP_List.DocNumber, dbo.vw_AXEntityTable.NAME AS Entity, " +
                         " dbo.vw_AXOperatingUnitTable.NAME AS Department, dbo.tbl_MRP_List_Workflow.MasterKey, " +
                         " dbo.tbl_MRP_List.MRPMonth, dbo.tbl_MRP_List.MRPYear, dbo.tbl_MRP_List.DateCreated, dbo.tbl_MRP_List_Workflow.Line " +
                         " FROM dbo.tbl_MRP_List_Workflow LEFT OUTER JOIN " +
                         " dbo.tbl_MRP_List ON dbo.tbl_MRP_List_Workflow.MasterKey = dbo.tbl_MRP_List.PK LEFT OUTER JOIN " +
                         " dbo.vw_AXEntityTable ON dbo.tbl_MRP_List.EntityCode = dbo.vw_AXEntityTable.ID LEFT OUTER JOIN " +
                         " dbo.vw_AXOperatingUnitTable ON dbo.tbl_MRP_List.BUCode = dbo.vw_AXOperatingUnitTable.OMOPERATINGUNITNUMBER LEFT OUTER JOIN " +
                         " dbo.tbl_MRP_Status ON dbo.tbl_MRP_List.StatusKey = dbo.tbl_MRP_Status.PK " +
                         " WHERE(dbo.tbl_MRP_List_Workflow.Line = 3) " +
                         " AND(dbo.tbl_MRP_List_Workflow.Status = 0) " +
                         " AND(dbo.tbl_MRP_List_Workflow.Visible = 1)";

            cmd = new SqlCommand(qry);
            cmd.Connection = conn;
            adp = new SqlDataAdapter(cmd);
            adp.Fill(dtable);
            if (dtable.Rows.Count > 0)
            {
                foreach (DataRow row in dtable.Rows)
                {
                    DataRow rowAdd = dtTable.NewRow();
                    rowAdd["PK"] = row["MasterKey"].ToString();
                    rowAdd["DocNumber"] = row["DocNumber"].ToString();
                    rowAdd["DateCreated"] = Convert.ToDateTime(row["DateCreated"]).ToString("MM/dd/yyyy");
                    rowAdd["EntityCodeDesc"] = row["Entity"].ToString();
                    rowAdd["BUCodeDesc"] = row["Department"].ToString();
                    rowAdd["MRPMonthDesc"] = Month_Name(Convert.ToInt32(row["MRPMonth"]));
                    rowAdd["MRPYear"] = row["MRPYear"].ToString();
                    rowAdd["WorkLine"] = row["Line"].ToString();
                    dtTable.Rows.Add(rowAdd);
                }
            }
            dtable.Clear();
            conn.Close();

            return dtTable;
        }

        public static DataTable MRP_ListforDeliberation()
        {
            DataTable dtTable = new DataTable();
            SqlCommand cmd = null;
            SqlDataAdapter adp;
            DataTable dtable = new DataTable();

            if (dtTable.Columns.Count == 0)
            {
                dtTable.Columns.Add("PK", typeof(string));
                dtTable.Columns.Add("DocNumber", typeof(string));
                dtTable.Columns.Add("DateCreated", typeof(string));
                dtTable.Columns.Add("EntityCodeDesc", typeof(string));
                dtTable.Columns.Add("BUCodeDesc", typeof(string));
                dtTable.Columns.Add("MRPMonthDesc", typeof(string));
                dtTable.Columns.Add("MRPYear", typeof(string));
                dtTable.Columns.Add("WorkLine", typeof(string));
            }
            dtTable.Clear();

            SqlConnection conn = new SqlConnection(GlobalClass.SQLConnString());
            conn.Open();
            string qry = "SELECT dbo.tbl_MRP_List.DocNumber, dbo.vw_AXEntityTable.NAME AS Entity, " +
                         " dbo.vw_AXOperatingUnitTable.NAME AS Department, dbo.tbl_MRP_List_Workflow.MasterKey, " +
                         " dbo.tbl_MRP_List.MRPMonth, dbo.tbl_MRP_List.MRPYear, dbo.tbl_MRP_List.DateCreated, dbo.tbl_MRP_List_Workflow.Line " +
                         " FROM dbo.tbl_MRP_List_Workflow LEFT OUTER JOIN " +
                         " dbo.tbl_MRP_List ON dbo.tbl_MRP_List_Workflow.MasterKey = dbo.tbl_MRP_List.PK LEFT OUTER JOIN " +
                         " dbo.vw_AXEntityTable ON dbo.tbl_MRP_List.EntityCode = dbo.vw_AXEntityTable.ID LEFT OUTER JOIN " +
                         " dbo.vw_AXOperatingUnitTable ON dbo.tbl_MRP_List.BUCode = dbo.vw_AXOperatingUnitTable.OMOPERATINGUNITNUMBER LEFT OUTER JOIN " +
                         " dbo.tbl_MRP_Status ON dbo.tbl_MRP_List.StatusKey = dbo.tbl_MRP_Status.PK " +
                         " WHERE(dbo.tbl_MRP_List_Workflow.Line = 3) " +
                         " AND(dbo.tbl_MRP_List_Workflow.Status = 0) " +
                         " AND(dbo.tbl_MRP_List_Workflow.Visible = 1)";

            cmd = new SqlCommand(qry);
            cmd.Connection = conn;
            adp = new SqlDataAdapter(cmd);
            adp.Fill(dtable);
            if (dtable.Rows.Count > 0)
            {
                foreach (DataRow row in dtable.Rows)
                {
                    DataRow rowAdd = dtTable.NewRow();
                    rowAdd["PK"] = row["MasterKey"].ToString();
                    rowAdd["DocNumber"] = row["DocNumber"].ToString();
                    rowAdd["DateCreated"] = Convert.ToDateTime(row["DateCreated"]).ToString("MM/dd/yyyy");
                    rowAdd["EntityCodeDesc"] = row["Entity"].ToString();
                    rowAdd["BUCodeDesc"] = row["Department"].ToString();
                    rowAdd["MRPMonthDesc"] = Month_Name(Convert.ToInt32(row["MRPMonth"]));
                    rowAdd["MRPYear"] = row["MRPYear"].ToString();
                    rowAdd["WorkLine"] = row["Line"].ToString();
                    dtTable.Rows.Add(rowAdd);
                }
            }
            dtable.Clear();

            conn.Close();

            return dtTable;
        }

        public static int MRP_ApprvLine_Status(int masterKey, int line)
        {
            int aprvLineStat = 0;
            string qry = "";

            SqlConnection conn = new SqlConnection(GlobalClass.SQLConnString());
            SqlCommand cmd = null;
            SqlDataAdapter adp;
            DataTable dtable = new DataTable();

            conn.Open();
            qry = "SELECT Status " +
                  " FROM tbl_MRP_List_Approval " +
                  " WHERE (MasterKey = " + masterKey + ") " +
                  " AND (Line = " + line + ")";
            cmd = new SqlCommand(qry);
            cmd.Connection = conn;
            adp = new SqlDataAdapter(cmd);
            adp.Fill(dtable);
            if (dtable.Rows.Count > 0)
            {
                foreach (DataRow row in dtable.Rows)
                {
                    aprvLineStat = Convert.ToInt32(row["Status"]);
                }
            }
            dtable.Clear();
            conn.Close();
            return aprvLineStat;
        }

        public static DataTable PO_ItemCodes(string docnumber)
        {
            DataTable dtTable = new DataTable();
            SqlCommand cmd = null;
            SqlDataAdapter adp, adp2;
            DataTable dtable = new DataTable();
            DataTable dtable2 = new DataTable();

            if (dtTable.Columns.Count == 0)
            {
                dtTable.Columns.Add("PK", typeof(string));
                dtTable.Columns.Add("Identifier", typeof(string));
                dtTable.Columns.Add("ItemCode", typeof(string));
                dtTable.Columns.Add("Description", typeof(string));
                dtTable.Columns.Add("UOM", typeof(string));
                dtTable.Columns.Add("Cost", typeof(string));
                dtTable.Columns.Add("Qty", typeof(string));
                dtTable.Columns.Add("TotalCost", typeof(string));
                dtTable.Columns.Add("Type", typeof(string));
            }
            dtTable.Clear();

            SqlConnection conn = new SqlConnection(GlobalClass.SQLConnString());
            conn.Open();
            string qry = "SELECT dbo.tbl_MRP_List_OPEX.PK, dbo.tbl_MRP_List_OPEX.TableIdentifier, dbo.tbl_MRP_List_OPEX.ItemCode, dbo.tbl_MRP_List_OPEX.Description, dbo.tbl_MRP_List_OPEX.UOM, dbo.tbl_MRP_List_OPEX.ApprovedCost, dbo.tbl_MRP_List_OPEX.AvailForPO FROM dbo.tbl_MRP_List LEFT OUTER JOIN dbo.tbl_MRP_List_OPEX ON dbo.tbl_MRP_List.DocNumber = dbo.tbl_MRP_List_OPEX.HeaderDocNum WHERE(dbo.tbl_MRP_List.DocNumber = '" + docnumber + "') AND(dbo.tbl_MRP_List_OPEX.AvailForPO > '0')";

            string qry2 = "SELECT dbo.tbl_MRP_List_DirectMaterials.PK, dbo.tbl_MRP_List_DirectMaterials.TableIdentifier, dbo.tbl_MRP_List_DirectMaterials.ItemCode, dbo.tbl_MRP_List_DirectMaterials.ItemDescription, dbo.tbl_MRP_List_DirectMaterials.UOM, dbo.tbl_MRP_List_DirectMaterials.ApprovedCost, dbo.tbl_MRP_List_DirectMaterials.AvailForPO FROM dbo.tbl_MRP_List LEFT OUTER JOIN dbo.tbl_MRP_List_DirectMaterials ON dbo.tbl_MRP_List.DocNumber = dbo.tbl_MRP_List_DirectMaterials.HeaderDocNum WHERE(dbo.tbl_MRP_List.DocNumber = '" + docnumber + "') AND (dbo.tbl_MRP_List_DirectMaterials.AvailForPO > '0')";

            cmd = new SqlCommand(qry);
            cmd.Connection = conn;
            adp = new SqlDataAdapter(cmd);
            adp.Fill(dtable);
            if (dtable.Rows.Count > 0)
            {
                foreach (DataRow row in dtable.Rows)
                {
                    DataRow rowAdd = dtTable.NewRow();
                    rowAdd["PK"] = row["PK"].ToString();
                    rowAdd["Identifier"] = row["TableIdentifier"].ToString();
                    rowAdd["ItemCode"] = row["ItemCode"].ToString();
                    rowAdd["Description"] = row["Description"].ToString();
                    rowAdd["UOM"] = row["UOM"].ToString();
                    rowAdd["Cost"] = row["ApprovedCost"].ToString();
                    rowAdd["Qty"] = row["AvailForPO"].ToString();
                    rowAdd["TotalCost"] = (Convert.ToDouble(row["AvailForPO"].ToString()) * Convert.ToDouble(row["ApprovedCost"].ToString())).ToString();
                    rowAdd["Type"] = "OPEX";
                    dtTable.Rows.Add(rowAdd);
                }
            }

            cmd = new SqlCommand(qry2);
            cmd.Connection = conn;
            adp2 = new SqlDataAdapter(cmd);
            adp2.Fill(dtable2);
            if (dtable2.Rows.Count > 0)
            {
                foreach (DataRow row in dtable2.Rows)
                {
                    DataRow rowAdd = dtTable.NewRow();
                    rowAdd["PK"] = row["PK"].ToString();
                    rowAdd["Identifier"] = row["TableIdentifier"].ToString();
                    rowAdd["ItemCode"] = row["ItemCode"].ToString();
                    rowAdd["Description"] = row["ItemDescription"].ToString();
                    rowAdd["UOM"] = row["UOM"].ToString();
                    rowAdd["Cost"] = row["ApprovedCost"].ToString();
                    rowAdd["Qty"] = row["AvailForPO"].ToString();
                    rowAdd["TotalCost"] = (Convert.ToDouble(row["AvailForPO"].ToString()) * Convert.ToDouble(row["ApprovedCost"].ToString())).ToString();
                    rowAdd["Type"] = "Direct Materials";
                    dtTable.Rows.Add(rowAdd);
                }
            }


            dtable.Clear();

            conn.Close();

            return dtTable;
        }


        public static DataTable PO_Creation_Details(string ponumber)
        {
            DataTable dtTable = new DataTable();
            SqlCommand cmd = null;
            SqlDataAdapter adp, adp2;
            DataTable dtable = new DataTable();
            DataTable dtable2 = new DataTable();

            if (dtTable.Columns.Count == 0)
            {
                dtTable.Columns.Add("PK", typeof(string));
                dtTable.Columns.Add("ItemPK", typeof(string));
                dtTable.Columns.Add("Identifier", typeof(string));
                dtTable.Columns.Add("ItemCode", typeof(string));
                dtTable.Columns.Add("TaxGroup", typeof(string));
                dtTable.Columns.Add("TaxItemGroup", typeof(string));
                dtTable.Columns.Add("UOM", typeof(string));
                dtTable.Columns.Add("Cost", typeof(string));
                dtTable.Columns.Add("Qty", typeof(string));
                dtTable.Columns.Add("TotalCost", typeof(string));
                dtTable.Columns.Add("AvailForPO", typeof(string));
            }
            dtTable.Clear();

            SqlConnection conn = new SqlConnection(GlobalClass.SQLConnString());
            conn.Open();

            //string qry = "SELECT dbo.tbl_POCreation_Details.*, dbo.tbl_MRP_List_DirectMaterials.UOM AS DM_UOM, dbo.tbl_MRP_List_OPEX.UOM AS OP_UOM FROM   dbo.tbl_POCreation LEFT OUTER JOIN dbo.tbl_POCreation_Details ON dbo.tbl_POCreation.PONumber = dbo.tbl_POCreation_Details.PONumber LEFT OUTER JOIN dbo.tbl_MRP_List_OPEX ON dbo.tbl_POCreation_Details.ItemPK = dbo.tbl_MRP_List_OPEX.PK AND dbo.tbl_POCreation_Details.Identifier = dbo.tbl_MRP_List_OPEX.TableIdentifier LEFT OUTER JOIN dbo.tbl_MRP_List_DirectMaterials ON dbo.tbl_POCreation_Details.ItemPK = dbo.tbl_MRP_List_DirectMaterials.PK AND dbo.tbl_POCreation_Details.Identifier = dbo.tbl_MRP_List_DirectMaterials.TableIdentifier";

            string qry = "SELECT dbo.tbl_POCreation_Details.*, dbo.tbl_MRP_List_DirectMaterials.UOM, dbo.tbl_MRP_List_DirectMaterials.AvailForPO FROM dbo.tbl_POCreation_Details LEFT OUTER JOIN dbo.tbl_MRP_List_DirectMaterials ON dbo.tbl_POCreation_Details.ItemPK = dbo.tbl_MRP_List_DirectMaterials.PK CROSS JOIN dbo.tbl_MRP_List_OPEX";

            cmd = new SqlCommand(qry);
            cmd.Connection = conn;
            adp = new SqlDataAdapter(cmd);
            adp.Fill(dtable);
            if (dtable.Rows.Count > 0)
            {
                foreach (DataRow row in dtable.Rows)
                {
                    DataRow rowAdd = dtTable.NewRow();
                    rowAdd["PK"] = row["PK"].ToString();
                    rowAdd["ItemPK"] = row["ItemPK"].ToString();
                    rowAdd["Identifier"] = row["Identifier"].ToString();
                    rowAdd["ItemCode"] = row["ItemCode"].ToString();
                    rowAdd["TaxGroup"] = row["TaxGroup"].ToString();
                    rowAdd["TaxItemGroup"] = row["TaxItemGroup"].ToString();
                    rowAdd["Cost"] = row["Cost"].ToString();
                    rowAdd["Qty"] = row["Qty"].ToString();
                    rowAdd["TotalCost"] = row["TotalCost"].ToString();
                    rowAdd["AvailForPO"] = row["AvailForPO"].ToString();
                    rowAdd["UOM"] = row["UOM"].ToString();
                    //if (string.IsNullOrEmpty(row["DM_UOM"].ToString()))
                    //    rowAdd["UOM"] = row["OP_UOM"].ToString();
                    //else
                    //    rowAdd["UOM"] = row["DM_UOM"].ToString();
                    dtTable.Rows.Add(rowAdd);
                }
            }

            string qry2 = "SELECT dbo.tbl_MRP_List_OPEX.AvailForPO, dbo.tbl_POCreation_Details.*, dbo.tbl_MRP_List_OPEX.UOM FROM dbo.tbl_POCreation_Details LEFT OUTER JOIN dbo.tbl_MRP_List_OPEX ON dbo.tbl_POCreation_Details.ItemPK = dbo.tbl_MRP_List_OPEX.PK";

            cmd = new SqlCommand(qry2);
            cmd.Connection = conn;
            adp2 = new SqlDataAdapter(cmd);
            adp2.Fill(dtable2);
            if (dtable2.Rows.Count > 0)
            {
                foreach (DataRow row in dtable2.Rows)
                {
                    DataRow rowAdd = dtTable.NewRow();
                    rowAdd["PK"] = row["PK"].ToString();
                    rowAdd["ItemPK"] = row["ItemPK"].ToString();
                    rowAdd["Identifier"] = row["Identifier"].ToString();
                    rowAdd["ItemCode"] = row["ItemCode"].ToString();
                    rowAdd["TaxGroup"] = row["TaxGroup"].ToString();
                    rowAdd["TaxItemGroup"] = row["TaxItemGroup"].ToString();
                    rowAdd["Cost"] = row["Cost"].ToString();
                    rowAdd["Qty"] = row["Qty"].ToString();
                    rowAdd["TotalCost"] = row["TotalCost"].ToString();
                    rowAdd["AvailForPO"] = row["AvailForPO"].ToString();
                    rowAdd["UOM"] = row["UOM"].ToString();
                    //if (string.IsNullOrEmpty(row["DM_UOM"].ToString()))
                    //    rowAdd["UOM"] = row["OP_UOM"].ToString();
                    //else
                    //    rowAdd["UOM"] = row["DM_UOM"].ToString();
                    dtTable.Rows.Add(rowAdd);
                }
            }

            dtable.Clear();

            conn.Close();

            return dtTable;
        }

        public static void trial()
        {
            string query_arraycode = "SELECT DISTINCT ActivityCode FROM [dbo].[tbl_MRP_List_DirectMaterials] WHERE HeaderDocNum = '0000-0119MRP-000019' ORDER BY ActivityCode ASC";

            SqlConnection conn = new SqlConnection(GlobalClass.SQLConnString());
            conn.Open();

            SqlCommand cmd = new SqlCommand(query_arraycode, conn);
            SqlDataReader reader = cmd.ExecuteReader();
            ArrayList list = new ArrayList();
            while (reader.Read())
            {
                list.Add(reader[0].ToString());
            }
            reader.Close();

            for (int i = 0; i < list.Count; i++)
            {
                string query = "SELECT * FROM [dbo].[tbl_MRP_List_DirectMaterials] WHERE HeaderDocNum = '0000-0119MRP-000019' AND ActivityCode = '" + list[i] + "' ";

                cmd = new SqlCommand(query, conn);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    PrintString("array list:" + list[i]);
                    PrintString(reader["ActivityCode"].ToString() + ": " + reader["ItemCode"].ToString() + "---" + reader["ItemDescription"].ToString());
                }
                reader.Close();

            }
        }

        public static DataTable POListDetails(string mopNum)
        {
            DataTable dtTable = new DataTable();
            SqlConnection cn = new SqlConnection(GlobalClass.SQLConnString());
            DataTable dt = new DataTable();
            SqlCommand cmd = null;
            SqlDataAdapter adp;
            string qry = "";
            DataTable dt1 = new DataTable();
            SqlCommand cmd1 = null;
            SqlDataAdapter adp1;

            cn.Open();
            if (dtTable.Columns.Count == 0)
            {
                //Columns for AspxGridview
                dtTable.Columns.Add("PK", typeof(string));
                dtTable.Columns.Add("PONumber", typeof(string));
                dtTable.Columns.Add("POStatus", typeof(string));
                dtTable.Columns.Add("POAXStatus", typeof(string));
                dtTable.Columns.Add("Total", typeof(string));
            }
            qry = "SELECT dbo.tbl_POCreation.* FROM dbo.tbl_POCreation WHERE(MRPNumber = '" + mopNum + "')";
            cmd = new SqlCommand(qry);
            cmd.Connection = cn;
            adp = new SqlDataAdapter(cmd);
            adp.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    DataRow dtRow = dtTable.NewRow();
                    dtRow["PK"] = row["PK"].ToString();
                    dtRow["PONumber"] = row["PONumber"].ToString();
                    if (Convert.ToInt32(row["POStatus"]) == 0)
                    {
                        dtRow["POStatus"] = "Created";
                    }
                    else
                    {
                        dtRow["POStatus"] = "Submitted";
                    }
                    dtRow["POAXStatus"] = "";

                    qry = "SELECT ISNULL(SUM(TotalCost), 0) AS Total FROM dbo.tbl_POCreation_Details WHERE(PONumber = '" + row["PONumber"].ToString() + "')";
                    cmd1 = new SqlCommand(qry);
                    cmd1.Connection = cn;
                    adp1 = new SqlDataAdapter(cmd1);
                    adp1.Fill(dt1);
                    if (dt1.Rows.Count > 0)
                    {
                        foreach (DataRow row1 in dt1.Rows)
                        {
                            dtRow["Total"] = Convert.ToDouble(row1["Total"]).ToString("#,##0.00");
                        }
                    }
                    dt1.Clear();
                    dtTable.Rows.Add(dtRow);
                }
            }
            dt.Clear();
            return dtTable;
        }

        public static DataTable POListDetailsLines()
        {
            DataTable dtTable = new DataTable();
            SqlConnection cn = new SqlConnection(GlobalClass.SQLConnString());
            DataTable dt = new DataTable();
            SqlCommand cmd = null;
            SqlDataAdapter adp;
            string qry = "";
            DataTable dt1 = new DataTable();
            SqlCommand cmd1 = null;
            SqlDataAdapter adp1;

            return dtTable;
        }

        public static DataTable MonthYear(string entitycode, string bucode)
        {
            DataTable dtTable = new DataTable();
            SqlConnection cn = new SqlConnection(GlobalClass.SQLConnString());
            DataTable dt = new DataTable();
            SqlCommand cmd = null;
            SqlDataAdapter adp;

            cn.Open();
            if (dtTable.Columns.Count == 0)
            {
                //Columns for AspxGridview
                dtTable.Columns.Add("PK", typeof(string));
                dtTable.Columns.Add("DocNumber", typeof(string));
                dtTable.Columns.Add("MRPMonth", typeof(string));
                dtTable.Columns.Add("MRPYear", typeof(string));

            }

            string query = "SELECT [PK],[DocNumber],[MRPMonth],[MRPYear] FROM [dbo].[tbl_MRP_List] WHERE EntityCode = '" + entitycode + "' AND BUCode = '" + bucode + "'";

            cmd = new SqlCommand(query);
            cmd.Connection = cn;
            adp = new SqlDataAdapter(cmd);
            adp.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    DataRow dtRow = dtTable.NewRow();
                    dtRow["PK"] = row["PK"].ToString();
                    dtRow["DocNumber"] = row["DocNumber"].ToString();
                    dtRow["MRPMonth"] = Convertion.INDEX_TO_MONTH(Convert.ToInt32(row["MRPMonth"].ToString()));
                    dtRow["MRPYear"] = row["MRPYear"].ToString();
                    dtTable.Rows.Add(dtRow);
                }
            }
            dt.Clear();
            cn.Close();
            return dtTable;
        }

        public static bool PreviewRights(int CreatorKey, string DocNum, int WorkLine)
        {
            bool isAllowed = false;

            SqlConnection cn = new SqlConnection(GlobalClass.SQLConnString());
            DataTable dt = new DataTable();
            SqlCommand cmd = null;
            SqlDataAdapter adp;
            cn.Open();
            string query = "SELECT tbl_MRP_List.* FROM tbl_MRP_List WHERE (DocNumber = '"+ DocNum + "')";
            cmd = new SqlCommand(query);
            cmd.Connection = cn;
            adp = new SqlDataAdapter(cmd);
            adp.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    if (Convert.ToInt32(row["CreatorKey"]) != CreatorKey)
                    {
                        switch (WorkLine)
                        {
                            case 1: //BU Lead
                                {
                                    if (GlobalClass.IsSuperAdmin(CreatorKey))
                                    {
                                        isAllowed = true;
                                    } else
                                    {
                                        isAllowed = GlobalClass.IsAllowed(CreatorKey, "MOPBULead", Convert.ToDateTime(row["DateCreated"]), row["EntityCode"].ToString(), row["BUCode"].ToString(), "");
                                    }
                                    
                                    break;
                                }
                            case 2: //Inventory Analyst
                                {
                                    if (GlobalClass.IsSuperAdmin(CreatorKey))
                                    {
                                        isAllowed = true;
                                    } else
                                    {
                                        isAllowed = GlobalClass.IsAllowed(CreatorKey, "MOPInventoryAnalyst", Convert.ToDateTime(row["DateCreated"]), row["EntityCode"].ToString(), row["BUCode"].ToString(), "");
                                    }
                                        
                                    break;
                                }
                            case 3: //Inventory Analyst (Deliberation)
                                {
                                    if (GlobalClass.IsSuperAdmin(CreatorKey))
                                    {
                                        isAllowed = true;
                                    } else
                                    {
                                        isAllowed = GlobalClass.IsAllowed(CreatorKey, "MOPInventoryAnalyst", Convert.ToDateTime(row["DateCreated"]), row["EntityCode"].ToString(), row["BUCode"].ToString(), "");
                                    }
                                    
                                    break;
                                }
                        }
                        
                    } else
                    {
                        isAllowed = true;
                    }
                }
            }
            dt.Clear();
            cn.Close();

            return isAllowed;
        }

        public static bool PreviewApprovalRights(int CreatorKey, string DocNum)
        {
            bool isAllowed = false;

            SqlConnection cn = new SqlConnection(GlobalClass.SQLConnString());
            DataTable dt = new DataTable();
            SqlCommand cmd = null;
            SqlDataAdapter adp;
            cn.Open();
            string query = "SELECT tbl_MRP_List.* FROM tbl_MRP_List WHERE (DocNumber = '" + DocNum + "')";
            cmd = new SqlCommand(query);
            cmd.Connection = cn;
            adp = new SqlDataAdapter(cmd);
            adp.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    if (GlobalClass.IsSuperAdmin(CreatorKey))
                    {
                        isAllowed = true;
                        goto RetAllowed;
                    } else
                    {
                        if (GlobalClass.IsAllowed(CreatorKey, "MOPSCMLead", Convert.ToDateTime(row["DateCreated"]), row["EntityCode"].ToString(), row["BUCode"].ToString(), "") == true)
                        {
                            isAllowed = true;
                            goto RetAllowed;
                        }
                        if (GlobalClass.IsAllowed(CreatorKey, "MOPFinanceLead", Convert.ToDateTime(row["DateCreated"]), row["EntityCode"].ToString(), row["BUCode"].ToString(), "") == true)
                        {
                            isAllowed = true;
                            goto RetAllowed;
                        }
                        if (GlobalClass.IsAllowed(CreatorKey, "MOPExecutive", Convert.ToDateTime(row["DateCreated"]), row["EntityCode"].ToString(), row["BUCode"].ToString(), "") == true)
                        {
                            isAllowed = true;
                            goto RetAllowed;
                        }
                    }
                    
                }
            }
            dt.Clear();
            cn.Close();

            RetAllowed:
            return isAllowed;
        }

        public static DataTable DM_Preview_AtSource(string docnumber, string entitycode)
        {
            DataTable dtTable = new DataTable();
            dtTable.TableName = "Table";

            SqlConnection cn = new SqlConnection(GlobalClass.SQLConnString());
            DataTable dt = new DataTable();
            SqlCommand cmd = null;
            //SqlDataReader reader = null;
            SqlDataAdapter adp;
            cn.Open();

            if (dtTable.Columns.Count == 0)
            {
                //Columns for AspxGridview
                dtTable.Columns.Add("PK", typeof(string));
                dtTable.Columns.Add("OperatingUnit", typeof(string));
                dtTable.Columns.Add("Expense", typeof(string));
                dtTable.Columns.Add("Activity", typeof(string));
                dtTable.Columns.Add("Descripiton", typeof(string));
                dtTable.Columns.Add("UOM", typeof(string));
                dtTable.Columns.Add("Qty", typeof(string));
                dtTable.Columns.Add("Cost", typeof(string));
                dtTable.Columns.Add("TotalCost", typeof(string));
                //dtTable.Columns.Add("RecQty", typeof(string));
                //dtTable.Columns.Add("RecCost", typeof(string));
                //dtTable.Columns.Add("RecTotalCost", typeof(string));
            }
            string farm_query = "[dbo].[DirectMaterialPreview]";
            cmd = new SqlCommand(farm_query, cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@headerdocnum", docnumber);
            cmd.Parameters.AddWithValue("@entity", entitycode);
            //cmd.ExecuteNonQuery();

            adp = new SqlDataAdapter(cmd);
            adp.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    DataRow dtRow = dtTable.NewRow();
                    dtRow["PK"] = row["PK"].ToString();
                    dtRow["OperatingUnit"] = row["OperUinit"].ToString();
                    dtRow["Activity"] = row["Activity"].ToString();
                    dtRow["Expense"] = row["ExpenseCode"].ToString();
                    dtRow["Descripiton"] = row["ItemDescription"].ToString();
                    dtRow["UOM"] = row["UOM"].ToString();
                    dtRow["Qty"] = Convert.ToDouble(row["Qty"].ToString()).ToString("N");
                    dtRow["Cost"] = Convert.ToDouble(row["Cost"].ToString()).ToString("N");
                    dtRow["TotalCost"] = Convert.ToDouble(row["TotalCost"].ToString()).ToString("N");
                    //dtRow["RecQty"] = Convert.ToDouble(row["EdittedQty"].ToString()).ToString("N");
                    //dtRow["RecCost"] = Convert.ToDouble(row["EdittedCost"].ToString()).ToString("N");
                    //dtRow["RecTotalCost"] = Convert.ToDouble(row["EdittiedTotalCost"].ToString()).ToString("N");
                    dtTable.Rows.Add(dtRow);
                }
            }
            dt.Clear();
            cn.Close();
            return dtTable;
        }

        public static DataTable OP_Preview_AtSource(string docnumber, string entitycode)
        {
            DataTable dtTable = new DataTable();
            dtTable.TableName = "Table";

            SqlConnection cn = new SqlConnection(GlobalClass.SQLConnString());
            System.Data.DataTable dt = new System.Data.DataTable();
            SqlCommand cmd = null;
            SqlDataAdapter adp;

            cn.Open();

            if (dtTable.Columns.Count == 0)
            {
                //Columns for AspxGridview
                dtTable.Columns.Add("PK", typeof(string));
                dtTable.Columns.Add("OperatingUnit", typeof(string));
                dtTable.Columns.Add("Expense", typeof(string));
                dtTable.Columns.Add("ProcurementCategory", typeof(string));
                dtTable.Columns.Add("Descripiton", typeof(string));
                dtTable.Columns.Add("UOM", typeof(string));
                dtTable.Columns.Add("Qty", typeof(string));
                dtTable.Columns.Add("Cost", typeof(string));
                dtTable.Columns.Add("TotalCost", typeof(string));

                //dtTable.Columns.Add("RecQty", typeof(string));
                //dtTable.Columns.Add("RecCost", typeof(string));
                //dtTable.Columns.Add("RecTotalCost", typeof(string));
            }

            string farm_query = "[dbo].[OPEXPreview]";
            cmd = new SqlCommand(farm_query, cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@headerdocnum", docnumber);
            cmd.Parameters.AddWithValue("@entity", entitycode);
            //cmd.ExecuteNonQuery();

            adp = new SqlDataAdapter(cmd);
            adp.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    DataRow dtRow = dtTable.NewRow();
                    dtRow["PK"] = row["PK"].ToString(); ;
                    dtRow["OperatingUnit"] = row["OprUnit"].ToString();
                    dtRow["ProcurementCategory"] = row["ProcCat"].ToString();
                    dtRow["Expense"] = row["ExpenseCode"].ToString();
                    dtRow["Descripiton"] = row["OPEX_Description"].ToString();
                    dtRow["UOM"] = row["UOMDesc"].ToString();
                    dtRow["Qty"] = Convert.ToDouble(row["Qty"].ToString()).ToString("N");
                    dtRow["Cost"] = Convert.ToDouble(row["Cost"].ToString()).ToString("N");
                    dtRow["TotalCost"] = Convert.ToDouble(row["TotalCost"].ToString()).ToString("N");

                    //dtRow["RecQty"] = Convert.ToDouble(row["EdittedQty"].ToString()).ToString("N");
                    //dtRow["RecCost"] = Convert.ToDouble(row["EdittedCost"].ToString()).ToString("N");
                    //dtRow["RecTotalCost"] = Convert.ToDouble(row["EdittedTotalCost"].ToString()).ToString("N");
                    dtTable.Rows.Add(dtRow);
                }
            }
            dt.Clear();
            cn.Close();
            return dtTable;
        }


        public static DataTable MAN_Preview_AtSource(string docnumber, string entitycode)
        {
            DataTable dtTable = new DataTable();
            dtTable.TableName = "Table";

            SqlConnection cn = new SqlConnection(GlobalClass.SQLConnString());
            System.Data.DataTable dt = new System.Data.DataTable();
            SqlCommand cmd = null;
            SqlDataReader reader = null;
            SqlDataAdapter adp;

            cn.Open();

            if (dtTable.Columns.Count == 0)
            {
                //Columns for AspxGridview
                dtTable.Columns.Add("PK", typeof(string));
                dtTable.Columns.Add("OperatingUnit", typeof(string));
                dtTable.Columns.Add("Activity", typeof(string));
                dtTable.Columns.Add("Descripiton", typeof(string));
                dtTable.Columns.Add("UOM", typeof(string));
                dtTable.Columns.Add("Qty", typeof(string));
                dtTable.Columns.Add("Cost", typeof(string));
                dtTable.Columns.Add("TotalCost", typeof(string));

                //dtTable.Columns.Add("RecQty", typeof(string));
                //dtTable.Columns.Add("RecCost", typeof(string));
                //dtTable.Columns.Add("RecTotalCost", typeof(string));
            }

            string farm_query = "[dbo].[ManPowerPreview]";
            cmd = new SqlCommand(farm_query, cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@headerdocnum", docnumber);
            cmd.Parameters.AddWithValue("@entity", entitycode);
            //cmd.ExecuteNonQuery();

            adp = new SqlDataAdapter(cmd);
            adp.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    DataRow dtRow = dtTable.NewRow();
                    dtRow["PK"] = row["PK"].ToString(); ;
                    dtRow["OperatingUnit"] = row["OprUnitDesc"].ToString();
                    dtRow["Activity"] = row["ActivityName"].ToString();
                    dtRow["Descripiton"] = row["Description"].ToString();
                    dtRow["UOM"] = row["UOMDesc"].ToString();
                    dtRow["Qty"] = Convert.ToDouble(row["Qty"].ToString()).ToString("N");
                    dtRow["Cost"] = Convert.ToDouble(row["Cost"].ToString()).ToString("N");
                    dtRow["TotalCost"] = Convert.ToDouble(row["TotalCost"].ToString()).ToString("N");

                    //dtRow["RecQty"] = Convert.ToDouble(row["EdittedQty"].ToString()).ToString("N");
                    //dtRow["RecCost"] = Convert.ToDouble(row["EdittedCost"].ToString()).ToString("N");
                    //dtRow["RecTotalCost"] = Convert.ToDouble(row["EdittiedTotalCost"].ToString()).ToString("N");
                    dtTable.Rows.Add(dtRow);
                }
            }
            dt.Clear();
            cn.Close();
            return dtTable;

        }

        public static DataTable CA_Preview_AtSource(string docnumber, string entitycode)
        {
            DataTable dtTable = new DataTable();
            dtTable.TableName = "Table";

            SqlConnection cn = new SqlConnection(GlobalClass.SQLConnString());
            System.Data.DataTable dt = new System.Data.DataTable();
            SqlCommand cmd = null;
            SqlDataAdapter adp;

            cn.Open();

            if (dtTable.Columns.Count == 0)
            {
                //Columns for AspxGridview
                dtTable.Columns.Add("PK", typeof(string));
                dtTable.Columns.Add("OperatingUnit", typeof(string));
                dtTable.Columns.Add("Expense", typeof(string));
                dtTable.Columns.Add("ProcurementCategory", typeof(string));
                dtTable.Columns.Add("Descripiton", typeof(string));
                dtTable.Columns.Add("UOM", typeof(string));
                dtTable.Columns.Add("Qty", typeof(string));
                dtTable.Columns.Add("Cost", typeof(string));
                dtTable.Columns.Add("TotalCost", typeof(string));

                //dtTable.Columns.Add("RecQty", typeof(string));
                //dtTable.Columns.Add("RecCost", typeof(string));
                //dtTable.Columns.Add("RecTotalCost", typeof(string));

            }

            string farm_query = "[dbo].[CAPEXPreview]";
            cmd = new SqlCommand(farm_query, cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@headerdocnum", docnumber);
            cmd.Parameters.AddWithValue("@entity", entitycode);
            //cmd.ExecuteNonQuery();

            adp = new SqlDataAdapter(cmd);
            adp.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    DataRow dtRow = dtTable.NewRow();
                    dtRow["PK"] = row["PK"].ToString();
                    dtRow["OperatingUnit"] = row["OprUnit"].ToString();
                    dtRow["ProcurementCategory"] = row["ProdCat"].ToString();
                    dtRow["Descripiton"] = row["Description"].ToString();
                    dtRow["UOM"] = row["UOM"].ToString();
                    dtRow["Qty"] = Convert.ToDouble(row["Qty"].ToString()).ToString("N");
                    dtRow["Cost"] = Convert.ToDouble(row["Cost"].ToString()).ToString("N");
                    dtRow["TotalCost"] = Convert.ToDouble(row["TotalCost"].ToString()).ToString("N");

                    //dtRow["RecQty"] = Convert.ToDouble(row["EdittedQty"].ToString()).ToString("N");
                    //dtRow["RecCost"] = Convert.ToDouble(row["EdittedCost"].ToString()).ToString("N");
                    //dtRow["RecTotalCost"] = Convert.ToDouble(row["EdittiedTotalCost"].ToString()).ToString("N");
                    dtTable.Rows.Add(dtRow);
                }
            }
            dt.Clear();
            cn.Close();
            return dtTable;

        }


        public static DataTable REV_Preview(string docnumber, string entitycode)
        {
            DataTable dtTable = new DataTable();
            dtTable.TableName = "Table";

            SqlConnection cn = new SqlConnection(GlobalClass.SQLConnString());
            System.Data.DataTable dt = new System.Data.DataTable();
            SqlCommand cmd = null;
            SqlDataAdapter adp;

            cn.Open();

            if (dtTable.Columns.Count == 0)
            {
                //Columns for AspxGridview
                dtTable.Columns.Add("PK", typeof(string));
                dtTable.Columns.Add("OperatingUnit", typeof(string));
                dtTable.Columns.Add("ProductName", typeof(string));
                dtTable.Columns.Add("FarmName", typeof(string));
                dtTable.Columns.Add("Price", typeof(string));
                dtTable.Columns.Add("Volume", typeof(string));
                dtTable.Columns.Add("TotalPrice", typeof(string));
            }

            string farm_query = "[dbo].[RevenueAssumptionPreview]";
            cmd = new SqlCommand(farm_query, cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@headerdocnum", docnumber);
            cmd.Parameters.AddWithValue("@entity", entitycode);
            //cmd.ExecuteNonQuery();

            adp = new SqlDataAdapter(cmd);
            adp.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    DataRow dtRow = dtTable.NewRow();
                    dtRow["PK"] = row["PK"].ToString();
                    dtRow["OperatingUnit"] = row["OperUinit"].ToString();
                    dtRow["ProductName"] = row["ProductName"].ToString();
                    dtRow["FarmName"] = row["FarmName"].ToString();
                    dtRow["Price"] = Convert.ToDouble(row["Price"].ToString()).ToString("N");
                    dtRow["Volume"] = Convert.ToDouble(row["Volume"].ToString()).ToString("N");
                    dtRow["TotalPrice"] = Convert.ToDouble(row["TotalPrice"].ToString()).ToString("N");
                    dtTable.Rows.Add(dtRow);
                }
            }
            dt.Clear();
            cn.Close();
            return dtTable;

        }

        public static DataTable SUMMARY_Preview_AtSource(string docnumber, string entitycode)
        {
            DataTable dtTable = new DataTable();
            dtTable.TableName = "Table";

            SqlConnection cn = new SqlConnection(GlobalClass.SQLConnString());
            System.Data.DataTable dt = new System.Data.DataTable();
            SqlCommand cmd = null;
            SqlDataAdapter adp;

            cn.Open();

            if (dtTable.Columns.Count == 0)
            {
                //Columns for AspxGridview
                dtTable.Columns.Add("PK", typeof(string));
                dtTable.Columns.Add("Description", typeof(string));
                dtTable.Columns.Add("Total", typeof(string));
            }

            string farm_query = "[dbo].[MRPSummaryPreview]";
            cmd = new SqlCommand(farm_query, cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@headerdocnum", docnumber);
            cmd.Parameters.AddWithValue("@entity", entitycode);

            adp = new SqlDataAdapter(cmd);
            adp.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    DataRow dtRow = dtTable.NewRow();
                    dtRow["PK"] = row["Sort"].ToString();
                    dtRow["Description"] = row["MRPGroup"].ToString();
                    dtRow["Total"] = Convert.ToDouble(row["TotalCost"].ToString()).ToString("N");
                    dtTable.Rows.Add(dtRow);
                }
            }
            dt.Clear();
            cn.Close();
            return dtTable;

        }
    }
}