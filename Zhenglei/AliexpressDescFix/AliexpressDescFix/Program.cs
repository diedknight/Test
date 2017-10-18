using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AliexpressDescFix
{
    class Program
    {
        static void Main(string[] args)
        {
            string relatedProductFormatFilePath = System.Configuration.ConfigurationManager.AppSettings["RelatedProductFormatFile"];
            List<List<string>> relatedProductFormatList = GetRelatedProductFormatList(relatedProductFormatFilePath);

            string shopDBConnStr = System.Configuration.ConfigurationManager.ConnectionStrings["PriceMe_Shop"].ConnectionString;
            Dictionary<int, string> productInfoDic = GetProductInfoDic(shopDBConnStr);

            FixAndSaveDesc(productInfoDic, shopDBConnStr, relatedProductFormatList);
        }

        private static void FixAndSaveDesc(Dictionary<int, string> productInfoDic, string shopDBConnStr, List<List<string>> relatedProductFormatList)
        {
            string updateSqlStr = "UPDATE [dbo].[Product] SET [FullDescription] = @desc WHERE Id = @id";
            using (System.Data.SqlClient.SqlConnection sqlConn = new System.Data.SqlClient.SqlConnection(shopDBConnStr))
            {
                sqlConn.Open();
                foreach (int pid in productInfoDic.Keys)
                {
                    string desc = productInfoDic[pid];
                    string fixedDesc = GetFixDesc(desc, relatedProductFormatList);

                    if (desc != fixedDesc)
                    {
                        using (System.Data.SqlClient.SqlCommand sqlCmd = new System.Data.SqlClient.SqlCommand(updateSqlStr, sqlConn))
                        {
                            try
                            {
                                sqlCmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@desc", fixedDesc));
                                sqlCmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@id", pid));
                                sqlCmd.ExecuteNonQuery();
                            }
                            catch(Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }
                    }
                }
            }
        }

        private static Dictionary<int, string> GetProductInfoDic(string shopDBConnStr)
        {
            Dictionary<int, string> dic = new Dictionary<int, string>();
            string sqlStr = "SELECT [Id],[FullDescription] FROM .[dbo].[Product] where FullDescription is not null";
            using (System.Data.SqlClient.SqlConnection sqlConn = new System.Data.SqlClient.SqlConnection(shopDBConnStr))
            using (System.Data.SqlClient.SqlCommand sqlCmd = new System.Data.SqlClient.SqlCommand(sqlStr, sqlConn))
            {
                sqlConn.Open();
                using (System.Data.SqlClient.SqlDataReader sqlDR = sqlCmd.ExecuteReader())
                {
                    while(sqlDR.Read())
                    {
                        dic.Add(sqlDR.GetInt32(0), sqlDR.GetString(1));
                    }
                }
            }
            return dic;
        }

        private static List<List<string>> GetRelatedProductFormatList(string relatedProductFormatFilePath)
        {
            //内容格式 标签名，属性名，属性值 之间用Tab符号隔开
            List<List<string>> formatList = new List<List<string>>();
            using (System.IO.StreamReader sr = new System.IO.StreamReader(relatedProductFormatFilePath))
            {
                string line = sr.ReadLine();
                while (!string.IsNullOrEmpty(line))
                {
                    string[] infos = line.Split(new string[] { "\t" }, StringSplitOptions.RemoveEmptyEntries);
                    List<string> formats = new List<string>();
                    formats.AddRange(infos);
                    formatList.Add(formats);

                    line = sr.ReadLine();
                }
            }

            return formatList;
        }

        private static string GetFixDesc(string fullDescription, List<List<string>> relatedProductFormatList)
        {
            string html = fullDescription;
            CsQuery.CQ cq = CsQuery.CQ.Create(html);

            bool removed = false;
            foreach (var formats in relatedProductFormatList)
            {
                if (cq[formats[0]] != null)
                {
                    var divList = cq[formats[0]].ToList();
                    foreach (var div in divList)
                    {
                        string style = div.Attributes[formats[1]];
                        if (style.Equals(formats[2], StringComparison.InvariantCultureIgnoreCase))
                        {
                            div.Remove();
                            removed = true;
                            break;
                        }
                    }
                }
                if (removed) break;
            }

            string newHtml = cq.Render();
            return newHtml;
        }
    }
}