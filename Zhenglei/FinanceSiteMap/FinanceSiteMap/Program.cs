using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Xml;

namespace FinanceSiteMap
{
    internal class Program
    {
        private static void Main(string[] args)
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
            SetFixedUrl(xmlDoc, xmlRoot);
            List<string> productList = new List<string>();
            FinancePage.GetProviders(productList);
            SetFinanceUrl(xmlDoc, xmlRoot, productList);
            productList = new List<string>();
            FinancePage.GetCreditCards(productList);
            SetFinanceUrl(xmlDoc, xmlRoot, productList);
            productList = new List<string>();
            FinancePage.GetSavingsAccounts(productList);
            SetFinanceUrl(xmlDoc, xmlRoot, productList);
            productList = new List<string>();
            FinancePage.GetTermDeposits(productList);
            SetFinanceUrl(xmlDoc, xmlRoot, productList);
            productList = new List<string>();
            FinancePage.GetHomeLoans(productList);
            SetFinanceUrl(xmlDoc, xmlRoot, productList);
            productList = new List<string>();
            productList = FinancePage.GetCategoryProviderInfo();
            SetFinanceUrl(xmlDoc, xmlRoot, productList);
            string xmlFileName = ConfigurationManager.AppSettings["SiteMapPath"].ToString();
            if (!Directory.Exists(xmlFileName))
            {
                Directory.CreateDirectory(xmlFileName);
            }
            xmlFileName += "\\sitemap.xml";
            xmlDoc.Save(xmlFileName);
            CopySiteMap(xmlFileName);
        }

        private static void SetFixedUrl(XmlDocument xmlDoc, XmlElement xmlRoot)
        {
            string siteMapUrl = FinancePage.WebSiteRootUrl_Static;
            string about = string.Empty;
            about = siteMapUrl;
            SetXml(xmlDoc, xmlRoot, about);
            about = siteMapUrl + "credit-cards";
            SetXml(xmlDoc, xmlRoot, about);
            about = siteMapUrl + "compare-all-credit-cards";
            SetXml(xmlDoc, xmlRoot, about);
            about = siteMapUrl + "low-interest-credit-cards_s-1";
            SetXml(xmlDoc, xmlRoot, about);
            about = siteMapUrl + "low-fee-credit-cards_s-2";
            SetXml(xmlDoc, xmlRoot, about);
            about = siteMapUrl + "balance-transfer-credit-cards_s-3";
            SetXml(xmlDoc, xmlRoot, about);
            about = siteMapUrl + "rewards-credit-cards_s-4";
            SetXml(xmlDoc, xmlRoot, about);
            about = siteMapUrl + "frequent-flyer-credit-cards_s-7";
            SetXml(xmlDoc, xmlRoot, about);
            about = siteMapUrl + "savings-accounts";
            SetXml(xmlDoc, xmlRoot, about);
            about = siteMapUrl + "compare-all-savings-accounts";
            SetXml(xmlDoc, xmlRoot, about);
            about = siteMapUrl + "term-deposits";
            SetXml(xmlDoc, xmlRoot, about);
            about = siteMapUrl + "all-term-deposits";
            SetXml(xmlDoc, xmlRoot, about);
            about = siteMapUrl + "home-loans";
            SetXml(xmlDoc, xmlRoot, about);
            about = siteMapUrl + "compare-all-home-loans";
            SetXml(xmlDoc, xmlRoot, about);
            about = siteMapUrl + "kiwisaver";
            SetXml(xmlDoc, xmlRoot, about);
        }

        private static void SetXml(XmlDocument xmlDoc, XmlElement xmlRoot, string innerXml)
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

        private static void SetFinanceUrl(XmlDocument xmlDoc, XmlElement xmlRoot, List<string> productList)
        {
            foreach (string url in productList)
            {
                SetXml(xmlDoc, xmlRoot, url);
            }
        }

        private static void CopySiteMap(string path)
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
    }
}