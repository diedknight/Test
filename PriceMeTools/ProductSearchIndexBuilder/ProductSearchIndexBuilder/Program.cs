using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Xml;

namespace ProductSearchIndexBuilder
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json");

            var configuration = builder.Build();

            LogController.Init(configuration);
            AppValue.Init(configuration);

            DbInfo priceme205DbInfo = new DbInfo();
            priceme205DbInfo.ConnectionString = configuration.GetConnectionString("205Db");
            priceme205DbInfo.ProviderName = configuration["205Db_ProviderName"];

            DbInfo pamUserDbInfo = new DbInfo();
            pamUserDbInfo.ConnectionString = configuration.GetConnectionString("PamUser");
            pamUserDbInfo.ProviderName = configuration["PamUser_ProviderName"];

            DbInfo subDbInfo = new DbInfo();
            subDbInfo.ConnectionString = configuration.GetConnectionString("SubDb");
            subDbInfo.ProviderName = configuration["SubDb_ProviderName"];

            LogController.WriteLog("Init data --- at : " + DateTime.Now.ToString("yyyyMMdd HH:mm:ss"));
            Console.WriteLine("Please wait......");

            CachBuilder.Init(AppValue.RedisHost, AppValue.RedisName);
            DataController.Init(priceme205DbInfo, pamUserDbInfo, subDbInfo);

            if (args.Length == 0)
            {
                BuildIndexByHand(priceme205DbInfo, pamUserDbInfo);
            }
            else
            {
                BuildIndexAuto(priceme205DbInfo, pamUserDbInfo);
            }
        }

        private static void BuildIndexAuto(DbInfo priceme205DbInfo, DbInfo pamUserbInfo)
        {
            LogController.WriteLog("Build Index Auto --- at : " + DateTime.Now.ToString("yyyyMMdd HH:mm:ss"));
            
            string indexPath = GetTodayIndexPath(false);

            int startHours = DateTime.Now.Hour;
            bool needReBuildCache = true;
            //如果当前时间大于指定时间则不造Cache
            if (startHours > AppValue.FlagVelocityHour)
            {
                needReBuildCache = false;
            }

            LogController.WriteLog("Current Index Save To:" + indexPath);

            IndexSpeedLog indexSpeedLog = new IndexSpeedLog();

            int trackCount = GetRetailerTrackCount(priceme205DbInfo);
            if (trackCount == 0)
            {
                SetAdminEmail(2);
                Console.WriteLine("Build index failed! Track count 0.");
            }
            else
            {
                bool successful = BuildAllIndex(indexPath, priceme205DbInfo, pamUserbInfo);

                if (successful)
                {
                    if (needReBuildCache)
                    {
                        LogController.WriteLog("Build cache --- at : " + DateTime.Now.ToString("yyyyMMdd HH:mm:ss"));
                        CachBuilder.BuildCache();
                    }

                    string[] luceneConfigFilePathArray = AppValue.LocalLuceneConfigPath.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (var lcPath in luceneConfigFilePathArray)
                    {
                        if (File.Exists(lcPath))
                            UpDateWebConfig(lcPath, AppValue.LuceneKey, indexPath);
                        else
                            LogController.WriteException("LuceneConfigPath : " + lcPath + " file not exists.");
                    }
                }

                if (AppValue.ToFTP)
                {
                    try
                    {
                        FtpCopy.UploadDirectorySmall(indexPath, AppValue.FtpTargetPath, AppValue.FtpTargetIP, AppValue.FtpUserID, AppValue.FtpPassword, "-no");

                        DirectoryInfo dirInfo = new DirectoryInfo(indexPath);

                        ModifyLuceneConfigFTP(dirInfo.FullName);
                    }
                    catch (Exception e)
                    {
                        LogController.WriteException(e.StackTrace + e.Message);
                    }
                }
            }
        }

        private static void ModifyLuceneConfigFTP(string path)
        {
            try
            {
                string luceneConfigFilePath = Path.Combine(AppValue.FtpTargetLuceneConfigPath, AppValue.FtpLuceneConfigFileName);

                FtpCopy.Download(AppValue.FtpLuceneConfigFileCopyDir, luceneConfigFilePath, AppValue.FtpLuceneConfigFileName, AppValue.FtpTargetIP, AppValue.FtpUserID, AppValue.FtpPassword);

                string localluceneConfigFilePath = Path.Combine(AppValue.FtpLuceneConfigFileCopyDir, AppValue.FtpLuceneConfigFileName);

                path = path.TrimEnd('\\');

                string updateLucenePath = Path.Combine(AppValue.FtpTargetLuceneIndexRootPath, path + "\\");
                UpDateWebConfig(localluceneConfigFilePath, AppValue.LuceneKey, updateLucenePath);

                FtpCopy.UploadFileSmall(localluceneConfigFilePath, AppValue.FtpTargetLuceneConfigPath, AppValue.FtpTargetIP, AppValue.FtpUserID, AppValue.FtpPassword);
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

        private static bool BuildAllIndex(string todayIndexPathIndexPath, DbInfo priceme205DbInfo, DbInfo pamUserbInfo)
        {
            //
            //Attributes
            bool flog = BuildingProductsDescriptorIndex(todayIndexPathIndexPath, priceme205DbInfo);
            if (!flog)
            {
                LogController.WriteException("BuildingProductsDescriptorIndex failed!");
                return flog;
            }

            flog = BuildAllCategoriesProductIndex(todayIndexPathIndexPath, priceme205DbInfo, pamUserbInfo);
            if (!flog)
            {
                LogController.WriteException("BuildAllCategoriesProductIndex failed!");
                return flog;
            }

            flog = BuildingCategoriesIndex(todayIndexPathIndexPath, priceme205DbInfo);
            if (!flog)
            {
                LogController.WriteException("BuildingCategoriesIndex failed!");
                return flog;
            }

            flog = BuildingRetailerProductsIndex(todayIndexPathIndexPath, priceme205DbInfo);
            if (!flog)
            {
                LogController.WriteException("BuildingCategoriesIndex failed!");
                return flog;
            }

            //
            flog = BuildingProductRetailerMapIndex(todayIndexPathIndexPath, priceme205DbInfo);
            if (!flog)
            {
                LogController.WriteException("BuildingProductRetailerMapIndex failed!");
                return flog;
            }

            return flog;
        }

        private static bool BuildingProductRetailerMapIndex(string todayIndexPathIndexPath, DbInfo priceme205DbInfo)
        {
            LogController.WriteLog("Start Building <ProductRetailerMap> Index on " + DateTime.Now.ToLocalTime().ToString("yyyyMMdd hh:mm:ss"));

            try
            {
                IndexBuilder.ProductRetailerMap(todayIndexPathIndexPath + "ProductRetailerMap", AppValue.CountryId, priceme205DbInfo);
            }
            catch (Exception ex)
            {
                LogController.WriteException(ex.Message + "--------" + ex.StackTrace);
                if (ex.InnerException != null)
                {
                    LogController.WriteException("InnerException : " + ex.InnerException.Message + "--------" + ex.InnerException.StackTrace);
                }
                return false;
            }

            LogController.WriteLog("<ProductRetailerMap> Index Build Finished on " + DateTime.Now.ToLocalTime().ToString("yyyyMMdd hh:mm:ss"));

            return true;
        }

        private static bool BuildingRetailerProductsIndex(string todayIndexPathIndexPath, DbInfo priceme205DbInfo)
        {
            LogController.WriteLog("Start Building <RetailerProducts> Index on " + DateTime.Now.ToLocalTime().ToString("yyyyMMdd hh:mm:ss"));

            try
            {
                IndexBuilder.BuildRetailerProductsIndex(todayIndexPathIndexPath + "RetailerProducts", AppValue.CountryId, priceme205DbInfo);
            }
            catch (Exception ex)
            {
                LogController.WriteException(ex.Message + "--------" + ex.StackTrace);
                if (ex.InnerException != null)
                {
                    LogController.WriteException("InnerException : " + ex.InnerException.Message + "--------" + ex.InnerException.StackTrace);
                }
                return false;
            }

            LogController.WriteLog("<RetailerProducts> Index Build Finished on " + DateTime.Now.ToLocalTime().ToString("yyyyMMdd hh:mm:ss"));
            return true;
        }

        private static bool BuildingCategoriesIndex(string todayIndexPathIndexPath, DbInfo priceme205DbInfo)
        {
            LogController.WriteLog("Start Building <Categories> Index on " + DateTime.Now.ToLocalTime().ToString("yyyyMMdd hh:mm:ss"));

            try
            {
                IndexBuilder.BuildCategoriesIndex(todayIndexPathIndexPath + "Categories", priceme205DbInfo);
            }
            catch (Exception ex)
            {
                LogController.WriteException(ex.Message + "--------" + ex.StackTrace);
                if (ex.InnerException != null)
                {
                    LogController.WriteException("InnerException : " + ex.InnerException.Message + "--------" + ex.InnerException.StackTrace);
                }
                return false;
            }

            LogController.WriteLog("<Categories> Index Build Finished on " + DateTime.Now.ToLocalTime().ToString("yyyyMMdd hh:mm:ss"));
            return true;
        }

        private static bool BuildingProductsDescriptorIndex(string todayIndexPathIndexPath, DbInfo priceme205DbInfo)
        {
            LogController.WriteLog("Start Building <ProductsDescriptor> Index on " + DateTime.Now.ToLocalTime().ToString("yyyyMMdd hh:mm:ss"));

            try
            {
                IndexBuilder.BuildProductAttributesIndex(todayIndexPathIndexPath + "ProductsDescriptor", AppValue.CountryId, priceme205DbInfo);
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
            Console.WriteLine();
            return true;
        }

        private static bool BuildAllCategoriesProductIndex(string todayIndexPathIndexPath, DbInfo priceme205DbInfo, DbInfo pamUserbInfo)
        {
            LogController.WriteLog("Start Building <All Categories Product> Index on " + DateTime.Now.ToLocalTime().ToString("yyyyMMdd hh:mm:ss"));

            bool flag = false;
            try
            {
                int hours = DateTime.Now.Hour;
                flag = IndexBuilder.AsyncBuildAllCategoryProductsIndex(todayIndexPathIndexPath + "AllCategoriesProduct\\", AppValue.CountryId, hours >= AppValue.FlagHour, AppValue.ThreadCount, priceme205DbInfo, pamUserbInfo);
            }
            catch (Exception ex)
            {
                LogController.WriteException(ex.Message + "--------" + ex.StackTrace);
                return false;
            }

            LogController.WriteLog("<Categories> Index Build Finished on " + DateTime.Now.ToLocalTime().ToString("yyyyMMdd hh:mm:ss"));

            return flag;
        }

        private static void BuildIndexByHand(DbInfo priceme205DbInfo, DbInfo pamUserbInfo)
        {
            int MaxNumber = 2;

            while (true)
            {
                string inputString = "";
                Console.WriteLine("----------------------------------------------------------------------");
                Console.WriteLine("---------------------------Building Index-----------------------------");
                Console.WriteLine("----------------------------------------------------------------------");
                Console.WriteLine();
                Console.WriteLine("Select <1> Building All Index");
                Console.WriteLine("Select <2> Building Cache");
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
                        string todayIndexPathIndexPath = GetTodayIndexPath(true);
                        LogController.WriteLog("Build Index by hand --- at : " + DateTime.Now.ToString("yyyyMMdd HH:mm:ss"));
                        LogController.WriteLog("Current Index Save To:" + todayIndexPathIndexPath);
                        BuildAllIndex(todayIndexPathIndexPath, priceme205DbInfo, pamUserbInfo);
                        break;
                           
                    case 2:
                        LogController.WriteLog("Build cache --- at : " + DateTime.Now.ToString("yyyyMMdd HH:mm:ss"));
                        CachBuilder.BuildCache();
                        break;
                }
            }
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
                path = Path.Combine(AppValue.IndexRootPath, DateTime.Now.ToLocalTime().ToString("yyyyMMdd") + " byHand");
            }
            else
            {
                int hours = DateTime.Now.Hour;
                //如果超过指定时间则目录名字最后加上小时
                if (hours < AppValue.FlagHour)
                {
                    path = Path.Combine(AppValue.IndexRootPath, DateTime.Now.ToLocalTime().ToString("yyyyMMdd"));
                }
                else
                {
                    path = Path.Combine(AppValue.IndexRootPath, DateTime.Now.ToLocalTime().ToString("yyyyMMddHH"));
                }
            }
            return path + @"\";
        }

        private static int GetRetailerTrackCount(DbInfo priceme205DbInfo)
        {
            int count = 0;
            string selectSql = "SELECT count(id) as c FROM CSK_Store_RetailerTracker";
            using (var sqlConn = DBController.CreateDBConnection(priceme205DbInfo))
            {
                using (var sqlCMD = DBController.CreateDbCommand(selectSql, sqlConn))
                {
                    sqlConn.Open();

                    try
                    {
                        using (var sqlDR = sqlCMD.ExecuteReader())
                        {
                            if (sqlDR.Read())
                            {
                                count = sqlDR.GetInt32(0);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        LogController.WriteException(ex.Message + "--------" + ex.StackTrace);
                    }
                }
            }

            LogController.WriteLog("Retailer tracker count : " + count + " --- finished on " + DateTime.Now.ToLocalTime().ToString("yyyyMMdd hh:mm:ss"));
            return count;
        }

        private static void SetAdminEmail(int type)
        {
            string emailInfo = AppValue.Email;
            string[] emailInfos = emailInfo.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

            System.Net.Mail.MailMessage emailMessage = new System.Net.Mail.MailMessage();
            emailMessage.From = new System.Net.Mail.MailAddress(AppValue.InfoEmail, "PriceMe");
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

        private static bool UpDateWebConfig(string configFilePath, string appKey, string indexPath)
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
    }
}