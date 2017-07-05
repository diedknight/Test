using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SubSonic;
using Commerce.Common;
using PriceMe;
using PriceMeCache;
using PriceMeCommon;
using PriceMeDBA;
using PriceMeCommon.Extend;
using PriceMeCommon.BusinessLogic;
using PriceMeCommon.Data;


namespace HotterWinds.Modules.Products
{
    public partial class MDProductPriceCompare : System.Web.UI.UserControl
    {
        public int CategoryId;
        public int RetailerId;
        public string ProductGUID;
        public string ProductID;
        public string ProductName;
        public decimal RetailerPrice;
        public string RetailerProductDescription;
        public int RetailerProductID;
        public decimal Freight;
        public string stockImg = string.Empty;
        public string StockStatus;
        public string itempropStock;

        public decimal FreightValue = -1;
        public string CCFeeAmount;
        public decimal TotalPrice;
        public string Stock;
        public int PPCMemberType;
        public bool IsNoLink;
        public bool IsFeaturedProduct = false;
        public decimal ccfeeValue = 0;
        public string OriginalPrice;
        public string type = "p";
        //public string PriceLocalCurrencyString;
        public decimal PriceLocalCurrency;
        public int RPCondition;
        public string OriginalString;

        protected string featuredProductCSS = "";
        public int pricesCount = 0;
        public string ccFeeString;
        public string lastDevDate;

        public string christmasString = "";

        public RetailerProductCondition condition;
        public string shortProductName;

        public PriceMeCache.RetailerCache retailer = null;
        protected CommonPPCMember ppc = null;
        public DateTime LastModify;
        public string TodayDate;
        public string YesterdayDate;

        protected bool isChristmasDate = false;
        //protected List<string> viewTrackingRetailers = new List<string>();
        public int PricePosition;
        public int PricePositionCount;
        public int Loc;
        public int? TradeMeSellerId;
        public string ExpirationDate;

        protected void Page_Load(object sender, EventArgs e)
        {
            //decimal.TryParse(PriceLocalCurrencyString, out PriceLocalCurrency);
            FreightValue = Freight;
            if (!String.IsNullOrEmpty(CCFeeAmount))
                ccfeeValue = decimal.Parse(CCFeeAmount);
            if (IsFeaturedProduct)
            {
                featuredProductCSS = " pricesDivFeatured";
            }

            if (!string.IsNullOrEmpty(OriginalPrice))
            {
                decimal price = 0m;
                decimal.TryParse(OriginalPrice, out price);
                if (price > 0 && price > RetailerPrice)
                    OriginalString = (decimal.Round(((price - RetailerPrice) / price) * 100, 0)) + "%";
            }

            retailer = RetailerController.GetRetailerDeep(RetailerId, WebConfig.CountryId);
            if (retailer.CCFee > 0)
                ccFeeString = decimal.Round((retailer.CCFee * 100), 1) + "%";

            if (RetailerController.IsPPcRetailer(RetailerId, WebConfig.CountryId))
            {
                ppc = RetailerController.GetPPcInfoByRetailerId(RetailerId, WebConfig.CountryId);
                if (ppc.PPCforInStockOnly && StockStatus == "0")
                    IsNoLink = true;
            }

            condition = ProductController.GetRetailerProductCondition(RPCondition, WebConfig.CountryId);

            if (IsNoLink)
            {
                if (condition == null)
                    shortProductName = ProductName.Length > 48 ? ProductName.Substring(0, 45) + "..." : ProductName;
                else
                    shortProductName = ProductName.Length > (48 - condition.ConditionName.Length) ? ProductName.Substring(0, (45 - condition.ConditionName.Length)) + "..." : ProductName;
            }
            else
            {
                if (condition == null)
                    shortProductName = ProductName.Length > 85 ? ProductName.Substring(0, 82) + "..." : ProductName;
                else
                    shortProductName = ProductName.Length > (85 - condition.ConditionName.Length) ? ProductName.Substring(0, (82 - condition.ConditionName.Length)) + "..." : ProductName;
            }

            BindStockStatus();
        }



        protected string GetProductDate(DateTime? time)
        {
            string date = (time ?? DateTime.Now).ToString("dd MMM", new System.Globalization.CultureInfo("en-us"));
            return date;
        }

