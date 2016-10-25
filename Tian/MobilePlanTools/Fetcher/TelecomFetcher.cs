using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Data;
using Winista.Text.HtmlParser;
using Winista.Text.HtmlParser.Filters;
using Winista.Text.HtmlParser.Util;
using Winista.Text.HtmlParser.Tags;
using Common;
using System.Text.RegularExpressions;
using Pricealyser.Crawler.HtmlParser.Query;

namespace Fetcher
{
    public class TelecomFetcher : BaseFetcher
    {
        public TelecomFetcher()
            : base(1, "Telecom")
        {
        }

        public override List<MobilePlanInfo> GetMobilePlanInfoList()
        {
            StartCrawlingLog();
            List<MobilePlanInfo> mobilePlanList = new List<MobilePlanInfo>();

            string url = "http://www.spark.co.nz/shop/mobile/mobile/plansandpricing.html";
            string phone_url = "http://www.spark.co.nz/shop/mobile/phones.html";
            GetData(mobilePlanList, url, phone_url, true);

            //url = "http://www.spark.co.nz/shop/mobile/mobilebroadband/plansandpricing.html";
            //phone_url = "http://www.spark.co.nz/shop/mobile/mobilebroadbandandtablets/devices.html";
            //GetData(mobilePlanList, url, phone_url, false);


            #region PhonePlan
            //var parser = this.GetParser(url);
            //var plan_filter = new HasAttributeFilter("id", "PricingTabContent");
            //var plan_obj = parser.ExtractAllNodesThatMatch(plan_filter);
            //var nodelists = plan_obj[0].Children.SearchFor(typeof(Div), false);
            //NodeList plan_list;
            
            //foreach (Div bl in nodelists.ToNodeArray())
            //{
            //    plan_list = bl.Children.ExtractAllNodesThatMatch(new HasAttributeFilter("class", "clearfix device-carousel-card"), true);

            //    foreach (var plan in plan_list.ToNodeArray())
            //    {
            //        if (plan.Children[5].Children[1].Children[1].ToPlainTextString().Contains("29")) continue;
            //        TableColumn data;
            //        if (plan.Children[11].Children == null)
            //        {
            //            data = null;
            //        }
            //        else if (plan.Children[11].Children[4] == null)
            //        {
            //            data = plan.Children[14].Children[5].Children[3] as TableColumn;
            //        }
            //        else if (plan.Children[11].Children[4].Children[5].Children == null)
            //        {
            //            data =null;
            //        }
            //        else if(plan.Children[11].Children[4].Children[5].Children[3] != null)
            //        {
            //            data = plan.Children[11].Children[4].Children[5].Children[3] as TableColumn;
            //        }
            //        else
            //        {
            //            data = plan.Children[11].Children[4].Children[5].Children[3] as TableColumn;
            //        }

            //        //var data = plan.Children.ExtractAllNodesThatMatch(new HasAttributeFilter("class", "data"), true);
            //        var DataMB = data != null ? data.ToPlainTextString() : "-1GB";
            //        var p_name = plan.Children[5].Children[1].Children[1] as Span;
            //        var text = "";
            //        if (plan.Children[11].Children == null)
            //        {
            //            text = plan.Children[13].Children[5].Children[3].ToPlainTextString().Trim();
            //        }
            //        else if (plan.Children[11].Children[4] == null)
            //        {
            //            text = plan.Children[14].Children[3].Children[3].Children[1].ToPlainTextString().Trim();
            //        }
                    
            //        else text = plan.Children[11].Children[4].Children[3].Children[3].Children[1].ToPlainTextString();
            //        if (!text.Contains("text") && !text.Contains("Unlimited"))
            //        {
            //            text = plan.Children[11].Children[4].Children[3].Children[3].ToPlainTextString();
            //        }
            //        text = text.Contains("Unlimited") ? "-1" : text.Split(' ')[0];
            //        var talk = "";
            //        if (plan.Children[11].Children == null)
            //        {
            //            talk = plan.Children[13].Children[3].Children[3].ToPlainTextString();
            //        }
            //        else if (plan.Children[11].Children[4] == null)
            //        {
            //            talk = plan.Children[14].Children[1].Children[3].ToPlainTextString();
            //        }
                    
            //        else talk = plan.Children[11].Children[4].Children[1].Children[3].ToPlainTextString();
            //        talk = talk.Contains("Unlimited") ? "-1" : talk.Split(' ')[0];

            //        var PlanName = p_name.ToPlainTextString();
            //        var PlanUrl = "";
            //        if (plan.Children[11].Children == null)
            //        {
            //            PlanUrl = (plan.Children[7] as ATag).Link;

            //        }

            //         else PlanUrl = (plan.Children[11].Children[0] as ATag).Link;

            //        var info = new MobilePlanInfo();
            //        info.CarrierName = this.ProviderName;
            //        info.DataMB = DataMB.Replace("\t", "").Replace("\n", "");
            //        info.Minutes = int.Parse(talk);
            //        info.MobilePlanName = "$" + PlanName.Replace("\t", "").Replace("\n", "");
            //        info.MobilePlanURL = PlanUrl;
            //        info.Price = int.Parse(PlanName);
            //        info.Texts = int.Parse(text);
            //        info.plus = 0;
            //        var PhoneList = new List<MobilePhoneInfo>();
            //        foreach (var item in pInfos)
            //        {
            //            if (item.PhoneName.Contains("]") || item.PhoneName.Contains("["))
            //            {
            //                var plan_name = item.PhoneName.Substring(item.PhoneName.IndexOf('[') + 1, item.PhoneName.LastIndexOf(']') - 1);
            //                if (info.MobilePlanName == plan_name)
            //                {
            //                    item.PhoneName = item.PhoneName.Replace("[" + plan_name + "]", "");
            //                    PhoneList.Add(pInfos[pInfos.IndexOf(item)]);
            //                }
            //            }

            //        }
            //        info.Phones = PhoneList;
            //        mobilePlanList.Add(info);
            //    }
            //}
           // var plan_list = plan_obj[0].Children.ExtractAllNodesThatMatch(new HasAttributeFilter("class", "clearfix device-carousel-card"), true).ToNodeArray();

            
            #endregion

            FinishCrawlingLog();

            return mobilePlanList;
        }

