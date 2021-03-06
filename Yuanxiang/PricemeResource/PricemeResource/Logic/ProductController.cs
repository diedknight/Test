﻿using PricemeResource.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PricemeResource.Logic
{
    public static class ProductController
    {
        public static ProductNewData GetProductNew(int productId, DbInfo dbInfo)
        {
            ProductNewData pro = new ProductNewData();
            var sql = string.Format("SELECT ProductId, ProductName FROM CSK_Store_ProductNew where ProductID = " + productId);
            using (var sqlConn = DBController.CreateDBConnection(dbInfo))
            {
                using (var sqlCMD = DBController.CreateDbCommand(sql, sqlConn))
                {
                    sqlConn.Open();
                    using (var sqlDR = sqlCMD.ExecuteReader())
                    {
                        while (sqlDR.Read())
                        {
                            pro = DbConvertController<ProductNewData>.ReadDataFromDataReader(sqlDR);
                        }
                    }
                }
            }

            return pro;
        }

        public static ProductNewData GetRealProductSimplified(int productId, DbInfo dbInfo)
        {
            ProductNewData product = GetProductNewSimplified(productId, dbInfo);

            if (product == null || product.ProductID == 0)
            {
                while (true)
                {
                    ProductIsMergedData pm = GetProductIdInProductIsMergedByProductId(productId, dbInfo);
                    if (pm != null)
                    {
                        productId = pm.ToProductID;
                        product = GetProductNewSimplified(pm.ToProductID, dbInfo);
                        if (product == null || product.ProductID == 0)
                            product = GetProductHistorySimplified(pm.ToProductID, dbInfo);

                        if (product != null)
                        {
                            return product;
                        }
                    }
                    else
                    {
                        product = GetProductHistorySimplified(productId, dbInfo);
                        return product;
                    }
                }
            }

            return product;
        }

        private static ProductNewData GetProductNewSimplified(int productId, DbInfo dbInfo)
        {
            ProductNewData pro = new ProductNewData();

            var sql = string.Format("SELECT p.ProductId, p.ProductName, rp.RetailerPrice as BestPrice FROM CSK_Store_ProductNew p inner join CSK_Store_RetailerProductNew rp On p.ProductID = rp.ProductId where p.ProductID = " + productId + " order by rp.RetailerPrice limit 1");
            using (var sqlConn = DBController.CreateDBConnection(dbInfo))
            {
                using (var sqlCMD = DBController.CreateDbCommand(sql, sqlConn))
                {
                    sqlConn.Open();
                    using (var sqlDR = sqlCMD.ExecuteReader())
                    {
                        while (sqlDR.Read())
                        {
                            pro = DbConvertController<ProductNewData>.ReadDataFromDataReader(sqlDR);
                        }
                    }
                }
            }

            return pro;
        }

        private static ProductNewData GetProductHistorySimplified(int productId, DbInfo dbInfo)
        {
            ProductNewData pro = new ProductNewData();
            var sql = string.Format("SELECT ProductId, ProductName FROM CSK_Store_Product where ProductID = " + productId);
            using (var sqlConn = DBController.CreateDBConnection(dbInfo))
            {
                using (var sqlCMD = DBController.CreateDbCommand(sql, sqlConn))
                {
                    sqlConn.Open();
                    using (var sqlDR = sqlCMD.ExecuteReader())
                    {
                        while (sqlDR.Read())
                        {
                            pro = DbConvertController<ProductNewData>.ReadDataFromDataReader(sqlDR);
                        }
                    }
                }
            }

            return pro;
        }

        public static List<ProductNewData> GetRealProductSimplified(List<int> pids, DbInfo dbInfo)
        {
            List<ProductNewData> products = new List<ProductNewData>();

            string sql = string.Empty;
            foreach (int pid in pids)
            {
                sql += string.Format("SELECT p.ProductId, p.ProductName, rp.RetailerPrice as BestPrice FROM CSK_Store_ProductNew p inner join CSK_Store_RetailerProductNew rp On p.ProductID = rp.ProductId where p.ProductID = " + pid + " order by rp.RetailerPrice limit 1;");
            }

            using (var sqlConn = DBController.CreateDBConnection(dbInfo))
            {
                using (var sqlCMD = DBController.CreateDbCommand(sql, sqlConn))
                {
                    sqlConn.Open();
                    using (var sqlDR = sqlCMD.ExecuteReader())
                    {
                        while (sqlDR.Read())
                        {
                            ProductNewData pro = DbConvertController<ProductNewData>.ReadDataFromDataReader(sqlDR);
                            products.Add(pro);
                        }
                        sqlDR.NextResult();
                        while (sqlDR.Read())
                        {
                            ProductNewData pro = DbConvertController<ProductNewData>.ReadDataFromDataReader(sqlDR);
                            products.Add(pro);
                        }
                        sqlDR.NextResult();
                        while (sqlDR.Read())
                        {
                            ProductNewData pro = DbConvertController<ProductNewData>.ReadDataFromDataReader(sqlDR);
                            products.Add(pro);
                        }
                    }
                }
            }

            return products;
        }
        
        public static List<PriceHistory> GetPriceHistory(int productId, DbInfo dbInfo)
        {
            List<PriceHistory> phs = new List<PriceHistory>();
            var sql = string.Format("SELECT * FROM CSK_Store_PriceHistory where ProductID = " + productId + " order by CreatedOn");
            using (var sqlConn = DBController.CreateDBConnection(dbInfo))
            {
                using (var sqlCMD = DBController.CreateDbCommand(sql, sqlConn))
                {
                    sqlConn.Open();
                    using (var sqlDR = sqlCMD.ExecuteReader())
                    {
                        while (sqlDR.Read())
                        {
                            PriceHistory ph = DbConvertController<PriceHistory>.ReadDataFromDataReader(sqlDR);
                            phs.Add(ph);
                        }
                    }
                }
            }

            return phs;
        }

        public static List<ProductRetailerCountHistory> ProductRetailerCountHistory(int pid, DateTime createOn, DbInfo dbInfo)
        {
            List<ProductRetailerCountHistory> pls = new List<ProductRetailerCountHistory>();

            var sql = "Select * From CSK_Store_ProductRetailerCountHistory Where ProductID = " + pid + " And CreatedOn > '" + createOn.ToString("yyyy-MM-dd HH:mm:ss") + "' order by CreatedOn";
            using (var sqlConn = DBController.CreateDBConnection(dbInfo))
            {
                using (var sqlCMD = DBController.CreateDbCommand(sql, sqlConn))
                {
                    sqlConn.Open();
                    using (var sqlDR = sqlCMD.ExecuteReader())
                    {
                        while (sqlDR.Read())
                        {
                            ProductRetailerCountHistory pl = DbConvertController<ProductRetailerCountHistory>.ReadDataFromDataReader(sqlDR);
                            pls.Add(pl);
                        }
                    }
                }
            }

            return pls;
        }

        public static ProductIsMergedData GetProductIdInProductIsMergedByProductId(int pid, DbInfo dbInfo)
        {
            ProductIsMergedData pim = new ProductIsMergedData();

            string sql = "Select * From CSK_Store_ProductIsMerged where ProductID = " + pid;
            using (var sqlConn = DBController.CreateDBConnection(dbInfo))
            {
                using (var sqlCMD = DBController.CreateDbCommand(sql, sqlConn))
                {
                    sqlConn.Open();
                    using (var sqlDR = sqlCMD.ExecuteReader())
                    {
                        while (sqlDR.Read())
                        {
                            pim = DbConvertController<ProductIsMergedData>.ReadDataFromDataReader(sqlDR);
                        }
                    }
                }
            }

            return pim;
        }

        public static List<RetailerProductItem> GetRetailerProductItems(int productId, DbInfo dbInfo)
        {
            List<RetailerProductItem> rpis = new List<RetailerProductItem>();

            List<RetailerProductNew> rps = new List<RetailerProductNew>();

            var sql = "SELECT RP.RetailerId,RP.RetailerPrice,RP.PurchaseURL,RP.RetailerProductId,"
                    + "RP.RetailerProductName,RP.RetailerProductDescription,RP.PriceLocalCurrency,"
                    + "RP.DefaultImage FROM CSK_Store_RetailerProductNew RP WHERE RP.ProductId = " + productId;

            using (var sqlConn = DBController.CreateDBConnection(dbInfo))
            {
                using (var sqlCMD = DBController.CreateDbCommand(sql, sqlConn))
                {
                    sqlConn.Open();
                    using (var sqlDR = sqlCMD.ExecuteReader())
                    {
                        while (sqlDR.Read())
                        {
                            RetailerProductNew rp = DbConvertController<RetailerProductNew>.ReadDataFromDataReader(sqlDR);
                            rps.Add(rp);
                        }
                    }
                }
            }
            
            Dictionary<int, List<RetailerProductNew>> rpDic = new Dictionary<int, List<RetailerProductNew>>();

            foreach (RetailerProductNew rp in rps)
            {
                List<RetailerProductNew> temps = new List<RetailerProductNew>();

                if (rpDic.ContainsKey(rp.RetailerId))
                {
                    temps = rpDic[rp.RetailerId];
                    temps.Add(rp);
                    rpDic[rp.RetailerId] = temps;
                }
                else
                {
                    temps.Add(rp);
                    rpDic.Add(rp.RetailerId, temps);
                }
            }

            foreach (KeyValuePair<int, List<RetailerProductNew>> pair in rpDic)
            {
                RetailerProductItem rpi = new RetailerProductItem();
                rpi.RetailerId = pair.Key;
                rpi.RpList = pair.Value;

                rpis.Add(rpi);
            }

            return rpis;
        }

        public static List<PriceHistory> GetPriceHistoryData(List<int> listpId, DbInfo dbInfo)
        {
            List<PriceHistory> phs = new List<PriceHistory>();
            string pids = string.Empty;
            foreach (int pid in listpId)
            {
                pids += pid + ",";
            }
            pids = pids.Substring(0, pids.LastIndexOf(','));

            var sql = "select ProductID, PriceDate, Price, CreatedOn from CSK_Store_PriceHistory where productid in (" + pids + ")";
            using (var sqlConn = DBController.CreateDBConnection(dbInfo))
            {
                using (var sqlCMD = DBController.CreateDbCommand(sql, sqlConn))
                {
                    sqlConn.Open();
                    using (var sqlDR = sqlCMD.ExecuteReader())
                    {
                        while (sqlDR.Read())
                        {
                            int pid = 0;
                            decimal Price = 0;
                            DateTime PriceDate = DateTime.Now;
                            DateTime CreatedOn = DateTime.Now;
                            int.TryParse(sqlDR["ProductID"].ToString(), out pid);
                            decimal.TryParse(sqlDR["Price"].ToString(), out Price);
                            DateTime.TryParse(sqlDR["PriceDate"].ToString(), out PriceDate);
                            DateTime.TryParse(sqlDR["CreatedOn"].ToString(), out CreatedOn);

                            PriceHistory ph = new PriceHistory();
                            ph.ProductID = pid;
                            ph.Price = Price;
                            ph.PriceDate = PriceDate;
                            ph.CreatedOn = CreatedOn;
                            phs.Add(ph);
                        }
                    }
                }
            }

            return phs;
        }

        public static List<ExpertReview> GetExpertReviewResult(List<int> listpId, DbInfo dbInfo)
        {
            List<ExpertReview> ers = new List<ExpertReview>();
            string pids = string.Empty;
            foreach (int pid in listpId)
            {
                pids += pid + ",";
            }
            pids = pids.Substring(0, pids.LastIndexOf(','));

            var sql = "Select ProductID, PriceMeScore, SourceID from CSK_Store_ExpertReviewAU Where productId in (" + pids + ")";
            using (var sqlConn = DBController.CreateDBConnection(dbInfo))
            {
                using (var sqlCMD = DBController.CreateDbCommand(sql, sqlConn))
                {
                    sqlConn.Open();
                    using (var sqlDR = sqlCMD.ExecuteReader())
                    {
                        while (sqlDR.Read())
                        {
                            int pid = 0, sourceid = 0;
                            decimal PriceMeScore = 0;
                            int.TryParse(sqlDR["ProductID"].ToString(), out pid);
                            int.TryParse(sqlDR["SourceID"].ToString(), out sourceid);
                            decimal.TryParse(sqlDR["PriceMeScore"].ToString(), out PriceMeScore);

                            ExpertReview er = new ExpertReview();
                            er.ProductId = pid;
                            er.SourceID = sourceid;
                            er.PriceMeScore = PriceMeScore;
                            ers.Add(er);
                        }
                    }
                }
            }

            return ers;
        }
    }
}
