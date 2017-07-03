using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PriceMeCommon.Data
{
    public class RetailerProductCatalog
    {
        private string retailerId;
        private string productId;
        private string catgoryId;
        private string retailerProductId;
        private string retailerProductName;
        private string retailerProductDescription;
        private string retailerProductDefaultImage;
        private string retailerPrice;
        private string productAvRating;
        private string productReviewCount;
        private string retailerAvRating;
        private string productName;

        public string RetailerId
        {
            get { return retailerId; }
            set { retailerId = value; }
        }

        public string ProductId
        {
            get { return productId; }
            set { productId = value; }
        }

        public string CatgoryId
        {
            get { return catgoryId; }
            set { catgoryId = value; }
        }

        public string RetailerProductId
        {
            get { return retailerProductId; }
            set { retailerProductId = value; }
        }

        public string RetailerProductName
        {
            get { return retailerProductName; }
            set { retailerProductName = value; }
        }
        public string ProductName
        {
            get { return productName; }
            set { productName = value; }
        }
        public string RetailerProductDescription
        {
            get { return retailerProductDescription; }
            set { retailerProductDescription = value; }
        }

        public string RetailerProductDefaultImage
        {
            get { return retailerProductDefaultImage; }
            set { retailerProductDefaultImage = value; }
        }

        public string RetailerPrice
        {
            get { return retailerPrice; }
            set { retailerPrice = value; }
        }

        public string ProductAvRating
        {
            get { return productAvRating; }
            set { productAvRating = value; }
        }

        public string ProductReviewCount
        {
            get { return productReviewCount; }
            set { productReviewCount = value; }
        }

        public string RetailerAvRating
        {
            get { return retailerAvRating; }
            set { retailerAvRating = value; }
        }

        public string RetailerProductCondition { get; set; }
    }
}