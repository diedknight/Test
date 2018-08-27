using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SiteMap
{
    public static class SiteConfig
    {
        private static IConfigurationRoot _configRoot = null;

        static SiteConfig()
        {
            var builder = new ConfigurationBuilder()
             .SetBasePath(Directory.GetCurrentDirectory())
             .AddJsonFile("appsettings.json");
            _configRoot = builder.Build();
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
    }
}