        private void GetData(List<MobilePlanInfo> mobilePlanList, string url, string phone_url, bool isPlan)
        {
            StarlCrawlPlansLinkLog(url);
            #region Phone
            Parser nParser = this.GetParser(phone_url);
            var pro_obj = new HasAttributeFilter("class", "device_listing");
            var pro_obj_list = nParser.ExtractAllNodesThatMatch(pro_obj);

            var pInfos = new List<MobilePhoneInfo>();
            if (pro_obj_list.Count > 0)
            {
                var pro_list = pro_obj_list[0].Children[1].Children.SearchFor(typeof(Bullet), false).ToNodeArray();
                foreach (var pro in pro_list)
                {
                    try
                    {
                        var li = pro as Bullet;
                        if (li.Children.Count < 4 || li.GetAttribute("id") == "SIMcard005" || li.GetAttribute("id") == "SIMcard006") continue;
                        var img = li.Children.SearchFor(typeof(Bullet), true).ToNodeArray();
                        var pro_img = (img[0].Children[0] as ImageTag).ImageURL;

                        var div = img[1].Children.SearchFor(typeof(Div), false).ToNodeArray();
                        var Name = div[0].ToPlainTextString() + " " + div[1].ToPlainTextString().Trim().Replace("\t", "").Replace("\r", "").Replace("\n", "");
                        var str_obj = div[4].ToPlainTextString().Replace(" ", "").Replace("\t", "").Replace("\r", "").Replace("\n", "");
                        var str_arr = str_obj.Split('$');
                        //if (Name.Contains("Galaxy A5 Pearl White")) {
                        //    var dsad = "";
                        //}
                        decimal price = 0m;
                        decimal.TryParse(str_arr[1].Replace("RRP", "").Replace("UPFRONT", "").Replace("Plus", ""), out price);
                        if (price == 0m)
                            price = str_arr[1].Contains("RRP") ? int.Parse(str_arr[1].Substring(0, str_arr[1].IndexOf('R'))) : int.Parse(str_arr[1].Contains("UPFRONT") ? str_arr[1].Substring(0, str_arr[1].IndexOf('U')) : str_arr[1].Substring(0, str_arr[1].IndexOf('O')));

                        var ConID = str_arr[1].Contains("24") ? 3 : 2;
                        var Url = (li.Children[3] as ATag).Link;

                        var Plan = "";
                        if (str_arr.Length > 2) Plan = "$" + str_arr[2];

                        var infos = new MobilePhoneInfo();
                        infos.ContractTypeID = ConID;
                        infos.PhoneName = "[" + Plan + "]" + Name;
                        infos.UpfrontPrice = price;
                        infos.PhoneImage = pro_img;
                        infos.phoneURL = Url;
                        pInfos.Add(infos);
                    }
                    catch { }
                }

            }
            else
            {
                LogEventArgs logEventArgs = new LogEventArgs(LogType.CrawlLog, "Telecom Phone not found!", "TelecomFetcher");
                GenerateLog(logEventArgs);
            }
            #endregion

            GetMobilePlan(mobilePlanList, pInfos, url);

            url = "http://www.spark.co.nz/shop/mobile/prepaid.html";
            GetMobilePlan(mobilePlanList, pInfos, url);
        }

