using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;

namespace CopyFile
{
    public static class CopyController
    {
        public static void CopyCurrentDirectoryFile(string localPath, string targetPath)
        {
            string[] files = System.IO.Directory.GetFiles(localPath);
            foreach (string file in files)
            {
                System.IO.FileInfo fileInfo = new System.IO.FileInfo(file);
                System.IO.File.Copy(file, targetPath + @"\" + fileInfo.Name, true);
            }
        }

        public static void CopyAllSubDirectoryFile(string localPath, string targetPath, string notCopyFlag)
        {
            if (!string.IsNullOrEmpty(notCopyFlag))
            {
                if (localPath.EndsWith(notCopyFlag))
                {
                    return;
                }
            }

            CopyCurrentDirectoryFile(localPath, targetPath);

            System.IO.DirectoryInfo directoryInfo = new System.IO.DirectoryInfo(localPath);
            System.IO.DirectoryInfo[] dirInfos = directoryInfo.GetDirectories();

            foreach (System.IO.DirectoryInfo dirInfo in dirInfos)
            {
                if (!string.IsNullOrEmpty(notCopyFlag))
                {
                    if (dirInfo.FullName.EndsWith(notCopyFlag))
                    {
                        continue;
                    }
                }

                if (!System.IO.Directory.Exists(targetPath + @"\" + dirInfo.Name))
                {
                    System.IO.Directory.CreateDirectory(targetPath + @"\" + dirInfo.Name);
                }
                CopyCurrentDirectoryFile(dirInfo.FullName, targetPath + @"\" + dirInfo.Name);
                CopyAllSubDirectoryFile(dirInfo.FullName, targetPath + @"\" + dirInfo.Name, notCopyFlag);
            }
        }

        /// <summary>
        /// 复制指定的文件
        /// </summary>
        /// <param name="localPath"></param>
        /// <param name="targetPath"></param>
        /// <param name="file"></param>
        public static void CopySpecifiedFile(string localPath, string targetPath, string file)
        {
            System.IO.FileInfo fileInfo = new System.IO.FileInfo(localPath + @"\" + file);
            System.IO.File.Copy(fileInfo.FullName, targetPath + @"\" + fileInfo.Name, true);
        }

        public static void CopyCurrentFile(string localFilePath, string targetFilePath)
        {
            if (File.Exists(targetFilePath))
            {
                File.Delete(targetFilePath);
            }
            File.Copy(localFilePath, targetFilePath, true);
        }

        public static bool UpdateWebConfig(string configFilePath, string appKey)
        {
            if (!File.Exists(configFilePath))
            {
                Console.WriteLine("no file path :" + configFilePath);
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
                    xmlNode.Attributes[1].Value = DateTime.Today.ToString("yyyyMMdd");

                    successful = true;
                    break;
                }
            }

            xmlDoc.Save(configFilePath);

            return successful;
        }
    }
}
