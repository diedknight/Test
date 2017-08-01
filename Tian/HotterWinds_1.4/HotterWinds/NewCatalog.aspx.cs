using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PriceMe;
using PriceMeCache;
using PriceMeCommon;
using PriceMeCommon.BusinessLogic;
using PriceMeCommon.Data;

namespace HotterWinds
{    
    public partial class NewCatalog : System.Web.UI.Page
    {
        static readonly int Static_PageSize = 24;
        static readonly string Static_DefaultSortBy = "Clicks";

        bool needRedirect = false;
        bool needReSearchForNoParameters = false;

        int categoryID;
        int currentPage;
        protected string DefaultView;

        string retailerIDs;
        string manufeaturerIDs;
        string attributeValuesIDString;
        string attributeGroupIDString;
        string attributeValuesRangeString;//slider attr selected parameter
        string displayAllManufeaturerIDString;//quick view display all manufaeturer product
        string priceRangeString;
        protected string v;
        protected string swi;
        string adm;//Admin
        string daysRangeString;
        protected string sortBy;
        string onSaleOnly;

        PriceRange priceRange = null;
        List<int> brandIDList;
        List<int> selectedAttributeIDs;
        List<int> selectedAttributeRangeIDs;
        List<int> selectedAttributeGroupIDs;
        List<int> selectedRetailers;
        List<string> displayAllProductsManufeaturerIDs;
        Dictionary<int, string> selectedAttrRangeValues;//slider attr selected value split by AttributeTitleID
        Dictionary<int, string> sliderAttrRange = new Dictionary<int, string>();//slider attr 的值范围
        bool isAdmin = false;
        Dictionary<string, string> currentParameters;
        AttributeParameterCollection urlAttributeParameterList;
        Dictionary<string, string> removeUrlDic;
        DaysRange daysRange;
        ProductSearcher productSeacherWithoutFilters;

        protected CategoryCache category;
        protected int isFromProductPage;
        protected string filterStatusBySorting;
        protected Dictionary<int, string> filterStatus = new Dictionary<int, string>();
        protected int totalProductCount;
        protected List<LinkInfo> selections;
        protected List<LinkInfo> sortByInfoList;
        protected string GridViewUrl;
        protected string ListViewUrl;
        protected string QuickViewUrl;

        string priceTitleInfo = "";
        double priceRangeMaxValue;

        protected CatalogPageInfo catalogPageInfo;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (GetParameters())
            {
                SetValue();
                LoadCrumbs();
                SetPageInfo();

                if (!category.IsSiteMapDetail && !category.IsSiteMap)
                {
                    BuilderSelections();
                    SetControlsValue();

                    if (this.NewFilters1 != null)
                    {
                        SetProductFilters();
                    }
                    SetSortAndViewInfo(currentParameters);


                    if (v.ToLower().Equals("quick"))
                    {
                        this.PrettyPager1.IsDisplay = false;
                    }
                    else
                    {
                        this.PrettyPager1.PageTo = PageName.Catalog;
                        this.PrettyPager1.totalPages = catalogPageInfo.PageCount;
                        this.PrettyPager1.currentPage = this.currentPage;
                        this.PrettyPager1.ps = new Dictionary<string, string>(currentParameters);
                        this.PrettyPager1.SetPaginationHeader();
                        this.PrettyPager1.PageSize = Static_PageSize;
                        this.PrettyPager1.ItemCount = catalogPageInfo.CurrentProductCount;
                        this.PrettyPager1.DisplayItemCountInfo = true;
                    }
                }
                else
                {
                    if (category.IsSiteMap)
                    {
                        if (this.CatalogSiteMap1 != null)
                        {
                            bool hasDealsProduct = false;

                            if (WebConfig.CountryId == 3)
                            {
                                List<int> cidList = new List<int>();
                                cidList.Add(categoryID);
                                ProductSearcher productSearcher = new ProductSearcher("", cidList, null, null, null, null, "Sale", null, 10, WebConfig.CountryId, false, true, false, true, null, "", null);
                                hasDealsProduct = IsHasDealsProduct(productSearcher);
                            }
                            this.CatalogSiteMap1.catalogSitemapCategories = CategoryController.GetCatalogSitemapCategories(category.CategoryID, true, WebConfig.CountryId);
                            this.CatalogSiteMap1.catalogSitemapPopularCategories = CategoryController.GetCatalogSitemapPopularCategories(category.CategoryID, WebConfig.CountryId);
                            this.CatalogSiteMap1.Category = this.category;
                            this.CatalogSiteMap1.HasDealsProduct = hasDealsProduct;
                        }

                    }
                    else if (category.IsSiteMapDetail)
                    {
                        if (this.CatalogSiteMapDetail1 != null)
                        {
                            bool hasDealsProduct = false;

                            if (WebConfig.CountryId == 3)
                            {
                                List<int> cidList = new List<int>();
                                cidList.Add(categoryID);
                                ProductSearcher productSearcher = new ProductSearcher("", cidList, null, null, null, null, "Sale", null, 10, WebConfig.CountryId, false, true, false, true, null, "", null);
                                hasDealsProduct = IsHasDealsProduct(productSearcher);
                            }

                            this.CatalogSiteMapDetail1.Category = category;
                            this.CatalogSiteMapDetail1.HasDealsProduct = hasDealsProduct;
                        }
                    }
                }


                Breadcrumbs.CategoryId = category.CategoryID;
            }
        }

        private bool IsHasDealsProduct(ProductSearcher productSearcher)
        {
            var products = productSearcher.GetSearchResult(1, 10);
            foreach (var p in products.ProductCatalogList)
            {
                float bp = float.Parse(p.BestPrice);
                float sale = -p.Sale;

                if (bp >= PriceMe.WebConfig.MinimumPrice && sale >= PriceMe.WebConfig.SaleRate)
                    return true;
            }

            return false;
        }

