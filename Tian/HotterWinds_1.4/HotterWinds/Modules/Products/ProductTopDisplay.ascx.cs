using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PriceMeCommon.Extend;
using PriceMeDBA;
using PriceMe;
using PriceMeCommon.Data;
using PriceMeCommon.BusinessLogic;
using System.Data;
using PriceMeCache;

namespace HotterWinds.Modules.Products
{
    public partial class ProductTopDisplay : System.Web.UI.UserControl
    {
        protected CSK_Store_RetailerProductNew RetailerProduct;
        protected RetailerCache Retailer;
        protected string VisitShopRetailerProductURL;

        public DataSet ds = null;
        public CSK_Store_ProductNew Product;
        public int ExpertReviewCount;
        public int ReviewCount;
        public double PRatingValue;
        public string pid;
        public bool IsShowhReview = false;
        public bool IsSinglePrice;
        public decimal BestPrice;
        public decimal MaxPrice;
        public int RetailerCount;
        public string fromretailer;
        public string title = string.Empty;
        public int RetailerId;
        public int RetailerProductId;
        public ManufacturerInfo manufacturer = null;
        public string categoryName;
        public string manuLink = string.Empty;
        public string AttributeText = string.Empty;

        public string type = string.Empty;
        protected bool isPriceRange = false;
        public int id = 0;
        public string reportType = "d";
        public string parentPage = "";
        public string fromPage = "";
        public bool showLink = false;
        public string ManufacturerUrl = string.Empty;

        public bool ismylist = false;

        public int InStockStatus = 0;

        public Tuple<int, int> ReviewsRatings = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            Retailer = RetailerController.GetRetailerFromCache(RetailerId, WebConfig.CountryId);
            Retailer = Retailer ?? new RetailerCache();

            RetailerProduct = ProductController.GetRetailerProductNew(RetailerProductId, WebConfig.CountryId);
            RetailerProduct = RetailerProduct ?? new CSK_Store_RetailerProductNew();

            ManufacturerUrl = PriceMe.UrlController.GetBrandPageUrl(manufacturer.ManufacturerID);
            //新加代码 2015-8-18 10：10
            var userReviews = ReviewController.SearchUserReviewByProductID(Product.ProductID, WebConfig.CountryId);

            ReviewsRatings = DBQuery.ProductQuery.GetProductRating(Product.ProductID);
            if (ReviewsRatings == null)
            {
                ReviewsRatings = new Tuple<int, int>(0, 0);
            }

            //ProductRatingControl1.productId = Product.ProductID;
            //ProductRatingControl1.productName = Product.ProductName;
            //ProductRatingControl1.PRatingValue = PRatingValue;
            //ProductRatingControl1.ExpertReviewCount = ExpertReviewCount + userReviews.Count();
            //ProductRatingControl1.ReviewCount = ReviewCount;
            //ProductRatingControl1.type = type;

            id = Product.ProductID;
            pid = Product.ProductID.ToString();

            fromretailer = Resources.Resource.Product_TopDisplay_FromRetailer.Replace("{0}", "<span itemprop=\"offerCount\">" + RetailerCount.ToString() + "</span>");
            if (RetailerCount != 1 && WebConfig.CountryId != 41 && WebConfig.CountryId != 56)
            {
                fromretailer += "s";
                if (RetailerCount == 0)
                {
                    fromretailer = fromretailer.Replace("from", "From");
                }
            }
            else if (RetailerCount == 1)
            {
                PriceMeCache.RetailerCache rc = RetailerController.GetRetailerDeep(RetailerId, WebConfig.CountryId);
                if (rc != null)
                {
                    string scriptFormat = "on_clickOut('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', " + PriceMe.WebConfig.Use_GoogleTrackConversion + ")";
                    string VSOnclickScript = string.Format(scriptFormat, Guid.NewGuid(), Product.ProductID, RetailerId, RetailerProductId,
                        Product.ProductName.Replace("'", " ").Replace("\"", "").Replace("\r\n", "").Replace("\r", "").Replace("\n", ""),
                        "0", BestPrice.ToString("0.00"), "&t=pt", WebConfig.CountryId, WebConfig.Environment == "prod");
                    //VSOnclickScript = "RecordabTesting('product price compare','');" + VSOnclickScript;

                    string RetailerProductURL = Utility.GetRootUrl("/ResponseRedirect.aspx?aid=40&pid=" + Product.ProductID + "&rid=" +
                        RetailerId + "&rpid=" + RetailerProductId + "&countryID=" + WebConfig.CountryId + "&cid=" +
                        Product.CategoryID + "&t=pt", WebConfig.CountryId);
                    string uuid = Guid.NewGuid().ToString();
                    RetailerProductURL += "&uuid=" + uuid;

                    string retailerName = rc.RetailerName;
                    bool isNolink = true;
                    if (RetailerController.IsPPcRetailer(rc.RetailerId, WebConfig.CountryId))
                    {
                        isNolink = false;
                    }

                    if (!isNolink)
                        fromretailer = Resources.Resource.TextString_comparepricesFrom + " <a href=\"" + RetailerProductURL + "\" rel=\"nofollow\" target=\"_blank\" onclick=\"" + VSOnclickScript + "\">" + retailerName + "</a>";
                    else
                        fromretailer = Resources.Resource.TextString_comparepricesFrom + " " + retailerName;
                }
            }
            else if (RetailerCount == 0)
            {
                fromretailer = Resources.Resource.Product_TopDisplay_FromRetailer.Replace("{0}", "<span>" + RetailerCount.ToString() + "</span>");
            }
            if (BestPrice != MaxPrice)
                isPriceRange = true;

