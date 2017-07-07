using PriceMe;
using PriceMeCache;
using PriceMeCommon.BusinessLogic;
using PriceMeCommon.Data;
using PriceMeDBA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

 
namespace HotterWinds
{
    public partial class Product : Page
    {
        protected int productID;
        protected CSK_Store_ProductNew product = null;
        protected CategoryCache category = null;
        protected ManufacturerInfo manufacturer = null;
        protected int flag = 1;
        public string productUrl;
        public string ImageUrl;
        protected bool hasTreepodiaVideo = false;

        public Timer.DKTimer dkTimer = null;

        protected List<RetailerProductItem> rpisInt;
        protected List<CSK_Store_RetailerProductNew> rpsInt;
        protected int allprice = 0;
        protected List<CSK_Store_RetailerProductNew> rps;
        protected decimal bestPrice;
        protected decimal maxPrice;
        protected bool singlePrice;
        protected bool showFeatured;
        protected bool isInternational;
        protected decimal overseasPices;
        protected bool featuredProduct = false;
        protected bool isShowhReview = false;
        protected List<int> retailerIds = new List<int>();
        protected List<RetailerProductItem> rpis;

        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (Utility.IsAdmin())
            {
                dkTimer = new Timer.DKTimer();
                Session["DKTimer"] = dkTimer;
                dkTimer.Start();
                dkTimer.Set("Begin Product.Page_Load()");
            }

            flag = Utility.GetIntParameter("flag");
            flag = flag == 0 || flag == 1 ? 1 : -1;

            productID = Utility.GetIntParameter("pid");
            //URL
            string productName = Utility.GetParameter("name");
            int aid = Utility.GetIntParameter("aid");

            if (productID == 0)
            {
                Utility.PageNotFound();
                return;
            }

            product = ProductController.GetRealProductSimplified(productID, WebConfig.CountryId);
            if (product == null)
            {
                Utility.PageNotFound();
                return;
            }
            else if (product.ProductID != productID)
            {
                string realUrl = UrlController.GetProductUrl(product.ProductID, product.ProductName);
                if (aid != 0)
                    realUrl += "?aid=" + aid;

                ResponseRedirect(realUrl);
                return;
            }

            if (CategoryController.IsSearchOnly(product.CategoryID.Value, WebConfig.CountryId))
                ResponseRedirect404();

            //Redirect Mobile Plans
            if (product.CategoryID == 1283)
            {
                string pageValue = string.Empty;
                if (product.ManufacturerID == 120342)
                    pageValue = "/2-degrees";
                else if (product.ManufacturerID == 116223)
                    pageValue = "/vodafone";
                else if (product.ManufacturerID == 118470)
                    pageValue = "/telecom";

                ResponseRedirect("https://www.priceme.co.nz/plans/mobile-plans" + pageValue);
                return;
            }

            bool isLink = false;
            productUrl = UrlController.GetProductUrl(product.ProductID, product.ProductName);

            string rawurl = Request.RawUrl;
            if (rawurl.Contains("?gclid="))
                rawurl = rawurl.Split(new string[] { "?gclid=" }, StringSplitOptions.None)[0];

            if (!WebConfig.UrlSeo)
            {
                if (!string.IsNullOrEmpty(productName))
                    isLink = true;
            }
            else if (productUrl != rawurl)
                isLink = true;

            if (isLink)
            {
                if (aid != 0)
                    productUrl += "?aid=" + aid;

                HttpContext.Current.Response.Status = "301 Moved Permanently";
                HttpContext.Current.Response.AddHeader("Location", productUrl);
                HttpContext.Current.Response.End();
                return;
            }

            if (product.CategoryID != null && product.CategoryID != 0)
                category = CategoryController.GetCategoryByCategoryID(product.CategoryID.Value, WebConfig.CountryId);
            else
                category = CategoryController.GetCategoryByProductID(product.ProductID, WebConfig.CountryId);

            int rootCategoryID = 0;
            if (category != null)
            {
                rootCategoryID = CategoryController.GetRootCategory(category.CategoryID, WebConfig.CountryId).CategoryID;
            }

            manufacturer = ManufacturerController.GetManufacturerByID(product.ManufacturerID ?? 0, WebConfig.CountryId);

            BreadCrumbInfo breadCrumbInfo = null;
            if (category == null)
                breadCrumbInfo = Utility.GetBreadCrumbInfo(product.ProductName);
            else
                breadCrumbInfo = Utility.GetCatalogBreadCrumbInfo(category, manufacturer, true, "");

