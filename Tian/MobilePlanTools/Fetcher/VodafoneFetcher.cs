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

namespace Fetcher
{
    public class VodafoneFetcher : BaseFetcher
    {
        public VodafoneFetcher()
            : base(3, "Vodafone")
        {
        }

        System.Net.CookieCollection cc = new System.Net.CookieCollection();

        public override List<MobilePlanInfo> GetMobilePlanInfoList()
        {
            StartCrawlingLog();
            List<MobilePlanInfo> mobilePlanList = new List<MobilePlanInfo>();
            try
            {
                string url = "http://www.vodafone.co.nz/shop/mobileListing.jsp?selectionKey=mobile&reset=true&categoryId=cat80064";
                StarlCrawlPhonesLinkLog(url);
               //获取手机数据列表
                var pInfos = new List<MobilePhoneInfo>();
                GetPhonesData(url, pInfos);

                //获取手机套餐
                var pager = GetParser("http://www.vodafone.co.nz/mobile-plans/other-on-account-plans/");
                var plan_top_tag = pager.ExtractAllNodesThatMatch(new HasAttributeFilter("id", "1375766535535"));
                var filter = new HasAttributeFilter("class", "plan-tbl__info");
                var plan_obj = plan_top_tag[0].Children.ExtractAllNodesThatMatch(filter,true).ToNodeArray();
                
                foreach (var plan in plan_obj) {
                    var PlanName = plan.Children.ExtractAllNodesThatMatch(new HasAttributeFilter("class", "plan-tbl__name__title"),true)[0].ToPlainTextString();
                    var Data = plan.Children.ExtractAllNodesThatMatch(new HasAttributeFilter("class", "plan-tbl__feature__title"), true);
                    var DataMB = Data[0].ToPlainTextString();
                    var talk = Data[1].ToPlainTextString();
                    talk = talk.Contains("Unlimited") ? "-1" : talk.Replace(" mins","");
                    var PlanPrice = plan.Children.ExtractAllNodesThatMatch(new HasAttributeFilter("class", "plan-tbl__price__sum"), true)[0].ToPlainTextString().Replace("$","");
                    var PlanUrl_obj = plan.Children.ExtractAllNodesThatMatch(new HasAttributeFilter("class", "plan-tbl__cta plan-tbl__cta--buy"), true)[0];

                    var PlanUrl = (PlanUrl_obj as ATag).Link;
                    var info = new MobilePlanInfo();
                    info.CarrierName = this.ProviderName;
                    info.DataMB = DataMB.Replace("\t", "").Replace("\n", "");
                    info.Minutes = int.Parse(talk);
                    info.MobilePlanName = PlanName.Replace("\t", "").Replace("\n", "");
                    info.MobilePlanURL = PlanUrl;
                    info.Price = int.Parse(PlanPrice);
                    info.Texts = -1;
                    info.plus = 0;

                    var PhoneList = new List<MobilePhoneInfo>();
                    foreach (var item in pInfos)
                    {
                        if (item.PhoneName.Contains("]") || item.PhoneName.Contains("["))
                        {
                            var plan_name = item.PhoneName.Substring(item.PhoneName.IndexOf('[') + 1, item.PhoneName.LastIndexOf(']') - 1);
                            if (info.MobilePlanName == plan_name)
                            {
                                item.PhoneName = item.PhoneName.Replace("[" + plan_name + "]", "");
                                PhoneList.Add(pInfos[pInfos.IndexOf(item)]);
                            }
                        }

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

        private void GetPhonesData(string url, List<MobilePhoneInfo> pInfos)
        {
            var nParser = GetParser(url, out cc, cc);
            var pro_obj = new HasAttributeFilter("id", "tab1content");
            var pro_obj_list = nParser.ExtractAllNodesThatMatch(pro_obj);
            
            if (pro_obj_list.Count > 0)
            {
                var pro_list = pro_obj_list[0].Children.ExtractAllNodesThatMatch(new HasAttributeFilter("class", "tripleMod"), true).ToNodeArray();
                foreach (var pro in pro_list)
                {
                    var div = pro as Div;
                    var img = div.Children.ExtractAllNodesThatMatch(new HasAttributeFilter("class", "prodImage"), true)[0];
                    var pro_img = (img.Children[1] as ImageTag).ImageURL;
                    var Name = div.Children.ExtractAllNodesThatMatch(new HasAttributeFilter("class", "productName"), true)[0].ToPlainTextString();
                    Name = Name.Trim().Replace("\r", "").Replace("\n", "");
                    var a_tag = div.Children[1] as ATag;
                    var Url = a_tag.Link;

                    var filter1=new HasAttributeFilter("class", "withPlanName displayPlan");
                    var filter2=new HasAttributeFilter("class", "withPlanName");
                    var filter3=new OrFilter(filter1,filter2);

                    var str_obj = div.Children.ExtractAllNodesThatMatch(filter3, true);
                    if (str_obj[0].ToPlainTextString().Trim().Contains("Mobile only")) continue;
                    var Plan = "";
                    var ConID = 0;
                    if (str_obj[0].ToPlainTextString().Contains(","))
                    {
                        var str_arr = str_obj[0].ToPlainTextString().Split(',');
                        Plan = str_arr[0].Trim();
                        ConID = str_arr[1].Contains("24") ? 3 : 2;
                    }
                    else if (str_obj[0].ToPlainTextString().Contains("-"))
                    {
                        var str_arr = str_obj[0].ToPlainTextString().Split('-');
                        Plan = str_arr[0].Trim();
                        ConID = str_arr[1].Contains("24") ? 3 : 2;
                    }

                    var Price = div.Children.ExtractAllNodesThatMatch(new HasAttributeFilter("class", "price1"), true)[0].ToPlainTextString();
                    Price = Price.Replace(" ", "").Replace("\r", "").Replace("\n", "").Replace("$", "");

                    var infos = new MobilePhoneInfo();
                    infos.ContractTypeID = ConID;
                    infos.PhoneName = "[" + Plan + "]" + Name;
                    infos.UpfrontPrice = int.Parse(Price);
                    infos.PhoneImage = pro_img;
                    infos.phoneURL = Url;
                    pInfos.Add(infos);
                }

                #region 分页
                var pager = pro_obj_list[0].Children.ExtractAllNodesThatMatch(new HasAttributeFilter("id", "paging"));
                if (pager.Count > 0)
                {
                    var page_div = pager[0].Children.ExtractAllNodesThatMatch(new HasAttributeFilter("class", "column2_2"))[0];
                    var paging_div = page_div as Div;
                    var next_page = paging_div.LastChild;
                    var a_next =next_page.PreviousSibling as ATag;
                    if (a_next != null)
                        GetPhonesData(a_next.Link, pInfos);
                }
                #endregion

            }
            else
            {
                LogEventArgs logEventArgs = new LogEventArgs(LogType.CrawlLog, "Telecom Phone not found!", "TelecomFetcher");
                GenerateLog(logEventArgs);
            }
        }

        

        #region GetHttpContent

        private string GetHttpContent(string url, out System.Net.CookieCollection outCC, System.Net.CookieCollection cc)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

                request.Method = "GET";
                request.UserAgent = "Mozilla/5.0 (Windows NT 6.1) AppleWebKit/535.19 (KHTML, like Gecko) Chrome/18.0.1025.168 Safari/535.19";
                request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
                request.Headers["Accept-Charset"] = "GBK,utf-8;q=0.7,*;q=0.3";
                request.Headers["Accept-Language"] = "zh-CN,zh;q=0.8";
                request.CookieContainer = new CookieContainer();
                System.Net.CookieCollection c = cc; 
                if (c.Count > 0)
                    request.CookieContainer.Add(c);
                request.Timeout = 1000000;

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                StreamReader streamReader = new StreamReader(response.GetResponseStream());

                c.Add(response.Cookies);
                outCC = c;

                string html = streamReader.ReadToEnd();
                return html;
            }
            catch (Exception ex)
            {
                LogEventArgs logEventArgs = new LogEventArgs(LogType.ExceptionLog, "url : " + url + "\n" + ex.Message, "GetHttpContent");
                GenerateLog(logEventArgs);
            }
            outCC = null;
            return "";
        }

        private Parser GetParser(string url, out CookieCollection outCC, CookieCollection cc)
        {
            string html = GetHttpContent(url, out outCC, cc);

            if (!string.IsNullOrEmpty(html))
            {
                Lexer lexer = new Lexer(html);

                Parser parser = new Parser(lexer);
                parser.URL = url;

                return parser;
            }

            return null;
        }

        #endregion

        #region 原来的代码
        private void GetPhones(Dictionary<string, List<MobilePhoneInfo>> dicPhones, string url)
        {
            #region get phones
            Parser parser = this.GetParser(url, out cc, cc);
            if (parser == null) return;
            #region
            try
            {
                parser.Reset();
                HasAttributeFilter hasAttributeFilter = new HasAttributeFilter("class", "parentMobileDetail");
                NodeList nodes = parser.ExtractAllNodesThatMatch(hasAttributeFilter);
                if (nodes == null || nodes.Count == 0)
                {
                    LogEventArgs logEventArgs = new LogEventArgs(LogType.CrawlLog, "Mobile phones not found!", "");
                    GenerateLog(logEventArgs);
                }

                NodeClassFilter aFilter = new NodeClassFilter(typeof(ATag));

                var contract = 1;
                for (int i = 0; i < nodes.Count; i++)
                {
                    try
                    {
                        ATag atag = nodes[i].Children[1].Children[1] as ATag;
                        var htm = nodes[i].ToHtml();

                        var phoneName = htm.Substring(htm.IndexOf("<h3>") + 4);
                        phoneName = phoneName.Substring(0, phoneName.IndexOf("</h3>")).Trim();

                        htm = htm.Substring(htm.IndexOf("price1\">") + 8);
                        var price = htm.Substring(0, htm.IndexOf("</span>")).Replace("$", "").Trim();

                        string plan = nodes[i].ToHtml();
                        var planIndex = plan.LastIndexOf("withPlanName displayPlan\">");
                        if (planIndex > 0)
                        {
                            plan = plan.Substring(planIndex + "withPlanName displayPlan\">".Length);
                            plan = plan.Substring(0, plan.IndexOf("</span>"));
                        }
                        else
                        {
                            planIndex = plan.LastIndexOf("withPlanName\">");
                            if (planIndex > 0)
                            {
                                plan = plan.Substring(planIndex + "withPlanName\">".Length);
                                plan = plan.Substring(0, plan.IndexOf("</span>"));
                                if (!plan.Contains("-")) continue;
                            }
                        }

                        string img = nodes[i].ToHtml();
                        img = img.Substring(img.IndexOf("prodImage\">") + "prodImage\">".Length);
                        img = img.Substring(img.IndexOf("src=\"") + "src=\"".Length);
                        img = "http://www.vodafone.co.nz" + img.Substring(0, img.IndexOf("\""));


                        if (plan.Contains('-'))
                        {
                            string[] plans = plan.Split('-');
                            var month = plans[1].Contains("24") ? ",24" : ",12";
                            plan = (plans[0]).Trim();
                            contract = plans[1].Contains("24") ? 3 : 2;
                        }
                        else if (plan.Contains(","))
                        {
                            string[] plans = plan.Split(',');
                            var month = plans[1].Contains("24") ? ",24" : ",12";
                            plan = (plans[0]).Trim();
                            contract = plans[1].Contains("24") ? 3 : 2;
                        }

                        MobilePhoneInfo info = new MobilePhoneInfo();
                        info.ContractTypeID = contract;
                        info.PhoneName = phoneName;
                        info.UpfrontPrice = Convert.ToDecimal(price);
                        info.PhoneImage = img;
                        info.phoneURL = atag.Link;


                        if (dicPhones.ContainsKey(plan))
                        {
                            var phones = dicPhones[plan];
                            phones.Add(info);
                            dicPhones[plan] = phones;
                        }
                        else
                        {
                            var phones = new List<MobilePhoneInfo>();
                            phones.Add(info);

                            dicPhones.Add(plan, phones);
                        }
                    }
                    catch (Exception exc)
                    {

                    }
                }


                parser.Reset();
                HasAttributeFilter nextAttributeFilter = new HasAttributeFilter("class", "pages");
                nodes = parser.ExtractAllNodesThatMatch(nextAttributeFilter);
                if (nodes != null && nodes.Count > 0)
                {
                    INode node = nodes[nodes.Count - 1];
                    if (node.Children.Count > 0)
                    {
                        ATag next = node.NextSibling.NextSibling as ATag;
                        if (next != null)
                            GetPhones(dicPhones, next.Link);
                    }
                }

            }
            catch (Exception ex)
            {

            }

            #endregion

            #endregion
        }

        #region http://www.vodafone.co.nz/red/

        private List<MobilePlanInfo> GetMobilePlanInfoList0(Dictionary<string, List<MobilePhoneInfo>> dicPhones)
        {
            List<MobilePlanInfo> MobilePlanInfoList = new List<MobilePlanInfo>();

            #region get plans

            string url = "http://www.vodafone.co.nz/red/";
            StarlCrawlPlansLinkLog(url);
            try
            {
                string html = GetHttpContent(url);

                if (string.IsNullOrEmpty(html))
                {
                    return MobilePlanInfoList;
                }

                #region plans

                #region plan products

                html = html.Substring(html.IndexOf("choiceConfiguration"));
                html = html.Substring(html.IndexOf("{"));
                html = html.Substring(0, html.IndexOf("</script>"));
                html = html.Substring(0, html.LastIndexOf("}") + 1);

                JsonReader reader = new JsonTextReader(new StringReader(html));
                MobilePlanInfo plan = null;
                string desc = "";
                string name = "";

                while (reader.Read())
                {
                    if (reader.Value == null) continue;
                    if (reader.Value.ToString() == "includedCharges")//plan
                    {
                        plan = new MobilePlanInfo();
                        plan.CarrierName = this.ProviderName;
                        plan.Texts = -1;
                        plan.Minutes = -1;
                    }
                    else if (reader.Value.ToString() == "description")
                    {
                        reader.Read();
                        desc = reader.Value.ToString();
                        reader.Read();
                        reader.Read();
                        name = reader.Value.ToString();
                        if (name.Contains("Extra"))
                        {
                            //desc = desc.Substring(desc.LastIndexOf("$") + 1);
                            //desc = desc.Substring(0, desc.IndexOf("per MB"));
                            //plan.DataRate = decimal.Parse(desc);
                            var DataRate_ = desc.Replace(" for $", "$");
                            DataRate_ = DataRate_.Split(' ').Where(p => p.Contains("$")).First().Replace("MB", "").Replace("GB", "");
                            var DataRates_ = DataRate_.Split('$');
                            decimal val = decimal.Parse(DataRates_[1]) / decimal.Parse(DataRates_[0]);
                        }
                    }
                    else if (reader.Value.ToString() == "price")
                    {
                        if (plan.Price > 0) continue;
                        reader.Read();
                        plan.Price = int.Parse(reader.Value.ToString());
                    }
                    else if (reader.Value.ToString() == "displayName")
                    {
                        if (!string.IsNullOrEmpty(plan.MobilePlanName)) continue;
                        reader.Read();
                        plan.MobilePlanName = reader.Value.ToString().Split('-')[0].Trim();
                    }
                    else if (reader.Value.ToString() == "url")
                    {
                        if (!string.IsNullOrEmpty(plan.MobilePlanURL)) continue;
                        reader.Read();
                        plan.MobilePlanURL = reader.Value.ToString();
                    }
                    else if (reader.Value.ToString() == "dataValue")
                    {
                        reader.Read();
                        plan.DataMB = reader.Value.ToString();
                        MobilePlanInfoList.Add(plan);
                    }
                    else if (reader.Value.ToString() == "dataUnit")
                    {
                        reader.Read();
                        plan.DataMB = plan.DataMB + reader.Value.ToString();
                    }
                }
                #endregion

                #region plan phones
                foreach (var info in MobilePlanInfoList)
                {
                    if (dicPhones.ContainsKey(info.MobilePlanName))
                    {
                        info.Phones = dicPhones[info.MobilePlanName];
                        foreach (var item in info.Phones)
                        {
                            if (item.UpfrontPrice > 0 && info.plus > 0)
                            {
                                item.UpfrontPrice -= info.plus;
                                item.UpfrontPrice = item.UpfrontPrice < 0 ? 0 : item.UpfrontPrice;
                            }
                        }
                    }
                    if (info.Phones == null || info.Phones.Count == 0)
                    {
                        #region
                        /*
                        info.Phones = new List<MobilePhoneInfo>();

                        MobilePhoneInfo phone = new MobilePhoneInfo();
                        phone.PhoneId = 0;
                        phone.PhoneName = string.Empty;
                        phone.UpfrontPrice = 0;
                        phone.ManufacturerID = 0;
                        phone.ManufacturerName = "";
                        phone.ContractTypeID = 1;
                        phone.PhoneProductId = 0;
                        info.Phones.Add(phone);

                        MobilePhoneInfo phone2 = new MobilePhoneInfo();
                        phone2.PhoneId = 0;
                        phone2.PhoneName = string.Empty;
                        phone2.UpfrontPrice = 0;
                        phone2.ManufacturerID = 0;
                        phone2.ManufacturerName = "";
                        phone2.ContractTypeID = 2;
                        phone2.PhoneProductId = 0;
                        info.Phones.Add(phone2);

                        MobilePhoneInfo phone3 = new MobilePhoneInfo();
                        phone3.PhoneId = 0;
                        phone3.PhoneName = string.Empty;
                        phone3.UpfrontPrice = 0;
                        phone3.ManufacturerID = 0;
                        phone3.ManufacturerName = "";
                        phone3.ContractTypeID = 3;
                        phone3.PhoneProductId = 0;
                        info.Phones.Add(phone3);
                        */
                        #endregion
                    }
                }
                #endregion

                #endregion
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                GenerateLog(string.Format("VodafoneFetcher crawling '" + url +
                    "' exception, at {0}:\r\n{1}\r\n{2}",
                    DateTime.Now.ToString("yyyyMMdd hh:mm:ss"), ex.Message, ex.Source));
            }

            #endregion

            if (MobilePlanInfoList.Count == 0)
            {
                LogEventArgs logEventArgs = new LogEventArgs(LogType.CrawlLog, "Mobile Plans not found!", "");
                GenerateLog(logEventArgs);
            }

            return MobilePlanInfoList;
        }

        #endregion

        #region http://www.vodafone.co.nz/mobile-plans/other-on-account-plans/

        private List<MobilePlanInfo> GetMobilePlanInfoList1(Dictionary<string, List<MobilePhoneInfo>> dicPhones)
        {
            List<MobilePlanInfo> MobilePlanInfoList = new List<MobilePlanInfo>();

            string url = "http://www.vodafone.co.nz/mobile-plans/other-on-account-plans/";
            Parser parser = this.GetParser(url);
            StarlCrawlPlansLinkLog(url);

            #region get plans

            try
            {
                #region plans
                parser.Reset();
                HasAttributeFilter hasAttributeFilter = new HasAttributeFilter("class", "planColumn");
                NodeList nodes = parser.ExtractAllNodesThatMatch(hasAttributeFilter);
                if (nodes == null || nodes.Count == 0)
                {
                    LogEventArgs logEventArgs = new LogEventArgs(LogType.CrawlLog, "Mobile Plans not found!", "");
                    GenerateLog(logEventArgs);
                    return null;
                }
                parser.Reset();
                hasAttributeFilter = new HasAttributeFilter("class", "tableStripes");
                NodeList otherChargeNodes = parser.ExtractAllNodesThatMatch(hasAttributeFilter);

                NodeClassFilter aFilter = new NodeClassFilter(typeof(ATag));

                for (int i = 0; i < nodes.Count; i++)
                {
                    Div div = nodes[i] as Div;

                    #region planInfo
                    var startIndex = 1;
                    if (div.ChildCount > 13) startIndex = 3;

                    var planName = div.Children[startIndex].Children[1].ToPlainTextString();
                    var data = div.Children[startIndex + 2].Children[1].Children[1].ToPlainTextString()
                        .Replace("data", "").Replace("\r\n", "").Replace(" ", "").Trim();
                    if (data.Contains("Unlimited")) data = "-1";
                    var mins = div.Children[startIndex + 2].Children[1].Children[3].Children[1].ToPlainTextString()
                        .Replace("talk", "").Replace("mins", "").Replace("&nbsp;", "").Replace("\r\n", "").Trim();
                    if (mins.Contains("Unlimited")) mins = "-1";
                    var txts = div.Children[startIndex + 2].Children[1].Children[3].Children[5].ToPlainTextString()
                        .Replace("TXTs", "").Replace("&nbsp;", "").Replace("\r\n", "").Trim();
                    if (txts.Contains("Unlimited")) txts = "-1";
                    var mthcost = div.Children[startIndex + 6].Children[1].ToPlainTextString()
                        .Replace("$", "").Replace("&nbsp;", "").Trim();

                    ATag link = div.Children[div.ChildCount - 2].Children[1] as ATag;
                    var pUri = link.Link.Replace("&amp;", "&");

                    MobilePlanInfo info = new MobilePlanInfo();
                    info.CarrierName = this.ProviderName;
                    info.DataMB = data;
                    info.Minutes = int.Parse(mins);
                    info.MobilePlanName = planName;
                    info.MobilePlanURL = url;
                    info.Price = int.Parse(mthcost);
                    info.Texts = int.Parse(txts);
                    info.plus = 0;

                    #region otherCharge

                    TableTag tbl = otherChargeNodes[i] as TableTag;
                    foreach (var row in tbl.Rows)
                    {
                        var header = row.Headers[0].ToPlainTextString();
                        if (header == "NZ calls")
                        {
                            var CallRate_ = row.Columns[0].ToPlainTextString();
                            CallRate_ = CallRate_.Substring(0, CallRate_.IndexOf("per")).Replace("$", "").Trim();
                            info.CallRate = Convert.ToDecimal(CallRate_);
                        }
                        else if (header == "PXTs")
                        {
                            var textCost = row.Columns[0].ToPlainTextString();
                            textCost = textCost.Substring(textCost.IndexOf(":") + 1);
                            textCost = textCost.Substring(0, textCost.IndexOf("per")).Trim();
                            if (textCost.Contains("c"))
                                info.TextCostPer = Convert.ToDecimal(textCost.Replace("c", "")) / 100;
                            else
                                info.TextCostPer = Convert.ToDecimal(textCost);
                        }
                        else if (header == "Extra Data")
                        {
                            var DataRate_ = row.Columns[0].ToPlainTextString().Replace(" for $", "$");
                            DataRate_ = DataRate_.Split(' ').Where(p => p.Contains("$")).First().Replace("MB", "").Replace("GB", "");
                            var DataRates_ = DataRate_.Split('$');
                            decimal val = decimal.Parse(DataRates_[1]) / decimal.Parse(DataRates_[0]);
                            info.DataRate = val;
                        }
                    }

                    #endregion

                    if (dicPhones.ContainsKey(info.MobilePlanName))
                    {
                        info.Phones = dicPhones[info.MobilePlanName];
                        foreach (var phone in info.Phones)
                        {
                            if (phone.UpfrontPrice > 0 && info.plus > 0)
                            {
                                phone.UpfrontPrice -= info.plus;
                                phone.UpfrontPrice = phone.UpfrontPrice < 0 ? 0 : phone.UpfrontPrice;
                            }
                        }
                    }
                    if (info.Phones == null || info.Phones.Count == 0)
                    {
                        #region
                        /*
                        info.Phones = new List<MobilePhoneInfo>();

                        MobilePhoneInfo phone = new MobilePhoneInfo();
                        phone.PhoneId = 0;
                        phone.PhoneName = string.Empty;
                        phone.UpfrontPrice = 0;
                        phone.ManufacturerID = 0;
                        phone.ManufacturerName = "";
                        phone.ContractTypeID = 1;
                        phone.PhoneProductId = 0;
                        info.Phones.Add(phone);

                        MobilePhoneInfo phone2 = new MobilePhoneInfo();
                        phone2.PhoneId = 0;
                        phone2.PhoneName = string.Empty;
                        phone2.UpfrontPrice = 0;
                        phone2.ManufacturerID = 0;
                        phone2.ManufacturerName = "";
                        phone2.ContractTypeID = 2;
                        phone2.PhoneProductId = 0;
                        info.Phones.Add(phone2);

                        MobilePhoneInfo phone3 = new MobilePhoneInfo();
                        phone3.PhoneId = 0;
                        phone3.PhoneName = string.Empty;
                        phone3.UpfrontPrice = 0;
                        phone3.ManufacturerID = 0;
                        phone3.ManufacturerName = "";
                        phone3.ContractTypeID = 3;
                        phone3.PhoneProductId = 0;
                        info.Phones.Add(phone3);
                        */
                        #endregion
                    }
                    #endregion

                    MobilePlanInfoList.Add(info);
                }
                #endregion
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                GenerateLog(string.Format("VodafoneFetcher crawling '" + url + "' exception, at {0}:\r\n{1}\r\n{2}",
                    DateTime.Now.ToString("yyyyMMdd hh:mm:ss"), ex.Message, ex.Source));
            }

            #endregion

            if (MobilePlanInfoList.Count == 0)
            {
                LogEventArgs logEventArgs = new LogEventArgs(LogType.CrawlLog, "Mobile Plans not found!", "");
                GenerateLog(logEventArgs);
            }

            return MobilePlanInfoList;
        }

        #endregion

        #endregion
    }
}
