using Microsoft.Extensions.Configuration;
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
        }
    }
}
