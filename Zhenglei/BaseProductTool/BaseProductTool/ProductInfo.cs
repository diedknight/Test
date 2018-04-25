using System;

namespace BaseProductTool
{
    public class ProductInfo
    {
        public int ProductId { get; set; }
        public int CategoryId { get; set; }
        public string ProductName { get; set; }
        public string ProductNameLower { get; set; }
        public float AttributeData { get; set; }
        public int Clicks { get; set; }
    }
}
