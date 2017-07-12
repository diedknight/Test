using PriceMe;
using PriceMeCache;
using PriceMeCommon;
using PriceMeCommon.BusinessLogic;
using PriceMeCommon.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HotterWinds
{
    public partial class CatalogAjax : System.Web.UI.Page
    {
        static int Static_DefaultPageSize = 48;

        int categoryID;
        int currentPage;
        string queryKeywords;
        int pageSize;
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
        string pageTo;
        string pageSizeString;
        string defaultView;
        string defaultSortBy;
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
        ProductSearcher productSeacherWithoutFilters;
        Dictionary<string, string> removeUrlDic;
        DaysRange daysRange;
        PageName pageToName;

        protected string currentUrl;
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

        protected CatalogPageInfo catalogPageInfo;

        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (GetParameters())
            {
                SetValue();
                SetControlsValue();
                BuilderSelections();
                SetProductFilters();
            }
        }

        private bool GetParameters()
        {
            categoryID = Utility.GetIntParameter("c");
            queryKeywords = Utility.GetParameter("q");
            manufeaturerIDs = Utility.GetParameter("m");
            attributeValuesIDString = Utility.GetParameter("avs");
            attributeValuesRangeString = Utility.GetParameter("avsr");
            swi = Utility.GetParameter("swi");
            priceRangeString = Utility.GetParameter("pr").Replace(", ", "").Replace(".", "");
            currentPage = Utility.GetIntParameter("pg");
            sortBy = Utility.GetParameter("sb");
            retailerIDs = Utility.GetParameter("pcsid");
            adm = Utility.GetParameter("adm");
            daysRangeString = Utility.GetParameter("dr");
            v = Utility.GetParameter("v");
            displayAllManufeaturerIDString = Utility.GetParameter("samp");
            pageTo = Utility.GetParameter("pt");
            pageSize = Utility.GetIntParameter("ps");
            defaultView = Utility.GetParameter("dv");
            defaultSortBy = Utility.GetParameter("dsb");
            onSaleOnly = Utility.GetParameter("os");

            if (categoryID == 0 && string.IsNullOrEmpty(queryKeywords))
            {
                return false;
            }

            return true;
        }

        private void SetValue()
        {
            category = CategoryController.GetCategoryByCategoryID(categoryID, WebConfig.CountryId);

            if (pageTo.Equals("catalog", StringComparison.InvariantCultureIgnoreCase))
            {
                pageToName = PageName.Catalog;
            }
            else
            {
                pageToName = PageName.Search;
            }

            if (pageSize == 0)
            {
                pageSize = Static_DefaultPageSize;
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

            if (!string.IsNullOrEmpty(adm))
            {
                isAdmin = true;
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

            List<int> avIDs = new List<int>(selectedAttributeIDs);
            foreach (int avgID in selectedAttributeGroupIDs)
            {
                List<int> avIDList = AttributesController.GetCatalogAttributeValues(avgID);
                if (avIDList != null)
                {
                    avIDs.AddRange(avIDList);
                }
            }

            catalogPageInfo = PriceMeCommon.BusinessLogic.CatalogProductSearchController.SearchProducts(this.categoryID, this.brandIDList, priceRange, avIDs, selectedAttributeRangeIDs, this.sortBy, swi + " " + queryKeywords, selectedRetailers, true, WebConfig.CountryId, false, selectedAttrRangeValues, true, currentPage, pageSize, daysRange, onSaleOnly);
            if (catalogPageInfo.PageCount < this.currentPage)
            {
                currentPage = catalogPageInfo.PageCount;
                catalogPageInfo = PriceMeCommon.BusinessLogic.CatalogProductSearchController.SearchProducts(this.categoryID, this.brandIDList, priceRange, avIDs, selectedAttributeRangeIDs, this.sortBy, swi + " " + queryKeywords, selectedRetailers, true, WebConfig.CountryId, false, selectedAttrRangeValues, true, currentPage, pageSize, daysRange, onSaleOnly);
            }
            Utility.FixProductCatalogList(catalogPageInfo.ProductCatalogList, v);

            currentParameters = UrlController.GetCatalogParameters(this.categoryID, this.brandIDList, priceRangeString,
            selectedAttributeIDs, selectedAttributeRangeIDs, this.sortBy, selectedRetailers, v, swi,
            this.displayAllManufeaturerIDString, selectedAttrRangeValues, selectedAttributeGroupIDs, daysRangeString, onSaleOnly);

            if (pageToName == PageName.Search && !string.IsNullOrEmpty(queryKeywords))
            {
                currentParameters.Add("q", queryKeywords);
                if (categoryID == 0)
                {
                    currentParameters.Remove("c");
                }
            }

            if (category != null && currentParameters.ContainsKey("m") && !category.IsFilterByBrand)
            {
                currentParameters.Remove("m");
            }

            Dictionary<string, string> newPS = new Dictionary<string, string>(currentParameters);
            if (currentPage > 1)
            {
                newPS.Add("pg", currentPage.ToString());
            }
            if (sortBy.Equals(defaultSortBy, StringComparison.InvariantCultureIgnoreCase))
            {
                newPS.Remove("sb");
            }
            if (v.Equals(defaultView, StringComparison.InvariantCultureIgnoreCase))
            {
                newPS.Remove("v");
            }

            currentUrl = UrlController.GetRewriterUrl(pageToName, newPS);

            SetSortAndViewInfo(newPS);

            //不含过滤条件的ProductSearcher
            string kw = "";
            if (pageToName == PageName.Search)
            {
                kw = queryKeywords;
            }
            productSeacherWithoutFilters = new ProductSearcher(kw, this.categoryID, null, null, null, null, "clicks", null, SearchController.MaxSearchCount_Static, WebConfig.CountryId, false, true, false, true, null, "", null);
        }

        void SetControlsValue()
        {
            if (NewCatalogProducts1 != null)
            {
                if (category != null)
                {
                    List<CategoryCache> ccList = CategoryController.GetBreadCrumbCategoryList(category, WebConfig.CountryId);
                    int rootCategoryID = ccList[ccList.Count - 1].CategoryID;
                    int ccid = rootCategoryID;
                    if (ccList.Count > 1)
                    {
                        ccid = ccList[ccList.Count - 2].CategoryID;
                    }

                    NewCatalogProducts1.RootCategoryID = rootCategoryID;
                    NewCatalogProducts1.IsFilterByBrand = category.IsFilterByBrand;
                }

                NewCatalogProducts1.ShowCompareBox = true;

                NewCatalogProducts1.View = v;


                if (!v.Equals("quick", StringComparison.InvariantCultureIgnoreCase))
                {
                    List<string> popularTop3 = productSeacherWithoutFilters.GetTop3Products();
                    SetTop3(catalogPageInfo.ProductCatalogList, popularTop3);
                    NewCatalogProducts1.ProductCatalogList = catalogPageInfo.ProductCatalogList;
                }
                else
                {
                    if (isAdmin)
                    {
                        NewCatalogProducts1.IsAdmin = true;
                        NewCatalogProducts1.AttributesInfo = catalogPageInfo.MyProductSearcher.GetAttributesResulte_New(null);
                    }
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


            if (!v.Equals("quick", StringComparison.InvariantCultureIgnoreCase))
            {
                this.PrettyPager1.PageTo = pageToName;
                this.PrettyPager1.totalPages = catalogPageInfo.PageCount;
                this.PrettyPager1.currentPage = this.currentPage;
                this.PrettyPager1.ps = new Dictionary<string, string>(currentParameters);
                this.PrettyPager1.SetPaginationHeader();
                this.PrettyPager1.PageSize = pageSize;
                this.PrettyPager1.ItemCount = catalogPageInfo.CurrentProductCount;
                this.PrettyPager1.DisplayItemCountInfo = true;
            }
            else
            {
                PrettyPager1.IsDisplay = false;
            }
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

        private void SetSortAndViewInfo(Dictionary<string, string> cp)
        {
            Dictionary<string, string> currentParameters = new Dictionary<string, string>(cp);

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

            //string titleKey = "Title";
            //psTemp.Remove(qName);
            //psTemp.Add(qName, titleKey);
            //LinkInfo titleInfo = new LinkInfo();
            //titleInfo.Value = titleKey;
            //titleInfo.LinkText = Resources.Resource.TextString_ProductNameAZ;
            //titleInfo.LinkURL = UrlController.GetRewriterUrl(pageToName, psTemp);
            //sortByInfoList.Add(titleInfo);

            //string onSaleKey = "Sale";
            //psTemp.Remove(qName);
            //psTemp.Add(qName, onSaleKey);
            //LinkInfo onSaleInfo = new LinkInfo();
            //onSaleInfo.Value = onSaleKey;
            //onSaleInfo.LinkText = Resources.Resource.TextString_OnSale;
            //onSaleInfo.LinkURL = UrlController.GetRewriterUrl(pageToName, psTemp);
            //sortByInfoList.Add(onSaleInfo);

            Dictionary<string, string> psTemp2 = new Dictionary<string, string>(cp);

            psTemp2.Remove("v");
            if (!defaultView.Equals("Grid", StringComparison.InvariantCultureIgnoreCase))
            {
                psTemp2.Add("v", "Grid");
            }
            GridViewUrl = UrlController.GetRewriterUrl(pageToName, psTemp2);

            psTemp2.Remove("v");
            if (!defaultView.Equals("List", StringComparison.InvariantCultureIgnoreCase))
            {
                psTemp2.Add("v", "List");
            }
            ListViewUrl = UrlController.GetRewriterUrl(pageToName, psTemp2);

            psTemp2.Remove("v");
            if (!defaultView.Equals("Quick", StringComparison.InvariantCultureIgnoreCase))
            {
                psTemp2.Add("v", "Quick");
            }
            psTemp2.Remove("pg");
            QuickViewUrl = UrlController.GetRewriterUrl(pageToName, psTemp2);
        }

        private List<int> BuilderSelections()
        {
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
                        selections.Add(selection);
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
                        selections.Add(selection);
                    }
                }
            }

            if (priceRange != null)
            {
                LinkInfo selection = new LinkInfo();
                selection.LinkText = Utility.GetPriceRangeString(priceRange);
                selections.Add(selection);
            }

            if (onSaleOnly == "1")
            {
                LinkInfo selection = new LinkInfo();
                selection.LinkText = "On sale only";

                selections.Add(selection);
            }

            //
            foreach (int avgid in selectedAttributeGroupIDs)
            {
                LinkInfo selection = new LinkInfo();
                selection.LinkText = AttributesController.GetCatalogAttributeGroupName(avgid);

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
                selections.Add(selection);
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

                selections.Add(selection);
            }
            if (!string.IsNullOrEmpty(swi))
            {
                LinkInfo selection = new LinkInfo();
                selection.LinkText = "\"" + swi.Replace("-", " ") + "\"";
                selections.Add(selection);
            }

            if (!string.IsNullOrEmpty(daysRangeString))
            {
                LinkInfo selection = new LinkInfo();
                selection.LinkText = daysRangeString + " " + Resources.Resource.TextString_days;

                selections.Add(selection);
            }

            if (pageToName == PageName.Search && categoryID != 0)
            {
                CategoryCache category = CategoryController.GetCategoryByCategoryID(categoryID, WebConfig.CountryId);
                if (category != null)
                {
                    LinkInfo selection = new LinkInfo();
                    selection.LinkText = category.CategoryName;

                    selections.Add(selection);
                }
            }

            return selectedAttributeTitleIDs;
        }

        private void SetProductFilters()
        {

            totalProductCount = productSeacherWithoutFilters.GetProductCount();
            List<string> popularTop3Products = productSeacherWithoutFilters.GetTop3Products();

            List<NarrowByInfo> narrowByInfoList = new List<NarrowByInfo>();

            if (pageToName == PageName.Catalog)
            {
                NarrowByInfo categoryNarrowByInfo = productSeacherWithoutFilters.GetCatalogCategoryResulte();
                if (categoryNarrowByInfo.NarrowItemList.Count > 0)
                {
                    AddNarrowByCatalogyUrl(categoryNarrowByInfo.NarrowItemList);
                    narrowByInfoList.Add(categoryNarrowByInfo);
                }
            }
            else
            {
                if (categoryID == 0)
                {
                    NarrowByInfo categoryNarrowByInfo = catalogPageInfo.MyProductSearcher.GetSearchCategoryResulte();
                    if (categoryNarrowByInfo.NarrowItemList.Count > 0)
                    {
                        AddNarrowBySearchCatalogyUrl(categoryNarrowByInfo.NarrowItemList);
                        narrowByInfoList.Add(categoryNarrowByInfo);
                    }
                }
            }

            //brands
            if (category == null || (category != null && !CategoryController.IsHiddenBrandsCategoryID(category.CategoryID, WebConfig.CountryId)))
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
                priceRangeNarrowByInfo.ProductCountListWithoutP = priceRangeNarrowByInfo.NarrowItemList.Select(ni => ni.ProductCount).ToList();
                double priceRangeMaxValue = double.Parse(priceRangeNarrowByInfo.NarrowItemList[priceRangeNarrowByInfo.NarrowItemList.Count - 1].Value);

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

                NarrowByInfo onSaleOnlyNarrowByInfoWithP = catalogPageInfo.MyProductSearcher.GetOnSaleOnlyResulte();
                CatalogProductSearchController.FixFiltersProductCount(onSaleOnlyNarrowByInfo, onSaleOnlyNarrowByInfoWithP);

                narrowByInfoList.Add(onSaleOnlyNarrowByInfo);
            }

            //attributes
            //获取分类所包含的所有Attributes.
            List<PriceMeCommon.Data.NarrowByInfo> attributesNarrowByInfoList = productSeacherWithoutFilters.GetAttributesResulte_New(selectedAttrRangeValues);
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
            List<NarrowByInfo> attributesNarrowByInfoListWithP = catalogPageInfo.MyProductSearcher.GetAttributesResulte_New(selectedAttrRangeValues);
            CatalogProductSearchController.FixAttributesNarrowByInfoProductCount(attributesNarrowByInfoList, attributesNarrowByInfoListWithP);
            narrowByInfoList.AddRange(attributesNarrowByInfoList);

            //Product Name
            NarrowByInfo pnameNarrowByInfo = new NarrowByInfo();
            pnameNarrowByInfo.Title = Resources.Resource.TextString_ProductName;
            narrowByInfoList.Add(pnameNarrowByInfo);

            //Days on PriceMe
            NarrowByInfo daysNarrowByInfo = productSeacherWithoutFilters.GetDaysOnPriceMeResult();
            if (daysNarrowByInfo.NarrowItemList.Count > 0)
            {
                daysNarrowByInfo.Title = Resources.Resource.TextString_DaysOnPriceMe;
                daysNarrowByInfo.ProductCountListWithoutP = Utility.GetDaysProductCountList(daysNarrowByInfo);

                NarrowByInfo daysNarrowByInfoWithP = catalogPageInfo.MyProductSearcher.GetDaysOnPriceMeResult();
                CatalogProductSearchController.FixFiltersProductCount(daysNarrowByInfo, daysNarrowByInfoWithP);
                narrowByInfoList.Add(daysNarrowByInfo);
            }

            //retailers
            NarrowByInfo retailerNarrowByInfo = productSeacherWithoutFilters.GetRetailerResulte();
            if (retailerNarrowByInfo.NarrowItemList.Count > 0)
            {
                retailerNarrowByInfo.Title = Resources.Resource.TextString_Retailers;
                NarrowByInfo retailerNarrowByInfoWithP = catalogPageInfo.MyProductSearcher.GetRetailerResulte();
                CatalogProductSearchController.FixFiltersProductCount(retailerNarrowByInfo, retailerNarrowByInfoWithP);

                narrowByInfoList.Add(retailerNarrowByInfo);
            }

            //UrlController.SetNarrowByInfoUrl(narrowByInfoList, pageToName, currentParameters, selectedAttributeGroupIDs);

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
                                    narrowItemGroup.GroupUrl = UrlController.GetRewriterUrl(pageToName, psTemp);
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

            if (NewFilters1 != null)
            {
                this.NewFilters1.NarrowByInfoList = narrowByInfoList;
                this.NewFilters1.CurrentProductCount = catalogPageInfo.CurrentProductCount;
                this.NewFilters1.TotalProductCount = totalProductCount;
                this.NewFilters1.RemoveUrlDic = removeUrlDic;
                this.NewFilters1.MyPriceRange = priceRange;
                this.NewFilters1.SearchWithIn = swi;
                this.NewFilters1.MyDaysRange = daysRange;
                this.NewFilters1.Selections = selections;
                this.NewFilters1.CategoryID = categoryID;
                this.NewFilters1.SortBy = sortBy;
                this.NewFilters1.View = v;
                this.NewFilters1.IsAjax = true;
                this.NewFilters1.DefaultView = defaultView;
                this.NewFilters1.DefaultSortBy = defaultSortBy;
                this.NewFilters1.UrlNoSelection = GetDeleteAllUrl();
            }
        }

        private void AddNarrowByCatalogyUrl(List<NarrowItem> narrowItemList)
        {
            foreach (var ni in narrowItemList)
            {
                ni.Url = UrlController.GetCatalogUrl(int.Parse(ni.Value));
            }
        }

        private void AddNarrowBySearchCatalogyUrl(List<NarrowItem> narrowItemList)
        {
            var np = new Dictionary<string, string>();
            np.Add("q", currentParameters["q"]);
            np.Add("c", "0");
            foreach (var ni in narrowItemList)
            {
                np.Remove("c");
                np.Add("c", ni.Value);
                ni.Url = UrlController.GetRewriterUrl(PageName.Search, np);
            }
        }

        public string GetDeleteAllUrl()
        {
            var np = new Dictionary<string, string>();

            if (pageToName == PageName.Catalog)
            {
                np.Add("c", currentParameters["c"]);

                if (!string.IsNullOrEmpty(v) && v != defaultView)
                    np.Add("v", v);

                if (!string.IsNullOrEmpty(sortBy) && sortBy != defaultSortBy)
                    np.Add("sb", sortBy);
            }
            else
            {
                np.Add("q", currentParameters["q"]);

                if (!string.IsNullOrEmpty(v) && v != defaultView)
                    np.Add("v", v);

                if (!string.IsNullOrEmpty(sortBy) && sortBy != defaultSortBy)
                    np.Add("sb", sortBy);
            }

            return UrlController.GetRewriterUrl(pageToName, np);
        }
    }
}