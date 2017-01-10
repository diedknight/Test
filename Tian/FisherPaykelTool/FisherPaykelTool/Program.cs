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
            Log.Log log = new Log.Log();

            log.WriteLine("load excel");
            CateAttrCollection.Load();

            log.WriteLine("load products");
            var products = ProductAd.Get();

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
            titleList.Add("Price");

            new DateRange().GetRange().ForEach(range => { titleList.Add(range.ToString()); });
            titleList.Add("average_price");

            var exhelper = new ExcelSimpleHelper();
            exhelper.WriteLine(titleList.ToArray());
            //exhelper.WriteLine("Retailer Name",
            //    "Retailer Type",
            //    "PriceMe_Name",
            //    "Model No.",
            //    "Brand",
            //    "Product Category",
            //    "",
            //    "Size/Capacity",
            //    "",
            //    "Type/function",
            //    "",
            //    "Finish",
            //    "",
            //    "Energy/Water Rating",
            //    "Number_Retailers",
            //    "Lowest_Price",
            //    "Url",
            //    "RetailerProductName",
            //    "Price");

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

                item.NewPrices.ForEach(newprice =>
                {
                    if (newprice == 0) contentList.Add("");
                    else contentList.Add(newprice.ToString("0.00"));
                });

                contentList.Add(item.AvePrice.ToString("0.00"));

                exhelper.WriteLine(contentList.ToArray());

                //exhelper.WriteLine(item.RetailerName,
                //    item.RetailerType,
                //    item.ProductName,
                //    item.ModelNo,
                //    item.Brand,
                //    item.ProductCategory,
                //    item.Size_CapacityName,
                //    item.Size_CapacityValue,
                //    item.Type_FunctionsName,
                //    item.Type_FunctionsValue,
                //    item.FinishName,
                //    item.FinishValue,
                //    item.Energy_Water_RatingName,
                //    item.Energy_Water_RatingValue,
                //    item.NumberRetailers.ToString(),
                //    item.LowestPrice.ToString("0.00"),
                //    item.PurchaseURL,
                //    item.RetailerProductName,
                //    item.RetailerPrice.ToString("0.00"));
            });

            string outputFile = System.Configuration.ConfigurationManager.AppSettings["outputFilePath"];
            string name = Path.GetFileNameWithoutExtension(outputFile) + "_" + DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss");
            outputFile = outputFile.Replace(Path.GetFileNameWithoutExtension(outputFile), name);

            exhelper.Save(outputFile);

            log.WriteLine("finish");
        }
    }
}
