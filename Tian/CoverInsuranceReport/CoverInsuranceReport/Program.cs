using CoverInsuranceReport.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoverInsuranceReport
{
    class Program
    {
        static void Main(string[] args)
        {
            List<ExcelData> outputList = new List<ExcelData>();
            List<ExcelData> outputNewItemList = new List<ExcelData>();

            Priceme.Infrastructure.Excel.ExcelSimpleHelper excelHelper = new Priceme.Infrastructure.Excel.ExcelSimpleHelper();

            ExcelCtrl excelCtrl = new ExcelCtrl();
            ExcelCtrl excelCtrl1 = new ExcelCtrl(1);
            CateCtrl cateCtrl = new CateCtrl();

            var excelList = excelCtrl.GetData();
            var upComingExcelList = excelCtrl1.GetData();

            //active
            var activeList = DB.GetActive();
            var activePIds = activeList.Select(item => item.PId).ToList();
            var activepriceList = DB.GetActivePrice(activePIds);
            var activeImageList = DB.GetImage(activePIds);
            var activeUpdateList = DB.GetUpdate(activePIds);

            activeList.ForEach(item =>
            {
                //category
                item.CategoryName = cateCtrl.GetData().FirstOrDefault(c => c.CId == Convert.ToInt32(item.CategoryName)).CategoryName;

                //productfamily,series,model
                var excelItem = excelList.FirstOrDefault(p => p.PId == item.PId);
                if (excelItem != null)
                {
                    item.ProductFamily = excelItem.ProductFamily;
                    item.Series = excelItem.Series;
                    item.Model = excelItem.Model;
                }
                else
                {
                    item.IsNew = true;
                }

                //price
                var priceItem = activepriceList.FirstOrDefault(p => p.PId == item.PId);
                if (priceItem != null)
                {
                    item.MinPrice = priceItem.Min;
                    item.AveragePrice = priceItem.Avg;
                    item.MaxPrice = priceItem.Max;
                    item.NumberOfPrices = priceItem.Num;
                }

                //Image
                var imgItem = activeImageList.FirstOrDefault(p => p.PId == item.PId);
                if (imgItem != null)
                {
                    item.ProductImageUrl = imgItem.DefaultImage;
                }

                //updateOn
                var updateItem = activeUpdateList.FirstOrDefault(p => p.PId == item.PId);
                if (updateItem != null)
                {
                    item.UpdateOn = updateItem.UpdatedOn.ToString("yyyy-MM-dd");
                }

                if (item.IsNew)
                {
                    outputNewItemList.Add(item);
                }
                else
                {
                    outputList.Add(item);
                }

            });

            //inacative
            var inactiveList = DB.GetInActive();
            var inactivePIds = inactiveList.Select(item => item.PId).ToList();
            var inactivepriceList = DB.GetInactivePrice(inactivePIds);
            var inactiveImageList = DB.GetImage(inactivePIds);
            var inactiveUpdateList = DB.GetUpdate(inactivePIds);

            inactiveList.ForEach(item =>
            {
                //category
                item.CategoryName = cateCtrl.GetData().FirstOrDefault(c => c.CId == Convert.ToInt32(item.CategoryName)).CategoryName;

                //productfamily,series,model
                var excelItem = excelList.FirstOrDefault(p => p.PId == item.PId);
                if (excelItem != null)
                {
                    item.ProductFamily = excelItem.ProductFamily;
                    item.Series = excelItem.Series;
                    item.Model = excelItem.Model;
                }
                else
                {
                    item.IsNew = true;
                }

                //price
                var priceItem = inactivepriceList.FirstOrDefault(p => p.PId == item.PId);
                if (priceItem != null)
                {
                    item.MinPrice = priceItem.Min;
                    item.AveragePrice = priceItem.Avg;
                    item.MaxPrice = priceItem.Max;
                    item.NumberOfPrices = priceItem.Num;
                }

                //Image
                var imgItem = inactiveImageList.FirstOrDefault(p => p.PId == item.PId);
                if (imgItem != null)
                {
                    item.ProductImageUrl = imgItem.DefaultImage;
                }

                //updateOn
                var updateItem = inactiveUpdateList.FirstOrDefault(p => p.PId == item.PId);
                if (updateItem != null)
                {
                    item.UpdateOn = updateItem.UpdatedOn.ToString("yyyy-MM-dd");
                }

                if (item.IsNew)
                {
                    outputNewItemList.Add(item);
                }
                else
                {
                    outputList.Add(item);
                }
            });

            //output to excel
            excelHelper.WriteLine(
                "pid",
                "category name",
                "manufacturer",
                "product family",
                "series",
                "model",
                "product name",
                "min price",
                "average price",
                "max price",
                "number of prices",
                "productimageurl",
                "upcoming",
                "createdon",
                "UpdateOn",
                "For sale"
                );

            outputList.ForEach(item => WriteLine(excelHelper, item));
            outputNewItemList.ForEach(item => WriteLine(excelHelper, item, true));

            //clear output list
            outputList = new List<ExcelData>();
            outputNewItemList = new List<ExcelData>();


            //upcoming
            var upComingList = DB.GetUpComing();
            var upComingPIds = upComingList.Select(item => item.PId).ToList();
            var upComingpriceList = DB.GetUpComingPrice(upComingPIds);
            //var upComingImageList = DB.GetImage(upComingPIds);
            //var upComingUpdateList = DB.GetUpdate(upComingPIds);

            upComingList.ForEach(item =>
            {
                //category
                item.CategoryName = cateCtrl.GetData().FirstOrDefault(c => c.CId == Convert.ToInt32(item.CategoryName)).CategoryName;

                //productfamily,series,model
                var excelItem = upComingExcelList.FirstOrDefault(p => p.PId == item.PId);
                if (excelItem != null)
                {
                    item.ProductFamily = excelItem.ProductFamily;
                    item.Series = excelItem.Series;
                    item.Model = excelItem.Model;
                }
                else
                {
                    item.IsNew = true;
                }

                //price
                var priceItem = upComingpriceList.FirstOrDefault(p => p.PId == item.PId);
                if (priceItem != null)
                {
                    item.MinPrice = priceItem.Price;
                    item.AveragePrice = priceItem.Price;
                    item.MaxPrice = priceItem.Price;
                    //item.NumberOfPrices = priceItem.Num;
                    item.Upcoming = priceItem.Upcoming.ToString("yyyy-MM-dd");
                }


                //Image
                item.ProductImageUrl = GetImgUrl(item.ProductImageUrl);


                //updateOn
                //var updateItem = upComingUpdateList.FirstOrDefault(p => p.PId == item.PId);
                //if (updateItem != null)
                //{
                //    item.UpdateOn = updateItem.UpdatedOn.ToString("yyyy-MM-dd");
                //}

                if (item.IsNew)
                {
                    outputNewItemList.Add(item);
                }
                else
                {
                    outputList.Add(item);
                }
            });


            //output to excel
            excelHelper.ReadSheetAt(1);
            excelHelper.WriteLine(
                "pid",
                "category name",
                "manufacturer",
                "product family",
                "series",
                "model",
                "product name",
                "min price",
                "average price",
                "max price",
                "number of prices",
                "productimageurl",
                "upcoming",
                "createdon",
                "UpdateOn",
                "For sale"
                );

            outputList.ForEach(item => WriteLine(excelHelper, item));
            outputNewItemList.ForEach(item => WriteLine(excelHelper, item, true));

            excelHelper.Save(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Report_" + DateTime.Now.ToString("yyyy-MM-dd") + ".xls"));
        }

        public static void WriteLine(Priceme.Infrastructure.Excel.ExcelSimpleHelper helper, ExcelData data, bool isRed = false)
        {
            helper.WriteLine(false, isRed,
                data.PId.ToString(),
                data.CategoryName,
                data.Manufacturer,
                data.ProductFamily,
                data.Series,
                data.Model,
                data.ProductName,
                data.MinPrice.ToString("0.00"),
                data.AveragePrice.ToString("0.00"),
                data.MaxPrice.ToString("0.00"),
                data.NumberOfPrices.ToString(),
                data.ProductImageUrl,
                data.Upcoming,
                data.CreatedOn,
                data.UpdateOn,
                data.ForSale
                );
        }

        public static string GetImgUrl(string defaultImage)
        {
            string imgUrl = "";

            if (!string.IsNullOrEmpty(defaultImage))
            {
                imgUrl = defaultImage.Insert(defaultImage.LastIndexOf('.'), "_l");
                if (!imgUrl.ToLower().Contains("https://"))
                {
                    imgUrl = "https://images.pricemestatic.com/Large" + imgUrl.Replace("\\", "/");
                }
                else
                {
                    Uri u = new Uri(imgUrl);

                    imgUrl = u.Scheme + "://" + u.Host + "/Large" + u.AbsolutePath;
                }
            }

            return imgUrl;
        }
    }
}
