using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoverInsuranceReport.Data
{
    public class HistoricalPriceData
    {
        public decimal maxPrice_30 { get; set; }
        public decimal minPrice_30 { get; set; }
        public decimal medianPrice_30 { get; set; }

        public decimal maxPrice_180 { get; set; }
        public decimal minPrice_180 { get; set; }
        public decimal medianPrice_180 { get; set; }
        public decimal avgPrice_180 { get; set; }

        public decimal rollingMedianPrice_180 { get; set; }

        public int PId { get; set; }
    }
}
