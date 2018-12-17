using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PricemeResource.Data
{
    [Serializable]
    public class ProductIsMergedData
    {
        public int ProductID { get; set; }
        public int ToProductID { get; set; }
    }
}
