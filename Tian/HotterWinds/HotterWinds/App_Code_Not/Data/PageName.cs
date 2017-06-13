using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PriceMe
{
    public enum PageName
    {
        Catalog,
        Product,
        Search,
        RetailerInfo,
        RetailerList,
        Brand,
        BuyingGuide,
        RetailerReview,
        RetailerCategory,
        AllBrands,
        AllList,
        ExpertReview,
        all_lists,
        AllRrecentPriceDrop
    }

    #region ruangang
    public enum ForumPageName {
        TopicList,
        Topic,
        NewTopic,
        ForumAll,
        Edit
    }
    #endregion

    #region huangriling
    public enum CssFile
    {
        Home = 0,
        Catalog = 1,
        Product = 2,
        Retailer = 3,
        Brand = 4,
        Compare = 5,
        About = 6
    }
    #endregion
}