using System;
using System.Collections.Generic;
using System.Web;

public class DkCategoryCache
{
    int categoryID;
    int productCount;
    string categoryName;
    bool popularCategory;
    bool isSiteMapDetail;

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

    public bool PopularCategory
    {
        get { return popularCategory; }
        set { popularCategory = value; }
    }

    public bool IsSiteMapDetail
    {
        get { return isSiteMapDetail; }
        set { isSiteMapDetail = value; }
    }
}