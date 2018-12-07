using Microsoft.Extensions.Configuration;
using PriceMeCommon;
using PriceMeCommon.BusinessLogic;
using PriceMeCommon.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ChristmasSite
{
    public class SiteConfig
    {
        private static IConfigurationRoot _configRoot = null;

        public static string BlackFridayUrl { get; set; }
        public static bool IsDispaly { get; set; }
        public static string LogTitle { get; set; }
        public static string LogUrl { get; set; }
        public static string PriceMeUrl { get; set; }
        public static string CssVersion { get; set; }
        public static string Year { get; set; }
        public static string FridayDay { get; set; }
        static SiteConfig()
        {
            var builder = new ConfigurationBuilder()
             .SetBasePath(Directory.GetCurrentDirectory())
             .AddJsonFile("appsettings.json");
            _configRoot = builder.Build();

            Init();
        }

        public static string AppSettings(string key)
        {
            string str = string.Empty;
            if (_configRoot.GetSection("appSettings:" + key) != null)
            {
                str = _configRoot.GetSection("appSettings:" + key).Value;
            }
            return str;
        }

        public static string ConnectionStrings(string key)
        {
            string str = string.Empty;
            if (_configRoot.GetConnectionString(key) != null)
            {
                str = _configRoot.GetConnectionString(key);
            }
            return str;
        }

        private static void Init()
        {
            bool _IsDispaly = false;
            bool.TryParse(AppSettings("IsDisplay"), out _IsDispaly);
            IsDispaly = _IsDispaly;

            BlackFridayUrl = AppSettings("BlackFridayUrl");
            LogTitle = AppSettings("LogTitle");
            LogUrl = AppSettings("LogUrl");
            PriceMeUrl = AppSettings("PriceMeUrl");
            CssVersion = AppSettings("CssVersion");
            Year = AppSettings("Year");
            FridayDay = AppSettings("FridayDay");

            ConfigAppString.Init(_configRoot);
            LogController.Init(_configRoot["LogDirectory"]);

            var countriesSection = _configRoot.GetSection("Countries");
            var childrens = countriesSection.GetChildren();

            List<CountryInfo> ciList = new List<CountryInfo>();
            var cs = childrens.GetEnumerator();
            while (cs.MoveNext())
            {
                var cc = cs.Current.Get<CountryInfo>();
                ciList.Add(cc);
            }
            CountriesNodeInfo countriesNodeInfo = new CountriesNodeInfo(ciList);

            LogController.WriteLog(countriesNodeInfo.ToString() + " at : " + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "\r\n");

            DbInfo pamUserDbInfo = _configRoot.GetSection("PamUserDbInfo").Get<DbInfo>();

            bool checkIndexEmail = false;
            bool notCheckPriceChange = true;

            WatchTimer dkTimer = new WatchTimer();

            LogController.WriteLog("Start try at : " + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "\r\n");
            try
            {
                dkTimer.Start();

                dkTimer.Set(" ------ Website start. ------ ");

                dkTimer.Set("Start load Cache");

                MultiCountryController.LoadWithCheckIndexPath(countriesNodeInfo, pamUserDbInfo, ConfigAppString.Interval, checkIndexEmail, notCheckPriceChange);

                dkTimer.Set("Befor load Category Cache");
                CategoryController.Load(dkTimer);

                dkTimer.Set("End load Cache");

                dkTimer.Set(" ------ Website start end. ------ ");

                dkTimer.Stop();

                LogController.WriteLog("------------\r\n" + dkTimer.OutputText() + "\r\n------------ at : " + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "\r\n");
            }
            catch (Exception ex)
            {
                string msg = ex.Message + "-----------" + ex.StackTrace;
                LogController.WriteException("------------\n" + msg);
                if (ex.InnerException != null)
                {
                    LogController.WriteException("\t--------\nInnerException : " + ex.InnerException.Message + "-----------" + ex.InnerException.StackTrace + "\n");
                }

                dkTimer.Stop();
            }
        }
    }
}
