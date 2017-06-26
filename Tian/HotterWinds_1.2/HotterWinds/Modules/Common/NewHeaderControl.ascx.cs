using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HotterWinds.Modules.Common
{
    public partial class NewHeaderControl : System.Web.UI.UserControl
    {
        protected string loginUrl = string.Empty;
        public bool DisplayCategoryMenu = true;
        public int CategoryID = 0;
        protected bool isLogin = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            string urlhead = Resources.Resource.Global_HomePageUrl;
            this.SearchTextBox.Attributes.Add("placeholder", Resources.Resource.TextString_SearchCompareProducts);
            if (HttpContext.Current.User.Identity.IsAuthenticated)
                isLogin = true;
            //string oUrl = Request.RawUrl.ToLower();
            //if (!Request.RawUrl.ToLower().Contains("login.aspx") && !Request.RawUrl.ToLower().Contains("default.aspx"))
            //{
            //    loginUrl = "/Login.aspx" + "?url=" + PriceMe.Utility.UrlEncode(Request.RawUrl);
            //}
            //else
            //{
            //    loginUrl = "/Login.aspx";
            //}
            //if (this.CategoryNav != null)
            //{
            //    this.CategoryNav.CategoryID = CategoryID;
            //}


            this.SearchTextBox.Value = Request["q"];
        }

        protected void lnk_Click(object sender, EventArgs e)
        {
            System.Web.Security.FormsAuthentication.SignOut();
            HttpCookie hc = Request.Cookies["our_custom_session_cookienew_xxxx"];
            if (hc != null)
            {
                hc.Value = "0";
                hc.Expires = DateTime.Now.AddDays(-1);
                hc.Domain = "priceme.co.nz";

                Response.Cookies.Add(hc);
                Request.Cookies.Remove("our_custom_session_cookienew_xxxx");
            }
            if (Request.UrlReferrer.AbsoluteUri.Contains("/Community/"))
            {
                Response.Redirect("/");
            }
            else
            {
                Response.Redirect(Request.UrlReferrer.AbsoluteUri);
            }
        }

        protected string GetFirstName()
        {
            string userName = HttpContext.Current.User.Identity.Name;
            System.Web.Security.MembershipUserCollection msuc = System.Web.Security.Membership.FindUsersByName(userName);
            PriceMeDBA.aspnet_MembershipInfo msi = PriceMeCommon.BusinessLogic.MembershipInfoController.GetUserInfo(msuc[userName].ProviderUserKey.ToString(), PriceMe.WebConfig.CountryId);

            string fistName = msi.FirstName;

            if (fistName.Length > 2)
                fistName = fistName.Substring(0, 1).ToUpper() + fistName.Substring(1);
            return fistName;
        }

        public static string GetLoginControl(HttpContext context)
        {
            return PriceMe.Utility.GetLoginControl(context);
        }
    }
}