using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceMeCommon.Data
{
    public class CategoryHWDInfo
    {
        public int CategoryId { get; set; }
        public bool Heightslider { get; set; }
        public bool Widthslider { get; set; }
        public bool Depthslider { get; set; }
        public bool Weightslider { get; set; }
        /// <summary>
        /// 为false，单位为G，ture为KG
        /// </summary>
        public bool WeightUnitIsKG { get; set; }
    }
}
