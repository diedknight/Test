using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Common
{
    public class ImportCarrierLog
    {
        StreamWriter sw = null;

        public ImportCarrierLog(string filePath, FileMode fileMode)
        {
            FileStream fs = File.Open(filePath, fileMode);

            sw = new StreamWriter(fs);
        }

        public void WriteLine(string line)
        {
            sw.WriteLine(line);
            sw.Flush();
        }

        public void Close()
        {
            if (sw != null)
                sw.Close();
        }
    }
}
