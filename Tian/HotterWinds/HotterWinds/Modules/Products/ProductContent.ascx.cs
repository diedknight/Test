using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PriceMeDBA;
using PriceMeCache;
using PriceMeCommon.BusinessLogic;
using PriceMe;
using PriceMeCommon.Data;
using PriceMeCommon;
using HotterWinds.DBQuery;
using Dapper;

namespace HotterWinds.Modules.Products
{
    public partial class ProductContent : System.Web.UI.UserControl
    {
        protected int retailerCount = 0;
        public int spaceID;

        public CSK_Store_ProductNew product = null;
        public CategoryCache category = null;
        public ManufacturerInfo manufacturer = null;
        protected ProductRatingEntity prEntity = null;

        protected int buyingGuidesCount;
        protected bool hasRetailerProducts = true;
        protected bool featuredProduct = false;

        protected string stringRelated = string.Empty;

        public string peUrl;
        public string purUrl;
        public string pdesUrl;
        public string productUrl;
        protected string pmapUrl;

        public int RetailerId;
        public int RetailerProductId;

        protected bool isAllPrice = true;
        protected bool isEnergyStar = false;
        public Timer.DKTimer dkTimer;
        protected List<ProductVideo> pvList = null;
        public bool hasTreepodiaVideo = false;
        public bool hasVideo = false;

        protected int planCount = 0;
        protected int minPrice;
        public BreadCrumbInfo breadCrumbInfo = null;
        public bool hasGmap = false;
        public bool isDisplayDes = false;
        protected List<int> retailerIds = new List<int>();
        public bool isShowVideoTab = false;

        public int RootCategoryID;

        public int userReviewCount = 0;

        public List<RetailerProductItem> rpis;
        public List<RetailerProductItem> rpisInt;
        public List<CSK_Store_RetailerProductNew> rpsInt;
        public decimal bestPrice;
        public decimal maxPrice;
        public bool isInternational;
        public decimal overseasPices;
        public bool isShowhReview = false;
        public bool singlePrice = false;
        public int flag = 1;
        public int allprice = 0;
        public bool showFeatured;
        public List<CSK_Store_RetailerProductNew> rps;
        public string ProductDesc;
        public string upcoming = string.Empty;
        public bool isLogin = false;
        public bool isUpcoming = false;
        public bool hasppc = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            dkTimer = Session["DKTimer"] as Timer.DKTimer;
            if (dkTimer != null)
                dkTimer.Set("Begin Modules_Products_ProductContent.Page_Load()");

            prEntity = ProductController.GetAverageRating(product.ProductID, WebConfig.CountryId);
            userReviewCount = ReviewController.SearchUserReviewByProductID(product.ProductID, WebConfig.CountryId).Count();
            peUrl = PriceMe.UrlController.GetProductExperReviewUrl(product.ProductID, product.ProductName);
            purUrl = UrlController.GetProductUserReviewUrl(product.ProductID, product.ProductName);
            pdesUrl = UrlController.GetProductDescriptionUrl(product.ProductID, product.ProductName);

            pvList = ProductController.GetProductVideos(product.ProductID, WebConfig.CountryId);

            pmapUrl = UrlController.GetProductMapUrl(product.ProductID, product.ProductName);

            hasTreepodiaVideo = false;
            if (dkTimer != null)
                dkTimer.Set("Befor Bind RetailerList");

            BindRetailerList();

            if (dkTimer != null)
                dkTimer.Set("Befor Bind Product Description");
            BindProductDescription();

            if (dkTimer != null)
                dkTimer.Set("Befor Bind Other User control");
            SetImage();

            stringRelated = UrlController.GetCatalogUrl(product.CategoryID ?? 0);
            SetRelatedProduct();

            SetProductTopDisplay();

            hasGmap = ProductController.HaveProductMap(product.ProductID, rpis, WebConfig.CountryId);

            if (pvList != null && pvList.Count > 0)
                isShowVideoTab = true;

            ExpatSoftware.Helpers.ABTesting.FairlyCertain.Score("DescProductName");

            //this.BannerRight.RootCategoryID = RootCategoryID;
            //this.AdsHorisontalMiddle.RootCategoryID = RootCategoryID;
            //this.AdsHorisontalLower.RootCategoryID = RootCategoryID;


            //ProductDesc = ProductController.GetProductDescription(product.ProductID, WebConfig.CountryId);
            ProductDesc = this.GetDes(product.ProductID.ToString());

