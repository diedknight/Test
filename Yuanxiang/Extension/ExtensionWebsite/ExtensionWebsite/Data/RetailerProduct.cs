using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExtensionWebsite.Data
{
    public class RetailerProduct
    {
        public int RetailerProductId { get; set; }
        public int ProductId { get; set; }
        public int RetailerId { get; set; }
        public string RetailerProductName { get; set; }
        public string PurchaseURL { get; set; }
        public decimal RetailerPrice { get; set; }
        public decimal Freight { get; set; }
        public decimal DiffPrice { get; set; }
        public string RetailerLogo { get; set; }
        public string RetailerName { get; set; }
        public bool IsNolink { get; set; }
    }
}