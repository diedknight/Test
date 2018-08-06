using Pricealyser.Crawler.HtmlParser.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CrawlEvertenGenerate.Everten_com_au
{
    public class EvertenFetcher
    {
        string CrawleraKey = System.Configuration.ConfigurationManager.AppSettings["CrawleraKey"].ToString();
        bool IsContinue = false;
        List<string> listContinue = new List<string>();
        StreamWriter sw;

        public List<ProductItem> GetProducts()
        {
            bool.TryParse(System.Configuration.ConfigurationManager.AppSettings["IsContinue"].ToString(), out IsContinue);

            List<ProductCategory> pcs = GetProductCategoryList();
            System.Console.WriteLine("Get "+ pcs.Count + " category ......" + DateTime.Now);

            if (!IsContinue)
                BindContinueWriter();
            else
                GetContinue();

            List<ProductItem> ps = new List<ProductItem>();

            try
            {
                foreach (ProductCategory pc in pcs)
                {
                    if (listContinue.Contains(pc.CategoryName))
                        continue;
                    
                    if (!IsContinue)
                        ContinueWriter(pc.CategoryName);

                    List<ProductItem> items = GetProductItemList(pc);
                    System.Console.WriteLine("Get " + pc.CategoryName + " " + items.Count + "......" + DateTime.Now);
                    ps.AddRange(items);
                }

                if (!IsContinue)
                    sw.Close();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("Fetcher error: " + ex.Message + ex.StackTrace);
            }

            return ps;
        }

        private void BindContinueWriter()
        {
            string filepath = System.Configuration.ConfigurationManager.AppSettings["FilePath"].ToString();
            if (!Directory.Exists(filepath))
                Directory.CreateDirectory(filepath);

            string file = filepath + DateTime.Now.ToString("yyyy-MM-dd") + "_continue.txt";
            FileStream fs = new FileStream(file, FileMode.Create, FileAccess.Write);
            sw = new StreamWriter(fs, Encoding.UTF8);
        }

        private void ContinueWriter(string info)
        {
            sw.WriteLine(info);
            sw.Flush();
        }

        private void GetContinue()
        {
            string filepath = System.Configuration.ConfigurationManager.AppSettings["FilePath"].ToString();
            string file = filepath + DateTime.Now.ToString("yyyy-MM-dd") + "_continue.txt";
            if (File.Exists(file))
            {
                StreamReader sr = new StreamReader(file, Encoding.Default);
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    listContinue.Add(line);
                }
            }
        }

        public List<ProductCategory> GetProductCategoryList()
        {
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;

            List<ProductCategory> list = new List<ProductCategory>();

            JQuery doc = new JQuery(GetHttpContent("https://www.everten.com.au/sitemap"), "https://www.everten.com.au/sitemap");

            doc.find(".sitelink > ul > li > a").each(item =>
            {
                var node = item.ToJQuery();
                string name = node.val().Trim();

                string surl = node.getLink().Trim();
                if (string.IsNullOrEmpty(surl) && node.attr("rel") != null)
                    surl = "https://www.everten.com.au" + node.attr("rel").ToString();

                JQuery sdoc = new JQuery(GetHttpContent(surl), surl);
                sdoc.find(".category-list > .list-items ul > li.item").each(sitem =>
                {
                    var snode = sitem.ToJQuery();

                    string sname = snode.find(".product-name a").val().Trim();
                    string url = snode.find(".product-name a").getLink().Trim();

                    var cate = new ProductCategory();
                    cate.CategoryName = name + "->" + sname;
                    cate.CategoryUrl = url;

                    list.Add(cate);
                });
            });

            return list;
        }

        public List<ProductItem> GetProductItemList(ProductCategory productCategory)
        {
            List<ProductItem> ps = new List<ProductItem>();

            GetProducts(productCategory, ps);

            return ps;
        }

        private bool GetProducts(ProductCategory productCategory, List<ProductItem> ps)
        {
            JQuery doc = new JQuery(GetHttpContent(productCategory.CategoryUrl), productCategory.CategoryUrl);

            doc.find(".category-prods-list > li.list-items").each(item =>
            {
                var node = item.ToJQuery();

                ProductItem info = new ProductItem();
                info.ProductPrice = node.find(".price-box .special-price .price").text();
                info.PurchaseUrl = node.find("> a").getLink();

                GetProduct(info);

                ps.Add(info);
            });

            string nexturl = doc.find(".next.i-next").getLink().Trim();
            if (!string.IsNullOrEmpty(nexturl))
            {
                productCategory.CategoryUrl = nexturl;
                GetProducts(productCategory, ps);
            }

            return true;
        }

        private void GetProduct(ProductItem item)
        {
            string stringHtml = GetHttpContent(item.PurchaseUrl);
            if (stringHtml.Contains("productObject = {"))
            {
                string[] temps = stringHtml.Split(new string[] { "productObject = {" }, StringSplitOptions.None);
                string products = temps[2].Split('}')[0];
                products = "{" + products.Replace("\r\n", "") + "}";
                var obj = JsonConvert.DeserializeObject<JObject>(products);
                item.CategoryName = obj["categories"].ToString().Replace(",", " -> ");
                item.ProductName = obj["Name"].ToString();
                item.ProductSku = obj["sku"].ToString();
                item.ManufacturerName = obj["brand"].ToString();
                item.Visibility = obj["visibility"].ToString();
                item.InStock = obj["instock"].ToString();
                if (item.InStock == "1")
                    item.InStock = "yes";
                else
                    item.InStock = "no";

                item.NumberStock = obj["stock"].ToString();
                item.ProductPrice = obj["Price"].ToString();
            }
        }

        public string GetHttpContent(string url)
        {
            try
            {
                //Random rd = new Random();
                //int ran = rd.Next(3, 12);

                string apiKey = CrawleraKey + ":"; //Left the ":" sign at the end.

                var myProxy = new WebProxy("http://proxy.crawlera.com:8010");

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                var encodedApiKey = Base64Encode(apiKey);
                request.Headers.Add("Proxy-Authorization", "Basic " + encodedApiKey);
                request.Proxy = myProxy;
                request.PreAuthenticate = true;
                request.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
                
                WebResponse response = request.GetResponse();

                System.IO.StreamReader streamReader = new System.IO.StreamReader(response.GetResponseStream());

                string httpString = streamReader.ReadToEnd();

                streamReader.Close();
                response.Close();

                //System.Threading.Thread.Sleep(1000 * ran);

                return httpString;
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }

            return "";
        }

        public string Base64Encode(string apiKey)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(apiKey);
            return System.Convert.ToBase64String(plainTextBytes);
        }
    }
}
