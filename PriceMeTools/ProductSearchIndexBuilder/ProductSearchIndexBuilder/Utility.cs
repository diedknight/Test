using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace ProductSearchIndexBuilder
{
    public static class Utility
    {
        private static Regex illegalReg = new Regex(@"[^a-z0-9-]+", RegexOptions.Compiled | RegexOptions.IgnoreCase);

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

        static Regex seachKeywordsRegex = new Regex(@"(?<kwGroup>[^\s]+-[^\s]+)");
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
                    if (!DataController.IsOverseasRetailer(retailerID)) continue;
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
                LogController.WriteLog("no file path :" + configFilePath);
                return false;
            }
            XmlDocument xmlDoc = new XmlDocument();
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
                LogController.WriteLog("update " + configFilePath + " successful!");
            }
            else
            {
                LogController.WriteLog("update " + configFilePath + " fail!");
            }
            return successful;
        }

        public static bool IsDev()
        {
            var envs = Environment.GetEnvironmentVariables();
            if (envs.Contains("DEV_ENVIRONMENT") && envs["DEV_ENVIRONMENT"].ToString().Equals("1") || AppValue.IsDebug)
            {
                return true;
            }
            return false;
        }

        //public static bool CopyIndex(string todayIndexPathIndexPath)
        //{
        //    try
        //    {
        //        string userID = System.Configuration.ConfigurationManager.AppSettings["userid"];
        //        string password = System.Configuration.ConfigurationManager.AppSettings["password"];
        //        string targetIP = System.Configuration.ConfigurationManager.AppSettings["targetIP"];
        //        string targetPath = System.Configuration.ConfigurationManager.AppSettings["targetPath"];

        //        CopyFile.NetWorkCopy.Copy(targetIP, targetPath, userID, password, todayIndexPathIndexPath, "", false);
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //        PriceMeCommon.BusinessLogic.LogController.WriteException(ex.Message + ex.StackTrace);
        //        return false;
        //    }
        //    return true;
        //}

        //public static bool CopyIndex2(string todayIndexPathIndexPath)
        //{
        //    bool copyTo2 = bool.Parse(System.Configuration.ConfigurationManager.AppSettings["CopyTo2"]);
        //    if (copyTo2)
        //    {
        //        try
        //        {
        //            string userID = System.Configuration.ConfigurationManager.AppSettings["userid2"];
        //            string password = System.Configuration.ConfigurationManager.AppSettings["password2"];
        //            string targetIP = System.Configuration.ConfigurationManager.AppSettings["targetIP2"];
        //            string targetPath = System.Configuration.ConfigurationManager.AppSettings["targetPath2"];

        //            CopyFile.NetWorkCopy.Copy(targetIP, targetPath, userID, password, todayIndexPathIndexPath, "", false);
        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine(ex.Message);
        //            PriceMeCommon.BusinessLogic.LogController.WriteException(ex.Message + ex.StackTrace);
        //            return false;
        //        }
        //        return true;
        //    }
        //    else
        //    {
        //        return true;
        //    }
        //}

        //public static void ModifyLuceneConfig3(string path)
        //{
        //    try
        //    {
        //        string userID = System.Configuration.ConfigurationManager.AppSettings["userid"];
        //        string password = System.Configuration.ConfigurationManager.AppSettings["password"];
        //        string targetIP = System.Configuration.ConfigurationManager.AppSettings["targetIP"];
        //        string targetLuceneConfigPath = System.Configuration.ConfigurationManager.AppSettings["TargetLuceneConfigPath"];
        //        string targetLuceneIndexRootPath = System.Configuration.ConfigurationManager.AppSettings["TargetLuceneIndexRootPath"];

        //        targetLuceneConfigPath = @"\\" + targetIP + @"\" + targetLuceneConfigPath;

        //        string appKey = PriceMeCommon.BusinessLogic.MultiCountryController.GetIndexKey(AppValue.CountryId);
        //        path = path.TrimEnd('\\');
        //        using (IdentityScope c = new IdentityScope(userID, targetIP, password))
        //        {
        //            string updateLucenePath = targetLuceneIndexRootPath + path + "\\";
        //            UpDateWebConfig(targetLuceneConfigPath, appKey, updateLucenePath);
        //        }

        //        string localLuceneConfigPath = System.Configuration.ConfigurationManager.AppSettings["LuceneConfigPath"];
        //        string localLuceneIndexRootPath = AppValue.IndexRootPath;
        //        string updatelocalLucenePath = localLuceneIndexRootPath + path + "\\";
        //        UpDateWebConfig(localLuceneConfigPath, appKey, updatelocalLucenePath);
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //        PriceMeCommon.BusinessLogic.LogController.WriteException(ex.Message + ex.StackTrace);
        //        if (ex.InnerException != null)
        //        {
        //            PriceMeCommon.BusinessLogic.LogController.WriteException("InnerException : " + ex.InnerException.Message + "--------" + ex.InnerException.StackTrace);
        //        }
        //    }
        //}

        //public static void ModifyLuceneConfig(string path)
        //{
        //    try
        //    {
        //        string userID = System.Configuration.ConfigurationManager.AppSettings["userid"];
        //        string password = System.Configuration.ConfigurationManager.AppSettings["password"];
        //        string targetIP = System.Configuration.ConfigurationManager.AppSettings["targetIP"];
        //        string targetLuceneConfigPath = System.Configuration.ConfigurationManager.AppSettings["TargetLuceneConfigPath"];
        //        string targetLuceneIndexRootPath = System.Configuration.ConfigurationManager.AppSettings["TargetLuceneIndexRootPath"];

        //        targetLuceneConfigPath = @"\\" + targetIP + @"\" + targetLuceneConfigPath;

        //        string appKey = PriceMeCommon.BusinessLogic.MultiCountryController.GetIndexKey(AppValue.CountryId);
        //        path = path.TrimEnd('\\');
        //        using (IdentityScope c = new IdentityScope(userID, targetIP, password))
        //        {
        //            string updateLucenePath = targetLuceneIndexRootPath + path + "\\";
        //            UpDateWebConfig(targetLuceneConfigPath, appKey, updateLucenePath);
        //        }

        //        string localLuceneConfigPath = System.Configuration.ConfigurationManager.AppSettings["LocalLuceneConfigPath"];
        //        string localLuceneIndexRootPath = AppValue.IndexRootPath;
        //        string updatelocalLucenePath = localLuceneIndexRootPath + path + "\\";
        //        UpDateWebConfig(localLuceneConfigPath, appKey, updatelocalLucenePath);
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //        PriceMeCommon.BusinessLogic.LogController.WriteException(ex.Message + ex.StackTrace);
        //        if (ex.InnerException != null)
        //        {
        //            PriceMeCommon.BusinessLogic.LogController.WriteException("InnerException : " + ex.InnerException.Message + "--------" + ex.InnerException.StackTrace);
        //        }
        //    }
        //}

        //public static void ModifyLuceneConfig2(string path)
        //{
        //    try
        //    {
        //        string userID = System.Configuration.ConfigurationManager.AppSettings["userid2"];
        //        string password = System.Configuration.ConfigurationManager.AppSettings["password2"];
        //        string targetIP = System.Configuration.ConfigurationManager.AppSettings["targetIP2"];
        //        string targetLuceneConfigPath = System.Configuration.ConfigurationManager.AppSettings["TargetLuceneConfigPath2"];
        //        string targetLuceneIndexRootPath = System.Configuration.ConfigurationManager.AppSettings["TargetLuceneIndexRootPath2"];

        //        targetLuceneConfigPath = @"\\" + targetIP + @"\" + targetLuceneConfigPath;

        //        string appKey = PriceMeCommon.BusinessLogic.MultiCountryController.GetIndexKey(AppValue.CountryId);
        //        path = path.TrimEnd('\\');
        //        using (IdentityScope c = new IdentityScope(userID, targetIP, password))
        //        {
        //            string updateLucenePath = targetLuceneIndexRootPath + path + "\\";
        //            UpDateWebConfig(targetLuceneConfigPath, appKey, updateLucenePath);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //        PriceMeCommon.BusinessLogic.LogController.WriteException(ex.Message + ex.StackTrace);
        //        if (ex.InnerException != null)
        //        {
        //            PriceMeCommon.BusinessLogic.LogController.WriteException("InnerException : " + ex.InnerException.Message + "--------" + ex.InnerException.StackTrace);
        //        }
        //    }
        //}

        internal static string FixKeywords(string searchFieldString)
        {
            string rs = illegalReg.Replace(searchFieldString, "-");
            rs = rs.Replace("-", " ");
            return rs.Trim();
        }
    }
}