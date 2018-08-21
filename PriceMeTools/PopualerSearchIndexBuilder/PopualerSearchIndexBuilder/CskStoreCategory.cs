using System;
using System.Collections.Generic;

namespace PopualerSearchIndexBuilder
{
    public class CskStoreCategory
    {
        [System.ComponentModel.DataAnnotations.Key]
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string CategoryNameEn { get; set; }
        public string ImageFile { get; set; }
        public int? ParentId { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public int ListOrder { get; set; }
        public string AdminComments { get; set; }
        public int? MinPrice { get; set; }
        public int? MaxPrice { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
        public bool IsActive { get; set; }
        public bool PopularCategory { get; set; }
        public bool IsAccessories { get; set; }
        public string TopBrands { get; set; }
        public string AttributeId { get; set; }
        public decimal? DefaultWeight { get; set; }
        public string IsBulk { get; set; }
        public bool? IsSiteMap { get; set; }
        public string BannerCategoryName { get; set; }
        public bool? IsSiteMapPopular { get; set; }
        public bool? IsSiteMapDetail { get; set; }
        public bool? IsFooterCategory { get; set; }
        public bool? IsSiteMapDetailPopular { get; set; }
        public bool AttributesCategory { get; set; }
        public bool? IsAutomatic { get; set; }
        public bool? IsDisplayIsMerged { get; set; }
        public int? AdminId { get; set; }
        public bool? DebugAutomatic { get; set; }
        public bool? IsProductMatchCategory { get; set; }
        public bool? IsRenameCategory { get; set; }
        public bool? IsStandardProductName { get; set; }
        public bool IsFilterByBrand { get; set; }
        public bool IsMergeByKeyword { get; set; }
        public bool IsSearchOnly { get; set; }
        public int CategoryViewType { get; set; }
        public bool WeightUnit { get; set; }
        public string Categoryicon { get; set; }
        public string Iconcode { get; set; }
    }
}
