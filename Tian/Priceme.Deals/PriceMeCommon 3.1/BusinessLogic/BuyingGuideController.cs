using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PriceMeDBA;
using SubSonic.Schema;
using System.Data;
using System.Data.SqlClient;
using PriceMeCommon.Data;

namespace PriceMeCommon
{
    public static class BuyingGuideController
    {
        static Dictionary<int, CSK_Store_BuyingGuide> bgDictionary;
        static Dictionary<int, List<int>> categoryBugingGuideMap;
        static Dictionary<int, List<CSK_Store_BuyingGuide>> relatedugingGuideDic;
        static List<CategoryBugingGuideMap> categoryBGMapList;
        public static List<CategoryBugingGuideMap> CategoryBGMapList
        {
            get { return categoryBGMapList; }
        }

        public static Dictionary<int, List<int>> CategoryBugingGuideMap
        {
            get { return categoryBugingGuideMap; }
        }

        public static Dictionary<int, List<CSK_Store_BuyingGuide>> RelatedugingGuideDic
        {
            get { return relatedugingGuideDic; }
        }

        public static void LoadBuyingGuideAndMaps()
        {
            categoryBGMapList = VelocityController.GetCache<List<CategoryBugingGuideMap>>(PriceMeCommon.Data.VelocityCacheKey.CategoryBGMapList);
            categoryBugingGuideMap = VelocityController.GetCache<Dictionary<int, List<int>>>(PriceMeCommon.Data.VelocityCacheKey.CategoryBugingGuideMap);

            if (categoryBGMapList == null || categoryBGMapList.Count == 0 || categoryBugingGuideMap == null || categoryBugingGuideMap.Count == 0)
            {
                LogWriter.WriteLineToFile(ConfigAppString.LogPath, "categoryBugingGuideMap no Velocity.");
                categoryBugingGuideMap = GetCategoryBugingGuideMapDictionaryCache();
            }

            LoadBuyingGuides();
        }

        public static void LoadBuyingGuides()
        {
            bgDictionary = new Dictionary<int, CSK_Store_BuyingGuide>();

            #region CSK_Store_BuyingGuide
            List<CSK_Store_BuyingGuide> buyGuideCollection = new List<CSK_Store_BuyingGuide>();
            try
            {
                string sql = "SELECT * FROM [DBO].[CSK_Store_BuyingGuide] where WebSiteName = 'PriceMe' and BGID in (select BGID from CSK_Store_CategoryBugingGuide_Map where CountryID="+ConfigAppString.CountryID+")";
                using (SqlConnection conn = new SqlConnection(ConfigAppString.CommerceTemplateCommon))
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandTimeout = 0;
                    conn.Open();
                    IDataReader idr = cmd.ExecuteReader();
                    while (idr.Read())
                    {
                        CSK_Store_BuyingGuide bg = new CSK_Store_BuyingGuide();
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

                        if (!bgDictionary.ContainsKey(bg.BGID))
                        {
                            bgDictionary.Add(bg.BGID, bg);
                        }
                    }
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                LogWriter.WriteLineToFile(PriceMeCommon.ConfigAppString.ExceptionLogPath, ex.Message + "\t" + ex.StackTrace);
            }
            #endregion
        }

        public static List<CSK_Store_BuyingGuide> GetRelatedBuyingGuidesbyID(int bgid)
        {
            #region CSK_Store_BuyingGuideRelated

            try
            {
                #region CSK_Store_BuyingGuideRelated

                string related = string.Empty;

                List<CSK_Store_BuyingGuide> buyGuideCollection = new List<CSK_Store_BuyingGuide>();
                try
                {
                    string sql = "Select RelatedBG From CSK_Store_BuyingGuideRelated where BGID = " + bgid;
                    using (SqlConnection conn = new SqlConnection(ConfigAppString.CommerceTemplateCommon))
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandTimeout = 0;
                        conn.Open();
                        IDataReader idr = cmd.ExecuteReader();
                        while (idr.Read())
                        {
                            related = idr["RelatedBG"].ToString();
                        }
                        conn.Close();
                    }
                }
                catch (Exception ex)
                {
                    LogWriter.WriteLineToFile(PriceMeCommon.ConfigAppString.ExceptionLogPath, ex.Message + "\t" + ex.StackTrace);
                }
                #endregion

                string[] relatedBGid = related.Split(',');
                List<int> relatedBGidList = new List<int>();
                foreach (string id in relatedBGid)
                {
                    int bg = 0;
                    int.TryParse(id, out bg);
                    if (bgid == 0) continue;
                    relatedBGidList.Add(bg);
                }
                return bgDictionary.Values.Where(r => relatedBGidList.Contains(r.BGID)).ToList();
            }
            catch (Exception ex)
            {
                LogWriter.WriteLineToFile(PriceMeCommon.ConfigAppString.ExceptionLogPath, ex.Message + "\t" + ex.StackTrace);
                return null;
            }
            #endregion            
        }

        public static Dictionary<int, List<int>> GetCategoryBugingGuideMapDictionaryCache()
        {
            #region CSK_Store_CategoryBugingGuide_Map

            categoryBGMapList = new List<CategoryBugingGuideMap>();
            try
            {
                string sql = string.Format("SELECT * FROM [DBO].[CSK_Store_CategoryBugingGuide_Map] WHERE CountryID = {0} ORDER BY CategoryID" 
                    , ConfigAppString.CountryID);
                using (SqlConnection conn = new SqlConnection(ConfigAppString.CommerceTemplateCommon))
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandTimeout = 0;
                    conn.Open();
                    IDataReader idr = cmd.ExecuteReader();
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

                        categoryBGMapList.Add(map);
                    }
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                LogWriter.WriteLineToFile(PriceMeCommon.ConfigAppString.ExceptionLogPath, ex.Message + "\t" + ex.StackTrace);
            }
            #endregion

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

            return categoryBugingGuideMapDictionary;
        }

        public static List<CSK_Store_BuyingGuide> GetBuyingGuideCollectionByCategoryID(int cid)
        {
            List<CSK_Store_BuyingGuide> bgc = new List<CSK_Store_BuyingGuide>();
            if (categoryBugingGuideMap != null && categoryBugingGuideMap.ContainsKey(cid))
            {
                List<int> BGIDList = categoryBugingGuideMap[cid];
                foreach (int BGID in BGIDList)
                {
                    CSK_Store_BuyingGuide bg = GetBuyingGuideByBGID(BGID);
                    if (bg != null)
                    {
                        bgc.Add(bg);
                    }
                }
            }
            return bgc;
        }

        public static CSK_Store_BuyingGuide GetBuyingGuideByBGID(int bgid)
        {
            if (bgDictionary.ContainsKey(bgid))
            {
                return bgDictionary[bgid];
            }
            return null;
        }

        public static int GetCategoryIDBybgid(int bgid)
        {
            List<CategoryBugingGuideMap> maps = categoryBGMapList.Where(c => c.BGID == bgid).ToList();
            if (maps != null && maps.Count > 0)
                return maps[0].Categoryid;
            return 0;
        }
    }
}