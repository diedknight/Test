using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;


namespace BaseProductTool
{
    class Program
    {
        static string ConnStr_Static = ConfigurationManager.ConnectionStrings["PriceMe_PM"].ConnectionString;
        static LogWriter LogWriter_Static;
        static List<string> KeywordsList_Static;
        static Dictionary<int, string> ExclusiveKeywordsList_Static;
        static int MaxCount_Static = int.Parse(ConfigurationManager.AppSettings["MaxCount"]);
        static int Interval_Static = int.Parse(ConfigurationManager.AppSettings["Interval"]);
        static Dictionary<string, int> VariantTypeUnitDic_Static;
        static Dictionary<string, int> VariantTypeTitleDic_Static;

        static List<int> CategoryIds_Static;

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
                    ilgr.BaseProductValue = list[0].VariantValue;
                    ilgr.LinedPID = list[i].ProductId;
                    ilgr.VariantProductValue = list[i].VariantValue;
                    ilgr.LinedPname = list[i].ProductName.Replace("'", " ");
                    ilgr.VariantTypeID = list[0].VariantTypeID;
                    ilgrList.Add(ilgr);
                }

                baseProductsOperator.Add(ilgrList);
            }
        }

        private static Dictionary<int, Dictionary<string, List<ProductInfo>>> GetStorageResults(Dictionary<int, List<ProductInfo>> productCategoryDic)
        {
            List<KeywordsInfocs> regexList = new List<KeywordsInfocs>();
            foreach (string kw in KeywordsList_Static)
            {
                KeywordsInfocs ki = new KeywordsInfocs();

                System.Text.RegularExpressions.Regex storageRegex = new System.Text.RegularExpressions.Regex("\\s+(?<data>\\d+(\\.\\d+)?)\\s?" + kw + "\\s+", System.Text.RegularExpressions.RegexOptions.Compiled | System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                ki.Keywords = kw.ToLower();
                ki.MyRegex = storageRegex;

                regexList.Add(ki);
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
                        string origName = pi.ProductNameLower;
                        //ExclusiveKeywordsList_Static.ForEach(kw => origName = origName.Replace(kw, ""));

                        var extractedKeyword = ExtractKeyword(origName);

                        if (extractedKeyword.keywordId != 0)
                        {
                            pi.VariantValue = extractedKeyword.keyword;
                            pi.VariantTypeID = extractedKeyword.keywordId;

                            string newName = extractedKeyword.str;

                            if (newName != origName)
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
                        else
                        {
                            string newName = " " + origName + " ";
                            foreach (var regex in regexList)
                            {
                                newName = regex.MyRegex.Replace(newName, "");
                                var match = regex.MyRegex.Match(pi.ProductNameLower);
                                if (match.Success)
                                {
                                    pi.VariantValue = match.Groups["data"].Value;
                                    if (VariantTypeUnitDic_Static.ContainsKey(regex.Keywords))
                                    {
                                        pi.VariantTypeID = VariantTypeUnitDic_Static[regex.Keywords];
                                    }
                                }
                            }

                            newName = newName.Substring(1);
                            newName = newName.Substring(0, newName.Length - 1);

                            if (newName != origName)
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
                }
                else
                {
                    string titleName = "Lens configurations".ToLower();
                    if (VariantTypeTitleDic_Static.ContainsKey(titleName))
                    {
                        int typeId = VariantTypeTitleDic_Static[titleName];

                        foreach (var pi in pList)
                        {
                            pi.VariantTypeID = typeId;
                            string newName = pi.ProductName;

                            int plusIndex = pi.ProductName.IndexOf('+');
                            if (plusIndex > 1)
                            {
                                newName = pi.ProductName.Substring(0, plusIndex).Trim();
                                pi.VariantValue = pi.ProductName.Substring(plusIndex + 1, pi.ProductName.Length - plusIndex - 1).Trim();
                            }
                            else
                            {
                                pi.VariantValue = "Body";
                            }

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



            ExclusiveKeywordsList_Static = new Dictionary<int, string>();
            using (SqlConnection sqlConn = new SqlConnection(ConnStr_Static))
            {
                string sql = "select VariantTypeID,keyword from VariantType where isUnit=0 and isnull(keyword,'')<>''";
                using (var sqlDr = sqlConn.ExecuteReader(sql))
                {
                    while (sqlDr.Read())
                    {
                        int variantTypeID = sqlDr.GetInt32(0);
                        string keyword = sqlDr.GetString(1).ToLower();

                        if (ExclusiveKeywordsList_Static.ContainsKey(variantTypeID))
                        {
                            ExclusiveKeywordsList_Static[variantTypeID] = keyword;
                        }
                        else
                        {
                            ExclusiveKeywordsList_Static.Add(variantTypeID, keyword);
                        }
                    }
                }
            }


            KeywordsList_Static = new List<string>();
            string selectVariantTypeSql = "SELECT VariantTypeID,VariantTitleName,Unit FROM VariantType where isUnit=1 order by VariantTypeID desc";
            VariantTypeUnitDic_Static = new Dictionary<string, int>();
            VariantTypeTitleDic_Static = new Dictionary<string, int>();
            using (SqlConnection sqlConn = new SqlConnection(ConnStr_Static))
            {
                sqlConn.Open();
                List<int> pidList = new List<int>();
                using (SqlCommand sqlCmd1 = new SqlCommand(selectVariantTypeSql, sqlConn))
                {
                    sqlCmd1.CommandTimeout = 0;
                    using (SqlDataReader sqlDr = sqlCmd1.ExecuteReader())
                    {
                        while (sqlDr.Read())
                        {
                            int variantTypeID = sqlDr.GetInt32(0);
                            string variantTitleName = sqlDr.GetString(1).ToLower();
                            string unit = sqlDr.GetString(2).ToLower();

                            //Initialization keyword 
                            KeywordsList_Static.Add(unit);

                            if (!VariantTypeUnitDic_Static.ContainsKey(unit))
                            {
                                VariantTypeUnitDic_Static.Add(unit, variantTypeID);
                            }

                            if (!VariantTypeTitleDic_Static.ContainsKey(variantTitleName))
                            {
                                VariantTypeTitleDic_Static.Add(variantTitleName, variantTypeID);
                            }
                        }
                    }
                }
            }

            CategoryIds_Static = new List<int>();
            string sqlStr = ConfigurationManager.AppSettings["Categories"];
            using (SqlConnection sqlConn = new SqlConnection(ConnStr_Static))
            {
                CategoryIds_Static = sqlConn.Query<int>(sqlStr).ToList();
            }

        }

        static Dictionary<int, List<ProductInfo>> GetProductCategoryDic()
        {
            Dictionary<int, List<ProductInfo>> dic = new Dictionary<int, List<ProductInfo>>();

            string selectSql = @"select PT.ProductID, ProductName, PT.CategoryID, clicks from CSK_Store_Product PT
                                left join (select ProductId, sum(clicks) as clicks from ProductClickTemp group by ProductId) as TPT
                                on TPT.ProductId = PT.ProductID
                                where IsMerge=1 and PT.ProductId in(
                                select distinct(ProductId) from csk_store_retailerproduct where RetailerProductStatus=1 and IsDeleted=0 and RetailerId in
                                (select RetailerId from CSK_Store_Retailer where RetailerStatus=1))                            
                                
                                
                                and CategoryID in (" + string.Join(",", CategoryIds_Static) + ")";

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
                            if (!sqlDr.IsDBNull(3))
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

        private static (string str, int keywordId, string keyword) ExtractKeyword(string str)
        {
            var pairList = ExclusiveKeywordsList_Static.ToList();

            for (int i = 0; i < pairList.Count; i++)
            {
                var pair = pairList[i];
                var keywordId = pair.Key;
                var keyword = pair.Value;

                var str1 = str.Replace(keyword, "");

                if (str1 != str)
                {
                    return (str1.TrimA(), keywordId, keyword);
                }


                //var terms = str.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                //var terms1 = terms.Where(term => term != keyword).ToArray();

                //if (terms.Length != terms1.Length)
                //{
                //    return (string.Join(" ", terms1), keywordId, keyword);
                //}                
            }

            return ("", 0, "");
        }


    }
}
