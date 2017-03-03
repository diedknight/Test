using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FisherPaykelTool.Model;
using FisherPaykelTool.Excel;
using System.IO;

namespace FisherPaykelTool
{
    class Program
    {
        static void Main(string[] args)
        {
            //var range1 = new DateRange().GetRange();

            Log.Log log = new Log.Log();

            log.WriteLine("load excel");
            CateAttrCollection.Load();

            log.WriteLine("load products");
            var products = ProductAd.Get(0);

            log.WriteLine("write to excel");

            List<string> titleList = new List<string>();
            titleList.Add("Retailer Name");
            titleList.Add("Retailer Type");
            titleList.Add("PriceMe_Name");
            titleList.Add("Model No.");
            titleList.Add("Brand");
            titleList.Add("Product Category");
            titleList.Add("");
            titleList.Add("Size/Capacity");
            titleList.Add("");
            titleList.Add("Type/function");
            titleList.Add("");
            titleList.Add("Finish");
            titleList.Add("");
            titleList.Add("Energy/Water Rating");
            titleList.Add("Number_Retailers");
            titleList.Add("Lowest_Price");
            titleList.Add("Url");
            titleList.Add("RetailerProductName");
            titleList.Add("Price when report was extracted ");

            new DateRange().GetRange().ForEach(range => {
                titleList.Add("Lowest Price " + range.ToString());
                titleList.Add("Average Price " + range.ToString());
            });

            //titleList.Add("average_price");

            var exhelper = new ExcelSimpleHelper();
            exhelper.WriteLine(titleList.ToArray());

            products.ForEach(item =>
            {
                List<string> contentList = new List<string>();

                contentList.Add(item.RetailerName);
                contentList.Add(item.RetailerType);
                contentList.Add(item.ProductName);
                contentList.Add(item.ModelNo);
                contentList.Add(item.Brand);
                contentList.Add(item.ProductCategory);
                contentList.Add(item.Size_CapacityName);
                contentList.Add(item.Size_CapacityValue);
                contentList.Add(item.Type_FunctionsName);
                contentList.Add(item.Type_FunctionsValue);
                contentList.Add(item.FinishName);
                contentList.Add(item.FinishValue);
                contentList.Add(item.Energy_Water_RatingName);
                contentList.Add(item.Energy_Water_RatingValue);
                contentList.Add(item.NumberRetailers.ToString());
                contentList.Add(item.LowestPrice.ToString("0.00"));
                contentList.Add(item.PurchaseURL);
                contentList.Add(item.RetailerProductName);
                contentList.Add(item.RetailerPrice.ToString("0.00"));

                for (int i = 0; i < item.HistoryPrices.Count; i++)
                {
                    var historyPrice = item.HistoryPrices[i];
                    var avgPrice = item.AvgPrices[i];

                    if (historyPrice == 0) contentList.Add("");
                    else contentList.Add(historyPrice.ToString("0.00"));

                    if (avgPrice == 0) contentList.Add("");
                    else contentList.Add(avgPrice.ToString("0.00"));
                }


                //item.HistoryPrices.ForEach(newprice =>
                //{
                //    if (newprice == 0) contentList.Add("");
                //    else contentList.Add(newprice.ToString("0.00"));
                //});

                //contentList.Add(item.AvePrice.ToString("0.00"));

                exhelper.WriteLine(contentList.ToArray());
            });
            
            //other 
            //Sort(ProductAd.Get(1)).ForEach(item => {
            //    exhelper.WriteLine("");
            //    exhelper.WriteLine(item.Manufacturer, "Other", item.CategoryName);

            //    item.List.ForEach(product => {
            //        List<string> contentList = new List<string>();
            //        contentList.Add(product.RetailerName);
            //        contentList.Add(product.RetailerType);
            //        contentList.Add(product.ProductName);
            //        contentList.Add(product.Brand);
            //        contentList.Add(product.ProductCategory);
            //        contentList.Add(product.NumberRetailers.ToString());
            //        contentList.Add(product.LowestPrice.ToString("0.00"));
            //        contentList.Add(product.PurchaseURL);
            //        contentList.Add(product.RetailerProductName);
            //        contentList.Add(product.RetailerPrice.ToString("0.00"));

            //        product.NewPrices.ForEach(newprice =>
            //        {
            //            if (newprice == 0) contentList.Add("");
            //            else contentList.Add(newprice.ToString("0.00"));
            //        });

            //        contentList.Add(product.AvePrice.ToString("0.00"));

            //        exhelper.WriteLine(contentList.ToArray());

            //    });
            //});

            string outputFile = System.Configuration.ConfigurationManager.AppSettings["outputFilePath"];
            string name = Path.GetFileNameWithoutExtension(outputFile) + "_" + DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss");
            outputFile = outputFile.Replace(Path.GetFileNameWithoutExtension(outputFile), name);

            exhelper.Save(outputFile);

            log.WriteLine("finish");
        }


        private static List<OtherProduct> Sort(List<ProductAd> productlist)
        {
            List<OtherProduct> list = new List<OtherProduct>();

            productlist.ForEach(product => {
                var otherProduct = list.FirstOrDefault(item => product.Brand == item.Manufacturer && product.ProductCategory == item.CategoryName);

                if (otherProduct == null)
                {
                    otherProduct = new OtherProduct();
                    otherProduct.CategoryName = product.ProductCategory;
                    otherProduct.Manufacturer = product.Brand;

                    list.Add(otherProduct);
                }

                otherProduct.List.Add(product);                
            });
            
            return list;
        }


        //class
        private class OtherProduct
        {
            public string Manufacturer { get; set; }
            public string CategoryName { get; set; }

            public List<ProductAd> List { get; set; } = new List<ProductAd>();
        }

    }    

}
