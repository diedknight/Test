using Lucene.Net.Analysis;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Util;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;

namespace PopualerSearchIndexBuilder
{
    class Program
    {
        static Regex InvalidCharacter_Static = new Regex("[^a-z0-9_\\-\\s]", RegexOptions.Compiled | RegexOptions.Multiline | RegexOptions.IgnoreCase);

        static Dictionary<string, int> categoryClicksDic;
        static Dictionary<int, int> productClicksDic;
        static IFormatProvider CurrentCulture_Static;
        static IFormatProvider Provider_Static;

        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json");

            var configuration = builder.Build();

            LogController.Init(configuration);

            Provider_Static = new System.Globalization.CultureInfo("en-US");
            CurrentCulture_Static = new System.Globalization.CultureInfo(configuration["CurrentCulture"]);

            string idxDir = Path.Combine(configuration["IndexDir"], DateTime.Now.ToString("yyyyMMddHH"));

            DbInfo priceme205DbInfo = new DbInfo();
            priceme205DbInfo.ConnectionString = configuration.GetConnectionString("205Db");
            priceme205DbInfo.ProviderName = configuration["205Db_ProviderName"];

            DbInfo pamUserDbInfo = new DbInfo();
            pamUserDbInfo.ConnectionString = configuration.GetConnectionString("PamUser");
            pamUserDbInfo.ProviderName = configuration["PamUser_ProviderName"];

            DbInfo subDbInfo = new DbInfo();
            subDbInfo.ConnectionString = configuration.GetConnectionString("SubDb");
            subDbInfo.ProviderName = configuration["SubDb_ProviderName"];
            subDbInfo.CountryId = int.Parse(configuration["CountryId"]);

            int clickDays = int.Parse(configuration["ClickDays"]);
            bool checkClick = bool.Parse(configuration["CheckClick"]);

            SetClicksDic(clickDays, pamUserDbInfo);

            if (checkClick)
            {
                int clicksCount = categoryClicksDic.Values.Sum();

                LogController.WriteLog("ClickCount : " + clicksCount + " on " + DateTime.Now.ToLocalTime().ToString("yyyyMMdd hh:mm:ss"));

                if (clicksCount == 0)
                    return;
            }

            DataController.Init(subDbInfo);

            bool ppcOnly = false;
            bool.TryParse(configuration["PPCOnly"], out ppcOnly);
            string rootCategoryIds = configuration["RootCategoryIds"];

            BuildPopularSearchIndex(idxDir, pamUserDbInfo, subDbInfo, priceme205DbInfo, ppcOnly, rootCategoryIds);

            //idxDir = "E:\\PopularSearchIndex\\2018082109";

            string luceneConfigFilePathStr = configuration["LocalLuceneConfigPath"];
            string[] luceneConfigFilePathArray = luceneConfigFilePathStr.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var lcPath in luceneConfigFilePathArray)
            {
                if (File.Exists(lcPath))
                    UpDateWebConfig(lcPath, "PopularSearchIndexPath2", idxDir);
                else
                    LogController.WriteException("LuceneConfigPath : " + lcPath + " file not exists.");
            }

