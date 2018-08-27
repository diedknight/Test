using System;

namespace ProductSearchIndexBuilder.Data
{
    public class AttributeTitleCache
    {
        public int TypeID { get; set; }

        public string Title { get; set; }

        public int LessThan { get; set; }

        public int MoreThan { get; set; }

        public string IsNumeric { get; set; }

        public string Unit { get; set; }

        public string ShortDescription { get; set; }

        public int AttributeGroupID { get; set; }

        public int AttributeTypeID {get; set;}
    }
}