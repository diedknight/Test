using System;
using System.Collections.Generic;

namespace ProductSearchIndexBuilder.Data
{
    public class AttributeGroup
    {
        public int AttributeGroupId { get; set; }
        public string AttributeGroupName { get; set; }
        public List<AttributeGroupList> AttributeGroupList { get; set; }
        public int OrderID { get; set; }
    }
}