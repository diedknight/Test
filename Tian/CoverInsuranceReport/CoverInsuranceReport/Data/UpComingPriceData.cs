using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoverInsuranceReport.Data
{
    public class UpComingPriceData
    {
        public int PId { get; set; }
        public decimal Price { get; set; }

        public DateTime Upcoming { get; set; }
    }
}