            if (!string.IsNullOrEmpty(Product.ProductAttributeText))
            {
                AttributeText = Product.ProductAttributeText;
            }

            if (fromPage == "product") showLink = false;

            ismylist = Utility.IsMyList(Product.ProductID);

            VisitShopRetailerProductURL = PriceMe.Utility.GetRootUrl("/ResponseRedirect.aspx?pid=" + Product.ProductID + "&rid=" + Retailer.RetailerId + "&rpid=" + RetailerProduct.RetailerProductId + "&countryID=" + PriceMe.WebConfig.CountryId + "&cid=" + Product.CategoryID + "&aid=40&t=" + "HW", PriceMe.WebConfig.CountryId);
            VisitShopRetailerProductURL += "&uuid=" + Guid.NewGuid().ToString(); ;
        }

        protected string GetShorDescription()
        {
            string shortDes = "";
            if (!string.IsNullOrEmpty(Product.ShortDescriptionZH))
                shortDes = Product.ShortDescriptionZH;

            return shortDes;
        }

        protected string GetPriceRange()
        {
            string priceRange = "";

            if (BestPrice != 0)
            {
                if (BestPrice == MaxPrice)
                {
                    priceRange = Utility.FormatPrice((double)BestPrice);
                    priceRange = "<meta itemprop=\"lowPrice\" content=\"" + priceRange.Split(new string[] { "<span class=\'priceSpan\'>" }, StringSplitOptions.None)[1].Split('<')[0].Replace(",", "") + "\" />" + priceRange;
                }
                else
                {
                    string bestPriceString = Utility.FormatPrice((double)BestPrice);
                    bestPriceString = "<meta itemprop=\"lowPrice\" content=\"" + bestPriceString.Split(new string[] { "<span class=\'priceSpan\'>" }, StringSplitOptions.None)[1].Split('<')[0].Replace(",", "") + "\" />" + bestPriceString;
                    //bestPriceString = bestPriceString.Replace("<span class=\'priceSpan\'>", "<span class=\"priceSpan\" itemprop=\"lowPrice\">");

                    string maxPriceString = Utility.FormatPrice((double)MaxPrice);
                    maxPriceString = "<meta itemprop=\"highPrice\" content=\"" + maxPriceString.Split(new string[] { "<span class=\'priceSpan\'>" }, StringSplitOptions.None)[1].Split('<')[0].Replace(",", "") + "\" />" + maxPriceString;
                    //maxPriceString = maxPriceString.Replace("<span class=\'priceSpan\'>", "<span class=\"priceSpan\" itemprop=\"highPrice\">");

                    priceRange = bestPriceString + " - " + maxPriceString;
                }
            }

            priceRange = priceRange.Replace("¥", "$");

            return priceRange;
        }

        protected string GetMinPrice()
        {
            var price_ = PriceMe.Utility.FormatPriceNotPriceSymbol((double)BestPrice).Replace(",", "");
            if (WebConfig.CountryId == 51
                || WebConfig.CountryId == 56
                )
                price_ = price_.Replace(".", "");
            return price_;
        }
    }
}