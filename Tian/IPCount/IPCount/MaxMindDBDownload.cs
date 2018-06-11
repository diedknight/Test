using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.GZip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace IpAnalytics
{
    public class MaxMindDBDownload
    {
        public static void Download()
        {
            DateTime pointDay = DateTime.MinValue;
            DateTime firstDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            int dayofWeek = Convert.ToInt32(firstDay.DayOfWeek) == 0 ? 7 : Convert.ToInt32(firstDay.DayOfWeek);

            if (dayofWeek == 3) pointDay = firstDay;
            if (dayofWeek < 3) pointDay = firstDay.AddDays(3 - dayofWeek);
            if (dayofWeek > 3) pointDay = firstDay.AddDays(7 - dayofWeek + 3);

            //当天不是每个月的第一个星期三就不下载
            if ((DateTime.Now - pointDay).Days != 0) return;

            Console.WriteLine("downloading maxmind db");

            string url = "http://geolite.maxmind.com/download/geoip/database/GeoLite2-Country.mmdb.gz";
            string path = Path.Combine(Environment.CurrentDirectory, "GeoLite2-Country.mmdb");
                        
            using (WebClient w = new WebClient())
            {                
                using (MemoryStream ms = new MemoryStream(w.DownloadData(url)))
                {
                    using (GZipInputStream gs = new GZipInputStream(ms))
                    {                        
                        using (FileStream fs = File.Create(path))
                        {
                            byte[] buf = new byte[4096];
                            StreamUtils.Copy(gs, fs, buf);                            
                        }
                    }
                }
            }

        }
    }
}
