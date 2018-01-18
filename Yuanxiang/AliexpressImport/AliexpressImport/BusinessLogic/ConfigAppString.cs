using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace AliexpressImport.BusinessLogic
{
    public static class ConfigAppString
    {
        public static string LogLocation { get; set; }
        public static float EndTaskTime { get; set; }
        public static string TargetIP { get; set; }
        public static string TargetPath { get; set; }
        public static string UserID { get; set; }
        public static string Password { get; set; }
        public static decimal Rate { get; set; }
        public static int Day { get; set; }

        static ConfigAppString()
        {
            Init();
        }

        public static void Init()
        {
            LogLocation = ConfigurationManager.AppSettings["LogLocation"].ToString();
            EndTaskTime = GetAppSetting("EndTaskTime", 0f);

            string stringRate = ConfigurationManager.AppSettings["Rate"].ToString().Replace("%", "");
            decimal _rate = 0m;
            decimal.TryParse(stringRate, out _rate);
            Rate = decimal.Round((_rate / 100), 2);

            int _day = 0;
            int.TryParse(ConfigurationManager.AppSettings["Day"].ToString(), out _day);
            Day = _day;
        }

        private static float GetAppSetting(string key, float defVal)
        {
            if (ConfigurationManager.AppSettings[key] != null)
            {
                string val = ConfigurationManager.AppSettings[key].ToString();

                float result = 0f;
                if (float.TryParse(val, out result))
                {
                    return result;
                }
            }

            return defVal;
        }
    }
}