            var generationProduct = ProductController.GetGenerationProduct(product.ProductID, WebConfig.CountryId);
            if (generationProduct != null)
            {
                string aTagFormat = "<a href='{0}'>{1}</a>";
                string pLink = string.Format(aTagFormat, UrlController.GetProductUrl(generationProduct.ProductID, generationProduct.ProductName), generationProduct.ProductName);
                string generationProductInfo = "<p>" + string.Format(Resources.Resource.TextString_GenerationProductInfo, pLink, generationProduct.CreatedOn.Value.ToString("MMM yyyy", System.Globalization.CultureInfo.CreateSpecificCulture("en-US"))) + "</p>";

                if (string.IsNullOrEmpty(ProductDesc))
                {
                    ProductDesc = generationProductInfo;
                }
                else
                {
                    ProductDesc += "<br />" + generationProductInfo;
                }
            }
        }

        protected void setProductData(List<RetailerProductItem> rpis, int allprice)
        {
            retailerCount = rpis.Count();
            if (rpis.Count == 1)
            {
                RetailerId = rpis[0].RetailerId;
                RetailerProductId = rpis[0].RpList[0].RetailerProductId;
            }

            //if (ProductItemPriceCompareNomal1 != null)
            //{
            //    ProductItemPriceCompareNomal1.ProductID = product.ProductID;
            //    ProductItemPriceCompareNomal1.PricesCount = allprice;
            //    ProductItemPriceCompareNomal1.DataSource = rpis;
            //    ProductItemPriceCompareNomal1.CategoryId = category.CategoryID;
            //    ProductItemPriceCompareNomal1.isSinglePrice = singlePrice;
            //}
        }

        protected void setProductData(List<CSK_Store_RetailerProductNew> rps, int allprice)
        {
            retailerCount = rps.Select(rp => rp.RetailerId).Distinct().Count();
            if (retailerCount == 1)
            {
                RetailerId = rps[0].RetailerId;
                RetailerProductId = rps[0].RetailerProductId;
            }

            //if (ProductPriceCompareNomal1 != null)
            //{
            //    ProductPriceCompareNomal1.ProductID = product.ProductID;
            //    ProductPriceCompareNomal1.PricesCount = rps.Count;
            //    ProductPriceCompareNomal1.DataSource = rps;
            //    ProductPriceCompareNomal1.CategoryId = category.CategoryID;
            //    ProductPriceCompareNomal1.isSinglePrice = singlePrice;
            //}
        }

        protected void setProductDataInt(List<CSK_Store_RetailerProductNew> rpsInt)
        {
            //if (ProductPriceCompareNomalInt != null)
            //{
            //    ProductPriceCompareNomalInt.ProductID = product.ProductID;
            //    ProductPriceCompareNomalInt.DataSource = rpsInt;
            //    ProductPriceCompareNomalInt.CategoryId = category.CategoryID;
            //    ProductPriceCompareNomalInt.isSinglePrice = singlePrice;
            //}
        }

        protected void setProductDataInt(List<RetailerProductItem> rpisInt)
        {
            //if (ProductItemPriceCompareNomalInt != null)
            //{
            //    ProductItemPriceCompareNomalInt.ProductID = product.ProductID;
            //    ProductItemPriceCompareNomalInt.DataSource = rpisInt;
            //    ProductItemPriceCompareNomalInt.CategoryId = category.CategoryID;
            //    ProductItemPriceCompareNomalInt.isSinglePrice = singlePrice;
            //}
        }

        private void SetProductTopDisplay() 
        {
            if (ProductTopDisplay1 != null)
            {
                int inStockStatus = -1;
                if (allprice == 1)
                {
                    inStockStatus = GetInStockStatus(rps[0]);
                }

                ProductTopDisplay1.Product = product;
                ProductTopDisplay1.ReviewCount = prEntity.ReviewCount;
                ProductTopDisplay1.ExpertReviewCount = prEntity.ExpertReviewCount;
                ProductTopDisplay1.PRatingValue = prEntity.ProductRating;
                ProductTopDisplay1.BestPrice = bestPrice;
                ProductTopDisplay1.MaxPrice = maxPrice;
                ProductTopDisplay1.RetailerCount = retailerCount;
                ProductTopDisplay1.IsSinglePrice = singlePrice;
                ProductTopDisplay1.title = product.ProductName;
                ProductTopDisplay1.RetailerId = RetailerId;
                ProductTopDisplay1.RetailerProductId = RetailerProductId;
                ProductTopDisplay1.manufacturer = manufacturer;
                ProductTopDisplay1.categoryName = category.CategoryName;
                ProductTopDisplay1.fromPage = "product";
                ProductTopDisplay1.InStockStatus = inStockStatus;
            }
        }

        private void SetImage()
        {
            if (ImagesDisplay1 != null)
            {
                ImagesDisplay1.Product = product;
                if (pvList != null)
                {
                    ImagesDisplay1.VideoCount = pvList.Count;
                }
            }
        }

