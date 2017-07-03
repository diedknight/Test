using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PriceMeCommon
{
    public static class PriceMeStatic
    {
        public static IFormatProvider Provider = new System.Globalization.CultureInfo("en-US");

        public static string PriceIntCultureString(double price, IFormatProvider provider, string priceSymbol, int countryId)
        {
            string pp = PriceIntCultureString(price, provider, countryId);
            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex("^\\d.*\\d$");
            if (regex.Match(pp).Success)
            {
                if (countryId == 56)
                    return pp + priceSymbol;
                else
                    return priceSymbol + pp;
            }
            else
            {
                return pp;
            }
        }

        public static string PriceCultureString(double price, IFormatProvider provider, string priceSymbol, int countryId)
        {
            string pp = string.Empty;
            if (countryId == 56)
                pp = PriceIntCultureString(price, provider, countryId);
            else
                pp = PriceCultureString(price, provider, countryId);
            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex("^\\d.*\\d$");
            if (regex.Match(pp).Success)
            {
                if (countryId == 56)
                    return pp + priceSymbol;
                else
                    return priceSymbol + pp;
            }
            else
            {
                return pp;
            }
        }

        public static string PriceIntCultureString(double price, IFormatProvider provider, int countryId)
        {
            string priceString = price.ToString("C0", provider).Replace("Php", "P");

            if (countryId == 45)
            {
                priceString = priceString.Replace("£", "RM");
            }
            else if (countryId == 36)
            {
                priceString = priceString.Replace("£", "S$");
            }
            else if (countryId == 56)
                priceString = priceString.Replace("₫", "").Trim();

            return priceString;
        }

        public static string PriceCultureString(double price, IFormatProvider provider, int countryId)
        {
            string priceString = price.ToString("C", provider).Replace("Php", "P");

            if (countryId == 45)
            {
                priceString = priceString.Replace("£", "RM");
            }
            else if (countryId == 36)
            {
                priceString = priceString.Replace("£", "S$");
            }
            else if(countryId == 56)
                priceString = priceString.Replace("₫", "").Trim();

            return priceString;
        }
    }
}
