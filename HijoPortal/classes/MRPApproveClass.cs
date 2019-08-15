﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Text;

namespace HijoPortal.classes
{
    public class MRPApproveClass
    {
        public static void MRP_Approve(string docNum, int MRPKey, DateTime dteCreated, int ApproveLine, string EntCode, string BuCode, int usrKey)
        {
            switch (ApproveLine)
            {
                case 1: //SMC Lead
                    {
                        MRP_Approve_SCM(docNum, MRPKey, dteCreated, ApproveLine, EntCode, BuCode, usrKey);
                        break;
                    }
                case 2: //Finance Lead
                    {
                        MRP_Approve_Finance(docNum, MRPKey, dteCreated, ApproveLine, EntCode, BuCode, usrKey);
                        break;
                    }
                case 3: //Executive 
                    {
                        MRP_Approve_Executive(docNum, MRPKey, dteCreated, ApproveLine, EntCode, BuCode, usrKey);
                        break;
                    }
            }
        }

        private static void MRP_Approve_SCM(string docNum, int MRPKey, DateTime dteCreated, int ApproveLine, string EntCode, string BuCode, int usrKey)
        {
            SqlConnection conn = new SqlConnection(GlobalClass.SQLConnString());
            string qry = "";
            string CreatorEmail = "", CreatorSubject = "", CreatorGreetings = "";
            var sCreatorBody = new StringBuilder();
            string sEmail = "", sSubject = "", sGreetings = "";
            var sBody = new StringBuilder();

            int ApproveLineNext = ApproveLine + 1;

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

            SqlCommand cmd3 = null;
            SqlDataAdapter adp3;
            DataTable dtable3 = new DataTable();

            conn.Open();
            qry = "SELECT dbo.tbl_Users.Email, dbo.tbl_Users.Gender, dbo.tbl_Users.Lastname " +
                  " FROM dbo.tbl_MRP_List LEFT OUTER JOIN " +
                  " dbo.tbl_Users ON dbo.tbl_MRP_List.CreatorKey = dbo.tbl_Users.PK " +
                  " WHERE(dbo.tbl_MRP_List.PK = " + MRPKey + ")";
            cmd = new SqlCommand(qry);
            cmd.Connection = conn;
            adp = new SqlDataAdapter(cmd);
            adp.Fill(dtable);
            if (dtable.Rows.Count > 0)
            {
                foreach (DataRow row in dtable.Rows)
                {
                    CreatorEmail = EncryptionClass.Decrypt(row["Email"].ToString());
                    if (Convert.ToInt32(row["Gender"]) == 1)
                    {
                        CreatorGreetings = "Dear Mr. " + EncryptionClass.Decrypt(row["Lastname"].ToString());
                    }
                    else
                    {
                        CreatorGreetings = "Dear Ms. " + EncryptionClass.Decrypt(row["Lastname"].ToString());
                    }
                    CreatorSubject = "MOP DocNum " + docNum.ToString() + " status";

                    sCreatorBody.Append("<!DOCTYPE html>");
                    sCreatorBody.Append("<html>");
                    sCreatorBody.Append("<head>");
                    sCreatorBody.Append("</head>");
                    sCreatorBody.Append("<body>");
                    sCreatorBody.Append("<p style='font-family:Tahoma; font-size: 12px;'>" + CreatorGreetings + ",</p>");
                    sCreatorBody.Append("<p style='font-family:Tahoma; font-size: 12px;'>MOP Document # " + docNum.ToString() + " has been approved by Supply Chain Management Lead.</p>");
                    sCreatorBody.Append("<p style='font-family:Tahoma; font-size: 10px;font-style:italic;'>***This is a system-generated message. please do not reply to this email.***</p>");
                    sCreatorBody.Append("<p style='font-family:Tahoma; font-size: 10px;'>DISCLAIMER: This email is confidential and intended solely for the use of the individual to whom it is addressed. If you are not the intended recipient, be advised that you have received this email in error and that any use, dissemination, forwarding, printing or copying of this email is strictly prohibited. If you have received this email in error please notify the sender or email info@hijoresources.net, telephone number (082) 282-3662.</p>");
                    sCreatorBody.Append("</body>");
                    sCreatorBody.Append("</html>");
                }
            }
            dtable.Clear();

            //Update Approval
            qry = "UPDATE tbl_MRP_List_Approval " +
                   " SET Visible = 0, " +
                   " Status = 1 " +
                   " WHERE (MasterKey = " + MRPKey + ") " +
                   " AND (Line = " + ApproveLine + ")";
            cmdUp = new SqlCommand(qry, conn);
            cmdUp.ExecuteNonQuery();

            //bool msgSendToCreator = GlobalClass.IsMailSent(CreatorEmail, CreatorSubject, sCreatorBody.ToString());
            GlobalClass.CreateEmailNotification(CreatorEmail, CreatorGreetings, docNum, "has been approved by Supply Chain Management Lead", 4);

            //Check if Approve All
            qry = "SELECT COUNT(*) AS RecCnt " +
                  " FROM dbo.tbl_MRP_List_Approval " +
                  " WHERE(MasterKey = " + MRPKey + ") " +
                  " AND(Status = 0)";
            cmd = new SqlCommand(qry);
            cmd.Connection = conn;
            adp = new SqlDataAdapter(cmd);
            adp.Fill(dtable);
            if (dtable.Rows.Count > 0)
            {
                foreach (DataRow row in dtable.Rows)
                {
                    if (Convert.ToInt32(row["RecCnt"]) == 0)
                    {
                        qry = "UPDATE tbl_MRP_List " +
                              " SET StatusKey = 4 " +
                              " WHERE (PK = " + MRPKey + ")";
                        cmdUp = new SqlCommand(qry, conn);
                        cmdUp.ExecuteNonQuery();

                        qry = "UPDATE tbl_MRP_List_Workflow " +
                              " SET Visible = 0, " +
                              " Status = 1 " +
                              " WHERE (MasterKey = " + MRPKey + ") " +
                              " AND (Line = 4)";
                        cmdUp = new SqlCommand(qry, conn);
                        cmdUp.ExecuteNonQuery();
                    }
                }
            }
            dtable.Clear();
            conn.Close();
        }

