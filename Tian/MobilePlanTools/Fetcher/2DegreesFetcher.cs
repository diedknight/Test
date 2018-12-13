using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using Common;
using Common.Data;
using Winista.Text.HtmlParser;
using Winista.Text.HtmlParser.Filters;
using Winista.Text.HtmlParser.Lex;
using Winista.Text.HtmlParser.Tags;
using Winista.Text.HtmlParser.Util;
using Pricealyser.Crawler.HtmlParser.Query;
using Newtonsoft.Json;
using Pricealyser.Crawler.Request;

namespace Fetcher
{
    public class PhoneEntity
    {
        public string itemATGName { get; set; }
        public string itemBrand { get; set; }
        public string itemDetailsPageUrl { get; set; }
        public string itemImageUrl { get; set; }
        public string itemRRPAmount { get; set; }
    }

    public class _2DegreesFetcher : BaseFetcher
    {
        public _2DegreesFetcher()
            : base(2, "2 Degrees")
        { }

        public override List<MobilePlanInfo> GetMobilePlanInfoList()
        {
            StartCrawlingLog();

            XbaiRequest req = new XbaiRequest();
            List<MobilePlanInfo> list = new List<MobilePlanInfo>();

            string planUrl = "https://www.2degreesmobile.co.nz/mobile/pay-monthly/";

            req.Uri = new Uri(planUrl);
            JQuery doc = new JQuery(req.Get(), planUrl);

            doc.find("#js-pack-list-218 .c-pack-list__item").each(item =>
            {
                var node = item.ToJQuery();
                MobilePlanInfo info = new MobilePlanInfo();
                info.CarrierName = this.ProviderName;
                info.DataMB = node.find(".c-inclusion__number").first().text().Trim() + " GB";
                var minStr = "Unlimited";
                if (node.find(".c-inclusion__number").length > 1)
                {
                    minStr = node.find(".c-inclusion__number").last().text().Trim();
                }
                info.Minutes = minStr.Contains("Unlimited") ? -1 : Convert.ToInt32(minStr.ToDecimal());
                info.MobilePlanName = "Pay monthly " + info.DataMB + " $" + node.find(".c-price-spot__dollars").text().Trim();
                info.MobilePlanURL = planUrl;
                info.Price = node.find(".c-price-spot__dollars").text().ToDecimal();
                info.Texts = -1;
                info.plus = 0;
                info.Phones = new List<MobilePhoneInfo>();

                list.Add(info);
            });

            //string productListUrl = "https://www.2degreesmobile.co.nz/shop?p_p_id=konakart_portlet_WAR_konakart_portlet&p_p_lifecycle=1&p_p_state=normal&p_p_mode=view&p_p_col_id=column-1&p_p_col_count=1&_konakart_portlet_WAR_konakart_portlet__spage=%2FSelectCat.do%3FcatId%3D21&_konakart_portlet_WAR_konakart_portlet_catId=21&_konakart_portlet_WAR_konakart_portlet__sorig=%2FbuildJSON.do";
            string phonesUrl = "https://www.2degreesmobile.co.nz/2d/rest/model/com/twodegree/actor/ProductListActor/ProductList?pageName=phone";


            //读取产品列表页，获取Cookie等信息
            req.Uri = new Uri(phonesUrl);
            string json = req.Get();

            //req.Uri = new Uri(phonesUrl);

            var jsonObj = JsonConvert.DeserializeObject(json) as Newtonsoft.Json.Linq.JObject;

            var productList = jsonObj["itemDetails"] as Newtonsoft.Json.Linq.JArray;

            for(int i = 0; i < productList.Count(); i++)
            {
                MobilePhoneInfo info = new MobilePhoneInfo();
                info.ContractTypeID = 3;
                info.PhoneName = productList[i]["itemATGName"].ToString();
                info.ManufacturerName = productList[i]["itemBrand"].ToString();
                info.phoneURL = productList[i]["itemDetailsPageUrl"].ToString();
                info.PhoneImage = productList[i]["itemImageUrl"].ToString();
                info.UpfrontPrice = productList[i]["itemRRPAmount"].ToString().ToDecimal();
                list.ForEach(item =>
                {
                    item.Phones.Add(info);
                });
            }

            //list.ForEach(item =>
            //{
            //    productList.ForEach(pro =>
            //    {
            //        pro.ToList().ForEach(phone =>
            //        {
            //            MobilePhoneInfo info = new MobilePhoneInfo();
            //            info.ContractTypeID = 3;
            //            //info.UpfrontPrice = phone.Value.upfrontAmount.ToDecimal();
            //            //info.phoneURL = productUrl + phone.Key;
            //            //info.PhoneName = phone.Value.name;
            //            //info.PhoneImage = new Uri(new Uri(rootUrl), phone.Value.image).ToString();

            //            item.Phones.Add(info);
            //        });
            //    });
            //});

            string planUrl2 = "https://www.2degreesmobile.co.nz/mobile/prepay/";

            req.Uri = new Uri(planUrl2);
            JQuery doc2 = new JQuery(req.Get(), planUrl2);

            doc2.find("#js-pack-list-192 .c-pack-list__item").each(item =>
            {
                var node = item.ToJQuery();
                MobilePlanInfo info = new MobilePlanInfo();
                info.CarrierName = this.ProviderName;
                info.DataMB = node.find(".c-inclusion__number").first().text().Trim() + " GB";
                var minStr = "Unlimited";
                if (node.find(".c-inclusion__number").length > 1)
                {
                    minStr = node.find(".c-inclusion__number").last().text().Trim();
                }
                info.Minutes = minStr.Contains("Unlimited") ? -1 : Convert.ToInt32(minStr.ToDecimal());
                info.MobilePlanName = "Prepay " + info.DataMB + " $" + node.find(".c-price-spot__dollars").text().Trim();
                info.MobilePlanURL = planUrl2;
                info.Price = node.find(".c-price-spot__dollars").text().ToDecimal();
                info.Texts = -1;
                info.plus = 0;
                info.Phones = new List<MobilePhoneInfo>();

                list.Add(info);
            });

            FinishCrawlingLog();
            return list;
        }

