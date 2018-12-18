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
    public class CompareHistoryModel : PageModel
    {
        public ResourcesData resData;
        public string pids;
        public int countryId;

        public List<ProductNewData> products;
        public string json;
        public List<PriceHistory> phs;

        public List<int> listPids;

        public void OnGet()
        {
            pids = Utility.GetParameter("pids", this.Request);
            countryId = Utility.GetIntParameter("cid", this.Request);
            resData = WebSiteConfig.dicResources[countryId];

            BindProductHistory();
            products = ProductController.GetRealProductSimplified(listPids, resData.DbInfo);

            if (products.Count > 0)
                json = GetHistoryChartJson(listPids, 365);
        }

        protected void BindProductHistory()
        {
            phs = new List<PriceHistory>();
            List<string> listpid = pids.Split(',').ToList();
            listPids = new List<int>();

            if (listpid.Count > 0)
            {
                foreach (string pid in listpid)
                {
                    int id = 0;
                    int.TryParse(pid, out id);
                    listPids.Add(id);
                }

                phs = ProductController.GetPriceHistoryData(listPids, resData.DbInfo);
            }
        }

        public string GetHistoryChartJson(List<int> pidList, int dayInfo)
        {
            string json = "[";

            DateTime nowDT = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

            Dictionary<int, int> fPriceDic = new Dictionary<int, int>();
            List<PriceHistory> phCollection = phs.Where(ph => pidList.Contains(ph.ProductID)).ToList();
            List<ProductPriceHistoryList> historyList = new List<ProductPriceHistoryList>();

            if (phCollection.Count == 0)
            {
                foreach (var pc in products)
                {
                    ProductPriceHistoryList pphl = new ProductPriceHistoryList();
                    pphl.ProductName = pc.ProductName;
                    pphl.ProductId = pc.ProductID;

                    ProductPriceHistory pph = new ProductPriceHistory();
                    pph.PriceDate = nowDT;
                    pph.Price = (int)pc.BestPrice;
                    pphl.Add(pph);
                    historyList.Add(pphl);
                }
            }
            else
            {
                DateTime minDate = nowDT;
                foreach (int pid in pidList)
                {
                    ProductNewData pc = null;
                    foreach (ProductNewData mPc in products)
                    {
                        if (mPc.ProductID == pid)
                        {
                            pc = mPc;
                            break;
                        }
                    }

                    if (pc == null)
                        continue;

                    ProductPriceHistoryList pphl = new ProductPriceHistoryList();
                    pphl.ProductName = pc.ProductName;
                    pphl.ProductId = pc.ProductID;

                    List<PriceHistory> phList = phCollection.FindAll(ph => ph.ProductID == pid).OrderBy(ph => ph.CreatedOn).ToList();
                    if (phList.Count == 0)
                    {
                        ProductPriceHistory pph = new ProductPriceHistory();
                        pph.PriceDate = nowDT;
                        pph.Price = (int)pc.BestPrice;
                        pphl.Add(pph);
                        historyList.Add(pphl);
                    }
                    else
                    {
                        foreach (PriceHistory ph in phList)
                        {
                            ProductPriceHistory pph = new ProductPriceHistory();
                            pph.PriceDate = new DateTime(ph.PriceDate.Year, ph.PriceDate.Month, ph.PriceDate.Day);
                            pph.Price = (int)ph.Price;
                            pphl.Add(pph);
                        }

                        if (pphl[0].PriceDate < minDate)
                        {
                            minDate = pphl[0].PriceDate;
                        }

                        historyList.Add(pphl);
                    }
                }

                foreach (var history in historyList)
                {
                    history.UseStartDT = true;
                    history.StartDT = minDate;
                }
            }

            foreach (var history in historyList)
            {
                json += history.ToJson2() + ",";
            }

            json = json.TrimEnd(',') + "]";
            return json;
        }
    }
}