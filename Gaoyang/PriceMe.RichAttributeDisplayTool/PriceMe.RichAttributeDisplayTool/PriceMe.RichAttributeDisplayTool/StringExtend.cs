using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace PriceMe.RichAttributeDisplayTool
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
            valStr = valStr.Replace("SGD", "");

            decimal.TryParse(valStr.Trim(), out val);
            
            return val;
        }

        public static int ToInt32(this string str)
        {

            if (string.IsNullOrEmpty(str)) return 0;

            int result = 0;

            int.TryParse(str.Trim(),out result);

            return result;
        }

        public static double ToDouble(this string str)
        {

            if (string.IsNullOrEmpty(str)) return 0;

            double result = 0;

            double.TryParse(str.Trim(), out result);

            return result;
        }

        /// <summary>
        /// 实现数据的四舍五入法
        /// </summary>
        /// <param name="v">要进行处理的数据</param>
        /// <param name="x">保留的小数位数</param>
        /// <returns>四舍五入后的结果</returns>
        public static int Round(this double v)
        {
            var dou = v.ToString();
            int result = 0;
            if (dou.Contains("."))
            {
                var dot = int.Parse(dou.Split('.')[1].ToString());
                var math = int.Parse(dou.Split('.')[0].ToString());
                if (dot >= 5)
                    result = math + 1;
                else
                    result = math;
            }
            else
                result = dou.ToInt32();

            return result;
        }


     

        public static PropertyInfo[] GetPropertyInfoArray<T>(this T clas) where T:new()
        {

            var ref_clas = clas;
            PropertyInfo[] props = null;
            try
            {
                Type type = typeof(T);
                object obj = Activator.CreateInstance(type);
                props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            }
            catch (Exception ex)
            { }
            return props;
        }

       
    }
}