            breadCrumbInfo.linkInfoList[0].LinkText = Resources.Resource.Global_HomePageName;
            if (manufacturer != null && manufacturer.ManufacturerID != -1 && category.IsFilterByBrand)
            {
                if (WebConfig.CountryId == 56)
                    breadCrumbInfo.linkInfoList[breadCrumbInfo.linkInfoList.Count - 1].LinkText = category.CategoryName + " " + manufacturer.ManufacturerName;
                else
                    breadCrumbInfo.linkInfoList[breadCrumbInfo.linkInfoList.Count - 1].LinkText = manufacturer.ManufacturerName + " " + category.CategoryName;
            }
            breadCrumbInfo.CurrentPageId = product.ProductID.ToString();
            breadCrumbInfo.CurrentPageKey = "product";

            List<CategoryCache> ccList = CategoryController.GetBreadCrumbCategoryList(category, WebConfig.CountryId);
            int rootCID = ccList[ccList.Count - 1].CategoryID;
            int ccid = rootCategoryID;
            if (ccList.Count > 1)
            {
                ccid = ccList[ccList.Count - 2].CategoryID;
            }

            List<RetailerProductItem> rpisInt = new List<RetailerProductItem>();
            List<CSK_Store_RetailerProductNew> rpsInt = new List<CSK_Store_RetailerProductNew>();

            ProductController.GetRetailerProductItems(product.ProductID, out bestPrice, out maxPrice, out singlePrice, out featuredProduct, out rpis, out allprice, flag, out isInternational, out overseasPices, rpisInt, out retailerIds, out rps, WebConfig.CountryId);
            bool hasppc = rps.Where(p => RetailerController.GetAllPPcRetaielrIds(WebConfig.CountryId).Contains(p.RetailerId)).Count() > 0 ? true : false;
            ///
            /// prodp=1 -> $0-$100
            /// prodp=2 -> $101 - $250
            /// prodp=3 -> $251 - $500
            /// prodp=4 -> Above $500
            /// 
            //非NZD转换成NZD
            decimal lowerPrice = bestPrice;
            if (WebConfig.CountryId != 3)
            {
                var uc = RetailerController.GetUtilCountry(WebConfig.CountryId);
                if (uc != null)
                {
                    bestPrice = bestPrice / (decimal)(uc.PriceMeExchangeRate);
                }
            }
            int prodp = 0;
            if (bestPrice > 0 && bestPrice <= 100)
            {
                prodp = 1;
            }
            else if (bestPrice > 100 && bestPrice <= 250)
            {
                prodp = 2;
            }
            else if (bestPrice > 250 && bestPrice <= 500)
            {
                prodp = 3;
            }
            else if (bestPrice > 500)
            {
                prodp = 4;
            }
            Master.LoadDFPAds(rootCID, ccid, prodp);

            hasTreepodiaVideo = false;
            if (ProductContent != null)
            {
                ProductContent.product = product;
                ProductContent.category = category;
                ProductContent.manufacturer = manufacturer;
                ProductContent.productUrl = productUrl;
                ProductContent.RootCategoryID = rootCategoryID;


                ProductContent.rpis = rpis;
                ProductContent.rps = rps;
                ProductContent.flag = flag;
                ProductContent.allprice = allprice;
                ProductContent.bestPrice = lowerPrice;
                ProductContent.maxPrice = maxPrice;
                ProductContent.singlePrice = singlePrice;
                ProductContent.showFeatured = showFeatured;
                ProductContent.isInternational = isInternational;
                ProductContent.overseasPices = overseasPices;
                ProductContent.rpisInt = rpisInt;
                ProductContent.hasppc = hasppc;
            }
            Master.SetBreadCrumb(breadCrumbInfo, product.ProductID);

            Master.InitATag(string.Format(Resources.Resource.TextString_Checkout, product.ProductName));

            SetRecentlyViewSession();
            BindProductInfo(lowerPrice);
            LoadCrumbs();

            breadCrumbInfo.ProductBestPrice = lowerPrice;
            Master.SetBreadCrumb(breadCrumbInfo, category.CategoryID);
            Breadcrumbs1.CategoryId = category.CategoryID;
            Breadcrumbs1.ProductName = product.ProductName;

            Master.AddCanonical(productUrl);

            string alternate = "android-app://PriceMe.Android/priceme-app/app/product/pid=" + productID;
            Master.AddAlternate(alternate);

            Master.pageID = (int)CssFile.Product;
            Master.AddProductJs();
            Master.SetCategory(product.CategoryID.Value);
            if (manufacturer != null)
            {
                Master.SetBrandID(manufacturer.ManufacturerID);
            }

            Master.SetIosAppArgument("priceme31415926://com.priceme/p-" + product.ProductID);
        }

