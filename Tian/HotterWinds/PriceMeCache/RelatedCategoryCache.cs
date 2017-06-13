using System;
using System.Collections.Generic;

namespace PriceMeCache
{
    [Serializable]
    public class RelatedCategoryCache
    {
        string _categoryName;
        string _url;
        string _categoryID;

        public string CategoryID
        {
            get { return _categoryID; }
            set { _categoryID = value; }
        }

        public string CategoryName
        {
            get { return _categoryName; }
            set { _categoryName = value; }
        }

        public string Url
        {
            get { return _url; }
            set { _url = value; }
        }

        
    }
}