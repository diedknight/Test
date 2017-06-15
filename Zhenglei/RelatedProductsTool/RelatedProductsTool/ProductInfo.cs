using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RelatedProductsTool
{
    public class ProductInfo
    {
        public int ProductId { get; set; }
        public int BrandId { get; set; }
        public int CategoryId { get; set; }
        public decimal BestPrice { get; set; }
        public int Clicks { get; set; }
        public int PPCRetailerCount { get; set; }
        public int CategoryClickIndex { get; set; }
    }
}