        //CookieCollection cookieColle;
        //string location;
        //string value;

        //public override List<MobilePlanInfo> GetMobilePlanInfoList()
        //{
        //    StartCrawlingLog();
        //    List<MobilePlanInfo> mobilePlanList = new List<MobilePlanInfo>();
        //    //return mobilePlanList;
        //    try
        //    {
        //        string url = "http://www.2degreesmobile.co.nz/paymonthly/plans";
        //        Parser parser = this.GetParser(url);
        //        StarlCrawlPlansLinkLog(url);

        //        var plan_obj = parser.ExtractAllNodesThatMatch(new HasAttributeFilter("class", "container-fixed"))[0];

        //        var plan_data = plan_obj.Children.SearchFor(typeof(Div), false).ToNodeArray();

        //        foreach (var plan in plan_data) {
        //            var Data = plan.Children.ExtractAllNodesThatMatch(new HasAttributeFilter("class", "second-row"), true);

        //            var str_arr = Data[0].ToPlainTextString().Split(' ');
        //            var min =Data[0].ToPlainTextString().Contains("Unlimited")?"-1": str_arr[5];
        //            var PlanName = plan.Children.ExtractAllNodesThatMatch(new HasAttributeFilter("class", "first-row"), true)[0].ToPlainTextString();
        //            var a_obj = plan.Children.ExtractAllNodesThatMatch(new HasAttributeFilter("class", "third-row"), true)[0];

        //            var PlanUrl = (a_obj.Children.SearchFor(typeof(ATag),false)[0] as ATag).Link;

        //            var info = new MobilePlanInfo();
        //            info.CarrierName = this.ProviderName;
        //            info.DataMB = str_arr[0];
        //            info.Minutes = int.Parse(min);
        //            info.MobilePlanName = PlanName;
        //            //info.MobilePlanURL = PlanUrl;
        //            info.MobilePlanURL = "http://www.2degreesmobile.co.nz/paymonthly/plans";
        //            info.Price = int.Parse(PlanName.Replace("$","").Trim());
        //            info.Texts = -1;
        //            info.plus = 0;

        //            var PhoneList = new List<MobilePhoneInfo>();
        //            foreach (var phone in GetProDataList())
        //            {
        //                var ph = new MobilePhoneInfo();
        //                ph.ContractTypeID = phone.ContractTypeID;
        //                ph.UpfrontPrice = phone.Price;
        //                ph.phoneURL = phone.ProUrl;
        //                ph.PhoneName = phone.Name;
        //                ph.PhoneImage = phone.ImageUrl;
        //                PhoneList.Add(ph);
        //            }
        //            info.Phones = PhoneList;
        //            mobilePlanList.Add(info);
        //        }
        //        //Dictionary<string, Dictionary<int, int>> bonus = GetMobilePlanBonus(parser);
        //        //Dictionary<string, Dictionary<string, string>> phoneDic = new Dictionary<string, Dictionary<string, string>>();
        //        //Dictionary<string, string> phoneImg = new Dictionary<string, string>();
        //        //Dictionary<string, string> phoneUrl = new Dictionary<string, string>();
        //        //GetMobilePlanPhoneMap(phoneDic, phoneImg, phoneUrl);

                

                
        //    }
        //    catch (Exception ex)
        //    {
        //        GenerateLog(string.Format("2DegreesFetcher crawling exception, at {0}:\r\n{1}\r\n{2}",
        //            DateTime.Now.ToString("yyyyMMdd hh:mm:ss"), ex.Message, ex.Source));
        //    }

        //    FinishCrawlingLog();
            
        //    return mobilePlanList;
        //}

        //public List<pro_list> GetProDataList()
        //{

