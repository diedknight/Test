using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PricemeResource.Data
{
    [Serializable]
    public class ProductNewData
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public decimal BestPrice { get; set; }
    }
}
