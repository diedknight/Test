using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrawlEvertenGenerate.Everten_com_au
{
    public class ProductItem
    {
        public string ProductSku { get; set; }
        public string InStock { get; set; }
        public string NumberStock { get; set; }
        public string ProductPrice { get; set; }
        public string PurchaseUrl { get; set; }
        public string ProductName { get; set; }
        public string ManufacturerName { get; set; }
        public string CategoryName { get; set; }
        public string Visibility { get; set; }

        //string title = "Category,Brand,Product name,Product URL,Price,SKU,Visibility,In stock,Number in stock";
        public string ToCSVString()
        {
            string csvFormat = "\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\",\"{5}\",\"{6}\",\"{7}\",\"{8}\"";
            string csv = string.Format(csvFormat, ToCsvSafeString(CategoryName), ToCsvSafeString(ManufacturerName), ToCsvSafeString(ProductName), ToCsvSafeString(PurchaseUrl), ToCsvSafeString(ProductPrice), ToCsvSafeString(ProductSku), ToCsvSafeString(Visibility), ToCsvSafeString(InStock), ToCsvSafeString(NumberStock));

            return csv;
        }

        public static string ToCsvSafeString(string mStr)
        {
            if (string.IsNullOrEmpty(mStr))
                return string.Empty;

            return mStr.Replace("\"", "\"\"").Trim();
        }
    }
}
