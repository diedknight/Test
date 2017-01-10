using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lucene.Net.Search;
using Lucene.Net.Index;
using PriceMeDBA;
using PriceMeCache;
using PriceMeCommon.Data;

namespace PriceMeCommon
{
    public static class LuceneController
    {
        public static void ReleaseAllSearchIndex()
        {
            foreach (int key in luceneInfoDictionary.Keys)
            {
                LuceneInfo luceneInfo = luceneInfoDictionary[key];

                if (luceneInfo.AllBrandsIndexSearcher != null)
                {
                    luceneInfo.AllBrandsIndexSearcher.Dispose();
                }

                if (luceneInfo.AttributesIndexSearcher != null)
                {
                    luceneInfo.AttributesIndexSearcher.Dispose();
                }

                if (luceneInfo.CategoriesIndexSearcher != null)
                {
                    luceneInfo.CategoriesIndexSearcher.Dispose();
                }

                if (luceneInfo.PopularIndexSearcher != null)
                {
                    luceneInfo.PopularIndexSearcher.Dispose();
                }

                if (luceneInfo.ProductRetailerMapIndexSearcher != null)
                {
                    luceneInfo.ProductRetailerMapIndexSearcher.Dispose();
                }

                if (luceneInfo.RetailerProductsIndexSearcher != null)
                {
                    luceneInfo.RetailerProductsIndexSearcher.Dispose();
                }

                if (luceneInfo.CategoriesProductLuceneIndex != null)
                {
                    foreach (var searchIndex in luceneInfo.CategoriesProductLuceneIndex.Values)
                    {
                        if (searchIndex != null)
                        {
                            searchIndex.Dispose();
                        }
                    }
                }
            }
        }

        static Dictionary<int, LuceneInfo> luceneInfoDictionary = new Dictionary<int, LuceneInfo>();
        static LuceneInfo defaultLuceneInfo;

        static IndexSearcher reviewAverageIndexSearcher;
        static IndexSearcher expertReviewIndexSearcher;
        static IndexSearcher productVideoIndexSearcher;

        static IEnumerable<CSK_Store_Category> AllActiveRootCategories;

        #region getIndexSeacher
        /// <summary>
        /// both NZ and AU CategoriesProductLuceneIndex
        /// </summary>

        public static Searcher ReviewAverageIndexSearcher
        {
            get { return LuceneController.reviewAverageIndexSearcher; }
        }

        public static Searcher ExpertReviewIndexSearcher
        {
            get { return LuceneController.expertReviewIndexSearcher; }
        }

        public static Searcher ProductVideoIndexSearcher
        {
            get { return LuceneController.productVideoIndexSearcher; }
        }

        public static Searcher GetAllBrandsIndexSearcher(int countryID)
        {
            foreach (var luceneInfo in luceneInfoDictionary.Values)
            {
                if (luceneInfo.CountryID == countryID)
                {
                    return luceneInfo.AllBrandsIndexSearcher;
                }
            }
            return null;
        }

        public static Searcher GetProductRetailerMapIndexSearcher(int countryID)
        {
            foreach (var luceneInfo in luceneInfoDictionary.Values)
            {
                if (luceneInfo.CountryID == countryID)
                {
                    return luceneInfo.ProductRetailerMapIndexSearcher;
                }
            }
            return null;
        }

        public static Searcher AllBrandsIndexSearcher
        {
            get
            {
                return GetAllBrandsIndexSearcher(ConfigAppString.CountryID);
            }
        }

        public static Searcher ProductRetailerMapIndexSearcher
        {
            get
            {
                return GetProductRetailerMapIndexSearcher(ConfigAppString.CountryID);
            }
        }

        public static Searcher GetPopularIndexSearcher(int countryID)
        {
            foreach (var luceneInfo in luceneInfoDictionary.Values)
            {
                if (luceneInfo.CountryID == countryID)
                {
                    return luceneInfo.PopularIndexSearcher;
                }
            }
            return null;
        }

        public static Searcher PopularIndexSearcher
        {
            get
            {
                return GetPopularIndexSearcher(ConfigAppString.CountryID);
            }
        }

        public static Searcher GetCategoriesIndexSearcher(int countryID)
        {
            foreach (var luceneInfo in luceneInfoDictionary.Values)
            {
                if (luceneInfo.CountryID == countryID)
                {
                    return luceneInfo.CategoriesIndexSearcher;
                }
            }
            return null;
        }

