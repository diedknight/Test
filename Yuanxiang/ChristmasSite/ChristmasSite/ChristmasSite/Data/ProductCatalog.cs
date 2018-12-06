using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChristmasSite.Data
{
    public class ProductCatalog
    {
        public int RID { get; set; }
        public int rpid { get; set; }
        public int pid { get; set; }
        public string productname { get; set; }
        public int CID { get; set; }
        public string CategoryName { get; set; }
        public string retailername { get; set; }
        public decimal newprice { get; set; }
        public decimal oldprice { get; set; }
        public decimal rate { get; set; }
        public string retailerlogo { get; set; }
        public string productimages { get; set; }
        public string trackURL { get; set; }
        public DateTime CreatedOn { get; set; }
        public int tracks { get; set; }
    }
}
