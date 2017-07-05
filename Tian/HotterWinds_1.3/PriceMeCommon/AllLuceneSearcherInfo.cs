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

        Dictionary<int, LuceneSearcherInfo> luceneSearcherInfoDic;
        Thread checkModifyThread;
        int interval;

        public LuceneSearcherInfo GetLuceneSearcherInfoByCountryId(int countryId)
        {
            if (luceneSearcherInfoDic.ContainsKey(countryId))
                return luceneSearcherInfoDic[countryId];

            return null;
        }

        /// <summary>
        /// 开始检测Index的路径变化
        /// </summary>
        public void StartCheckPathModify()
        {
            checkModifyThread = new Thread(new ThreadStart(CheckPathModify));
            checkModifyThread.Start();
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
            if (checkModifyThread != null && checkModifyThread.IsAlive)
                checkModifyThread.Abort();
        }

        public void Dispose()
        {
            EndCheck();
        }

        public static AllLuceneSearcherInfo CreateFromCountriesNodeInfo(CountriesNodeInfo countriesNodeInfo)
        {
            AllLuceneSearcherInfo luceneSearcherInfo = new AllLuceneSearcherInfo();
            luceneSearcherInfo.luceneSearcherInfoDic = new Dictionary<int, LuceneSearcherInfo>();
            luceneSearcherInfo.interval = countriesNodeInfo.Interval;

            foreach (CountryInfo ci in countriesNodeInfo.CountryInfoListDic.Values)
            {
                LuceneSearcherInfo lsi = new LuceneSearcherInfo(ci.ConfigPath, ci.IndexPathKey);

                luceneSearcherInfo.luceneSearcherInfoDic.Add(ci.CountryId, lsi);
            }

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

        public readonly Dictionary<string, IndexSearcher> CategoriesProductLuceneIndexSearcherDic;
        public readonly IndexSearcher AllCategoryProductsIndexSearcher;
        public readonly IndexSearcher CategoriesIndexSearcher;
        public readonly IndexSearcher AttributesIndexSearcher;
        public readonly IndexSearcher ProductRetailerMapIndexSearcher;
        public readonly IndexSearcher RetailerProductsIndexSearcher;
        public readonly IndexSearcher AllBrandsIndexSearcher;
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

            CategoriesProductLuceneIndexSearcherDic = CreateAllCategoryProductsLundexSearcher(Path.Combine(LuceneIndexPath, "AllCategoriesProduct"));
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

            IndexSearcher allCategoryProductsIndexSearcher;
            if (CategoriesProductLuceneIndexSearcherDic.TryGetValue("Products", out allCategoryProductsIndexSearcher))
            {
                AllCategoryProductsIndexSearcher = allCategoryProductsIndexSearcher;
            }
            else
            {
                AllCategoryProductsIndexSearcher = null;
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
                    indexSearcher = new IndexSearcher(ramDirectory, readOnly);
                }
                else
                {
                    FSDirectory fsDirectory = FSDirectory.Open(new DirectoryInfo(indexDirectory));
                    indexSearcher = new IndexSearcher(fsDirectory, readOnly);
                }
                return indexSearcher;
            }
            else
            {
                LogController.WriteException(indexDirectory + " have no index file!");
            }
            return null;
        }
    }
}
