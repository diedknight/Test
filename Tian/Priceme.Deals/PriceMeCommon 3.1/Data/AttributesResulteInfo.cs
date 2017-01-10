using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PriceMeCommon.Data
{
    public class AttributesResulteInfo
    {
        int _id;
        string _attributeValue;
        int _productCount;
        bool _isRange;
        int _attributeTilteID;
        bool _popularAttributeValue;
        string _url;

        public int ID
        {
            get { return _id; }
        }

        public string AttributeValue
        {
            get { return _attributeValue; }
        }

        public int ProductCount
        {
            get { return _productCount; }
            set { _productCount = value; }
        }

        public bool IsRange
        {
            get { return _isRange; }
        }

        public int AttributeTilteID
        {
            get { return _attributeTilteID; }
        }

        public bool PopularAttributeValue
        {
            get { return _popularAttributeValue; }
        }

        public string Url
        {
            get { return _url; }
            set { this._url = value; }
        }

        public AttributesResulteInfo(int id,
                                     string attributeValue,
                                     bool isRange,
                                     int attributeTilteID,
                                     bool popularAttributeValue)
        {
            this._id = id;
            this._attributeValue = attributeValue;
            this._isRange = isRange;
            this._attributeTilteID = attributeTilteID;
            this._popularAttributeValue = popularAttributeValue;
        }
    }
}