using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PricemeResource.Data
{
    [Serializable]
    public class PriceHistory
    {
        public int PriceHistoryID { get; set; }
        public DateTime PriceDate { get; set; }
        public decimal Price { get; set; }
        public int ProductID { get; set; }
        public int RetailerID { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public int Retailerproductid { get; set; }
    }
}
