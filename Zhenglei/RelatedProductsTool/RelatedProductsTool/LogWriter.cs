using System;
using System.IO;

namespace RelatedProductsTool
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
            Console.WriteLine(line + " --- at : " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            sw.WriteLine(line + " --- at : " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
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
