using AliexpressImport.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AliexpressDBA;
using AliexpressImport.BusinessLogic;
using System.Diagnostics;
using System.IO;

namespace AliexpressImport
{
    public class ImportProduct
    {
        int categoryid;
        DateTime startTime;
        int newcount = 0;
        int updatecount = 0;

        public object ProcessFeed(object messageObj)
        {
            OutManagerContronller.WriterInfo(TraceEventType.Verbose, "Begin..." + DateTime.Now);
            startTime = DateTime.Now;

            CrawlerParameter parameter = (CrawlerParameter)messageObj;
            string feedFile = parameter.FilePath;
            OutManagerContronller.WriterInfo(TraceEventType.Verbose, "FilePath: " + feedFile);
            
            //解压
            if (File.Exists(feedFile + ".gz"))
            {
                FSuite.FZip.Unzip(feedFile + ".gz", feedFile.Substring(0, feedFile.LastIndexOf('\\')), string.Empty);
            }
            if (!File.Exists(feedFile))
            {
                OutManagerContronller.WriterInfo(TraceEventType.Error, "Unable to locate feed file : " + feedFile + " " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                return false;
            }

            List<ProductInfoEntity> products = FeedContronller.GetProductInfoEntity(feedFile);
            OutManagerContronller.WriterInfo(TraceEventType.Verbose, "Get " + products.Count + " products in feedfile.");

            if (products.Count > 0)
            {
                CategoryData cate = CategoryContronller.listCategory.SingleOrDefault(c => c.CategoryName == products[0].Category);
                if (cate != null)
                    categoryid = cate.CategoryId;
            }

            foreach (ProductInfoEntity p in products)
            {
                try
                {
                    int type = ProductContronller.ProductMatching(p);
                    if (type == 1)
                        newcount++;
                }
                catch (Exception ex)
                {
                    OutManagerContronller.WriterInfo(TraceEventType.Verbose, "Error: " + p.ProductName + "  " + ex.Message + ex.StackTrace);
                }
            }

            string report = startTime.ToString("yyyy-MM-dd HH:mm:ss") + " \t " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " \t " + products.Count + " \t " + updatecount + " \t " + newcount;
            OutManagerContronller.WriterReport(report);
            
            return true;
        }
    }
}
