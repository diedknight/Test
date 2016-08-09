using PriceMeDBA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImportIP
{
    class Program
    {
        static void Main(string[] args)
        {
            string sqlConnection = System.Configuration.ConfigurationManager.ConnectionStrings["CommerceTemplate"].ConnectionString;

            List<string> ipList = new List<string>();

            string selectSql = string.Format("select distinct UserIP from CSK_Store_RetailerTracker where CreatedOn > '2016-01-01' and UserIP not in (select IPAddress COLLATE DATABASE_DEFAULT as IP from CSK_Store_IP_Blacklist)");

            using (System.Data.SqlClient.SqlConnection sqlCon = new System.Data.SqlClient.SqlConnection(sqlConnection))
            using (System.Data.SqlClient.SqlCommand sqlCmd = new System.Data.SqlClient.SqlCommand(selectSql, sqlCon))
            {
                sqlCon.Open();

                sqlCmd.CommandTimeout = 0;

                using (System.Data.SqlClient.SqlDataReader sqlDR = sqlCmd.ExecuteReader())
                {
                    while (sqlDR.Read())
                    {
                        string ip = sqlDR.GetString(0);
                        if (!string.IsNullOrEmpty(ip))
                        {
                            ipList.Add(ip);
                        }
                    }
                }
            }

            ipList.ForEach(ip => {

                int ipInt = (int)IpToInt(ip);

                if (ipInt == 0) return;

                CSK_Store_IP_Address ipInfo = new CSK_Store_IP_Address();
                ipInfo.IPAddress = ip;
                ipInfo.IPInt = ipInt;

                ipInfo.Save();
            });
        }

        private static long IpToInt(string ip)
        {
            string[] items = ip.Split('.');

            if (items.Length != 4) return 0;

            items[3] = "0";

            return long.Parse(items[0]) << 24
                    | long.Parse(items[1]) << 16
                    | long.Parse(items[2]) << 8
                    | long.Parse(items[3]);
        }

        

    }
}
