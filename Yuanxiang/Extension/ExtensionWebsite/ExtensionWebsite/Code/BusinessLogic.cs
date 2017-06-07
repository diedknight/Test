using ExtensionWebsite.Code;
using ExtensionWebsite.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExtensionWebsite.Code
{
    public static class BusinessLogic
    {
        public static List<RetailerProduct> GetRetailerProducts(Retailer retailer, string key)
        {
            List<RetailerProduct> datas = new List<RetailerProduct>();

            RetailerProduct rp = DatabaseLogic.GetRetailerProduct(retailer, key);
            List<RetailerProduct> rps = DatabaseLogic.GetRetailerProducts(retailer, rp.ProductId);
            rps = rps.Where(r => !SiteConfig.ListOverseasRetailer.Contains(r.RetailerId)).OrderBy(r => r.RetailerPrice).ToList();

            List<int> listRid = new List<int>();
            foreach (RetailerProduct p in rps)
            {
                if (listRid.Contains(p.RetailerId)) continue;
                listRid.Add(p.RetailerId);

                decimal diff = rp.RetailerPrice - p.RetailerPrice;
                List<Retailer> rs = SiteConfig.ListRetailer.Where(l => l.RetailerId == p.RetailerId).ToList();
                if (rs != null && rs.Count > 0)
                {
                    Retailer r = rs[0];
                    p.DiffPrice = diff;
                    p.RetailerLogo = r.RetailerLog;
                    p.RetailerName = r.RetailerName;
                    p.IsNolink = r.IsNolink;

                    datas.Add(p);
                }
            }

            return datas;
        }
    }
}