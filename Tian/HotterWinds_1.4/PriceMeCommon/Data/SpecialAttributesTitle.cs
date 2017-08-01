using PriceMeCache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceMeCommon.Data
{
    public static class SpecialAttributesTitle
    {
        
        public static AttributeTitleCache HeightAttribute { get; private set; }
        public static AttributeTitleCache WidthAttribute { get; private set; }
        public static AttributeTitleCache LengthAttribute { get; private set; }
        public static AttributeTitleCache WeightAttribute { get; private set; }

        static SpecialAttributesTitle()
        {
            HeightAttribute = new AttributeTitleCache();
            HeightAttribute.TypeID = -1;
            HeightAttribute.Unit = "cm";
            HeightAttribute.Title = "Height";

            WidthAttribute = new AttributeTitleCache();
            WidthAttribute.TypeID = -2;
            WidthAttribute.Unit = "cm";
            WidthAttribute.Title = "Width";

            LengthAttribute = new AttributeTitleCache();
            LengthAttribute.TypeID = -3;
            LengthAttribute.Unit = "cm";
            LengthAttribute.Title = "Depth";

            WeightAttribute = new AttributeTitleCache();
            WeightAttribute.TypeID = -4;
            WeightAttribute.Unit = "";
            WeightAttribute.Title = "Weight";
        }
    }
}