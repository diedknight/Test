using System;
using System.Data;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PriceMeCommon.BusinessLogic;
using PriceMeDBA;
using SubSonic;
using SubSonic.Schema;
using SubSonic.DataProviders;
using Lucene.Net.Search;
using Lucene.Net.Index;
using PriceMeCommon.Data;
using Lucene.Net.Documents;

namespace PriceMeCommon
{
    /// <summary>
    /// Summary description for RetailerProductController
    /// </summary>
    public class RetailerProductController
    {
        //SubSonic Columns
        private static readonly string[] rpCol = new string[] { "RetailerId", "ProductId", "RetailerProductStatus", "RetailerProductId", "RetailerProductName", "DefaultImage", "RetailerPrice", "Freight", "Stock", "RetailerProductDescription", "RetailerProductUIC", "RetailerProductSKU", "StockStatus" };

        private static PriceMeDBDB db = PriceMeStatic.PriceMeDB;

        public static List<CSK_Store_RetailerProduct> GetRetailerProductByRetailerIDProductID(int retailerID, int productID)
        {
            return CSK_Store_RetailerProduct.Find(rp => rp.RetailerId == retailerID && rp.ProductId == productID).ToList();
        }

        //public static CSK_Store_RetailerProduct GetByProductID(int productID)
        //{
        //    StoredProcedure sp = db.CSK_Store_12RMB_GetBestPriceRetailerProduct();
        //    sp.Command.AddParameter("@productID", productID, DbType.Int32);

        //    IDataReader idr = sp.ExecuteReader();

        //    if (idr.Read())
        //    {
        //        CSK_Store_RetailerProduct retailerproduct = CSK_Store_RetailerProduct.SingleOrDefault(rp => rp.RetailerProductId == int.Parse(idr[0].ToString()));

        //        idr.Close();
        //        return retailerproduct;
        //    }
        //    idr.Close();

        //    return null;
        //}

        #region ruangang
        public static List<CSK_Store_RetailerProductNew> GetRetailerProductsByProductId(int productId)
        {
            StoredProcedure sp = db.CSK_Store_GetRetailerProductsByProductID();
            sp.Command.AddParameter("@ProductID", productId, DbType.Int32);
            return sp.ExecuteTypedList<CSK_Store_RetailerProductNew>();
        }

        public static CSK_Store_RetailerProduct GetRetailerProduct(int rpid)
        {
            CSK_Store_RetailerProduct rpn = (new SubSonic.Query.Select(rpCol).From("CSK_Store_RetailerProduct").Where("RetailerProductId").In(rpid)).ExecuteSingle<CSK_Store_RetailerProduct>();
            return rpn;
        }

        public static CSK_Store_RetailerProductNew GetRetailerProductNew(int rpid)
        {
            CSK_Store_RetailerProductNew rpn = (new SubSonic.Query.Select(rpCol).From("CSK_Store_RetailerProductNew").Where("RetailerProductId").In(rpid)).ExecuteSingle<CSK_Store_RetailerProductNew>();
            return rpn;
        }

        public static CSK_Store_RetailerProductNew GetRetailerProductHistory(int rpid)
        {
            CSK_Store_RetailerProduct rp = CSK_Store_RetailerProduct.SingleOrDefault(p => p.RetailerProductId == rpid);
            if (rp != null)
            {
                CSK_Store_RetailerProductNew rpn = new CSK_Store_RetailerProductNew();
                rpn.RetailerId = rp.RetailerId;
                rpn.RetailerProductId = rp.RetailerProductId;
                rpn.ProductId = rp.ProductId;
                rpn.RetailerProductName = rp.RetailerProductName;
                rpn.RetailerPrice = rp.RetailerPrice;
                rpn.RetailerProductStatus = rp.RetailerProductStatus;
                rpn.RetailerProductDescription = rp.RetailerProductDescription;
                rpn.RetailerProductSKU = rp.RetailerProductSKU;
                rpn.RetailerProductUIC = rp.RetailerProductUIC;
                rpn.Stock = rp.Stock;
                rpn.StockStatus = rp.StockStatus;
                rpn.CCFeeAmount = rp.CCFeeAmount;
                rpn.DefaultImage = rp.DefaultImage;
                rpn.Freight = rp.Freight;
                rpn.LongDescriptionEN = rp.LongDescriptionEN;
                rpn.OriginalPrice = rp.OriginalPrice;
                rpn.PurchaseURL = rp.PurchaseURL;
                rpn.RetailerProductMessage = rp.RetailerProductMessage;

                return rpn;
            }
            return null;
        }
        #endregion

