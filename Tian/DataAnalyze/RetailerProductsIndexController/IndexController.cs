using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Configuration;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Search;
using System.Linq;
using PriceMeDBA;
using System.IO;

namespace RetailerProductsIndexController
{
    public static class IndexController
    {
        private static readonly object obj = new object();

        static Lucene.Net.Index.IndexWriter Static_IndexWriter;
        public static string DirectoryPath
        {
            get
            {
                string path = "";

                path = ConfigurationManager.AppSettings["RPIndexPath"];
                path += "\\" + DateTime.Now.ToString("yyyyMMddHH");
                if (!System.IO.Directory.Exists(path))
                {
                    System.IO.Directory.CreateDirectory(path);
                }

                return path;
            }
        }

        static Lucene.Net.Search.IndexSearcher indexSearcher = null;
        static Lucene.Net.Index.IndexWriter indexModifier = null;
        static bool buildIndexFinished = false;
        static int MAXDOCS = 100000;
        static Sort sort = new Sort();
        static Dictionary<int, TermQuery> rQuerDic = new Dictionary<int, TermQuery>();

        private static string _currentPath = "";

        public static bool BuildIndexFinished
        {
            get { return buildIndexFinished; }
        }

        //public static void Load(int a)
        //{
        //    Sort sort = new Sort();
        //}

        static IndexController()
        {

        }

        /// <summary>
        /// 简单测试简单做。做更新和修改Index前需要调用一次这个方法
        /// </summary>
        public static void InitUpdate()
        {
            Lucene.Net.Store.FSDirectory fsDir = Lucene.Net.Store.FSDirectory.Open(new System.IO.DirectoryInfo(DirectoryPath));
            Lucene.Net.Analysis.Analyzer analyzer = new Lucene.Net.Analysis.Standard.StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_29);
            Static_IndexWriter = new Lucene.Net.Index.IndexWriter(fsDir, analyzer, false, Lucene.Net.Index.IndexWriter.MaxFieldLength.UNLIMITED);
        }

        /// <summary>
        /// 插入新的Doc
        /// </summary>
        /// <param name="retailerProductIndexEntiy"></param>
        /// <returns></returns>
        public static bool InsertIndex(RetailerProductIndexEntiy_New retailerProductIndexEntiy, bool reload)
        {
            Document doc = GetDocument_New(retailerProductIndexEntiy);
            Static_IndexWriter.AddDocument(doc);
            Static_IndexWriter.Commit();

            if (reload)
            {
                indexSearcher = new IndexSearcher(Static_IndexWriter.GetReader());
            }
            return true;
        }

        public static void InsertIndex(CSK_Store_RetailerProduct info, int cid)
        {
            RetailerProductIndex index = new RetailerProductIndex();
            index.CategoryID = cid;
            index.IsDeleted = info.IsDeleted;
            index.ModifiedOn = info.Modifiedon;
            index.ProductID = info.ProductId;
            index.PurchaseURL = info.PurchaseURL.ToLower();
            index.PurchaseURLOrg = info.PurchaseURL;
            index.RetailerID = info.RetailerId;
            index.RetailerPrice = info.RetailerPrice;
            index.RetailerProductID = info.RetailerProductId;
            index.RetailerProductName = info.RetailerProductName;
            index.RetailerProductNameMatch = info.RetailerProductName.ToLower();
            index.RetailerProductSKU = info.RetailerProductSKU;
            index.RetailerProductUIC = info.RetailerProductUIC;

            index.Save();
        }

        /// <summary>
        /// 更新已存在的Doc
        /// </summary>
        /// <param name="rpID"></param>
        /// <param name="retailerProductIndexEntiy"></param>
        /// <returns></returns>
        public static bool UpdateIndex(int rpID, RetailerProductIndexEntiy_New retailerProductIndexEntiy, bool reload)
        {
            Document doc = GetDocument_New(retailerProductIndexEntiy);
            Lucene.Net.Index.Term term = new Term("RetailerProductID", rpID.ToString());
            Static_IndexWriter.UpdateDocument(term, doc);
            Static_IndexWriter.Commit();
            if (reload)
            {
                indexSearcher = new IndexSearcher(Static_IndexWriter.GetReader());
            }
            return true;
        }

        public static void UpdateIndex(CSK_Store_RetailerProduct info)
        {
            RetailerProductIndex index = RetailerProductIndex.SingleOrDefault(item => item.RetailerProductID == info.RetailerProductId);
            if (index == null) index = new RetailerProductIndex();

            index.IsDeleted = info.IsDeleted;
            index.ModifiedOn = info.Modifiedon;
            index.ProductID = info.ProductId;
            index.PurchaseURL = info.PurchaseURL.ToLower();
            index.PurchaseURLOrg = info.PurchaseURL;
            index.RetailerID = info.RetailerId;
            index.RetailerPrice = info.RetailerPrice;
            index.RetailerProductID = info.RetailerProductId;
            index.RetailerProductName = info.RetailerProductName;
            index.RetailerProductNameMatch = info.RetailerProductName.ToLower();
            index.RetailerProductSKU = info.RetailerProductSKU;
            index.RetailerProductUIC = info.RetailerProductUIC;

            index.Save();
        }

        /// <summary>
        /// 应用更新后的Index
        /// </summary>
        public static void ApplyUpdatedIndex()
        {
            indexSearcher = new IndexSearcher(Static_IndexWriter.GetReader());
        }

