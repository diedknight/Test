﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Data.SqlClient;

namespace FisherPaykelTool.Model
{
    public class ProductAd : Product
    {
        private static bool _isancient = GetAppConfig("isancient", false);
        private static bool _isReplace = GetAppConfig("isreplace", false);

        public List<decimal> HistoryPrices { get; set; }
        public List<decimal> AvgPrices { get; set; }

        public ProductAd(Product p)
        {
            this.HistoryPrices = new List<decimal>();
            this.AvgPrices = new List<decimal>();

            this.Brand = p.Brand;
            this.CategoryId = p.CategoryId;
            this.Energy_Water_RatingName = p.Energy_Water_RatingName;
            this.Energy_Water_RatingValue = p.Energy_Water_RatingValue;
            this.FinishName = p.FinishName;
            this.FinishValue = p.FinishValue;
            this.LowestPrice = p.LowestPrice;
            this.ManufacturerId = p.ManufacturerId;
            this.ModelNo = p.ModelNo;
            this.NumberRetailers = p.NumberRetailers;
            this.ProductCategory = p.ProductCategory;
            this.ProductId = p.ProductId;
            this.ProductName = p.ProductName;
            this.PurchaseURL = p.PurchaseURL;
            this.RetailerId = p.RetailerId;
            this.RetailerName = p.RetailerName;
            this.RetailerPrice = p.RetailerPrice;
            this.RetailerProductName = p.RetailerProductName;
            this.RetailerType = p.RetailerType;
            this.Size_CapacityName = p.Size_CapacityName;
            this.Size_CapacityValue = p.Size_CapacityValue;
            this.StoreType = p.StoreType;
            this.Type_FunctionsName = p.Type_FunctionsName;
            this.Type_FunctionsValue = p.Type_FunctionsValue;
            this.RetailerProductId = p.RetailerProductId;
            this.RetailerProductStatus = p.RetailerProductStatus;
            this.RetailerProductModifiedOn = p.RetailerProductModifiedOn;
            this.OriginalPrice = p.OriginalPrice;
        }

        public static new List<ProductAd> Get(int type = 0)
        {
            var list = Product.Get(type).Select(item => new ProductAd(item)).ToList();

            var dateRange = new DateRange();
            var rangs = dateRange.GetRange();

            var cache = new Cache(list);            

            list.ForEach(item =>
            {
                rangs.ForEach(range =>
                {
                    decimal price = 0m;

                    price = cache.GetLowestPrice(item.RetailerProductId, range.Start, range.End);
                    if (_isancient && price == 0m) price = cache.GetLowestPrice(item.RetailerProductId, range.End);

                    //当前价格替换空白
                    if (_isReplace && price <= 0)
                    {
                        price = item.RetailerPrice;
                    }

                    item.HistoryPrices.Add(price);

                    //avgprice
                    decimal avgPrice = 0m;
                    avgPrice = cache.GetAvePrice(item.RetailerProductId, range.Start, range.End);

                    //当前价格替换空白
                    if (_isReplace && avgPrice <= 0)
                    {
                        avgPrice = item.RetailerPrice;
                    }

                    item.AvgPrices.Add(avgPrice);
                });

                //item.AvePrice = cache.GetAvePrice(item.RetailerProductId, dateRange.Start, dateRange.End);
                //if (item.AvePrice == 0) item.AvePrice = item.RetailerPrice;
            });

            return list;
        }

        private static bool GetAppConfig(string key, bool defVal)
        {
            string str = System.Configuration.ConfigurationManager.AppSettings[key];
            if (string.IsNullOrEmpty(str)) return defVal;

            if (str.Trim() == "0") return false;
            if (str.Trim() == "1") return true;

            bool result = false;

            if (!bool.TryParse(str, out result))
            {
                result = defVal;
            }

            return result;
        }

        //class
        private class Cache
        {
            private List<CacheData> _list = new List<CacheData>();
            private Dictionary<int, List<int>> _index = new Dictionary<int, List<int>>();

            public Cache(List<ProductAd> list)
            {
                List<int> ridList = new List<int>();
                list.ForEach(item =>
                {
                    if (ridList.Contains(item.RetailerId)) return;

                    ridList.Add(item.RetailerId);
                });

                string conStr = System.Configuration.ConfigurationManager.ConnectionStrings["EDW"].ConnectionString;
                string sql = "select RPID,NewPrice,CreatedOn from CSK_Store_RetailerProductHistory where RID in @RIds order by CreatedOn asc";
                string sql1 = "select RPID,NewPrice,CreatedOn from PriceMe_D.dbo.EDW_CSK_Store_RetailerProductHistory where RID in @RIds order by CreatedOn asc";

                //string sql = "";
                //sql += " select * from(";
                //sql += " select RPID, NewPrice, CreatedOn, RID from CSK_Store_RetailerProductHistory";
                //sql += " UNION ALL";
                //sql += " select RPID, NewPrice, CreatedOn, RID from CSK_Store_RetailerProductHistory_bak";
                //sql += " ) as a";
                //sql += " where a.RID in (20) order by CreatedOn asc";

                using (SqlConnection con = new SqlConnection(conStr))
                {
                    this._list = con.Query<CacheData>(sql, new { RIds = ridList }, null, true, 3000).ToList();
                }

                using (SqlConnection con = new SqlConnection(conStr))
                {
                    this._list.AddRange(con.Query<CacheData>(sql1, new { RIds = ridList }, null, true, 3000).ToList());
                }


                for (int i = 0; i < this._list.Count; i++)
                {
                    var item = this._list[i];

                    if (this._index.ContainsKey(item.RPID))
                    {
                        this._index[item.RPID].Add(i);
                    }
                    else
                    {
                        this._index.Add(item.RPID, new List<int>() { i });
                    }
                }
            }
            
