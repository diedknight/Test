using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IndexBuildCommon.Data;
using PriceMeCommon;
using PriceMeDBA;
using System.IO;
using System.Xml;
using CopyFile;
using System.Text.RegularExpressions;

namespace IndexBuildCommon
{
    public static class Utility
    {
        public static string updateDBConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["UpdateDBConnection"].ConnectionString;
        public static SubSonic.DataProviders.IDataProvider UpdateMasterDBProvider = SubSonic.DataProviders.ProviderFactory.GetProvider("UpdateDBConnection");

        public static List<FixPriceInfo> fixPriceInfoList = new List<FixPriceInfo>();

        //private static PriceMeDBA.PriceMeDBDB priceDB = PriceMeDBStatic.PriceMeDB;
        public static Dictionary<int, ExpertReview> _expertReviewDic = new Dictionary<int, ExpertReview>();
        private static Regex illegalReg = new Regex(@"[^a-z0-9-]+", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        //static List<PriceMeDBA.CSK_Store_ExpertReview> _expertReviewList = PriceMeDBA.CSK_Store_ExpertReview.All().ToList();

        public static void LoadAllExpertReview()
        {
            //SubSonic.Schema.StoredProcedure sp = priceDB.GetPriceMeExpertAverageRatingTF();
            //sp.Command.AddParameter("@CountryID", AppValue.CountryID, System.Data.DbType.Int32);
            //System.Data.IDataReader dr = sp.ExecuteReader();
            //while (dr.Read())
            //{
            //    ExpertReview er = new ExpertReview();
            //    er.ProductID = int.Parse(dr["ProductID"].ToString());
            //    er.PriceMeScore = double.Parse(dr["AverageRating"].ToString());
            //    er.Votes = int.Parse(dr["Votes"].ToString());
            //    er.VotesHasScore = int.Parse(dr["VotesHasScore"].ToString());

            //    if (!_expertReviewDic.ContainsKey(er.ProductID))
            //        _expertReviewDic.Add(er.ProductID, er);
            //}
        }

        public static string GetString(string inputString, int lenght)
        {
            if (inputString.Length > lenght)
            {
                return inputString.Substring(0, lenght) + "...";
            }
            return inputString;
        }

        public static float GetAverageRating(float productRatingSum, int productRatingVotes, float expertAverageRating)
        {
            float returnFloat = 0.0f;
            if (productRatingVotes > 1)
            {
                productRatingSum = productRatingSum - 3;
                productRatingVotes = productRatingVotes - 1;
                returnFloat = (productRatingSum) / (productRatingVotes * 1.0f);
            }
            return returnFloat + expertAverageRating;
        }

        public static double GetAverageRating(float userRatingSum, int userRatingVotes, int productid, out int ecount)
        {
            double expertScore = 0f;
            ecount = 0;
            if (_expertReviewDic.ContainsKey(productid))
            {
                ExpertReview er = _expertReviewDic[productid];
                userRatingSum = userRatingSum - 3;
                userRatingVotes = userRatingVotes - 1;
                expertScore = (userRatingSum * 1.0d + (er.PriceMeScore * er.VotesHasScore)) / (userRatingVotes * 1.0d + er.VotesHasScore);
                expertScore = double.Parse(expertScore.ToString("0.0"));
                ecount = er.Votes;
            }
            else
            {
                userRatingSum = userRatingSum - 3;
                userRatingVotes = userRatingVotes - 1;
                expertScore = (userRatingSum * 1.0d) / (userRatingVotes * 1.0d);
                expertScore = double.Parse(expertScore.ToString("0.0"));
            }

            return expertScore;
        }

        public static void IsLoadExpertReviewList()
        {
            if (_expertReviewDic.Count == 0)
                LoadAllExpertReview();
        }

        static System.Text.RegularExpressions.Regex seachKeywordsRegex = new Regex(@"(?<kwGroup>[^\s]+-[^\s]+)");
        public static string GetKeywords(string categoryName, string manufacturerName, string keywords, string productName, string otherKeywords)
        {
            string pN = productName.Replace("&", " ").Replace(",", " ").Replace("-", " ").Replace("_", " ").ToLower().Trim();

            MatchCollection matches = seachKeywordsRegex.Matches(productName);
            if (matches.Count > 0)
            {
                string newKw = "";
                foreach (Match m in matches)
                {
                    newKw += m.Groups["kwGroup"].Value.Replace("-", "") + " ";
                }
                pN = pN + " " + newKw.Trim().ToLower();
            }

            string[] mN = manufacturerName.Replace("&", " ").ToLower().Split(' ');
            string[] others = otherKeywords.Replace("&", " ").Replace(",", " ").ToLower().Split(' ');
            string kw = keywords == null ? "" : keywords.ToLower().Replace("&", " ").Replace(",", " ").Replace(":", " ");

            categoryName = categoryName.ToLower();

            if (categoryName.Equals("Tail Pads", StringComparison.InvariantCultureIgnoreCase)
                || categoryName.Equals("Eyeglasses", StringComparison.InvariantCultureIgnoreCase))
            {
                kw += " " + categoryName;
            }
            else
            {
                string[] cNs = categoryName.Split(new char[]{ '&', ',' },  StringSplitOptions.RemoveEmptyEntries);

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
                        else
                        {
                            if (lastName.EndsWith("s"))
                            {
                                kw += " " + lastName.Substring(0, lastName.Length - 1);
                            }
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

        public static string FixPrice(string retailerProductList, out int rid, out double newBestPrice)
        {
            rid = 0;
            newBestPrice = 0;
            int bestPriceIndex = 0;

            List<string> needFixString = new List<string>();

            string[] _retailerProductList = retailerProductList.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            if (_retailerProductList.Length <= 2)
            {
                return retailerProductList;
            }

            int mid = _retailerProductList.Length / 2;

            string _retailerProductString = _retailerProductList[mid];
            string[] _infos = _retailerProductString.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
            if (_infos.Length >= 3)
            {
                double midPrice = double.Parse(_infos[2]);
                double priceLimit = midPrice * (double)AppValue.PriceLimitPercent;
                for (int i = 0; i < mid; i++)
                {
                    if (i >= AppValue.FixPriceFlag)
                    {
                        break;
                    }
                    string[] infos = _retailerProductList[i].Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                    if (infos.Length >= 3)
                    {
                        double price = double.Parse(infos[2]);
                        if (price < priceLimit)
                        {
                            bestPriceIndex = i;

                            Data.FixPriceInfo fixPriceInfo = new Data.FixPriceInfo();
                            fixPriceInfo.RetailerID = int.Parse(infos[0]);
                            fixPriceInfo.RetailerProductID = int.Parse(infos[1]);
                            fixPriceInfo.Price = price;
                            fixPriceInfoList.Add(fixPriceInfo);
                            needFixString.Add(_retailerProductList[i]);

                            if (rid == 0)
                            {
                                rid = fixPriceInfo.RetailerID;
                            }

                            PriceMeCommon.BusinessLogic.LogController.WriteLog("productId :" + fixPriceInfo.ToString());
                        }
                    }
                }
            }

            foreach (string fs in needFixString)
            {
                retailerProductList = retailerProductList.Replace(fs, "");
            }

            if (rid != 0)
            {
                string[] infos = _retailerProductList[bestPriceIndex + 1].Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                newBestPrice = double.Parse(infos[2]);
            }

            return retailerProductList;
        }


        public static string FixOverseasPrice(string retailerProductList, out double newBestPrice, out double newMaxPrice, out int overSeasPriceCount)
        {
            newBestPrice = 0;
            newMaxPrice = 0;
            overSeasPriceCount = 0;
            List<string> needFixString = new List<string>();

            string[] _retailerProductList = retailerProductList.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < _retailerProductList.Length; i++)
            {
                string[] infos = _retailerProductList[i].Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                if (infos.Length >= 3)
                {
                    int retailerID = int.Parse(infos[0]);
                    if (NotOverseasRetailer.Contains(retailerID)) continue;
                    //needFixString.Add(_retailerProductList[i]);
                    overSeasPriceCount++;
                }
                needFixString.Add(_retailerProductList[i]);
            }

            string newRetailerProductList = retailerProductList;
            foreach (string fs in needFixString)
            {
                newRetailerProductList = newRetailerProductList.Replace(fs, "");
            }

            _retailerProductList = newRetailerProductList.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            if (_retailerProductList.Length > 0)
            {
                string[] infos = _retailerProductList[0].Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                newBestPrice = double.Parse(infos[2]);
                string[] infos2 = _retailerProductList[_retailerProductList.Length - 1].Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                newMaxPrice = double.Parse(infos2[2]);
                return newRetailerProductList;

            }
            else
            {
                overSeasPriceCount = 0;
                return retailerProductList;
            }
        }

        static List<int> NotOverseasRetailer = LoadNotOverseasRetailer();
        public static List<int> LoadNotOverseasRetailer()
        {
            List<int> notOverseasRetailer = new List<int>();

            string sqlString = "select retailerid from csk_store_ppcmember where retailerCountry = " + AppValue.CountryId;
            using (System.Data.SqlClient.SqlConnection sqlConn = new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CommerceTemplate"].ConnectionString))
            {
                using (System.Data.SqlClient.SqlCommand sqlCMD = new System.Data.SqlClient.SqlCommand(sqlString, sqlConn))
                {
                    sqlConn.Open();
                    using (System.Data.SqlClient.SqlDataReader sqlDR = sqlCMD.ExecuteReader())
                    {
                        while (sqlDR.Read())
                        {
                            int rid = int.Parse(sqlDR["retailerid"].ToString());
                            notOverseasRetailer.Add(rid);
                        }
                    }
                }
            }

            return notOverseasRetailer;
        }

        public static void UpdateFixRetailerProduct()
        {
            if (AppValue.UpdateDataBase.Equals("true", StringComparison.InvariantCultureIgnoreCase))
            {
                if (fixPriceInfoList.Count == 0) return;
                List<int> rpIDList = fixPriceInfoList.Select(pi => pi.RetailerProductID).ToList();
                string rpIDsString = string.Join(",", rpIDList);

                string updateSQLString = "UPDATE [dbo].[CSK_Store_RetailerProduct] SET [RetailerProductStatus] = 0 WHERE RetailerProductId in (" + rpIDsString + ")";
                using (System.Data.SqlClient.SqlConnection sqlConn = new System.Data.SqlClient.SqlConnection(updateDBConnectionString))
                {
                    using (System.Data.SqlClient.SqlCommand sqlCMD = new System.Data.SqlClient.SqlCommand(updateSQLString, sqlConn))
                    {
                        sqlConn.Open();
                        int k = 0;
                        while (true)
                        {
                            try
                            {
                                sqlCMD.ExecuteNonQuery();
                                break;
                            }
                            catch (Exception ex)
                            {
                                k++;

                                PriceMeCommon.BusinessLogic.LogController.WriteException(ex.Message + "\n" + ex.StackTrace);
                            }
                            if (k > 2)
                            {
                                break;
                            }
                        }
                    }
                }
            }
        }

        public static string CatalogUrl(Dictionary<string, string> queryParameters)
        {
            if (!queryParameters.ContainsKey("c"))
            {
                throw new Exception("catalogParameterError! queryParameters not contains 'c'");
            }

            string catalogUrlDescriptionString = GetCatalogUrlDescriptionString(queryParameters);
            string queryString = GetUrlParameterString(queryParameters);
            string url = "/" + catalogUrlDescriptionString + "/p_" + queryString + ".aspx";

            return url;
        }

        public static void WriteFixPriceLog()
        {

            PriceMeCommon.BusinessLogic.LogController.WriteLog("--------------");
            PriceMeCommon.BusinessLogic.LogController.WriteLog("Need fix RetailerProduct count : " + fixPriceInfoList.Count);

            string logHead = "RetailerID \t RetailerProductID \t Price";
            PriceMeCommon.BusinessLogic.LogController.WriteLog(logHead);

            Dictionary<int, int> results = new Dictionary<int, int>();
            foreach (FixPriceInfo fixPriceInfo in fixPriceInfoList)
            {
                string logInfo = fixPriceInfo.RetailerID + " \t " + fixPriceInfo.RetailerProductID + " \t " + fixPriceInfo.Price;
                PriceMeCommon.BusinessLogic.LogController.WriteLog(logInfo);
                if (results.ContainsKey(fixPriceInfo.RetailerID))
                {
                    results[fixPriceInfo.RetailerID] = results[fixPriceInfo.RetailerID] + 1;
                }
                else
                {
                    results.Add(fixPriceInfo.RetailerID, 1);
                }
            }

            foreach (int key in results.Keys)
            {
                string logInfo = "RetailerID : " + key + " InActive count : " + results[key];
                PriceMeCommon.BusinessLogic.LogController.WriteLog(logInfo);
            }
        }

        private static string GetCatalogUrlDescriptionString(Dictionary<string, string> queryParameters)
        {
            string catalogUrlDescriptionString = "";

            if (queryParameters.ContainsKey("m") && queryParameters["m"].Length > 0)
            {
                int mid = int.Parse(queryParameters["m"]);
                var manufacturer = PriceMeCommon.BusinessLogic.ManufacturerController.GetManufacturerByID(mid, AppValue.CountryId);
                if (manufacturer == null)
                {
                    throw new Exception("catalogParameterError! mid : " + mid + " not exist!");
                }

                catalogUrlDescriptionString = manufacturer.ManufacturerName + " ";
            }
            

            int cid = int.Parse(queryParameters["c"]);
            PriceMeCache.CategoryCache category = PriceMeCommon.BusinessLogic.CategoryController.GetCategoryByCategoryID(cid, AppValue.CountryId);

            catalogUrlDescriptionString += category.CategoryName + " ";

            if (queryParameters.ContainsKey("avs") && queryParameters["avs"].Length > 0)
            {
                AttributeParameterCollection urlAttributeParameterList = new AttributeParameterCollection();

                string attributeValuesIDString = queryParameters["avs"];
                string[] avids = attributeValuesIDString.Split(new char[] { '_' }, StringSplitOptions.RemoveEmptyEntries);
                int aid;

                foreach (string avid in avids)
                {
                    string avidString = avid;
                    if (avidString.ToLower().EndsWith("r"))
                    {
                        avidString = avid.Substring(0, avidString.Length - 1);
                        int.TryParse(avidString, out aid);
                        if (aid != 0)
                        {
                            AttributeParameter attributeParameter = GetAttributeParameterByAttributeRangeIDAndCategoryID(aid, category.CategoryID);
                            if (attributeParameter != null)
                            {
                                urlAttributeParameterList.Add(attributeParameter);
                            }
                        }
                    }
                    else
                    {
                        int.TryParse(avidString, out aid);
                        if (aid != 0)
                        {
                            AttributeParameter attributeParameter = GetAttributeParameterByAttributeValueIDAndCategoryID(aid, category.CategoryID);
                            if (attributeParameter != null)
                            {
                                urlAttributeParameterList.Add(attributeParameter);
                            }
                        }
                    }
                }

                if( urlAttributeParameterList.Count > 0)
                {
                    catalogUrlDescriptionString += urlAttributeParameterList.ToURLString();
                }
            }

            return catalogUrlDescriptionString.TrimEnd(' ');
        }

        public static AttributeParameter GetAttributeParameterByAttributeValueIDAndCategoryID(int aid, int cid)
        {
            PriceMeCache.AttributeTitleCache productDescriptorTitle = PriceMeCommon.BusinessLogic.AttributesController.GetAttributeTitleByVauleID(aid);
            if (productDescriptorTitle != null)
            {
                PriceMeCache.AttributeValueCache attributeValue = PriceMeCommon.BusinessLogic.AttributesController.GetAttributeValueByID(aid);
                if (attributeValue != null)
                {
                    string key = cid + "," + productDescriptorTitle.TypeID;
                    PriceMeCache.CategoryAttributeTitleMapCache categoryAttributeTitleMap = PriceMeCommon.BusinessLogic.AttributesController.GetCategoryAttributeTitleMapByKey(key);
                    if (categoryAttributeTitleMap != null)
                    {
                        AttributeParameter attributeParameter = new AttributeParameter();
                        attributeParameter.ListOrder = categoryAttributeTitleMap.AttributeOrder;
                        attributeParameter.AttributeName = productDescriptorTitle.Title;
                        attributeParameter.AttributeValue = attributeValue.Value + productDescriptorTitle.Unit.Trim();
                        return attributeParameter;
                    }
                }
            }
            return null;
        }

        public static AttributeParameter GetAttributeParameterByAttributeRangeIDAndCategoryID(int arid, int cid)
        {
            var attributeValueRange = PriceMeCommon.BusinessLogic.AttributesController.GetAttributeValueRangeByID(arid);
            if (attributeValueRange != null)
            {
                PriceMeCache.AttributeTitleCache productDescriptorTitle = PriceMeCommon.BusinessLogic.AttributesController.GetAttributeTitleByID(attributeValueRange.AttributeTitleID);
                string key = cid + "," + productDescriptorTitle.TypeID;

                PriceMeCache.CategoryAttributeTitleMapCache categoryAttributeTitleMap = PriceMeCommon.BusinessLogic.AttributesController.GetCategoryAttributeTitleMapByKey(key);
                if (categoryAttributeTitleMap != null)
                {
                    AttributeParameter attributeParameter = new AttributeParameter();
                    attributeParameter.ListOrder = categoryAttributeTitleMap.AttributeOrder;
                    attributeParameter.AttributeName = productDescriptorTitle.Title;
                    attributeParameter.AttributeValue = PriceMeCommon.BusinessLogic.AttributesController.GetAttributeValueString(attributeValueRange, productDescriptorTitle.Unit);
                    return attributeParameter;
                }
            }
            return null;
        }

        private static string GetUrlParameterString(Dictionary<string, string> queryParameters)
        {
            string queryString = "";
            foreach (string pName in queryParameters.Keys)
            {
                if (pName == "pg" && (queryParameters[pName] == "1" || queryParameters[pName] == "0"))
                {
                    continue;
                }
                queryString += pName + "-" + FilterInvalidUrlPathChar(queryParameters[pName]) + ",";
            }
            return queryString.TrimEnd(',');
        }

        public static string FilterInvalidUrlPathChar(string sourceString)
        {
            string sOut = "";
            string[] illegalChar = { "/", "\\", "&", "?", " ", ":", ",", ";", "'", "\"", "*", "#", "@", "%", ".", "<", ">", "|", "[", "]", ",", "+", "~", "$", "(", ")" };
            string[] result = sourceString.Split(illegalChar, StringSplitOptions.RemoveEmptyEntries);
            foreach (string s in result)
            {
                if (!s.Equals("-"))
                {
                    sOut += s + "-";
                }
            }
            sOut = sOut.TrimEnd('-');
            return sOut;
        }

        public static bool UpDateWebConfig(string configFilePath, string appKey, string indexPath)
        {
            if (!File.Exists(configFilePath))
            {
                Console.WriteLine("no file path :" + configFilePath);
                PriceMeCommon.BusinessLogic.LogController.WriteLog("no file path :" + configFilePath);
                return false;
            }
            System.Xml.XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(configFilePath);

            XmlNode configNode = null;
            foreach (XmlNode xmlNode in xmlDoc.ChildNodes)
            {
                if (xmlNode.Name.ToLower() == "configuration")
                {
                    configNode = xmlNode;
                    break;
                }
            }

            XmlNode appSettings = null;
            foreach (XmlNode xmlNode in configNode.ChildNodes)
            {
                if (xmlNode.Name.ToLower() == "appsettings")
                {
                    appSettings = xmlNode;
                    break;
                }
            }

            bool successful = false;
            foreach (XmlNode xmlNode in appSettings.ChildNodes)
            {
                if (xmlNode.Attributes[0].Value == appKey)
                {
                    xmlNode.Attributes[1].Value = indexPath;
                    successful = true;
                    break;
                }
            }

            xmlDoc.Save(configFilePath);

            if (successful)
            {
                Console.WriteLine("update " + configFilePath + " successful!");
                PriceMeCommon.BusinessLogic.LogController.WriteLog("update " + configFilePath + " successful!");
            }
            else
            {
                Console.WriteLine("update " + configFilePath + " fail!");
                PriceMeCommon.BusinessLogic.LogController.WriteLog("update " + configFilePath + " fail!");
            }
            return successful;
        }

        public static bool CopyIndex(string todayIndexPathIndexPath)
        {
            try
            {
                string userID = System.Configuration.ConfigurationManager.AppSettings["userid"];
                string password = System.Configuration.ConfigurationManager.AppSettings["password"];
                string targetIP = System.Configuration.ConfigurationManager.AppSettings["targetIP"];
                string targetPath = System.Configuration.ConfigurationManager.AppSettings["targetPath"];

                CopyFile.NetWorkCopy.Copy(targetIP, targetPath, userID, password, todayIndexPathIndexPath, "", false);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                PriceMeCommon.BusinessLogic.LogController.WriteException(ex.Message + ex.StackTrace);
                return false;
            }
            return true;
        }

        public static bool CopyIndex2(string todayIndexPathIndexPath)
        {
            bool copyTo2 = bool.Parse(System.Configuration.ConfigurationManager.AppSettings["CopyTo2"]);
            if (copyTo2)
            {
                try
                {
                    string userID = System.Configuration.ConfigurationManager.AppSettings["userid2"];
                    string password = System.Configuration.ConfigurationManager.AppSettings["password2"];
                    string targetIP = System.Configuration.ConfigurationManager.AppSettings["targetIP2"];
                    string targetPath = System.Configuration.ConfigurationManager.AppSettings["targetPath2"];

                    CopyFile.NetWorkCopy.Copy(targetIP, targetPath, userID, password, todayIndexPathIndexPath, "", false);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    PriceMeCommon.BusinessLogic.LogController.WriteException(ex.Message + ex.StackTrace);
                    return false;
                }
                return true;
            }
            else
            {
                return true;
            }
        }

        public static void ModifyLuceneConfig3(string path)
        {
            try
            {
                string userID = System.Configuration.ConfigurationManager.AppSettings["userid"];
                string password = System.Configuration.ConfigurationManager.AppSettings["password"];
                string targetIP = System.Configuration.ConfigurationManager.AppSettings["targetIP"];
                string targetLuceneConfigPath = System.Configuration.ConfigurationManager.AppSettings["TargetLuceneConfigPath"];
                string targetLuceneIndexRootPath = System.Configuration.ConfigurationManager.AppSettings["TargetLuceneIndexRootPath"];

                targetLuceneConfigPath = @"\\" + targetIP + @"\" + targetLuceneConfigPath;
                
                string appKey = PriceMeCommon.BusinessLogic.MultiCountryController.GetIndexKey(AppValue.CountryId);
                path = path.TrimEnd('\\');
                using (IdentityScope c = new IdentityScope(userID, targetIP, password))
                {
                    string updateLucenePath = targetLuceneIndexRootPath + path + "\\";
                    UpDateWebConfig(targetLuceneConfigPath, appKey, updateLucenePath);
                }

                string localLuceneConfigPath = System.Configuration.ConfigurationManager.AppSettings["LuceneConfigPath"];
                string localLuceneIndexRootPath = AppValue.IndexRootPath;
                string updatelocalLucenePath = localLuceneIndexRootPath + path + "\\";
                UpDateWebConfig(localLuceneConfigPath, appKey, updatelocalLucenePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                PriceMeCommon.BusinessLogic.LogController.WriteException(ex.Message + ex.StackTrace);
                if (ex.InnerException != null)
                {
                    PriceMeCommon.BusinessLogic.LogController.WriteException("InnerException : " + ex.InnerException.Message + "--------" + ex.InnerException.StackTrace);
                }
            }
        }

        public static void ModifyLuceneConfig(string path)
        {
            try
            {
                string userID = System.Configuration.ConfigurationManager.AppSettings["userid"];
                string password = System.Configuration.ConfigurationManager.AppSettings["password"];
                string targetIP = System.Configuration.ConfigurationManager.AppSettings["targetIP"];
                string targetLuceneConfigPath = System.Configuration.ConfigurationManager.AppSettings["TargetLuceneConfigPath"];
                string targetLuceneIndexRootPath = System.Configuration.ConfigurationManager.AppSettings["TargetLuceneIndexRootPath"];

                targetLuceneConfigPath = @"\\" + targetIP + @"\" + targetLuceneConfigPath;

                string appKey = PriceMeCommon.BusinessLogic.MultiCountryController.GetIndexKey(AppValue.CountryId);
                path = path.TrimEnd('\\');
                using (IdentityScope c = new IdentityScope(userID, targetIP, password))
                {
                    string updateLucenePath = targetLuceneIndexRootPath + path + "\\";
                    UpDateWebConfig(targetLuceneConfigPath, appKey, updateLucenePath);
                }

                string localLuceneConfigPath = System.Configuration.ConfigurationManager.AppSettings["LocalLuceneConfigPath"];
                string localLuceneIndexRootPath = AppValue.IndexRootPath;
                string updatelocalLucenePath = localLuceneIndexRootPath + path + "\\";
                UpDateWebConfig(localLuceneConfigPath, appKey, updatelocalLucenePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                PriceMeCommon.BusinessLogic.LogController.WriteException(ex.Message + ex.StackTrace);
                if (ex.InnerException != null)
                {
                    PriceMeCommon.BusinessLogic.LogController.WriteException("InnerException : " + ex.InnerException.Message + "--------" + ex.InnerException.StackTrace);
                }
            }
        }

        public static void ModifyLuceneConfig2(string path)
        {
            try
            {
                string userID = System.Configuration.ConfigurationManager.AppSettings["userid2"];
                string password = System.Configuration.ConfigurationManager.AppSettings["password2"];
                string targetIP = System.Configuration.ConfigurationManager.AppSettings["targetIP2"];
                string targetLuceneConfigPath = System.Configuration.ConfigurationManager.AppSettings["TargetLuceneConfigPath2"];
                string targetLuceneIndexRootPath = System.Configuration.ConfigurationManager.AppSettings["TargetLuceneIndexRootPath2"];

                targetLuceneConfigPath = @"\\" + targetIP + @"\" + targetLuceneConfigPath;

                string appKey = PriceMeCommon.BusinessLogic.MultiCountryController.GetIndexKey(AppValue.CountryId);
                path = path.TrimEnd('\\');
                using (IdentityScope c = new IdentityScope(userID, targetIP, password))
                {
                    string updateLucenePath = targetLuceneIndexRootPath + path + "\\";
                    UpDateWebConfig(targetLuceneConfigPath, appKey, updateLucenePath);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                PriceMeCommon.BusinessLogic.LogController.WriteException(ex.Message + ex.StackTrace);
                if (ex.InnerException != null)
                {
                    PriceMeCommon.BusinessLogic.LogController.WriteException("InnerException : " + ex.InnerException.Message + "--------" + ex.InnerException.StackTrace);
                }
            }
        }

        internal static string FixKeywords(string searchFieldString)
        {
            string rs = illegalReg.Replace(searchFieldString, "-");
            rs = rs.Replace("-", " ");
            return rs.Trim();
        }
    }
}