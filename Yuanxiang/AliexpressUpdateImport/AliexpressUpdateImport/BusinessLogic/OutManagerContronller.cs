using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;

namespace AliexpressImport.BusinessLogic
{
    public static class OutManagerContronller
    {
        static TraceSource _outInfo;
        static StreamWriter _outReport;

        public static void Load()
        {
            string _directory = ConfigAppString.LogLocation;
            string _currentDate = DateTime.Now.ToString("yyyyMMdd_HH") + ".txt";

            if (!Directory.Exists(_directory + @"\CrawlInfo"))
                Directory.CreateDirectory(_directory + @"\CrawlInfo");
            if (!Directory.Exists(_directory + @"\CrawlReport"))
                Directory.CreateDirectory(_directory + @"\CrawlReport");

            _outInfo = new TraceSource("Aliexpress Import");
            TextWriterTraceListener listener = new TextWriterTraceListener(_directory + @"\CrawlInfo\" + _currentDate, "txt");
            _outInfo.Listeners.Add(listener);

            FileStream _reportFile = File.Open(_directory + @"\CrawlReport\" + _currentDate, FileMode.Create, FileAccess.Write, FileShare.Read);
            _outReport = new StreamWriter(_reportFile, Encoding.Default);
            string crawlReportTitle = "Start time  \tEnd time    \tCrawled\tupdate\tnew";
            _outReport.WriteLine(crawlReportTitle);
            _outReport.Flush();
        }

        public static void WriterInfo(TraceEventType traceTryp, string info)
        {
            _outInfo.TraceEvent(traceTryp, 0, info);
        }
        
        public static void WriterReport(string info)
        {
            _outReport.WriteLine(info);
            _outReport.Flush();
        }

        public static void Close()
        {
            if (_outInfo != null)
                _outInfo.Close();
            if (_outReport != null)
                _outReport.Close();
        }
    }
}
