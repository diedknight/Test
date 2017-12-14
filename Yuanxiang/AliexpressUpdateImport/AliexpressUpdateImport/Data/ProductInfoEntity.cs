using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AliexpressImport.Data
{
    public class ProductInfoEntity
    {
        public string ProductName { get; set; }
        public string Sku { get; set; }
        public decimal Length { get; set; }
        public decimal Width { get; set; }
        public decimal Height { get; set; }
        public decimal Weight { get; set; }
        public string FullDescription { get; set; }
        public string Vendor { get; set; }
        public string DeliveryDate { get; set; }
        public string PriceCurrency { get; set; }
        public decimal Price { get; set; }
        public decimal OldPrice { get; set; }
        public string ProductCost { get; set; }
        public string Category { get; set; }
        public string AdminComment { get; set; }
        public int Stock { get; set; }
        public string Picture1 { get; set; }
        public string Picture2 { get; set; }
        public string Picture3 { get; set; }
        public string Picture4 { get; set; }
        public string Picture5 { get; set; }
        public string Picture6 { get; set; }
    }
}
