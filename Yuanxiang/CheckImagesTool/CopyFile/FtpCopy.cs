using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;

namespace CopyFile
{
    public class FtpCopy
    {
        public static int timeOut = 600000;

        public static void Download(string localFileDir, string fileSrc, string fileName, string ftpServerIP, string ftpUserName, string ftpPwd)
        {
            if (!Directory.Exists(localFileDir))
            {
                Directory.CreateDirectory(localFileDir);
            }
            using (FileStream OutputStream = new FileStream(localFileDir + "\\" + fileName, FileMode.Create))
            {
                FtpWebRequest ReqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://" + ftpServerIP + "/" + fileSrc));

                ReqFTP.Method = WebRequestMethods.Ftp.DownloadFile;

                ReqFTP.UseBinary = true;
                ReqFTP.Timeout = 999999999;
                ReqFTP.Credentials = new NetworkCredential(ftpUserName, ftpPwd);

                using (FtpWebResponse response = (FtpWebResponse)ReqFTP.GetResponse())
                {
                    using (Stream FtpStream = response.GetResponseStream())
                    {
                        long Cl = response.ContentLength;

                        int bufferSize = 2048;

                        int readCount;

                        byte[] buffer = new byte[bufferSize];

                        readCount = FtpStream.Read(buffer, 0, bufferSize);

                        while (readCount > 0)
                        {
                            OutputStream.Write(buffer, 0, readCount);
                            readCount = FtpStream.Read(buffer, 0, bufferSize);
                        }
                        FtpStream.Close();
                    }
                    response.Close();
                }
                OutputStream.Close();
            }
        }

        public static void MakeDir(string ftpFolderPath, string ftpServerIP, string ftpUserName, string ftpPwd)
        {
            try
            {
                string newFtpFolderPath = ftpFolderPath.Replace("\\", "/");
                string uri = "ftp://" + ftpServerIP + "/" + newFtpFolderPath;

                FtpWebRequest reqFTP;
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri));
                reqFTP.Credentials = new NetworkCredential(ftpUserName, ftpPwd);
                reqFTP.KeepAlive = false;
                reqFTP.Method = WebRequestMethods.Ftp.MakeDirectory;
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                response.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static void UploadFileSmall(string sFileDstPath, string FolderName, string ftpServerIP, string ftpUserName, string ftpPwd)
        {
            FileInfo fileInf = new FileInfo(sFileDstPath);

            FtpWebRequest reqFTP;

            string newFoderName = FolderName.Replace("\\", "/");

            reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://" + ftpServerIP + "/" + newFoderName + "/" + fileInf.Name));

            reqFTP.Credentials = new NetworkCredential(ftpUserName, ftpPwd);

            reqFTP.KeepAlive = false;

            reqFTP.Method = WebRequestMethods.Ftp.UploadFile;

            reqFTP.UseBinary = true;

            reqFTP.ContentLength = fileInf.Length;

            int buffLength = 2048;

            byte[] buff = new byte[buffLength];

            int contentLen;

            using (FileStream fs = fileInf.OpenRead())
            {
                using (Stream strm = reqFTP.GetRequestStream())
                {
                    contentLen = fs.Read(buff, 0, buffLength);

                    while (contentLen != 0)
                    {
                        strm.Write(buff, 0, contentLen);

                        contentLen = fs.Read(buff, 0, buffLength);
                    }
                    strm.Close();
                }
                fs.Close();
            }
        }

        public static void UploadDirectorySmall(string localDirectory, string ftpFolderName, string ftpServerIP, string ftpUserName, string ftpPwd, string notCopyFlag)
        {
            CopyDirectory(localDirectory, ftpFolderName, ftpServerIP, ftpUserName, ftpPwd, notCopyFlag);
        }

        static void CopyDirectory(string localPath, string ftpFolderName, string ftpServerIP, string ftpUserName, string ftpPwd, string notCopyFlag)
        {
            DirectoryInfo localDirInfo = new DirectoryInfo(localPath);
            string newFtpFolderName = ftpFolderName + "\\" + localDirInfo.Name;
            MakeDir(newFtpFolderName, ftpServerIP, ftpUserName, ftpPwd);
            CopyCurrentDirectoryFile(localPath, newFtpFolderName, ftpServerIP, ftpUserName, ftpPwd);
            CopyAllSubDirectoryFile(localPath, localDirInfo.Name, newFtpFolderName, ftpServerIP, ftpUserName, ftpPwd, notCopyFlag);
        }

        static void CopyCurrentDirectoryFile(string localPath, string folderName, string ftpServerIP, string ftpUserName, string ftpPwd)
        {
            string[] files = System.IO.Directory.GetFiles(localPath);
            foreach (string file in files)
            {
                System.IO.FileInfo fileInfo = new System.IO.FileInfo(file);
                UploadFileSmall(fileInfo.FullName, folderName, ftpServerIP, ftpUserName, ftpPwd);
            }
        }

