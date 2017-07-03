using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PriceMeCommon.BusinessLogic;
using PriceMeCache;

namespace PriceMeCommon.Data
{
    public class CatalogPageInfo
    {
        [JsonProperty]
        public int PageCount { get; set; }
        [JsonProperty]
        public int CurrentProductCount { get; set; }
        [JsonProperty]
        public int TotalProductCount { get; set; }
        [JsonProperty(PropertyName = "Products")]
        public List<ProductCatalog> ProductCatalogList { get; set; }
        [JsonIgnore]
        public ProductSearcher MyProductSearcher { get; set; }
    }
}
