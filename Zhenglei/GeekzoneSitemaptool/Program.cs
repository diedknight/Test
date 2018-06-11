using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Pricealyser.SiteMap
{
    class Program
    {
        static void Main(string[] args)
        {
            PriceMeCommon.BusinessLogic.MultiCountryController.LoadWithoutCheckIndexPath();
            PriceMeCommon.BusinessLogic.CategoryController.Load(null);

            System.Diagnostics.Process.GetCurrentProcess().PriorityClass = System.Diagnostics.ProcessPriorityClass.BelowNormal;

            string date = DateTime.Now.ToString("yyyy MM dd");

            string path = System.Configuration.ConfigurationManager.AppSettings["LogPath"].ToString();

            CreateDir(path);
            StreamWriter sw = new StreamWriter(path + DateTime.Now.ToString("yyyyMMdd HH") + ".txt");

            SiteMapListing sml = new SiteMapListing();
            sml.SW = sw;
            sml.SetSiteMapListing();

            sw.Close();

            PriceMeCommon.BusinessLogic.MultiCountryController.AllLuceneSearcherInfo_Static.EndCheck();
        }

        private static void CreateDir(string path)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }
    }
}
