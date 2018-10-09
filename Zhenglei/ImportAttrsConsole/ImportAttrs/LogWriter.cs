using ImportAttrs.Data;
using PriceMeCrawlerTask.Common.Log;
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

        private Log _log = null;

        //StreamWriter sw;
        public LogWriter(string filePath)
        {
            FilePath = filePath;

            this._log = new Log(filePath);
            

            //sw = new StreamWriter(filePath, true);
        }

        public void WriteLine(string line)
        {
            WriteLine(line, false);
        }

        public void WriteLine(UnmatchReportData data)
        {
            WriteLine(" ID : " + data.ID + " RId : " + data.RID + " CId : " + data.CID + " PId : " + data.PID + " AttType : " + data.AttType + " AttTitleID : " + data.AttTitleID + " PM_AttName : " + data.PM_AttName + " DR_AttName : " + data.DR_AttName + " DR_AttValue_Orignal : " + data.DR_AttValue_Orignal + " DR_AttValue_Changed : " + data.DR_AttValue_Changed, false);
        }

        public void WriteLine(string line, bool addTime)
        {
            if(addTime)
            {
                line += " --- at : " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            }
            Console.WriteLine(line);

            this._log.WriteLine(line);
            //sw.WriteLine(line);
            //sw.Flush();
        }

        bool isDispose = false;
        public void Dispose()
        {
            if (!isDispose)
            {
                //sw.Close();
                isDispose = true;
            }
        }
    }
}
