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
        private static string _conStr = System.Configuration.ConfigurationManager.ConnectionStrings["Pam_user"].ConnectionString;

        public int UpcomingProductAlterID { get; set; }
        public int UpcomingProductID { get; set; }
        public string email { get; set; }
        public bool Status { get; set; }
        public int CountryID { get; set; }

        public static List<UpcomingProductAlter> GetAll()
        {            
            string sql = "select * from UpcomingProductAlter where [status]=0";

            using (SqlConnection con = new SqlConnection(_conStr))
            {
                return con.Query<UpcomingProductAlter>(sql).ToList();
            }
        }

        public void UpdateStatus(bool status)
        {
            if (this.Status == status) return;

            string sql = "update UpcomingProductAlter set [status]=@Status where UpcomingProductAlterID=@UpcomingProductAlterID";

            using (SqlConnection con = new SqlConnection(_conStr))
            {
                con.Execute(sql, new { Status = status, UpcomingProductAlterID = this.UpcomingProductAlterID });
            }

            this.Status = status;
        }

    }
}
