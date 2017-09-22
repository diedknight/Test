using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductStorageAttr
{
    class Program
    {
        static string ConnStr_Static = ConfigurationManager.ConnectionStrings["PriceMe_PM"].ConnectionString;
        static LogWriter LogWriter_Static;
        static List<string> KeywordsList_Static;
        static int MaxCount_Static = int.Parse(ConfigurationManager.AppSettings["MaxCount"]);
        static int Interval_Static = int.Parse(ConfigurationManager.AppSettings["Interval"]);

        static void Main(string[] args)
        {
            Init();

            Dictionary<int, List<ProductInfo>> productCategoryDic = GetProductCategoryDic();
            var dic = GetStorageResults(productCategoryDic);

            Process(dic);
        }

        private static void Process(Dictionary<int, Dictionary<string, List<ProductInfo>>> dic)
        {
            RelatedProductsOperator relatedProductsOperator = new RelatedProductsOperator(MaxCount_Static, Interval_Static, ConnStr_Static, LogWriter_Static);
            foreach (int cId in dic.Keys)
            {
                LogWriter_Static.WriteLine("CategoryId : " + cId + " \t Related storage product count : " + dic[cId].Count);
                CreateIntraLinkingGenerationAndRelatedAndWriteToDB(dic[cId], relatedProductsOperator);
            }
        }

        private static void CreateIntraLinkingGenerationAndRelatedAndWriteToDB(Dictionary<string, List<ProductInfo>> dic, RelatedProductsOperator relatedProductsOperator)
        {
            foreach (string pName in dic.Keys)
            {
                List<IntraLinkingGenerationAndRelated> ilgrList = new List<IntraLinkingGenerationAndRelated>();
                var list = dic[pName];
                foreach(var pi in list)
                {
                    foreach(var pi2 in list)
                    {
                        if(pi.ProductId != pi2.ProductId)
                        {
                            IntraLinkingGenerationAndRelated ilgr = new IntraLinkingGenerationAndRelated();
                            ilgr.ProductId = pi.ProductId;
                            ilgr.LinedPID = pi2.ProductId;
                            ilgr.LinedPname = pi2.ProductName.Replace("'", " ");
                            ilgrList.Add(ilgr);
                        }
                    }
                }
                relatedProductsOperator.Add(ilgrList);
            }
        }

        private static Dictionary<int, Dictionary<string, List<ProductInfo>>> GetStorageResults(Dictionary<int, List<ProductInfo>> productCategoryDic)
        {
            List<System.Text.RegularExpressions.Regex> regexList = new List<System.Text.RegularExpressions.Regex>();
            foreach (string kw in KeywordsList_Static)
            {
                System.Text.RegularExpressions.Regex storageRegex = new System.Text.RegularExpressions.Regex("\\d+" + kw, System.Text.RegularExpressions.RegexOptions.Compiled);
                regexList.Add(storageRegex);
            }

            Dictionary<int, Dictionary<string, List<ProductInfo>>> categoryProductsDic = new Dictionary<int, Dictionary<string, List<ProductInfo>>>();
            foreach (int cId in productCategoryDic.Keys)
            {
                Dictionary<string, List<ProductInfo>> dic = new Dictionary<string, List<ProductInfo>>();
                List<ProductInfo> pList = productCategoryDic[cId];
                foreach (var pi in pList)
                {
                    foreach(var regex in regexList)
                    {
                        string newName = regex.Replace(pi.ProductNameLower, "");
                        if (newName != pi.ProductNameLower)
                        {
                            if (dic.ContainsKey(newName))
                            {
                                dic[newName].Add(pi);
                            }
                            else
                            {
                                List<ProductInfo> list = new List<ProductInfo>();
                                list.Add(pi);
                                dic.Add(newName, list);
                            }
                        }
                    }
                }

                List<string> removeKeys = new List<string>();
                foreach(string key in dic.Keys)
                {
                    if(dic[key].Count == 1)
                    {
                        removeKeys.Add(key);
                    }
                }
                foreach(string key in removeKeys)
                {
                    dic.Remove(key);
                }
                categoryProductsDic.Add(cId, dic);
            }

            return categoryProductsDic;
        }

        private static void Init()
        {
            string logDir = ConfigurationManager.AppSettings["LogDir"];
            string logPath = System.IO.Path.Combine(logDir, DateTime.Now.ToString("yyyy-MM-dd"));
            System.IO.DirectoryInfo dirInfo = new System.IO.DirectoryInfo(logPath);
            if (!dirInfo.Exists)
            {
                dirInfo.Create();
            }
            string logFilePath = System.IO.Path.Combine(logPath, "Log_" + DateTime.Now.ToString("HH_mm") + ".txt");
            LogWriter_Static = new LogWriter(logFilePath);

            string kwStr = ConfigurationManager.AppSettings["Keywords"];
            KeywordsList_Static = new List<string>();
            string[] kws = kwStr.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            foreach(string kw in kws)
            {
                KeywordsList_Static.Add(kw.Trim().ToLower());
            }
        }

        static Dictionary<int, List<ProductInfo>> GetProductCategoryDic()
        {
            Dictionary<int, List<ProductInfo>> dic = new Dictionary<int, List<ProductInfo>>();
            string selectSql = @"select ProductID, ProductName, CategoryID from CSK_Store_Product 
                                where CategoryID in (
                                select distinct(categoryid) from CSK_Store_Category_AttributeTitle_Map where AttributeTitleID in
                                (select typeid from CSK_Store_ProductDescriptorTitle) and CategoryID in
                                (select CategoryID from CSK_Store_Category where IsActive = 1 and IsDisplayIsMerged = 0 and isSearchOnly = 0))
                                and ProductId in(
                                select distinct(ProductId) from csk_store_retailerproduct where RetailerProductStatus=1 and IsDeleted=0 and RetailerId in
                                (select RetailerId from CSK_Store_Retailer where RetailerStatus=1))";

            //string selectSql = @"select ProductID, ProductName, CategoryID from CSK_Store_Product where CategoryID = 11";

            using (SqlConnection sqlConn = new SqlConnection(ConnStr_Static))
            {
                sqlConn.Open();
                List<int> pidList = new List<int>();
                using (SqlCommand sqlCmd1 = new SqlCommand(selectSql, sqlConn))
                {
                    sqlCmd1.CommandTimeout = 0;
                    using (SqlDataReader sqlDr = sqlCmd1.ExecuteReader())
                    {
                        while (sqlDr.Read())
                        {
                            int pId = sqlDr.GetInt32(0);
                            string pName = sqlDr.GetString(1);
                            int cId = sqlDr.GetInt32(2);

                            ProductInfo pi = new ProductInfo();
                            pi.ProductId = pId;
                            pi.CategoryId = cId;
                            pi.ProductName = pName;
                            pi.ProductNameLower = pi.ProductName.ToLower();

                            if (dic.ContainsKey(cId))
                            {
                                dic[cId].Add(pi);
                            }
                            else
                            {
                                List<ProductInfo> pList = new List<ProductInfo>();
                                pList.Add(pi);
                                dic.Add(cId, pList);
                            }
                        }
                    }
                }
            }

            return dic;
        }
    }
}