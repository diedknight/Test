using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckRetailerReviewEmail
{
    class Program
    {
        static void Main(string[] args)
        {
            string logpath = System.Configuration.ConfigurationManager.AppSettings["LogPath"];
            CreateDir(logpath);
            StreamWriter sw = new StreamWriter(logpath + "\\" + DateTime.Now.ToString("yyyyMMddHH") + ".txt");

            RetailerReviews r = new RetailerReviews();
            r.SW = sw;
            r.Check();
        }

        private static void CreateDir(string path)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }
    }
}
