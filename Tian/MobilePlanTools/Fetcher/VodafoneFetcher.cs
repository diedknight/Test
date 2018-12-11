using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Data;
using Winista.Text.HtmlParser;
using Common;
using Winista.Text.HtmlParser.Filters;
using Winista.Text.HtmlParser.Util;
using Winista.Text.HtmlParser.Tags;
using System.Net;
using System.IO;
using Winista.Text.HtmlParser.Lex;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Pricealyser.Crawler.Request;
using Pricealyser.Crawler.HtmlParser.Query;

namespace Fetcher
{
    public class VodafoneFetcher : BaseFetcher
    {
        public VodafoneFetcher()
            : base(3, "Vodafone")
        {
        }

        public static bool CheckValidationResult(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certificate, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors errors)
        {
            return true;
        }

        System.Net.CookieCollection cc = new System.Net.CookieCollection();

        public override List<MobilePlanInfo> GetMobilePlanInfoList()
        {
            StartCrawlingLog();
            List<MobilePlanInfo> mobilePlanList = new List<MobilePlanInfo>();
            //return mobilePlanList;
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
            ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(CheckValidationResult);

            try
            {
                string url = "https://www.vodafone.co.nz/shop/rest/model/atg/commerce/catalog/ProductCatalogActor/getProducts?callback=angular.callbacks._5&categoryId=cat1190007&filterBy=planView_onAccount&pageNum={0}&paymentType=interestFree&sortKey=SortBundleRecommended";
                StarlCrawlPhonesLinkLog(url);
               //获取手机数据列表
                var pInfos = GetPhonesData(url);

                //获取手机套餐
                string plansUrl = "https://www.vodafone.co.nz/pay-monthly/";
                XbaiRequest req = new XbaiRequest(plansUrl);
                JQuery doc = new JQuery(req.Get(), url);

                string json = doc.find("._bundles-carousel").attr("data-json");

                var jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject(json) as Newtonsoft.Json.Linq.JObject;
                var plansObj = jsonObj["assets"] as JArray;

                for(int i = 0; i < plansObj.Count; i++)
                {
                    var plan = plansObj[i]["vfoPlanSku"] as JObject;

                    var info = new MobilePlanInfo();
                    info.CarrierName = this.ProviderName;
                    info.MobilePlanName = plan.Value<string>("vfoPlanDisplayName") + " " + plan.Value<string>("vfoMonthlyPrice");

                    var items = plan["vfoCorePlanItems"] as JArray;

                    info.DataMB = items[0].Value<string>("vfoValue") + " " + items[0].Value<string>("vfoUnits");
                    var talk = items[1].Value<string>("vfoValue");
                    talk = talk.Contains("Unlimited") ? "-1" : talk.Replace(" mins", "");
                    info.Minutes = int.Parse(talk);

                    info.Texts = -1;
                    info.plus = 0;

                    info.ContractTypeId = 1;

                    //string termInfo = item.Find(".plan-tbl__price__term").InnerText.Trim();
                    //if (termInfo.Contains("1 month"))
                    //{
                    //    info.ContractTypeId = 1;
                    //}
                    //else if (termInfo.Contains("12 month"))
                    //{
                    //    info.ContractTypeId = 2;
                    //}
                    //else if (termInfo.Contains("24 month"))
                    //{
                    //    info.ContractTypeId = 3;
                    //}

                    var PhoneList = new List<MobilePhoneInfo>();
                    foreach (var pi in pInfos)
                    {
                        PhoneList.Add(pi);
                    }
                    info.Phones = PhoneList;
                    mobilePlanList.Add(info);
                }
            }
            catch (Exception ex)
            {
                GenerateLog(string.Format("VodafoneFetcher crawling exception, at {0}:\r\n{1}\r\n{2}",
                    DateTime.Now.ToString("yyyyMMdd hh:mm:ss"), ex.Message, ex.Source));
            }

            FinishCrawlingLog();
            return mobilePlanList;
        }

        private List<MobilePhoneInfo> GetPhonesData(string urlFormat)
        {
            List<MobilePhoneInfo> pInfos = new List<MobilePhoneInfo>();
            bool hasProducts = true;
            int pageIndex = 0;
            string url = string.Format(urlFormat, pageIndex);
            pageIndex++;

            while (hasProducts)
            {
                XbaiRequest req = new XbaiRequest(url);
                string json = req.Get();
                json = json.Replace("angular.callbacks._5(", "");
                json = json.Substring(0, json.Length - 2);
                Newtonsoft.Json.Linq.JObject jsonObj = (Newtonsoft.Json.Linq.JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(json);

                if (jsonObj["status"].Value<string>().Equals("success", StringComparison.InvariantCultureIgnoreCase))
                {
                    var products = jsonObj["childProducts"] as JArray;

                    foreach(var p in products)
                    {
                        string productUrl = p["mobileDetailsURI"].Value<string>();
                        var infos = new MobilePhoneInfo();

                        XbaiRequest productReq = new XbaiRequest(productUrl);
                        JQuery doc = new JQuery(productReq.Get(), url);
                        infos.PhoneName = doc.find(".productTitle h1").text().Trim();
                        infos.phoneURL = productUrl;
                        Uri imgAbsoluteUri = new Uri(new Uri("https://www.vodafone.co.nz/"), doc.find(".productPic1 img").attr("src"));
                        infos.PhoneImage = imgAbsoluteUri.ToString();
                        infos.PlanName = doc.find(".withPlanName").text().Trim();
                        infos.UpfrontPrice = doc.find(".price-container").text().Trim().Replace("\t", "").Replace("\n", "").ToDecimal();
                        infos.ContractTypeID = 3;

                        //infos.PhoneName = p["displayName"].Value<string>();
                        //infos.phoneURL = productUrl;

                        pInfos.Add(infos);
                    }

                    url = string.Format(urlFormat, pageIndex);
                    pageIndex++;
                }
                else
                {
                    hasProducts = false;
                }
            }

            if(pInfos.Count == 0)
            {
                LogEventArgs logEventArgs = new LogEventArgs(LogType.CrawlLog, "Telecom Phone not found!", "TelecomFetcher");
                GenerateLog(logEventArgs);
            }

            return pInfos;
        }
    }
}
