using GhostInspectorTool.GhostInspector.Result;
using Newtonsoft.Json;
using Pricealyser.Crawler.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GhostInspectorTool.GhostInspector
{
    public class API
    {
        protected string adress = "https://api.ghostinspector.com/v1/";
        protected string method = "";

        public string apiKey { get; set; }

        public API() { }

        public API(string apiKey)
        {
            this.apiKey = apiKey;
        }

        public virtual ResultInfo<T> Request<T>()
        {
            RESTful req = new RESTful();
            req.SetHeader("Content-Type", "application/json; charset=utf-8");
            req.Method = this.method;

            this.GetType().GetProperties().ToList().ForEach(item =>
            {
                var value = item.GetValue(this, null);
                if (value == null) return;

                string name = item.Name == "Params" ? "params" : item.Name;
                string val = value.ToString();
                val = (val == "True" || val == "False") ? val.ToLower() : val;

                if (item.PropertyType.FullName.Contains("System.DateTime")) val = Convert.ToDateTime(val).ToString("yyyy-MM-dd HH:mm:ss");
                if (item.PropertyType.FullName.Contains("System.Decimal")) val = Math.Round(Convert.ToDecimal(val)).ToString("0.00");

                req.UrlParams.Add(name, val);
            });

            string result = req.Request(this.adress);

            return JsonConvert.DeserializeObject<ResultInfo<T>>(result);
        }

    }
}
