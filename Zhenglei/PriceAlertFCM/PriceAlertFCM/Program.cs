using Parse;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace PriceAlertFCM
{
    class Program
    {
        static int CountryId = int.Parse(ConfigurationManager.AppSettings["CountryId"].ToString());
        static string ParseAPPID = ConfigurationManager.AppSettings["ParseAPPID"];
        static string ParseNETSDK = ConfigurationManager.AppSettings["ParseNETSDK"];
        static string ParseServer = ConfigurationManager.AppSettings["ParseServer"];

        static void Main(string[] args)
        {
            if (!string.IsNullOrEmpty(ParseServer))
            {
                ParseClient.Initialize(new ParseClient.Configuration
                {
                    ApplicationId = ParseAPPID,
                    Server = ParseServer
                });
            }
            else
            {
                ParseClient.Initialize(ParseAPPID, ParseNETSDK);
            }

            string logPath = ConfigurationManager.AppSettings["LogPath"].ToString();
            System.Diagnostics.Process.GetCurrentProcess().PriorityClass = System.Diagnostics.ProcessPriorityClass.BelowNormal;

            string date = DateTime.Now.ToString("yyyyMMddHH");

            if (!Directory.Exists(logPath))
                Directory.CreateDirectory(logPath);
            StreamWriter sw = new StreamWriter(logPath + "\\" + date + ".txt");

            ProductAlert pa = new ProductAlert();
            pa.SW = sw;
            pa.Alert();
        }
    }
}