using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PriceMeDBA;
using SubSonic.Schema;

namespace PriceMeCommon.BusinessLogic
{
    public class MostPopularProductController
    {

        private static PriceMeDBDB db = PriceMeStatic.PriceMeDB;

        private static List<MostPopularProduct> _top5RetailerTrackProduct = null;
        private static Dictionary<int, List<Data.ProductCatalog>> _top5RetailerTrackProductByCategory = new Dictionary<int, List<Data.ProductCatalog>>();

        public static Dictionary<int, List<Data.ProductCatalog>> Top5RetailerTrackProductByCategory
        {
            get { return _top5RetailerTrackProductByCategory; }
        }

        public static List<MostPopularProduct> Top5RetailerTrackProduct
        {
            get { return _top5RetailerTrackProduct; }
        }

        //public static List<MostPopularProduct> GetTop5RetailerTrackProduct(int countryID)
        //{
        //    if (Top5RetailerTrackProduct == null)
        //    {

        //        DateTime ds = DateTime.Now.AddDays(-30), de = DateTime.Now.AddDays(-1);

        //        //StoredProcedure sp = new StoredProcedure("CSK_Store_TopNRetailerTrackProducts");
        //        StoredProcedure sp = db.CSK_Store_TopNRetailerTrackProducts();
        //        sp.Command.Parameters.Add("@BEGIN_DATE", ds, System.Data.DbType.DateTime);
        //        sp.Command.Parameters.Add("@END_DATE", de, System.Data.DbType.DateTime);
        //        sp.Command.Parameters.Add("@COUNT", 15, System.Data.DbType.Int32);
        //        sp.Command.Parameters.Add("@COUNTRY", countryID, System.Data.DbType.Int32);

        //        List<MostPopularProduct> ls = sp.ExecuteTypedList<MostPopularProduct>();
        //        _top5RetailerTrackProduct = ls.Take(5).Where(l => l.BestPrice > 0).ToList();
        //        return ls;
        //    }
        //    else
        //    {
        //        return Top5RetailerTrackProduct;
        //    }
        //}

        static object threadObj = new object();
        public static List<Data.ProductCatalog> GetTopNRetailerTrackProductByCategory(int cid, int countryID, int count)
        {
            try
            {
                //
                //TODO: Top5RetailerTrackProductByCategory 和 Top5RetailerTrackProduct 如何更新,生命周期
                //
                //
                //TODO: 这个存储过程有点慢，放这里合适吗？
                //
                if (Top5RetailerTrackProductByCategory.ContainsKey(cid))
                    return Top5RetailerTrackProductByCategory[cid];
                else
                {
                    lock (threadObj)
                    {
                        if (Top5RetailerTrackProductByCategory.ContainsKey(cid))
                            return Top5RetailerTrackProductByCategory[cid];
                        else
                        {
                            ProductSearcher productSearcher = new ProductSearcher(cid, null, null, null, null, "", "", null, true, 10, ConfigAppString.CountryID, true);
                            Data.SearchResult sr = productSearcher.GetSearchResult(1, 10);

                            List<Data.ProductCatalog> pcLs = sr.ProductCatalogList;

                            if (!Top5RetailerTrackProductByCategory.ContainsKey(cid))
                                Top5RetailerTrackProductByCategory.Add(cid, pcLs);

                            return pcLs;
                        }
                    }
                }
            }
            catch (Exception)
            {

            }
            return new List<Data.ProductCatalog>();
        }
    }

    public class MostPopularProduct
    {
        int productID, cnt;
        double bestPrice;
        string productName;
        string defaultImage;

        public string DefaultImage
        {
            get { return defaultImage; }
            set { defaultImage = value; }
        }

        public int Cnt
        {
            get { return cnt; }
            set { cnt = value; }
        }

        public double BestPrice
        {
            get { return bestPrice; }
            set { bestPrice = value; }
        }

        public int ProductID
        {
            get { return productID; }
            set { productID = value; }
        }

        public string ProductName
        {
            get { return productName; }
            set { productName = value; }
        }
    }
}
