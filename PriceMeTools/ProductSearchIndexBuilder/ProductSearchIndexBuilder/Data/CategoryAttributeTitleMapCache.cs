using System;
using System.Collections.Generic;
using System.Text;

namespace ProductSearchIndexBuilder.Data
{
    public class CategoryAttributeTitleMapCache
    {
        public int MapID { get; set; }
        public int CategoryID { get; set; }
        public int AttributeTitleID { get; set; }
        public bool IsPrimary { get; set; }
        public int AttributeOrder { get; set; }
        public bool IsSlider { get; set; }
        public float Step { get; set; }
        public float Step2 { get; set; }
        public int Step3 { get; set; }
        public float MinValue { get; set; }
        public float MaxValue { get; set; }
    }
}