        static void CopyAllSubDirectoryFile(string localPath, string folderName, string ftpFolderName, string ftpServerIP, string ftpUserName, string ftpPwd, string notCopyFlag)
        {
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

                //if (!System.IO.Directory.Exists(targetPath + @"\" + dirInfo.Name))
                //{
                //    System.IO.Directory.CreateDirectory(targetPath + @"\" + dirInfo.Name);
                //}
                string newFtpFolderName = ftpFolderName + "\\" + dirInfo.Name;
                //string newFtpFolderName = newCurrentFolderName + "\\" + dirInfo.Name;
                MakeDir(newFtpFolderName, ftpServerIP, ftpUserName, ftpPwd);
                CopyCurrentDirectoryFile(dirInfo.FullName, newFtpFolderName, ftpServerIP, ftpUserName, ftpPwd);
                CopyAllSubDirectoryFile(dirInfo.FullName, dirInfo.Name, newFtpFolderName, ftpServerIP, ftpUserName, ftpPwd, notCopyFlag);
            }
        }

        public static bool CheckDirExists(string ftpfilePath, string ftpServerIP, string ftpUserName, string ftpPwd)
        {
            string dirName = ftpfilePath.Replace("/", "\\").Substring(ftpfilePath.Replace("/", "\\").LastIndexOf("\\") + 1);
            ftpfilePath = ftpfilePath.Replace("/", "\\").Substring(0, ftpfilePath.Replace("/", "\\").LastIndexOf("\\"));
            var request = (FtpWebRequest)WebRequest.Create(new Uri("ftp://" + ftpServerIP + "/" + ftpfilePath));
            request.Credentials = new NetworkCredential(ftpUserName, ftpPwd);
            request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
            try
            {
                string line = "";
                using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream());
                    line = reader.ReadToEnd().ToString();
                    reader.Close();
                    response.Close();
                }
                if (dirName.Length > 2)
                {
                    if (!String.IsNullOrEmpty(line) && line.Contains(dirName))
                        return true;
                }
                else
                {
                    if (!String.IsNullOrEmpty(line) && line.Contains(dirName + "\r\n"))
                        return true;
                }
            }
            catch (WebException ex)
            {
                Console.WriteLine(ex.Message + ex.StackTrace);
                return false;
            }
            return false;
        }

        public static string[] GetDirList(string ftpServerIP, string ftpUserID, string ftpPassword, string dirName)
        {
            MakeDir(dirName, ftpServerIP, ftpUserID, ftpPassword);

            string[] downloadFiles;
            StringBuilder result = new StringBuilder(); FtpWebRequest reqFTP;
            try
            {
                //    dirName= Test/2013 12 20
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://" + ftpServerIP + "/" + dirName));
                reqFTP.UseBinary = true;
                reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
                reqFTP.Method = WebRequestMethods.Ftp.ListDirectory;
                reqFTP.Timeout = timeOut;
                reqFTP.ReadWriteTimeout = timeOut;
                reqFTP.KeepAlive = true;
                using (WebResponse response = reqFTP.GetResponse())
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        string line = reader.ReadLine();
                        while (line != null)
                        {
                            result.Append(line);
                            result.Append("\n");
                            line = reader.ReadLine();
                        }
                        // to remove the trailing '\n'    
                        if (result.ToString().Contains("\n"))
                            result.Remove(result.ToString().LastIndexOf('\n'), 1);
                        reader.Close();
                    }
                    response.Close();
                }
                return result.ToString().Split('\n');
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ex.StackTrace);
                downloadFiles = null;
                throw ex;
            }
        }

        /// <summary>
        /// 获取目录下最新的一个文件
        /// </summary>
        /// <param name="ftpServerIP"></param>
        /// <param name="ftpUserID"></param>
        /// <param name="ftpPassword"></param>
        /// <param name="dirName"></param>
        /// <returns></returns>
        public static string DownLoadLatestFile(string ftpServerIP, string ftpUserID, string ftpPassword, string dirName, string localFileDir)
        {
            string latestFile = "";
            FtpWebRequest reqFTP;
            List<string> allFiles = new List<string>();

            string[] files = GetDirList(ftpServerIP, ftpUserID, ftpPassword, dirName);
            try
            {
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://" + ftpServerIP + "/" + dirName));
                reqFTP.Timeout = timeOut;
                reqFTP.KeepAlive = true;
                reqFTP.ReadWriteTimeout = timeOut;
                reqFTP.UseBinary = true;
                reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
                reqFTP.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
                using (WebResponse response = reqFTP.GetResponse())
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        string line = reader.ReadLine();
                        if (!String.IsNullOrEmpty(line)) allFiles.Add(line);
                        while (line != null)
                        {
                            line = reader.ReadLine();
                            if (!String.IsNullOrEmpty(line)) allFiles.Add(line);
                        }
                        reader.Close();
                    }
                    response.Close();
                }

                DateTime temp = DateTime.Now;
                TimeSpan ts = TimeSpan.MaxValue;
                foreach (string fileDetail in allFiles)
                {
                    List<string> de = fileDetail.Split(' ').ToList();
                    de = de.Where(s => s != "").ToList();
                    string[] dates = de[0].Split('-');
                    string time = de[1];
                    if (time.Contains("PM"))
                    {
                        string[] times = time.Split(':');
                        time = ((int.Parse(times[0]) + 12).ToString() + ":" + times[1]).Replace("PM", "") + ":00";
                    }
                    else
                        time = time.Replace("AM", ":00");

                    string dateStr = "";

                    string[] timeStr = time.Split(':');
                    int year = int.Parse(DateTime.Now.Year.ToString().Substring(0, 2) + dates[2]);
                    DateTime dt = new DateTime(year, int.Parse(dates[0]), int.Parse(dates[1]), int.Parse(timeStr[0]), int.Parse(timeStr[1]), int.Parse(timeStr[2]));
                    TimeSpan span = temp - dt;
                    if (span < ts)
                    {
                        ts = span;
                        latestFile = de[3];
                    }
                }
                Download(localFileDir, dirName + "//" + latestFile, latestFile, ftpServerIP, ftpUserID, ftpPassword);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ex.StackTrace);
                throw ex;
            }
            return latestFile;
        }
        /// <summary>    
        /// 获取目录列表  
        ///  </summary> 
        /// <returns></returns> 
        public static string GetFileList(string ftpServerIP, string ftpUserID, string ftpPassword, string dirName, string file)
        {
            List<string> downloadFiles = new List<string>();
            StringBuilder result = new StringBuilder();
            FtpWebRequest reqFTP;
            try
            {
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://" + ftpServerIP + "/" + dirName));
                reqFTP.Timeout = timeOut;
                reqFTP.KeepAlive = true;
                reqFTP.ReadWriteTimeout = timeOut;
                reqFTP.UseBinary = true;
                reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
                reqFTP.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
                using (WebResponse response = reqFTP.GetResponse())
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        string line = reader.ReadLine();
                        while (line != null)
                        {
                            if (line.Contains(file)) return dirName + "/" + file;
                            else
                                line = reader.ReadLine();
                        }
                        reader.Close();
                    }
                    response.Close();
                }

                return "";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ex.StackTrace);
                downloadFiles = null;
                throw ex;
                //return "";
            }
        }

        /// <summary>
        /// method to check the existance of a file on the server
        /// </summary>
        /// <param name="fileName">file name e.g file1.txt</param>
        /// <param name="strFTPPath">FTP server path i.e: ftp://yourserver/foldername</param>
        /// <param name="strftpUserID">username</param>
        /// <param name="strftpPassword">password</param>
        /// <returns>true (if file exists) or false</returns>
        public static bool CheckFTPFile(string targetPath, string fileName, string ftpServerIP, string strftpUserID, string strftpPassword)
        {
            try
            {
                FtpWebRequest reqFTP;
                // dirName = name of the directory to create.
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://" + ftpServerIP + targetPath));
                reqFTP.ReadWriteTimeout = timeOut;
                reqFTP.Timeout = timeOut;
                reqFTP.KeepAlive = true;
                reqFTP.Method = WebRequestMethods.Ftp.ListDirectory;
                reqFTP.UseBinary = true;
                reqFTP.Credentials = new NetworkCredential(strftpUserID, strftpPassword);
                using (FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse())
                {
                    using (StreamReader ftpStream = new StreamReader(response.GetResponseStream()))
                    {
                        List<string> files = new List<string>();
                        string line = ftpStream.ReadLine();
                        while (line != null)
                        {
                            files.Add(line);
                            line = ftpStream.ReadLine();
                        }
                        ftpStream.Close();
                    }
                    response.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ex.StackTrace);
            }
            return false;
        }


        /// <summary>    
        ///  删除指定文件  
        /// </summary> 
        /// <param name="fileName">远程文件的路径</param> 
        /// 
        public static string DeleteFileName(string ftpServerIP, string ftpUserID, string ftpPassword, string targetPath, string targetFile)
        {
            string sRet = "OK";
            FtpWebRequest reqFTP;
            try
            {
                //targetPath = Test/2013 12 20/11
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://" + ftpServerIP + "/" + targetPath + "/" + targetFile));
                reqFTP.Timeout = timeOut;
                // 在一个命令之后被执行 
                reqFTP.KeepAlive = true;
                reqFTP.ReadWriteTimeout = timeOut;
                // 指定执行什么命令 
                reqFTP.Method = WebRequestMethods.Ftp.DeleteFile;
                reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                response.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ex.StackTrace);
                throw ex;
            }
            return "";
        }

        public static string DeleteDirName(string ftpServerIP, string ftpUserID, string ftpPassword, string targetPath, string targetFile)
        {
            string sRet = "OK";
            FtpWebRequest reqFTP;
            try
            {
                //targetPath = Test/2013 12 20/11
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://" + ftpServerIP + "/" + targetPath));
                reqFTP.Timeout = timeOut;
                // 在一个命令之后被执行
                reqFTP.ReadWriteTimeout = timeOut;
                reqFTP.KeepAlive = true;
                // 指定执行什么命令 
                reqFTP.Method = WebRequestMethods.Ftp.RemoveDirectory;
                reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                response.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ex.StackTrace);
                throw ex;
            }
            return "";
        }
    }
}
