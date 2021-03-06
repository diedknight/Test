﻿using System;
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
using Pricealyser.Crawler.Request;
using System.Net;

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
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;

            StartCrawlingLog();
            List<MobilePlanInfo> mobilePlanList = new List<MobilePlanInfo>();

            string url = "https://www.spark.co.nz/shop/mobile-plans/paymonthly.html";
            //string phone_url = "https://www.spark.co.nz/shop/mobile/phones.html";
            GetData(mobilePlanList, url, true);

            FinishCrawlingLog();

            return mobilePlanList;
        }

        private void GetData(List<MobilePlanInfo> mobilePlanList, string url, bool isPlan)
        {
            StarlCrawlPlansLinkLog(url);

            #region Phone
            //var pInfos = new List<MobilePhoneInfo>();
            //XbaiRequest req = new XbaiRequest(phone_url);
            //JQuery doc = new JQuery(req.Get(), phone_url);

            //doc.find("#entry > .container-fluid > *").each(item =>
            //{
            //    var node = item.ToJQuery();
            //    if (node.get(0).OuterHtml.StartsWith("<spark-device-card"))
            //    {
            //        var infos = new MobilePhoneInfo();
            //        //infos.ContractTypeID = node.attr("prop-id");
            //        infos.PhoneName = node.attr("prop-brandName") + " " + node.attr("prop-name");
            //        infos.UpfrontPrice = node.attr("prop-deferredPrice").ToDecimal();
            //        infos.PhoneImage = node.attr("prop-productImage");
            //        Uri imageAbsoluteUri = new Uri(new Uri("https://www.spark.co.nz/"), infos.PhoneImage);
            //        infos.PhoneImage = imageAbsoluteUri.ToString();
            //        infos.phoneURL = node.attr("prop-cta");
            //        Uri productAbsoluteUri = new Uri(new Uri("https://www.spark.co.nz/"), infos.phoneURL);
            //        infos.phoneURL = productAbsoluteUri.ToString();
            //        infos.PlanName = node.attr("prop-listedPlanName");
            //        pInfos.Add(infos);
            //    }
            //});


            //if(pInfos.Count == 0)
            //{
            //    LogEventArgs logEventArgs = new LogEventArgs(LogType.CrawlLog, "Telecom Phone not found!", "TelecomFetcher");
            //    GenerateLog(logEventArgs);
            //}
            #endregion

            GetMobilePlan(mobilePlanList, url);

            //url = "https://www.spark.co.nz/shop/mobile-plans/prepaid.html";
            //GetPrepaidMobilePlan(mobilePlanList, url);
        }

        private void GetPrepaidMobilePlan(List<MobilePlanInfo> mobilePlanList, string url)
        {
            string phonesUrlBase = "https://www.spark.co.nz/shop/mobile/phones.html?contr=phones&tab=-1&source=SOPO_ServicePlan&servicePlanId=";

            JQuery doc = new JQuery(this.GetHttpContent(url), url);
            doc.find(".device-carousel-card").each(item =>
            {
                var node = item.ToJQuery();

                string min = node.find(".detail-table .right").eq(1).val().Replace("mins to any NZ phone", "").Replace("to any NZ/Aus phone", "").Replace("NZ only", "").Replace("NZ Rollover Mins*", "");
                string text = node.find(".detail-table .right").eq(2).val().Replace("to any NZ network", "").Replace("to any NZ/Aus network", "").Replace("texts", "");
                string dataMB = node.find(".detail-table .first-cell .right").text().Trim();
                string nameMB = "";
                dataMB = dataMB.Replace("<span class=\"data\"> + 500MB</span>", "");
                dataMB = dataMB.Replace("<span class=\"data\"> + 250MB</span>", "");
                dataMB = dataMB.Replace("<span class=\"data\">", "");
                dataMB = dataMB.Replace("</span>", "");
                dataMB = dataMB.Replace("Rollover Data", "").Replace("Bonus Data*", "").Replace("Rollover", "").Replace("Socialiser*", "").Replace("*", "").Trim();

                if (dataMB == "")
                {
                    dataMB = "0";
                }
                else
                {
                    if (dataMB.Contains("+"))
                    {
                        var strList = dataMB.Split('+');

                        dataMB = (Convert.ToDecimal(strList[0].Replace("GB", "").Trim()) + Convert.ToDecimal(strList[1].Replace("GB", "").Trim())).ToString("0.0");

                        dataMB += "GB";

                        nameMB = strList[0].Trim();
                    }
                    else
                    {
                        nameMB = dataMB;
                    }
                }

                var info = new MobilePlanInfo();
                info.CarrierName = this.ProviderName;
                info.DataMB = dataMB;
                info.Minutes = min.Contains("Unlimited") ? -1 : Convert.ToInt32(min.ToDecimal());
                info.MobilePlanName = "$" + node.find(".service-price .number").text().Trim().Replace("\r\n", "").Replace("\t", "").Replace("/MTH", "");

                info.MobilePlanURL = url;
                info.Price = node.find(".service-price .number").text().ToDecimal();
                info.Texts = text.Contains("Unlimited") ? -1 : Convert.ToInt32(text.ToDecimal());
                info.plus = 0;

                info.MobilePlanName = "Prepaid " + dataMB;

                var PhoneList = new List<MobilePhoneInfo>();


                string phonesUrlFlag = node.find(".hightlight-text.view-mobi-add").attr("data-serviceplanid");
                string phonesUrl = phonesUrlBase + phonesUrlFlag;
                JQuery phonesDoc = new JQuery(this.GetHttpContent(phonesUrl), phonesUrl);

                List<MobilePhoneInfo> pInfos = new List<MobilePhoneInfo>();
                phonesDoc.find("#entry > .container-fluid > *").each(phonesItem =>
                {
                    var phoneNode = phonesItem.ToJQuery();
                    if (phoneNode.get(0).OuterHtml.StartsWith("<spark-device-card"))
                    {
                        var mpInfo = new MobilePhoneInfo();
                        //infos.ContractTypeID = node.attr("prop-id");
                        mpInfo.PhoneName = phoneNode.attr("prop-brandName") + " " + phoneNode.attr("prop-name");
                        mpInfo.UpfrontPrice = phoneNode.attr("prop-deferredPrice").ToDecimal();
                        mpInfo.PhoneImage = phoneNode.attr("prop-productImage");
                        Uri imageAbsoluteUri = new Uri(new Uri("https://www.spark.co.nz/"), mpInfo.PhoneImage);
                        mpInfo.PhoneImage = imageAbsoluteUri.ToString();
                        mpInfo.phoneURL = phoneNode.attr("prop-cta");
                        Uri productAbsoluteUri = new Uri(new Uri("https://www.spark.co.nz/"), mpInfo.phoneURL);
                        mpInfo.phoneURL = productAbsoluteUri.ToString();
                        mpInfo.PlanName = phoneNode.attr("prop-listedPlanName");
                        mpInfo.ContractTypeID = 1;
                        pInfos.Add(mpInfo);
                    }
                });
                info.Phones = pInfos;

                mobilePlanList.Add(info);
            });
        }

        private void GetMobilePlan(List<MobilePlanInfo> mobilePlanList, string url)
        {
            string phonesUrlBase = "https://www.spark.co.nz/shop/mobile/phones.html?contr=phones&tab=-1&source=SOPO_ServicePlan&servicePlanId=";

            JQuery doc = new JQuery(this.GetHttpContent(url), url);
            doc.find(".clearfix.device-carousel-card").each(item =>
            {
                var node = item.ToJQuery();

                string min = node.find(".detail-table .right").eq(1).val().Replace("mins to any NZ phone", "").Replace("to any NZ/Aus phone", "").Replace("NZ only", "").Replace("NZ Rollover Mins*", "");
                string text = node.find(".detail-table .right").eq(2).val().Replace("to any NZ network", "").Replace("to any NZ/Aus network", "").Replace("texts", "");
                string dataMB = node.find(".detail-table .first-cell .right").text().Trim();
                string nameMB = "";
                dataMB = dataMB.Replace("<span class=\"data\"> + 500MB</span>", "");
                dataMB = dataMB.Replace("<span class=\"data\"> + 250MB</span>", "");
                dataMB = dataMB.Replace("<span class=\"data\">", "");
                dataMB = dataMB.Replace("</span>", "");
                dataMB = dataMB.Replace("Rollover Data", "").Replace("Bonus Data*", "").Replace("Rollover", "").Replace("Socialiser*", "").Replace("*", "").Trim();

                if (dataMB == "")
                {
                    dataMB = "0";
                }
                else
                {
                    if (dataMB.Contains("+"))
                    {
                        var strList = dataMB.Split('+');

                        dataMB = (Convert.ToDecimal(strList[0].Replace("GB", "").Trim()) + Convert.ToDecimal(strList[1].Replace("GB", "").Trim())).ToString("0.0");

                        dataMB += "GB";

                        nameMB = strList[0].Trim();
                    }
                    else
                    {
                        nameMB = dataMB;
                    }
                }

                if(nameMB.Contains("Unlimited"))
                {
                    nameMB = "Unlimited Data";
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

                string otherNames = node.find(".pink-tag").text().Trim();
                if (otherNames.Contains("12 Mths"))
                {
                    info.MobilePlanName = "12 Months " + nameMB + " $" + info.Price.ToString("0.00");
                    info.ContractTypeId = 2;
                }
                else if (otherNames.Contains("24 Mths"))
                {
                    info.MobilePlanName = "24 Months " + nameMB + " $" + info.Price.ToString("0.00");
                    info.ContractTypeId = 3;
                }
                else if(otherNames.Contains("Unlimited"))
                {
                    info.MobilePlanName = nameMB + " $" + info.Price.ToString("0.00");
                    info.ContractTypeId = 1;
                }
                else if (otherNames.Contains("New Rollover Plans"))
                {
                    info.MobilePlanName = "24 Months " + nameMB + " $" + info.Price.ToString("0.00");
                    info.ContractTypeId = 3;
                }
                else
                {
                    info.MobilePlanName = otherNames + " " + info.MobilePlanName;

                }

                var PhoneList = new List<MobilePhoneInfo>();

                string phonesUrlFlag = node.find(".hightlight-text.view-mobi-add").attr("data-serviceplanid");
                string phonesUrl = phonesUrlBase + phonesUrlFlag;
                JQuery phonesDoc = new JQuery(this.GetHttpContent(phonesUrl), phonesUrl);

                List<MobilePhoneInfo> pInfos = new List<MobilePhoneInfo>();
                phonesDoc.find("#entry > .container-fluid > *").each(phonesItem =>
                {
                    var phoneNode = phonesItem.ToJQuery();
                    if (phoneNode.get(0).OuterHtml.StartsWith("<spark-device-card"))
                    {
                        var mpInfo = new MobilePhoneInfo();
                        //infos.ContractTypeID = node.attr("prop-id");
                        mpInfo.PhoneName = phoneNode.attr("prop-brandName") + " " + phoneNode.attr("prop-name");
                        mpInfo.UpfrontPrice = phoneNode.attr("prop-deferredPrice").ToDecimal();
                        mpInfo.PhoneImage = phoneNode.attr("prop-productImage");
                        Uri imageAbsoluteUri = new Uri(new Uri("https://www.spark.co.nz/"), mpInfo.PhoneImage);
                        mpInfo.PhoneImage = imageAbsoluteUri.ToString();
                        mpInfo.phoneURL = phoneNode.attr("prop-cta");
                        Uri productAbsoluteUri = new Uri(new Uri("https://www.spark.co.nz/"), mpInfo.phoneURL);
                        mpInfo.phoneURL = productAbsoluteUri.ToString();
                        mpInfo.PlanName = phoneNode.attr("prop-listedPlanName");
                        mpInfo.ContractTypeID = info.ContractTypeId;
                        pInfos.Add(mpInfo);
                    }
                });
                info.Phones = pInfos;

                mobilePlanList.Add(info);
            });
        }
    }
}
