using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PriceMe
{
    public class AttributeStringSort : IComparer<string>
    {
        public AttributeStringSort()
        {
            //
            //TODO: 在此处添加构造函数逻辑
            //
        }

        #region IComparer<string> 成员

        public int Compare(string x, string y)
        {
            int a = int.Parse(x.Replace("r", ""));
            int b = int.Parse(y.Replace("r", ""));
            return a.CompareTo(b);
        }

        #endregion
    }
}