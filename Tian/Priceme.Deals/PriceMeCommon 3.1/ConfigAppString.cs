using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Globalization;
using PriceMeCommon.Data;
using System.Configuration;

namespace PriceMeCommon
{
    public static class ConfigAppString
    {
        public readonly static int NewDayCount = 35;

        static List<CountryInfo> _countryInfoList;

        static bool _useVelocity = false;

        static string _exceptionLogPath;
        static string _logDirectory;
        static string _logPath;

        static string _luceneConfigPath;
        static string _indexRootPath;
        static string _defaultInfoPath;
        static string _popularSearchIndexPath2;
        static bool _useCurrentDatePath = false;
        static bool _useCustomDatePath = false;
        static int _customDay = 0;
        static string _reviewAverageIndexPath;
        static string _expertReviewIndexPath;

        static int _countryID = 0;
        static string _dollarSign;
        static bool _luceneIndexReadOnly = true;
        static int _interval = 100000;

        static int _categoryHotSearchDays;
        static int _categoryHotSearchCount;
        static int _relatedManufacturerCategoriesCount;
        static int _quickListCount;

        static bool _urlSeo;
        static string _emailAddress;

        static string _velocityHostName;
        static int _velocityPort;
        static string _velocityCacheHostName;
        static string _velocityRegion;
        static string _logoVersion;
        static bool _financeWebsite;
        static int _productGMapZoom;
        static string _TestFreaksUrl;
        static bool _StartDebug;
        static string _ProductVideoIndexPath;
        static string _pricemeLogo;

        public static string ParseAPPID { get; set; }
        public static string ParseNETSDK { get; set; }
        public static string ParseJavascriptSDK { get; set; }

        public static string TestFreaksUrl
        {
            get { return ConfigAppString._TestFreaksUrl; }
            set { ConfigAppString._TestFreaksUrl = value; }
        }
        public static int ProductGMapZoom
        {
            get { return _productGMapZoom; }
        }
        public static string PricemeLogo
        {
            get { return _pricemeLogo; }
        }
        public static bool FinanceWebsite
        {
            get { return ConfigAppString._financeWebsite; }
        }

        public static string LogoVersion
        {
            get { return ConfigAppString._logoVersion; }
        }

        static string _retailerProductInfoPath;
        public static string RetailerProductInfoPath
        { get { return _retailerProductInfoPath; } }

        static string _bannerTop;
        public static string BannerTop
        { get { return _bannerTop; } }

        static string _bannerTopUrl;
        public static string BannerTopUrl
        { get { return _bannerTopUrl; } }

        static string _ABTestingKey;
        public static string ABTestingKey
        { get { return _ABTestingKey; } }

        static int _reducedPriceForEach;
        public static int ReducedPriceForEach
        { get { return _reducedPriceForEach; } }

        static List<int> _listVersionNoEnglishCountryid;
        public static List<int> ListVersionNoEnglishCountryid
        { get { return _listVersionNoEnglishCountryid; } }

        static string _CommerceTemplateCommon;
        public static string CommerceTemplateCommon
        {
            get { return _CommerceTemplateCommon; }
        }

        public static string CssJsPath { get; set; }

        #region ruangang
        static CultureInfo _currentCulture = CultureInfo.CurrentCulture;
        #endregion

        #region Config property
        public static List<CountryInfo> CountryInfoList
        {
            get { return ConfigAppString._countryInfoList; }
        }

        public static int QuickListCount
        {
            get { return ConfigAppString._quickListCount; }
        }

        public static string PopularSearchIndexPath2
        {
            get { return _popularSearchIndexPath2; }
        }

        public static int RelatedManufacturerCategoriesCount
        {
            get { return _relatedManufacturerCategoriesCount; }
        }

        public static int CategoryHotSearchCount
        {
            get { return _categoryHotSearchCount; }
        }

        public static int CategoryHotSearchDays
        {
            get { return _categoryHotSearchDays; }
        }

        public static bool UseCurrentDatePath
        {
            get { return _useCurrentDatePath; }
        }

        public static bool UseVelocity
        {
            get { return _useVelocity; }
        }

        public static string VelocityHostName
        {
            get { return _velocityHostName; }
        }

        public static int VelocityPort
        {
            get { return _velocityPort; }
        }

        public static string VelocityCacheHostName
        {
            get { return _velocityCacheHostName; }
        }

        public static string VelocityRegion
        {
            get { return _velocityRegion; }
        }

        public static string LogDirectory
        {
            get { return _logDirectory; }
        }

        public static bool LuceneIndexReadOnly
        {
            get { return _luceneIndexReadOnly; }
        }

