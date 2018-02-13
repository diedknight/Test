using MapExistingRp.Data;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace MapExistingRp
{
    public class DB
    {
        public static SqlConnection DBConnection
        {
            get
            {
                return new SqlConnection(AppConfig.DBConStr);
            }
        }


        public static List<ProductData> GetProduct(List<int> cids)
        {
            List<ProductData> list = new List<ProductData>();

            using (var sr = System.IO.File.OpenText(Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "Sql", "ProductSql.sql")))
            {
                string sql = sr.ReadToEnd();

                using (var con = DBConnection)
                {
                    list = con.Query<ProductData>(sql, new { CIds = cids }, null, true, 40000).ToList();
                }
            }

            return list;
        }


    }
}
