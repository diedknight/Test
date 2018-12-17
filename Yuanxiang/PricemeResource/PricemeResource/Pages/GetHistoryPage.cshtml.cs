using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PricemeResource.Data;
using PricemeResource.Logic;

namespace PricemeResource.Pages
{
    public class GetHistoryPageModel : PageModel
    {
        public ProductNewData product;
        public ResourcesData resData;

        public string PriceTrend;
        public string NoHistory;
        public string downloadPrice;

        public string json;

        public int productId;
        public string productName;
        public int countryId;

        public void OnGet()
        {
            productId = Utility.GetIntParameter("pid", this.Request);
            countryId = Utility.GetIntParameter("cid", this.Request);
            resData = WebSiteConfig.dicResources[countryId];
            PriceTrend = resData.PriceTrend;

            product = ProductController.GetRealProductSimplified(productId, resData.DbInfo);
            if (product == null)
            {
                json = "";
            }
            else
            {
                productName = product.ProductName;
                
                json = GetChartJson();
            }

            GetJsonSaveDownloadPriceHistoryFileName();
        }

        string GetChartJson()
        {
            ProductPriceHistoryList pphl = new ProductPriceHistoryList();

            List<PriceHistory> phCollection = ProductController.GetPriceHistory(productId, resData.DbInfo);

            if (phCollection == null || phCollection.Count == 0)
            {
                var product = ProductController.GetProductNew(productId, resData.DbInfo);
                if (product == null) return "";

                ProductPriceHistory pph2 = new ProductPriceHistory();
                pph2.PriceDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                pph2.Price = (int)product.BestPrice;
                pphl.Add(pph2);
            }
            else
            {
                foreach (PriceHistory ph in phCollection)
                {
                    ProductPriceHistory pph = new ProductPriceHistory();
                    pph.PriceDate = new DateTime(ph.PriceDate.Year, ph.PriceDate.Month, ph.PriceDate.Day);
                    pph.Price = int.Parse(ph.Price.ToString("0"));
                    pphl.Add(pph);
                }
                if (product.BestPrice > 0)
                {
                    float p = (float)product.BestPrice;
                    ProductPriceHistory pph = new ProductPriceHistory();
                    pph.PriceDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                    pph.Price = int.Parse(p.ToString("0"));
                    pphl.Add(pph);
                }
            }
            
            string json = pphl.ToJson();
            return json;
        }

        protected void GetJsonSaveDownloadPriceHistoryFileName()
        {
            downloadPrice = Newtonsoft.Json.JsonConvert.SerializeObject(productName + "_PriceHistory_PriceMe");
        }
    }
}