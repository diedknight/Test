using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ExpertReviewIndex
{
    public static class BuildIndexLog
    {
        static StreamWriter sw = null;

        public static void Load()
        {
            string date = DateTime.Now.ToString("yyyyMMddHH");
            string dir = SiteConfig.AppSettings("LogDirectory");
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            sw = new StreamWriter(dir + date + ".txt");
        }

        public static void WriterLog(string info)
        {
            if (sw == null) Load();

            sw.WriteLine(info);
            sw.Flush();
        }

        public static void CloseLog()
        {
            if (sw != null)
                sw.Close();
        }
    }
}
