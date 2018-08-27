using System;
using System.Collections.Generic;
using System.Text;

namespace SiteMap.Data
{
    [Serializable]
    public class ProductCatalog
    {
        public int DayCount { get; set; }
        public int ListOrder { get; set; }
        public string HelpListOrder { get; set; }
        public string NZbestPrice { get; set; }
        public string UnitOfMeasure { get; set; }
        public string UnitOfLength { get; set; }
        public string Height { get; set; }
        public string Weight { get; set; }
        public string Width { get; set; }
        public string BestPPCRetailerProductID { get; set; }
        public string BestPPCRetailerID { get; set; }
        public string IsAccessory { get; set; }
        public string CategoryName { get; set; }
        public string BestPPCRetailerName { get; set; }
        public string DefaultImage { get; set; }
        public string ProductUrl { get; set; }
        public string BestPriceUrl { get; set; }
        public string ProductName { get; set; }
        public string ProductID { get; set; }
        public string ShortDescriptionZH { get; set; }
        public string ProductRatingSum { get; set; }
        public string ProductRatingVotes { get; set; }
        public string MaxPrice { get; set; }
        public string BestPrice { get; set; }
        public string PriceCount { get; set; }
        public int CategoryID { get; set; }
        public string SKU { get; set; }
        public string Rating { get; set; }
        public string AvRating { get; set; }
        public string ReviewCount { get; set; }
        public int Click { get; set; }
        public string BestPricePPCIndex { get; set; }
        public string BestPPCLogoPath { get; set; }
        public string ManufacturerID { get; set; }
        public string PPCRetailerProductID { get; set; }
        public string RetailerProductInfoString { get; set; }
        public bool IsMerged { get; set; }
        public string SecondPrice { get; set; }
        public string SecondRetailerName { get; set; }
        public string SecondOnClick { get; set; }
        public string OnClick { get; set; }
        public bool IsDisplay { get; set; }
        public string CatalogDescription { get; set; }
        public string RetailerCount { get; set; }
        public string DisplayName { get; set; }
        public string BestPPCPrice { get; set; }
        public bool IsUpComing { get; set; }
        public string ClickOutUrl { get; set; }
        public string LinkUrl { get; set; }
        public string ImageAlt { get; set; }
        public string StarsImage { get; set; }
        public string StarsImageAlt { get; set; }
        public string ComparePriceString { get; set; }
        public bool IsSearchOnly { get; set; }
        public string AttrDescription { get; set; }
        public float Sale { get; set; }
        public double PrevPrice { get; set; }
        public double CurrentPrice { get; set; }
        public double RatingPercent { get; set; }
        public bool IsTop3 { get; set; }
        public int Position { get; set; }
        public bool DisplayUpcomingPrice { get; set; }
        public bool IsAdsRow { get; set; }
    }
}
