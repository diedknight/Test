using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using PriceMeDBA;
using SubSonic.Schema;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PamAccountGenerator
{
    public partial class SendEmail : System.Web.UI.Page
    {
        string accessKeyID = System.Configuration.ConfigurationManager.AppSettings["AWSAccessKey"];
        string secretAccessKeyID = System.Configuration.ConfigurationManager.AppSettings["AWSSecretKey"];
        AmazonSimpleEmailServiceClient ses;
        string CC = "";
        string BCC = "";
        string Title = "";
        string FromEmail = "";
        string FromName = "";
        string EmailContent = "";
        int country = 0;
        string uname = "";
        string upwd = "";
        int rid;
        string rContactName;
        string contactEmail;
        string cookiePPCRate;
        string cookieGST;
        public bool isAdmin = true;
        private string LogInUser = string.Empty;
        Dictionary<int, string> currencys = new Dictionary<int, string>();
        string RetailerName = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            //SendTest();
            LogInUser = HttpContext.Current.User.Identity.Name;
            if (string.IsNullOrEmpty(LogInUser)
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
            //var aa = EncodeClickStatisticsParam(1666, "Appliance-Outlet");
            ses = new AmazonSimpleEmailServiceClient(accessKeyID, secretAccessKeyID);

            if (ConfigurationManager.AppSettings["User"].Contains(LogInUser.ToLower()))
                isAdmin = true;

            string NewRetailerName = "";
            if (HttpContext.Current.Request.Cookies["NewRetailerName"] != null
                && HttpContext.Current.Request.Cookies["NewRetailerName"].Value != null)
            {
                NewRetailerName = HttpContext.Current.Request.Cookies["NewRetailerName"].Value;
                uname = NewRetailerName.Replace(" ", "").ToLower().Trim();
                upwd = HttpContext.Current.Request.Cookies["NewUPwd"].Value;
                country = int.Parse(HttpContext.Current.Request.Cookies["NewRetailerCountry"].Value);
                contactEmail = HttpContext.Current.Request.Cookies["NewContactEmail"].Value;
                rContactName = HttpContext.Current.Request.Cookies["NewRetailerContactName"].Value;
                cookiePPCRate = HttpContext.Current.Request.Cookies["cookiePPCRate"].Value;
                cookieGST = HttpContext.Current.Request.Cookies["cookieGST"].Value;
                rid = int.Parse(HttpContext.Current.Request.Cookies["NewRetailerId"].Value);

                string[] info = NewRetailerName.Split(';');
                string lblMsgStr = string.Format("Retailer {0} created successfully!", NewRetailerName);
                lblMsg.Text = lblMsgStr;
                lblNewUser.Text = string.Format("retailerID: {2}<br />username: {0}<br />user password: {1}",
                    uname, upwd, rid);
            }
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["Currency"]))
            {
                var currencyStr = ConfigurationManager.AppSettings["Currency"].Split(',');
                int cc = 0;
                foreach (var item in currencyStr)
                {
                    var aa = item.Split(':');
                    int.TryParse(aa[0], out cc);
                    if (cc > 0)
                        currencys.Add(cc, aa[1]);
                }
            }

            GetContentByCountryID_w();

           var Retailer = CSK_Store_Retailer.SingleOrDefault(s => s.RetailerId==rid);
           if (Retailer != null)
               RetailerName = Retailer.RetailerName;

            lblContent.Text = GetEmailBody(uname, upwd, cookiePPCRate, cookieGST, RetailerName);
        }

        protected void btnLogOut_Click(object sender, EventArgs e)
        {
            try
            {
                System.Web.Security.FormsAuthentication.SignOut();
                Response.Redirect("/Login.aspx?url=/", false);
            }
            catch (Exception ex)
            {
                RecordException(ex, "PamAccount - Redirect");
            }
        }
        
        private void GetContentByCountryID_w()
        {
            //CMSContent cc = CMSContentController.FindByPageID("Welcome email AU");//"PriceMe Account Confirmation");
            string pageID = "";

            if (country == 3)
                pageID = "PriceMe Account ConfirmationNZ_New";
            else if (country == 1)
                pageID = "PriceMe Account ConfirmationAU";
            else if (country == 28)
                pageID = "PriceMe Account ConfirmationPH";
            else if (country == 41)
                pageID = "PriceMe Account ConfirmationHK";
            else if (country == 45)
                pageID = "PriceMe Account ConfirmationMY";
            else if (country == 36)
                pageID = "PriceMe Account ConfirmationSG";
            else if (country == 51)
                pageID = "PriceMe Account ConfirmationID";
            else
                pageID = "";

            var sql = "select Ctx,CC,BCC,Title,FromEmail,FromName from CSK_Content where PageID like '%" + pageID + "%'";
            if (pageID != "")
            {
                StoredProcedure sp = new StoredProcedure("");
                sp.Command.CommandType = CommandType.Text;
                sp.Command.CommandSql = sql;
                IDataReader reader = sp.ExecuteReader();
                while (reader.Read())
                {
                    EmailContent = reader["Ctx"].ToString();
                    CC = reader["CC"].ToString();
                    BCC = reader["BCC"].ToString();
                    Title = reader["Title"].ToString();
                    FromEmail = reader["FromEmail"].ToString();
                    FromName = reader["FromName"].ToString();
                }
            }
        }
        
        private string Send2(string userName, string psd)
        {
            try
            {
                if (FromEmail == string.Empty)
                {
                    //SetResultMessage("TechnicalEmail is null.");
                    return "";
                }
                MailMessage emailMessage = new MailMessage();
                emailMessage.From = new MailAddress(FromEmail, FromName);
                emailMessage.To.Add(new MailAddress(contactEmail));

                if (CC != string.Empty)
                {
                    if (CC.Contains(";") || CC.Contains(","))
                    {
                        string[] temps = CC.Split(';');
                        if (!(temps.Length > 1))
                            temps = CC.Split(',');
                        for (int i = 0; i < temps.Length; i++)
                            emailMessage.CC.Add(new MailAddress(temps[i]));
                    }
                    else
                        emailMessage.CC.Add(new MailAddress(CC));
                }

                if (BCC != string.Empty)
                {
                    if (BCC.Contains(";") || BCC.Contains(","))
                    {
                        string[] temps = BCC.Split(';');
                        if (!(temps.Length > 1))
                            temps = BCC.Split(',');
                        for (int i = 0; i < temps.Length; i++)
                            emailMessage.Bcc.Add(new MailAddress(temps[i]));
                    }
                    else
                        emailMessage.Bcc.Add(new MailAddress(BCC));
                }

                emailMessage.ReplyToList.Add(new MailAddress(FromEmail));
                emailMessage.IsBodyHtml = true;
                emailMessage.Subject = Title;
                emailMessage.Body = GetEmailBody(userName, psd, cookiePPCRate, cookieGST, RetailerName);

                SmtpClient smtpClient = new SmtpClient();
                smtpClient.Send(emailMessage);

                return "";
            }
            catch (Exception e)
            {
                return e.Message + e.StackTrace;
            }
        }

        private bool Send(string userName, string psd)
        {
            try
            {
                if (FromEmail == string.Empty)
                {
                    //SetResultMessage("TechnicalEmail is null.");
                    return false;
                }

                AmazonSimpleEmailServiceClient ses = new AmazonSimpleEmailServiceClient(accessKeyID, secretAccessKeyID);
                List<string> list = new List<string>();
                Destination det = new Destination();
                list.Add(contactEmail);
                det.ToAddresses = list;
                if (CC != string.Empty)
                {
                    if (CC.Contains(";") || CC.Contains(","))
                    {
                        string[] temps = CC.Split(';');
                        if (!(temps.Length > 1))
                            temps = CC.Split(',');
                        for (int i = 0; i < temps.Length; i++)
                            det.CcAddresses.Add(temps[i]);
                    }
                    else
                        det.CcAddresses.Add(CC);
                }
                if (BCC != string.Empty)
                {
                    if (BCC.Contains(";") || BCC.Contains(","))
                    {
                        string[] temps = BCC.Split(';');
                        if (!(temps.Length > 1))
                            temps = BCC.Split(',');
                        for (int i = 0; i < temps.Length; i++)
                            det.BccAddresses.Add(temps[i]);
                    }
                    else
                        det.BccAddresses.Add(BCC);
                }

                Message mes = new Message();
                Amazon.SimpleEmail.Model.Content con = new Amazon.SimpleEmail.Model.Content();
                con.Data = Title;
                mes.Subject = con;
                string data = GetEmailBody(userName, psd, cookiePPCRate, cookieGST, RetailerName);
                Body body = new Body();
                Amazon.SimpleEmail.Model.Content conHtml = new Amazon.SimpleEmail.Model.Content();
                conHtml.Data = data;
                body.Text = conHtml;
                body.Html = conHtml;
                mes.Body = body;

                var sendEmailRequest = new SendEmailRequest(string.Format("{0} <{1}>", FromName, FromEmail), det, mes);
                List<string> replyToAddressesList = new List<string>();
                replyToAddressesList.Add(FromEmail);
                sendEmailRequest.ReplyToAddresses = replyToAddressesList;

                ses.SendEmail(sendEmailRequest);

            }
            catch (Exception ex)
            {
                CSK_Store_ExceptionCollect exc = new CSK_Store_ExceptionCollect();
                exc.ExceptionAppName = "PamAccountGenerator - SendEmail";
                exc.ExceptionInfo = ex.Message + "\r\n\r\n" + ex.StackTrace;
                exc.ExceptionType = "PamAccountGenerator - SendEmail";
                exc.errorPagePath = Request.Url.AbsoluteUri;
                exc.Save();
                return false;
            }
            return true;
        }

        private bool SendTest()
        {
            try
            {
                AmazonSimpleEmailServiceClient ses = new AmazonSimpleEmailServiceClient(accessKeyID, secretAccessKeyID);
                List<string> list = new List<string>();
                Destination det = new Destination();
                list.Add("huang@priceme.co.nz");
                det.ToAddresses = list;

                Message mes = new Message();
                Amazon.SimpleEmail.Model.Content con = new Amazon.SimpleEmail.Model.Content();
                con.Data = "test email";
                mes.Subject = con;
                string data = "test";
                Body body = new Body();
                Amazon.SimpleEmail.Model.Content conHtml = new Amazon.SimpleEmail.Model.Content();
                conHtml.Data = data;
                body.Text = conHtml;
                body.Html = conHtml;
                mes.Body = body;

                var sendEmailRequest = new SendEmailRequest(string.Format("PriceMe <support@priceme.com>"), det, mes);
                List<string> replyToAddressesList = new List<string>();
                replyToAddressesList.Add("huang@priceme.co.nz");
                sendEmailRequest.ReplyToAddresses = replyToAddressesList;

                ses.SendEmail(sendEmailRequest);

            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        private string GetEmailBody(string userName, string psd, string ppcRate, string gst, string retailerName)
        {
            string body = EmailContent;
            string firstName = string.Empty;
            if (rContactName != string.Empty)
                firstName = "Dear " + rContactName + ",";
            else
                firstName = "Hello";
            body = body.Replace("[f_firstname]", firstName);
            body = body.Replace("[f_username]", userName); //GetRetailerUser()
            body = body.Replace("[f_password]", psd);//GetPassWord()

            //displayed online email's content( url )
            //string retailerName = uname.Replace(" ", "-");
            body = body.Replace("[RetailerName]", "<b>" + retailerName + "</b>");
            body = body.Replace("[PPC_Rate]", ppcRate);
            body = body.Replace("[NeedGST]", gst);
            body = body.Replace("[Currency]", currencys[country]);
            body = body.Replace("[retailerid]", rid.ToString());
            body = body.Replace(ConfigurationManager.AppSettings["UpdatePamPwdSentence"],
                EncodeClickStatisticsParam(rid, userName));
            return body;
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("Success.aspx", false);
            }
            catch (Exception ex)
            {
                RecordException(ex, "PamAccount - Redirect");
            }
        }

        protected void btnSend_Click(object sender, EventArgs e)
        {
            bool sendSucess = Send(uname, upwd);
            //如果Accept T&Cs:选择的是Yes， 点Sign up按钮后，你需要判断retailer表中的列IsSetupComplete 是否为0，如果为0，就不需要做什么。如果为1，你还需要把PPCmember表的列IsNoLinkToPPC改为1。
            var retailer = CSK_Store_Retailer.SingleOrDefault(s => s.RetailerId == rid);
            if (retailer != null) {
                if (retailer.IsSetupComplete ?? false)
                {
                    var ppcs = CSK_Store_PPCMember.SingleOrDefault(s => s.RetailerId == retailer.RetailerId);
                    ppcs.IsNoLinkToPPC = true;
                    ppcs.Save();
                }
            }
            

            HttpCookie sendSucessC = new HttpCookie("NewSendSucess", sendSucess.ToString());
            //sendSucessC. = DateTime.Now.AddHours(1);
            Response.Cookies.Add(sendSucessC);

            try
            {
                Response.Redirect("Success.aspx", false);
            }
            catch (Exception ex)
            {
                RecordException(ex, "PamAccount - Redirect");
            }
        }

        public static string EncodeClickStatisticsParam(int retailerId, string userName)
        {
            string pwd = string.Format(ConfigurationManager.AppSettings.Get("ClickStatisticsSecurityKey"), retailerId);
            string param = string.Format("{0}", userName);
            byte[] bytes = Encoding.UTF8.GetBytes(param);
            MemoryStream msm = new MemoryStream(bytes);
            Stream st = MyCrypto.MyRijndael.EncryptStream(msm, pwd);
            bytes = new byte[st.Length];
            st.Read(bytes, 0, bytes.Length);
            st.Close();
            msm.Close();

            return string.Format("https://merchant.priceme.com/allow/SetupPassword.aspx?rid={0}&un={1}", 
                retailerId, Convert.ToBase64String(bytes).Replace('+', '_'));//base64 里面包括加号，但是加号在URL里被转成一个空格
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