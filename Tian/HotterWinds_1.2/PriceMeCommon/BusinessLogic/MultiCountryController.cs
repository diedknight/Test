using Lucene.Net.Search;
using SubSonic.DataProviders;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceMeCommon.BusinessLogic
{
    /// <summary>
    /// 使用前需要调用Load()方法
    /// </summary>
    public static class MultiCountryController
    {
        public static event IndexChangedDelegate OnIndexChanged;

        //保存配置文件国家节点信息
        static CountriesNodeInfo CountriesNodeInfo_Static;
        //保存所有配置的国家LuceneIndexSearcher的信息
        static AllLuceneSearcherInfo AllLuceneSearcherInfo_Static;
        //所有的国家id
        public static List<int> CountryIdList { get; private set; }
        //国家id对应的VelocityController
        static Dictionary<int, VelocityController> VelocityControllerDic_Static;

        /// <summary>
        /// 公共数据库的链接字符串
        /// </summary>
        public static ConnectionStringSettings CommonConnectionStringSettings_Static { get; private set; }

        static void Load()
        {
            CountriesNodeInfo_Static = (CountriesNodeInfo)ConfigurationManager.GetSection("countries");
            CountryIdList = CountriesNodeInfo_Static.CountryInfoListDic.Values.Select(ci => ci.CountryId).ToList();

            AllLuceneSearcherInfo_Static = AllLuceneSearcherInfo.CreateFromCountriesNodeInfo(CountriesNodeInfo_Static);
            CommonConnectionStringSettings_Static = ConfigurationManager.ConnectionStrings["PriceMeDB_Common"];

            VelocityControllerDic_Static = new Dictionary<int, VelocityController>();
            foreach (var ci in CountriesNodeInfo_Static.CountryInfoListDic.Values)
            {
                VelocityController vc = new VelocityController(ci.VelocityInfo);
                VelocityControllerDic_Static.Add(ci.CountryId, vc);
            }
        }

        public static void LoadForBuildIndex()
        {
            CountriesNodeInfo_Static = (CountriesNodeInfo)ConfigurationManager.GetSection("countries");
            CountryIdList = CountriesNodeInfo_Static.CountryInfoListDic.Values.Select(ci => ci.CountryId).ToList();

            CommonConnectionStringSettings_Static = ConfigurationManager.ConnectionStrings["PriceMeDB_Common"];

            VelocityControllerDic_Static = new Dictionary<int, VelocityController>();
            foreach (var ci in CountriesNodeInfo_Static.CountryInfoListDic.Values)
            {
                VelocityController vc = new VelocityController(ci.VelocityInfo);
                VelocityControllerDic_Static.Add(ci.CountryId, vc);
            }
        }

        public static void LoadWithCheckIndexPath()
        {
            Load();
            AllLuceneSearcherInfo_Static.StartCheckPathModify();
            AllLuceneSearcherInfo_Static.OnIndexChanged += AllLuceneSearcherInfo_Static_OnIndexChanged;
        }

        private static void AllLuceneSearcherInfo_Static_OnIndexChanged(int countryId, string newLuceneIndexPath)
        {
            if(OnIndexChanged != null)
            {
                OnIndexChanged(countryId, newLuceneIndexPath);
            }
        }

        public static void LoadWithoutCheckIndexPath()
        {
            Load();
        }

        public static bool HasFinanceSite(int countryId)
        {
            if (CountriesNodeInfo_Static.CountryInfoListDic.ContainsKey(countryId))
                return CountriesNodeInfo_Static.CountryInfoListDic[countryId].Finance;

            return false;
        }

        public static VelocityController GetVelocityController(int countryId)
        {
            if(VelocityControllerDic_Static.ContainsKey(countryId))
            {
                return VelocityControllerDic_Static[countryId];
            }

            return null;
        }

        /// <summary>
        /// 判断是否是配置文件中存在的国家
        /// </summary>
        /// <param name="countryId"></param>
        /// <returns></returns>
        public static bool IsExistCountry(int countryId)
        {
            return CountryIdList.Contains(countryId);
        }

        public static IndexSearcher GetRootCategoryProductsLuceneSearcher(string rootCategoryName, int countryId)
        {
            LuceneSearcherInfo luceneSearcherInfo = AllLuceneSearcherInfo_Static.GetLuceneSearcherInfoByCountryId(countryId);
            if(luceneSearcherInfo != null && luceneSearcherInfo.CategoriesProductLuceneIndexSearcherDic.ContainsKey(rootCategoryName))
            {
                return luceneSearcherInfo.CategoriesProductLuceneIndexSearcherDic[rootCategoryName];
            }

            return null;
        }

        public static IndexSearcher GetAllBrandsLuceneSearcher(int countryId)
        {
            LuceneSearcherInfo luceneSearcherInfo = AllLuceneSearcherInfo_Static.GetLuceneSearcherInfoByCountryId(countryId);
            if (luceneSearcherInfo != null)
            {
                return luceneSearcherInfo.AllBrandsIndexSearcher;
            }

            return null;
        }

        public static IndexSearcher GetAllCategoryProductsIndexSearcher(int countryId)
        {
            LuceneSearcherInfo luceneSearcherInfo = AllLuceneSearcherInfo_Static.GetLuceneSearcherInfoByCountryId(countryId);
            if (luceneSearcherInfo != null)
            {
                return luceneSearcherInfo.AllCategoryProductsIndexSearcher;
            }

            return null;
        }

        public static IndexSearcher GetAttributesLuceneSearcher(int countryId)
        {
            LuceneSearcherInfo luceneSearcherInfo = AllLuceneSearcherInfo_Static.GetLuceneSearcherInfoByCountryId(countryId);
            if (luceneSearcherInfo != null)
            {
                return luceneSearcherInfo.AttributesIndexSearcher;
            }

            return null;
        }

        public static IndexSearcher GetCategoriesLuceneSearcher(int countryId)
        {
            LuceneSearcherInfo luceneSearcherInfo = AllLuceneSearcherInfo_Static.GetLuceneSearcherInfoByCountryId(countryId);
            if (luceneSearcherInfo != null)
            {
                return luceneSearcherInfo.CategoriesIndexSearcher;
            }

            return null;
        }

        public static IndexSearcher GetExpertReviewLuceneSearcher(int countryId)
        {
            LuceneSearcherInfo luceneSearcherInfo = AllLuceneSearcherInfo_Static.GetLuceneSearcherInfoByCountryId(countryId);
            if (luceneSearcherInfo != null)
            {
                return luceneSearcherInfo.ExpertReviewIndexSearcher;
            }

            return null;
        }

        public static IndexSearcher GetPopularLuceneSearcher(int countryId)
        {
            LuceneSearcherInfo luceneSearcherInfo = AllLuceneSearcherInfo_Static.GetLuceneSearcherInfoByCountryId(countryId);
            if (luceneSearcherInfo != null)
            {
                return luceneSearcherInfo.PopularIndexSearcher;
            }

            return null;
        }

        public static IndexSearcher GetProductRetailerMapLuceneSearcher(int countryId)
        {
            LuceneSearcherInfo luceneSearcherInfo = AllLuceneSearcherInfo_Static.GetLuceneSearcherInfoByCountryId(countryId);
            if (luceneSearcherInfo != null)
            {
                return luceneSearcherInfo.ProductRetailerMapIndexSearcher;
            }

            return null;
        }

        public static IndexSearcher GetProductVideoLuceneSearcher(int countryId)
        {
            LuceneSearcherInfo luceneSearcherInfo = AllLuceneSearcherInfo_Static.GetLuceneSearcherInfoByCountryId(countryId);
            if (luceneSearcherInfo != null)
            {
                return luceneSearcherInfo.ProductVideoIndexSearcher;
            }

            return null;
        }

        public static IndexSearcher GetRetailerProductsLuceneSearcher(int countryId)
        {
            LuceneSearcherInfo luceneSearcherInfo = AllLuceneSearcherInfo_Static.GetLuceneSearcherInfoByCountryId(countryId);
            if (luceneSearcherInfo != null)
            {
                return luceneSearcherInfo.RetailerProductsIndexSearcher;
            }

            return null;
        }

        public static IndexSearcher GetReviewAverageLuceneSearcher(int countryId)
        {
            LuceneSearcherInfo luceneSearcherInfo = AllLuceneSearcherInfo_Static.GetLuceneSearcherInfoByCountryId(countryId);
            if (luceneSearcherInfo != null)
            {
                return luceneSearcherInfo.ReviewAverageIndexSearcher;
            }

            return null;
        }

        public static IDataProvider GetDBProvider(int countryId)
        {
            CountryInfo countryInfo = CountriesNodeInfo_Static.GetCountryInfo(countryId);
            if (countryInfo != null)
            {
                return ProviderFactory.GetProvider(countryInfo.MyConnectionStringSettings.Name);
            }

            return null;
        }

        public static string GetDBProviderName(int countryId)
        {
            CountryInfo countryInfo = CountriesNodeInfo_Static.GetCountryInfo(countryId);
            if (countryInfo != null)
            {
                return countryInfo.MyConnectionStringSettings.ProviderName;
            }

            return null;
        }

        public static string GetDBConnectionString(int countryId)
        {
            CountryInfo countryInfo = CountriesNodeInfo_Static.GetCountryInfo(countryId);
            if (countryInfo != null)
            {
                return countryInfo.MyConnectionStringSettings.ConnectionString;
            }

            return null;
        }

        public static string GetCountryInfoLogoVersion(int countryId)
        {
            CountryInfo countryInfo = CountriesNodeInfo_Static.GetCountryInfo(countryId);
            if (countryInfo != null)
            {
                return AllLuceneSearcherInfo.GetLuceneConfigValue(countryInfo.ConfigPath, "LogoVersion");
            }

            return "";
        }

        public static string GetCurrentIndexPath(int countryId)
        {
            LuceneSearcherInfo luceneSearcherInfo = AllLuceneSearcherInfo_Static.GetLuceneSearcherInfoByCountryId(countryId);
            if (luceneSearcherInfo != null)
            {
                return luceneSearcherInfo.LuceneIndexPath;
            }

            return "";
        }

        public static string GetIndexKey(int countryId)
        {
            CountryInfo countryInfo = CountriesNodeInfo_Static.GetCountryInfo(countryId);
            if (countryInfo != null)
            {
                return countryInfo.IndexPathKey;
            }

            return "";
        }
    }
}
