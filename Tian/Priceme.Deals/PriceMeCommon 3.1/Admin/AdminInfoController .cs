using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PriceMeCommon.Admin
{
    public static class AdminInfoController
    {
        public static AdminInfo GetAdminInfo(string adminName)
        {
            string selectSql = "select * from CSK_Store_AdminInformation where adminname = '" + adminName + "'";
            string connString = System.Configuration.ConfigurationManager.ConnectionStrings["CommerceTemplate"].ConnectionString;
            using (System.Data.SqlClient.SqlConnection sqlConn = new System.Data.SqlClient.SqlConnection(connString))
            {
                using (System.Data.SqlClient.SqlCommand sqlCMD = new System.Data.SqlClient.SqlCommand(selectSql, sqlConn))
                {
                    sqlConn.Open();
                    using (System.Data.SqlClient.SqlDataReader sqlDR = sqlCMD.ExecuteReader())
                    {
                        if (sqlDR.Read())
                        {
                            int adminInfoID = int.Parse(sqlDR["AdminID"].ToString());
                            string adminInfoName = sqlDR["AdminName"].ToString();
                            string firstName = sqlDR["Firstname"].ToString();
                            string technicalEmailNZ = sqlDR["TechnicalEmailNZ"].ToString();
                            int helpMergeCounts = int.Parse(sqlDR["HelpMergeCounts"].ToString());

                            AdminInfo adminInfo = new AdminInfo();
                            adminInfo.AdminInfoID = adminInfoID;
                            adminInfo.AdminInfoName = adminInfoName;
                            adminInfo.FirstName = firstName;
                            adminInfo.TechnicalEmailNZ = technicalEmailNZ;
                            adminInfo.HelpMergeCounts = helpMergeCounts;
                            return adminInfo;
                        }
                    }
                }
            }

            return null;
        }

        public static List<int> GetAdminCategoryList(int adminInfoID)
        {
            List<int> cidList = new List<int>();
            string selectSql = "select * from CSK_Store_Category where AdminID = " + adminInfoID;
            string connString = System.Configuration.ConfigurationManager.ConnectionStrings["CommerceTemplate"].ConnectionString;
            using (System.Data.SqlClient.SqlConnection sqlConn = new System.Data.SqlClient.SqlConnection(connString))
            {
                using (System.Data.SqlClient.SqlCommand sqlCMD = new System.Data.SqlClient.SqlCommand(selectSql, sqlConn))
                {
                    sqlConn.Open();
                    using (System.Data.SqlClient.SqlDataReader sqlDR = sqlCMD.ExecuteReader())
                    {
                        while (sqlDR.Read())
                        {
                            int categoryID = int.Parse(sqlDR["CategoryID"].ToString());

                            cidList.Add(categoryID);
                        }
                    }
                }
            }

            return cidList;
        }

        public static Dictionary<int, int> GetAllTopCategoryList()
        {
            Dictionary<int, int> topCategoryDic = new Dictionary<int, int>();
            string selectSql = "SELECT [Id],[Categoryid],[Priority],[Frequency] FROM [dbo].[CSK_Store_HelpTopCategory]";
            string connString = System.Configuration.ConfigurationManager.ConnectionStrings["CommerceTemplate"].ConnectionString;
            using (System.Data.SqlClient.SqlConnection sqlConn = new System.Data.SqlClient.SqlConnection(connString))
            {
                using (System.Data.SqlClient.SqlCommand sqlCMD = new System.Data.SqlClient.SqlCommand(selectSql, sqlConn))
                {
                    sqlConn.Open();
                    using (System.Data.SqlClient.SqlDataReader sqlDR = sqlCMD.ExecuteReader())
                    {
                        while (sqlDR.Read())
                        {
                            int categoryID = int.Parse(sqlDR["Categoryid"].ToString());
                            int priority = int.Parse(sqlDR["Priority"].ToString());

                            topCategoryDic.Add(categoryID, priority);
                        }
                    }
                }
            }

            return topCategoryDic;
        }

        public static Dictionary<int, int> GetAdminTopCategoryList(int adminInfoID, List<int> adminCategoryList)
        {
            Dictionary<int, int> topCategoryDic = new Dictionary<int, int>();
            string selectSql = "SELECT [Id],[Categoryid],[Priority],[Frequency] FROM [dbo].[CSK_Store_HelpTopCategory] where Categoryid in (" + string.Join(",", adminCategoryList.ToArray()) + ")";
            string connString = System.Configuration.ConfigurationManager.ConnectionStrings["CommerceTemplate"].ConnectionString;
            using (System.Data.SqlClient.SqlConnection sqlConn = new System.Data.SqlClient.SqlConnection(connString))
            {
                using (System.Data.SqlClient.SqlCommand sqlCMD = new System.Data.SqlClient.SqlCommand(selectSql, sqlConn))
                {
                    sqlConn.Open();
                    using (System.Data.SqlClient.SqlDataReader sqlDR = sqlCMD.ExecuteReader())
                    {
                        while (sqlDR.Read())
                        {
                            int categoryID = int.Parse(sqlDR["Categoryid"].ToString());
                            int priority = int.Parse(sqlDR["Priority"].ToString());

                            topCategoryDic.Add(categoryID, priority);
                        }
                    }
                }
            }

            return topCategoryDic;
        }

        public static Dictionary<int, int> GetTopRetailerList()
        {
            Dictionary<int, int> topRetailerDic = new Dictionary<int, int>();
            string selectSql = "SELECT [Id],[Retailerid],[Priority],[Frequency] FROM [dbo].[CSK_Store_HelpTopRetailer]";
            string connString = System.Configuration.ConfigurationManager.ConnectionStrings["CommerceTemplate"].ConnectionString;
            using (System.Data.SqlClient.SqlConnection sqlConn = new System.Data.SqlClient.SqlConnection(connString))
            {
                using (System.Data.SqlClient.SqlCommand sqlCMD = new System.Data.SqlClient.SqlCommand(selectSql, sqlConn))
                {
                    sqlConn.Open();
                    using (System.Data.SqlClient.SqlDataReader sqlDR = sqlCMD.ExecuteReader())
                    {
                        while (sqlDR.Read())
                        {
                            int retailerId = int.Parse(sqlDR["Retailerid"].ToString());
                            int priority = int.Parse(sqlDR["Priority"].ToString());

                            topRetailerDic.Add(retailerId, priority);
                        }
                    }
                }
            }

            return topRetailerDic;
        }

        public static int GetAllNotSupportCategoriesCountByAdmin(string adminName)
        {
            int count = 0;
            string selectSql = "GetAllNotSupportCategoriesByAdmin";
            string connString = System.Configuration.ConfigurationManager.ConnectionStrings["CommerceTemplate"].ConnectionString;
            using (System.Data.SqlClient.SqlConnection sqlConn = new System.Data.SqlClient.SqlConnection(connString))
            {
                using (System.Data.SqlClient.SqlCommand sqlCMD = new System.Data.SqlClient.SqlCommand(selectSql, sqlConn))
                {
                    sqlCMD.CommandType = System.Data.CommandType.StoredProcedure;
                    System.Data.SqlClient.SqlParameter adminNameP = new System.Data.SqlClient.SqlParameter();
                    adminNameP.DbType = System.Data.DbType.String;
                    adminNameP.Value = adminName;
                    adminNameP.ParameterName = "@admin";
                    sqlCMD.Parameters.Add(adminNameP);

                    sqlConn.Open();
                    using (System.Data.SqlClient.SqlDataReader sqlDR = sqlCMD.ExecuteReader())
                    {
                        while (sqlDR.Read())
                        {
                            count++;
                        }
                    }
                }
            }
            return count; 
        }
        
    }
}