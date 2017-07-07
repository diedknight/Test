using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
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
using System.IO;

namespace HotterWinds.RetailerCenter
{
    public partial class RetailerSignUp : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Title = "Retailer Sign up - Hotter Winds";

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

            Master.AddCanonical("/RetailerCenter/RetailerSignUp.aspx");

            DynamicHtmlHeader.SetHtmlHeader("sign up, retailer, product, list, sales",
                "Sign up and list your products on Price Me. Increase sales and conversions.", this.Page);
            if (WebConfig.CountryId != 3)
            {
                txtGSTNumber.ValidationGroup = rfvGst.ValidationGroup = "step1" + WebConfig.CountryId;
            }
        }

        protected void btnSetp1_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            #region input
            string retailerSignUpEmails = ConfigurationManager.AppSettings["RetailerSignUpEmails"].ToString();
            string[] signUpEmails = retailerSignUpEmails.Split(',');
            string retailerName = txtRName.Text;
            string firstName = "";
            string lastName = "";
            var contactName = txtContactName.Text.Trim().Split(' ');
            if (contactName.Length == 1)
            {
                firstName = contactName[0];
            }
            else if (contactName.Length == 2)
            {
                firstName = contactName[0];
                lastName = contactName[1];
            }
            else
            {
                lastName = contactName[contactName.Length - 1];
                firstName = String.Join(" ", contactName, 0, contactName.Length - 1);
            }

            string emailAddress = txtEAddress.Text;
            string webSiteURL = txtWUrl.Text;
            string gstNumber = txtGSTNumber.Text;

            Session["retailerName"] = retailerName;
            Session["firstName"] = firstName;
            Session["lastName"] = lastName;
            Session["emailAddress"] = emailAddress;
            Session["webSiteURL"] = webSiteURL;
            Session["GSTNumber"] = gstNumber;

            #endregion

            #region crm

            var pid = string.Empty;
            var hid = string.Empty;
            var oid = CapsuleCRMHelper.NewOrganisationAndPerson(retailerName, webSiteURL, firstName, lastName, emailAddress, gstNumber, out pid, out hid);

            Session["NewOrganisationID"] = oid;
            Session["NewPersonID"] = pid;
            Session["NewHistoryID"] = hid;

            #endregion

            try
            {
                #region email
                string bodyadmin = "<table><tbody><tr><td>Retaile Name:</td><td>" + retailerName
                       + "</td></tr><tr><td>First Name:</td><td>" + firstName
                       + "</td></tr><tr><td>Last Name:</td><td>" + lastName
                       + "</td></tr><tr><td>Email Address:</td><td>" + emailAddress
                       + "</td></tr><tr><td>Web Site URL:</td><td>" + webSiteURL;
                if (WebConfig.CountryId == 3)
                {
                    bodyadmin += "</td></tr><tr><td>" + Resources.Resource.RetailerInfo_StoreInfo_GSTNumber
                    + ":</td><td>" + gstNumber;
                }
                bodyadmin += "</td></tr></tbody></table>";

                string body = "<p>Dear " + firstName + ",</p>"

                + "<p>Thanks for your interest in PriceMe.We have successfully received your details and we will be contacting you with more information shortly.</p>"

                + "<p>In the meantime please find more information in our services portal:</p>"

                + "<p><a href='https://support.priceme.co.nz'>https://support.priceme.co.nz</a></p>"

                + "<p>We look forward to providing your online store with shopper referrals.</p>"

                + "<p>Best regards, PriceMe</p>";

                CSK_Store_EmailDatum email = new CSK_Store_EmailDatum();
                email.FullName = lastName + " " + firstName;
                email.EMail = emailAddress;
                email.Message = body;
                email.Save();

                CSK_Content cc = CMSContentController.FindByPageID("Retailer Sign Up Email", WebConfig.CountryId);
                var body_ = GetEmailBody(cc, firstName);
                CSK_Store_EmailSendOut emailSend = new CSK_Store_EmailSendOut();
                emailSend.ID = email.ID;
                emailSend.EMail = emailAddress;
                emailSend.Message = body_;
                emailSend.RetailerName = retailerName;
                emailSend.SendStatus = 0;
                emailSend.ResourceCountry = Resources.Resource.Country;
                emailSend.Save();

                var source_ = "PriceMe <support@priceme.co.nz>";

                CapsuleCRMHelper.DoSendEmail(new List<string> { emailAddress }, signUpEmails[0], body_, cc.Title, source_);

                #region retailerLead

                int oid_ = 0;
                int.TryParse(oid, out oid_);
                Session["NewLeadID"] = RetailerSignUpHelper.NewRetailerLead(retailerName, webSiteURL, firstName, lastName, emailAddress, gstNumber, oid_);

                #endregion


                List<string> list = new List<string>();
                list.AddRange(signUpEmails);
                var subject = retailerName + " " + Resources.Resource.TextString_RetailerSignUp + " - " + Resources.Resource.Country;
                var source = string.Format("{0} <{1}>", retailerName, "support@priceme.co.nz");

                CapsuleCRMHelper.DoSendEmail(list, emailAddress, bodyadmin, subject, source);

                #endregion

                Response.Redirect("ThanksForRetailerSignUp.aspx", false);
            }
            catch (Exception ex)
            {
                CSK_Store_ExceptionCollect ec = new CSK_Store_ExceptionCollect();
                ec.ExceptionInfo = ex.Message;
                ec.ExceptionType = "Retailer SignUp Send Email";
                ec.errorPagePath = Request.RawUrl;
                ec.Save();
                Response.Redirect("ThanksForRetailerSignUp.aspx", false);
            }
        }

        private string GetEmailBody(CSK_Content cc, string firstName)
        {
            string body = string.Empty;

            string path = Page.Server.MapPath("").Replace("RetailerCenter", "") + "Retailer_Sign_Up_Email-NZ-activation.html";
            if (File.Exists(path))
                body = File.ReadAllText(path);

            string _firstName = firstName;
            body = body.Replace("[f_firstname]", _firstName);

            return body;
        }
    }
}