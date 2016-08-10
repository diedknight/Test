using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MonitoringManufacturerProductLinks
{
    public class UrlInfo
    {        
        public TableInfo TableInfo { get; set; }
        public string Url { get; set; }
        public string Status { get; set; }
        public string Id { get; set; }

        public UrlInfo()
        {
            this.Url = "";
            this.Status = "0";
            this.Id = "";
        }

        public bool VerifyUrl()
        {
            if (string.IsNullOrEmpty(this.Url)) return false;
            
            string html = this.GetHttpContent(this.Url);
            if (Keywords.Exist(html))
            {
                this.Status = "0";
                Console.WriteLine("verify url:" + this.Url + " status:" + this.Status);

                return false;
            }

            this.Status = "1";
            Console.WriteLine("verify url:" + this.Url + " status:" + this.Status);

            return true;
        }

        public void Save()
        {
            if (string.IsNullOrEmpty(this.Url)) return;

            string tempUrl = this.Url.Replace("'", "''");
            if (this.Status == "0")
            {
                string sql = string.Format("Update {0} set {1}=0 where {2}='{3}'", this.TableInfo.TableName, this.TableInfo.StatusColumn, this.TableInfo.UrlColumn, tempUrl);

                using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CommerceTemplate"].ConnectionString))
                {
                    con.Open();

                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        private string GetHttpContent(string url)
        {
            string httpString = "";

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

                request.Method = "GET";
                request.UserAgent = "Mozilla/5.0 (Windows NT 6.2; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/27.0.1453.94 Safari/537.36";
                //request.KeepAlive = false;
                //request.ContentType = "application/x-www-form-urlencoded";

                request.Timeout = 100000;
                request.CookieContainer = new CookieContainer();
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                System.IO.StreamReader streamReader = new System.IO.StreamReader(response.GetResponseStream());

                httpString = streamReader.ReadToEnd();
            }
            catch (Exception ex) { httpString = ex.Message + " " + ex.StackTrace; }

            return httpString;
        }

    }
}