        private void GetMobilePlan(List<MobilePlanInfo> mobilePlanList, List<MobilePhoneInfo> pInfos, string url)
        {
            JQuery doc = new JQuery(this.GetHttpContent(url), url);
            doc.find(".clearfix.device-carousel-card").each(item =>
            {
                var node = item.ToJQuery();

                string min = node.find(".detail-table .right").eq(1).val().Replace("mins to any NZ phone", "").Replace("to any NZ/Aus phone", "").Replace("NZ only", "");
                string text = node.find(".detail-table .right").eq(2).val().Replace("to any NZ network", "").Replace("to any NZ/Aus network", "").Replace("texts", "");
                string dataMB = node.find(".detail-table .first-cell .right").text().Trim();

                dataMB = dataMB.Replace("<span class=\"data\"> + 500MB</span>", "");
                dataMB = dataMB.Replace("<span class=\"data\"> + 250MB</span>", "");
                dataMB = dataMB.Replace("<span class=\"data\">", "");
                dataMB = dataMB.Replace("</span>", "");

                if (dataMB == "")
                {
                    dataMB = "0";
                }
                else
                {
                    if (dataMB.Contains("+"))
                    {
                        var strList = dataMB.Split('+');
                        dataMB = (Convert.ToInt32(Convert.ToDecimal(strList[0].Replace("GB", "").Trim()) * 1024)).ToString();

                        if (strList[1].Contains("GB^"))
                        {
                            dataMB = (Convert.ToInt32(dataMB) + Convert.ToInt32(Convert.ToDecimal(strList[1].Replace("GB^", "").Trim()) * 1024)).ToString();
                        }
                        else
                        {
                            dataMB = (Convert.ToInt32(dataMB) + Convert.ToDecimal(strList[1].Replace("MB", "").Trim())).ToString();
                        }

                        dataMB += "MB";
                    }
                }

                var info = new MobilePlanInfo();
                info.CarrierName = this.ProviderName;
                info.DataMB = dataMB;
                info.Minutes = min.Contains("Unlimited") ? -1 : Convert.ToInt32(min.ToDecimal());
                info.MobilePlanName = "$" + node.find(".service-price").text().Trim().Replace("\r\n", "").Replace("\t", "").Replace("/MTH", "");
                info.MobilePlanURL = url;
                info.Price = node.find(".service-price .number").text().ToDecimal();
                info.Texts = text.Contains("Unlimited") ? -1 : Convert.ToInt32(text.ToDecimal());
                info.plus = 0;
                var PhoneList = new List<MobilePhoneInfo>();
                foreach (var p in pInfos)
                {
                    if (p.PhoneName.Contains("]") || p.PhoneName.Contains("["))
                    {
                        var machname = p.PhoneName.Substring(p.PhoneName.IndexOf('[') + 1, p.PhoneName.LastIndexOf(']') - 1);
                        string plan_name = machname.Replace("OpenTerm", "").Replace("Plus", "").Replace("Value Pack", "").Trim();
                        if (info.MobilePlanName == plan_name)
                        {
                            p.PhoneName = p.PhoneName.Replace("[" + machname + "]", "");
                            PhoneList.Add(pInfos[pInfos.IndexOf(p)]);
                        }
                    }
                }
                info.Phones = PhoneList;

                mobilePlanList.Add(info);
            });
        }
    }
}
