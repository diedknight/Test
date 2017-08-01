using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HotterWinds.ViewModels
{
    public class HomeViewModel
    {
        public string DressesName { get; set; }
        public List<Product> Dresses { get; set; }

        public string SunGlassesName { get; set; }
        public List<Product> SunGlasses { get; set; }

        public string TentsName { get; set; }
        public List<Product> Tents { get; set;}

        public string BootsName { get; set; }
        public List<Product> Boots { get; set; }

        public List<Product> BestSellerProducts { get; set; }
        public List<Product> FeaturesProducts { get; set; }
        public Product HotDeal { get; set; }

        public List<BLog> Blogs { get; set; }
    }
}