        public static Searcher CategoriesIndexSearcher
        {
            get
            {
                return GetCategoriesIndexSearcher(ConfigAppString.CountryID);
            }
        }

        public static Searcher GetAttributesIndexSearcher(int countryID)
        {
            foreach (var luceneInfo in luceneInfoDictionary.Values)
            {
                if (luceneInfo.CountryID == countryID)
                {
                    return luceneInfo.AttributesIndexSearcher;
                }
            }
            return null;
        }

        public static Searcher AttributesIndexSearcher
        {
            get
            {
                return GetAttributesIndexSearcher(ConfigAppString.CountryID);
            }
        }

        public static Searcher GetRetailerProductsIndexSearcher(int countryID)
        {
            foreach (var luceneInfo in luceneInfoDictionary.Values)
            {
                if (luceneInfo.CountryID == countryID)
                {
                    return luceneInfo.RetailerProductsIndexSearcher;
                }
            }
            return null;
        }

        public static Searcher RetailerProductsIndexSearcher
        {
            get
            {
                return GetRetailerProductsIndexSearcher(ConfigAppString.CountryID);
            }
        }

        public static Dictionary<int, Searcher> GetCategoriesProductLuceneIndex(int countryID)
        {
            foreach (var luceneInfo in luceneInfoDictionary.Values)
            {
                if (luceneInfo.CountryID == countryID)
                {
                    return luceneInfo.CategoriesProductLuceneIndex;
                }
            }
            return null;
        }

        public static Dictionary<int, Searcher> CategoriesProductLuceneIndex
        {
            get
            {
                return GetCategoriesProductLuceneIndex(ConfigAppString.CountryID);
            }
        }

        #endregion

        static LuceneController()
        {
            if (!LoadAllIndexSearcher())
            {
                throw new Exception("LoadAllIndexSearcher error! Detail in log file!");
            }
        }

        public static LuceneInfo GetLuceneInfo(int countryID)
        {
            if (luceneInfoDictionary.ContainsKey(countryID))
            {
                return luceneInfoDictionary[countryID];
            }
            return null;
        }
        
