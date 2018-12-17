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
    public class GetRetailerHistoryJsonModel : PageModel
    {
        public int productId;
        public int countryId;
        public string json;

        private ResourcesData resData;

        public void OnGet()
        {
            productId = Utility.GetIntParameter("pid", this.Request);
            countryId = Utility.GetIntParameter("cid", this.Request);

            resData = WebSiteConfig.dicResources[countryId];

            json = GetJson();
        }

        private string GetJson()
        {
            var product = ProductController.GetProductNew(productId, resData.DbInfo);

            if (product != null)
            {
                DateTime dt = DateTime.Now;
                dt = dt.AddMonths(-12);
                List<ProductRetailerCountHistory> pchs = ProductController.ProductRetailerCountHistory(product.ProductID, dt, resData.DbInfo);

                RetailerCountHistoryList retailerCountHistoryList = CovertToRetailerCountHistoryList(pchs);
                if (retailerCountHistoryList.Count == 0)
                {
                    return "";
                }

                return retailerCountHistoryList.ToJson();
            }
            return "";
        }

        private RetailerCountHistoryList CovertToRetailerCountHistoryList(List<ProductRetailerCountHistory> pchs)
        {
            RetailerCountHistoryList retailerCountHistoryList = new RetailerCountHistoryList();
            foreach (ProductRetailerCountHistory prch in pchs)
            {
                RetailerCountHistory rchp = new RetailerCountHistory();
                rchp.Count = prch.RetialerCount;
                rchp.Date = prch.CreatedOn;
                rchp.Date = new DateTime(rchp.Date.Year, rchp.Date.Month, rchp.Date.Day);
                retailerCountHistoryList.Add(rchp);
            }
            return retailerCountHistoryList;
        }
    }
}