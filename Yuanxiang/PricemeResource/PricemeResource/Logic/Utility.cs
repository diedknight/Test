using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace PricemeResource.Logic
{
    public static class Utility
    {
        static System.Text.RegularExpressions.Regex PriceRegex = new System.Text.RegularExpressions.Regex("(?<tag1>[^\\d]*)(?<price>[\\d,\\.]+)(?<tag2>[^\\d]*)");
        public static IFormatProvider CurrentCulture; //new System.Globalization.CultureInfo(Resources.Resource.TextString_Culture);

        public static string GetParameter(string sParam, HttpRequest request)
        {
            StringValues sv;
            request.Query.TryGetValue(sParam, out sv);
            return sv.ToString();
        }
        
        public static int GetIntParameter(string sParam, HttpRequest request)
        {
            int iOut = 0;

            string sOut = GetParameter(sParam, request);
            if (!String.IsNullOrEmpty(sOut))
            {
                if (sOut.Contains(','))
                {
                    int.TryParse(sOut.Split(',')[0], out iOut);
                }
                else
                {
                    int.TryParse(sOut, out iOut);
                }
            }

            return iOut;
        }

        public static string FormatPrice(double price, int CountryId, IFormatProvider _CurrentCulture)
        {
            CurrentCulture = _CurrentCulture;

            if (CountryId == 28
                || CountryId == 51
                || CountryId == 41
                || CountryId == 55
                || CountryId == 45
                || CountryId == 56)
            {
                return NewPriceCultureString_Int(price, CountryId);
            }
            else
            {
                return NewPriceCultureString_Double(price, CountryId);
            }
        }

        static string NewPriceCultureString_Double(double price, int CountryId)
        {
            string priceInfo = price.ToString("C", CurrentCulture);

            priceInfo = FixPriceFormat(priceInfo, CountryId);

            return priceInfo;
        }

        static string NewPriceCultureString_Int(double price, int CountryId)
        {
            string priceInfo = price.ToString("C0", CurrentCulture);

            priceInfo = FixPriceFormat(priceInfo, CountryId);

            return priceInfo;
        }

        static string FixPriceFormat(string priceString, int CountryId)
        {
            System.Text.RegularExpressions.Match match = PriceRegex.Match(priceString);
            if (match.Success)
            {
                string priceInfo = match.Groups["price"].Value;

                string priceTag1 = match.Groups["tag1"].Value.Trim();

                string priceTag2 = match.Groups["tag2"].Value.Trim();

                if (!string.IsNullOrEmpty(priceTag1))
                {
                    priceInfo = "<span class='priceSymbol'>" + priceTag1 + "</span><span class='priceSpan'>" + priceInfo + "</span>";

                }
                else
                {
                    priceInfo = "<span class='priceSpan'>" + priceInfo + "</span><span class='priceSymbol'>" + priceTag2 + "</span>";
                }
                return priceInfo;
            }
            else
            {
                return priceString;
            }
        }

        public static string GetRetailerInfoUrl(int rid, string rname)
        {
            string typeName = FilterInvalidUrlPathChar(rname);
            
            typeName = typeName.Length > 80 ? typeName.Substring(0, 80) : typeName;
            typeName.Replace("?", "");

            string sOut = "/" + typeName + "/r-" + rid + ".aspx";

            return sOut;
        }

        private static Regex illegalReg = new Regex(@"[^a-z0-9-+]+", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static Regex illegalReg2 = new Regex("-+", RegexOptions.Compiled);

        public static string FilterInvalidUrlPathChar(string sourceString)
        {
            sourceString = illegalReg.Replace(sourceString, "-");
            sourceString = illegalReg2.Replace(sourceString, "-");

            return System.Web.HttpUtility.UrlEncode(sourceString);
        }
    }
}
