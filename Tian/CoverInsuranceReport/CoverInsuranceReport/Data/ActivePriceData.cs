using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoverInsuranceReport.Data
{
    public class ActivePriceData
    {
        public int PId { get; set; }
        public decimal Min { get; set; }
        public decimal Avg { get; set; }
        public decimal Max { get; set; }
        public int Num { get; set; }
    }
}
