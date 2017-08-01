using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HotterWinds.ViewModels
{
    public class ProductViewModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public string ImgUrl { get; set; }
        public int CategoryId { get; set; }        
        public string QuickOverView { get; set; }
        public string Description { get; set; }

        public List<CategoryV> CategoryList { get; set; }
        public List<Product> RelatedProducts { get; set; }

    }
}