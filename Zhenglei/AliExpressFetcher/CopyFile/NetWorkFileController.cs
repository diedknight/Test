using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CopyFile
{
    public class NetWorkFileController
    {
        public static void Write(string targetIP, string targetFilePath, string userID, string password)
        {
            string copyPath = @"\\" + targetIP + @"\" + targetFilePath;
            //调用;指定共享机器上的用户名,机器名,密码,(不在域控中的.)  
            using (IdentityScope c = new IdentityScope(userID, targetIP, password))
            {
                System.IO.StreamWriter sw = System.IO.File.AppendText(targetFilePath);
                sw.Write('D');
                sw.Flush();
                sw.Close();
            }
        }
    }
}