        public static int CountryID
        {
            get { return _countryID; }
        }
        public static string DollarSign
        {
            get { return _dollarSign; }
        }

        public static bool UrlSeo
        {
            get { return _urlSeo; }
        }

        public static string EmailAddress
        {
            get { return _emailAddress; }
        }

        public static string LogPath
        {
            get { return _logPath; }
        }

        public static int Interval
        {
            get
            {
                return _interval;
            }
        }

        public static string IndexRootPath
        {
            get { return _indexRootPath; }
        }

        public static string LuceneConfigPath
        {
            get { return _luceneConfigPath; }
        }

        public static string ExceptionLogPath
        {
            get { return _exceptionLogPath; }
        }

        public static string ReviewAverageIndexPath
        { get { return _reviewAverageIndexPath; } }

        public static string ExpertReviewIndexPath
        { get { return _expertReviewIndexPath; } }

        public static string ProductVideoIndexPath
        { get { return _ProductVideoIndexPath; } }
        
        public static bool StartDebug
        { get { return _StartDebug; } }

        public static string ConsumerFeedUrl { get; set; }

        public static string BootstrapCssPath { get; set; }

        public static decimal PriceChange { get; set; }
        #region ruangang
        public static CultureInfo CurrentCulture {
            get { return _currentCulture; }
        }
        #endregion

        #endregion

        public static void SetLucenePath(string lucenePath)
        {
            foreach (var countryInfo in _countryInfoList)
            {
                countryInfo.LuceneIndexPath = lucenePath;
            }
        }

        public static CountryInfo GetCountryInfo(int countryID)
        {
            return _countryInfoList.Single(ci => ci.CountryID == countryID);
        }

        private static void LoadLuceneConfig()
        {
            if (!string.IsNullOrEmpty(_luceneConfigPath))
            {
                _indexRootPath = GetConfigValue(_luceneConfigPath, "IndexRootPath");
                _popularSearchIndexPath2 = GetConfigValue(_luceneConfigPath, "PopularSearchIndexPath2");
                _logoVersion = GetConfigValue(_luceneConfigPath, "LogoVersion");

                string useLatestLuceneDir_congfig = System.Configuration.ConfigurationManager.AppSettings["UseLatestLuceneDir"];
                bool useLatestLuceneDir = false;

                if (!string.IsNullOrEmpty(useLatestLuceneDir_congfig))
                {
                    useLatestLuceneDir = bool.Parse(useLatestLuceneDir_congfig);
                }

                if (useLatestLuceneDir)
                {
                    string latestLuceneDir = GetLatestLuceneDir(_indexRootPath);
                    if (!string.IsNullOrEmpty(latestLuceneDir))
                    {
                        _defaultInfoPath = latestLuceneDir;

                        foreach (var countryInfo in _countryInfoList)
                        {
                            countryInfo.LuceneIndexPath = _defaultInfoPath;
                        }
                    }
                    else
                    {
                        useLatestLuceneDir = false;
                    }
                }
                /*else if (_useCurrentDatePath)
                {
                    string currentDateString = DateTime.Now.ToString("yyyyMMdd") + "\\";
                    _defaultInfoPath = _indexRootPath + currentDateString;

                    foreach (var countryInfo in _countryInfoList)
                    {
                        countryInfo.LuceneIndexPath = _defaultInfoPath;
                    }
                }
                else if (_useCustomDatePath)
                {
                    _indexRootPath = ConfigurationManager.AppSettings["IndexRootPath"];
                    string customDateString = DateTime.Now.AddDays(_customDay).ToString("yyyyMMdd") + "\\";
                    _defaultInfoPath = _indexRootPath + customDateString;
                    foreach (var countryInfo in _countryInfoList)
                    {
                        countryInfo.LuceneIndexPath = _defaultInfoPath;
                    }
                }*/
                if(!useLatestLuceneDir)
                {
                    foreach (var countryInfo in _countryInfoList)
                    {
                        countryInfo.LuceneIndexPath = GetConfigValue(_luceneConfigPath, countryInfo.KeyName);
                    }
                    string il = GetConfigValue(_luceneConfigPath, "Interval");
                    if (string.IsNullOrEmpty(il))
                    {
                        throw new Exception("luceneConfigPath path : " + _luceneConfigPath + "\t Interval is empty!");
                    }
                    _interval = int.Parse(il);
                }

                string expertReviewRootIndexPath = (GetConfigValue(_luceneConfigPath, "ExpertReviewIndexPath"));
                _reviewAverageIndexPath = expertReviewRootIndexPath + "ExpertAverage";
                _expertReviewIndexPath = expertReviewRootIndexPath + "ExpertReview";
                _ProductVideoIndexPath = expertReviewRootIndexPath + "ProductVideo";
            }
            else
            {
                throw new Exception("luceneConfigPath path is empty!");
            }
        }

