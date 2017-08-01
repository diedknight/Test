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
using System.Text.RegularExpressions;

namespace HotterWinds
{
    public partial class Search : System.Web.UI.Page
    {
        static readonly int Static_PageSize = 24;
        static readonly string Static_DefaultSortBy = "Clicks";

        int categoryID;
        int currentPage;
        protected string DefaultView = "List";
        protected int ReferenceCategoryID;//引用页的CategoryID;

        string queryKeywords;
        string searchKeywords;
        string retailerIDs;
        string manufeaturerIDs;
        string attributeValuesIDString;
        string attributeGroupIDString;
        string attributeValuesRangeString;//slider attr selected parameter
        string displayAllManufeaturerIDString;//quick view display all manufaeturer product
        string priceRangeString;
        protected string v;
        protected string swi;
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
        Dictionary<string, string> currentParameters;
        ProductSearcher productSeacherWithoutFilters;
        Dictionary<string, string> removeUrlDic;
        DaysRange daysRange;
        CategoryCache category;

        protected int isFromProductPage;
        protected string filterStatusBySorting;
        protected Dictionary<int, string> filterStatus = new Dictionary<int, string>();
        protected int totalProductCount;
        protected List<LinkInfo> selections;
        protected List<LinkInfo> sortByInfoList;
        protected string GridViewUrl;
        protected string ListViewUrl;
        protected string QuickViewUrl;
        protected string GoogleSearchKws;
        protected string EncodeSW;

        string priceTitleInfo = "";
        double priceRangeMaxValue;

        protected CatalogPageInfo catalogPageInfo;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (GetParameters())
            {
                SetValue();
                SetPageInfo();
                BuilderSelections();
                SetControlsValue();

                SetProductFilters();

                SetSortAndViewInfo(currentParameters);
                LoadCrumbs();
                SetBanner();

                if (v.ToLower().Equals("quick"))
                {
                    this.PrettyPager1.IsDisplay = false;
                }
                else
                {
                    this.PrettyPager1.PageTo = PageName.Search;
                    this.PrettyPager1.totalPages = catalogPageInfo.PageCount;
                    this.PrettyPager1.currentPage = this.currentPage;
                    this.PrettyPager1.ps = new Dictionary<string, string>(currentParameters);
                    this.PrettyPager1.SetPaginationHeader();
                    this.PrettyPager1.PageSize = Static_PageSize;
                    this.PrettyPager1.ItemCount = catalogPageInfo.CurrentProductCount;
                    this.PrettyPager1.DisplayItemCountInfo = true;
                }
            }
        }

        private bool GetParameters()
        {
            queryKeywords = Utility.GetParameter("q");
            categoryID = Utility.GetIntParameter("c");//category ID
            manufeaturerIDs = Utility.GetParameter("m");//selected manufeaturers
            attributeValuesIDString = Utility.GetParameter("avs");//selected attribute values ID string
            attributeValuesRangeString = Utility.GetParameter("avsr");//selected attribute values range string
            swi = Utility.GetParameter("swi");//search with in
            attributeGroupIDString = Utility.GetParameter("avg");//attribute value groups
            priceRangeString = Utility.GetParameter("pr").Replace(",", "").Replace(".", "");
            currentPage = Utility.GetIntParameter("pg");//current page number
            sortBy = Utility.GetParameter("sb");//sort by value
            retailerIDs = Utility.GetParameter("pcsid");
            daysRangeString = Utility.GetParameter("dr");
            selectedRetailers = new List<int>();
            v = Utility.GetParameter("v");
            displayAllManufeaturerIDString = Utility.GetParameter("samp");
            onSaleOnly = Utility.GetParameter("os");

            return true;
        }

