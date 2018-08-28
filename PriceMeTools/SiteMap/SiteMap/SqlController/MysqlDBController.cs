using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace SiteMap.SqlController
{
    public class MysqlDBController
    {
        public static void LoadParentCategory(out Dictionary<int, string> categorys)
        {
            categorys = new Dictionary<int, string>();

            string sql = "select CategoryID,CategoryName from CSK_Store_Category where ParentID = 0";
            using (MySqlConnection conn = new MySqlConnection(SiteConfig.ConnectionStrings("conn_NZ")))
            {
                conn.Open();
                using (MySqlCommand comm = new MySqlCommand(sql, conn))
                {
                    using (IDataReader idr = comm.ExecuteReader())
                    {
                        while (idr.Read())
                        {
                            int cid = int.Parse(idr["CategoryId"].ToString());
                            string cname = idr["CategoryName"].ToString();
                            if (!categorys.ContainsKey(cid))
                                categorys.Add(cid, cname);
                        }
                    }
                }
            }
        }

        public static int GetParentCategory(int cid)
        {
            //3210
            int parentid = 0;

            string sql = "select ParentID From CSK_Store_Category where CategoryID = " + cid;
            using (MySqlConnection conn = new MySqlConnection(SiteConfig.ConnectionStrings("conn_NZ")))
            {
                conn.Open();
                using (MySqlCommand comm = new MySqlCommand(sql, conn))
                {
                    using (IDataReader idr = comm.ExecuteReader())
                    {
                        while (idr.Read())
                        {
                            int.TryParse(idr["ParentID"].ToString(), out parentid);
                        }
                    }
                }
            }

            if (parentid != 0)
                parentid = GetParentCategory(parentid);
            else
                parentid = cid;

            return parentid;
        }

        public static void LoadParentCategory(out Dictionary<int, string> categorys, Dictionary<int, string> subCategoryDic)
        {
            categorys = new Dictionary<int, string>();

            string sql = "select CategoryID,CategoryName from CSK_Store_Category where CategoryID in ( "
                     + "select ParentID from CSK_Store_Category where categoryid in (select categoryid from CSK_Store_ProductNew)) and ParentID = 0";
            using (MySqlConnection conn = new MySqlConnection(SiteConfig.ConnectionStrings("conn_NZ")))
            {
                conn.Open();
                using (MySqlCommand comm = new MySqlCommand(sql, conn))
                {
                    using (IDataReader idr = comm.ExecuteReader())
                    {
                        while (idr.Read())
                        {
                            int cid = int.Parse(idr["CategoryId"].ToString());
                            string cname = idr["CategoryName"].ToString();
                            if (!subCategoryDic.ContainsKey(cid))
                                categorys.Add(cid, cname);
                        }
                    }
                }
            }
        }

        public static void LoadSubCategory(out Dictionary<int, string> subCategoryDic)
        {
            subCategoryDic = new Dictionary<int, string>();

            string sql = "select * from csk_store_category where isSearchOnly = 0 and categoryId not in (select parentId from csk_store_category) and CategoryID in (select CategoryID from CSK_Store_ProductNew)";
            using (MySqlConnection conn = new MySqlConnection(SiteConfig.ConnectionStrings("conn_NZ")))
            {
                conn.Open();
                using (MySqlCommand comm = new MySqlCommand(sql, conn))
                {
                    using (IDataReader idr = comm.ExecuteReader())
                    {
                        while (idr.Read())
                        {
                            int cid = int.Parse(idr["CategoryId"].ToString());
                            string cname = idr["CategoryName"].ToString();
                            if (!subCategoryDic.ContainsKey(cid))
                                subCategoryDic.Add(cid, cname);
                        }
                    }
                }
            }
        }

        public static List<string> LoadVariants()
        {
            List<string>  VariantProductIds = new List<string>();

            string sql = "select LinedPID from IntraLinkingGenerationAndRelated where LinkType = 'Variant'";
            using (MySqlConnection conn = new MySqlConnection(SiteConfig.ConnectionStrings("conn_Common")))
            {
                conn.Open();
                using (MySqlCommand comm = new MySqlCommand(sql, conn))
                {
                    using (IDataReader idr = comm.ExecuteReader())
                    {
                        while (idr.Read())
                        {
                            VariantProductIds.Add(idr.GetInt32(0).ToString());
                        }
                    }
                }
            }

            return VariantProductIds;
        }

        public static List<int> LoadIsSearchOnly()
        {
            List<int> isSearchOnlyList = new List<int>();

            string sql = "select CategoryID from CSK_Store_Category where isSearchOnly = 1";
            using (MySqlConnection conn = new MySqlConnection(SiteConfig.ConnectionStrings("conn_Common")))
            {
                conn.Open();
                using (MySqlCommand comm = new MySqlCommand(sql, conn))
                {
                    using (IDataReader idr = comm.ExecuteReader())
                    {
                        while (idr.Read())
                        {
                            isSearchOnlyList.Add(idr.GetInt32(0));
                        }
                    }
                }
            }

            return isSearchOnlyList;
        }
    }
}