        private bool GetParameters()
        {
            categoryID = Utility.GetIntParameter("c");//category ID
            if (categoryID == 0)
            {
                return false; 
            }

            if (WebConfig.CountryId == 3 && categoryID == 1283)
            {
                Context.Response.Status = "301 Moved Permanently";
                Context.Response.AddHeader("Location", "https://www.priceme.co.nz/plans/mobile-plans");
                Context.Response.End();
            }
            else if (WebConfig.CountryId == 3 && categoryID == 359)
            {
                Context.Response.Status = "301 Moved Permanently";
                Context.Response.AddHeader("Location", "https://www.priceme.co.nz/plans/all-broadband-plans?type=1");
                Context.Response.End();
            }
            else if (WebConfig.CountryId == 3 && categoryID == 360)
            {
                Context.Response.Status = "301 Moved Permanently";
                Context.Response.AddHeader("Location", "https://www.priceme.co.nz/plans/all-broadband-plans?type=5");
                Context.Response.End();
            }
            else if (WebConfig.CountryId == 3 && categoryID == 436)
            {
                Context.Response.Status = "301 Moved Permanently";
                Context.Response.AddHeader("Location", "https://www.priceme.co.nz/plans/all-broadband-plans?type=4");
                Context.Response.End();
            }
            else if (WebConfig.CountryId == 3 && categoryID == 361)
            {
                Context.Response.Status = "301 Moved Permanently";
                Context.Response.AddHeader("Location", "https://www.priceme.co.nz/plans/all-broadband-plans");
                Context.Response.End();
            }

            if (CategoryController.Category301Dictionary_Static.ContainsKey(categoryID))
            {
                int _301Cid = CategoryController.Category301Dictionary_Static[categoryID];
                string _301url = UrlController.GetCatalogUrl(_301Cid);

                Context.Response.Status = "301 Moved Permanently";
                Context.Response.AddHeader("Location", _301url);
                Context.Response.End();
            }

            manufeaturerIDs = Utility.GetParameter("m");//selected manufeaturers
            attributeValuesIDString = Utility.GetParameter("avs");//selected attribute values ID string
            attributeValuesRangeString = Utility.GetParameter("avsr");//selected attribute values range string
            swi = Utility.GetParameter("swi");//search with in
            attributeGroupIDString = Utility.GetParameter("avg");//attribute value groups
            priceRangeString = Utility.GetParameter("pr").Replace(",", "").Replace(".", "");
            currentPage = Utility.GetIntParameter("pg");//current page number
            sortBy = Utility.GetParameter("sb");//sort by value
            retailerIDs = Utility.GetParameter("pcsid");
            adm = Utility.GetParameter("adm");//是否是admin
            daysRangeString = Utility.GetParameter("dr");
            v = Utility.GetParameter("v");//view
            displayAllManufeaturerIDString = Utility.GetParameter("samp");
            onSaleOnly = Utility.GetParameter("os");//是否只要显示有OnSale标签的产品

            return true;
        }

        private string GetDefaultLayout(CategoryCache cc)
        {
            if (cc.CategoryViewType == 2)
            {
                return "List";
            }
            else if (cc.CategoryViewType == 3)
            {
                return "Quick";
            }
            else
            {
                return "Grid";
            }
        }

        private void SetValue()
        {
            category = CategoryController.GetCategoryByCategoryID(categoryID, WebConfig.CountryId);

            if (category == null || !category.IsActive)
            {
                Context.Response.Status = "404 not found";
                Context.Response.Redirect("/404.aspx", true);
                Context.Response.End();
            }

            if (CategoryController.IsSearchOnly(category.CategoryID, WebConfig.CountryId))
            {
                Session["SocCaterory"] = category.CategoryID;
                HttpContext.Current.Response.Status = "404 not found";
                HttpContext.Current.Response.SubStatusCode = 1;
                HttpContext.Current.Response.Redirect("/404_1.aspx", true);
                HttpContext.Current.Response.End();
            }

            DefaultView = GetDefaultLayout(category);
            if (v.Equals(DefaultView, StringComparison.InvariantCultureIgnoreCase))
            {
                v = "";
            }

            if (sortBy.Equals(Static_DefaultSortBy, StringComparison.InvariantCultureIgnoreCase))
            {
                sortBy = "";
            }

            double minPrice, maxPrice;
            if (!string.IsNullOrEmpty(priceRangeString))
            {
                string[] prices = priceRangeString.Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
                if (prices.Length == 2)
                {
                    double.TryParse(prices[0], out minPrice);
                    double.TryParse(prices[1], out maxPrice);

                    if (minPrice > maxPrice)
                    {
                        var pp = minPrice;
                        minPrice = maxPrice;
                        maxPrice = pp;
                        priceRangeString = minPrice + "-" + maxPrice;
                    }

                    if (minPrice >= 0 || maxPrice >= 0)
                    {
                        priceRange = new PriceRange(minPrice, maxPrice);
                    }

                    needReSearchForNoParameters = true;
                }
                else
                {
                    needRedirect = true;
                    priceRangeString = "";
                }
            }

            if (!string.IsNullOrEmpty(daysRangeString))
            {
                daysRange = DaysRange.Create(daysRangeString);
                needReSearchForNoParameters = true;
            }

            brandIDList = new List<int>();
            string[] mIDs = manufeaturerIDs.Split(new string[] { "-" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string mID in mIDs)
            {
                int m = 0;
                int.TryParse(mID, out m);
                if (m > 0 && !brandIDList.Contains(m))
                {
                    brandIDList.Add(m);
                }
            }
            if (brandIDList.Count > 0)
            {
                needReSearchForNoParameters = true;
            }

            selectedAttributeGroupIDs = new List<int>();
            if (!string.IsNullOrEmpty(attributeGroupIDString))
            {
                string[] avgIDs = attributeGroupIDString.Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);

                foreach (string avgIDStr in avgIDs)
                {
                    int avgID = int.Parse(avgIDStr);
                    if (!selectedAttributeGroupIDs.Contains(avgID))
                    {
                        selectedAttributeGroupIDs.Add(avgID);
                    }
                }
            }

            selectedAttributeIDs = new List<int>();//selected attribute values ID
            selectedAttributeRangeIDs = new List<int>();//selected attribute range ID
            if (attributeValuesIDString.Length > 0)
            {
                string[] avids = attributeValuesIDString.Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
                int aid;

                foreach (string avid in avids)
                {
                    string avidString = avid;
                    if (avidString.ToLower().EndsWith("r"))
                    {
                        avidString = avid.Substring(0, avidString.Length - 1);
                        if (int.TryParse(avidString, out aid))
                        {
                            if (!selectedAttributeRangeIDs.Contains(aid))
                            {
                                selectedAttributeRangeIDs.Add(aid);
                            }
                        }
                    }
                    else
                    {
                        if (int.TryParse(avidString, out aid))
                        {
                            //如果选择了AttributeGroup，则全选Group对应的所有AttributeValue
                            int avgID = PriceMeCommon.BusinessLogic.AttributesController.GetCatalogAttributeGroupID(aid);
                            if (!selectedAttributeGroupIDs.Contains(avgID))
                            {
                                if (!selectedAttributeIDs.Contains(aid))
                                {
                                    selectedAttributeIDs.Add(aid);
                                }
                            }
                        }
                    }
                }
            }
            if (selectedAttributeIDs.Count > 0)
            {
                needReSearchForNoParameters = true;
            }

            selectedAttrRangeValues = new Dictionary<int, string>();//slider attr selected value split by AttributeTitleID
            if (attributeValuesRangeString.Length > 0)
            {
                string[] avsrParams = attributeValuesRangeString.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string avsrP in avsrParams)
                {
                    string[] avsrs = avsrP.Split(new char[] { '_' }, StringSplitOptions.RemoveEmptyEntries);
                    int aid = 0;
                    for (int i = 0; i < avsrs.Length; i++)
                    {
                        if (string.IsNullOrEmpty(avsrs[i])) continue;
                        if (avsrs[i].Contains("-"))
                        {
                            var aa = avsrs[i].Split('-');
                            float minValue, maxValue;
                            float.TryParse(aa[0], out minValue);
                            float.TryParse(aa[1], out maxValue);
                            if (minValue > maxValue)
                                selectedAttrRangeValues.Add(aid, maxValue + "-" + minValue);
                            else
                                selectedAttrRangeValues.Add(aid, avsrs[i]);
                        }
                        else
                        {
                            aid = int.Parse(avsrs[i]);
                        }
                    }
                }
            }
            if (selectedAttrRangeValues.Count > 0)
            {
                needReSearchForNoParameters = true;
            }

