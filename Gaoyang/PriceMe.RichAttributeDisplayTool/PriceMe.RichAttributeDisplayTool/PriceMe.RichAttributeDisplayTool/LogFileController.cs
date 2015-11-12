using System;
using System.IO;
using System.Configuration;

namespace PriceMe.RichAttributeDisplayTool
{
    public  class LogFileController
    {
        private  string SiteMap = System.Configuration.ConfigurationManager.AppSettings["RichAttributeLog"].ToString();
        private StreamWriter success;
        private StreamWriter failed;

        public LogFileController() {
            if (!Directory.Exists(SiteMap))
                Directory.CreateDirectory(SiteMap);
            failed = new StreamWriter(SiteMap + "\\RichAttribute-Fail-" + DateTime.Now.ToString("yyyy-MM-dd") + ".xml");
        }

        public LogFileController(string succ)
        {
            if (!Directory.Exists(SiteMap))
                Directory.CreateDirectory(SiteMap);
            success = new StreamWriter(SiteMap + "\\RichAttribute-" + succ + "-" + DateTime.Now.ToString("yyyy-MM-dd") + ".xml");
        }


        public  void succClose()
        {
            success.Close();
        }

        public  void writeSuccInfo(string info)
        {
            //System.Console.WriteLine(info);
            success.WriteLine(info);
            success.Flush();
        }

        public  void failedClose()
        {
            failed.Close();
        }

        public  void writeFailedInfo(string info)
        {
            failed.WriteLine(info);
            failed.Flush();
        }

       
    }
}
