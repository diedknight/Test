using IpAnalytics.Config;
using PriceMeCrawlerTask.Common.Log;
using PriceMeDBA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IpAnalytics
{
    class Program
    {
        private static DateTime CurRunTime = DateTime.Now;

        static void Main(string[] args)
        {
            //string ipInfo = GetIPInfo("210.55.213.185"); //NZ
            //string ipInfo2 = GetIPInfo("58.6.0.22"); //AU

            Console.WriteLine("读取数据库...");

            JobConfig.Load("ipAnalytics");
            List<string> ipList = GetIPs();

            AnalyticAndWriteLog(ipList);

            JobConfig.SetValue("prevRunningTime", CurRunTime.ToString("yyyy-MM-dd HH:mm:ss"));

            //Console.WriteLine("分析IP地址...");
            //Dictionary<string, List<string>> ipDic = GetIPInfosDic(ipList);

            //Console.WriteLine("写日志...");
            //WriteLog(ipDic);
        }

        /// <summary>
        /// 分析IP并写日志
        /// </summary>
        /// <param name="ipList"></param>
        private static void AnalyticAndWriteLog(List<string> ipList)
        {
            string exclude = System.Configuration.ConfigurationManager.AppSettings["Exclude"];
            string[] excludeInfos = exclude.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            string logPath = System.Configuration.ConfigurationManager.AppSettings["LogPath"];
            string logFile = DateTime.Now.ToString("yyyy-MM-dd_HH") + ".txt";
            string logFilePath = System.IO.Path.Combine(logPath, logFile);

            int whiteCount = 0;

            using (LogWriter logWriter = new LogWriter(logFilePath))
            {
                List<string> allIPs = new List<string>();
                foreach (string ip in ipList)
                {
                    
                    string ipInfo = GetIPInfo(ip);
                    string[] infos = ipInfo.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    string msg = null;
                    

                    if (string.IsNullOrEmpty(ipInfo))
                    {
                        msg = "UnKown\t" + ip;
                        allIPs.Add(ip);
                    }
                    else
                    {                        
                        if (!excludeInfos.Contains(infos[0]))
                        {
                            msg = infos[0] + "\t" + ip;
                            allIPs.Add(ip);

                            PriceMeDBA.CSK_Store_IP_Blacklist blackIP = PriceMeDBA.CSK_Store_IP_Blacklist.SingleOrDefault(item => item.IPAddress == ip);
                            if (blackIP == null)
                            {
                                blackIP = new PriceMeDBA.CSK_Store_IP_Blacklist();
                                blackIP.IPAddress = ip;
                                blackIP.CreatedOn = DateTime.Now;
                                blackIP.Save();
                            }
                        }
                        else
                        {

                            CSK_Store_IP_Address ipAddrInfo = new CSK_Store_IP_Address();
                            ipAddrInfo.IPAddress = ip;
                            ipAddrInfo.IPInt = (int)IpToInt(ip);
                            ipAddrInfo.Save();                            
                        }
                    }

                    PriceMeCrawlerTask.Common.Log.XbaiLog.WriteLog(System.IO.Path.Combine(logPath, string.Format("IPnumber-{0}", logFile)), ip + " " + infos[0]);
                    whiteCount++;

                    if (!string.IsNullOrEmpty(msg))
                    {
                        logWriter.WriteLog(msg);
                        Console.WriteLine(msg);
                    }
                }

                logWriter.WriteLog("All IP : ");
                logWriter.WriteLog(string.Join(",", allIPs));

                PriceMeCrawlerTask.Common.Log.XbaiLog.WriteLog(System.IO.Path.Combine(logPath, string.Format("IPnumber-{0}", logFile)), "Sum: " + whiteCount);
            }
        }

        private static void WriteLog(Dictionary<string, List<string>> ipDic)
        {
            string logPath = System.Configuration.ConfigurationManager.AppSettings["LogPath"];
            string logFile = DateTime.Now.ToString("yyyy-MM-dd_HH") + ".txt";
            string logFilePath = System.IO.Path.Combine(logPath, logFile);

            List<string> allIPs = new List<string>();
            using (System.IO.StreamWriter sw = new System.IO.StreamWriter(logFilePath, false))
            {
                foreach(string key in ipDic.Keys)
                {
                    List<string> ips = ipDic[key];
                    allIPs.AddRange(ips);
                    foreach (string ip in ips)
                    {
                        sw.WriteLine(key + "\t" + ip);
                    }
                }

                sw.WriteLine("All IP : ");
                sw.WriteLine(string.Join(",", allIPs));
            }
        }

        private static Dictionary<string, List<string>> GetIPInfosDic(List<string> ipList)
        {
            Dictionary<string, List<string>> dic = new Dictionary<string, List<string>>();
            dic.Add("UnKown", new List<string>());
            string exclude = System.Configuration.ConfigurationManager.AppSettings["Exclude"];
            string[] excludeInfos = exclude.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            foreach(string ip in ipList)
            {
                string ipInfo = GetIPInfo(ip);
                if (string.IsNullOrEmpty(ipInfo))
                {
                    dic["UnKown"].Add(ip);
                }
                else
                {
                    string[] infos = ipInfo.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    if (!excludeInfos.Contains(infos[0]))
                    {
                        if (dic.ContainsKey(infos[0]))
                        {
                            dic[infos[0]].Add(ip);
                        }
                        else
                        {
                            List<string> ips = new List<string>();
                            ips.Add(ip);
                            dic.Add(infos[0], ips);
                        }
                    }
                }
            }

            return dic;
        }

        /// <summary>
        /// 获取所有需要解析的IP
        /// </summary>
        /// <returns></returns>
        private static List<string> GetIPs()
        {
            string logPath = System.Configuration.ConfigurationManager.AppSettings["LogPath"];
            string logFile = DateTime.Now.ToString("yyyy-MM-dd_HH") + ".txt";

            List<string> ipList = new List<string>();
            List<string> tempIpList = new List<string>();            

            string selectSqlFormat = @"
                                    select distinct UserIP COLLATE DATABASE_DEFAULT as IP from CSK_Store_RetailerTracker where CreatedOn between {0} 
                                    and UserIP not in (select IPAddress COLLATE DATABASE_DEFAULT as IP from CSK_Store_IP_Blacklist)                                      
                                    and RetailerId in (select RetailerId from CSK_Store_Retailer where RetailerCountry = {1}) 
                                    ";

            string sqlConnection = System.Configuration.ConfigurationManager.ConnectionStrings["CommerceTemplate"].ConnectionString;
            
            //string dateRange = System.Configuration.ConfigurationManager.AppSettings["DateRange"];
            string dateRange = "'" + JobConfig.GetValue("prevRunningTime") + "' and '" + CurRunTime.ToString("yyyy-MM-dd HH:mm:ss") + "'";

            string countryID = System.Configuration.ConfigurationManager.AppSettings["CountryID"];
            string selectSql = string.Format(selectSqlFormat, dateRange, countryID);            

            using (System.Data.SqlClient.SqlConnection sqlCon = new System.Data.SqlClient.SqlConnection(sqlConnection))
            using (System.Data.SqlClient.SqlCommand sqlCmd = new System.Data.SqlClient.SqlCommand(selectSql, sqlCon))
            {
                sqlCon.Open();

                sqlCmd.CommandTimeout = 0;

                using (System.Data.SqlClient.SqlDataReader sqlDR = sqlCmd.ExecuteReader())
                {
                    while(sqlDR.Read())
                    {
                        string ip = sqlDR.GetString(0);
                        if (!string.IsNullOrEmpty(ip))
                        {
                            ipList.Add(ip);
                        }
                    }
                }
            }

            ipList.ForEach(ipStr =>
            {
                int ipInt = (int)IpToInt(ipStr);
                var ipInfo = CSK_Store_IP_Address.SingleOrDefault(item => item.IPInt == ipInt);

                if (ipInfo == null) tempIpList.Add(ipStr);
                else PriceMeCrawlerTask.Common.Log.XbaiLog.WriteLog(System.IO.Path.Combine(logPath, string.Format("nocheckip-{0}", logFile)), ipStr + " , int:" + ipInt);

                //休息一下
                System.Threading.Thread.Sleep(200);
            });           

            return tempIpList;
        }

        /// <summary>
        /// 使用maxmind的API获取ip的地址信息
        /// </summary>
        /// <param name="IP"></param>
        /// <returns></returns>
        static string GetIPInfo(string IP)
        {
            string license_key = "nA0fQfDH4zKt";
            System.Uri objUrl = new System.Uri("http://geoip.maxmind.com/b?l=" + license_key + "&i=" + IP);
            System.Net.WebRequest objWebReq;
            System.Net.WebResponse objResp;
            System.IO.StreamReader sReader;
            string strReturn = string.Empty;

            //Try to connect to the server and retrieve data. 
            try
            {
                objWebReq = System.Net.WebRequest.Create(objUrl);
                objResp = objWebReq.GetResponse();

                //Get the data and store in a return string. 
                sReader = new System.IO.StreamReader(objResp.GetResponseStream());
                strReturn = sReader.ReadToEnd();

                //Close the objects. 
                sReader.Close();
                objResp.Close();
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {                
                objWebReq = null;
            }

            return strReturn;
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

        private static string IntToIp(long ipInt)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append((ipInt >> 24) & 0xFF).Append(".");
            sb.Append((ipInt >> 16) & 0xFF).Append(".");
            sb.Append((ipInt >> 8) & 0xFF).Append(".");
            sb.Append(ipInt & 0xFF);
            return sb.ToString();
        }

    }
}
