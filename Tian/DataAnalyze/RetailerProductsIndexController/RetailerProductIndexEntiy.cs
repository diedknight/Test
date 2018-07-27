using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RetailerProductsIndexController
{
    public class RetailerProductIndexEntiy
    {
        public string CategoryRank { get; set; }
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public string ManufacturerPartNumber { get; set; }
        public string ManufacturerID { get; set; }
        public string CategoryID { get; set; }
        public string RetailerAmount { get; set; }
        public string RetailerCount { get; set; }
        public string IsMerge { get; set; }
        public float BestPrice { get; set; }
        public double AvRating { get; set; }
        public string DefaultImage { get; set; }
        public string ShortDescriptionZH { get; set; }
        public string ProductRatingVotes { get; set; }
        public string ProductRatingSum { get; set; }
        public string Clicks { get; set; }
        public string CreatedOn { get; set; }
        public string BestRetailerName { get; set; }
        public string RetailerProductID { get; set; }
        public string ProductReviewCount { get; set; }
        /// <summary>
        /// 1 or 0
        /// </summary>
        public string IsAccessories { get; set; }
        public string Keywords { get; set; }
        public string PPCLogoPath { get; set; }
        public string PPCRetailerProductID { get; set; }
        public string PPCLogo { get; set; }
        public string RetailerProductList { get; set; }
    }
}