        private static void MRP_Approve_Finance(string docNum, int MRPKey, DateTime dteCreated, int ApproveLine, string EntCode, string BuCode, int usrKey)
        {
            SqlConnection conn = new SqlConnection(GlobalClass.SQLConnString());
            string qry = "";
            string CreatorEmail = "", CreatorSubject = "", CreatorGreetings = "";
            var sCreatorBody = new StringBuilder();
            string sEmail = "", sSubject = "", sGreetings = "";
            var sBody = new StringBuilder();

            int ApproveLineNext = ApproveLine + 1;

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

            SqlCommand cmd3 = null;
            SqlDataAdapter adp3;
            DataTable dtable3 = new DataTable();

            conn.Open();
            qry = "SELECT dbo.tbl_Users.Email, dbo.tbl_Users.Gender, dbo.tbl_Users.Lastname " +
                  " FROM dbo.tbl_MRP_List LEFT OUTER JOIN " +
                  " dbo.tbl_Users ON dbo.tbl_MRP_List.CreatorKey = dbo.tbl_Users.PK " +
                  " WHERE(dbo.tbl_MRP_List.PK = " + MRPKey + ")";
            cmd = new SqlCommand(qry);
            cmd.Connection = conn;
            adp = new SqlDataAdapter(cmd);
            adp.Fill(dtable);
            if (dtable.Rows.Count > 0)
            {
                foreach (DataRow row in dtable.Rows)
                {
                    CreatorEmail = EncryptionClass.Decrypt(row["Email"].ToString());
                    if (Convert.ToInt32(row["Gender"]) == 1)
                    {
                        CreatorGreetings = "Dear Mr. " + EncryptionClass.Decrypt(row["Lastname"].ToString());
                    }
                    else
                    {
                        CreatorGreetings = "Dear Ms. " + EncryptionClass.Decrypt(row["Lastname"].ToString());
                    }
                    CreatorSubject = "MOP DocNum " + docNum.ToString() + " status";

                    sCreatorBody.Append("<!DOCTYPE html>");
                    sCreatorBody.Append("<html>");
                    sCreatorBody.Append("<head>");
                    sCreatorBody.Append("</head>");
                    sCreatorBody.Append("<body>");
                    sCreatorBody.Append("<p style='font-family:Tahoma; font-size: 12px;'>" + CreatorGreetings + ",</p>");
                    sCreatorBody.Append("<p style='font-family:Tahoma; font-size: 12px;'>MOP Document # " + docNum.ToString() + " has been approved by Finance Lead.</p>");
                    sCreatorBody.Append("<p style='font-family:Tahoma; font-size: 10px;font-style:italic;'>***This is a system-generated message. please do not reply to this email.***</p>");
                    sCreatorBody.Append("<p style='font-family:Tahoma; font-size: 10px;'>DISCLAIMER: This email is confidential and intended solely for the use of the individual to whom it is addressed. If you are not the intended recipient, be advised that you have received this email in error and that any use, dissemination, forwarding, printing or copying of this email is strictly prohibited. If you have received this email in error please notify the sender or email info@hijoresources.net, telephone number (082) 282-3662.</p>");
                    sCreatorBody.Append("</body>");
                    sCreatorBody.Append("</html>");
                }
            }
            dtable.Clear();

            //Update Approval
            qry = "UPDATE tbl_MRP_List_Approval " +
                   " SET Visible = 0, " +
                   " Status = 1 " +
                   " WHERE (MasterKey = " + MRPKey + ") " +
                   " AND (Line = " + ApproveLine + ")";
            cmdUp = new SqlCommand(qry, conn);
            cmdUp.ExecuteNonQuery();

            //bool msgSendToCreator = GlobalClass.IsMailSent(CreatorEmail, CreatorSubject, sCreatorBody.ToString());
            GlobalClass.CreateEmailNotification(CreatorEmail, CreatorGreetings, docNum, "has been approved by Finance Lead", 4);

            //Check if Approve All
            qry = "SELECT COUNT(*) AS RecCnt " +
                  " FROM dbo.tbl_MRP_List_Approval " +
                  " WHERE(MasterKey = " + MRPKey + ") " +
                  " AND(Status = 0)";
            cmd = new SqlCommand(qry);
            cmd.Connection = conn;
            adp = new SqlDataAdapter(cmd);
            adp.Fill(dtable);
            if (dtable.Rows.Count > 0)
            {
                foreach (DataRow row in dtable.Rows)
                {
                    if (Convert.ToInt32(row["RecCnt"]) == 0)
                    {
                        qry = "UPDATE tbl_MRP_List " +
                              " SET StatusKey = 4 " +
                              " WHERE (PK = " + MRPKey + ")";
                        cmdUp = new SqlCommand(qry, conn);
                        cmdUp.ExecuteNonQuery();

                        qry = "UPDATE tbl_MRP_List_Workflow " +
                              " SET Visible = 0, " +
                              " Status = 1 " +
                              " WHERE (MasterKey = " + MRPKey + ") " +
                              " AND (Line = 4)";
                        cmdUp = new SqlCommand(qry, conn);
                        cmdUp.ExecuteNonQuery();
                    }
                }
            }
            dtable.Clear();
            conn.Close();
        }

