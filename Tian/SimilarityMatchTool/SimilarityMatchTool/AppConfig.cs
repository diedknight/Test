using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimilarityMatchTool
{
    public class AppConfig
    {
        public static double W1 { get; set; }
        public static double W2 { get; set; }
        public static double A { get; set; }

        public static string CID { get; set; }

        public static string DBStr { get; set; }

        public static double Score { get; set; }

        static AppConfig()
        {
            W1 = Convert.ToDouble(System.Configuration.ConfigurationManager.AppSettings["W1"]);
            W2 = Convert.ToDouble(System.Configuration.ConfigurationManager.AppSettings["W2"]);
            A = Convert.ToDouble(System.Configuration.ConfigurationManager.AppSettings["A"]);
            Score = Convert.ToDouble(System.Configuration.ConfigurationManager.AppSettings["Score"]);

            CID = System.Configuration.ConfigurationManager.AppSettings["CID"];

            DBStr = System.Configuration.ConfigurationManager.ConnectionStrings["pricemedb"].ConnectionString;
        }
    }
}
