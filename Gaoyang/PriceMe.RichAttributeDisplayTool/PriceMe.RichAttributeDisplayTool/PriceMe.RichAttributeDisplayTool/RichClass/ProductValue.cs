using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceMe.RichAttributeDisplayTool.RichClass
{
    class ProductValue:IClass
    {
        public int ProductID { get; set; }
        public double Value { get; set; }

    }
}
