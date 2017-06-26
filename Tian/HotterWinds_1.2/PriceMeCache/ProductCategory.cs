using System;
using System.Collections.Generic;

namespace PriceMeCache
{
    [Serializable]
    public class ProductCatalog
    {
        private string bestRetailerName;
        public string BestRetailerName
        {
            get { return bestRetailerName; }
            set { bestRetailerName = value; }
        }

        private string defaultImage;

        public string DefaultImage
        {
            get { return defaultImage; }
            set { defaultImage = value; }
        }

        private string manufacturerID;

        public string ManufacturerID
        {
            get { return manufacturerID; }
            set { manufacturerID = value; }
        }

        private string retailerProductID;

        public string RetailerProductID
        {
            get { return retailerProductID; }
            set { retailerProductID = value; }
        }

        private string productUrl;

        public string ProductUrl
        {
            get { return productUrl; }
            set { productUrl = value; }
        }

        private string bestPriceUrl;

        public string BestPriceUrl
        {
            get { return bestPriceUrl; }
            set { bestPriceUrl = value; }
        }

        private string productName;

        public string ProductName
        {
            get { return productName; }
            set { productName = value; }
        }

        private string productGUID;

        public string ProductGUID
        {
            get { return productGUID; }
            set { productGUID = value; }
        }

        private string productID;

        public string ProductID
        {
            get { return productID; }
            set { productID = value; }
        }

        private string shortDescriptionZH;

        public string ShortDescriptionZH
        {
            get { return shortDescriptionZH; }
            set { shortDescriptionZH = value; }
        }

        private string productRatingSum;

        public string ProductRatingSum
        {
            get { return productRatingSum; }
            set { productRatingSum = value; }
        }

        private string productRatingVotes;

        public string ProductRatingVotes
        {
            get { return productRatingVotes; }
            set { productRatingVotes = value; }
        }
        private string maxPrice;

        public string MaxPrice
        {
            get { return maxPrice; }
            set { maxPrice = value; }
        }

        private string bestPrice;
        public string BestPrice
        {
            get { return bestPrice; }
            set { bestPrice = value; }
        }

        private string retailerAmount;

        public string RetailerAmount
        {
            get { return retailerAmount; }
            set { retailerAmount = value; }
        }
        private int categoryID;
        public int CategoryID
        {
            get { return categoryID; }
            set { categoryID = value; }
        }
        private string sku;
        public string SKU
        {
            get { return sku; }
            set { sku = value; }
        }
        private string rating;
        public string Rating
        {
            get { return rating; }
            set { rating = value; }
        }
        private string avRating;
        public string AvRating
        {
            get { return avRating; }
            set { avRating = value; }
        }

        private string reviewCount;
        public string ReviewCount
        {
            get { return reviewCount; }
            set { reviewCount = value; }
        }

        private string prevPrice;
        public string PrevPrice
        {
            get { return prevPrice; }
            set { prevPrice = value; }
        }

        private string dropPrice;
        public string DropPrice
        {
            get { return dropPrice; }
            set { dropPrice = value; }
        }

        private int click;
        public int Click
        {
            get { return click; }
            set { click = value; }
        }

        private string _PPCIndex;
        public string PPCIndex
        {
            get { return _PPCIndex; }
            set { _PPCIndex = value; }
        }

        private string _PPCLogoPath;
        public string PPCLogoPath
        {
            get { return _PPCLogoPath; }
            set { _PPCLogoPath = value; }
        }

        private string _PPCLogo;
        public string PPCLogo
        {
            get { return _PPCLogo; }
            set { _PPCLogo = value; }
        }

        private string _PPCRetailerProductID;
        public string PPCRetailerProductID
        {
            get { return _PPCRetailerProductID; }
            set { _PPCRetailerProductID = value; }
        }

        private string _RetailerProductInfoString;
        public string RetailerProductInfoString
        {
            get { return _RetailerProductInfoString; }
            set { _RetailerProductInfoString = value; }
        }

        #region IComparable<ProductCatalog> 成员

        public int CompareTo(ProductCatalog other)
        {
            return float.Parse(this.bestPrice).CompareTo(float.Parse(other.bestPrice));
        }

        #endregion
    }
}
