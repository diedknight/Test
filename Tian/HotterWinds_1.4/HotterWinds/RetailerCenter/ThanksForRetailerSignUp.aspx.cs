using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Net.Mail;
using System.Text;
using System.Net;
using PriceMeDBA;
using PriceMeCommon.Data;
using PriceMe;
using SubSonic.Schema;

namespace HotterWinds.RetailerCenter
{
    class CSK_Store_ECommercePlatform
    {
        public string Platform { get; set; }
        public string ID { get; set; }
    }

    public partial class ThanksForRetailerSignUp : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Title = "Thanks For Retailer Sign Up - Hotter Winds";

            if (Request.UrlReferrer == null || !Request.UrlReferrer.ToString().Contains("RetailerSignUp"))
                Response.Redirect("RetailerSignUp.aspx", false);

            if (!IsPostBack)
            {
                #region init
                var sql = "select * from CSK_Store_ECommercePlatform order by [Platform]";
                var sp = new StoredProcedure("");
                sp.Command.CommandType = CommandType.Text;
                sp.Command.CommandSql = sql;
                var ds = sp.ExecuteTypedList<CSK_Store_ECommercePlatform>();
                var item = ds.Find(p => p.Platform == "Don't know");
                if (item != null)
                {
                    ds.Remove(item);
                    ds.Insert(0, item);
                }
                item = ds.Find(p => p.Platform == "Other");
                if (item != null)
                {
                    ds.Remove(item);
                    ds.Insert(1, item);
                }

                ddlPlatforms.DataTextField = "Platform";
                ddlPlatforms.DataValueField = "ID";
                ddlPlatforms.DataSource = ds;
                ddlPlatforms.DataBind();

                sql = "select * from [CSK_Store_JobFunction]";
                sp = new StoredProcedure("");
                sp.Command.CommandType = CommandType.Text;
                sp.Command.CommandSql = sql;
                var ds2 = sp.ExecuteDataSet();

                ddlJobFunction.DataTextField = "JobFunction";
                ddlJobFunction.DataValueField = "ID";
                ddlJobFunction.DataSource = ds2;
                ddlJobFunction.DataBind();
                #endregion
            }
            if (WebConfig.Environment == "prod")
            {
                btnStep2.OnClientClick = "ga('send', 'event', 'thanks for retailer sign up', 'retailer', '0'); ";
            }

            //
            List<LinkInfo> linkInfoList = new List<LinkInfo>();
            LinkInfo linkInfo = new LinkInfo();
            linkInfo.LinkText = Resources.Resource.TextString_RetailerCentre;
            if (WebConfig.CountryId == 3)
                linkInfo.LinkURL = "http://promote.priceme.co.nz";
            else
                linkInfo.LinkURL = "/RetailerCenter/RetailerCenter.aspx";
            linkInfoList.Add(linkInfo);

            BreadCrumbInfo breadCrumbInfo = Utility.GetBreadCrumbInfo(linkInfoList, Resources.Resource.TextString_RetailerSignUp);

            Master.SetBreadCrumb(breadCrumbInfo);

            string check = string.Format(Resources.Resource.TextString_Checkout, this.Title);
            Master.InitATag(check);

            Master.AddCanonical("/RetailerCenter/ThanksForRetailerSignUp.aspx");