        public static bool Load()
        {
            lock (obj)
            {
                string tempPath = ConfigurationManager.AppSettings["RPIndexPath"];
                var dirs = System.IO.Directory.GetDirectories(tempPath).ToList();
                dirs = dirs.OrderByDescending(item => Convert.ToInt32(Path.GetFileName(item))).ToList();

                if (dirs.Count == 0) return false;
                if (indexSearcher != null && _currentPath == dirs[0]) return true;


                if (indexSearcher != null)
                {
                    indexSearcher.Close();
                    indexSearcher = null;
                }

                bool isIndex = false;

                foreach (var dir in dirs)
                {
                    try
                    {
                        Lucene.Net.Store.FSDirectory fsDirectory = Lucene.Net.Store.FSDirectory.Open(new System.IO.DirectoryInfo(dir));
                        indexSearcher = new Lucene.Net.Search.IndexSearcher(fsDirectory, true);
                        if (indexSearcher.MaxDoc() > 0)
                        {
                            _currentPath = dir;
                            isIndex = true;
                            break;
                        }
                    }
                    catch { }
                }

                return isIndex;
            }
        }

        public static void BuildIndex()
        {
            lock (obj)
            {

                Console.WriteLine("Start building retailerProduct index...");
                string connectionString = ConfigurationManager.ConnectionStrings["Pricealyser"].ConnectionString;

                bool isBuilt = false;
                string sql1 = " delete from RetailerProductIndex"; //删除数据库临时Index表

                string sql = "";
                sql += " select";
                sql += " RP.RetailerProductId as RetailerProductId,";
                sql += " RP.RetailerId as RetailerId,";
                sql += " RP.ProductId as ProductId,";
                sql += " RP.RetailerProductName as RetailerProductName,";
                sql += " RP.RetailerPrice as RetailerPrice,";
                sql += " RP.Stock as Stock,";
                sql += " RP.ModifiedOn as ModifiedOn,";
                sql += " PCM.CategoryID as CategoryID,";
                sql += " PCM.IsMerge as IsMerge,";
                sql += " PCM.ProductName as ProductName,";
                sql += " RP.PurchaseURL as PurchaseURL,";
                sql += " RP.RetailerProductStatus as [Status],";
                sql += " RP.IsDeleted as IsDeleted,";
                sql += " RP.RetailerProductUIC as RetailerProductUIC,";
                sql += " RP.RetailerProductSKU as RetailerProductSKU";
                sql += " from dbo.CSK_Store_RetailerProduct as RP";
                sql += " inner join dbo.CSK_Store_Product as PCM on RP.ProductId = PCM.ProductId and RP.ModifiedOn > dateadd(day,-365,getdate())";
                sql += " order by RP.RetailerProductStatus desc,RP.ModifiedOn desc";

                for (int i = 0; i < 3; i++)
                {
                    try
                    {
                        using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                        {
                            sqlConnection.Open();
                            SqlCommand sqlCMD = new SqlCommand();
                            sqlCMD.CommandText = sql;
                            sqlCMD.CommandType = System.Data.CommandType.Text;
                            sqlCMD.Connection = sqlConnection;
                            sqlCMD.CommandTimeout = 0;

                            System.Data.Common.DbDataReader dr = sqlCMD.ExecuteReader(System.Data.CommandBehavior.CloseConnection);

                            WriteIndex(dr, DirectoryPath);
                            sqlCMD.Dispose();
                            buildIndexFinished = true;
                            isBuilt = true;
                        }
                        break;
                    }
                    catch (Exception ex) { Console.WriteLine(ex.Message + "  " + ex.StackTrace); System.Threading.Thread.Sleep(180000); }
                }

                if (isBuilt)
                {
                    using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                    {
                        sqlConnection.Open();
                        SqlCommand sqlCMD = new SqlCommand();
                        sqlCMD.CommandText = sql1;
                        sqlCMD.CommandType = System.Data.CommandType.Text;
                        sqlCMD.Connection = sqlConnection;
                        sqlCMD.CommandTimeout = 0;

                        sqlCMD.ExecuteNonQuery();
                    }

                    //重新读取IndexSearch
                    Load();
                }

                Console.WriteLine("Build finished...");
            }
        }

        //public static void BuildIndex()
        //{
        //    Console.WriteLine("Start building retailerProduct index...");
        //    string countryId = System.Configuration.ConfigurationManager.AppSettings["CountryId"].ToString();
        //    string connectionString = ConfigurationManager.ConnectionStrings["Pricealyser"].ConnectionString;
        //    for (int i = 0; i < 3; i++)
        //    {
        //        try
        //        {
        //            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
        //            {
        //                sqlConnection.Open();
        //                SqlCommand sqlCMD = new SqlCommand();
        //                sqlCMD.CommandText = "CSK_Store_12RMB_Index_GetRetailerProductsByCountryId";
        //                SqlParameter sp = new SqlParameter("@CountryId", countryId);
        //                sqlCMD.Parameters.Add(sp);
        //                sqlCMD.CommandType = System.Data.CommandType.StoredProcedure;
        //                sqlCMD.Connection = sqlConnection;
        //                sqlCMD.CommandTimeout = 0;

        //                System.Data.Common.DbDataReader dr = sqlCMD.ExecuteReader(System.Data.CommandBehavior.CloseConnection);

