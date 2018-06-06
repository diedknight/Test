using PriceMePlansDBA;
using SubSonic.Schema;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;

namespace PriceMePlansSiteMap
{
    public class PlansSiteMap
    {
        private static Regex illegalReg = new Regex("[^a-z0-9-]+", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        private static Regex illegalReg2 = new Regex("-+", RegexOptions.Compiled);

        private static Regex illegalReg3 = new Regex("^-+|-+$", RegexOptions.Compiled);

        private Dictionary<int, string> carrDic;

        private string appPath = ConfigurationManager.AppSettings["WebSiteRootUrl"];

        public void SiteMap()
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlDeclaration xmlDeclaration = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);
            xmlDoc.AppendChild(xmlDeclaration);
            XmlElement xmlRoot = xmlDoc.CreateElement("urlset", "http://www.sitemaps.org/schemas/sitemap/0.9");
            xmlRoot.SetAttribute("xmlns", "http://www.sitemaps.org/schemas/sitemap/0.9");
            xmlRoot.SetAttribute("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");
            XmlAttribute xa = xmlDoc.CreateAttribute("xsi", "schemaLocation", "http://www.w3.org/2001/XMLSchema-instance");
            xa.Value = "http://www.sitemaps.org/schemas/sitemap/0.9 http://www.sitemaps.org/schemas/sitemap/0.9/sitemap.xsd";
            xmlRoot.Attributes.Append(xa);
            xmlDoc.AppendChild(xmlRoot);
            this.SetFixedUrl(xmlDoc, xmlRoot);
            this.GetCarrier();
            List<string> plans = new List<string>();
            this.GetMobilePlan(plans);
            this.SetPriceMePlansUrl(xmlDoc, xmlRoot, plans);
            List<string> phones = new List<string>();
            this.GetMobilePhone(phones);
            this.SetPriceMePlansUrl(xmlDoc, xmlRoot, phones);
            string xmlFileName = ConfigurationManager.AppSettings["SiteMapPath"].ToString();
            if (!Directory.Exists(xmlFileName))
            {
                Directory.CreateDirectory(xmlFileName);
            }
            xmlFileName += "\\sitemap.xml";
            xmlDoc.Save(xmlFileName);
            this.CopySiteMap(xmlFileName);
        }

        private void GetMobilePlan(List<string> plans)
        {
            string sql = @"Select Id, CarrierId, PlanName From dbo.CSK_Store_MobilePlan
                           where Status = 1 and Id in (select MobilePlanId from CSK_Store_MobilePlanPhoneMap where Status = 1)";
            IDataReader dr = new StoredProcedure("")
            {
                Command =
                {
                    CommandSql = sql,
                    CommandType = CommandType.Text,
                    CommandTimeout = 0
                }
            }.ExecuteReader();
            while (dr.Read())
            {
                string id = dr["Id"].ToString();
                string carrierId = dr["CarrierId"].ToString();
                string planName = dr["PlanName"].ToString();
                int cid = 0;
                int.TryParse(carrierId, out cid);
                string carrierName = string.Empty;
                if (this.carrDic.ContainsKey(cid))
                {
                    carrierName = this.carrDic[cid];
                }
                string url = this.appPath + this.FilterInvalidUrlPathChar(carrierName + "-" + planName) + "_p_" + id;
                plans.Add(url);
            }
            dr.Close();
        }

