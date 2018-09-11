using ProductSearchIndexBuilder.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProductSearchIndexBuilder
{
    public static class CachBuilder
    {
        static RedisManager RedisManager_Static;

        public static void Init(string redisHost, string redisName)
        {
            RedisManager_Static = new RedisManager(redisHost, redisName);
        }

        public static void BuildCache()
        {
            List<CategoryCache> categoryCaches = GetCategoryByNameWithClicks();

            if (categoryCaches.Count > 0)
            {
                RedisManager_Static.Set<List<CategoryCache>>(CacheKey.CategoryByNameWithClicks, categoryCaches);
                var r = RedisManager_Static.Get<List<CategoryCache>>(CacheKey.CategoryByNameWithClicks);
            }
            LogController.WriteLog(CacheKey.CategoryByNameWithClicks + " count :" + categoryCaches.Count);

            //可以优化
            Dictionary<int, List<AttributeGroup>> attGroupDic = GetAllCategoryAttributeGroup();
            if (attGroupDic.Count > 0)
            {
                RedisManager_Static.Set<Dictionary<int, List<AttributeGroup>>>(CacheKey.AllCategoryAttributeGroup, attGroupDic);
                var r = RedisManager_Static.Get<Dictionary<int, List<AttributeGroup>>>(CacheKey.AllCategoryAttributeGroup);
            }
            LogController.WriteLog(CacheKey.AllCategoryAttributeGroup + " count :" + attGroupDic.Count);

            List<RetailerCache> retailerListOrderByNameWithClicks = DataController.GetRetailersWithVotesSumOrderByClicksFromDB();
            if (retailerListOrderByNameWithClicks.Count > 0)
            {
                RedisManager_Static.Set<List<RetailerCache>>(CacheKey.RetailerListOrderByNameWithClicks, retailerListOrderByNameWithClicks);
                var r = RedisManager_Static.Get<List<RetailerCache>>(CacheKey.RetailerListOrderByNameWithClicks);
            }
            LogController.WriteLog(CacheKey.RetailerListOrderByNameWithClicks + " count :" + retailerListOrderByNameWithClicks.Count);

            Dictionary<string,string> statusBarData = DataController.GetStatusBarData();
            if (statusBarData.Count > 0)
            {
                RedisManager_Static.Set<Dictionary<string, string>>(CacheKey.StatusBarInfo, statusBarData);
                var r = RedisManager_Static.Get<Dictionary<string, string>>(CacheKey.StatusBarInfo);
            }
            LogController.WriteLog(CacheKey.StatusBarInfo + " count :" + statusBarData.Count);

            List<RetailerReviewCache> retailerReviewList = DataController.GetAllRetailerReviewList(AppValue.CountryId);
            if (retailerReviewList.Count > 0)
            {
                RedisManager_Static.Set<List<RetailerReviewCache>>(CacheKey.RetailerReviewList, retailerReviewList);
                var r = RedisManager_Static.Get<List<RetailerReviewCache>>(CacheKey.RetailerReviewList);
            }
            LogController.WriteLog(CacheKey.RetailerReviewList + " count :" + retailerReviewList.Count);

            List<FeaturedTabCache> featuredTabCaches = DataController.GetAllFeaturedProducts(AppValue.CountryId);
            if (featuredTabCaches.Count > 0)
            {
                RedisManager_Static.Set<List<FeaturedTabCache>>(CacheKey.FeaturedProducts, featuredTabCaches);
                var r = RedisManager_Static.Get<List<FeaturedTabCache>>(CacheKey.FeaturedProducts);
            }
            LogController.WriteLog(CacheKey.FeaturedProducts + " count :" + featuredTabCaches.Count);

            Dictionary<int, string> energyImgs = DataController.GetEnergyImgs();
            if (energyImgs.Count > 0)
            {
                RedisManager_Static.Set<Dictionary<int, string>>(CacheKey.EnergyImgs, energyImgs);
                var r = RedisManager_Static.Get<Dictionary<int, string>>(CacheKey.EnergyImgs);
            }
            LogController.WriteLog(CacheKey.EnergyImgs + " count :" + energyImgs.Count);

            Dictionary<int, Dictionary<string, List<ProductVariants>>> variantsInfo = DataController.GetVariants();
            if (variantsInfo.Count > 0)
            {
                RedisManager_Static.Set<Dictionary<int, Dictionary<string, List<ProductVariants>>>>(CacheKey.ProductVariants, variantsInfo);
                var r = RedisManager_Static.Get<Dictionary<int, Dictionary<string, List<ProductVariants>>>>(CacheKey.ProductVariants);
            }
            LogController.WriteLog(CacheKey.ProductVariants + " count :" + variantsInfo.Count);
        }

        private static Dictionary<int, List<AttributeGroup>> GetAllCategoryAttributeGroup()
        {
            var attGroupDic = new Dictionary<int, List<AttributeGroup>>();

            try
            {
                var cidList = DataController.GetAllCategoryAttribute();
                
                Dictionary<int, AttributeGroup> attributeGroupDic = DataController.GetAllAttributeGroupDic(AppValue.CountryId);
                foreach (int cid in cidList)
                {
                    Dictionary<int, AttributeGroup> groupDic = new Dictionary<int, AttributeGroup>();
                    DataController.SetAllAttributeByCategoryId(cid, groupDic, attributeGroupDic);
                    DataController.SetAllCompareAttributeByCategoryId(cid, groupDic, attributeGroupDic);
                    List<AttributeGroup> groupList = new List<AttributeGroup>(groupDic.Values);
                    groupList = groupList.OrderBy(g => g.OrderID).ToList();
                    attGroupDic.Add(cid, groupList);
                }
            }
            catch(Exception ex)
            {
                LogController.WriteException(ex.Message + "\t" + ex.StackTrace);
            }
            return attGroupDic;
        }

        private static List<CategoryCache> GetCategoryByNameWithClicks()
        {
            List<CategoryCache> categoryCaches = new List<CategoryCache>();

            try
            {
                var allCategories = DataController.CategoryListOrderByName;
                var categoryClicksDic = DataController.CategoryClicksDic;
                var categoryProductsCountDic = DataController.CategoryProductsCountDic;

                foreach (var cate in allCategories)
                {
                    if (categoryProductsCountDic.ContainsKey(cate.CategoryID))
                    {
                        cate.ProductsCount = categoryProductsCountDic[cate.CategoryID];
                        if (categoryClicksDic.ContainsKey(cate.CategoryID))
                        {
                            cate.Clicks = categoryClicksDic[cate.CategoryID];
                        }
                        categoryCaches.Add(cate);
                    }
                    else
                    {
                        SetClicksAndProductsCount(cate, cate.CategoryID, categoryClicksDic, categoryProductsCountDic);
                        if (cate.ProductsCount > 0)
                        {
                            categoryCaches.Add(cate);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogController.WriteException(ex.Message + "\t" + ex.StackTrace);
            }

            return categoryCaches;
        }

        private static void SetClicksAndProductsCount(CategoryCache cate, int categoryId, Dictionary<int, int> categoryClicksDic, Dictionary<int, int> categoryProductsCountDic)
        {
            var subCates = DataController.GetNextLevelSubCategories(categoryId);
            foreach(var c in subCates)
            {
                if(categoryClicksDic.ContainsKey(c.CategoryID))
                {
                    cate.Clicks += categoryClicksDic[c.CategoryID];
                }
                if(categoryProductsCountDic.ContainsKey(c.CategoryID))
                {
                    cate.ProductsCount += categoryProductsCountDic[c.CategoryID];
                }
                SetClicksAndProductsCount(cate, c.CategoryID, categoryClicksDic, categoryProductsCountDic);
            }
        }
    }
}