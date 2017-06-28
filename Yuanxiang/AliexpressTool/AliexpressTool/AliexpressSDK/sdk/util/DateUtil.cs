using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.alibaba.openapi.client.util
{
    class DateUtil
    {
        //这里定义两个日期格式，由于.Net平台的毫秒格式用fff表示，Ocean平台(Java)的毫秒格式用SSS表示。
        private static string Date_Pattern = "yyyyMMddHHmmssfff";
     
        private static string Date_PatternForOcean = "yyyyMMddHHmmssSSS";


        public static String getDatePattern()
        {
            return Date_PatternForOcean;
        }

        public static String format(DateTime date)
        {
            return date.ToString(Date_Pattern);
        }

        public static String formatForOcean(DateTime date)
        {
            String value = date.ToString("yyyyMMddHHmmssfffzzz");
            String newValue = value.Replace(":", "");
            return newValue;
        }

        public static DateTime formatFromStr(String dateDesc)
        {
            if (dateDesc.Contains("+") || dateDesc.Contains("-"))
            {
                try
                {
                    IFormatProvider culture = new CultureInfo("zh-CN", true);
                    DateTime datetime = DateTime.ParseExact(dateDesc, "yyyyMMddHHmmssfffzzz", culture);
                    return datetime;
                }
                catch (Exception x)
                {

                }
            }
          
                IFormatProvider newculture = new CultureInfo("zh-CN", true);
                DateTime newdatetime = DateTime.ParseExact(dateDesc, Date_Pattern, newculture);
                return newdatetime;
           
        }



        public static long currentTimeMillis()
        {
            System.DateTime current = new DateTime();
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            double ms = (current - startTime).TotalMilliseconds;
            long b = Convert.ToInt64(ms);
            return b;
        }
    }
}
