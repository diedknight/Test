using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MergeReportTool
{
    public class ProductData
    {
        public int RetailerProductId { get; set; }
        public string RetailerProductName { get; set; }
        public decimal RetailerPrice { get; set; }
        public string PurchaseURL { get; set; }
        public int RetailerProductCondition { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int ManufacturerID { get; set; }
        public int CategoryID { get; set; }
    }
}