        protected void SetRelatedProduct()
        {
            if (SimilarLinks1 != null)
            {
                List<PriceMeCommon.Data.ProductCatalog> pcc = ProductController.GetRelatedProductsByCategoryAndBrand(category.CategoryID, manufacturer.ManufacturerID, WebConfig.CountryId);

                if (pcc.Count > 0)
                {
                    SimilarLinks1.ProductCatalogs = pcc;
                    SimilarLinks1.ProductName = this.product.ProductName;
                    SimilarLinks1.ProductID = this.product.ProductID;
                    SimilarLinks1.CategoryName = category.CategoryName;

                    stringRelated = "#relatedProduct";
                }
            }
        }

        void BindRetailerList()
        {
            rpsInt = new List<CSK_Store_RetailerProductNew>();

            if (rpis.Count == 0)
            {
                hasRetailerProducts = false;

                upcoming = ProductController.CheckUpcomingProduct(product.ProductID, WebConfig.CountryId);
                if (HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    isLogin = true;
                    string UserEmail = string.Empty;
                    UserData user_data = Utility.GetUserInfoFromCookie();
                    if (user_data != null)
                        UserEmail = user_data.email;
                    isUpcoming = ProductController.UpcomingProductSelect(product.ProductID, UserEmail, WebConfig.CountryId);
                }

                return;
            }

            int priceCount = 0;//统计多少行价格，不统计隐藏行
            int position = 0;//当前价格排名

            if (allprice < 5)
            {
                priceCount = allprice;
                if (isInternational)
                {
                    List<CSK_Store_RetailerProductNew> priceRps = rps.Where(r => !RetailerController.IsInternationalRetailer(r.RetailerId, WebConfig.CountryId)).ToList();
                    if (priceRps.Count > 0)
                    {
                        singlePrice = priceRps.Count == 1;

                        bestPrice = priceRps.First().RetailerPrice;
                        if (!singlePrice && priceRps[1].RetailerPrice < bestPrice)
                            bestPrice = priceRps[1].RetailerPrice;

                        maxPrice = priceRps.Last().RetailerPrice;
                        if (!singlePrice && priceRps[0].RetailerPrice > maxPrice)
                            maxPrice = priceRps[0].RetailerPrice;
                    }
                    rpsInt = rps.Where(r => RetailerController.IsInternationalRetailer(r.RetailerId, WebConfig.CountryId)).ToList();
                    rps = priceRps;

                    retailerIds = new List<int>();
                    foreach (CSK_Store_RetailerProductNew rp in rps)
                    {
                        if (!retailerIds.Contains(rp.RetailerId))
                            retailerIds.Add(rp.RetailerId);
                    }
                }

                foreach (CSK_Store_RetailerProductNew rp in rps)
                {
                    position++;
                    rp.PricePositionCount = priceCount;
                    rp.PricePosition = position;
                    rp.Loc = 0;
                }

                setProductData(rps, allprice);
                if (rpsInt.Count > 0)
                {
                    foreach (CSK_Store_RetailerProductNew rp in rpsInt)
                    {
                        position++;
                        rp.PricePositionCount = priceCount;
                        rp.PricePosition = position;
                        rp.Loc = 1;
                    }
                    setProductDataInt(rpsInt);
                }

                isAllPrice = false;
            }
            else
            {
                priceCount = rpis.Count;

                foreach (RetailerProductItem rpi in rpis)
                {
                    position++;
                    foreach (CSK_Store_RetailerProductNew rp in rpi.RpList)
                    {
                        rp.PricePositionCount = priceCount;
                        rp.PricePosition = position;
                        rp.Loc = 0;
                    }
                }

                setProductData(rpis, allprice);
                if (rpisInt.Count > 0)
                {
                    foreach (RetailerProductItem rpi in rpisInt)
                    {
                        position++;
                        foreach (CSK_Store_RetailerProductNew rp in rpi.RpList)
                        {
                            rp.PricePositionCount = priceCount;
                            rp.PricePosition = position;
                            rp.Loc = 1;
                        }
                    }
                    setProductDataInt(rpisInt);
                }
            }
        }

        private int GetInStockStatus(CSK_Store_RetailerProductNew retailerProductNew)
        {
            int inStockStatus = -1;
            string stockStatus = retailerProductNew.StockStatus;
            string stock = retailerProductNew.Stock;

            if (!string.IsNullOrEmpty(stockStatus))
            {
                int st = 0;
                int.TryParse(stock, out st);

                if (stockStatus == "-3")
                {
                    inStockStatus = -1;
                }
                else if (stockStatus == "-2" || st > 0)
                {
                    inStockStatus = 1;
                }
                else if (stockStatus == "0")
                {
                    inStockStatus = 0;
                }
                else if (stockStatus == "-1")
                    inStockStatus = 1;
            }

            return inStockStatus;
        }

