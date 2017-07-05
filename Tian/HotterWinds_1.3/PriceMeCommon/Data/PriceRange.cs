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

        public string ToPriceString(IFormatProvider provider)
        {
            string priceRangeString = "";
            //if (_minPrice <= 0)
            //{
            //    priceRangeString = PriceMeCommon.PriceMeStatic.PriceIntCultureString(0, provider) +
            //        "-" + PriceMeCommon.PriceMeStatic.PriceIntCultureString(_maxPrice, provider);
            //}
            //else if (_maxPrice >= int.MaxValue)
            //{
            //    priceRangeString = "Above " + PriceMeCommon.PriceMeStatic.PriceIntCultureString(_minPrice, provider);
            //}
            //else if (_minPrice > 0 && _maxPrice > 0)
            //{
            //    priceRangeString = PriceMeCommon.PriceMeStatic.PriceIntCultureString(_minPrice, provider) + 
            //        "-" + PriceMeCommon.PriceMeStatic.PriceIntCultureString(_maxPrice, provider);
            //}
            return priceRangeString;
        }

        public string ToPriceString(IFormatProvider provider, string priceSymbol)
        {
            string priceRangeString = "";
            //if (_minPrice <= 0)
            //{
            //    priceRangeString = PriceMeCommon.PriceMeStatic.PriceIntCultureString(0, provider, priceSymbol) + 
            //        "-" + PriceMeCommon.PriceMeStatic.PriceIntCultureString(_maxPrice, provider, priceSymbol);
            //}
            //else if (_maxPrice >= int.MaxValue)
            //{
            //    priceRangeString = "Above " + PriceMeCommon.PriceMeStatic.PriceIntCultureString(_minPrice, provider, priceSymbol);
            //}
            //else if (_minPrice > 0 && _maxPrice > 0)
            //{
            //    priceRangeString = PriceMeCommon.PriceMeStatic.PriceIntCultureString(_minPrice, provider, priceSymbol) + 
            //        "-" + PriceMeCommon.PriceMeStatic.PriceIntCultureString(_maxPrice, provider, priceSymbol);
            //}
            return priceRangeString;
        }

        public string ToUrlString()
        {
            return this._minPrice + "-" + this._maxPrice;
        }
    }
}