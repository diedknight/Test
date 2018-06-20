﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseProductTool
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
            BaseProductsOperator relatedProductsOperator = new BaseProductsOperator(MaxCount_Static, Interval_Static, ConnStr_Static, LogWriter_Static);
            foreach (int cId in dic.Keys)
            {
                LogWriter_Static.WriteLine("CategoryId : " + cId + " \t Related storage product count : " + dic[cId].Count);
                CreateIntraLinkingGenerationAndRelatedAndWriteToDB(dic[cId], relatedProductsOperator);
            }

            relatedProductsOperator.Finish();
        }

        private static void CreateIntraLinkingGenerationAndRelatedAndWriteToDB(Dictionary<string, List<ProductInfo>> dic, BaseProductsOperator baseProductsOperator)
        {
            foreach (string pName in dic.Keys)
            {
                List<IntraLinkingGenerationAndRelated> ilgrList = new List<IntraLinkingGenerationAndRelated>();
                var list = dic[pName];

                if (list[0].CategoryId != 2)
                {
                    //list = list.OrderBy(l => l.AttributeData).ToList();
                    list = list.OrderByDescending(l => l.Clicks).ThenBy(l => l.ProductName.Length).ToList();
                }
                else
                {
                    list = list.OrderBy(l => l.ProductName.Length).ToList();
                }
                
                for (int i = 1; i < list.Count; i++)
                {
                    IntraLinkingGenerationAndRelated ilgr = new IntraLinkingGenerationAndRelated();
                    ilgr.ProductId = list[0].ProductId;
                    ilgr.LinedPID = list[i].ProductId;
                    ilgr.LinedPname = list[i].ProductName.Replace("'", " ");
                    ilgrList.Add(ilgr);
                }

                baseProductsOperator.Add(ilgrList);
            }
        }

        private static Dictionary<int, Dictionary<string, List<ProductInfo>>> GetStorageResults(Dictionary<int, List<ProductInfo>> productCategoryDic)
        {
            List<System.Text.RegularExpressions.Regex> regexList = new List<System.Text.RegularExpressions.Regex>();
            foreach (string kw in KeywordsList_Static)
            {
                System.Text.RegularExpressions.Regex storageRegex = new System.Text.RegularExpressions.Regex("(?<data>\\d+(\\.\\d+)?)\\s?" + kw, System.Text.RegularExpressions.RegexOptions.Compiled | System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                regexList.Add(storageRegex);
            }

            Dictionary<int, Dictionary<string, List<ProductInfo>>> categoryProductsDic = new Dictionary<int, Dictionary<string, List<ProductInfo>>>();
            foreach (int cId in productCategoryDic.Keys)
            {
                Dictionary<string, List<ProductInfo>> dic = new Dictionary<string, List<ProductInfo>>();
                List<ProductInfo> pList = productCategoryDic[cId];
                if (cId != 2)
                {
                    foreach (var pi in pList)
                    {
                        string newName = pi.ProductNameLower;
                        foreach (var regex in regexList)
                        {
                            newName = regex.Replace(newName, "");
                        }

                        if (newName != pi.ProductNameLower)
                        {
                            //var match = regex.Match(pi.ProductNameLower);
                            //if (match.Success)
                            //{
                            //    pi.AttributeData = float.Parse(match.Groups["data"].Value);
                            //}

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
                else
                {
                    foreach (var pi in pList)
                    {
                        string newName = pi.ProductNameLower.Split('+')[0].Trim();

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

                List<string> removeKeys = new List<string>();
                foreach (string key in dic.Keys)
                {
                    if (dic[key].Count == 1)
                    {
                        removeKeys.Add(key);
                    }
                }
                foreach (string key in removeKeys)
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
            foreach (string kw in kws)
            {
                KeywordsList_Static.Add(kw.Trim().ToLower());
            }
        }

        static Dictionary<int, List<ProductInfo>> GetProductCategoryDic()
        {
            Dictionary<int, List<ProductInfo>> dic = new Dictionary<int, List<ProductInfo>>();
            string selectSql = @"select PT.ProductID, ProductName, PT.CategoryID, clicks from CSK_Store_Product PT
                                left join (select ProductId, sum(clicks) as clicks from [dbo].[ProductClickTemp] group by ProductId) as TPT
                                on TPT.ProductId = PT.ProductID
                                where CategoryID in (
                                select distinct(categoryid) from CSK_Store_Category_AttributeTitle_Map where AttributeTitleID in
                                (select typeid from CSK_Store_ProductDescriptorTitle) and CategoryID in
                                (select CategoryID from CSK_Store_Category where IsActive = 1 and IsDisplayIsMerged = 0 and isSearchOnly = 0))
                                and IsMerge=1 and PT.ProductId in(
                                select distinct(ProductId) from csk_store_retailerproduct where RetailerProductStatus=1 and IsDeleted=0 and RetailerId in
                                (select RetailerId from CSK_Store_Retailer where RetailerStatus=1))";

            //string selectSql = @"select PT.ProductID, ProductName, PT.CategoryID, 0 from CSK_Store_Product PT where CategoryID in (459,2)
            //                    and CategoryID in (
            //                    select distinct(categoryid) from CSK_Store_Category_AttributeTitle_Map where AttributeTitleID in
            //                    (select typeid from CSK_Store_ProductDescriptorTitle) and CategoryID in
            //                    (select CategoryID from CSK_Store_Category where IsActive = 1 and IsDisplayIsMerged = 0 and isSearchOnly = 0))
            //                    and IsMerge=1 and PT.ProductId in(
            //                    select distinct(ProductId) from csk_store_retailerproduct where RetailerProductStatus=1 and IsDeleted=0 and RetailerId in
            //                    (select RetailerId from CSK_Store_Retailer where RetailerStatus=1))";

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
                            pi.Clicks = 0;
                            if(!sqlDr.IsDBNull(3))
                            {
                                pi.Clicks = sqlDr.GetInt32(3);
                            }

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
