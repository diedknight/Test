using System;
using System.Data.SqlClient;
using System.Collections.Specialized;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Security.Permissions;
using System.Data.Common;
using System.Data;
using System.Xml;
using System.IO;
using SubSonic;
using System.Text.RegularExpressions;
using PriceMeDBA;
using PriceMeCommon;
using FSuite;
using CopyFile;
using System.Management;
using PriceMeCommon.BusinessLogic;
using System.Linq;

namespace Pricealyser.SiteMap
{
    class SiteMapListing
    {
        private static Regex illegalReg = new Regex(@"[^a-z0-9-]+", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static Regex illegalReg2 = new Regex("-+", RegexOptions.Compiled);
        private static Regex illegalReg3 = new Regex("^-+|-+$", RegexOptions.Compiled);
        private static int PageSize = int.Parse(System.Configuration.ConfigurationManager.AppSettings["PageSize"].ToString());
        List<PriceMeCommon.Data.ProductCatalog> products;
        Dictionary<int, string> subCategoryDic;
        List<string> categoryUrlList = new List<string>();
        int productCount = 0;
        string sitemappath;

        List<int> geekzonIdList;

        StreamWriter _sw;
        public StreamWriter SW
        {
            get { return _sw; }
            set { _sw = value; }
        }

        int countryId = int.Parse(System.Configuration.ConfigurationManager.AppSettings["CountryID"]);
        string siteMapUrl = System.Configuration.ConfigurationManager.AppSettings["SiteMapUrl"];
        string siteMapWebUrl = System.Configuration.ConfigurationManager.AppSettings["SiteMapWebUrl"];
        string siteMapFileName = System.Configuration.ConfigurationManager.AppSettings["SiteMapFileName"];

        public void SetSiteMapListing()
        {
            //CategoryController.LoadForBuildIndex();
            geekzonIdList = new List<int>();
            string[] cata_s = System.Configuration.ConfigurationManager.AppSettings["Catalog"].Split(';');

            foreach (var item in cata_s)
            {
                string[] catas = item.Split(',');
                int categoryID = int.Parse(catas[0]);
                geekzonIdList.Add(categoryID);
                var allSubI = CategoryController.GetAllSubCategory(categoryID, 3);
                foreach (var sub in allSubI)
                {
                    if (!sub.IsAccessories)
                    {
                        geekzonIdList.Add(sub.CategoryID);
                    }
                }
            }

            sitemappath = System.Configuration.ConfigurationManager.AppSettings["SiteMapPath"].ToString();
            Console.WriteLine("ClearSiteMap..");
            ClearSiteMap();
            Console.WriteLine("GetSubCategory..");
            GetSubCategory();

            Console.WriteLine("SetPriceme..");
            SetPriceme();
            Console.WriteLine("SetSiteMapCategory..");
            foreach (KeyValuePair<int, string> pair in subCategoryDic)
            {
                SetSiteMapCategory(pair.Key, pair.Value);
                Write(pair.Value + " --> " + products.Count);
                productCount += products.Count;
            }

            Write("product All " + productCount);
            Console.WriteLine("SetSiteMap..");
            SetSiteMap();
            Console.WriteLine("SetIndexSiteMap..");
            SetIndexSiteMap();

            if (System.Configuration.ConfigurationManager.AppSettings["isFTP"] != null && bool.Parse(System.Configuration.ConfigurationManager.AppSettings["isFTP"]) == true)
            {
                Console.WriteLine("CopySiteMapXmlByFTP..");
                CopySiteMapXmlByFTP(sitemappath + "SiteMap.xml");
            }
            if (System.Configuration.ConfigurationManager.AppSettings["isOriginalCopy"] != null && bool.Parse(System.Configuration.ConfigurationManager.AppSettings["isOriginalCopy"]) == true)
            {
                Console.WriteLine("CopySiteMapXml..");
                CopySiteMapXml(sitemappath + "SiteMap.xml");//
            }

            Console.WriteLine("CopySiteMapRAR..");
            CopySiteMapRAR(sitemappath);
            Console.WriteLine("ClearSiteMapRAR..");
            ClearSiteMapRAR(sitemappath);
            Console.WriteLine("End ClearSiteMapRAR..");
        }

        private void CopySiteMapByFTP()
        {
            //string copyPath = System.Configuration.ConfigurationManager.AppSettings["CopySiteMapPath"].ToString();

            DirectoryInfo dir = new DirectoryInfo(sitemappath);
            FileInfo[] fileInfo = dir.GetFiles();

            for (int i = 0; i < fileInfo.Length; i++)
            {
                string path = sitemappath + fileInfo[i].Name;
                //string dpath = copyPath + fileInfo[i].Name;
                //if (File.Exists(dpath))
                //    File.Delete(dpath);

                try
                {
                    string userID = System.Configuration.ConfigurationManager.AppSettings["userid_FTP"];
                    string password = System.Configuration.ConfigurationManager.AppSettings["password_FTP"];
                    string targetIP = System.Configuration.ConfigurationManager.AppSettings["targetIP_FTP"];
                    string targetPath = System.Configuration.ConfigurationManager.AppSettings["targetPath_FTP"];
                    CopyFile.FtpCopy.UploadFileSmall(path, targetPath, targetIP, userID, password);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        private void CopySiteMapXmlByFTP(string path)
        {
            try
            {
                string userID = System.Configuration.ConfigurationManager.AppSettings["userid_FTP"];
                string password = System.Configuration.ConfigurationManager.AppSettings["password_FTP"];
                string targetIP = System.Configuration.ConfigurationManager.AppSettings["targetIP_FTP"];
                string targetPath = System.Configuration.ConfigurationManager.AppSettings["targetPath_FTP"];
                CopyFile.FtpCopy.UploadFileSmall(path, targetPath, targetIP, userID, password);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void CopySiteMapXml(string path)
        {
            string targetPath = System.Configuration.ConfigurationManager.AppSettings["targetPath"];
            string targetIP = System.Configuration.ConfigurationManager.AppSettings["targetIP"];
            string userID = System.Configuration.ConfigurationManager.AppSettings["userid"];
            string password = System.Configuration.ConfigurationManager.AppSettings["password"];

            CopyFile.NetWorkCopy.CopyCurrentFile(targetIP, targetPath, userID, password, path);
        }

        private void CopySiteMapRAR(string path)
        {
            string userID = System.Configuration.ConfigurationManager.AppSettings["useridImage"];
            string password = System.Configuration.ConfigurationManager.AppSettings["passwordImage"];
            string targetIP = System.Configuration.ConfigurationManager.AppSettings["targetIPImage"];
            string targetPath = System.Configuration.ConfigurationManager.AppSettings["targetPathImage"];

            CopyFile.NetWorkCopy.Copy(targetIP, targetPath, userID, password, path, "_notcopy");
        }

        private void ClearSiteMapRAR(string path)
        {
            DirectoryInfo cdir = new DirectoryInfo(path);
            System.IO.FileInfo[] files = cdir.GetFiles();

            string userID = System.Configuration.ConfigurationManager.AppSettings["useridImage"];
            string password = System.Configuration.ConfigurationManager.AppSettings["passwordImage"];
            string targetIP = System.Configuration.ConfigurationManager.AppSettings["targetIPImage"];
            string targetPath = System.Configuration.ConfigurationManager.AppSettings["targetPathImage"];

            CopyFile.NetWorkCopy.DeleteSiteMapRAR(targetIP, targetPath + @"\" + cdir.Name, userID, password, files.Length);
        }

        private void CopySiteMap()
        {
            string copyPath = System.Configuration.ConfigurationManager.AppSettings["CopySiteMapPath"].ToString();
            
            //清空
            DirectoryInfo cdir = new DirectoryInfo(copyPath);
            DirectoryInfo[] cfileInfo = cdir.GetDirectories();
            for (int i = 0; i < cfileInfo.Length; i++)
                cfileInfo[i].Delete();

            //copy
            DirectoryInfo dir = new DirectoryInfo(sitemappath);
            FileInfo[] fileInfo = dir.GetFiles();

            for (int i = 0; i < fileInfo.Length; i++)
            {
                string path = sitemappath + fileInfo[i].Name;
                string dpath = copyPath + fileInfo[i].Name;
                if (File.Exists(dpath))
                    File.Delete(dpath);

                File.Copy(path, dpath);
            }
        }

        private void ClearSiteMap()
        {
            DirectoryInfo dir = new DirectoryInfo(sitemappath);
            FileInfo[] fileInfo = dir.GetFiles();

            for (int i = 0; i < fileInfo.Length; i++)
                fileInfo[i].Delete();
        }

        private void GetSubCategory()
        {
            subCategoryDic = new Dictionary<int, string>();
            Dictionary<int, string> subIdDic = new Dictionary<int, string>();
            using (SubSonic.DataProviders.SharedDbConnectionScope sdbs = new SubSonic.DataProviders.SharedDbConnectionScope(MultiCountryController.GetDBProvider(countryId)))
            {
                SubSonic.Schema.StoredProcedure sp = new SubSonic.Schema.StoredProcedure("CSK_Store_Category_GetSubCategory");
                IDataReader dr = sp.ExecuteReader();
                while (dr.Read())
                {
                    int cid = int.Parse(dr["CategoryId"].ToString());
                    string cname = dr["CategoryName"].ToString();
                    subIdDic.Add(cid, cname);
                }
            }

            foreach (int cid in subIdDic.Keys)
            {
                if (geekzonIdList.Contains(cid))
                {
                    
                    if (!subCategoryDic.ContainsKey(cid))
                        subCategoryDic.Add(cid, subIdDic[cid]);
                }
            }
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

                    //https://price.geekzone.co.nz/product.aspx?pid=893913126
                    int k = 0;
                    for (int i = (j * PageSize); i < products.Count; i++)
                    {
                        if (k >= PageSize) break;

                        try
                        {
                            string productUrl = siteMapUrl + "/product.aspx?pid=" + products[i].ProductID;

                            SetXml(xmlDoc, xmlRoot, productUrl.Replace("+", ""));
                        }
                        catch (Exception ex) { Write("ProductId: " + products[i].ProductID + "     Error: " + ex.Message); }
                        k++;
                    }

                    if (products.Count > 0)
                    {
                        //string timeName = DateTime.Now.ToString("yyyy-MM-dd").Replace("-", "");
                        string timeName = "20180608";
                        string xmlFileName = sitemappath + "SiteMap-" + timeName + "-" + FilterIllegalChar(categoryName) + "-" + categoryId + ".xml";
                        if (j > 0)
                        {
                            xmlFileName = sitemappath + "SiteMap-" + timeName + "-" + FilterIllegalChar(categoryName) + "-" + j + "-" + categoryId + ".xml";
                            Write("Exceed: " + xmlFileName);
                        }
                        xmlDoc.Save(xmlFileName);
                        FGzip.Gzip(xmlFileName, xmlFileName + ".gz");
                        File.Delete(xmlFileName);
                    }

                    xmlDoc.RemoveAll();
                }
            }
            catch (Exception ex) { Write("CategoryId: " + categoryId + "     Error: " + ex.Message); }
        }

        private void SearchProduct(int categoryId)
        {
            if (products != null && products.Count > 0)
                products.Clear();
            else
                products = new List<PriceMeCommon.Data.ProductCatalog>();

            if (!CategoryController.IsSearchOnly(categoryId, countryId))
            {
                ProductSearcher ps = new ProductSearcher("", categoryId, null, null, null, null, "", null, int.MaxValue, countryId, false, false, true, true, null, "", null);

                int pCount = ps.GetProductCount();
                for (int i = 0; i < pCount; i++)
                {
                    var pi = ps.GetProductCatalog(i);
                    if (!string.IsNullOrEmpty(pi.BestPPCRetailerID))
                    {
                        products.Add(pi);
                    }
                }
            }
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
                string categoryUrl = siteMapWebUrl + siteMapFileName + "/" + categoryUrlList[j];

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

            Dictionary<int, string> parentCaetgoryDic = new Dictionary<int, string>();
            GetParentCategory(parentCaetgoryDic);
            //https://price.geekzone.co.nz/Catalog.aspx?cid=189
            foreach (KeyValuePair<int, string> pari in parentCaetgoryDic)
            {
                string categoryUrl = siteMapUrl + "/Catalog.aspx?cid=" + pari.Key;
                SetXml(xmlDoc, xmlRoot, categoryUrl);
            }

            string xmlFileName = sitemappath + "SiteMap-20180608-priceme.xml";
            xmlDoc.Save(xmlFileName);
        }

        private void GetParentCategory(Dictionary<int, string> categorys)
        {
            using (SubSonic.DataProviders.SharedDbConnectionScope sdbs = new SubSonic.DataProviders.SharedDbConnectionScope(MultiCountryController.GetDBProvider(countryId)))
            {
                SubSonic.Schema.StoredProcedure sp = new SubSonic.Schema.StoredProcedure("csk_store_CreateSitemap2");
                //sp.Command.AddParameter("@countryId", countryId, DbType.Int32);
                sp.Command.CommandTimeout = 0;
                IDataReader dr = sp.ExecuteReader();
                while (dr.Read())
                {
                    int cid = int.Parse(dr["CategoryId"].ToString());
                    if (geekzonIdList.Contains(cid))
                    {
                        string cname = dr["CategoryName"].ToString();
                        if (!subCategoryDic.ContainsKey(cid))
                            categorys.Add(cid, cname);
                    }
                }
            }
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

        public string FilterIllegalChar(string sourceString)
        {
            sourceString = illegalReg.Replace(sourceString, "-");
            sourceString = illegalReg2.Replace(sourceString, "-");
            sourceString = illegalReg3.Replace(sourceString, "");
            return sourceString;
        }

        private void Write(string info)
        {
            _sw.WriteLine(info);
            _sw.Flush();
        }
    }
}

