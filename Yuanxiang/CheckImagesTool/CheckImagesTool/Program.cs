using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckImagesTool
{
    class Program
    {
        static void Main(string[] args)
        {
            IndexController.Compare(@"\Images\ProductImages\StRetailer477\bg42655004610.jpg");

            string logpath = System.Configuration.ConfigurationManager.AppSettings["LogPath"];
            CreateDir(logpath);
            StreamWriter sw = new StreamWriter(logpath + "\\" + DateTime.Now.ToString("yyyyMMddHH") + ".txt");

            CheckImages ci = new CheckImages();
            ci.SW = sw;
            ci.Check();
        }

        private static void CreateDir(string path)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }
    }
}
