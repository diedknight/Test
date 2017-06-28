using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AliexpressTool.Data
{
    public class ProductData
    {
        public int ShopProductId { get; set; }
        public int ShopNZProductId{ get; set; }
        public List<int> ListProduct { get; set; }
        public string Manufacturer { get; set; }
        public string SuitableProducts { get; set; }
    }
}
