using System;

namespace ProductSearchIndexBuilder.Data
{
    public class ProductVariants
    {
        public int ProductId { get; set; }
        public int LinedPID { get; set; }
        public string ProductName { get; set; }
        public string BaseProductValue { get; set; }
        public string VariantProductValue { get; set; }
        public int VariantType { get; set; }
        public string VariantTitleName { get; set; }
        public string Unit { get; set; }
        public string DisplayName { get; set; }
    }
}