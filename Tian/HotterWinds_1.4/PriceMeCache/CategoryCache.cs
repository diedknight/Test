using System;
using System.Collections.Generic;

namespace PriceMeCache
{
    [Serializable]
    public class CategoryCache
    {
        public int Clicks { get; set; }
        public int ProductsCount { get; set; }
        public int CategoryID { get; set; }
        public string CategoryGuid { get; set; }
        public string CategoryName { get; set; }
        public string CategoryNameEN { get; set; }
        public string ImageFile { get; set; }
        public int ParentID { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public int ListOrder { get; set; }
        public string AdminComments { get; set; }
        public int MinPrice { get; set; }
        public int MaxPrice { get; set; }
        public bool IsActive { get; set; }
        public bool PopularCategory { get; set; }
        public bool IsAccessories { get; set; }
        public bool IsSiteMap { get; set; }
        public bool IsSiteMapDetail { get; set; }
        public bool IsSiteMapDetailPopular { get; set; }
        public bool IsSiteMapPopular { get; set; }
        public string TopBrands { get; set; }
        public string AttributeID { get; set; }
        public int CategoryTotal { get; set; }
        public string CategoryRootName { get; set; }
        public int CategoryRootID { get; set; }
        public string CategoryRootGUID { get; set; }
        public bool AttributesCategory { get; set; }
        public bool IsFooterCategory { get; set; }
        public bool IsDisplayIsMerged { get; set; }
        public bool IsAutomatic { get; set; }
        public string DisplayName { get; set; }
        public bool IsFilterByBrand { get; set; }
        public bool IsShortCutPopular { get; set; }
        public string LocalDescription { get; set; }
        /// <summary>
        /// 1 Grid, 2 List, 3 Quick
        /// </summary>
        public int CategoryViewType { get; set; }
        public string IconUrl { get; set; }
        public string Categoryicon { get; set; }
        public string CategoryIconCode { get; set; }
        public bool IsSearchOnly { get; set; }
        public bool WeightUnit { get; set; }

        public CategoryCache()
        {
            IsActive = false;
            PopularCategory = false;
            IsAccessories = false;
            IsSiteMap = false;
            IsSiteMapDetail = false;
            IsSiteMapDetailPopular = false;
            IsSiteMapPopular = false;
            AttributesCategory = false;
            IsFooterCategory = false;
            IsDisplayIsMerged = false;
            IsAutomatic = false;
            IsFilterByBrand = true;
            IsShortCutPopular = false;
            CategoryViewType = 1;
            WeightUnit = true;
        }
    }
}
