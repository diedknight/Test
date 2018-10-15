using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RelatedProductsTool
{
    public static class RelatedProductsController
    {
        static Dictionary<int, int> mIntraLinkingGenerationAndRelatedDic_Static;
        static Dictionary<int, Dictionary<int,int>> mMultiCountryProductClicksDic_Static;
        static string mSelectProductsSqlFormat_Static = @"SELECT PT.[ProductID]
                                                        ,Min([ManufacturerID]) as BrandId
                                                        ,Min([CategoryID]) as CategoryId
	                                                    ,Min(RPT.RetailerPrice) as BestPrice
	                                                    ,count(distinct PPCT.RetailerId) as PPCCount
                                                        FROM CSK_Store_Product as PT
                                                        inner join CSK_Store_RetailerProduct as RPT on PT.ProductID = RPT.ProductId
                                                        inner join CSK_Store_Retailer as RT on RT.RetailerId = RPT.RetailerId
                                                        left join CSK_Store_PPCMember as PPCT on PPCT.RetailerId = RPT.RetailerId and PPCT.PPCMemberTypeID = 2";

        //使用RelatedProductsController前一定要先执行
        public static void Init()
        {
            string pamuserConnStr = ConfigurationManager.ConnectionStrings["PriceMe_Pamuser"].ConnectionString;
            mIntraLinkingGenerationAndRelatedDic_Static = GetIntraLinkingGenerationAndRelated(pamuserConnStr);

            string priceme205ConnStr = ConfigurationManager.ConnectionStrings["PriceMe_205"].ConnectionString;
            mMultiCountryProductClicksDic_Static = GetMultiCountryProductClicksDic(priceme205ConnStr);
        }

        static RelatedProductsController()
        {
            
        }

        private static Dictionary<int, Dictionary<int, int>> GetMultiCountryProductClicksDic(string priceme205ConnStr)
        {
            Dictionary<int, Dictionary<int, int>> dic = new Dictionary<int, Dictionary<int, int>>();

            string selectSql = @"SELECT [ProductId]
                                  ,[Clicks]
                                  ,[CountryID]
                                FROM[dbo].[ProductClickTemp]";

            using (SqlConnection sqlConn = new SqlConnection(priceme205ConnStr))
            {
                sqlConn.Open();
                List<int> pidList = new List<int>();
                using (SqlCommand sqlCmd1 = new SqlCommand(selectSql, sqlConn))
                {
                    sqlCmd1.CommandTimeout = 0;
                    using (SqlDataReader sqlDr = sqlCmd1.ExecuteReader())
                    {
                        while (sqlDr.Read())
                        {
                            int pid = sqlDr.GetInt32(0);
                            int clicks = sqlDr.GetInt32(1);
                            int countryId = sqlDr.GetInt32(2);

                            if(dic.ContainsKey(countryId))
                            {
                                dic[countryId].Add(pid, clicks);
                            }
                            else
                            {
                                Dictionary<int, int> clicksDic = new Dictionary<int, int>();
                                clicksDic.Add(pid, clicks);
                                dic.Add(countryId, clicksDic);
                            }
                        }
                    }
                }
            }

            return dic;
        }
        internal static void WriteToDB(Dictionary<int, List<ProductRelatedScore>> cRelatedProductScoreDic, int countryId, string connStr)
        {
            List<ProductRelatedScore> list = new List<ProductRelatedScore>();
            foreach(int key in cRelatedProductScoreDic.Keys)
            {
                list.AddRange(cRelatedProductScoreDic[key]);
            }

            DateTime dtNow = DateTime.Now;
            string deleteSql = @"delete [dbo].[RelatedProductWithScore] where [CountryId] = " + countryId;
            string insertSql = @"INSERT INTO [dbo].[RelatedProductWithScore]
                               ([ProductId]
                               ,[CountryId]
                               ,[RelatedProductId]
                               ,[Score]
                               ,[CreatedOn]
                               ,[ModifiedOn]
                               ,[CategoryId])";
            foreach(var prs in list)
            {
                insertSql += prs.ToSqlString(dtNow, countryId) + " union all ";
            }

            if (insertSql.EndsWith(" union all "))
            {
                insertSql = insertSql.Substring(0, insertSql.Length - " union all ".Length);
            }

            string sql = "BEGIN TRANSACTION ";
            sql += @"
                    " + deleteSql + " ";
            sql += @"
                    IF @@ERROR>0 GOTO TRANS_ERR ";
            sql += @"
                    " + insertSql + " ";
            sql += @"
                    IF @@ERROR>0 GOTO TRANS_ERR ";
            sql += @"
                    COMMIT TRANSACTION 
                    Return
                    TRANS_ERR:
	                    ROLLBACK TRANSACTION";

            using (SqlConnection sqlConn = new SqlConnection(connStr))
            {
                sqlConn.Open();
                using (SqlCommand sqlCmd1 = new SqlCommand(sql, sqlConn))
                {
                    sqlCmd1.CommandTimeout = 0;
                    sqlCmd1.ExecuteNonQuery();
                }
            }
        }

        internal static void WriteToDBByProductId(List<ProductRelatedScore> cRelatedProductScoreList, int productId, int countryId, DateTime dtNow, string connStr)
        {
            string deleteSql = @"delete [dbo].[RelatedProductWithScore] where [CountryId] = " + countryId + " and [ProductId] = " + productId;
            string insertSql = @"INSERT INTO [dbo].[RelatedProductWithScore]
                               ([ProductId]
                               ,[CountryId]
                               ,[RelatedProductId]
                               ,[Score]
                               ,[CreatedOn]
                               ,[ModifiedOn]
                               ,[CategoryId])";
            foreach (var prs in cRelatedProductScoreList)
            {
                insertSql += prs.ToSqlString(dtNow, countryId) + " union all ";
            }

            if (insertSql.EndsWith(" union all "))
            {
                insertSql = insertSql.Substring(0, insertSql.Length - " union all ".Length);
            }

            string sql = "BEGIN TRANSACTION ";
            sql += @"
                    " + deleteSql + " ";
            sql += @"
                    IF @@ERROR>0 GOTO TRANS_ERR ";
            sql += @"
                    " + insertSql + " ";
            sql += @"
                    IF @@ERROR>0 GOTO TRANS_ERR ";
            sql += @"
                    COMMIT TRANSACTION 
                    Return
                    TRANS_ERR:
	                    ROLLBACK TRANSACTION";

            using (SqlConnection sqlConn = new SqlConnection(connStr))
            {
                sqlConn.Open();
                using (SqlCommand sqlCmd1 = new SqlCommand(sql, sqlConn))
                {
                    sqlCmd1.CommandTimeout = 0;
                    sqlCmd1.ExecuteNonQuery();
                }
            }
        }

        internal static void WriteToDBByProductId(List<ProductRelatedScore> cRelatedProductScoreList, List<int> productIds, int countryId, DateTime dtNow, string connStr)
        {
            string pIdsStr = string.Join(",", productIds);
            string deleteSql = @"delete [dbo].[RelatedProductWithScore] where [CountryId] = " + countryId + " and [ProductId] in (" + pIdsStr + ")";
            string insertSql = @"INSERT INTO [dbo].[RelatedProductWithScore]
                               ([ProductId]
                               ,[CountryId]
                               ,[RelatedProductId]
                               ,[Score]
                               ,[CreatedOn]
                               ,[ModifiedOn]
                               ,[CategoryId])";
            foreach (var prs in cRelatedProductScoreList)
            {
                insertSql += prs.ToSqlString(dtNow, countryId) + " union all ";
            }

            if (insertSql.EndsWith(" union all "))
            {
                insertSql = insertSql.Substring(0, insertSql.Length - " union all ".Length);
            }

            string sql = "BEGIN TRANSACTION ";
            sql += @"
                    " + deleteSql + " ";
            sql += @"
                    IF @@ERROR>0 GOTO TRANS_ERR ";
            sql += @"
                    " + insertSql + " ";
            sql += @"
                    IF @@ERROR>0 GOTO TRANS_ERR ";
            sql += @"
                    COMMIT TRANSACTION 
                    Return
                    TRANS_ERR:
	                    ROLLBACK TRANSACTION";

            using (SqlConnection sqlConn = new SqlConnection(connStr))
            {
                sqlConn.Open();
                using (SqlCommand sqlCmd1 = new SqlCommand(sql, sqlConn))
                {
                    sqlCmd1.CommandTimeout = 0;
                    sqlCmd1.ExecuteNonQuery();
                }
            }
        }

        public static List<ProductInfo> GetProductInfosByProductIds(List<int> pids, int countryId, string condition, string connStr)
        {
            List<ProductInfo> pList = new List<ProductInfo>();

            string pidsString = string.Join(",", pids);
            string selectSql = string.Format(mSelectProductsSqlFormat_Static + " where RT.RetailerCountry = " + countryId  + " and RPT." + condition + " = 1 group by PT.ProductID having PT.ProductID in ({0})", pidsString);

            using (SqlConnection sqlConn = new SqlConnection(connStr))
            {
                sqlConn.Open();
                List<int> pidList = new List<int>();
                using (SqlCommand sqlCmd1 = new SqlCommand(selectSql, sqlConn))
                {
                    sqlCmd1.CommandTimeout = 0;
                    using (SqlDataReader sqlDr = sqlCmd1.ExecuteReader())
                    {
                        while (sqlDr.Read())
                        {
                            ProductInfo pi = new ProductInfo();
                            pi.ProductId = sqlDr.GetInt32(0);
                            pi.BrandId = sqlDr.GetInt32(1);
                            pi.CategoryId = sqlDr.GetInt32(2);
                            pi.BestPrice = sqlDr.GetDecimal(3);
                            pi.PPCRetailerCount = sqlDr.GetInt32(4);
                            pList.Add(pi);
                        }
                    }
                }
            }

            return pList;
        }

        public static Dictionary<int, List<ProductInfo>> GetProductInfosByCategoryIds(List<int> cids, string connStr, int countryId, string condition)
        {
            Dictionary<int, List<ProductInfo>> dic = new Dictionary<int, List<ProductInfo>>();

            string selectSql = "";
            if (cids.Count == 1 && cids[0] == 0)
            {
                selectSql = string.Format(mSelectProductsSqlFormat_Static + " where RT.RetailerCountry = " + countryId + " and RPT." + condition + " = 1 group by PT.ProductID");
            }
            else
            {
                string cidsString = string.Join(",", cids);
                selectSql = string.Format(mSelectProductsSqlFormat_Static + " where RT.RetailerCountry = " + countryId + " and RPT." + condition + " = 1 and PT.CategoryId in ({0}) group by PT.ProductID", cidsString);
            }

            Dictionary<int, int> clicksDic;
            if(mMultiCountryProductClicksDic_Static.ContainsKey(countryId))
            {
                clicksDic = mMultiCountryProductClicksDic_Static[countryId];
            }
            else
            {
                clicksDic = new Dictionary<int, int>();
            }

            using (SqlConnection sqlConn = new SqlConnection(connStr))
            {
                sqlConn.Open();
                List<int> pidList = new List<int>();
                using (SqlCommand sqlCmd1 = new SqlCommand(selectSql, sqlConn))
                {
                    sqlCmd1.CommandTimeout = 0;
                    using (SqlDataReader sqlDr = sqlCmd1.ExecuteReader())
                    {
                        while (sqlDr.Read())
                        {
                            int cid = sqlDr.GetInt32(2);

                            ProductInfo pi = new ProductInfo();
                            pi.ProductId = sqlDr.GetInt32(0);
                            pi.BrandId = sqlDr.GetInt32(1);
                            pi.CategoryId = cid;
                            pi.BestPrice = sqlDr.GetDecimal(3);
                            pi.PPCRetailerCount = sqlDr.GetInt32(4);

                            if(clicksDic.ContainsKey(pi.ProductId))
                            {
                                pi.Clicks = clicksDic[pi.ProductId];
                            }

                            if (dic.ContainsKey(cid))
                            {
                                dic[cid].Add(pi);
                            }
                            else
                            {
                                List<ProductInfo> pList = new List<ProductInfo>();
                                pList.Add(pi);
                                dic.Add(cid, pList);
                            }
                        }
                    }
                }
            }

            return dic;
        }

        public static Dictionary<int, int> GetIntraLinkingGenerationAndRelated(string connStr)
        {
            Dictionary<int, int> dic = new Dictionary<int, int>();

            string selectSql = @"SELECT [ProductID]
                                ,[LinedPID]
                                FROM [dbo].[IntraLinkingGenerationAndRelated] where [ShowType] = 'Product'";

            
            using (SqlConnection sqlConn = new SqlConnection(connStr))
            {
                sqlConn.Open();
                List<int> pidList = new List<int>();
                using (SqlCommand sqlCmd1 = new SqlCommand(selectSql, sqlConn))
                {
                    sqlCmd1.CommandTimeout = 0;
                    using (SqlDataReader sqlDr = sqlCmd1.ExecuteReader())
                    {
                        while (sqlDr.Read())
                        {
                            dic.Add(sqlDr.GetInt32(0), sqlDr.GetInt32(1));
                        }
                    }
                }
            }

            return dic;
        }

        public static ProductRelatedScore GetRelatedScore(ProductInfo mainPI, ProductInfo pi)
        {
            ProductRelatedScore prs = new ProductRelatedScore();
            prs.MainProductId = mainPI.ProductId;
            prs.RelatedProductId = pi.ProductId;
            if((mIntraLinkingGenerationAndRelatedDic_Static.ContainsKey(mainPI.ProductId) && mIntraLinkingGenerationAndRelatedDic_Static[mainPI.ProductId] == pi.ProductId) ||
                (mIntraLinkingGenerationAndRelatedDic_Static.ContainsKey(pi.ProductId) && mIntraLinkingGenerationAndRelatedDic_Static[pi.ProductId] == mainPI.ProductId))
            {
                prs.SuccessorScore = 5;
            }
            if(mainPI.BrandId == pi.BrandId)
            {
                prs.BrandScore = 3;
            }
            if(pi.CategoryClickIndex < 3)
            {
                prs.BestSellerScore = 3;
            }
            float ppcScore = pi.PPCRetailerCount * 0.5f;
            if(ppcScore > 4)
            {
                ppcScore = 4f;
            }
            prs.PPCCountScore = ppcScore;

            decimal priceP = Math.Abs((mainPI.BestPrice - pi.BestPrice) / mainPI.BestPrice);
            if(priceP <= 0.05m)
            {
                prs.PriceScore = 3;
            }
            else if (priceP <= 0.1m)
            {
                prs.PriceScore = 2;
            }

            return prs;
        }
    }
}