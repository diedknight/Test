using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HotterWinds.ViewModels
{
    public class Product
    {
        public int ProductId { get; set; }
        public int RetailerId { get; set; }
        public int RetailerProductId { get; set; }
        public int CategoryId { get; set; }

        public string ImgUrl { get; set; }
        public string ProductName { get; set; }
        public double Stars { get; set; }
        public decimal Price { get; set; }
        public int RPCount { get; set; }

        public string PurchaseUrl { get; set; }
        public string RetailerProductName { get; set; }
        public string SKU { get; set; }
    }
}