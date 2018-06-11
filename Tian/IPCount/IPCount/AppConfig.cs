using IPCount.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IPCount
{
    public class AppConfig
    {
        public static List<DateRange> DateRange { get; set; }
        public static List<string> Country { get; set; }

        public static bool DownloadMaxMindDB { get; set; }
        public static int Count { get; set; }

        static AppConfig()
        {
            DateRange = GetDate();
            Country = System.Configuration.ConfigurationManager.AppSettings["country"].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            DownloadMaxMindDB = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["downloadMaxMindDB"]);

            Count = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["count"]);
        }

        private static List<DateRange> GetDate()
        {
            List<DateRange> list = new List<DateRange>();

            System.Configuration.ConfigurationManager.AppSettings["date"].Split(new string[] { "and" }, StringSplitOptions.RemoveEmptyEntries)
                .ToList().ForEach(month =>
                {
                    int intMonth = 0;
                    month = month.Trim().ToLower();
                    switch (month)
                    {
                        case "january": intMonth = 1; break;
                        case "february": intMonth = 2; break;
                        case "march": intMonth = 3; break;
                        case "april": intMonth = 4; break;
                        case "may": intMonth = 5; break;
                        case "june": intMonth = 6; break;
                        case "july": intMonth = 7; break;
                        case "august": intMonth = 8; break;
                        case "september": intMonth = 9; break;
                        case "october": intMonth = 10; break;
                        case "november": intMonth = 11; break;
                        case "december": intMonth = 12; break;
                    }

                    DateTime start = new DateTime(DateTime.Now.Year, intMonth, 1, 0, 0, 0, 0);
                    DateTime end = new DateTime(DateTime.Now.Year, intMonth + 1, 1, 0, 0, 0, 0).AddMilliseconds(-1);

                    DateRange range = new DateRange();
                    range.Start = start;
                    range.End = end;
                    range.Month = month;

                    list.Add(range);                    
                });

            return list;
        }

    }
}
