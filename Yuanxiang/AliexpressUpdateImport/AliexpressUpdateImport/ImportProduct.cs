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
        List<string> listCates;

        public object ProcessFeed(object messageObj)
        {
            OutManagerContronller.WriterInfo(TraceEventType.Verbose, "Begin..." + DateTime.Now);
            startTime = DateTime.Now;

            CrawlerParameter parameter = (CrawlerParameter)messageObj;
            string feedFile = parameter.FilePath.Replace(".gz", "");
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

            listCates = new List<string>();
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
                    if (!listCates.Contains(p.Category))
                        listCates.Add(p.Category);

                    int type = ProductContronller.ProductMatching(p);
                    if (type == 1)
                        newcount++;
                    else if (type == 2)
                        updatecount++;
                }
                catch (Exception ex)
                {
                    OutManagerContronller.WriterInfo(TraceEventType.Verbose, "Error: " + p.ProductName + "  " + ex.Message + ex.StackTrace);
                }
            }

            string report = startTime.ToString("yyyy-MM-dd HH:mm:ss") + " \t " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " \t " + products.Count + " \t " + updatecount + " \t " + newcount;
            OutManagerContronller.WriterReport(report);

            foreach (string category in listCates)
            {
                CategoryData cate = CategoryContronller.listCategory.SingleOrDefault(c => c.CategoryName == category);
                if (cate != null)
                {
                    UpdateProportion(cate.CategoryId);
                }
            }

            return true;
        }

        private void UpdateProportion(int categoryid)
        {
            int allcount = ProductContronller.GetAllProductCountByCategory(categoryid);
            int updatecount = ProductContronller.GetUpdateProductCountByCategory(categoryid, startTime);

            string sql = string.Empty;
            decimal rate = (decimal)updatecount / (decimal)allcount;
            if (rate > ConfigAppString.Rate)
                sql = "Update Product Set Deleted = 1, UpdatedOnUtc = '" + startTime.ToString("yyyy-MM-dd HH:mm:ss") + "' Where Deleted = 0 And UpdatedOnUtc < '" + startTime.ToString("yyyy-MM-dd HH:mm:ss") + "' And Id in (Select ProductId From Product_Category_Mapping Where CategoryId = " + categoryid + ")";
            else
                sql = "Update Product Set Deleted = 1, UpdatedOnUtc = '" + startTime.ToString("yyyy-MM-dd HH:mm:ss") + "' Where Deleted = 0 And UpdatedOnUtc < '" + startTime.AddDays(-ConfigAppString.Day).ToString("yyyy-MM-dd HH:mm:ss") + "' And Id in (Select ProductId From Product_Category_Mapping Where CategoryId = " + categoryid + ")";
            ProductContronller.UpdateProductBySql(sql);
            OutManagerContronller.WriterInfo(TraceEventType.Verbose, "Update Proportion: " + sql);
        }
    }
}
