using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PriceMeCache
{
    [Serializable]
    public class MostPopularProduct
    {
        int productID, cnt;
        double bestPrice;
        string productName;
        string defaultImage;

        public string DefaultImage
        {
            get { return defaultImage; }
            set { defaultImage = value; }
        }

        public int Cnt
        {
            get { return cnt; }
            set { cnt = value; }
        }

        public double BestPrice
        {
            get { return bestPrice; }
            set { bestPrice = value; }
        }

        public int ProductID
        {
            get { return productID; }
            set { productID = value; }
        }

        public string ProductName
        {
            get { return productName; }
            set { productName = value; }
        }
    }
}
