using Dapper;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Priceme.Deals.Code.Data
{
    public class VoucherCategory
    {
        public static List<Category> GetList()
        {
            string conStr = System.Configuration.ConfigurationManager.ConnectionStrings["CommerceTemplate_Common"].ConnectionString;
            string sql = "select distinct maincategoryid from Deals_Voucher where Status=1 and TIMESTAMPDIFF(day,now(),ValidUntilDate)>=-7";

            List<Category> cateList = new List<Category>();
            List<int> cateIdList = new List<int>();

            using (var con = GetCon(conStr))
            {
                cateIdList = con.Query<int>(sql).ToList();
            }

            conStr = System.Configuration.ConfigurationManager.ConnectionStrings["CommerceTemplate"].ConnectionString;
            sql = "select CategoryID as Id,CategoryName as Name from CSK_Store_Category where categoryid in @Cids";

            using (var con = GetCon(conStr))
            {
                cateList = con.Query<Category>(sql, new { Cids = cateIdList }).ToList();
            }


            return cateList;
        }

        private static IDbConnection GetCon(string conStr)
        {
            return new MySqlConnection(conStr);
        }


        //class
        public class Category
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }
    }
}