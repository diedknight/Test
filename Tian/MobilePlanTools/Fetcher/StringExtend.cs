using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fetcher
{
    public static class StringExtend
    {
        public static decimal ToDecimal(this string str, decimal defVal = 0m)
        {
            decimal val = defVal;
            string valStr = str;

            if (string.IsNullOrEmpty(valStr)) return val;

            valStr = valStr.Replace("$", "");
            valStr = valStr.Replace("đ", "");
            valStr = valStr.Replace("₱", "");

            decimal.TryParse(valStr.Trim(), out val);
            
            return val;
        }
    }
}
