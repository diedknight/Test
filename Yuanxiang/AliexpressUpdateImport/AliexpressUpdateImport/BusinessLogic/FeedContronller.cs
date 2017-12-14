using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LumenWorks.Framework.IO.Csv;
using AliexpressImport.Data;
using System.Diagnostics;
using System.IO;

namespace AliexpressImport.BusinessLogic
{
    public static class FeedContronller
    {
        public static List<ProductInfoEntity> GetProductInfoEntity(string feedpath)
        {
            List<ProductInfoEntity> products = new List<ProductInfoEntity>();

            using (CsvReader csvReader = new CsvReader(new StreamReader(feedpath), false))
            {
                bool isHeader = true;
                int currentCount = 0;
                bool isHasNext = false;
                isHasNext = csvReader.ReadNextRecord();
                bool isCatch = false;
                bool isError = false;

                while (isHasNext)
                {
                    isCatch = false;
                    try
                    {
                        if (isHeader)
                        {
                            isHeader = false;
                            isHasNext = CheckIsHasNextWithCSV(csvReader);
                            continue;
                        }
                        if (csvReader.FieldCount == 1)
                        {
                            isHasNext = CheckIsHasNextWithCSV(csvReader);
                            continue;
                        }

                        currentCount++;
                        ProductInfoEntity productInfoEntity = GetProductInfoFormat(csvReader);

                        if (String.IsNullOrEmpty(productInfoEntity.ProductName))
                        {
                            isHasNext = CheckIsHasNextWithCSV(csvReader);
                            continue;
                        }

                        products.Add(productInfoEntity);
                        isHasNext = CheckIsHasNextWithCSV(csvReader);
                    }
                    catch (Exception ex)
                    {
                        isCatch = true;
                        if (!isError)
                        {
                            OutManagerContronller.WriterInfo(TraceEventType.Error, "Line: " + currentCount.ToString() + " Error: " + ex.Message);
                            isError = true;
                        }
                    }

                    //因为CheckIsHasNextWithCSV放在Catch里面也可能会报错
                    try { if (isCatch) { isHasNext = CheckIsHasNextWithCSV(csvReader); } }
                    catch { continue; }
                }
            }

            OutManagerContronller.WriterInfo(TraceEventType.Verbose, "End read file......");

            return products;
        }

        public static ProductInfoEntity GetProductInfoFormat(CsvReader csvReader)
        {
            ProductInfoEntity productInfoEntity = new ProductInfoEntity();
            
            productInfoEntity.ProductName = GetColumnData(csvReader, 0);
            productInfoEntity.Sku = GetColumnData(csvReader, 1);
            
            decimal price = 0m;
            decimal.TryParse(GetColumnData(csvReader, 2), out price);
            productInfoEntity.Price = price;
            
            productInfoEntity.Category = GetColumnData(csvReader, 3);
            productInfoEntity.AdminComment = GetColumnData(csvReader, 4);
            
            return productInfoEntity;
        }

        public static bool CheckIsHasNextWithCSV(CsvReader csvReader)
        {
            bool isHasNext = false;
            try
            {
                isHasNext = csvReader.ReadNextRecord();
            }
            catch (Exception ex)
            {
                isHasNext = csvReader.ReadNextRecord();
            }

            return isHasNext;
        }

        public static string GetColumnData(CsvReader csvReader, int columnNum)
        {
            return CleanString(csvReader[columnNum]).Trim();
        }

        public static string CleanString(string str)
        {
            if (null == str)
            {
                throw new ArgumentException("Invalid string specified for cleaning");
            }
            String strRet = str.Replace("\r\n", "");
            strRet = strRet.Replace("\t", "");
            strRet = strRet.Replace("&amp;", "&");
            strRet = strRet.Replace("&nbsp;", " ");
            strRet = strRet.Replace("&quot;", "\"");
            strRet = strRet.Replace("\r", "");
            strRet = strRet.Replace("\n", "");
            strRet = System.Web.HttpUtility.HtmlDecode(strRet);
            strRet = strRet.Trim();
            return strRet;
        }
    }
}