        public static bool LoadAllIndexSearcher()
        {
            try
            {
                List<Searcher> expireSearcherList = new List<Searcher>();

                if (luceneInfoDictionary.Values.Count > 0)
                {
                    foreach (var luceneInfo in luceneInfoDictionary.Values)
                    {
                        expireSearcherList.Add(luceneInfo.AllBrandsIndexSearcher);
                        expireSearcherList.Add(luceneInfo.AttributesIndexSearcher);
                        expireSearcherList.Add(luceneInfo.CategoriesIndexSearcher);
                        expireSearcherList.Add(luceneInfo.PopularIndexSearcher);
                        expireSearcherList.Add(luceneInfo.ProductRetailerMapIndexSearcher);
                        expireSearcherList.Add(luceneInfo.RetailerProductsIndexSearcher);
                        expireSearcherList.AddRange(luceneInfo.CategoriesProductLuceneIndex.Values.ToArray());
                    }
                }
                reviewAverageIndexSearcher = GetIndexSearcher(ConfigAppString.ReviewAverageIndexPath);
                expertReviewIndexSearcher = GetIndexSearcher(ConfigAppString.ExpertReviewIndexPath);
                productVideoIndexSearcher = GetIndexSearcher(ConfigAppString.ProductVideoIndexPath);

                AllActiveRootCategories = CSK_Store_Category.Find(cat => cat.ParentID == new int?(0) && cat.IsActive == true);

                luceneInfoDictionary = new Dictionary<int, LuceneInfo>();
                bool includeCountry = false;
                foreach (var countryInfo in ConfigAppString.CountryInfoList)
                {
                    LuceneInfo luceneInfo = new LuceneInfo();
                    luceneInfo.CountryID = countryInfo.CountryID;
                    luceneInfo.CategoriesProductLuceneIndex = GetCategoriesProductLuceneIndex(countryInfo.LuceneIndexPath);
                    luceneInfo.CategoriesIndexSearcher = GetIndexSearcher(countryInfo.LuceneIndexPath + "Categories");
                    luceneInfo.AttributesIndexSearcher = GetIndexSearcher(countryInfo.LuceneIndexPath + "ProductsDescriptor", true);
                    luceneInfo.ProductRetailerMapIndexSearcher = GetIndexSearcher(countryInfo.LuceneIndexPath + "ProductRetailerMap", true);
                    luceneInfo.RetailerProductsIndexSearcher = GetIndexSearcher(countryInfo.LuceneIndexPath + "RetailerProducts");
                    luceneInfo.AllBrandsIndexSearcher = GetIndexSearcher(countryInfo.LuceneIndexPath + "AllCategoriesProduct\\AllBrands", true);
                    luceneInfo.PopularIndexSearcher = GetIndexSearcher(ConfigAppString.PopularSearchIndexPath2, true);
                    luceneInfoDictionary.Add(luceneInfo.CountryID, luceneInfo);

                    if (countryInfo.CountryID == ConfigAppString.CountryID)
                    {
                        includeCountry = true;
                    }
                }

                int defaultCountryID = ConfigAppString.CountryInfoList[0].CountryID;
                defaultLuceneInfo = new LuceneInfo();
                defaultLuceneInfo.CountryID = 0;
                if (luceneInfoDictionary.Values.Count > 1)
                {
                    defaultLuceneInfo.CategoriesProductLuceneIndex = GetAllCategoriesProductLuceneIndex(luceneInfoDictionary);
                }
                else
                {
                    defaultLuceneInfo.CategoriesProductLuceneIndex = luceneInfoDictionary[ConfigAppString.CountryInfoList[0].CountryID].CategoriesProductLuceneIndex;
                }
                defaultLuceneInfo.CategoriesIndexSearcher = luceneInfoDictionary[defaultCountryID].CategoriesIndexSearcher;
                defaultLuceneInfo.AttributesIndexSearcher = luceneInfoDictionary[defaultCountryID].AttributesIndexSearcher;
                defaultLuceneInfo.ProductRetailerMapIndexSearcher = luceneInfoDictionary[defaultCountryID].ProductRetailerMapIndexSearcher;
                defaultLuceneInfo.RetailerProductsIndexSearcher = luceneInfoDictionary[defaultCountryID].RetailerProductsIndexSearcher;
                defaultLuceneInfo.AllBrandsIndexSearcher = luceneInfoDictionary[defaultCountryID].AllBrandsIndexSearcher;
                defaultLuceneInfo.PopularIndexSearcher = luceneInfoDictionary[defaultCountryID].PopularIndexSearcher;
                luceneInfoDictionary.Add(defaultLuceneInfo.CountryID, defaultLuceneInfo);

                if (!includeCountry && ConfigAppString.CountryID != 0)
                {
                    throw new Exception("Country : " + ConfigAppString.CountryID + " not supported!");
                }

                foreach (var expireSearcher in expireSearcherList)
                {
                    if (expireSearcher != null)
                    {
                        expireSearcher.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                LogWriter.WriteLineToFile(ConfigAppString.ExceptionLogPath, ex.Message + "\t" + ex.StackTrace);
                return false;
            }
            return true;
        }

        private static Dictionary<int, Searcher> GetAllCategoriesProductLuceneIndex(Dictionary<int, LuceneInfo> _luceneInfoDictionary)
        {
            Dictionary<int, Searcher> allCategoriesProductLuceneIndex = new Dictionary<int, Searcher>();
            
            foreach (CategoryCache pc in CategoryController.AllActiveRootCategoriesOrderByName)
            {
                List<Searcher> searchers = new List<Searcher>();
                foreach (var luceneInfo in _luceneInfoDictionary.Values)
                {
                    if (luceneInfo.CategoriesProductLuceneIndex.ContainsKey(pc.CategoryID) && luceneInfo.CategoriesProductLuceneIndex[pc.CategoryID] != null)
                    {
                        searchers.Add(luceneInfo.CategoriesProductLuceneIndex[pc.CategoryID]);
                    }
                }
                if (searchers.Count > 0)
                {
                    ParallelMultiSearcher parallelMultiSearcher = new ParallelMultiSearcher(searchers.ToArray());
                    allCategoriesProductLuceneIndex.Add(pc.CategoryID, parallelMultiSearcher);
                }
            }
            List<Searcher> _searchers = new List<Searcher>();
            foreach (var luceneInfo in _luceneInfoDictionary.Values)
            {
                if (luceneInfo.CategoriesProductLuceneIndex.ContainsKey(0) && luceneInfo.CategoriesProductLuceneIndex[0] != null)
                {
                    _searchers.Add(luceneInfo.CategoriesProductLuceneIndex[0]);
                }
            }
            if (_searchers.Count > 0)
            {
                ParallelMultiSearcher allParallelMultiSearcher = new ParallelMultiSearcher(_searchers.ToArray());
                allCategoriesProductLuceneIndex.Add(0, allParallelMultiSearcher);
            }
            return allCategoriesProductLuceneIndex;
        }

        public static IndexSearcher GetIndexSearcher(string indexDirectory, bool useRamDirectory)
        {
            if (System.IO.Directory.Exists(indexDirectory) && System.IO.File.Exists(indexDirectory + "\\segments.gen"))
            {
                IndexSearcher indexSearcher = null;
                if (useRamDirectory)
                {
                    Lucene.Net.Store.FSDirectory fsDirectory = Lucene.Net.Store.FSDirectory.Open(new System.IO.DirectoryInfo(indexDirectory));
                    Lucene.Net.Store.RAMDirectory ramDirectory = new Lucene.Net.Store.RAMDirectory(fsDirectory);
                    indexSearcher = new IndexSearcher(ramDirectory, ConfigAppString.LuceneIndexReadOnly);
                }
                else
                {
                    Lucene.Net.Store.FSDirectory fsDirectory = Lucene.Net.Store.FSDirectory.Open(new System.IO.DirectoryInfo(indexDirectory));
                    indexSearcher = new IndexSearcher(fsDirectory, ConfigAppString.LuceneIndexReadOnly);
                }
                return indexSearcher;
            }
            else
            {
                LogWriter.WriteLineToFile(ConfigAppString.ExceptionLogPath, indexDirectory + " have no index file!");
            }
            return null;
        }

        public static IndexSearcher GetIndexSearcher(string indexDirectory)
        {
            return GetIndexSearcher(indexDirectory, false);
        }

        private static Dictionary<int, Searcher> GetCategoriesProductLuceneIndex(string indexRootPath)
        {
            Dictionary<int, Searcher> categoriesProductLuceneIndex = new Dictionary<int, Searcher>();
            //List<Searcher> searcherList = new List<Searcher>();
            foreach (CSK_Store_Category pc in AllActiveRootCategories)
            {
                string path = indexRootPath + "AllCategoriesProduct\\" + pc.CategoryName.Trim();
                if (!System.IO.Directory.Exists(path) || string.IsNullOrEmpty(pc.CategoryName) || !System.IO.File.Exists(path + "\\segments.gen"))
                {
                    LogWriter.WriteLineToFile(ConfigAppString.ExceptionLogPath, path + " - " + pc.CategoryName + " have no index file! --- " + DateTime.Now.ToString());
                    continue;
                }

                IndexSearcher pCategoryIndexSearcher = GetIndexSearcher(path);
                //searcherList.Add(pCategoryIndexSearcher);
                categoriesProductLuceneIndex.Add(pc.CategoryID, pCategoryIndexSearcher);
            }
            //if (searcherList.Count > 0)
            //{
            //    ParallelMultiSearcher allParallelMultiSearcher = new ParallelMultiSearcher(searcherList.ToArray());
            //    categoriesProductLuceneIndex.Add(0, allParallelMultiSearcher);
            //}

            string productsIndexPath = indexRootPath + "AllCategoriesProduct\\Products";
            if (!System.IO.Directory.Exists(productsIndexPath) || !System.IO.File.Exists(productsIndexPath + "\\segments.gen"))
            {
                LogWriter.WriteLineToFile(ConfigAppString.ExceptionLogPath, productsIndexPath + " have no index file!");
            }

            IndexSearcher productCategoryIndexSearcher = GetIndexSearcher(productsIndexPath);
            //searcherList.Add(pCategoryIndexSearcher);
            categoriesProductLuceneIndex.Add(0, productCategoryIndexSearcher);

            return categoriesProductLuceneIndex;
        }

        public static int DeleteProductFromIndex(string fieldName, string value)
        {
            Term term = new Term(fieldName, value);
            int deleted = 0;

            foreach (var luceneInfo in luceneInfoDictionary.Values)
            {
                foreach (IndexSearcher indexSearcher in luceneInfo.CategoriesProductLuceneIndex.Values)
                {
                    if (indexSearcher != null)
                    {
                        deleted += indexSearcher.IndexReader.DeleteDocuments(term);
                    }
                }
            }

            return deleted;
        }
    }
}