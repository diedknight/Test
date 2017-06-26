using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web;

namespace PriceMe
{
    /// <summary>
    /// 只是网站所需要的配置写到这个文件，不要写到PriceMeCommon
    /// </summary>
    public static class WebConfig
    {
        public readonly static float SaleRate = float.Parse(ConfigurationManager.AppSettings["SaleRate"]);
        public readonly static float MinimumPrice = float.Parse(ConfigurationManager.AppSettings["minimumPrice"]);

        public readonly static string ParseAPPID = ConfigurationManager.AppSettings["ParseAPPID"];
        public readonly static string ParseNETSDK = ConfigurationManager.AppSettings["ParseNETSDK"];
        public readonly static string ParseJavascriptSDK = ConfigurationManager.AppSettings["ParseJavascriptSDK"];
        public readonly static string ParseServer = ConfigurationManager.AppSettings["ParseServer"];

        public readonly static string iContact_listid = ConfigurationManager.AppSettings["iContact_listid"];

        public readonly static string iContact_specialid_value = ConfigurationManager.AppSettings["iContact_specialid_value"];

        public readonly static string iContact_formid = ConfigurationManager.AppSettings["iContact_formid"];

        public readonly static string WEB_cssVersion = ConfigurationManager.AppSettings["cssVersion"];

        //GoogleAdsense
        public readonly static string GoogleAdsense = ConfigurationManager.AppSettings["GoogleAdsense"];
        public readonly static string GoogleAdsense2 = ConfigurationManager.AppSettings["GoogleAdsense2"];
        //google_ad_slot
        /// <summary>
        /// 页面顶部，logo旁边
        /// </summary>
        public readonly static string GoogleSlot_GoogleAds_Top = ConfigurationManager.AppSettings["GoogleSlot_GoogleAds_Top"];
        /// <summary>
        /// 产品页面右上角
        /// </summary>
        public readonly static string GoogleSlot_BannerRight = ConfigurationManager.AppSettings["GoogleSlot_BannerRight"];
        /// <summary>
        /// http://www.priceme.co.nz/Home-Garden/c-578.aspx 右边
        /// </summary>
        public readonly static string GoogleSlot_GoogleAds = ConfigurationManager.AppSettings["GoogleSlot_GoogleAds"];
        /// <summary>
        /// product.aspx，rp list 下面
        /// </summary>
        public readonly static string GoogleSlot_GoogleHorizontalAds = ConfigurationManager.AppSettings["GoogleSlot_GoogleHorizontalAds"];

        public readonly static string UUID = ConfigurationManager.AppSettings["UUID"];

        public static readonly int CountryId = int.Parse(ConfigurationManager.AppSettings["CountryID"]);
        public readonly static bool UrlSeo;
        public readonly static string PricemeLogo = ConfigurationManager.AppSettings["pricemeLogo"];
        public readonly static string CssJsPath = ConfigurationManager.AppSettings["CssJsPath"];
        public readonly static string ABTestingKey = ConfigurationManager.AppSettings["ABTestingKey"];
        public readonly static string TestFreaksUrl = ConfigurationManager.AppSettings["TestFreaksUrl"];
        public readonly static int NewDayCount = 35;
        public readonly static string BlogURLPath = ConfigurationManager.AppSettings["BlogURLPath"];
        public readonly static string ConsumerFeedUrl = ConfigurationManager.AppSettings["ConsumerFeedUrl"];
        public readonly static int QuickListCount = 20;

        public readonly static int Use_GoogleTrackConversion = int.Parse(ConfigurationManager.AppSettings["Use_GoogleTrackConversion"]);

        public readonly static string NPcategoryID = ConfigurationManager.AppSettings["NPcategoryID"];

        public static bool StartDebug { get; private set; }
        public static CultureInfo CurrentCulture { get; private set; }
        public static int ReducedPriceForEach { get; private set; }
        public static int ProductGMapZoom { get; private set; }
        public static decimal PriceChange { get; private set; }
        public static bool DisplayApp { get; set; }
        public static string Gclid { get; set; }
        public static bool IphoneSignup { get; set; }
        public static string GoogTagKey { get; set; }
        /// <summary>
        /// 配置当前环境, 决定是否显示googel Ads and facebook
        /// </summary>
        public static string Environment
        {
            get
            {
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["env"]))
                    return ConfigurationManager.AppSettings["env"];
                else return "prod";
            }
        }

        /// <summary>
        /// 配置当前环境, 决定是否显示Forum
        /// </summary>
        public static bool ShowForum
        {
            get
            {
                var showForum = false;
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["ShowForum"]))
                    bool.TryParse(ConfigurationManager.AppSettings["ShowForum"], out showForum);
                return showForum;
            }
        }

        /// <summary>
        /// 相当于 crm login id, 
        /// 可从crm上获得或修改:[My preferences] -> API Authentication Token
        /// crm 上的token 修改后, 对应的key 也要修改
        /// pamAccountGenerator,SeleniumTest 也用到了这个token
        /// SeleniumTest 用于删除测试账号
        /// </summary>
        public static string CRMToken
        {
            get
            {
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["CRMToken"]))
                    return ConfigurationManager.AppSettings["CRMToken"];
                else return string.Empty;
            }
        }

        static WebConfig()
        {
            string urlseo = ConfigurationManager.AppSettings["UrlSeo"];
            if (string.IsNullOrEmpty(urlseo))
                UrlSeo = true;
            else
                UrlSeo = bool.Parse(urlseo);

            bool startDebug;
            bool.TryParse(ConfigurationManager.AppSettings["StartDebug"], out startDebug);
            StartDebug = startDebug;

            switch (CountryId)
            {
                case 1:
                    CurrentCulture = CultureInfo.CreateSpecificCulture("en-AU");
                    break;
                case 3:
                    CurrentCulture = CultureInfo.CreateSpecificCulture("en-NZ");
                    break;
                case 28:
                    CurrentCulture = CultureInfo.CreateSpecificCulture("en-PH");
                    break;
                default:
                    CurrentCulture = CultureInfo.CurrentCulture;
                    break;
            }

            int quickListCount;
            if (int.TryParse(ConfigurationManager.AppSettings["QuickListCount"], out quickListCount))
            {
                QuickListCount = quickListCount;
            }

            int reducedPriceForEach;
            if (int.TryParse(ConfigurationManager.AppSettings["ReducedPriceForEach"], out reducedPriceForEach))
            {
                ReducedPriceForEach = reducedPriceForEach;
            }

            if (ReducedPriceForEach < 1)
            {
                ReducedPriceForEach = 1;
            }

            int productGMapZoom;
            if (int.TryParse(ConfigurationManager.AppSettings["productGMapZoom"], out productGMapZoom))
            {
                ProductGMapZoom = productGMapZoom;
            }
            else
            {
                ProductGMapZoom = 1;
            }

            decimal priceChange;
            if (decimal.TryParse(ConfigurationManager.AppSettings["PriceChange"].ToString(), out priceChange))
            {
                PriceChange = priceChange;
            }

            bool _displayApp;
            if (bool.TryParse(ConfigurationManager.AppSettings["DisplayApp"].ToString(), out _displayApp))
                DisplayApp = _displayApp;

            Gclid = ConfigurationManager.AppSettings["Gclid"].ToString();
            bool _iphoneSignup;
            if (bool.TryParse(ConfigurationManager.AppSettings["IphoneSignup"].ToString(), out _iphoneSignup))
                IphoneSignup = _iphoneSignup;

            GoogTagKey = ConfigurationManager.AppSettings["GoogTagKey"].ToString();
        }
    }
}