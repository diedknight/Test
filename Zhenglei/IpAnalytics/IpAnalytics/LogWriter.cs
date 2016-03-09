using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IpAnalytics
{
    public class LogWriter : IDisposable
    {
        public string LogFilePath { get; private set; }

        System.IO.StreamWriter _streamWriter;
        public LogWriter(string logFilePath)
        {
            LogFilePath = logFilePath;
            _streamWriter = new System.IO.StreamWriter(logFilePath, false);
        }

        object locker = new object();
        public void WriteLog(string msg)
        {
            if (!isDispose)
            {
                lock (locker)
                {
                    _streamWriter.WriteLine(msg);
                }
            }
        }

        bool isDispose = false;
        public void Dispose()
        {
            if (!isDispose)
            {
                isDispose = true;
                _streamWriter.Dispose();
            }
        }
    }
}
