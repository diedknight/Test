using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace ProductSearchIndexBuilder
{
    public class FtpCopy
    {
        public static int timeOut = 120000;
        public static void Download(string localFileDir, string fileSrc, string fileName, string ftpServerIP, string ftpUserName, string ftpPwd)
        {
            if (!Directory.Exists(localFileDir))
            {
                Directory.CreateDirectory(localFileDir);
            }
            using (FileStream OutputStream = new FileStream(localFileDir + "\\" + fileName, FileMode.Create))
            {
                FtpWebRequest ReqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://" + ftpServerIP + "/" + fileSrc));
                ReqFTP.Timeout = timeOut;
                ReqFTP.Method = WebRequestMethods.Ftp.DownloadFile;

                ReqFTP.UseBinary = true;

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
                if (!CheckDirExists(ftpFolderPath, ftpServerIP, ftpUserName, ftpPwd))
                {
                    string newFtpFolderPath = ftpFolderPath.Replace("\\", "/");
                    string uri = "ftp://" + ftpServerIP + "/" + newFtpFolderPath;

                    FtpWebRequest reqFTP;
                    reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri));
                    reqFTP.Timeout = timeOut;
                    reqFTP.Credentials = new NetworkCredential(ftpUserName, ftpPwd);
                    reqFTP.KeepAlive = false;
                    reqFTP.Method = WebRequestMethods.Ftp.MakeDirectory;
                    FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                    response.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw ex;
            }
        }

        public static void UploadFileSmall(string sFileDstPath, string FolderName, string ftpServerIP, string ftpUserName, string ftpPwd)
        {
            try
            {
                FileInfo fileInf = new FileInfo(sFileDstPath);
                if (fileInf.Exists)
                {
                    FtpWebRequest reqFTP;

                    string newFoderName = FolderName.Replace("\\", "/");
                    string url = "ftp://" + ftpServerIP + "/" + newFoderName + "/" + fileInf.Name;
                    reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(url));
                    reqFTP.Timeout = timeOut;
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
            }
            catch (Exception e)
            {
                throw e;
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
            string[] files = Directory.GetFiles(localPath);
            foreach (string file in files)
            {
                FileInfo fileInfo = new FileInfo(file);
                UploadFileSmall(fileInfo.FullName, folderName, ftpServerIP, ftpUserName, ftpPwd);
            }
        }

        static void CopyAllSubDirectoryFile(string localPath, string folderName, string ftpFolderName, string ftpServerIP, string ftpUserName, string ftpPwd, string notCopyFlag)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(localPath);
            DirectoryInfo[] dirInfos = directoryInfo.GetDirectories();

            foreach (DirectoryInfo dirInfo in dirInfos)
            {
                if (!string.IsNullOrEmpty(notCopyFlag))
                {
                    if (dirInfo.FullName.EndsWith(notCopyFlag))
                    {
                        continue;
                    }
                }

                string newFtpFolderName = ftpFolderName + "\\" + dirInfo.Name;
                MakeDir(newFtpFolderName, ftpServerIP, ftpUserName, ftpPwd);
                CopyCurrentDirectoryFile(dirInfo.FullName, newFtpFolderName, ftpServerIP, ftpUserName, ftpPwd);
                CopyAllSubDirectoryFile(dirInfo.FullName, dirInfo.Name, newFtpFolderName, ftpServerIP, ftpUserName, ftpPwd, notCopyFlag);
            }
        }

        public static bool CheckDirExists(string ftpfilePath, string ftpServerIP, string ftpUserName, string ftpPwd)
        {
            string dirName = ftpfilePath.Replace("/", "\\").Substring(ftpfilePath.Replace("/", "\\").LastIndexOf("\\") + 1);
            ftpfilePath = ftpfilePath.Replace("/", "\\").Substring(0, ftpfilePath.Replace("/", "\\").LastIndexOf("\\"));
            string requestURL = "ftp://" + ftpServerIP + "/" + ftpfilePath.Replace("\\", "/");
            var request = (FtpWebRequest)WebRequest.Create(new Uri(requestURL.Trim()));
            request.Credentials = new NetworkCredential(ftpUserName, ftpPwd);
            request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
            try
            {
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();

                StreamReader ftpStream = new StreamReader(response.GetResponseStream());
                List<string> dirs = new List<string>();
                string line = ftpStream.ReadLine();
                while (line != null)
                {
                    if (line.Contains("<DIR>"))
                    {
                        string[] fileInfo = line.Split(new string[] { "<DIR>" }, StringSplitOptions.RemoveEmptyEntries);
                        string dir = fileInfo[1].Trim();
                        dirs.Add(dir);
                    }
                    line = ftpStream.ReadLine();
                }
                ftpStream.Close();
                response.Close();

                return dirs.Contains(dirName);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public static string[] GetDirList(string ftpServerIP, string ftpUserID, string ftpPassword, string dirName)
        {
            MakeDir(dirName, ftpServerIP, ftpUserID, ftpPassword);

            StringBuilder result = new StringBuilder(); FtpWebRequest reqFTP;
            try
            {
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://" + ftpServerIP + "/" + dirName));
                reqFTP.UseBinary = true;
                reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
                reqFTP.Method = WebRequestMethods.Ftp.ListDirectory;
                reqFTP.Timeout = timeOut;
                WebResponse response = reqFTP.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream());
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
                response.Close();
                return result.ToString().Split('\n');
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw ex;
            }
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
                reqFTP.UseBinary = true;
                reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
                reqFTP.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
                WebResponse response = reqFTP.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream());
                string line = reader.ReadLine();
                while (line != null)
                {
                    if (line.Contains(file)) return dirName + "/" + file;
                    else
                        line = reader.ReadLine();
                }
                reader.Close();
                response.Close();

                return "";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
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
            FtpWebRequest reqFTP;
            // dirName = name of the directory to create.
            reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://" + ftpServerIP + targetPath));
            reqFTP.Method = WebRequestMethods.Ftp.ListDirectory;
            reqFTP.UseBinary = true;
            reqFTP.Credentials = new NetworkCredential(strftpUserID, strftpPassword);
            FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
            StreamReader ftpStream = new StreamReader(response.GetResponseStream());
            List<string> files = new List<string>();
            string line = ftpStream.ReadLine();
            while (line != null)
            {
                files.Add(line);
                line = ftpStream.ReadLine();
            }
            ftpStream.Close();
            response.Close();
            return false;
        }


        /// <summary>    
        ///  删除指定文件  
        /// </summary> 
        /// <param name="fileName">远程文件的路径</param> 
        /// 
        public static string DeleteFileName(string ftpServerIP, string ftpUserID, string ftpPassword, string targetPath, string targetFile)
        {
            FtpWebRequest reqFTP;
            try
            {
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://" + ftpServerIP + "/" + targetPath + "/" + targetFile));
                reqFTP.Timeout = timeOut;

                reqFTP.KeepAlive = false;

                reqFTP.Method = WebRequestMethods.Ftp.DeleteFile;
                reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                response.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return "";
        }

        public static string DeleteDir(string ftpServerIP, string ftpUserID, string ftpPassword, string targetPath)
        {
            DeleteSubDir(ftpServerIP, ftpUserID, ftpPassword, targetPath);
            DeleteAllFiles(ftpServerIP, ftpUserID, ftpPassword, targetPath);
            DeleteCurrentDir(ftpServerIP, ftpUserID, ftpPassword, targetPath);

            return "";
        }

        private static void DeleteCurrentDir(string ftpServerIP, string ftpUserID, string ftpPassword, string targetPath)
        {
            try
            {
                FtpWebRequest reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://" + ftpServerIP + "/" + targetPath));
                reqFTP.Timeout = timeOut;

                reqFTP.KeepAlive = false;

                reqFTP.Method = WebRequestMethods.Ftp.RemoveDirectory;
                reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                response.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static void DeleteAllFiles(string ftpServerIP, string ftpUserID, string ftpPassword, string targetPath)
        {
            try
            {
                FtpWebRequest reqFTP;
                string url = "ftp://" + ftpServerIP + "/" + targetPath;
                url = url.Replace("\\", "/");
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(url));
                reqFTP.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
                reqFTP.UseBinary = true;
                reqFTP.Timeout = timeOut;
                reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                StreamReader ftpStream = new StreamReader(response.GetResponseStream());
                List<string> files = new List<string>();
                string line = ftpStream.ReadLine();
                while (line != null)
                {
                    if (!line.Contains("<DIR>"))
                    {
                        string[] fileInfo = line.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                        string file = fileInfo[fileInfo.Length - 1];
                        files.Add(file);
                    }
                    line = ftpStream.ReadLine();
                }
                ftpStream.Close();
                response.Close();

                foreach (string file in files)
                {
                    url = "ftp://" + ftpServerIP + "/" + targetPath.Replace("\\", "/") + "/" + file;
                    FtpWebRequest deletereqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(url));
                    deletereqFTP.Timeout = timeOut;

                    deletereqFTP.KeepAlive = false;

                    deletereqFTP.Method = WebRequestMethods.Ftp.DeleteFile;
                    deletereqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
                    FtpWebResponse deleteResponse = (FtpWebResponse)deletereqFTP.GetResponse();
                    deleteResponse.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static void DeleteSubDir(string ftpServerIP, string ftpUserID, string ftpPassword, string targetPath)
        {
            FtpWebRequest reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://" + ftpServerIP + "/" + targetPath));
            reqFTP.Timeout = timeOut;

            reqFTP.KeepAlive = false;

            reqFTP.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
            reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
            FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
            StreamReader ftpStream = new StreamReader(response.GetResponseStream());
            List<string> dirs = new List<string>();
            string line = ftpStream.ReadLine();
            while (line != null)
            {
                if (line.Contains("<DIR"))
                {
                    string dir = line.Split(new string[] { "<DIR>" }, StringSplitOptions.RemoveEmptyEntries)[1].Trim();
                    dirs.Add(dir);
                }
                line = ftpStream.ReadLine();
            }
            ftpStream.Close();
            response.Close();

            foreach (string dir in dirs)
            {
                DeleteAllFiles(ftpServerIP, ftpUserID, ftpPassword, targetPath + "/" + dir);
                DeleteSubDir(ftpServerIP, ftpUserID, ftpPassword, targetPath + "/" + dir);
                DeleteCurrentDir(ftpServerIP, ftpUserID, ftpPassword, targetPath + "/" + dir);
            }
        }
    }
}