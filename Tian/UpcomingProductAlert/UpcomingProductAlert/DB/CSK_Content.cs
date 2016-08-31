using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace UpcomingProductAlert.DB
{
    public class CSK_Content
    {
        public int ID { get; set; }
        public string PageID { get; set; }
        public string MetaDescription { get; set; }
        public string MetaKeywords { get; set; }
        public string Title { get; set; }
        public string Ctx { get; set; }
        public string FromEmail { get; set; }
        public string FromName { get; set; }
        public string ReplyTo { get; set; }
        public string Type { get; set; }
        public string CC { get; set; }
        public string BCC { get; set; }

        public static CSK_Content Get()
        {
            string conStr = System.Configuration.ConfigurationManager.ConnectionStrings["Priceme"].ConnectionString;
            string sql="select top 1 * from CSK_Content where PageID=@PageID";

            using (SqlConnection con = new SqlConnection(conStr))
            {
                var info = con.Query<CSK_Content>(sql, new { PageID = "Upcoming product alert" }).SingleOrDefault();

                string html = "";
                html+="<!DOCTYPE html>";
                html+="<html xmlns=\"http://www.w3.org/1999/xhtml\">";
                html+="<head>";
                html+="<title></title>";
                html+="</head>";
                html+="<body>";
                html += info.Ctx;
                html+="</body>";
                html+="</html>";

                info.Ctx = html;

                return info;
            }            
        }

        public void TitleReplace(string oldValue, string newValue)
        {
            if (string.IsNullOrEmpty(oldValue)) return;

            this.Title = this.Title.Replace(oldValue, newValue);
        }

        public void CtxReplace(string oldValue, string newValue)
        {
            if (string.IsNullOrEmpty(oldValue)) return;

            this.Ctx = this.Ctx.Replace(oldValue, newValue);
        }

    }
}
