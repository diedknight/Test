using System;

namespace ProductSearchIndexBuilder.Data
{
    public class AttributeGroupList
    {
        public int AttributeId { get; set; }
        public string AttributeName { get; set; }
        public int Avs { get; set; }
        public string ShortDescription { get; set; }
        public string Value { get; set; }
        public string Unit { get; set; }
        public int T { get; set; }
        public int AttributeTypeID { get; set; }
        public bool IsCategoryAttribute { get; set; }
    }
}