            displayAllProductsManufeaturerIDs = new List<string>();
            if (!string.IsNullOrEmpty(displayAllManufeaturerIDString))
            {
                string[] mids = displayAllManufeaturerIDString.Split(new char[] { '_' }, StringSplitOptions.RemoveEmptyEntries);
                if (mids.Length == 1 && mids[0].Contains("-"))
                {
                    mids[0] = "-1";
                    displayAllManufeaturerIDString = "-1";
                }
                foreach (string mid in mids)
                {
                    if (!displayAllProductsManufeaturerIDs.Contains(mid))
                    {
                        displayAllProductsManufeaturerIDs.Add(mid);
                    }
                }
                displayAllProductsManufeaturerIDs.Sort();
            }

            selectedRetailers = new List<int>();
            string[] rIDs = retailerIDs.Split(new string[] { "-" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string rID in rIDs)
            {
                int r = 0;
                int.TryParse(rID, out r);
                if (r > 0 && !selectedRetailers.Contains(r))
                {
                    selectedRetailers.Add(r);
                }
            }
            if (selectedRetailers.Count > 0)
            {
                needReSearchForNoParameters = true;
            }

            currentParameters = UrlController.GetCatalogParameters(this.categoryID, this.brandIDList, priceRangeString,
                selectedAttributeIDs, selectedAttributeRangeIDs, this.sortBy, selectedRetailers, v, swi,
                this.displayAllManufeaturerIDString, selectedAttrRangeValues, selectedAttributeGroupIDs, daysRangeString, onSaleOnly);
            if (currentParameters.ContainsKey("m") && !category.IsFilterByBrand)
            {
                currentParameters.Remove("m");
            }
            if (currentParameters["c"] == "2118")
            {
                currentParameters["c"] = "147";
            }
            else if (currentParameters["c"] == "715")
            {
                currentParameters["c"] = "2012";
            }
            else if (currentParameters["c"] == "1356")
            {
                currentParameters["c"] = "500";
            }

            if (category.IsSiteMap || category.IsSiteMapDetail)
            {
                needRedirect = false;
            }

            if (!string.IsNullOrEmpty(adm))
            {
                isAdmin = true;
            }

            Dictionary<string, string> newPS = new Dictionary<string, string>(currentParameters);
            if (currentPage > 1)
            {
                newPS.Add("pg", currentPage.ToString());
            }
            string checkURL = UrlController.GetRewriterUrl(PageName.Catalog, newPS);
            if (!checkURL.Equals(Request.RawUrl.Replace("?fp=ps", "").Replace("&fp=ps", "").Replace("&gclid=" + PriceMe.WebConfig.Gclid, "").Replace("?gclid=" + PriceMe.WebConfig.Gclid, ""), StringComparison.InvariantCultureIgnoreCase))
            {
                needRedirect = true;
            }

            if (needRedirect && !isAdmin)
            {
                Context.Response.Status = "301 Moved Permanently";
                Context.Response.AddHeader("Location", checkURL);
                Context.Response.End();
            }
            else
            {
                if (newPS.ContainsKey("v"))
                {
                    newPS.Remove("v");
                    checkURL = UrlController.GetRewriterUrl(PageName.Catalog, newPS);
                }
            }

            if (priceRange != null)
            {
                priceTitleInfo = " priced " + priceRange.ToPriceString(PriceMe.Utility.CurrentCulture, Resources.Resource.TextString_PriceSymbol).Replace("-", " to ");
            }

            urlAttributeParameterList = UrlController.GetAttributeParameterCollectionAndSortAttributesParamters(currentParameters, category);

            List<int> avIDs = new List<int>(selectedAttributeIDs);
            foreach (int avgID in selectedAttributeGroupIDs)
            {
                List<int> avIDList = PriceMeCommon.BusinessLogic.AttributesController.GetCatalogAttributeValues(avgID);
                if (avIDList != null)
                {
                    avIDs.AddRange(avIDList);
                }
            }
            if (avIDs.Count > 0)
            {
                needReSearchForNoParameters = true;
            }

            if (string.IsNullOrEmpty(sortBy))
            {
                sortBy = Static_DefaultSortBy;
            }

            if (!category.IsSiteMapDetail && !category.IsSiteMap)
            {
                catalogPageInfo = CatalogProductSearchController.SearchProducts(this.categoryID, this.brandIDList, priceRange, avIDs, selectedAttributeRangeIDs, this.sortBy, swi, selectedRetailers, true, WebConfig.CountryId, false, selectedAttrRangeValues, true, currentPage, Static_PageSize, daysRange, onSaleOnly);
                Utility.FixProductCatalogList(catalogPageInfo.ProductCatalogList, v);

                //不含过滤条件的ProductSearcher
                if (needReSearchForNoParameters)
                {
                    productSeacherWithoutFilters = new ProductSearcher("", this.categoryID, null, null, null, null, "clicks", null, SearchController.MaxSearchCount_Static, WebConfig.CountryId, false, true, false, true, null, "", null);
                }
                else
                {
                    productSeacherWithoutFilters = catalogPageInfo.MyProductSearcher;
                }
                totalProductCount = productSeacherWithoutFilters.GetProductCount();
            }
        }

