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
    /// <summary>
    /// 公共的配置
    /// </summary>
    public static class ConfigAppString
    {
        public static string EmailAddress { get; private set; }
        public static List<int> ListVersionNoEnglishCountryId { get; private set; }

        static ConfigAppString()
        {
            Init();
        }

        public static void Init()
        {
            EmailAddress = ConfigurationManager.AppSettings["EmailAddress"];

            //单词不需要加s 的国家
            string VersionNoEnglishCountryidString = ConfigurationManager.AppSettings["VersionNoEnglishCountryid"];
            if (!String.IsNullOrEmpty(VersionNoEnglishCountryidString))
            {
                string[] versionNoEnglishCountryid = VersionNoEnglishCountryidString.Split(',');
                ListVersionNoEnglishCountryId = new List<int>();

                foreach (string countryId in versionNoEnglishCountryid)
                {
                    int cid = 0;
                    int.TryParse(countryId, out cid);
                    if (cid != 0)
                        ListVersionNoEnglishCountryId.Add(cid);
                }
            }
        }
    }
}