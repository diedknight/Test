using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;
using SiteMap.Data;
using SiteMap.SqlController;

namespace SiteMap
{
    public class SiteMapListing
    {
        StreamWriter sw;

        string sitemappath = SiteConfig.AppSettings("SiteMapPath");
        string siteMapUrl = SiteConfig.AppSettings("SiteMapUrl");
        string siteMapWebUrl = SiteConfig.AppSettings("SiteMapWebUrl");
        //string siteMapFileName = SiteConfig.AppSettings("SiteMapFileName");

        int countryId = int.Parse(SiteConfig.AppSettings("CountryID"));
        int PageSize = int.Parse(SiteConfig.AppSettings("PageSize"));
        int productCount = 0;

        List<string> VariantProductIds;
        Dictionary<int, string> parentCaetgoryDic;
        Dictionary<int, string> subCategoryDic;
        List<ProductCatalog> products;
        List<string> categoryUrlList = new List<string>();
        List<int> isSearchOnlyList;

        //string financeSiteMapPath = SiteConfig.AppSettings("FinanceSiteMapPath").ToString();
        //string plansSiteMapPath = SiteConfig.AppSettings("PlansSiteMapPath").ToString();
        //string blogSiteMapPath = SiteConfig.AppSettings("BlogSiteMapPath").ToString();
        //string consumerSiteMapPath = SiteConfig.AppSettings("ConsumerSiteMapPath").ToString();

        string allOtherSiteMaps = SiteConfig.AppSettings("AllOtherSiteMaps").ToString();
        string allOtherSiteMaps2 = SiteConfig.AppSettings("AllOtherSiteMaps2").ToString();

        public void CreateSiteMap()
        {
            BindLog();
            Write("Begin...");

            SetVariants();
            Write("Load Variants...");
            
            ClearSiteMap();
            Write("ClearSiteMap...");

            GetSubCategory();
            Write("Load SubCategory...");

            SetPriceme();
            Write("Set Priceme site map...");

            Write("SetSiteMapCategory...");
            foreach (KeyValuePair<int, string> pair in subCategoryDic)
            {
                products = new List<ProductCatalog>();

                SetSiteMapCategory(pair.Key, pair.Value);
                Write(pair.Value + " --> " + products.Count);
                productCount += products.Count;
            }
            Write("product All " + productCount);
            
            SetSiteMap();
            Write("SetSiteMap...");
            
            SetIndexSiteMap();
            Write("SetIndexSiteMap..");

            if (SiteConfig.AppSettings("isFTP") == "true")
            {
                CopySiteMapXmlByFTP(sitemappath + "SiteMap.xml");
                Write("CopySiteMapXmlByFTP...");
            }
            if (SiteConfig.AppSettings("isOriginalCopy") == "true")
            {
                CopySiteMapXml(sitemappath + "SiteMap.xml");
                Write("CopySiteMapXml..");
            }

            CopySiteMapRAR();
            ClearSiteMapRAR();
            Write("Copy and clear...");

            Write("End...");
        }

