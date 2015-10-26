using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Common.Data;

namespace Common
{
    public class FileLogWriter : IDisposable
    {
        StreamWriter sw = null;

        public FileLogWriter(string filePath, FileMode fileMode)
        {
            FileStream fs = File.Open(filePath, fileMode);

            sw = new StreamWriter(fs);
        }

        public void WriteLine(string line)
        {
            sw.WriteLine(line);
        }

        public void Flush()
        {
            sw.Flush();
        }

        public void Dispose()
        {
            if (sw != null)
                sw.Dispose();
        }
    }
}