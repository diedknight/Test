using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceMeCommon.Data
{
    public class DaysRange
    {
        public int MinDays { get; set; }
        public int MaxDays { get; set; }
        
        public static DaysRange Create(string daysRangeString)
        {
            string[] ds = daysRangeString.Split('-');
            int minDays = 0;
            int maxDays = 0;
            if(int.TryParse(ds[0], out minDays) && int.TryParse(ds[1], out maxDays))
            {
                DaysRange dr = new DaysRange();
                dr.MinDays = minDays;
                dr.MaxDays = maxDays;
                return dr;
            }
            return null;
        }
    }
}
