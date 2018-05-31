using Lucene.Net.Documents;
using PriceMeCache;
using PriceMeCommon.Data;
using PriceMeDBA;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceMeCommon.BusinessLogic
{
    public static class ProductController
    {
        static Dictionary<int, Dictionary<int, ReviewAverage>> MultiContryReviewAverageDic_Static;
        static Dictionary<int, Dictionary<int, List<ProductVideo>>> MultiContryProductVideoDic_Static;
        static Dictionary<int, Dictionary<int, ExpertReviewSource>> MultiCountryExpertReviewSourcesDic_Static;
        static Dictionary<int, Dictionary<int, string>> MultiCountryEnergyImgsDic_Static;
        static Dictionary<int, List<HotProduct>> MultiCountryHotProducts_Static;
        static Dictionary<int, Dictionary<int, RetailerProductCondition>> MultiCountryRetailerProductConditionDic_Static;

        static Dictionary<int, Dictionary<int, string>> MultiCountryProductDescriptionDic_Static;

        static Dictionary<int, int> IntraLinkingGenerationsDic_Static;
        static Dictionary<int, List<ProductDescAndAttr>> ProductDescAndAttrsDic_Static;

        public static void Load(Timer.DKTimer dkTimer)
        {
            if (dkTimer != null)
            {
                dkTimer.Set("ProductController.Load() --- Befor load GetAllReviewAverage");
            }
            MultiContryReviewAverageDic_Static = GetMultiContryReviewAverageDic();
            MultiContryProductVideoDic_Static = GetMultiContryProductVideoDic();
            MultiCountryExpertReviewSourcesDic_Static = GetMultiCountryExpertReviewSourcesDic();
            MultiCountryEnergyImgsDic_Static = GetMultiCountryEnergyImgsDic();

            MultiCountryHotProducts_Static = GetMultiCountryHotProducts();
            MultiCountryRetailerProductConditionDic_Static = GetMultiCountryRetailerProductConditionDic();

            MultiCountryProductDescriptionDic_Static = GetMultiCountryProductDescriptionDic();
            IntraLinkingGenerationsDic_Static = GetIntraLinkingGenerationsDic();

            ProductDescAndAttrsDic_Static = GetProductDescAndAttrsDic();
        }

        private static Dictionary<int, List<ProductDescAndAttr>> GetProductDescAndAttrsDic()
        {
            Dictionary<int, List<ProductDescAndAttr>> dic = new Dictionary<int, List<ProductDescAndAttr>>();

            string connectionStr = MultiCountryController.CommonConnectionStringSettings_Static.ConnectionString;
            var sql = string.Format("SELECT * FROM [dbo].Product_DescAndAttr");
            using (SqlConnection conn = new SqlConnection(connectionStr))
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.CommandTimeout = 0;
                conn.Open();
                var idr = cmd.ExecuteReader();
                while (idr.Read())
                {
                    ProductDescAndAttr pda = new ProductDescAndAttr();
                    int productId = int.Parse(idr["ProductID"].ToString());
                    string title = idr["title"] == null ? "" : idr["title"].ToString().Trim();
                    string sdesc = idr["ShortDescription"] == null ? "" : idr["ShortDescription"].ToString().Trim();
                    string value = idr["Value"] == null ? "" : idr["Value"].ToString().Trim();
                    string unit = idr["Unit"] == null ? "" : idr["Unit"].ToString().Trim();
                    int t, avs, cid, tid, sid;
                    int.TryParse(idr["T"].ToString(), out t);
                    int.TryParse(idr["AVS"].ToString(), out avs);
                    int.TryParse(idr["CID"].ToString(), out cid);
                    int.TryParse(idr["AttributeTypeID"].ToString(), out tid);
                    int.TryParse(idr["SID"].ToString(), out sid);

                    pda.Title = title;
                    pda.ShortDescription = sdesc;
                    pda.Value = value;
                    pda.Unit = unit;
                    pda.T = t;
                    pda.AVS = avs;
                    pda.CID = cid;
                    pda.AttributeTypeID = tid;
                    pda.AttributeTitleID = sid;
                    pda.ProductId = productId;

                    if (dic.ContainsKey(productId))
                    {
                        dic[productId].Add(pda);
                    }
                    else
                    {
                        List<ProductDescAndAttr> productDescAndAttrList = new List<ProductDescAndAttr>();
                        productDescAndAttrList.Add(pda);
                        dic.Add(productId, productDescAndAttrList);
                    }
                }
            }

            return dic;
        }




        public static List<ProductDescAndAttr> GetDescriptionAndAttribute(int pId)
        {
            if (ProductDescAndAttrsDic_Static.ContainsKey(pId))
            {
                return ProductDescAndAttrsDic_Static[pId];
            }

            return new List<ProductDescAndAttr>();
        }

        private static Dictionary<int, int> GetIntraLinkingGenerationsDic()
        {
            Dictionary<int, int> dic = new Dictionary<int, int>();

            var connectionStr = MultiCountryController.CommonConnectionStringSettings_Static.ConnectionString;
            string sql = string.Format("SELECT [ProductID],[LinedPID] FROM [dbo].[IntraLinkingGenerationAndRelated] where LinkType = 'Generation'and ShowType = 'Product'");
            using (SqlConnection conn = new SqlConnection(connectionStr))
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.CommandTimeout = 0;
                conn.Open();
                using (var idr = cmd.ExecuteReader())
                {
                    while (idr.Read())
                    {
                        int pId = idr.GetInt32(0);
                        int linedProductId = idr.GetInt32(1);
                        if (dic.ContainsKey(pId))
                        {
                            dic[pId] = linedProductId;
                        }
                        else
                        {
                            dic.Add(pId, linedProductId);
                        }
                    }
                }
            }

            return dic;
        }

        private static Dictionary<int, Dictionary<int, string>> GetMultiCountryProductDescriptionDic()
        {
            Dictionary<int, Dictionary<int, string>> multiDic = new Dictionary<int, Dictionary<int, string>>();

            var connectionStr = MultiCountryController.CommonConnectionStringSettings_Static.ConnectionString;
            string sql = string.Format("SELECT [PID],[Description], COUNTRYID FROM [CSK_Store_PM_ProductDescription] WHERE COUNTRYID in (" + string.Join(",", MultiCountryController.CountryIdList) + ")");
            using (SqlConnection conn = new SqlConnection(connectionStr))
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.CommandTimeout = 0;
                conn.Open();
                using (var idr = cmd.ExecuteReader())
                {
                    while (idr.Read())
                    {
                        int pid = idr.GetInt32(0);
                        string desc = idr.GetString(1);
                        int countryId = idr.GetInt32(2);
                        if (multiDic.ContainsKey(countryId))
                        {
                            multiDic[countryId].Add(pid, desc);
                        }
                        else
                        {
                            Dictionary<int, string> dic = new Dictionary<int, string>();
                            dic.Add(pid, desc);
                            multiDic.Add(countryId, dic);
                        }
                    }
                }
            }

            return multiDic;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private static Dictionary<int, Dictionary<int, RetailerProductCondition>> GetMultiCountryRetailerProductConditionDic()
        {
            Dictionary<int, Dictionary<int, RetailerProductCondition>> multiDic = new Dictionary<int, Dictionary<int, RetailerProductCondition>>();

            foreach (int countryId in MultiCountryController.CountryIdList)
            {
                Dictionary<int, RetailerProductCondition> dic = GetRetailerProductCondition(countryId);
                multiDic.Add(countryId, dic);
            }

            return multiDic;
        }

        private static Dictionary<int, RetailerProductCondition> GetRetailerProductCondition(int countryId)
        {
            Dictionary<int, RetailerProductCondition> dic = new Dictionary<int, RetailerProductCondition>();
            string sql = "Select RetailerProductConditionId, ConditionName, ConditionDescription From CSK_Store_RetailerProductCondition";
            using (SqlConnection conn = new SqlConnection(MultiCountryController.GetDBConnectionString(countryId)))
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.CommandTimeout = 0;
                conn.Open();
                using (var idr = cmd.ExecuteReader())
                {
                    while (idr.Read())
                    {
                        int id = 0;
                        int.TryParse(idr["RetailerProductConditionId"].ToString(), out id);
                        string name = idr["ConditionName"].ToString();
                        string description = idr["ConditionDescription"].ToString();

                        RetailerProductCondition cond = new RetailerProductCondition();
                        cond.Id = id;
                        cond.ConditionName = name;
                        cond.ConditionDescription = description;

                        if (!dic.ContainsKey(id))
                            dic.Add(id, cond);
                    }
                }
            }

            return dic;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private static Dictionary<int, List<HotProduct>> GetMultiCountryHotProducts()
        {
            List<HotProduct> commonList = GetCommonHotProducts();

            Dictionary<int, List<HotProduct>> multiDic = new Dictionary<int, List<HotProduct>>();

            foreach (int countryId in MultiCountryController.CountryIdList)
            {
                List<HotProduct> topList = GetTopNHotProducts(commonList, countryId);
                multiDic.Add(countryId, topList);
            }

            return multiDic;
        }

        private static List<HotProduct> GetTopNHotProducts(List<HotProduct> commonList, int countryId)
        {
            List<HotProduct> hotProducts = new List<HotProduct>();

            List<HotProduct> hotProductsLocal = new List<HotProduct>();

            var sql = @"SELECT p.ProductId,p.ProductName, MIN(RetailerPrice) Price, p.DefaultImage FROM CSK_Store_Product p
                INNER JOIN CSK_Store_RetailerProductNew rp  ON p.ProductID = rp.ProductId WHERE p.ProductId IN({0}) 
                AND rp.RetailerProductStatus = 1 GROUP BY p.ProductId,p.ProductName, p.DefaultImage";

            sql = string.Format(sql, string.Join(",", commonList.Select(h => h.ProductID)));

            using (SqlConnection conn = new SqlConnection(MultiCountryController.GetDBConnectionString(countryId)))
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.CommandTimeout = 0;
                conn.Open();
                using (var idr = cmd.ExecuteReader())
                {
                    while (idr.Read())
                    {
                        HotProduct p = new HotProduct();
                        p.ProductID = int.Parse(idr["ProductId"].ToString());
                        p.BestPrice = double.Parse(idr["Price"].ToString());
                        p.ProductName = idr["ProductName"].ToString();
                        p.DefaultImage = idr["DefaultImage"].ToString();

                        hotProductsLocal.Add(p);
                    }
                }
            }

            #region filter have no rp product 
            foreach (var p2 in hotProductsLocal)
            {
                var p1 = commonList.SingleOrDefault(p => p.ProductID == p2.ProductID);
                if (p1 != null)
                {
                    HotProduct hp = new HotProduct();
                    hp.ProductID = p1.ProductID;
                    hp.DisplayName = p1.DisplayName;
                    hp.DisplayOrder = p1.DisplayOrder;
                    hp.BestPrice = p2.BestPrice;
                    hp.ProductName = p2.ProductName;
                    hp.DefaultImage = p2.DefaultImage;
                    hp.CommonImage = p1.CommonImage;

                    hotProducts.Add(hp);
                }
            }
            hotProducts = hotProducts.OrderBy(p => p.DisplayOrder).ToList();
            #endregion

            return hotProducts;
        }

        private static List<HotProduct> GetCommonHotProducts()
        {
            List<HotProduct> hotProducts = new List<HotProduct>();

            var pids = new List<int>();
            string connectionStr = MultiCountryController.CommonConnectionStringSettings_Static.ConnectionString;
            var sql = string.Format("SELECT * FROM [dbo].[HotProducts] ORDER BY DisplayOrder");
            using (SqlConnection conn = new SqlConnection(connectionStr))
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.CommandTimeout = 0;
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        HotProduct p = new HotProduct();
                        p.ProductID = int.Parse(reader["ProductId"].ToString());
                        p.DisplayName = reader["DisplayName"].ToString();
                        p.DisplayOrder = int.Parse(reader["DisplayOrder"].ToString());
                        p.CommonImage = reader["Imagesurl"].ToString();

                        pids.Add(p.ProductID);
                        hotProducts.Add(p);
                    }
                }
            }

            return hotProducts;
        }

        private static Dictionary<int, Dictionary<int, string>> GetMultiCountryEnergyImgsDic()
        {
            Dictionary<int, Dictionary<int, string>> multiDic = new Dictionary<int, Dictionary<int, string>>();

            foreach (int countryId in MultiCountryController.CountryIdList)
            {
                Dictionary<int, string> dic = GetEnergyImgs(countryId);
                multiDic.Add(countryId, dic);
            }

            return multiDic;
        }

        private static Dictionary<int, string> GetEnergyImgs(int countryId)
        {
            //原Velocity - VelocityCacheKey.EnergyImgs
            Dictionary<int, string> energyImgs = new Dictionary<int, string>();
            string sql = "select ProductId, EnergyImage from CSK_Store_Energy where ProductId is not null And ProductId != ''";
            using (SqlConnection conn = new SqlConnection(MultiCountryController.GetDBConnectionString(countryId)))
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.CommandTimeout = 0;
                conn.Open();
                using (var idr = cmd.ExecuteReader())
                {
                    while (idr.Read())
                    {
                        string pidString = idr["ProductId"].ToString();
                        string image = idr["EnergyImage"].ToString();
                        if (pidString.Contains(","))
                        {
                            string[] temps = pidString.Split(',');
                            for (int i = 0; i < temps.Length; i++)
                            {
                                int pid = 0;
                                int.TryParse(temps[i], out pid);
                                if (!energyImgs.ContainsKey(pid))
                                    energyImgs.Add(pid, image);
                            }
                        }
                        else
                        {
                            int pid = 0;
                            int.TryParse(pidString, out pid);
                            if (!energyImgs.ContainsKey(pid))
                                energyImgs.Add(pid, image);
                        }
                    }
                }
            }

            return energyImgs;
        }

        private static Dictionary<int, Dictionary<int, ExpertReviewSource>> GetMultiCountryExpertReviewSourcesDic()
        {
            Dictionary<int, Dictionary<int, ExpertReviewSource>> multiDic = new Dictionary<int, Dictionary<int, ExpertReviewSource>>();

            foreach (int countryId in MultiCountryController.CountryIdList)
            {
                Dictionary<int, ExpertReviewSource> dic = GetExpertReviewSources(countryId);
                multiDic.Add(countryId, dic);
            }

            return multiDic;
        }

        private static Dictionary<int, ExpertReviewSource> GetExpertReviewSources(int countryId)
        {
            //原Velocity - VelocityCacheKey.ExpertReviewSource
            Dictionary<int, ExpertReviewSource> dic = new Dictionary<int, ExpertReviewSource>();

            string sql = "GetAllExpertReviewSourceTF";
            using (SqlConnection conn = new SqlConnection(MultiCountryController.GetDBConnectionString(countryId)))
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;
                conn.Open();
                using (var idr = cmd.ExecuteReader())
                {
                    while (idr.Read())
                    {
                        ExpertReviewSource expertReviewSource = new ExpertReviewSource();
                        expertReviewSource.SourceID = int.Parse(idr["SourceID"].ToString());
                        expertReviewSource.WebSiteName = idr["WebSiteName"].ToString();
                        expertReviewSource.LogoFile = idr["LogoFile"].ToString();
                        //expertReviewSource.CountryID = int.Parse(idr["CountryID"].ToString());
                        expertReviewSource.HomePage = idr["HomePage"].ToString();

                        if (!dic.ContainsKey(expertReviewSource.SourceID))
                            dic.Add(expertReviewSource.SourceID, expertReviewSource);
                    }
                }
            }

            return dic;
        }

        private static Dictionary<int, Dictionary<int, List<ProductVideo>>> GetMultiContryProductVideoDic()
        {
            Dictionary<int, Dictionary<int, List<ProductVideo>>> multiDic = new Dictionary<int, Dictionary<int, List<ProductVideo>>>();

            foreach (int countryId in MultiCountryController.CountryIdList)
            {
                Dictionary<int, List<ProductVideo>> dic = GetAllProductVideo(countryId);
                multiDic.Add(countryId, dic);
            }

            return multiDic;
        }

        public static Dictionary<int, List<ProductVideo>> GetAllProductVideo(int countryId)
        {
            var searcher = MultiCountryController.GetProductVideoLuceneSearcher(countryId);

            Dictionary<int, List<ProductVideo>> pvs = new Dictionary<int, List<ProductVideo>>();

            for (int i = 0; i < searcher.IndexReader.MaxDoc; i++)
            {
                ProductVideo pv = new ProductVideo();
                Document doc = searcher.Doc(i);
                pv.ProductID = int.Parse(doc.Get("ProductID"));
                pv.Url = doc.Get("Url").ToString();
                pv.Thumbnail = doc.Get("Thumbnail").ToString();

                if (!pvs.ContainsKey(pv.ProductID))
                {
                    List<ProductVideo> pvList = new List<ProductVideo>();
                    pvList.Add(pv);
                    pvs.Add(pv.ProductID, pvList);
                }
                else
                {
                    List<ProductVideo> pvList = pvs[pv.ProductID];
                    pvList.Add(pv);
                    pvs[pv.ProductID] = pvList;
                }
            }

            return pvs;
        }

        private static Dictionary<int, Dictionary<int, ReviewAverage>> GetMultiContryReviewAverageDic()
        {
            Dictionary<int, Dictionary<int, ReviewAverage>> multiDic = new Dictionary<int, Dictionary<int, ReviewAverage>>();

            foreach (int countryId in MultiCountryController.CountryIdList)
            {
                Dictionary<int, ReviewAverage> dic = GetReviewAverageDic(countryId);
                multiDic.Add(countryId, dic);
            }

            return multiDic;
        }

        private static Dictionary<int, ReviewAverage> GetReviewAverageDic(int countryId)
        {
            Dictionary<int, ReviewAverage> ras = new Dictionary<int, ReviewAverage>();

            var searcher = MultiCountryController.GetReviewAverageLuceneSearcher(countryId);

            for (int i = 0; i < searcher.IndexReader.MaxDoc; i++)
            {
                ReviewAverage ra = new ReviewAverage();
                Document doc = searcher.Doc(i);
                ra.ProductID = int.Parse(doc.Get("ProductID"));
                ra.ExpertReviewCount = int.Parse(doc.Get("ExpertReviewCount"));
                ra.UserReviewCount = int.Parse(doc.Get("UserReviewCount"));
                ra.ReviewCount = ra.ExpertReviewCount + ra.UserReviewCount;

                float pr = 0;
                float.TryParse(doc.Get("ProductRating").ToString(), out pr);
                ra.ProductRating = pr;
                ra.FeatureScore = doc.Get("FeatureScore").ToString();

                float erating = 0;
                float.TryParse(doc.Get("TFEAverageRating").ToString(), out erating);
                ra.TFEAverageRating = erating;
                float urating = 0;
                float.TryParse(doc.Get("TFUAverageRating").ToString(), out urating);
                ra.TFUAverageRating = urating;

                if (!ras.ContainsKey(ra.ProductID))
                    ras.Add(ra.ProductID, ra);
            }

            return ras;
        }

        public static bool CheckInternationalRetailerProducts(List<CSK_Store_RetailerProductNew> rps, out decimal overseasPices, int countryId)
        {
            bool isInternational = false;
            overseasPices = 0;

            int localRP = rps.Where(r => !RetailerController.IsInternationalRetailer(r.RetailerId, countryId)).ToList().Count;
            int internationalRP = rps.Where(r => RetailerController.IsInternationalRetailer(r.RetailerId, countryId)).ToList().Count;
            if (localRP > 0 && internationalRP > 0)
            {
                overseasPices = rps.Where(r => RetailerController.IsInternationalRetailer(r.RetailerId, countryId)).OrderBy(o => o.RetailerPrice).ToList()[0].RetailerPrice;
                isInternational = true;
            }

            return isInternational;
        }

        public static RetailerProductCondition GetRetailerProductCondition(int key, int countryId)
        {
            if (key != 0 && MultiCountryRetailerProductConditionDic_Static.ContainsKey(countryId) && MultiCountryRetailerProductConditionDic_Static[countryId].ContainsKey(key))
                return MultiCountryRetailerProductConditionDic_Static[countryId][key];

            return null;
        }

        public static CSK_Store_ProductNew GetProductNew(int productId, int countryId)
        {
            using (SubSonic.DataProviders.SharedDbConnectionScope sdbs = new SubSonic.DataProviders.SharedDbConnectionScope(MultiCountryController.GetDBProvider(countryId)))
            {
                CSK_Store_ProductNew pn = CSK_Store_ProductNew.SingleOrDefault(p => p.ProductID == productId);
                return pn;
            }
        }

        public static CSK_Store_ProductIsMerged GetProductIdInProductIsMergedByProductId(int pid, int countryId)
        {
            //数据有问题   有些会有多条记录
            CSK_Store_ProductIsMerged pim = null;
            try { pim = CSK_Store_ProductIsMerged.SingleOrDefault(pm => pm.ProductID == pid); }
            catch { pim = CSK_Store_ProductIsMerged.Find(pm => pm.ProductID == pid).FirstOrDefault(); }

            return pim;
        }

        public static CSK_Store_ProductNew GetProductHistory(int productId, int countryId)
        {
            CSK_Store_Product pn = CSK_Store_Product.Find(p => p.ProductID == productId).SingleOrDefault();
            if (pn != null)
            {
                CSK_Store_ProductNew product = new CSK_Store_ProductNew();
                product.ProductID = pn.ProductID;
                product.ProductName = pn.ProductName;
                product.CategoryID = pn.CategoryID;
                product.ManufacturerID = pn.ManufacturerID;
                product.DefaultImage = pn.DefaultImage;
                product.ShortDescriptionZH = pn.ShortDescriptionZH;
                product.ProductAttributeText = pn.ProductAttributeText;
                product.CreatedOn = pn.CreatedOn;
                product.Weight = pn.Weight;
                product.Height = pn.Height;
                product.Width = pn.Width;
                product.Height = pn.Height;

                return product;
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="countryId"></param>
        /// <returns></returns>
        public static CSK_Store_ProductNew GetRealProduct(int productId, int countryId)
        {
            CSK_Store_ProductNew product = GetProductNew(productId, countryId);

            if (product == null)
            {
                while (true)
                {
                    CSK_Store_ProductIsMerged pm = GetProductIdInProductIsMergedByProductId(productId, countryId);
                    if (pm != null)
                    {
                        productId = pm.ToProductID;
                        product = GetProductNew(pm.ToProductID, countryId);
                        if (product == null)
                            product = GetProductHistory(pm.ToProductID, countryId);

                        if (product != null)
                        {
                            return product;
                        }
                    }
                    else
                    {
                        product = GetProductHistory(productId, countryId);
                        return product;
                    }
                }
            }

            return product;
        }

        /// <summary>
        /// 先只用于产品页面
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="countryId"></param>
        /// <returns></returns>
        public static CSK_Store_ProductNew GetRealProductSimplified(int productId, int countryId, bool UseLibraryTable)
        {
            CSK_Store_ProductNew product = GetProductNewSimplified(productId, countryId);

            if (product == null)
            {
                while (true)
                {
                    CSK_Store_ProductIsMerged pm = GetProductIdInProductIsMergedByProductId(productId, countryId);
                    if (pm != null)
                    {
                        productId = pm.ToProductID;
                        product = GetProductNewSimplified(pm.ToProductID, countryId);
                        if (product == null)
                            product = GetProductHistorySimplified(pm.ToProductID, countryId);

                        if (product != null)
                        {
                            return product;
                        }
                    }
                    else
                    {
                        if (UseLibraryTable)
                        {
                            product = GetProductHistorySimplified(productId, countryId);
                            return product;
                        }
                        else
                            return null;
                    }
                }
            }

            return product;
        }

        private static CSK_Store_ProductNew GetProductHistorySimplified(int productId, int countryId)
        {
            string connectionStr = MultiCountryController.GetDBConnectionString(countryId);
            var sql = string.Format("SELECT [ProductID],[ProductName],[ShortDescriptionZH],[ManufacturerID],[DefaultImage],[CategoryID],[ProductAttributeText],[CreatedOn],[Weight],[Height],[Width],[Length],[UnitOfMeasure] FROM [dbo].[CSK_Store_Product] where ProductID = " + productId);
            using (SqlConnection conn = new SqlConnection(connectionStr))
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.CommandTimeout = 0;
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        CSK_Store_ProductNew product = new CSK_Store_ProductNew();
                        product.ProductID = reader.GetInt32(0);
                        product.ProductName = reader.GetString(1);
                        product.ShortDescriptionZH = reader.IsDBNull(2) ? "" : reader.GetString(2);

                        product.ManufacturerID = reader.GetInt32(3);
                        product.DefaultImage = reader.IsDBNull(4) ? "" : reader.GetString(4);
                        product.CategoryID = reader.GetInt32(5);
                        product.ProductAttributeText = reader.IsDBNull(6) ? "" : reader.GetString(6);

                        product.CreatedOn = reader.IsDBNull(7) ? DateTime.Now : reader.GetDateTime(7);
                        product.Weight = reader.IsDBNull(8) ? 0 : reader.GetDecimal(8);
                        product.Height = reader.IsDBNull(9) ? 0 : reader.GetDecimal(9);
                        product.Width = reader.IsDBNull(10) ? 0 : reader.GetDecimal(10);
                        product.Length = reader.IsDBNull(11) ? 0 : reader.GetDecimal(11);
                        product.UnitOfMeasure = reader.IsDBNull(12) ? "" : reader.GetString(12);

                        return product;
                    }
                }
            }

            return null;
        }

        private static CSK_Store_ProductNew GetProductNewSimplified(int productId, int countryId)
        {
            string connectionStr = MultiCountryController.GetDBConnectionString(countryId);
            var sql = string.Format("SELECT [ProductID],[ProductName],[ShortDescriptionZH],[ManufacturerID],[DefaultImage],[CategoryID],[ProductAttributeText],[CreatedOn],[Weight],[Height],[Width],[Length],[UnitOfMeasure] FROM [dbo].[CSK_Store_ProductNew] where ProductID = " + productId);
            using (SqlConnection conn = new SqlConnection(connectionStr))
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.CommandTimeout = 0;
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        CSK_Store_ProductNew product = new CSK_Store_ProductNew();
                        product.ProductID = reader.GetInt32(0);
                        product.ProductName = reader.GetString(1);
                        product.ShortDescriptionZH = reader.IsDBNull(2) ? "" : reader.GetString(2);

                        product.ManufacturerID = reader.GetInt32(3);
                        product.DefaultImage = reader.IsDBNull(4) ? "" : reader.GetString(4);
                        product.CategoryID = reader.GetInt32(5);
                        product.ProductAttributeText = reader.IsDBNull(6) ? "" : reader.GetString(6);
                        product.CreatedOn = reader.GetDateTime(7);

                        product.Weight = reader.IsDBNull(8) ? 0 : reader.GetDecimal(8);
                        product.Height = reader.IsDBNull(9) ? 0 : reader.GetDecimal(9);
                        product.Width = reader.IsDBNull(10) ? 0 : reader.GetDecimal(10);
                        product.Length = reader.IsDBNull(11) ? 0 : reader.GetDecimal(11);
                        product.UnitOfMeasure = reader.IsDBNull(12) ? "" : reader.GetString(12);

                        return product;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="countryId"></param>
        /// <returns></returns>
        public static string CheckUpcomingProduct(int pid, int countryId)
        {
            string upcoming = string.Empty;
            string connectionStr = MultiCountryController.CommonConnectionStringSettings_Static.ConnectionString;
            var sql = string.Format("Select ReleaseDate, ReleaseDateIsEstimated From UpcomingProduct Where ProductId = " + pid + " And ReleaseDate > '" + DateTime.Today.ToString("yyyy-MM-dd") + "'");
            using (SqlConnection conn = new SqlConnection(connectionStr))
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.CommandTimeout = 0;
                conn.Open();
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    DateTime release = DateTime.Now;
                    DateTime.TryParse(reader["ReleaseDate"].ToString(), out release);
                    if (release.AddDays(60) < DateTime.Now)
                        return string.Empty;

                    bool isEstimated = false;
                    bool.TryParse(reader["ReleaseDateIsEstimated"].ToString(), out isEstimated);

                    string countryname = CSK_Util_Country.SingleOrDefault(c => c.countryID == countryId).country;

                    string stringdate = release.ToString("dd MMM yyyy");
                    if (isEstimated)
                        upcoming = "This is a new release that's launching soon in " + countryname + ". The estimated release date is " + stringdate + ".";
                    else
                        upcoming = "This is a new release that's launching soon in " + countryname + ". The release date is " + stringdate + ".";
                }
            }

            return upcoming;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="email"></param>
        /// <param name="countryId"></param>
        /// <returns></returns>
        public static bool UpcomingProductSelect(int pid, string email, int countryId)
        {
            bool isupcoming = false;
            string connectionStr = MultiCountryController.CommonConnectionStringSettings_Static.ConnectionString;
            var sql = string.Format("Select * from UpcomingProductAlter Where UpcomingProductID = " + pid + " And email = '" + email.Replace("'", "''") + "'");
            using (SqlConnection conn = new SqlConnection(connectionStr))
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.CommandTimeout = 0;
                conn.Open();

                var dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    isupcoming = true;
                }
                conn.Close();
            }

            return isupcoming;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="email"></param>
        public static void UpcomingProductSave(int pid, string email, int countryId)
        {
            string upcoming = string.Empty;
            string connectionStr = MultiCountryController.CommonConnectionStringSettings_Static.ConnectionString;
            var sql = string.Format("insert UpcomingProductAlter values(" + pid + ", '" + email.Replace("'", "''") + "', 0, " + countryId + ")");
            using (SqlConnection conn = new SqlConnection(connectionStr))
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.CommandTimeout = 0;
                conn.Open();
                cmd.ExecuteScalar();
                conn.Close();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="newprice"></param>
        /// <param name="pid"></param>
        /// <param name="createdon"></param>
        /// <returns></returns>
        public static decimal GetPriceHistory(decimal newprice, int pid, out DateTime createdon, int countryId)
        {
            using (SubSonic.DataProviders.SharedDbConnectionScope sdbs = new SubSonic.DataProviders.SharedDbConnectionScope(MultiCountryController.GetDBProvider(countryId)))
            {
                createdon = DateTime.Now;
                decimal price = 0m;
                List<CSK_Store_PriceHistory> his = CSK_Store_PriceHistory.Find(p => p.ProductID == pid).OrderByDescending(p => p.CreatedOn).ToList().Take(2).ToList();
                if (his != null && his.Count > 0)
                {
                    if (newprice != his[0].Price)
                    {
                        price = his[0].Price;
                        createdon = his[0].CreatedOn ?? DateTime.Now;
                    }
                    else if (his.Count > 1)
                    {
                        price = his[1].Price;
                        createdon = his[1].CreatedOn ?? DateTime.Now;
                    }
                }

                return price;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="countryId"></param>
        /// <returns></returns>
        public static List<CSK_Store_RetailerProductNew> GetRetailerProductsByProductId(int productId, int countryId)
        {
            using (SubSonic.DataProviders.SharedDbConnectionScope sdbs = new SubSonic.DataProviders.SharedDbConnectionScope(MultiCountryController.GetDBProvider(countryId)))
            {
                SubSonic.Schema.StoredProcedure sp = new SubSonic.Schema.StoredProcedure("CSK_Store_GetRetailerProductsByProductID");
                sp.Command.AddParameter("@ProductID", productId, System.Data.DbType.Int32);
                return sp.ExecuteTypedList<CSK_Store_RetailerProductNew>();
            }
        }

        public static List<int> GetRetailerIdListByProductId(int productId, int countryId)
        {
            var rpList = GetRetailerProductsByProductId(productId, countryId);
            List<int> retailerIDs = rpList.Select(rp => rp.RetailerId).Distinct().ToList();
            return retailerIDs;
        }

        public static ExpertReviewSource GetReviewSource(int sId, int countryId)
        {
            if (MultiCountryExpertReviewSourcesDic_Static.ContainsKey(countryId) && MultiCountryExpertReviewSourcesDic_Static[countryId].ContainsKey(sId))
            {
                return MultiCountryExpertReviewSourcesDic_Static[countryId][sId];
            }

            return null;
        }

        public static Dictionary<int, ExpertReviewSource> GetAllReviewSources(int countryId)
        {
            if (MultiCountryExpertReviewSourcesDic_Static.ContainsKey(countryId))
            {
                return MultiCountryExpertReviewSourcesDic_Static[countryId];
            }

            return null;
        }

        public static void GetRetailerProducts(int productId, out decimal bestPrice, out decimal maxPrice, out int count, out int RetailerId, out int RetailerProductId, out CSK_Store_RetailerProductNew lowestrp, int countryId)
        {
            bestPrice = 0;
            maxPrice = 0;
            count = 0;
            RetailerId = 0;
            RetailerProductId = 0;
            lowestrp = null;

            List<CSK_Store_RetailerProductNew> rps = GetRetailerProductsByProductId(productId, countryId);
            if (rps == null || rps.Count == 0) return;

            for (int i = 0; i < rps.Count; i++)
            {
                CSK_Store_RetailerProductNew rp = rps[i];

                if (RetailerController.IsPPcRetailer(rp.RetailerId, countryId))
                {
                    rp.IsNoLink = false;
                }
                else
                {
                    rp.PPCMemberType = 5;
                    rp.IsNoLink = true;
                }
            }

            rps = rps.OrderBy(rp => rp.RetailerPrice).ToList();
            List<CSK_Store_RetailerProductNew> ppclowestrps = rps.Where(rp => !rp.IsNoLink).OrderBy(rp => rp.RetailerPrice).ToList();
            if (ppclowestrps != null && ppclowestrps.Count > 0 && ppclowestrps[0].RetailerPrice == rps[0].RetailerPrice)
                lowestrp = ppclowestrps[0];
            else
                lowestrp = rps[0];

            bestPrice = rps.First().RetailerPrice;
            maxPrice = rps.Last().RetailerPrice;
            count = rps.Select(rp => rp.RetailerId).Distinct().Count();
            if (count == 1)
            {
                RetailerId = rps[0].RetailerId;
                RetailerProductId = rps[0].RetailerProductId;
            }
        }

        public static void GetRetailerProducts(int productID, out decimal bestPrice, out decimal maxPrice, out bool singlePrice, out bool showFeatured, out List<CSK_Store_RetailerProductNew> retailerProducts, out int allprice, int flag, out bool isInternational, out decimal overseasPices, int countryId)
        {
            bestPrice = 0;
            maxPrice = 0;
            singlePrice = false;
            showFeatured = false;

            retailerProducts = new List<CSK_Store_RetailerProductNew>();

            List<CSK_Store_RetailerProductNew> rps = GetRetailerProductsByProductId(productID, countryId);

            isInternational = CheckInternationalRetailerProducts(rps, out overseasPices, countryId);

            List<int> restrictedRetailer = new List<int>();

            int hour = DateTime.Now.Hour;
            for (int i = 0; i < rps.Count; i++)
            {
                CSK_Store_RetailerProductNew rp = rps[i];

                if (rp.RetailerId == 531 && rp.ExpirationDate < DateTime.Now)
                {
                    rps.RemoveAt(i);
                    i--;
                    continue;
                }

                rp.ProductId = productID;
                rp.IsFeaturedProduct = false;

                if (RetailerController.IsPPcRetailer(rp.RetailerId, countryId))
                {
                    rp.PPCMemberType = 2;
                    rp.IsNoLink = false;
                }
                else
                {
                    rp.PPCMemberType = 5;
                    rp.IsNoLink = true;
                }
                //如果价格相同PPCMemberType = 2的在前面
                if (!rp.IsNoLink)
                {
                    rp.OrderbyProduct = rp.RetailerPrice - 0.01m;
                    rp.PPCOrderbyProduct = rp.RetailerPrice - 0.01m;
                }
                else
                {
                    rp.OrderbyProduct = rp.RetailerPrice;
                    rp.PPCOrderbyProduct = rp.RetailerPrice;
                }
            }

            rps = rps.OrderBy(rp => rp.OrderbyProduct).ToList();

            //rps = rps.OrderByDescending(rp => rp.IsFeaturedProduct)//临时推广合作
            //    .ToList();

            allprice = rps.Count;

            if (rps.Count > 0)
            {
                singlePrice = rps.Count == 1;

                bestPrice = rps.First().RetailerPrice;
                if (!singlePrice && rps[1].RetailerPrice < bestPrice)
                    bestPrice = rps[1].RetailerPrice;

                maxPrice = rps.Last().RetailerPrice;
                if (!singlePrice && rps[0].RetailerPrice > maxPrice)
                    maxPrice = rps[0].RetailerPrice;
            }

            retailerProducts = rps;
        }

        public static void GetRetailerProductItems(int productID, out decimal bestPrice, out decimal maxPrice, out bool singlePrice, out bool showFeatured, out List<RetailerProductItem> rpis, out int allprice, int flag, out bool isInternational, out decimal overseasPices, int countryId)
        {
            List<CSK_Store_RetailerProductNew> rps;
            GetRetailerProducts(productID, out bestPrice, out maxPrice, out singlePrice, out showFeatured, out rps, out allprice, flag, out isInternational, out overseasPices, countryId);
            if (isInternational)
            {
                List<CSK_Store_RetailerProductNew> priceRps = rps.Where(r => !RetailerController.IsInternationalRetailer(r.RetailerId, countryId)).ToList();
                singlePrice = priceRps.Count == 1;

                if (priceRps.Count > 0)
                {
                    bestPrice = priceRps.First().RetailerPrice;
                    if (priceRps.Count > 1)
                    {
                        if (!singlePrice && priceRps[1].RetailerPrice < bestPrice)
                            bestPrice = priceRps[1].RetailerPrice;
                    }

                    maxPrice = priceRps.Last().RetailerPrice;
                    if (!singlePrice && priceRps[0].RetailerPrice > maxPrice)
                        maxPrice = priceRps[0].RetailerPrice;
                }
            }

            rpis = new List<RetailerProductItem>();
            Dictionary<int, List<CSK_Store_RetailerProductNew>> rpDic = new Dictionary<int, List<CSK_Store_RetailerProductNew>>();

            foreach (CSK_Store_RetailerProductNew rp in rps)
            {
                List<CSK_Store_RetailerProductNew> temps = new List<CSK_Store_RetailerProductNew>();

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

            foreach (KeyValuePair<int, List<CSK_Store_RetailerProductNew>> pair in rpDic)
            {
                RetailerProductItem rpi = new RetailerProductItem();
                rpi.RetailerId = pair.Key;
                rpi.RpList = pair.Value;

                rpis.Add(rpi);
            }
        }

        /// <summary>
        /// 获取所有的网店价格
        /// </summary>
        /// <param name="productID"></param>
        /// <param name="bestPrice"></param>
        /// <param name="maxPrice"></param>
        /// <param name="singlePrice"></param>
        /// <param name="showFeatured"></param>
        /// <param name="rpis"></param>
        /// <param name="allprice"></param>
        /// <param name="flag"></param>
        /// <param name="lowestrp"></param>
        /// <param name="isInternational"></param>
        /// <param name="overseasPices"></param>
        public static void GetRetailerProductItems(int productID, out decimal bestPrice, out decimal maxPrice, out bool singlePrice, out bool showFeatured, out List<RetailerProductItem> rpis, out int allprice, int flag, out bool isInternational, out decimal overseasPices, List<RetailerProductItem> rpisInt, out List<int> retailerids, out List<CSK_Store_RetailerProductNew> rps, int countryId)
        {
            rps = new List<CSK_Store_RetailerProductNew>();
            retailerids = new List<int>();

            ProductController.GetRetailerProducts(productID, out bestPrice, out maxPrice, out singlePrice, out showFeatured, out rps, out allprice, flag, out isInternational, out overseasPices, countryId);

            //if (isInternational)
            //{
            //    List<CSK_Store_RetailerProductNew> priceRps = rps.Where(r => !RetailerController.IsInternationalRetailer(r.RetailerId, countryId)).ToList();
            //    if (priceRps.Count > 0)
            //    {
            //        singlePrice = priceRps.Count == 1;

            //        bestPrice = priceRps.First().RetailerPrice;
            //        if (!singlePrice && priceRps[1].RetailerPrice < bestPrice)
            //            bestPrice = priceRps[1].RetailerPrice;

            //        maxPrice = priceRps.Last().RetailerPrice;
            //        if (!singlePrice && priceRps[0].RetailerPrice > maxPrice)
            //            maxPrice = priceRps[0].RetailerPrice;
            //    }
            //}

            var count531 = rps.Where(w => w.RetailerId == 531).ToList();
            if (count531.Count() > 1)
            {
                rps.RemoveAll(r => r.RetailerId == 531);
                rps.Add(count531.FirstOrDefault());
                rps = rps.OrderBy(o => o.RetailerPrice).ToList();
            }

            rpis = new List<RetailerProductItem>();
            Dictionary<string, List<CSK_Store_RetailerProductNew>> rpDic = new Dictionary<string, List<CSK_Store_RetailerProductNew>>();

            int i = 0;
            foreach (CSK_Store_RetailerProductNew rp in rps)
            {
                List<CSK_Store_RetailerProductNew> temps = new List<CSK_Store_RetailerProductNew>();
                string key = rp.RetailerId.ToString();
                if (rp.RetailerId == 344)
                {
                    key = rp.RetailerId + "_" + i;
                    i++;
                }

                if (rpDic.ContainsKey(key))
                {
                    temps = rpDic[key];
                    temps.Add(rp);
                    rpDic[key] = temps;
                }
                else
                {
                    temps.Add(rp);
                    rpDic.Add(key, temps);
                }
            }

            foreach (KeyValuePair<string, List<CSK_Store_RetailerProductNew>> pair in rpDic)
            {
                int rid = 0;
                if (pair.Key.Contains("_"))
                    int.TryParse(pair.Key.Split('_')[0], out rid);
                else
                    int.TryParse(pair.Key, out rid);

                RetailerProductItem rpi = new RetailerProductItem();
                rpi.RetailerId = rid;
                rpi.RpList = pair.Value;

                if (isInternational && RetailerController.IsInternationalRetailer(rid, countryId))
                    rpisInt.Add(rpi);
                else
                {
                    rpis.Add(rpi);
                    retailerids.Add(rid);
                }
            }
        }

        public static ReviewAverage GetReviewAverage(int productId, int countryId)
        {
            if (MultiContryReviewAverageDic_Static.ContainsKey(countryId) && MultiContryReviewAverageDic_Static[countryId].ContainsKey(productId))
            {
                return MultiContryReviewAverageDic_Static[countryId][productId];
            }

            return null;
        }

        public static ProductRatingEntity GetAverageRating(int productId, int countryId)
        {
            ProductRatingEntity pr = new ProductRatingEntity();
            ReviewAverage ra = GetReviewAverage(productId, countryId);

            if (ra != null)
            {
                pr.ReviewCount = ra.ReviewCount;
                pr.UserReviewCount = ra.UserReviewCount;
                pr.ExpertReviewCount = ra.ExpertReviewCount;
                if (ra.ProductRating > 0)
                    pr.ProductRating = ra.ProductRating;
                pr.FeatureScore = ra.FeatureScore;
                pr.TFERating = ra.TFEAverageRating;
                pr.TFURating = ra.TFUAverageRating;
            }

            return pr;
        }

        public static List<ProductDescAndAttr> GetAllDescriptionAndAttribute(int pId, int countryId)
        {
            List<ProductDescAndAttr> pdas = new List<ProductDescAndAttr>();

            var connectionStr = MultiCountryController.GetDBConnectionString(countryId);
            string sql = "CSK_Product_GetAllDescAndAttr";
            using (SqlConnection conn = new SqlConnection(connectionStr))
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                SqlParameter pm = new SqlParameter("@ProductID", System.Data.SqlDbType.Int);
                pm.Value = pId;
                cmd.Parameters.Add(pm);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;
                conn.Open();
                using (var idr = cmd.ExecuteReader())
                {
                    while (idr.Read())
                    {
                        ProductDescAndAttr pda = new ProductDescAndAttr();
                        string title = idr["Title"] == null ? "" : idr["Title"].ToString().Trim();
                        int t, avs, cid;
                        int.TryParse(idr["T"].ToString(), out t);
                        int.TryParse(idr["AVS"].ToString(), out avs);
                        int.TryParse(idr["CID"].ToString(), out cid);

                        pda.Title = title;
                        pda.T = t;
                        pda.AVS = avs;
                        pda.CID = cid;

                        pdas.Add(pda);
                    }
                }
            }

            return pdas;
        }

        public static string GetEnergyImg(int productId, int countryId)
        {
            if (MultiCountryEnergyImgsDic_Static.ContainsKey(countryId) && MultiCountryEnergyImgsDic_Static[countryId].ContainsKey(productId))
            {
                return MultiCountryEnergyImgsDic_Static[countryId][productId];
            }

            return string.Empty;
        }

        public static string GetProductDescription(int productId, int countryId)
        {
            if (MultiCountryProductDescriptionDic_Static.ContainsKey(countryId) && MultiCountryProductDescriptionDic_Static[countryId].ContainsKey(productId))
            {
                return MultiCountryProductDescriptionDic_Static[countryId][productId];
            }

            return string.Empty;
        }

        public static CSK_Store_RetailerProduct GetRetailerProduct(int retailerProductId, int countryId)
        {
            using (SubSonic.DataProviders.SharedDbConnectionScope sdbs = new SubSonic.DataProviders.SharedDbConnectionScope(MultiCountryController.GetDBProvider(countryId)))
            {
                return CSK_Store_RetailerProduct.SingleOrDefault(rp => rp.RetailerProductId == retailerProductId);
            }
        }

        public static CSK_Store_RetailerProductNew GetRetailerProductNew(int retailerProductId, int countryId)
        {
            using (SubSonic.DataProviders.SharedDbConnectionScope sdbs = new SubSonic.DataProviders.SharedDbConnectionScope(MultiCountryController.GetDBProvider(countryId)))
            {
                return CSK_Store_RetailerProductNew.SingleOrDefault(rp => rp.RetailerProductId == retailerProductId);
            }
        }

        public static decimal GetRetailerProductPrice(int retailerProductId, int countryId)
        {
            var rp = GetRetailerProductNew(retailerProductId, countryId);
            if (rp != null)
            {
                return rp.RetailerPrice;
            }
            return 0;
        }

        public static List<ProductVideo> GetProductVideos(int productId, int countryId)
        {
            if (MultiContryProductVideoDic_Static.ContainsKey(countryId) && MultiContryProductVideoDic_Static[countryId].ContainsKey(productId))
            {
                return MultiContryProductVideoDic_Static[countryId][productId];
            }

            return null;
        }

        public static decimal GetBestPrice(int productId, int countryId)
        {
            using (SubSonic.DataProviders.SharedDbConnectionScope sdbs = new SubSonic.DataProviders.SharedDbConnectionScope(MultiCountryController.GetDBProvider(countryId)))
            {
                decimal result = 0.0M;
                SubSonic.Schema.StoredProcedure sp = new SubSonic.Schema.StoredProcedure("CSK_Store_12RMB_GetBestPriceByProductID");
                sp.Command.AddParameter("@productID", productId, DbType.Int32);
                sp.Command.AddParameter("@CountryId", countryId, DbType.Int32);
                using (IDataReader idr = sp.ExecuteReader())
                {
                    if (idr.Read())
                    {
                        result = string.IsNullOrEmpty(idr[0].ToString()) ? 0.0M : decimal.Parse(idr[0].ToString());
                    }
                }

                return result;
            }
        }

        public static List<HotProduct> GetHotProducts(int countryId)
        {
            if (MultiCountryHotProducts_Static.ContainsKey(countryId))
            {
                return MultiCountryHotProducts_Static[countryId];
            }

            return null;
        }

        public static List<CSK_Store_ProductList> GetProductListByListIDAndProductId(int listID, int productId, int countryId)
        {
            using (SubSonic.DataProviders.SharedDbConnectionScope sdbs = new SubSonic.DataProviders.SharedDbConnectionScope(MultiCountryController.GetDBProvider(countryId)))
            {
                return CSK_Store_ProductList.Find(pl => pl.ListID == listID && pl.ProductID == productId).ToList();
            }
        }

        public static List<CSK_Store_ProductList> GetProductList(int listId, int countryId)
        {
            using (SubSonic.DataProviders.SharedDbConnectionScope sdbs = new SubSonic.DataProviders.SharedDbConnectionScope(MultiCountryController.GetDBProvider(countryId)))
            {
                return CSK_Store_ProductList.Find(pl => pl.ListID == listId).ToList();
            }
        }

        public static List<PriceDropItem> GetPriceDropItems(string parseId, int countryId, decimal configChangePrice)
        {
            List<PriceDropItem> priceDropItems = new List<PriceDropItem>();

            List<int> productIdList = new List<int>();
            List<Data.ProductCatalog> pcList = new List<Data.ProductCatalog>();

            List<FavouriteProductData> fProducts = FavoritesController.GetUserFavouriteProductData(countryId, parseId).ToList();
            List<FavouritesPageData> fPages = FavoritesController.GetUserFavouriteCatalogPages(countryId, parseId).ToList();

            foreach (var fp in fPages)
            {
                int cid = int.Parse(fp.PageId);
                ProductSearcher productSeacher = new ProductSearcher("", cid, null, null, null, null, "Clicks", null, SearchController.MaxSearchCount_Static, countryId, false, true, false, true, null, "", null);
                int pCount = productSeacher.GetProductCount();

                SearchResult searchResult = productSeacher.GetSearchResult(1, pCount);

                List<Data.ProductCatalog> rsList = searchResult.ProductCatalogList.Where(c => !CategoryController.IsSearchOnly(c.CategoryID, countryId)).ToList();

                foreach (var pc in rsList)
                {
                    int pid = int.Parse(pc.ProductID);
                    if (!productIdList.Contains(pid))
                    {
                        productIdList.Add(pid);
                        pcList.Add(pc);
                    }
                }
            }

            int[] pidArray = fProducts.Select(fp => fp.ProductId).ToArray();
            foreach (var fp in fProducts)
            {
                List<Data.ProductCatalog> list = SearchController.SearchProductsByPIDs(pidArray, countryId);
                foreach (var pc in list)
                {
                    int pid = int.Parse(pc.ProductID);
                    if (!productIdList.Contains(pid))
                    {
                        productIdList.Add(pid);

                        pcList.Add(pc);
                    }
                }
            }

            foreach (var pc in pcList)
            {
                int pid = int.Parse(pc.ProductID);
                decimal price = 0;
                decimal.TryParse(pc.BestPrice, out price);
                DateTime createdon = DateTime.Now;
                decimal oldprice = ProductController.GetPriceHistory(price, pid, out createdon, countryId);
                if (oldprice == 0) continue;

                decimal changeprice = price - oldprice;
                if (changeprice == 0 || changeprice < configChangePrice) continue;

                PriceDropItem pdi = new PriceDropItem();
                pdi.ProductId = pid;
                pdi.ProductName = pc.ProductName;
                pdi.ChangeDate = createdon;
                pdi.CurrentPrice = decimal.Parse(pc.BestPPCPrice);
                pdi.ImageUrl = pc.DefaultImage;
                pdi.HasAlert = false;
                pdi.LastChange = changeprice;
                priceDropItems.Add(pdi);

                if (priceDropItems.Count > 49)
                {
                    break;
                }
            }

            return priceDropItems;
        }

        public static void GetTop5PPCRetailerProducts(int productID, out List<CSK_Store_RetailerProductNew> retailerProducts, out decimal bestPrice, out decimal maxPrice, out int retailerCount, out int RetailerId, out int RetailerProductId, out CSK_Store_RetailerProductNew lowestrp, out bool isInternational, out decimal overseasPices, int countryId)
        {
            bestPrice = 0;
            maxPrice = 0;
            retailerCount = 0;
            RetailerId = 0;
            RetailerProductId = 0;
            lowestrp = null;
            isInternational = false;
            overseasPices = 0;

            retailerProducts = new List<CSK_Store_RetailerProductNew>();

            List<CSK_Store_RetailerProductNew> rps = GetRetailerProductsByProductId(productID, countryId);

            if (rps != null && rps.Count > 0)
            {
                isInternational = CheckInternationalRetailerProducts(rps, out overseasPices, countryId);

                List<int> restrictedRetailer = new List<int>();

                int hour = DateTime.Now.Hour;
                for (int i = 0; i < rps.Count; i++)
                {
                    CSK_Store_RetailerProductNew rp = rps[i];

                    rp.ProductId = productID;
                    rp.IsFeaturedProduct = false;
                    if (RetailerController.IsPPcRetailer(rp.RetailerId, countryId))
                    {
                        rp.PPCMemberType = 2;
                        rp.IsNoLink = false;
                    }
                    else
                    {
                        rp.PPCMemberType = 5;
                        rp.IsNoLink = true;
                    }

                    if (rp.PPCMemberType == 2)
                        rp.OrderbyProduct = rp.RetailerPrice - 0.01m;
                    else
                        rp.OrderbyProduct = rp.RetailerPrice;
                }

                if (isInternational)
                    rps = rps.Where(rp => !RetailerController.IsInternationalRetailer(rp.RetailerId, countryId)).ToList();

                rps = rps.OrderBy(rp => rp.RetailerPrice).ToList();
                List<CSK_Store_RetailerProductNew> ppclowestrps = rps.Where(rp => !rp.IsNoLink).OrderBy(rp => rp.RetailerPrice).ToList();
                if (ppclowestrps != null && ppclowestrps.Count > 0 && ppclowestrps[0].RetailerPrice == rps[0].RetailerPrice)
                    lowestrp = ppclowestrps[0];
                else
                    lowestrp = rps[0];

                bestPrice = rps.First().RetailerPrice;
                maxPrice = rps.Last().RetailerPrice;
                retailerCount = rps.Select(rp => rp.RetailerId).Distinct().Count();
                if (retailerCount == 1)
                {
                    RetailerId = rps[0].RetailerId;
                    RetailerProductId = rps[0].RetailerProductId;
                }

                rps = rps.OrderBy(rp => rp.OrderbyProduct).Where(rp => rp.IsNoLink == false).Take(5).ToList();
            }

            retailerProducts = rps;
        }

        public static bool HaveProductMap(int productId, int countryId)
        {
            int count = 0;

            List<RetailerProductItem> rpis = new List<RetailerProductItem>();
            decimal bestPrice, maxPrice; bool singlePrice, featuredProduct; int allprice; bool isInternational; decimal overseasPices;
            GetRetailerProductItems(productId, out bestPrice, out maxPrice, out singlePrice, out featuredProduct, out rpis, out allprice, 0, out isInternational, out overseasPices, countryId);

            foreach (RetailerProductItem rp in rpis)
            {
                if (rp.RetailerId == 0) continue;


                RetailerCache retailer = RetailerController.GetRetailerDeep(rp.RetailerId, countryId);
                if (retailer == null || retailer.RetailerId == 0) continue;
                bool isShow = true;

                if (isShow == false) continue;

                var glats = RetailerController.GetRetailerGLatLng(rp.RetailerId, countryId);
                if (glats == null || glats.Count == 0) continue;
                count++;
            }

            if (count > 0) return true;
            else return false;
        }

        public static bool HaveProductMap(int productid, List<RetailerProductItem> rpis, int countryId)
        {
            int count = 0;
            if (rpis.Count <= 0)
                rpis = new List<RetailerProductItem>();

            foreach (RetailerProductItem rp in rpis)
            {
                if (rp.RetailerId == 0) continue;

                RetailerCache retailer = RetailerController.GetRetailerDeep(rp.RetailerId, countryId);
                if (retailer == null || retailer.RetailerId == 0) continue;
                bool isShow = true;

                if (isShow == false) continue;

                var glats = RetailerController.GetRetailerGLatLng(rp.RetailerId, countryId);
                if (glats == null || glats.Count == 0) continue;
                count++;
            }

            if (count > 0) return true;
            else return false;
        }

        public static List<Data.ProductCatalog> GetRelatedProductsByCategoryAndBrand(int cId, int mId, int countryId)
        {
            //TODO 把内容从 Application 移到 静态变量里
            Dictionary<string, List<PriceMeCommon.Data.ProductCatalog>> similarCaches = new Dictionary<string, List<Data.ProductCatalog>>();

            CategoryCache category = CategoryController.GetCategoryByCategoryID(cId, countryId);
            List<Data.ProductCatalog> pcc = new List<Data.ProductCatalog>();
            List<int> cidList = new List<int>();
            if (cId > 0)
            {
                cidList.Add(cId);
            }
            List<int> brandIDs = new List<int>();
            if (mId > 0)
            {
                brandIDs.Add(mId);
            }
            HitsInfo hi = SearchController.SearchProducts("", cidList, brandIDs, null, null, null, "", null, 1000, countryId, false, true, false, null, true, null, "");
            if (hi == null) return pcc;

            int count = hi.ResultCount > 30 ? 30 : hi.ResultCount;

            for (int i = 0; i < count; i++)
            {
                Data.ProductCatalog pc = SearchController.GetProductCatalog(hi, i);
                pcc.Add(pc);
            }

            return pcc;
        }

        public static List<CSK_Store_RetailerProductNew> GetTopNRetailerProductsByCategoryId(int cid, int countryId, int count)
        {
            return GetTopNRetailerProductsByCategoryId(cid, countryId, count, PriceMeStatic.Provider);
        }

        public static List<CSK_Store_RetailerProductNew> GetTopNRetailerProductsByCategoryId(int cid, int countryId, int count, IFormatProvider provider)
        {
            RetailerProductSearcher retailerProductSearcher = new RetailerProductSearcher(0, cid, countryId);
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
                rp.RetailerPrice = decimal.Parse(rpc.RetailerPrice, System.Globalization.NumberStyles.Any, provider);
                rp.CCFeeAmount = 0;
                rp.Freight = -1;
                int rpcon = 0;
                int.TryParse(rpc.RetailerProductCondition, out rpcon);
                rp.RetailerProductCondition = rpcon;
                rps.Add(rp);
            }

            return rps;
        }

        public static List<CSK_Store_PriceHistory> GetPriceHistoryData(List<int> listpId, int countryId)
        {
            List<CSK_Store_PriceHistory> phs = new List<CSK_Store_PriceHistory>();
            string pids = string.Empty;
            foreach (int pid in listpId)
            {
                pids += pid + ",";
            }
            pids = pids.Substring(0, pids.LastIndexOf(','));
            string connectionStr = MultiCountryController.GetDBConnectionString(countryId);
            var sql = "select ProductID, PriceDate, Price, CreatedOn from CSK_Store_PriceHistory where productid in (" + pids + ")";
            using (SqlConnection conn = new SqlConnection(connectionStr))
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                conn.Open();
                cmd.CommandTimeout = 0;

                IDataReader dr = cmd.ExecuteReader();
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
            }

            return phs;
        }

        public static int GetRedirectForRetailerProduct(int rpId, int countryId)
        {
            int pid = 0;

            string connectionStr = MultiCountryController.GetDBConnectionString(countryId);
            var sql = "Select ProductId from Csk_store_RedirectforRetailerproduct Where RetailerProductId = " + rpId;
            using (SqlConnection conn = new SqlConnection(connectionStr))
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                conn.Open();
                cmd.CommandTimeout = 0;

                using (IDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        pid = dr.GetInt32(0);
                    }
                }
            }

            return pid;
        }

        public static CSK_Store_ProductNew GetGenerationProduct(int productId, int countryId)
        {
            if (IntraLinkingGenerationsDic_Static.ContainsKey(productId))
            {
                return GetProductNew(IntraLinkingGenerationsDic_Static[productId], countryId);
            }
            return null;
        }

        public static List<RelatedPartsData> GetRelatedParts(int pid, int countryId)
        {
            List<RelatedPartsData> datas = new List<RelatedPartsData>();

            string connectionStr = MultiCountryController.GetDBConnectionString(countryId);
            var sql = "Select * from csk_store_RelatedParts Where ProductId = " + pid;
            using (SqlConnection conn = new SqlConnection(connectionStr))
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                conn.Open();
                cmd.CommandTimeout = 0;

                using (IDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        int id = 0, spid = 0;
                        int.TryParse(dr["Id"].ToString(), out id);
                        int.TryParse(dr["ShopProductId"].ToString(), out spid);

                        RelatedPartsData data = new RelatedPartsData();
                        data.Id = id;
                        data.ProductId = pid;
                        data.ShopProductId = spid;

                        datas.Add(data);
                    }
                    dr.Close();
                }
            }

            return datas;
        }

        public static List<RelatedPartsData> GetRelatedProduct(int pid, int countryId)
        {
            List<RelatedPartsData> datas = new List<RelatedPartsData>();

            string connectionStr = MultiCountryController.CommonConnectionStringSettings_Static.ConnectionString;
            var sql = "Select* From RelatedProductWithScore Where ProductId = " + pid
                    + " And countryid = " + countryId + " order by Score desc";
            using (SqlConnection conn = new SqlConnection(connectionStr))
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                conn.Open();
                cmd.CommandTimeout = 0;

                using (IDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        int id = 0, spid = 0;
                        int.TryParse(dr["ID"].ToString(), out id);
                        int.TryParse(dr["RelatedProductId"].ToString(), out spid);

                        RelatedPartsData data = new RelatedPartsData();
                        data.Id = id;
                        data.ProductId = pid;
                        data.ShopProductId = spid;

                        datas.Add(data);
                    }
                    dr.Close();
                }
            }

            return datas;
        }
    }

    #region class ProductRatingEntity
    public class ProductRatingEntity
    {
        private int _ReviewCount;
        private int _UserReviewCount;
        private int _ExpertReviewCount;
        private double _productRating;
        private string _FeatureScore;
        private double _TFERating;

        public double TFERating
        {
            get { return _TFERating; }
            set { _TFERating = value; }
        }
        private double _TFURating;

        public double TFURating
        {
            get { return _TFURating; }
            set { _TFURating = value; }
        }

        public string FeatureScore
        {
            get { return _FeatureScore; }
            set { _FeatureScore = value; }
        }

        public int ReviewCount
        {
            get { return _ReviewCount; }
            set { _ReviewCount = value; }
        }

        public int UserReviewCount
        {
            get { return _UserReviewCount; }
            set { _UserReviewCount = value; }
        }

        public int ExpertReviewCount
        {
            get { return _ExpertReviewCount; }
            set { _ExpertReviewCount = value; }
        }

        public double ProductRating
        {
            get { return _productRating; }
            set { _productRating = value; }
        }

        public ProductRatingEntity()
        {
            //
            // TODO: Add constructor logic here
            //
        }
    }

    #endregion

    #region ProductAttributesATag
    /// <summary>
    ///  ProductController 在 PriceMeCommon 项目下面， UrlController 在网站项目下面，这里无法计算 Url 的值，所以，把 URL删除。
    /// </summary>
    public class ProductAttributesATag
    {
        int categoryID;
        string url;
        string attributeValueName;
        string title;
        int valueID;
        bool isManufacturer = false;
        bool isAttributeRange;

        public int CategoryID
        {
            get { return categoryID; }
            set { categoryID = value; }
        }

        public int ValueID
        {
            get { return valueID; }
            set { valueID = value; }
        }

        public bool IsAttributeRange
        {
            get { return isAttributeRange; }
            set { isAttributeRange = value; }
        }

        public string Url
        {
            get { return url; }
            set { url = value; }
        }

        public string AttributeValueName
        {
            get { return attributeValueName; }
            set { attributeValueName = value; }
        }

        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        public bool IsManufacturer
        {
            get { return isManufacturer; }
            set { isManufacturer = value; }
        }
    }
    #endregion
}