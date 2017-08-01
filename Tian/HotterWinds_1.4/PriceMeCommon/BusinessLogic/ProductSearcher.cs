using Lucene.Net.Documents;
using PriceMeCommon.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceMeCommon.BusinessLogic
{
    public class ProductSearcher
    {
        int _categoryId;
        List<int> _brandIds;
        PriceRange _priceRange;
        List<int> _selectedAttributeIds;
        List<int> _selectedAttributeRangeIds;
        Dictionary<int, string> _selectedAttributeValueRanges;
        string _sortby;
        string _keywords;
        List<int> _retailerIds;
        int _countryId;
        bool _includeAccessoies = true;
        List<int> _categoryIdList;
        bool _ppcOnly = false;

        //
        HitsInfo _resultHitsInfo;
        List<string> _resultProductIDs = null;
        Dictionary<string, int> _attributeProductCountDictionary = null;
        Dictionary<int, int> _attrValueProductCountDictionary = null;
        List<int> selectAttributeTitle = new List<int>();

        private void Init(string keywords, int categoryID, List<int> manufacturerIDs, PriceRange priceRange, List<int> attributeValueIDList, List<int> attributeRangeIDList, string sortby, List<int> retailerIDs, int maxDocCount, int countryId, bool multiAttribute, bool includeAccessories, bool ppcOnly)
        {
            this._keywords = keywords;
            this._categoryId = categoryID;
            this._brandIds = manufacturerIDs;
            this._priceRange = priceRange;
            this._selectedAttributeIds = attributeValueIDList;
            this._sortby = sortby;
            this._selectedAttributeRangeIds = attributeRangeIDList;
            this._keywords = keywords;
            this._retailerIds = retailerIDs;
            this._countryId = countryId;
            this._includeAccessoies = includeAccessories;
            this._ppcOnly = ppcOnly;
        }

        public static List<ProductCatalog> SearchByProductIDs(int[] productIDs, int countryID)
        {
            return SearchController.SearchProductsByPIDs(productIDs, countryID);
        }

        /// <summary>
        /// All
        /// </summary>
        /// <param name="keywords"></param>
        /// <param name="categoryIDs"></param>
        /// <param name="manufacturerIDs"></param>
        /// <param name="priceRange"></param>
        /// <param name="attributeValueIDList"></param>
        /// <param name="attributeRangeIDList"></param>
        /// <param name="sortby"></param>
        /// <param name="retailerIDs"></param>
        /// <param name="maxDocCount"></param>
        /// <param name="countryID"></param>
        /// <param name="multiAttribute"></param>
        /// <param name="includeAccessories"></param>
        /// <param name="ppcOnly"></param>
        /// <param name="isSearchonly"></param>
        /// <param name="daysRange"></param>
        /// <param name="onSaleOnly"></param>
        public ProductSearcher(string keywords, List<int> categoryIDs, List<int> manufacturerIDs, PriceRange priceRange, List<int> attributeValueIDList, List<int> attributeRangeIDList, string sortby, List<int> retailerIDs, int maxDocCount, int countryId, bool multiAttribute, bool includeAccessories, bool ppcOnly, bool useIsSearchonly, DaysRange daysRange, string onSaleOnly, Dictionary<int, string> attrRanges)
        {
            int categoryID = 0;
            if (categoryIDs != null && categoryIDs.Count > 0)
            {
                categoryID = categoryIDs[0];
            }
            Init(keywords, categoryID, manufacturerIDs, priceRange, attributeValueIDList, attributeRangeIDList, sortby, retailerIDs, maxDocCount, countryId, multiAttribute, includeAccessories, ppcOnly);

            _resultHitsInfo = SearchController.SearchProducts(this._keywords, categoryIDs, this._brandIds, this._priceRange, this._selectedAttributeIds, this._selectedAttributeRangeIds, this._sortby, this._retailerIds, maxDocCount, countryId, multiAttribute, includeAccessories, ppcOnly, attrRanges, useIsSearchonly, daysRange, onSaleOnly);
        }

        public ProductSearcher(string keywords, int categoryId, List<int> manufacturerIDs, PriceRange priceRange, List<int> attributeValueIDList, List<int> attributeRangeIDList, string sortby, List<int> retailerIDs, int maxDocCount, int countryId, bool multiAttribute, bool includeAccessories, bool ppcOnly, bool useIsSearchonly, DaysRange daysRange, string onSaleOnly, Dictionary<int, string> attrRanges)
        {
            Init(keywords, categoryId, manufacturerIDs, priceRange, attributeValueIDList, attributeRangeIDList, sortby, retailerIDs, maxDocCount, countryId, multiAttribute, includeAccessories, ppcOnly);

            List<int> cidList = new List<int>();
            cidList.Add(categoryId);

            _resultHitsInfo = SearchController.SearchProducts(this._keywords, cidList, this._brandIds, this._priceRange, this._selectedAttributeIds, this._selectedAttributeRangeIds, this._sortby, this._retailerIds, maxDocCount, countryId, multiAttribute, includeAccessories, ppcOnly, attrRanges, useIsSearchonly, daysRange, onSaleOnly);
        }

        public static int GetCategoryProductCount(int categoryId, bool useSearchOnly, int countryId)
        {
            List<int> cIdList = new List<int>();
            cIdList.Add(categoryId);
            HitsInfo hitsInfo = SearchController.SearchProducts("", cIdList, null, null, null, null, "", null, int.MaxValue, countryId, false, true, false, null, useSearchOnly, null, "");
            if (hitsInfo != null)
            {
                return hitsInfo.ResultCount;
            }
            return 0;
        }

        public int GetProductCount()
        {
            if (_resultHitsInfo != null)
            {
                return _resultHitsInfo.ResultCount;
            }
            return 0;
        }

        public SearchResult GetSearchResult(int pageIndex, int pageSize)
        {
            SearchResult searchResult = new SearchResult();
            if (_resultHitsInfo == null || _resultHitsInfo.ResultCount == 0 || pageSize == 0)
            {
                return searchResult;
            }

            PagerInfo pagerInfo = new PagerInfo(pageIndex, pageSize, _resultHitsInfo.ResultCount);

            List<ProductCatalog> productCatalogList = new List<ProductCatalog>();
            for (int i = pagerInfo.StartIndex; i < pagerInfo.EndIndex; i++)
            {
                ProductCatalog productCatalog = SearchController.GetProductCatalog(_resultHitsInfo, i);
                if (productCatalog != null)
                {
                    productCatalogList.Add(productCatalog);
                }
            }

            searchResult.CurrentPageIndex = pagerInfo.CurrentPageIndex;
            searchResult.ProductCatalogList = productCatalogList;
            searchResult.PageCount = pagerInfo.PageCount;
            searchResult.ProductCount = _resultHitsInfo.ResultCount;

            return searchResult;
        }

        public ProductCatalog GetProductCatalog(int position)
        {
            if (_resultHitsInfo == null || _resultHitsInfo.ResultCount == 0 || position < 0 || position >= _resultHitsInfo.ResultCount)
            {
                return null;
            }

            ProductCatalog productCatalog = SearchController.GetProductCatalog(_resultHitsInfo, position);

            return productCatalog;
        }

        /// <summary>
        /// for geekzone
        /// </summary>
        /// <param name="allowCategoryIDs">要返回的分类的ID</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public SearchResult GetSearchResult(List<int> allowCategoryIDs, int pageIndex, int pageSize)
        {
            SearchResult searchResult = new SearchResult();
            if (_resultHitsInfo == null || _resultHitsInfo.ResultCount == 0 || pageSize == 0)
            {
                return searchResult;
            }

            PagerInfo pagerInfo = new PagerInfo(pageIndex, pageSize, _resultHitsInfo.ResultCount);

            List<ProductCatalog> productCatalogList = new List<ProductCatalog>();
            for (int i = pagerInfo.StartIndex; i < pagerInfo.EndIndex; i++)
            {
                ProductCatalog productCatalog = SearchController.GetProductCatalog(_resultHitsInfo, i);
                productCatalogList.Add(productCatalog);
            }

            searchResult.CurrentPageIndex = pagerInfo.CurrentPageIndex;
            searchResult.ProductCatalogList = productCatalogList;
            searchResult.PageCount = pagerInfo.PageCount;
            searchResult.ProductCount = _resultHitsInfo.ResultCount;

            return searchResult;
        }

        /// <summary>
        /// Get deals count
        /// </summary>
        /// <param name="allowCategoryIDs"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static int GetDealsCount(float minimumPrice, float saleRate, int countryId)
        {
            int count = 0;
            PriceRange range = new PriceRange(minimumPrice, 1000000d);
            Lucene.Net.Search.Searcher searcher = MultiCountryController.GetAllCategoryProductsIndexSearcher(countryId);
            for (int i = 0; i < searcher.MaxDoc; i++)
            {
                ProductCatalog productCatalog = SearchController.GetProductCatalogFromDoc(searcher.Doc(i));
                float bp = float.Parse(productCatalog.BestPrice);
                float sale = -productCatalog.Sale;
                if (bp >= minimumPrice && sale >= saleRate)
                    count++;
            }

            return count;
        }

        public NarrowByInfo GetMatchCategoryResulte()
        {
            NarrowByInfo narrowByInfo = new NarrowByInfo();
            narrowByInfo.ListOrder = 0;
            narrowByInfo.Title = "Match Categories";
            narrowByInfo.Name = "Catalog Category";

            HitsInfo hitsInfo = SearchController.SearchCategories(this._keywords, this._countryId);

            if (hitsInfo != null && hitsInfo.ResultCount > 0)
            {
                string[] fieldNameArray = new string[] { "CategoryID", "CategoryName" };
                List<NarrowItem> narrowList = new List<NarrowItem>();
                int count = hitsInfo.ResultCount;
                for (int i = 0; i < count; i++)
                {
                    Document doc = hitsInfo.GetDocument(i, fieldNameArray);
                    NarrowItem narrowItem = new NarrowItem();
                    narrowItem.DisplayName = doc.Get("CategoryName");
                    narrowItem.Value = doc.Get("CategoryID");
                    narrowItem.ProductCount = CategoryController.GetCategoryProductCount(int.Parse(narrowItem.Value), this._countryId);
                    narrowList.Add(narrowItem);
                }
                narrowByInfo.NarrowItemList = narrowList;
            }

            return narrowByInfo;
        }

        public NarrowByInfo GetSearchCategoryResulte()
        {
            NarrowByInfo narrowByInfo = new NarrowByInfo();
            if (_resultHitsInfo == null || _resultHitsInfo.ResultCount == 0)
            {
                return narrowByInfo;
            }

            narrowByInfo.ListOrder = 1;
            narrowByInfo.Title = "Categories";
            narrowByInfo.Name = "Search Category";

            if (_resultHitsInfo == null || _resultHitsInfo.ResultCount == 0)
            {
                return narrowByInfo;
            }

            Dictionary<string, int> categoryProductCountDictionary = GetProductCountDictionary(_resultHitsInfo, "CategoryID");

            List<NarrowItem> narrowList = new List<NarrowItem>();
            foreach (string categoryID in categoryProductCountDictionary.Keys)
            {
                PriceMeCache.CategoryCache category = CategoryController.GetCategoryByCategoryID(int.Parse(categoryID), _countryId);
                if (category == null)
                {
                    continue;
                }

                NarrowItem narrowItem = new NarrowItem();
                narrowItem.DisplayName = category.CategoryName;
                narrowItem.Value = categoryID;
                narrowItem.ProductCount = categoryProductCountDictionary[categoryID];
                narrowList.Add(narrowItem);
            }
            narrowByInfo.NarrowItemList = narrowList;

            return narrowByInfo;
        }

        public NarrowByInfo GetDaysOnPriceMeResult()
        {
            NarrowByInfo narrowByInfo = new NarrowByInfo();
            if (_resultHitsInfo == null || _resultHitsInfo.ResultCount == 0)
            {
                return narrowByInfo;
            }

            Dictionary<string, int> productCountDictionary = GetProductCountDictionary(_resultHitsInfo, "DayCount");

            narrowByInfo.Name = "Days";
            narrowByInfo.IsSlider = true;

            int maxDays = 0;
            foreach (string ds in productCountDictionary.Keys)
            {
                int d = int.Parse(ds);
                if (d == int.MaxValue)
                    continue;
                if (maxDays < d)
                {
                    maxDays = d;
                }
            }

            List<NarrowItem> narrowList = new List<NarrowItem>();
            for (int i = 1; i <= maxDays; i++)
            {
                NarrowItem narrowItem = new NarrowItem();
                narrowItem.DisplayName = i.ToString();
                narrowItem.Value = narrowItem.DisplayName;
                int pc = 0;
                productCountDictionary.TryGetValue(i.ToString(), out pc);
                narrowItem.ProductCount = pc;
                narrowList.Add(narrowItem);
            }
            narrowByInfo.NarrowItemList = narrowList;
            return narrowByInfo;
        }

        public NarrowByInfo GetOnSaleOnlyResulte()
        {
            NarrowByInfo narrowByInfo = new NarrowByInfo();
            if (_resultHitsInfo == null || _resultHitsInfo.ResultCount == 0)
            {
                return narrowByInfo;
            }

            Dictionary<string, int> productCountDictionary = GetProductCountDictionary(_resultHitsInfo, "DisplaySaleTag");

            narrowByInfo.Name = "On sale";
            narrowByInfo.IsSlider = false;

            if (productCountDictionary.ContainsKey("1"))
            {
                List<NarrowItem> narrowList = new List<NarrowItem>();

                NarrowItem narrowItem = new NarrowItem();
                narrowItem.DisplayName = "On sale only";
                narrowItem.Value = "1";
                narrowItem.ProductCount = productCountDictionary["1"];
                narrowList.Add(narrowItem);

                narrowByInfo.NarrowItemList = narrowList;
            }
            return narrowByInfo;
        }

        public NarrowByInfo GetCatalogCategoryResulte()
        {
            NarrowByInfo narrowByInfo = new NarrowByInfo();
            if (_resultHitsInfo == null || _resultHitsInfo.ResultCount == 0)
            {
                return narrowByInfo;
            }

            List<PriceMeCache.CategoryCache> nextLevelSubCategories = CategoryController.GetNextLevelSubCategories(this._categoryId, this._countryId);
            if (nextLevelSubCategories == null || nextLevelSubCategories.Count == 0)
            {
                return narrowByInfo;
            }

            narrowByInfo.ListOrder = 1;
            narrowByInfo.Title = "Categories";
            narrowByInfo.Name = "Catalog Category";

            List<NarrowItem> narrowList = new List<NarrowItem>();

            foreach (PriceMeCache.CategoryCache category in nextLevelSubCategories)
            {
                if (CategoryController.IsSearchOnly(category.CategoryID, this._countryId))
                    continue;

                NarrowItem narrowItem = new NarrowItem();
                narrowItem.DisplayName = category.CategoryName;
                narrowItem.IsPopular = category.PopularCategory;
                narrowItem.Value = category.CategoryID.ToString();
                narrowItem.ProductCount = CategoryController.GetCategoryProductCount(category.CategoryID, this._countryId);
                if (narrowItem.ProductCount == 0)
                    continue;
                narrowList.Add(narrowItem);
            }
            narrowByInfo.NarrowItemList = narrowList;

            return narrowByInfo;
        }

        public int GetClickCount()
        {
            List<NarrowItem> narrowItemList = new List<NarrowItem>();
            if (_resultHitsInfo == null || _resultHitsInfo.ResultCount == 0)
            {
                return 0;
            }

            int count = 0;
            for (int j = 0; j < _resultHitsInfo.ResultCount; j++)
            {
                var doc = _resultHitsInfo.GetDocument(j, new string[] { "Clicks" });

                int clicks = int.Parse(doc.Get("Clicks"));

                count += clicks;
            }
            return count;
        }

        public List<NarrowItem> GetRelatedCategoryManufacturerResulte()
        {
            List<NarrowItem> narrowItemList = new List<NarrowItem>();
            if (_resultHitsInfo == null || _resultHitsInfo.ResultCount == 0)
            {
                return narrowItemList;
            }

            Dictionary<string, NarrowItem> narrowItemDictionary = new Dictionary<string, NarrowItem>();
            for (int j = 0; j < _resultHitsInfo.ResultCount; j++)
            {
                var doc = _resultHitsInfo.GetDocument(j, new string[] { "Clicks", "ManufacturerID" });

                string manufacturerID = doc.Get("ManufacturerID");
                int clicks = int.Parse(doc.Get("Clicks"));

                if (narrowItemDictionary.ContainsKey(manufacturerID))
                {
                    NarrowItem narrowItem = narrowItemDictionary[manufacturerID];
                    narrowItem.ListOrder += clicks;
                    narrowItem.ProductCount += 1;
                }
                else
                {
                    NarrowItem narrowItem = new NarrowItem();
                    narrowItem.DisplayName = doc.Get("ManufacturerID");
                    narrowItem.Value = manufacturerID;
                    narrowItem.ProductCount = 1;
                    narrowItem.ListOrder = clicks;
                    narrowItemDictionary.Add(manufacturerID, narrowItem);
                }
            }

            narrowItemList = narrowItemDictionary.Values.ToList();
            return narrowItemList;
        }

        public NarrowByInfo GetManufacturerResulte()
        {
            NarrowByInfo narrowByInfo = new NarrowByInfo();
            if (_resultHitsInfo == null || _resultHitsInfo.ResultCount == 0)
            {
                return narrowByInfo;
            }

            narrowByInfo.ListOrder = 2;
            narrowByInfo.Title = "Brands";
            narrowByInfo.Name = "Manufacturer";

            Dictionary<string, int> manufacturerProductDictionary = GetProductCountDictionary(_resultHitsInfo, "ManufacturerID");

            List<NarrowItem> narrowListTop = new List<NarrowItem>();
            List<NarrowItem> narrowListMore = new List<NarrowItem>();
            int moreFlag = 8;
            int i = 0;
            foreach (string manufacturerID in manufacturerProductDictionary.Keys)
            {
                ManufacturerInfo manufacturer = ManufacturerController.GetManufacturerByID(int.Parse(manufacturerID), this._countryId);
                if (manufacturer == null)
                {
                    continue;
                }

                i++;
                NarrowItem narrowItem = new NarrowItem();
                narrowItem.DisplayName = manufacturer.ManufacturerName;
                narrowItem.Value = manufacturerID;
                narrowItem.ProductCount = manufacturerProductDictionary[manufacturerID];
                if (i < moreFlag)
                {
                    narrowListTop.Add(narrowItem);
                }
                else
                {
                    narrowListMore.Add(narrowItem);
                }
            }
            narrowListMore = narrowListMore.OrderBy(nar => nar.DisplayName).ToList();
            narrowByInfo.NarrowItemList.AddRange(narrowListTop);
            narrowByInfo.NarrowItemList.AddRange(narrowListMore);

            return narrowByInfo;
        }

        public NarrowByInfo GetTopManufacturerResulte(int topCount)
        {
            NarrowByInfo narrowByInfo = new NarrowByInfo();
            if (_resultHitsInfo == null || _resultHitsInfo.ResultCount == 0)
            {
                return narrowByInfo;
            }

            narrowByInfo.ListOrder = 2;
            narrowByInfo.Title = "TopBrands";
            narrowByInfo.Name = "TopManufacturer";

            Dictionary<string, int> manufacturerProductDictionary = GetProductCountDictionary(_resultHitsInfo, "ManufacturerID");

            string topBrandsString = CategoryController.GetCategoryByCategoryID(this._categoryId, this._countryId).TopBrands;
            List<NarrowItem> topManufacturers = new List<NarrowItem>();
            if (!string.IsNullOrEmpty(topBrandsString.Trim()))
            {
                string[] topBrandsID = topBrandsString.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string bidString in topBrandsID)
                {
                    if (topManufacturers.Count >= topCount) break;
                    if (manufacturerProductDictionary.ContainsKey(bidString) && bidString != "-1")
                    {
                        ManufacturerInfo manufacturer = ManufacturerController.GetManufacturerByID(int.Parse(bidString), _countryId);
                        if (manufacturer == null)
                        {
                            continue;
                        }

                        NarrowItem narrowItem = new NarrowItem();
                        narrowItem.DisplayName = manufacturer.ManufacturerName;
                        narrowItem.Value = bidString;
                        narrowItem.ProductCount = manufacturerProductDictionary[bidString];
                        topManufacturers.Add(narrowItem);
                        manufacturerProductDictionary.Remove(bidString);
                    }
                }
            }

            if (topManufacturers.Count < topCount)
            {
                foreach (string bidString in manufacturerProductDictionary.Keys)
                {
                    if (topManufacturers.Count >= topCount) break;
                    if (manufacturerProductDictionary.ContainsKey(bidString) && bidString != "-1")
                    {
                        ManufacturerInfo manufacturer = ManufacturerController.GetManufacturerByID(int.Parse(bidString), _countryId);
                        if (manufacturer == null)
                        {
                            continue;
                        }

                        NarrowItem narrowItem = new NarrowItem();
                        narrowItem.DisplayName = manufacturer.ManufacturerName;
                        narrowItem.Value = bidString;
                        narrowItem.ProductCount = manufacturerProductDictionary[bidString];
                        topManufacturers.Add(narrowItem);
                    }
                }
            }
            topManufacturers = topManufacturers.OrderBy(ni => ni.DisplayName).ToList();
            narrowByInfo.NarrowItemList = topManufacturers;

            return narrowByInfo;
        }

        public List<NarrowByInfo> GetAttributesResulte(Dictionary<int, string> selectedAttrRangeValues)
        {
            List<NarrowByInfo> narrowByInfoList = new List<NarrowByInfo>();
            List<PriceMeCache.AttributeTitleCache> attributeTitles = AttributesController.GetAttributesTitleByCategoryID(this._categoryId);
            if (attributeTitles == null)
            {
                return narrowByInfoList;
            }
            //获取attr products
            SetAttributeData();

            foreach (PriceMeCache.AttributeTitleCache pdt in attributeTitles)
            {
                //if (selectAttributeTitle.Contains(pdt.TypeID)) continue;//选中的分类改为任然显示，用以支持多选
                NarrowByInfo narrowByInfo = AttributesController.GetAttributesResulteList(pdt.TypeID, this._categoryId);
                if (pdt.AttributeTypeID == 2)
                {
                    narrowByInfo.IsBool = true;
                }
                narrowByInfo.ID = pdt.TypeID;
                narrowByInfo.Title = pdt.Title;
                string key = this._categoryId + "," + pdt.TypeID;
                PriceMeCache.CategoryAttributeTitleMapCache categoryAttributeTitleMap = AttributesController.GetCategoryAttributeTitleMapByKey(key);
                if (categoryAttributeTitleMap != null)
                {
                    narrowByInfo.ListOrder = categoryAttributeTitleMap.AttributeOrder;
                }
                else
                {
                    narrowByInfo.ListOrder = 3;
                }
                narrowByInfo.Description = pdt.ShortDescription;
                if (narrowByInfo.IsSlider)
                {
                    if (selectedAttrRangeValues != null && selectedAttrRangeValues.Count > 0)
                    {
                        //设置选中的值， 以获取选中值的下标
                        if (selectedAttrRangeValues.ContainsKey(pdt.TypeID))
                            narrowByInfo.SelectedValue = selectedAttrRangeValues[pdt.TypeID];
                    }
                    //reset and reorder attribute value
                    var narrow_ = ReorderNarrowItemList(narrowByInfo, (PriceMeCache.CategoryAttributeTitleMapCache)narrowByInfo.CategoryAttributeTitleMap);
                    narrowByInfo.NarrowItemList = narrow_.NarrowItemList;
                    narrowByInfo.SelectedValue = narrow_.SelectedValue;
                }
                else
                {
                    SetProductCount(narrowByInfo);
                }
                if (narrowByInfo.NarrowItemList.Count == 0)
                {
                    continue;
                }
                narrowByInfoList.Add(narrowByInfo);
            }
            return narrowByInfoList.OrderBy(ni => ni.ListOrder).ToList();
        }


        public List<NarrowByInfo> GetAttributesResulte_New(Dictionary<int, string> selectedAttrRangeValues)
        {
            List<NarrowByInfo> narrowByInfoList = new List<NarrowByInfo>();
            List<PriceMeCache.AttributeTitleCache> attributeTitles = AttributesController.GetAttributesTitleByCategoryID(this._categoryId);
            if (attributeTitles == null)
            {
                return narrowByInfoList;
            }
            //获取attr products
            SetAttributeData();

            foreach (PriceMeCache.AttributeTitleCache pdt in attributeTitles)
            {
                //if (selectAttributeTitle.Contains(pdt.TypeID)) continue;//选中的分类改为任然显示，用以支持多选
                NarrowByInfo narrowByInfo = AttributesController.GetAttributesResulteList(pdt.TypeID, this._categoryId);
                if (pdt.AttributeTypeID == 2)
                {
                    narrowByInfo.IsBool = true;
                }
                narrowByInfo.ID = pdt.TypeID;
                narrowByInfo.Title = pdt.Title;
                string key = this._categoryId + "," + pdt.TypeID;
                PriceMeCache.CategoryAttributeTitleMapCache categoryAttributeTitleMap = AttributesController.GetCategoryAttributeTitleMapByKey(key);
                if (categoryAttributeTitleMap != null)
                {
                    narrowByInfo.ListOrder = categoryAttributeTitleMap.AttributeOrder;
                }
                else
                {
                    narrowByInfo.ListOrder = 3;
                }
                narrowByInfo.Description = pdt.ShortDescription;
                if (narrowByInfo.IsSlider)
                {
                    if (selectedAttrRangeValues != null && selectedAttrRangeValues.Count > 0)
                    {
                        //设置选中的值， 以获取选中值的下标
                        if (selectedAttrRangeValues.ContainsKey(pdt.TypeID))
                            narrowByInfo.SelectedValue = selectedAttrRangeValues[pdt.TypeID];
                    }

                }

                SetProductCount(narrowByInfo);

                if (narrowByInfo.NarrowItemList.Count == 0)
                {
                    continue;
                }

                narrowByInfoList.Add(narrowByInfo);
            }
            return narrowByInfoList.OrderBy(ni => ni.ListOrder).ToList();
        }

        private void FixSliderAttributeItems(List<NarrowItem> narrowItemList)
        {
            int i = 1;
            int count = narrowItemList.Count;
            int preListOrder = narrowItemList[0].ListOrder;
            while (i < count)
            {
                if (narrowItemList[i].ListOrder == preListOrder)
                {
                    narrowItemList[i - 1].ProductCount += narrowItemList[i].ProductCount;
                    narrowItemList.RemoveAt(i);
                    count--;
                }
                else
                {
                    preListOrder = narrowItemList[i].ListOrder;
                    i++;
                }
            }
        }

        /// <summary>
        /// 按AttributeValue重新筛选并排序
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        private NarrowByInfo ReorderNarrowItemList(NarrowByInfo narrowByInfo, PriceMeCache.CategoryAttributeTitleMapCache map)
        {
            if (narrowByInfo.NarrowItemList == null || narrowByInfo.NarrowItemList.Count == 0) return narrowByInfo;

            //取整数
            int min = (int)map.MinValue;
            if (min == 0)
                map.MinValue = min = ResetMinValue(narrowByInfo.NarrowItemList.First().ListOrder);
            int max = (int)map.MaxValue;
            if (max == 0)
                map.MaxValue = max = ResetMaxValue(narrowByInfo.NarrowItemList.Last().ListOrder);
            int step = (int)map.Step;
            if (step == 0)
                map.Step = step = (max - min) / 4;
            int step2 = (int)map.Step2;
            if (step2 == 0)
                map.Step2 = step2 = step / 6;

            List<NarrowItem> newItems = new List<NarrowItem>();
            var unit = narrowByInfo.NarrowItemList[0].OtherInfo == null ? "" : narrowByInfo.NarrowItemList[0].OtherInfo.ToString();
            var isPop = narrowByInfo.NarrowItemList[0].IsPopular;

            //重置values 值，min ... max
            var min_ = min;
            var max_ = min_ + step;
            for (int i = min; i <= max; i += step2)
            {
                #region 重置 NarrowItems

                NarrowItem narrowItem = new NarrowItem();
                if (!string.IsNullOrEmpty(unit))
                {
                    narrowItem.DisplayName = i + " " + unit;
                    narrowItem.OtherInfo = unit;
                }
                else
                {
                    narrowItem.DisplayName = i.ToString();
                }
                narrowItem.IsPopular = isPop;
                narrowItem.Value = i.ToString();
                narrowItem.ListOrder = i;

                #endregion

                if (i == max_)
                {
                    if (_attrValueProductCountDictionary != null)
                    {
                        #region 获取产品数量
                        var pCount = 0;
                        Dictionary<int, int> items = _attrValueProductCountDictionary.Where(p => p.Key >= min_ && p.Key <= max_).ToDictionary(k => k.Key, v => v.Value);
                        if (items.Count() > 0)
                        {
                            foreach (var item in items)
                            {
                                pCount += item.Value;
                            }
                            narrowItem.ProductCount = pCount;
                        }
                        #endregion

                        min_ = i;
                        max_ = min_ + step;
                    }
                }

                newItems.Add(narrowItem);
            }

            #region 得到选中attr的下标
            var selected_ = string.Empty;
            if (newItems.Count > 0)
            {
                if (string.IsNullOrEmpty(narrowByInfo.SelectedValue))
                {
                    selected_ = newItems.First().Value + "-" + newItems.Last().Value + "|0," + (newItems.Count - 1).ToString();
                }
                else
                {
                    selected_ = narrowByInfo.SelectedValue + "|";//选中attr value的 需要保留，要显示在input box里
                    var selected = narrowByInfo.SelectedValue.Split('-');
                    var insertMin = int.Parse(selected[0]);
                    var insertMax = int.Parse(selected[1]);

                    var itemMin = newItems.LastOrDefault(p => p.ListOrder <= insertMin);
                    if (itemMin == null)
                    {
                        selected_ += "0";
                    }
                    else
                    {
                        selected_ += newItems.IndexOf(itemMin).ToString();
                    }
                    var itemMax = newItems.FirstOrDefault(p => p.ListOrder >= insertMax);
                    if (itemMax == null)
                    {
                        selected_ += "," + (newItems.Count - 1).ToString();
                    }
                    else
                    {
                        selected_ += "," + newItems.IndexOf(itemMax).ToString();
                    }
                }
            }
            #endregion

            NarrowByInfo narrow = new NarrowByInfo();
            narrow.NarrowItemList = newItems;
            narrow.SelectedValue = selected_;
            return narrow;
        }

        /// <summary>
        /// 向下取整
        /// (1,2,3...8,9,10) => (1,2,3...8,9,10)
        /// 11 => 10, 21 => 20, 101 => 100, 1001 => 1000
        /// </summary>
        int ResetMinValue(int value)
        {
            if (value <= 10) return value;
            int ys = value % 10;
            value = value - ys;
            return value;
        }

        /// <summary>
        /// 向上取整
        /// 0 => 0
        /// 11 => 20, 21 => 30, 101 => 110, 1001 => 1010
        /// </summary>
        int ResetMaxValue(int value)
        {
            int ys = value % 10;
            if (ys == 0) return value;
            value = value + 10 - ys;
            return value;
        }

        /// <summary>
        /// 向上or向下取整
        /// (1,2,3...8,9,10) => (1,2,3...8,9,10)
        /// 11 => 10, 15 => 20, 16 => 20
        /// 101 => 100, 105 => 110, 106 => 110
        /// </summary>
        int ResetValue(int value)
        {
            if (value <= 10) return value;
            int ys = value % 10;
            if (ys >= 5)
            {
                value = value + 10 - ys;
            }
            else
            {
                value = value - ys;
            }
            return value;
        }

        //public NarrowByInfo GetRetailerResulteNZ()
        //{
        //    return GetRetailerResulte(3);
        //}

        //public NarrowByInfo GetRetailerResulteAU()
        //{
        //    return GetRetailerResulte(1);
        //}

        public NarrowByInfo GetRetailerResulte()
        {
            NarrowByInfo narrowByInfo = new NarrowByInfo();
            if (_resultHitsInfo == null || _resultHitsInfo.ResultCount == 0)
            {
                return narrowByInfo;
            }

            narrowByInfo.ListOrder = 4;
            narrowByInfo.Title = "Retailers";
            narrowByInfo.Name = "Retailer";

            List<string> productIDList = GetResultProductIDs();
            HitsInfo retailerHitsInfo = SearchController.SearchRetailer(productIDList, _countryId);
            List<NarrowItem> narrowList = new List<NarrowItem>();
            Dictionary<string, int> retailerProductCountDictionary = GetProductCountDictionary(retailerHitsInfo, "RetailerID");
            foreach (string retailerID in retailerProductCountDictionary.Keys)
            {
                PriceMeCache.RetailerCache retailer = RetailerController.GetRetailerDeep(int.Parse(retailerID), _countryId);
                if (retailer == null)
                {
                    continue;
                }

                NarrowItem narrowItem = new NarrowItem();
                narrowItem.DisplayName = retailer.RetailerName;
                narrowItem.Value = retailerID;
                narrowItem.ProductCount = retailerProductCountDictionary[retailerID];
                //只显示是ppc的网店
                if (!RetailerController.IsPPcRetailer(retailer.RetailerId, _countryId))
                {
                    continue;
                }

                narrowList.Add(narrowItem);
            }

            narrowByInfo.NarrowItemList = narrowList.OrderBy(n => n.DisplayName).ToList();
            return narrowByInfo;
        }

        public NarrowByInfo GetSearchPriceRangeResulte0(IFormatProvider provider)
        {
            NarrowByInfo narrowByInfo = new NarrowByInfo();
            //结果小于10个不需要显示PriceRange
            if (_resultHitsInfo == null || _resultHitsInfo.ResultCount <= 10)
            {
                return narrowByInfo;
            }

            narrowByInfo.Name = "SearchPriceRange";
            narrowByInfo.Title = "Price";
            narrowByInfo.ListOrder = 5;

            List<NarrowItem> narrowList = new List<NarrowItem>();

            int count = _resultHitsInfo.ResultCount;
            List<float> priceList = new List<float>();
            for (int i = 0; i < count; i++)
            {
                float price = float.Parse(_resultHitsInfo.GetDocument(i, new string[] { "BestPrice" }).Get("BestPrice"));
                priceList.Add(price);
            }

            priceList.Sort();
            NarrowItem narrowItem = new NarrowItem();
            narrowItem.Value = priceList.Last().ToString();
            narrowList.Add(narrowItem);

            narrowByInfo.NarrowItemList = narrowList;
            return narrowByInfo;
        }

        float GetSearchPrice(float price)
        {
            float newPrice = (float)Math.Round(price, 0, MidpointRounding.AwayFromZero);
            int ys = (int)newPrice % 10;
            if (ys >= 5)
            {
                newPrice = newPrice - ys + 10;
            }
            else
            {
                newPrice = newPrice - ys;
            }
            return newPrice;
        }

        public string FormatPrice(decimal price, string PriceSymbol)
        {
            string dec = "";
            price = decimal.Round(price, 2);
            string result = "";
            if (_countryId == 28 || _countryId == 51)
            {
                //sg 36 my 45 au 1 nz 3 ph 28 hk 41
                price = decimal.Round(price, 0);
                dec = price.ToString();
            }
            //印度尼西亚 整数 00.000.000
            else
            {
                price = decimal.Round(price, 2);
                dec = price.ToString("0.00");
            }
            if (Math.Abs(price) < 1000)
                result = dec;
            else
            {
                int len = 3; string tail = string.Empty;
                if (dec.Contains('.')) { len = 6; tail = dec.Substring(dec.IndexOf('.')); }
                result = dec.Substring(dec.Length - len, len);
                int quotient = System.Convert.ToInt32(price) / 1000;
                if (Math.Abs(quotient) > 0)
                    result = ConvertInt(quotient) + "," + result;
                if (_countryId == 51)
                {
                    result = result.Replace(",", ".");
                }
            }

            return PriceSymbol + result;
        }

        public static string ConvertInt(double src)
        {
            string result = "";
            if (Math.Abs(src) < 1000)
                result = src.ToString();
            else
            {
                result = src.ToString().Substring(src.ToString().Length - 3, 3);
                int quotient = System.Convert.ToInt32(src) / 1000;
                if (Math.Abs(quotient) > 0)
                    result = ConvertInt(quotient) + "," + result;
            }

            return result;
        }

        public NarrowByInfo GetSearchPriceRangeResulte_old(string priceSymbol)
        {
            NarrowByInfo narrowByInfo = new NarrowByInfo();
            //结果小于10个不需要显示PriceRange
            if (this._priceRange != null || _resultHitsInfo == null || _resultHitsInfo.ResultCount <= 10)
            {
                return narrowByInfo;
            }

            narrowByInfo.Name = "SearchPriceRange";
            narrowByInfo.Title = "Price";
            narrowByInfo.ListOrder = 5;

            List<NarrowItem> narrowList = new List<NarrowItem>();

            int count = _resultHitsInfo.ResultCount;
            List<float> priceList = new List<float>();
            for (int i = 0; i < count; i++)
            {
                float price = float.Parse(_resultHitsInfo.GetDocument(i, new string[] { "BestPrice" }).Get("BestPrice"));
                priceList.Add(price);
            }

            priceList.Sort();
            int c = 5;
            int step = priceList.Count / c;

            for (int i = 0; i < c; i++)
            {
                NarrowItem narrowItem = new NarrowItem();
                if (i == 0)
                {
                    float maxPrice = priceList[step - 1];
                    narrowItem.ProductCount = step;
                    narrowItem.DisplayName = "Below " + FormatPrice(decimal.Parse(maxPrice.ToString("0")), priceSymbol);
                    narrowItem.Value = "0-" + maxPrice.ToString("0.00");
                }
                else if (i > 0 && i < c - 1)
                {
                    float minPrice = priceList[step * i];
                    float maxPrice = priceList[step * (i + 1) - 1];
                    narrowItem.ProductCount = step;
                    narrowItem.DisplayName = FormatPrice(decimal.Parse(minPrice.ToString("0")), priceSymbol) + "-" + FormatPrice(decimal.Parse(maxPrice.ToString("0")), priceSymbol);
                    narrowItem.Value = minPrice.ToString("0.00") + "-" + maxPrice.ToString("0.00");
                }
                else
                {
                    float minPrice = priceList[step * i];
                    narrowItem.ProductCount = priceList.Count - step * i;
                    narrowItem.DisplayName = "Above " + FormatPrice(decimal.Parse(minPrice.ToString("0")), priceSymbol);
                    narrowItem.Value = minPrice.ToString("0.00") + "-0";
                }
                narrowList.Add(narrowItem);
            }

            narrowByInfo.NarrowItemList = narrowList;
            return narrowByInfo;
        }

        public NarrowByInfo GetCatalogPriceRangeResulte0(IFormatProvider provider, string priceSymbol)
        {
            NarrowByInfo narrowByInfo = new NarrowByInfo();
            if (_resultHitsInfo == null || _resultHitsInfo.ResultCount < 24)
            {
                return narrowByInfo;
            }

            narrowByInfo.Name = "CatalogPriceRange";
            narrowByInfo.Title = "Price";
            narrowByInfo.ListOrder = 5;

            List<NarrowItem> narrowList = new List<NarrowItem>();

            int count = _resultHitsInfo.ResultCount;
            List<float> priceList = new List<float>();
            List<float> priceList2 = new List<float>();
            for (int i = 0; i < count; i++)
            {
                float price = float.Parse(_resultHitsInfo.GetDocument(i, new string[] { "BestPrice" }).Get("BestPrice"));
                priceList.Add(price);
            }

            priceList.Sort();
            int c = 22;//0 ...(22个)...max, 总共24个slider step

            int step = priceList.Count / c;
            if (priceList.Count % c == 0)
            {
                //如果除得尽， 那么max == 第23个， 有重复的
                step = priceList.Count / 23;
            }

            NarrowItem narrowItem = new NarrowItem();
            float maxPrice = 0;
            narrowItem.ProductCount = step;
            narrowItem.DisplayName = narrowItem.Value = "0";
            narrowList.Add(narrowItem);
            if (priceList.Last() < 24)
            {
                //if max < 24, slider:
                //0 1 2 3 4 5 ......23
                for (int i = 1; i < 24; i++)
                {
                    narrowItem = new NarrowItem();

                    narrowItem.ProductCount = step;
                    narrowItem.DisplayName = narrowItem.Value = i.ToString("0");
                    narrowList.Add(narrowItem);
                }

                narrowByInfo.NarrowItemList = narrowList;
                return narrowByInfo;
            }

            //所有products 分成4大份， 
            //第1和第4份，分成5小份，加上0或max value， 
            //中间每份再分6小份，总共24份
            var cc = 0;
            for (int i = 0; i < 4; i++)
            {
                var narrowList_ = new List<NarrowItem>();
                priceList2 = new List<float>();
                //5|6|6|5 = 22
                var mm = 6;//每大份分成的份数
                if (i == 0 || i == 3) mm = 5;
                for (int j = 0; j < mm; j++)
                {
                    cc++;
                    narrowItem = new NarrowItem();
                    var pi = step * cc - 1;
                    if (pi >= priceList.Count) continue;
                    maxPrice = priceList[pi];
                    maxPrice = GetSearchPrice(maxPrice);//取整数
                    if (priceList2.Contains(maxPrice)) continue;//去掉重复的

                    narrowItem.ProductCount = step;
                    narrowItem.DisplayName = narrowItem.Value = maxPrice.ToString("0");
                    narrowList_.Add(narrowItem);

                    priceList2.Add(maxPrice);
                }

                //如果第1，第4大份没有分成5小份， 2,3 大份没有分成6小份，要再分
                //按每大分的min-max， 平均分成5、6小份
                if (narrowList_.Count < mm)
                {
                    narrowList_ = new List<NarrowItem>();
                    var min = priceList[step * (cc - mm + 1) - 1];
                    var max = priceList[step * cc - 1];
                    var range = max - min;
                    var step_ = (int)(range / (mm - 1));
                    if (step_ == 0) step_ = 1;
                    priceList2 = new List<float>();
                    for (int k = 0; k < (mm - 1); k++)
                    {
                        narrowItem = new NarrowItem();
                        var price = min + k * step_;
                        if (price > max) price = max;

                        narrowItem.ProductCount = step;
                        narrowItem.DisplayName = narrowItem.Value = price.ToString("0");
                        narrowList_.Add(narrowItem);
                    }
                    narrowItem = new NarrowItem();
                    narrowItem.ProductCount = step;
                    narrowItem.DisplayName = narrowItem.Value = max.ToString("0");
                    narrowList_.Add(narrowItem);
                }

                narrowList.AddRange(narrowList_);
            }

            maxPrice = GetSearchPrice(priceList.Last());
            narrowItem = new NarrowItem();
            narrowItem.ProductCount = step;
            narrowItem.DisplayName = narrowItem.Value = maxPrice.ToString("0");
            narrowList.Add(narrowItem);

            narrowByInfo.NarrowItemList = narrowList;
            return narrowByInfo;
        }

        public NarrowByInfo GetCatalogPriceRangeResulte_New(IFormatProvider provider, string priceSymbol, int step, double maxP)
        {
            NarrowByInfo narrowByInfo = new NarrowByInfo();
            if (_resultHitsInfo == null || _resultHitsInfo.ResultCount < 24)
            {
                return narrowByInfo;
            }

            int count = _resultHitsInfo.ResultCount;
            List<double> priceList = new List<double>();
            List<double> priceList2 = new List<double>();
            for (int i = 0; i < count; i++)
            {
                double price = double.Parse(_resultHitsInfo.GetDocument(i, new string[] { "BestPrice" }).Get("BestPrice"));
                priceList.Add(price);
            }

            priceList.Sort();

            double maxPrice = priceList.Last() + 1;
            if (maxP > 0)
            {
                maxPrice = maxP;
            }

            if (maxPrice <= step)
            {
                return narrowByInfo;
            }

            narrowByInfo.Name = "CatalogPriceRange";
            narrowByInfo.Title = "Price";
            narrowByInfo.ListOrder = 5;

            double priceStep = maxPrice / step;

            List<NarrowItem> narrowList = new List<NarrowItem>();

            int pCount = 0;
            for (int i = 1; i <= step; i++)
            {
                double p = priceStep * i;
                NarrowItem narrowItem = new NarrowItem();
                narrowItem.DisplayName = narrowItem.Value = p.ToString("0");

                int pc = priceList.Where(pr => pr <= p).Count();
                narrowItem.ProductCount = pc - pCount;
                pCount = pc;
                narrowList.Add(narrowItem);
            }

            narrowByInfo.NarrowItemList = narrowList;
            return narrowByInfo;
        }

        public NarrowByInfo GetSearchPriceRangeResulte(IFormatProvider provider)
        {
            NarrowByInfo narrowByInfo = new NarrowByInfo();
            //结果小于10个不需要显示PriceRange
            if (this._priceRange != null || _resultHitsInfo == null || _resultHitsInfo.ResultCount <= 10)
            {
                return narrowByInfo;
            }

            narrowByInfo.Name = "SearchPriceRange";
            narrowByInfo.Title = "Price";
            narrowByInfo.ListOrder = 5;

            List<NarrowItem> narrowList = new List<NarrowItem>();

            int count = _resultHitsInfo.ResultCount;
            List<float> priceList = new List<float>();
            for (int i = 0; i < count; i++)
            {
                float price = float.Parse(_resultHitsInfo.GetDocument(i, new string[] { "BestPrice" }).Get("BestPrice"));
                priceList.Add(price);
            }

            priceList.Sort();
            int c = 5;
            int step = priceList.Count / c;

            for (int i = 0; i < c; i++)
            {
                NarrowItem narrowItem = new NarrowItem();
                if (i == 0)
                {
                    float maxPrice = priceList[step - 1];
                    narrowItem.ProductCount = step;

                    maxPrice = GetSearchPrice(maxPrice);

                    narrowItem.DisplayName = "Below " + PriceMeStatic.PriceIntCultureString(maxPrice, provider, _countryId);
                    narrowItem.Value = "0-" + maxPrice.ToString("0");
                }
                else if (i > 0 && i < c - 1)
                {
                    float minPrice = priceList[step * i];
                    float maxPrice = priceList[step * (i + 1) - 1];
                    narrowItem.ProductCount = step;

                    minPrice = GetSearchPrice(minPrice);
                    maxPrice = GetSearchPrice(maxPrice);

                    narrowItem.DisplayName = PriceMeStatic.PriceIntCultureString(minPrice, provider, _countryId) + "-" + PriceMeStatic.PriceIntCultureString(maxPrice, provider, _countryId);
                    narrowItem.Value = minPrice.ToString("0") + "-" + maxPrice.ToString("0");
                }
                else
                {
                    float minPrice = priceList[step * i];
                    narrowItem.ProductCount = priceList.Count - step * i;

                    minPrice = GetSearchPrice(minPrice);

                    narrowItem.DisplayName = "Above " + PriceMeStatic.PriceIntCultureString(minPrice, provider, _countryId);
                    narrowItem.Value = minPrice.ToString("0") + "-0";
                }
                narrowList.Add(narrowItem);
            }

            narrowByInfo.NarrowItemList = narrowList;
            return narrowByInfo;
        }

        public NarrowByInfo GetSearchPriceRangeResult2(IFormatProvider provider)
        {
            NarrowByInfo narrowByInfo = new NarrowByInfo();
            //结果小于24(每页数量)个不需要显示PriceRange
            if (_resultHitsInfo == null || _resultHitsInfo.ResultCount < 24)
            {
                return narrowByInfo;
            }

            narrowByInfo.Name = "SearchPriceRange";
            narrowByInfo.Title = "Price";
            narrowByInfo.ListOrder = 5;

            List<NarrowItem> narrowList = new List<NarrowItem>();

            int count = _resultHitsInfo.ResultCount;
            List<float> priceList = new List<float>();
            List<float> priceList2 = new List<float>();
            for (int i = 0; i < count; i++)
            {
                float price = float.Parse(_resultHitsInfo.GetDocument(i, new string[] { "BestPrice" }).Get("BestPrice"));
                priceList.Add(price);
            }

            priceList.Sort();
            int step = priceList.Count / 22;//0 ...(22个)...max, 总共24个slider step
            if (priceList.Count % 22 == 0)
            {
                //如果除得尽， 那么max == 第23个， 有重复的
                step = priceList.Count / 23;
            }

            NarrowItem narrowItem = new NarrowItem();
            float maxPrice = 0;
            narrowItem.ProductCount = step;
            narrowItem.DisplayName = narrowItem.Value = "0";
            narrowList.Add(narrowItem);
            if (priceList.Last() < 24)
            {
                //if max < 24, slider:
                //0 1 2 3 4 5 ......23
                for (int i = 1; i < 24; i++)
                {
                    narrowItem = new NarrowItem();

                    narrowItem.ProductCount = step;
                    narrowItem.DisplayName = narrowItem.Value = i.ToString("0");
                    narrowList.Add(narrowItem);
                }

                narrowByInfo.NarrowItemList = narrowList;
                return narrowByInfo;
            }

            //所有products 分成4大份， 
            //第1和第4份，分成5小份，加上0或max value， 
            //中间每份再分6小份，总共24份
            var cc = 0;
            for (int i = 0; i < 4; i++)
            {
                var narrowList_ = new List<NarrowItem>();
                priceList2 = new List<float>();
                //5|6|6|5 = 22
                var mm = 6;//每大份分成的份数
                if (i == 0 || i == 3) mm = 5;
                for (int j = 0; j < mm; j++)
                {
                    cc++;
                    narrowItem = new NarrowItem();
                    var pi = step * cc - 1;
                    if (pi >= priceList.Count) continue;
                    maxPrice = priceList[pi];
                    maxPrice = GetSearchPrice(maxPrice);//取整数
                    if (priceList2.Contains(maxPrice)) continue;//去掉重复的

                    narrowItem.ProductCount = step;
                    narrowItem.DisplayName = narrowItem.Value = maxPrice.ToString("0");
                    narrowList_.Add(narrowItem);

                    priceList2.Add(maxPrice);
                }

                //如果第1，第4大份没有分成5小份， 2,3 大份没有分成6小份， 要再分
                //按每大份的min-max， 平均分成5、6小份                
                if (narrowList_.Count < mm)
                {
                    narrowList_ = new List<NarrowItem>();
                    var min = priceList[step * (cc - mm + 1) - 1];
                    var max = priceList[step * cc - 1];
                    var range = max - min;
                    var step_ = (int)(range / (mm - 1));
                    if (step_ == 0) step_ = 1;
                    priceList2 = new List<float>();
                    for (int k = 0; k < (mm - 1); k++)
                    {
                        narrowItem = new NarrowItem();
                        var price = min + k * step_;
                        if (price > max) price = max;

                        narrowItem.ProductCount = step;
                        narrowItem.DisplayName = narrowItem.Value = price.ToString("0");
                        narrowList_.Add(narrowItem);
                    }
                    narrowItem = new NarrowItem();
                    narrowItem.ProductCount = step;
                    narrowItem.DisplayName = narrowItem.Value = max.ToString("0");
                    narrowList_.Add(narrowItem);
                }

                narrowList.AddRange(narrowList_);
            }
            maxPrice = GetSearchPrice(priceList.Last());
            narrowItem = new NarrowItem();
            narrowItem.ProductCount = step;
            narrowItem.DisplayName = narrowItem.Value = maxPrice.ToString("0");
            narrowList.Add(narrowItem);

            narrowByInfo.NarrowItemList = narrowList;
            return narrowByInfo;
        }

        private void SetAttributeData()
        {
            List<string> productIdList = GetResultProductIDs();

            HitsInfo attributeHitsInfo = SearchController.SearchAttributes(productIdList, _countryId);

            if (_attributeProductCountDictionary == null)
            {
                if (attributeHitsInfo != null)
                {
                    _attributeProductCountDictionary = GetProductCountDictionary(attributeHitsInfo, "AttributeValueID");
                }
            }

            //得到对应attribute 的产品数量
            if (_attrValueProductCountDictionary == null)
            {
                if (attributeHitsInfo != null)
                {
                    _attrValueProductCountDictionary = new Dictionary<int, int>();
                    var dict = GetProductCountDictionary(attributeHitsInfo, "AttributeValue");
                    //10.1 => 10, 10.9 => 10
                    foreach (var item in dict)
                    {
                        int key = 0;
                        string key_ = item.Key;
                        if (key_.Contains("."))
                            key_ = key_.Substring(0, key_.IndexOf("."));
                        if (int.TryParse(key_, out key))
                        {
                            if (_attrValueProductCountDictionary.ContainsKey(key))
                            {
                                _attrValueProductCountDictionary[key] += item.Value;
                            }
                            else
                            {
                                _attrValueProductCountDictionary.Add(key, item.Value);
                            }
                        }
                    }
                }
            }

            if (this._selectedAttributeIds != null)
            {
                foreach (int avID in this._selectedAttributeIds)
                {
                    PriceMeCache.AttributeTitleCache title = AttributesController.GetAttributeTitleByVauleID(avID);
                    if (title != null && !selectAttributeTitle.Contains(title.TypeID))
                    {
                        selectAttributeTitle.Add(title.TypeID);
                    }
                }
            }

            if (this._selectedAttributeRangeIds != null)
            {
                foreach (int arID in this._selectedAttributeRangeIds)
                {
                    var title = AttributesController.GetAttributeTitleByAttributeRangeID(arID);
                    if (title != null && !selectAttributeTitle.Contains(title.TypeID))
                    {
                        selectAttributeTitle.Add(title.TypeID);
                    }
                }
            }
        }

        /// <summary>
        /// get product count of every attribute value
        /// </summary>
        private void SetAttributeValueRangeData(int typeID, string attrUnit)
        {
            List<string> productIdList = GetResultProductIDs();

            HitsInfo attributeHitsInfo = SearchController.SearchAttributes(productIdList, _countryId);

            if (_attrValueProductCountDictionary == null)
            {
                if (attributeHitsInfo != null)
                {
                    _attrValueProductCountDictionary = new Dictionary<int, int>();
                    var dict = GetProductCountDictionary(attributeHitsInfo, typeID, attrUnit);
                    foreach (var item in dict)
                    {
                        int key = 0;
                        string key_ = item.Key;
                        if (key_.Contains("."))
                            key_ = key_.Substring(0, key_.IndexOf("."));
                        if (int.TryParse(key_, out key))
                        {
                            if (_attrValueProductCountDictionary.ContainsKey(key))
                            {
                                _attrValueProductCountDictionary[key] += item.Value;
                            }
                            else
                            {
                                _attrValueProductCountDictionary.Add(key, item.Value);
                            }
                        }
                    }
                }
            }
        }

        private List<string> GetResultProductIDs()
        {
            if (_resultProductIDs == null)
            {
                _resultProductIDs = new List<string>();
                if (_resultHitsInfo != null)
                {
                    for (int i = 0; i < _resultHitsInfo.ResultCount; i++)
                    {
                        _resultProductIDs.Add(_resultHitsInfo.GetDocument(i, new string[] { "ProductID" }).Get("ProductID"));
                    }
                }
            }

            return _resultProductIDs;
        }

        private Dictionary<string, int> GetProductCountDictionary(HitsInfo hitsInfo, string fieldName)
        {
            Dictionary<string, int> productCountDictionary = new Dictionary<string, int>();
            for (int j = 0; j < hitsInfo.ResultCount; j++)
            {
                string attrValue = hitsInfo.GetDocument(j, new string[] { fieldName }).Get(fieldName);
                if (string.IsNullOrEmpty(attrValue))
                {
                    continue;
                }
                if (productCountDictionary.ContainsKey(attrValue))
                {
                    productCountDictionary[attrValue]++;
                }
                else
                {
                    productCountDictionary.Add(attrValue, 1);
                }
            }
            return productCountDictionary;
        }

        /// <summary>
        /// get product count of attribute value
        /// </summary>
        /// <param name="hitsInfo"></param>
        /// <param name="typeID"></param>
        /// <param name="attrUnit"></param>
        /// <returns></returns>
        private Dictionary<string, int> GetProductCountDictionary(HitsInfo hitsInfo, int typeID, string attrUnit)
        {
            Dictionary<string, int> productCountDictionary = new Dictionary<string, int>();
            for (int j = 0; j < hitsInfo.ResultCount; j++)
            {
                string attrValue = hitsInfo.GetDocument(j, new string[] { "AttributeValue" }).Get("AttributeValue");
                var TypeID = hitsInfo.GetDocument(j, new string[] { "TypeID" }).Get("TypeID");
                if (typeID.ToString() == TypeID)
                {
                    if (string.IsNullOrEmpty(attrValue) || attrValue == "0")
                    {
                        var AttributeValueName = hitsInfo.GetDocument(j, new string[] { "AttributeValueName" }).Get("AttributeValueName");
                        attrValue = AttributeValueName.Replace(attrUnit, "").Trim();
                    }
                    if (productCountDictionary.ContainsKey(attrValue))
                    {
                        productCountDictionary[attrValue]++;
                    }
                    else
                    {
                        productCountDictionary.Add(attrValue, 1);
                    }
                }
                else
                {
                    continue;
                }
            }
            return productCountDictionary;
        }

        void SetProductCount(NarrowByInfo attributeNarrowByInfo)
        {
            int i = 0;
            int count = attributeNarrowByInfo.NarrowItemList.Count;
            if (attributeNarrowByInfo.Name.Equals("Attribute"))
            {
                while (i < count)
                {
                    List<string> attributeIDs = new List<string>();
                    attributeIDs.Add(attributeNarrowByInfo.NarrowItemList[i].Value);
                    int productCount = GetAttributeProductCount(attributeIDs);
                    if (productCount == 0)
                    {
                        attributeNarrowByInfo.NarrowItemList.RemoveAt(i);
                        count--;
                    }
                    else
                    {
                        attributeNarrowByInfo.NarrowItemList[i].ProductCount = productCount;
                        i++;
                    }
                }
            }
            else if (attributeNarrowByInfo.Name.Equals("AttributeRange"))
            {
                while (i < count)
                {
                    List<string> attributeIDs = new List<string>();
                    List<PriceMeCache.AttributeValueCache> attributeValueList = AttributesController.GetAttributeValuesByValueRangeID(int.Parse(attributeNarrowByInfo.NarrowItemList[i].Value));
                    foreach (PriceMeCache.AttributeValueCache attributeValue in attributeValueList)
                    {
                        attributeIDs.Add(attributeValue.AttributeValueID.ToString());
                    }

                    int productCount = GetAttributeProductCount(attributeIDs);
                    if (productCount == 0)
                    {
                        attributeNarrowByInfo.NarrowItemList.RemoveAt(i);
                        count--;
                    }
                    else
                    {
                        attributeNarrowByInfo.NarrowItemList[i].ProductCount = productCount;
                        i++;
                    }
                }
            }
        }
        void SetAttrValueProductCount(NarrowByInfo attributeNarrowByInfo)
        {
            if (_attrValueProductCountDictionary == null)
            {
                attributeNarrowByInfo.NarrowItemList.Clear();
                return;
            }

            int i = 0;
            int count = attributeNarrowByInfo.NarrowItemList.Count;

            while (i < count)
            {
                int sum = 0;
                if (_attrValueProductCountDictionary.ContainsKey(attributeNarrowByInfo.NarrowItemList[i].ListOrder))
                    sum = _attrValueProductCountDictionary[attributeNarrowByInfo.NarrowItemList[i].ListOrder];
                if (sum == 0)
                {
                    attributeNarrowByInfo.NarrowItemList.RemoveAt(i);
                    count--;
                }
                else
                {
                    attributeNarrowByInfo.NarrowItemList[i].ProductCount = sum;
                    i++;
                }
            }
        }

        private int GetAttributeProductCount(List<string> attributeValueIDs)
        {
            if (_attributeProductCountDictionary == null)
            {
                return 0;
            }

            int sum = 0;
            foreach (string attributeValueID in attributeValueIDs)
            {
                if (_attributeProductCountDictionary.ContainsKey(attributeValueID))
                {
                    sum += _attributeProductCountDictionary[attributeValueID];
                }
            }
            return sum;
        }

        public List<CatalogManufeaturerProduct> GetManufeaturerProductList(List<string> displayAllProductsManufeaturerIDs, int quickListCount)
        {
            List<CatalogManufeaturerProduct> catalogManufeaturerProductList = new List<CatalogManufeaturerProduct>();
            if (_resultHitsInfo != null)
            {
                for (int i = 0; i < _resultHitsInfo.ResultCount; i++)
                {
                    Document doc = _resultHitsInfo.GetDocument(i, new string[] { "ManufacturerID", "CategoryID", "ProductID", "ProductName", "BestPrice", "PriceCount", "AvRating", "DayCount", "IsUpComing" });

                    string manufacturerID = doc.Get("ManufacturerID");

                    CatalogManufeaturerProduct manufacturerCache = null;
                    foreach (CatalogManufeaturerProduct mfc in catalogManufeaturerProductList)
                    {
                        if (mfc.ManufacturerCache.Value == manufacturerID)
                        {
                            manufacturerCache = mfc;
                            break;
                        }
                    }

                    if (manufacturerCache == null)
                    {
                        manufacturerCache = new CatalogManufeaturerProduct();
                        manufacturerCache.ManufacturerCache = new LinkInfo();
                        ManufacturerInfo manu = ManufacturerController.GetManufacturerByID(int.Parse(manufacturerID), _countryId);
                        if (manu != null)
                        {
                            manufacturerCache.ManufacturerCache.LinkText = manu.ManufacturerName;
                        }
                        else
                        {
                            manufacturerCache.ManufacturerCache.LinkText = "";
                        }

                        manufacturerCache.ManufacturerCache.Value = manufacturerID;
                        manufacturerCache.DisplayAllManufeaturerProducts = displayAllProductsManufeaturerIDs;
                        manufacturerCache.ProductCatalogCollection = new List<ProductCatalog>();
                        manufacturerCache.NeedDisplayMoreLink = false;
                        catalogManufeaturerProductList.Add(manufacturerCache);
                    }

                    if (displayAllProductsManufeaturerIDs.Contains(manufacturerCache.ManufacturerCache.Value))
                    {
                        ProductCatalog productCatalog = SearchController.GetProductCatalog(_resultHitsInfo, i);
                        manufacturerCache.ProductCatalogCollection.Add(productCatalog);
                    }
                    else
                    {
                        if (manufacturerCache.ProductCatalogCollection.Count < quickListCount)
                        {
                            ProductCatalog productCatalog = SearchController.GetProductCatalog(_resultHitsInfo, i);
                            manufacturerCache.ProductCatalogCollection.Add(productCatalog);
                        }
                        else
                        {
                            manufacturerCache.NeedDisplayMoreLink = true;
                        }
                    }
                }
            }

            return catalogManufeaturerProductList;
        }

        private ProductCatalog GetQuickCatalogProduct(Document doc)
        {
            ProductCatalog pc = new ProductCatalog();
            string productID = doc.Get("ProductID");
            string productName = doc.Get("ProductName");
            string categoryID = doc.Get("CategoryID");
            //string retailerProductList = doc.Get("RetailerProductList");
            string priceCount = doc.Get("PriceCount");
            string bestPrice = doc.Get("BestPrice");
            string avRating = doc.Get("AvRating");
            int dayCount = int.Parse(doc.Get("DayCount"));
            int cid = 0;
            int.TryParse(categoryID, out cid);

            pc.DisplayName = productName;
            pc.ProductName = productName;
            pc.ProductID = productID;
            pc.CategoryID = cid;
            pc.BestPrice = bestPrice;
            //pc.RetailerProductInfoString = retailerProductList;
            pc.PriceCount = priceCount;
            pc.AvRating = avRating;
            pc.DayCount = dayCount;
            //FillData(pc);
            return pc;
        }

        public IEnumerable<string> attrValues { get; set; }

        public List<string> GetTop3Products()
        {
            List<string> pidList = new List<string>();
            SearchResult sr = GetSearchResult(1, 3);
            int pCount = sr.ProductCatalogList.Count;
            for (int i = 0; i < pCount; i++)
            {
                if (sr.ProductCatalogList[i].Click >= 10)
                {
                    pidList.Add(sr.ProductCatalogList[i].ProductID);
                }
            }
            return pidList;
        }
    }
}