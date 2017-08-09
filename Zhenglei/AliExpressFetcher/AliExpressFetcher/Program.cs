using MT.Extend;
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
            //CopyAndSetMessage_Test("E:\\Ali1_2017_08_03 14_59.csv.gz");
            //return;

            string timeStr = DateTime.Now.ToString("yyyy_MM_dd HH_mm");
            string feedFilePrefix = System.Configuration.ConfigurationManager.AppSettings["FeedFilePrefix"];
            
            string logPath = Path.Combine(System.Configuration.ConfigurationManager.AppSettings["LogRootPath"], timeStr + ".txt");
            //string feedPath = Path.Combine(System.Configuration.ConfigurationManager.AppSettings["FeedRootPath"], timeStr + ".xml");
            string feedPath = Path.Combine(System.Configuration.ConfigurationManager.AppSettings["FeedRootPath"], feedFilePrefix + "_" + timeStr + ".csv");
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
            CopyAndSetMessage(feedPath);
        }

        private class Consumer : BaseConsumer<MT.Contract.IShopContract>
        {
            protected override void ConsumeHandle(SimpleConsumeContext<MT.Contract.IShopContract> context)
            {
                
            }
        }


        private static void CopyAndSetMessage_Test(string feedPath)
        {
            try
            {
                var bus = new SimpleConsumerBus<Consumer>();
                bus.Start();

                System.Threading.Thread.Sleep(3000);

                bus.Stop();
            }
            catch { }

            SimplePublisherBus publisherBus = new SimplePublisherBus();

            publisherBus.Start();
            var info = new MT.Contract.ShopContract();
            info.Body = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            info.Label = feedPath;
            info.Recoverable = true;
            publisherBus.Publish<MT.Contract.IShopContract>(info);
            System.Threading.Thread.Sleep(10000);
            publisherBus.Stop();
        }

        private static void CopyAndSetMessage(string feedPath)
        {
            //try
            //{
            //    var bus = new SimpleConsumerBus<Consumer>();
            //    bus.Start();

            //    System.Threading.Thread.Sleep(3000);

            //    bus.Stop();
            //}
            //catch { }

            SimplePublisherBus publisherBus = new SimplePublisherBus();

            string outZipFile = feedPath + ".gz";
            FSuite.FZip.Zip(feedPath, outZipFile);

            string targetPathFTP = System.Configuration.ConfigurationManager.AppSettings["targetPathFTP"];
            string targetIPFTP = System.Configuration.ConfigurationManager.AppSettings["targetIPFTP"];
            string userIDFTP = System.Configuration.ConfigurationManager.AppSettings["userIDFTP"];
            string passwordFTP = System.Configuration.ConfigurationManager.AppSettings["passwordFTP"];
            string msgFileDir = System.Configuration.ConfigurationManager.AppSettings["MsgFileDir"];
            
            CopyFile.FtpCopy.UploadFileSmall(outZipFile, targetPathFTP, targetIPFTP, userIDFTP, passwordFTP);

            publisherBus.Start();
            string msgFilePath = Path.Combine(msgFileDir, outZipFile.Substring(outZipFile.LastIndexOf("\\") + 1));
            var info = new MT.Contract.ShopContract();
            info.Body = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            info.Label = outZipFile;
            info.Recoverable = true;
            publisherBus.Publish<MT.Contract.IShopContract>(info);

            System.Threading.Thread.Sleep(10000);
            publisherBus.Stop();
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
