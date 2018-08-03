using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoverInsuranceReport
{
    public class AppConfig
    {
        public static List<string> PricesDisplay { get; set; }


        static AppConfig()
        {
            string str = System.Configuration.ConfigurationManager.AppSettings["pricesdisplay"];

            PricesDisplay = str.Replace("'", "").Trim().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
        }

    }
}
