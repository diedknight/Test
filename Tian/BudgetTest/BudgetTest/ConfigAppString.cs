using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetTest
{
    public class ConfigAppString
    {
        public static string BudgetReportEmail { get; set; }
        public static bool SendBudgetEmail { get; set; }

        public static string Subject { get; set; }

        static ConfigAppString()
        {
            BudgetReportEmail = GetAppSetting("BudgetReportEmail");
            SendBudgetEmail = Convert.ToInt32(GetAppSetting("SendBudgetEmail")) != 0;

            Subject = GetAppSetting("Subject");
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
