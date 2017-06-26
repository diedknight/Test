using PriceMe;
using PriceMeCommon;
using PriceMeCommon.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HotterWinds.Modules.Catalog
{
    public partial class NewListView : System.Web.UI.UserControl
    {
        public string ProductName;
        public string ProductID;
        public string ShortDescriptionZH;
        public string MaxPrice;
        public string BestPrice;
        public string DefaultImage;
        public string PriceCount;
        public string CategoryID;
        public string AvRating;
        public string BestPPCRetailerName;
        public string CatalogProductURL;
        public string BestPPCRetailerProductID;
        public string CatalogDescription;
        public string DisplayName;
        public string BestPPCRetailerID;
        public string IsSearchProduct = "0";
        public int DayCount;
        public bool IsUpComing;
        public string ImageAlt;
        public string StarsImageAlt;
        public string ClickOutUrl;
        public string StarsScore;
        public bool IsSearchOnly;
        public string ComparePriceString;
        public bool ShowCompareBox;
        public string ParentPage = "c";
        public string ReviewCount;
        public string AttrDescription;
        public double PrevPrice;
        public float Sale;
        public float RatingPercent;
        public double CurrentPrice;
        public bool IsTop3;

        protected string VSOnclickScript;

        protected string linkUrl;
        protected bool isBestPPc = false;
        protected string ClassName;
        protected string ReviewCountString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (ProductID == null)
                return;
            if (DefaultImage == null)
            {
                DefaultImage = "";
            }

            if (string.IsNullOrEmpty(DefaultImage))
            {
                ImageAlt = "No Image";
            }
            else
            {
                ImageAlt = HttpUtility.HtmlEncode(ProductName);
            }

            CatalogProductURL = UrlController.GetProductUrl(int.Parse(ProductID), ProductName);

            //StarsScore = Utility.GetStarImage(score);

            ClickOutUrl = Utility.GetRootUrl("/ResponseRedirect.aspx?aid=40&pid=" + ProductID + "&rid=" + BestPPCRetailerID + "&rpid=" + BestPPCRetailerProductID + "&countryID=" + WebConfig.CountryId + "&cid=" + CategoryID + "&t=c", WebConfig.CountryId).Replace("&", "&amp;");
            string uuid = Guid.NewGuid().ToString();
            ClickOutUrl += "&uuid=" + uuid;

            linkUrl = CatalogProductURL;
            if (CategoryController.IsSearchOnly(int.Parse(CategoryID), WebConfig.CountryId))
            {
                if (!string.IsNullOrEmpty(BestPPCRetailerName))
                {
                    linkUrl = ClickOutUrl;
                    isBestPPc = true;
                }
                else
                    linkUrl = string.Empty;
            }

            if (this.Parent.ClientID.EndsWith("00"))
            {
                ClassName = "first";
            }

            int rCount = 0;
            int.TryParse(ReviewCount, out rCount);
            if (rCount > 0)
            {
                if (rCount == 1)
                {
                    ReviewCountString = 1 + " " + Resources.Resource.TextString_Review;
                }
                else
                {
                    ReviewCountString = rCount + " " + Resources.Resource.TextString_Reviews;
                }
            }
        }
    }
}