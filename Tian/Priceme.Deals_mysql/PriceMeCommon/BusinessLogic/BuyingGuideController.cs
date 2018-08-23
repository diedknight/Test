using PriceMeCache;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceMeCommon.BusinessLogic
{
    public static class BuyingGuideController
    {
        static Dictionary<int, Dictionary<int, CategoryBugingGuideMap>> MultiCountryCategoryBGMapDic_Static;
        static Dictionary<int, Dictionary<int, List<int>>> MultiCountryCategoryBugingGuideMap_Static;
        static Dictionary<int, Dictionary<int, BuyingGuide>> MultiCountryBugingGuid_Static;
        static Dictionary<int, List<int>> RelatedBuyingGuidesDic_Static;

        public static void Load(Timer.DKTimer dkTimer)
        {
            MultiCountryCategoryBGMapDic_Static = GetMultiCountryCategoryBGMapDic();
            MultiCountryCategoryBugingGuideMap_Static = CreateMultiCountryCategoryBugingGuideMap(MultiCountryCategoryBGMapDic_Static);

            MultiCountryBugingGuid_Static = GetMultiCountryBugingGuid();
            RelatedBuyingGuidesDic_Static = GetRelatedBuyingGuidesDic();
        }

        private static Dictionary<int, List<int>> GetRelatedBuyingGuidesDic()
        {
            Dictionary<int, List<int>> dic = new Dictionary<int, List<int>>();

            string sql = "Select RelatedBG, BGID From CSK_Store_BuyingGuideRelated";
            using (SqlConnection conn = new SqlConnection(MultiCountryController.CommonConnectionStringSettings_Static.ConnectionString))
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.CommandTimeout = 0;
                conn.Open();
                using (var idr = cmd.ExecuteReader())
                {
                    while (idr.Read())
                    {
                        string related = idr.GetString(0);
                        int bgId = idr.GetInt32(1);

                        string[] relatedBGid = related.Split(',');
                        List<int> relatedBGidList = new List<int>();
                        foreach (string id in relatedBGid)
                        {
                            int bg = 0;
                            if (int.TryParse(id, out bg))
                            {
                                relatedBGidList.Add(bg);
                            }
                        }

                        dic.Add(bgId, relatedBGidList);
                    }
                }
            }

            return dic;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cId"></param>
        /// <param name="countryId"></param>
        /// <returns></returns>
        public static List<BuyingGuide> GetBuyingGuideCollectionByCategoryID(int cId, int countryId)
        {
            List<BuyingGuide> bgc = new List<BuyingGuide>();
            if (MultiCountryCategoryBugingGuideMap_Static.ContainsKey(countryId) && MultiCountryCategoryBugingGuideMap_Static[countryId].ContainsKey(cId))
            {
                List<int> BGIDList = MultiCountryCategoryBugingGuideMap_Static[countryId][cId];
                foreach (int BGID in BGIDList)
                {
                    BuyingGuide bg = GetBuyingGuideByBGID(BGID, countryId);
                    if (bg != null)
                    {
                        bgc.Add(bg);
                    }
                }
            }

            return bgc;
        }

        public static Dictionary<int, List<int>> GetCategoryBugingGuideMapDic(int countryId)
        {
            if(MultiCountryCategoryBugingGuideMap_Static.ContainsKey(countryId))
            {
                return MultiCountryCategoryBugingGuideMap_Static[countryId];
            }
            return null;
        }

        public static BuyingGuide GetBuyingGuideByBGID(int bgId, int countryId)
        {
            if (MultiCountryBugingGuid_Static.ContainsKey(countryId) && MultiCountryBugingGuid_Static[countryId].ContainsKey(bgId))
            {
                return MultiCountryBugingGuid_Static[countryId][bgId];
            }
            return null;
        }

        private static Dictionary<int, Dictionary<int, BuyingGuide>> GetMultiCountryBugingGuid()
        {
            Dictionary<int, Dictionary<int, BuyingGuide>> multiDic = new Dictionary<int, Dictionary<int, BuyingGuide>>();

            foreach (int countryId in MultiCountryController.CountryIdList)
            {
                Dictionary<int, BuyingGuide> dic = new Dictionary<int, BuyingGuide>();

                string sql = "SELECT * FROM [DBO].[CSK_Store_BuyingGuide] where WebSiteName = 'PriceMe' and BGID in (select BGID from CSK_Store_CategoryBugingGuide_Map where CountryID=" + countryId + ")";
                using (SqlConnection conn = new SqlConnection(MultiCountryController.CommonConnectionStringSettings_Static.ConnectionString))
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.CommandTimeout = 0;
                    conn.Open();
                    using (var idr = cmd.ExecuteReader())
                    {
                        while (idr.Read())
                        {
                            BuyingGuide bg = new BuyingGuide();
                            bg.BGID = int.Parse(idr["BGID"].ToString());
                            bg.BGTypeID = int.Parse(idr["BGTypeID"].ToString());
                            bg.BGName = idr["BGName"].ToString();
                            bg.WebSiteName = idr["WebSiteName"].ToString();
                            bg.LinkCreguired = idr["LinkCreguired"].ToString();
                            bg.ShortDescription = idr["ShortDescription"].ToString();
                            bg.FullContent = idr["FullContent"].ToString();
                            bg.CreatedBy = idr["CreatedBy"].ToString();
                            bg.ModifiedBy = idr["ModifiedBy"].ToString();

                            DateTime dt = DateTime.Now;
                            DateTime.TryParse(idr["CreatedOn"].ToString(), out dt);
                            bg.CreatedOn = dt;

                            DateTime.TryParse(idr["ModifiedOn"].ToString(), out dt);
                            bg.ModifiedOn = dt;

                            if (!dic.ContainsKey(bg.BGID))
                            {
                                dic.Add(bg.BGID, bg);
                            }
                        }
                    }

                    multiDic.Add(countryId, dic);
                }
            }

            return multiDic;
        }

        private static Dictionary<int, Dictionary<int, List<int>>> CreateMultiCountryCategoryBugingGuideMap(Dictionary<int, Dictionary<int, CategoryBugingGuideMap>> multiCountryCategoryBGMapDic)
        {
            Dictionary<int, Dictionary<int, List<int>>> mutilDic = new Dictionary<int, Dictionary<int, List<int>>>();

            foreach (int countryId in multiCountryCategoryBGMapDic.Keys)
            {
                List<CategoryBugingGuideMap> categoryBGMapList = multiCountryCategoryBGMapDic[countryId].Values.ToList();

                Dictionary<int, List<int>> categoryBugingGuideMapDictionary = new Dictionary<int, List<int>>();

                foreach (CategoryBugingGuideMap categoryBugingGuideMap in categoryBGMapList)
                {
                    if (categoryBugingGuideMapDictionary.ContainsKey(categoryBugingGuideMap.Categoryid))
                    {
                        categoryBugingGuideMapDictionary[categoryBugingGuideMap.Categoryid].Add(categoryBugingGuideMap.BGID);
                    }
                    else
                    {
                        List<int> categoryBugingGuideIDList = new List<int>();
                        categoryBugingGuideIDList.Add(categoryBugingGuideMap.BGID);
                        categoryBugingGuideMapDictionary.Add(categoryBugingGuideMap.Categoryid, categoryBugingGuideIDList);
                    }
                }

                mutilDic.Add(countryId, categoryBugingGuideMapDictionary);
            }

            return mutilDic;
        }

        private static Dictionary<int, Dictionary<int, CategoryBugingGuideMap>> GetMultiCountryCategoryBGMapDic()
        {
            Dictionary<int, Dictionary<int, CategoryBugingGuideMap>> mutilDic = new Dictionary<int, Dictionary<int, CategoryBugingGuideMap>>();
            foreach (int countryId in MultiCountryController.CountryIdList)
            {
                VelocityController vc = MultiCountryController.GetVelocityController(countryId);

                Dictionary<int, CategoryBugingGuideMap> categoryBGMapDic = null;
                if (vc != null)
                {
                    categoryBGMapDic = vc.GetCache<Dictionary<int, CategoryBugingGuideMap>>(Data.VelocityCacheKey.CategoryBGMapDic);
                }

                if(categoryBGMapDic == null || categoryBGMapDic.Count == 0)
                {
                    categoryBGMapDic = new Dictionary<int, CategoryBugingGuideMap>();
  
                    string sql = string.Format("SELECT * FROM [DBO].[CSK_Store_CategoryBugingGuide_Map] WHERE CountryID = {0} ORDER BY CategoryID", countryId);
                    using (SqlConnection conn = new SqlConnection(MultiCountryController.CommonConnectionStringSettings_Static.ConnectionString))
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.CommandTimeout = 0;
                        conn.Open();
                        using (var idr = cmd.ExecuteReader())
                        {
                            while (idr.Read())
                            {
                                CategoryBugingGuideMap map = new CategoryBugingGuideMap();
                                map.BGMapID = int.Parse(idr["BGMapID"].ToString());
                                map.BGID = int.Parse(idr["BGID"].ToString());
                                map.Categoryid = int.Parse(idr["Categoryid"].ToString());
                                map.CreatedBy = idr["CreatedBy"].ToString();
                                map.ModifiedBy = idr["ModifiedBy"].ToString();

                                DateTime dt = DateTime.Now;
                                DateTime.TryParse(idr["CreatedOn"].ToString(), out dt);
                                map.CreatedOn = dt;

                                DateTime.TryParse(idr["ModifiedOn"].ToString(), out dt);
                                map.ModifiedOn = dt;

                                if (!categoryBGMapDic.ContainsKey(map.BGID))
                                {
                                    categoryBGMapDic.Add(map.BGID, map);
                                }
                            }
                        }
                    }
                }

                mutilDic.Add(countryId, categoryBGMapDic);
            }

            return mutilDic;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bgId"></param>
        /// <param name="countryId"></param>
        /// <returns></returns>
        public static int GetCategoryIDBybgid(int bgId, int countryId)
        {
            if (MultiCountryCategoryBGMapDic_Static.ContainsKey(countryId))
            {
                if (MultiCountryCategoryBGMapDic_Static[countryId].ContainsKey(bgId))
                    return MultiCountryCategoryBGMapDic_Static[countryId][bgId].Categoryid;
            }
            return 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bgId"></param>
        /// <param name="countryId"></param>
        /// <returns></returns>
        public static List<BuyingGuide> GetRelatedBuyingGuidesbyID(int bgId, int countryId)
        {
            if (RelatedBuyingGuidesDic_Static.ContainsKey(bgId) && MultiCountryBugingGuid_Static.ContainsKey(countryId))
            {
                return MultiCountryBugingGuid_Static[countryId].Values.Where(r => RelatedBuyingGuidesDic_Static[bgId].Contains(r.BGID)).ToList();
            }

            return null;
        }
    }
}