        //    string pro_url = "https://www.2degreesmobile.co.nz/shop?p_p_id=konakart_portlet_WAR_konakart_portlet&p_p_lifecycle=1&p_p_state=normal&p_p_mode=view&p_p_col_id=column-1&p_p_col_count=1&_konakart_portlet_WAR_konakart_portlet__spage=%2FSelectProd.do&_konakart_portlet_WAR_konakart_portlet__sorig=%2FSelectCat.do%3FcatId%3D21&productId=";
        //    var pro_list = new List<pro_list>();
        //    #region 第一页
        //    pro_list.Add(new pro_list { Name = "iPhone 6 Plus 128GB Gold", ImageUrl = ImgUrl("888462040730"), ProUrl = pro_url + "530", Price = 95, ContractTypeID = 3 });
        //    pro_list.Add(new pro_list { Name = "iPhone 6 Plus 128GB Silver", ImageUrl = ImgUrl("888462040433"), ProUrl = pro_url + "529", Price = 95, ContractTypeID = 3 });
        //    pro_list.Add(new pro_list { Name = "iPhone 6 Plus 128GB Space Grey", ImageUrl = ImgUrl("888462040136"), ProUrl = pro_url + "531", Price = 95, ContractTypeID = 3 });
        //    pro_list.Add(new pro_list { Name = "iPhone 6 Plus 128GB Gold", ImageUrl = ImgUrl("888462040730"), ProUrl = pro_url + "521", Price = 88, ContractTypeID = 3 });
        //    pro_list.Add(new pro_list { Name = "iPhone 6 128GB Silver", ImageUrl = ImgUrl("888462063517"), ProUrl = pro_url + "520", Price = 88, ContractTypeID = 3 });
        //    pro_list.Add(new pro_list { Name = "iPhone 6 128GB Space Grey", ImageUrl = ImgUrl("888462063227"), ProUrl = pro_url + "522", Price = 88, ContractTypeID = 3 });
        //    pro_list.Add(new pro_list { Name = "iPhone 6 Plus 64GB Gold", ImageUrl = ImgUrl("888462041638"), ProUrl = pro_url + "527", Price = 88, ContractTypeID = 3 });
        //    pro_list.Add(new pro_list { Name = "iPhone 6 Plus 64GB Silver", ImageUrl = ImgUrl("888462041331"), ProUrl = pro_url + "526", Price = 88, ContractTypeID = 3 });
        //    pro_list.Add(new pro_list { Name = "iPhone 6 Plus 64GB Space Grey", ImageUrl = ImgUrl("888462041034"), ProUrl = pro_url + "528", Price = 88, ContractTypeID = 3 });
        //    pro_list.Add(new pro_list { Name = "Samsung Galaxy S6 Edge 32GB White", ImageUrl = ImgUrl("8806086773041"), ProUrl = pro_url + "597", Price = 84, ContractTypeID = 3 });
        //    pro_list.Add(new pro_list { Name = "iPhone 6 64GB Gold", ImageUrl = ImgUrl("888462064675"), ProUrl = pro_url + "518", Price = 80, ContractTypeID = 3 });
        //    #endregion
        //    #region 第二页
        //    pro_list.Add(new pro_list { Name = "iPhone 6 64GB Silver", ImageUrl = ImgUrl("888462064385"), ProUrl = pro_url + "517", Price = 80, ContractTypeID = 3 });
        //    pro_list.Add(new pro_list { Name = "iPhone 6 64GB Space Grey", ImageUrl = ImgUrl("888462064095"), ProUrl = pro_url + "519", Price = 80, ContractTypeID = 3 });
        //    pro_list.Add(new pro_list { Name = "iPhone 6 Plus 16GB Gold", ImageUrl = ImgUrl("888462039833"), ProUrl = pro_url + "524", Price = 80, ContractTypeID = 3 });
        //    pro_list.Add(new pro_list { Name = "iPhone 6 Plus 16GB Silver", ImageUrl = ImgUrl("888462039536"), ProUrl = pro_url + "523", Price = 80, ContractTypeID = 3 });
        //    pro_list.Add(new pro_list { Name = "iPhone 6 Plus 16GB Space Grey", ImageUrl = ImgUrl("888462039239"), ProUrl = pro_url + "525", Price = 80, ContractTypeID = 3 });
        //    pro_list.Add(new pro_list { Name = "Samsung Galaxy S6 64GB Black", ImageUrl = ImgUrl("8806086800105"), ProUrl = pro_url + "590", Price = 80, ContractTypeID = 3 });
        //    pro_list.Add(new pro_list { Name = "Samsung Galaxy S6 64GB White", ImageUrl = ImgUrl("8806086799775"), ProUrl = pro_url + "593", Price = 80, ContractTypeID = 3 });
        //    pro_list.Add(new pro_list { Name = "Motorola Nexus 6", ImageUrl = ImgUrl("9400022044944"), ProUrl = pro_url + "559", Price = 76, ContractTypeID = 3 });
        //    pro_list.Add(new pro_list { Name = "Samsung Galaxy Note4 Gold", ImageUrl = ImgUrl("9400006011047"), ProUrl = pro_url + "549", Price = 76, ContractTypeID = 3 });
        //    pro_list.Add(new pro_list { Name = "Samsung Galaxy Note4 Pink", ImageUrl = ImgUrl("9400006011016"), ProUrl = pro_url + "548", Price = 76, ContractTypeID = 3 });
        //    pro_list.Add(new pro_list { Name = "HTC One M9", ImageUrl = ImgUrl("9400022045170"), ProUrl = pro_url + "604", Price = 74, ContractTypeID = 3 });
        //    #endregion
        //    #region 第三页
        //    pro_list.Add(new pro_list { Name = "Samsung Galaxy S6 32GB Black", ImageUrl = ImgUrl("8806086786256"), ProUrl = pro_url + "586", Price = 74, ContractTypeID = 3 });
        //    pro_list.Add(new pro_list { Name = "Samsung Galaxy S6 32GB Gold", ImageUrl = ImgUrl("8806086738323"), ProUrl = pro_url + "588", Price = 74, ContractTypeID = 3 });
        //    pro_list.Add(new pro_list { Name = "Samsung Galaxy S6 32GB White", ImageUrl = ImgUrl("8806086799935"), ProUrl = pro_url + "589", Price = 74, ContractTypeID = 3 });
        //    pro_list.Add(new pro_list { Name = "iPhone 6 16GB Gold", ImageUrl = ImgUrl("888462062930"), ProUrl = pro_url + "515", Price = 73, ContractTypeID = 3 });
        //    pro_list.Add(new pro_list { Name = "iPhone 6 16GB Silver", ImageUrl = ImgUrl("888462062640"), ProUrl = pro_url + "514", Price = 73, ContractTypeID = 3 });
        //    pro_list.Add(new pro_list { Name = "iPhone 6 16GB Space Grey", ImageUrl = ImgUrl("888462062350"), ProUrl = pro_url + "516", Price = 73, ContractTypeID = 3 });
        //    pro_list.Add(new pro_list { Name = "iPhone 5s 32GB Gold", ImageUrl = ImgUrl("885909844999"), ProUrl = pro_url + "394", Price = 70, ContractTypeID = 3 });
        //    pro_list.Add(new pro_list { Name = "iPhone 5s 32GB Silver", ImageUrl = ImgUrl("885909844845"), ProUrl = pro_url + "395", Price = 70, ContractTypeID = 3 });
        //    pro_list.Add(new pro_list { Name = "iPhone 5s 32GB Space Grey", ImageUrl = ImgUrl("885909844692"), ProUrl = pro_url + "396", Price = 70, ContractTypeID = 3 });
        //    pro_list.Add(new pro_list { Name = "Motorola Moto X (2nd Gen) Black", ImageUrl = ImgUrl("9400022044432"), ProUrl = pro_url + "546", Price = 68, ContractTypeID = 3 });
        //    pro_list.Add(new pro_list { Name = "Motorola Moto X (2nd Gen) White", ImageUrl = ImgUrl("9400022044425"), ProUrl = pro_url + "547", Price = 68, ContractTypeID = 3 });
        //    pro_list.Add(new pro_list { Name = "iPhone 5s 16GB Gold", ImageUrl = ImgUrl("885909844531"), ProUrl = pro_url + "391", Price = 67, ContractTypeID = 3 });
        //    #endregion
        //    #region 第四页
        //    pro_list.Add(new pro_list { Name = "iPhone 5s 16GB Silver", ImageUrl = ImgUrl("885909844388"), ProUrl = pro_url + "392", Price = 67, ContractTypeID = 3 });
        //    pro_list.Add(new pro_list { Name = "iPhone 5s 16GB Space Grey", ImageUrl = ImgUrl("885909844234"), ProUrl = pro_url + "393", Price = 67, ContractTypeID = 3 });
        //    pro_list.Add(new pro_list { Name = "Samsung Galaxy S5 SM-900I Black", ImageUrl = ImgUrl("9400006010262"), ProUrl = pro_url + "449", Price = 66, ContractTypeID = 3 });
        //    pro_list.Add(new pro_list { Name = "Samsung Galaxy S5 SM-900I Blue", ImageUrl = ImgUrl("9400006010286"), ProUrl = pro_url + "470", Price = 66, ContractTypeID = 3 });
        //    pro_list.Add(new pro_list { Name = "Samsung Galaxy S5 SM-900I Gold", ImageUrl = ImgUrl("9400006010668"), ProUrl = pro_url + "471", Price = 66, ContractTypeID = 3 });
        //    pro_list.Add(new pro_list { Name = "Samsung Galaxy S5 SM-900I White", ImageUrl = ImgUrl("9400006010279"), ProUrl = pro_url + "450", Price = 66, ContractTypeID = 3 });
        //    pro_list.Add(new pro_list { Name = "Samsung Galaxy K Zoom", ImageUrl = ImgUrl("9400006010873"), ProUrl = pro_url + "477", Price = 62, ContractTypeID = 3 });
        //    pro_list.Add(new pro_list { Name = "Samsung Galaxy A5 Black", ImageUrl = ImgUrl("8806086609968"), ProUrl = pro_url + "576", Price = 57, ContractTypeID = 3 });
        //    pro_list.Add(new pro_list { Name = "Samsung Galaxy A5 Gold", ImageUrl = ImgUrl("8806086609647"), ProUrl = pro_url + "577", Price = 57, ContractTypeID = 3 });
        //    pro_list.Add(new pro_list { Name = "Samsung Galaxy A5 Silver", ImageUrl = ImgUrl("8806086610063"), ProUrl = pro_url + "578", Price = 57, ContractTypeID = 3 });
        //    pro_list.Add(new pro_list { Name = "Samsung Galaxy A5 White", ImageUrl = ImgUrl("8806086610131"), ProUrl = pro_url + "579", Price = 57, ContractTypeID = 3 });
        //    pro_list.Add(new pro_list { Name = "iPhone 5c 16GB Pink", ImageUrl = ImgUrl("885909837830"), ProUrl = pro_url + "383", Price = 54, ContractTypeID = 3 });
        //    #endregion
        //    #region 第五页
        //    pro_list.Add(new pro_list { Name = "iPhone 5c 8GB Blue", ImageUrl = ImgUrl("885909939664"), ProUrl = pro_url + "509", Price = 54, ContractTypeID = 3 });
        //    pro_list.Add(new pro_list { Name = "iPhone 5c 8GB Green", ImageUrl = ImgUrl("885909939671"), ProUrl = pro_url + "510", Price = 54, ContractTypeID = 3 });
        //    pro_list.Add(new pro_list { Name = "iPhone 5c 8GB White", ImageUrl = ImgUrl("885909939640"), ProUrl = pro_url + "512", Price = 54, ContractTypeID = 3 });
        //    pro_list.Add(new pro_list { Name = "iPhone 5c 8GB Yellow", ImageUrl = ImgUrl("885909939657"), ProUrl = pro_url + "513", Price = 54, ContractTypeID = 3 });
        //    pro_list.Add(new pro_list { Name = "HTC Desire 820", ImageUrl = ImgUrl("9400022045163"), ProUrl = pro_url + "610", Price = 53, ContractTypeID = 3 });
        //    pro_list.Add(new pro_list { Name = "Samsung Galaxy S5 mini Black", ImageUrl = ImgUrl("9400006010798"), ProUrl = pro_url + "494", Price = 53, ContractTypeID = 3 });
        //    pro_list.Add(new pro_list { Name = "Samsung Galaxy S5 mini White", ImageUrl = ImgUrl("9400006010804"), ProUrl = pro_url + "495", Price = 53, ContractTypeID = 3 });
        //    pro_list.Add(new pro_list { Name = "Samsung Galaxy A3 Black", ImageUrl = ImgUrl("8806086639491"), ProUrl = pro_url + "572", Price = 49, ContractTypeID = 3 });
        //    pro_list.Add(new pro_list { Name = "Samsung Galaxy A3 Gold", ImageUrl = ImgUrl("8806086639590"), ProUrl = pro_url + "573", Price = 49, ContractTypeID = 3 });
        //    pro_list.Add(new pro_list { Name = "Samsung Galaxy A3 White", ImageUrl = ImgUrl("8806086639415"), ProUrl = pro_url + "575", Price = 49, ContractTypeID = 3 });
        //    pro_list.Add(new pro_list { Name = "LG G3 Beat", ImageUrl = ImgUrl("9400022044678"), ProUrl = pro_url + "552", Price = 45, ContractTypeID = 3 });
        //    pro_list.Add(new pro_list { Name = "Samsung Galaxy S4 mini Black", ImageUrl = ImgUrl("9400006008207"), ProUrl = pro_url + "347", Price = 45, ContractTypeID = 3 });
        //    #endregion
        //    #region 第六页
        //    pro_list.Add(new pro_list { Name = "Samsung Galaxy S4 mini Red", ImageUrl = ImgUrl("9400006011061"), ProUrl = pro_url + "542", Price = 45, ContractTypeID = 3 });
        //    pro_list.Add(new pro_list { Name = "Samsung Galaxy S4 mini White", ImageUrl = ImgUrl("9400006011078"), ProUrl = pro_url + "540", Price = 45, ContractTypeID = 3 });
        //    pro_list.Add(new pro_list { Name = "Motorola Moto G (2nd Gen)", ImageUrl = ImgUrl("6947681519312"), ProUrl = pro_url + "532", Price = 42, ContractTypeID = 3 });
        //    pro_list.Add(new pro_list { Name = "HTC Desire 510", ImageUrl = ImgUrl("4718487657537"), ProUrl = pro_url + "535", Price = 41, ContractTypeID = 3 });
        //    pro_list.Add(new pro_list { Name = "Nokia Lumia 635", ImageUrl = ImgUrl("9400006010736"), ProUrl = pro_url + "472", Price = 39, ContractTypeID = 3 });
        //    pro_list.Add(new pro_list { Name = "Samsung Galaxy Core Prime White", ImageUrl = ImgUrl("8806086537810"), ProUrl = pro_url + "566", Price = 39, ContractTypeID = 3 });
        //    #endregion
        //    return pro_list;
        //}


