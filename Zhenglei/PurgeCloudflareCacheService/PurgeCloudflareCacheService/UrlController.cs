using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PurgeCloudflareCacheService
{
    public static class UrlController
    {
        private static Regex illegalReg1 = new Regex(@"[^a-z0-9-]+", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static Regex illegalReg2 = new Regex("-+", RegexOptions.Compiled);
        private static Regex illegalReg3 = new Regex("^-+|-+$", RegexOptions.Compiled);

        public static string GetProductUrl(int productId, string productName, bool urlSeo)
        {
            if (urlSeo)
                return string.Format("{0}/p-{1}.aspx", FilterInvalidNameChar(productName), productId);

            return string.Format("p-{0}.aspx", productId);
        }

        public static string FilterInvalidNameChar(string name)
        {
            name = illegalReg1.Replace(name, "-");
            name = illegalReg2.Replace(name, "-");
            name = illegalReg3.Replace(name, "");

            return name;
        }

        public static string GetCatalogUrl(string categoryName, int categoryId)
        {
            return FilterInvalidNameChar(categoryName) + "/c-" + categoryId + ".aspx";
        }
    }
}