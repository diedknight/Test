using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Net;

namespace MonitoringManufacturerProductLinks
{
    class Program
    {
        static void Main(string[] args)
        {

            //UrlInfo urlInfo1 = new UrlInfo();
            //urlInfo1.Url = "http://www.123.com.my/";

            //urlInfo1.VerifyUrl();


            List<UrlInfo> urlInfoList = new List<UrlInfo>();

            Console.WriteLine("load url from db");
            TableInfo.GetInfos().ForEach(tableInfo =>
            {
                string sql = string.Format("select distinct {0} from {1} where {2}=1", tableInfo.UrlColumn, tableInfo.TableName, tableInfo.StatusColumn);

                using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CommerceTemplate"].ConnectionString))
                {
                    con.Open();

                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                UrlInfo urlInfo = new UrlInfo();
                                urlInfo.TableInfo = tableInfo;
                                //urlInfo.Id = reader[tableInfo.IdColumn].ToString();
                                //urlInfo.Status = reader[tableInfo.StatusColumn].ToString();
                                urlInfo.Url = reader[tableInfo.UrlColumn].ToString();

                                urlInfoList.Add(urlInfo);
                            }
                        }
                    }
                }
            });

            Console.WriteLine("load end");

            PaymentToosCVS.Log.Log log = new PaymentToosCVS.Log.Log(DateTime.Now.ToString("yyyy-MM-dd"));

            urlInfoList.ForEach(urlInfo => {
                if (!urlInfo.VerifyUrl())
                {
                    log.Add(urlInfo.Url + "\tword:" + urlInfo.Word);
                    urlInfo.Save();
                }
            });

            log.Write();
            log.SendEmail();
        }

        

    }
}
