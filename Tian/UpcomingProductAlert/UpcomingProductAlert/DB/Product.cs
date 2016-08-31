using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace UpcomingProductAlert.DB
{
    public class Product
    {
        public int ProductId { get; set; }
        public decimal RetailerPrice { get; set; }
        public string ProductName { get; set; }

        public static Product Get(int productId, int countryId)
        {
            string conStr = System.Configuration.ConfigurationManager.ConnectionStrings["Priceme"].ConnectionString;
            string sql = "";
            sql += " select top 1 a.ProductId, a.RetailerPrice, b.ProductName from csk_store_retailerproduct as a";
            sql += " left join csk_store_product as b on a.productId=b.productId";
            sql += " where a.ProductId=" + productId + " and a.RetailerProductStatus=1 and a.IsDeleted=0 and a.RetailerPrice>0.2";
            sql += " and a.RetailerId in (select RetailerId from CSK_Store_Retailer where RetailerStatus=1 and RetailerCountry=" + countryId + ")";

            using (SqlConnection con = new SqlConnection(conStr))
            {
                return con.Query<Product>(sql).SingleOrDefault();
            }      
        }

    }
}
