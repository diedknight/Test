using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PriceMeCommon.Data
{
    public class NarrowItem
    {
        public float FloatValue { get; set; }
        public int ListOrder { get; set; }
        public string DisplayName { get; set; }
        public string Value { get; set; }
        public int ProductCount { get; set; }
        public bool IsPopular { get; set; }
        public string Url { get; set; }
        public object OtherInfo { get; set; }

        public NarrowItem()
        {
            IsPopular = true;
        }
    }
}