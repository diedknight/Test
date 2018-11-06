﻿using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Util;
using ProductSearchIndexBuilder.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.IO;

namespace ProductSearchIndexBuilder
{
    public static class IndexBuilder
    {
        static bool AllProductCategoryIndexIsOk = true;
        static Dictionary<int, int> manufacturerHasProductDic = null;
        static List<string> hiddenManufacturerCategoryIDList = null;

        static Dictionary<int, float> CurrenciesInfo = new Dictionary<int, float>();

        public static IndexSpeedLog indexSpeedLog = new IndexSpeedLog();

        public static Dictionary<string, Dictionary<int, string>> Static_ProductAttrValueList = new Dictionary<string, Dictionary<int, string>>();
        static Dictionary<string, int> Exp156CountDic = new Dictionary<string, int>();

        static IndexBuilder()
        {
            string[] currenciesInfos = AppValue.Currencies.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string currencieInfo in currenciesInfos)
            {
                string[] cis = currencieInfo.Split(':');
                CurrenciesInfo.Add(int.Parse(cis[0]), float.Parse(cis[1]));
            }
        }

        public static bool AsyncBuildAllCategoryProductsIndex(string path, int countryId, bool useCreatedOn, int maxThread, DbInfo priceme205DbInfo, DbInfo pamUserInfo)
        {
            AllProductCategoryIndexIsOk = true;
            Exp156CountDic = GetExp156CountDic(priceme205DbInfo);

            List<Amib.Threading.IWorkItemResult> itemresultList = new List<Amib.Threading.IWorkItemResult>();

            maxThread = 1;//为多次重试造Index临时修改

            Amib.Threading.SmartThreadPool smartThreadPool = new Amib.Threading.SmartThreadPool(0, maxThread, 1);

            Amib.Threading.WorkItemCallback workItem = new Amib.Threading.WorkItemCallback(BuildCategoryProductIndexTask);

            var rootCategoryList = DataController.GetAllRootCategoriesOrderByName();

            hiddenManufacturerCategoryIDList = new List<string>();
            string[] hiddenManufacturerCategoryIDArray = AppValue.HiddenManufacturerCategoryIDs.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string id in hiddenManufacturerCategoryIDArray)
            {
                int cid = 0;
                if (int.TryParse(id, out cid))
                {
                    List<int> subCategoryIDs = DataController.GetAllSubCategoryId(cid);
                    foreach (var scId in subCategoryIDs)
                    {
                        hiddenManufacturerCategoryIDList.Add(scId.ToString());
                    }
                }
            }
            //manufacturerHasProduct = new List<string>();
            manufacturerHasProductDic = new Dictionary<int, int>();

            Dictionary<int, List<ProductCatalog>> upComingProductDic;
            if (countryId == 25)
            {
                upComingProductDic = new Dictionary<int, List<ProductCatalog>>();
            }
            else
            {
                upComingProductDic = GetUpComingProductDic(priceme205DbInfo, pamUserInfo);
            }

            Dictionary<int, decimal> prePriceDic = GetProductPrevPriceDic(AppValue.PrevPriceDay, priceme205DbInfo);

            if (rootCategoryList.Count == 0)
            {
                LogController.WriteLog("Error : RootCategoryList Count is 0");
                return false;
            }

            foreach (CategoryCache category in rootCategoryList)
            {
                try
                {
                    CategoryProductIndexParameter cpp = new CategoryProductIndexParameter();
                    cpp.Path = path;
                    cpp.Category = category;
                    cpp.CountryID = countryId;
                    cpp.UseCreatedOn = useCreatedOn;
                    cpp.UpComingProductDic = upComingProductDic;
                    cpp.PrevPriceDic = prePriceDic;
                    cpp.PamUserDbInfo = pamUserInfo;
                    cpp.PriceMe205DbInfo = priceme205DbInfo;
                    itemresultList.Add(smartThreadPool.QueueWorkItem(workItem, cpp));
                }
                catch (Exception ex)
                {
                    LogController.WriteException(ex.Message + "\t " + ex.StackTrace);
                    return false;
                }
            }

            Amib.Threading.IWorkItemResult[] itemresultArray = itemresultList.ToArray();

            bool success = Amib.Threading.SmartThreadPool.WaitAll(itemresultArray);
            if (!success)
                return success;

            MergeProductIndex(path, AppValue.NotCopy);

            BuildAllBrandsIndex(path);

