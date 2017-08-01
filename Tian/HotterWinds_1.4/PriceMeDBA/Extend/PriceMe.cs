using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PriceMeDBA
{
    public static class PriceMeDBStatic
    {
        public static PriceMeDBA.PriceMeDBDB PriceMeDB = new PriceMeDBA.PriceMeDBDB();

        public static List<int> ListVersionNoEnglishCountryid;

        static PriceMeDBStatic()
        {
            ListVersionNoEnglishCountryid = new List<int>();

            //单词不需要加s 的国家
            string versionNoEnglishCountryidString = System.Configuration.ConfigurationManager.AppSettings["VersionNoEnglishCountryid"];
            if (!string.IsNullOrEmpty(versionNoEnglishCountryidString))
            {
                string[] versionNoEnglishCountryid = versionNoEnglishCountryidString.ToString().Split(',');

                foreach (string countryId in versionNoEnglishCountryid)
                {
                    int cid = 0;
                    int.TryParse(countryId, out cid);
                    if (cid != 0)
                        ListVersionNoEnglishCountryid.Add(cid);
                }
            }
        }
    }
}