        void SetControlsValue()
        {
            string alternate = "android-app://PriceMe.Android/priceme-app/app/catalog/c=" + categoryID;
            Master.AddAlternate(alternate);

            //if (this.CategoryTopControl1 != null)
            //{
            //    this.CategoryTopControl1.category = this.category;
            //    this.CategoryTopControl1.attributeParameterList = this.urlAttributeParameterList;
            //    this.CategoryTopControl1.priceTitleInfo = priceTitleInfo;
            //}

            List<CategoryCache> ccList = CategoryController.GetBreadCrumbCategoryList(category, WebConfig.CountryId);
            int rootCategoryID = ccList[ccList.Count - 1].CategoryID;
            int ccid = rootCategoryID;
            if (ccList.Count > 1)
            {
                ccid = ccList[ccList.Count - 2].CategoryID;
            }
            Master.LoadDFPAds(rootCategoryID, ccid, 0);
            Master.SetCategory(categoryID);
            if (brandIDList != null)
            {
                Master.SetBrandIDs(brandIDList);
            }

            if (NewCatalogProducts1 != null)
            {
                NewCatalogProducts1.RootCategoryID = rootCategoryID;
                NewCatalogProducts1.ShowCompareBox = true;
                if (string.IsNullOrEmpty(v))
                {
                    NewCatalogProducts1.View = DefaultView;
                }
                else
                {
                    NewCatalogProducts1.View = v;
                }
                NewCatalogProducts1.IsFilterByBrand = category.IsFilterByBrand;
                if (!v.Equals("quick", StringComparison.InvariantCultureIgnoreCase))
                {
                    List<string> popularTop3 = productSeacherWithoutFilters.GetTop3Products();
                    SetTop3(catalogPageInfo.ProductCatalogList, popularTop3);
                    NewCatalogProducts1.ProductCatalogList = catalogPageInfo.ProductCatalogList;
                }
                else
                {
                    //if (isAdmin)
                    //{
                    //    NewCatalogProducts1.IsAdmin = true;
                    //    NewCatalogProducts1.AttributesInfo = catalogPageInfo.MyProductSearcher.GetAttributesResulte_New(null);
                    //}
                    List<CatalogManufeaturerProduct> temps = new List<CatalogManufeaturerProduct>();
                    List<CatalogManufeaturerProduct> cmps = catalogPageInfo.MyProductSearcher.GetManufeaturerProductList(displayAllProductsManufeaturerIDs, WebConfig.QuickListCount);
                    foreach (CatalogManufeaturerProduct cmp in cmps)
                    {
                        List<PriceMeCommon.Data.ProductCatalog> pcs = cmp.ProductCatalogCollection;
                        if (pcs.Count > 0)
                        {
                            CatalogManufeaturerProduct temp = cmp;
                            temp.ProductCatalogCollection = pcs;
                            temps.Add(temp);
                        }
                    }
                    NewCatalogProducts1.QuickCatalogManufeaturerProductList = temps;
                    NewCatalogProducts1.CurrentPS = currentParameters;
                }
            }

            //GoogleAds_Catalog_Horizontal_Down.RootCategoryID = rootCategoryID;
            //GoogleAds_Catalog_Horizontal_Centre.RootCategoryID = rootCategoryID;

            if (catalogPageInfo.PageCount < this.currentPage)
            {
                if (currentParameters.ContainsKey("pg"))
                {
                    currentParameters.Remove("pg");
                }
                Context.Response.Status = "301 Moved Permanently";
                Context.Response.AddHeader("Location", UrlController.GetRewriterUrl(PageName.Catalog, currentParameters));
                Context.Response.End();
            }

            //if (this.RelatedCategories1 != null)
            //{
            //    List<PriceMeCache.RelatedCategoryCache> relatedCategory = CategoryController.GetRelatedCategory(category.CategoryID);
            //    if (relatedCategory != null)
            //    {
            //        foreach (PriceMeCache.RelatedCategoryCache rcc in relatedCategory)
            //        {
            //            rcc.Url = UrlController.GetCatalogUrl(int.Parse(rcc.CategoryID));
            //        }

            //        this.RelatedCategories1.relatedCategory = relatedCategory;
            //    }
            //}

            //FooterBuyingGuides1.category = category;
        }

        private void SetPageInfo()
        {
            string sTitle = " " + System.Configuration.ConfigurationManager.AppSettings["CatalogPageTilteTail"];

            string categorySynonymsTitle = "";
            var categorySynonyms = CategoryController.GetCategorySynonym(category.CategoryID, WebConfig.CountryId);

            if (categorySynonyms != null && categorySynonyms.Count > 0)
            {
                categorySynonyms = categorySynonyms.Where(cs => !string.IsNullOrEmpty(cs.LocalName)).ToList();
                if (categorySynonyms.Count > 1)
                {
                    categorySynonymsTitle += ", " + string.Join(", ", categorySynonyms.Take(categorySynonyms.Count - 1).Select(cs => cs.LocalName)) + " & " + categorySynonyms[categorySynonyms.Count - 1].LocalName;
                }
                else if (categorySynonyms.Count == 1)
                {
                    categorySynonymsTitle = " & " + categorySynonyms[0].LocalName;
                }
            }

            this.Title = category.CategoryName + categorySynonymsTitle + sTitle.Replace("{0}", category.CategoryName);

            string description = string.Format(Resources.Resource.TextString_CatelogDesc, category.CategoryName, Resources.Resource.Country);
            string keywords = string.Format(Resources.Resource.TextString_CatelogKeyword, category.CategoryName, category.CategoryName, category.CategoryName);

            DynamicHtmlHeader.SetHtmlHeader(keywords, description, this.Page);

            Dictionary<string, string> canonicalUrlPars = new Dictionary<string, string>();
            canonicalUrlPars.Add("c", categoryID.ToString());
            string canonicalUrl = UrlController.GetRewriterUrl(PageName.Catalog, canonicalUrlPars);
            Master.AddCanonical(canonicalUrl);

            string fbTitle = category.CategoryName + urlAttributeParameterList.ToH1String() + priceTitleInfo;
            string fbUrl = Resources.Resource.Global_HomePageUrl + Request.RawUrl;
            string fbDes = "Compare prices on " + category.CategoryName + ". Find the product you're looking for, read reviews and shop smarter online.";

            string cImage = "";
            if (!string.IsNullOrEmpty(category.Categoryicon))
                cImage = Utility.GetImage(category.Categoryicon.Replace(".svg", ".png"), "_m");

            DynamicHtmlHeader.SetFaceBookHeader(fbTitle, "product.group", cImage, fbUrl, fbDes, string.Empty, string.Empty, this);

            Master.pageID = (int)CssFile.Catalog;
        }