        private int GetReferenceCategory(Uri urlReferrer)
        {
            int cid = 0;

            if (urlReferrer != null)
            {
                string referrerUrl = urlReferrer.AbsolutePath;

                if (referrerUrl.IndexOf("/c-", StringComparison.InvariantCultureIgnoreCase) > -1)
                {
                    Regex regex = new Regex(@"/c-(?<cid>\d+)");
                    Match match = regex.Match(referrerUrl);
                    if (match.Success)
                    {
                        cid = int.Parse(match.Groups["cid"].Value);
                    }
                }
                else if (referrerUrl.IndexOf("/p-", StringComparison.InvariantCultureIgnoreCase) > -1)
                {
                    Match ma = Regex.Match(referrerUrl, @"(/(?<name>[^/]*))?/p-(?<pid>\d+)\.aspx", RegexOptions.IgnoreCase);

                    if (ma.Success)
                    {
                        int pid = int.Parse(ma.Groups["pid"].Value);
                        var pCate = CategoryController.GetCategoryByProductID(pid, WebConfig.CountryId);
                        if (pCate != null)
                        {
                            cid = pCate.CategoryID;
                        }
                    }
                }
            }

            return cid;
        }

        private bool SetValue()
        {
            if (categoryID == 0)
            {
                ReferenceCategoryID = GetReferenceCategory(Request.UrlReferrer);
            }
            else
            {
                ReferenceCategoryID = categoryID;
            }

            category = CategoryController.GetCategoryByCategoryID(categoryID, WebConfig.CountryId);

            GoogleSearchKws = queryKeywords.Replace("'", "");

            if (string.IsNullOrEmpty(queryKeywords))
            {
                queryKeywords = System.Configuration.ConfigurationManager.AppSettings["NullSearchKeyWords"];
            }

            searchKeywords = (queryKeywords.Trim() + " " + swi.Trim()).Trim();

            EncodeSW = Server.HtmlEncode(searchKeywords);

            if (v.Equals(DefaultView, StringComparison.InvariantCultureIgnoreCase))
            {
                v = "";
            }

            if (sortBy.Equals(Static_DefaultSortBy, StringComparison.InvariantCultureIgnoreCase))
            {
                sortBy = "";
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
                }
                else
                {
                    priceRangeString = "";
                }
            }

            if (!string.IsNullOrEmpty(daysRangeString))
            {
                daysRange = DaysRange.Create(daysRangeString);
            }

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
                            int avgID = AttributesController.GetCatalogAttributeGroupID(aid);
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

            currentParameters = UrlController.GetCatalogParameters(this.categoryID, this.brandIDList, priceRangeString,
                selectedAttributeIDs, selectedAttributeRangeIDs, this.sortBy, selectedRetailers, v, swi,
                this.displayAllManufeaturerIDString, selectedAttrRangeValues, selectedAttributeGroupIDs, daysRangeString, onSaleOnly);
            currentParameters.Add("q", queryKeywords);
            if (categoryID == 0)
            {
                currentParameters.Remove("c");
            }

            Dictionary<string, string> newPS = new Dictionary<string, string>(currentParameters);
            if (currentPage > 1)
            {
                newPS.Add("pg", currentPage.ToString());
            }

            if (priceRange != null)
            {
                priceTitleInfo = " priced " + priceRange.ToPriceString(PriceMe.Utility.CurrentCulture, Resources.Resource.TextString_PriceSymbol).Replace("-", " to ");
            }

            List<int> avIDs = new List<int>(selectedAttributeIDs);
            foreach (int avgID in selectedAttributeGroupIDs)
            {
                List<int> avIDList = AttributesController.GetCatalogAttributeValues(avgID);
                if (avIDList != null)
                {
                    avIDs.AddRange(avIDList);
                }
            }

            if (string.IsNullOrEmpty(sortBy))
            {
                sortBy = Static_DefaultSortBy;
            }

            catalogPageInfo = PriceMeCommon.BusinessLogic.CatalogProductSearchController.SearchProducts(this.categoryID, this.brandIDList, priceRange, avIDs, selectedAttributeRangeIDs, this.sortBy, searchKeywords, selectedRetailers, true, WebConfig.CountryId, false, selectedAttrRangeValues, true, currentPage, Static_PageSize, daysRange, onSaleOnly);
            Utility.FixProductCatalogList(catalogPageInfo.ProductCatalogList, v);

