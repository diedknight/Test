using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;

namespace Fetcher
{
    public static class FetcherController
    {
        public static void WriteToFile<T>(List<T> infoList, string filePath) where T : Common.Data.ICrawlDataInfo<T>
        {
            using (FileLogWriter fileLogWriter = new FileLogWriter(filePath, System.IO.FileMode.Append))
            {
                fileLogWriter.WriteLine("Carrier\tPlanName\tPlanURL\tPrice\tDataMB\tMinutes\tTexts\tPhonesCount");
                foreach (var t in infoList)
                {
                    fileLogWriter.WriteLine(t.ToTextLine());
                }
                fileLogWriter.WriteLine("---------------------------------------------------------------------");
                fileLogWriter.Flush();
            }
        }
        public static void WritePhonesToFile<T>(List<T> infoList, string filePath) where T : Common.Data.ICrawlDataInfo<T>
        {
            using (FileLogWriter fileLogWriter = new FileLogWriter(filePath, System.IO.FileMode.Append))
            {
                fileLogWriter.WriteLine("\r\nPlan\tPhoneName\tContractTypeID\tUpfrontPrice\tPhoneURL");
                foreach (var t in infoList)
                {
                    var str = t.WritePhones();
                    if (!string.IsNullOrEmpty(str))
                        fileLogWriter.WriteLine(str);
                }
                fileLogWriter.WriteLine("---------------------------------------------------------------------");
                fileLogWriter.Flush();
            }
        }

        public static void WriteFetcherCrawlUrl(string filePath, List<string> urls)
        {
            using (FileLogWriter fileLogWriter = new FileLogWriter(filePath, System.IO.FileMode.Create))
            {
                foreach (var u in urls)
                {
                    fileLogWriter.WriteLine(u);
                }
                fileLogWriter.Flush();
            }
        }
    }
}