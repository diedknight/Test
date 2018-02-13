using RetailerProductsIndexController;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapExistingRp
{
    class Program
    {
        static void Main(string[] args)
        {
            if (ConfigurationManager.AppSettings["buildIndex"] == "1")
            {
                IndexController.BuildIndex();
            }

            Priceme.Infrastructure.Excel.ExcelSimpleHelper helper = new Priceme.Infrastructure.Excel.ExcelSimpleHelper();
            helper.WriteLine(
                "CID",
                "ProductID",
                "ProductName",
                "ToCID",
                "ToProductID",
                "ToProductName"
                );

            IndexController.Load();

            var categoryList = CateCtrl.Get();

            int sheetIndex = 0;
            DB.GetProduct(categoryList.Select(item => item.CategoryId).ToList()).ForEach(product =>
            {
                var list = IndexController.SearchIndex("RetailerProductNameMatch", product.ProductName.ToLower());
                var rpIndex = list.FirstOrDefault(item => (!string.IsNullOrEmpty(item.IsMerge) && item.IsMerge.ToLower() == "true") || item.ProductID != product.ProductID.ToString());

                if (rpIndex != null)
                {
                    var rate = 1m;
                    if (product.Price > 0)
                    {
                        rate = Math.Abs(product.Price - Convert.ToDecimal(rpIndex.RetailerPrice)) / product.Price;
                    }

                    if ((double)rate < AppConfig.PriceRate)
                    {
                        helper.WriteLine(
                            product.CategoryID.ToString(),
                            product.ProductID.ToString(),
                            product.ProductName,
                            rpIndex.CategoryID,
                            rpIndex.ProductID,
                            rpIndex.ProductName
                            );

                        if (helper.CurIndex > 40000 && sheetIndex < 4)
                        {
                            sheetIndex++;
                            helper.ReadSheetAt(sheetIndex);
                        }
                    }
                }
            });

            helper.Save(AppConfig.LogPath);
        }
    }
}
