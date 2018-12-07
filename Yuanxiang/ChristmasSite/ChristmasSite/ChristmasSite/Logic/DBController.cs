using ChristmasSite.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ChristmasSite.Logic
{
    public static class DBController
    {
        public static List<CategoryData> listCates { get; set; }

        public static void LoadCategory()
        {
            listCates = new List<CategoryData>();

            string sql = "select distinct(cid), CategoryName from BlackFriday p left join BlackFriday_tracker t on p.rpid = t.retailerproductid order by CategoryName";
            
            using (SqlConnection conn = new SqlConnection(SiteConfig.ConnectionStrings("conn_EDW")))
            {
                conn.Open();
                using (SqlCommand comm = new SqlCommand(sql, conn))
                {
                    using (var idr = comm.ExecuteReader())
                    {
                        while (idr.Read())
                        {
                            int cid = 0;
                            int.TryParse(idr["cid"].ToString(), out cid);
                            string cname = idr["CategoryName"].ToString();

                            CategoryData c = new CategoryData();
                            c.CategoryId = cid;
                            c.CategoryName = cname;
                            listCates.Add(c);
                        }
                    }
                }
            }
        }

        public static List<ProductCatalog> GetBlackProducts(int cid, int pageindex, int pagesize, string sb)
        {
            List<ProductCatalog> datas = new List<ProductCatalog>();

            string sql = "select * from BlackFriday p left join BlackFriday_tracker t on p.rpid = t.retailerproductid";
            if (cid > 0)
                sql = sql + " Where p.CID = " + cid;

            if (sb == "Clicks")
                sql += " order by tracks desc";
            else if (sb == "BestPrice")
                sql += " order by newprice";
            else
                sql += " order by rate";

            string spsql = "Exec sp_les_AllowPaging " + pageindex + ", " + pagesize + ", '" + sql + "'";

            using (SqlConnection conn = new SqlConnection(SiteConfig.ConnectionStrings("conn_EDW")))
            {
                conn.Open();
                using (SqlCommand comm = new SqlCommand(spsql, conn))
                {
                    using (var idr = comm.ExecuteReader())
                    {
                        while (idr.Read())
                        {
                            ProductCatalog pc = DbConvertController<ProductCatalog>.ReadDataFromDataReader(idr);
                            datas.Add(pc);
                        }
                    }
                }
            }

            return datas;
        }

        public static int GetBlackProductsCount(int cid)
        {
            int total = 0;

            string sql = "select count(rpid) as total from BlackFriday p left join BlackFriday_tracker t on p.rpid = t.retailerproductid";
            if (cid > 0)
                sql = sql + " Where p.CID = " + cid;
            
            using (SqlConnection conn = new SqlConnection(SiteConfig.ConnectionStrings("conn_EDW")))
            {
                conn.Open();
                using (SqlCommand comm = new SqlCommand(sql, conn))
                {
                    using (var idr = comm.ExecuteReader())
                    {
                        while (idr.Read())
                        {
                            int.TryParse(idr["total"].ToString(), out total);
                        }
                    }
                }
            }

            return total;
        }

        public static string GetChristmasInfomation()
        {
            string info = string.Empty;
            string sql = "select Ctx from CSK_Content where PageId = 'Christmas information'";

            using (SqlConnection conn = new SqlConnection(SiteConfig.ConnectionStrings("conn_EDW")))
            {
                conn.Open();
                using (SqlCommand comm = new SqlCommand(sql, conn))
                {
                    using (var idr = comm.ExecuteReader())
                    {
                        while (idr.Read())
                        {
                            info = idr["Ctx"].ToString();
                        }
                    }
                }
            }

            return info;
        }

        public static void NewsletterSignup(string email)
        {
            string sql = "INSERT INTO WeeklyDealsUser (emailaddress) VALUES ('"+ email + "')";

            using (SqlConnection conn = new SqlConnection(SiteConfig.ConnectionStrings("conn_EDW")))
            {
                conn.Open();
                using (SqlCommand comm = new SqlCommand(sql, conn))
                {
                    comm.ExecuteNonQuery();
                }
            }
        }
    }
}
