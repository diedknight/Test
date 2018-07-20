using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BaseProductTool
{
    public static class StringExtend
    {
        public static decimal ToDecimal(this string str, decimal defVal = 0m)
        {
            decimal val = defVal;
            string valStr = str;

            if (string.IsNullOrEmpty(valStr)) return val;

            valStr = valStr.Replace("SGD", "");
            valStr = valStr.Replace("Php", "");
            valStr = valStr.Replace("USD", "");
            valStr = valStr.Replace("MYR", "");
            valStr = valStr.Replace("RRP", "");
            valStr = valStr.Replace("NZD", "");
            valStr = valStr.Replace("RUB", "");
            valStr = valStr.Replace("GBP", "");
            valStr = valStr.Replace("VNĐ", "");


            valStr = valStr.Replace("Rp.", "");
            valStr = valStr.Replace("Rp", "");
            valStr = valStr.Replace("SG", "");
            valStr = valStr.Replace("RM", "");
            valStr = valStr.Replace("NZ", "");
            valStr = valStr.Replace("HK", "");


            valStr = valStr.Replace("$", "");
            valStr = valStr.Replace("đ", "");
            valStr = valStr.Replace("₫", "");
            valStr = valStr.Replace("₱", "");
            valStr = valStr.Replace("฿", "");
            valStr = valStr.Replace("£", "");
            valStr = valStr.Replace(",", "");
            valStr = valStr.Replace(":", "");

            valStr = valStr.Replace(Environment.NewLine, "");
            valStr = valStr.Replace("\n", "");
            valStr = valStr.Replace(" ", "");            

            decimal.TryParse(valStr.Trim(), out val);

            return val;
        }

        public static string TrimA(this string str)
        {
            str = str.Replace(Environment.NewLine, " ").Replace("\n", " ").Replace("\r", " ").Trim();
            
            StringBuilder sb = new StringBuilder();
            bool isRepeatWhiteSpace = false;

            foreach (char c in str)
            {
                if (!char.IsWhiteSpace(c))
                {
                    isRepeatWhiteSpace = false;
                    sb.Append(c);                    
                }
                else if (!isRepeatWhiteSpace)
                {
                    isRepeatWhiteSpace = true;
                    sb.Append(c);
                }
            }

            return sb.ToString();
        }

        public static string TirmStr(this string str)
        {

            if (string.IsNullOrEmpty(str)) return "";

            string getStr = str.Replace("\n", "").Replace("/n", "").TrimStart(' ');

            if (str.StartsWith(" "))
                TirmStr(getStr);

            getStr = getStr.TrimEnd(' ');

            if (str.EndsWith(" "))
                TirmStr(getStr);

            return getStr;
        }

        //public static string GetLargeImg(this string img,string[] imgs)
        //{

        //    if (string.IsNullOrEmpty(img)||imgs.Length<=0) return "";

        //    var pro_img = img;
        //    imgs.ToList().ForEach(f => {
        //        var str = f.Split(',');
        //        if(str.Length>=2)
        //            pro_img = pro_img.Replace(str[0], str[1]);
        //    });

        //    return pro_img;
        //}
    }
}
