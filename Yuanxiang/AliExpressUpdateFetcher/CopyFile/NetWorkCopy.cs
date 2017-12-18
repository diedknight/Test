using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CopyFile
{
    public static class NetWorkCopy
    {
        public static bool Copy(string targetIP, string targetPath, string userID, string password, string localPath, string notCopyFlag)
        {
            return Copy(targetIP, targetPath, userID, password, localPath, notCopyFlag, true);
        }

        public static bool Copy(string targetIP, string targetPath, string userID, string password, string localPath, string notCopyFlag, bool coverDir)
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
                else if (!coverDir)
                {
                    return false;
                }
                CopyController.CopyAllSubDirectoryFile(localPath, copyPath + @"\" + dirInfo.Name, notCopyFlag);
                //在此就可以访问了.  
            }

            return true;
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
    }
}
