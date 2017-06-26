using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PriceMe;
using PriceMeCommon.Data;
using PriceMeDBA;

namespace HotterWinds.About
{
    public partial class ContactUs : System.Web.UI.Page
    {
        private string userIp = "";
        public int count = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            RequiredFieldValidator1.ErrorMessage = Resources.Resource.TextString_ContactUsError1;
            RequiredFieldValidator2.ErrorMessage = Resources.Resource.TextString_ContactUsError2;
            RequiredFieldValidator3.ErrorMessage = Resources.Resource.TextString_ContactUsError3;

            userIp = Utility.GetClientIPAddress(this.Context);
            count = GetIPPost(userIp);

            if (!IsPostBack)
            {
                List<EMailInfo> emailInfoCollection = null;
                object obj = Cache.Get("EMailInfo");
                if (obj != null)
                {
                    emailInfoCollection = obj as List<PriceMeDBA.EMailInfo>;
                }
                else
                {
                    emailInfoCollection = PriceMeDBA.EMailInfo.All().ToList();
                    emailInfoCollection.RemoveAll(p => p.EMailID == 2);
                    PriceMeDBA.EMailInfo eMailInfo = new PriceMeDBA.EMailInfo();
                    eMailInfo.EmailName = Resources.Resource.TextString_PleaseSelect;
                    eMailInfo.EmailAddress = "-1";
                    emailInfoCollection.Insert(0, eMailInfo);
                    if (WebConfig.CountryId == 1)
                    {
                        emailInfoCollection.RemoveAt(1);
                    }

                    Cache.Add("EMailInfo", emailInfoCollection, null, DateTime.Now.AddHours(4), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.Normal, null);
                }



                this.EMailDropDownList.DataValueField = "EMailID";
                this.EMailDropDownList.DataTextField = "EmailName";

                this.EMailDropDownList.DataSource = emailInfoCollection;
                this.EMailDropDownList.Attributes.Add("onchange", "checkEmailSelect('" + this.EMailDropDownList.ClientID + "')");                
                this.EMailDropDownList.DataBind();

                this.EMailDropDownList.Items.Add(new ListItem("Website suggestion", "1"));
                this.EMailDropDownList.SelectedValue = "1";
            }

            string description = "Contact PriceMe using email.";
            string keywords = "Contact, PriceMe, email";

            DynamicHtmlHeader.SetHtmlHeader(keywords, description, this);
            // this.btnSend.ImageUrl = (Resources.Resource.ImageWebsite + "/images/button_send.gif");

            SetBreadCrumb();

            this.Title = Resources.Resource.TextString_ContactUs;

            Master.AddCanonical("/About/ContactUs.aspx");
        }

        private void SetBreadCrumb()
        {
            System.Collections.Generic.List<LinkInfo> linkInfoList = new System.Collections.Generic.List<LinkInfo>();
            LinkInfo linkInfo = new LinkInfo();
            linkInfo.LinkText = "About";
            linkInfo.LinkURL = "/About/AboutUs.aspx";
            linkInfoList.Add(linkInfo);

            BreadCrumbInfo breadCrumbInfo = Utility.GetBreadCrumbInfo(linkInfoList, Resources.Resource.TextString_ContactUs);

            Master.SetBreadCrumb(breadCrumbInfo);
            string check = string.Format(Resources.Resource.TextString_Checkout, this.Title);
            Master.InitATag(check);
        }

        protected void btnSend_Click1(object sender, EventArgs e)
        {
            if (count > 1 && Session[SessionKey.ProductReviewCaptcha.ToString()] != null && String.Compare(txtCaptcha.Text.Trim().ToLower(), Session[SessionKey.ProductReviewCaptcha.ToString()].ToString().ToLower(), true) != 0)
            {
                ResultMessage1.ShowFail("Captcha error!");
                return;
            }

            if (this.EMailDropDownList.SelectedValue == "-1" || this.EMailDropDownList.SelectedValue == "0")
            {
                ResultMessage1.ShowFail(" Message not sent. Please select a topic.");
                return;
            }

            List<string> emails = GetEmails(this.EMailDropDownList.SelectedValue);

            if (GlobalOperator.ContactUsEmail(txtFullName.Text, txtEmail.Text, txtMSG.Text, emails, this.EMailDropDownList.SelectedItem.Text))
            {
                count++;
                Cache.Insert(userIp, count, null, DateTime.Now.AddHours(5), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.Normal, null);
                Response.Redirect("~/About/ContactUsSuccess.aspx");
            }
            else
            {
                ResultMessage1.ShowFail(Resources.Resource.TextString_EmailFails);
            }
        }

        private List<string> GetEmails(string emailID)
        {
            int eID = int.Parse(emailID);
            PriceMeDBA.EMailInfo emailInfo = PriceMeDBA.EMailInfo.SingleOrDefault(e => e.EMailID == eID);
            string[] emails = emailInfo.EmailAddress.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            List<string> emailList = new List<string>();
            emailList.AddRange(emails);
            return emailList;
        }

        int GetIPPost(string ip)
        {
            object c = Cache.Get(ip);
            if (c == null)
                return 0;
            int count;
            int.TryParse(c.ToString(), out count);
            return count;
        }
    }
}