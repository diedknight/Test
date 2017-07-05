using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PriceMeCache
{
    [Serializable]
    public class MostPopularProduct
    {
        public string DefaultImage { get; set; }

        public int Cnt { get; set; }

        public double BestPrice { get; set; }

        public int ProductID { get; set; }

        public string ProductName { get; set; }
    }
}
