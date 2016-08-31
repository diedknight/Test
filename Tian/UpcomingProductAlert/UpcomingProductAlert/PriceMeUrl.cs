using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace UpcomingProductAlert
{
    public class PriceMeUrl
    {
        //不能用[^\w-]+ 因为 \w 包括字母，数字，下划线，汉字等其它非符号
        private static Regex illegalReg = new Regex(@"[^a-z0-9-+]+", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static Regex illegalReg1 = new Regex(@"[^a-z0-9-]+", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static Regex illegalReg2 = new Regex("-+", RegexOptions.Compiled);
        private static Regex illegalReg3 = new Regex("^-+|-+$", RegexOptions.Compiled);

        public static string FilterInvalidNameChar(string name)
        {
            name = illegalReg1.Replace(name, "-");
            name = illegalReg2.Replace(name, "-");
            name = illegalReg3.Replace(name, "");

            return name;
        }


        public static string GetProductUrl(CountryDetail countryDetail, int productID, string productName)
        {
            if (countryDetail.CountryId == 41) return countryDetail.PriceMeHost + string.Format("/p-{0}.aspx", productID);

            return countryDetail.PriceMeHost + string.Format("/{0}/p-{1}.aspx", FilterInvalidNameChar(productName), productID);
        }


    }
}
