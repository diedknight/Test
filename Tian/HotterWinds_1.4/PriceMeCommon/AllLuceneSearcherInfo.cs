using Lucene.Net.Search;
using Lucene.Net.Store;
using PriceMeCommon.BusinessLogic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Xml;

namespace PriceMeCommon
{
    public class AllLuceneSearcherInfo : IDisposable
    {
        public event IndexChangedDelegate OnIndexChanged;

        private AllLuceneSearcherInfo() { }

        CountriesNodeInfo mCountriesNodeInfo;
        Dictionary<int, LuceneSearcherInfo> luceneSearcherInfoDic;
        Thread mCheckModifyThread;
        Thread mCheckProductPriceThread;
        int interval;

        public LuceneSearcherInfo GetLuceneSearcherInfoByCountryId(int countryId)
        {
            if (luceneSearcherInfoDic.ContainsKey(countryId))
                return luceneSearcherInfoDic[countryId];

            return null;
        }

        void CheckProductPrice()
        {
            if (interval < 100)
                interval = 10000;

            while (true)
            {
                try
                {
                    foreach (var ci in mCountriesNodeInfo.CountryInfoListDic.Values)
                    {
                        if(ci.RealTimeUpdateIndex)
                        {
                            SearchController.UpdateIndex(ci.CountryId);
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogController.WriteException(ex.Message + "\t" + ex.StackTrace);
                }
                Thread.Sleep(interval);
            }
        }

        /// <summary>
        /// 开始检测Index的路径变化
        /// </summary>
        public void StartCheckPathModify()
        {
            mCheckModifyThread = new Thread(new ThreadStart(CheckPathModify));
            mCheckModifyThread.Start();
        }

        /// <summary>
        /// 检测Index的路径变化，如果有变化则更新luceneSearcherInfoDic
        /// </summary>
        void CheckPathModify()
        {
            if (interval < 100)
                interval = 10000;

            while (true)
            {
                try
                {
                    foreach (int countryId in luceneSearcherInfoDic.Keys)
                    {
                        var lsi = luceneSearcherInfoDic[countryId];
                        string newLuceneIndexPath = GetLuceneConfigValue(lsi.LuceneConfigPath, lsi.IndexKey);
                        if(newLuceneIndexPath != lsi.LuceneIndexPath)
                        {
                            LuceneSearcherInfo newlsi = new LuceneSearcherInfo(lsi.LuceneConfigPath, lsi.IndexKey);
                            luceneSearcherInfoDic[countryId] = newlsi;

                            LogController.WriteLog("Country: " + countryId + " lucene index path changed: " + newLuceneIndexPath + " at: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                            if(OnIndexChanged != null)
                            {
                                OnIndexChanged(countryId, newLuceneIndexPath);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogController.WriteException(ex.Message + "\t" + ex.StackTrace);
                }
                Thread.Sleep(interval);
            }
        }

        public void EndCheck()
        {
            if (mCheckModifyThread != null && mCheckModifyThread.IsAlive)
                mCheckModifyThread.Abort();

            if (mCheckProductPriceThread != null && mCheckProductPriceThread.IsAlive)
                mCheckProductPriceThread.Abort();
        }

        public void Dispose()
        {
            EndCheck();
        }

        public static AllLuceneSearcherInfo CreateFromCountriesNodeInfo(CountriesNodeInfo countriesNodeInfo)
        {
            AllLuceneSearcherInfo luceneSearcherInfo = new AllLuceneSearcherInfo();
            luceneSearcherInfo.mCountriesNodeInfo = countriesNodeInfo;
            luceneSearcherInfo.luceneSearcherInfoDic = new Dictionary<int, LuceneSearcherInfo>();
            luceneSearcherInfo.interval = countriesNodeInfo.Interval;

            foreach (CountryInfo ci in countriesNodeInfo.CountryInfoListDic.Values)
            {
                LuceneSearcherInfo lsi = new LuceneSearcherInfo(ci.ConfigPath, ci.IndexPathKey);

                luceneSearcherInfo.luceneSearcherInfoDic.Add(ci.CountryId, lsi);
            }

            //luceneSearcherInfo.mCheckProductPriceThread = new Thread(new ThreadStart(luceneSearcherInfo.CheckProductPrice));
            //luceneSearcherInfo.mCheckProductPriceThread.Start();

            return luceneSearcherInfo;
        }

        public static string GetLuceneConfigValue(string configPath, string appKey)
        {
            if (!File.Exists(configPath))
                return string.Empty;
            XmlDocument xmlDoc = new XmlDocument();

            xmlDoc.Load(configPath);

            XmlNode configNode = null;
            foreach (XmlNode xmlNode in xmlDoc.ChildNodes)
            {
                if (xmlNode.Name.Equals("configuration", StringComparison.InvariantCultureIgnoreCase))
                {
                    configNode = xmlNode;
                    break;
                }
            }

            XmlNode appSettings = null;
            foreach (XmlNode xmlNode in configNode.ChildNodes)
            {
                if (xmlNode.Name.Equals("appsettings", StringComparison.InvariantCultureIgnoreCase))
                {
                    appSettings = xmlNode;
                    break;
                }
            }

            foreach (XmlNode xmlNode in appSettings.ChildNodes)
            {
                if (xmlNode.Attributes[0].Value == appKey)
                {
                    return xmlNode.Attributes[1].Value;
                }
            }
            return string.Empty;
        }
    }

    /// <summary>
    /// 保存所有的Lucene IndexSearcher信息
    /// </summary>
    public class LuceneSearcherInfo
    {
        public string LuceneConfigPath { get; private set; }
        public string LuceneIndexPath { get; private set; }
        public string IndexKey { get; private set; }
        public string ProductIndexRootPath { get; private set; }

        public Dictionary<string, IndexSearcher> CategoriesProductLuceneIndexSearcherDic;
        public IndexSearcher AllCategoryProductsIndexSearcher;

        public readonly IndexSearcher AllBrandsIndexSearcher;
        public readonly IndexSearcher CategoriesIndexSearcher;
        public readonly IndexSearcher AttributesIndexSearcher;
        public readonly IndexSearcher ProductRetailerMapIndexSearcher;
        public readonly IndexSearcher RetailerProductsIndexSearcher;
        public readonly IndexSearcher PopularIndexSearcher;
        public readonly IndexSearcher ReviewAverageIndexSearcher;
        public readonly IndexSearcher ExpertReviewIndexSearcher;
        public readonly IndexSearcher ProductVideoIndexSearcher;

        public LuceneSearcherInfo(string luceneConfigPath, string indexKey)
        {
            LuceneConfigPath = luceneConfigPath;
            IndexKey = indexKey;

            LuceneIndexPath = AllLuceneSearcherInfo.GetLuceneConfigValue(luceneConfigPath, indexKey);

            string indexRootPath = AllLuceneSearcherInfo.GetLuceneConfigValue(luceneConfigPath, "IndexRootPath");
            string popularSearchIndexPath = AllLuceneSearcherInfo.GetLuceneConfigValue(luceneConfigPath, "PopularSearchIndexPath2");
            string logoVersion = AllLuceneSearcherInfo.GetLuceneConfigValue(luceneConfigPath, "LogoVersion");
            string expertReviewRootIndexPath = AllLuceneSearcherInfo.GetLuceneConfigValue(luceneConfigPath, "ExpertReviewIndexPath");
            string reviewAverageIndexPath = Path.Combine(expertReviewRootIndexPath, "ExpertAverage");
            string expertReviewIndexPath = Path.Combine(expertReviewRootIndexPath, "ExpertReview");
            string productVideoIndexPath = Path.Combine(expertReviewRootIndexPath, "ProductVideo");
            ProductIndexRootPath = Path.Combine(LuceneIndexPath, "AllCategoriesProduct");

            LoadProductIndex();

            CategoriesIndexSearcher = GetIndexSearcher(Path.Combine(LuceneIndexPath, "Categories"));
            AttributesIndexSearcher = GetIndexSearcher(Path.Combine(LuceneIndexPath, "ProductsDescriptor"));
            ProductRetailerMapIndexSearcher = GetIndexSearcher(Path.Combine(LuceneIndexPath, "ProductRetailerMap"));
            RetailerProductsIndexSearcher = GetIndexSearcher(Path.Combine(LuceneIndexPath, "RetailerProducts"));
            IndexSearcher allBrandsIndexSearcher;
            if(CategoriesProductLuceneIndexSearcherDic.TryGetValue("AllBrands", out allBrandsIndexSearcher))
            {
                AllBrandsIndexSearcher = allBrandsIndexSearcher;
            }
            else
            {
                AllBrandsIndexSearcher = null;
            }

            PopularIndexSearcher = GetIndexSearcher(popularSearchIndexPath);

            ReviewAverageIndexSearcher = GetIndexSearcher(reviewAverageIndexPath);
            ExpertReviewIndexSearcher = GetIndexSearcher(expertReviewIndexPath);
            ProductVideoIndexSearcher = GetIndexSearcher(productVideoIndexPath);
        }

        private Dictionary<string, IndexSearcher> CreateAllCategoryProductsLundexSearcher(string luceneIndexPath)
        {
            Dictionary<string, IndexSearcher> dic = new Dictionary<string, IndexSearcher>();

            if (System.IO.Directory.Exists(luceneIndexPath))
            {
                DirectoryInfo dirInfo = new DirectoryInfo(luceneIndexPath);
                DirectoryInfo[] subDirInfos = dirInfo.GetDirectories();
                foreach (DirectoryInfo di in subDirInfos)
                {
                    IndexSearcher searcher = GetIndexSearcher(di.FullName);
                    dic.Add(di.Name, searcher);
                }
            }

            return dic;
        }

        static IndexSearcher GetIndexSearcher(string indexDirectory)
        {
            return GetIndexSearcher(indexDirectory, false, true);
        }

        static IndexSearcher GetIndexSearcher(string indexDirectory, bool useRamDirectory, bool readOnly)
        {
            if (System.IO.Directory.Exists(indexDirectory) && File.Exists(indexDirectory + "\\segments.gen"))
            {
                IndexSearcher indexSearcher = null;
                if (useRamDirectory)
                {
                    FSDirectory fsDirectory = FSDirectory.Open(new DirectoryInfo(indexDirectory));
                    RAMDirectory ramDirectory = new RAMDirectory(fsDirectory);
                    Lucene.Net.Index.IndexReader indexReader = Lucene.Net.Index.DirectoryReader.Open(ramDirectory, readOnly);
                    indexSearcher = new IndexSearcher(indexReader);
                }
                else
                {
                    FSDirectory fsDirectory = FSDirectory.Open(new DirectoryInfo(indexDirectory));
                    Lucene.Net.Index.IndexReader indexReader = Lucene.Net.Index.DirectoryReader.Open(fsDirectory, readOnly);
                    indexSearcher = new IndexSearcher(indexReader);
                }
                return indexSearcher;
            }
            else
            {
                LogController.WriteException(indexDirectory + " have no index file!");
            }
            return null;
        }

        public void LoadProductIndex()
        {
            CategoriesProductLuceneIndexSearcherDic = CreateAllCategoryProductsLundexSearcher(ProductIndexRootPath);

            IndexSearcher allCategoryProductsIndexSearcher;
            if (CategoriesProductLuceneIndexSearcherDic.TryGetValue("Products", out allCategoryProductsIndexSearcher))
            {
                AllCategoryProductsIndexSearcher = allCategoryProductsIndexSearcher;
            }
            else
            {
                AllCategoryProductsIndexSearcher = null;
            }
        }

        public void ReLoadProductIndex()
        {
            LoadProductIndex();
        }
    }
}