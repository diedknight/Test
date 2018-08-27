using System;
using System.IO;
using System.Text;

namespace ProductSearchIndexBuilder
{
    public class LogWriter : IDisposable
    {
        FileStream fileStream;
        StreamWriter sw;
        public LogWriter(string filePath)
        {
            FileInfo fileInfo = new FileInfo(filePath);
            if (!fileInfo.Exists)
            {
                var dir = fileInfo.Directory;
                if (!dir.Exists)
                {
                    dir.Create();
                }
                File.Create(filePath).Close();
            }
            fileStream = new FileStream(filePath, FileMode.OpenOrCreate);
            sw = new StreamWriter(fileStream, Encoding.UTF8);
        }

        public void WriteLine(string info)
        {
            sw.WriteLine(info);
            sw.Flush();
        }

        #region IDisposable 成员

        public void Dispose()
        {
            sw.Dispose();
            fileStream.Dispose();
        }

        #endregion
    }
}