        private void SetProductFilters()
        {
            List<NarrowByInfo> narrowByInfoList = new List<NarrowByInfo>();
            NarrowByInfo categoryNarrowByInfo = productSeacherWithoutFilters.GetCatalogCategoryResulte();
            if (categoryNarrowByInfo.NarrowItemList.Count > 0)
            {
                AddNarrowByCatalogyUrl(categoryNarrowByInfo.NarrowItemList);
                narrowByInfoList.Add(categoryNarrowByInfo);
            }

            //brands
            if (!CategoryController.IsHiddenBrandsCategoryID(category.CategoryID, WebConfig.CountryId))
            {
                PriceMeCommon.Data.NarrowByInfo manufacturerNarrowByInfo = productSeacherWithoutFilters.GetManufacturerResulte();
                if (manufacturerNarrowByInfo.NarrowItemList.Count > 0)
                {
                    for (int i = 0; i < manufacturerNarrowByInfo.NarrowItemList.Count; i++)
                    {
                        if (manufacturerNarrowByInfo.NarrowItemList[i].Value == "-1")
                        {
                            manufacturerNarrowByInfo.NarrowItemList.RemoveAt(i);
                            break;
                        }
                    }
                    if (manufacturerNarrowByInfo.NarrowItemList.Count > 0)
                    {
                        manufacturerNarrowByInfo.Title = Resources.Resource.TextString_Brands;

                        PriceMeCommon.Data.NarrowByInfo manufacturerNarrowByInfoWithP = catalogPageInfo.MyProductSearcher.GetManufacturerResulte();
                        CatalogProductSearchController.FixFiltersProductCount(manufacturerNarrowByInfo, manufacturerNarrowByInfoWithP);

                        narrowByInfoList.Add(manufacturerNarrowByInfo);
                    }
                }
            }

            //price filter
            PriceMeCommon.Data.NarrowByInfo priceRangeNarrowByInfo = productSeacherWithoutFilters.GetCatalogPriceRangeResulte_New(PriceMe.Utility.CurrentCulture, Resources.Resource.TextString_PriceSymbol, 10, -1);
            if (priceRangeNarrowByInfo.NarrowItemList.Count > 0)
            {
                priceRangeNarrowByInfo.Title = Resources.Resource.TextString_Price;
                priceRangeNarrowByInfo.SelectedValue = priceRangeString;
                priceRangeNarrowByInfo.IsSlider = true;
                priceRangeMaxValue = double.Parse(priceRangeNarrowByInfo.NarrowItemList[priceRangeNarrowByInfo.NarrowItemList.Count - 1].Value);
                priceRangeNarrowByInfo.ProductCountListWithoutP = priceRangeNarrowByInfo.NarrowItemList.Select(ni => ni.ProductCount).ToList();

                PriceMeCommon.Data.NarrowByInfo priceRangeNarrowByInfoWithP = catalogPageInfo.MyProductSearcher.GetCatalogPriceRangeResulte_New(PriceMe.Utility.CurrentCulture, Resources.Resource.TextString_PriceSymbol, 10, priceRangeMaxValue);
                CatalogProductSearchController.FixFiltersProductCount(priceRangeNarrowByInfo, priceRangeNarrowByInfoWithP);

                narrowByInfoList.Add(priceRangeNarrowByInfo);
            }

            //on sale only
            PriceMeCommon.Data.NarrowByInfo onSaleOnlyNarrowByInfo = productSeacherWithoutFilters.GetOnSaleOnlyResulte();
            if (onSaleOnlyNarrowByInfo.NarrowItemList.Count > 0)
            {
                onSaleOnlyNarrowByInfo.Title = Resources.Resource.TextString_OnSale;
                onSaleOnlyNarrowByInfo.Description = Resources.Resource.TextString_OnSaleDesc;

                PriceMeCommon.Data.NarrowByInfo onSaleOnlyNarrowByInfoWithP = catalogPageInfo.MyProductSearcher.GetOnSaleOnlyResulte();
                CatalogProductSearchController.FixFiltersProductCount(onSaleOnlyNarrowByInfo, onSaleOnlyNarrowByInfoWithP);

                narrowByInfoList.Add(onSaleOnlyNarrowByInfo);
            }

            //attributes
            //获取分类所包含的所有Attributes.
            List<PriceMeCommon.Data.NarrowByInfo> attributesNarrowByInfoList = productSeacherWithoutFilters.GetAttributesResulte_New(selectedAttrRangeValues, null);
            //得到各slider attribute 的min-max值范围
            foreach (var narrow in attributesNarrowByInfoList)
            {
                if (narrow.IsSlider)
                {
                    var map = (CategoryAttributeTitleMapCache)narrow.CategoryAttributeTitleMap;
                    narrow.NarrowItemList = narrow.NarrowItemList.OrderBy(ni => ni.FloatValue).ToList();
                    var min = narrow.NarrowItemList[0].FloatValue;
                    var max = narrow.NarrowItemList[narrow.NarrowItemList.Count - 1].FloatValue;
                    narrow.ProductCountListWithoutP = Utility.GetAttributeSliderProductCountList_New(narrow);

                    sliderAttrRange.Add(map.AttributeTitleID, min + "-" + max);
                }
            }

            List<PriceMeCommon.Data.NarrowByInfo> attributesNarrowByInfoListWithP = catalogPageInfo.MyProductSearcher.GetAttributesResulte_New(selectedAttrRangeValues, attributesNarrowByInfoList);
            CatalogProductSearchController.FixAttributesNarrowByInfoProductCount(attributesNarrowByInfoList, attributesNarrowByInfoListWithP);
            narrowByInfoList.AddRange(attributesNarrowByInfoList);

            //Days on PriceMe
            NarrowByInfo daysNarrowByInfo = productSeacherWithoutFilters.GetDaysOnPriceMeResult();
            if (daysNarrowByInfo.NarrowItemList.Count > 0)
            {
                daysNarrowByInfo.Title = Resources.Resource.TextString_DaysOnPriceMe;
                daysNarrowByInfo.ProductCountListWithoutP = Utility.GetDaysProductCountList(daysNarrowByInfo);

                PriceMeCommon.Data.NarrowByInfo daysNarrowByInfoWithP = catalogPageInfo.MyProductSearcher.GetDaysOnPriceMeResult();
                CatalogProductSearchController.FixFiltersProductCount(daysNarrowByInfo, daysNarrowByInfoWithP);
                narrowByInfoList.Add(daysNarrowByInfo);
            }

            //retailers
            PriceMeCommon.Data.NarrowByInfo retailerNarrowByInfo = productSeacherWithoutFilters.GetRetailerResulte();
            if (retailerNarrowByInfo.NarrowItemList.Count > 0)
            {
                retailerNarrowByInfo.Title = Resources.Resource.TextString_Retailers;
                PriceMeCommon.Data.NarrowByInfo retailerNarrowByInfoWithP = catalogPageInfo.MyProductSearcher.GetRetailerResulte();
                CatalogProductSearchController.FixFiltersProductCount(retailerNarrowByInfo, retailerNarrowByInfoWithP);

                narrowByInfoList.Add(retailerNarrowByInfo);
            }

            //Product Name
            PriceMeCommon.Data.NarrowByInfo pnameNarrowByInfo = new NarrowByInfo();
            pnameNarrowByInfo.Title = Resources.Resource.TextString_ProductName;
            if (narrowByInfoList.Count < 6)
            {
                narrowByInfoList.Add(pnameNarrowByInfo);
            }
            else
            {
                narrowByInfoList.Insert(6, pnameNarrowByInfo);
            }

            //UrlController.SetNarrowByInfoUrl(narrowByInfoList, PageName.Catalog, currentParameters, selectedAttributeGroupIDs);

            foreach (var info in narrowByInfoList)
            {
                if (info.Name == "Attribute" && !info.IsSlider)
                {
                    for (int i = 0; i < info.NarrowItemList.Count();)
                    {
                        NarrowItem narrowItem = info.NarrowItemList[i];
                        int attributeID = int.Parse(narrowItem.Value);
                        int groupID = PriceMeCommon.BusinessLogic.AttributesController.GetCatalogAttributeGroupID(attributeID);
                        if (groupID > 0)
                        {
                            if (info.NarrowItemGroupDic.ContainsKey(groupID))
                            {
                                info.NarrowItemGroupDic[groupID].GroupItems.Add(narrowItem);
                                info.NarrowItemList.RemoveAt(i);
                            }
                            else
                            {
                                NarrowItemGroup narrowItemGroup = new NarrowItemGroup();
                                narrowItemGroup.GroupID = groupID;
                                narrowItemGroup.GroupName = PriceMeCommon.BusinessLogic.AttributesController.GetCatalogAttributeGroupName(groupID);

                                if (selectedAttributeGroupIDs.Contains(groupID))
                                {
                                    string url = "";
                                    removeUrlDic.TryGetValue("avg-" + groupID, out url);
                                    narrowItemGroup.IsSelected = true;
                                    narrowItemGroup.GroupUrl = url;
                                }
                                else
                                {
                                    narrowItemGroup.IsSelected = false;

                                    Dictionary<string, string> psTemp = new Dictionary<string, string>(currentParameters);
                                    psTemp.Remove("avg");
                                    List<int> avgIDs = new List<int>(this.selectedAttributeGroupIDs);
                                    avgIDs.Add(groupID);
                                    psTemp.Add("avg", string.Join("-", avgIDs));
                                    narrowItemGroup.GroupUrl = UrlController.GetRewriterUrl(PageName.Catalog, psTemp);
                                }

                                narrowItemGroup.GroupItems.Add(narrowItem);
                                info.NarrowItemList.RemoveAt(i);

                                info.NarrowItemGroupDic.Add(groupID, narrowItemGroup);
                            }
                        }
                        else
                        {
                            i++;
                        }
                    }
                }
            }

            this.NewFilters1.NarrowByInfoList = narrowByInfoList;
            this.NewFilters1.CurrentProductCount = catalogPageInfo.CurrentProductCount;
            this.NewFilters1.TotalProductCount = totalProductCount;
            this.NewFilters1.RemoveUrlDic = removeUrlDic;
            this.NewFilters1.MyPriceRange = priceRange;
            this.NewFilters1.SearchWithIn = swi;
            this.NewFilters1.MyDaysRange = daysRange;
            this.NewFilters1.Selections = selections;
            this.NewFilters1.UrlNoSelection = GetDeleteAllUrl();
            this.NewFilters1.CategoryID = categoryID;
            this.NewFilters1.PageSize = Static_PageSize;
            this.NewFilters1.PagePosition = currentPage;
            this.NewFilters1.SortBy = sortBy;
            this.NewFilters1.DefaultView = DefaultView;
            this.NewFilters1.DefaultSortBy = Static_DefaultSortBy;
            if (string.IsNullOrEmpty(v))
            {
                this.NewFilters1.View = DefaultView;
            }
            else
            {
                this.NewFilters1.View = v;
            }
            this.NewFilters1.PageToName = "Catalog";
        }

