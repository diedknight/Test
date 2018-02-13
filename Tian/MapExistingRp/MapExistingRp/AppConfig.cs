using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace MapExistingRp
{
    public class AppConfig
    {
        public static string DBConStr{ get; set; }
        public static string CIdSql { get; set; }
        public static double PriceRate { get; set; }
        public static string LogPath { get; set; }

        static AppConfig()
        {
            DBConStr = ConfigurationManager.ConnectionStrings["Pricealyser"].ConnectionString;
            CIdSql = ConfigurationManager.AppSettings["CID"];
            PriceRate = Convert.ToDouble(ConfigurationManager.AppSettings["PriceRate"].Replace("%", "")) / 100;
            LogPath = ConfigurationManager.AppSettings["LogPath"];
        }
    }
}
