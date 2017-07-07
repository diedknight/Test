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
using PriceMe;
using PriceMeCommon.Data;

namespace HotterWinds.RetailerCenter
{
    public partial class RetailerSignUpSuccess : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //
            List<LinkInfo> linkInfoList = new List<LinkInfo>();
            LinkInfo linkInfo = new LinkInfo();
            linkInfo.LinkText = Resources.Resource.TextString_RetailerCentre;
            if (WebConfig.CountryId == 3)
                linkInfo.LinkURL = "http://promote.priceme.co.nz";
            else
                linkInfo.LinkURL = "/RetailerCenter/RetailerCenter.aspx";
            linkInfoList.Add(linkInfo);

            BreadCrumbInfo breadCrumbInfo = Utility.GetBreadCrumbInfo(linkInfoList, Resources.Resource.TextString_RetailerSignUpSuccessful);

            Master.SetBreadCrumb(breadCrumbInfo);

            Master.InitATag(string.Format(Resources.Resource.TextString_Checkout, this.Title));

            DynamicHtmlHeader.SetHtmlHeader("sign up, retailer, product, list, sales",
                "Sign up and list your products on Price Me. Increase sales and conversions.", this.Page);
        }

        protected void btnContinue_Click(object sender, EventArgs e)
        {
            Response.Redirect("/", false);
        }
    }
}