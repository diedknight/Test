using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PriceMe
{
    public static class SpaceIDsController
    {
        static string ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString;
        static Dictionary<int, int> spaceIDsMap = new Dictionary<int, int>();

        public static void Init()
        {
            Dictionary<int, int> _spaceIDsMap = new Dictionary<int, int>();
            if (PriceMeCommon.ConfigAppString.CountryID != 3)
                return;

            using (System.Data.SqlClient.SqlConnection sqlConn = new System.Data.SqlClient.SqlConnection(ConnectionString))
            {
                sqlConn.Open();
                using (System.Data.SqlClient.SqlCommand sqlCmd = new System.Data.SqlClient.SqlCommand())
                {
                    sqlCmd.CommandText = "SELECT * FROM [CSK_Store_YPricemeSpaceIDs]";
                    sqlCmd.CommandType = System.Data.CommandType.Text;
                    sqlCmd.CommandTimeout = 0;
                    sqlCmd.Connection = sqlConn;

                    System.Data.SqlClient.SqlDataReader sdr = sqlCmd.ExecuteReader();
                    while (sdr.Read())
                    {
                        string pid = sdr["PageID"].ToString();
                        if (string.IsNullOrEmpty(pid))
                            continue;
                        int pageID = int.Parse(pid);
                        int spaceID = int.Parse(sdr["YahooSpaceID"].ToString());
                        _spaceIDsMap.Add(pageID, spaceID);
                    }
                    spaceIDsMap = _spaceIDsMap;
                }
            }
        }

        public static int GetSpaceIDByCategoryID(int categoryID)
        {
            if (spaceIDsMap.Count == 0 || PriceMeCommon.ConfigAppString.CountryID != 3)
                return 0;

            if (spaceIDsMap.ContainsKey(categoryID))
            {
                return spaceIDsMap[categoryID];
            }
            else
            {
                var c = PriceMeCommon.CategoryController.GetCategoryByCategoryID(categoryID);
                if (c != null)
                {
                    int parentCategoryID = c.ParentID;
                    while (!spaceIDsMap.ContainsKey(parentCategoryID))
                    {
                        parentCategoryID = PriceMeCommon.CategoryController.GetCategoryByCategoryID(parentCategoryID).ParentID;
                    }
                    return spaceIDsMap[parentCategoryID];
                }
                else
                {
                    if (spaceIDsMap.ContainsKey(0))
                    {
                        return spaceIDsMap[0];
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
        }

        public static int GetSpaceIDByPageID(int pageID)
        {
            if (spaceIDsMap.ContainsKey(pageID))
            {
                return spaceIDsMap[pageID];
            }
            else
            {
                if (spaceIDsMap.ContainsKey(0))
                {
                    return spaceIDsMap[0];
                }
                else
                {
                    return 0;
                }
            }
        }
    }
}