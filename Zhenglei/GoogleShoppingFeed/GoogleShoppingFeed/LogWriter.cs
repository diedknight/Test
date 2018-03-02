using System;
using System.IO;

namespace GoogleShoppingFeed
{
    public class LogWriter : IDisposable
    {
        public string FilePath
        {
            get;
            private set;
        }

        StreamWriter sw;
        public LogWriter(string filePath)
        {
            FilePath = filePath;

            sw = new StreamWriter(filePath, true);
        }

        public void WriteLine(string line)
        {
            sw.WriteLine(line);
            sw.Flush();
        }

        bool isDispose = false;
        public void Dispose()
        {
            if (!isDispose)
            {
                sw.Close();
                isDispose = true;
            }
        }
    }
}