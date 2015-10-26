using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GhostInspectorTool
{
    public class Config
    {
        public static string GetAppSetting(string key)
        {
            string value = ConfigurationManager.AppSettings[key];

            if (string.IsNullOrEmpty(value)) return "";

            return value;
        }
    }
}
