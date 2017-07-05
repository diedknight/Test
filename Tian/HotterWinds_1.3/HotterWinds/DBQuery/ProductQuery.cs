using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dapper;
using System.Data.SqlClient;
using HotterWinds.ViewModels;
using HotterWinds.Extension;
using HotterWinds.HWUtility;

namespace HotterWinds.DBQuery
{
    public class ProductQuery : HotterWindsQuery
    {
        public static ViewModels.CategoryV GetCategory(int cid)
        {
            string sql = "select CategoryId,CategoryName,ParentId from CSK_Store_Category where CategoryId=@cId";

            var con = GetConnection();
            return con.Query<ViewModels.CategoryV>(sql, new { cId = cid }).FirstOrDefault();
        }

        public static List<ViewModels.CategoryV> GetCategoryList(int cid)
        {
            List<ViewModels.CategoryV> list = new List<ViewModels.CategoryV>();

            var cate = GetCategory(cid);
            while (cate != null)
            {
                list.Insert(0, cate);
                cate = GetCategory(cate.ParentId);
            }

            return list;
        }

        public static List<ViewModels.Product> GetRelatedProducts(int cid)
        {
            string sql = "select top 5"
                     + " ProductId, "
                     + " ProductName = (select top 1 ProductName from CSK_Store_Product where ProductID = a.ProductId), "
                     + " PurchaseURL, "
                     + " ImgUrl = (select top 1 DefaultImage from CSK_Store_Product where ProductID = a.ProductId), "
                     + " Stars = (select top 1 ProductRatingSum from CSK_Store_Product where ProductID = a.ProductId), "
                     + " RetailerPrice as Price, "
                     + " RPCount = (select count(1) from CSK_Store_RetailerProduct where ProductID = a.ProductId) "
                     + " from CSK_Store_RetailerProduct as a"
                     + " where ProductId in("
                     + "    select top 8 ProductId from CSK_Store_RetailerTracker"
                     + "    where retailerproductid in(select retailerproductid from csk_store_retailerproduct where retailerproductstatus = 1 and isdeleted = 0)"
                     + "	and CategoryID = @CId"
                     + "    group by ProductId order by count(ProductId) desc"
                     + " )";

            var con = GetConnection();
            var list = con.Query<ViewModels.Product>(sql, new { cId = cid }).ToList();

            list.ForEach(item => { item.ImgUrl = Utility.FixImagePath(item.ImgUrl, "_ms"); });

            return list;
        }

        public static ViewModels.ProductViewModel GetProductViewModel(int pid)
        {
            string sql = "select"
                    + " ProductId,"
                    + " ProductName,"
                    + " Price = (select top 1 RetailerPrice from CSK_Store_RetailerProduct where ProductID = a.ProductID),"
                    + " DefaultImage as ImgUrl,"
                    + " CategoryId,"
                    + " ShortDescriptionZH as QuickOverView,"
                    + " LongDescriptionEN as [Description]"
                    + " from CSK_Store_Product as a"
                    + " where ProductID = @PId";

            var con = GetConnection();
            var info = con.Query<ViewModels.ProductViewModel>(sql, new { PId = pid }).FirstOrDefault();
            if (info == null) return null;

            info.ImgUrl = Utility.FixImagePath(info.ImgUrl, "");
            info.CategoryList = new List<ViewModels.CategoryV>();
            var cate = GetCategory(info.CategoryId);
            while (cate != null)
            {
                info.CategoryList.Insert(0, cate);
                cate = GetCategory(cate.ParentId);
            }

            info.RelatedProducts = GetRelatedProducts(info.CategoryId);

            return info;
        }

        public static int GetProductCount(int categoryId, decimal minPrice = -1, decimal maxPrice = -1, int brandId = 0)
        {
            string sql = "  select count(*) from ("
                        + "     select"
                        + "     categoryId,"
                        + "     ProductStatus,"
                        + "     ProductID,DefaultImage as ImgUrl, "
                        + "     ProductName, "
                        + "     ProductRatingSum as Stars,"
                        + "     LongDescriptionEN as [Description],"
                        + "     Price = (select top 1 RetailerPrice from CSK_Store_RetailerProduct where ProductID = a.ProductID),"
                        + "     RPCount = (select count(1) from CSK_Store_RetailerProduct where ProductID = a.ProductID),"
                        + "     PurchaseUrl = (select top 1 PurchaseURL from CSK_Store_RetailerProduct where ProductID = a.ProductID)"
                        + "     from CSK_Store_Product as a"
                        + " ) as b"
                        + " where ProductStatus=1 {0}";

            string where = "";
            if (categoryId != 0) where += " and categoryId = @cId";
            if (minPrice > 0) where += " and Price >= @minPrice";
            if (maxPrice > 0) where += " and Price <= @maxPrice";
            if (brandId != 0) where += "";

            sql = string.Format(sql, where);

            var con = GetConnection();

            return con.ExecuteScalar<int>(sql, new { cId = categoryId, minPrice = minPrice, maxPrice = maxPrice, brandId = brandId });
        }

        public static List<ViewModels.Product1> GetProducts(int categoryId, decimal minPrice = -1, decimal maxPrice = -1, int brandId = 0, string orderbySql = "order by b.ProductID asc", int pageNum = 1, int pageSize = 20)
        {
            List<ViewModels.Product1> list = new List<ViewModels.Product1>();

            string sql = "  select ROW_NUMBER() over(" + orderbySql + ") as rownum, * from ("
                        + "     select"
                        + "     categoryId,"
                        + "     ProductStatus,"
                        + "     ProductID,DefaultImage as ImgUrl, "
                        + "     ProductName, "
                        + "     ProductRatingSum as Stars,"
                        + "     LongDescriptionEN as [Description],"
                        + "     Price = (select top 1 RetailerPrice from CSK_Store_RetailerProduct where ProductID = a.ProductID),"
                        + "     RPCount = (select count(1) from CSK_Store_RetailerProduct where ProductID = a.ProductID),"
                        + "     PurchaseUrl = (select top 1 PurchaseURL from CSK_Store_RetailerProduct where ProductID = a.ProductID)"
                        + "     from CSK_Store_Product as a"
                        + " ) as b"
                        + " where ProductStatus=1 {0}";

            string where = "";
            if (categoryId != 0) where += " and categoryId = @cId";
            if (minPrice > 0) where += " and Price >= @minPrice";
            if (maxPrice > 0) where += " and Price <= @maxPrice";
            if (brandId != 0) where += "";

            sql = string.Format(sql, where);
            sql = sql.SQLWrapPageStr(pageNum, pageSize);

            var con = GetConnection();
            list = con.Query<ViewModels.Product1>(sql, new { cId = categoryId, minPrice = minPrice, maxPrice = maxPrice, brandId = brandId }).ToList();

            list.ForEach(item => { item.ImgUrl = Utility.FixImagePath(item.ImgUrl, "_ms"); });

            return list;
        }

    }
}