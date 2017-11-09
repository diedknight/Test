using System;
using System.IO;

namespace ImportAttrs
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
            WriteLine(line, false);
        }

        public void WriteLine(string line, bool addTime)
        {
            if(addTime)
            {
                line += " --- at : " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            }
            Console.WriteLine(line);
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