        private void AddNarrowByCatalogyUrl(List<NarrowItem> narrowItemList)
        {
            foreach (var ni in narrowItemList)
            {
                ni.Url = PriceMe.UrlController.GetCatalogUrl(int.Parse(ni.Value));
            }
        }

        private void SetSortAndViewInfo(Dictionary<string, string> cp)
        {
            Dictionary<string, string> currentParameters = new Dictionary<string, string>(cp);
            if (currentPage > 1)
            {
                currentParameters.Add("pg", currentPage.ToString());
            }

            string qName = "sb";
            Dictionary<string, string> psTemp = new Dictionary<string, string>(currentParameters);
            sortByInfoList = new List<LinkInfo>();

            string clicksKey = "Clicks";
            psTemp.Remove(qName);
            //psTemp.Add(qName, clicksKey);
            LinkInfo clicksLinkInfo = new LinkInfo();
            clicksLinkInfo.Value = clicksKey;
            clicksLinkInfo.LinkText = "Sort by popularity";
            clicksLinkInfo.LinkURL = UrlController.GetRewriterUrl(PageName.Catalog, psTemp);
            sortByInfoList.Add(clicksLinkInfo);

            string bestPriceKey = "BestPrice";
            psTemp.Remove(qName);
            psTemp.Add(qName, bestPriceKey);
            LinkInfo bestPriceLinkInfo = new LinkInfo();
            bestPriceLinkInfo.Value = bestPriceKey;
            bestPriceLinkInfo.LinkText = "Sort by price: low to high";
            bestPriceLinkInfo.LinkURL = UrlController.GetRewriterUrl(PageName.Catalog, psTemp);
            sortByInfoList.Add(bestPriceLinkInfo);

            string bestPriceRevKey = "BestPrice-rev";
            psTemp.Remove(qName);
            psTemp.Add(qName, bestPriceRevKey);
            LinkInfo bestPriceRevLinkInfo = new LinkInfo();
            bestPriceRevLinkInfo.Value = bestPriceRevKey;
            bestPriceRevLinkInfo.LinkText = "Sort by price: high to low";
            bestPriceRevLinkInfo.LinkURL = UrlController.GetRewriterUrl(PageName.Catalog, psTemp);
            sortByInfoList.Add(bestPriceRevLinkInfo);

            string ratingKey = "Rating";
            psTemp.Remove(qName);
            psTemp.Add(qName, ratingKey);
            LinkInfo ratingLinkInfo = new LinkInfo();
            ratingLinkInfo.Value = ratingKey;
            ratingLinkInfo.LinkText = "Sort by average rating";
            ratingLinkInfo.LinkURL = UrlController.GetRewriterUrl(PageName.Catalog, psTemp);
            sortByInfoList.Add(ratingLinkInfo);

            string newestKey = "Newest";
            psTemp.Remove(qName);
            psTemp.Add(qName, newestKey);
            LinkInfo newestInfo = new LinkInfo();
            newestInfo.Value = newestKey;
            newestInfo.LinkText = "Sort by newness";
            newestInfo.LinkURL = UrlController.GetRewriterUrl(PageName.Catalog, psTemp);
            sortByInfoList.Add(newestInfo);

            string titleKey = "Title";
            psTemp.Remove(qName);
            psTemp.Add(qName, titleKey);
            LinkInfo titleInfo = new LinkInfo();
            titleInfo.Value = titleKey;
            titleInfo.LinkText = "Sort by name: A to Z";
            titleInfo.LinkURL = UrlController.GetRewriterUrl(PageName.Catalog, psTemp);
            sortByInfoList.Add(titleInfo);

            //string onSaleKey = "Sale";
            //psTemp.Remove(qName);
            //psTemp.Add(qName, onSaleKey);
            //LinkInfo onSaleInfo = new LinkInfo();
            //onSaleInfo.Value = onSaleKey;
            //onSaleInfo.LinkText = Resources.Resource.TextString_OnSale;
            //onSaleInfo.LinkURL = UrlController.GetRewriterUrl(PageName.Catalog, psTemp);
            //sortByInfoList.Add(onSaleInfo);

            Dictionary<string, string> psTemp2 = new Dictionary<string, string>(currentParameters);

            psTemp2.Remove("v");
            if (!DefaultView.Equals("Grid", StringComparison.InvariantCultureIgnoreCase))
            {
                psTemp2.Add("v", "Grid");
            }
            GridViewUrl = UrlController.GetRewriterUrl(PageName.Catalog, psTemp2);

            psTemp2.Remove("v");
            if (!DefaultView.Equals("List", StringComparison.InvariantCultureIgnoreCase))
            {
                psTemp2.Add("v", "List");
            }
            ListViewUrl = UrlController.GetRewriterUrl(PageName.Catalog, psTemp2);

            psTemp2.Remove("v");
            if (!DefaultView.Equals("Quick", StringComparison.InvariantCultureIgnoreCase))
            {
                psTemp2.Add("v", "Quick");
            }
            psTemp2.Remove("pg");
            QuickViewUrl = UrlController.GetRewriterUrl(PageName.Catalog, psTemp2);
        }

