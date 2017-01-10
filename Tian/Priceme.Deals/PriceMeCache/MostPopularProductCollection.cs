using System;
using System.Collections.Generic;

namespace PriceMeCache
{
    [Serializable]
    public class MPProduct
    {
        string guid;
        decimal bestPrice;
        string productName;
        string imageFile;
        int productID;

        public int ProductID
        {
            get { return productID; }
            set { productID = value; }
        }

        public string Guid
        {
            get { return guid; }
            set { guid = value; }
        }

        public decimal BestPrice
        {
            get { return bestPrice; }
            set { bestPrice = value; }
        }

        public string ProductName
        {
            get { return productName; }
            set { productName = value; }
        }

        public string ImageFile
        {
            get { return imageFile; }
            set { imageFile = value; }
        }
    }
}