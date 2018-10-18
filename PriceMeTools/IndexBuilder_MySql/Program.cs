using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using CopyFile;
using System.Xml;
using PriceMeDBA;
using PriceMeCommon;
using System.Diagnostics;
using PriceMeCache;
using IndexBuildCommon;
using PriceMeCommon.BusinessLogic;

namespace LuceneIndexBuild
{
    class Program
    {
        static string notCopyFlag = System.Configuration.ConfigurationManager.AppSettings["NotCopy"];
        static void Main(string[] args)
        {
            MultiCountryController.LoadForBuildIndex();
            //MultiCountryController.LoadWithoutCheckIndexPath();
            if (args.Length == 0)
            {
                BuildIndexByHand();
            }
            else
            {
                BuildIndexAuto();
            }
        }

        private static void BuildIndexAuto()
        {
            string todayIndexPathIndexPath = GetTodayIndexPath(false);

            int startHours = DateTime.Now.Hour;
            bool needReBuildVelocity = true;
            //如果当前时间大于指定时间则不造velocity
            if (startHours > AppValue.FlagVelocityHour)
            {
                needReBuildVelocity = false;
            }

            Console.WriteLine("Build Index Auto");
            Console.WriteLine("Current Index Save To:" + todayIndexPathIndexPath);

            IndexBuildCommon.Data.IndexSpeedLog indexSpeedLog = new IndexBuildCommon.Data.IndexSpeedLog();
            bool updateTrackAndRating = false;
            bool.TryParse(System.Configuration.ConfigurationManager.AppSettings["UpdateTrackAndRating"], out updateTrackAndRating);
            if (updateTrackAndRating)
            {
                IndexBuildCommon.Data.BulidIndexSpeedInfo updateRetailerTrackerSpeedInfo = new IndexBuildCommon.Data.BulidIndexSpeedInfo();
                updateRetailerTrackerSpeedInfo.StartReadDBTime = DateTime.Now;
                UpdateRetailerTracker();
                updateRetailerTrackerSpeedInfo.EndReadDBTime = DateTime.Now;

                IndexBuildCommon.Data.BulidIndexSpeedInfo updateProductRatingSpeedInfo = new IndexBuildCommon.Data.BulidIndexSpeedInfo();
                updateProductRatingSpeedInfo.StartReadDBTime = DateTime.Now;
                UpdateProductRating();
                updateProductRatingSpeedInfo.EndReadDBTime = DateTime.Now;

                indexSpeedLog.UpdateRetailerTrackerSpeedInfo = updateRetailerTrackerSpeedInfo;
                indexSpeedLog.UpdateProductRatingSpeedInfo = updateProductRatingSpeedInfo;
            }

            //if (AppValue.UpdateProductCategory)
            //{
            //    IndexBuildCommon.Data.BulidIndexSpeedInfo updateProductCategorySpeedInfo = new IndexBuildCommon.Data.BulidIndexSpeedInfo();
            //    updateProductCategorySpeedInfo.StartReadDBTime = DateTime.Now;
            //    UpdateProductCategory();
            //    updateProductCategorySpeedInfo.EndWriteIndexTime = DateTime.Now;

            //    indexSpeedLog.UpdateProductCategorySpeedInfo = updateProductCategorySpeedInfo;
            //}

            SearchController.Load();
            ManufacturerController.LoadForBuildIndex();

            IndexBuilder.indexSpeedLog = indexSpeedLog;

            try
            {
                int trackCount = GetRetailerTrackCount();
                if (trackCount == 0)
                {
                    SetAdminEmail(2);
                    Console.WriteLine("Build index failed! Track count 0.");
                }
                else
                {
                    if (BuildAllIndex(todayIndexPathIndexPath))
                    {
                        if (CopyIndex(todayIndexPathIndexPath))
                        {
                            bool copyIndexBool2 = CopyIndex2(todayIndexPathIndexPath);

                            bool copyIndexBool3 = CopyIndex3(todayIndexPathIndexPath);

                            bool copyIndexBoolFTP = CopyIndexToFtp(todayIndexPathIndexPath);

                            if (needReBuildVelocity)
                            {
                                IndexBuildCommon.Data.BulidIndexSpeedInfo bulidVelocitySpeedInfo = new IndexBuildCommon.Data.BulidIndexSpeedInfo();
                                bulidVelocitySpeedInfo.StartReadDBTime = DateTime.Now;
                                MultiCountryController.LoadWithoutCheckIndexPath();
                                CacheBuilder.BuildCache();
                                bulidVelocitySpeedInfo.EndReadDBTime = DateTime.Now;

                                IndexBuildCommon.IndexBuilder.indexSpeedLog.BuildVelocitySpeedInfo = bulidVelocitySpeedInfo;
                            }

                            if (NeedUpdateUpdateRelatedManufacturerCategories())
                            {
                                IndexBuildCommon.Data.BulidIndexSpeedInfo updateRelatedManufacturerCategoriesSpeedInfo = new IndexBuildCommon.Data.BulidIndexSpeedInfo();
                                updateRelatedManufacturerCategoriesSpeedInfo.StartReadDBTime = DateTime.Now;
                                UpdateRelatedManufacturerCategories();
                                updateRelatedManufacturerCategoriesSpeedInfo.EndReadDBTime = DateTime.Now;

                                IndexBuildCommon.IndexBuilder.indexSpeedLog.UpdateRelatedManufacturerCategoriesSpeedInfo = updateRelatedManufacturerCategoriesSpeedInfo;
                            }

                            if (copyIndexBool2)
                            {
                                ModifyLuceneConfig2(todayIndexPathIndexPath.Replace(AppValue.IndexRootPath, ""));
                            }

                            if (copyIndexBool3)
                            {
                                ModifyLuceneConfig3(todayIndexPathIndexPath.Replace(AppValue.IndexRootPath, ""));
                            }

                            if (copyIndexBoolFTP)
                            {
                                ModifyLuceneConfigFTP(todayIndexPathIndexPath.Replace(AppValue.IndexRootPath, ""));
                            }

                            if (CopyIndexMerged(todayIndexPathIndexPath))
                            {
                                ModifyLuceneConfigMerged(todayIndexPathIndexPath.Replace(AppValue.IndexRootPath, ""));
                            }
                            ModifyLuceneConfig(todayIndexPathIndexPath.Replace(AppValue.IndexRootPath, ""));

                            try
                            {
                                string priceAlertPath = System.Configuration.ConfigurationManager.AppSettings["PriceAlertPath"];
                                System.Diagnostics.Process proc = System.Diagnostics.Process.Start(priceAlertPath);
                                proc.WaitForExit();

                                LogController.WriteLog("SendPriceAlert Successful.");
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                                LogController.WriteException(ex.Message + ex.StackTrace);
                            }
                            Console.WriteLine("Build finish");
                        }
                        else
                        {
                            SetAdminEmail(1);
                            Console.WriteLine("Copy index failed!");
                        }
                    }
                    else
                    {
                        SetAdminEmail(0);
                        Console.WriteLine("Build index failed!");
                    }
                }
            }
            catch(Exception ex)
            {
                SetAdminEmail(0);
                Console.WriteLine("Build index failed!");
                PriceMeCommon.BusinessLogic.LogController.WriteException(ex.Message + ex.StackTrace);
            }

            string buildLog = indexSpeedLog.ToString();
            PriceMeCommon.BusinessLogic.LogController.WriteLog(buildLog);
        }

