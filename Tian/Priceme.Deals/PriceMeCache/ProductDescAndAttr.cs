using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PriceMeCache
{
    [Serializable]
    public partial class ProductDescAndAttr
    {
        public string Title { get; set; }

        public string ShortDescription { get; set; }
        public string Value { get; set; }
        public string Unit { get; set; }
        public int T { get; set; }
        public int AVS { get; set; }
        public int CID { get; set; }
        public int AttributeTypeID { get; set; }
        public int AttributeTitleID { get; set; }

        public int ProductId { get; set; }
    }
}
