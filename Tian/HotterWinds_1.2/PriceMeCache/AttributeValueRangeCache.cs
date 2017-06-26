using System;
using System.Collections.Generic;

namespace PriceMeCache
{
    [Serializable]
    public class AttributeValueRangeCache
    {
        private int valueRangeID;

        public int ValueRangeID
        {
            get { return valueRangeID; }
            set { valueRangeID = value; }
        }
        private int categoryID;

        public int CategoryID
        {
            get { return categoryID; }
            set { categoryID = value; }
        }
        private int attributeTitleID;

        public int AttributeTitleID
        {
            get { return attributeTitleID; }
            set { attributeTitleID = value; }
        }
        private int minValue;

        public int MinValue
        {
            get { return minValue; }
            set { minValue = value; }
        }
        private int maxValue;

        public int MaxValue
        {
            get { return maxValue; }
            set { maxValue = value; }
        }
    }
}