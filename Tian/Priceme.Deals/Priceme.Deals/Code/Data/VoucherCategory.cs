using Dapper;
using System;
using System.Collections.Generic;
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
            string sql = "select distinct maincategoryid from Deals_Voucher where [Status]=1 and DATEDIFF(day,getdate(),ValidUntilDate)>=-7";

            List<Category> cateList = new List<Category>();
            List<int> cateIdList = new List<int>();

            using (SqlConnection con = new SqlConnection(conStr))
            {
                cateIdList = con.Query<int>(sql).ToList();
            }

            conStr = System.Configuration.ConfigurationManager.ConnectionStrings["CommerceTemplate"].ConnectionString;
            sql = "select CategoryID as Id,CategoryName as Name from CSK_Store_Category where categoryid in @Cids";

            using (SqlConnection con = new SqlConnection(conStr))
            {
                cateList = con.Query<Category>(sql, new { Cids = cateIdList }).ToList();
            }


            return cateList;
        }

        //class
        public class Category
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }
    }
}