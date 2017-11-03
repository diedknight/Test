using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Dapper;

namespace Report4SmithsCity
{
    public class Report
    {
        public int ProductId { get; set; }
        public decimal RetailerPrice { get; set; }
        public int RetailerId { get; set; }
        public string RetailerProductSKU { get; set; }
        public string ProductName { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public decimal Lowest { get; set; }
        public decimal Price1 { get; set; }
        public decimal Price2 { get; set; }
        public decimal Price3 { get; set; }
        public decimal Price4 { get; set; }
        public decimal Price5 { get; set; }
        public decimal Price6 { get; set; }
        public decimal Price7 { get; set; }
        public decimal Price8 { get; set; }


        public void SendMail()
        {
            using (MemoryStream ms = new MemoryStream(this.WriteToExcel()))
            {
                string userId = ConfigurationManager.AppSettings["Email_UserId"];

                var shipInfo = aspnet_MembershipInfo.SingleOrDefault(item => item.UserID == userId);
                var ship = aspnet_Membership.SingleOrDefault(item => item.UserId == Guid.Parse(userId));

                string html = Channelyser.Job.EmailTemplate.Template.Load("action.html");

                html = html.Replace("{firstName}", shipInfo.FirstName);

                string emailStr = ConfigurationManager.AppSettings["Email"];
                string wSAccessKey = ConfigurationManager.AppSettings["WSAccessKey"];
                string wSSecretKey = ConfigurationManager.AppSettings["WSSecretKey"];

                IEmail email = new AmazonEmail(emailStr, wSAccessKey, wSSecretKey);
                email.addAttachment(ms, "report.xls");
                email.Send("Weekly intelligence report from PriceMe", html, ship.Email);
            }
        }


        public static List<Report> Get()
        {
            List<Report> list = new List<Report>();

            using (var sr = System.IO.File.OpenText(Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "Report.sql")))
            {
                string sql = sr.ReadToEnd();

                using (var conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["PriceMeDB"].ConnectionString))
                {
                    list = conn.Query<Report>(sql, null, null, true, 4000).ToList();
                }
            }

            return list;
        }

    }
}
