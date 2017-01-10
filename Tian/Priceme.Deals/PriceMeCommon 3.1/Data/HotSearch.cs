using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PriceMeCommon.Data
{
    public class HotSearch : IComparable<HotSearch>
    {
        string keywords;
        int count;
        int number;
        bool hasResult;

        public int Number
        {
            get { return number; }
            set { number = value; }
        }

        public int Count
        {
            get { return count; }
            set { count = value; }
        }

        public string Keywords
        {
            get { return keywords; }
            set { keywords = value; }
        }

        public bool HasResult
        {
            get { return hasResult; }
            set { hasResult = value; }
        }


        #region IComparable<HotSearch> 成员

        public int CompareTo(HotSearch other)
        {
            return this.keywords.CompareTo(other.Keywords);
        }

        #endregion
    }
}