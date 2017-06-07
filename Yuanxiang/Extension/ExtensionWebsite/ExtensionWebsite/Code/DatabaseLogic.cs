using ExtensionWebsite.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ExtensionWebsite.Code
{
    public static class DatabaseLogic
    {
        public static RetailerProduct GetRetailerProduct(Retailer retailer, string key)
        {
            string tempkey = string.Empty;
            RetailerExtension re = SiteConfig.ListRetailerExtension.SingleOrDefault(r => r.RetailerId == retailer.RetailerId);
            if (re != null)
            {
                if (re.TypeId == 2)
                {
                    tempkey = key;
                    key = key + re.ConvertTxt;
                }
                else if (re.TypeId == 3)
                {
                    tempkey = key;
                    key = key.Replace(re.ConvertTxt, "");
                }
            }

            RetailerProduct rp = new RetailerProduct();

            string sql = "Select RetailerProductId, ProductId, RetailerProductName, RetailerPrice From "
                       + "CSK_Store_RetailerProductNew Where RetailerId = " + retailer.RetailerId + " And PurchaseURL = '" + key + "'";

            GetRetailerProducts(sql, rp, retailer);

            if (key.Contains("https:") && (rp == null || string.IsNullOrEmpty(rp.RetailerProductName)))
            {
                sql = "Select RetailerProductId, ProductId, RetailerProductName, RetailerPrice From "
                       + "CSK_Store_RetailerProductNew Where RetailerId = " + retailer.RetailerId + " And PurchaseURL = '" + key.Replace("https:", "http:") + "'";

                GetRetailerProducts(sql, rp, retailer);
            }

            if (!string.IsNullOrEmpty(tempkey) && string.IsNullOrEmpty(rp.RetailerProductName))
            {
                sql = "Select RetailerProductId, ProductId, RetailerProductName, RetailerPrice From "
                    + "CSK_Store_RetailerProductNew Where RetailerId = " + retailer.RetailerId + " And PurchaseURL = '" + tempkey + "'";

                GetRetailerProducts(sql, rp, retailer);
            }

            return rp;
        }

        private static void GetRetailerProducts(string sql, RetailerProduct rp, Retailer retailer)
        {
            using (SqlConnection sqlConn = new SqlConnection(SiteConfig.dicConnection[retailer.RetailerCountry]))
            {
                sqlConn.Open();
                using (System.Data.SqlClient.SqlCommand sqlCMD = new System.Data.SqlClient.SqlCommand())
                {
                    sqlCMD.CommandText = sql;
                    sqlCMD.CommandTimeout = 0;
                    sqlCMD.CommandType = CommandType.Text;
                    sqlCMD.Connection = sqlConn;

                    IDataReader dr = sqlCMD.ExecuteReader();
                    while (dr.Read())
                    {
                        int rpid = 0, pid = 0;
                        int.TryParse(dr["RetailerProductId"].ToString(), out rpid);
                        int.TryParse(dr["ProductId"].ToString(), out pid);
                        decimal price = 0m;
                        decimal.TryParse(dr["RetailerPrice"].ToString(), out price);

                        rp.RetailerId = retailer.RetailerId;
                        rp.ProductId = pid;
                        rp.RetailerProductId = rpid;
                        rp.RetailerProductName = dr["RetailerProductName"].ToString();
                        rp.RetailerPrice = price;
                    }
                    dr.Close();
                }
                sqlConn.Close();
            }
        }

        public static List<RetailerProduct> GetRetailerProducts(Retailer retailer, int pid)
        {
            List<RetailerProduct> rps = new List<RetailerProduct>();

            string sql = "select RetailerProductId, RetailerId, ProductId, RetailerProductName, RetailerPrice, Freight from "
                       + "CSK_Store_RetailerProductNew Where RetailerId != " + retailer.RetailerId + " And RetailerProductCondition = 0 And ProductId = " + pid;
            using (SqlConnection sqlConn = new SqlConnection(SiteConfig.dicConnection[retailer.RetailerCountry]))
            {
                sqlConn.Open();
                using (System.Data.SqlClient.SqlCommand sqlCMD = new System.Data.SqlClient.SqlCommand())
                {
                    sqlCMD.CommandText = sql;
                    sqlCMD.CommandTimeout = 0;
                    sqlCMD.CommandType = CommandType.Text;
                    sqlCMD.Connection = sqlConn;

                    IDataReader dr = sqlCMD.ExecuteReader();
                    while (dr.Read())
                    {
                        int rpid = 0, rid = 0;
                        int.TryParse(dr["RetailerProductId"].ToString(), out rpid);
                        int.TryParse(dr["RetailerId"].ToString(), out rid);
                        decimal price = 0m, freight = 0;
                        decimal.TryParse(dr["RetailerPrice"].ToString(), out price);
                        decimal.TryParse(dr["Freight"].ToString(), out freight);

                        RetailerProduct rp = new RetailerProduct();
                        rp.RetailerId = rid;
                        rp.ProductId = pid;
                        rp.RetailerProductId = rpid;
                        rp.RetailerProductName = dr["RetailerProductName"].ToString();
                        rp.RetailerPrice = price;
                        rp.Freight = freight;
                        rps.Add(rp);
                    }
                    dr.Close();
                }
                sqlConn.Close();
            }

            return rps;
        }
    }
}