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
    }
}
