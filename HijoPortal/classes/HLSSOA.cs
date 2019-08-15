using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using DevExpress.Web;

namespace HijoPortal.classes
{
    public class HLSSOA
    {
        public static string Saving_Error = "";
        public static int hlsSOAPrintKey = 0;
        public static int hlsBillInvPrintKey = 0;

        public static DataTable HLSTable()
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
                dtTable.Columns.Add("Ctrl", typeof(string));
                dtTable.Columns.Add("EffectDate", typeof(string));
                dtTable.Columns.Add("UserKey", typeof(string));
                dtTable.Columns.Add("UserCompleteName", typeof(string));
                dtTable.Columns.Add("StatusKey", typeof(string));
                dtTable.Columns.Add("StatusDesc", typeof(string));
                dtTable.Columns.Add("LastModified", typeof(string));
            }

            string qry = "SELECT dbo.tbl_System_HLS.PK, dbo.tbl_System_HLS.Ctrl, " +
                         " dbo.tbl_System_HLS.EffectDate, dbo.tbl_System_HLS.UserKey, " +
                         " dbo.tbl_Users.Lastname, dbo.tbl_Users.Firstname, dbo.tbl_System_HLS.StatusKey, " +
                         " dbo.tbl_System_HLS.LastModified " +
                         " FROM dbo.tbl_System_HLS LEFT OUTER JOIN " +
                         " dbo.tbl_Users ON dbo.tbl_System_HLS.UserKey = dbo.tbl_Users.PK";
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
                    dtRow["Ctrl"] = row["Ctrl"].ToString();
                    dtRow["EffectDate"] = Convert.ToDateTime(row["EffectDate"]).ToString("MM/dd/yyyy");
                    dtRow["UserKey"] = row["UserKey"].ToString();
                    dtRow["UserCompleteName"] = EncryptionClass.Decrypt(row["Lastname"].ToString()) + ",  " + EncryptionClass.Decrypt(row["Firstname"].ToString());
                    dtRow["StatusKey"] = row["StatusKey"].ToString();
                    if (Convert.ToInt32(row["StatusKey"]) == 1)
                    {
                        dtRow["StatusDesc"] = "Active";
                    }
                    else
                    {
                        dtRow["StatusDesc"] = "Inactive";
                    }
                    dtRow["LastModified"] = row["LastModified"].ToString();
                    dtTable.Rows.Add(dtRow);
                }
            }
            dt.Clear();
            cn.Close();

            return dtTable;
        }

        public static DataTable HLSSOAYear()
        {
            DataTable dtTable = new DataTable();
            SqlConnection cn = new SqlConnection(GlobalClass.SQLConnString());
            DataTable dt = new DataTable();
            SqlCommand cmd = null;
            SqlDataAdapter adp;

            cn.Open();
            if (dtTable.Columns.Count == 0)
            {
                dtTable.Columns.Add("sYear", typeof(string));
            }

            string query = "SELECT Yr FROM dbo.vw_AXHLSSOA_Group GROUP BY Yr";

            cmd = new SqlCommand(query);
            cmd.Connection = cn;
            adp = new SqlDataAdapter(cmd);
            adp.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    DataRow dtRow = dtTable.NewRow();
                    dtRow["sYear"] = row["Yr"].ToString();
                    dtTable.Rows.Add(dtRow);
                }
            }
            dt.Clear();
            cn.Close();

            return dtTable;
        }

        public static DataTable HLSSOAWeekNum(int iYear)
        {
            DataTable dtTable = new DataTable();
            SqlConnection cn = new SqlConnection(GlobalClass.SQLConnString());
            DataTable dt = new DataTable();
            SqlCommand cmd = null;
            SqlDataAdapter adp;

            cn.Open();
            if (dtTable.Columns.Count == 0)
            {
                dtTable.Columns.Add("sWeekNum", typeof(string));
            }

            string query = "SELECT WEEK_NO FROM dbo.vw_AXHLSSOA_Group WHERE(Yr = "+ iYear + ") GROUP BY WEEK_NO HAVING(WEEK_NO > 0) ORDER BY WEEK_NO DESC";

            cmd = new SqlCommand(query);
            cmd.Connection = cn;
            adp = new SqlDataAdapter(cmd);
            adp.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    DataRow dtRow = dtTable.NewRow();
                    dtRow["sWeekNum"] = row["WEEK_NO"].ToString();
                    dtTable.Rows.Add(dtRow);
                }
            }
            dt.Clear();
            cn.Close();

            return dtTable;
        }

        public static DataTable HLSSOACustomer (int iYear, int iWeekNum)
        {
            DataTable dtTable = new DataTable();
            SqlConnection cn = new SqlConnection(GlobalClass.SQLConnString());
            DataTable dt = new DataTable();
            SqlCommand cmd = null;
            SqlDataAdapter adp;

            cn.Open();
            if (dtTable.Columns.Count == 0)
            {
                dtTable.Columns.Add("CustAccount", typeof(string));
                dtTable.Columns.Add("CustName", typeof(string));
            }

            string query = "SELECT CUSTACCOUNT, NAME FROM dbo.vw_AXHLSSOA_Group WHERE(Yr = "+ iYear + ") AND(WEEK_NO = "+ iWeekNum + ") GROUP BY CUSTACCOUNT, NAME ORDER BY NAME";

            cmd = new SqlCommand(query);
            cmd.Connection = cn;
            adp = new SqlDataAdapter(cmd);
            adp.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    DataRow dtRow = dtTable.NewRow();
                    dtRow["CustAccount"] = row["CUSTACCOUNT"].ToString();
                    dtRow["CustName"] = row["NAME"].ToString();
                    dtTable.Rows.Add(dtRow);
                }
            }
            dt.Clear();
            cn.Close();

            return dtTable;
        }

        public static DataTable HLSSOAWaybills (int iYear, int iWeekNum, string CustNum)
        {
            DataTable dtTable = new DataTable();
            SqlConnection cn = new SqlConnection(GlobalClass.SQLConnString());
            DataTable dt = new DataTable();
            SqlCommand cmd = null;
            SqlDataAdapter adp;

            cn.Open();
            if (dtTable.Columns.Count == 0)
            {
                dtTable.Columns.Add("Waybill", typeof(string));
                dtTable.Columns.Add("Year", typeof(string));
                dtTable.Columns.Add("WeekNum", typeof(string));
                dtTable.Columns.Add("CustCode", typeof(string));
            }

            //string query = "SELECT WAYBILLNO FROM dbo.vw_AXHLSSOA WHERE(CUSTACCOUNT = '"+ CustNum + "') AND(WEEK_NO = "+ iWeekNum + ") AND(Yr = "+ iYear + ") GROUP BY WAYBILLNO ORDER BY WAYBILLNO";


            string query = "SELECT WAYBILLNO FROM dbo.vw_AXHLSSOA WHERE(CUSTACCOUNT = '" + CustNum + "') AND(WEEK_NO = " + iWeekNum + ") AND(Yr = " + iYear + ") GROUP BY WAYBILLNO HAVING(NOT(WAYBILLNO IN (SELECT WaybillNum FROM  dbo.tbl_HLS_StatementOfAccount_Details))) ORDER BY WAYBILLNO";

            cmd = new SqlCommand(query);
            cmd.Connection = cn;
            adp = new SqlDataAdapter(cmd);
            adp.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    DataRow dtRow = dtTable.NewRow();
                    dtRow["Waybill"] = row["WAYBILLNO"].ToString();
                    dtRow["Year"] = iYear.ToString();
                    dtRow["WeekNum"] = iWeekNum.ToString();
                    dtRow["CustCode"] = CustNum.ToString();
                    dtTable.Rows.Add(dtRow);
                }
            }
            dt.Clear();
            cn.Close();

            return dtTable;
        }

        public static void SaveUpdateCustomerAttention(string sCustCode, string sAttName, string sAttNum)
        {
            SqlConnection cn = new SqlConnection(GlobalClass.SQLConnString());
            SqlCommand cmdIns = null;
            SqlCommand cmdUp = null;

            DataTable dt = new DataTable();
            SqlCommand cmd = null;
            SqlDataAdapter adp;
            string query, insert, update;

            query = "SELECT NAME, ISNULL(ADDRESS,'') AS ADDRESS FROM dbo.vw_AXCustomerTable WHERE(ACCOUNTNUM = '" + sCustCode + "')";

            cmd = new SqlCommand(query);
            cmd.Connection = cn;
            adp = new SqlDataAdapter(cmd);
            adp.Fill(dt);
            if (dt.Rows.Count == 0)
            {
                insert = "INSERT INTO tbl_HLS_CustomerAttention (CustCode, AttentionName, AttentionNumber) VALUES (@CustCode, @AttentionName, @AttentionNumber)";
                cmdIns = new SqlCommand(insert, cn);
                cmdIns.Parameters.AddWithValue("@CustCode", sCustCode);
                cmdIns.Parameters.AddWithValue("@AttentionName", sAttName);
                cmdIns.Parameters.AddWithValue("@AttentionNumber", sAttNum);
                cmdIns.ExecuteNonQuery();
            } else
            {
                update = "UPDATE tbl_HLS_CustomerAttention SET AttentionName = @AttentionName, AttentionNumber = @AttentionNumber WHERE (CustCode = @CustCode)";
                cmdUp = new SqlCommand(update, cn);
                cmdUp.Parameters.AddWithValue("@CustCode", sCustCode);
                cmdUp.Parameters.AddWithValue("@AttentionName", sAttName);
                cmdUp.Parameters.AddWithValue("@AttentionNumber", sAttNum);
                cmdUp.ExecuteNonQuery();
            }
            dt.Clear();           
            cn.Close();
        }

        public static DataTable Customer_Info(string sCustCode)
        {
            DataTable dtTable = new DataTable();
            SqlConnection cn = new SqlConnection(GlobalClass.SQLConnString());
            DataTable dt = new DataTable();
            SqlCommand cmd = null;
            SqlDataAdapter adp;

            DataTable dt1 = new DataTable();
            SqlCommand cmd1 = null;
            SqlDataAdapter adp1;
            string query, query1;
            cn.Open();
            if (dtTable.Columns.Count == 0)
            {
                dtTable.Columns.Add("CustCode", typeof(string));
                dtTable.Columns.Add("CustTIN", typeof(string));
                dtTable.Columns.Add("CustName", typeof(string));
                dtTable.Columns.Add("CustAdd", typeof(string));
                dtTable.Columns.Add("CustAtt", typeof(string));
                dtTable.Columns.Add("CustAttNum", typeof(string));
            }

            query = "SELECT NAME, ISNULL(ADDRESS,'') AS ADDRESS, VATNUM FROM dbo.vw_AXCustomerTable WHERE(ACCOUNTNUM = '" + sCustCode + "')";

            cmd = new SqlCommand(query);
            cmd.Connection = cn;
            adp = new SqlDataAdapter(cmd);
            adp.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    DataRow dtRow = dtTable.NewRow();
                    dtRow["CustCode"] = sCustCode.ToString();
                    dtRow["CustTIN"] = row["VATNUM"].ToString();
                    dtRow["CustName"] = row["NAME"].ToString();
                    dtRow["CustAdd"] = row["ADDRESS"].ToString();

                    query1 = "SELECT AttentionName, AttentionNumber FROM tbl_HLS_CustomerAttention WHERE (CustCode  = '" + sCustCode + "')";

                    cmd1 = new SqlCommand(query1);
                    cmd1.Connection = cn;
                    adp1 = new SqlDataAdapter(cmd1);
                    adp1.Fill(dt1);
                    if (dt1.Rows.Count > 0)
                    {
                        foreach (DataRow row1 in dt1.Rows)
                        {
                            dtRow["CustAtt"] = row1["AttentionName"].ToString();
                            dtRow["CustAttNum"] = row1["AttentionNumber"].ToString();
                        }
                    }
                    dt1.Clear();
                    
                    dtTable.Rows.Add(dtRow);
                }
            }
            dt.Clear();
            cn.Close();

            return dtTable;
        }

        public static DataTable SOA_Footer(string sSOANum, string sTrans)
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
                dtTable.Columns.Add("InvNum", typeof(string));
                dtTable.Columns.Add("InvDate", typeof(string));
                dtTable.Columns.Add("SOANum", typeof(string));
                dtTable.Columns.Add("SOADate", typeof(string));
                dtTable.Columns.Add("Year", typeof(string));
                dtTable.Columns.Add("WeekNum", typeof(string));
                dtTable.Columns.Add("Remarks", typeof(string));
                dtTable.Columns.Add("PreparedBy", typeof(string));
                dtTable.Columns.Add("PreparedByPost", typeof(string));
                dtTable.Columns.Add("CheckedBy", typeof(string));
                dtTable.Columns.Add("CheckedByPost", typeof(string));
                dtTable.Columns.Add("ApprovedBy", typeof(string));
                dtTable.Columns.Add("ApprovedByPost", typeof(string));
            }

            switch (sTrans)
            {
                case "Add":
                    {
                        query = "SELECT TOP (1) '' AS InvNum, '' AS InvDate, '' AS SOANum, '' AS SOADate, '' AS YearNo, '' AS WeekNo, '' AS Remarks, PreparedBy, PreparedByPost, CheckedBy, CheckedByPost, ApprovedBy, ApprovedByPost FROM tbl_HLS_Footer ORDER BY EffectDate DESC";
                        break;
                    }
                case "View":
                    {
                        query = "SELECT BillingInvoiceNum AS InvNum, ISNULL(BillingInvoiceDate, '') AS InvDate, SOANum, ISNULL(SOADate, '') AS SOADate , YearNo, WeekNo, Remarks, PreparedBy, PreparedByPost, CheckedBy, CheckedByPost, ApprovedBy, ApprovedByPost FROM tbl_HLS_StatementOfAccount WHERE (SOANum = '" + sSOANum + "')";
                        break;
                    }
            }

            cmd = new SqlCommand(query);
            cmd.Connection = cn;
            adp = new SqlDataAdapter(cmd);
            adp.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    DataRow dtRow = dtTable.NewRow();
                    dtRow["InvNum"] = row["InvNum"].ToString();
                    if (row["InvDate"].ToString() == "")
                    {
                        dtRow["InvDate"] = row["InvDate"].ToString();
                    } else
                    {
                        dtRow["InvDate"] = Convert.ToDateTime(row["InvDate"]).ToString("MM/dd/yyyy");
                    }                    
                    dtRow["SOANum"] = row["SOANum"].ToString();
                    if (row["SOADate"].ToString() == "")
                    {
                        dtRow["SOADate"] = row["SOADate"].ToString();
                    } else
                    {
                        dtRow["SOADate"] = Convert.ToDateTime(row["SOADate"]).ToString("MM/dd/yyyy");
                    }
                    dtRow["Year"] = row["YearNo"].ToString();
                    dtRow["WeekNum"] = row["WeekNo"].ToString();
                    dtRow["Remarks"] = row["Remarks"].ToString();
                    dtRow["PreparedBy"] = row["PreparedBy"].ToString();
                    dtRow["PreparedByPost"] = row["PreparedByPost"].ToString();
                    dtRow["CheckedBy"] = row["CheckedBy"].ToString();
                    dtRow["CheckedByPost"] = row["CheckedByPost"].ToString();
                    dtRow["ApprovedBy"] = row["ApprovedBy"].ToString();
                    dtRow["ApprovedByPost"] = row["ApprovedByPost"].ToString();
                    dtTable.Rows.Add(dtRow);
                }
            }
            dt.Clear();
            cn.Close();

            return dtTable;
        }

        public static DataTable SOAWaybill_for_Save(string sUserKey)
        {
            DataTable dtTable = new DataTable();
            SqlConnection cn = new SqlConnection(GlobalClass.SQLConnString());
            DataTable dt = new DataTable();
            SqlCommand cmd = null;
            SqlDataAdapter adp;
            cn.Open();
            if (dtTable.Columns.Count == 0)
            {
                dtTable.Columns.Add("Waybill", typeof(string));
                dtTable.Columns.Add("CustCode", typeof(string));
                dtTable.Columns.Add("Year", typeof(string));
                dtTable.Columns.Add("WeekNum", typeof(string));
            }
            string query = "SELECT WaybillNum, CustCode, WeekYear, WeekNum FROM tbl_HLS_StatementOfAccount_Temp WHERE (UserKey = " + sUserKey + ")";
            cmd = new SqlCommand(query);
            cmd.Connection = cn;
            adp = new SqlDataAdapter(cmd);
            adp.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    DataRow dtRow = dtTable.NewRow();
                    dtRow["Waybill"] = row["WaybillNum"].ToString();
                    dtRow["CustCode"] = row["CustCode"].ToString();
                    dtRow["Year"] = row["WeekYear"].ToString();
                    dtRow["WeekNum"] = row["WeekNum"].ToString();
                    dtTable.Rows.Add(dtRow);
                }
            }
            dt.Clear();
            cn.Close();
            return dtTable;
        }

        public static DataTable HLSSOA_Details (string sSOANum, string sUserKey, string sCustCode, string sYear, string sWeekNum)
        {
            DataTable dtTable = new DataTable();
            SqlConnection cn = new SqlConnection(GlobalClass.SQLConnString());
            DataTable dt = new DataTable();
            SqlCommand cmd = null;
            SqlDataAdapter adp;
            //DataTable dt1 = new DataTable();
            //SqlCommand cmd1 = null;
            //SqlDataAdapter adp1;
            string query = "", sWaybill = "";
            int iNum = 0;
            cn.Open();
            if (dtTable.Columns.Count == 0)
            {
                dtTable.Columns.Add("Num", typeof(string));
                dtTable.Columns.Add("Date", typeof(string));
                dtTable.Columns.Add("PlateNum", typeof(string));
                dtTable.Columns.Add("Particulars", typeof(string));
                dtTable.Columns.Add("ContainerNum", typeof(string));
                dtTable.Columns.Add("Waybill", typeof(string));
                dtTable.Columns.Add("From", typeof(string));
                dtTable.Columns.Add("To", typeof(string));
                dtTable.Columns.Add("Amount", typeof(string));
                dtTable.Columns.Add("VAT", typeof(string));
                dtTable.Columns.Add("AmountVAT", typeof(string));
                dtTable.Columns.Add("ItemID", typeof(string));
                dtTable.Columns.Add("ItemDesc", typeof(string));
            }
            if (sSOANum == "")
            {
                query = "SELECT WaybillNum FROM tbl_HLS_StatementOfAccount_Temp WHERE (UserKey = " + sUserKey + ")";
            } else
            {
                query = "SELECT dbo.tbl_HLS_StatementOfAccount_Details.WaybillNum FROM dbo.tbl_HLS_StatementOfAccount_Details LEFT OUTER JOIN dbo.tbl_HLS_StatementOfAccount ON dbo.tbl_HLS_StatementOfAccount_Details.MasterKey = dbo.tbl_HLS_StatementOfAccount.PK WHERE (dbo.tbl_HLS_StatementOfAccount.SOANum = '" + sSOANum + "')";
            }
            
            cmd = new SqlCommand(query);
            cmd.Connection = cn;
            adp = new SqlDataAdapter(cmd);
            adp.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    sWaybill = sWaybill + "," + row["WaybillNum"].ToString();
                }
            }
            dt.Clear();

            query = "AX_HLS_SODetails '" + sCustCode + "', '" + sWeekNum + "', " + sYear + ", '" + sWaybill + "'";
            cmd = new SqlCommand(query);
            cmd.Connection = cn;
            adp = new SqlDataAdapter(cmd);
            adp.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {

                    DataRow dtRow = dtTable.NewRow();
                    if (row["ITEMID"].ToString() == "SH-00001")
                    {
                        iNum = iNum + 1;
                        dtRow["Num"] = iNum.ToString("0#");
                        dtRow["Date"] = Convert.ToDateTime(row["ShippingDateRequested"]).ToString("MM/dd/yyyy");
                        dtRow["PlateNum"] = row["PLATENUM"].ToString();
                        dtRow["Particulars"] = row["DRIVER"].ToString();
                        dtRow["ContainerNum"] = row["CONTAINER_NO"].ToString();
                        dtRow["Waybill"] = row["WAYBILLNO"].ToString();
                        dtRow["From"] = row["DISTINATION_FROM"].ToString();
                        dtRow["To"] = row["DISTINCATION_TO"].ToString();
                        //dtRow["Amount"] = Convert.ToDouble(row["LINEAMOUNT"]).ToString("#,##0.00");
                        dtRow["Amount"] = Convert.ToDouble(row["LINEAMOUNT"]).ToString("#,##0.00");
                        dtRow["VAT"] = Convert.ToDouble(row["VAT_AMOUNT"]).ToString("#,##0.00");
                        dtRow["AmountVAT"] = Convert.ToDouble(row["LineAmount_VAT"]).ToString("#,##0.00");
                        
                    } else
                    {
                        dtRow["Num"] = "";
                        dtRow["Date"] = "";
                        dtRow["PlateNum"] = "";
                        if (row["ITEMID"].ToString() == "SH-00002")
                        {
                            dtRow["Particulars"] = row["ITEMDesc"].ToString() + " (" + Convert.ToDouble(row["SALESPRICE"]).ToString("#,##0.00") + " PER " + row["SALESUNIT"].ToString() + " X " + Convert.ToDouble(row["SALESQTY"]).ToString("#,##0") + " " + row["SALESUNIT"].ToString() + ")";
                        } else
                        {
                            double dSalesPrice = Convert.ToDouble(row["SALESPRICE"]) * (1 + (Convert.ToDouble(row["OVAT"])/100));
                            dtRow["Particulars"] = row["ITEMDesc"].ToString() + " (" + Convert.ToDouble(row["SALESQTY"]).ToString("#,##0") + " " + row["SALESUNIT"].ToString() + " X " + dSalesPrice.ToString("#0") + "/" + row["SALESUNIT"].ToString() + ")";
                        }
                            
                        dtRow["ContainerNum"] = "";
                        dtRow["Waybill"] = "";
                        dtRow["From"] = "";
                        dtRow["To"] = "";
                        //if (row["ITEMID"].ToString() == "SH-00002")
                        //{
                        //    dtRow["Amount"] = Convert.ToDouble(row["LINEAMOUNT"]).ToString("#,##0.00");
                        //} else
                        //{
                        //dtRow["Amount"] = Convert.ToDouble(row["LineAmount_VAT"]).ToString("#,##0.00");
                        //}
                        dtRow["Amount"] = Convert.ToDouble(row["LINEAMOUNT"]).ToString("#,##0.00");
                        dtRow["VAT"] = Convert.ToDouble(row["VAT_AMOUNT"]).ToString("#,##0.00");
                        dtRow["AmountVAT"] = Convert.ToDouble(row["LineAmount_VAT"]).ToString("#,##0.00");
                    }
                    dtRow["ItemID"] = row["ITEMID"].ToString();
                    dtRow["ItemDesc"] = row["ITEMDesc"].ToString();
                    dtTable.Rows.Add(dtRow);
                }
            }
            dt.Clear();

            cn.Close();
            return dtTable;
        }

        public static void Save_HLSSOA(string sSOANum, DateTime dSOADate, int iYear, int iWeek, string sCustCode, string sRemarks, string sPreparedBy, string sPreparedByPost, string sCheckedBy, string sCheckedByPost, string sApprovedBy, string sApprovedByPost, ASPxGridView grdWaybills, string sAttention, string sAttentionNum)
        {
            SqlConnection cn = new SqlConnection(GlobalClass.SQLConnString());
            SqlCommand cmdIns = null;
            DataTable dt = new DataTable();
            SqlCommand cmd = null;
            SqlDataAdapter adp;
            string sInsert = "", query = "";
            int iMasterKey = 0;
            cn.Open();
            Saving_Error = "";
            try
            {
                sInsert = "INSERT INTO tbl_HLS_StatementOfAccount (SOANum, SOADate, YearNo, WeekNo, CustomerCode, Remarks, PreparedBy, PreparedByPost, CheckedBy, CheckedByPost, ApprovedBy, ApprovedByPost) VALUES (@SOANum, @SOADate, @YearNo, @WeekNo, @CustomerCode, @Remarks, @PreparedBy, @PreparedByPost, @CheckedBy, @CheckedByPost, @ApprovedBy, @ApprovedByPost)";
                cmdIns = new SqlCommand(sInsert, cn);
                cmdIns.Parameters.AddWithValue("@SOANum", sSOANum);
                cmdIns.Parameters.AddWithValue("@SOADate", dSOADate);
                cmdIns.Parameters.AddWithValue("@YearNo", iYear);
                cmdIns.Parameters.AddWithValue("@WeekNo", iWeek);
                cmdIns.Parameters.AddWithValue("@CustomerCode", sCustCode);
                cmdIns.Parameters.AddWithValue("@Remarks", sRemarks);
                cmdIns.Parameters.AddWithValue("@PreparedBy", sPreparedBy);
                cmdIns.Parameters.AddWithValue("@PreparedByPost", sPreparedByPost);
                cmdIns.Parameters.AddWithValue("@CheckedBy", sCheckedBy);
                cmdIns.Parameters.AddWithValue("@CheckedByPost", sCheckedByPost);
                cmdIns.Parameters.AddWithValue("@ApprovedBy", sApprovedBy);
                cmdIns.Parameters.AddWithValue("@ApprovedByPost", sApprovedByPost);
                cmdIns.ExecuteNonQuery();
            } catch (SqlException ex)
            {
                Saving_Error = ex.ToString();
            } finally
            {
                query = "SELECT PK FROM tbl_HLS_StatementOfAccount WHERE (SOANum = '" + sSOANum + "')";
                cmd = new SqlCommand(query);
                cmd.Connection = cn;
                adp = new SqlDataAdapter(cmd);
                adp.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        iMasterKey = Convert.ToInt32(row["PK"]);
                    }
                }
                dt.Clear();
            }
                        
            if (iMasterKey > 0)
            {
                for (int i = 0; i <= (grdWaybills.VisibleRowCount - 1); i++)
                {
                    object keyValue = grdWaybills.GetRowValues(i, "Waybill");
                    sInsert = "INSERT INTO tbl_HLS_StatementOfAccount_Details (MasterKey, Line, WaybillNum) VALUES (@MasterKey, @Line, @WaybillNum)";
                    cmdIns = new SqlCommand(sInsert, cn);
                    cmdIns.Parameters.AddWithValue("@MasterKey", iMasterKey);
                    cmdIns.Parameters.AddWithValue("@Line", i + 1);
                    cmdIns.Parameters.AddWithValue("@WaybillNum", keyValue.ToString());
                    cmdIns.ExecuteNonQuery();
                }
            }

            query = "SELECT tbl_HLS_CustomerAttention.* FROM tbl_HLS_CustomerAttention WHERE (CustCode = '" + sCustCode + "')";
            cmd = new SqlCommand(query);
            cmd.Connection = cn;
            adp = new SqlDataAdapter(cmd);
            adp.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                sInsert = "UPDATE tbl_HLS_CustomerAttention SET AttentionName = @AttentionName, AttentionNumber = @AttentionNumber WHERE (CustCode = @CustCode)";
                cmdIns = new SqlCommand(sInsert, cn);
                cmdIns.Parameters.AddWithValue("@CustCode", sCustCode);
                cmdIns.Parameters.AddWithValue("@AttentionName", sAttention);
                cmdIns.Parameters.AddWithValue("@AttentionNumber", sAttentionNum);
                cmdIns.ExecuteNonQuery();
            } else
            {
                sInsert = "INSERT INTO tbl_HLS_CustomerAttention (CustCode, AttentionName, AttentionNumber) VALUES (@CustCode, @AttentionName, @AttentionNumber)";
                cmdIns = new SqlCommand(sInsert, cn);
                cmdIns.Parameters.AddWithValue("@CustCode", sCustCode);
                cmdIns.Parameters.AddWithValue("@AttentionName", sAttention);
                cmdIns.Parameters.AddWithValue("@AttentionNumber", sAttentionNum);
                cmdIns.ExecuteNonQuery();
            }
            dt.Clear();

            cn.Close();
        }

        public static void Print_HLS_SOA(int iUserKey, ASPxGridView grdDetails, string sSOANum, string sSOADate, string sCustomerName, string sCustomerAdd, string sCustomerAtt, string sCustomerAttNum, string sRemarks, string sPreparedBy, string sPreparedByPost, string sCheckedBy, string sCheckedByPost, string sApprovedBy, string sApprovedByPost)
        {
            SqlConnection cn = new SqlConnection(GlobalClass.SQLConnString());
            SqlCommand cmdIns = null;
            DataTable dt = new DataTable();
            SqlCommand cmd = null;
            SqlDataAdapter adp;
            int iMasterKey = 0, iSubLine = 0;
            double dTotalAmt = 0, dVAT = 0, dTotalAmtVAT = 0;
            string qry = "";
            cn.Open();

            //Delete Record
            cmdIns = new SqlCommand("DELETE FROM tbl_HLS_StatementOfAccountReport WHERE (UserKey = @UserKey)", cn);
            cmdIns.Parameters.AddWithValue("@UserKey", iUserKey);
            cmdIns.ExecuteNonQuery();

            //Insert Record
            cmdIns = new SqlCommand("INSERT INTO tbl_HLS_StatementOfAccountReport (UserKey, SOANum, SOADate, CustomerName, CustomerAdd, CustomerAtt, CustomerAttNum, Remarks, PreparedBy, PreparedByPost, CheckedBy, CheckedByPost, ApprovedBy, ApprovedByPost) VALUES (@UserKey, @SOANum, @SOADate, @CustomerName, @CustomerAdd, @CustomerAtt, @CustomerAttNum, @Remarks, @PreparedBy, @PreparedByPost, @CheckedBy, @CheckedByPost, @ApprovedBy, @ApprovedByPost)", cn);
            cmdIns.Parameters.AddWithValue("@UserKey", iUserKey);
            cmdIns.Parameters.AddWithValue("@SOANum", sSOANum.ToString());
            cmdIns.Parameters.AddWithValue("@SOADate", sSOADate.ToString());
            cmdIns.Parameters.AddWithValue("@CustomerName", sCustomerName.ToString());
            cmdIns.Parameters.AddWithValue("@CustomerAdd", sCustomerAdd.ToString());
            cmdIns.Parameters.AddWithValue("@CustomerAtt", sCustomerAtt.ToString());
            cmdIns.Parameters.AddWithValue("@CustomerAttNum", sCustomerAttNum.ToString());
            cmdIns.Parameters.AddWithValue("@Remarks", sRemarks.ToString());
            cmdIns.Parameters.AddWithValue("@PreparedBy", sPreparedBy.ToString());
            cmdIns.Parameters.AddWithValue("@PreparedByPost", sPreparedByPost.ToString());
            cmdIns.Parameters.AddWithValue("@CheckedBy", sCheckedBy.ToString());
            cmdIns.Parameters.AddWithValue("@CheckedByPost", sCheckedByPost.ToString());
            cmdIns.Parameters.AddWithValue("@ApprovedBy", sApprovedBy.ToString());
            cmdIns.Parameters.AddWithValue("@ApprovedByPost", sApprovedByPost.ToString());
            cmdIns.ExecuteNonQuery();

            qry = "SELECT PK FROM tbl_HLS_StatementOfAccountReport WHERE (UserKey = " + iUserKey + ")";
            cmd = new SqlCommand(qry);
            cmd.Connection = cn;
            adp = new SqlDataAdapter(cmd);
            adp.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    iMasterKey = Convert.ToInt32(row["PK"]);
                }
            }
            dt.Clear();

            hlsSOAPrintKey = iMasterKey;

            if (iMasterKey > 0)
            {
                //update logo
                string logoPath = HttpContext.Current.Server.MapPath("~") + @"images\hls.jpg";
                if (File.Exists(logoPath))
                {
                    byte[] imageData = GlobalClass.ReadImageFile(logoPath);
                    if (imageData != null)
                    {
                        cmdIns = new SqlCommand("UPDATE tbl_HLS_StatementOfAccountReport SET PictLogo = @PictLogo WHERE (PK = @PK)", cn);
                        cmdIns.Parameters.AddWithValue("@PK", iMasterKey);
                        cmdIns.Parameters.AddWithValue("@PictLogo", imageData);
                        cmdIns.ExecuteNonQuery();
                    }

                }

                for (int i = 0; i <= (grdDetails.VisibleRowCount - 1); i++)
                {
                    object oNum = grdDetails.GetRowValues(i, "Num");
                    object oDate = grdDetails.GetRowValues(i, "Date");
                    object oPlateNum = grdDetails.GetRowValues(i, "PlateNum");
                    object oParticulars = grdDetails.GetRowValues(i, "Particulars");
                    object oContainerNum = grdDetails.GetRowValues(i, "ContainerNum");
                    object oWaybill = grdDetails.GetRowValues(i, "Waybill");
                    object oFrom = grdDetails.GetRowValues(i, "From");
                    object oTo = grdDetails.GetRowValues(i, "To");
                    object oAmount = grdDetails.GetRowValues(i, "Amount");
                    object oVAT = grdDetails.GetRowValues(i, "VAT");
                    object oAmountVAT = grdDetails.GetRowValues(i, "AmountVAT");
                    object oItemID = grdDetails.GetRowValues(i, "ItemID");
                    object oItemDesc = grdDetails.GetRowValues(i, "ItemDesc");

                    dTotalAmt = dTotalAmt + Convert.ToDouble(oAmount);
                    dVAT = dVAT + Convert.ToDouble(oVAT); 
                    dTotalAmtVAT = dTotalAmtVAT + Convert.ToDouble(oAmountVAT);

                    cmdIns = new SqlCommand("INSERT INTO tbl_HLS_StatementOfAccountReport_Det (MasterKey, Line, LineNum, LineDate, PlateNum, Particulars, ContNum, WayBillNum, LineFrom, LineTo, Amount, VAT) VALUES (@MasterKey, @Line, @LineNum, @LineDate, @PlateNum, @Particulars, @ContNum, @WayBillNum, @LineFrom, @LineTo, @Amount, @VAT)", cn);
                    cmdIns.Parameters.AddWithValue("@MasterKey", iMasterKey);
                    cmdIns.Parameters.AddWithValue("@Line", i + i);
                    cmdIns.Parameters.AddWithValue("@LineNum", oNum.ToString());
                    cmdIns.Parameters.AddWithValue("@LineDate", oDate.ToString());
                    cmdIns.Parameters.AddWithValue("@PlateNum", oPlateNum.ToString());
                    cmdIns.Parameters.AddWithValue("@Particulars", oParticulars.ToString());
                    cmdIns.Parameters.AddWithValue("@ContNum", oContainerNum.ToString());
                    cmdIns.Parameters.AddWithValue("@WayBillNum", oWaybill.ToString());
                    cmdIns.Parameters.AddWithValue("@LineFrom", oFrom.ToString());
                    cmdIns.Parameters.AddWithValue("@LineTo", oTo.ToString());
                    cmdIns.Parameters.AddWithValue("@Amount", Convert.ToDouble(oAmount));
                    cmdIns.Parameters.AddWithValue("@VAT", Convert.ToDouble(oVAT));
                    cmdIns.ExecuteNonQuery();

                    //Sub-Total
                    qry = "SELECT tbl_HLS_StatementOfAccountReport_Sub.* FROM tbl_HLS_StatementOfAccountReport_Sub WHERE (MasterKey = " + iMasterKey + ") AND (ItemID = '" + oItemID + "')";
                    cmd = new SqlCommand(qry);
                    cmd.Connection = cn;
                    adp = new SqlDataAdapter(cmd);
                    adp.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        cmdIns = new SqlCommand("UPDATE tbl_HLS_StatementOfAccountReport_Sub SET Amount = Amount + @Amount, Vat = Vat + @Vat WHERE (MasterKey = @MasterKey) AND (ItemID = @ItemID)", cn);
                        cmdIns.Parameters.AddWithValue("@MasterKey", iMasterKey);
                        cmdIns.Parameters.AddWithValue("@ItemID", oItemID);
                        cmdIns.Parameters.AddWithValue("@Amount", Convert.ToDouble(oAmount));
                        cmdIns.Parameters.AddWithValue("@Vat", Convert.ToDouble(oVAT));
                        cmdIns.ExecuteNonQuery();
                    } else
                    {
                        iSubLine = iSubLine + 1;
                        cmdIns = new SqlCommand("INSERT INTO tbl_HLS_StatementOfAccountReport_Sub (MasterKey, Line, ItemID, RepDesc, Amount, Vat) VALUES (@MasterKey, @Line, @ItemID, @RepDesc, @Amount, @Vat)", cn);
                        cmdIns.Parameters.AddWithValue("@MasterKey", iMasterKey);
                        cmdIns.Parameters.AddWithValue("@Line", iSubLine);
                        cmdIns.Parameters.AddWithValue("@ItemID", oItemID);
                        cmdIns.Parameters.AddWithValue("@RepDesc", oItemDesc);
                        cmdIns.Parameters.AddWithValue("@Amount", Convert.ToDouble(oAmount));
                        cmdIns.Parameters.AddWithValue("@Vat", Convert.ToDouble(oVAT));
                        cmdIns.ExecuteNonQuery();
                    }
                    dt.Clear();
                }
            }

            //Sub-Total
            iSubLine = iSubLine + 1;
            cmdIns = new SqlCommand("INSERT INTO tbl_HLS_StatementOfAccountReport_Sub (MasterKey, Line, ItemID, RepDesc, Amount) VALUES (@MasterKey, @Line, @ItemID, @RepDesc, @Amount)", cn);
            cmdIns.Parameters.AddWithValue("@MasterKey", iMasterKey);
            cmdIns.Parameters.AddWithValue("@Line", iSubLine);
            cmdIns.Parameters.AddWithValue("@ItemID", "SUB-TOTAL");
            cmdIns.Parameters.AddWithValue("@RepDesc", "SUB-TOTAL");
            cmdIns.Parameters.AddWithValue("@Amount", Convert.ToDouble(dTotalAmt));
            cmdIns.ExecuteNonQuery();

            //VAT
            iSubLine = iSubLine + 1;
            cmdIns = new SqlCommand("INSERT INTO tbl_HLS_StatementOfAccountReport_Sub (MasterKey, Line, ItemID, RepDesc, Amount) VALUES (@MasterKey, @Line, @ItemID, @RepDesc, @Amount)", cn);
            cmdIns.Parameters.AddWithValue("@MasterKey", iMasterKey);
            cmdIns.Parameters.AddWithValue("@Line", iSubLine);
            cmdIns.Parameters.AddWithValue("@ItemID", "12% VAT");
            cmdIns.Parameters.AddWithValue("@RepDesc", "12% VAT");
            cmdIns.Parameters.AddWithValue("@Amount", Convert.ToDouble(dVAT));
            cmdIns.ExecuteNonQuery();

            //TOTAL
            iSubLine = iSubLine + 1;
            cmdIns = new SqlCommand("INSERT INTO tbl_HLS_StatementOfAccountReport_Sub (MasterKey, Line, ItemID, RepDesc, Amount) VALUES (@MasterKey, @Line, @ItemID, @RepDesc, @Amount)", cn);
            cmdIns.Parameters.AddWithValue("@MasterKey", iMasterKey);
            cmdIns.Parameters.AddWithValue("@Line", iSubLine);
            cmdIns.Parameters.AddWithValue("@ItemID", "TOTAL");
            cmdIns.Parameters.AddWithValue("@RepDesc", "TOTAL");
            cmdIns.Parameters.AddWithValue("@Amount", Convert.ToDouble(dTotalAmtVAT));
            cmdIns.ExecuteNonQuery();

            cmdIns = new SqlCommand("UPDATE tbl_HLS_StatementOfAccountReport SET TotalAmount = @TotalAmount, TotalVAT = @TotalVAT WHERE (PK = @PK)", cn);
            cmdIns.Parameters.AddWithValue("@PK", iMasterKey);
            cmdIns.Parameters.AddWithValue("@TotalAmount", dTotalAmt);
            cmdIns.Parameters.AddWithValue("@TotalVAT", dVAT);
            cmdIns.ExecuteNonQuery();

            cn.Close();
        }

        public static void Print_HLS_BillInv(int iUserKey, string sInvDate, string sCustCode, string sCustomerName, string sCustomerAdd, string sCustomerTIN, string sYear, string sWeekNum, string sPreparedBy, string sApprovedBy, string sSOANum)
        {
            SqlConnection cn = new SqlConnection(GlobalClass.SQLConnString());
            SqlCommand cmdIns = null;
            DataTable dt = new DataTable();
            SqlCommand cmd = null;
            SqlDataAdapter adp;
            DataTable dt1 = new DataTable();
            SqlCommand cmd1 = null;
            SqlDataAdapter adp1;
            int iMasterKey = 0, iSubLine = 0;
            double dTotalAmt = 0, dVAT = 0, dTotalAmtVAT = 0;
            string qry = "", sWaybill = "";
            cn.Open();

            //if (sCustomerAdd == "")
            //{
            //    sCustomerAdd = "Brgy. Madaum, Tagum City, PHL";
            //}
            //if (sCustomerTIN == "")
            //{
            //    sCustomerTIN = "012-265-356-000";
            //}

            //Delete Record
            cmdIns = new SqlCommand("DELETE FROM tbl_HLS_StatementOfAccountReportInv WHERE (UserKey = @UserKey)", cn);
            cmdIns.Parameters.AddWithValue("@UserKey", iUserKey);
            cmdIns.ExecuteNonQuery();

            //Insert Record
            cmdIns = new SqlCommand("INSERT INTO tbl_HLS_StatementOfAccountReportInv (UserKey, InvDate, CustomerName, CustomerAdd, CustomerTIN, WeekNum, PreparedBy, ApprovedBy) VALUES (@UserKey, @InvDate, @CustomerName, @CustomerAdd, @CustomerTIN, @WeekNum, @PreparedBy, @ApprovedBy)", cn);
            cmdIns.Parameters.AddWithValue("@UserKey", iUserKey);
            cmdIns.Parameters.AddWithValue("@InvDate", Convert.ToDateTime(sInvDate).ToString("MMMM dd,yyyy"));
            cmdIns.Parameters.AddWithValue("@CustomerName", sCustomerName.ToString());
            cmdIns.Parameters.AddWithValue("@CustomerAdd", sCustomerAdd.ToString());
            cmdIns.Parameters.AddWithValue("@CustomerTIN", sCustomerTIN.ToString());
            cmdIns.Parameters.AddWithValue("@WeekNum", " WEEK " + sWeekNum.ToString());
            cmdIns.Parameters.AddWithValue("@PreparedBy", sPreparedBy.ToString());
            cmdIns.Parameters.AddWithValue("@ApprovedBy", sApprovedBy.ToString());
            cmdIns.ExecuteNonQuery();

            qry = "SELECT PK FROM tbl_HLS_StatementOfAccountReportInv WHERE (UserKey = " + iUserKey + ")";
            cmd = new SqlCommand(qry);
            cmd.Connection = cn;
            adp = new SqlDataAdapter(cmd);
            adp.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    iMasterKey = Convert.ToInt32(row["PK"]);
                }
            }
            dt.Clear();

            hlsBillInvPrintKey = iMasterKey;

            if (iMasterKey > 0)
            {
                qry = "SELECT dbo.tbl_HLS_StatementOfAccount_Details.WaybillNum FROM dbo.tbl_HLS_StatementOfAccount_Details LEFT OUTER JOIN dbo.tbl_HLS_StatementOfAccount ON dbo.tbl_HLS_StatementOfAccount_Details.MasterKey = dbo.tbl_HLS_StatementOfAccount.PK WHERE (dbo.tbl_HLS_StatementOfAccount.SOANum = '" + sSOANum + "')";
                cmd = new SqlCommand(qry);
                cmd.Connection = cn;
                adp = new SqlDataAdapter(cmd);
                adp.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        sWaybill = sWaybill + "," + row["WaybillNum"].ToString();
                    }
                }
                dt.Clear();

                qry = "AX_HLS_SODetails '" + sCustCode + "', '" + sWeekNum + "', " + sYear + ", '" + sWaybill + "'";
                cmd = new SqlCommand(qry);
                cmd.Connection = cn;
                adp = new SqlDataAdapter(cmd);
                adp.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    foreach(DataRow row in dt.Rows)
                    {
                        dTotalAmtVAT = dTotalAmtVAT + Convert.ToDouble(row["LineAmount_VAT"]);

                        qry = "SELECT tbl_HLS_StatementOfAccountReportInv_Det.* FROM tbl_HLS_StatementOfAccountReportInv_Det WHERE (MasterKey = " + iMasterKey + ") AND (WaybillNum = '" + row["WAYBILLNO"].ToString() + "')";
                        cmd1 = new SqlCommand(qry);
                        cmd1.Connection = cn;
                        adp1 = new SqlDataAdapter(cmd1);
                        adp1.Fill(dt1);
                        if (dt1.Rows.Count > 0)
                        {
                            cmdIns = new SqlCommand("UPDATE tbl_HLS_StatementOfAccountReportInv_Det SET Amount = Amount + @Amount WHERE (MasterKey = @MasterKey) AND (WaybillNum = @WaybillNum)", cn);
                            cmdIns.Parameters.AddWithValue("@MasterKey", iMasterKey);
                            cmdIns.Parameters.AddWithValue("@WaybillNum", row["WAYBILLNO"].ToString());
                            cmdIns.Parameters.AddWithValue("@Amount", Convert.ToDouble(row["LineAmount_VAT"]));
                            cmdIns.ExecuteNonQuery();
                        } else
                        {
                            iSubLine = iSubLine + 1;
                            cmdIns = new SqlCommand("INSERT INTO tbl_HLS_StatementOfAccountReportInv_Det (MasterKey, Line, ContainerNum, PlateNum, WaybillNum, Amount) VALUES (@MasterKey, @Line, @ContainerNum, @PlateNum, @WaybillNum, @Amount)", cn);
                            cmdIns.Parameters.AddWithValue("@MasterKey", iMasterKey);
                            cmdIns.Parameters.AddWithValue("@Line", iSubLine);
                            cmdIns.Parameters.AddWithValue("@ContainerNum", row["CONTAINER_NO"].ToString());
                            cmdIns.Parameters.AddWithValue("@PlateNum", row["PLATENUM"].ToString());
                            cmdIns.Parameters.AddWithValue("@WaybillNum", row["WAYBILLNO"].ToString());
                            cmdIns.Parameters.AddWithValue("@Amount", Convert.ToDouble(row["LineAmount_VAT"]));
                            cmdIns.ExecuteNonQuery();
                        }
                        dt1.Clear();
                    }
                }
                dt.Clear();

                //Update Record
                cmdIns = new SqlCommand("UPDATE tbl_HLS_StatementOfAccountReportInv SET TotalAmount = @TotalAmount WHERE (PK = @PK)", cn);
                cmdIns.Parameters.AddWithValue("@PK", iMasterKey);
                cmdIns.Parameters.AddWithValue("@TotalAmount", dTotalAmtVAT);
                cmdIns.ExecuteNonQuery();
            }
            
            cn.Close();
        }

        public static DataTable HLSSOA_List()
        {
            DataTable dtTable = new DataTable();
            SqlConnection cn = new SqlConnection(GlobalClass.SQLConnString());
            DataTable dt = new DataTable();
            SqlCommand cmd = null;
            SqlDataAdapter adp;
            DataTable dt1 = new DataTable();
            SqlCommand cmd1 = null;
            SqlDataAdapter adp1;
            string query = "", query1 = "", sWaybill = "";
            double dAmount = 0;
            cn.Open();
            if (dtTable.Columns.Count == 0)
            {
                dtTable.Columns.Add("PK", typeof(string));
                dtTable.Columns.Add("SOANum", typeof(string));
                dtTable.Columns.Add("SOADate", typeof(string));
                dtTable.Columns.Add("CustCode", typeof(string));
                dtTable.Columns.Add("CustName", typeof(string));
                dtTable.Columns.Add("Year", typeof(string));
                dtTable.Columns.Add("WeekNum", typeof(string));
                dtTable.Columns.Add("BillingInv", typeof(string));
                dtTable.Columns.Add("BillingDate", typeof(string));
                dtTable.Columns.Add("Amount", typeof(string));
            }
            query = "SELECT dbo.tbl_HLS_StatementOfAccount.PK, dbo.tbl_HLS_StatementOfAccount.SOANum, dbo.tbl_HLS_StatementOfAccount.SOADate, dbo.tbl_HLS_StatementOfAccount.CustomerCode, dbo.vw_AXCustomerTable.NAME, dbo.tbl_HLS_StatementOfAccount.YearNo, dbo.tbl_HLS_StatementOfAccount.WeekNo, dbo.tbl_HLS_StatementOfAccount.BillingInvoiceNum, dbo.tbl_HLS_StatementOfAccount.BillingInvoiceDate FROM dbo.tbl_HLS_StatementOfAccount LEFT OUTER JOIN dbo.vw_AXCustomerTable ON dbo.tbl_HLS_StatementOfAccount.CustomerCode = dbo.vw_AXCustomerTable.ACCOUNTNUM ORDER BY dbo.tbl_HLS_StatementOfAccount.SOANum DESC";
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
                    dtRow["SOANum"] = row["SOANum"].ToString();
                    dtRow["SOADate"] = Convert.ToDateTime(row["SOADate"]).ToString("MM/dd/yyyy");
                    dtRow["CustCode"] = row["CustomerCode"].ToString();
                    dtRow["CustName"] = row["NAME"].ToString();
                    dtRow["Year"] = row["YearNo"].ToString();
                    dtRow["WeekNum"] = row["WeekNo"].ToString();
                    dtRow["BillingInv"] = row["BillingInvoiceNum"].ToString();
                    if (row["BillingInvoiceDate"] != DBNull.Value)
                    {
                        dtRow["BillingDate"] = Convert.ToDateTime(row["BillingInvoiceDate"]).ToString("MM/dd/yyyy");
                    } else
                    {
                        dtRow["BillingDate"] = "";
                    }                    
                    sWaybill = ""; dAmount = 0;
                    query1 = "SELECT dbo.tbl_HLS_StatementOfAccount_Details.WaybillNum FROM dbo.tbl_HLS_StatementOfAccount_Details LEFT OUTER JOIN dbo.tbl_HLS_StatementOfAccount ON dbo.tbl_HLS_StatementOfAccount_Details.MasterKey = dbo.tbl_HLS_StatementOfAccount.PK WHERE (dbo.tbl_HLS_StatementOfAccount.SOANum = '" + row["SOANum"].ToString() + "')";
                    cmd1 = new SqlCommand(query1);
                    cmd1.Connection = cn;
                    adp1 = new SqlDataAdapter(cmd1);
                    adp1.Fill(dt1);
                    if (dt1.Rows.Count > 0)
                    {
                        foreach (DataRow row1 in dt1.Rows)
                        {
                            sWaybill = sWaybill + "," + row1["WaybillNum"].ToString();
                        }
                        
                    }
                    dt1.Clear();

                    query1 = "AX_HLS_SODetails '" + row["CustomerCode"].ToString() + "', '" + row["WeekNo"].ToString() + "', " + row["YearNo"].ToString() + ", '" + sWaybill + "'"; ;
                    cmd1 = new SqlCommand(query1);
                    cmd1.Connection = cn;
                    adp1 = new SqlDataAdapter(cmd1);
                    adp1.Fill(dt1);
                    if (dt1.Rows.Count > 0)
                    {
                        foreach (DataRow row1 in dt1.Rows)
                        {
                            //if (row1["ITEMID"].ToString() == "SH-00001" || row1["ITEMID"].ToString() == "SH-00002")
                            //{
                            //    dAmount = dAmount + Convert.ToDouble(row1["LINEAMOUNT"]);
                            //} else
                            //{
                                dAmount = dAmount + Convert.ToDouble(row1["LineAmount_VAT"]);
                            //}
                        }
                    }
                    dt1.Clear();

                    dtRow["Amount"] = dAmount.ToString("#,##0.00");
                    dtTable.Rows.Add(dtRow);
                }
            }
            dt.Clear();
            cn.Close();
            return dtTable;
        }
    }
}