        #region zhenglei
        public static CSK_Store_RetailerProduct GetRetailerProductByRetailerProductID(int retailerProductID)
        {
            return CSK_Store_RetailerProduct.SingleOrDefault(rp => rp.RetailerProductId == retailerProductID);
        }

        public static List<ProductCatalog> SearchRetailerProductByRetailerName(int retailerID)
        {
            return null;
        }

        public static decimal GetRetailerProductPrice(int retailerproductID)
        {
            if (retailerproductID == 0) return 0;

            CSK_Store_RetailerProductNew rpNew = GetRetailerProductHistory(retailerproductID);
            if(rpNew != null)
            {
                return rpNew.RetailerPrice;
            }
            return 0;
        }

        #endregion

        #region yuanxiang
        public static ProductCatalog GetRetailerProductCountByProductId(int productId)
        {
            return SearchController.SearchProducts(productId);
        }

        public static List<CSK_Store_RetailerProductNew> GetTopNRetailerProductsByCategoryId(int cid, int countryID, int count)
        {
            return GetTopNRetailerProductsByCategoryId(cid, countryID, count, PriceMeCommon.PriceMeStatic.Provider);
        }

        public static List<CSK_Store_RetailerProductNew> GetTopNRetailerProductsByCategoryId(int cid, int countryID, int count, IFormatProvider provider)
        {
            //读数据库
            //StoredProcedure sp = db.CSK_Store_TopNRetailerTrackRetailerProductsByCategory();

            //DateTime ds = DateTime.Now.AddDays(-30), de = DateTime.Now.AddDays(-1);
            //sp.Command.Parameters.Add("@CATEGORYID", cid, System.Data.DbType.Int32);
            //sp.Command.Parameters.Add("@BEGIN_DATE", ds, System.Data.DbType.DateTime);
            //sp.Command.Parameters.Add("@END_DATE", de, System.Data.DbType.DateTime);
            //sp.Command.Parameters.Add("@COUNT", count, System.Data.DbType.Int32);
            //sp.Command.Parameters.Add("@COUNTRY", countryID, System.Data.DbType.Int32);

            //return sp.ExecuteTypedList<CSK_Store_RetailerProduct>();

            RetailerProductSearcher retailerProductSearcher = new RetailerProductSearcher(0, cid);
            SearchResult<RetailerProductCatalog> searchResult = retailerProductSearcher.GetSearchResult(1, count);

            List<CSK_Store_RetailerProductNew> rps = new List<CSK_Store_RetailerProductNew>();
            foreach (RetailerProductCatalog rpc in searchResult.ResultList)
            {
                CSK_Store_RetailerProductNew rp = new CSK_Store_RetailerProductNew();
                rp.RetailerId = int.Parse(rpc.RetailerId);
                rp.ProductId = int.Parse(rpc.ProductId);
                rp.RetailerProductId = int.Parse(rpc.RetailerProductId);
                rp.RetailerProductName = rpc.RetailerProductName;
                rp.RetailerProductDescription = rpc.RetailerProductDescription;
                rp.DefaultImage = rpc.RetailerProductDefaultImage;
                rp.RetailerPrice = decimal.Parse(rpc.RetailerPrice, System.Globalization.NumberStyles.Any, PriceMeCommon.PriceMeStatic.Provider);
                rp.CCFeeAmount = 0;
                rp.Freight = -1;
                int rpcon = 0;
                int.TryParse(rpc.RetailerProductCondition,out rpcon);
                rp.RetailerProductCondition = rpcon;
                rps.Add(rp);
            }

            return rps;
        }
        #endregion

        #region huangriling

        //public static int GetRetailerProductCountByRetailerID(int retailer)
        //{
        //    try
        //    {
        //        StoredProcedure sp = db.GetRetailerProductCountByRetailerID();
        //        sp.Command.AddParameter("@rid", retailer, DbType.Int32);

        //        var rs = sp.ExecuteScalar<int>();

        //        return rs;
        //    }
        //    catch { return 0; }           
        //}



        public static int GetRetailerPhysicalStoreCountByRetailerID(int retailer)
        {
            return 0;
        }

        #endregion

        public static List<RetailerProductCatalog> SearchTopRetailerProduct(int retailerID)
        {
            RetailerProductSearcher retailerProductSearcher = new RetailerProductSearcher(retailerID, 0);
            SearchResult<RetailerProductCatalog> searchResult = retailerProductSearcher.GetSearchResult(1, 1000);

            return searchResult.ResultList;
        }

