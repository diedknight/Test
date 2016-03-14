using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetailerDataReview
{
    public class ConfigAppString
    {
        public static int[] RetailerIds { get; private set; }
        public static string Subject { get; private set; }
        public static int CountryId { get; private set; }

        public static string DebugEmail { get; private set; }

        public static string SuccessUrl { get; private set; }

        static ConfigAppString()
        {
            string retailersStr = GetAppSetting("Retailers");
            if (retailersStr != "")
            {
                List<int> retailerTempList = new List<int>();
                retailersStr.Split(',').ToList().ForEach(item => {
                    retailerTempList.Add(Convert.ToInt32(item));
                });

                RetailerIds = retailerTempList.ToArray();
            }

            Subject = GetAppSetting("Subject");
            CountryId = Convert.ToInt32(GetAppSetting("CountryId", "3"));
            DebugEmail = GetAppSetting("DebugEmail");
            SuccessUrl = GetAppSetting("SuccessUrl");
        }

        private static string GetAppSetting(string key, string defVal = "")
        {
            return ConfigurationManager.AppSettings[key] == null ? defVal : ConfigurationManager.AppSettings[key].ToString();
        }

        public static string GetConnection(string name)
        {
            return ConfigurationManager.ConnectionStrings[name].ConnectionString;
        }

    }
}
