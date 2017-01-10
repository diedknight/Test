using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace PriceMe
{
    public static class WebConfig
    {
        public static string iContact_listid = ConfigurationManager.AppSettings["iContact_listid"];

        public static string iContact_specialid_value = ConfigurationManager.AppSettings["iContact_specialid_value"];

        public static string iContact_formid = ConfigurationManager.AppSettings["iContact_formid"];

        public static string WEB_cssVersion = ConfigurationManager.AppSettings["cssVersion"];

        //GoogleAdsense
        public static string GoogleAdsense = ConfigurationManager.AppSettings["GoogleAdsense"];
        public static string GoogleAdsense2 = ConfigurationManager.AppSettings["GoogleAdsense2"];
        //google_ad_slot
        /// <summary>
        /// 页面顶部，logo旁边
        /// </summary>
        public static string GoogleSlot_GoogleAds_Top = ConfigurationManager.AppSettings["GoogleSlot_GoogleAds_Top"];
        /// <summary>
        /// 产品页面右上角
        /// </summary>
        public static string GoogleSlot_BannerRight = ConfigurationManager.AppSettings["GoogleSlot_BannerRight"];
        /// <summary>
        /// http://www.priceme.co.nz/Home-Garden/c-578.aspx 右边
        /// </summary>
        public static string GoogleSlot_GoogleAds = ConfigurationManager.AppSettings["GoogleSlot_GoogleAds"];
        /// <summary>
        /// product.aspx，rp list 下面
        /// </summary>
        public static string GoogleSlot_GoogleHorizontalAds = ConfigurationManager.AppSettings["GoogleSlot_GoogleHorizontalAds"];

        public static string UUID = ConfigurationManager.AppSettings["UUID"];

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
    }
}