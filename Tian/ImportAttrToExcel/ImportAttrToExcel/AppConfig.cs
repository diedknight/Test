using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace ImportAttrToExcel
{
    public class AppConfig
    {
        public static string DR_Name { get; set; }
        public static int RID { get; set; }

        public static List<CIdUrl> CidAndProductURL { get; set; }
        public static List<string> Unit { get; set; }
        public static List<string> MultipleAttributes { get; set; }
        public static string LogPath { get; set; }

        static AppConfig()
        {
            DR_Name = ConfigurationManager.AppSettings["DR_Name"];
            RID = Convert.ToInt32(ConfigurationManager.AppSettings["RID"]);

            CidAndProductURL = new List<CIdUrl>();
            ConfigurationManager.AppSettings["CidAndProductURL"].Split(';').ToList().ForEach(item =>
            {
                if (!string.IsNullOrEmpty(item))
                {
                    var tempVals = item.Split(',');
                    CIdUrl cidUrl = new CIdUrl();
                    cidUrl.CId = Convert.ToInt32(tempVals[0]);
                    cidUrl.ProductUrl = tempVals[1];

                    CidAndProductURL.Add(cidUrl);
                }
            });

            Unit = new List<string>();
            ConfigurationManager.AppSettings["unit"].Split(';').ToList().ForEach(item =>
            {
                Unit.Add(item);
            });

            MultipleAttributes = new List<string>();
            ConfigurationManager.AppSettings["MultipleAttributes"].Split(';').ToList().ForEach(item =>
            {
                if (!string.IsNullOrEmpty(item))
                {
                    MultipleAttributes.Add(item);
                }
            });

            LogPath = ConfigurationManager.AppSettings["LogPath"];
        }


        public class CIdUrl
        {
            public int CId { get; set; }
            public string ProductUrl { get; set; }
        }

    }


}
