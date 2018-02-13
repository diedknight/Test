using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace CheckInvalidImageUrl
{
    public class AppConfig
    {
        public static string file { get; set; }
        public static int columnIndex { get; set; }
        public static int startRowIndex { get; set; }


        static AppConfig()
        {
            file = ConfigurationManager.AppSettings["file"];
            columnIndex = Convert.ToInt32(ConfigurationManager.AppSettings["columnIndex"]);
            startRowIndex = Convert.ToInt32(ConfigurationManager.AppSettings["startRowIndex"]);
        }

    }
}
