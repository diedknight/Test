using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PriceMeCommon
{
    public static class PriceMeStatic
    {
        public static PriceMeDBA.PriceMeDBDB PriceMeDB = new PriceMeDBA.PriceMeDBDB();
        public static IFormatProvider Provider = new System.Globalization.CultureInfo("en-US");

        public static string PriceIntCultureString(double price, IFormatProvider provider, string priceSymbol)
        {
            string pp = PriceIntCultureString(price, provider);
            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex("^\\d.*\\d$");
            if (regex.Match(pp).Success)
            {
                if (ConfigAppString.CountryID == 56)
                    return pp + priceSymbol;
                else
                    return priceSymbol + pp;
            }
            else
            {
                return pp;
            }
        }

        public static string PriceCultureString(double price, IFormatProvider provider, string priceSymbol)
        {
            string pp = string.Empty;
            if (ConfigAppString.CountryID == 56)
                pp = PriceIntCultureString(price, provider);
            else
                pp = PriceCultureString(price, provider);
            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex("^\\d.*\\d$");
            if (regex.Match(pp).Success)
            {
                if (ConfigAppString.CountryID == 56)
                    return pp + priceSymbol;
                else
                    return priceSymbol + pp;
            }
            else
            {
                return pp;
            }
        }

        public static string PriceIntCultureString(double price, IFormatProvider provider)
        {
            string priceString = price.ToString("C0", provider).Replace("Php", "P");

            if (PriceMeCommon.ConfigAppString.CountryID == 45)
            {
                priceString = priceString.Replace("£", "RM");
            }
            else if (PriceMeCommon.ConfigAppString.CountryID == 36)
            {
                priceString = priceString.Replace("£", "S$");
            }
            else if (PriceMeCommon.ConfigAppString.CountryID == 56)
                priceString = priceString.Replace("₫", "").Trim();

            return priceString;
        }

        public static string PriceCultureString(double price, IFormatProvider provider)
        {
            string priceString = price.ToString("C", provider).Replace("Php", "P");

            if (PriceMeCommon.ConfigAppString.CountryID == 45)
            {
                priceString = priceString.Replace("£", "RM");
            }
            else if (PriceMeCommon.ConfigAppString.CountryID == 36)
            {
                priceString = priceString.Replace("£", "S$");
            }
            else if(PriceMeCommon.ConfigAppString.CountryID==56)
                priceString = priceString.Replace("₫", "").Trim();

            return priceString;
        }
    }
}
