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

            //var a = Product.RRPProduct.Get();

            //var range1 = new DateRange().GetRange();

            Log.Log log = new Log.Log();

            log.WriteLine("load excel");
            CateAttrCollection.Load();

            log.WriteLine("load products");
            var products = ProductAd.Get(0);
            products = RemoveOverdueRepeatData(products);

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
            titleList.Add("RRP");
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

                if (/*GetConfigArr("RRP-rid").Contains(item.RetailerId) && */item.OriginalPrice > 0) contentList.Add(item.OriginalPrice.ToString("0.00"));
                else contentList.Add("");

                if (item.RetailerProductStatus) contentList.Add(item.RetailerPrice.ToString("0.00"));
                else contentList.Add("");
                
                for (int i = 0; i < item.HistoryPrices.Count; i++)
                {
                    var historyPrice = item.HistoryPrices[i];
                    var avgPrice = item.AvgPrices[i];

                    //last data
                    if (i == item.HistoryPrices.Count - 1 && item.RetailerProductStatus)
                    {
                        if (historyPrice == 0) historyPrice = item.RetailerPrice;
                        if (avgPrice == 0) avgPrice = item.RetailerPrice;
                    }

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

                if (item.RetailerProductStatus) exhelper.WriteLine(contentList.ToArray());
                else exhelper.WriteLine(true, contentList.ToArray());

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

        private static List<ProductAd> RemoveOverdueRepeatData(List<ProductAd> list)
        {
            HashSet<string> set = new HashSet<string>();
            HashSet<string> set1 = new HashSet<string>();
            HashSet<string> set2 = new HashSet<string>();
            List<ProductAd> products = new List<ProductAd>();

            list.ForEach(item => {

                string key = item.RetailerId + "_" + item.ProductId;
                string rpname1 = item.RetailerId + "_" + item.RetailerProductName.ToLower().TrimA();
                string rpname2 = item.RetailerProductName.ToLower().TrimA();

                if (item.RetailerProductStatus == true)
                {                    
                    products.Add(item);

                    set.Add(key);
                    set1.Add(rpname1);
                    set2.Add(rpname2);
                }
                else
                {                    
                    if (set.Contains(key)) return;
                    if (set1.Contains(rpname1)) return;

                    set.Add(key);
                    set1.Add(rpname1);
                    set2.Add(rpname2);

                    products.Add(item);
                }
            });
             
            return products;
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

        private static List<int> GetConfigArr(string key)
        {
            List<int> list = new List<int>();

            string str = System.Configuration.ConfigurationManager.AppSettings[key];
            if (string.IsNullOrEmpty(str)) return list;

            list = str.Split(',').Select(item => Convert.ToInt32(item.Trim())).ToList();

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