        private static int GetRetailerTrackCount()
        {
            int count = 0;
            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CommerceTemplate"].ConnectionString))
            using (SqlCommand sqlCmd = new SqlCommand())
            {
                sqlCmd.Connection = conn;
                sqlCmd.CommandText = "SELECT count([id]) as c FROM CSK_Store_RetailerTracker";
                sqlCmd.CommandTimeout = 0;
                try
                {
                    conn.Open();
                    using (SqlDataReader dr = sqlCmd.ExecuteReader())
                    {
                        if(dr.Read())
                        {
                            count = dr.GetInt32(0);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error occured: " + ex.Message + ex.StackTrace);
                    LogController.WriteException(ex.Message + "--------" + ex.StackTrace);
                }
            }

            LogController.WriteLog("Retailer tracker count : " + count + " --- finished on " + DateTime.Now.ToLocalTime().ToString("yyyyMMdd hh:mm:ss"));
            return count;
        }

        private static void SetAdminEmail(int type)
        {
            string emailInfo = System.Configuration.ConfigurationManager.AppSettings["Email"];
            string[] emailInfos = emailInfo.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

            System.Net.Mail.MailMessage emailMessage = new System.Net.Mail.MailMessage();
            emailMessage.From = new System.Net.Mail.MailAddress(System.Configuration.ConfigurationManager.AppSettings["InfoEmail"], "PriceMe");
            //emailMessage.To.Add(new System.Net.Mail.MailAddress("17656681@qq.com"));

            foreach (string email in emailInfos)
            {
                emailMessage.To.Add(new System.Net.Mail.MailAddress(email));
            }

            emailMessage.IsBodyHtml = false;
            emailMessage.Subject = "Lucene Index Build Fail. Country ID : " + AppValue.CountryId;
            if (type == 0)
            {
                emailMessage.Body = "Wo jiu ti shi : Lucene index build fail.";
            }
            else if (type == 1)
            {
                emailMessage.Body = "Wo jiu ti shi : Lucene index copy fail.";
            }
            else if (type == 2)
            {
                emailMessage.Body = "Retailer track mei la.";
            }
            else
            {
                emailMessage.Body = "Index mei zao.";
            }
            System.Net.Mail.SmtpClient smtpClient = new System.Net.Mail.SmtpClient();
            smtpClient.Send(emailMessage);
        }

        private static void ModifyLuceneConfigMerged(string path)
        {
            try
            {
                string userID = System.Configuration.ConfigurationManager.AppSettings["useridMerged"];
                string password = System.Configuration.ConfigurationManager.AppSettings["passwordMerged"];
                string targetIP = System.Configuration.ConfigurationManager.AppSettings["targetIPMerged"];
                string targetLuceneConfigPath = System.Configuration.ConfigurationManager.AppSettings["TargetLuceneConfigPathMerged"];
                string targetLuceneIndexRootPath = System.Configuration.ConfigurationManager.AppSettings["TargetLuceneIndexRootPathMerged"];

                targetLuceneConfigPath = @"\\" + targetIP + @"\" + targetLuceneConfigPath;

                string appKey = MultiCountryController.GetIndexKey(AppValue.CountryId);
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

        private static bool CopyIndexMerged(string todayIndexPathIndexPath)
        {
            bool copyMerged = false;
            bool.TryParse(System.Configuration.ConfigurationManager.AppSettings["CopyMerged"], out copyMerged);
            if (copyMerged)
            {
                try
                {
                    string userID = System.Configuration.ConfigurationManager.AppSettings["useridMerged"];
                    string password = System.Configuration.ConfigurationManager.AppSettings["passwordMerged"];
                    string targetIP = System.Configuration.ConfigurationManager.AppSettings["targetIPMerged"];
                    string targetPath = System.Configuration.ConfigurationManager.AppSettings["targetPathMerged"];

                    CopyFile.NetWorkCopy.Copy(targetIP, targetPath, userID, password, todayIndexPathIndexPath, notCopyFlag, false);
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
                return false;
            }
        }

        private static bool NeedUpdateUpdateRelatedManufacturerCategories()
        {
            string updateRelatedManufacturerCategoriesDays = System.Configuration.ConfigurationManager.AppSettings["UpdateRelatedManufacturerCategoriesDays"];
            string[] days = updateRelatedManufacturerCategoriesDays.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            List<DayOfWeek> dayOfWeekList = new List<DayOfWeek>();
            foreach (string dayInfo in days)
            {
                if (dayInfo.Trim() == "1")
                {
                    dayOfWeekList.Add(DayOfWeek.Monday);
                }
                else if (dayInfo.Trim() == "2")
                {
                    dayOfWeekList.Add(DayOfWeek.Tuesday);
                }
                else if (dayInfo.Trim() == "3")
                {
                    dayOfWeekList.Add(DayOfWeek.Wednesday);
                }
                else if (dayInfo.Trim() == "4")
                {
                    dayOfWeekList.Add(DayOfWeek.Thursday);
                }
                else if (dayInfo.Trim() == "5")
                {
                    dayOfWeekList.Add(DayOfWeek.Friday);
                }
                else if (dayInfo.Trim() == "6")
                {
                    dayOfWeekList.Add(DayOfWeek.Saturday);
                }
                else if (dayInfo.Trim() == "7")
                {
                    dayOfWeekList.Add(DayOfWeek.Sunday);
                }
            }

            DayOfWeek toDay = DateTime.Now.DayOfWeek;
            if (dayOfWeekList.Contains(toDay))
            {
                return true;
            }
            else
            {
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

                string appKey = PriceMeCommon.BusinessLogic.MultiCountryController.GetIndexKey(AppValue.CountryId);
                path = path.TrimEnd('\\');

                string updateLucenePath = targetLuceneIndexRootPath + path + "\\";
                UpDateWebConfig(localluceneConfigFilePath, appKey, updateLucenePath);

                CopyFile.FtpCopy.UploadFileSmall(localluceneConfigFilePath, targetLuceneConfigPath, targetIP, userID, password);
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

        private static void ModifyLuceneConfig3(string path)
        {
            try
            {
                string userID = System.Configuration.ConfigurationManager.AppSettings["userid3"];
                string password = System.Configuration.ConfigurationManager.AppSettings["password3"];
                string targetIP = System.Configuration.ConfigurationManager.AppSettings["targetIP3"];
                string targetLuceneConfigPath = System.Configuration.ConfigurationManager.AppSettings["TargetLuceneConfigPath3"];
                string targetLuceneIndexRootPath = System.Configuration.ConfigurationManager.AppSettings["TargetLuceneIndexRootPath3"];

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

        private static bool CopyIndex3(string todayIndexPathIndexPath)
        {
            bool copyTo3 = false;
            bool.TryParse(System.Configuration.ConfigurationManager.AppSettings["CopyTo3"], out copyTo3);
            if (copyTo3)
            {
                try
                {
                    string userID = System.Configuration.ConfigurationManager.AppSettings["userid3"];
                    string password = System.Configuration.ConfigurationManager.AppSettings["password3"];
                    string targetIP = System.Configuration.ConfigurationManager.AppSettings["targetIP3"];
                    string targetPath = System.Configuration.ConfigurationManager.AppSettings["targetPath3"];

                    CopyFile.NetWorkCopy.Copy(targetIP, targetPath, userID, password, todayIndexPathIndexPath, notCopyFlag, false);
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
                return false;
            }
        }

        //private static void MergeCategoryProductsIndex(string todayIndexPathIndexPath, bool releaseIndex)
        //{
        //    if (releaseIndex)
        //    {
        //        LuceneController.ReleaseAllSearchIndex();
        //    }

        //    string path = todayIndexPathIndexPath + "AllCategoriesProduct\\";
        //    if (IndexBuildCommon.IndexBuilder.MergeNotCopyProductsIndex(path, notCopyFlag))
        //    {
        //        string allProductsIndexNotCopyPath = path + "Products" + notCopyFlag;
        //        string[] dirs = Directory.GetDirectories(path);

        //        foreach (string dir in dirs)
        //        {
        //            if (dir.EndsWith(notCopyFlag))
        //            {
        //                continue;
        //            }
        //            string newDir = dir + "_new";
        //            string notCopyPath = dir + notCopyFlag;
        //            if (Directory.Exists(notCopyPath))
        //            {
        //                if (IndexBuildCommon.IndexBuilder.MergeIndex(dir, notCopyPath, newDir))
        //                {
        //                    DeletAndRename(dir, notCopyPath, newDir);
        //                }
        //            }
        //        }

        //        dirs = Directory.GetDirectories(path);

        //        foreach (string dir in dirs)
        //        {
        //            if (dir.EndsWith(notCopyFlag))
        //            {
        //                string newDir = dir.Replace(notCopyFlag, "");
        //                Directory.Move(dir, newDir);
        //            }
        //        }
        //    }
        //}

        private static void DeletAndRename(string dir, string notCopyPath, string newDir)
        {
            IndexBuildCommon.IndexBuilder.DeleteDirectory(dir);
            IndexBuildCommon.IndexBuilder.DeleteDirectory(notCopyPath);
            Directory.Move(newDir, dir);
        }

        private static void UpdateProductCategory()
        {
            Console.WriteLine("Start update product category 205");
            try
            {
                using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CommerceTemplate"].ConnectionString))
                {
                    conn.Open();
                    using (SqlCommand sqlCmd = new SqlCommand())
                    {
                        sqlCmd.Connection = conn;
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.CommandText = "UpdateProductCategory";
                        sqlCmd.CommandTimeout = 0;

                        sqlCmd.ExecuteNonQuery();
                    }
                    PriceMeCommon.BusinessLogic.LogController.WriteLog("205 UpdateProductCategory Successful.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                PriceMeCommon.BusinessLogic.LogController.WriteException(ex.Message + ex.StackTrace);
            }
            Console.WriteLine("Update product category finished!");
        }

        private static void ModifyLuceneConfig(string path)
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

        private static void ModifyLuceneConfig2(string path)
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

        private static bool UpDateWebConfig(string configFilePath, string appKey, string indexPath)
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

        private static bool CopyIndexToFtp(string topdayIndexPath)
        {
            bool copyToFtp = bool.Parse(System.Configuration.ConfigurationManager.AppSettings["CopyToFtp"]);
            if (copyToFtp)
            {
                try
                {
                    string userID = System.Configuration.ConfigurationManager.AppSettings["userid_FTP"];
                    string password = System.Configuration.ConfigurationManager.AppSettings["password_FTP"];
                    string targetIP = System.Configuration.ConfigurationManager.AppSettings["targetIP_FTP"];
                    string targetPath = System.Configuration.ConfigurationManager.AppSettings["targetPath_FTP"];

                    CopyFile.FtpCopy.UploadDirectorySmall(topdayIndexPath, targetPath, targetIP, userID, password, notCopyFlag);
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
                return false;
            }
        }

        private static bool CopyIndex2(string todayIndexPathIndexPath)
        {
            bool copyTo2 = false;
            bool.TryParse(System.Configuration.ConfigurationManager.AppSettings["CopyTo2"], out copyTo2);
            if (copyTo2)
            {
                try
                {
                    string userID = System.Configuration.ConfigurationManager.AppSettings["userid2"];
                    string password = System.Configuration.ConfigurationManager.AppSettings["password2"];
                    string targetIP = System.Configuration.ConfigurationManager.AppSettings["targetIP2"];
                    string targetPath = System.Configuration.ConfigurationManager.AppSettings["targetPath2"];

                    CopyFile.NetWorkCopy.Copy(targetIP, targetPath, userID, password, todayIndexPathIndexPath, notCopyFlag, false);
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
                return false;
            }
        }

        private static bool CopyIndex(string todayIndexPathIndexPath)
        {
            try
            {
                string userID = System.Configuration.ConfigurationManager.AppSettings["userid"];
                string password = System.Configuration.ConfigurationManager.AppSettings["password"];
                string targetIP = System.Configuration.ConfigurationManager.AppSettings["targetIP"];
                string targetPath = System.Configuration.ConfigurationManager.AppSettings["targetPath"];

                return CopyFile.NetWorkCopy.CopyAllWeNeed(targetIP, targetPath, userID, password, todayIndexPathIndexPath, notCopyFlag, false);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                PriceMeCommon.BusinessLogic.LogController.WriteException(ex.Message + ex.StackTrace);
                return false;
            }
        }

        private static bool BuildAllIndex(string todayIndexPathIndexPath)
        {
            //
            //Attributes
            bool flog = BuildingProductsDescriptorIndex(todayIndexPathIndexPath);
            if (!flog)
            {
                Console.WriteLine("BuildingProductsDescriptorIndex failed!");
                LogController.WriteException("BuildingProductsDescriptorIndex failed!");
                return flog;
            }

            flog = BuildAllCategoriesProductIndex(todayIndexPathIndexPath);
            if (!flog)
            {
                Console.WriteLine("BuildAllCategoriesProductIndex failed!");
                LogController.WriteException("BuildAllCategoriesProductIndex failed!");
                return flog;
            }

            flog = BuildingCategoriesIndex(todayIndexPathIndexPath);
            if (!flog)
            {
                Console.WriteLine("BuildingCategoriesIndex failed!");
                LogController.WriteException("BuildingCategoriesIndex failed!");
                return flog;
            }

            flog = BuildingRetailerProductsIndex(todayIndexPathIndexPath);
            if (!flog)
            {
                Console.WriteLine("BuildingRetailerProductsIndex failed!");
                LogController.WriteException("BuildingCategoriesIndex failed!");
                return flog;
            }

            //
            flog = BuildingProductRetailerMapIndex(todayIndexPathIndexPath);
            if (!flog)
            {
                Console.WriteLine("BuildingProductRetailerMapIndex failed!");
                LogController.WriteException("BuildingProductRetailerMapIndex failed!");
                return flog;
            }

            return flog;
        }

        /// <summary>
        /// 根据配置文件的时间判断是否需要重新创建Categories，RetailerProducts，ProductRetailerMap，ProductsDescriptor的index
        /// </summary>
        /// <returns></returns>
        private static bool IsNeedCopy()
        {
            string useOriginalIndexString = System.Configuration.ConfigurationManager.AppSettings["UseOriginalIndex"];
            if (string.IsNullOrEmpty(useOriginalIndexString)) return true;

            int timeHour = int.Parse(useOriginalIndexString);
            int currentTimeHour = DateTime.Now.Hour;

            if (currentTimeHour < timeHour)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private static bool BuildingRetailerProductsIndex(string todayIndexPathIndexPath)
        {
            Console.WriteLine("---- Start Building <RetailerProducts> Index ----");
            LogController.WriteLog("Start Building <RetailerProducts> Index on " + DateTime.Now.ToLocalTime().ToString("yyyyMMdd hh:mm:ss"));

            try
            {
                RetailerController.LoadForBuildIndex();
                IndexBuilder.BuildRetailerProductsIndex(todayIndexPathIndexPath + "RetailerProducts", AppValue.CountryId);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                LogController.WriteException(ex.Message + "--------" + ex.StackTrace);
                if (ex.InnerException != null)
                {
                    LogController.WriteException("InnerException : " + ex.InnerException.Message + "--------" + ex.InnerException.StackTrace);
                }
                return false;
            }

            LogController.WriteLog("<RetailerProducts> Index Build Finished on " + DateTime.Now.ToLocalTime().ToString("yyyyMMdd hh:mm:ss"));
            Console.WriteLine("---- <RetailerProducts> Index Build Finished ----");
            Console.WriteLine();
            return true;
        }

        private static void BuildIndexByHand()
        {
            int MaxNumber = 9;
            string todayIndexPathIndexPath = GetTodayIndexPath(true);
            
            while (true)
            {
                string inputString = "";
                Console.WriteLine("----------------------------------------------------------------------");
                Console.WriteLine("---------------------------Building Index-----------------------------");
                Console.WriteLine("----------------------------------------------------------------------");
                Console.WriteLine();
                Console.WriteLine("Current Index Save To:" + todayIndexPathIndexPath);
                Console.WriteLine();
                Console.WriteLine("Select <1> Building All Index");
                Console.WriteLine("Select <2> Building All Categories Product Index");
                Console.WriteLine("Select <3> Building Categories Index");
                Console.WriteLine("Select <4> Building Attributes Index");
                Console.WriteLine("Select <5> Building ProductRetailerMap Index");
                Console.WriteLine("Select <6> Building Velocity Cache");
                Console.WriteLine("Select <7> Building RetailerProducts Index");
                Console.WriteLine("Select <8> UpdateRelatedManufacturerCategories");
                Console.WriteLine("Select <0> Exit");
                Console.WriteLine();
                Console.Write("Select number : ");

                inputString = Console.ReadLine();

                int selectedIndex = 0;

                while (!int.TryParse(inputString, out selectedIndex) || selectedIndex < 0 || selectedIndex > MaxNumber)
                {
                    Console.WriteLine();
                    Console.WriteLine("Input Fromat Error Please retype again! (Between 0 and " + MaxNumber + ")");
                    Console.Write("Select number : ");
                    inputString = Console.ReadLine();
                }

                Console.Clear();

                switch (selectedIndex)
                {
                    case 0:
                        return;

                    case 1:
                        SearchController.Load();
                        PriceMeCommon.BusinessLogic.MultiCountryController.LoadWithoutCheckIndexPath();
                        BuildingProductsDescriptorIndex(todayIndexPathIndexPath);
                        BuildAllCategoriesProductIndex(todayIndexPathIndexPath);
                        BuildingCategoriesIndex(todayIndexPathIndexPath);
                        BuildingProductRetailerMapIndex(todayIndexPathIndexPath);
                        BuildingRetailerProductsIndex(todayIndexPathIndexPath);
                        break;

                    case 2:
                        SearchController.Load();
                        PriceMeCommon.BusinessLogic.MultiCountryController.LoadWithoutCheckIndexPath();
                        BuildingProductsDescriptorIndex(todayIndexPathIndexPath);
                        BuildAllCategoriesProductIndex(todayIndexPathIndexPath);
                        break;

                    case 3:
                        SearchController.Load();
                        PriceMeCommon.BusinessLogic.MultiCountryController.LoadWithoutCheckIndexPath();
                        BuildingCategoriesIndex(todayIndexPathIndexPath);
                        break;

                    case 4:
                        SearchController.Load();
                        PriceMeCommon.BusinessLogic.MultiCountryController.LoadWithoutCheckIndexPath();
                        BuildingProductsDescriptorIndex(todayIndexPathIndexPath);
                        break;

                    case 5:
                        PriceMeCommon.BusinessLogic.MultiCountryController.LoadWithoutCheckIndexPath();
                        BuildingProductRetailerMapIndex(todayIndexPathIndexPath);
                        break;
                    case 6:
                        SearchController.Load();
                        PriceMeCommon.BusinessLogic.MultiCountryController.LoadWithoutCheckIndexPath();
                        PriceMeCommon.BusinessLogic.ManufacturerController.LoadForBuildIndex();
                        CacheBuilder.BuildCache();
                        break;
                    case 7:
                        SearchController.Load();
                        PriceMeCommon.BusinessLogic.MultiCountryController.LoadWithoutCheckIndexPath();
                        BuildingRetailerProductsIndex(todayIndexPathIndexPath);
                        break;
                    case 8:
                        UpdateRelatedManufacturerCategories();
                        break;


                    default:
                        break;
                }
            }
        }

        private static bool BuildingProductRetailerMapIndex(string todayIndexPathIndexPath)
        {
            Console.WriteLine("---- Start Building <ProductRetailerMap> Index ----");
            LogController.WriteLog("Start Building <ProductRetailerMap> Index on " + DateTime.Now.ToLocalTime().ToString("yyyyMMdd hh:mm:ss"));

            try
            {
                IndexBuilder.ProductRetailerMap(todayIndexPathIndexPath + "ProductRetailerMap", AppValue.CountryId);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                LogController.WriteException(ex.Message + "--------" + ex.StackTrace);
                if (ex.InnerException != null)
                {
                    LogController.WriteException("InnerException : " + ex.InnerException.Message + "--------" + ex.InnerException.StackTrace);
                }
                return false;
            }

            LogController.WriteLog("<ProductRetailerMap> Index Build Finished on " + DateTime.Now.ToLocalTime().ToString("yyyyMMdd hh:mm:ss"));
            Console.WriteLine("---- <ProductRetailerMap> Index Build Finished ----");
            Console.WriteLine();
            return true;
        }

        private static bool BuildingProductsDescriptorIndex(string todayIndexPathIndexPath)
        {
            Console.WriteLine("---- Start Building <ProductsDescriptor> Index ----");
            LogController.WriteLog("Start Building <ProductsDescriptor> Index on " + DateTime.Now.ToLocalTime().ToString("yyyyMMdd hh:mm:ss"));

            try
            {
                CategoryController.LoadForBuildIndex();
                AttributesController.LoadForBuildIndex();
                IndexBuilder.BuildProductAttributesIndex(todayIndexPathIndexPath + "ProductsDescriptor", AppValue.CountryId);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                LogController.WriteException(ex.Message + "--------" + ex.StackTrace);
                if (ex.InnerException != null)
                {
                    LogController.WriteException("InnerException : " + ex.InnerException.Message + "--------" + ex.InnerException.StackTrace);
                }
                return false;
            }

            LogController.WriteLog("<ProductsDescriptor> Index Build Finished on " + DateTime.Now.ToLocalTime().ToString("yyyyMMdd hh:mm:ss"));
            Console.WriteLine("---- <ProductsDescriptor> Index Build Finished ----");
            Console.WriteLine();
            return true;
        }

        private static bool BuildingCategoriesIndex(string todayIndexPathIndexPath)
        {
            Console.WriteLine("---- Start Building <Categories> Index ----");
            LogController.WriteLog("Start Building <Categories> Index on " + DateTime.Now.ToLocalTime().ToString("yyyyMMdd hh:mm:ss"));

            try
            {
                IndexBuilder.BuildCategoriesIndex(todayIndexPathIndexPath + "Categories");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                LogController.WriteException(ex.Message + "--------" + ex.StackTrace);
                if (ex.InnerException != null)
                {
                    LogController.WriteException("InnerException : " + ex.InnerException.Message + "--------" + ex.InnerException.StackTrace);
                }
                return false;
            }

            LogController.WriteLog("<Categories> Index Build Finished on " + DateTime.Now.ToLocalTime().ToString("yyyyMMdd hh:mm:ss"));
            Console.WriteLine("---- <Categories> Index Build Finished ----");
            Console.WriteLine();
            return true;
        }

        private static bool BuildAllCategoriesProductIndex(string todayIndexPathIndexPath)
        {
            Console.WriteLine("---- Start Building <All Categories Product> Index ----");
            PriceMeCommon.BusinessLogic.LogController.WriteLog("Start Building <All Categories Product> Index on " + DateTime.Now.ToLocalTime().ToString("yyyyMMdd hh:mm:ss"));

            bool flag = false;
            try
            {
                int threadCount = int.Parse(System.Configuration.ConfigurationManager.AppSettings["ThreadCount"]);
                int hours = DateTime.Now.Hour;
                flag = IndexBuilder.AsyncBuildAllCategoryProductsIndex(todayIndexPathIndexPath + "AllCategoriesProduct\\", AppValue.CountryId, hours >= AppValue.FlagHour, threadCount);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                PriceMeCommon.BusinessLogic.LogController.WriteException(ex.Message + "--------" + ex.StackTrace);
                return false;
            }

            PriceMeCommon.BusinessLogic.LogController.WriteLog("<Categories> Index Build Finished on " + DateTime.Now.ToLocalTime().ToString("yyyyMMdd hh:mm:ss"));
            Console.WriteLine("---- <Categories> Index Build Finished ----");
            Console.WriteLine();
            return flag;
        }

        private static bool CopyFileIfExist(string indexPath, string pathName)
        {
            string oldPath = AppValue.IndexRootPath + DateTime.Now.ToLocalTime().ToString("yyyyMMdd") + "\\" + pathName;
            if (!System.IO.Directory.Exists(oldPath))
            {
                return false;
            }
            string copyPath = indexPath + pathName;

            if (!System.IO.Directory.Exists(copyPath))
                System.IO.Directory.CreateDirectory(copyPath);
            CopyFile.CopyController.CopyAllSubDirectoryFile(oldPath, copyPath, "");
            return true;
        }

        static void UpdateRetailerTracker()
        {
            Console.WriteLine("Start update retailer tracker from 205");
            PriceMeCommon.BusinessLogic.LogController.WriteLog("Start update retailer tracker from 205 on " + DateTime.Now.ToLocalTime().ToString("yyyyMMdd hh:mm:ss"));

            try
            {
                using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CommerceTemplate"].ConnectionString))
                {
                    conn.Open();
                    using (SqlCommand sqlCmd = new SqlCommand())
                    {
                        sqlCmd.Connection = conn;
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.CommandText = "CSK_Store_UPDATE_RetailerTracker";
                        sqlCmd.CommandTimeout = 0;
                        try
                        {
                            sqlCmd.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Error occured: " + ex.Message + ex.StackTrace);
                            PriceMeCommon.BusinessLogic.LogController.WriteException(ex.Message + "--------" + ex.StackTrace);
                        }
                        PriceMeCommon.BusinessLogic.LogController.WriteLog("UpdateRetailerTracker Successful.");
                    }
                }
            }
            catch (Exception ex)
            {
                PriceMeCommon.BusinessLogic.LogController.WriteException(ex.Message + ex.StackTrace);
            }

            PriceMeCommon.BusinessLogic.LogController.WriteLog("Update retailer tracker from 205 finished on " + DateTime.Now.ToLocalTime().ToString("yyyyMMdd hh:mm:ss"));
            Console.WriteLine("Update retailer tracker from 205 finished");
        }

        static void UpdateProductRating()
        {
            Console.WriteLine("Start update ProductRating from 205");
            PriceMeCommon.BusinessLogic.LogController.WriteLog("Start update ProductRating from 205 on " + DateTime.Now.ToLocalTime().ToString("yyyyMMdd hh:mm:ss"));

            try
            {
                using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CommerceTemplate"].ConnectionString))
                {
                    conn.Open();
                    using (SqlCommand sqlCmd = new SqlCommand())
                    {
                        sqlCmd.Connection = conn;
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.CommandText = "CSK_Store_UPDATE_ProductRating";
                        sqlCmd.CommandTimeout = 0;
                        try
                        {
                            sqlCmd.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Error occured: " + ex.Message);
                            PriceMeCommon.BusinessLogic.LogController.WriteException(ex.Message + "--------" + ex.StackTrace);
                        }
                        PriceMeCommon.BusinessLogic.LogController.WriteLog("UpdateProductRating Successful.");
                    }
                }
            }
            catch (Exception ex)
            {
                PriceMeCommon.BusinessLogic.LogController.WriteException(ex.Message + ex.StackTrace);
            }

            PriceMeCommon.BusinessLogic.LogController.WriteLog("Update ProductRating from 205 finished on " + DateTime.Now.ToLocalTime().ToString("yyyyMMdd hh:mm:ss"));
            Console.WriteLine("Update ProductRating from 205 finished");
        }

        /// <summary>
        /// 获取造index的目录
        /// </summary>
        /// <param name="byHand"></param>
        /// <returns></returns>
        private static string GetTodayIndexPath(bool byHand)
        {
            string path = "";
            if (byHand)
            {
                path = System.IO.Path.Combine(AppValue.IndexRootPath, DateTime.Now.ToLocalTime().ToString("yyyyMMdd") + " byHand");
            }
            else
            {
                int hours = DateTime.Now.Hour;
                //如果超过指定时间则目录名字最后加上小时
                if (hours < AppValue.FlagHour)
                {
                    path = System.IO.Path.Combine(AppValue.IndexRootPath, DateTime.Now.ToLocalTime().ToString("yyyyMMdd"));
                }
                else
                {
                    path = System.IO.Path.Combine(AppValue.IndexRootPath, DateTime.Now.ToLocalTime().ToString("yyyyMMddHH"));
                }
            }
            return path + @"\";
        }

        private static void UpdateRelatedManufacturerCategories()
        {
            Console.WriteLine("UpdateRelatedManufacturerCategories");
            PriceMeCommon.BusinessLogic.LogController.WriteLog("Start UpdateRelatedManufacturerCategories on " + DateTime.Now.ToLocalTime().ToString("yyyyMMdd hh:mm:ss"));

            DeleteAllRelatedManufacturerCategories();

            PriceMeCommon.BusinessLogic.MultiCountryController.LoadWithoutCheckIndexPath();

            var list = CategoryController.GetAllRootCategoriesOrderByName(AppValue.CountryId);
            foreach (var cc in list)
            {
                try
                {
                    List<int> categoryCacheList = CategoryController.GetAllSubCategoryId(cc.CategoryID, AppValue.CountryId);

                    foreach (var subCid in categoryCacheList)
                    {
                        List<int> cidList = new List<int>();
                        cidList.Add(subCid);
                        ProductSearcher productSearcher = new ProductSearcher("", cidList, null, null, null, null, "", null, 100000, AppValue.CountryId, false, true, false, false, null, "", null, false);
                        var manufacturerInfo = productSearcher.GetRelatedCategoryManufacturerResulte();
                        foreach (var mi in manufacturerInfo)
                        {
                            if (mi.Value == "-1")
                                continue;
                            string key = subCid + "-" + mi.Value + "-" + AppValue.CountryId;
                            int productCount = mi.ProductCount;
                            int totalClicks = mi.ListOrder;

                            Console.WriteLine(key + " : " + totalClicks);
                            if (totalClicks > 0)
                            {
                                PriceMeDBA.RelatedCategory retatedCategory = new PriceMeDBA.RelatedCategory();
                                retatedCategory.CountryID = AppValue.CountryId;
                                retatedCategory.Clicks = totalClicks;
                                retatedCategory.RelatedCategoriesKey = key;
                                retatedCategory.Save(Utility.UpdateMasterDBProvider);
                            }
                            PriceMeCommon.BusinessLogic.LogController.WriteLog(key + " : " + totalClicks);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            Console.WriteLine("UpdateRelatedManufacturerCategories finished!");
            PriceMeCommon.BusinessLogic.LogController.WriteLog("End UpdateRelatedManufacturerCategories on " + DateTime.Now.ToLocalTime().ToString("yyyyMMdd hh:mm:ss"));
        }

        private static void DeleteAllRelatedManufacturerCategories()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(Utility.updateDBConnectionString))
                {
                    conn.Open();
                    using (SqlCommand sqlCmd = new SqlCommand())
                    {
                        sqlCmd.Connection = conn;
                        sqlCmd.CommandType = CommandType.Text;
                        sqlCmd.CommandText = "DELETE FROM RelatedCategories WHERE CountryID = " + AppValue.CountryId;
                        sqlCmd.CommandTimeout = 0;
                        try
                        {
                            sqlCmd.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Error occured: " + ex.Message + ex.StackTrace);
                            PriceMeCommon.BusinessLogic.LogController.WriteException(ex.Message + "--------" + ex.StackTrace);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                PriceMeCommon.BusinessLogic.LogController.WriteException(ex.Message + ex.StackTrace);
            }
        }
    }
}