        //string ImgUrl(string markID)
        //{
        //    string url = "https://www.2degreesmobile.co.nz/kk-images/images/" + markID + "/catalogue.jpg";
        //    return url;
        //}

       




        //#region 原来的代码

        //private MobilePlanMoreInfo GetMobilePlanMoreInfo(Parser parser)
        //{
        //    MobilePlanMoreInfo info = new MobilePlanMoreInfo();

        //    parser.Reset();
        //    RegexFilter rFilter = new RegexFilter("Other pricing stuff...");
        //    NodeList rNode = parser.ExtractAllNodesThatMatch(rFilter);
        //    if (rNode == null || rNode.Count == 0) return info;

        //    TableTag tbl = rNode[0].Parent.NextSibling as TableTag;
        //    TableRow[] tr = tbl.Rows;

        //    Regex regex = new Regex(@"(?<value>[\d+]*)c");
        //    Match mat = regex.Match(tr[0].Columns[1].ToPlainTextString());
        //    string minutes = mat.Groups["value"].Value;
        //    mat = regex.Match(tr[1].Columns[1].ToPlainTextString());
        //    string texts = mat.Groups["value"].Value;
        //    mat = regex.Match(tr[4].Columns[1].ToPlainTextString());
        //    string data = mat.Groups["value"].Value;