        private static void MRP_Approve_Executive(string docNum, int MRPKey, DateTime dteCreated, int ApproveLine, string EntCode, string BuCode, int usrKey)
        {
            SqlConnection conn = new SqlConnection(GlobalClass.SQLConnString());
            string qry = "";
            string CreatorEmail = "", CreatorSubject = "", CreatorGreetings = "";
            var sCreatorBody = new StringBuilder();
            string sEmail = "", sSubject = "", sGreetings = "";
            var sBody = new StringBuilder();

            int ApproveLineNext = ApproveLine + 1;

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

            SqlCommand cmd3 = null;
            SqlDataAdapter adp3;
            DataTable dtable3 = new DataTable();

            conn.Open();
            qry = "SELECT dbo.tbl_Users.Email, dbo.tbl_Users.Gender, dbo.tbl_Users.Lastname " +
                  " FROM dbo.tbl_MRP_List LEFT OUTER JOIN " +
                  " dbo.tbl_Users ON dbo.tbl_MRP_List.CreatorKey = dbo.tbl_Users.PK " +
                  " WHERE(dbo.tbl_MRP_List.PK = " + MRPKey + ")";
            cmd = new SqlCommand(qry);
            cmd.Connection = conn;
            adp = new SqlDataAdapter(cmd);
            adp.Fill(dtable);
            if (dtable.Rows.Count > 0)
            {
                foreach (DataRow row in dtable.Rows)
                {
                    CreatorEmail = EncryptionClass.Decrypt(row["Email"].ToString());
                    if (Convert.ToInt32(row["Gender"]) == 1)
                    {
                        CreatorGreetings = "Dear Mr. " + EncryptionClass.Decrypt(row["Lastname"].ToString());
                    }
                    else
                    {
                        CreatorGreetings = "Dear Ms. " + EncryptionClass.Decrypt(row["Lastname"].ToString());
                    }
                    CreatorSubject = "MOP DocNum " + docNum.ToString() + " status";

                    sCreatorBody.Append("<!DOCTYPE html>");
                    sCreatorBody.Append("<html>");
                    sCreatorBody.Append("<head>");
                    sCreatorBody.Append("</head>");
                    sCreatorBody.Append("<body>");
                    sCreatorBody.Append("<p style='font-family:Tahoma; font-size: 12px;'>" + CreatorGreetings + ",</p>");
                    sCreatorBody.Append("<p style='font-family:Tahoma; font-size: 12px;'>MOP Document # " + docNum.ToString() + " has been approved by Executive.</p>");
                    sCreatorBody.Append("<p style='font-family:Tahoma; font-size: 10px;font-style:italic;'>***This is a system-generated message. please do not reply to this email.***</p>");
                    sCreatorBody.Append("<p style='font-family:Tahoma; font-size: 10px;'>DISCLAIMER: This email is confidential and intended solely for the use of the individual to whom it is addressed. If you are not the intended recipient, be advised that you have received this email in error and that any use, dissemination, forwarding, printing or copying of this email is strictly prohibited. If you have received this email in error please notify the sender or email info@hijoresources.net, telephone number (082) 282-3662.</p>");
                    sCreatorBody.Append("</body>");
                    sCreatorBody.Append("</html>");
                }
            }
            dtable.Clear();

            //Update Approval
            qry = "UPDATE tbl_MRP_List_Approval " +
                   " SET Visible = 0, " +
                   " Status = 1 " +
                   " WHERE (MasterKey = " + MRPKey + ") " +
                   " AND (Line = " + ApproveLine + ")";
            cmdUp = new SqlCommand(qry, conn);
            cmdUp.ExecuteNonQuery();

            //bool msgSendToCreator = GlobalClass.IsMailSent(CreatorEmail, CreatorSubject, sCreatorBody.ToString());
            GlobalClass.CreateEmailNotification(CreatorEmail, CreatorGreetings, docNum, "has been approved by Executive", 4);

            //Check if Approve All
            qry = "SELECT COUNT(*) AS RecCnt " +
                  " FROM dbo.tbl_MRP_List_Approval " +
                  " WHERE(MasterKey = " + MRPKey + ") " +
                  " AND(Status = 0)";
            cmd = new SqlCommand(qry);
            cmd.Connection = conn;
            adp = new SqlDataAdapter(cmd);
            adp.Fill(dtable);
            if (dtable.Rows.Count > 0)
            {
                foreach (DataRow row in dtable.Rows)
                {
                    if (Convert.ToInt32(row["RecCnt"]) == 0)
                    {
                        qry = "UPDATE tbl_MRP_List " +
                              " SET StatusKey = 4 " +
                              " WHERE (PK = " + MRPKey + ")";
                        cmdUp = new SqlCommand(qry, conn);
                        cmdUp.ExecuteNonQuery();

                        qry = "UPDATE tbl_MRP_List_Workflow " +
                              " SET Visible = 0, " +
                              " Status = 1 " +
                              " WHERE (MasterKey = " + MRPKey + ") " +
                              " AND (Line = 4)";
                        cmdUp = new SqlCommand(qry, conn);
                        cmdUp.ExecuteNonQuery();
                    }
                }
                
            }
            dtable.Clear();
            conn.Close();
        }
    }
}