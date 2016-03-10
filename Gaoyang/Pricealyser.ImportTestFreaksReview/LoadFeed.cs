using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Net;
using System.Configuration;
using ICSharpCode.SharpZipLib.GZip;
using Chilkat;

namespace Pricealyser.ImportTestFreaksReview
{
    public static class LoadFeed
    {
        static string UserId = ConfigurationManager.AppSettings["UserId"].ToString();
        static string Password = ConfigurationManager.AppSettings["Password"].ToString();
        static string FeedFilePath = ConfigurationManager.AppSettings["FeedFilePath"].ToString();

        static FtpWebRequest reqFTP;
        public static string GetFTPFile()
        {
            string fileName = DateTime.Now.ToString("yyyyMMdd");

            CopyFile.SFtpClient.SetSFtpClient("ftp.testfreaks.com", 22, UserId, Password);
            List<string> fileList = CopyFile.SFtpClient.ListFiles("files/testfreaks-to-priceme");
            bool isExist = IsExistFile(fileList, fileName);
            if (!isExist)
            {
                for (int i = 1; i < 5; i++)
                {
                    fileName = DateTime.Now.AddDays(-i).ToString("yyyyMMdd");
                    isExist = IsExistFile(fileList, fileName);
                    if (isExist)
                        break;
                }
                if (!isExist) return string.Empty;
            }

            fileName = "testfreaks-priceme-" + fileName + ".xml.gz";
            string filePath = FeedFilePath + fileName;
            CopyFile.SFtpClient.Download("files/testfreaks-to-priceme/" + fileName, filePath);

            string file = filePath.Replace(".gz", "");
            Decrypt(filePath, file);

            return file;
        }

        private static bool IsExistFile(List<string> fileList, string name)
        {
            bool isExist = false;
            foreach (string file in fileList)
            {
                if (file.Contains(name))
                {
                    isExist = true;
                    break;
                }
            }

            return isExist;
        }

        private static void Decrypt(string encryptFileName, string decryptFileName)
        {
            Stream s = new GZipInputStream(File.OpenRead(encryptFileName));
            FileStream fs = new FileStream(decryptFileName, FileMode.Create);

            int size = 2048;
            byte[] writeData = new byte[2048];
            while (true)
            {
                size = s.Read(writeData, 0, size);
                if (size > 0)
                {
                    fs.Write(writeData, 0, size);
                }
                else
                {
                    break;
                }
            }
            s.Close();
            fs.Close();
        }

        private static void Connect(string path)
        {
            // 根据uri创建FtpWebRequest对象 
            reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(path)); 
            // 指定数据传输类型 
            reqFTP.UseBinary = true;
            reqFTP.Method = WebRequestMethods.Ftp.DownloadFile;
            // ftp用户名和密码 
            reqFTP.Credentials = new NetworkCredential(UserId, Password);
        }

        public static XmlNodeList Load(string feedFile)
        {
            XmlDocument xmlDoc = new XmlDocument();

            try { xmlDoc.Load(feedFile); }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                string xmlString = GetContent(feedFile);
                xmlDoc.LoadXml(xmlString);
            }

            XmlNodeList nodes = null;

            nodes = xmlDoc.GetElementsByTagName("product");

            return nodes;
        }

        public static string GetContent(string feedFile)
        {
            StringBuilder sb = new StringBuilder();
            using (StreamReader sr = new StreamReader(feedFile))
            {
                string content = "";
                int lineCount = 0;
                while (null != (content = sr.ReadLine()))
                {
                    if (content.Contains("&"))
                    {
                        if (content.Contains("&quot;"))
                        {
                            content = content.Replace("&quot;", "");
                        }
                        if (content.Contains("&Mac176;"))
                        {
                            content = content.Replace("&Mac176;", "");
                        }
                        if (content.Contains("&reg;"))
                        {
                            content = content.Replace("&reg;", "");
                        }
                        if (content.Contains("&rsquo;"))
                        {
                            content = content.Replace("&rsquo;", "'");
                        }
                        if (content.Contains("&trade;"))
                        {
                            content = content.Replace("&trade;", "");
                        }
                        if (content.Contains("&lsquo;"))
                        {
                            content = content.Replace("&lsquo;", "");
                        }
                        if (content.Contains("&rdquo;"))
                        {
                            content = content.Replace("&rdquo;", "");
                        }
                        if (content.Contains("&ndash;"))
                        {
                            content = content.Replace("&ndash;", "-");
                        }
                        if (content.Contains("&nbsp;"))
                        {
                            content = content.Replace("&nbsp;", " ");
                        }
                        if (content.Contains("&ldquo;"))
                        {
                            content = content.Replace("&ldquo;", "");
                        }
                        if (content.Contains("&ldquo;"))
                        {
                            content = content.Replace("&ldquo;", "'");
                        }
                        if (content.Contains("&rdquo;"))
                        {
                            content = content.Replace("&rdquo;", "'");
                        }
                        if (content.Contains("&uuml;"))
                        {
                            content = content.Replace("&uuml;", "u");
                        }

                        if (content.Contains("&mdash;"))
                        {
                            content = content.Replace("&mdash;", "-");
                        }

                        content = content.Replace("&", "&amp;");

                        if (content.Contains("&amp;amp;"))
                        {
                            content = content.Replace("&amp;amp;", "&amp;");
                        }

                        if (content.Contains("&amp;gt;"))
                        {
                            content = content.Replace("&amp;gt;", "&gt;");
                        }
                    }
                    if (content.Contains("?/title>"))
                        content = content.Replace("?/title>", "</title>");
                    if (content.Contains("?/extract>"))
                        content = content.Replace("?/extract>", "</extract>");
                    content = content.Replace("鈥檚", " ").Replace("庐", "").Replace("鈩?", "").Replace("鈩", "");
                    content = content.Replace("鈥擟", " ").Replace("鈥?", " ");
                    content = content.Replace("\a ", "").Replace(">\a", ">");
                    sb.Append(content + Environment.NewLine);

                    lineCount++;
                }   
            }

            return sb.ToString();
        }
    }
}
