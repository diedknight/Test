using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PriceMeCommon.Data
{
    public class CategoryResultsInfo
    {
        int _categoryID;
        string _categoryName;
        int _count;
        string _url;

        public string Url
        {
            get { return _url; }
            set { _url = value; }
        }

        public int CategoryID
        {
            get { return _categoryID; }
        }

        public string CategoryName
        {
            get { return _categoryName; }
        }

        public int Count
        {
            get { return _count; }
        }

        public CategoryResultsInfo(int categoryID, string categoryName, int count)
        {
            this._categoryID = categoryID;
            this._categoryName = categoryName;
            this._count = count;
        }
    }
}