        private void GetMobilePhone(List<string> phones)
        {
            string sql = "Select m.Id, m.MobilePlanId, m.MobilePhoneId, p.CarrierId, p.PlanName, ph.Name From dbo.CSK_Store_MobilePlanPhoneMap m " +
                         "inner join CSK_Store_MobilePlan p on m.MobilePlanId = p.Id " +
                         "inner join dbo.CSK_Store_MobilePhone ph on m.MobilePhoneId = ph.Id " +
                         "where m.Status = 1 and p.Status = 1 and ph.Pid > 0";
            IDataReader dr = new StoredProcedure("")
            {
                Command =
                {
                    CommandSql = sql,
                    CommandType = CommandType.Text,
                    CommandTimeout = 0
                }
            }.ExecuteReader();
            while (dr.Read())
            {
                string id = dr["Id"].ToString();
                string carrierId = dr["CarrierId"].ToString();
                string planName = dr["PlanName"].ToString();
                string mobilePhoneId = dr["MobilePhoneId"].ToString();
                string phoneName = dr["Name"].ToString();
                int cid = 0;
                int.TryParse(carrierId, out cid);
                string carrierName = string.Empty;
                if (this.carrDic.ContainsKey(cid))
                {
                    carrierName = this.carrDic[cid];
                }
                int pid = 0;
                int.TryParse(mobilePhoneId, out pid);
                string url = string.Empty;
                if (pid > 0)
                {
                    continue;
                    url = string.Concat(new object[]
                    {
                        this.appPath,
                        this.FilterInvalidUrlPathChar(string.Concat(new string[]
                        {
                            carrierName,
                            "-",
                            planName
                        })),
                        "_p_",
                        id,
                        "_",
                        pid
                    });
                }
                else
                {
                    url = this.appPath + this.FilterInvalidUrlPathChar(carrierName + "-" + planName) + "_p_" + id;
                }
                phones.Add(url);
            }
            dr.Close();
        }

        private void GetCarrier()
        {
            this.carrDic = new Dictionary<int, string>();
            List<CSK_Store_Carrier> carrs = CSK_Store_Carrier.All().ToList<CSK_Store_Carrier>();
            foreach (CSK_Store_Carrier carr in carrs)
            {
                if (!this.carrDic.ContainsKey(carr.Id))
                {
                    this.carrDic.Add(carr.Id, carr.Name);
                }
            }
        }

        private void SetFixedUrl(XmlDocument xmlDoc, XmlElement xmlRoot)
        {
            string about = string.Empty;
            about = appPath;
            this.SetXml(xmlDoc, xmlRoot, about);
            about = appPath + "mobile-plans";
            this.SetXml(xmlDoc, xmlRoot, about);
            about = appPath + "all-mobile-plans";
            this.SetXml(xmlDoc, xmlRoot, about);
        }

        private void SetXml(XmlDocument xmlDoc, XmlElement xmlRoot, string innerXml)
        {
            XmlNode xmlElement = xmlDoc.CreateElement("url", xmlDoc.DocumentElement.NamespaceURI);
            XmlNode xmlNode = xmlDoc.CreateElement("loc", xmlDoc.DocumentElement.NamespaceURI);
            xmlNode.InnerXml = innerXml;
            xmlElement.AppendChild(xmlNode);
            xmlNode = xmlDoc.CreateElement("changefreq", xmlDoc.DocumentElement.NamespaceURI);
            xmlNode.InnerXml = "daily";
            xmlElement.AppendChild(xmlNode);
            xmlRoot.AppendChild(xmlElement);
        }

        private void CopySiteMap(string path)
        {
            string copyPath = ConfigurationManager.AppSettings["CopyPath"].ToString();
            string copyFileName = ConfigurationManager.AppSettings["CopyFileName"].ToString();
            DirectoryInfo dir = new DirectoryInfo(copyPath);
            DirectoryInfo[] fileInfo = dir.GetDirectories();
            for (int i = 0; i < fileInfo.Length; i++)
            {
                if (fileInfo[i].Name.ToLower().Contains(copyFileName.ToLower()))
                {
                    string dpath = copyPath + fileInfo[i].Name + "\\sitemap.xml";
                    if (File.Exists(dpath))
                    {
                        File.Delete(dpath);
                    }
                    File.Copy(path, dpath);
                }
            }
        }

        private void SetPriceMePlansUrl(XmlDocument xmlDoc, XmlElement xmlRoot, List<string> productList)
        {
            foreach (string url in productList)
            {
                this.SetXml(xmlDoc, xmlRoot, url);
            }
        }

        private string FilterInvalidUrlPathChar(string sourceString)
        {
            sourceString = illegalReg.Replace(sourceString, "-");
            sourceString = illegalReg2.Replace(sourceString, "-");
            sourceString = illegalReg3.Replace(sourceString, "");
            return sourceString;
        }
    }
}