            public decimal GetPrice(int rpId, DateTime end)
            {
                if (!this._index.ContainsKey(rpId)) return 0m;

                List<CacheData> tempList = new List<CacheData>();
                this._index[rpId].ForEach(index => { tempList.Add(this._list[index]); });

                var data = tempList.LastOrDefault(item => item.CreatedOn < end);
                if (data == null) return 0m;

                return data.NewPrice;
            }

            public decimal GetPrice(int rpId, DateTime start, DateTime end)
            {
                if (!this._index.ContainsKey(rpId)) return 0m;

                List<CacheData> tempList = new List<CacheData>();
                this._index[rpId].ForEach(index => { tempList.Add(this._list[index]); });

                var data = tempList.FirstOrDefault(item => item.CreatedOn > start && item.CreatedOn < end);
                if (data == null) return 0m;

                return data.NewPrice;
            }

            public decimal GetLowestPrice(int rpId, DateTime start, DateTime end)
            {
                if (!this._index.ContainsKey(rpId)) return 0m;

                List<CacheData> tempList = new List<CacheData>();
                this._index[rpId].ForEach(index => { tempList.Add(this._list[index]); });

                AvePriceCollection apc = new AvePriceCollection(tempList);

                return apc.GetLowestPrice(start, end);
            }

            public decimal GetLowestPrice(int rpId, DateTime end)
            {
                return this.GetLowestPrice(rpId, DateTime.MinValue, end);
            }

            public decimal GetAvePrice(int rpId, DateTime start, DateTime end)
            {
                if (!this._index.ContainsKey(rpId)) return 0m;

                List<CacheData> tempList = new List<CacheData>();
                this._index[rpId].ForEach(index => { tempList.Add(this._list[index]); });

                AvePriceCollection apc = new AvePriceCollection(tempList);

                return apc.GetAvePrice(start, end);
            }

        }

        private class CacheData
        {
            public int RPID { get; set; }
            public decimal NewPrice { get; set; }
            public DateTime CreatedOn { get; set; }
        }

        private class PerPrice
        {
            public PerPrice(decimal price, DateTime date)
            {
                this.Price = price;
                this.Date = date;
            }

            public decimal Price { get; set; }
            public DateTime Date { get; set; }
        }

        private class AvePriceCollection
        {
            private List<PerPrice> _priceList = new List<PerPrice>();

            public AvePriceCollection(List<CacheData> list)
            {
                if (list == null || list.Count == 0) return;

                List<CacheData> tempList = new List<CacheData>();
                list.ForEach(item => tempList.Add(item));

                DateTime firstTime = Convert.ToDateTime(list[0].CreatedOn.ToString("yyyy-MM-dd"));
                DateTime lastTime = Convert.ToDateTime(list[list.Count - 1].CreatedOn.ToString("yyyy-MM-dd"));
                DateTime nowTime = DateTime.Now;

                int daySpan = (lastTime - firstTime).Days;

                for (int i = 0; i <= daySpan; i++)
                {
                    if (tempList.Count == 0)
                    {
                        var prevPrice = this._priceList[this._priceList.Count - 1].Price;

                        this._priceList.Add(new PerPrice(prevPrice, firstTime.AddDays(i)));
                        continue;
                    }

                    var perDay = firstTime.AddDays(i);
                    var pointDay = Convert.ToDateTime(tempList[0].CreatedOn.ToString("yyyy-MM-dd"));

                    while ((pointDay - perDay).Days < 0)
                    {
                        tempList.RemoveAt(0);
                        pointDay = Convert.ToDateTime(tempList[0].CreatedOn.ToString("yyyy-MM-dd"));
                    }

                    if ((pointDay - perDay).Days == 0)
                        this._priceList.Add(new PerPrice(tempList[0].NewPrice, perDay));

                    if ((pointDay - perDay).Days > 0)
                    {
                        var prevPrice = this._priceList[this._priceList.Count - 1].Price;

                        this._priceList.Add(new PerPrice(prevPrice, firstTime.AddDays(i)));
                    }
                }

                daySpan = (nowTime - this._priceList.Last().Date).Days;
                for (int i = 0; i < daySpan; i++)
                {
                    var prevPrice = this._priceList.Last().Price;
                    var prevDate = this._priceList.Last().Date;

                    this._priceList.Add(new PerPrice(prevPrice, prevDate.AddDays(1)));
                }

            }

            public decimal GetAvePrice(DateTime start, DateTime end)
            {
                var list = this._priceList.Where(item => item.Date >= start && item.Date <= end).ToList();
                if (list.Count == 0) return 0m;

                return list.Average(item => item.Price);
            }

            public decimal GetLowestPrice(DateTime start, DateTime end)
            {
                var price = 0m;

                var list = this._priceList.Where(item => item.Date >= start && item.Date <= end).ToList();
                if (list.Count == 0) return price;

                price = list[0].Price;
                list.ForEach(item =>
                {
                    if (item.Price <= 0) return;
                    if (item.Price >= price) return;

                    price = item.Price;
                });

                return price;
            }

            public decimal GetLowestPrice(DateTime end)
            {
                return this.GetLowestPrice(DateTime.MinValue, end);
            }

        }

    }
}