        private static string GetLatestLuceneDir(string indexRootPath)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(indexRootPath);
            DirectoryInfo[] subDirs = dirInfo.GetDirectories();
            DateTime latestDateTime = new DateTime(2014,1,1);
            DirectoryInfo latestDir = null;
            foreach(DirectoryInfo dir in subDirs)
            {
                if(dir.CreationTime > latestDateTime)
                {
                    if (IsValid(dir.FullName))
                    {
                        if (dir.GetDirectories().Count() > 1)
                        {
                            latestDir = dir;
                            latestDateTime = dir.CreationTime;
                        }
                    }
                }
            }
            if (latestDir != null)
            {
                return latestDir.FullName + "\\";
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// 是否是有效的Index目录
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        private static bool IsValid(string dir)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(dir);

            foreach(FileInfo fInfo in dirInfo.GetFiles())
            {
                if(fInfo.Name.Equals("write.lock"))
                {
                    return false;
                }
            }

            foreach(DirectoryInfo dInfo in dirInfo.GetDirectories())
            {
                if(!IsValid(dInfo.FullName))
                {
                    return false;
                }
            }

            return true;
        }

        public static bool Init()
        {
            string luceneCountryInfo = ConfigurationManager.AppSettings["LuceneCountryInfo"];
            string[] countryInfoStringArray = luceneCountryInfo.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            _countryInfoList = new List<CountryInfo>();
            foreach (var str in countryInfoStringArray)
            {
                CountryInfo countryInfo = new CountryInfo();
                string[] info = str.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                int countryID = int.Parse(info[0]);
                countryInfo.CountryID = countryID;
                countryInfo.KeyName = info[1];
                _countryInfoList.Add(countryInfo);
            }

            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["UseCurrentDatePath"]))
            {
                bool.TryParse(ConfigurationManager.AppSettings["UseCurrentDatePath"], out _useCurrentDatePath);
            }
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["UseCustomDatePath"]))
            {
                bool.TryParse(ConfigurationManager.AppSettings["UseCustomDatePath"], out _useCustomDatePath);
                int.TryParse(ConfigurationManager.AppSettings["CustomDay"], out _customDay);
            }
            _logDirectory = ConfigurationManager.AppSettings["LogDirectory"];
            //if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["UseNewzealandAndAustraliaIndex"]))
            //{
            //    _useNewzealandAndAustraliaIndex = bool.Parse(ConfigurationManager.AppSettings["UseNewzealandAndAustraliaIndex"]);
            //}
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["UseVelocity"]))
            {
                _useVelocity = bool.Parse(ConfigurationManager.AppSettings["UseVelocity"]);
            }
            _exceptionLogPath = _logDirectory + "Ex\\Excetpion" + DateTime.Now.ToString("yyyy-MM-dd_HH") + ".txt";
            _logPath = _logDirectory + "Log\\Log" + DateTime.Now.ToString("yyyy-MM-dd_HH") + ".txt";
            _luceneConfigPath = ConfigurationManager.AppSettings["LuceneConfigPath"];
            _retailerProductInfoPath = ConfigurationManager.AppSettings["RetailerProductInfoPath"];

            string urlseo = ConfigurationManager.AppSettings["UrlSeo"];
            if (string.IsNullOrEmpty(urlseo))
                _urlSeo = true;
            else
                _urlSeo = bool.Parse(urlseo);
            _emailAddress = ConfigurationManager.AppSettings["EmailAddress"];
            _countryID = int.Parse(ConfigurationManager.AppSettings["CountryID"]);
            _dollarSign = ConfigurationManager.AppSettings["DollarSign"];
            #region ruangang
            switch (_countryID) { 
                case 1:
                    _currentCulture = CultureInfo.CreateSpecificCulture("en-AU");
                    break;
                case 3:
                    _currentCulture = CultureInfo.CreateSpecificCulture("en-NZ");
                    break;
                case 28:
                     _currentCulture = CultureInfo.CreateSpecificCulture("en-PH");
                    break;
                default:
                    _currentCulture = CultureInfo.CurrentCulture;
                    break;
            }
            #endregion

            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["LuceneIndexReadOnly"]))
            {
                _luceneIndexReadOnly = bool.Parse(ConfigurationManager.AppSettings["LuceneIndexReadOnly"]);
            }

            _velocityHostName = ConfigurationManager.AppSettings["VelocityHostName"];
            _velocityPort = int.Parse(ConfigurationManager.AppSettings["VelocityPort"]);
            _velocityCacheHostName = ConfigurationManager.AppSettings["VelocityCacheHostName"];
            _velocityRegion = ConfigurationManager.AppSettings["VelocityRegion"];

            int.TryParse(ConfigurationManager.AppSettings["CategoryHotSearchDays"], out _categoryHotSearchDays);
            int.TryParse(ConfigurationManager.AppSettings["CategoryHotSearchCount"], out _categoryHotSearchCount);
            int.TryParse(ConfigurationManager.AppSettings["RelatedManufacturerCategories"], out _relatedManufacturerCategoriesCount);
            int.TryParse(ConfigurationManager.AppSettings["QuickListCount"], out _quickListCount);

            _bannerTop = ConfigurationManager.AppSettings["BannerTop"];
            _bannerTopUrl = ConfigurationManager.AppSettings["BannerTopUrl"];
            if (!string.IsNullOrEmpty(_bannerTopUrl))
            {
                _bannerTopUrl = _bannerTopUrl.Replace("and", "&");
            }

            bool.TryParse(ConfigurationManager.AppSettings["FinanceWebsite"], out _financeWebsite);
            int.TryParse(ConfigurationManager.AppSettings["productGMapZoom"], out _productGMapZoom);
            _TestFreaksUrl = ConfigurationManager.AppSettings["TestFreaksUrl"];
            bool.TryParse(ConfigurationManager.AppSettings["StartDebug"], out _StartDebug);
            _ABTestingKey = ConfigurationManager.AppSettings["ABTestingKey"];
            _pricemeLogo = ConfigurationManager.AppSettings["pricemeLogo"];
            int.TryParse(ConfigurationManager.AppSettings["ReducedPriceForEach"], out _reducedPriceForEach);
            if (_reducedPriceForEach == 0) _reducedPriceForEach = 1;
            LoadLuceneConfig();

            //单词不需要加s 的国家
            string VersionNoEnglishCountryidString = ConfigurationManager.AppSettings["VersionNoEnglishCountryid"];
            if (!String.IsNullOrEmpty(VersionNoEnglishCountryidString))
            {
                string[] versionNoEnglishCountryid = VersionNoEnglishCountryidString.Split(',');
                _listVersionNoEnglishCountryid = new List<int>();

                foreach (string countryId in versionNoEnglishCountryid)
                {
                    int cid = 0;
                    int.TryParse(countryId, out cid);
                    if (cid != 0)
                        _listVersionNoEnglishCountryid.Add(cid);
                }
            }

            if (ConfigurationManager.ConnectionStrings["CommerceTemplate_Common"] != null)
            {
                _CommerceTemplateCommon = ConfigurationManager.ConnectionStrings["CommerceTemplate_Common"].ConnectionString;
            }
            ParseAPPID = ConfigurationManager.AppSettings["ParseAPPID"];
            ParseNETSDK = ConfigurationManager.AppSettings["ParseNETSDK"];
            ParseJavascriptSDK = ConfigurationManager.AppSettings["ParseJavascriptSDK"];
            ConsumerFeedUrl = ConfigurationManager.AppSettings["ConsumerFeedUrl"];

            BootstrapCssPath = ConfigurationManager.AppSettings["BootstrapCssPath"];
            CssJsPath = ConfigurationManager.AppSettings["CssJsPath"];

            decimal pricechange = 0m;
            decimal.TryParse(ConfigurationManager.AppSettings["PriceChange"].ToString(), out pricechange);
            PriceChange = pricechange;

            return true;
        }

        public static string GetIndexPath(int country)
        {
            foreach (var countryInfo in _countryInfoList)
            {
                if (countryInfo.CountryID == country)
                {
                    return countryInfo.LuceneIndexPath;
                }
            }
            return "";
        }

        public static string GetRealTimeIndexPath(int country)
        {
            foreach (var countryInfo in _countryInfoList)
            {
                if (countryInfo.CountryID == country)
                {
                    return GetConfigValue(_luceneConfigPath, countryInfo.KeyName);
                }
            }
            return "";
        }

        public static string GetConfigValue(string configFilePath, string appKey)
        {
            if (!File.Exists(configFilePath))
                return string.Empty;
            System.Xml.XmlDocument xmlDoc = new XmlDocument();

            xmlDoc.Load(configFilePath);

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

        static ConfigAppString()
        {
            Init();
        }
    }
}