            productSeacherWithoutFilters = new ProductSearcher(queryKeywords, this.categoryID, null, null, null, null, "clicks", null, SearchController.MaxSearchCount_Static, WebConfig.CountryId, false, true, false, true, null, "", null);

            return true;
        }

        void SetControlsValue()
        {
            if (NewCatalogProducts1 != null)
            {
                NewCatalogProducts1.RootCategoryID = 0;
                NewCatalogProducts1.ShowCompareBox = true;
                if (string.IsNullOrEmpty(v))
                {
                    NewCatalogProducts1.View = DefaultView;
                }
                else
                {
                    NewCatalogProducts1.View = v;
                }
                if (category != null)
                {
                    NewCatalogProducts1.IsFilterByBrand = category.IsFilterByBrand;
                }
                if (!v.Equals("quick", StringComparison.InvariantCultureIgnoreCase))
                {
                    List<string> popularTop3 = productSeacherWithoutFilters.GetTop3Products();
                    SetTop3(catalogPageInfo.ProductCatalogList, popularTop3);
                    NewCatalogProducts1.ProductCatalogList = catalogPageInfo.ProductCatalogList;
                }
                else
                {
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
                NewCatalogProducts1.IsSearchProduct = "1";
            }

            if (catalogPageInfo.PageCount < this.currentPage)
            {
                if (currentParameters.ContainsKey("pg"))
                {
                    currentParameters.Remove("pg");
                }
                Context.Response.Status = "301 Moved Permanently";
                Context.Response.AddHeader("Location", UrlController.GetRewriterUrl(PageName.Search, currentParameters));
                Context.Response.End();
            }

            string pageViewUrl = "";
            if (catalogPageInfo.CurrentProductCount == 0)
            {
                pageViewUrl = Request.RawUrl + "&sisc=-1";
            }
            else
            {
                pageViewUrl = Request.RawUrl + "&sisc=" + this.ReferenceCategoryID;
            }
            Master.SetPageViewLink(pageViewUrl);
        }

        private void SetPageInfo()
        {
            Title = PriceMe.Utility.UrlEncode(Resources.Resource.TextString_Search + " " + EncodeSW + " " + Resources.Resource.TextString_OnPriceMe);

            DynamicHtmlHeader.SetHtmlHeader(searchKeywords, "search", this.Page);

            Master.pageID = (int)CssFile.Catalog;
            Master.IsSearchPage = true;
        }

        private void SetProductFilters()
        {
            totalProductCount = productSeacherWithoutFilters.GetProductCount();
            if (totalProductCount == 1)
            {
                var pc = productSeacherWithoutFilters.GetSearchResult(1, 1).ProductCatalogList[0];
                if (!CategoryController.IsSearchOnly(pc.CategoryID, WebConfig.CountryId))
                    RedirectCheck(searchKeywords, pc);
            }

            if (this.NewFilters1 != null)
            {
                List<NarrowByInfo> narrowByInfoList = new List<NarrowByInfo>();

                NarrowByInfo matchCategoryNarrowByInfo = catalogPageInfo.MyProductSearcher.GetMatchCategoryResulte();
                if (matchCategoryNarrowByInfo.NarrowItemList.Count == 1)
                {
                    int cid = int.Parse(matchCategoryNarrowByInfo.NarrowItemList[0].Value);
                    if (!CategoryController.IsSearchOnly(cid, WebConfig.CountryId))
                    {
                        string url = UrlController.GetCatalogUrl(cid);
                        Context.Response.Status = "301 Moved Permanently";
                        Context.Response.AddHeader("Location", url);
                        Context.Response.End();
                    }
                }
                else if (matchCategoryNarrowByInfo.NarrowItemList.Count > 1)
                {
                    narrowByInfoList.Add(matchCategoryNarrowByInfo);
                }

                if (categoryID == 0)
                {
                    NarrowByInfo categoryNarrowByInfo = catalogPageInfo.MyProductSearcher.GetSearchCategoryResulte();
                    if (categoryNarrowByInfo.NarrowItemList.Count > 0)
                    {
                        AddNarrowByCatalogyUrl(categoryNarrowByInfo.NarrowItemList);
                        narrowByInfoList.Add(categoryNarrowByInfo);
                    }
                }

                PriceMeCommon.Data.NarrowByInfo searchPriceRangeNarrowByInfo = productSeacherWithoutFilters.GetCatalogPriceRangeResulte_New(Utility.CurrentCulture, Resources.Resource.TextString_PriceSymbol, 10, -1);
                if (searchPriceRangeNarrowByInfo.NarrowItemList.Count > 0)
                {
                    searchPriceRangeNarrowByInfo.Title = Resources.Resource.TextString_Price;
                    searchPriceRangeNarrowByInfo.Description = priceRangeString;
                    searchPriceRangeNarrowByInfo.IsSlider = true;
                    priceRangeMaxValue = double.Parse(searchPriceRangeNarrowByInfo.NarrowItemList[searchPriceRangeNarrowByInfo.NarrowItemList.Count - 1].Value);
                    searchPriceRangeNarrowByInfo.ProductCountListWithoutP = searchPriceRangeNarrowByInfo.NarrowItemList.Select(ni => ni.ProductCount).ToList();

                    PriceMeCommon.Data.NarrowByInfo priceRangeNarrowByInfoWithP = catalogPageInfo.MyProductSearcher.GetCatalogPriceRangeResulte_New(PriceMe.Utility.CurrentCulture, Resources.Resource.TextString_PriceSymbol, 10, priceRangeMaxValue);
                    CatalogProductSearchController.FixFiltersProductCount(searchPriceRangeNarrowByInfo, priceRangeNarrowByInfoWithP);

                    narrowByInfoList.Add(searchPriceRangeNarrowByInfo);
                }

                if (categoryID > 0)
                {
                    List<PriceMeCommon.Data.NarrowByInfo> attributesNarrowByInfoList = productSeacherWithoutFilters.GetAttributesResulte_New(selectedAttrRangeValues, null);
                    if (attributesNarrowByInfoList.Count > 0)
                    {
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
                    }
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

                ////brands
                //if (CategoryController.IsDisplayBrands(category))
                //{
                //    PriceMeCommon.Data.NarrowByInfo manufacturerNarrowByInfo = productSeacherWithoutFilters.GetManufacturerResulte();
                //    if (manufacturerNarrowByInfo.NarrowItemList.Count > 0)
                //    {
                //        for (int i = 0; i < manufacturerNarrowByInfo.NarrowItemList.Count; i++)
                //        {
                //            if (manufacturerNarrowByInfo.NarrowItemList[i].Value == "-1")
                //            {
                //                manufacturerNarrowByInfo.NarrowItemList.RemoveAt(i);
                //                break;
                //            }
                //        }
                //        if (manufacturerNarrowByInfo.NarrowItemList.Count > 0)
                //        {
                //            manufacturerNarrowByInfo.Title = Resources.Resource.TextString_Brands;
                //            narrowByInfoList.Add(manufacturerNarrowByInfo);
                //        }
                //    }
                //}

                ////retailers
                //PriceMeCommon.Data.NarrowByInfo retailerNarrowByInfo = productSeacherWithoutFilters.GetRetailerResulte();
                //if (retailerNarrowByInfo.NarrowItemList.Count > 0)
                //{
                //    retailerNarrowByInfo.Title = Resources.Resource.TextString_Retailers;
                //    narrowByInfoList.Add(retailerNarrowByInfo);
                //}

                //UrlController.SetNarrowByInfoUrl(narrowByInfoList, PageName.Search, currentParameters, selectedAttributeGroupIDs);

                foreach (var info in narrowByInfoList)
                {
                    if (info.Name == "Attribute" && !info.IsSlider)
                    {
                        for (int i = 0; i < info.NarrowItemList.Count();)
                        {
                            NarrowItem narrowItem = info.NarrowItemList[i];
                            int attributeID = int.Parse(narrowItem.Value);
                            int groupID = AttributesController.GetCatalogAttributeGroupID(attributeID);
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
                                    narrowItemGroup.GroupName = AttributesController.GetCatalogAttributeGroupName(groupID);

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
                                        narrowItemGroup.GroupUrl = UrlController.GetRewriterUrl(PageName.Search, psTemp);
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
                this.NewFilters1.QueryKeywords = queryKeywords;
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
                this.NewFilters1.PageToName = "Search";
            }
        }

        private void AddNarrowByCatalogyUrl(List<NarrowItem> narrowItemList)
        {
            var np = new Dictionary<string, string>();
            np.Add("q", currentParameters["q"]);
            np.Add("c", "0");
            foreach (var ni in narrowItemList)
            {
                np.Remove("c");
                np.Add("c", ni.Value);
                ni.Url = PriceMe.UrlController.GetRewriterUrl(PageName.Search, np);
            }
        }

        private void RedirectCheck(string searchKeywords, PriceMeCommon.Data.ProductCatalog productCatalog)
        {
            //只搜索到一个产品则跳转到产品页面
            //if (productCatalog.ProductName.Equals(searchKeywords, StringComparison.InvariantCultureIgnoreCase))
            //{
            string productUrl = UrlController.GetProductUrl(int.Parse(productCatalog.ProductID), productCatalog.ProductName);
            Context.Response.Status = "301 Moved Permanently";
            Context.Response.AddHeader("Location", productUrl);
            Context.Response.End();
            //}
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
            clicksLinkInfo.LinkURL = UrlController.GetRewriterUrl(PageName.Search, psTemp);
            sortByInfoList.Add(clicksLinkInfo);

            string bestPriceKey = "BestPrice";
            psTemp.Remove(qName);
            psTemp.Add(qName, bestPriceKey);
            LinkInfo bestPriceLinkInfo = new LinkInfo();
            bestPriceLinkInfo.Value = bestPriceKey;
            bestPriceLinkInfo.LinkText = "Sort by price: low to high";
            bestPriceLinkInfo.LinkURL = UrlController.GetRewriterUrl(PageName.Search, psTemp);
            sortByInfoList.Add(bestPriceLinkInfo);

            string bestPriceRevKey = "BestPrice-rev";
            psTemp.Remove(qName);
            psTemp.Add(qName, bestPriceRevKey);
            LinkInfo bestPriceRevLinkInfo = new LinkInfo();
            bestPriceRevLinkInfo.Value = bestPriceRevKey;
            bestPriceRevLinkInfo.LinkText = "Sort by price: high to low";
            bestPriceRevLinkInfo.LinkURL = UrlController.GetRewriterUrl(PageName.Search, psTemp);
            sortByInfoList.Add(bestPriceRevLinkInfo);

            string ratingKey = "Rating";
            psTemp.Remove(qName);
            psTemp.Add(qName, ratingKey);
            LinkInfo ratingLinkInfo = new LinkInfo();
            ratingLinkInfo.Value = ratingKey;
            ratingLinkInfo.LinkText = "Sort by average rating";
            ratingLinkInfo.LinkURL = UrlController.GetRewriterUrl(PageName.Search, psTemp);
            sortByInfoList.Add(ratingLinkInfo);

            string newestKey = "Newest";
            psTemp.Remove(qName);
            psTemp.Add(qName, newestKey);
            LinkInfo newestInfo = new LinkInfo();
            newestInfo.Value = newestKey;
            newestInfo.LinkText = "Sort by newness";
            newestInfo.LinkURL = UrlController.GetRewriterUrl(PageName.Search, psTemp);
            sortByInfoList.Add(newestInfo);            

            string titleKey = "Title";
            psTemp.Remove(qName);
            psTemp.Add(qName, titleKey);
            LinkInfo titleInfo = new LinkInfo();
            titleInfo.Value = titleKey;
            titleInfo.LinkText = "Sort by name: A to Z";
            titleInfo.LinkURL = UrlController.GetRewriterUrl(PageName.Search, psTemp);
            sortByInfoList.Add(titleInfo);

            //string onSaleKey = "Sale";
            //psTemp.Remove(qName);
            //psTemp.Add(qName, onSaleKey);
            //LinkInfo onSaleInfo = new LinkInfo();
            //onSaleInfo.Value = onSaleKey;
            //onSaleInfo.LinkText = Resources.Resource.TextString_OnSale;
            //onSaleInfo.LinkURL = UrlController.GetRewriterUrl(PageName.Search, psTemp);
            //sortByInfoList.Add(onSaleInfo);

            Dictionary<string, string> psTemp2 = new Dictionary<string, string>(currentParameters);

            psTemp2.Remove("v");
            psTemp2.Add("v", "Grid");
            GridViewUrl = UrlController.GetRewriterUrl(PageName.Search, psTemp2);

            psTemp2.Remove("v");
            ListViewUrl = UrlController.GetRewriterUrl(PageName.Search, psTemp2);

            psTemp2.Add("v", "Quick");
            psTemp2.Remove("pg");
            QuickViewUrl = UrlController.GetRewriterUrl(PageName.Search, psTemp2);
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
                        //selection.LinkURL = UrlController.GetRewriterUrl(PageName.Search, psTemp);
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
                        //selection.LinkURL = UrlController.GetRewriterUrl(PageName.Search, psTemp);
                        selections.Add(selection);
                        selection.LinkURL = "1";
                        removeUrlDic.Add("m-" + brandID, selection.LinkURL);
                    }
                }
            }

            if (priceRange != null && (priceRange.MinPrice != 0 || priceRange.MaxPrice != priceRangeMaxValue))
            {
                LinkInfo selection = new LinkInfo();
                selection.LinkText = GetPriceRangeString(priceRange);
                //Dictionary<string, string> psTemp = new Dictionary<string, string>(currentParameters);
                //psTemp.Remove("pr");
                //selection.LinkURL = UrlController.GetRewriterUrl(PageName.Search, psTemp);
                selection.LinkURL = "1";
                selections.Add(selection);
            }

            if (onSaleOnly == "1")
            {
                LinkInfo selection = new LinkInfo();
                selection.LinkText = "On sale only";
                selection.LinkURL = "1";
                selections.Add(selection);
                removeUrlDic.Add("os-" + onSaleOnly, selection.LinkURL);
            }
            

            //
            foreach (int avgid in selectedAttributeGroupIDs)
            {
                LinkInfo selection = new LinkInfo();
                selection.LinkText = AttributesController.GetCatalogAttributeGroupName(avgid);
                //Dictionary<string, string> psTemp = new Dictionary<string, string>(currentParameters);
                //psTemp.Remove("avg");
                //List<int> avgIDs = new List<int>(this.selectedAttributeGroupIDs);
                //avgIDs.Remove(avgid);
                //if (avgIDs.Count > 0)
                //{
                //    psTemp.Add("avg", string.Join("-", avgIDs));
                //}
                //selection.LinkURL = UrlController.GetRewriterUrl(PageName.Search, psTemp);
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
                //psTemp.Add("q", queryKeywords);
                //selection.LinkURL = UrlController.GetRewriterUrl(PageName.Search, psTemp);
                selection.LinkURL = "1";
                selections.Add(selection);

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
                //List<int> savIDs = new List<int>();
                //foreach (int aid in selectedAttributeRangeIDs)
                //{
                //    if (aid == avrid)
                //    {
                //        continue;
                //    }
                //    savIDs.Add(aid);
                //}
                //Dictionary<string, string> psTemp = UrlController.GetCatalogParameters(categoryID, this.brandIDList,
                //    priceRangeString, selectedAttributeIDs, savIDs, this.sortBy, selectedRetailers, this.v, selectedAttrRangeValues);
                //psTemp.Add("q", queryKeywords);
                //selection.LinkURL = UrlController.GetRewriterUrl(PageName.Search, psTemp);
                selection.LinkURL = "1";
                selections.Add(selection);
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
                //var selectedAttrRangeValues_ = new Dictionary<int, string>();
                //foreach (var value in selectedAttrRangeValues)
                //{
                //    if (value.Key == attrTitleID)//去掉当前attribute
                //    {
                //        continue;
                //    }
                //    selectedAttrRangeValues_.Add(value.Key, value.Value);
                //}
                //Dictionary<string, string> psTemp = UrlController.GetCatalogParameters(categoryID, this.brandIDList,
                //    priceRangeString, selectedAttributeIDs, selectedAttributeRangeIDs, this.sortBy, selectedRetailers, this.v, selectedAttrRangeValues_);
                //psTemp.Add("q", queryKeywords);
                //selection.LinkURL = UrlController.GetRewriterUrl(PageName.Search, psTemp);
                selection.LinkURL = "1";
                selections.Add(selection);
            }
            if (!string.IsNullOrEmpty(swi))
            {
                LinkInfo selection = new LinkInfo();
                selection.LinkText = "\"" + swi.Replace("-", " ") + "\"";
                //Dictionary<string, string> psTemp = new Dictionary<string, string>(currentParameters);
                //psTemp.Remove("swi");
                //selection.LinkURL = UrlController.GetRewriterUrl(PageName.Search, psTemp);

                selections.Add(selection);
            }

            if (!string.IsNullOrEmpty(daysRangeString))
            {
                LinkInfo selection = new LinkInfo();
                selection.LinkText = daysRangeString + " " + Resources.Resource.TextString_days;
                //Dictionary<string, string> psTemp = new Dictionary<string, string>(currentParameters);
                //psTemp.Remove("dr");
                //selection.LinkURL = UrlController.GetRewriterUrl(PageName.Search, psTemp);

                selections.Add(selection);
            }

            if (categoryID != 0)
            {
                CategoryCache category = CategoryController.GetCategoryByCategoryID(categoryID, WebConfig.CountryId);
                if (category != null)
                {
                    LinkInfo selection = new LinkInfo();
                    selection.LinkText = category.CategoryName;
                    Dictionary<string, string> psTemp = new Dictionary<string, string>(currentParameters);
                    psTemp.Remove("c");
                    selection.LinkURL = UrlController.GetRewriterUrl(PageName.Search, psTemp);
                    selections.Add(selection);
                }
            }

            return selectedAttributeTitleIDs;
        }

        private string GetPriceRangeString(PriceRange priceRange)
        {
            return Utility.FormatPriceForPriceFilter(priceRange.MinPrice)
                    + "-" + Utility.FormatPriceForPriceFilter(priceRange.MaxPrice);
        }

        public string GetDeleteAllUrl()
        {
            var np = new Dictionary<string, string>();

            np.Add("q", currentParameters["q"]);

            if (!string.IsNullOrEmpty(v) && v != DefaultView)
                np.Add("v", v);

            if (!string.IsNullOrEmpty(sortBy) && sortBy != Static_DefaultSortBy)
                np.Add("sb", sortBy);

            return UrlController.GetRewriterUrl(PageName.Search, np);
        }

        void LoadCrumbs()
        {
            List<LinkInfo> linkInfoList = new List<LinkInfo>();

            BreadCrumbInfo breadCrumbInfo = Utility.GetBreadCrumbInfo(linkInfoList, EncodeSW);
            breadCrumbInfo.CurrentPageId = Page.Request.Url.PathAndQuery;
            breadCrumbInfo.CurrentPageKey = "search";

            Master.SetBreadCrumb(breadCrumbInfo);

            Master.InitATag(string.Format(Resources.Resource.TextString_Checkout, this.Title));
        }

        private void SetBanner()
        {
            Master.SetSpaceID(0);
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