        private List<int> BuilderSelections()
        {
            removeUrlDic = new Dictionary<string, string>();//用于保存去除过滤的url
            List<int> selectedAttributeTitleIDs = new List<int>();
            selections = new List<LinkInfo>();
            if (selectedRetailers != null && selectedRetailers.Count > 0)
            {
                foreach (int retailerID in selectedRetailers)
                {
                    var retailer = RetailerController.GetRetailerDeep(retailerID, WebConfig.CountryId);
                    if (retailer != null)
                    {
                        LinkInfo selection = new LinkInfo();
                        selection.LinkText = retailer.RetailerName;
                        //Dictionary<string, string> psTemp = new Dictionary<string, string>(currentParameters);
                        //psTemp.Remove("pcsid");
                        //List<int> rIDs = new List<int>(this.selectedRetailers);
                        //rIDs.Remove(retailerID);
                        //if (rIDs.Count > 0)
                        //{
                        //    psTemp.Add("pcsid", string.Join("-", rIDs));
                        //}
                        //selection.LinkURL = UrlController.GetRewriterUrl(PageName.Catalog, psTemp);
                        selections.Add(selection);
                        selection.LinkURL = "1";
                        removeUrlDic.Add("r-" + retailerID, selection.LinkURL);
                    }
                }
            }
            if (brandIDList != null && brandIDList.Count > 0)
            {
                foreach (int brandID in brandIDList)
                {
                    var manufacturer = ManufacturerController.GetManufacturerByID(brandID, WebConfig.CountryId);
                    if (manufacturer != null)
                    {
                        LinkInfo selection = new LinkInfo();
                        selection.LinkText = manufacturer.ManufacturerName;
                        //Dictionary<string, string> psTemp = new Dictionary<string, string>(currentParameters);
                        //psTemp.Remove("m");
                        //List<int> bIDs = new List<int>(this.brandIDList);
                        //bIDs.Remove(brandID);
                        //if (bIDs.Count > 0)
                        //{
                        //    psTemp.Add("m", string.Join("-", bIDs));
                        //}
                        //selection.LinkURL = UrlController.GetRewriterUrl(PageName.Catalog, psTemp);
                        selections.Add(selection);
                        selection.LinkURL = "1";
                        removeUrlDic.Add("m-" + brandID, selection.LinkURL);
                    }
                }
            }

            if (priceRange != null && (priceRange.MinPrice != 0 || priceRange.MaxPrice != priceRangeMaxValue))
            {
                LinkInfo selection = new LinkInfo();
                selection.LinkText = Utility.GetPriceRangeString(priceRange);
                //Dictionary<string, string> psTemp = new Dictionary<string, string>(currentParameters);
                //psTemp.Remove("pr");
                //selection.LinkURL = UrlController.GetRewriterUrl(PageName.Catalog, psTemp);

                selections.Add(selection);
            }

            if (onSaleOnly == "1")
            {
                LinkInfo selection = new LinkInfo();
                selection.LinkText = "On sale only";

                selections.Add(selection);
                selection.LinkURL = "1";
                removeUrlDic.Add("os-" + onSaleOnly, selection.LinkURL);
            }

            //
            foreach (int avgid in selectedAttributeGroupIDs)
            {
                LinkInfo selection = new LinkInfo();
                selection.LinkText = PriceMeCommon.BusinessLogic.AttributesController.GetCatalogAttributeGroupName(avgid);
                //Dictionary<string, string> psTemp = new Dictionary<string, string>(currentParameters);
                //psTemp.Remove("avg");
                //List<int> avgIDs = new List<int>(this.selectedAttributeGroupIDs);
                //avgIDs.Remove(avgid);
                //if (avgIDs.Count > 0)
                //{
                //    psTemp.Add("avg", string.Join("-", avgIDs));
                //}
                //selection.LinkURL = UrlController.GetRewriterUrl(PageName.Catalog, psTemp);

                selection.LinkURL = "1";
                removeUrlDic.Add("avg-" + avgid, selection.LinkURL);

                selections.Add(selection);
            }

            foreach (int avid in selectedAttributeIDs)
            {
                LinkInfo selection = new LinkInfo();
                AttributeValueCache attributeValue = AttributesController.GetAttributeValueByID(avid);
                if (attributeValue == null)
                {
                    continue;
                }
                AttributeTitleCache attributeTitle = AttributesController.GetAttributeTitleByVauleID(avid);
                if (attributeTitle == null)
                {
                    continue;
                }
                selectedAttributeTitleIDs.Add(attributeTitle.TypeID);
                if (!attributeValue.Value.Equals("yes", StringComparison.InvariantCultureIgnoreCase) && !attributeValue.Value.Equals("no", StringComparison.InvariantCultureIgnoreCase))
                {
                    selection.LinkText = attributeValue.Value + (attributeTitle.Unit == null ? "" : " " + attributeTitle.Unit.Trim());
                }
                else
                {
                    selection.LinkText = attributeTitle.Title + ": " + attributeValue.Value;
                }
                //List<int> savIDs = new List<int>();
                //foreach (int aid in selectedAttributeIDs)
                //{
                //    if (aid == avid)
                //    {
                //        continue;
                //    }
                //    savIDs.Add(aid);
                //}
                //Dictionary<string, string> psTemp = UrlController.GetCatalogParameters(categoryID, brandIDList,
                //    priceRangeString, savIDs, selectedAttributeRangeIDs, this.sortBy, selectedRetailers, this.v, selectedAttrRangeValues);
                //selection.LinkURL = UrlController.GetRewriterUrl(PageName.Catalog, psTemp);
                selections.Add(selection);
                selection.LinkURL = "1";
                removeUrlDic.Add("av-" + avid, selection.LinkURL);
            }
            foreach (int avrid in selectedAttributeRangeIDs)
            {
                LinkInfo selection = new LinkInfo();
                var attributeValueRange = AttributesController.GetAttributeValueRangeByID(avrid);
                if (attributeValueRange == null)
                {
                    continue;
                }
                AttributeTitleCache attributeTitle = AttributesController.GetAttributeTitleByID(attributeValueRange.AttributeTitleID);
                if (attributeTitle == null)
                {
                    continue;
                }
                selectedAttributeTitleIDs.Add(attributeTitle.TypeID);
                selection.LinkText = AttributesController.GetAttributeValueString(attributeValueRange, attributeTitle.Unit);

                selections.Add(selection);
                selection.LinkURL = "1";
                if (!removeUrlDic.Keys.Contains("avr-" + avrid))
                    removeUrlDic.Add("avr-" + avrid, selection.LinkURL);
            }
            //slider attribute selection
            foreach (int attrTitleID in selectedAttrRangeValues.Keys)
            {
                if (sliderAttrRange.ContainsKey(attrTitleID))
                {
                    //如果所选值范围 == 该 attr min-max, 不显示selection
                    if (sliderAttrRange[attrTitleID] == selectedAttrRangeValues[attrTitleID])
                        continue;
                }

                LinkInfo selection = new LinkInfo();
                AttributeTitleCache attributeTitle = AttributesController.GetAttributeTitleByID(attrTitleID);
                if (attributeTitle == null)
                {
                    continue;
                }
                selection.LinkText = selectedAttrRangeValues[attrTitleID] + " " + attributeTitle.Unit;
                //生成不带当前attribute的url
                var selectedAttrRangeValues_ = new Dictionary<int, string>();
                foreach (var value in selectedAttrRangeValues)
                {
                    if (value.Key == attrTitleID)//去掉当前attribute
                    {
                        continue;
                    }
                    selectedAttrRangeValues_.Add(value.Key, value.Value);
                }

                selections.Add(selection);
            }

            if (!string.IsNullOrEmpty(swi))
            {
                LinkInfo selection = new LinkInfo();
                selection.LinkText = "\"" + swi.Replace("-", " ") + "\"";
                Dictionary<string, string> psTemp = new Dictionary<string, string>(currentParameters);
                psTemp.Remove("swi");
                //selection.LinkURL = UrlController.GetRewriterUrl(PageName.Catalog, psTemp);

                selections.Add(selection);
            }

            if (!string.IsNullOrEmpty(daysRangeString))
            {
                LinkInfo selection = new LinkInfo();
                selection.LinkText = daysRangeString + " " + Resources.Resource.TextString_days;
                //Dictionary<string, string> psTemp = new Dictionary<string, string>(currentParameters);
                //psTemp.Remove("dr");
                //selection.LinkURL = UrlController.GetRewriterUrl(PageName.Catalog, psTemp);

                selections.Add(selection);
            }

            return selectedAttributeTitleIDs;
        }

