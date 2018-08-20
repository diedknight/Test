using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;
using System.Xml;
using Lucene.Net.Documents;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Store;
using Lucene.Net.Index;
using PriceMeCommon;
using CopyFile;
using PriceMeCommon.BusinessLogic;
using Lucene.Net.Util;
using PriceMeCache;

namespace ProductsForPopularSearch
{
    class Program
    {
        static Regex invalidCharacter = new Regex("[^a-z0-9_\\-\\s]", RegexOptions.Compiled | RegexOptions.Multiline | RegexOptions.IgnoreCase);

        static int CountryId_Static;
        static IFormatProvider CurrentCulture = new System.Globalization.CultureInfo(System.Configuration.ConfigurationManager.AppSettings["CurrentCulture"]);
        public static IFormatProvider Provider = new System.Globalization.CultureInfo("en-US");
        static string idxDir;
        static string idexDir2;
        static string idexDieFTP;
        static Dictionary<string, int> categoryClicksDic;
        static Dictionary<int, int> productClicksDic;

        static void Main(string[] args)
        {
            MultiCountryController.LoadWithoutCheckIndexPath();

            SearchController.GetRootCategoryId(11);

            CategoryController.LoadForBuildIndex();

            int dbProductCount = 0;
            int indexProductCount = 0;
            int indexUpComingProductCount = 0;
            int indexCategoryCount = 0;
            int indexRetailerCount = 0;
            int indexBrandCount = 0;
            int indexindexBrandAndCategoryCount = 0;
            int indexPopularSearchCount = 0;
            int indexOfferProductCount = 0;

            CountryId_Static = int.Parse(ConfigurationManager.AppSettings.Get("CountryID"));
            idxDir = ConfigurationManager.AppSettings.Get("IndexDir") + @"\" + DateTime.Now.ToString("yyyyMMddHH");
            idexDir2 = ConfigurationManager.AppSettings.Get("IndexDir2") + @"\" + DateTime.Now.ToString("yyyyMMddHH");
            idexDieFTP = ConfigurationManager.AppSettings.Get("IdexDieFTP") + @"\" + DateTime.Now.ToString("yyyyMMddHH");
            bool CheckClick = bool.Parse(ConfigurationManager.AppSettings.Get("CheckClick"));
            int CheckClickDays = int.Parse(ConfigurationManager.AppSettings.Get("CheckClickDays"));
            string ClickDBInfo = ConfigurationManager.AppSettings.Get("ClickDBInfo");
            int ClickDays = int.Parse(ConfigurationManager.AppSettings.Get("ClickDays"));

            SetClicksDic(ClickDays);

            if (CheckClick)
            {
                int clicksCount = categoryClicksDic.Values.Sum();

                LogController.WriteLog("ClickCount : " + clicksCount + " on " + DateTime.Now.ToLocalTime().ToString("yyyyMMdd hh:mm:ss"));

                if (clicksCount == 0)
                    return;
            }

            if (!System.IO.Directory.Exists(idxDir))
            {
                System.IO.Directory.CreateDirectory(idxDir);
            }

            Analyzer analyzer = new WhitespaceAnalyzer();
            FSDirectory ramDir = FSDirectory.Open(new System.IO.DirectoryInfo(idxDir));

            //IndexWriterConfig iwc = new IndexWriterConfig(LuceneVersion.LUCENE_48, analyzer);
            //iwc.OpenMode = OpenMode.CREATE;
            //IndexWriter idw = new IndexWriter(ramDir, iwc);

            IndexWriter idw = new IndexWriter(ramDir, analyzer, IndexWriter.MaxFieldLength.UNLIMITED);

            //float categoryBoost = float.Parse(ConfigurationManager.AppSettings.Get("CategoryBoost"));
            //float manufacturerBoost = float.Parse(ConfigurationManager.AppSettings.Get("ManufacturerBoost"));

            List<string> isSearchOnlyCategories = new List<string>();

            #region
            Dictionary<string, List<CategoryDataInfo>> dictionary = new Dictionary<string, List<CategoryDataInfo>>();
            Dictionary<int, string> categorySynonymDic = GetCategorySynonymDic();

            //Dictionary<int, string> productAttributeDic = GetProductAttributeDic();
            //Dictionary<int, string> retailerProductNameDic = GetRetailerProductNameDic();

            using (var sqlConn = DBController.CreateDBConnection(MultiCountryController.GetDBConnectionSettings(CountryId_Static)))
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
                            Select ProductId from CSK_Store_Product where CategoryID in (select CategoryID from CSK_Store_Category where isSearchOnly = 1))
                            and(select count(*) from CSK_Store_RetailerProductNew b where a.ProductId = b.ProductId and a.RetailerPrice < b.RetailerPrice) = 0";
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
                            and ProductId in (Select ProductId from CSK_Store_Product where CategoryID in (select CategoryID from CSK_Store_Category where isSearchOnly = 1))
                            )
                            select * from tp where rn = 1";
                }

                var scRP = DBController.CreateDbCommand(query, sqlConn);
                idr = scRP.ExecuteReader();

                while(idr.Read())
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
                                    and C.IsActive = 1 and LCT.CountryID = " + CountryId_Static + " order by C.CategoryID";


                #region DK
                string categoryQueryString = " and (";
                string rootCategoryIds = ConfigurationManager.AppSettings["RootCategoryIds"];
                if (rootCategoryIds != "0")
                {
                    string[] cids = rootCategoryIds.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (string cid in cids)
                    {
                        //categoryQueryString += " c.CategoryID in (SELECT id FROM GetChildren (" + cid + ")) or";

                        List<int> allSubCIds = CategoryController.GetAllSubCategoryId(int.Parse(cid), CountryId_Static);
                        allSubCIds.Add(int.Parse(cid));
                        categoryQueryString += " c.CategoryID in (" + string.Join(",", allSubCIds) + ") or ";
                    }
                    categoryQueryString = categoryQueryString.Substring(0, categoryQueryString.Length - 3) + ")";

                    query = @"select c.CategoryName, c.CategoryID, c.IsSiteMap, c.IsSiteMapDetail,
                                    LCT.CategoryName as LocalName, c.isSearchOnly from CSK_Store_Category as C left join Local_CategoryName as LCT on C.CategoryID = LCT.CategoryID
                                    and C.IsActive = 1 and LCT.CountryID =" + CountryId_Static + categoryQueryString
                                    + @"
                                    order by C.CategoryID";
                    //
                }
                #endregion

                var sc2 = DBController.CreateDbCommand(query, sqlConn);

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

                    if(isSearchOnly.Equals("true", StringComparison.InvariantCultureIgnoreCase))
                    {
                        isSearchOnlyCategories.Add(cid);
                    }

                    categoryDataList.Add(baseData);
                }
                idr.Close();

                Dictionary<string, CategoryDataInfo> categoryDataInfoDictionary = new Dictionary<string, CategoryDataInfo>();

                string selectC = " (P.IsMerge = 1 or C.IsDisplayIsMerged = 1) and ";
                if(CountryId_Static == 25)
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

                bool ppcOnly = false;
                bool.TryParse(System.Configuration.ConfigurationManager.AppSettings["PPCOnly"], out ppcOnly);

                var cmd = DBController.CreateDbCommand(query, sqlConn);

                idr = cmd.ExecuteReader();

                List<string> hasProductCategoryList = new List<string>();
                Console.WriteLine("Write Product Index...");
                List<int> accessroiesCIDs = new List<int>();
                
                while (idr.Read())
                {
                    dbProductCount++;
                    bool isAccessroies = true;

                    if(!bool.TryParse(idr["IsAccessories"].ToString(), out isAccessroies))
                    {
                        isAccessroies = true;
                    }

                    string categoryName = idr["CategoryName"].ToString();//需要翻译

                    string cid = idr["CategoryID"].ToString();

                    int categoryID = int.Parse(cid);

                    string categorySynonym = "";
                    if(categorySynonymDic.ContainsKey(categoryID))
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

                    //int ppc = 0;
                    //int.TryParse(idr["PPC"].ToString(), out ppc);

                    if (CountryId_Static != 25)
                    {
                        //if (isSearchOnlyCategories.Contains(cid) && ppc == 0)
                        if (isSearchOnlyCategories.Contains(cid))
                            continue;
                    }

                    if (!isAccessroies || CountryId_Static == 25)
                    {
                        //if ((ppcOnly && ppc == 1) || !ppcOnly)
                        //{
                            string manufacturerName = idr["ManufacturerName"].ToString();
                            if(manufacturerName.Equals("na", StringComparison.InvariantCultureIgnoreCase))
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
                            //string productAttributeString = "";
                            //if (productAttributeDic.ContainsKey(productID))
                            //{
                            //    productAttributeString = productAttributeDic[productID];
                            //}
                            //string rpProductNames = "";
                            //if (retailerProductNameDic.ContainsKey(productID))
                            //{
                            //    rpProductNames = retailerProductNameDic[productID];
                            //}

                            string minPriceString = PriceIntCultureString(double.Parse(minPrice.ToString(), System.Globalization.NumberStyles.Any, Provider), CurrentCulture);
                            //keyword = Utility.GetKeywords(categoryName, manufacturerName, pk, productName, categorySynonym + " " + productAttributeString + " " + rpProductNames);
                            keyword = Utility.GetKeywords(categoryName, manufacturerName, pk, productName, categorySynonym);

                            Document doc = new Document();

                            //doc.Add(new StoredField("ID", productID));
                            //doc.Add(new TextField("Name", keyword, Field.Store.NO));
                            //doc.Add(new StoredField("DisplayValue", productName));
                            //doc.Add(new StoredField("Type", "P"));
                            //doc.Add(new StoredField("Other", categoryName));
                            //doc.Add(new StoredField("ListOrder", "3"));
                            //doc.Add(new StringField("IncludePPC", ppc.ToString(), Field.Store.YES));
                            //doc.Add(new StringField("CategoryID", cid, Field.Store.YES));
                            //doc.Add(new StoredField("DefaultImage", defaultImage));
                            //int clicks = 0;
                            //string clickstring = idr["TotalClicks"].ToString();
                            //int.TryParse(clickstring, out clicks);
                            //doc.Add(new Int32Field("Clicks", clicks, Field.Store.YES));
                            //doc.Add(new StoredField("MinPrice", minPriceString));
                            //if(rpInfoDic.ContainsKey(productID))
                            //{
                            //    RPInfo rpInfo = rpInfoDic[productID];
                            //    doc.Add(new StoredField("PPC_MinPrice", (double)rpInfo.MinPrice));
                            //    doc.Add(new StoredField("PPC_RetailerProductID", rpInfo.RetailerProductID));
                            //    doc.Add(new StoredField("PPC_RetailerID", rpInfo.RetailerID));
                            //}

                            doc.Add(new Field("ID", productID.ToString(), Field.Store.YES, Field.Index.NO));
                            doc.Add(new Field("Name", keyword, Field.Store.NO, Field.Index.ANALYZED));
                            doc.Add(new Field("DisplayValue", productName, Field.Store.YES, Field.Index.NO));
                            doc.Add(new Field("Type", "P", Field.Store.YES, Field.Index.NO));
                            doc.Add(new Field("Other", categoryName, Field.Store.YES, Field.Index.NO));
                            doc.Add(new Field("ListOrder", "3", Field.Store.YES, Field.Index.NO));
                            //doc.Add(new Field("IncludePPC", ppc.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
                            doc.Add(new Field("IncludePPC", "1", Field.Store.YES, Field.Index.NOT_ANALYZED));
                            doc.Add(new Field("CategoryID", cid, Field.Store.YES, Field.Index.NOT_ANALYZED));
                            doc.Add(new Field("DefaultImage", defaultImage, Field.Store.YES, Field.Index.NO));
                            float orderScroe = 0;
                            if(isSearchOnlyCategories.Contains(cid))
                            {
                                orderScroe = 1;
                            }
                            else
                            {
                                orderScroe = 5;
                            }
                            NumericField orderField = new NumericField("Order", Field.Store.NO, true);
                            orderField.SetFloatValue(orderScroe);
                            doc.Add(orderField);

                            int clicks = 0;

                            //string clickstring = idr["TotalClicks"].ToString();
                            //int.TryParse(clickstring, out clicks);
                            if(productClicksDic.ContainsKey(productID))
                            {
                                clicks = productClicksDic[productID];
                            }

                            if(categoryClicksDic.ContainsKey(cid))
                            {
                                clicks += categoryClicksDic[cid];
                            }
                            NumericField clicksField = new NumericField("Clicks", Field.Store.YES, true);
                            clicksField.SetIntValue(clicks);
                            doc.Add(clicksField);
                            doc.Add(new Field("MinPrice", minPriceString, Field.Store.YES, Field.Index.NO));
                            if (rpInfoDic.ContainsKey(productID))
                            {
                                RPInfo rpInfo = rpInfoDic[productID];
                                doc.Add(new Field("PPC_MinPrice", rpInfo.MinPrice.ToString("0.00"), Field.Store.YES, Field.Index.NO));
                                doc.Add(new Field("PPC_RetailerProductID", rpInfo.RetailerProductID.ToString(), Field.Store.YES, Field.Index.NO));
                                doc.Add(new Field("PPC_RetailerID", rpInfo.RetailerID.ToString(), Field.Store.YES, Field.Index.NO));
                            }

                            idw.AddDocument(doc);
                            indexProductCount++;

                            //if (ppc == 1)
                            //{
                                categoryDataInfo.PPCProductCount++;
                        //    }
                        //}
                    }
                    else
                    {
                        if(!accessroiesCIDs.Contains(categoryID))
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
                        //AddDictionary(dictionary, cid, manufacturerID, categoryName, ppc);
                        AddDictionary(dictionary, cid, manufacturerID, categoryName, 1);
                    }
                }
                idr.Close();

                Dictionary<int, List<PriceMeCommon.Data.ProductCatalog>> ucpDic = GetUpComingProductDic();
                if (ucpDic != null)
                {
                    foreach (int key in ucpDic.Keys)
                    {
                        foreach (PriceMeCommon.Data.ProductCatalog pc in ucpDic[key])
                        {
                            string localName = GetLocalName(pc.CategoryID.ToString(), categoryDataList);
                            string categorySynonym = "";
                            if (categorySynonymDic.ContainsKey(pc.CategoryID))
                            {
                                categorySynonym = categorySynonymDic[pc.CategoryID];
                            }

                            string kw = Utility.GetKeywords(localName, pc.RetailerProductInfoString, "", pc.ProductName, categorySynonym);

                            Document doc = new Document();
                            //doc.Add(new StoredField("ID", pc.ProductID));
                            //doc.Add(new TextField("Name", kw, Field.Store.NO));
                            //doc.Add(new StoredField("DisplayValue", pc.ProductName));
                            //doc.Add(new StoredField("Type", "UCP"));
                            //doc.Add(new StoredField("Other", localName));
                            //doc.Add(new StoredField("ListOrder", "3"));
                            //doc.Add(new StringField("CategoryID", pc.CategoryID.ToString(), Field.Store.YES));
                            //doc.Add(new StoredField("DefaultImage", pc.DefaultImage));
                            //doc.Add(new Int32Field("Clicks", 0, Field.Store.YES));

                            doc.Add(new Field("ID", pc.ProductID, Field.Store.YES, Field.Index.NO));
                            doc.Add(new Field("Name", kw, Field.Store.NO, Field.Index.ANALYZED));
                            doc.Add(new Field("DisplayValue", pc.ProductName, Field.Store.YES, Field.Index.NO));
                            doc.Add(new Field("Type", "UCP", Field.Store.YES, Field.Index.NO));
                            doc.Add(new Field("Other", localName, Field.Store.YES, Field.Index.NO));
                            doc.Add(new Field("ListOrder", "3", Field.Store.YES, Field.Index.NO));
                            doc.Add(new Field("CategoryID", pc.CategoryID.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
                            doc.Add(new Field("DefaultImage", pc.DefaultImage, Field.Store.YES, Field.Index.NO));
                            NumericField clicksField = new NumericField("Clicks", Field.Store.YES, true);
                            clicksField.SetIntValue(0);
                            doc.Add(clicksField);

                            float orderScroe = 4;
                            NumericField orderField = new NumericField("Order", Field.Store.NO, true);
                            orderField.SetFloatValue(orderScroe);
                            doc.Add(orderField);

                            idw.AddDocument(doc);
                            indexUpComingProductCount++;
                        }
                    }
                }

                List<PriceMeCommon.Data.ProductCatalog> offerProducts = GetOfferProducts(CountryId_Static);
                foreach(PriceMeCommon.Data.ProductCatalog pc in offerProducts)
                {
                    string categorySynonym = "";
                    string localName = GetLocalName(pc.CategoryID.ToString(), categoryDataList);
                    if (categorySynonymDic.ContainsKey(pc.CategoryID))
                    {
                        categorySynonym = categorySynonymDic[pc.CategoryID];
                    }

                    string kw = Utility.GetKeywords(localName, "", "", pc.ProductName, categorySynonym);

                    Document doc = new Document();

                    doc.Add(new Field("ID", pc.ProductID, Field.Store.YES, Field.Index.NO));
                    doc.Add(new Field("Name", kw, Field.Store.NO, Field.Index.ANALYZED));
                    doc.Add(new Field("DisplayValue", pc.ProductName, Field.Store.YES, Field.Index.NO));
                    doc.Add(new Field("Type", "Offer", Field.Store.YES, Field.Index.NO));
                    doc.Add(new Field("Other", localName, Field.Store.YES, Field.Index.NO));
                    doc.Add(new Field("ListOrder", "3", Field.Store.YES, Field.Index.NO));
                    doc.Add(new Field("CategoryID", pc.CategoryID.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
                    doc.Add(new Field("DefaultImage", pc.DefaultImage, Field.Store.YES, Field.Index.NO));
                    NumericField clicksField = new NumericField("Clicks", Field.Store.YES, true);
                    clicksField.SetIntValue(-1);
                    doc.Add(clicksField);

                    string minPriceString = PriceIntCultureString(double.Parse(pc.BestPrice, System.Globalization.NumberStyles.Any, Provider), CurrentCulture);
                    doc.Add(new Field("MinPrice", minPriceString, Field.Store.YES, Field.Index.NO));

                    float orderScroe = 4;
                    NumericField orderField = new NumericField("Order", Field.Store.NO, true);
                    orderField.SetFloatValue(orderScroe);
                    doc.Add(orderField);

                    idw.AddDocument(doc);
                    indexOfferProductCount++;
                }

                Console.WriteLine("Write Category Index...");
                foreach (var data in categoryDataList)
                {
                    //if (isSearchOnlyCategories.Contains(data.Id))
                    //    continue;

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

                        //keyword = invalidCharacter.Replace(data.Value.ToLower().Trim(), "");
                        keyword = data.Value.ToLower().Trim() + " " + categorySynonym;

                        //doc.Add(new StoredField("ID", data.Id));
                        //doc.Add(new TextField("Name", keyword, Field.Store.NO));
                        //doc.Add(new StoredField("DisplayValue", data.Value));
                        //doc.Add(new StoredField("Type", "C"));
                        //doc.Add(new StoredField("Other", ""));
                        //doc.Add(new StoredField("ListOrder", "1"));
                        //doc.Add(new StringField("CategoryID", data.Id, Field.Store.YES));
                        //doc.Add(new StoredField("DefaultImage", ""));
                        //doc.Add(new Int32Field("Clicks", int.MaxValue, Field.Store.YES));
                        //string ppcStr = "0";
                        //if (categoryDataInfoDictionary.ContainsKey(data.Id) && categoryDataInfoDictionary[data.Id].PPCProductCount > 0)
                        //{
                        //    ppcStr = "1";
                        //}
                        //doc.Add(new StringField("IncludePPC", ppcStr, Field.Store.YES));

                        doc.Add(new Field("ID", data.Id, Field.Store.YES, Field.Index.NO));
                        doc.Add(new Field("Name", keyword, Field.Store.NO, Field.Index.ANALYZED));
                        doc.Add(new Field("DisplayValue", data.Value, Field.Store.YES, Field.Index.NO));
                        doc.Add(new Field("Type", "C", Field.Store.YES, Field.Index.NO));
                        doc.Add(new Field("Other", "", Field.Store.YES, Field.Index.NO));
                        doc.Add(new Field("ListOrder", "1", Field.Store.YES, Field.Index.NO));
                        doc.Add(new Field("CategoryID", data.Id, Field.Store.YES, Field.Index.NOT_ANALYZED));
                        doc.Add(new Field("DefaultImage", "", Field.Store.YES, Field.Index.NO));
                        NumericField clicksField = new NumericField("Clicks", Field.Store.YES, true);
                        clicksField.SetIntValue(int.MaxValue);
                        doc.Add(clicksField);
                        string ppcStr = "0";
                        if (categoryDataInfoDictionary.ContainsKey(data.Id) && categoryDataInfoDictionary[data.Id].PPCProductCount > 0)
                        {
                            ppcStr = "1";
                        }
                        doc.Add(new Field("IncludePPC", ppcStr, Field.Store.YES, Field.Index.NOT_ANALYZED));

                        float orderScroe = 0;
                        if (isSearchOnlyCategories.Contains(categoryID.ToString()))
                        {
                            orderScroe = 4.5f;
                        }
                        else
                        {
                            orderScroe = 5;
                        }
                        NumericField orderField = new NumericField("Order", Field.Store.NO, true);
                        orderField.SetFloatValue(orderScroe);
                        doc.Add(orderField);

                        idw.AddDocument(doc);
                        indexCategoryCount++;
                    }
                }

                //List<PriceMeDBA.CSK_Store_Retailer> _retailerList = PriceMeDBA.PriceMeDBStatic.PriceMeDB.CSK_Store_Retailers
                //         .Where(rt => rt.IsSetupComplete == true)//&& rt.RetailerCountry == ConfigAppString.CountryID
                //         .OrderBy(rt => rt.RetailerName).ToList();
                var _retailerList = GetAllRetailersOrderByName();
                var _retailerVotes = GetRetailerVotesSums(CountryId_Static);
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
                //List<PriceMeCache.RetailerCache> retailerCacheList = ConvertController<PriceMeCache.RetailerCache, PriceMeDBA.CSK_Store_Retailer>.ConvertData(_retailerList);

                //RetailerController.Load();

                Console.WriteLine("Write Retailer Index...");
                foreach (var retailer in _retailerList)
                {
                    //if (!retailer.IsSetupComplete || retailer.RetailerStatus == 99) continue;

                    Document doc = new Document();
                    keyword = invalidCharacter.Replace(retailer.RetailerName.ToLower().Trim(), "");

                    double avRating = (double)retailer.AvRating;
                    string ratingImageUrl = GetStarImageUrl(avRating);

                    //doc.Add(new StoredField("ID", retailer.RetailerId));
                    //doc.Add(new TextField("Name", Utility.FixKeywords(keyword), Field.Store.NO));
                    //doc.Add(new StoredField("DisplayValue", retailer.RetailerName));
                    //doc.Add(new StoredField("Type", "R"));
                    //doc.Add(new StoredField("Other", ratingImageUrl));
                    //doc.Add(new StringField("IncludePPC", "1", Field.Store.YES));
                    //doc.Add(new StringField("CategoryID", "-1", Field.Store.YES));
                    //doc.Add(new StoredField("DefaultImage", ""));
                    //doc.Add(new Int32Field("Clicks", 1000000, Field.Store.YES));
                    //string listOrderStr = "1";
                    //if (retailer.RetailerStatus == 99)
                    //{
                    //    listOrderStr = "-1";
                    //}
                    //doc.Add(new StoredField("ListOrder", listOrderStr));

                    doc.Add(new Field("ID", retailer.RetailerId.ToString(), Field.Store.YES, Field.Index.NO));
                    doc.Add(new Field("Name", Utility.FixKeywords(keyword), Field.Store.NO, Field.Index.ANALYZED));
                    doc.Add(new Field("DisplayValue", retailer.RetailerName, Field.Store.YES, Field.Index.NO));
                    doc.Add(new Field("Type", "R", Field.Store.YES, Field.Index.NO));
                    doc.Add(new Field("Other", ratingImageUrl, Field.Store.YES, Field.Index.NO));
                    doc.Add(new Field("IncludePPC", "1", Field.Store.YES, Field.Index.NOT_ANALYZED));
                    doc.Add(new Field("CategoryID", "-1", Field.Store.YES, Field.Index.NOT_ANALYZED));
                    doc.Add(new Field("DefaultImage", "", Field.Store.YES, Field.Index.NO));
                    NumericField clicksField = new NumericField("Clicks", Field.Store.YES, true);
                    clicksField.SetIntValue(1000000);
                    doc.Add(clicksField);
                    string listOrderStr = "1";
                    if (retailer.RetailerStatus == 99)
                    {
                        listOrderStr = "-1";
                    }
                    doc.Add(new Field("ListOrder", listOrderStr, Field.Store.YES, Field.Index.NO));

                    float orderScroe = 4;
                    NumericField orderField = new NumericField("Order", Field.Store.NO, true);
                    orderField.SetFloatValue(orderScroe);
                    doc.Add(orderField);

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

                    //doc.Add(new StoredField("ID", mid));
                    //doc.Add(new TextField("Name", Utility.FixKeywords(keyword), Field.Store.NO));
                    //doc.Add(new StoredField("DisplayValue", idr["ManufacturerName"].ToString()));
                    //doc.Add(new StoredField("Type", "M"));
                    //doc.Add(new StoredField("Other", idr["IsPopular"].ToString()));
                    //doc.Add(new StoredField("ListOrder", "2"));
                    //doc.Add(new StringField("IncludePPC", "1", Field.Store.YES));
                    //doc.Add(new StringField("CategoryID", "-1", Field.Store.YES));
                    //doc.Add(new StoredField("DefaultImage", ""));
                    //doc.Add(new Int32Field("Clicks", 1000000, Field.Store.YES));

                    doc.Add(new Field("ID", mid, Field.Store.YES, Field.Index.NO));
                    doc.Add(new Field("Name", Utility.FixKeywords(keyword), Field.Store.NO, Field.Index.ANALYZED));
                    doc.Add(new Field("DisplayValue", idr["ManufacturerName"].ToString(), Field.Store.YES, Field.Index.NO));
                    doc.Add(new Field("Type", "M", Field.Store.YES, Field.Index.NO));
                    doc.Add(new Field("Other", idr["IsPopular"].ToString(), Field.Store.YES, Field.Index.NO));
                    doc.Add(new Field("ListOrder", "2", Field.Store.YES, Field.Index.NO));
                    doc.Add(new Field("IncludePPC", "1", Field.Store.YES, Field.Index.NOT_ANALYZED));
                    doc.Add(new Field("CategoryID", "-1", Field.Store.YES, Field.Index.NOT_ANALYZED));
                    doc.Add(new Field("DefaultImage", "", Field.Store.YES, Field.Index.NO));
                    NumericField clicksField = new NumericField("Clicks", Field.Store.YES, true);
                    clicksField.SetIntValue(1000000);
                    doc.Add(clicksField);
                    float orderScroe = 3;
                    NumericField orderField = new NumericField("Order", Field.Store.NO, true);
                    orderField.SetFloatValue(orderScroe);
                    doc.Add(orderField);

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

                                ProductSearcher productSearcher = new ProductSearcher("", _cid, brandList, null, null, null, "", null, 10000, CountryId_Static, false, true, true, true, null, "", null);
                                int pCount = productSearcher.GetProductCount();
                                if (pCount == 0)
                                    continue;

                                string brandsAndCategoryName = idr["ManufacturerName"].ToString().Trim() + " " + cdi.CategoryName.Trim();
                                Document docBaC = new Document();

                                //docBaC.Add(new StoredField("ID", mid + "," + _cid));
                                //docBaC.Add(new TextField("Name", Utility.FixKeywords(brandsAndCategoryName.ToLower() + " " + categorySynonym), Field.Store.NO));
                                //docBaC.Add(new StoredField("DisplayValue", brandsAndCategoryName));
                                //docBaC.Add(new StoredField("Type", "BAC"));
                                //docBaC.Add(new StoredField("Other", cdi.ProductCount.ToString()));
                                //docBaC.Add(new StoredField("ListOrder", "5"));
                                //docBaC.Add(new StringField("IncludePPC", "1", Field.Store.YES));
                                //docBaC.Add(new StringField("CategoryID", _cid.ToString(), Field.Store.YES));
                                //docBaC.Add(new StoredField("DefaultImage", ""));
                                //docBaC.Add(new StoredField("IndexProductCount", pCount));
                                //docBaC.Add(new Int32Field("Clicks", productSearcher.GetClickCount(), Field.Store.YES));

                                docBaC.Add(new Field("ID", mid + "," + _cid, Field.Store.YES, Field.Index.NO));
                                docBaC.Add(new Field("Name", Utility.FixKeywords(brandsAndCategoryName.ToLower() + " " + categorySynonym), Field.Store.NO, Field.Index.ANALYZED));
                                docBaC.Add(new Field("DisplayValue", brandsAndCategoryName, Field.Store.YES, Field.Index.NO));
                                docBaC.Add(new Field("Type", "BAC", Field.Store.YES, Field.Index.NO));
                                docBaC.Add(new Field("Other", cdi.ProductCount.ToString(), Field.Store.YES, Field.Index.NO));
                                docBaC.Add(new Field("ListOrder", "5", Field.Store.YES, Field.Index.NO));
                                docBaC.Add(new Field("IncludePPC", "1", Field.Store.YES, Field.Index.NOT_ANALYZED));
                                docBaC.Add(new Field("CategoryID", _cid.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
                                docBaC.Add(new Field("DefaultImage", "", Field.Store.YES, Field.Index.NO));
                                docBaC.Add(new Field("IndexProductCount", pCount.ToString(), Field.Store.YES, Field.Index.NO));
                                NumericField bacClicksField = new NumericField("Clicks", Field.Store.YES, true);
                                bacClicksField.SetIntValue(productSearcher.GetClickCount());
                                docBaC.Add(bacClicksField);

                                orderScroe = 0;
                                if (isSearchOnlyCategories.Contains(_cid.ToString()))
                                {
                                    orderScroe = 1;
                                }
                                else
                                {
                                    orderScroe = 4.5f;
                                }
                                orderField = new NumericField("Order", Field.Store.NO, true);
                                orderField.SetFloatValue(orderScroe);
                                doc.Add(orderField);

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
                    keyword = invalidCharacter.Replace(name.ToLower().Trim(), "");

                    string url = idr["Url"].ToString().Trim();
                    string otherInfo = idr["OtherInfo"].ToString().Trim();
                    int type = int.Parse(idr["Type"].ToString().Trim());

                    doc.Add(new Field("ID", "0", Field.Store.YES, Field.Index.NO));
                    doc.Add(new Field("Name", keyword, Field.Store.NO, Field.Index.ANALYZED));
                    doc.Add(new Field("DisplayValue", name, Field.Store.YES, Field.Index.NO));
                    doc.Add(new Field("Type", "C1", Field.Store.YES, Field.Index.NO));
                    doc.Add(new Field("Other", url, Field.Store.YES, Field.Index.NO));
                    doc.Add(new Field("ListOrder", "1", Field.Store.YES, Field.Index.NO));
                    doc.Add(new Field("IncludePPC", "1", Field.Store.YES, Field.Index.NOT_ANALYZED));
                    doc.Add(new Field("CategoryID", "0", Field.Store.YES, Field.Index.NOT_ANALYZED));
                    doc.Add(new Field("DefaultImage", "", Field.Store.YES, Field.Index.NO));
                    NumericField clicksField = new NumericField("Clicks", Field.Store.YES, true);
                    clicksField.SetIntValue(int.MaxValue);
                    doc.Add(clicksField);

                    float orderScroe = 3;
                    NumericField orderField = new NumericField("Order", Field.Store.NO, true);
                    orderField.SetFloatValue(orderScroe);
                    doc.Add(orderField);

                    idw.AddDocument(doc);
                    indexPopularSearchCount++;
                }
            }
            #endregion
            idw.Dispose();

            string luceneConfigFilePath = ConfigurationManager.AppSettings["LuceneConfigPath"];
            string luceneConfigFilePath2 = ConfigurationManager.AppSettings["LuceneConfigPath2"];
            string luceneConfigFilePathFTP = ConfigurationManager.AppSettings["luceneConfigFilePathFTP"];
            if (File.Exists(luceneConfigFilePath))
                UpDateWebConfig(luceneConfigFilePath, "PopularSearchIndexPath2", idxDir);

            if (bool.Parse(ConfigurationManager.AppSettings["byShare"]))
            {
                bool success = CopyF(idxDir);
                if (success)
                {
                    string targetPath = System.Configuration.ConfigurationManager.AppSettings["targetPath"];
                    string targetIP = System.Configuration.ConfigurationManager.AppSettings["targetIP"];
                    string userID = System.Configuration.ConfigurationManager.AppSettings["userid"];
                    string password = System.Configuration.ConfigurationManager.AppSettings["password"];

                    UpdateConfigFile(userID, targetIP, password,luceneConfigFilePath2, "PopularSearchIndexPath2", idexDir2);
                }
            }
            if (bool.Parse(ConfigurationManager.AppSettings["byFTP"]))
            {
                bool success = CopyByFTP(idxDir);
                if (success)
                {
                    ModifyLuceneConfigFTP(idexDieFTP.Replace(ConfigurationManager.AppSettings.Get("IdexDieFTP")+@"\",""));
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

        private static List<RetailerCache> GetAllRetailersOrderByName()
        {
            List<RetailerCache> list = new List<RetailerCache>();
            string sql = "select * from CSK_Store_Retailer where IsSetupComplete = 1 and RetailerStatus = 1 order by RetailerName";

            using (var sqlConn = DBController.CreateDBConnection(MultiCountryController.GetDBConnectionSettings(CountryId_Static)))
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

        private static void SetClicksDic(int clickDays)
        {
            categoryClicksDic = new Dictionary<string, int>();
            productClicksDic = new Dictionary<int, int>();

            string sql = "SELECT categoryId, ProductId, count(id) as c from csk_store_retailertracker where ClickTime > ";

            using (var sqlConn = DBController.CreateDBConnection(MultiCountryController.CommonConnectionStringSettings_Static))
            {
                if (sqlConn is MySql.Data.MySqlClient.MySqlConnection)
                {
                    sql += " date_add(Now(),  interval -" + clickDays + " day) group by CategoryID;";
                }
                else
                {
                    sql = " CONVERT(varchar(100), dateadd(day,-" + clickDays + ",GETDATE()), 23) group by CategoryID";
                }
                using (var sqlCMD = DBController.CreateDbCommand(sql, sqlConn))
                {
                    sqlConn.Open();
                    using (var sqlDR = sqlCMD.ExecuteReader())
                    {
                        if (sqlDR.Read())
                        {
                            string cid = sqlDR.GetInt32(0).ToString();
                            int pid = sqlDR.GetInt32(1);
                            int clicks = sqlDR.GetInt32(2);
                            productClicksDic.Add(pid, clicks);

                            if(categoryClicksDic.ContainsKey(cid))
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

        private static Dictionary<int, List<PriceMeCommon.Data.ProductCatalog>> GetUpComingProductDic()
        {
            LogController.WriteLog("Start read DB for GetUpComingProductDic on " + DateTime.Now.ToLocalTime().ToString("yyyyMMdd hh:mm:ss"));

            Dictionary<int, List<PriceMeCommon.Data.ProductCatalog>> ucpDic = new Dictionary<int, List<PriceMeCommon.Data.ProductCatalog>>();

            string connectionString1 = ConfigurationManager.ConnectionStrings["CommerceTemplate_205"].ConnectionString;
            Dictionary<int, int> excludePidDic = new Dictionary<int, int>();
            Dictionary<int, PriceMeCommon.Data.ProductCatalog> includePidDic = new Dictionary<int, PriceMeCommon.Data.ProductCatalog>();

            using (SqlConnection sqlConnection = new SqlConnection(connectionString1))
            {
                using (SqlCommand sqlCMD = new SqlCommand("SELECT distinct ProductID from CSK_Store_ProductIsMerged", sqlConnection))
                {
                    sqlConnection.Open();
                    sqlCMD.CommandTimeout = 0;
                    using (SqlDataReader sdr = sqlCMD.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            excludePidDic.Add(sdr.GetInt32(0), 0);
                        }
                    }
                }

                using (SqlCommand sqlCMD = new SqlCommand(@"SELECT ProductID
                                                            ,ProductName
                                                            ,CSK_Store_Manufacturer.ManufacturerID
                                                            ,CSK_Store_Manufacturer.ManufacturerName
                                                            ,CategoryID
                                                            ,CatalogDescription
                                                            ,DefaultImage
                                                            FROM CSK_Store_Product
                                                            inner join CSK_Store_Manufacturer on CSK_Store_Manufacturer.ManufacturerID = CSK_Store_Product.ManufacturerID
                                                            where ProductID not in (select ProductID from CSK_Store_RetailerProduct)", sqlConnection))
                {
                    sqlCMD.CommandTimeout = 0;
                    using (SqlDataReader sdr = sqlCMD.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            PriceMeCommon.Data.ProductCatalog pc = new PriceMeCommon.Data.ProductCatalog();
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

            string sql;

            using (var sqlConn = DBController.CreateDBConnection(MultiCountryController.CommonConnectionStringSettings_Static))
            {
                if(sqlConn is MySql.Data.MySqlClient.MySqlConnection)
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

                            PriceMeCommon.Data.ProductCatalog pc = includePidDic[pid];
                            if (ucpDic.ContainsKey(pc.CategoryID))
                            {
                                ucpDic[pc.CategoryID].Add(pc);
                            }
                            else
                            {
                                List<PriceMeCommon.Data.ProductCatalog> pcList = new List<PriceMeCommon.Data.ProductCatalog>();
                                pcList.Add(pc);
                                ucpDic.Add(pc.CategoryID, pcList);
                            }

                        }
                    }
                }
            }

            return ucpDic;
        }

        private static List<PriceMeCommon.Data.ProductCatalog> GetOfferProducts(int countryId)
        {
            List<PriceMeCommon.Data.ProductCatalog> list = new List<PriceMeCommon.Data.ProductCatalog>();
            string selectSql = @"SELECT OfferId
                                  ,OfferName
                                  ,Price
                                  ,CategoryID
                                  ,DefaultImage
                                FROM CSK_Store_RetailerOffer
                                where Status = 1 and Price > 0.2 and 
                                RetailerId in (select retailerId from CSK_Store_Retailer where IsSetupComplete = 1 and RetailerStatus = 1 and RetailerCountry = " + countryId + " and RetailerId in (select RetailerId from CSK_Store_PPCMember where PPCMemberTypeID = 2))";

            using (var sqlConn = DBController.CreateDBConnection(MultiCountryController.CommonConnectionStringSettings_Static))
            {
                using (var sqlCMD = DBController.CreateDbCommand(selectSql, sqlConn))
                {
                    sqlConn.Open();
                    using (var sdr = sqlCMD.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            PriceMeCommon.Data.ProductCatalog pc = new PriceMeCommon.Data.ProductCatalog();
                            int offerId = sdr.GetInt32(0);
                            pc.ProductID = offerId.ToString();
                            pc.ProductName = sdr.GetString(1);
                            pc.BestPrice = sdr.GetDecimal(2).ToString("0.00");
                            pc.CategoryID = sdr.GetInt32(3);
                            pc.DefaultImage = sdr["DefaultImage"].ToString();//sdr.GetString(6);
                            list.Add(pc);
                        }
                    }
                }
            }

            return list;
        }

        //private static Dictionary<int, string> GetRetailerProductNameDic()
        //{
        //    Dictionary<int, string> dic = new Dictionary<int, string>();
        //    string selectSql = @"SELECT ProductID, RetailerProductName FROM CSK_Store_RetailerProductNew";
        //    string connString = MultiCountryController.GetDBConnectionString(CountryId_Static);
        //    using (SqlConnection sqlConn = new SqlConnection(connString))
        //    {
        //        using (SqlCommand sqlCMD = new SqlCommand(selectSql, sqlConn))
        //        {
        //            sqlConn.Open();
        //            sqlCMD.CommandTimeout = 0;
        //            using (SqlDataReader sqlDR = sqlCMD.ExecuteReader())
        //            {
        //                while (sqlDR.Read())
        //                {
        //                    int pid = sqlDR.GetInt32(0);
        //                    string rpName = sqlDR.GetString(1).Trim();

        //                    if (!dic.ContainsKey(pid))
        //                    {
        //                        dic.Add(pid, rpName);
        //                    }
        //                    else
        //                    {
        //                        dic[pid] = dic[pid] + " " + rpName;
        //                    }
        //                }
        //            }
        //        }
        //    }

        //    return dic;    
        //}

        //private static Dictionary<int, string> GetProductAttributeDic()
        //{
        //    Dictionary<int, string> dic = new Dictionary<int, string>();
        //    string selectSql = @"SELECT ProductID, AV.Value, AT.Unit FROM CSK_Store_ProductDescriptor as PA 
        //                        inner join CSK_Store_AttributeValue as AV on PA.AttributeValueID = AV.AttributeValueID
        //                        inner join CSK_Store_ProductDescriptorTitle as AT on AT.TypeID = AV.AttributeTitleID";
        //    string connString = MultiCountryController.GetDBConnectionString(CountryId_Static);
        //    using (SqlConnection sqlConn = new SqlConnection(connString))
        //    {
        //        using (SqlCommand sqlCMD = new SqlCommand(selectSql, sqlConn))
        //        {
        //            sqlCMD.CommandTimeout = 0;
        //            sqlConn.Open();
        //            using (SqlDataReader sqlDR = sqlCMD.ExecuteReader())
        //            {
        //                while (sqlDR.Read())
        //                {
        //                    int pid = sqlDR.GetInt32(0);
        //                    string value = sqlDR["Value"].ToString().Trim();
        //                    string unit = sqlDR["Unit"].ToString().Trim();

        //                    if (!dic.ContainsKey(pid))
        //                    {
        //                        dic.Add(pid, value + unit);
        //                    }
        //                    else
        //                    {
        //                        dic[pid] = dic[pid] + " " + value + unit;
        //                    }
        //                }
        //            }
        //        }
        //    }

        //    return dic;
        //}

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

        private static bool HasProduct(BaseData bd, List<string> hasProductCategoryList)
        {
            int myCid = int.Parse(bd.Id);
            List<int> subCidList = CategoryController.GetAllSubCategoryId(myCid, CountryId_Static);
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

        static string GetStarImageUrl(double score)
        {
            if (score > 5.0f)
            {
                score = 5.0f;
            }
            string url = "http://images.priceme.co.nz/images/rating/";
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

        private static void UpDateWebConfig(string configFilePath, string appKey, string indexPath)
        {
            if (!File.Exists(configFilePath))
                return;
            System.Xml.XmlDocument xmlDoc = new XmlDocument();
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

        public static List<RetailerVotesSum> GetRetailerVotesSums(int country)
        {
            List<RetailerVotesSum> votes = new List<RetailerVotesSum>();

            string commonConnStr = ConfigurationManager.ConnectionStrings["CommerceTemplate_205"].ConnectionString;
            using (SqlConnection sqlConn = new SqlConnection(commonConnStr))
            using(SqlCommand sqlCMD = new SqlCommand("GetAllRetailerReview", sqlConn))
            {
                sqlCMD.CommandType = CommandType.StoredProcedure;
                sqlCMD.Parameters.AddWithValue("@countryid", country);

                sqlConn.Open();

                using (IDataReader dr = sqlCMD.ExecuteReader())
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
            return votes;
        }
        //static string PriceIntCultureString(double price)
        //{
        //    PriceIntCultureString(price, CurrentCulture);
        //}

        static string PriceIntCultureString(double price, IFormatProvider provider)
        {
            string priceString = price.ToString("C0", provider).Replace("Php", "P");

            if (CountryId_Static == 45)
            {
                priceString = priceString.Replace("£", "RM");
            }
            else if (CountryId_Static == 36)
            {
                priceString = priceString.Replace("£", "$");
                priceString = priceString.Replace("$", "S$");
            }
            else if (CountryId_Static == 56)
            {
                priceString = priceString.Replace("₫", "").Trim();
                priceString = priceString + "Đ";
            }

            return priceString;
        }

        static bool CopyF(string path)
        {
            string targetPath = System.Configuration.ConfigurationManager.AppSettings["targetPath"];
            string targetIP = System.Configuration.ConfigurationManager.AppSettings["targetIP"];
            string userID = System.Configuration.ConfigurationManager.AppSettings["userid"];
            string password = System.Configuration.ConfigurationManager.AppSettings["password"];

            try
            {
                NetWorkCopy.Copy(targetIP, targetPath, userID, password, path, "-no");
                //int s = int.Parse("k");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                LogController.WriteException("InnerException : " + ex.Message + "--------" + ex.StackTrace);
                return false;
            }
        }

        static void UpdateConfigFile(string userID, string targetIP, string password,string configFile, string appKey, string appValue)
        {
            //   string tp = path.Substring(path.LastIndexOf("\\"));
            using (IdentityScope c = new IdentityScope(userID, targetIP, password))
            {
                try
                {
                    string configFilePath = "\\" + configFile;
                    if (!File.Exists(configFilePath))// 
                        return;
                    System.Xml.XmlDocument xmlDoc = new XmlDocument();
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
                    node.Attributes["value"].Value = appValue;

                    xmlDoc.Save(configFilePath);
                }
                catch (Exception e) { Console.WriteLine(e.Message); }
            }
        }

        static bool CopyByFTP(string path)
        {
            string userID = System.Configuration.ConfigurationManager.AppSettings["userid_FTP"];
            string password = System.Configuration.ConfigurationManager.AppSettings["password_FTP"];
            string targetIP = System.Configuration.ConfigurationManager.AppSettings["targetIP_FTP"];
            string targetPath = System.Configuration.ConfigurationManager.AppSettings["targetPath_FTP"];
            try
            {
                CopyFile.FtpCopy.UploadDirectorySmall(path, targetPath, targetIP, userID, password, "-no");

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace + e.Message);
                return false;
            }
        }

        private static void ModifyLuceneConfigFTP(string path)
        {
            try
            {
                string userID = System.Configuration.ConfigurationManager.AppSettings["userid_FTP"];
                string password = System.Configuration.ConfigurationManager.AppSettings["password_FTP"];
                string targetIP = System.Configuration.ConfigurationManager.AppSettings["targetIP_FTP"];
                string targetPath = System.Configuration.ConfigurationManager.AppSettings["targetPath_FTP"];

                string targetLuceneConfigPath = System.Configuration.ConfigurationManager.AppSettings["TargetLuceneConfigPath_FTP"];
                string targetLuceneIndexRootPath = System.Configuration.ConfigurationManager.AppSettings["TargetLuceneIndexRootPath_FTP"];
                string luceneConfigFileCopyDir = System.Configuration.ConfigurationManager.AppSettings["LuceneConfigFileCopyDir"];

                string luceneConfigFileName = System.Configuration.ConfigurationManager.AppSettings["TargetLuceneConfigName"];
                string luceneConfigFilePath = targetLuceneConfigPath + luceneConfigFileName;

                CopyFile.FtpCopy.Download(luceneConfigFileCopyDir, luceneConfigFilePath, luceneConfigFileName, targetIP, userID, password);

                string localluceneConfigFilePath = luceneConfigFileCopyDir + luceneConfigFileName;

                string appKey = "PopularSearchIndexPath2";//ConfigAppString.CountryInfoList[0].KeyName;
                path = path.TrimEnd('\\');

                string updateLucenePath = targetLuceneIndexRootPath + path + "\\";
                UpDateWebConfig(localluceneConfigFilePath, appKey, updateLucenePath);

                CopyFile.FtpCopy.UploadFileSmall(localluceneConfigFilePath, targetLuceneConfigPath, targetIP, userID, password);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                LogController.WriteException(ex.Message + ex.StackTrace);
                if (ex.InnerException != null)
                {
                    LogController.WriteException("InnerException : " + ex.InnerException.Message + "--------" + ex.InnerException.StackTrace);
                }
            }
        }

        private static Dictionary<int, string> GetCategorySynonymDic()
        {
            Dictionary<int, string> dic = new Dictionary<int, string>();

            string sql = "SELECT CategoryID,Synonym FROM CategorySynonym_New where CountryID = " + CountryId_Static;
            using (var sqlConn = DBController.CreateDBConnection(MultiCountryController.CommonConnectionStringSettings_Static))
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

    }
}