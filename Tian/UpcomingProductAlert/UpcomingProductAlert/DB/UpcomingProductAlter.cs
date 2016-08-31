using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Data.SqlClient;

namespace UpcomingProductAlert.DB
{
    public class UpcomingProductAlter
    {
        public int UpcomingProductAlterID { get; set; }
        public int UpcomingProductID { get; set; }
        public string email { get; set; }
        public bool Status { get; set; }
        public int CountryID { get; set; }

        public static List<UpcomingProductAlter> GetAll()
        {
            string conStr = System.Configuration.ConfigurationManager.ConnectionStrings["Pam_user"].ConnectionString;
            string sql = "select * from UpcomingProductAlter where [status]=0";

            using (SqlConnection con = new SqlConnection(conStr))
            {
                return con.Query<UpcomingProductAlter>(sql).ToList();
            }
        }

    }
}
