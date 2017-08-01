//using System;
//using System.Data;
//using System.Configuration;
//using System.Web;
//using System.Web.Security;
//using System.Web.UI;
//using System.Web.UI.WebControls;
//using System.Web.UI.WebControls.WebParts;
//using System.Web.UI.HtmlControls;
//using Commerce.Common;

///// <summary>
///// Summary description for CategoryProductNumber
///// </summary>
//public class CategoryProductNumber
//{
//    public CategoryProductNumber(int cid, int num)
//    {
//        categoryId = cid;
//        productNumber = num;
//        category = (CategoryCache)HttpContext.Current.Application[cid.ToString() + "-CategoryCount"];
//    }

//    //public string CategoryGUID
//    //{
//    //    get  {
//    //        if (category == null)
//    //        {
//    //            return CategoryController.GetCategoryByCategoryID(categoryId).CategoryGUID;
//    //        }
//    //        return category.CategoryGuid; }
//    //}

//    public string CategoryName
//    {
//        get {
//            if (category == null)
//            {
//                return CategoryController.GetCategoryByCategoryID(categoryId).CategoryName;
//            }
//            return category.CategoryName; }
//    }

//    public int categoryId;
//    public int productNumber;
//    private CategoryCache category;
//}