        public static List<RetailerProductCatalog> SearchRetailerProduct(int retailerID, int categoryID, int indexPage, int pageSize, out int totalPage, out int totalProductCount)
        {
            RetailerProductSearcher retailerProductSearcher = new RetailerProductSearcher(retailerID, categoryID);
            SearchResult<RetailerProductCatalog> searchResult = retailerProductSearcher.GetSearchResult(indexPage, pageSize);
            totalPage = searchResult.PageCount;
            totalProductCount = searchResult.ProductCount;

            return searchResult.ResultList;
        }

        public static List<CategoryResultsInfo> SearchRetailerCategoryByRetailerId(int retailerId)
        {
            RetailerProductSearcher retailerProductSearcher = new RetailerProductSearcher(retailerId, 0);
            List<CategoryResultsInfo> dictionary = retailerProductSearcher.GetCategoryInfo();

            dictionary = dictionary.OrderBy(di => di.CategoryName).ToList();

            return dictionary;
        }

        public static int GetRedirectforRetailerproduct(int rpid)
        {
            int pid = 0;
            string sql = "Select ProductId from Csk_store_RedirectforRetailerproduct Where RetailerProductId = " + rpid;
            StoredProcedure sp = new StoredProcedure("");
            sp.Command.CommandSql = sql;
            sp.Command.CommandTimeout = 0;
            sp.Command.CommandType = CommandType.Text;
            IDataReader dr = sp.ExecuteReader();
            while (dr.Read())
            {
                int.TryParse(dr["ProductId"].ToString(), out pid);
            }
            dr.Close();
            return pid;
        }

        public static bool CheckInternationalRetailerProducts(List<CSK_Store_RetailerProductNew> rps, out decimal overseasPices)
        {
            bool isInternational = false;
            overseasPices = 0;

            int localRP = rps.Where(r => !RetailerController.InternationalRetailers.ContainsKey(r.RetailerId)).ToList().Count;
            int internationalRP = rps.Where(r => RetailerController.InternationalRetailers.ContainsKey(r.RetailerId)).ToList().Count;
            if (localRP > 0 && internationalRP > 0)
            {
                overseasPices = rps.Where(r => RetailerController.InternationalRetailers.ContainsKey(r.RetailerId)).OrderBy(o => o.RetailerPrice).ToList()[0].RetailerPrice;
                isInternational = true;
            }

            return isInternational;
        }

        public static bool CheckInternationalRetailerProducts(int pid, out decimal overseasPices)
        {
            List<CSK_Store_RetailerProductNew> rps = RetailerProductController.GetRetailerProductsByProductId(pid);
            bool isInternational = CheckInternationalRetailerProducts(rps, out overseasPices);

            return isInternational;
        }

        public static List<int> GetRetailerIdListByProductId(int pid)
        {
            List<int> retailerIDs = new List<int>();

            retailerIDs = RetailerProductController.GetRetailerProductsByProductId(pid)
                .Select(rp => rp.RetailerId).Distinct().ToList();

            return retailerIDs;
        }

        public static List<CSK_Store_PriceHistory> GetPriceHistory(List<int> listpid)
        {
            List<CSK_Store_PriceHistory> phs = new List<CSK_Store_PriceHistory>();
            string pids = string.Empty;
            foreach (int pid in listpid)
            {
                pids += pid + ",";
            }
            pids = pids.Substring(0, pids.LastIndexOf(','));
            string sql = "select ProductID, PriceDate, Price, CreatedOn from CSK_Store_PriceHistory where productid in (" + pids + ")";
            StoredProcedure sp = new StoredProcedure("");
            sp.Command.CommandSql = sql;
            sp.Command.CommandTimeout = 0;
            sp.Command.CommandType = CommandType.Text;
            IDataReader dr = sp.ExecuteReader();
            while (dr.Read())
            {
                int pid = 0;
                decimal Price = 0;
                DateTime PriceDate = DateTime.Now;
                DateTime CreatedOn = DateTime.Now;
                int.TryParse(dr["ProductID"].ToString(), out pid);
                decimal.TryParse(dr["Price"].ToString(), out Price);
                DateTime.TryParse(dr["PriceDate"].ToString(), out PriceDate);
                DateTime.TryParse(dr["CreatedOn"].ToString(), out CreatedOn);

                CSK_Store_PriceHistory ph = new CSK_Store_PriceHistory();
                ph.ProductID = pid;
                ph.Price = Price;
                ph.PriceDate = PriceDate;
                ph.CreatedOn = CreatedOn;
                phs.Add(ph);
            }
            dr.Close();

            return phs;
        }
    }
}