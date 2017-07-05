using System;
using System.Collections.Generic;

namespace PriceMeCache
{
    [Serializable]
    public class AttributeValueCache
    {
        private int attributeValueID;

        public int AttributeValueID
        {
            get { return attributeValueID; }
            set { attributeValueID = value; }
        }
        private int attributeTitleID;

        public int AttributeTitleID
        {
            get { return attributeTitleID; }
            set { attributeTitleID = value; }
        }
        private string _value;

        public string Value
        {
            get { return _value; }
            set { _value = value; }
        }
        private int listOrder;

        public int ListOrder
        {
            get { return listOrder; }
            set { listOrder = value; }
        }
        private bool popularAttribute;

        public bool PopularAttribute
        {
            get { return popularAttribute; }
            set { popularAttribute = value; }
        }

        float _NumericValue;
        public float NumericValue
        {
            get { return _NumericValue; }
            set { _NumericValue = value; }
        }
    }
}