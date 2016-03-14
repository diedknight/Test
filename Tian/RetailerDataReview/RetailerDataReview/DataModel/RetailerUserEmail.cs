using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetailerDataReview.DataModel
{
    public class RetailerUserEmail:BaseModel
    {
        public int RetailerId { get; set; }
        public string ContactFirstName { get; set; }
        public string ContactLastName { get; set; }
        public string Email { get; set; }

        protected override string ConnectionNameString()
        {
            return "Pam_User";
        }

        public static RetailerUserEmail Get(int retailerId)
        {
            RetailerUserEmail info = new RetailerUserEmail();

            var dr = info.ExecuteReader("select * from CSK_Store_RetailerUserEmail where RetailerId=" + retailerId);

            while (dr.Read())
            {
                info.RetailerId = dr["RetailerId"] == DBNull.Value ? 0 : Convert.ToInt32(dr["RetailerId"]);
                info.ContactFirstName = dr["ContactFirstName"].ToString();
                info.ContactLastName = dr["ContactLastName"].ToString();
                info.Email = dr["Email"].ToString();
            }

            dr.Close();

            return info;
        }

        public static List<RetailerUserEmail> GetList(int retailerId)
        {
            RetailerUserEmail db = new RetailerUserEmail();
            List<RetailerUserEmail> list = new List<RetailerUserEmail>();

            var dr = db.ExecuteReader("select * from CSK_Store_RetailerUserEmail where RetailerId=" + retailerId);

            while (dr.Read())
            {
                RetailerUserEmail info = new RetailerUserEmail();
                info.RetailerId = dr["RetailerId"] == DBNull.Value ? 0 : Convert.ToInt32(dr["RetailerId"]);
                info.ContactFirstName = dr["ContactFirstName"].ToString();
                info.ContactLastName = dr["ContactLastName"].ToString();
                info.Email = dr["Email"].ToString();

                list.Add(info);
            }

            dr.Close();

            return list;
        }

    }
}
