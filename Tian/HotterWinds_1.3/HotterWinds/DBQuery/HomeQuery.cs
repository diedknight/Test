using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dapper;
using System.Data.SqlClient;
using HotterWinds.ViewModels;
using HotterWinds.HWUtility;
using Pricealyser.Crawler.Request;
using System.Xml;
using Pricealyser.Crawler.HtmlParser.Query;

namespace HotterWinds.DBQuery
{
    public class HomeQuery : HotterWindsQuery
    {
        public static List<ViewModels.Product> GetProducts(int top, int categoryId)
        {
            List<ViewModels.Product> list = new List<ViewModels.Product>();

            string sql = " select top " + top
                        + " ProductID,"
                        + " DefaultImage as ImgUrl,"
                        + " ProductName, "
                        + " ProductRatingSum as Stars,"
                        + " CategoryId,"
                        + " Price = (select top 1 RetailerPrice from CSK_Store_RetailerProduct where ProductID = a.ProductID),"
                        + " RPCount = (select count(1) from CSK_Store_RetailerProduct where ProductID = a.ProductID),"
                        + " PurchaseUrl = (select top 1 PurchaseURL from CSK_Store_RetailerProduct where ProductID = a.ProductID),"
                        + " RetailerId=(select top 1 RetailerId from CSK_Store_RetailerProduct where ProductID = a.ProductID),"
                        + " RetailerProductId=(select top 1 RetailerProductId from CSK_Store_RetailerProduct where ProductID = a.ProductID),"
                        + " RetailerProductName=(select top 1 RetailerProductName from CSK_Store_RetailerProduct where ProductID = a.ProductID),"
                        + " SKU=(select top 1 RetailerProductSKU from CSK_Store_RetailerProduct where ProductID = a.ProductID)"
                        + " from CSK_Store_Product as a"
                        + " where categoryId = @cId and ProductStatus=1 order by createdon desc";

            var con = GetConnection();
            list = con.Query<ViewModels.Product>(sql, new { cId = categoryId }).ToList();

            list.ForEach(item =>
            {
                item.ImgUrl = Utility.FixImagePath(item.ImgUrl, "_ms");

                string retailerProductURL = PriceMe.Utility.GetRootUrl("/ResponseRedirect.aspx?pid=" + item.ProductId + "&rid=" + item.RetailerId + "&rpid=" + item.RetailerProductId + "&countryID=" + PriceMe.WebConfig.CountryId + "&cid=" + item.CategoryId + "&aid=40&t=" + "HW", PriceMe.WebConfig.CountryId);
                string uuid = Guid.NewGuid().ToString();
                retailerProductURL += "&uuid=" + uuid;

                item.PurchaseUrl = retailerProductURL;
            });

            return list;
        }

        public static List<ViewModels.Product> GetBestSellerProducts()
        {
            string trackday = System.Configuration.ConfigurationManager.AppSettings["trackday"];

            List<ViewModels.Product> list = new List<ViewModels.Product>();

            string sql = "select top 8 "
                        + " ProductId, "
                        + " ProductName = (select top 1 ProductName from CSK_Store_Product where ProductID = a.ProductId), "
                        + " PurchaseURL, "
                        + " ImgUrl = (select top 1 DefaultImage from CSK_Store_Product where ProductID = a.ProductId), "
                        + " Stars = (select top 1 ProductRatingSum from CSK_Store_Product where ProductID = a.ProductId), "
                        + " RetailerPrice as Price, "
                        + " RPCount = (select count(1) from CSK_Store_RetailerProduct where ProductID = a.ProductId), "
                        + " RetailerId,"
                        + " RetailerProductId,"
                        + " RetailerProductName,"
                        + " RetailerProductSKU as SKU,"
                        + " CategoryId = (select top 1 CategoryID from CSK_Store_Product where ProductID = a.ProductId)"
                        + " from CSK_Store_RetailerProduct as a"
                        + " where"
                        + " ProductId in"
                        + " ("
                        + "     select top 8 ProductId from Pam_user..CSK_Store_RetailerTracker"
                        + "     where retailerproductid in(select retailerproductid from csk_store_retailerproduct where retailerproductstatus = 1 and isdeleted = 0) "
                        + "     and AffiliateID = 40"
                        + "     and ClickTime >getdate() - " + trackday
                        + "     group by ProductId"
                        + "     order by count(ProductId) desc"
                        + " )";

            var con = GetConnection();
            list = con.Query<ViewModels.Product>(sql).ToList();

            list.ForEach(item => {
                item.ImgUrl = Utility.FixImagePath(item.ImgUrl, "_ms");

                string retailerProductURL = PriceMe.Utility.GetRootUrl("/ResponseRedirect.aspx?pid=" + item.ProductId + "&rid=" + item.RetailerId + "&rpid=" + item.RetailerProductId + "&countryID=" + PriceMe.WebConfig.CountryId + "&cid=" + item.CategoryId + "&aid=40&t=" + "HW", PriceMe.WebConfig.CountryId);
                string uuid = Guid.NewGuid().ToString();
                retailerProductURL += "&uuid=" + uuid;

                item.PurchaseUrl = retailerProductURL;
            });

            return list;
        }

