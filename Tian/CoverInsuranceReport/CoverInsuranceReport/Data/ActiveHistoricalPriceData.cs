using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoverInsuranceReport.Data
{
    public class ActiveHistoricalPriceData
    {
        public int PId { get; set; }
        public decimal NewPrice { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