        //protected String GetRetailerProductDescription(string RetailerProductDescription)
        //{
        //    string description = "";
        //    if (string.IsNullOrEmpty(RetailerProductDescription))
        //        return description;
        //    if (RetailerProductDescription.Length > 150)
        //        description = RetailerProductDescription.CutOut(150, "") + "...";
        //    if (description != "")
        //        description += " ";
        //    return description;
        //}

        //protected String GetStock(String stock)
        //{
        //    if (stock == null || stock.Length < 1 || stock == "" || stock.Equals("Nil"))
        //    {
        //        return "";
        //    }
        //    else
        //    {
        //        if (stock.Contains(".0"))
        //        {
        //            stock = stock.Split('.')[0];
        //        }
        //        return stock;
        //    }
        //}

        protected string GetDelivery(decimal freightValue)
        {
            string DeliveryInfo = "";

            if (freightValue > 0)
            {
                DeliveryInfo += Resources.Resource.TextString_PriceSymbol + decimal.Round(freightValue, 2);
            }
            else if (freightValue == 0)
            {
                DeliveryInfo += "<span class=\"pricesDivNoFee\">" + Resources.Resource.TextString_FreeShipping + "</span>";
            }

            return DeliveryInfo;
        }

        protected bool CheckIsCar()
        {
            bool isShowDes = false;

            int parentCategoryID = CategoryController.GetCategoryByCategoryID(CategoryId, WebConfig.CountryId).ParentID;

            if (parentCategoryID == 1754)
            {
                isShowDes = true;
            }
            return isShowDes;
        }

        protected string GetCCFee(decimal ccfee, decimal retailerProductPrice)
        {
            return decimal.Round(retailerProductPrice * ccfee, 4).Format(WebConfig.CurrentCulture);
        }

        public string FormatDecimal(decimal price)
        {
            string result = Utility.FormatPrice((double)price);

            return result;
        }

        //public string GetRetailerInfoUrl()
        //{
        //    Dictionary<string, string> param = new Dictionary<string, string>();
        //    param.Add("retailerId", retailer.RetailerId.ToString());

        //    return UrlController.GetRewriterUrl(PageName.RetailerInfo, param);
        //}

        //protected string GetRetailerProductUrl(int rpid, string rpname)
        //{
        //    return PriceMe.UrlController.GetRetailerProductUrl(rpid, rpname);
        //}

        protected string GetProductFees()
        {
            string fees = string.Empty;
            if (FreightValue > 0)
                fees = PriceMe.Utility.FormatPrice((double)FreightValue);
            else if (FreightValue == 0)
                fees = Resources.Resource.TextString_FREE_shipping;
            else
                fees = "&nbsp;";

            return fees;
        }

        protected void BindStockStatus()
        {
            if (!string.IsNullOrEmpty(StockStatus))
            {
                int st = 0;
                int.TryParse(Stock, out st);

                if (StockStatus == "-3")
                {
                    stockImg = "";
                    Stock = "Future stock";
                }
                else if (StockStatus == "-2" || st > 0)
                {
                    stockImg = " instock";
                    if (st > 0)
                        Stock = Stock + Resources.Resource.TextString_CountInStock;
                    else
                        Stock = Resources.Resource.TextString_InStock;
                    itempropStock = "InStock";
                }
                else if (StockStatus == "0")
                {
                    stockImg = " outofstock";
                    Stock = Resources.Resource.TextString_OutofStock;
                    itempropStock = "OutOfStock";
                }
                else if (StockStatus == "-1")
                    Stock = string.Empty;
            }
        }

        //public string GetLastModify(DateTime lastModify)
        //{
        //    var str = string.Empty;
        //    if (lastModify == DateTime.MinValue) return str;
        //    var date = lastModify.Date.ToString("yyyy-MM-dd");
        //    if (date == "1900-01-01") return str;
        //    if (date == YesterdayDate)
        //    {
        //        str = string.Format(Resources.Resource.TextString_UpdatedYesterdayAt, lastModify.ToString("HH:mm"));
        //    }
        //    else if (date == TodayDate)
        //    {
        //        str = string.Format(Resources.Resource.TextString_UpdatedTodayAt, lastModify.ToString("HH:mm"));
        //    }
        //    else
        //    {
        //        str = string.Format(Resources.Resource.TextString_UpdatedAt, lastModify.ToString("yyyy-MM-dd HH:mm", Utility.CurrentCulture));
        //    }
        //    return str;
        //}
    }
}