using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Infrastructure.Log
{
    public class XbaiLog
    {
        private static string basePath = AppDomain.CurrentDomain.BaseDirectory + "\\Log\\";
        private static readonly object obj = new object();
        public static void WriteLog(string fileName, string msg)
        {
            lock (XbaiLog.obj)
            {
                FileStream fileStream = null;
                StreamWriter streamWriter = null;
                try
                {
                    string text = DateTime.Now.ToString("yyyy-MM-dd");
                    text = text.Replace('-', '\\') + "\\";
                    text = XbaiLog.basePath + text;
                    string path = text + fileName + ".txt";
                    msg = DateTime.Now + "： " + msg;
                    if (!Directory.Exists(text))
                    {
                        Directory.CreateDirectory(text);
                    }
                    if (!File.Exists(path))
                    {
                        fileStream = File.Create(path);
                    }
                    else
                    {
                        fileStream = File.Open(path, FileMode.Append);
                    }
                    streamWriter = new StreamWriter(fileStream);
                    streamWriter.WriteLine(msg);
                    streamWriter.Close();
                    fileStream.Close();
                    streamWriter.Dispose();
                    fileStream.Dispose();
                }
                catch
                {
                    if (streamWriter != null)
                    {
                        streamWriter.Close();
                        streamWriter.Dispose();
                    }
                    if (fileStream != null)
                    {
                        fileStream.Close();
                        fileStream.Dispose();
                    }
                }
            }
        }
    }
}