            return AllProductCategoryIndexIsOk;
        }

        private static Dictionary<string, int> GetExp156CountDic(DbInfo priceme205DbInfo)
        {
            Dictionary<string, int> dic = new Dictionary<string, int>();

            string sql = "SELECT ProductID, count(ProductID) c FROM CSK_Store_ExpertReviewAU where SourceID = 156 group by ProductID";

            using (var sqlConn = DBController.CreateDBConnection(priceme205DbInfo))
            {
                sqlConn.Open();
                using (var sqlCMD = DBController.CreateDbCommand(sql, sqlConn))
                {
                    using (var sqlDR = sqlCMD.ExecuteReader())
                    {
                        while (sqlDR.Read())
                        {
                            dic.Add(sqlDR.GetInt32(0).ToString(), sqlDR.GetInt32(1));
                        }
                    }
                }
            }
            return dic;
        }

        public static Dictionary<int, decimal> GetProductPrevPriceDic(int prevPriceDay, DbInfo priceme205DbInfo)
        {
            LogController.WriteLog("Start read DB for GetProductPrevPriceDic on " + DateTime.Now.ToLocalTime().ToString("yyyyMMdd hh:mm:ss"));

            Dictionary<int, decimal> dic = new Dictionary<int, decimal>();

            string sql = "";

            using (var sqlConn = DBController.CreateDBConnection(priceme205DbInfo))
            {
                if (sqlConn is MySql.Data.MySqlClient.MySqlConnection)
                {
                    sql = "select ProductId, min(price) as MinPrice from CSK_Store_PriceHistory where createdon >= date_add(Now(),  interval -" + prevPriceDay + " day) and RetailerID in (select retailerid from CSK_Store_Retailer where RetailerCountry = " + AppValue.CountryId + ") group by ProductId";
                }
                else
                {
                    sql = "select ProductId, min(price) as MinPrice from CSK_Store_PriceHistory where createdon >= dateadd(day, -" + prevPriceDay + ", getdate()) and RetailerID in (select retailerid from CSK_Store_Retailer where RetailerCountry = " + AppValue.CountryId + ") group by ProductId";
                }

                using (var sqlCMD = DBController.CreateDbCommand(sql, sqlConn))
                {
                    sqlConn.Open();
                    using (var sqlDR = sqlCMD.ExecuteReader())
                    {
                        while (sqlDR.Read())
                        {
                            int pid = sqlDR.GetInt32(0);
                            decimal prevPrice = sqlDR.GetDecimal(1);
                            dic.Add(pid, prevPrice);
                        }
                    }
                }
            }

            LogController.WriteLog("ProvPrice Products Count: " + dic.Count);

            return dic;
        }

        public static Dictionary<int, List<ProductCatalog>> GetUpComingProductDic(DbInfo priceme205DbInfo, DbInfo pamUserDbInfo)
        {
            LogController.WriteLog("Start read DB for GetUpComingProductDic on " + DateTime.Now.ToLocalTime().ToString("yyyyMMdd hh:mm:ss"));

            Dictionary<int, List<ProductCatalog>> ucpDic = new Dictionary<int, List<ProductCatalog>>();
            Dictionary<int, int> excludePidDic = new Dictionary<int, int>();
            Dictionary<int, ProductCatalog> includePidDic = new Dictionary<int, ProductCatalog>();

            using (var sqlConn = DBController.CreateDBConnection(priceme205DbInfo))
            {
                string sql = "SELECT distinct ProductID from CSK_Store_ProductIsMerged";

                using (var sqlCMD = DBController.CreateDbCommand(sql, sqlConn))
                {
                    sqlConn.Open();
                    using (var sdr = sqlCMD.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            excludePidDic.Add(sdr.GetInt32(0), 0);
                        }
                    }
                }

                sql = @"SELECT ProductID
                        ,ProductName
                        ,CSK_Store_Manufacturer.ManufacturerID
                        ,CSK_Store_Manufacturer.ManufacturerName
                        ,CategoryID
                        ,CatalogDescription
                        ,DefaultImage
                        FROM CSK_Store_Product
                        inner join CSK_Store_Manufacturer on CSK_Store_Manufacturer.ManufacturerID = CSK_Store_Product.ManufacturerID
                        where ProductID not in 
                        (select ProductID from CSK_Store_RetailerProduct where retailerId in (select retailerId from CSK_Store_Retailer where RetailerCountry = " + AppValue.CountryId + "))";

                using (var sqlCMD = DBController.CreateDbCommand(sql, sqlConn))
                {
                    sqlCMD.CommandTimeout = 0;
                    using (var sdr = sqlCMD.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            ProductCatalog pc = new ProductCatalog();
                            int pid = sdr.GetInt32(0);
                            pc.ProductID = pid.ToString();
                            pc.ProductName = sdr.GetString(1);
                            pc.ManufacturerID = sdr.GetInt32(2).ToString();
                            pc.RetailerProductInfoString = sdr.GetString(3);//暂时用来保存ManufacturerName
                            pc.CategoryID = sdr.GetInt32(4);
                            pc.ShortDescriptionZH = sdr["CatalogDescription"].ToString(); //sdr.GetString(5);
                            pc.DefaultImage = sdr["DefaultImage"].ToString();//sdr.GetString(6);
                            includePidDic.Add(pid, pc);
                        }
                    }
                }
            }

            using (var sqlConn = DBController.CreateDBConnection(pamUserDbInfo))
            {
                string sql = "";
                if (sqlConn is MySql.Data.MySqlClient.MySqlConnection)
                {
                    sql = "select productid from UpcomingProduct where ReleaseDate > Now()";
                }
                else
                {
                    sql = "select productid from UpcomingProduct where ReleaseDate > GETDATE()";
                }
                using (var sqlCMD = DBController.CreateDbCommand(sql, sqlConn))
                {
                    sqlConn.Open();
                    using (var sdr = sqlCMD.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            int pid = sdr.GetInt32(0);
                            if (excludePidDic.ContainsKey(pid) || !includePidDic.ContainsKey(pid))
                                continue;

                            ProductCatalog pc = includePidDic[pid];
                            int parentID = GetRootCategoryID(pc.CategoryID, priceme205DbInfo);
                            if (ucpDic.ContainsKey(parentID))
                            {
                                ucpDic[parentID].Add(pc);
                            }
                            else
                            {
                                List<ProductCatalog> pcList = new List<ProductCatalog>();
                                pcList.Add(pc);
                                ucpDic.Add(parentID, pcList);
                            }

                        }
                    }
                }
            }

            return ucpDic;
        }

        private static int GetRootCategoryID(int categoryId, DbInfo priceme205DbInfo)
        {
            using (var sqlConn = DBController.CreateDBConnection(priceme205DbInfo))
            {
                string sql = "select dbo.GetParentCategory(" + categoryId + ")";

                using (var sqlCMD = DBController.CreateDbCommand(sql, sqlConn))
                {
                    sqlConn.Open();
                    using (var sdr = sqlCMD.ExecuteReader())
                    {
                        if (sdr.Read())
                        {
                            int prantID = sdr.GetInt32(0);
                            return prantID;
                        }
                    }
                }
            }

            return 0;
        }

        //private static int GetParentID(int categoryID)
        //{
        //    string connectionString2 = System.Configuration.ConfigurationManager.ConnectionStrings["CommerceTemplate"].ConnectionString;
        //    using (SqlConnection sqlConnection = new SqlConnection(connectionString2))
        //    {
        //        using (SqlCommand sqlCMD = new SqlCommand("select [dbo].[GetParentCategory](" + categoryID + ")", sqlConnection))
        //        {
        //            sqlConnection.Open();
        //            sqlCMD.CommandTimeout = 0;
        //            using (SqlDataReader sdr = sqlCMD.ExecuteReader())
        //            {
        //                if (sdr.Read())
        //                {
        //                    int prantID = sdr.GetInt32(0);
        //                    return prantID;
        //                }
        //            }
        //        }
        //    }
        //    return 0;
        //}

        public static object BuildCategoryProductIndexTask(object parameters)
        {
            CategoryProductIndexParameter p = parameters as CategoryProductIndexParameter;
            List<ProductCatalog> pcList = null;
            if (p.UpComingProductDic.ContainsKey(p.Category.CategoryID))
            {
                pcList = p.UpComingProductDic[p.Category.CategoryID];
                LogController.WriteLog("Cid:" + p.Category.CategoryID + " Upcoming Products Count: " + pcList.Count);
            }
            else
            {
                LogController.WriteLog("Cid:" + p.Category.CategoryID + " no Upcoming Products");
            }
            int i = BuildCategoryProductIndex(p.Path, p.Category, p.CountryID, p.UseCreatedOn, true, pcList, p.PrevPriceDic, p.PriceMe205DbInfo);
            return i;
        }

        private static void BuildAllBrandsIndex(string path)
        {
            LogController.WriteLog("Begin BuildAllBrandsIndex index on " + DateTime.Now.ToLocalTime().ToString("yyyyMMdd hh:mm:ss"));

            using (IndexWriter indexWriter = CreateIndexWriter(path + "AllBrands"))
            {
                try
                {
                    foreach (int mid in manufacturerHasProductDic.Keys)
                    {
                        Document doc = new Document();

                        var midField = new Int32Field("ManufacturerID", mid, Field.Store.YES);
                        var midProductCountField = new Int32Field("ProductCount", manufacturerHasProductDic[mid], Field.Store.YES);

                        doc.Add(midField);
                        doc.Add(midProductCountField);

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
                    }
                    indexWriter.Commit();
                }
                catch (Exception ex)
                {
                    LogController.WriteException(ex.Message + ex.StackTrace);
                }
            }
            LogController.WriteLog("End BuildAllBrandsIndex index on " + DateTime.Now.ToLocalTime().ToString("yyyyMMdd hh:mm:ss"));
        }

        public static void MergeProductIndex(string path, string notCopyFlag)
        {
            LogController.WriteLog("Begin MergeProductIndex index on " + DateTime.Now.ToLocalTime().ToString("yyyyMMdd hh:mm:ss"));

            string[] dirs = Directory.GetDirectories(path);

            if (!string.IsNullOrEmpty(notCopyFlag))
            {
                List<string> dirList = new List<string>();
                dirList.AddRange(dirs);

                for (int i = 0; i < dirList.Count;)
                {
                    if (dirList[i].EndsWith(notCopyFlag) || dirList[i].EndsWith(@"\Products") || dirList[i].EndsWith(@"\AllBrands"))
                    {
                        dirList.RemoveAt(i);
                    }
                    else
                    {
                        i++;
                    }
                }

                dirs = dirList.ToArray();
            }

            Lucene.Net.Store.FSDirectory[] fsDirs = new Lucene.Net.Store.FSDirectory[dirs.Length];
            for (int i = 0; i < fsDirs.Length; i++)
            {
                Lucene.Net.Store.FSDirectory fsDirectory = Lucene.Net.Store.FSDirectory.Open(new System.IO.DirectoryInfo(dirs[i]));
                fsDirs[i] = fsDirectory;
            }

            IndexReader[] idrs = new IndexReader[fsDirs.Length];
            for (int i = 0; i < fsDirs.Length; i++)
            {
                IndexReader idr = DirectoryReader.Open(fsDirs[i]);
                idrs[i] = idr;
            }

            using (IndexWriter indexWriter = CreateIndexWriter(path + "Products"))
            {
                //indexWriter.AddIndexes(fsDirs);
                indexWriter.AddIndexes(idrs);
                indexWriter.Commit();
            }

            foreach (Lucene.Net.Store.FSDirectory fs in fsDirs)
            {
                fs.Dispose();
            }

            LogController.WriteLog("End MergeProductIndex index on " + DateTime.Now.ToLocalTime().ToString("yyyyMMdd hh:mm:ss"));
        }

        public static bool MergeNotCopyProductsIndex(string path, string notCopyFlag)
        {
            LogController.WriteLog("Begin MergeNotCopyProductIndex index on " + DateTime.Now.ToLocalTime().ToString("yyyyMMdd hh:mm:ss"));
            Console.WriteLine("Begin MergeNotCopyProductIndex index at " + DateTime.Now.ToString());

            if (string.IsNullOrEmpty(notCopyFlag))
            {
                Console.WriteLine("notCopyFlag is null.");
                return false;
            }
            try
            {
                IndexWriter indexWriter = CreateIndexWriter(path + "Products" + notCopyFlag);

                string[] dirs = Directory.GetDirectories(path);

                List<string> dirList = new List<string>();
                dirList.AddRange(dirs);

                for (int i = 0; i < dirList.Count;)
                {
                    if (!dirList[i].EndsWith(notCopyFlag))
                    {
                        dirList.RemoveAt(i);
                    }
                    else
                    {
                        i++;
                    }
                }

                dirs = dirList.ToArray();

                Lucene.Net.Store.FSDirectory[] fsDirs = new Lucene.Net.Store.FSDirectory[dirs.Length];
                for (int i = 0; i < fsDirs.Length; i++)
                {
                    Lucene.Net.Store.FSDirectory fsDirectory = Lucene.Net.Store.FSDirectory.Open(new DirectoryInfo(dirs[i]));
                    fsDirs[i] = fsDirectory;
                }

                IndexReader[] idrs = new IndexReader[fsDirs.Length];
                for (int i = 0; i < fsDirs.Length; i++)
                {
                    IndexReader idr = DirectoryReader.Open(fsDirs[i]);
                    idrs[i] = idr;
                }

                indexWriter.AddIndexes(idrs);
                indexWriter.Dispose();

                foreach (Lucene.Net.Store.FSDirectory fs in fsDirs)
                {
                    fs.Dispose();
                }

                LogController.WriteLog("End MergeNotCopyProductIndex index on " + DateTime.Now.ToLocalTime().ToString("yyyyMMdd hh:mm:ss"));
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ex.StackTrace);
            }
            return false;
        }

        public static int BuildCategoriesIndex(string path, DbInfo priceme205DbInfo)
        {
            int count = 0;
            BulidIndexSpeedInfo bulidIndexSpeedInfo = new BulidIndexSpeedInfo();

            using (IndexWriter indexWriter = CreateIndexWriter(path))
            {
                string sql = "GetCategoriesIndex";
                bulidIndexSpeedInfo.StartReadDBTime = DateTime.Now;
                using (var sqlConn = DBController.CreateDBConnection(priceme205DbInfo))
                {
                    sqlConn.Open();

                    using (var sqlCMD = DBController.CreateDbCommand(sql, sqlConn))
                    {
                        sqlCMD.CommandType = System.Data.CommandType.StoredProcedure;
                        using (var sqlDR = sqlCMD.ExecuteReader())
                        {
                            bulidIndexSpeedInfo.EndReadDBTime = DateTime.Now;

                            bulidIndexSpeedInfo.StartWriteIndexTime = DateTime.Now;

                            while (sqlDR.Read())
                            {
                                try
                                {
                                    string CategoryID = sqlDR["CategoryID"].ToString();
                                    string CategoryName = sqlDR["CategoryName"].ToString().Trim().ToLower();
                                    string ParentID = sqlDR["ParentID"].ToString();
                                    int Count = int.Parse(sqlDR["Count"].ToString());
                                    string SubCategoriesString = sqlDR["SubCategoriesString"].ToString();
                                    string ParentCategoriesString = sqlDR["ParentCategoriesString"].ToString();

                                    Document doc = new Document();
                                    doc.Add(new Int32Field("CategoryID", int.Parse(CategoryID), Field.Store.YES));
                                    doc.Add(new StringField("CategoryName", CategoryName.Trim().ToLower(), Field.Store.YES));
                                    doc.Add(new Int32Field("ParentID", int.Parse(ParentID), Field.Store.YES));
                                    doc.Add(new Int32Field("Count", Count, Field.Store.YES));
                                    doc.Add(new StoredField("SubCategoriesString", SubCategoriesString));
                                    doc.Add(new StoredField("ParentCategoriesString", ParentCategoriesString));

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
                                catch (Exception ex)
                                {
                                    LogController.WriteException(ex.Message + ex.StackTrace);
                                }
                            }
                        }
                    }
                }

                indexWriter.Commit();
            }

            bulidIndexSpeedInfo.EndWriteIndexTime = DateTime.Now;
            indexSpeedLog.CategoriesBulidIndexSpeedInfo = bulidIndexSpeedInfo;

            return count;
        }

        public static int BuildProductAttributesIndex(string path, int countryId, DbInfo priceme205DbInfo)
        {
            var productDescriptorDictionary = new Dictionary<string, string>();
            string sql = "GetAllProductAttributeInfo";
            using (var sqlConn = DBController.CreateDBConnection(priceme205DbInfo))
            {
                sqlConn.Open();

                using (var sqlCMD = DBController.CreateDbCommand(sql, sqlConn))
                {
                    sqlCMD.CommandType = System.Data.CommandType.StoredProcedure;
                    using (var sqlDR = sqlCMD.ExecuteReader())
                    {
                        while (sqlDR.Read())
                        {
                            string productDescriptor_ProductID = sqlDR["ProductID"].ToString();
                            string productDescriptor_ProductDescriptorName = sqlDR["ProductDescriptorName"].ToString();
                            int productDescriptor_TypeID = sqlDR.GetInt32(2);

                            string key = productDescriptor_ProductID + "," + productDescriptor_TypeID;
                            AttributeTitleCache attributeTitle = DataController.GetAttributeTitleById(productDescriptor_TypeID);
                            string value = productDescriptor_ProductDescriptorName;
                            if (attributeTitle != null)
                            {
                                value = value.Replace("&nbsp;", " ").Trim();
                                value += " " + attributeTitle.Unit;
                            }
                            if (!productDescriptorDictionary.ContainsKey(key))
                            {
                                productDescriptorDictionary.Add(key, value);
                            }
                        }
                    }
                }
            }
 
            Static_ProductAttrValueList = new Dictionary<string, Dictionary<int, string>>();
            int count = 0;

            BulidIndexSpeedInfo bulidIndexSpeedInfo = new BulidIndexSpeedInfo();

            bulidIndexSpeedInfo.StartReadDBTime = DateTime.Now;

            using (IndexWriter indexWriter = CreateIndexWriter(path))
            {
                sql = "CSK_Store_12RMB_Index_GetProductsDescriptor_V2";

                using (var sqlConn = DBController.CreateDBConnection(priceme205DbInfo))
                {
                    sqlConn.Open();

                    using (var sqlCMD = DBController.CreateDbCommand(sql, sqlConn))
                    {
                        sqlCMD.CommandType = System.Data.CommandType.StoredProcedure;
                        sqlCMD.CommandTimeout = 0;
                        var countryIdP = sqlCMD.CreateParameter();
                        countryIdP.DbType = System.Data.DbType.Int32;
                        countryIdP.Value = countryId;
                        countryIdP.ParameterName = "@countryID";
                        sqlCMD.Parameters.Add(countryIdP);

                        using (var sqlDR = sqlCMD.ExecuteReader())
                        {
                            bulidIndexSpeedInfo.EndReadDBTime = DateTime.Now;

                            bulidIndexSpeedInfo.StartWriteIndexTime = DateTime.Now;
                            while (sqlDR.Read())
                            {
                                try
                                {
                                    string productID = sqlDR["ProductID"].ToString();
                                    string attributeValueID = sqlDR["AttributeValueID"].ToString();
                                    string typeID = sqlDR["TypeID"].ToString();
                                    string categoryID = sqlDR["CategoryID"].ToString();
                                    bool catelogAttributes = bool.Parse(sqlDR["CatelogAttributes"].ToString());
                                    string attrUnit = sqlDR["Unit"].ToString();
                                    string attrValue = sqlDR["value"].ToString();
                                    bool isSlider = bool.Parse(sqlDR["IsSlider"].ToString());
                                    string attributeValueName = "";
                                    string productAttrValue = "";

                                    int attrTitleId = int.Parse(typeID);
                                    float numericValue = 0;
                                    try
                                    {
                                        int cid = int.Parse(categoryID);
                                        var category = DataController.GetCategoryById(cid);
                                        if (category == null) continue;

                                        int avid = int.Parse(attributeValueID);

                                        string mapKey = cid + "," + typeID;

                                        
                                        string key = productID + "," + typeID;

                                        string tagTitle = "";
                                        productDescriptorDictionary.TryGetValue(key, out tagTitle);
                                        if (string.IsNullOrEmpty(tagTitle))
                                        {
                                            tagTitle = attrValue + " " + attrUnit;
                                        }
                                        List<int> _avids = new List<int>();
                                        _avids.Add(avid);

                                        attributeValueName = tagTitle.Trim();

                                        if (isSlider)
                                        {
                                            float val = 0;
                                            if (float.TryParse(attrValue, out val))
                                            {
                                                numericValue = float.Parse(attrValue);
                                            }
                                            else
                                            {
                                                LogController.WriteException("Map : " + mapKey + " is slider but value is " + attrValue);
                                            }
                                        }

                                        productAttrValue = attributeValueName;

  
                                        if (typeID == "249")
                                        {
                                            productAttrValue = "Energy Star";
                                        }

                                        Document doc = new Document();

                                        doc.Add(new Int32Field("ProductID", int.Parse(productID), Field.Store.YES));
                                        doc.Add(new Int32Field("AttributeValueID", int.Parse(attributeValueID), Field.Store.YES));
                                        doc.Add(new Int32Field("TypeID", attrTitleId, Field.Store.YES));
                                        doc.Add(new Int32Field("CategoryID", int.Parse(categoryID), Field.Store.YES));
                                        doc.Add(new StoredField("AttributeValueName", attributeValueName));
                                        doc.Add(new StoredField("IsRange", "0"));
                                        doc.Add(new SingleField("AttributeValue", numericValue, Field.Store.YES));


                                        if (!string.IsNullOrEmpty(productAttrValue) && catelogAttributes)
                                        {
                                            if (Static_ProductAttrValueList.ContainsKey(productID))
                                            {
                                                Static_ProductAttrValueList[productID][attrTitleId] = productAttrValue;
                                            }
                                            else
                                            {
                                                List<AttributeTitleCache> pdtc = DataController.GetAttributesTitleByCategoryID(cid);

                                                List<int> typeIdList = pdtc.Select(pt => pt.TypeID).ToList();
                                                List<CategoryAttributeTitleMapCache> ctmList = new List<CategoryAttributeTitleMapCache>();
                                                foreach (int atId in typeIdList)
                                                {
                                                    CategoryAttributeTitleMapCache categoryAttributeTitleMap = DataController.GetCategoryAttributeTitleMapByCidAttrTId(cid, atId);
                                                    if (categoryAttributeTitleMap == null)
                                                    {
                                                        categoryAttributeTitleMap = new CategoryAttributeTitleMapCache();
                                                        categoryAttributeTitleMap.AttributeTitleID = atId;
                                                        categoryAttributeTitleMap.AttributeOrder = 3;
                                                    }

                                                    ctmList.Add(categoryAttributeTitleMap);
                                                }

                                                ctmList = ctmList.OrderBy(ni => ni.AttributeOrder).ToList();

                                                Dictionary<int, string> pdtcDic = new Dictionary<int, string>();
                                                foreach (var ctm in ctmList)
                                                {
                                                    if (!pdtcDic.ContainsKey(ctm.AttributeTitleID))
                                                    {
                                                        pdtcDic.Add(ctm.AttributeTitleID, "");
                                                    }
                                                }
                                                pdtcDic[attrTitleId] = productAttrValue;
                                                Static_ProductAttrValueList.Add(productID, pdtcDic);
                                            }
                                        }

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
                                    catch (Exception ex)
                                    {
                                        LogController.WriteException(ex.Message + "\t" + ex.StackTrace + "\tattributeValueID : " + attributeValueID + "\t PID : " + productID);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    LogController.WriteException(ex.Message + ex.StackTrace);
                                }
                            }
                        }
                    }
                }

                var hwldDic = GetProductHWLDAttrDic(countryId, priceme205DbInfo);
                foreach (int pid in hwldDic.Keys)
                {
                    List<BaseAttr> baList = hwldDic[pid];
                    foreach (var ba in baList)
                    {
                        Document doc = new Document();

                        doc.Add(new Int32Field("ProductID", pid, Field.Store.YES));
                        doc.Add(new Int32Field("AttributeValueID", -999, Field.Store.YES));
                        doc.Add(new Int32Field("TypeID", ba.TypeId, Field.Store.YES));
                        doc.Add(new Int32Field("CategoryID", ba.CategoryId, Field.Store.YES));
                        doc.Add(new StoredField("AttributeValueName", ba.Value));
                        doc.Add(new StoredField("IsRange", "1"));
                        doc.Add(new SingleField("AttributeValue", ba.Value, Field.Store.YES));

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
                    }
                }

                indexWriter.Commit();
            }

            bulidIndexSpeedInfo.EndWriteIndexTime = DateTime.Now;

            indexSpeedLog.ProductsAttributesBulidIndexSpeedInfo = bulidIndexSpeedInfo;

            return count;
        }

        private static Dictionary<int, List<BaseAttr>> GetProductHWLDAttrDic(int countryId, DbInfo priceme205DbInfo)
        {
            Dictionary<int, List<BaseAttr>> dic = new Dictionary<int, List<BaseAttr>>();

            string sql = @"select ProductID, CategoryID, Height,Width,Length, Weight from CSK_Store_Product where CategoryID in (select CategoryID from HWDSliderAttribute) and IsDeleted = 0
                                    and ProductID in (select productid from CSK_Store_RetailerProduct where ProductStatus = 1 and RetailerId in (select retailerid from CSK_Store_Retailer where RetailerCountry = " + countryId + "))";

            using (var sqlConn = DBController.CreateDBConnection(priceme205DbInfo))
            {
                sqlConn.Open();
                //Height TypeId -1, Width TypeId -2, Length TypeId -3, Weight TypeId -4
                using (var sqlCMD = DBController.CreateDbCommand(sql, sqlConn))
                {
                    sqlCMD.CommandTimeout = 0;
                    using (var dr = sqlCMD.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            List<BaseAttr> list = new List<BaseAttr>();
                            int productId = dr.GetInt32(0);
                            int categoryId = dr.GetInt32(1);
                            decimal height = dr.IsDBNull(2) ? 0 : dr.GetDecimal(2);
                            decimal width = dr.IsDBNull(3) ? 0 : dr.GetDecimal(3);
                            decimal length = dr.IsDBNull(4) ? 0 : dr.GetDecimal(4);
                            decimal weight = dr.IsDBNull(5) ? 0 : dr.GetDecimal(5);

                            var hwdInfo = DataController.GetCategoryHWDInfo(categoryId);
                            if (hwdInfo != null)
                            {
                                if (hwdInfo.Heightslider && height > 0)
                                {
                                    BaseAttr ba = new BaseAttr();
                                    ba.TypeId = -1;
                                    ba.Value = (float)height;
                                    ba.CategoryId = categoryId;
                                    list.Add(ba);
                                }

                                if (hwdInfo.Widthslider && width > 0)
                                {
                                    BaseAttr ba = new BaseAttr();
                                    ba.TypeId = -2;
                                    ba.Value = (float)width;
                                    ba.CategoryId = categoryId;
                                    list.Add(ba);
                                }

                                if (hwdInfo.Depthslider && length > 0)
                                {
                                    BaseAttr ba = new BaseAttr();
                                    ba.TypeId = -3;
                                    ba.Value = (float)length;
                                    ba.CategoryId = categoryId;
                                    list.Add(ba);
                                }

                                if (hwdInfo.Weightslider && weight > 0)
                                {
                                    BaseAttr ba = new BaseAttr();
                                    ba.TypeId = -4;
                                    ba.Value = (float)weight;
                                    ba.CategoryId = categoryId;
                                    if (!hwdInfo.WeightUnitIsKG)
                                    {
                                        ba.Value = ba.Value * 1000;
                                    }
                                    list.Add(ba);
                                }
                            }

                            if (list.Count > 0)
                            {
                                dic.Add(productId, list);
                            }
                        }
                    }
                }
            }

            return dic;
        }

        public static int ProductRetailerMap(string path, int countryId, DbInfo priceme205DbInfo)
        {
            int count = 0;

            BulidIndexSpeedInfo bulidIndexSpeedInfo = new BulidIndexSpeedInfo();

            bulidIndexSpeedInfo.StartReadDBTime = DateTime.Now;

            using (IndexWriter indexWriter = CreateIndexWriter(path))
            {
                string sql = "CSK_Store_12RMB_Index_GetProductRetailerMap";

                using (var sqlConn = DBController.CreateDBConnection(priceme205DbInfo))
                {
                    sqlConn.Open();

                    using (var sqlCMD = DBController.CreateDbCommand(sql, sqlConn))
                    {
                        sqlCMD.CommandType = System.Data.CommandType.StoredProcedure;

                        var countryIdP = sqlCMD.CreateParameter();
                        countryIdP.DbType = System.Data.DbType.Int32;
                        countryIdP.Value = countryId;
                        countryIdP.ParameterName = "@countryID";
                        sqlCMD.Parameters.Add(countryIdP);

                        using (var sqlDR = sqlCMD.ExecuteReader())
                        {
                            bulidIndexSpeedInfo.EndReadDBTime = DateTime.Now;

                            bulidIndexSpeedInfo.StartWriteIndexTime = DateTime.Now;
                            while (sqlDR.Read())
                            {
                                try
                                {
                                    string retailerID = sqlDR["RetailerId"].ToString();
                                    string productID = sqlDR["ProductId"].ToString();

                                    Document doc = new Document();
                                    doc.Add(new Int32Field("RetailerID", int.Parse(retailerID), Field.Store.YES));
                                    doc.Add(new Int32Field("ProductID", int.Parse(productID), Field.Store.YES));

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
                                }
                                catch (Exception ex)
                                {
                                    LogController.WriteException(ex.Message + ex.StackTrace);
                                }
                            }
                        }
                    }
                }
                indexWriter.Commit();
            }

            bulidIndexSpeedInfo.EndWriteIndexTime = DateTime.Now;

            indexSpeedLog.ProductRetailerMapBulidIndexSpeedInfo = bulidIndexSpeedInfo;

            return count;
        }

        //static string GetProductAttributesParameterString(int categoryID, int avid)
        //{
        //    List<PriceMeCache.AttributeTitleCache> pdtc = PriceMeCommon.BusinessLogic.AttributesController.GetAttributesTitleByCategoryID(categoryID);
        //    if (pdtc == null || pdtc.Count == 0)
        //    {
        //        return "";
        //    }

        //    PriceMeCache.AttributeTitleCache pdt = PriceMeCommon.BusinessLogic.AttributesController.GetAttributeTitleByVauleID(avid);
        //    if (pdt == null)
        //    {
        //        return "";
        //    }
        //    bool isInCategory = false;
        //    foreach (PriceMeCache.AttributeTitleCache temp in pdtc)
        //    {
        //        if (temp.TypeID == pdt.TypeID)
        //        {
        //            isInCategory = true;
        //            break;
        //        }
        //    }
        //    if (!isInCategory)
        //    {
        //        return "";
        //    }

        //    PriceMeCache.AttributeValueCache av = PriceMeCommon.BusinessLogic.AttributesController.GetAttributeValueByID(avid);
        //    if (av == null)
        //    {
        //        return "";
        //    }

        //    var attributeValueRange = PriceMeCommon.BusinessLogic.AttributesController.GetAttributeValueRangesByTitleIDAndCategoryID(av.AttributeTitleID, categoryID);

        //    if (attributeValueRange.Count == 0)
        //    {
        //        return av.AttributeTitleID.ToString();
        //    }
        //    else
        //    {
        //        return attributeValueRange[0].ValueRangeID + "r";
        //    }
        //}

        public static int BuildCategoryProductIndex(string path, CategoryCache category, int countryId, bool useCreatedOn, bool includeAccessories, List<ProductCatalog> upComingProducts, Dictionary<int, decimal> prevPriceDic, DbInfo priceme205DbInfo)
        {
            int buildCount = 0;
            int count = 0;
            bool thisCategoryIsOk = false;
            while (buildCount < 2)
            {
                count = 0;

                BulidIndexSpeedInfo buldIndexSpeedInfo = new BulidIndexSpeedInfo();

                Console.WriteLine("Begin " + category.CategoryNameEN + " index at " + DateTime.Now.ToString());

                string indexPath = path + category.CategoryNameEN.Trim();

                int display = 0;

                Dictionary<int, int> categoryProductCountDic = new Dictionary<int, int>();

                string sql = "New_GetCategoryProductsByCID_V2";

                using (var sqlConn = DBController.CreateDBConnection(priceme205DbInfo))
                {
                    sqlConn.Open();

                    using (var sqlCMD = DBController.CreateDbCommand(sql, sqlConn))
                    {
                        sqlCMD.CommandType = System.Data.CommandType.StoredProcedure;

                        var countryIdP = sqlCMD.CreateParameter();
                        countryIdP.DbType = System.Data.DbType.Int32;
                        countryIdP.Value = countryId;
                        countryIdP.ParameterName = "@countryID";
                        sqlCMD.Parameters.Add(countryIdP);

                        var categoryIdP = sqlCMD.CreateParameter();
                        categoryIdP.DbType = System.Data.DbType.Int32;
                        categoryIdP.Value = category.CategoryID;
                        categoryIdP.ParameterName = "@cid";
                        sqlCMD.Parameters.Add(categoryIdP);

                        var websiteIdP = sqlCMD.CreateParameter();
                        websiteIdP.DbType = System.Data.DbType.Int32;
                        websiteIdP.Value = AppValue.WebsiteID;
                        websiteIdP.ParameterName = "@websiteID";
                        sqlCMD.Parameters.Add(websiteIdP);

                        var onlyPPCP = sqlCMD.CreateParameter();
                        onlyPPCP.DbType = System.Data.DbType.Boolean;
                        onlyPPCP.Value = AppValue.OnlyPPC;
                        onlyPPCP.ParameterName = "@onlyPPC";
                        sqlCMD.Parameters.Add(onlyPPCP);

                        if (buildCount > 0)
                        {
                            LogController.WriteLog("ReStart read DB for " + category.CategoryName + " on " + DateTime.Now.ToLocalTime().ToString("yyyyMMdd hh:mm:ss"));
                        }
                        else
                        {
                            LogController.WriteLog("Start read DB for " + category.CategoryName + " on " + DateTime.Now.ToLocalTime().ToString("yyyyMMdd hh:mm:ss"));
                        }

                        float exchangeRate = 1;
                        if (CurrenciesInfo.ContainsKey(countryId))
                        {
                            exchangeRate = CurrenciesInfo[countryId];
                        }

                        try
                        {
                            var categorySynonyms = DataController.GetCategorySynonym(category.CategoryID);
                            using (IndexWriter indexWriter = CreateIndexWriter(indexPath))
                            {
                                buldIndexSpeedInfo.StartReadDBTime = DateTime.Now;
                                using (var idr = sqlCMD.ExecuteReader())
                                {
                                    buldIndexSpeedInfo.EndReadDBTime = DateTime.Now;

                                    LogController.WriteLog("end read DB for " + category.CategoryName + " on " + DateTime.Now.ToLocalTime().ToString("yyyyMMdd hh:mm:ss"));

                                    if (buildCount > 0)
                                    {
                                        LogController.WriteLog("ReStart write index for " + category.CategoryName + " on " + DateTime.Now.ToLocalTime().ToString("yyyyMMdd hh:mm:ss"));
                                    }
                                    else
                                    {
                                        LogController.WriteLog("Start write index for " + category.CategoryName + " on " + DateTime.Now.ToLocalTime().ToString("yyyyMMdd hh:mm:ss"));
                                    }

                                    buldIndexSpeedInfo.StartWriteIndexTime = DateTime.Now;

                                    while (idr.Read())
                                    {
                                        #region 一条一条读取数据库并写入Index
                                        string productId = idr["ProductID"].ToString();
                                        try
                                        {
                                            string isAccessory = string.Empty;
                                            string isAccssories = idr["IsAccessories"].ToString();
                                            if (isAccssories.Equals("true", StringComparison.InvariantCultureIgnoreCase))
                                            {
                                                isAccessory = "1";
                                            }
                                            else
                                            {
                                                isAccessory = "0";
                                            }
                                            if (!includeAccessories && isAccessory == "1")
                                            {
                                                continue;
                                            }

                                            //为false，则需要合并，为ture，则不需要合并
                                            bool isDisplayIsMerged = bool.Parse(idr["IsDisplayIsMerged"].ToString());
                                            string isMerge = idr["IsMerge"].ToString();//Ture Or False
                                            string IsDisplay = "true";

                                            //if(!isDisplayIsMerged && isMerge.Equals("false", StringComparison.InvariantCultureIgnoreCase))
                                            //{
                                            //    Console.WriteLine(pidInt);
                                            //}

                                            if (!isDisplayIsMerged && isMerge.Equals("false", StringComparison.InvariantCultureIgnoreCase) && AppValue.CountryId != 25)
                                            {
                                                continue;
                                            }

                                            string categoryId = idr["CategoryID"].ToString();
                                            int cid = int.Parse(categoryId);

                                            string bestPPCRetailerID = idr["BestPPCRetailerID"].ToString();
                                            string includePPC = "1";
                                            if (string.IsNullOrEmpty(bestPPCRetailerID))
                                            {
                                                includePPC = "0";

                                                if (AppValue.CountryId != 25 && DataController.IsSearchOnly(cid))
                                                {
                                                    continue;
                                                }
                                            }

                                            string productName = idr["ProductName"].ToString().Trim();
                                            string manufacturerName = idr["ManufacturerName"].ToString();
                                            string manufacturerID = idr["ManufacturerID"].ToString();
                                            
                                            if (!hiddenManufacturerCategoryIDList.Contains(categoryId) && !DataController.IsSearchOnly(cid))
                                            {
                                                int mid = int.Parse(manufacturerID);
                                                if (!manufacturerHasProductDic.ContainsKey(mid))
                                                {
                                                    manufacturerHasProductDic.Add(mid, 1);
                                                }
                                                else
                                                {
                                                    manufacturerHasProductDic[mid]++;
                                                }
                                            }

                                            string retailerCount = idr["RetailerCount"].ToString();
                                            
                                            double BestPrice = double.Parse(idr["BestPrice"].ToString());
                                            double MaxPrice = double.Parse(idr["MaxPrice"].ToString());
                                            string defaultImage = idr["DefaultImage"].ToString();

                                            int upRV = 0;
                                            float upRS = 0f;
                                            int epRV = 0;
                                            int epHasScrotRV = 0;
                                            float epAV = 0f;
                                            string productRatingVotes = idr["ProductRatingVotes"].ToString();
                                            string productRatingSum = idr["ProductRatingSum"].ToString();

                                            int.TryParse(productRatingVotes, out upRV);
                                            float.TryParse(productRatingSum, out upRS);
                                            string averageRating = idr["AverageRating"].ToString();
                                            string votesHasScore = idr["VotesHasScore"].ToString();
                                            string expertVotes = idr["Votes"].ToString();
                                            float.TryParse(averageRating, out epAV);
                                            int.TryParse(expertVotes, out epRV);
                                            int.TryParse(votesHasScore, out epHasScrotRV);

                                            float priceMeScore = 0;
                                            float.TryParse(averageRating, out priceMeScore);
                                            float userAverageRating = 0;
                                            string userAverageRatingString = idr["UserAverageRating"].ToString();
                                            float.TryParse(userAverageRatingString, out userAverageRating);
                                            int userVotes = 0;
                                            string userVotesString = idr["UserVotes"].ToString();
                                            int.TryParse(userVotesString, out userVotes);

                                            float avRating = GetAverageRating(upRS, upRV, priceMeScore, epHasScrotRV, userAverageRating, userVotes);

                                            if (float.IsNaN(avRating))
                                            {
                                                avRating = 0f;
                                            }

                                            productRatingSum = string.IsNullOrEmpty(productRatingSum) ? "3" : productRatingSum;
                                            productRatingVotes = string.IsNullOrEmpty(productRatingVotes) ? "1" : productRatingVotes;

                                            int reviewCount = upRV + epRV + userVotes;
                                            if (Exp156CountDic.ContainsKey(productId))
                                            {
                                                reviewCount -= Exp156CountDic[productId];
                                            }

                                            int clicks = 0;
                                            int.TryParse(idr["Clicks"].ToString(), out clicks);

                                            string bestPPCRetailerName = idr["BestPPCRetailerName"].ToString();
                                            
                                            string bestPPCRetailerProductID = idr["BestPPCRetailerProductID"].ToString();
                                            int priceCount = int.Parse(idr["PriceCount"].ToString());
                                            string bestPPCLogo = idr["BestPPCLogo"].ToString();

                                            string catalogDescription = idr["CatalogDescription"].ToString();

                                            string keywords = idr["keywords"].ToString();
                                            float bestPricePPCIndex = 0;
                                            float.TryParse(idr["BestPricePPCIndex"].ToString(), out bestPricePPCIndex);

                                            if (bestPricePPCIndex == 0f)
                                            {
                                                bestPricePPCIndex = AppValue.UnlimitedPPC;
                                            }

                                            double bestPPCPrice = 0;
                                            double.TryParse(idr["BestPPCPrice"].ToString(), out bestPPCPrice);

                                            string RetailerProductList = idr["RetailerProductList"].ToString();

                                            double newBestPrice;
                                            double newMaxPrice;
                                            int overSeasPriceCount;
                                            RetailerProductList = Utility.FixOverseasPrice(RetailerProductList, out newBestPrice, out newMaxPrice, out overSeasPriceCount);
                                            if (newBestPrice > 1)
                                            {
                                                BestPrice = newBestPrice;
                                            }
                                            if (newMaxPrice > 1)
                                            {
                                                MaxPrice = newMaxPrice;
                                            }

                                            int bestPriceRetailerId = GetBestPriceRetailerId(RetailerProductList);

                                            int dayCount = int.Parse(idr["DayCount"].ToString());

                                            if (avRating >= 0f && avRating <= 5f)
                                            {
                                                //Console.WriteLine(avRating);
                                            }
                                            else
                                            {
                                                LogController.WriteException("productId :" + productId + " - avRating : " + avRating + " - productRatingSum : " + productRatingSum + " - productRatingVotes : " + productRatingVotes + " - averageRating : " + averageRating + " - votesHasScore : " + votesHasScore + " - userAverageRating : " + userAverageRatingString + " - userVotes : " + userVotesString);

                                                if (avRating > 5f)
                                                {
                                                    avRating = 5f;
                                                }
                                                else
                                                {
                                                    avRating = 0f;
                                                }
                                            }

                                            string attrDescription = "";
                                            if (Static_ProductAttrValueList.ContainsKey(productId))
                                            {
                                                List<string> attrList = Static_ProductAttrValueList[productId].Values.Where(v => !string.IsNullOrEmpty(v)).Take(4).ToList();
                                                attrDescription = string.Join(", ", attrList);
                                            }

                                            Document doc = new Document();

                                            string pCategoryName = DataController.GetCategoryNameByCategoryID(cid);

                                            string categorySynonym = "";
                                            if (categorySynonyms != null && categorySynonyms.Count > 0)
                                            {
                                                categorySynonym = string.Join(" ", categorySynonyms);
                                            }

                                            string searchFieldString = Utility.GetKeywords(pCategoryName, manufacturerName, keywords, productName, categorySynonym).Trim();
                                            string searchFieldString2 = Utility.FixKeywords(searchFieldString);

                                            doc.Add(new TextField("SearchField", searchFieldString, Field.Store.NO));
                                            doc.Add(new TextField("SearchField2", searchFieldString2, Field.Store.NO));
                                            doc.Add(new Int32Field("ProductID", int.Parse(productId), Field.Store.YES));
                                            doc.Add(new StoredField("ProductName", productName));
                                            doc.Add(new Int32Field("ManufacturerID", int.Parse(manufacturerID), Field.Store.YES));
                                            doc.Add(new Int32Field("CategoryID", cid, Field.Store.YES));
                                            doc.Add(new StoredField("RetailerCount", int.Parse(retailerCount)));
                                            doc.Add(new StringField("IsAccessory", isAccessory, Field.Store.YES));
                                            doc.Add(new StringField("IncludePPC", includePPC, Field.Store.YES));
                                            doc.Add(new DoubleField("BestPrice", BestPrice, Field.Store.YES));

                                            double prevPrice;
                                            int pid = int.Parse(productId);

                                            if (prevPriceDic.ContainsKey(pid))
                                            {
                                                prevPrice = (double)prevPriceDic[pid];
                                            }
                                            else
                                            {
                                                prevPrice = BestPrice;
                                            }

                                            float sale = (float)((prevPrice - BestPrice) / prevPrice);
                                            if (sale > 0.84 || sale < -0.84)
                                            {
                                                prevPrice = BestPrice;
                                                sale = 0;
                                            }
                                            else
                                            {
                                                sale = -sale;
                                            }

                                            doc.Add(new StoredField("PrevPrice", prevPrice));
                                            doc.Add(new SingleField("Sale", sale, Field.Store.YES));
                                            doc.Add(new StoredField("DropPrice", prevPrice - BestPrice));
                                            doc.Add(new StoredField("MaxPrice", MaxPrice));
                                            doc.Add(new StoredField("DefaultImage", defaultImage));
                                            doc.Add(new StoredField("ProductRatingVotes", productRatingVotes));
                                            doc.Add(new StringField("Title", productName, Field.Store.YES));
                                            doc.Add(new StoredField("CatalogDescription", catalogDescription));
                                            doc.Add(new StoredField("ProductRatingSum", productRatingSum));
                                            doc.Add(new Int32Field("Clicks", clicks, Field.Store.YES));
                                            doc.Add(new StoredField("BestPriceRetailerId", bestPriceRetailerId));
                                            doc.Add(new StoredField("BestPPCRetailerName", bestPPCRetailerName));
                                            int bPPCRid = 0;
                                            int.TryParse(bestPPCRetailerID, out bPPCRid);
                                            doc.Add(new StoredField("BestPPCRetailerID", bPPCRid));
                                            int bPPCRpid = 0;
                                            int.TryParse(bestPPCRetailerProductID, out bPPCRpid);
                                            doc.Add(new StoredField("BestPPCRetailerProductID", bPPCRpid));
                                            doc.Add(new StoredField("PriceCount", priceCount));
                                            doc.Add(new StoredField("BestPPCLogoPath", bestPPCLogo));
                                            doc.Add(new StoredField("BestPPCPrice", bestPPCPrice));
                                            doc.Add(new StoredField("ProductReviewCount", reviewCount));
                                            doc.Add(new SingleField("AvRating", avRating, Field.Store.YES));
                                            doc.Add(new StringField("IsDisplay", IsDisplay, Field.Store.YES));
                                            doc.Add(new Int32Field("DayCount", dayCount, Field.Store.YES));
                                            doc.Add(new StringField("IsUpComing", "0", Field.Store.YES));
                                            doc.Add(new StoredField("AttrDescription", attrDescription));

                                            string displaySaleTag = "0";
                                            if (sale <= -0.1 && (prevPrice - BestPrice >= 10))
                                            {
                                                displaySaleTag = "1";
                                            }
                                            doc.Add(new StringField("DisplaySaleTag", displaySaleTag, Field.Store.YES));

                                            if (IsDisplay.Equals("true"))
                                            {
                                                while (true)
                                                {
                                                    try
                                                    {
                                                        indexWriter.AddDocument(doc);
                                                        break;
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        Console.WriteLine(ex.Message);
                                                    }
                                                }
                                                display++;
                                            }

                                            count++;


                                            if (categoryProductCountDic.ContainsKey(cid))
                                            {
                                                categoryProductCountDic[cid]++;
                                            }
                                            else
                                            {
                                                categoryProductCountDic.Add(cid, 1);
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            LogController.WriteException("productId :" + productId + ex.Message + ex.StackTrace);
                                        }
                                        #endregion
                                    }

                                    if (upComingProducts != null)
                                    {
                                        foreach (ProductCatalog pc in upComingProducts)
                                        {
                                            Document doc = new Document();

                                            LogController.WriteLog("Upcoming productId: " + pc.ProductID + " cid: " + pc.CategoryID);

                                            string pCategoryName = DataController.GetCategoryNameByCategoryID(pc.CategoryID);

                                            string categorySynonym = "";
                                            if (categorySynonyms != null && categorySynonyms.Count > 0)
                                            {
                                                categorySynonym = string.Join(" ", categorySynonyms);
                                            }

                                            string searchFieldString = Utility.GetKeywords(pCategoryName, pc.RetailerProductInfoString, "", pc.ProductName, categorySynonym).Trim();
                                            string searchFieldString2 = Utility.FixKeywords(searchFieldString);
                                            double bp;
                                            double.TryParse(pc.BestPrice, out bp);

                                            doc.Add(new TextField("SearchField", searchFieldString, Field.Store.NO));
                                            doc.Add(new TextField("SearchField2", searchFieldString2, Field.Store.NO));
                                            doc.Add(new Int32Field("ProductID", int.Parse(pc.ProductID), Field.Store.YES));
                                            doc.Add(new StoredField("ProductName", pc.ProductName));
                                            doc.Add(new Int32Field("ManufacturerID", int.Parse(pc.ManufacturerID), Field.Store.YES));
                                            doc.Add(new Int32Field("CategoryID", pc.CategoryID, Field.Store.YES));
                                            doc.Add(new StoredField("RetailerCount", 0));
                                            doc.Add(new StringField("IsAccessory", "0", Field.Store.YES));
                                            doc.Add(new StringField("IncludePPC", "1", Field.Store.YES));
                                            doc.Add(new DoubleField("BestPrice", bp, Field.Store.YES));
                                            doc.Add(new StoredField("MaxPrice", 0d));

                                            doc.Add(new StoredField("DefaultImage", pc.DefaultImage));
                                            doc.Add(new StoredField("ProductRatingVotes", "0"));
                                            doc.Add(new StringField("Title", pc.ProductName, Field.Store.YES));
                                            doc.Add(new StoredField("CatalogDescription", pc.ShortDescriptionZH));

                                            doc.Add(new StoredField("ProductRatingSum", "0"));
                                            doc.Add(new Int32Field("Clicks", 0, Field.Store.YES));
                                            doc.Add(new StoredField("BestPriceRetailerId", 0));
                                            doc.Add(new StoredField("BestPPCRetailerName", ""));
                                            doc.Add(new StoredField("BestPPCRetailerID", 0));
                                            doc.Add(new StoredField("BestPPCRetailerProductID", 0));
                                            doc.Add(new StoredField("PriceCount", 0));
                                            doc.Add(new StoredField("BestPPCLogoPath", ""));
                                            doc.Add(new StoredField("BestPPCPrice", 0));
                                            doc.Add(new StoredField("ProductReviewCount", 0));
                                            doc.Add(new SingleField("AvRating", 0, Field.Store.YES));
                                            doc.Add(new StringField("IsDisplay", "True", Field.Store.YES));
                                            doc.Add(new Int32Field("DayCount", int.MaxValue, Field.Store.YES));
                                            doc.Add(new StringField("IsUpComing", "1", Field.Store.YES));
                                            doc.Add(new StoredField("AttrDescription", ""));

                                            while (true)
                                            {
                                                try
                                                {
                                                    indexWriter.AddDocument(doc);
                                                    break;
                                                }
                                                catch (Exception ex)
                                                {
                                                    Console.WriteLine(ex.Message);
                                                }
                                            }

                                            if (categoryProductCountDic.ContainsKey(pc.CategoryID))
                                            {
                                                categoryProductCountDic[pc.CategoryID]++;
                                            }
                                            else
                                            {
                                                categoryProductCountDic.Add(pc.CategoryID, 1);
                                            }
                                        }
                                    }
                                }

                                if (display > 0)
                                {
                                    while (true)
                                    {
                                        try
                                        {
                                            indexWriter.Commit();
                                            break;
                                        }
                                        catch (Exception ex)
                                        {
                                            Console.WriteLine(ex.Message);
                                        }
                                    }
                                }
                                else
                                {
                                    indexWriter.Dispose();
                                    DeleteDirectory(indexPath);
                                }
                            }
                            thisCategoryIsOk = true;
                            LogController.WriteLog("End write index for " + category.CategoryName + " on " + DateTime.Now.ToLocalTime().ToString("yyyyMMdd hh:mm:ss") + " | Product count : " + display);
                        }
                        catch (Exception ex)
                        {
                            LogController.WriteException(ex.Message + ex.StackTrace);
                            LogController.WriteException("CategoryID : " + category.CategoryID + " at : " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                            buildCount++;
                            continue;
                        }
                    }
                }

                LogController.WriteLog("RootCategoryId : " + category.CategoryID);
                foreach (int cid in categoryProductCountDic.Keys)
                {
                    LogController.WriteLog("SubCategoryId : " + cid + "\tProductCount : " + categoryProductCountDic[cid]);
                }

                buldIndexSpeedInfo.EndWriteIndexTime = DateTime.Now;

                string speedInfoKey = category.CategoryID + "_" + category.CategoryName;
                indexSpeedLog.CategoriesProductBuildSpeedInfoDictionary.Add(speedInfoKey, buldIndexSpeedInfo);

                break;
            }

            if (!thisCategoryIsOk)
            {
                AllProductCategoryIndexIsOk = false;
            }

            return count;
        }

        private static int GetBestPriceRetailerId(string retailerProductList)
        {
            string[] _retailerProductList = retailerProductList.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < _retailerProductList.Length; i++)
            {
                string[] infos = _retailerProductList[i].Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                if (infos.Length >= 3)
                {
                    int retailerID = int.Parse(infos[0]);
                    return retailerID;
                }
            }

            return 0;
        }

        private static float GetAverageRating(float userRatingSum, int userRatingVotes, float priceMeScore, int votesHasScore, float userAverageRating, int userVotes)
        {
            float score = 0f;

            if (userRatingVotes > 1)
            {
                userRatingSum = userRatingSum - 3;
                userRatingVotes = userRatingVotes - 1;
            }

            if (userRatingVotes + votesHasScore + userVotes == 0)
            {
                score = 0;
            }
            else
            {
                score = (userRatingSum * 1.0f + (priceMeScore * votesHasScore) + (userAverageRating * userVotes)) / (userRatingVotes * 1.0f + votesHasScore + userVotes);
                score = float.Parse(score.ToString("0.0"));
            }
            return score;
        }

        private static float GetAverageRating(float upRS, int upRV, float epAV, int epHasScrotRV)
        {
            if (upRV + epHasScrotRV == 0)
                return 0;

            return (upRS + epAV * epHasScrotRV) / (upRV + epHasScrotRV);
        }

        private static IndexWriter CreateIndexWriter(string indexPath)
        {
            StandardAnalyzer standardAnalyzer = new StandardAnalyzer(LuceneVersion.LUCENE_48);
            Lucene.Net.Store.FSDirectory fsDirectory = Lucene.Net.Store.FSDirectory.Open(new System.IO.DirectoryInfo(indexPath));
            IndexWriterConfig iwc = new IndexWriterConfig(LuceneVersion.LUCENE_48, standardAnalyzer);
            iwc.RAMBufferSizeMB = 300;
            iwc.MaxBufferedDocs = 2000;
            iwc.OpenMode = OpenMode.CREATE;
            IndexWriter indexWriter = new IndexWriter(fsDirectory, iwc);

            return indexWriter;
        }

        public static int BuildRetailerProductsIndex(string path, int countryId, DbInfo priceme205DbInfo)
        {
            IFormatProvider provider = new System.Globalization.CultureInfo("en-US");
            int count = 0;

            BulidIndexSpeedInfo bulidIndexSpeedInfo = new BulidIndexSpeedInfo();

            bulidIndexSpeedInfo.StartReadDBTime = DateTime.Now;

            using (IndexWriter indexWriter = CreateIndexWriter(path))
            {
                string sql = "CSK_Store_12RMB_Index_GetRetailerProductsByCountry";

                using (var sqlConn = DBController.CreateDBConnection(priceme205DbInfo))
                {
                    sqlConn.Open();

                    using (var sqlCMD = DBController.CreateDbCommand(sql, sqlConn))
                    {
                        sqlCMD.CommandType = System.Data.CommandType.StoredProcedure;
                        sqlCMD.CommandTimeout = 0;
                        var countryIdP = sqlCMD.CreateParameter();
                        countryIdP.DbType = System.Data.DbType.Int32;
                        countryIdP.Value = countryId;
                        countryIdP.ParameterName = "@countryID";
                        sqlCMD.Parameters.Add(countryIdP);

                        using (var sqlDR = sqlCMD.ExecuteReader())
                        {
                            bulidIndexSpeedInfo.EndReadDBTime = DateTime.Now;

                            bulidIndexSpeedInfo.StartWriteIndexTime = DateTime.Now;
                            while (sqlDR.Read())
                            {
                                try
                                {
                                    string categoryID = sqlDR["CategoryID"].ToString();

                                    bool isDisplayIsMerged = bool.Parse(sqlDR["IsDisplayIsMerged"].ToString());
                                    bool isMerge = bool.Parse(sqlDR["IsMerge"].ToString());
                                    if (!isDisplayIsMerged && !isMerge)
                                    {
                                        continue;
                                    }

                                    string productID = sqlDR["ProductID"].ToString();
                                    string retailerProductDefaultImage = sqlDR["DefaultImage"].ToString();
                                    string retailerProductDescription = sqlDR["RetailerProductDescription"].ToString();

                                    string retailerProductName = sqlDR["RetailerProductName"].ToString().Trim();
                                    string productName = sqlDR["ProductName"].ToString().Trim();
                                    string retailerPriceString = sqlDR["RetailerPrice"].ToString();
                                    string retailerId = sqlDR["RetailerId"].ToString();
                                    string clicksString = sqlDR["clicks"].ToString();
                                    string retailerProductId = sqlDR["RetailerProductId"].ToString();
                                    string MPN = sqlDR["MPN"].ToString().ToLower();

                                    string retailerProductCondition = sqlDR["RetailerProductCondition"].ToString();

                                    int productRatingVotes = 0;
                                    float productRatingSum = 0f;
                                    int expertVotes = 0;
                                    int exVotesHasScore = 0;
                                    float exAverageRating = 0f;
                                    int exUserVotes = 0;
                                    float exUserAverageRating = 0f;

                                    string productRatingVotesString = sqlDR["ProductRatingVotes"].ToString();
                                    string productRatingSumString = sqlDR["ProductRatingSum"].ToString();
                                    int.TryParse(productRatingVotesString, out productRatingVotes);
                                    float.TryParse(productRatingSumString, out productRatingSum);
                                    string exAverageRatingString = sqlDR["AverageRating"].ToString();
                                    string exVotesHasScoreString = sqlDR["VotesHasScore"].ToString();
                                    string expertVotesString = sqlDR["Votes"].ToString();
                                    float.TryParse(exAverageRatingString, out exAverageRating);
                                    int.TryParse(expertVotesString, out expertVotes);
                                    int.TryParse(exVotesHasScoreString, out exVotesHasScore);

                                    string exUserAverageRatingString = sqlDR["UserAverageRating"].ToString();
                                    string exUserVotesString = sqlDR["UserVotes"].ToString();
                                    float.TryParse(exUserAverageRatingString, out exUserAverageRating);
                                    int.TryParse(exUserVotesString, out exUserVotes);

                                    Document doc = new Document();

                                    doc.Add(new Int32Field("ProductID", int.Parse(productID), Field.Store.YES));
                                    doc.Add(new StoredField("RetailerProductDefaultImage", retailerProductDefaultImage));
                                    doc.Add(new StoredField("RetailerProductDescription", retailerProductDescription));
                                    doc.Add(new Int32Field("CategoryID", int.Parse(categoryID), Field.Store.YES));
                                    doc.Add(new StoredField("RetailerProductName", retailerProductName));
                                    doc.Add(new StoredField("ProductName", productName));
                                    doc.Add(new Int32Field("RetailerId", int.Parse(retailerId), Field.Store.YES));
                                    doc.Add(new Int32Field("RetailerProductId", int.Parse(retailerProductId), Field.Store.YES));
                                    doc.Add(new StringField("RetailerProductCondition", retailerProductCondition, Field.Store.YES));
                                    doc.Add(new StringField("MPN", MPN, Field.Store.YES));

                                    string searchFieldString = Utility.GetKeywords("", "", "", retailerProductName, productName).Trim();
                                    doc.Add(new TextField("SearchField", searchFieldString, Field.Store.YES));

                                    double bestPrice = double.Parse(retailerPriceString, System.Globalization.NumberStyles.Any, provider);
                                    doc.Add(new DoubleField("RetailerPrice", bestPrice, Field.Store.YES));

                                    int clicks;
                                    int.TryParse(clicksString, out clicks);
                                    doc.Add(new Int32Field("Clicks", clicks, Field.Store.YES));

                                    float avRating = GetAverageRating(productRatingSum, productRatingVotes, exAverageRating, exVotesHasScore, exUserAverageRating, exUserVotes);

                                    int reviewCount = productRatingVotes + expertVotes + exUserVotes;
                                    if (productRatingVotes > 1)
                                    {
                                        reviewCount--;
                                    }
                                    if (expertVotes + exUserVotes > 1)
                                    {
                                        reviewCount--;
                                    }
                                    if (reviewCount == 0)
                                    {
                                        avRating = 0;
                                    }

                                    doc.Add(new SingleField("AvRating", avRating, Field.Store.YES));
                                    doc.Add(new StoredField("ReviewCount", reviewCount.ToString()));

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
                                catch (Exception ex)
                                {
                                    LogController.WriteException(ex.Message + ex.StackTrace);
                                }
                            }
                        }
                    }
                }
                indexWriter.Commit();
            }

            bulidIndexSpeedInfo.EndWriteIndexTime = DateTime.Now;

            indexSpeedLog.RetailerProductsBulidIndexSpeedInfo = bulidIndexSpeedInfo;

            return count;
        }

        //private static Dictionary<int, RetailerCache> GetAllRetailerDictionary()
        //{
        //    List<CSK_Store_Retailer> allRetailer = CSK_Store_Retailer.All().ToList();
        //    List<PriceMeCache.RetailerCache> retailerCacheList = ConvertController<PriceMeCache.RetailerCache, CSK_Store_Retailer>.ConvertData(allRetailer);
        //    Dictionary<int, PriceMeCache.RetailerCache> retailerDictionary = retailerCacheList.ToDictionary(rc => rc.RetailerId);

        //    return retailerDictionary;
        //}

        public static void DeleteDirectory(string dir)
        {
            string[] files = Directory.GetFiles(dir);
            foreach (string file in files)
            {
                int i = 0;
                while (i < 3)
                {
                    try
                    {
                        File.Delete(file);
                        break;
                    }
                    catch (Exception ex)
                    {
                        LogController.WriteException(ex.Message + ex.StackTrace);
                    }
                    i++;
                    System.Threading.Thread.CurrentThread.Join(3000);
                }
            }
            Directory.Delete(dir);
        }

        //public static string BuildRetailerIndexByID(string path, PriceMeCache.RetailerCache retailerCache)
        //{
        //    BulidIndexSpeedInfo buldIndexSpeedInfo = new BulidIndexSpeedInfo();

        //    Console.WriteLine("Begin retailer : " + retailerCache.RetailerName + " index at " + DateTime.Now.ToString());

        //    int countryID = retailerCache.RetailerCountry;

        //    string indexPath = path + DateTime.Now.ToString("yyyyMMdd") + "_retailer_" + retailerCache.RetailerId;
        //    Lucene.Net.Index.IndexWriter indexWriter = CreateIndexWriter(indexPath);

        //    PriceMeDBA.CSK_Util_Country cskUtilCountry =
        //        PriceMeDBA.CSK_Util_Country.SingleOrDefault(country => country.countryID == countryID);

        //    int display = 0;

        //    float priceMeExchangeRate = 1;
        //    if (cskUtilCountry != null)
        //    {
        //        priceMeExchangeRate = (float)cskUtilCountry.PriceMeExchangeRate;
        //    }

        //    int count = 0;

        //    string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["CommerceTemplate"].ConnectionString;
        //    using (SqlConnection sqlConnection = new SqlConnection(connectionString))
        //    {
        //        LogController.WriteLog("Start read DB for retailer " + retailerCache.RetailerName + " on " + DateTime.Now.ToLocalTime().ToString("yyyyMMdd hh:mm:ss"));

        //        float exchangeRate = 1;
        //        if (CurrenciesInfo.ContainsKey(countryID))
        //        {
        //            exchangeRate = CurrenciesInfo[countryID];
        //        }

        //        using (SqlCommand sqlCMD = new SqlCommand("New_GetCategoryProductsByRetailerID", sqlConnection))
        //        {
        //            sqlCMD.CommandType = System.Data.CommandType.StoredProcedure;
        //            sqlCMD.CommandTimeout = 0;

        //            SqlParameter conutryPM = new SqlParameter();
        //            conutryPM.ParameterName = "@countryID";
        //            conutryPM.Value = retailerCache.RetailerCountry;
        //            sqlCMD.Parameters.Add(conutryPM);

        //            SqlParameter retailerPM = new SqlParameter();
        //            retailerPM.ParameterName = "@retailerID";
        //            retailerPM.Value = retailerCache.RetailerId;
        //            sqlCMD.Parameters.Add(retailerPM);

        //            try
        //            {
        //                sqlConnection.Open();
        //                buldIndexSpeedInfo.StartReadDBTime = DateTime.Now;
        //                using (System.Data.IDataReader idr = sqlCMD.ExecuteReader())
        //                {
        //                    buldIndexSpeedInfo.EndReadDBTime = DateTime.Now;

        //                    LogController.WriteLog("end read DB for retailer " + retailerCache.RetailerName + " on " + DateTime.Now.ToLocalTime().ToString("yyyyMMdd hh:mm:ss"));
        //                    LogController.WriteLog("Start write index for retailer " + retailerCache.RetailerName + " on " + DateTime.Now.ToLocalTime().ToString("yyyyMMdd hh:mm:ss"));

        //                    buldIndexSpeedInfo.StartWriteIndexTime = DateTime.Now;
        //                    while (idr.Read())
        //                    {
        //                        string productId = idr["ProductID"].ToString();
        //                        try
        //                        {
        //                            string isAccessory = string.Empty;
        //                            string isAccssories = idr["IsAccessories"].ToString();
        //                            if (isAccssories.Equals("true", StringComparison.InvariantCultureIgnoreCase))
        //                            {
        //                                isAccessory = "1";
        //                            }
        //                            else
        //                            {
        //                                isAccessory = "0";
        //                            }

        //                            string productName = idr["ProductName"].ToString();
        //                            string manufacturerName = idr["ManufacturerName"].ToString();
        //                            string manufacturerID = idr["ManufacturerID"].ToString();
        //                            string categoryId = idr["CategoryID"].ToString();

        //                            string retailerCount = idr["RetailerCount"].ToString();
        //                            string isMerge = idr["IsMerge"].ToString();//Ture Or False
        //                            double BestPrice = double.Parse(idr["BestPrice"].ToString());
        //                            float MaxPrice = float.Parse(idr["MaxPrice"].ToString());
        //                            string defaultImage = idr["DefaultImage"].ToString();
        //                            string shortDescriptionZH = idr["ShortDescriptionZH"].ToString();
        //                            shortDescriptionZH = Utility.GetString(shortDescriptionZH, 200);
        //                            string SKU = idr["SKU"].ToString();

        //                            int upRV = 0;
        //                            float upRS = 0f;
        //                            int epRV = 0;
        //                            int epHasScrotRV = 0;
        //                            float epAV = 0f;
        //                            string productRatingVotes = idr["ProductRatingVotes"].ToString();
        //                            string productRatingSum = idr["ProductRatingSum"].ToString();
        //                            int.TryParse(productRatingVotes, out upRV);
        //                            float.TryParse(productRatingSum, out upRS);
        //                            string averageRating = idr["AverageRating"].ToString();
        //                            string votesHasScore = idr["VotesHasScore"].ToString();
        //                            string expertVotes = idr["Votes"].ToString();
        //                            float.TryParse(averageRating, out epAV);
        //                            int.TryParse(expertVotes, out epRV);
        //                            int.TryParse(votesHasScore, out epHasScrotRV);

        //                            float priceMeScore = 0;
        //                            float.TryParse(idr["AverageRating"].ToString(), out priceMeScore);
        //                            float userAverageRating = 0;
        //                            float.TryParse(idr["UserAverageRating"].ToString(), out userAverageRating);
        //                            int userVotes = 0;
        //                            int.TryParse(idr["UserVotes"].ToString(), out userVotes);

        //                            //float avRating = GetAverageRating(upRS, upRV, epAV, epHasScrotRV);
        //                            float avRating = GetAverageRating(upRS, upRV, priceMeScore, epHasScrotRV, userAverageRating, userVotes);

        //                            int reviewCount = upRV + epRV;

        //                            productRatingSum = string.IsNullOrEmpty(productRatingSum) ? "3" : productRatingSum;
        //                            productRatingVotes = string.IsNullOrEmpty(productRatingVotes) ? "1" : productRatingVotes;

        //                            int clicks = 0;
        //                            int.TryParse(idr["Clicks"].ToString(), out clicks);

        //                            //string createdOn = string.IsNullOrEmpty(idr["CreatedOn"] == null ? "" : idr["CreatedOn"].ToString()) ? "1999-01-01" : idr["createdOn"].ToString();
        //                            string bestPPCRetailerName = idr["BestPPCRetailerName"].ToString();
        //                            string bestPPCRetailerID = idr["BestPPCRetailerID"].ToString();
        //                            string includePPC = "1";
        //                            if (string.IsNullOrEmpty(bestPPCRetailerID))
        //                            {
        //                                includePPC = "0";
        //                            }
        //                            string bestPPCRetailerProductID = idr["BestPPCRetailerProductID"].ToString();
        //                            int priceCount = int.Parse(idr["PriceCount"].ToString());
        //                            string bestPPCLogo = idr["BestPPCLogo"].ToString();

        //                            float PrevPrice = 0f;
        //                            float.TryParse(idr["PrevPrice"].ToString(), out PrevPrice);

        //                            string MPN = idr["MPN"].ToString().ToLower();
        //                            string UPC = idr["UPC"].ToString().ToLower();

        //                            string catalogDescription = idr["CatalogDescription"].ToString();
        //                            bool isDisplayIsMerged = bool.Parse(idr["IsDisplayIsMerged"].ToString());

        //                            string keywords = idr["keywords"].ToString();
        //                            float bestPricePPCIndex = 0;
        //                            float.TryParse(idr["BestPricePPCIndex"].ToString(), out bestPricePPCIndex);

        //                            if (bestPricePPCIndex == 0f)
        //                            {
        //                                bestPricePPCIndex = AppValue.UnlimitedPPC;
        //                            }

        //                            double bestPPCPrice = 0;
        //                            double.TryParse(idr["BestPPCPrice"].ToString(), out bestPPCPrice);

        //                            string IsReNamed = idr["IsReNamed"].ToString();
        //                            string IsDisplay = "";
        //                            if (!isDisplayIsMerged)
        //                            {
        //                                if (isMerge.Equals("true", StringComparison.InvariantCultureIgnoreCase))
        //                                {
        //                                    IsDisplay = "true";
        //                                }
        //                                else
        //                                {
        //                                    IsDisplay = "false";
        //                                }
        //                            }
        //                            else
        //                            {
        //                                IsDisplay = "true";
        //                            }
        //                            string RPS = idr["RPS"].ToString();
        //                            if ((RPS == "0" || RPS.Equals("false", StringComparison.InvariantCultureIgnoreCase)) && retailerCount == "1")
        //                            {
        //                                IsDisplay = "false";
        //                            }

        //                            double NZBestPrice = BestPrice * exchangeRate;
        //                            Document doc = new Document();

        //                            string pCategoryName = CategoryController.GetCategoryNameByCategoryID(int.Parse(categoryId), AppValue.CountryId);

        //                            string searchFieldString = Utility.GetKeywords(pCategoryName, manufacturerName, keywords, productName, "").Trim();
        //                            doc.Add(new Field("SearchField", searchFieldString, Field.Store.YES, Field.Index.ANALYZED));
        //                            string searchFieldString2 = Utility.FixKeywords(searchFieldString);
        //                            doc.Add(new Field("SearchField2", searchFieldString2, Field.Store.YES, Field.Index.ANALYZED));
        //                            doc.Add(new Field("ProductID", productId, Field.Store.YES, Field.Index.NOT_ANALYZED));
        //                            doc.Add(new Field("ProductName", productName, Field.Store.YES, Field.Index.NO));
        //                            doc.Add(new Field("ProductNameForImport", productName.ToLower(), Field.Store.YES, Field.Index.NOT_ANALYZED));
        //                            //doc.Add(new Field("ManufacturerName", manufacturerName, Field.Store.YES, Field.Index.NOT_ANALYZED, Field.TermVector.NO));
        //                            doc.Add(new Field("ManufacturerID", manufacturerID, Field.Store.YES, Field.Index.NOT_ANALYZED));
        //                            doc.Add(new Field("CategoryID", categoryId, Field.Store.YES, Field.Index.NOT_ANALYZED));
        //                            doc.Add(new Field("RetailerCount", retailerCount, Field.Store.YES, Field.Index.NO));
        //                            doc.Add(new Field("IsAccessory", isAccessory, Field.Store.YES, Field.Index.NOT_ANALYZED));
        //                            doc.Add(new Field("IncludePPC", includePPC, Field.Store.YES, Field.Index.NOT_ANALYZED));

        //                            //doc.Add(new Field("IsActive", isActive, Field.Store.YES, Field.Index.NOT_ANALYZED, Field.TermVector.YES));
        //                            doc.Add(new Field("IsMerge", isMerge, Field.Store.YES, Field.Index.NOT_ANALYZED));

        //                            NumericField bestPriceField = new NumericField("BestPrice", Field.Store.YES, true);
        //                            bestPriceField.SetDoubleValue(BestPrice);
        //                            doc.Add(bestPriceField);

        //                            //----------
        //                            NumericField nzBestPriceField = new NumericField("NZBestPrice", Field.Store.YES, true);
        //                            nzBestPriceField.SetDoubleValue(NZBestPrice);
        //                            doc.Add(nzBestPriceField);
        //                            //----------

        //                            NumericField maxPriceField = new NumericField("MaxPrice", Field.Store.YES, true);
        //                            maxPriceField.SetDoubleValue(MaxPrice);
        //                            doc.Add(maxPriceField);

        //                            NumericField pricemeBestPriceField = new NumericField("PriceMeBestPriceField", Field.Store.YES, true);
        //                            pricemeBestPriceField.SetDoubleValue(BestPrice / priceMeExchangeRate);
        //                            doc.Add(pricemeBestPriceField);
        //                            NumericField pricemeMaxPriceField = new NumericField("PriceMeMaxPriceField", Field.Store.YES, true);
        //                            pricemeMaxPriceField.SetDoubleValue(MaxPrice / priceMeExchangeRate);
        //                            doc.Add(pricemeMaxPriceField);

        //                            doc.Add(new Field("DefaultImage", defaultImage, Field.Store.YES, Field.Index.NO));
        //                            doc.Add(new Field("ShortDescriptionZH", shortDescriptionZH, Field.Store.YES, Field.Index.NO));
        //                            doc.Add(new Field("ProductRatingVotes", productRatingVotes, Field.Store.YES, Field.Index.NO));
        //                            doc.Add(new Field("Title", productName, Field.Store.YES, Field.Index.NOT_ANALYZED));
        //                            doc.Add(new Field("CatalogDescription", catalogDescription, Field.Store.YES, Field.Index.NO));
        //                            doc.Add(new Field("SKU", SKU, Field.Store.YES, Field.Index.NOT_ANALYZED));
        //                            doc.Add(new Field("ProductRatingSum", productRatingSum, Field.Store.YES, Field.Index.NO));

        //                            NumericField clicksField = new NumericField("Clicks", Field.Store.YES, true);
        //                            clicksField.SetIntValue(clicks);
        //                            doc.Add(clicksField);

        //                            //doc.Add(new Field("CreatedOn", createdOn, Field.Store.YES, Field.Index.NOT_ANALYZED, Field.TermVector.NO));
        //                            doc.Add(new Field("BestRetailerName", bestPPCRetailerName, Field.Store.YES, Field.Index.NO));
        //                            doc.Add(new Field("RetailerAmount", priceCount.ToString(), Field.Store.YES, Field.Index.NO));
        //                            doc.Add(new Field("RetailerProductID", bestPPCRetailerProductID, Field.Store.YES, Field.Index.NO));
        //                            doc.Add(new Field("PPCLogoPath", bestPPCLogo, Field.Store.YES, Field.Index.NO));
        //                            doc.Add(new Field("PPCRetailerProductID", bestPPCRetailerProductID, Field.Store.YES, Field.Index.NO));
        //                            doc.Add(new Field("PPCLogo", bestPPCLogo, Field.Store.YES, Field.Index.NO));
        //                            NumericField ppcIndexField = new NumericField("PPCIndex", Field.Store.YES, true);
        //                            ppcIndexField.SetFloatValue(bestPricePPCIndex);
        //                            doc.Add(ppcIndexField);
        //                            //

        //                            doc.Add(new Field("BestPPCRetailerName", bestPPCRetailerName, Field.Store.YES, Field.Index.NO));
        //                            doc.Add(new Field("BestPPCRetailerID", bestPPCRetailerID, Field.Store.YES, Field.Index.NO));
        //                            doc.Add(new Field("BestPPCRetailerProductID", bestPPCRetailerProductID, Field.Store.YES, Field.Index.NO));
        //                            doc.Add(new Field("PriceCount", priceCount.ToString(), Field.Store.YES, Field.Index.NO));
        //                            doc.Add(new Field("BestPPCLogoPath", bestPPCLogo, Field.Store.YES, Field.Index.NO));
        //                            NumericField bestPricePPCIndexField = new NumericField("BestPricePPCIndex", Field.Store.YES, true);
        //                            bestPricePPCIndexField.SetFloatValue(bestPricePPCIndex);
        //                            doc.Add(bestPricePPCIndexField);

        //                            NumericField bestPPCPriceField = new NumericField("BestPPCPrice", Field.Store.YES, true);
        //                            bestPPCPriceField.SetDoubleValue(bestPPCPrice);
        //                            doc.Add(bestPPCPriceField);

        //                            doc.Add(new Field("ProductReviewCount", reviewCount.ToString(), Field.Store.YES, Field.Index.NO));

        //                            NumericField prevPriceField = new NumericField("PrevPrice", Field.Store.YES, true);
        //                            prevPriceField.SetDoubleValue(PrevPrice);
        //                            doc.Add(prevPriceField);

        //                            NumericField avRatingField = new NumericField("AvRating", Field.Store.YES, true);
        //                            avRatingField.SetFloatValue(avRating);
        //                            doc.Add(avRatingField);

        //                            //doc.Add(new Field("RetailerProductList", RetailerProductList, Field.Store.YES, Field.Index.NO));
        //                            doc.Add(new Field("IsReNamed", IsReNamed, Field.Store.YES, Field.Index.NO));
        //                            doc.Add(new Field("MPN", MPN, Field.Store.YES, Field.Index.NOT_ANALYZED));
        //                            doc.Add(new Field("UPC", UPC, Field.Store.YES, Field.Index.NOT_ANALYZED));
        //                            doc.Add(new Field("IsDisplay", IsDisplay, Field.Store.YES, Field.Index.NOT_ANALYZED));
        //                            //

        //                            float boost = 1f;
        //                            if (isAccessory == "1")
        //                            {
        //                                boost = 0.1f;
        //                            }

        //                            if (isMerge.ToLower() == "true" || isMerge.ToLower() == "1")
        //                            {
        //                                boost = 2 * boost;
        //                            }
        //                            boost += clicks / AppValue.ClickBoost * 0.1f;
        //                            doc.Boost = boost;

        //                            if (IsDisplay.Equals("true"))
        //                            {
        //                                while (true)
        //                                {
        //                                    try
        //                                    {
        //                                        indexWriter.AddDocument(doc);
        //                                        break;
        //                                    }
        //                                    catch (Exception ex)
        //                                    {
        //                                        Console.WriteLine(ex.Message);
        //                                    }
        //                                }
        //                                display++;
        //                            }

        //                            count++;
        //                        }
        //                        catch (Exception ex)
        //                        {
        //                            PriceMeCommon.BusinessLogic.LogController.WriteException("productId :" + productId + ex.Message + ex.StackTrace);
        //                        }
        //                    }
        //                }

        //                if (display > 0)
        //                {
        //                    while (true)
        //                    {
        //                        try
        //                        {
        //                            indexWriter.Optimize();
        //                            indexWriter.Dispose();
        //                            break;
        //                        }
        //                        catch (Exception ex)
        //                        {
        //                            Console.WriteLine(ex.Message);
        //                        }
        //                    }
        //                }
        //                else
        //                {
        //                    indexWriter.Dispose();
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                PriceMeCommon.BusinessLogic.LogController.WriteException(ex.Message + ex.StackTrace);
        //            }
        //        }
        //    }

        //    PriceMeCommon.BusinessLogic.LogController.WriteLog("End write index for retailer " + retailerCache.RetailerName + " on " + DateTime.Now.ToLocalTime().ToString("yyyyMMdd hh:mm:ss"));

        //    buldIndexSpeedInfo.EndWriteIndexTime = DateTime.Now;

        //    string speedInfoKey = retailerCache.RetailerId + "_" + retailerCache.RetailerName;

        //    PriceMeCommon.BusinessLogic.LogController.WriteLog("Index product count : " + count);

        //    return indexPath;
        //}
    }
}