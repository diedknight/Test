using System;
using System.Collections.Generic;
using System.Web;

namespace Commerce.Common
{
    public class CategoryInfo
    {
        int categoryID;
        int productCount;
        string categoryName;
        int level;

        public int CategoryID
        {
            get { return categoryID; }
            set { categoryID = value; }
        }

        public int ProductCount
        {
            get { return productCount; }
            set { productCount = value; }
        }

        public string CategoryName
        {
            get { return categoryName; }
            set { categoryName = value; }
        }

        public int Level
        {
            get { return level; }
            set { level = value; }
        }
    }
}