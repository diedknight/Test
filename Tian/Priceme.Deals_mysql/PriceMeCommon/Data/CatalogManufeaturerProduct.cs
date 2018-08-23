using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PriceMeCommon.Data
{
    [Serializable]
    public class CatalogManufeaturerProduct : IComparable<CatalogManufeaturerProduct>
    {
        public LinkInfo ManufacturerCache = null;
        public List<ProductCatalog> ProductCatalogCollection = null;
        public bool NeedDisplayMoreLink = false;
        public string MoreLinkUrl = "";        
        public Dictionary<string, string> ps;
        public List<string> DisplayAllManufeaturerProducts;

        public CatalogManufeaturerProduct()
        {
            ManufacturerCache = new LinkInfo();
            ProductCatalogCollection = new List<ProductCatalog>();
        }

        #region IComparable<CatalogManufeaturerProduct> Members

        public int CompareTo(CatalogManufeaturerProduct other)
        {
            return this.ManufacturerCache.LinkText.CompareTo(other.ManufacturerCache.LinkText);
        }

        #endregion
    }
}