        //    info.CallRate = minutes;
        //    info.TextRate = texts;
        //    info.DataRate = data;

        //    return info;
        //}

        ///// <summary>
        ///// Get Plan Bonus
        ///// </summary>
        ///// <param name="parser"></param>
        ///// <returns></returns>
        //private Dictionary<string, Dictionary<int, int>> GetMobilePlanBonus(Parser parser)
        //{
        //    parser.Reset();
        //    RegexFilter rFilter = new RegexFilter("Get a Plan Bonus");
        //    NodeList rNode = parser.ExtractAllNodesThatMatch(rFilter);
        //    if (rNode == null || rNode.Count == 0) return null;

        //    TableTag tbl = rNode[0].Parent.NextSibling.NextSibling.NextSibling.NextSibling as TableTag;
        //    Dictionary<string, Dictionary<int, int>> dic = new Dictionary<string, Dictionary<int, int>>();
            
        //    foreach (TableRow row in tbl.Rows)
        //    {
        //        if (row.ColumnCount < 3) continue;

        //        var planName = row.Columns[0].ToPlainTextString().Trim();
        //        int bonus12 = int.Parse(row.Columns[1].ToPlainTextString().Replace("$", "").Trim());
        //        int bonus24 = int.Parse(row.Columns[2].ToPlainTextString().Replace("$", "").Trim());