        //                WriteIndex(dr);
        //                sqlCMD.Dispose();
        //                buildIndexFinished = true;
        //            }
        //            break;
        //        }
        //        catch (Exception ex) { Console.WriteLine(ex.Message + "  " + ex.StackTrace); System.Threading.Thread.Sleep(180000); }
        //    }

        //    Console.WriteLine("Build finished...");
        //}

        public static void BuildRetailerProductInfoIndex(int retailerID)
        {
            Console.WriteLine("Start building RetailerProductInfo index...");
            string retailerProductInfoPath = ConfigurationManager.AppSettings["RetailerProductInfoPath"] + retailerID;
            string connectionString = ConfigurationManager.ConnectionStrings["Pricealyser"].ConnectionString;

            Lucene.Net.Index.IndexWriter indexWriter = CreateIndexWriter(retailerProductInfoPath);

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                SqlCommand sqlCMD = new SqlCommand();
                sqlCMD.CommandText = "CSK_Store_12RMB_Index_GetProductsByRID";
                sqlCMD.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCMD.Connection = sqlConnection;
                SqlParameter parameter = new SqlParameter("@rid", System.Data.SqlDbType.Int);
                parameter.Value = retailerID;
                sqlCMD.Parameters.Add(parameter);
                sqlCMD.CommandTimeout = 0;

                System.Data.Common.DbDataReader idr = sqlCMD.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                int count = 0;
                while (idr.Read())
                {
                    string CategoryRank = idr["CategoryRank"].ToString();
                    string productId = idr["ProductID"].ToString();
                    string productName = idr["ProductName"].ToString();
                    string manufacturerPartNumber = idr["ManufacturerPartNumber"].ToString();
                    string manufacturerID = idr["ManufacturerID"].ToString();
                    string categoryId = idr["CategoryID"].ToString();
                    string retailerAmount = idr["RetailerAmount"].ToString();
                    string retailerCount = idr["RetailerCount"].ToString();
                    string isAccessory = string.Empty;
                    string isMerge = idr["IsMerge"].ToString();
                    float BestPrice = float.Parse(idr["BestPrice"].ToString());
                    float MaxPrice = float.Parse(idr["MaxPrice"].ToString());
                    string defaultImage = idr["DefaultImage"].ToString();
                    string shortDescriptionZH = idr["ShortDescriptionZH"].ToString();
                    string productRatingVotes = idr["ProductRatingVotes"].ToString();
                    //string productGuid = idr["ProductGuid"].ToString();
                    //string SKU = idr["SKU"].ToString();
                    string productRatingSum = idr["ProductRatingSum"].ToString();
                    string clicks = string.IsNullOrEmpty(idr["Clicks"] == null ? "" : idr["Clicks"].ToString()) ? "0" : idr["Clicks"].ToString();
                    string createdOn = string.IsNullOrEmpty(idr["CreatedOn"] == null ? "" : idr["CreatedOn"].ToString()) ? "1999-01-01" : idr["createdOn"].ToString();
                    string bestRetailerName = idr["BestRetailerName"].ToString();
                    string retailerProductID = idr["RetailerProductID"].ToString();
                    string reviewCount = idr["ProductReviewCount"].ToString();
                    string keywords = idr["keywords"] == null ? "" : idr["keywords"].ToString();
                    float ppcIndex = (idr["PPCIndex"] == null || idr["PPCIndex"].ToString() == string.Empty) ? 0 : float.Parse(idr["PPCIndex"].ToString());
                    string PPCLogoPath = idr["PPCLogoPath"].ToString();
                    string PPCRetailerProductID = idr["PPCRetailerProductID"].ToString();
                    string PPCLogo = idr["PPCLogo"].ToString();
                    string RetailerProductList = idr["RetailerProductList"].ToString();
                    string isAccssories = idr["IsAccessories"].ToString();

                    //string PrevPrice = idr["PrevPrice"].ToString();
                    //float DropPrice = 0;
                    //if (string.IsNullOrEmpty(PrevPrice) || PrevPrice == "0")
                    //{
                    //    PrevPrice = "0";
                    //}
                    //else
                    //{
                    //    float bp = BestPrice;
                    //    float pp = float.Parse(PrevPrice);
                    //    if (pp > bp)
                    //    {
                    //        DropPrice = float.Parse(((bp - pp) / pp).ToString("0.00"));
                    //    }
                    //}
                    productRatingSum = string.IsNullOrEmpty(productRatingSum) ? "0" : productRatingSum;
                    productRatingVotes = string.IsNullOrEmpty(productRatingVotes) ? "0" : productRatingVotes;
                    double avRating = Utility.GetAverageRating(int.Parse(productRatingSum), int.Parse(productRatingVotes), int.Parse(productId));

                    RetailerProductIndexEntiy retailerProductIndexEntiy = new RetailerProductIndexEntiy();
                    if (isAccssories.ToLower() == "true" || isAccssories.ToLower() == "1")
                    {
                        retailerProductIndexEntiy.IsAccessories = "1";
                    }
                    else
                    {
                        retailerProductIndexEntiy.IsAccessories = "0";
                    }
                    retailerProductIndexEntiy.AvRating = avRating;
                    retailerProductIndexEntiy.BestPrice = BestPrice;
                    retailerProductIndexEntiy.BestRetailerName = bestRetailerName;
                    retailerProductIndexEntiy.CategoryID = categoryId;
                    retailerProductIndexEntiy.CategoryRank = CategoryRank;
                    retailerProductIndexEntiy.Clicks = clicks;
                    retailerProductIndexEntiy.CreatedOn = createdOn;
                    retailerProductIndexEntiy.DefaultImage = defaultImage;
                    retailerProductIndexEntiy.IsMerge = isMerge;
                    retailerProductIndexEntiy.Keywords = keywords;
                    retailerProductIndexEntiy.ManufacturerID = manufacturerID;
                    retailerProductIndexEntiy.ManufacturerPartNumber = manufacturerPartNumber;
                    retailerProductIndexEntiy.PPCLogo = PPCLogo;
                    retailerProductIndexEntiy.PPCLogoPath = PPCLogoPath;
                    retailerProductIndexEntiy.PPCRetailerProductID = PPCRetailerProductID;
                    retailerProductIndexEntiy.ProductId = productId;
                    retailerProductIndexEntiy.ProductName = productName;
                    retailerProductIndexEntiy.ProductRatingSum = productRatingSum;
                    retailerProductIndexEntiy.ProductRatingVotes = productRatingVotes;
                    retailerProductIndexEntiy.ProductReviewCount = reviewCount;
                    retailerProductIndexEntiy.RetailerAmount = retailerAmount;
                    retailerProductIndexEntiy.RetailerCount = retailerCount;
                    retailerProductIndexEntiy.RetailerProductID = retailerProductID;
                    retailerProductIndexEntiy.RetailerProductList = RetailerProductList;
                    retailerProductIndexEntiy.ShortDescriptionZH = shortDescriptionZH;

                    Document doc = GetDocument(retailerProductIndexEntiy);

                    while (true)
                    {
                        try
                        {
                            indexWriter.AddDocument(doc);
                            break;
                        }
                        catch
                        {
                        }
                    }
                    count++;
                }
                while (true)
                {
                    try
                    {
                        indexWriter.Optimize();
                        break;
                    }
                    catch
                    {
                    }
                }
                idr.Close();
                indexWriter.Close();

                sqlCMD.Dispose();
            }
            buildIndexFinished = true;