        public static List<ViewModels.Product> GetFeatureProducts()
        {
            List<ViewModels.Product> list = new List<ViewModels.Product>();

            string sql = "select top 8"
                        + " ProductID,DefaultImage as ImgUrl,"
                        + " ProductName,"
                        + " ProductRatingSum as Stars,"
                        + " CategoryId,"
                        + " Price = (select top 1 RetailerPrice from CSK_Store_RetailerProduct where ProductID = a.ProductID),"
                        + " RPCount = (select count(1) from CSK_Store_RetailerProduct where ProductID = a.ProductID),"
                        + " PurchaseUrl = (select top 1 PurchaseURL from CSK_Store_RetailerProduct where ProductID = a.ProductID),"
                        + " RetailerId=(select top 1 RetailerId from CSK_Store_RetailerProduct where ProductID = a.ProductID),"
                        + " RetailerProductId=(select top 1 RetailerProductId from CSK_Store_RetailerProduct where ProductID = a.ProductID),"
                        + " RetailerProductName=(select top 1 RetailerProductName from CSK_Store_RetailerProduct where ProductID = a.ProductID),"
                        + " SKU=(select top 1 RetailerProductSKU from CSK_Store_RetailerProduct where ProductID = a.ProductID)"
                        + " from CSK_Store_Product as a"
                        + " where ProductStatus = 1 order by ModifiedOn desc";

            var con = GetConnection();
            list = con.Query<ViewModels.Product>(sql).ToList();

            list.ForEach(item =>
            {
                item.ImgUrl = Utility.FixImagePath(item.ImgUrl, "_ms");
                string retailerProductURL = PriceMe.Utility.GetRootUrl("/ResponseRedirect.aspx?pid=" + item.ProductId + "&rid=" + item.RetailerId + "&rpid=" + item.RetailerProductId + "&countryID=" + PriceMe.WebConfig.CountryId + "&cid=" + item.CategoryId + "&aid=40&t=" + "HW", PriceMe.WebConfig.CountryId);
                string uuid = Guid.NewGuid().ToString();
                retailerProductURL += "&uuid=" + uuid;

                item.PurchaseUrl = retailerProductURL;
            });

            return list;
        }

        public static List<ViewModels.BLog> GetBlogs()
        {
            List<ViewModels.BLog> list = new List<ViewModels.BLog>();

            XbaiRequest req = new XbaiRequest("https://hotterwinds.co.nz/blog/feed/");
            string xml = req.Get();
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);


            XmlNamespaceManager nsp = new XmlNamespaceManager(doc.NameTable);
            nsp.AddNamespace("dc", "http://purl.org/dc/elements/1.1/");
            nsp.AddNamespace("slash", "http://purl.org/rss/1.0/modules/slash/");

            foreach (XmlNode item in doc.SelectNodes("rss/channel/item"))
            {
                if (list.Count >= 2) break;

                HotterWinds.ViewModels.BLog blog = new HotterWinds.ViewModels.BLog();
                blog.Creator = item.SelectSingleNode("dc:creator", nsp).InnerText.Trim();
                blog.Link = item.SelectSingleNode("link").InnerText.Trim();
                blog.PubDate = Convert.ToDateTime(item.SelectSingleNode("pubDate").InnerText.Trim());
                blog.Title = item.SelectSingleNode("title").InnerText.Trim();
                blog.Comments = Convert.ToInt32(item.SelectSingleNode("slash:comments", nsp).InnerText.Trim());

                string html = item.SelectSingleNode("description").InnerText.Trim();

                JQuery jquery = new JQuery(html);
                blog.ImgUrl = jquery.children().first().getLink();
                blog.Description = jquery.last().text();

                list.Add(blog);
            }

            return list;
        }
    }
}