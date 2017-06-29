using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Data.SqlClient;
using System.Configuration;

namespace PriceMeCommon.Data
{
    public class DealsVoucher
    {
        public int Id { get; set; }
        public string VoucherName { get; set; }
        public string VoucherUrl { get; set; }
        public string Image { get; set; }
        public string StoreName { get; set; }
        public int RetailerId { get; set; }
        public string Value { get; set; }
        public string CouponCode { get; set; }
        public string Description { get; set; }
        public int MainCategoryId { get; set; }
        public DateTime ValidStartDate { get; set; }
        public DateTime ValidUntilDate { get; set; }
        public int Status { get; set; }
        public string SuggestedByUserEmail { get; set; }
        public DateTime SuggestOnDate { get; set; }
        public bool SuggestByRetailer { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
        public int WillExpire { get; set; }
        public string RetailerLogo { get; set; }
        public bool ExpireSoon { get; set; }


        public static string GetUrl(int id)
        {
            string conStr = System.Configuration.ConfigurationManager.ConnectionStrings["CommerceTemplate_Common"].ConnectionString;
            string sql = "select VoucherUrl from Deals_Voucher where Id=" + id;

            string url = "";

            using (SqlConnection con = new SqlConnection(conStr))
            {
                url = con.ExecuteScalar<string>(sql);
            }

            return url;
        }

        public static List<DealsVoucher> GetList(List<int> cids, string orderby, int pageNum, int pageSize)
        {
            List<DealsVoucher> list = new List<DealsVoucher>();

            if (cids == null) cids = new List<int>();

            string conStr = ConfigurationManager.ConnectionStrings["CommerceTemplate_Common"].ConnectionString;

            string sql1 = "select top " + pageSize + " * from (";
            sql1 += "   select row_number() over(order by {1}) as row_num,* from (";
            sql1 += "       select DATEDIFF(day,getdate(),ValidUntilDate) as willExpire,";
            sql1 += "       (case when DATEDIFF(HOUR,getdate(),ValidUntilDate)<=24 and DATEDIFF(HOUR,getdate(),ValidUntilDate)>=0 then 1 else 0 end) as expireSoon,";
            sql1 += "       v.*,r.LogoFile as RetailerLogo from Deals_Voucher as v left join CSK_Store_Retailer as r on v.RetailerId=r.RetailerId";
            sql1 += "   ) as b {0}";
            sql1 += " ) as a where row_num>" + (pageNum - 1) * pageSize;           

            string order = "id desc";
            string where = " where [status]=1";

            where += " and willExpire>=-7";

            if (cids.Count != 0) where += " and MainCategoryId in @CIds";
            //if (orderby == "soon") where += " and willExpire<" + System.Convert.ToInt32(ConfigurationManager.AppSettings["expireSoonSpan"]);

            if (orderby == "soon") order = " expireSoon desc";

            sql1 = string.Format(sql1, where, order);

            using (SqlConnection con = new SqlConnection(conStr))
            {
                list = con.Query<DealsVoucher>(sql1, new { CIds = cids }).ToList();
            }

            return list;
        }

        public static int GetCount(List<int> cids)
        {
            if (cids == null) cids = new List<int>();

            string conStr = System.Configuration.ConfigurationManager.ConnectionStrings["CommerceTemplate_Common"].ConnectionString;

            string sql = "select count(1) from Deals_Voucher where [Status]=1 and DATEDIFF(day,getdate(),ValidUntilDate)>=-7";
            if (cids.Count != 0) sql += " and MainCategoryId in @CIds";

            using (SqlConnection con = new SqlConnection(conStr))
            {
                var count = con.ExecuteScalar<int>(sql, new { CIds = cids });

                return count;
            }
        }

        public static void Insert(string code, string des, string email, string storeNmae, DateTime until, string value, string voucherName, string voucherUrl)
        {
            string conStr = System.Configuration.ConfigurationManager.ConnectionStrings["CommerceTemplate_Common_Write"].ConnectionString;
            string sql = "insert into Deals_Voucher ([VoucherName],[VoucherUrl],[Image],[StoreName],[RetailerId],[Value],[CouponCode],[Description],[MainCategoryId],[ValidStartDate],[ValidUntilDate],[Status],[SuggestedByUserEmail],[SuggestOnDate],[SuggestByRetailer],[ModifiedOn],[ModifiedBy])     values(@voucherName,@voucherUrl,'',@storeName,0,@val,@code,@des,0,getdate(),@until,2,@email,getdate(),0,getdate(),'')";

            using (SqlConnection con = new SqlConnection(conStr))
            {
                con.Execute(sql, new { storeName = storeNmae, val = value, code = code, des = des, until = until, email = email, voucherName = voucherName, voucherUrl = voucherUrl });
            }
        }

    }
}
