using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PricemeResource.Data
{
    public class ProductPriceHistoryList : List<ProductPriceHistory>
    {
        public string ProductName { get; set; }
        public int ProductId { get; set; }
        public int FPrice { get; set; }
        public DateTime StartDT { get; set; }
        public bool UseStartDT { get; set; }

        public ProductPriceHistoryList()
        {
            FPrice = 0;
            UseStartDT = false;
        }

        public string ToJson()
        {
            DateTime startDT = new DateTime(this[0].PriceDate.Year, this[0].PriceDate.Month, this[0].PriceDate.Day);
            DateTime endDT = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            string json = "[";
            int index = 0;
            ProductPriceHistory pph = new ProductPriceHistory();
            pph.Price = this[0].Price;
            while (startDT <= endDT)
            {
                pph.PriceDate = startDT;
                if (index < this.Count && this[index].PriceDate == startDT)
                {
                    pph.Price = this[index].Price;
                    index++;
                }
                json += pph.ToJson() + ",";
                startDT = startDT.AddDays(1);
            }
            json = json.TrimEnd(',') + "]";
            return json;
        }

        public string ToJson2()
        {
            DateTime startDT = new DateTime(this[0].PriceDate.Year, this[0].PriceDate.Month, this[0].PriceDate.Day);
            if (UseStartDT)
            {
                startDT = StartDT;
            }
            DateTime endDT = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            string json = "{\"Pid\":" + ProductId + ",\"ProductName\":" + Newtonsoft.Json.JsonConvert.SerializeObject(ProductName) + ",\"Data\":[";

            int index = 0;
            ProductPriceHistory pph = new ProductPriceHistory();
            if (startDT.Equals(this[0].PriceDate))
            {
                pph.Price = this[0].Price;
            }

            while (startDT <= endDT)
            {
                pph.PriceDate = startDT;
                if (index < this.Count && this[index].PriceDate == startDT)
                {
                    pph.Price = this[index].Price;
                    index++;
                }
                json += pph.ToJson() + ",";
                startDT = startDT.AddDays(1);
            }
            json = json.TrimEnd(',') + "]}";
            return json;
        }
    }

    public class ProductPriceHistory
    {
        static string JsonFormat_Static = "{{\"DT\":\"{0}\",\"Price\":{1}}}";

        public ProductPriceHistory()
        {
            Price = 0;
        }

        public ProductPriceHistory(DateTime dt, int price)
        {
            PriceDate = dt;
            Price = price;
        }

        public DateTime PriceDate
        {
            get; set;
        }

        public int Price
        {
            get; set;
        }

        public string ToJson()
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
            return string.Format(JsonFormat_Static, PriceDate.ToString("MMM dd yyyy"), Price);
        }
    }
}
