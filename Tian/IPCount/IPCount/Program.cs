using IpAnalytics;
using IpAnalytics.Config;
using IPCount.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Data.SqlClient;
using MaxMind.GeoIP2;
using System.IO;
using Priceme.Infrastructure.Excel;

namespace IPCount
{
    class Program
    {
        private static DateTime CurRunTime = DateTime.Now;

        static void Main(string[] args)
        {

            if (AppConfig.DownloadMaxMindDB)
            {
                MaxMindDBDownload.Download();
            }

            Console.WriteLine("读取数据库...");

            Dictionary<string, OutPutData> outputDataDict = new Dictionary<string, OutPutData>();

            AppConfig.DateRange.ForEach(range =>
            {
                var ipList = GetIPs(range);
                ipList.ForEach(item =>
                {
                    string country = "";

                    string ip = item.IP;
                    string ipInfo = GetIPInfo(ip);
                    string[] infos = ipInfo.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                    if (infos.Length == 0) infos = new string[1] { "" };

                    if (string.IsNullOrEmpty(ipInfo) || ipInfo == ",")
                    {
                        country = "UnKown";
                    }
                    else
                    {
                        country = infos[0];
                    }

                    if (AppConfig.Country.Count == 0 || AppConfig.Country.Exists(c => c.ToLower() == country.ToLower()))
                    {
                        if (item.Count >= AppConfig.Count)
                        {
                            if (outputDataDict.ContainsKey(ip))
                            {
                                outputDataDict[ip].Add(range.Month, item.Count);
                            }
                            else
                            {
                                OutPutData data = new OutPutData(ip, country, range.Month, item.Count);
                                outputDataDict.Add(ip, data);
                            }
                        }
                    }
                });
            });

            ExcelSimpleHelper sh = new ExcelSimpleHelper();

            List<string> headList = new List<string>() { "IP adrress", "country", };
            headList.AddRange(AppConfig.DateRange.Select(item => "clicks " + item.Month));

            sh.WriteLine(headList.ToArray());

            foreach (var item in outputDataDict)
            {
                var data = item.Value;

                List<string> bodyList = new List<string>();
                bodyList.Add(data.IP);
                bodyList.Add(data.Country);
                AppConfig.DateRange.ForEach(range => { bodyList.Add(data.GetCount(range.Month).ToString()); });

                sh.WriteLine(bodyList.ToArray());
            }

            sh.Save(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "output.xls"));
        }

        private static List<TrackIP> GetIPs(DateRange dateRange)
        {
            List<TrackIP> ipList = new List<TrackIP>();

            string sql = " select UserIP as IP,count(*) as [Count] from CSK_Store_RetailerTracker";
            sql += " where UserIP in (select distinct IPAddress from CSK_Store_IP_Blacklist)";
            sql += " and CreatedOn>='" + dateRange.Start + "' and CreatedOn<='" + dateRange.End + "'";
            sql += " group by UserIP";

            string conStr = System.Configuration.ConfigurationManager.ConnectionStrings["CommerceTemplate"].ConnectionString;

            using (SqlConnection con = new SqlConnection(conStr))
            {
                ipList = con.Query<TrackIP>(sql).ToList();
            }

            return ipList;
        }


        private static string GetIPInfo(string IP)
        {
            string str = "";

            using (var reader = new DatabaseReader(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "GeoLite2-Country.mmdb")))
            {
                try
                {
                    str = reader.Country(IP).Country.IsoCode + ",";
                }
                catch { }
            }

            return str;
        }

    }
}
