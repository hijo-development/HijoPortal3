﻿using System;
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
    public partial class createaccount : System.Web.UI.Page
    {
        private static string lastname = "", firstname = "", email = "", gender = "", employeepic = "/images/ID.jpg";
        private static int gender_int = -1, employeepic_int = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                lastname = "";
                firstname = "";
                email = "";
                gender = "";
                employeepic_int = 0;
                employeepic = "~/images/users/ID.jpg";
                EmployeeImage.ImageUrl = employeepic;
            }
        }
        protected void signUp_Click(object sender, EventArgs e)
        {
            int iEmployeeKey = 0;
            //MRPClass.PrintString("pass clicked signup");
            if (captcha.IsValid && ASPxEdit.ValidateEditorsInContainer(this))
            {
                ModalPopupExtenderLoading.Show();

                //MRPClass.PrintString("pass inside validation");
                DataTable dt = new DataTable();
                SqlCommand cmd = null;
                SqlDataAdapter adp;

                string qry = "";
                using (SqlConnection conHRIS = new SqlConnection(GlobalClass.SQLConnStringHRIS()))
                {
                    //MRPClass.PrintString("pass inside hris");
                    qry = "SELECT PK, IDNumber FROM dbo.tbl_EmployeeIDNumber WHERE(IDNumber = '" + IDNumTextBox.Text.ToString() + "')";
                    cmd = new SqlCommand(qry);
                    cmd.Connection = conHRIS;
                    adp = new SqlDataAdapter(cmd);
                    adp.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        //MRPClass.PrintString("pass inside hris with id");
                        foreach (DataRow row in dt.Rows)
                        {
                            iEmployeeKey = Convert.ToInt32(row["PK"]);
                        }
                    }
                    else
                    {
                        //Page.ClientScript.RegisterStartupScript(this.GetType(), "alert",
                        //    @"<script type=""text/javascript"">setTimeout(()=>{alert('ID Number not found in Employee MasterList!')},0);</script>");
                        ModalPopupExtenderLoading.Hide();
                        CreateAccntNotify.HeaderText = "Error...";
                        CreateAccntNotifyLbl.Text = "ID Number not found in Employee MasterList!";
                        CreateAccntNotifyLbl.ForeColor = System.Drawing.Color.Red;
                        CreateAccntNotify.ShowOnPageLoad = true;
                        return;
                    }
                    dt.Clear();
                    conHRIS.Close();
                }

                DataTable dtUser = AccountClass.UserList();

                //dtUser.CaseSensitive = true;
                string expressionID = "EmployeeKey = '" + iEmployeeKey.ToString().Trim() + "'";
                string sortOrderID = "PK ASC";
                DataRow[] foundRowsID;
                foundRowsID = dtUser.Select(expressionID, sortOrderID);
                if (foundRowsID.Length > 0)
                {
                    //Page.ClientScript.RegisterStartupScript(this.GetType(), "alert",
                    //        @"<script type=""text/javascript"">setTimeout(()=>{alert('Found Duplicate ID Number!')},0);</script>");
                    ModalPopupExtenderLoading.Hide();
                    CreateAccntNotify.HeaderText = "Error...";
                    CreateAccntNotifyLbl.Text = "Found Duplicate ID Number!";
                    CreateAccntNotifyLbl.ForeColor = System.Drawing.Color.Red;
                    CreateAccntNotify.ShowOnPageLoad = true;
                    return;
                }

                dtUser.CaseSensitive = true;
                string expressionName = "Lastname = '" + lastNameTextBox.Text.ToString().Trim() + "' AND Firstname = '" + firstNameTextBox.Text.ToString().Trim() + "'";
                string sortOrderName = "PK ASC";
                DataRow[] foundRowsName;
                foundRowsName = dtUser.Select(expressionName, sortOrderName);
                if (foundRowsName.Length > 0)
                {
                    //Page.ClientScript.RegisterStartupScript(this.GetType(), "alert",
                    //        @"<script type=""text/javascript"">setTimeout(()=>{alert('Found Duplicate Lastname and Firstname!')},0);</script>");
                    ModalPopupExtenderLoading.Hide();
                    CreateAccntNotify.HeaderText = "Error...";
                    CreateAccntNotifyLbl.Text = "Found Duplicate Lastname and Firstname!";
                    CreateAccntNotifyLbl.ForeColor = System.Drawing.Color.Red;
                    CreateAccntNotify.ShowOnPageLoad = true;
                    return;
                }

                string expressionEmail = "Email = '" + eMailTextBox.Text.ToString().Trim() + "'";
                string sortOrderEmail = "PK ASC";
                DataRow[] foundRowsEmail;
                foundRowsEmail = dtUser.Select(expressionEmail, sortOrderEmail);
                if (foundRowsEmail.Length > 0)
                {
                    //Page.ClientScript.RegisterStartupScript(this.GetType(), "alert",
                    //        @"<script type=""text/javascript"">setTimeout(()=>{alert('Found Duplicate Email!')},0);</script>");
                    ModalPopupExtenderLoading.Hide();
                    CreateAccntNotify.HeaderText = "Error...";
                    CreateAccntNotifyLbl.Text = "Found Duplicate Email!";
                    CreateAccntNotifyLbl.ForeColor = System.Drawing.Color.Red;
                    CreateAccntNotify.ShowOnPageLoad = true;
                    return;
                }

                string expressionUName = "Username = '" + userNameTextBox.Text.ToString().Trim() + "'";
                string sortOrderUName = "PK ASC";
                DataRow[] foundRowsUName;
                foundRowsUName = dtUser.Select(expressionUName, sortOrderUName);
                if (foundRowsUName.Length > 0)
                {
                    //Page.ClientScript.RegisterStartupScript(this.GetType(), "alert",
                    //        @"<script type=""text/javascript"">setTimeout(()=>{alert('Found Duplicate Username!')},0);</script>");
                    ModalPopupExtenderLoading.Hide();
                    CreateAccntNotify.HeaderText = "Error...";
                    CreateAccntNotifyLbl.Text = "Found Duplicate Username!";
                    CreateAccntNotifyLbl.ForeColor = System.Drawing.Color.Red;
                    CreateAccntNotify.ShowOnPageLoad = true;
                    return;
                }

                using (SqlConnection con = new SqlConnection(GlobalClass.SQLConnString()))
                {
                    string _sLastName, _sFirstName, _sEmail, _sUserName, _sPassword, _sIDNum;
                    int _Gender = 0;
                    _sLastName = EncryptionClass.Encrypt(GlobalClass.UpperCaseFirstLetter(lastNameTextBox.Text.ToString().Trim()));
                    _sFirstName = EncryptionClass.Encrypt(GlobalClass.UpperCaseFirstLetter(firstNameTextBox.Text.ToString().Trim()));
                    _Gender = gender_int;
                    _sEmail = EncryptionClass.Encrypt(eMailTextBox.Text.ToString().Trim());
                    _sUserName = EncryptionClass.Encrypt(userNameTextBox.Text.ToString().Trim());
                    _sPassword = EncryptionClass.Encrypt(passwordTextBox.Text.ToString().Trim());
                    _sIDNum = EncryptionClass.Encrypt(IDNumTextBox.Text.ToString().Trim());

                    con.Open();

                    qry = "INSERT INTO tbl_Users " +
                          " (Lastname, Firstname, Username, Password, Email, EmployeeKey, Gender) " +
                          " VALUES ('" + _sLastName + "', '" + _sFirstName + "', '" + _sUserName + "', " +
                          " '" + _sPassword + "', '" + _sEmail + "', " + iEmployeeKey + ", " + _Gender + ")"; ;
                    try
                    {
                        cmd = new SqlCommand(qry);
                        cmd.Connection = con;
                        cmd.ExecuteNonQuery();
                        con.Close();
                        //Page.ClientScript.RegisterStartupScript(this.GetType(), "alert",
                        //        @"<script type=""text/javascript"">setTimeout(()=>{alert('You have successfully registered')},0);</script>");

                        //MRPClass.PrintString("pass saved");

                        ModalPopupExtenderLoading.Hide();
                        CreateAccntNotify.HeaderText = "Info";
                        CreateAccntNotifyLbl.Text = "You is successfully registered.";
                        CreateAccntNotifyLbl.ForeColor = System.Drawing.Color.Black;
                        CreateAccntNotify.ShowOnPageLoad = true;

                        Response.Redirect("default.aspx");

                    }
                    catch (SqlException ex)
                    {
                        //MRPClass.PrintString(ex.ToString());
                        con.Close();
                        //Page.ClientScript.RegisterStartupScript(this.GetType(), "alert",
                        //        @"<script type=""text/javascript"">setTimeout(()=>{alert('" + ex.ToString() + "')},0);</script>");

                        CreateAccntNotify.HeaderText = "Error...";
                        CreateAccntNotifyLbl.Text = ex.ToString();
                        CreateAccntNotifyLbl.ForeColor = System.Drawing.Color.Red;
                        CreateAccntNotify.ShowOnPageLoad = true;
                    }
                }
            }
        }

        protected void firstnameCallback_Callback(object sender, CallbackEventArgsBase e)
        {
            FetchEmployeeInfo();
            if (!string.IsNullOrEmpty(firstname))
            {
                firstNameTextBox.Text = firstname;
                firstNameTextBox.IsValid = true;
            }
            else
                firstNameTextBox.Text = "";
        }

        protected void CallbackPanelIDNum_Callback(object sender, CallbackEventArgsBase e)
        {
            DataTable dt = new DataTable();
            SqlCommand cmd = null;
            SqlDataAdapter adp;
            string qry = "";
            IDNumTextBoxMatch.Text = "";
            //IDNumTextBoxMatch.Value = "";
            using (SqlConnection conHRIS = new SqlConnection(GlobalClass.SQLConnStringHRIS()))
            {
                qry = "SELECT PK, IDNumber FROM dbo.tbl_EmployeeIDNumber WHERE(IDNumber = '" + IDNumTextBox.Text.ToString() + "')";
                cmd = new SqlCommand(qry);
                cmd.Connection = conHRIS;
                adp = new SqlDataAdapter(cmd);
                adp.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        IDNumTextBoxMatch.Text = row["IDNumber"].ToString();
                        //IDNumTextBoxMatch.Value = row["IDNumber"].ToString();
                    }
                }
                dt.Clear();
                conHRIS.Close();
            }

            //if (IDNumTextBoxMatch.Text.Trim() == "")
            //{
            //    IDNumTextBox.IsValid = false;
            //    IDNumTextBox.ErrorText = "ID Number not found in Employee MasterList!";
            //} else
            //{
            //    IDNumTextBox.IsValid = true;
            //    IDNumTextBox.ErrorText = "";
            //}
        }

        protected void EmployeeImageCallback_Callback(object sender, CallbackEventArgsBase e)
        {
            FetchEmployeeInfo();
            //MRPClass.PrintString(employeepic);
            EmployeeImage.ImageUrl = employeepic;
        }

        protected void genderCallback_Callback(object sender, CallbackEventArgsBase e)
        {
            FetchEmployeeInfo();
            if (!string.IsNullOrEmpty(gender))
            {
                GenderTextbox.Text = gender;
                GenderTextbox.IsValid = true;
            }
            else
                GenderTextbox.Text = "";
        }

        protected void emailCallback_Callback(object sender, CallbackEventArgsBase e)
        {
            FetchEmployeeInfo();
            if (!string.IsNullOrEmpty(email))
            {
                eMailTextBox.Text = email;
                eMailTextBox.IsValid = true;
            }
            else
                eMailTextBox.Text = "";
        }

        protected void lastnameCallback_Callback(object sender, CallbackEventArgsBase e)
        {
            FetchEmployeeInfo();
            if (!string.IsNullOrEmpty(lastname))
            {
                lastNameTextBox.Text = lastname;
                lastNameTextBox.IsValid = true;
            }
            else
                lastNameTextBox.Text = "";
        }

        private void FetchEmployeeInfo()
        {
            string id = IDNumTextBox.Text;
            DataTable dt = AccountClass.GetEmployeeInfo(id);
            foreach (DataRow row in dt.Rows)
            {
                lastname = row["LastName"].ToString();
                firstname = row["FirstName"].ToString();
                email = row["Email"].ToString();
                gender = GetGender(row["Gender"].ToString());
                gender_int = Convert.ToInt32(row["Gender"].ToString());
                employeepic_int = AccountClass.EmployeePictureInHRIS(id);
                if (employeepic_int == 1)
                {
                    employeepic = "~/images/users/" + row["EmployeeKey"].ToString() + ".jpg";
                } else
                {
                    employeepic = "~/images/users/ID.jpg";
                }
            }
            if (dt.Rows.Count == 0)
            {
                lastname = "";
                firstname = "";
                email = "";
                gender = "";
                gender_int = -1;
                employeepic_int = 0;
                employeepic = "~/images/users/ID.jpg";
            }
        }

        private string GetGender(string str)
        {
            switch (str)
            {
                case "1":
                    return "Male";
                case "2":
                    return "Female";
                default:
                    return "";
            }
        }
    }
}