        //        Dictionary<int, int> bonus = new Dictionary<int, int>();
        //        bonus.Add(12, bonus12);
        //        bonus.Add(24, bonus24);
        //        dic.Add(planName, bonus);
        //    }

        //    return dic;
        //}

        //private void GetMobilePlanPhoneMap(Dictionary<string, Dictionary<string, string>> phoneDic, Dictionary<string, string> phoneImg, Dictionary<string, string> phoneUrl)
        //{
        //    try
        //    {
        //        GetHttpContentHeard();

        //        var url_ = "https://www.2degreesmobile.co.nz/shop?p_p_id=konakart_portlet_WAR_konakart_portlet&p_p_lifecycle=0&p_p_state=exclusive&p_p_mode=view&p_p_col_id=column-1&p_p_col_count=1&_konakart_portlet_WAR_konakart_portlet__spage=%2FbuildJSON.do";
        //        string httpString = GetPostHttpContent(url_);
        //        StarlCrawlPhonesLinkLog(url_);
                 
        //        Regex reg = new Regex(@"{[\s\S]*?""prodId"":(?<id>[\d+]*),[\s\S]*?""image"":""(?<image>[\s\S]*?)"",[\s\S]*?""name"":""(?<name>[\s\S]*?)"",[\s\S]*?""specialPriceExTax"":""(?<price>[\s\S]*?)""}},");
        //        MatchCollection mths = reg.Matches(httpString);
        //        var phonePlanUrl = string.Empty;
        //        foreach (Match mth in mths)
        //        {
        //            string name = mth.Groups["name"].Value.Replace("&trade;", "").Trim();
        //            name = name.Replace("&nbsp;", " ");
        //            while (name.Contains("  "))
        //            {
        //                name = name.Replace("  ", " ");
        //            } name = name.Trim();
        //            string id = mth.Groups["id"].Value;
        //            string url = location;
        //            string price = mth.Groups["price"].Value;
        //            string image = "https://www.2degreesmobile.co.nz" + mth.Groups["image"].Value;
        //            if (price.ToUpper().Contains("FREE"))
        //                price = string.Empty;

