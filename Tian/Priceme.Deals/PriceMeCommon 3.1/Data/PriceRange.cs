using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PriceMeCommon.Data
{
    public class PriceRange
    {
        double _minPrice;
        double _maxPrice;

        public double MinPrice
        {
            get { return _minPrice; }
        }

        public double MaxPrice
        {
            get { return _maxPrice; }
        }

        public PriceRange(double minPrice, double maxPrice)
        {
            if (minPrice > maxPrice)
            {
                maxPrice = double.MaxValue;
            }
            this._minPrice = minPrice;
            this._maxPrice = maxPrice;
        }
        /*
                public string ToPriceString(IFormatProvider provider)
                {
                    string priceRangeString = "";
                    if (_minPrice <= 0f)
                    {
                        priceRangeString = "Below " + PriceMeCommon.PriceMeStatic.PriceIntCultureString(_maxPrice, provider);
                    }
                    else if (_maxPrice >= float.MaxValue)
                    {
                        priceRangeString = "Above " + PriceMeCommon.PriceMeStatic.PriceIntCultureString(_minPrice, provider);
                    }
                    else if (_minPrice > 0 && _maxPrice > 0)
                    {
                        priceRangeString = PriceMeCommon.PriceMeStatic.PriceIntCultureString(_minPrice, provider) + "-" + PriceMeCommon.PriceMeStatic.PriceIntCultureString(_maxPrice, provider);
                    }
                    return priceRangeString;
                }

                public string ToPriceString(IFormatProvider provider, string priceSymbol)
                {
                    string priceRangeString = "";
                    if (_minPrice <= 0f)
                    {
                        priceRangeString = "Below " + PriceMeCommon.PriceMeStatic.PriceIntCultureString(_maxPrice, provider, priceSymbol);
                    }
                    else if (_maxPrice >= float.MaxValue)
                    {
                        priceRangeString = "Above " + PriceMeCommon.PriceMeStatic.PriceIntCultureString(_minPrice, provider, priceSymbol);
                    }
                    else if (_minPrice > 0 && _maxPrice > 0)
                    {
                        priceRangeString = PriceMeCommon.PriceMeStatic.PriceIntCultureString(_minPrice, provider, priceSymbol) + "-" + PriceMeCommon.PriceMeStatic.PriceIntCultureString(_maxPrice, provider, priceSymbol);
                    }
                    return priceRangeString;
                }
                */

        public string ToPriceString(IFormatProvider provider)
        {
            string priceRangeString = "";
            if (_minPrice <= 0)
            {
                priceRangeString = PriceMeCommon.PriceMeStatic.PriceIntCultureString(0, provider) +
                    "-" + PriceMeCommon.PriceMeStatic.PriceIntCultureString(_maxPrice, provider);
                //priceRangeString = "Below " + PriceMeCommon.PriceMeStatic.PriceIntCultureString(_maxPrice, provider);
            }
            else if (_maxPrice >= int.MaxValue)
            {
                priceRangeString = "Above " + PriceMeCommon.PriceMeStatic.PriceIntCultureString(_minPrice, provider);
            }
            else if (_minPrice > 0 && _maxPrice > 0)
            {
                priceRangeString = PriceMeCommon.PriceMeStatic.PriceIntCultureString(_minPrice, provider) + 
                    "-" + PriceMeCommon.PriceMeStatic.PriceIntCultureString(_maxPrice, provider);
            }
            return priceRangeString;
        }

        public string ToPriceString(IFormatProvider provider, string priceSymbol)
        {
            string priceRangeString = "";
            if (_minPrice <= 0)
            {
                priceRangeString = PriceMeCommon.PriceMeStatic.PriceIntCultureString(0, provider, priceSymbol) + 
                    "-" + PriceMeCommon.PriceMeStatic.PriceIntCultureString(_maxPrice, provider, priceSymbol);
                //priceRangeString = "Below " + PriceMeCommon.PriceMeStatic.PriceIntCultureString(_maxPrice, provider, priceSymbol);
            }
            else if (_maxPrice >= int.MaxValue)
            {
                priceRangeString = "Above " + PriceMeCommon.PriceMeStatic.PriceIntCultureString(_minPrice, provider, priceSymbol);
            }
            else if (_minPrice > 0 && _maxPrice > 0)
            {
                priceRangeString = PriceMeCommon.PriceMeStatic.PriceIntCultureString(_minPrice, provider, priceSymbol) + 
                    "-" + PriceMeCommon.PriceMeStatic.PriceIntCultureString(_maxPrice, provider, priceSymbol);
            }
            return priceRangeString;
        }
        //public string ToPriceString(string PriceSymbol)
        //{
        //    string priceRangeString = "";
        //    if (_minPrice <= 0f)
        //    {
        //        priceRangeString = "Below " + FormatPrice(decimal.Parse(_maxPrice.ToString("0")), PriceSymbol);
        //    }
        //    else if (_maxPrice == double.MaxValue)
        //    {
        //        priceRangeString = "Above " + FormatPrice(decimal.Parse(_minPrice.ToString("0")), PriceSymbol);
        //    }
        //    else if (_minPrice > 0 && _maxPrice > 0)
        //    {
        //        priceRangeString = FormatPrice(decimal.Parse(_minPrice.ToString("0")), PriceSymbol) + "-" + FormatPrice(decimal.Parse(_maxPrice.ToString("0")), PriceSymbol);
        //    }
        //    return priceRangeString;
        //}

        //public string FormatPrice(double price, string PriceSymbol)
        //{
        //    string dec = "";
        //    price = decimal.Round(price, 2);
        //    string result = "";
        //    if (PriceMeCommon.ConfigAppString.CountryID == 28 || PriceMeCommon.ConfigAppString.CountryID == 51)
        //    {
        //        //sg 36 my 45 au 1 nz 3 ph 28 hk 41
        //        price = decimal.Round(price, 0);
        //        dec = price.ToString();
        //    }
        //    //印度尼西亚 整数 00.000.000
        //    else
        //    {
        //        price = decimal.Round(price, 2);
        //        dec = price.ToString("0.00");
        //    }
        //    if (Math.Abs(price) < 1000)
        //        result = dec;
        //    else
        //    {
        //        int len = 3; string tail = string.Empty;
        //        if (dec.Contains('.')) { len = 6; tail = dec.Substring(dec.IndexOf('.')); }
        //        result = dec.Substring(dec.Length - len, len);
        //        int quotient = System.Convert.ToInt32(price) / 1000;
        //        if (Math.Abs(quotient) > 0)
        //            result = ConvertInt(quotient) + "," + result;
        //        if (PriceMeCommon.ConfigAppString.CountryID == 51)
        //        {
        //            result = result.Replace(",", ".");
        //        }
        //    }

        //    return PriceSymbol + result;
        //}

        //public static string ConvertInt(double src)
        //{
        //    string result = "";
        //    if (Math.Abs(src) < 1000)
        //        result = src.ToString();
        //    else
        //    {
        //        result = src.ToString().Substring(src.ToString().Length - 3, 3);
        //        int quotient = System.Convert.ToInt32(src) / 1000;
        //        if (Math.Abs(quotient) > 0)
        //            result = ConvertInt(quotient) + "," + result;
        //    }

        //    return result;
        //}

        public string ToUrlString()
        {
            return this._minPrice + "-" + this._maxPrice;
        }
    }
}