        private void SetSiteMapCategory(int categoryId, string categoryName)
        {
            SearchProduct(categoryId);
            int page = 0;
            page = products.Count / PageSize;
            int pagecount = page + 1;
            if ((products.Count % PageSize) == 0)
                pagecount = page;

            try
            {
                for (int j = 0; j < pagecount; j++)
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    XmlDeclaration xmlDeclaration = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);
                    xmlDoc.AppendChild(xmlDeclaration);

                    XmlElement xmlRoot = xmlDoc.CreateElement("urlset", "http://www.sitemaps.org/schemas/sitemap/0.9");
                    xmlRoot.SetAttribute("xmlns", "http://www.sitemaps.org/schemas/sitemap/0.9");
                    xmlRoot.SetAttribute("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");
                    System.Xml.XmlAttribute xa;
                    xa = xmlDoc.CreateAttribute("xsi", "schemaLocation", "http://www.w3.org/2001/XMLSchema-instance");
                    xa.Value = "http://www.sitemaps.org/schemas/sitemap/0.9 http://www.sitemaps.org/schemas/sitemap/0.9/sitemap.xsd";
                    xmlRoot.Attributes.Append(xa);

                    xmlDoc.AppendChild(xmlRoot);

                    int k = 0;
                    for (int i = (j * PageSize); i < products.Count; i++)
                    {
                        if (k >= PageSize) break;

                        try
                        {
                            string productUrl = siteMapUrl + "/" + FilterIllegalChar(products[i].ProductName) + "/p-" + products[i].ProductID + ".aspx";
                            if (countryId == 41)
                                productUrl = siteMapUrl + "/p-" + products[i].ProductID + ".aspx";

                            SetXml(xmlDoc, xmlRoot, productUrl.Replace("+", ""));
                        }
                        catch (Exception ex) { Write("ProductId: " + products[i].ProductID + "     Error: " + ex.Message); }
                        k++;
                    }

                    if (products.Count > 0)
                    {
                        string timeName = "20081021";
                        string xmlFileName = sitemappath + "SiteMap-" + timeName + "-" + FilterIllegalChar(categoryName) + "-" + categoryId + ".xml";
                        if (j > 0)
                        {
                            xmlFileName = sitemappath + "SiteMap-" + timeName + "-" + FilterIllegalChar(categoryName) + "-" + j + "-" + categoryId + ".xml";
                            Write("Exceed: " + xmlFileName);
                        }
                        xmlDoc.Save(xmlFileName);
                        Gzip(xmlFileName, xmlFileName + ".gz");
                        File.Delete(xmlFileName);
                    }

                    xmlDoc.RemoveAll();
                }
            }
            catch (Exception ex) { Write("CategoryId: " + categoryId + "     Error: " + ex.Message); }
        }

        private void SearchProduct(int categoryId)
        {
            if (!isSearchOnlyList.Contains(categoryId))
            {
                products = ProductSearcher.GetProducts(categoryId);
                products = products.Where(p => !VariantProductIds.Contains(p.ProductID)).ToList();
            }
        }

        private void SetIndexSiteMap()
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlDeclaration xmlDeclaration = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);
            xmlDoc.AppendChild(xmlDeclaration);

            XmlElement xmlRoot = xmlDoc.CreateElement("sitemapindex", "http://www.sitemaps.org/schemas/sitemap/0.9");
            xmlDoc.AppendChild(xmlRoot);

            for (int j = 0; j < categoryUrlList.Count; j++)
            {
                //string categoryUrl = siteMapWebUrl + siteMapFileName + "/" + categoryUrlList[j];
                string categoryUrl = siteMapWebUrl + categoryUrlList[j];
                if (categoryUrlList[j].StartsWith("https://"))
                    categoryUrl = categoryUrlList[j];

                SetIndexXml(xmlDoc, xmlRoot, categoryUrl);
            }

            if (categoryUrlList.Count > 0)
                xmlDoc.Save(sitemappath + "SiteMap.xml");
        }

        private void SetPriceme()
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlDeclaration xmlDeclaration = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);
            xmlDoc.AppendChild(xmlDeclaration);

            XmlElement xmlRoot = xmlDoc.CreateElement("urlset", "http://www.sitemaps.org/schemas/sitemap/0.9");
            xmlRoot.SetAttribute("xmlns", "http://www.sitemaps.org/schemas/sitemap/0.9");
            xmlRoot.SetAttribute("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");
            System.Xml.XmlAttribute xa;
            xa = xmlDoc.CreateAttribute("xsi", "schemaLocation", "http://www.w3.org/2001/XMLSchema-instance");
            xa.Value = "http://www.sitemaps.org/schemas/sitemap/0.9 http://www.sitemaps.org/schemas/sitemap/0.9/sitemap.xsd";
            xmlRoot.Attributes.Append(xa);

            xmlDoc.AppendChild(xmlRoot);

            string about = string.Empty;

            about = siteMapUrl;
            SetXml(xmlDoc, xmlRoot, about);
            
            about = siteMapUrl + "/PopularBrandsSiteMap.aspx";
            SetXml(xmlDoc, xmlRoot, about);
            
            about = siteMapUrl + "/About/AboutUs.aspx";
            SetXml(xmlDoc, xmlRoot, about);

            about = siteMapUrl + "/MostPopular.aspx";
            SetXml(xmlDoc, xmlRoot, about);

            about = siteMapUrl + "/SiteMap.aspx";
            SetXml(xmlDoc, xmlRoot, about);
            
            if (countryId == 3)
                about = "http://promote.priceme.co.nz/";
            else
                about = siteMapUrl + "/RetailerCenter/RetailerCenter.aspx";
            
            SetXml(xmlDoc, xmlRoot, about);

            if (countryId == 3)
                about = "http://promote.priceme.co.nz/#faqs";
            else
                about = siteMapUrl + "/RetailerCenter/FAQ.aspx";
            
            SetXml(xmlDoc, xmlRoot, about);
            
            about = siteMapUrl + "/About/FAQs.aspx";
            SetXml(xmlDoc, xmlRoot, about);
            
            GetParentCategory();
            foreach (KeyValuePair<int, string> pari in parentCaetgoryDic)
            {
                string categoryUrl = siteMapUrl + "/" + FilterIllegalChar(pari.Value) + "/c-" + pari.Key + ".aspx";
                if (countryId == 41)
                    categoryUrl = siteMapUrl + "/c-" + pari.Key + ".aspx";

                SetXml(xmlDoc, xmlRoot, categoryUrl);
            }

            string[] otherSiteMaps2 = allOtherSiteMaps2.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var sm in otherSiteMaps2)
            {
                SetXml(xmlDoc, xmlRoot, sm);
            }

            string xmlFileName = sitemappath + "SiteMap-20081021-priceme.xml";
            xmlDoc.Save(xmlFileName);
        }

        private void SetSiteMap()
        {
            DirectoryInfo dir = new DirectoryInfo(sitemappath);
            FileInfo[] fileInfo = dir.GetFiles();

            for (int i = 0; i < fileInfo.Length; i++)
            {
                if (fileInfo[i].Name.Contains("xml.gz"))
                    categoryUrlList.Add(fileInfo[i].Name);
                else if (fileInfo[i].Name.Contains("priceme.xml"))
                    categoryUrlList.Insert(0, fileInfo[i].Name);
            }

            string[] otherSiteMaps = allOtherSiteMaps.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var sm in otherSiteMaps)
            {
                categoryUrlList.Add(sm);
            }
        }

        private void GetParentCategory()
        {
            parentCaetgoryDic = new Dictionary<int, string>();

            MysqlDBController.LoadParentCategory(out parentCaetgoryDic, subCategoryDic);
        }

        private void GetSubCategory()
        {
            subCategoryDic = new Dictionary<int, string>();

            MysqlDBController.LoadSubCategory(out subCategoryDic);
        }

        private void ClearSiteMap()
        {
            if (!Directory.Exists(sitemappath))
                Directory.CreateDirectory(sitemappath);

            DirectoryInfo dir = new DirectoryInfo(sitemappath);
            FileInfo[] fileInfo = dir.GetFiles();

            for (int i = 0; i < fileInfo.Length; i++)
                fileInfo[i].Delete();
        }

        private void CopySiteMapXmlByFTP(string path)
        {
            try
            {
                string userID = SiteConfig.AppSettings("userid_FTP");
                string password = SiteConfig.AppSettings("password_FTP");
                string targetIP = SiteConfig.AppSettings("targetIP_FTP");
                string targetPath = SiteConfig.AppSettings("targetPath_FTP");
                CopyFile.FtpCopy.UploadFileSmall(path, targetPath, targetIP, userID, password);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void CopySiteMapXml(string path)
        {
            string targetPath = SiteConfig.AppSettings("CopySiteMapXMLTargetPath");
            if (!Directory.Exists(targetPath))
                Directory.CreateDirectory(targetPath);

            File.Copy(path, targetPath + "SiteMap.xml", true);
        }

        private void CopySiteMapRAR()
        {
            string targetPath = SiteConfig.AppSettings("CopySiteMapRARTargetPath");
            if (!Directory.Exists(targetPath))
                Directory.CreateDirectory(targetPath);

            DirectoryInfo dir = new DirectoryInfo(sitemappath);
            FileInfo[] fileInfo = dir.GetFiles();

            foreach (FileInfo file in fileInfo)
            {
                File.Copy(file.FullName, targetPath + file.Name, true);
            }
        }

        private void ClearSiteMapRAR()
        {
            string targetPath = SiteConfig.AppSettings("CopySiteMapRARTargetPath");
            if (!Directory.Exists(targetPath))
                Directory.CreateDirectory(targetPath);

            DirectoryInfo dir = new DirectoryInfo(targetPath);
            FileInfo[] fileInfo = dir.GetFiles();

            foreach (FileInfo file in fileInfo)
            {
                if (file.LastWriteTime < DateTime.Today)
                    file.Delete();
            }
        }

        private void SetVariants()
        {
            VariantProductIds = MysqlDBController.LoadVariants();

            isSearchOnlyList = MysqlDBController.LoadIsSearchOnly();
        }

        private void SetXml(XmlDocument xmlDoc, XmlElement xmlRoot, string innerXml)
        {
            XmlNode xmlElement;
            xmlElement = xmlDoc.CreateElement("url", xmlDoc.DocumentElement.NamespaceURI);

            XmlNode xmlNode = xmlDoc.CreateElement("loc", xmlDoc.DocumentElement.NamespaceURI);
            xmlNode.InnerXml = innerXml;
            xmlElement.AppendChild(xmlNode);

            xmlNode = xmlDoc.CreateElement("changefreq", xmlDoc.DocumentElement.NamespaceURI);
            xmlNode.InnerXml = "daily";
            xmlElement.AppendChild(xmlNode);

            xmlRoot.AppendChild(xmlElement);
        }

        private void SetIndexXml(XmlDocument xmlDoc, XmlElement xmlRoot, string innerXml)
        {
            XmlNode xmlElement;
            xmlElement = xmlDoc.CreateElement("sitemap", xmlDoc.DocumentElement.NamespaceURI);

            XmlNode xmlNode = xmlDoc.CreateElement("loc", xmlDoc.DocumentElement.NamespaceURI);
            xmlNode.InnerXml = innerXml;
            xmlElement.AppendChild(xmlNode);

            xmlRoot.AppendChild(xmlElement);
        }

        private void Gzip(string inputFile, string outputFile)
        {
            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    using (GZipStream compressedStream = new GZipStream(File.Create(outputFile), CompressionMode.Compress))
                    {
                        using (FileStream fileStream = File.OpenRead(inputFile))
                        {
                            byte[] buffer = new byte[fileStream.Length];
                            fileStream.Read(buffer);
                            compressedStream.Write(buffer, 0, buffer.Length);
                            compressedStream.Flush();
                            compressedStream.Close();
                        }
                    }                    
                }
            }
            catch (Exception ex)
            {
                Write("FZip.Zip : " + ex.Message);
            }
        }

        private void BindLog()
        {
            string date = DateTime.Now.ToString("yyyy MM dd");
            string path = SiteConfig.AppSettings("LogPath");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            sw = new StreamWriter(path + DateTime.Now.ToString("yyyyMMdd HH") + ".txt");
        }

        private Regex illegalReg = new Regex(@"[^a-z0-9-]+", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private Regex illegalReg2 = new Regex("-+", RegexOptions.Compiled);
        private Regex illegalReg3 = new Regex("^-+|-+$", RegexOptions.Compiled);

        public string FilterIllegalChar(string sourceString)
        {
            sourceString = illegalReg.Replace(sourceString, "-");
            sourceString = illegalReg2.Replace(sourceString, "-");
            sourceString = illegalReg3.Replace(sourceString, "");
            return sourceString;
        }

        private void Write(string info)
        {
            Console.WriteLine(info);

            sw.WriteLine(info);
            sw.Flush();
        }
    }
}
