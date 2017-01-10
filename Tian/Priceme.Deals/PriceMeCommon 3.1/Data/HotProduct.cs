using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PriceMeCommon.Data 
{
    [Serializable]
    public class HotProduct
    {
        int productID, order;
        double bestPrice;
        string productName, displayName, defaultImage;

        public string DefaultImage
        {
            get { return defaultImage; }
            set { defaultImage = value; }
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

        public int DisplayOrder
        {
            get { return order; }
            set { order = value; }
        }

        public string ProductName
        {
            get { return productName; }
            set { productName = value; }
        }

        public string DisplayName
        {
            get { return displayName; }
            set { displayName = value; }
        }
    }
}