            Console.WriteLine("Build finished...");
        }

        private static Document GetDocument(RetailerProductIndexEntiy retailerProductIndexEntiy)
        {
            Document doc = new Document();

            doc.Add(new Field("SearchField", retailerProductIndexEntiy.ProductName + " " + retailerProductIndexEntiy.Keywords, Field.Store.YES, Field.Index.ANALYZED, Field.TermVector.YES));
            doc.Add(new Field("CategoryRank", retailerProductIndexEntiy.CategoryRank, Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field("ProductID", retailerProductIndexEntiy.ProductId, Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field("ProductName", retailerProductIndexEntiy.ProductName, Field.Store.YES, Field.Index.ANALYZED, Field.TermVector.YES));
            doc.Add(new Field("ManufacturerPartNumber", retailerProductIndexEntiy.ManufacturerPartNumber, Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field("ManufacturerID", retailerProductIndexEntiy.ManufacturerID, Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field("CategoryID", retailerProductIndexEntiy.CategoryID, Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field("RetailerAmount", retailerProductIndexEntiy.RetailerAmount, Field.Store.YES, Field.Index.NO));
            doc.Add(new Field("RetailerCount", retailerProductIndexEntiy.RetailerCount, Field.Store.YES, Field.Index.NO));
            doc.Add(new Field("IsAccessory", retailerProductIndexEntiy.IsAccessories, Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field("IsMerge", retailerProductIndexEntiy.IsMerge, Field.Store.YES, Field.Index.NOT_ANALYZED));

            NumericField bestPriceField = new NumericField("BestPrice", Field.Store.YES, true);
            bestPriceField.SetFloatValue(retailerProductIndexEntiy.BestPrice);
            doc.Add(bestPriceField);

            //NumericField maxPriceField = new NumericField("BestPrice", Field.Store.YES, true);
            //bestPriceField.SetFloatValue(MaxPrice);
            //doc.Add(maxPriceField);

            doc.Add(new Field("DefaultImage", retailerProductIndexEntiy.DefaultImage, Field.Store.YES, Field.Index.NO));
            retailerProductIndexEntiy.ShortDescriptionZH = Utility.GetString(retailerProductIndexEntiy.ShortDescriptionZH, 200);
            doc.Add(new Field("ShortDescriptionZH", retailerProductIndexEntiy.ShortDescriptionZH, Field.Store.YES, Field.Index.NO));
            doc.Add(new Field("ProductRatingVotes", retailerProductIndexEntiy.ProductRatingVotes, Field.Store.YES, Field.Index.NO));
            doc.Add(new Field("Title", retailerProductIndexEntiy.ProductName, Field.Store.YES, Field.Index.NOT_ANALYZED));

            doc.Add(new Field("ProductRatingSum", retailerProductIndexEntiy.ProductRatingSum, Field.Store.YES, Field.Index.NO));
            doc.Add(new Field("Clicks", retailerProductIndexEntiy.Clicks, Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field("CreatedOn", retailerProductIndexEntiy.CreatedOn, Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field("BestRetailerName", retailerProductIndexEntiy.BestRetailerName, Field.Store.YES, Field.Index.NO));
            doc.Add(new Field("RetailerProductID", retailerProductIndexEntiy.RetailerProductID, Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field("ProductReviewCount", retailerProductIndexEntiy.ProductReviewCount, Field.Store.YES, Field.Index.NO));
            //NumericField prevPriceField = new NumericField("BestPrice", Field.Store.YES, true);
            //bestPriceField.SetFloatValue(float.Parse(PrevPrice));
            //doc.Add(prevPriceField);

            //NumericField dropPriceField = new NumericField("BestPrice", Field.Store.YES, true);
            //bestPriceField.SetFloatValue(DropPrice);
            //doc.Add(dropPriceField);

            //NumericField ppcIndexField = new NumericField("BestPrice", Field.Store.YES, true);
            //bestPriceField.SetFloatValue(ppcIndex);
            //doc.Add(ppcIndexField);

            doc.Add(new Field("AvRating", retailerProductIndexEntiy.AvRating.ToString("0.0"), Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field("keywords", retailerProductIndexEntiy.Keywords, Field.Store.YES, Field.Index.ANALYZED));
            doc.Add(new Field("PPCLogoPath", retailerProductIndexEntiy.PPCLogoPath, Field.Store.YES, Field.Index.NO));
            doc.Add(new Field("PPCRetailerProductID", retailerProductIndexEntiy.PPCRetailerProductID, Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field("PPCLogo", retailerProductIndexEntiy.PPCLogo, Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field("RetailerProductList", retailerProductIndexEntiy.RetailerProductList, Field.Store.YES, Field.Index.NO));

            float boost = 0.1f;
            if (retailerProductIndexEntiy.IsMerge.ToLower() == "true" || retailerProductIndexEntiy.IsMerge.ToLower() == "1")
            {
                boost = 2 * boost;
            }
            boost += float.Parse(retailerProductIndexEntiy.Clicks) / 0.1f;
            doc.SetBoost(boost);

            return doc;
        }

        private static Lucene.Net.Index.IndexWriter CreateIndexWriter(string indexPath)
        {
            Lucene.Net.Analysis.Standard.StandardAnalyzer standardAnalyzer = new Lucene.Net.Analysis.Standard.StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_29);
            Lucene.Net.Store.FSDirectory fsDirectory = Lucene.Net.Store.FSDirectory.Open(new System.IO.DirectoryInfo(indexPath));
            Lucene.Net.Index.IndexWriter indexWriter = new Lucene.Net.Index.IndexWriter(fsDirectory, standardAnalyzer, true, Lucene.Net.Index.IndexWriter.MaxFieldLength.UNLIMITED);
            indexWriter.SetMergeFactor(5000);
            indexWriter.SetMaxBufferedDocs(4000);
            return indexWriter;
        }

        private static void WriteIndex(System.Data.Common.DbDataReader dr, string path)
        {
            Lucene.Net.Index.IndexWriter indexWriter = CreateIndexWriter(path);

            while (dr.Read())
            {
                string RetailerProductID = dr["RetailerProductId"].ToString();
                string RetailerProductName = dr["RetailerProductName"].ToString();
                string RetailerProductNameMatch = RetailerProductName.ToLower();
                string RetailerPrice = dr["RetailerPrice"].ToString();
                string Stock = dr["Stock"].ToString();
                string ModifiedOn = dr["ModifiedOn"].ToString();
                string CategoryID = dr["CategoryID"].ToString();
                string RetailerID = dr["RetailerID"].ToString();
                string PurchaseURLOrg = dr["PurchaseURL"].ToString();
                string PurchaseURL = dr["PurchaseURL"].ToString().ToLower();
                string ProductID = dr["ProductId"].ToString();
                string Status = dr["Status"].ToString();
                string IsDeleted = dr["IsDeleted"].ToString();
                string RetailerProductSKU = dr["RetailerProductSKU"].ToString().ToLower();
                string RetailerProductUIC = dr["RetailerProductUIC"].ToString().ToLower();
                //string ProductName = dr["ProductName"].ToString().ToLower();
                //string UPC = dr["UPC"].ToString().ToLower();
                //string MPN = dr["MPN"].ToString().ToLower();
                string IsMerge = dr["IsMerge"].ToString();
                string ProductName = dr["ProductName"].ToString();

                RetailerProductIndexEntiy_New retailerProductIndexEntiy_New = new RetailerProductIndexEntiy_New();
                retailerProductIndexEntiy_New.CategoryID = CategoryID;
                retailerProductIndexEntiy_New.IsDeleted = IsDeleted;
                retailerProductIndexEntiy_New.ModifiedOn = ModifiedOn;
                retailerProductIndexEntiy_New.ProductID = ProductID;
                retailerProductIndexEntiy_New.PurchaseURL = PurchaseURL;
                retailerProductIndexEntiy_New.PurchaseURLOrg = PurchaseURLOrg;
                retailerProductIndexEntiy_New.RetailerID = RetailerID;
                retailerProductIndexEntiy_New.RetailerPrice = RetailerPrice;
                retailerProductIndexEntiy_New.RetailerProductID = RetailerProductID;
                retailerProductIndexEntiy_New.RetailerProductName = RetailerProductName;
                retailerProductIndexEntiy_New.RetailerProductNameMatch = RetailerProductNameMatch;
                retailerProductIndexEntiy_New.RetailerProductSKU = RetailerProductSKU;
                retailerProductIndexEntiy_New.RetailerProductUIC = RetailerProductUIC;
                retailerProductIndexEntiy_New.Status = Status;
                retailerProductIndexEntiy_New.Stock = Stock;
                retailerProductIndexEntiy_New.IsMerge = IsMerge;
                retailerProductIndexEntiy_New.ProductName = ProductName;


                Lucene.Net.Documents.Document document = GetDocument_New(retailerProductIndexEntiy_New);

                indexWriter.AddDocument(document);
            }
            dr.Close();

            indexWriter.Optimize();
            indexWriter.Close();
        }

        private static Document GetDocument_New(RetailerProductIndexEntiy_New retailerProductIndexEntiy_New)
        {
            Document document = new Document();

            document.Add(new Field("RetailerProductID", retailerProductIndexEntiy_New.RetailerProductID, Lucene.Net.Documents.Field.Store.YES, Lucene.Net.Documents.Field.Index.NOT_ANALYZED));
            document.Add(new Field("RetailerProductName", retailerProductIndexEntiy_New.RetailerProductName, Lucene.Net.Documents.Field.Store.YES, Lucene.Net.Documents.Field.Index.NOT_ANALYZED));
            document.Add(new Field("RetailerProductNameMatch", retailerProductIndexEntiy_New.RetailerProductNameMatch, Lucene.Net.Documents.Field.Store.YES, Lucene.Net.Documents.Field.Index.NOT_ANALYZED));
            document.Add(new Field("RetailerPrice", retailerProductIndexEntiy_New.RetailerPrice, Lucene.Net.Documents.Field.Store.YES, Lucene.Net.Documents.Field.Index.NOT_ANALYZED));
            document.Add(new Field("Stock", retailerProductIndexEntiy_New.Stock, Lucene.Net.Documents.Field.Store.YES, Lucene.Net.Documents.Field.Index.NOT_ANALYZED));
            document.Add(new Field("ModifiedOn", retailerProductIndexEntiy_New.ModifiedOn, Lucene.Net.Documents.Field.Store.YES, Lucene.Net.Documents.Field.Index.NOT_ANALYZED));
            document.Add(new Field("CategoryID", retailerProductIndexEntiy_New.CategoryID, Lucene.Net.Documents.Field.Store.YES, Lucene.Net.Documents.Field.Index.NOT_ANALYZED));
            document.Add(new Field("RetailerID", retailerProductIndexEntiy_New.RetailerID, Lucene.Net.Documents.Field.Store.YES, Lucene.Net.Documents.Field.Index.NOT_ANALYZED));
            document.Add(new Field("PurchaseURL", retailerProductIndexEntiy_New.PurchaseURL, Lucene.Net.Documents.Field.Store.YES, Lucene.Net.Documents.Field.Index.NOT_ANALYZED));
            document.Add(new Field("PurchaseURLOrg", retailerProductIndexEntiy_New.PurchaseURLOrg, Lucene.Net.Documents.Field.Store.YES, Lucene.Net.Documents.Field.Index.NOT_ANALYZED));
            document.Add(new Field("ProductID", retailerProductIndexEntiy_New.ProductID, Lucene.Net.Documents.Field.Store.YES, Lucene.Net.Documents.Field.Index.NOT_ANALYZED));
            document.Add(new Field("Status", retailerProductIndexEntiy_New.Status, Lucene.Net.Documents.Field.Store.YES, Lucene.Net.Documents.Field.Index.NOT_ANALYZED));
            document.Add(new Field("IsDeleted", retailerProductIndexEntiy_New.IsDeleted, Lucene.Net.Documents.Field.Store.YES, Lucene.Net.Documents.Field.Index.NOT_ANALYZED));
            document.Add(new Field("RetailerProductSKU", retailerProductIndexEntiy_New.RetailerProductSKU, Lucene.Net.Documents.Field.Store.YES, Lucene.Net.Documents.Field.Index.NOT_ANALYZED));
            document.Add(new Field("RetailerProductUIC", retailerProductIndexEntiy_New.RetailerProductUIC, Lucene.Net.Documents.Field.Store.YES, Lucene.Net.Documents.Field.Index.NOT_ANALYZED));
            document.Add(new Field("IsMerge", retailerProductIndexEntiy_New.IsMerge, Lucene.Net.Documents.Field.Store.YES, Lucene.Net.Documents.Field.Index.NOT_ANALYZED));
            document.Add(new Field("ProductName", retailerProductIndexEntiy_New.ProductName, Lucene.Net.Documents.Field.Store.YES, Lucene.Net.Documents.Field.Index.NOT_ANALYZED));
            //document.Add(new Field("ProductName", ProductName, Lucene.Net.Documents.Field.Store.YES, Lucene.Net.Documents.Field.Index.NOT_ANALYZED, Lucene.Net.Documents.Field.TermVector.YES));
            //document.Add(new Field("UPC", UPC, Lucene.Net.Documents.Field.Store.YES, Lucene.Net.Documents.Field.Index.NOT_ANALYZED, Lucene.Net.Documents.Field.TermVector.YES));
            //document.Add(new Field("MPN", MPN, Lucene.Net.Documents.Field.Store.YES, Lucene.Net.Documents.Field.Index.NOT_ANALYZED, Lucene.Net.Documents.Field.TermVector.YES));

            return document;
        }

        //public static List<RetailerProduct> SearchIndex(string retailerProductName, string retailerID)
        //{
        //    return SearchIndex(retailerProductName, "", retailerID);
        //}

        public static void LoopIndex(Action<RetailerProduct> action)
        {
            var count = indexSearcher.MaxDoc();

            for (int i = 0; i < count; i++)
            {
                Document doc = indexSearcher.Doc(i);
                RetailerProduct rp = new RetailerProduct();
                rp.RetailerProductID = doc.Get("RetailerProductID");
                rp.IsDeleted = doc.Get("IsDeleted");
                rp.RetailerPrice = doc.Get("RetailerPrice");
                rp.ProductID = doc.Get("ProductID");
                rp.ProductName = doc.Get("ProductName");
                rp.CategoryID = doc.Get("CategoryID");
                rp.RetailerProductName = doc.Get("RetailerProductName");
                rp.IsMerge = doc.Get("IsMerge");
                rp.ManufacturerID = doc.Get("ManufacturerID");

                action(rp);
            }
        }

        public static List<RetailerProduct> SearchIndex(string MatchName, string MatchNameValue)
        {
            lock (obj)
            {
                List<RetailerProduct> retailerProducts = new List<RetailerProduct>();

                if (indexSearcher == null)
                {
                    return retailerProducts;

                    Lucene.Net.Store.FSDirectory fsDirectory = Lucene.Net.Store.FSDirectory.Open(new System.IO.DirectoryInfo(DirectoryPath));
                    indexSearcher = new Lucene.Net.Search.IndexSearcher(fsDirectory, true);
                }

                MatchNameValue = MatchNameValue.ToLower();
                TopFieldDocs docs = null;

                BooleanQuery query = new BooleanQuery();

                //TermQuery rQuery = GetRetailerQuery(rid);

                TermQuery termQuery = new TermQuery(new Lucene.Net.Index.Term(MatchName, MatchNameValue));
                //query.Add(rQuery, BooleanClause.Occur.MUST);
                query.Add(termQuery, BooleanClause.Occur.MUST);

                docs = indexSearcher.Search(query, null, MAXDOCS, sort);



                for (int i = 0; i < docs.scoreDocs.Length; i++)
                {
                    RetailerProduct rp = new RetailerProduct();
                    Document doc = indexSearcher.Doc(docs.scoreDocs[i].doc);
                    rp.RetailerProductID = doc.Get("RetailerProductID");
                    rp.IsDeleted = doc.Get("IsDeleted");
                    rp.RetailerPrice = doc.Get("RetailerPrice");
                    rp.ProductID = doc.Get("ProductID");
                    rp.ProductName = doc.Get("ProductName");
                    rp.CategoryID = doc.Get("CategoryID");
                    rp.RetailerProductName = doc.Get("RetailerProductName");
                    rp.IsMerge = doc.Get("IsMerge");

                    retailerProducts.Add(rp);
                }

                //从数据库里面查
                //if (retailerProducts.Count == 0)
                //{
                //    IList<RetailerProductIndex> indexList = null;

                //    switch (MatchName)
                //    {
                //        case "RetailerProductNameMatch": indexList = RetailerProductIndex.Find(item => item.RetailerID == rid && item.RetailerProductNameMatch == MatchNameValue).ToList(); break;
                //        case "RetailerProductUIC": indexList = RetailerProductIndex.Find(item => item.RetailerID == rid && item.RetailerProductUIC == MatchNameValue).ToList(); break;
                //        case "RetailerProductSKU": indexList = RetailerProductIndex.Find(item => item.RetailerID == rid && item.RetailerProductSKU == MatchNameValue).ToList(); break;
                //        case "PurchaseURL": indexList = RetailerProductIndex.Find(item => item.RetailerID == rid && item.PurchaseURL == MatchNameValue).ToList(); break;
                //    }

                //    for (int i = 0; i < indexList.Count; i++)
                //    {
                //        RetailerProduct rp = new RetailerProduct();
                //        rp.RetailerProductID = indexList[i].RetailerProductID.ToString();
                //        rp.IsDeleted = indexList[i].IsDeleted.ToString();
                //        retailerProducts.Add(rp);
                //    }
                //}

                return retailerProducts;
            }
        }

        //public static List<RetailerProduct> SearchIndex(string retailerProductName, string categoryID, string retailerID)
        //{
        //    if (indexSearcher == null)
        //    {
        //        Lucene.Net.Store.FSDirectory fsDirectory = Lucene.Net.Store.FSDirectory.Open(new System.IO.DirectoryInfo(DirectoryPath));
        //        indexSearcher = new Lucene.Net.Search.IndexSearcher(fsDirectory, true);
        //    }

        //    BooleanQuery query = new BooleanQuery();
        //    TopFieldDocs docs = null;
        //    Sort sort = new Sort();

        //    TermQuery nameQuery = new TermQuery(new Lucene.Net.Index.Term("RetailerProductName", retailerProductName));
        //    query.Add(nameQuery, BooleanClause.Occur.MUST);

        //    TermQuery retailerIDQuery = new TermQuery(new Lucene.Net.Index.Term("RetailerID", retailerID));
        //    query.Add(retailerIDQuery, BooleanClause.Occur.MUST);

        //    if (!string.IsNullOrEmpty(categoryID) && categoryID != "0")
        //    {
        //        TermQuery categoryIDQuery = new TermQuery(new Lucene.Net.Index.Term("CategoryID", categoryID));
        //        query.Add(categoryIDQuery, BooleanClause.Occur.MUST);
        //    }

        //    docs = indexSearcher.Search(query, null, MAXDOCS, sort);

        //    List<RetailerProduct> retailerProducts = new List<RetailerProduct>();
        //    for (int i = 0; i < docs.scoreDocs.Length; i++)
        //    {
        //        RetailerProduct rp = new RetailerProduct();
        //        Lucene.Net.Documents.Document doc = indexSearcher.Doc(docs.scoreDocs[i].doc);
        //        rp.CategoryID = doc.Get("CategoryID");
        //        rp.RetailerID = doc.Get("RetailerID");
        //        rp.ModifiedOn = doc.Get("ModifiedOn");
        //        rp.RetailerPrice = doc.Get("RetailerPrice");
        //        rp.RetailerProductID = doc.Get("RetailerProductID");
        //        rp.RetailerProductName = doc.Get("RetailerProductName");
        //        rp.Stock = doc.Get("Stock");
        //        rp.PurchaseURL = doc.Get("PurchaseURL");
        //        rp.IsDeleted = doc.Get("IsDeleted");
        //        retailerProducts.Add(rp);
        //    }
        //    return retailerProducts;
        //}

        public static List<RetailerProduct> GetRetailerProductByRetailerId(int rid)
        {
            if (indexSearcher == null)
            {
                Lucene.Net.Store.FSDirectory fsDirectory = Lucene.Net.Store.FSDirectory.Open(new System.IO.DirectoryInfo(DirectoryPath));
                indexSearcher = new Lucene.Net.Search.IndexSearcher(fsDirectory, true);
            }

            TopFieldDocs docs = null;

            BooleanQuery query = new BooleanQuery();

            TermQuery rQuery = GetRetailerQuery(rid);

            TermQuery termQuery = new TermQuery(new Lucene.Net.Index.Term("Status", "True"));
            query.Add(rQuery, BooleanClause.Occur.MUST);
            query.Add(termQuery, BooleanClause.Occur.MUST);

            docs = indexSearcher.Search(query, null, MAXDOCS, sort);

            List<RetailerProduct> retailerProducts = new List<RetailerProduct>();
            for (int i = 0; i < docs.scoreDocs.Length; i++)
            {
                RetailerProduct rp = new RetailerProduct();
                Document doc = indexSearcher.Doc(docs.scoreDocs[i].doc);
                rp.RetailerProductID = doc.Get("RetailerProductID");
                rp.RetailerProductName = doc.Get("RetailerProductName");
                rp.PurchaseURL = doc.Get("PurchaseURL");
                retailerProducts.Add(rp);
            }
            return retailerProducts;
        }

        public static void UpdateIndex(RetailerProduct retailerProduct)
        {
            lock (new object())
            {
                if (indexModifier == null)
                    indexModifier = CreateIndexWriter(DirectoryPath);

                indexModifier.DeleteDocuments(new Lucene.Net.Index.Term("RetailerProductID", retailerProduct.RetailerProductID));

                Lucene.Net.Documents.Document document = new Lucene.Net.Documents.Document();
                document.Add(new Field("RetailerProductID", retailerProduct.RetailerProductID, Lucene.Net.Documents.Field.Store.YES, Lucene.Net.Documents.Field.Index.NOT_ANALYZED, Lucene.Net.Documents.Field.TermVector.NO));
                document.Add(new Field("RetailerProductName", retailerProduct.RetailerProductName, Lucene.Net.Documents.Field.Store.YES, Lucene.Net.Documents.Field.Index.NOT_ANALYZED, Lucene.Net.Documents.Field.TermVector.YES));
                document.Add(new Field("RetailerPrice", retailerProduct.RetailerPrice, Lucene.Net.Documents.Field.Store.YES, Lucene.Net.Documents.Field.Index.NOT_ANALYZED, Lucene.Net.Documents.Field.TermVector.YES));
                document.Add(new Field("Stock", retailerProduct.Stock, Lucene.Net.Documents.Field.Store.YES, Lucene.Net.Documents.Field.Index.NOT_ANALYZED, Lucene.Net.Documents.Field.TermVector.YES));
                document.Add(new Field("ModifiedOn", retailerProduct.ModifiedOn, Lucene.Net.Documents.Field.Store.YES, Lucene.Net.Documents.Field.Index.NOT_ANALYZED, Lucene.Net.Documents.Field.TermVector.YES));
                document.Add(new Field("CategoryID", retailerProduct.CategoryID, Lucene.Net.Documents.Field.Store.YES, Lucene.Net.Documents.Field.Index.NOT_ANALYZED, Lucene.Net.Documents.Field.TermVector.YES));
                document.Add(new Field("RetailerID", retailerProduct.RetailerID, Lucene.Net.Documents.Field.Store.YES, Lucene.Net.Documents.Field.Index.NOT_ANALYZED, Lucene.Net.Documents.Field.TermVector.YES));
                indexModifier.AddDocument(document);
            }
        }

        public static void DeleteDocument(string retailerProductID)
        {
            Lucene.Net.Index.IndexWriter indexWriter = CreateIndexWriter(DirectoryPath);
            indexWriter.DeleteDocuments(new Lucene.Net.Index.Term("RetailerProductID", retailerProductID));
        }

        public static void Optimize()
        {
            if (indexModifier != null)
            {
                indexModifier.Optimize();
                indexModifier.Close();
            }
        }

        public static void Close()
        {
            if (indexSearcher != null)
            {
                indexSearcher.Close();
            }
        }

        private static TermQuery GetRetailerQuery(int rid)
        {
            if (rQuerDic.ContainsKey(rid))
                return rQuerDic[rid];
            else
            {
                TermQuery rQuery = new TermQuery(new Lucene.Net.Index.Term("RetailerID", rid.ToString()));
                rQuerDic.Add(rid, rQuery);
                return rQuery;
            }
        }
    }
}
