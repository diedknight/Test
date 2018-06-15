using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using GEPREmail.Data;
using GEPREmail.EmailTemplate;
using Priceme.Infrastructure.Email;

namespace GEPREmail
{
    class Program
    {
        static void Main(string[] args)
        {
            List<WeeklyDealsUser> list = new List<WeeklyDealsUser>();

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["PriceMeTemplate"].ConnectionString))
            {
                string sql = "select * from WeeklyDealsUser";

                list = con.Query<WeeklyDealsUser>(sql).ToList();
            }

            list.ForEach(item => {
                if (string.IsNullOrEmpty(item.EmailAddress))
                {
                    return;
                }

                item.Name = item.Name ?? "";

                string html = Template.Load("GDPR.html");
                html = html.Replace("{firstname}", item.Name);
                html = html.Replace("{id}", item.Id.ToString());

                //发送邮件
                string emailStr = ConfigurationManager.AppSettings["Email"];
                string wSAccessKey = ConfigurationManager.AppSettings["WSAccessKey"];
                string wSSecretKey = ConfigurationManager.AppSettings["WSSecretKey"];
                string title= ConfigurationManager.AppSettings["Title"];

                IEmail email = new AmazonEmail(emailStr, wSAccessKey, wSSecretKey);
                email.Send(title, html, item.EmailAddress);
            });


        }
    }
}
