using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PricemeResource.Data
{
    [Serializable]
    public class ProductRetailerCountHistory
    {
        public int ProductRetailerCountHistoryID { get; set; }
        public int ProductID { get; set; }
        public int RetialerCount { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
        public int CountryID { get; set; }
    }
}
