using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AliExpressFetcher
{
    class Program
    {
        static void Main(string[] args)
        {
            string timeStr = DateTime.Now.ToString("yyyy_MM_dd HH_mm");
            string logPath = Path.Combine(System.Configuration.ConfigurationManager.AppSettings["LogRootPath"], timeStr + ".txt");
            //string feedPath = Path.Combine(System.Configuration.ConfigurationManager.AppSettings["FeedRootPath"], timeStr + ".xml");
            string feedPath = Path.Combine(System.Configuration.ConfigurationManager.AppSettings["FeedRootPath"], timeStr + ".csv");
            string chromeWebDriverDir = System.Configuration.ConfigurationManager.AppSettings["ChromeWebDriverDir"];
            string aliexpressMapFile = System.Configuration.ConfigurationManager.AppSettings["MapFile"];
            string account = System.Configuration.ConfigurationManager.AppSettings["Account"];
            string password = System.Configuration.ConfigurationManager.AppSettings["Password"];
            string country = System.Configuration.ConfigurationManager.AppSettings["Country"];
            string currency = System.Configuration.ConfigurationManager.AppSettings["Currency"];

            List<CategoryInfo> categoryInfoList = GetCategoryInfoList(aliexpressMapFile);

            AliExpressCrawler aliExpressCrawler = new AliExpressCrawler(account, password, country, currency, chromeWebDriverDir);
            List<ProductInfo> productInfoList = aliExpressCrawler.CrawlProducts(categoryInfoList);

            //WriteXmlFile(productInfoList, feedPath);
            WriteCsvFile(productInfoList, feedPath);
        }

        private static void WriteCsvFile(List<ProductInfo> productInfoList, string feedPath)
        {
            int maxImageCount = productInfoList.Max(p => p.Images.Count);
            using (StreamWriter sw = new StreamWriter(feedPath, false))
            {
                sw.WriteLine(ProductInfo.ToCsvHeaderString(maxImageCount));

                foreach (var pi in productInfoList)
                {
                    sw.WriteLine(pi.ToCsvString());
                }
            }
        }

        private static void WriteXmlFile(List<ProductInfo> productInfoList, string feedPath)
        {
            using (StreamWriter sw = new StreamWriter(feedPath, false))
            {
                sw.WriteLine("<xml encoding=\"utf-8\">");
                sw.WriteLine("<Products>");

                foreach(var pi in productInfoList)
                {
                    sw.WriteLine(pi.ToXmlString());
                }

                sw.WriteLine("</Products>");
                sw.WriteLine("</xml>");
            }
        }

        private static List<CategoryInfo> GetCategoryInfoList(string aliexpressMapFile)
        {
            List<CategoryInfo> list = new List<CategoryInfo>();

            using (StreamReader sr = new StreamReader(aliexpressMapFile))
            {
                string line = sr.ReadLine();

                while (!string.IsNullOrEmpty(line))
                {
                    string[] infos = line.Split(new string[] { "\t" }, StringSplitOptions.RemoveEmptyEntries);
                    if (infos.Length > 1)
                    {
                        CategoryInfo ci = new CategoryInfo();
                        ci.CategoryName = infos[0].Trim();
                        ci.CategoryUrl = infos[1].Trim();
                        list.Add(ci); 
                    }

                    line = sr.ReadLine();
                }
            }

            return list;
        }
    }
}
