using System;
using System.Collections.Generic;
using System.Text;

namespace RetailerProductsIndexController
{
    public class RetailerProduct
    {
        string retailerProductID;
        string retailerProductName;
        string retailerPrice;
        string stock;
        string modifiedOn;
        string categoryID;
        string retailerID;
        string purchaseURL;
        string isDeleted;
        string productID;
        
        public string IsMerge { get; set; }
        public string ProductName { get; set; }

        public string ProductID
        {
            get { return productID; }
            set { productID = value; }
        }

        public string RetailerProductID
        {
            get { return retailerProductID; }
            set { retailerProductID = value; }
        }

        public string RetailerProductName
        {
            get { return retailerProductName; }
            set { retailerProductName = value; }
        }

        public string RetailerPrice
        {
            get { return retailerPrice; }
            set { retailerPrice = value; }
        }

        public string Stock
        {
            get { return stock; }
            set { stock = value; }
        }

        public string ModifiedOn
        {
            get { return modifiedOn; }
            set { modifiedOn = value; }
        }

        public string CategoryID
        {
            get { return categoryID; }
            set { categoryID = value; }
        }

        public string RetailerID
        {
            get { return retailerID; }
            set { retailerID = value; }
        }

        public string PurchaseURL
        {
            get { return purchaseURL; }
            set { purchaseURL = value; }
        }

        public string IsDeleted
        {
            get { return isDeleted; }
            set { isDeleted = value; }
        }
    }
}