        private void BindProductDescription()
        {
            if (ProductDescription1 != null)
            {
                List<AttributeGroup> pdas;
                List<AttributeGroup> pdasTemp = new List<AttributeGroup>();

                List<AttributeGroup> groupDic = AttributesController.GetAttributeGroupByCategoryId(product.CategoryID ?? 0, WebConfig.CountryId);
                if (groupDic == null)
                {
                    pdas = new List<AttributeGroup>();
                }
                else
                {
                    pdas = groupDic;
                }
                GetDescAndAtt(pdas, pdasTemp);

                string ManufacturerProductUrl = ManufacturerController.GetManufacturerProductURL(product.ProductID, WebConfig.CountryId);
                if (!string.IsNullOrEmpty(ManufacturerProductUrl))
                {
                    isDisplayDes = true;
                }

                ProductDescription1.product = product;
                ProductDescription1.pdas = pdasTemp;
                ProductDescription1.categoryName = category.CategoryName;
                ProductDescription1.isDisplay = isDisplayDes;
                ProductDescription1.ManufacturerId = manufacturer.ManufacturerID;
                ProductDescription1.ManufacturerName = manufacturer.ManufacturerName;
                ProductDescription1.ManufacturerProductUrl = ManufacturerProductUrl;

            }

            //if (dkTimer != null)
            //    dkTimer.Set("Befor Bind Product UserReviews");
            //if (ProductUserReviews1 != null)
            //{
            //    ProductUserReviews1.product = product;
            //    ProductUserReviews1.productUrl = UrlController.GetProductUrl(product.ProductID, product.ProductName);
            //    ProductUserReviews1.ExpertReviewCount = prEntity.ExpertReviewCount;
            //    ProductUserReviews1.ExpertReviewUrl = peUrl;
            //    ProductUserReviews1.FeatureScore = prEntity.FeatureScore;
            //}
        }

        protected void GetDescAndAtt(List<AttributeGroup> pdas, List<AttributeGroup> pdasTemp)
        {
            List<string> uniqueList = new List<string>();

            List<ProductDescAndAttr> haspdas = ProductController.GetDescriptionAndAttribute(product.ProductID);

            foreach (AttributeGroup pair in pdas)
            {
                bool isAtt = false;

                foreach (AttributeGroupList hpd in pair.AttributeGroupList)
                {
                    string toLowerName = hpd.AttributeName.ToLower();
                    if (uniqueList.Contains(toLowerName))
                        continue;
                    else
                        uniqueList.Add(toLowerName);

                    hpd.Value = null;
                    hpd.Unit = null;
                    hpd.Avs = 0;

                    foreach (ProductDescAndAttr pd in haspdas)
                    {
                        if (toLowerName.TrimEnd(' ') == pd.Title.ToLower())
                        {
                            isAtt = true;
                            isDisplayDes = true;
                            hpd.Value = pd.Value;
                            hpd.Unit = pd.Unit;
                            hpd.Avs = pd.AVS;

                            break;
                        }
                    }
                }
                if (isAtt)
                    pdasTemp.Add(pair);
            }
        }

        /// <summary>
        /// 判断是否有phone plans 及数量
        /// </summary>
        private void GetPlans()
        {
            //11 == Mobile Phones
            if (WebConfig.CountryId == 3 && product.CategoryID == 11)
            {
                if (MobilePlanController.PhoneAndPlanMaps.ContainsKey(product.ProductID))
                    planCount = MobilePlanController.PhoneAndPlanMaps[product.ProductID];
                if (MobilePlanController.PhoneAndMinPlanMaps.ContainsKey(product.ProductID))
                    minPrice = MobilePlanController.PhoneAndMinPlanMaps[product.ProductID];
                if (planCount == 0)
                    minPrice = MobilePlanController.PhoneAndMinPlanMaps.Values.Min();
            }
        }

        public string ParentCategoryName = string.Empty;
        private void SetForumInfo()
        {
            if (WebConfig.CountryId == 3)
            {
                ParentCategoryName = CategoryController.GetRootCategory(category.CategoryID, WebConfig.CountryId).CategoryName;
            }
        }

        private string GetDes(string pid)
        {
            string sql = "select top 1 ShortDescriptionZH from CSK_Store_ProductNew where ProductID=" + pid;

            string des = HotterWindsQuery.GetConnection().ExecuteScalar<string>(sql);

            return des;
        }

    }
}