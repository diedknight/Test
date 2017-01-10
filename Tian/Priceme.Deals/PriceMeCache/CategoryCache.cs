using System;
using System.Collections.Generic;

namespace PriceMeCache
{
    [Serializable]
    public class CategoryCache
    {
        public int Clicks
        {
            get;
            set;
        }

        private int categoryID = 0;
        public int CategoryID
        {
            get { return categoryID; }
            set { categoryID = value; }
        }
        
        private string categoryGuid = "";
        public string CategoryGuid
        {
            get { return categoryGuid; }
            set { categoryGuid = value; }
        }

        private string categoryName = "";
        public string CategoryName
        {
            get { return categoryName; }
            set { categoryName = value; }
        }

        private string categoryNameEN = "";
        public string CategoryNameEN
        {
            get { return categoryNameEN; }
            set { categoryNameEN = value; }
        }

        private string imageFile = "";
        public string ImageFile
        {
            get { return imageFile; }
            set { imageFile = value; }
        }

        private int parentID = 0;
        public int ParentID
        {
            get { return parentID; }
            set { parentID = value; }
        }
 
        private string shortDescription = "";
        public string ShortDescription
        {
            get { return shortDescription; }
            set { shortDescription = value; }
        }

        private string longDescription = "";
        public string LongDescription
        {
            get { return longDescription; }
            set { longDescription = value; }
        }

        private int listOrder = 0;
        public int ListOrder
        {
            get { return listOrder; }
            set { listOrder = value; }
        }

        private string adminComments = "";
        public string AdminComments
        {
            get { return adminComments; }
            set { adminComments = value; }
        }

        private int minPrice = 0;
        public int MinPrice
        {
            get { return minPrice; }
            set { minPrice = value; }
        }

        private int maxPrice = 0;
        public int MaxPrice
        {
            get { return maxPrice; }
            set { maxPrice = value; }
        }

        private bool isActive = false;
        public bool IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }

        private bool popularCategory = false;
        public bool PopularCategory
        {
            get { return popularCategory; }
            set { popularCategory = value; }
        }

        private bool isAccessories = false;
        public bool IsAccessories
        {
            get { return isAccessories; }
            set { isAccessories = value; }
        }

        private bool isSiteMap = false;
        public bool IsSiteMap
        {
            get { return isSiteMap; }
            set { isSiteMap = value; }
        }

        private bool isSiteMapDetail = false;
        public bool IsSiteMapDetail
        {
            get { return isSiteMapDetail; }
            set { isSiteMapDetail = value; }
        }

        private bool isSiteMapDetailPopular = false;
        public bool IsSiteMapDetailPopular
        {
            get { return isSiteMapDetailPopular; }
            set { isSiteMapDetailPopular = value; }
        }

        private bool isSiteMapPopular = false;
        public bool IsSiteMapPopular
        {
            get { return isSiteMapPopular; }
            set { isSiteMapPopular = value; }
        }

        private string topBrands = "";
        public string TopBrands
        {
            get { return topBrands; }
            set { topBrands = value; }
        }

        private string attributeID = "";
        public string AttributeID
        {
            get { return attributeID; }
            set { attributeID = value; }
        }

        private int categoryTotal = 0;
        public int CategoryTotal
        {
            get { return categoryTotal; }
            set { categoryTotal = value; }
        }

        private string categoryRootName = "";
        public string CategoryRootName
        {
            get { return categoryRootName; }
            set { categoryRootName = value; }
        }

        private int categoryRootID = 0;
        public int CategoryRootID
        {
            get { return categoryRootID; }
            set { categoryRootID = value; }
        }

        private string _categoryRootGUID = "";
        public string CategoryRootGUID
        {
            get { return _categoryRootGUID; }
            set { _categoryRootGUID = value; }
        }

        private bool _attributesCategory = false;
        public bool AttributesCategory
        {
            get { return _attributesCategory; }
            set { _attributesCategory = value; }
        }

        private bool _isFooterCategory = false;
        public bool IsFooterCategory
        {
            get { return _isFooterCategory; }
            set { _isFooterCategory = value; }
        }

        private bool _isDisplayIsMerged = false;
        public bool IsDisplayIsMerged
        {
            get { return _isDisplayIsMerged; }
            set { _isDisplayIsMerged = value; }
        }

        private bool _isAutomatic = false;
        public bool IsAutomatic
        {
            get { return _isAutomatic; }
            set { _isAutomatic = value; }
        }

        private string _displayName;
        public string DisplayName
        {
            get { return _displayName; }
            set { _displayName = value; }
        }

        private bool _isFilterByBrand;
        public bool IsFilterByBrand
        {
            get { return _isFilterByBrand; }
            set { _isFilterByBrand = value; }
        }

        private bool _isShortCutPopular = false;
        public bool IsShortCutPopular
        {
            get { return _isShortCutPopular; }
            set { _isShortCutPopular = value; }
        }

        private string _localDescription;
        public string LocalDescription
        {
            get { return _localDescription; }
            set { _localDescription = value; }
        }

        public int CategoryViewType { get; set; }
        public string IconUrl { get; set; }
        public string Categoryicon { get; set; }
        public string CategoryIconCode { get; set; }
    }
}
