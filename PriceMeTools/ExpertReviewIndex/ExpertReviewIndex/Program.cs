using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

namespace ExpertReviewIndex
{
    class Program
    {
        public static bool isER = false;
        public static bool isRA = false;
        public static bool isVa = false;
        
        static string userIDFTP = SiteConfig.AppSettings("useridFTP");
        static string passwordFTP = SiteConfig.AppSettings("passwordFTP");
        static string targetIPFTP = SiteConfig.AppSettings("targetIPFTP");
        static string IsCopyFTP = SiteConfig.AppSettings("IsCopyFTP").ToString();
        public static List<int> ridList = new List<int>();

        static void Main(string[] args)
        {
            System.Console.WriteLine("Begin.......");

            string date = DateTime.Now.ToString("yyyyMMddHH");

            System.Console.WriteLine("Builder ExpertAverage Index.......");
            BuilderExpertAverageIndex bi = new BuilderExpertAverageIndex();
            isRA = bi.Builder(date);
            System.Console.WriteLine("Builder ExpertReview Index.......");
            BuilderExpertReviewIndex er = new BuilderExpertReviewIndex();
            isER = er.Builder(date);

            System.Console.WriteLine("Builder ProductVideo Index.......");
            BuilderProductVideoIndex pv = new BuilderProductVideoIndex();
            isVa = pv.Builder(date);

            if (isER && isRA && isVa)
            {
                try
                {
                    BuildIndexLog.WriterLog("Copy Index...    at: " + DateTime.Now);

                    string indexPath = SiteConfig.AppSettings("IndexRootPath") + @"\" + date;
                    
                    ModifyLuceneConfig(indexPath);
                    
                    if (IsCopyFTP.ToLower() == "true")
                    {
                        bool iserCopyFTP = CopyIndexFTP(indexPath);

                        if (iserCopyFTP)
                            ModifyLuceneConfigFTP();
                    }
                }
                catch (Exception ex)
                {
                    BuildIndexLog.WriterLog("Copy Index error at: " + ex.Message + ex.StackTrace);
                }
            }
        }

        private static void ModifyLuceneConfigFTP()
        {
            try
            {
                string targetLuceneConfigPathFTP = SiteConfig.AppSettings("TargetLuceneConfigPathFTP");
                string targetLuceneIndexRootPathFTP = SiteConfig.AppSettings("TargetLuceneIndexRootPathFTP");
                string luceneConfigFileCopyDirFTP = SiteConfig.AppSettings("LuceneConfigFileCopyDirFTP");

                string luceneConfigFileNameFTP = SiteConfig.AppSettings("TargetLuceneConfigNameFTP");
                string luceneConfigFilePathFTP = targetLuceneConfigPathFTP + luceneConfigFileNameFTP;

                CopyFile.FtpCopy.Download(luceneConfigFileCopyDirFTP, luceneConfigFilePathFTP, luceneConfigFileNameFTP, targetIPFTP, userIDFTP, passwordFTP);

                string localluceneConfigFilePath = luceneConfigFileCopyDirFTP + luceneConfigFileNameFTP;
                string appKey = "ExpertReviewIndexPath";

                string updateLucenePath = SiteConfig.AppSettings("targetLuceneIndexRootPathFTP") + "\\" + DateTime.Now.ToString("yyyyMMddHH") + "\\";
                UpDateWebConfig(localluceneConfigFilePath, appKey, updateLucenePath);

                CopyFile.FtpCopy.UploadFileSmall(localluceneConfigFilePath, targetLuceneConfigPathFTP, targetIPFTP, userIDFTP, passwordFTP);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                BuildIndexLog.WriterLog(ex.Message + ex.StackTrace);
            }
        }

        private static void ModifyLuceneConfig(string indexPath)
        {
            BuildIndexLog.WriterLog("ModifyLuceneConfig...");
            
            try
            {
                string[] lucenepaths = SiteConfig.AppSettings("LocalLuceneConfigPath").Split(';');
                foreach (string lucenepath in lucenepaths)
                {
                    string appKey = "ExpertReviewIndexPath";

                    UpDateWebConfig(lucenepath, appKey, indexPath);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                BuildIndexLog.WriterLog(ex.Message + ex.StackTrace);
            }
        }

        private static bool UpDateWebConfig(string configFilePath, string appKey, string indexPath)
        {
            if (!File.Exists(configFilePath))
            {
                Console.WriteLine("no file path :" + configFilePath);
                BuildIndexLog.WriterLog("ConfigFilePath no file path :" + configFilePath);
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
                BuildIndexLog.WriterLog("ConfigFilePath update " + configFilePath + " successful!");
            }
            else
            {
                Console.WriteLine("update " + configFilePath + " fail!");
                BuildIndexLog.WriterLog("ConfigFilePath update " + configFilePath + " fail!");
            }
            return successful;
        }

        private static bool CopyIndexFTP(string todayIndexPathIndexPath)
        {
            try
            {
                string targetPathFTP = SiteConfig.AppSettings("targetPathFTP");
                CopyFile.FtpCopy.UploadDirectorySmall(todayIndexPathIndexPath, targetPathFTP, targetIPFTP, userIDFTP, passwordFTP, "_notCopy");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                BuildIndexLog.WriterLog("Copy File: " + ex.Message);
                return false;
            }
        }
    }
}
