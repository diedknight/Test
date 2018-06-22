using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using GEPREmail.Data;
using GEPREmail.EmailTemplate;
using Priceme.Infrastructure.Email;
using PriceMeCrawlerTask.Common.Log;

namespace GEPREmail
{
    class Program
    {
        static void Main(string[] args)
        {

            DateTime curTime = DateTime.Now;
            string logfile1 = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log", curTime.ToString("yyyy_MM_dd_HH_mm") + ".txt");
            string logfile2 = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log", "error_" + curTime.ToString("yyyy_MM_dd_HH_mm") + ".txt");

            List<WeeklyDealsUser> list = new List<WeeklyDealsUser>();

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["PriceMeTemplate"].ConnectionString))
            {

                string userid = ConfigurationManager.AppSettings["UserID"];

                string sql = "select * from WeeklyDealsUser where ID>=" + userid;

                list = con.Query<WeeklyDealsUser>(sql).ToList();
            }

            list.ForEach(item =>
            {
                try
                {
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
                    string title = ConfigurationManager.AppSettings["Title"];

                    IEmail email = new AmazonEmail(emailStr, wSAccessKey, wSSecretKey);
                    email.Send(title, html, item.EmailAddress);

                    XbaiLog.WriteLog(logfile1, item.Id.ToString() + "\t" + item.EmailAddress);
                }
                catch (Exception ex)
                {
                    XbaiLog.WriteLog(logfile2, item.Id + "\t" + item.EmailAddress);
                    XbaiLog.WriteLog(logfile2, ex.Message);
                    XbaiLog.WriteLog(logfile2, ex.StackTrace);

                    throw ex;
                }

            });


        }
    }
}