            if (bool.Parse(configuration["byFTP"]))
            {
                string userID = configuration["userid_FTP"];
                string password = configuration["password_FTP"];
                string targetIP = configuration["targetIP_FTP"];
                string targetPath = configuration["targetPath_FTP"];
                try
                {
                    CopyFile.FtpCopy.UploadDirectorySmall(idxDir, targetPath, targetIP, userID, password, "-no");

                    DirectoryInfo dirInfo = new DirectoryInfo(idxDir);

                    ModifyLuceneConfigFTP(dirInfo.FullName, configuration);
                }
                catch (Exception e)
                {
                    LogController.WriteException(e.StackTrace + e.Message);
                }
            }
        }

        private static void BuildPopularSearchIndex(string idxDir, DbInfo pamUserDbInfo, DbInfo subDbInfo, DbInfo priceme205DbInfo, bool ppcOnly, string rootCategoryIds)
        {
            int dbProductCount = 0;
            int indexProductCount = 0;
            int indexUpComingProductCount = 0;
            int indexOfferProductCount = 0;
            int indexCategoryCount = 0;
            int indexRetailerCount = 0;
            int indexindexBrandAndCategoryCount = 0;
            int indexPopularSearchCount = 0;
            int indexBrandCount = 0;

            if (!Directory.Exists(idxDir))
            {
                Directory.CreateDirectory(idxDir);
            }

            List<string> isSearchOnlyCategories = new List<string>();
            Dictionary<string, List<CategoryDataInfo>> dictionary = new Dictionary<string, List<CategoryDataInfo>>();
            Dictionary<int, string> categorySynonymDic = GetCategorySynonymDic(subDbInfo.CountryId, pamUserDbInfo);

            Analyzer analyzer = new Lucene.Net.Analysis.Core.WhitespaceAnalyzer(LuceneVersion.LUCENE_48);
            Lucene.Net.Store.FSDirectory ramDir = Lucene.Net.Store.FSDirectory.Open(new DirectoryInfo(idxDir));

            IndexWriterConfig iwc = new IndexWriterConfig(LuceneVersion.LUCENE_48, analyzer);
            iwc.OpenMode = OpenMode.CREATE;
            using (IndexWriter idw = new IndexWriter(ramDir, iwc))
            {
                using (var sqlConn = DBController.CreateDBConnection(subDbInfo))
                {
                    sqlConn.Open();

                    string query = "";
                    IDataReader idr = null;

                    Dictionary<int, RPInfo> rpInfoDic = new Dictionary<int, RPInfo>();

                    if (sqlConn is MySql.Data.MySqlClient.MySqlConnection)
                    {
                        query = @"SELECT RetailerProductId
                            ,ProductId
                            ,RetailerId
                            ,RetailerPrice
                            from CSK_Store_RetailerProductNew a
                            where RetailerId in (select RetailerId from CSK_Store_PPCMember where PPCMemberTypeID = 2)
                            and ProductId in (
                            Select ProductId from CSK_Store_ProductNew where CategoryID in (select CategoryID from CSK_Store_Category where isSearchOnly = 1))
                            and (select count(*) from CSK_Store_RetailerProductNew b where a.ProductId = b.ProductId and a.RetailerPrice < b.RetailerPrice) = 0";
                    }
                    else
                    {
                        query = @"with tp as (
	                        SELECT RetailerProductId
		                    ,ProductId
                            ,RetailerId
	                        ,RetailerPrice
	                        ,ROW_NUMBER() OVER(PARTITION BY ProductID Order by RetailerPrice) as rn
	                        from CSK_Store_RetailerProductNew
	                        where RetailerId in (select RetailerId from CSK_Store_PPCMember where PPCMemberTypeID = 2" + @") 
                            and ProductId in (Select ProductId from CSK_Store_ProductNew where CategoryID in (select CategoryID from CSK_Store_Category where isSearchOnly = 1))
                            )
                            select * from tp where rn = 1";
                    }

                    var scRP = DBController.CreateDbCommand(query, sqlConn);
                    idr = scRP.ExecuteReader();
                    Console.WriteLine("exec sql : " + query);
                    while (idr.Read())
                    {
                        int rpid = idr.GetInt32(0);
                        int pid = idr.GetInt32(1);
                        int rid = idr.GetInt32(2);
                        decimal minPrice = idr.GetDecimal(3);

                        if (!rpInfoDic.ContainsKey(pid))
                        {
                            RPInfo rpInfo = new RPInfo();
                            rpInfo.ProductID = pid;
                            rpInfo.RetailerProductID = rpid;
                            rpInfo.RetailerID = rid;
                            rpInfo.MinPrice = minPrice;
                            rpInfoDic.Add(pid, rpInfo);
                        }
                    }
                    idr.Close();

                    List<string> siteMapCidList = new List<string>();

                    query = @"select c.CategoryName, c.CategoryID, c.IsSiteMap, c.IsSiteMapDetail,
                                    LCT.CategoryName as LocalName, c.isSearchOnly from CSK_Store_Category as C left join Local_CategoryName as LCT on C.CategoryID = LCT.CategoryID
                                    and C.IsActive = 1 and LCT.CountryID = " + subDbInfo.CountryId + " order by C.CategoryID";


                    #region DK
                    string categoryQueryString = " and (";

                    if (!string.IsNullOrEmpty(rootCategoryIds) && rootCategoryIds != "0")
                    {
                        string[] cids = rootCategoryIds.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                        foreach (string cid in cids)
                        {
                            List<int> allSubCIds = DataController.GetAllSubCategoryId(int.Parse(cid));
                            allSubCIds.Add(int.Parse(cid));
                            categoryQueryString += " c.CategoryID in (" + string.Join(",", allSubCIds) + ") or ";
                        }
                        categoryQueryString = categoryQueryString.Substring(0, categoryQueryString.Length - 3) + ")";

                        query = @"select c.CategoryName, c.CategoryID, c.IsSiteMap, c.IsSiteMapDetail,
                                    LCT.CategoryName as LocalName, c.isSearchOnly from CSK_Store_Category as C left join Local_CategoryName as LCT on C.CategoryID = LCT.CategoryID
                                    and C.IsActive = 1 and LCT.CountryID =" + subDbInfo.CountryId + categoryQueryString
                                        + @"
                                    order by C.CategoryID";
                        //
                    }
                    #endregion

                    var sc2 = DBController.CreateDbCommand(query, sqlConn);

                    Console.WriteLine("exec sql : " + query);

                    List<BaseData> categoryDataList = new List<BaseData>();
                    idr = sc2.ExecuteReader();

                    string keyword;

                    while (idr.Read())
                    {
                        string cid = idr["CategoryID"].ToString();

                        string isSiteMap = idr["IsSiteMap"].ToString();
                        string isSiteMapDetail = idr["IsSiteMapDetail"].ToString();
                        if (isSiteMap.Equals("true", StringComparison.InvariantCultureIgnoreCase))
                        {
                            siteMapCidList.Add(cid);
                        }

                        BaseData baseData = new BaseData();
                        baseData.Id = cid;
                        string LocalName = idr["LocalName"].ToString();
                        if (String.IsNullOrEmpty(LocalName))
                            LocalName = idr["CategoryName"].ToString();

                        baseData.Value = LocalName;
                        string isSearchOnly = idr["isSearchOnly"].ToString();

                        if (isSiteMapDetail.Equals("true", StringComparison.InvariantCultureIgnoreCase))
                        {
                            baseData.NeedCheck = true;
                        }
                        else
                        {
                            baseData.NeedCheck = false;
                        }

                        if (isSearchOnly.Equals("true", StringComparison.InvariantCultureIgnoreCase))
                        {
                            isSearchOnlyCategories.Add(cid);
                        }

                        categoryDataList.Add(baseData);
                    }
                    idr.Close();

                    Dictionary<string, CategoryDataInfo> categoryDataInfoDictionary = new Dictionary<string, CategoryDataInfo>();

                    string selectC = " (P.IsMerge = 1 or C.IsDisplayIsMerged = 1) and ";
                    if (subDbInfo.CountryId == 25)
                    {
                        selectC = "";
                    }

                    query = @"SELECT
	                        P.ProductID, P.ProductName, P.DefaultImage, C.CategoryName, P.ManufacturerID, P.CategoryID, P.Keywords, M.ManufacturerName, C.IsAccessories, RP.rpminprice
                        FROM
	                        CSK_Store_ProductNew P
	                        LEFT JOIN CSK_Store_Category C ON P.CategoryID = C.CategoryID 
	                        INNER JOIN CSK_Store_Manufacturer M on P.ManufacturerID = M.ManufacturerID
                            Inner join (SELECT ProductID, min(RetailerPrice) as rpminprice FROM CSK_Store_RetailerProductNew
                        WHERE RetailerId in (select RetailerId from CSK_Store_Retailer where RetailerStatus <> 99)
                        and RetailerId in (select RetailerId from CSK_Store_PPCMember where PPCMemberTypeID = 2)
                        group by ProductID) as RP on RP.ProductId = P.ProductId
                        WHERE " + selectC + " C.IsActive = 1 and C.IsSiteMap = 0 and C.IsSiteMapDetail = 0";

                    if (rootCategoryIds != "0")
                    {
                        query += categoryQueryString;
                    }

                    var cmd = DBController.CreateDbCommand(query, sqlConn);

                    Console.WriteLine("exec sql : " + query);

                    idr = cmd.ExecuteReader();

                    List<string> hasProductCategoryList = new List<string>();
                    Console.WriteLine("Write Product Index...");
                    List<int> accessroiesCIDs = new List<int>();

                    while (idr.Read())
                    {
                        dbProductCount++;
                        bool isAccessroies = true;

                        if (!bool.TryParse(idr["IsAccessories"].ToString(), out isAccessroies))
                        {
                            isAccessroies = true;
                        }

                        string categoryName = idr["CategoryName"].ToString();//需要翻译

                        string cid = idr["CategoryID"].ToString();

                        int categoryID = int.Parse(cid);

                        string categorySynonym = "";
                        if (categorySynonymDic.ContainsKey(categoryID))
                        {
                            categorySynonym = categorySynonymDic[categoryID];
                        }

                        string localName = GetLocalName(cid, categoryDataList);
                        if (!string.IsNullOrEmpty(localName))
                        {
                            categoryName = localName;
                        }

                        CategoryDataInfo categoryDataInfo = null;
                        if (categoryDataInfoDictionary.ContainsKey(cid))
                        {
                            categoryDataInfo = categoryDataInfoDictionary[cid];
                        }
                        else
                        {
                            categoryDataInfo = new CategoryDataInfo();
                            categoryDataInfoDictionary.Add(cid, categoryDataInfo);
                        }
                        categoryDataInfo.ProductCount++;

                        if (subDbInfo.CountryId != 25)
                        {
                            if (isSearchOnlyCategories.Contains(cid))
                                continue;
                        }

                        if (!isAccessroies || subDbInfo.CountryId == 25)
                        {
                            string manufacturerName = idr["ManufacturerName"].ToString();
                            if (manufacturerName.Equals("na", StringComparison.InvariantCultureIgnoreCase))
                            {
                                manufacturerName = "";
                            }
                            string pk = idr["Keywords"].ToString();
                            string productName = idr["ProductName"].ToString();
                            float minPrice = 0;
                            float.TryParse(idr["rpminprice"].ToString(), out minPrice);//
                            string defaultImage = idr["DefaultImage"].ToString();

                            if (minPrice < 0.1)
                            {
                                Console.WriteLine("Min price : " + minPrice.ToString("0.00"));
                                continue;
                            }

                            int productID = int.Parse(idr["ProductID"].ToString());

                            string minPriceString = PriceIntCultureString(double.Parse(minPrice.ToString(), System.Globalization.NumberStyles.Any, Provider_Static), subDbInfo.CountryId, CurrentCulture_Static);
                            keyword = GetKeywords(categoryName, manufacturerName, pk, productName, categorySynonym);

                            Document doc = new Document();

                            doc.Add(new StoredField("ID", productID));//doc.Add(new Field("ID", productID.ToString(), Field.Store.YES, Field.Index.NO));
                            doc.Add(new TextField("Name", keyword, Field.Store.NO));//doc.Add(new Field("Name", keyword, Field.Store.NO, Field.Index.ANALYZED));
                            doc.Add(new StoredField("DisplayValue", productName));//doc.Add(new Field("DisplayValue", productName, Field.Store.YES, Field.Index.NO));
                            doc.Add(new StoredField("Type", "P"));//doc.Add(new Field("Type", "P", Field.Store.YES, Field.Index.NO));
                            doc.Add(new StoredField("Other", categoryName));//doc.Add(new Field("Other", categoryName, Field.Store.YES, Field.Index.NO));
                            doc.Add(new StoredField("ListOrder", "3"));//doc.Add(new Field("ListOrder", "3", Field.Store.YES, Field.Index.NO));
                                                                       /*doc.Add(new Field("IncludePPC", ppc.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));//doc.Add(new StringField("IncludePPC", ppc.ToString(), Field.Store.YES));*/
                            doc.Add(new StringField("CategoryID", cid, Field.Store.YES));//doc.Add(new Field("IncludePPC", "1", Field.Store.YES, Field.Index.NOT_ANALYZED));
                            doc.Add(new StoredField("DefaultImage", defaultImage));//doc.Add(new Field("DefaultImage", defaultImage, Field.Store.YES, Field.Index.NO));                                                                              //int clicks = 0;
                            int clicks = 0;
                            if (productClicksDic.ContainsKey(productID))
                            {
                                clicks = productClicksDic[productID];
                            }

                            if (categoryClicksDic.ContainsKey(cid))
                            {
                                clicks += categoryClicksDic[cid];
                            }
                            //NumericField clicksField = new NumericField("Clicks", Field.Store.YES, true);
                            //clicksField.SetIntValue(clicks);
                            //doc.Add(clicksField);
                            doc.Add(new Int32Field("Clicks", clicks, Field.Store.YES));
                            doc.Add(new StoredField("MinPrice", minPriceString));//doc.Add(new Field("MinPrice", minPriceString, Field.Store.YES, Field.Index.NO));
                            if (rpInfoDic.ContainsKey(productID))
                            {
                                RPInfo rpInfo = rpInfoDic[productID];
                                doc.Add(new StoredField("PPC_MinPrice", rpInfo.MinPrice.ToString("0.00")));//doc.Add(new Field("PPC_MinPrice", rpInfo.MinPrice.ToString("0.00"), Field.Store.YES, Field.Index.NO));
                                doc.Add(new StoredField("PPC_RetailerProductID", rpInfo.RetailerProductID));//doc.Add(new Field("PPC_RetailerProductID", rpInfo.RetailerProductID.ToString(), Field.Store.YES, Field.Index.NO));
                                doc.Add(new StoredField("PPC_RetailerID", rpInfo.RetailerID));//doc.Add(new Field("PPC_RetailerID", rpInfo.RetailerID.ToString(), Field.Store.YES, Field.Index.NO));
                            }

                            float orderScroe = 0;
                            if (isSearchOnlyCategories.Contains(cid))
                            {
                                orderScroe = 1;
                            }
                            else
                            {
                                orderScroe = 5;
                            }
                            doc.Add(new SingleField("Order", orderScroe, Field.Store.NO));

                            idw.AddDocument(doc);
                            indexProductCount++;

                            categoryDataInfo.PPCProductCount++;
                        }
                        else
                        {
                            if (!accessroiesCIDs.Contains(categoryID))
                            {
                                accessroiesCIDs.Add(categoryID);
                            }
                        }

                        string manufacturerID = idr["ManufacturerID"].ToString();
                        if (!hasProductCategoryList.Contains(cid))
                        {
                            hasProductCategoryList.Add(cid);
                        }
                        if (!siteMapCidList.Contains(cid) && !isSearchOnlyCategories.Contains(cid))
                        {
                            AddDictionary(dictionary, cid, manufacturerID, categoryName, 1);
                        }
                    }
                    idr.Close();

                    Dictionary<int, List<ProductCatalog>> ucpDic = GetUpComingProductDic(priceme205DbInfo, pamUserDbInfo);
                    if (ucpDic != null)
                    {
                        foreach (int key in ucpDic.Keys)
                        {
                            foreach (ProductCatalog pc in ucpDic[key])
                            {
                                string localName = GetLocalName(pc.CategoryID.ToString(), categoryDataList);
                                string categorySynonym = "";
                                if (categorySynonymDic.ContainsKey(pc.CategoryID))
                                {
                                    categorySynonym = categorySynonymDic[pc.CategoryID];
                                }

                                string kw = GetKeywords(localName, pc.RetailerProductInfoString, "", pc.ProductName, categorySynonym);

                                Document doc = new Document();
                                doc.Add(new StoredField("ID", pc.ProductID));
                                doc.Add(new TextField("Name", kw, Field.Store.NO));
                                doc.Add(new StoredField("DisplayValue", pc.ProductName));
                                doc.Add(new StoredField("Type", "UCP"));
                                doc.Add(new StoredField("Other", localName));
                                doc.Add(new StoredField("ListOrder", "3"));
                                doc.Add(new StringField("CategoryID", pc.CategoryID.ToString(), Field.Store.YES));
                                doc.Add(new StoredField("DefaultImage", pc.DefaultImage));
                                doc.Add(new Int32Field("Clicks", 0, Field.Store.YES));
                                doc.Add(new SingleField("Order", 4, Field.Store.NO));

                                //doc.Add(new Field("ID", pc.ProductID, Field.Store.YES, Field.Index.NO));
                                //doc.Add(new Field("Name", kw, Field.Store.NO, Field.Index.ANALYZED));
                                //doc.Add(new Field("DisplayValue", pc.ProductName, Field.Store.YES, Field.Index.NO));
                                //doc.Add(new Field("Type", "UCP", Field.Store.YES, Field.Index.NO));
                                //doc.Add(new Field("Other", localName, Field.Store.YES, Field.Index.NO));
                                //doc.Add(new Field("ListOrder", "3", Field.Store.YES, Field.Index.NO));
                                //doc.Add(new Field("CategoryID", pc.CategoryID.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
                                //doc.Add(new Field("DefaultImage", pc.DefaultImage, Field.Store.YES, Field.Index.NO));
                                //NumericField clicksField = new NumericField("Clicks", Field.Store.YES, true);
                                //clicksField.SetIntValue(0);
                                //doc.Add(clicksField);

                                //float orderScroe = 4;
                                //NumericField orderField = new NumericField("Order", Field.Store.NO, true);
                                //orderField.SetFloatValue(orderScroe);
                                //doc.Add(orderField);

                                idw.AddDocument(doc);
                                indexUpComingProductCount++;
                            }
                        }
                    }

                    List<ProductCatalog> offerProducts = GetOfferProducts(subDbInfo.CountryId, pamUserDbInfo);
                    foreach (ProductCatalog pc in offerProducts)
                    {
                        string categorySynonym = "";
                        string localName = GetLocalName(pc.CategoryID.ToString(), categoryDataList);
                        if (categorySynonymDic.ContainsKey(pc.CategoryID))
                        {
                            categorySynonym = categorySynonymDic[pc.CategoryID];
                        }

                        string kw = GetKeywords(localName, "", "", pc.ProductName, categorySynonym);

                        Document doc = new Document();

                        doc.Add(new StoredField("ID", pc.ProductID));
                        doc.Add(new TextField("Name", kw, Field.Store.NO));
                        doc.Add(new StoredField("DisplayValue", pc.ProductName));
                        doc.Add(new StoredField("Type", "Offer"));
                        doc.Add(new StoredField("Other", localName));
                        doc.Add(new StoredField("ListOrder", "3"));
                        doc.Add(new StringField("CategoryID", pc.CategoryID.ToString(), Field.Store.YES));
                        doc.Add(new StoredField("DefaultImage", pc.DefaultImage));
                        doc.Add(new Int32Field("Clicks", -1, Field.Store.YES));

                        string minPriceString = PriceIntCultureString(double.Parse(pc.BestPrice, System.Globalization.NumberStyles.Any, Provider_Static), subDbInfo.CountryId, CurrentCulture_Static);
                        doc.Add(new StoredField("MinPrice", minPriceString));
                        doc.Add(new SingleField("Order", 4, Field.Store.NO));

                        //doc.Add(new Field("ID", pc.ProductID, Field.Store.YES, Field.Index.NO));
                        //doc.Add(new Field("Name", kw, Field.Store.NO, Field.Index.ANALYZED));
                        //doc.Add(new Field("DisplayValue", pc.ProductName, Field.Store.YES, Field.Index.NO));
                        //doc.Add(new Field("Type", "Offer", Field.Store.YES, Field.Index.NO));
                        //doc.Add(new Field("Other", localName, Field.Store.YES, Field.Index.NO));
                        //doc.Add(new Field("ListOrder", "3", Field.Store.YES, Field.Index.NO));
                        //doc.Add(new Field("CategoryID", pc.CategoryID.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
                        //doc.Add(new Field("DefaultImage", pc.DefaultImage, Field.Store.YES, Field.Index.NO));
                        //NumericField clicksField = new NumericField("Clicks", Field.Store.YES, true);
                        //clicksField.SetIntValue(-1);
                        //doc.Add(clicksField);
                        //string minPriceString = PriceIntCultureString(double.Parse(pc.BestPrice, System.Globalization.NumberStyles.Any, Provider_Static), subDbInfo.CountryId, CurrentCulture_Static);
                        //doc.Add(new Field("MinPrice", minPriceString, Field.Store.YES, Field.Index.NO));

                        //float orderScroe = 4;
                        //NumericField orderField = new NumericField("Order", Field.Store.NO, true);
                        //orderField.SetFloatValue(orderScroe);
                        //doc.Add(orderField);

                        idw.AddDocument(doc);
                        indexOfferProductCount++;
                    }

                    Console.WriteLine("Write Category Index...");
                    foreach (var data in categoryDataList)
                    {
                        if (hasProductCategoryList.Contains(data.Id) || HasProduct(data, hasProductCategoryList))
                        {
                            if (isSearchOnlyCategories.Contains(data.Id))
                            {
                                continue;
                            }

                            Document doc = new Document();

                            int categoryID = int.Parse(data.Id);
                            string categorySynonym = "";
                            if (categorySynonymDic.ContainsKey(categoryID))
                            {
                                categorySynonym = categorySynonymDic[categoryID];
                            }

                            keyword = data.Value.ToLower().Trim() + " " + categorySynonym;

                            doc.Add(new StoredField("ID", data.Id));
                            doc.Add(new TextField("Name", keyword, Field.Store.NO));
                            doc.Add(new StoredField("DisplayValue", data.Value));
                            doc.Add(new StoredField("Type", "C"));
                            doc.Add(new StoredField("Other", ""));
                            doc.Add(new StoredField("ListOrder", "1"));
                            doc.Add(new StringField("CategoryID", data.Id, Field.Store.YES));
                            doc.Add(new StoredField("DefaultImage", ""));
                            doc.Add(new Int32Field("Clicks", int.MaxValue, Field.Store.YES));
                            string ppcStr = "0";
                            if (categoryDataInfoDictionary.ContainsKey(data.Id) && categoryDataInfoDictionary[data.Id].PPCProductCount > 0)
                            {
                                ppcStr = "1";
                            }
                            doc.Add(new StringField("IncludePPC", ppcStr, Field.Store.YES));

                            float orderScroe = 0;
                            if (isSearchOnlyCategories.Contains(categoryID.ToString()))
                            {
                                orderScroe = 4.5f;
                            }
                            else
                            {
                                orderScroe = 5;
                            }
                            doc.Add(new SingleField("Order", orderScroe, Field.Store.NO));

                            //doc.Add(new Field("ID", data.Id, Field.Store.YES, Field.Index.NO));
                            //doc.Add(new Field("Name", keyword, Field.Store.NO, Field.Index.ANALYZED));
                            //doc.Add(new Field("DisplayValue", data.Value, Field.Store.YES, Field.Index.NO));
                            //doc.Add(new Field("Type", "C", Field.Store.YES, Field.Index.NO));
                            //doc.Add(new Field("Other", "", Field.Store.YES, Field.Index.NO));
                            //doc.Add(new Field("ListOrder", "1", Field.Store.YES, Field.Index.NO));
                            //doc.Add(new Field("CategoryID", data.Id, Field.Store.YES, Field.Index.NOT_ANALYZED));
                            //doc.Add(new Field("DefaultImage", "", Field.Store.YES, Field.Index.NO));
                            //NumericField clicksField = new NumericField("Clicks", Field.Store.YES, true);
                            //clicksField.SetIntValue(int.MaxValue);
                            //doc.Add(clicksField);
                            //string ppcStr = "0";
                            //if (categoryDataInfoDictionary.ContainsKey(data.Id) && categoryDataInfoDictionary[data.Id].PPCProductCount > 0)
                            //{
                            //    ppcStr = "1";
                            //}
                            //doc.Add(new Field("IncludePPC", ppcStr, Field.Store.YES, Field.Index.NOT_ANALYZED));

                            //float orderScroe = 0;
                            //if (isSearchOnlyCategories.Contains(categoryID.ToString()))
                            //{
                            //    orderScroe = 4.5f;
                            //}
                            //else
                            //{
                            //    orderScroe = 5;
                            //}
                            //NumericField orderField = new NumericField("Order", Field.Store.NO, true);
                            //orderField.SetFloatValue(orderScroe);
                            //doc.Add(orderField);

                            idw.AddDocument(doc);
                            indexCategoryCount++;
                        }
                    }

                    var _retailerList = GetAllRetailersOrderByName(subDbInfo);
                    var _retailerVotes = GetRetailerVotesSums(priceme205DbInfo, subDbInfo.CountryId);
                    foreach (var item in _retailerList)
                    {
                        var vote = _retailerVotes.FirstOrDefault(v => v.RetailerID == item.RetailerId);
                        if (vote != null)
                        {
                            item.RetailerRatingSum = vote.RetailerRatingSum;
                            item.RetailerTotalRatingVotes = vote.RetailerTotalRatingVotes;
                        }
                        else
                        {
                            item.RetailerRatingSum = 3;
                            item.RetailerTotalRatingVotes = 1;
                        }
                    }

                    Console.WriteLine("Write Retailer Index...");
                    foreach (var retailer in _retailerList)
                    {

                        Document doc = new Document();
                        keyword = InvalidCharacter_Static.Replace(retailer.RetailerName.ToLower().Trim(), "");

                        double avRating = (double)retailer.AvRating;
                        string ratingImageUrl = GetStarImageUrl(avRating);

                        doc.Add(new StoredField("ID", retailer.RetailerId));
                        doc.Add(new TextField("Name", FixKeywords(keyword), Field.Store.NO));
                        doc.Add(new StoredField("DisplayValue", retailer.RetailerName));
                        doc.Add(new StoredField("Type", "R"));
                        doc.Add(new StoredField("Other", ratingImageUrl));
                        doc.Add(new StringField("IncludePPC", "1", Field.Store.YES));
                        doc.Add(new StringField("CategoryID", "-1", Field.Store.YES));
                        doc.Add(new StoredField("DefaultImage", ""));
                        doc.Add(new Int32Field("Clicks", 1000000, Field.Store.YES));
                        string listOrderStr = "1";
                        if (retailer.RetailerStatus == 99)
                        {
                            listOrderStr = "-1";
                        }
                        doc.Add(new StoredField("ListOrder", listOrderStr));
                        doc.Add(new SingleField("Order", 4, Field.Store.NO));

                        //doc.Add(new Field("ID", retailer.RetailerId.ToString(), Field.Store.YES, Field.Index.NO));
                        //doc.Add(new Field("Name", FixKeywords(keyword), Field.Store.NO, Field.Index.ANALYZED));
                        //doc.Add(new Field("DisplayValue", retailer.RetailerName, Field.Store.YES, Field.Index.NO));
                        //doc.Add(new Field("Type", "R", Field.Store.YES, Field.Index.NO));
                        //doc.Add(new Field("Other", ratingImageUrl, Field.Store.YES, Field.Index.NO));
                        //doc.Add(new Field("IncludePPC", "1", Field.Store.YES, Field.Index.NOT_ANALYZED));
                        //doc.Add(new Field("CategoryID", "-1", Field.Store.YES, Field.Index.NOT_ANALYZED));
                        //doc.Add(new Field("DefaultImage", "", Field.Store.YES, Field.Index.NO));
                        //NumericField clicksField = new NumericField("Clicks", Field.Store.YES, true);
                        //clicksField.SetIntValue(1000000);
                        //doc.Add(clicksField);
                        //string listOrderStr = "1";
                        //if (retailer.RetailerStatus == 99)
                        //{
                        //    listOrderStr = "-1";
                        //}
                        //doc.Add(new Field("ListOrder", listOrderStr, Field.Store.YES, Field.Index.NO));

                        //float orderScroe = 4;
                        //NumericField orderField = new NumericField("Order", Field.Store.NO, true);
                        //orderField.SetFloatValue(orderScroe);
                        //doc.Add(orderField);

                        idw.AddDocument(doc);
                        indexRetailerCount++;
                    }
                    idr.Close();

                    query = @"
					SELECT ManufacturerID, ManufacturerName, IsPopular FROM CSK_Store_Manufacturer 
					WHERE ManufacturerID in (select manufacturerID from CSK_Store_Product)
				";
                    cmd.CommandText = query;

                    idr = cmd.ExecuteReader();
                    Console.WriteLine("Write Brand Index...");
                    while (idr.Read())
                    {
                        Document doc = new Document();
                        keyword = idr["ManufacturerName"].ToString().ToLower().Trim();

                        string mid = idr["ManufacturerID"].ToString();
                        if (!dictionary.ContainsKey(mid))
                        {
                            continue;
                        }

                        doc.Add(new StoredField("ID", mid));
                        doc.Add(new TextField("Name", FixKeywords(keyword), Field.Store.NO));
                        doc.Add(new StoredField("DisplayValue", idr["ManufacturerName"].ToString()));
                        doc.Add(new StoredField("Type", "M"));
                        doc.Add(new StoredField("Other", idr["IsPopular"].ToString()));
                        doc.Add(new StoredField("ListOrder", "2"));
                        doc.Add(new StringField("IncludePPC", "1", Field.Store.YES));
                        doc.Add(new StringField("CategoryID", "-1", Field.Store.YES));
                        doc.Add(new StoredField("DefaultImage", ""));
                        doc.Add(new Int32Field("Clicks", 1000000, Field.Store.YES));
                        doc.Add(new SingleField("Order", 3, Field.Store.NO));

                        //doc.Add(new Field("ID", mid, Field.Store.YES, Field.Index.NO));
                        //doc.Add(new Field("Name", FixKeywords(keyword), Field.Store.NO, Field.Index.ANALYZED));
                        //doc.Add(new Field("DisplayValue", idr["ManufacturerName"].ToString(), Field.Store.YES, Field.Index.NO));
                        //doc.Add(new Field("Type", "M", Field.Store.YES, Field.Index.NO));
                        //doc.Add(new Field("Other", idr["IsPopular"].ToString(), Field.Store.YES, Field.Index.NO));
                        //doc.Add(new Field("ListOrder", "2", Field.Store.YES, Field.Index.NO));
                        //doc.Add(new Field("IncludePPC", "1", Field.Store.YES, Field.Index.NOT_ANALYZED));
                        //doc.Add(new Field("CategoryID", "-1", Field.Store.YES, Field.Index.NOT_ANALYZED));
                        //doc.Add(new Field("DefaultImage", "", Field.Store.YES, Field.Index.NO));
                        //NumericField clicksField = new NumericField("Clicks", Field.Store.YES, true);
                        //clicksField.SetIntValue(1000000);
                        //doc.Add(clicksField);
                        //float orderScroe = 3;
                        //NumericField orderField = new NumericField("Order", Field.Store.NO, true);
                        //orderField.SetFloatValue(orderScroe);
                        //doc.Add(orderField);

                        if (dictionary.ContainsKey(mid))
                        {
                            int midInt = 0;
                            if (!int.TryParse(mid, out midInt))
                            {
                                Console.WriteLine("mid : " + mid + "try parse failed!");
                            }
                            else
                            {
                                List<int> brandList = new List<int>();
                                brandList.Add(midInt);
                                foreach (var cdi in dictionary[mid])
                                {
                                    int _cid = 0;
                                    if (!int.TryParse(cdi.CategoryID, out _cid))
                                    {
                                        Console.WriteLine("cdi.Id : " + cdi.CategoryID + "try parse failed!");
                                        continue;
                                    }

                                    if (accessroiesCIDs.Contains(_cid))
                                        continue;

                                    if (isSearchOnlyCategories.Contains(_cid.ToString()))
                                    {
                                        continue;
                                    }

                                    string categorySynonym = "";
                                    if (categorySynonymDic.ContainsKey(_cid))
                                    {
                                        categorySynonym = categorySynonymDic[_cid];
                                    }

                                    int pCount = 0;
                                    //ProductSearcher productSearcher = new ProductSearcher("", _cid, brandList, null, null, null, "", null, 10000, CountryId_Static, false, true, true, true, null, "", null);
                                    //int pCount = productSearcher.GetProductCount();
                                    //if (pCount == 0)
                                    //    continue;

                                    string brandsAndCategoryName = idr["ManufacturerName"].ToString().Trim() + " " + cdi.CategoryName.Trim();
                                    Document docBaC = new Document();

                                    float orderScroe = 0;
                                    if (isSearchOnlyCategories.Contains(_cid.ToString()))
                                    {
                                        orderScroe = 1;
                                    }
                                    else
                                    {
                                        orderScroe = 4.5f;
                                    }

                                    docBaC.Add(new StoredField("ID", mid + "," + _cid));
                                    docBaC.Add(new TextField("Name", FixKeywords(brandsAndCategoryName.ToLower() + " " + categorySynonym), Field.Store.NO));
                                    docBaC.Add(new StoredField("DisplayValue", brandsAndCategoryName));
                                    docBaC.Add(new StoredField("Type", "BAC"));
                                    docBaC.Add(new StoredField("Other", cdi.ProductCount.ToString()));
                                    docBaC.Add(new StoredField("ListOrder", "5"));
                                    docBaC.Add(new StringField("IncludePPC", "1", Field.Store.YES));
                                    docBaC.Add(new StringField("CategoryID", _cid.ToString(), Field.Store.YES));
                                    docBaC.Add(new StoredField("DefaultImage", ""));
                                    docBaC.Add(new StoredField("IndexProductCount", pCount));
                                    //docBaC.Add(new Int32Field("Clicks", productSearcher.GetClickCount(), Field.Store.YES));
                                    docBaC.Add(new Int32Field("Clicks", 10, Field.Store.YES));
                                    doc.Add(new SingleField("Order", orderScroe, Field.Store.NO));

                                    //docBaC.Add(new Field("ID", mid + "," + _cid, Field.Store.YES, Field.Index.NO));
                                    //docBaC.Add(new Field("Name", FixKeywords(brandsAndCategoryName.ToLower() + " " + categorySynonym), Field.Store.NO, Field.Index.ANALYZED));
                                    //docBaC.Add(new Field("DisplayValue", brandsAndCategoryName, Field.Store.YES, Field.Index.NO));
                                    //docBaC.Add(new Field("Type", "BAC", Field.Store.YES, Field.Index.NO));
                                    //docBaC.Add(new Field("Other", cdi.ProductCount.ToString(), Field.Store.YES, Field.Index.NO));
                                    //docBaC.Add(new Field("ListOrder", "5", Field.Store.YES, Field.Index.NO));
                                    //docBaC.Add(new Field("IncludePPC", "1", Field.Store.YES, Field.Index.NOT_ANALYZED));
                                    //docBaC.Add(new Field("CategoryID", _cid.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
                                    //docBaC.Add(new Field("DefaultImage", "", Field.Store.YES, Field.Index.NO));
                                    //docBaC.Add(new Field("IndexProductCount", pCount.ToString(), Field.Store.YES, Field.Index.NO));
                                    //NumericField bacClicksField = new NumericField("Clicks", Field.Store.YES, true);
                                    //bacClicksField.SetIntValue(productSearcher.GetClickCount());
                                    //docBaC.Add(bacClicksField);

                                    //orderField = new NumericField("Order", Field.Store.NO, true);
                                    //orderField.SetFloatValue(orderScroe);
                                    //doc.Add(orderField);

                                    idw.AddDocument(docBaC);
                                    indexindexBrandAndCategoryCount++;
                                }
                            }
                        }

                        idw.AddDocument(doc);
                        indexBrandCount++;
                    }

                    idr.Close();

                    query = "SELECT DisplayValue,Url,OtherInfo,Type FROM PopularSearch";
                    cmd.CommandText = query;

                    idr = cmd.ExecuteReader();
                    while (idr.Read())
                    {
                        Document doc = new Document();
                        string name = idr["DisplayValue"].ToString();
                        keyword = InvalidCharacter_Static.Replace(name.ToLower().Trim(), "");

                        string url = idr["Url"].ToString().Trim();
                        string otherInfo = idr["OtherInfo"].ToString().Trim();
                        int type = int.Parse(idr["Type"].ToString().Trim());

                        doc.Add(new StoredField("ID", "0"));
                        doc.Add(new TextField("Name", keyword, Field.Store.NO));
                        doc.Add(new StoredField("DisplayValue", name));
                        doc.Add(new StoredField("Type", "C1"));
                        doc.Add(new StoredField("Other", url));
                        doc.Add(new StoredField("ListOrder", "1"));
                        doc.Add(new StringField("IncludePPC", "1", Field.Store.YES));
                        doc.Add(new StringField("CategoryID", "0", Field.Store.YES));
                        doc.Add(new StoredField("DefaultImage", ""));
                        doc.Add(new Int32Field("Clicks", int.MaxValue, Field.Store.YES));
                        doc.Add(new SingleField("Order", 3, Field.Store.NO));

                        //doc.Add(new Field("ID", "0", Field.Store.YES, Field.Index.NO));
                        //doc.Add(new Field("Name", keyword, Field.Store.NO, Field.Index.ANALYZED));
                        //doc.Add(new Field("DisplayValue", name, Field.Store.YES, Field.Index.NO));
                        //doc.Add(new Field("Type", "C1", Field.Store.YES, Field.Index.NO));
                        //doc.Add(new Field("Other", url, Field.Store.YES, Field.Index.NO));
                        //doc.Add(new Field("ListOrder", "1", Field.Store.YES, Field.Index.NO));
                        //doc.Add(new Field("IncludePPC", "1", Field.Store.YES, Field.Index.NOT_ANALYZED));
                        //doc.Add(new Field("CategoryID", "0", Field.Store.YES, Field.Index.NOT_ANALYZED));
                        //doc.Add(new Field("DefaultImage", "", Field.Store.YES, Field.Index.NO));
                        //NumericField clicksField = new NumericField("Clicks", Field.Store.YES, true);
                        //clicksField.SetIntValue(int.MaxValue);
                        //doc.Add(clicksField);

                        //float orderScroe = 3;
                        //NumericField orderField = new NumericField("Order", Field.Store.NO, true);
                        //orderField.SetFloatValue(orderScroe);
                        //doc.Add(orderField);

                        idw.AddDocument(doc);
                        indexPopularSearchCount++;
                    }
                }
            }

            LogController.WriteLog("------------------------------");
            LogController.WriteLog("dbProductCount : " + dbProductCount);
            LogController.WriteLog("IndexProductCount : " + indexProductCount);
            LogController.WriteLog("IndexUpComingProductCount : " + indexUpComingProductCount);
            LogController.WriteLog("IndexCategoryCount : " + indexCategoryCount);
            LogController.WriteLog("IndexRetailerCount : " + indexRetailerCount);
            LogController.WriteLog("IndexBrandCount : " + indexBrandCount);
            LogController.WriteLog("IndexindexBrandAndCategoryCount : " + indexindexBrandAndCategoryCount);
            LogController.WriteLog("IndexPopularSearchCount : " + indexPopularSearchCount);
            LogController.WriteLog("IndexOfferProductCount : " + indexOfferProductCount);
        }

        private static List<ProductCatalog> GetOfferProducts(int countryId, DbInfo pamUserDbInfo)
        {
            List<ProductCatalog> list = new List<ProductCatalog>();
            string selectSql = @"SELECT OfferId
                                ,OfferName
                                ,Price
                                ,CategoryID
                                ,DefaultImage
                                FROM CSK_Store_RetailerOffer
                                where Status = 1 and Price > 0.2 and 
                                RetailerId in (select retailerId from CSK_Store_Retailer where IsSetupComplete = 1 and RetailerStatus = 1 and RetailerCountry = " + countryId + " and RetailerId in (select RetailerId from CSK_Store_PPCMember where PPCMemberTypeID = 2))";

            using (var sqlConn = DBController.CreateDBConnection(pamUserDbInfo))
            {
                using (var sqlCMD = DBController.CreateDbCommand(selectSql, sqlConn))
                {
                    sqlConn.Open();
                    using (var sdr = sqlCMD.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            ProductCatalog pc = new ProductCatalog();
                            int offerId = sdr.GetInt32(0);
                            pc.ProductID = offerId.ToString();
                            pc.ProductName = sdr.GetString(1);
                            pc.BestPrice = sdr.GetDecimal(2).ToString("0.00");
                            pc.CategoryID = sdr.GetInt32(3);
                            pc.DefaultImage = sdr["DefaultImage"].ToString();
                            list.Add(pc);
                        }
                    }
                }
            }

            return list;
        }

        private static Dictionary<int, string> GetCategorySynonymDic(int countryId, DbInfo pamUserDbInfo)
        {
            Dictionary<int, string> dic = new Dictionary<int, string>();

            string sql = "SELECT CategoryID,Synonym FROM CategorySynonym_New where CountryID = " + countryId;
            using (var sqlConn = DBController.CreateDBConnection(pamUserDbInfo))
            {
                using (var sqlCMD = DBController.CreateDbCommand(sql, sqlConn))
                {
                    sqlConn.Open();
                    using (var sqlDR = sqlCMD.ExecuteReader())
                    {
                        while (sqlDR.Read())
                        {
                            int cid = sqlDR.GetInt32(0);
                            string synonym = sqlDR.GetString(1).ToLower();
                            if (!dic.ContainsKey(cid))
                            {
                                dic.Add(cid, synonym);
                            }
                            else
                            {
                                dic[cid] = dic[cid] + " " + synonym;
                            }
                        }
                    }
                }
            }

            return dic;
        }

        private static void SetClicksDic(int clickDays, DbInfo dbInfo)
        {
            categoryClicksDic = new Dictionary<string, int>();
            productClicksDic = new Dictionary<int, int>();

            string sql = "SELECT ProductId, min(categoryId), count(id) as c from csk_store_retailertracker where ClickTime > ";

            using (var sqlConn = DBController.CreateDBConnection(dbInfo))
            {
                if (sqlConn is MySql.Data.MySqlClient.MySqlConnection)
                {
                    sql += " date_add(Now(),  interval -" + clickDays + " day) group by ProductId;";
                }
                else
                {
                    sql = " CONVERT(varchar(100), dateadd(day,-" + clickDays + ",GETDATE()), 23) group by ProductId";
                }
                using (var sqlCMD = DBController.CreateDbCommand(sql, sqlConn))
                {
                    sqlConn.Open();
                    using (var sqlDR = sqlCMD.ExecuteReader())
                    {
                        while (sqlDR.Read())
                        {
                            int pid = sqlDR.GetInt32(0);
                            string cid = sqlDR.GetInt32(1).ToString();
                            
                            int clicks = sqlDR.GetInt32(2);
                            productClicksDic.Add(pid, clicks);

                            if (categoryClicksDic.ContainsKey(cid))
                            {
                                categoryClicksDic[cid] += clicks;
                            }
                            else
                            {
                                categoryClicksDic.Add(cid, clicks);
                            }
                        }
                    }
                }
            }
        }

        private static string GetLocalName(string cid, List<BaseData> categoryDataList)
        {
            foreach (var bd in categoryDataList)
            {
                if (bd.Id == cid)
                {
                    return bd.Value;
                }
            }
            return "";
        }

        public static string FixKeywords(string kw)
        {
            string[] pNKws = kw.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            string newPN = "";
            foreach (string pnKW in pNKws)
            {
                newPN += pnKW + " ";
                //让包含 ' 号的关键字不用 ' 也能搜索到
                if (pnKW.Contains("'"))
                {
                    newPN += pnKW.Replace("'", "") + " ";
                }
                //让包含 - 号的关键字不用 - 也能搜索到
                if (pnKW.Contains("-"))
                {
                    newPN += pnKW.Replace("-", "") + " ";
                    newPN += pnKW.Replace("-", " ") + " ";
                }
            }
            return newPN;
        }

        public static string GetKeywords(string categoryName, string manufacturerName, string keywords, string productName, string otherKeywords)
        {
            string pN = productName.Replace("&", " ").Replace(",", " ").Replace("_", " ").ToLower();
            pN = FixKeywords(pN);

            string[] mN = manufacturerName.Replace("&", " ").ToLower().Split(' ');
            string[] others = otherKeywords.Replace("&", " ").Replace(",", " ").Replace("-", " ").ToLower().Split(' ');
            string kw = keywords == null ? "" : keywords.ToLower().Replace("&", " ").Replace(",", " ").Replace(":", " ");

            categoryName = categoryName.ToLower();

            if (categoryName.Equals("Tail Pads", StringComparison.InvariantCultureIgnoreCase)
                || categoryName.Equals("Eyeglasses", StringComparison.InvariantCultureIgnoreCase))
            {
                kw += " " + categoryName;
            }
            else
            {
                string[] cNs = categoryName.Split(new char[] { '&', ',' }, StringSplitOptions.RemoveEmptyEntries);

                foreach (string cn in cNs)
                {
                    string[] subCN = categoryName.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string str in subCN)
                    {
                        if (str.Equals("Accessories", StringComparison.InvariantCultureIgnoreCase)
                        || str.Equals("Product", StringComparison.InvariantCultureIgnoreCase)
                        || str.Equals("Products", StringComparison.InvariantCultureIgnoreCase))
                        {
                            continue;
                        }
                        kw += " " + str;
                    }
                    string lastName = cNs[cNs.Length - 1];

                    if (lastName.Equals("Jeans", StringComparison.InvariantCultureIgnoreCase)
                        || lastName.Equals("Gas", StringComparison.InvariantCultureIgnoreCase)
                        || lastName.Equals("Glasses", StringComparison.InvariantCultureIgnoreCase)
                        || lastName.EndsWith("ss")
                        || lastName.Equals("Accessories", StringComparison.InvariantCultureIgnoreCase)
                        || lastName.Equals("Product", StringComparison.InvariantCultureIgnoreCase)
                        || lastName.Equals("Products", StringComparison.InvariantCultureIgnoreCase))
                    {
                        continue;
                    }
                    else if (lastName.EndsWith("ies"))
                    {
                        kw += " " + lastName.Substring(0, lastName.Length - 3) + "y";
                    }
                    else
                    {
                        if (lastName.Equals("Toothbrushes", StringComparison.InvariantCultureIgnoreCase)
                        || lastName.Equals("Brushes", StringComparison.InvariantCultureIgnoreCase)
                        || lastName.Equals("Classes", StringComparison.InvariantCultureIgnoreCase)
                        || lastName.Equals("Watches", StringComparison.InvariantCultureIgnoreCase)
                        || lastName.Equals("Compasses", StringComparison.InvariantCultureIgnoreCase)
                        || lastName.Equals("Boxes", StringComparison.InvariantCultureIgnoreCase)
                        || lastName.Equals("Clothes", StringComparison.InvariantCultureIgnoreCase))
                        {
                            kw += " " + lastName.Substring(0, lastName.Length - 2);
                        }
                        else if (lastName.Equals("Sunglasses", StringComparison.InvariantCultureIgnoreCase))
                        {
                            kw += " sunglasses sun glasses";
                        }
                    }
                }
            }

            foreach (string str in mN)
            {
                if (mN.Equals("NA"))
                {
                    continue;
                }
                kw += " " + str;
            }

            foreach (string str in others)
            {
                kw += " " + str;
            }

            kw += " " + pN;

            return kw.Trim();
        }

        static string PriceIntCultureString(double price, int countryId, IFormatProvider provider)
        {
            string priceString = price.ToString("C0", provider).Replace("Php", "P");

            if (countryId == 45)
            {
                priceString = priceString.Replace("£", "RM");
            }
            else if (countryId == 36)
            {
                priceString = priceString.Replace("£", "$");
                priceString = priceString.Replace("$", "S$");
            }
            else if (countryId == 56)
            {
                priceString = priceString.Replace("₫", "").Trim();
                priceString = priceString + "Đ";
            }

            return priceString;
        }

        private static void AddDictionary(Dictionary<string, List<CategoryDataInfo>> dictionary, string cid, string manufacturerID, string categoryName, int ppc)
        {
            if (dictionary.ContainsKey(manufacturerID))
            {
                List<CategoryDataInfo> baseDataList = dictionary[manufacturerID];
                bool contains = false;
                foreach (CategoryDataInfo cdi in baseDataList)
                {
                    if (cdi.CategoryID == cid)
                    {
                        cdi.ProductCount++;
                        if (ppc == 1)
                        {
                            cdi.PPCProductCount++;
                        }
                        contains = true;
                        break;
                    }
                }
                if (!contains)
                {
                    CategoryDataInfo cdi = new CategoryDataInfo();
                    cdi.CategoryID = cid;
                    cdi.CategoryName = categoryName;
                    cdi.ProductCount++;
                    if (ppc == 1)
                    {
                        cdi.PPCProductCount++;
                    }
                    baseDataList.Add(cdi);
                }
            }
            else
            {
                if (manufacturerID != "-1")
                {
                    List<CategoryDataInfo> baseDataList = new List<CategoryDataInfo>();

                    CategoryDataInfo cdi = new CategoryDataInfo();
                    cdi.CategoryID = cid;
                    cdi.CategoryName = categoryName;
                    cdi.ProductCount++;
                    if (ppc == 1)
                    {
                        cdi.PPCProductCount++;
                    }
                    baseDataList.Add(cdi);

                    dictionary.Add(manufacturerID, baseDataList);
                }
            }
        }

        private static Dictionary<int, List<ProductCatalog>> GetUpComingProductDic(DbInfo priceme205DbInfo, DbInfo pamUserDbInfo)
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
                        where ProductID not in (select ProductID from CSK_Store_RetailerProduct)";

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
                            if (ucpDic.ContainsKey(pc.CategoryID))
                            {
                                ucpDic[pc.CategoryID].Add(pc);
                            }
                            else
                            {
                                List<ProductCatalog> pcList = new List<ProductCatalog>();
                                pcList.Add(pc);
                                ucpDic.Add(pc.CategoryID, pcList);
                            }

                        }
                    }
                }
            }

            return ucpDic;
        }

        private static bool HasProduct(BaseData bd, List<string> hasProductCategoryList)
        {
            int myCid = int.Parse(bd.Id);
            List<int> subCidList = DataController.GetAllSubCategoryId(myCid);
            subCidList.Add(myCid);

            foreach (int cid in subCidList)
            {
                if (hasProductCategoryList.Contains(cid.ToString()))
                {
                    return true;
                }
            }

            return false;
        }

        private static List<RetailerCache> GetAllRetailersOrderByName(DbInfo subDbInfo)
        {
            List<RetailerCache> list = new List<RetailerCache>();
            string sql = "select * from CSK_Store_Retailer where IsSetupComplete = 1 and RetailerStatus = 1 order by RetailerName";

            using (var sqlConn = DBController.CreateDBConnection(subDbInfo))
            {
                using (var sqlCMD = DBController.CreateDbCommand(sql, sqlConn))
                {
                    sqlConn.Open();
                    using (var sqlDR = sqlCMD.ExecuteReader())
                    {
                        while (sqlDR.Read())
                        {
                            var rs = DbConvertController<RetailerCache>.ReadDataFromDataReader(sqlDR);
                            list.Add(rs);
                        }
                    }
                }
            }

            return list;
        }

        public static List<RetailerVotesSum> GetRetailerVotesSums(DbInfo price205DbInfo, int countryId)
        {
            List<RetailerVotesSum> votes = new List<RetailerVotesSum>();

            using (var sqlConn = DBController.CreateDBConnection(price205DbInfo))
            {
                using (var sqlCMD = DBController.CreateDbCommand("GetAllRetailerReview", sqlConn))
                {
                    sqlCMD.CommandType = CommandType.StoredProcedure;

                    var countryIdP = sqlCMD.CreateParameter();
                    countryIdP.ParameterName = "@countryid";
                    countryIdP.Value = countryId;
                    countryIdP.DbType = DbType.Int32;
                    sqlCMD.Parameters.Add(countryIdP);

                    sqlConn.Open();

                    using (var dr = sqlCMD.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            RetailerVotesSum vote = new RetailerVotesSum();
                            vote.ID = int.Parse(dr["ID"].ToString());
                            vote.RetailerID = int.Parse(dr["RetailerID"].ToString());
                            vote.RetailerRatingSum = int.Parse(dr["RetailerRatingSum"].ToString());
                            vote.RetailerTotalRatingVotes = int.Parse(dr["RetailerTotalRatingVotes"].ToString());

                            votes.Add(vote);
                        }
                    }
                }
            }
                   
            return votes;
        }

        static string GetStarImageUrl(double score)
        {
            if (score > 5.0f)
            {
                score = 5.0f;
            }
            string url = "https://images.priceme.co.nz/images/rating/";
            string starFileName = "";
            int stars = 0;
            score -= 1f;
            while (score > 0d)
            {
                stars++;
                score -= 1f;
            }
            score += 1f;
            starFileName = stars.ToString();
            if (score >= 0.3d && score < 0.8d)
            {
                starFileName += "h";
            }
            else if (score >= 0.8d)
            {
                starFileName = (stars + 1) + "";
            }
            starFileName = "star_" + starFileName + ".gif";
            url += starFileName;
            return url;
        }

        private static void UpDateWebConfig(string configFilePath, string appKey, string indexPath)
        {
            if (!File.Exists(configFilePath))
                return;
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(configFilePath);

            XmlNamespaceManager xnm = new XmlNamespaceManager(xmlDoc.NameTable);
            xnm.AddNamespace("d", "http://schemas.microsoft.com/.NetConfiguration/v2.0");

            XmlNode node = xmlDoc.SelectSingleNode("/d:configuration/d:appSettings/d:add[@key='" + appKey + "']", xnm);
            if (node == null)
            {
                XmlNode appSettings = xmlDoc.SelectSingleNode("/d:configuration/d:appSettings", xnm);
                node = xmlDoc.CreateNode(XmlNodeType.Element, "add", "http://schemas.microsoft.com/.NetConfiguration/v2.0");
                appSettings.AppendChild(node);

                node.Attributes.Append(xmlDoc.CreateAttribute("key"));
                node.Attributes.Append(xmlDoc.CreateAttribute("value"));
            }
            node.Attributes["key"].Value = appKey;
            node.Attributes["value"].Value = indexPath;

            xmlDoc.Save(configFilePath);
        }

        private static void ModifyLuceneConfigFTP(string path, IConfigurationRoot configuration)
        {
            try
            {
                string userID = configuration["userid_FTP"];
                string password = configuration["password_FTP"];
                string targetIP = configuration["targetIP_FTP"];
                string targetPath = configuration["targetPath_FTP"];

                string targetLuceneConfigPath = configuration["TargetLuceneConfigPath_FTP"];
                string targetLuceneIndexRootPath = configuration["TargetLuceneIndexRootPath_FTP"];
                string luceneConfigFileCopyDir = configuration["LuceneConfigFileCopyDir"];

                string luceneConfigFileName = configuration["TargetLuceneConfigName"];
                string luceneConfigFilePath = Path.Combine(targetLuceneConfigPath, luceneConfigFileName);

                CopyFile.FtpCopy.Download(luceneConfigFileCopyDir, luceneConfigFilePath, luceneConfigFileName, targetIP, userID, password);

                string localluceneConfigFilePath = Path.Combine(luceneConfigFileCopyDir, luceneConfigFileName);

                string appKey = "PopularSearchIndexPath2";
                path = path.TrimEnd('\\');

                string updateLucenePath = Path.Combine(targetLuceneIndexRootPath, path + "\\");
                UpDateWebConfig(localluceneConfigFilePath, appKey, updateLucenePath);

                CopyFile.FtpCopy.UploadFileSmall(localluceneConfigFilePath, targetLuceneConfigPath, targetIP, userID, password);
            }
            catch (Exception ex)
            {
                LogController.WriteException(ex.Message + ex.StackTrace);
                if (ex.InnerException != null)
                {
                    LogController.WriteException("InnerException : " + ex.InnerException.Message + "--------" + ex.InnerException.StackTrace);
                }
            }
        }

        //static void UpdateConfigFile(string userID, string targetIP, string password, string configFile, string appKey, string appValue)
        //{
        //    //   string tp = path.Substring(path.LastIndexOf("\\"));
        //    using (IdentityScope c = new IdentityScope(userID, targetIP, password))
        //    {
        //        try
        //        {
        //            string configFilePath = "\\" + configFile;
        //            if (!File.Exists(configFilePath))// 
        //                return;
        //            System.Xml.XmlDocument xmlDoc = new XmlDocument();
        //            xmlDoc.Load(configFilePath);

        //            XmlNamespaceManager xnm = new XmlNamespaceManager(xmlDoc.NameTable);
        //            xnm.AddNamespace("d", "http://schemas.microsoft.com/.NetConfiguration/v2.0");

        //            XmlNode node = xmlDoc.SelectSingleNode("/d:configuration/d:appSettings/d:add[@key='" + appKey + "']", xnm);
        //            if (node == null)
        //            {
        //                XmlNode appSettings = xmlDoc.SelectSingleNode("/d:configuration/d:appSettings", xnm);
        //                node = xmlDoc.CreateNode(XmlNodeType.Element, "add", "http://schemas.microsoft.com/.NetConfiguration/v2.0");
        //                appSettings.AppendChild(node);

        //                node.Attributes.Append(xmlDoc.CreateAttribute("key"));
        //                node.Attributes.Append(xmlDoc.CreateAttribute("value"));
        //            }
        //            node.Attributes["key"].Value = appKey;
        //            node.Attributes["value"].Value = appValue;

        //            xmlDoc.Save(configFilePath);
        //        }
        //        catch (Exception e) { Console.WriteLine(e.Message); }
        //    }
        //}
    }
}