        void BindProductInfo(decimal lowerPrice)
        {
            string sTitle = " " + System.Configuration.ConfigurationManager.AppSettings["ProductPageTilteTail"];
            string shortCountry = System.Configuration.ConfigurationManager.AppSettings["shortCountry"];

            this.Title = product.ProductName.Replace("<br />", " ") + " " + shortCountry + sTitle;//set page title.

            //for facebook
            string fbUrl = Resources.Resource.Global_HomePageUrl + Request.RawUrl;
            string fbTitle = product.ProductName;
            string noImageAvailable = Resources.Resource.ImageWebsite + "/images/no_image_available.gif";

            ImageUrl = Utility.GetImage(product.DefaultImage, "_m");

            string fbDes = "Compare prices on " + product.ProductName + ". Read reviews, find deals and shop smarter online on PriceMe.";

            DynamicHtmlHeader.SetFaceBookHeader(fbTitle, "product", ImageUrl, fbUrl, fbDes, lowerPrice.ToString("0.00"), Resources.Resource.PriceCurrency, this);
            if (hasTreepodiaVideo)
                DynamicHtmlHeader.SetFaceBookHeaderForVideo(product.ProductID.ToString(), ImageUrl, this);
        }

        void LoadCrumbs()
        {
            string descStr = "";
            string keyWord = "";
            string manufacturerName = "";

            if (manufacturer.ManufacturerID != -1)
            {
                manufacturerName = manufacturer.ManufacturerName;
            }

            descStr = string.Format(Resources.Resource.TextString_ProductDesc, product.ProductName, manufacturerName, category.CategoryName);
            keyWord = string.Format(Resources.Resource.TextString_ProductKeyword, product.ProductName);
            DynamicHtmlHeader.SetHtmlHeader(keyWord, descStr, this.Page);
        }

        private void SetRecentlyViewSession()
        {
            string myrecentlyviewPidCollection = "";
            if (HttpContext.Current.Request.Cookies["myrecentlyviewPidCollection"] != null && HttpContext.Current.Request.Cookies["myrecentlyviewPidCollection"].Value != null)
                myrecentlyviewPidCollection = HttpContext.Current.Request.Cookies["myrecentlyviewPidCollection"].Value;

            bool isContain = false;
            string[] myrecentlyviewPidArray = myrecentlyviewPidCollection.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            List<string> pidList = new List<string>(myrecentlyviewPidArray);


            foreach (string pid in pidList)
            {
                if (pid == product.ProductID.ToString())
                {
                    isContain = true;
                    pidList.Remove(pid);

                    pidList.Insert(0, pid);
                    break;
                }
            }

            int maxCount = 8;
            if (!isContain)
            {
                pidList.Insert(0, product.ProductID.ToString());

                if (pidList.Count > maxCount)
                {
                    pidList.RemoveAt(maxCount);
                }
            }

            myrecentlyviewPidCollection = string.Join("|", pidList);

            Response.Cookies["myrecentlyviewPidCollection"].Value = myrecentlyviewPidCollection;
            Response.Cookies["myrecentlyviewPidCollection"].Expires = DateTime.Today.AddDays(180);

            //
            string myrecentlyviewCidCollection = "";
            if (HttpContext.Current.Request.Cookies["myrecentlyviewCidCollection"] != null && HttpContext.Current.Request.Cookies["myrecentlyviewCidCollection"].Value != null)
                myrecentlyviewCidCollection = HttpContext.Current.Request.Cookies["myrecentlyviewCidCollection"].Value;

            isContain = false;
            string[] myrecentlyviewCidArray = myrecentlyviewCidCollection.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            List<string> cidList = new List<string>(myrecentlyviewCidArray);


            foreach (string cid in cidList)
            {
                if (cid == product.CategoryID.ToString())
                {
                    isContain = true;
                    cidList.Remove(cid);

                    cidList.Insert(0, cid);
                    break;
                }
            }

            maxCount = 8;
            if (!isContain)
            {
                cidList.Insert(0, product.CategoryID.ToString());

                if (cidList.Count > maxCount)
                {
                    cidList.RemoveAt(maxCount);
                }
            }

            myrecentlyviewCidCollection = string.Join("|", cidList);

            Response.Cookies["myrecentlyviewCidCollection"].Value = myrecentlyviewCidCollection;
            Response.Cookies["myrecentlyviewCidCollection"].Expires = DateTime.Today.AddDays(180);
            Response.Cookies["myrecentlyviewCidCollection"].Domain = ".priceme.co.nz";
        }

        private void ResponseRedirect(string url)
        {
            HttpContext.Current.Response.Status = "301 Moved Permanently";
            HttpContext.Current.Response.AddHeader("Location", url);
            HttpContext.Current.Response.End();
        }

        private void ResponseRedirect404()
        {
            Session["SopProduct"] = product.ProductID;
            HttpContext.Current.Response.Status = "404 not found";
            HttpContext.Current.Response.SubStatusCode = 1;
            HttpContext.Current.Response.Redirect("/404_1.aspx", true);
            HttpContext.Current.Response.End();
        }
    }
}