using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceMeCommon.Data
{
    public class CatalogAttributeGroupInfo
    {
        public int CatalogAttributeGroupID { get; set; }
        public string CatalogAttributeGroupName { get; set; }
        public List<int> AttributeValues { get; set; }

        public CatalogAttributeGroupInfo()
        {
            AttributeValues = new List<int>();
        }
    }
}