            DynamicHtmlHeader.SetHtmlHeader("sign up, retailer, product, list, sales",
                "Sign up and list your products on Price Me. Increase sales and conversions.", this.Page);
        }

        protected void btnStep2_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            try
            {
                #region input
                string retailerSignUpEmails = ConfigurationManager.AppSettings["RetailerSignUpEmails"].ToString();
                string[] signUpEmails = new string[retailerSignUpEmails.Split(',').Length];
                if (retailerSignUpEmails.Contains(","))
                    signUpEmails = retailerSignUpEmails.Split(',');
                else
                    signUpEmails[0] = retailerSignUpEmails;
                string retailerName = Session["retailerName"].ToString();
                string firstName = Session["firstName"].ToString();
                string lastName = Session["lastName"].ToString();
                string emailAddress = Session["emailAddress"].ToString();
                string webSiteURL = Session["webSiteURL"].ToString();
                string gstNumber = string.Empty;
                if (WebConfig.CountryId == 3)
                    gstNumber = Session["GSTNumber"].ToString();
                else
                    gstNumber = txtGSTNumber.Text.Trim();
                string jobFunctionV = ddlJobFunction.SelectedValue;
                string jobFunctionT = ddlJobFunction.SelectedItem.Text;
                string feedURL = txtFUrl.Text;
                string platFormV = ddlPlatforms.SelectedValue;
                string platFormT = ddlPlatforms.SelectedItem.Text;
                int oid = 0;
                int.TryParse(Session["NewOrganisationID"].ToString(), out oid);
                int pid = 0;
                int.TryParse(Session["NewPersonID"].ToString(), out pid);
                int hid = 0;
                int.TryParse(Session["NewHistoryID"].ToString(), out hid);
                int lid = 0;
                int.TryParse(Session["NewLeadID"].ToString(), out lid);

                Session.Remove("retailerName");
                Session.Remove("firstName");
                Session.Remove("lastName");
                Session.Remove("emailAddress");
                Session.Remove("webSiteURL");
                Session.Remove("GSTNumber");

                #endregion

                #region Capsule CRM

                CapsuleCRMHelper.UpdatePersonJobTitleAndHistoryNote(oid, hid, pid, platFormT, jobFunctionT, feedURL);

                Session.Remove("NewOrganisationID");
                Session.Remove("NewPersonID");
                Session.Remove("NewHistoryID");
                Session.Remove("NewLeadID");

                #endregion

                #region email
                string body = "<table><tbody><tr><td>Retaile Name:</td><td>" + retailerName
                       + "</td></tr><tr><td>First Name:</td><td>" + firstName
                       + "</td></tr><tr><td>Last Name:</td><td>" + lastName
                       + "</td></tr><tr><td>Email Address:</td><td>" + emailAddress
                       + "</td></tr><tr><td>Web Site URL:</td><td>" + webSiteURL;
                body += "</td></tr>";
                bool updateEmail = false;
                if (!string.IsNullOrEmpty(gstNumber))
                {
                    body += "<tr><td>" + Resources.Resource.RetailerInfo_StoreInfo_GSTNumber
                    + ":</td><td>" + gstNumber + "</td></tr>";
                    updateEmail = true;
                }
                if (jobFunctionV != "-1")
                {
                    body += "<tr><td>" + Resources.Resource.TextString_JobFunction + ":</td><td>" + jobFunctionT + "</td></tr>";
                    updateEmail = true;
                }
                if (!string.IsNullOrEmpty(feedURL))
                {
                    body += "<tr><td>" + Resources.Resource.TextString_FeedURL + ":</td><td>" + feedURL + "</td></tr>";
                    updateEmail = true;
                }
                body += "</tbody></table>";

                var emails = CSK_Store_EmailDatum.Find(p => p.FullName == (lastName + " " + firstName)
                    && p.EMail == emailAddress);
                if (emails != null)
                {
                    if (updateEmail)
                    {
                        CSK_Store_EmailDatum email = emails[0];
                        email.Message = body;
                        email.Save();
                    }
                }
                else
                {
                    CSK_Store_EmailDatum email = new CSK_Store_EmailDatum();
                    email.FullName = lastName + " " + firstName;
                    email.EMail = emailAddress;
                    email.Message = body;
                    email.Save();
                }
                #endregion

                #region retailerLead

                RetailerSignUpHelper.UpdateRetailerLead(lid, retailerName, webSiteURL, firstName, lastName, emailAddress,
                    oid, gstNumber, feedURL, int.Parse(jobFunctionV), int.Parse(platFormV));

                #endregion

                #region MailMessage
                if (updateEmail)
                {
                    List<string> list = new List<string>();
                    list.AddRange(signUpEmails);
                    var subject = retailerName + " Retailer Sign Up - " + Resources.Resource.Country;
                    if (emails != null)
                        subject = retailerName + " Retailer Sign Up - Step 2 - " + Resources.Resource.Country;
                    var source = string.Format("{0} <{1}>", retailerName, "support@priceme.co.nz");
                    CapsuleCRMHelper.DoSendEmail(list, emailAddress, body, subject, source);
                }

                #endregion

                Response.Redirect("RetailerSignUpSuccess.aspx", false);
            }
            catch (Exception ex)
            {
                CSK_Store_ExceptionCollect ec = new CSK_Store_ExceptionCollect();
                ec.ExceptionInfo = ex.Message + " " + ex.StackTrace;
                ec.ExceptionType = "Retailer SignUp Send Email";
                ec.errorPagePath = Request.RawUrl;
                ec.Save();
                Response.Redirect("RetailerSignUpSuccess.aspx", false);
            }
        }
    }
}