        //            string html = GetPostHttpContent(url, id, price);
        //            Lexer lexer = new Lexer(html);
        //            Parser pParser = new Parser(lexer);
        //            pParser.Reset();

        //            //HasAttributeFilter hFilter = new HasAttributeFilter("id", "planGrids");
        //            //NodeList nodeList = pParser.ExtractAllNodesThatMatch(hFilter);
        //            //if (nodeList == null || nodeList.Count == 0) continue;

        //            //TableTag tbl = nodeList[0] as TableTag;
        //            //TableRow[] tr = tbl.Rows;

        //            Regex regexFilter = new Regex(@"javascript\:setPlanDetails\((?<value>[\d+]*),(?<aa>[\s\S]*?);");
        //            RegexFilter pReg = new RegexFilter("Select Plan");
        //            NodeList nodeList = pParser.ExtractAllNodesThatMatch(pReg);
        //            if (nodeList == null || nodeList.Count <= 3) continue;
        //            Dictionary<string, string> valueDic = new Dictionary<string, string>();

        //            for (int j = 1; j < nodeList.Count; j++)
        //            {
        //                Match mat = regexFilter.Match(nodeList[j].Parent.Parent.ToHtml());
        //                if (mat.Success)
        //                {
        //                    string value = mat.Groups["value"].Value;

        //                    pParser.Reset();
        //                    HasAttributeFilter nFilter = new HasAttributeFilter("id", "planName" + value);
        //                    NodeList nNode = pParser.ExtractAllNodesThatMatch(nFilter);
        //                    string planName = string.Empty;
        //                    if (nNode != null && nNode.Count > 0 && !string.IsNullOrEmpty(nNode[0].ToPlainTextString()))
        //                        planName = nNode[0].ToPlainTextString().ToLower();

        //                    pParser.Reset();
        //                    nFilter = new HasAttributeFilter("id", "planDesc" + value);
        //                    nNode = pParser.ExtractAllNodesThatMatch(nFilter);
        //                    if (nNode != null && nNode.Count > 0 && !string.IsNullOrEmpty(nNode[0].ToPlainTextString()))
        //                        planName += (" " + nNode[0].ToPlainTextString().ToLower());

        //                    pParser.Reset();
        //                    string code = value + 0;
        //                    HasAttributeFilter vFilter = new HasAttributeFilter("id", "termWisePrice" + code);
        //                    NodeList vNode = pParser.ExtractAllNodesThatMatch(vFilter);
        //                    string price0 = "0";
        //                    if (vNode != null && vNode.Count > 0 && !string.IsNullOrEmpty(vNode[0].ToPlainTextString()))
        //                        price0 = vNode[0].ToPlainTextString();

        //                    pParser.Reset();
        //                    code = value + 1;
        //                    vFilter = new HasAttributeFilter("id", "termWisePrice" + code);
        //                    vNode = pParser.ExtractAllNodesThatMatch(vFilter);
        //                    string price12 = "0";
        //                    if (vNode != null && vNode.Count > 0 && !string.IsNullOrEmpty(vNode[0].ToPlainTextString()))
        //                        price12 = vNode[0].ToPlainTextString();

        //                    pParser.Reset();
        //                    code = value + 2;
        //                    vFilter = new HasAttributeFilter("id", "termWisePrice" + code);
        //                    vNode = pParser.ExtractAllNodesThatMatch(vFilter);
        //                    string price24 = "0";
        //                    if (vNode != null && vNode.Count > 0 && !string.IsNullOrEmpty(vNode[0].ToPlainTextString()))
        //                        price24 = vNode[0].ToPlainTextString();

        //                    valueDic.Add(planName, price0 + "|" + price12 + "|" + price24);
        //                }
        //            }
        //            if (phoneDic.ContainsKey(name))
        //            {
        //                Dictionary<string, string> valueDic0 = phoneDic[name];
        //                foreach (string key in valueDic.Keys)
        //                {
        //                    if (!valueDic0.ContainsKey(key))
        //                        valueDic0.Add(key, valueDic[key]);
        //                }
        //                phoneDic[name] = valueDic0;
        //            }
        //            else
        //                phoneDic.Add(name, valueDic);

        //            if (!phoneImg.ContainsKey(name))
        //                phoneImg.Add(name, image);
        //            phonePlanUrl = url + "&productId=" + id;
        //            if (!phoneUrl.ContainsKey(name))
        //                phoneUrl.Add(name, phonePlanUrl);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //        GenerateLog(string.Format("2DegreesFetcher GetMobilePlanPhoneMap exception, at {0}:\r\n{1}\r\n{2}",
        //            DateTime.Now.ToString("yyyyMMdd hh:mm:ss"), ex.Message, ex.Source));
        //    }   
        //}

