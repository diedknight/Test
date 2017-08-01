using PriceMeCommon.Data;
using SubSonic.Schema;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceMeCommon.BusinessLogic
{
    public static class FavoritesController
    {
        public static List<FavouriteProductData> GetUserFavouriteProductData(int countryId, string parserId)
        {
            List<FavouriteProductData> listFavouriteProduct = new List<FavouriteProductData>();
            string sql = string.Format("select productID from ProductFavourites where productID > 0 and tokenID = '{0}' order by id desc", parserId);
            using (SqlConnection conn = new SqlConnection(MultiCountryController.GetDBConnectionString(countryId)))
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.CommandTimeout = 0;
                conn.Open();
                using (var idr = cmd.ExecuteReader())
                {
                    while (idr.Read())
                    {
                        int pid = 0;
                        int.TryParse(idr["productID"].ToString(), out pid);
                        FavouriteProductData fp = new FavouriteProductData();
                        fp.ProductId = pid;
                        fp.TokenId = parserId;
                        listFavouriteProduct.Add(fp);
                    }
                }
            }

            return listFavouriteProduct;
        }

        public static List<FavouritesPageData> GetUserFavouriteCatalogPages(int countryId, string parserId)
        {
            List<FavouritesPageData> listFavouritePages = new List<FavouritesPageData>();
            string sql = string.Format("select PageId from PageFavourites where PageId != '' And (PageName = 'catalog') and tokenID = '{0}' order by id desc", parserId);
            using (SqlConnection conn = new SqlConnection(MultiCountryController.GetDBConnectionString(countryId)))
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.CommandTimeout = 0;
                conn.Open();
                using (var idr = cmd.ExecuteReader())
                {
                    while (idr.Read())
                    {
                        string pageid = idr["PageId"].ToString();
                        FavouritesPageData fp = new FavouritesPageData();
                        fp.PageId = pageid.ToLower();
                        fp.TokenId = parserId;
                        fp.PageName = "catalog";
                        listFavouritePages.Add(fp);
                    }
                }
            }

            return listFavouritePages;
        }

        public static void AddUserFavouriteSearchPages(int countryId, string url, string parseId)
        {
            string sql = "Insert PageFavourites Values ('" + url.Replace("'", "''") + "', '" + parseId + "', 'search')";
            SqlTextExecuteNonQuery(countryId, sql);
        }

        public static void DeleteUserFavouriteSearchPages(int countryId, string url, string parseId)
        {
            string sql = "Delete PageFavourites Where PageId = '" + url.Replace("'", "''") + "' And tokenID = '" + parseId + "' And PageName = 'search'";
            SqlTextExecuteNonQuery(countryId, sql);
        }

        public static void DeleteUserFavouriteCatalogPages(int countryId, int cid, string parseid)
        {
            string sql = "Delete PageFavourites Where PageId = '" + cid + "' And tokenID = '" + parseid + "' And PageName = 'catalog'";
            SqlTextExecuteNonQuery(countryId, sql);
        }

        public static void AddUserFavouriteCatalogPages(int countryId, int cId, string parseId)
        {
            string sql = "Insert PageFavourites Values ('" + cId + "', '" + parseId + "', 'catalog')";
            SqlTextExecuteNonQuery(countryId, sql);
        }

        public static void AddUserFavouriteProduct(int countryId, int productId, string parseId)
        {
            string sql = "Insert ProductFavourites values(" + productId + ", '" + parseId + "')";
            SqlTextExecuteNonQuery(countryId, sql);
        }

        public static void DeleteUserFavouriteProduct(int countryId, int productId, string parseId)
        {
            string sql = "Delete ProductFavourites Where productID = " + productId + " And tokenID = '" + parseId + "'";
            SqlTextExecuteNonQuery(countryId, sql);
        }

        static void SqlTextExecuteNonQuery(int countryId, string sql)
        {
            using (SqlConnection conn = new SqlConnection(MultiCountryController.GetDBConnectionString(countryId)))
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.CommandTimeout = 0;
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public static List<FavouritesPageData> GetUserFavouriteSearchPages(int countryId, string parserId)
        {
            List<FavouritesPageData> listFavouritePages = new List<FavouritesPageData>();
            string sql = string.Format("select PageId from PageFavourites where PageId != '' And (PageName = 'search') and tokenID = '{0}' order by id desc", parserId);
            using (SqlConnection conn = new SqlConnection(MultiCountryController.GetDBConnectionString(countryId)))
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.CommandTimeout = 0;
                conn.Open();
                using (var idr = cmd.ExecuteReader())
                {
                    while (idr.Read())
                    {
                        string pageid = idr["PageId"].ToString();
                        FavouritesPageData fp = new FavouritesPageData();
                        fp.PageId = pageid.ToLower();
                        fp.TokenId = parserId;
                        fp.PageName = "search";
                        listFavouritePages.Add(fp);
                    }
                }
            }

            return listFavouritePages;
        }
    }
}