        public string GetDeleteAllUrl()
        {
            var np = new Dictionary<string, string>();

            np.Add("c", currentParameters["c"]);

            if (!string.IsNullOrEmpty(v) && v != DefaultView)
                np.Add("v", v);

            if (!string.IsNullOrEmpty(sortBy) && sortBy != Static_DefaultSortBy)
                np.Add("sb", sortBy);

            return UrlController.GetRewriterUrl(PageName.Catalog, np);
        }

        void LoadCrumbs()
        {
            BreadCrumbInfo breadCrumbInfo = Utility.GetCatalogBreadCrumbInfo(category, null, false, "");
            breadCrumbInfo.CurrentPageId = category.CategoryID.ToString();
            breadCrumbInfo.CurrentPageKey = "catalog";
            Master.SetBreadCrumb(breadCrumbInfo, category.CategoryID);
            string check = string.Format(Resources.Resource.TextString_Checkout, category.CategoryName);
            Master.InitATag(check);
        }

        private void SetTop3(List<PriceMeCommon.Data.ProductCatalog> productCatalogList, List<string> popularTop3)
        {
            foreach (var pc in productCatalogList)
            {
                if (popularTop3.Contains(pc.ProductID))
                {
                    pc.IsTop3 = true;
                }
            }
        }

        protected string GetSortByTitle(string sbValue, List<LinkInfo> linkList, string defaultValue)
        {
            foreach (var l in linkList)
            {
                if (sbValue.Equals(l.Value, StringComparison.InvariantCultureIgnoreCase))
                {
                    return l.LinkText;
                }
            }
            return defaultValue;
        }
    }
}