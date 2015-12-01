using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace CopyFile
{
    public static class NetWorkCopy
    {
        public static void Copy(string targetIP, string targetPath, string userID, string password, string localPath, string notCopyFlag)
        {
            string copyPath = @"\\" + targetIP + @"\" + targetPath;
            //调用;指定共享机器上的用户名,机器名,密码,(不在域控中的.)  
            using (IdentityScope c = new IdentityScope(userID, targetIP, password))
            {
                System.IO.DirectoryInfo dirInfo = new System.IO.DirectoryInfo(localPath);
                if (!System.IO.Directory.Exists(copyPath + @"\" + dirInfo.Name))
                {
                    Console.WriteLine("Begin CreateDirectory : " + copyPath + @"\" + dirInfo.Name);
                    System.IO.Directory.CreateDirectory(copyPath + @"\" + dirInfo.Name);
                    Console.WriteLine("CreateDirectory : " + copyPath + @"\" + dirInfo.Name + " successful!");
                }
                CopyController.CopyAllSubDirectoryFile(localPath, copyPath + @"\" + dirInfo.Name, notCopyFlag);
                //在此就可以访问了.  
            }
        }

        public static void CopyFile(string targetIP, string targetPath, string userID, string password, string filePath, string fileName)
        {
            string copyPath = @"\\" + targetIP + @"\" + targetPath;
            //调用;指定共享机器上的用户名,机器名,密码,(不在域控中的.)  
            using (IdentityScope c = new IdentityScope(userID, targetIP, password))
            {
                if (!System.IO.Directory.Exists(copyPath))
                {
                    Console.WriteLine("Begin CreateDirectory : " + copyPath);
                    System.IO.Directory.CreateDirectory(copyPath);
                    Console.WriteLine("CreateDirectory : " + copyPath + " successful!");
                }
                CopyController.CopyCurrentFile(filePath, copyPath + @"\" + fileName);
                //在此就可以访问了.  
            }
        }
        
        /// <summary>
        /// 从targetIP复制文件夹到localPath
        /// </summary>
        /// <param name="targetIP"></param>
        /// <param name="targetPath"></param>
        /// <param name="userID"></param>
        /// <param name="password"></param>
        /// <param name="localPath"></param>
        /// <param name="notCopyFlag"></param>
        /// <returns></returns>
        public static bool CopyFrom(string targetIP, string targetPath, string userID, string password, string localPath, string notCopyFlag)
        {
            string fromPath = @"\\" + targetIP + @"\" + targetPath;
            //调用;指定共享机器上的用户名,机器名,密码,(不在域控中的.)  
            using (IdentityScope c = new IdentityScope(userID, targetIP, password))
            {
                System.IO.DirectoryInfo targetdirInfo = new System.IO.DirectoryInfo(fromPath);
                if (System.IO.Directory.Exists(fromPath))//判断目标文件夹是否存在
                {
                    //判断本地文件夹是否存在，不存在则创建
                    if (!System.IO.Directory.Exists(localPath + @"\" + targetdirInfo.Name))
                    {
                        Console.WriteLine("Begin CreateDirectory : " + localPath + @"\" + targetdirInfo.Name);
                        System.IO.Directory.CreateDirectory(localPath + @"\" + targetdirInfo.Name);
                        Console.WriteLine("CreateDirectory : " + localPath + @"\" + targetdirInfo.Name + " successful!");
                    }

                    CopyController.CopyAllSubDirectoryFile(fromPath, localPath + @"\" + targetdirInfo.Name, notCopyFlag);
                }
                else//不存在则报错
                {
                    Console.WriteLine("Target folder isn't exist!");
                    return false;
                }
                return true;
                //在此就可以访问了.  
            }
        }

        /// <summary>
        /// 从targetIP复制文件到localPath
        /// </summary>
        /// <param name="targetIP"></param>
        /// <param name="targetPath"></param>
        /// <param name="userID"></param>
        /// <param name="password"></param>
        /// <param name="localPath"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        public static bool CopySpecifiedFileFrom(string targetIP, string targetPath, string userID, string password, string localPath, string file)
        {
            string fromPath = @"\\" + targetIP + @"\" + targetPath;
            //调用;指定共享机器上的用户名,机器名,密码,(不在域控中的.)  
            using (IdentityScope c = new IdentityScope(userID, targetIP, password))
            {
                System.IO.DirectoryInfo targetdirInfo = new System.IO.DirectoryInfo(fromPath);
                if (System.IO.Directory.Exists(fromPath))//判断目标文件夹是否存在
                {
                    //判断本地文件夹是否存在，不存在则创建
                    if (!System.IO.Directory.Exists(localPath + @"\" + targetdirInfo.Name))
                    {
                        Console.WriteLine("Begin CreateDirectory : " + localPath + @"\" + targetdirInfo.Name);
                        System.IO.Directory.CreateDirectory(localPath + @"\" + targetdirInfo.Name);
                        Console.WriteLine("CreateDirectory : " + localPath + @"\" + targetdirInfo.Name + " successful!");
                    }

                    CopyController.CopySpecifiedFile(fromPath, localPath + @"\" + targetdirInfo.Name, file);
                }
                else//不存在则报错
                {
                    Console.WriteLine("Target folder isn't exist!");
                    return false;
                }
                return true;
                //在此就可以访问了.  
            }
        }

        public static void CopyCurrentFile(string targetIP, string targetFilePath, string userID, string password, string localFilePath)
        {
            string str = @"\\" + targetIP + @"\" + targetFilePath;
            using (new IdentityScope(userID, targetIP, password))
            {
                CopyController.CopyCurrentFile(localFilePath, str);
            }
        }

        public static void CopyCurrentDirectory(string targetIP, string targetPath, string userID, string password, string localPath)
        {
            string path = @"\\" + targetIP + @"\" + targetPath;
            using (new IdentityScope(userID, targetIP, password))
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
            }
        }

        /// <summary>
        /// 从targetIP复制文件夹里面指定的文件到localPath
        /// </summary>
        /// <param name="targetIP"></param>
        /// <param name="targetPath"></param>
        /// <param name="userID"></param>
        /// <param name="password"></param>
        /// <param name="localPath"></param>
        /// <param name="notCopyFlag"></param>
        /// <returns></returns>
        public static bool CopyFromDesignationFile(string targetIP, string targetPath, string userID, string password, string localPath, string copyFlag, int targetString, StreamWriter sw)
        {
            string fromPath = @"\\" + targetIP + @"\" + targetPath;
            //调用;指定共享机器上的用户名,机器名,密码,(不在域控中的.)  
            using (IdentityScope c = new IdentityScope(userID, targetIP, password))
            {
                System.IO.DirectoryInfo targetdirInfo = new System.IO.DirectoryInfo(fromPath);
                if (System.IO.Directory.Exists(fromPath))//判断目标文件夹是否存在
                {
                    if (targetString == 206)
                    {
                        Dictionary<string, List<DateTime>> fileDic = new Dictionary<string, List<DateTime>>();
                        System.IO.FileInfo[] files = targetdirInfo.GetFiles();
                        foreach (FileInfo file in files)
                        {
                            if (file.Name.Contains(copyFlag))
                            {
                                string[] temps = file.Name.Split(new string[] { "-20" }, StringSplitOptions.None);
                                if (!fileDic.ContainsKey(temps[0]))
                                {
                                    List<DateTime> dateList = new List<DateTime>();
                                    dateList.Add(file.CreationTime);
                                    fileDic.Add(temps[0], dateList);
                                }
                                else
                                {
                                    List<DateTime> dateList = fileDic[temps[0]];
                                    dateList.Add(file.CreationTime);
                                    fileDic[temps[0]] = dateList;
                                }
                            }
                        }
                        foreach (var pair in fileDic)
                        {
                            try
                            {
                                pair.Value.Sort();
                                string fileName = pair.Key + "-" + pair.Value[pair.Value.Count - 1].ToString("yyyyMd") + ".bak";
                                sw.WriteLine("File name: " + fileName);
                                sw.Flush();
                                CopyController.CopySpecifiedFile(fromPath, localPath, fileName);
                            }
                            catch (Exception ex) {
                                sw.WriteLine("206 error: " + ex.Message + ex.StackTrace);
                                sw.Flush();
                                Console.WriteLine("206 error: " + ex.Message + ex.StackTrace); }
                        }
                    }
                    else if (targetString == 149)
                    {
                        try
                        {
                            DirectoryInfo dataDir = new DirectoryInfo(fromPath);
                            FileInfo[] dataInfos = dataDir.GetFiles();
                            Dictionary<string, DateTime> dataDic = new Dictionary<string, DateTime>();
                            foreach (FileInfo info in dataInfos)
                            {
                                if (info.Name.Contains(".bak"))
                                {
                                    if (!dataDic.ContainsKey(info.Name))
                                        dataDic.Add(info.Name, info.CreationTime);
                                }
                            }
                            var result = from pair in dataDic orderby pair.Value descending select pair;
                            string dataFileName = string.Empty;
                            foreach (KeyValuePair<string, DateTime> pair in result)
                            {
                                dataFileName = pair.Key;
                                break;
                            }
                            sw.WriteLine("File name: " + dataFileName);
                            sw.Flush();
                            CopyController.CopySpecifiedFile(fromPath, localPath, dataFileName);
                        }
                        catch (Exception ex) {
                            sw.WriteLine("149 error: " + ex.Message + ex.StackTrace);
                            sw.Flush();
                            Console.WriteLine("149 error: " + ex.Message + ex.StackTrace); }
                    }
                }
                else//不存在则报错
                {
                    sw.WriteLine("Target folder isn't exist!" + fromPath);
                    sw.Flush();
                    Console.WriteLine("Target folder isn't exist!" + fromPath);
                    return false;
                }
                return true;
                //在此就可以访问了.  
            }
        }

        /// <summary>
        /// 从targetIP文件夹里面删除文件
        /// </summary>
        /// <param name="targetIP"></param>
        /// <param name="targetPath"></param>
        /// <param name="userID"></param>
        /// <param name="password"></param>
        /// <param name="localPath"></param>
        /// <param name="notCopyFlag"></param>
        /// <returns></returns>
        public static bool DeleteFromDesignationFile(string targetIP, string targetPath, string userID, string password, string deleteFlag)
        {
            string fromPath = @"\\" + targetIP + @"\" + targetPath;
            //调用;指定共享机器上的用户名,机器名,密码,(不在域控中的.)  
            using (IdentityScope c = new IdentityScope(userID, targetIP, password))
            {
                System.IO.DirectoryInfo targetdirInfo = new System.IO.DirectoryInfo(fromPath);
                if (System.IO.Directory.Exists(fromPath))//判断目标文件夹是否存在
                {
                    System.IO.FileInfo[] files = targetdirInfo.GetFiles();
                    foreach (FileInfo file in files)
                    {
                        if (file.Name.Contains(deleteFlag))
                            File.Delete(file.FullName);
                    }
                }
                else//不存在则报错
                {
                    Console.WriteLine("Target folder isn't exist!" + fromPath);
                    return false;
                }
                return true;
                //在此就可以访问了.  
            }
        }

        public static bool DeleteSiteMapRAR(string targetIP, string targetPath, string userID, string password, int count)
        {
            string fromPath = @"\\" + targetIP + @"\" + targetPath;
            //调用;指定共享机器上的用户名,机器名,密码,(不在域控中的.)  
            using (IdentityScope c = new IdentityScope(userID, targetIP, password))
            {
                System.IO.DirectoryInfo targetdirInfo = new System.IO.DirectoryInfo(fromPath);
                if (System.IO.Directory.Exists(fromPath))//判断目标文件夹是否存在
                {
                    System.IO.FileInfo[] files = targetdirInfo.GetFiles();
                    if (files.Length >= count)
                    {
                        foreach (FileInfo file in files)
                        {
                            if (file.LastWriteTime < DateTime.Today)
                                file.Delete();
                        }
                    }
                }
                else//不存在则报错
                {
                    Console.WriteLine("Target folder isn't exist!" + fromPath);
                    return false;
                }
                return true;
                //在此就可以访问了.  
            }
        }

        public static bool UpdateConfig(string targetIP, string targetPath, string userID, string password, string key)
        {
            string fromPath = @"\\" + targetIP + @"\" + targetPath;
            //调用;指定共享机器上的用户名,机器名,密码,(不在域控中的.)  
            using (IdentityScope c = new IdentityScope(userID, targetIP, password))
            {
                System.IO.FileInfo targetdirInfo = new System.IO.FileInfo(fromPath);
                if (System.IO.File.Exists(fromPath))//判断目标文件夹是否存在
                {
                    CopyController.UpdateWebConfig(fromPath, key);
                }
                else//不存在则报错
                {
                    Console.WriteLine("Target folder isn't exist!" + fromPath);
                    return false;
                }
                return true;
                //在此就可以访问了.  
            }
        }
    }
}
