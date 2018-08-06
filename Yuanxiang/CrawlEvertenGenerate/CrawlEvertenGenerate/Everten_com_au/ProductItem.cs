using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrawlEvertenGenerate.Everten_com_au
{
    public class ProductItem
    {
        public string ProductSku { get; set; }
        public string InStock { get; set; }
        public string NumberStock { get; set; }
        public string ProductPrice { get; set; }
        public string PurchaseUrl { get; set; }
        public string ProductName { get; set; }
        public string ManufacturerName { get; set; }
        public string CategoryName { get; set; }
        public string Visibility { get; set; }
    }
}
