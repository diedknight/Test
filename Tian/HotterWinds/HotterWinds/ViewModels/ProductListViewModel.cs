using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HotterWinds.ViewModels
{
    public class ProductListViewModel
    {
        public List<Product1> ProductList { get; set; }
        public List<CategoryV> CategoryList { get; set; }

        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }
    }
}