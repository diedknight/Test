using ImportAttrToExcel.Data;
using Pricealyser.Crawler.HtmlParser.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Pricealyser.Crawler;
using System.IO;
using PriceMeCrawlerTask.Common.Log;
using System.Text.RegularExpressions;

namespace ImportAttrToExcel
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Priceme.Infrastructure.Excel.ExcelSimpleHelper helper = new Priceme.Infrastructure.Excel.ExcelSimpleHelper();
                helper.WriteLine(true, false,
                    "Data Source Name",
                    "Retailer ID",
                    "Cid",
                    "Category",
                    "Attribute Group",
                    "DS attribute name",
                    "DS Attribute Unit",
                    "PriceMe Attribute name",
                    "PriceMe Attribute Unit",
                    "Example value",
                    "Defined values",
                    "Value type",
                    "Attribute type",
                    "Remark",
                    "Description"
                    );


                AppConfig.CidAndProductURL.ForEach(cidUrl =>
                {
                    string html = GetHttpContent(cidUrl.ProductUrl);

                    JQuery doc = new JQuery(html, cidUrl.ProductUrl);

                    string groupName = "";
                    doc.find(".listcontainer > table > tr").each(tr =>
                    {
                        var trNode = tr.ToJQuery();

                        var groupNode = trNode.find("h3");
                        if (groupNode.length != 0)
                        {
                            groupName = groupNode.text().TrimA();
                            return;
                        }

                        if (!string.IsNullOrEmpty(trNode.attr("data-id")))
                        {
                            trNode.find(".contribute-hint").remove();

                            string attrName = trNode.find("th").text().TrimA();
                            string AttrVal = trNode.find("td:first").text().TrimA();

                            AppConfig.MultipleAttributes.ForEach(item =>
                            {
                                if (attrName == item)
                                {
                                    string tempHtml = trNode.find("td:first div").html();
                                    if (!string.IsNullOrEmpty(tempHtml))
                                    {
                                        tempHtml = tempHtml.Replace("<br>", ", ");
                                        tempHtml = tempHtml.Replace("</br>", "");

                                        AttrVal = tempHtml;
                                    }
                                }
                            });

                            ExcelLineData lineData = new ExcelLineData();
                            lineData.DataSourceName = AppConfig.DR_Name;
                            lineData.RetailerID = AppConfig.RID.ToString();
                            lineData.Cid = cidUrl.CId.ToString();
                            lineData.Category = CategoryCtrl.GetCategoryName(cidUrl.CId);
                            lineData.AttributeGroup = groupName;
                            lineData.DSattributename = attrName;
                            lineData.DSAttributeUnit = GetUnit(AttrVal);
                            lineData.PriceMeAttributename = lineData.DSattributename;
                            lineData.PriceMeAttributeUnit = lineData.DSAttributeUnit;
                            lineData.Examplevalue = "";

                            lineData.Definedvalues = AttrVal;
                            if (!string.IsNullOrEmpty(AttrVal) && !string.IsNullOrEmpty(lineData.DSAttributeUnit))
                            {
                                lineData.Definedvalues = AttrVal.Replace(lineData.DSAttributeUnit, "").Trim();
                            }

                            lineData.Valuetype = GetValueType(AttrVal);
                            lineData.Attributetype = "Compare attr";

                            WriteLine(helper, lineData);
                        }
                    });
                });

                helper.Save(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "output.xls"));
            }
            catch (Exception ex)
            {
                string filePath = Path.Combine(AppConfig.LogPath, "Log_" + DateTime.Now.ToString("yyyyMMdd") + ".txt");
                XbaiLog.WriteLog(filePath, ex.Message);
                XbaiLog.WriteLog(filePath, ex.StackTrace);
            }
        }

        public static void WriteLine(Priceme.Infrastructure.Excel.ExcelSimpleHelper helper, ExcelLineData lineData)
        {
            helper.WriteLine(
                lineData.DataSourceName,
                lineData.RetailerID,
                lineData.Cid,
                lineData.Category,
                lineData.AttributeGroup,
                lineData.DSattributename,
                lineData.DSAttributeUnit,
                lineData.PriceMeAttributename,
                lineData.PriceMeAttributeUnit,
                lineData.Examplevalue,
                lineData.Definedvalues,
                lineData.Valuetype,
                lineData.Attributetype,
                "",
                ""
                );
        }

        public static string GetValueType(string attrVal)
        {
            string valType = "";

            if (!string.IsNullOrEmpty(attrVal))
            {

                AppConfig.Unit.ForEach(item => attrVal = attrVal.Replace(item, ""));
                double d = 0;
                int i = 0;

                double.TryParse(attrVal, out d);
                int.TryParse(attrVal, out i);

                if (attrVal.ToLower() == "yes" || attrVal.ToLower() == "no")
                {
                    valType = "IsBool";
                }
                else if (d - i > 0)
                {
                    valType = "Numeric";
                }
                else if (i != 0)
                {
                    valType = "Int";
                }
                else
                {
                    valType = "Nvarchar";
                }
            }

            return valType;
        }

        public static string GetUnit(string attrVal)
        {
            if (string.IsNullOrEmpty(attrVal)) return "";

            attrVal = attrVal.TrimA();

            List<string> unitList = new List<string>();

            AppConfig.Unit.ForEach(item =>
            {
                var index = attrVal.LastIndexOf(item);
                if (index != -1 && index + item.Length == attrVal.Length && (index != 0 && Char.IsWhiteSpace(attrVal, index - 1)))
                {
                    unitList.Add(item);
                }

                //if (attrVal.Contains(item))
                //{
                //    unitList.Add(item);
                //}
            });

            if (unitList.Count != 0)
            {
                unitList = unitList.OrderByDescending(item => item.Length).ToList();
                return unitList[0];
            }
            else
            {
                return "";
            }

            //return string.Join(";", unitList);
        }


        public static string GetHttpContent(string url)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

                request.Method = "GET";
                request.UserAgent = "Mozilla/5.0 (Windows NT 6.2; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/27.0.1453.94 Safari/537.36";
                //request.KeepAlive = false;
                //request.ContentType = "application/x-www-form-urlencoded";

                request.Timeout = 100000;
                request.CookieContainer = new CookieContainer();


                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    using (System.IO.StreamReader streamReader = new System.IO.StreamReader(response.GetResponseStream()))
                    {
                        string httpString = streamReader.ReadToEnd();

                        return httpString;
                    }
                }
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }

            return "";
        }

    }
}
