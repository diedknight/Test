using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace PriceMeCommon.Data
{
    [Serializable]
    public class ProductCatalog
    {
        [JsonProperty]
        public int DayCount { get; set; }
        [JsonProperty]
        public int ListOrder { get; set; }
        [JsonIgnore]
        public string HelpListOrder { get; set; }
        [JsonIgnore]
        public string NZbestPrice { get; set; }
        [JsonIgnore]
        public string UnitOfMeasure { get; set; }
        [JsonIgnore]
        public string UnitOfLength { get; set; }
        [JsonIgnore]
        public string Height { get; set; }
        [JsonIgnore]
        public string Weight { get; set; }
        [JsonIgnore]
        public string Width { get; set; }
        [JsonProperty]
        public string BestPPCRetailerProductID { get; set; }
        [JsonProperty]
        public string BestPPCRetailerID { get; set; }
        [JsonProperty]
        public string IsAccessory { get; set; }
        [JsonIgnore]
        public string CategoryName { get; set; }
        [JsonProperty]
        public string BestPPCRetailerName { get; set; }
        [JsonProperty]
        public string DefaultImage { get; set; }
        [JsonProperty]
        public string ProductUrl { get; set; }
        [JsonProperty]
        public string BestPriceUrl { get; set; }
        [JsonProperty]
        public string ProductName { get; set; }
        [JsonProperty]
        public string ProductID { get; set; }
        [JsonProperty]
        public string ShortDescriptionZH { get; set; }
        [JsonProperty]
        public string ProductRatingSum { get; set; }
        [JsonProperty]
        public string ProductRatingVotes { get; set; }
        [JsonProperty]
        public string MaxPrice { get; set; }
        [JsonProperty]
        public string BestPrice { get; set; }
        [JsonProperty]
        public string PriceCount { get; set; }
        [JsonProperty]
        public int CategoryID { get; set; }
        [JsonIgnore]
        public string SKU { get; set; }
        [JsonProperty]
        public string Rating { get; set; }
        [JsonProperty]
        public string AvRating { get; set; }
        [JsonProperty]
        public string ReviewCount { get; set; }
        [JsonIgnore]
        public int Click { get; set; }
        [JsonIgnore]
        public string BestPricePPCIndex { get; set; }
        [JsonIgnore]
        public string BestPPCLogoPath { get; set; }
        [JsonProperty]
        public string ManufacturerID { get; set; }
        [JsonIgnore]
        public string PPCRetailerProductID { get; set; }
        [JsonIgnore]
        public string RetailerProductInfoString { get; set; }
        [JsonIgnore]
        public bool IsMerged { get; set; }
        [JsonProperty]
        public string SecondPrice { get; set; }
        [JsonProperty]
        public string SecondRetailerName { get; set; }
        [JsonProperty]
        public string SecondOnClick { get; set; }
        [JsonIgnore]
        public string OnClick { get; set; }
        [JsonIgnore]
        public bool IsDisplay { get; set; }
        [JsonIgnore]
        public string CatalogDescription { get; set; }
        [JsonProperty]
        public string RetailerCount { get; set; }
        [JsonProperty]
        public string DisplayName { get; set; }
        [JsonProperty]
        public string BestPPCPrice { get; set; }
        [JsonProperty]
        public bool IsUpComing { get; set; }
        [JsonProperty]
        public string ClickOutUrl { get; set; }
        [JsonProperty]
        public string LinkUrl { get; set; }
        [JsonProperty]
        public string ImageAlt { get; set; }
        [JsonProperty]
        public string StarsImage { get; set; }
        [JsonProperty]
        public string StarsImageAlt { get; set; }
        [JsonProperty]
        public string ComparePriceString { get; set; }
        [JsonProperty]
        public bool IsSearchOnly { get; set; }
        [JsonProperty]
        public string AttrDescription { get; set; }
        [JsonProperty]
        public float Sale { get; set; }
        [JsonProperty]
        public double PrevPrice { get; set; }
        [JsonProperty]
        public double CurrentPrice { get; set; }
        [JsonProperty]
        public float RatingPercent { get; set; }
        public bool IsTop3 { get; set; }
        public int Position { get; set; }
    }
}