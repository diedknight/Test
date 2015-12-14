using PriceMeDBA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PamAccountGenerator
{
    public partial class Success : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(HttpContext.Current.User.Identity.Name)
                || !HttpContext.Current.Request.IsAuthenticated)//可能登录超时了
            {
                try
                {
                    Response.Redirect("/Login.aspx?url=/", false);
                }
                catch (Exception ex)
                {
                    RecordException(ex, "PamAccount - Redirect");
                }
            }

            string NewRetailerName = "";
            if (HttpContext.Current.Request.Cookies["NewRetailerName"] != null
                && HttpContext.Current.Request.Cookies["NewRetailerName"].Value != null)
            {
                NewRetailerName = HttpContext.Current.Request.Cookies["NewRetailerName"].Value;
                var uname = NewRetailerName.Replace(" ", "").ToLower().Trim();
                var upwd = HttpContext.Current.Request.Cookies["NewUPwd"].Value;
                int rid = int.Parse(HttpContext.Current.Request.Cookies["NewRetailerId"].Value);
                string[] info = NewRetailerName.Split(';');
                string lblMsgStr = "";

                if (HttpContext.Current.Request.Cookies["NewNeedSendEmail"] != null && HttpContext.Current.Request.Cookies["NewNeedSendEmail"].Value != null)
                {
                    var emailSendSuccess = HttpContext.Current.Request.Cookies["NewNeedSendEmail"].Value;
                    if (emailSendSuccess.ToLower() == "true") lblMsgStr = string.Format("Retailer {0} created successfully", NewRetailerName);
                } 
                if (HttpContext.Current.Request.Cookies["NewSendSucess"] != null && HttpContext.Current.Request.Cookies["NewSendSucess"].Value != null)
                {
                    var emailSendSuccess = HttpContext.Current.Request.Cookies["NewSendSucess"].Value;
                    if (emailSendSuccess.ToLower() == "true") lblMsgStr = "Welcome email sent sucussefully!";
                    else
                    {
                        lblMsgStr = "Welcome email Sent failed.";
                        lblMsg.Style.Add("color", "red");
                    }
                }

                lblMsg.Text = lblMsgStr;
                lblNewUser.Text = string.Format("retailerID: {2}<br />username: {0}<br />user password: {1}",
                    uname, upwd, rid);
            }
            else
            {
                lblMsg.Text = "";
                lblNewUser.Text = "Can not read new retailer info from cookies";
            }
        }

        protected void btnLogOut_Click(object sender, EventArgs e)
        {
            try
            {
                System.Web.Security.FormsAuthentication.SignOut();
                Response.Redirect("/Login.aspx?url=/");
            }
            catch (Exception ex)
            {
                RecordException(ex, "PamAccount - Redirect");
            }
        }

        private void RecordException(Exception ex, string type)
        {
            CSK_Store_ExceptionCollect exc = new CSK_Store_ExceptionCollect();
            exc.ExceptionAppName = "PamAccountGenerator";
            exc.ExceptionInfo = ex.Message + "\r\n\r\n" + ex.StackTrace;
            exc.ExceptionType = type;
            exc.errorPagePath = Request.Url.AbsoluteUri;
            exc.Save();
        }
    }
}