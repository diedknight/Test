using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IPCount.Data
{
    public class OutPutData
    {
        public string IP { get; set; }
        public string Country { get; set; }
        public Dictionary<string, int> Months { get; set; }

        public OutPutData(string ip, string country, string month, int count)
        {
            this.IP = ip;
            this.Country = country;

            this.Months = new Dictionary<string, int>();
            this.Months.Add(month, count);

            //MonthClicks clicks = new MonthClicks();
            

            //clicks.Month = month;
            //clicks.Count = count;
            //this.Months.Add(clicks);
        }

        public void Add(string month, int count)
        {
            //MonthClicks clicks = new MonthClicks();
            //clicks.Month = month;
            //clicks.Count = count;
            //this.Months.Add(clicks);

            this.Months.Add(month, count);

        }

        public int GetCount(string month)
        {
            if (this.Months.ContainsKey(month))
            {
                return this.Months[month];
            }
            else
            {
                return 0;
            }
        }

    }

    public class MonthClicks
    {
        public string Month { get; set; }
        public int Count { get; set; }
    }

}
