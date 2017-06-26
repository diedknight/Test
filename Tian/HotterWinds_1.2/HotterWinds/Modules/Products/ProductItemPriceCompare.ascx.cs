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
using SubSonic;
using Commerce.Common;
using PriceMe;
using PriceMeCommon;
using PriceMeCache;
using PriceMeDBA;
using PriceMeCommon.Extend;
using PriceMeCommon.Data;
using PriceMeCommon.BusinessLogic;

namespace HotterWinds.Modules.Products
{
    public partial class ProductItemPriceCompare : System.Web.UI.UserControl
    {
        public List<CSK_Store_RetailerProductNew> rps;

        public int CategoryId;
        public int RetailerId;
        public string ProductID;
        public string ProductName;
        public decimal RetailerPrice;
        public string RetailerProductDescription;
        public int RetailerProductID;
        public string stockImg = string.Empty;
        public string StockStatus;
        public string itempropStock = string.Empty;
        public decimal OriginalPrice;
        public decimal PriceLocalCurrency;

        public string OriginalString;

        public decimal FreightValue = -1;
        public string CCFeeAmount;
        public decimal TotalPrice;
        public string Stock;
        public int PPCMemberType;
        public bool IsNoLink;
        public bool IsFeaturedProduct = false;
        public decimal ccfeeValue = 0;

        protected string retailerInfoCSS = "pricesDivRetailerInfo";
        public int pricesCount = 0;
        public string ccFeeString = string.Empty;
        public RetailerProductCondition condition;
        public string shortProductName;
        public string lastDevDate = string.Empty;

        public string christmasString = "";
        public PriceMeCache.RetailerCache retailer = null;
        protected CommonPPCMember ppc = null;
        public DateTime LastModify;
        public string TodayDate;
        public string YesterdayDate;

        protected bool isChristmasDate = false;

        protected string featuredProductCSS;

        public int PricePosition;
        public int PricePositionCount;
        public int Loc;
        public int TradeMeSellerId;


        //protected List<string> viewTrackingRetailers = new List<string>();
        protected void Page_Load(object sender, EventArgs e)
        {
            //用第一个List Value赋值
            RetailerProductID = rps[0].RetailerProductId;
            ProductName = rps[0].RetailerProductName;
            RetailerPrice = rps[0].RetailerPrice;
            TotalPrice = rps[0].TotalPrice;
            TradeMeSellerId = rps[0].TradeMeSellerId ?? 0;
            RetailerProductDescription = rps[0].RetailerProductDescription;
            FreightValue = rps[0].Freight ?? 0m;
            ccfeeValue = rps[0].CCFeeAmount ?? 0m;
            IsFeaturedProduct = rps[0].IsFeaturedProduct;
            IsNoLink = rps[0].IsNoLink;
            PPCMemberType = rps[0].PPCMemberType;
            Stock = rps[0].Stock;
            StockStatus = rps[0].StockStatus;
            OriginalPrice = rps[0].OriginalPrice ?? 0;

            if (OriginalPrice > 0 && OriginalPrice > RetailerPrice)
                OriginalString = (decimal.Round(((OriginalPrice - RetailerPrice) / OriginalPrice) * 100, 0)) + "%";
            ProductID = rps[0].ProductId.ToString();
            PricePosition = rps[0].PricePosition;
            PricePositionCount = rps[0].PricePositionCount;
            Loc = rps[0].Loc;

            TodayDate = DateTime.Now.Date.ToString("yyyy-MM-dd");
            YesterdayDate = DateTime.Now.Date.AddDays(-1).ToString("yyyy-MM-dd");

            retailer = RetailerController.GetRetailerDeep(RetailerId, WebConfig.CountryId);
            if (retailer.CCFee > 0)
                ccFeeString = decimal.Round((retailer.CCFee * 100), 1) + "%";
            condition = ProductController.GetRetailerProductCondition(rps[0].RetailerProductCondition ?? 0, WebConfig.CountryId);

            if (RetailerController.IsPPcRetailer(RetailerId, WebConfig.CountryId))
            {
                ppc = RetailerController.GetPPcInfoByRetailerId(RetailerId, WebConfig.CountryId);

                if (ppc.PPCforInStockOnly && StockStatus == "0")
                    IsNoLink = true;
            }

            if (IsNoLink)
            {
                if (condition == null)
                    shortProductName = ProductName;
                else
                    shortProductName = ProductName.Length > (48 - condition.ConditionName.Length) ? ProductName.Substring(0, (45 - condition.ConditionName.Length)) + "..." : ProductName;
            }
            else
            {
                if (condition == null)
                    shortProductName = ProductName.Length > 88 ? ProductName.Substring(0, 85) + "..." : ProductName;
                else
                    shortProductName = ProductName.Length > (88 - condition.ConditionName.Length) ? ProductName.Substring(0, (85 - condition.ConditionName.Length)) + "..." : ProductName;
            }

            if (IsFeaturedProduct)
            {
                featuredProductCSS = " pricesDivFeatured";
            }

            BindStockStatus();
        }


        protected string GetProductDate(DateTime? time)
        {
            string date = (time ?? DateTime.Now).ToString("dd MMM", new System.Globalization.CultureInfo("en-us"));
            return date;
        }

        //protected String GetRetailerProductDescription(string RetailerProductDescription) {
        //    string description = "";
        //    if (string.IsNullOrEmpty(RetailerProductDescription))
        //        return description;
        //    if (RetailerProductDescription.Length > 150)
        //        description = RetailerProductDescription.CutOut(150, "") + "...";
        //    if (description != "")
        //        description += " ";
        //    return description;
        //}

        //protected String GetStock(String stock) {
        //    if (stock == null || stock.Length < 1 || stock == "" || stock.Equals("Nil")) {
        //        return "";
        //    } else {
        //        if (stock.Contains(".0")) {
        //            stock = stock.Split('.')[0];
        //        }
        //        return stock;
        //    }
        //}

        //protected string GetDelivery(decimal freightValue) {
        //    string DeliveryInfo = "";

        //    if (freightValue > 0) {
        //        DeliveryInfo += Resources.Resource.TextString_PriceSymbol + decimal.Round(freightValue, 2);
        //    } else if (freightValue == 0) {
        //        DeliveryInfo += "<span class=\"pricesDivNoFee\">" + Resources.Resource.TextString_FreeShipping + "</span>";
        //    }

        //    return DeliveryInfo;
        //}

        protected string GetCCFee(decimal ccfee, decimal retailerProductPrice)
        {
            return decimal.Round(retailerProductPrice * ccfee, 4).Format(WebConfig.CurrentCulture);
        }

        public string FormatDecimal(decimal price)
        {
            return Utility.FormatPrice((double)price);
        }

        //public string GetRetailerInfoUrl() {
        //    Dictionary<string ,string> param = new Dictionary<string,string>();
        //    param.Add("retailerId", retailer.RetailerId.ToString());

        //    return UrlController.GetRewriterUrl(PageName.RetailerInfo, param);
        //}

        //protected string GetRetailerProductUrl(int rpid, string rpname) {
        //    return UrlController.GetRetailerProductUrl(rpid, rpname);
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