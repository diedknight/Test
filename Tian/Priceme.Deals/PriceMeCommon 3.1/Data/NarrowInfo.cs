using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PriceMeCommon.Data
{
    public class NarrowItem
    {
        string _value;
        int _productCount = 0;
        bool _isPopular = true;
        string _url;
        string _displayName;
        int _listOrder = 0;
        private object _otherInfo;

        public float FloatValue { get; set; }

        public int ListOrder
        {
            get { return _listOrder; }
            set { _listOrder = value; }
        }

        public string DisplayName
        {
            get { return _displayName; }
            set { _displayName = value; }
        }

        public string Value
        {
            get { return _value; }
            set { _value = value; }
        }

        public int ProductCount
        {
            get { return _productCount; }
            set { _productCount = value; }
        }

        public bool IsPopular
        {
            get { return _isPopular; }
            set { _isPopular = value; }
        }

        public string Url
        {
            get { return _url; }
            set { _url = value; }
        }

        public object OtherInfo
        {
            get { return _otherInfo; }
            set { _otherInfo = value; }
        }
    }
}