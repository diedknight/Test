using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PopualerSearchIndexBuilder
{
    public static class DataController
    {
        static Dictionary<int, List<CategoryCache>> NextLevelActiveCategories_Static;

        public static void Init(DbInfo dbInfo)
        {
            List<CategoryCache> categoryOrderByName = GetCatOrderByNameOrderByName(dbInfo);

            NextLevelActiveCategories_Static = new Dictionary<int, List<CategoryCache>>();
            foreach (var c in categoryOrderByName)
            {
                List<CategoryCache> categories = categoryOrderByName.Where(cat => cat.ParentID == c.CategoryID).ToList();
                NextLevelActiveCategories_Static.Add(c.CategoryID, categories);
            }
        }

        private static List<CategoryCache> GetCatOrderByNameOrderByName(DbInfo dbInfo)
        {
            List<CategoryCache> categoryOrderByName = new List<CategoryCache>();
            string selectSql = "select CT.*,LCT.CategoryName as LocalName,LCT.Description as LocalDesc,LCT.ShortDescription as LocalShortDesc from CSK_Store_Category as CT left join Local_CategoryName as LCT on CT.CategoryID = LCT.CategoryID and LCT.CountryID = " + dbInfo.CountryId + " where CT.IsActive = 1 order by CT.CategoryName";
            ;
            using (var sqlConn = DBController.CreateDBConnection(dbInfo))
            {
                using (var sqlCMD = DBController.CreateDbCommand(selectSql, sqlConn))
                {
                    sqlConn.Open();
                    using (var sqlDR = sqlCMD.ExecuteReader())
                    {
                        while (sqlDR.Read())
                        {
                            CategoryCache categoryCache = new CategoryCache();
                            categoryCache.AdminComments = sqlDR["AdminComments"].ToString();
                            categoryCache.AttributeID = sqlDR["AttributeID"].ToString();
                            categoryCache.AttributesCategory = bool.Parse(sqlDR["AttributesCategory"].ToString());
                            categoryCache.CategoryID = int.Parse(sqlDR["CategoryID"].ToString());
                            categoryCache.CategoryName = sqlDR["CategoryName"].ToString();
                            categoryCache.CategoryNameEN = categoryCache.CategoryName;
                            string localName = sqlDR["LocalName"].ToString();
                            if (!string.IsNullOrEmpty(localName))
                            {
                                categoryCache.CategoryName = localName;
                            }
                            categoryCache.LocalDescription = sqlDR["LocalDesc"].ToString();
                            categoryCache.ImageFile = sqlDR["ImageFile"].ToString();
                            categoryCache.IsAccessories = bool.Parse(sqlDR["IsAccessories"].ToString());
                            categoryCache.IsActive = bool.Parse(sqlDR["IsActive"].ToString());
                            categoryCache.IsDisplayIsMerged = bool.Parse(sqlDR["IsDisplayIsMerged"].ToString());
                            categoryCache.IsFilterByBrand = bool.Parse(sqlDR["IsFilterByBrand"].ToString());
                            categoryCache.IsSiteMap = bool.Parse(sqlDR["IsSiteMap"].ToString());
                            categoryCache.IsSiteMapDetail = bool.Parse(sqlDR["IsSiteMapDetail"].ToString());
                            categoryCache.IsSiteMapDetailPopular = bool.Parse(sqlDR["IsSiteMapDetailPopular"].ToString());
                            categoryCache.IsSiteMapPopular = bool.Parse(sqlDR["IsSiteMapPopular"].ToString());
                            categoryCache.LongDescription = sqlDR["LongDescription"].ToString();

                            int maxPrice;
                            int.TryParse(sqlDR["MaxPrice"].ToString(), out maxPrice);
                            categoryCache.MaxPrice = maxPrice;

                            int minPrice;
                            int.TryParse(sqlDR["MinPrice"].ToString(), out minPrice);
                            categoryCache.MinPrice = minPrice;

                            categoryCache.ParentID = int.Parse(sqlDR["ParentID"].ToString());
                            categoryCache.PopularCategory = bool.Parse(sqlDR["PopularCategory"].ToString());
                            categoryCache.ShortDescription = sqlDR["LocalShortDesc"].ToString();
                            categoryCache.TopBrands = sqlDR["TopBrands"].ToString();
                            categoryCache.Categoryicon = sqlDR["categoryicon"].ToString();
                            categoryCache.CategoryIconCode = sqlDR["iconcode"].ToString();
                            categoryCache.IconUrl = sqlDR["categoryicon"].ToString();
                            categoryCache.IsSearchOnly = bool.Parse(sqlDR["IsSearchOnly"].ToString());
                            categoryCache.CategoryViewType = int.Parse(sqlDR["CategoryViewType"].ToString());
                            categoryCache.WeightUnit = bool.Parse(sqlDR["WeightUnit"].ToString());
                            categoryCache.ListOrder = int.Parse(sqlDR["ListOrder"].ToString());

                            categoryOrderByName.Add(categoryCache);
                        }
                    }
                }
            }

            return categoryOrderByName;
        }

        public static List<int> GetAllSubCategoryId(int cId)
        {
            List<int> categoryIDs = new List<int>();

            List<CategoryCache> categoryCaches = GetNextLevelSubCategories(cId);
            foreach (var cache in categoryCaches)
            {
                SetAllSubCategoryIds(categoryIDs, cache.CategoryID);
            }

            return categoryIDs;
        }

        private static void SetAllSubCategoryIds(List<int> categoryIDs, int cid)
        {
            categoryIDs.Add(cid);

            List<CategoryCache> categoryCaches = GetNextLevelSubCategories(cid);
            foreach (var cache in categoryCaches)
            {
                SetAllSubCategoryIds(categoryIDs, cache.CategoryID);
            }
        }

        /// <summary>
        /// 获取下一级分类
        /// </summary>
        /// <param name="cid"></param>
        /// <param name="countryId"></param>
        /// <returns></returns>
        public static List<CategoryCache> GetNextLevelSubCategories(int cid)
        {
            if (NextLevelActiveCategories_Static.ContainsKey(cid))
                return NextLevelActiveCategories_Static[cid];

            return new List<CategoryCache>();
        }
    }
}