        //private void GetHttpContentHeard()
        //{
        //    try
        //    {

        //        HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://www.2degreesmobile.co.nz/shop?p_p_id=konakart_portlet_WAR_konakart_portlet&p_p_lifecycle=1&p_p_state=normal&p_p_mode=view&p_p_col_id=column-1&p_p_col_count=1&_konakart_portlet_WAR_konakart_portlet__spage=%2FSelectCat.do%3FcatId%3D21&_konakart_portlet_WAR_konakart_portlet_catId=21");
        //        request.Method = "GET";
        //        request.UserAgent = "Mozilla/5.0 (Windows; U; Windows NT 5.2; zh-CN; rv:1.8.1.6) Gecko/20070725 Firefox/2.0.0.6";
        //        request.ContentType = "application/x-www-form-urlencoded";
        //        request.CookieContainer = new CookieContainer();
        //        request.KeepAlive = false;
        //        request.ProtocolVersion = HttpVersion.Version10;
        //        HttpWebResponse response1 = (HttpWebResponse)request.GetResponse();
        //        if (cookieColle == null || cookieColle.Count == 0)
        //            cookieColle = response1.Cookies;
        //        StreamReader stIn1 = new StreamReader(response1.GetResponseStream(), System.Text.Encoding.GetEncoding("gb2312"));                
        //        string strResponse1 = stIn1.ReadToEnd();
        //        Lexer lexer = new Lexer(strResponse1);
        //        Parser parser = new Parser(lexer);
        //        parser.Reset();

        //        HasAttributeFilter fromFilter = new HasAttributeFilter("id", "ShowProductDetailsForm");
        //        NodeList fromNode = parser.ExtractAllNodesThatMatch(fromFilter);
        //        FormTag fTag = fromNode[0] as FormTag;
        //        location = fTag.FormLocation;

        //        NodeClassFilter inFilter = new NodeClassFilter(typeof(InputTag));
        //        NodeList inNode = fTag.Children.ExtractAllNodesThatMatch(inFilter, true);
        //        InputTag inTag = inNode[0] as InputTag;
        //        value = inTag.Attributes["VALUE"].ToString();
        //    }
        //    catch (Exception ex) { }
        //}

        //private string GetPostHttpContent(string url)
        //{
        //    try
        //    {
        //        string strResponse;

        //        HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
        //        req.Method = "POST";
        //        req.ContentType = "application/x-www-form-urlencoded";
        //        req.CookieContainer = new CookieContainer();
        //        if (cookieColle != null && cookieColle.Count > 0)
        //            req.CookieContainer.Add(cookieColle);
                
        //        // Do the request to get the response 
        //        HttpWebResponse response = (HttpWebResponse)req.GetResponse();
        //        StreamReader stIn = new StreamReader(response.GetResponseStream(), System.Text.Encoding.GetEncoding("gb2312"));
        //        strResponse = stIn.ReadToEnd();

        //        return strResponse;
        //    }
        //    catch (Exception ex) { }
        //    return "";
        //}

        //private string GetPostHttpContent(string url, string id, string price)
        //{
        //    try
        //    {
        //        string strResponse;
        //        string strNewValue = "org.apache.struts.taglib.html.TOKEN=" + value + "&productId=" + id + "&productSpecialPrice=" + price;

        //        HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
        //        req.Method = "POST";
        //        req.ContentType = "application/x-www-form-urlencoded";
        //        req.ContentLength = strNewValue.Length;
        //        req.CookieContainer = new CookieContainer();
        //        if (cookieColle != null && cookieColle.Count > 0)
        //            req.CookieContainer.Add(cookieColle);
        //        req.KeepAlive = false;
        //        req.ProtocolVersion = HttpVersion.Version10;

        //        // Write the request 
        //        StreamWriter stOut = new StreamWriter(req.GetRequestStream(), System.Text.Encoding.GetEncoding("gb2312"));
        //        stOut.Write(strNewValue);
        //        stOut.Close();

        //        // Do the request to get the response 
        //        HttpWebResponse response = (HttpWebResponse)req.GetResponse();
        //        StreamReader stIn = new StreamReader(response.GetResponseStream(), System.Text.Encoding.GetEncoding("gb2312"));
        //        strResponse = stIn.ReadToEnd();

        //        return strResponse;
        //    }
        //    catch (Exception ex) { }
        //    return "";
        //}
        //#endregion
    }

    //public class pro_list
    //{
    //    public string Name { get; set; }
    //    public string Plan { get; set; }

    //    public int Price { get; set; }

    //    public int ContractTypeID { get; set; }

    //    public string ImageUrl { get; set; }
    //    public string ProUrl { get; set; }
    //}
}
