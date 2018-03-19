using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace GoogleShoppingFeed
{
    class Program
    {
        static void Main(string[] args)
        {
            string timeStr = DateTime.Now.ToString("yyyy_MM_dd HH_mm");
            string logPath = Path.Combine(System.Configuration.ConfigurationManager.AppSettings["LogRootPath"], timeStr + ".txt");
            using (LogWriter logWriter = new LogWriter(logPath))
            {
                logWriter.WriteLine("Start load config. --- " + DateTime.Now.ToString("HH:mm:ss"));

                string feedPath = Path.Combine(System.Configuration.ConfigurationManager.AppSettings["FeedRootPath"], timeStr + ".xml");
                decimal minPrice = decimal.Parse(System.Configuration.ConfigurationManager.AppSettings["MinPrice"]);
                string categoryConfig = System.Configuration.ConfigurationManager.AppSettings["IncludeCategories"];
                List<int> categoriesList = GetCategoriesList(categoryConfig);
                string googleCategoryMapFile = System.Configuration.ConfigurationManager.AppSettings["GoogleCategoryMap"];
                Dictionary<int, int> googleCategoryMapDic = GetGoogleCategoryMapDic(googleCategoryMapFile, categoriesList);
                string dbConnStr = System.Configuration.ConfigurationManager.ConnectionStrings["PriceMeDB"].ConnectionString;

                foreach (int cId in categoriesList)
                {
                    if (!googleCategoryMapDic.ContainsKey(cId))
                    {
                        logWriter.WriteLine("Cid : " + cId + " no map.");
                    }
                }

                logWriter.WriteLine("Start load products. --- " + DateTime.Now.ToString("HH:mm:ss"));
                List<GoogleFeedProduct> gProducts = LoadGoogleFeedProducts(googleCategoryMapDic, minPrice, dbConnStr);
                logWriter.WriteLine("products count : " + gProducts.Count + " --- " + DateTime.Now.ToString("HH:mm:ss"));

                logWriter.WriteLine("Start write xml. --- " + DateTime.Now.ToString("HH:mm:ss"));
                GoogleFeedWriter gfw = new GoogleFeedWriter(gProducts, feedPath);
                gfw.WriteFile();
                logWriter.WriteLine("Write finished. Feed : " + feedPath + " --- " + DateTime.Now.ToString("HH:mm:ss"));
            }
        }

        private static List<GoogleFeedProduct> LoadGoogleFeedProducts(Dictionary<int, int> googleCategoryMapDic, decimal minPrice, string dbConnStr)
        {
            List<GoogleFeedProduct> pList = new List<GoogleFeedProduct>();

            if (googleCategoryMapDic == null || googleCategoryMapDic.Count == 0)
                return pList;

            string sqlFormat = @"with TempT as(
                    select ROW_NUMBER() over(partition BY RPT.ProductId ORDER BY RetailerPrice) as Num, PT.ProductId, PT.ProductName, PT.DefaultImage, PT.CategoryID, PT.ManufacturerID, RPT.RetailerPrice, RPT.Freight, RPT.RetailerProductCondition, RPT.StockStatus from CSK_Store_RetailerProduct as RPT 
                    inner join CSK_Store_Product as PT on RPT.ProductId = PT.ProductID
                    where PT.IsMerge = 1 {0}
                    and RetailerId in ( select RetailerId from CSK_Store_Retailer where RetailerCountry = 3 and RetailerStatus = 1 and RetailerId in (select RetailerId from CSK_Store_PPCMember where PPCMemberTypeID = 2)
                    ) and RetailerProductStatus = 1)

                    select TempT.*, MT.ManufacturerName, CT.CategoryName, PDT.Description from TempT
                    left join CSK_Store_Manufacturer MT on MT.ManufacturerID = TempT.ManufacturerID
                    left join CSK_Store_Category CT on CT.CategoryID = TempT.CategoryID
                    left join CSK_Store_PM_ProductDescription PDT on PDT.PID = TempT.ProductID and PDT.CountryID = 3
                    where TempT.Num = 1 and TempT.RetailerPrice > {1}";

            string cIdSqlFormat = "and PT.CategoryID in ({0}) ";
            string cidListStr = string.Join(",", googleCategoryMapDic.Keys);
            string cIdSql = string.Format(cIdSqlFormat, cidListStr);

            string querySql = string.Format(sqlFormat, cIdSql, minPrice);

            using (SqlConnection sqlConn = new SqlConnection(dbConnStr))
            using (SqlCommand sqlCmd = new SqlCommand(querySql, sqlConn))
            {
                sqlConn.Open();
                sqlCmd.CommandTimeout = 0;
                using (SqlDataReader sqlDr = sqlCmd.ExecuteReader())
                {
                    while(sqlDr.Read())
                    {
                        GoogleFeedProduct gfp = new GoogleFeedProduct();
                        gfp.Id = sqlDr.GetInt32(1);
                        gfp.Title = sqlDr.GetString(2);
                        string imagePath = sqlDr.GetString(3);
                        gfp.ImageLink = GetImageLink(imagePath);
                        int pCategoryId = sqlDr.GetInt32(4);
                        gfp.GoogleProductCategory = googleCategoryMapDic[pCategoryId];
                        gfp.Price = sqlDr.GetDecimal(6);
                        gfp.Shipping = sqlDr.GetDecimal(7);
                        int condition = sqlDr.GetInt32(8);
                        gfp.Condition = GetConditionStr(condition);
                        string stockStatus = sqlDr.GetString(9).Trim();
                        gfp.Availability = GetStockStatusStr(stockStatus);
                        gfp.Brand = sqlDr.GetString(10);
                        gfp.ProductType = sqlDr.GetString(11);
                        gfp.Description = sqlDr.IsDBNull(12) ? "" : sqlDr.GetString(12);
                        gfp.Link = GetPriceMeProductUrl(gfp.Title, gfp.Id, 3);

                        pList.Add(gfp);
                    }
                }
            }

            return pList;
        }

        private static Regex illegalReg = new Regex(@"[^a-z0-9-]+", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static Regex illegalReg2 = new Regex("-+", RegexOptions.Compiled);
        private static Regex illegalReg3 = new Regex("^-+|-+$", RegexOptions.Compiled);

        public static string GetPriceMeProductUrl(string productName, int productID, int countryId)
        {
            string pre = string.Empty;
            switch (countryId)
            {
                case 3:
                    pre = "https://www.priceme.co.nz";
                    break;
                case 1:
                    pre = "https://www.priceme.net.au";
                    break;
                case 28:
                    pre = "https://www.priceme.com.ph";
                    break;
                case 41:
                    pre = "https://www.priceme.com.hk";
                    break;
                case 36:
                    pre = "https://www.priceme.com.sg";
                    break;
                case 45:
                    pre = "https://www.priceme.com.my";
                    break;
                case 51:
                    pre = "https://www.priceme.co.id";
                    break;
                default:
                    pre = "https://www.priceme.co.nz";
                    break;
            }
            return string.Format(pre + "/{0}/p-{1}.aspx", FilterInvalidUrlPathChar(productName), productID);
        }

        public static string FilterInvalidUrlPathChar(string sourceString)
        {

            sourceString = illegalReg.Replace(sourceString, "-");
            sourceString = illegalReg2.Replace(sourceString, "-");
            sourceString = illegalReg3.Replace(sourceString, "");
            return sourceString;
        }

        private static string GetStockStatusStr(string stockStatus)
        {
            if(stockStatus != "-2")
            {
                return "in stock";
            }
            else
            {
                return "out of stock";
            }
        }

        private static string GetConditionStr(int condition)
        {
            switch (condition)
            {
                case 0:
                    return "new";
                case 4:
                    return "refurbished";
                default:
                    return "used";
            }
        }

        private static string GetImageLink(string imagePath)
        {
            if (imagePath.StartsWith("http:") || imagePath.StartsWith("https:"))
            {
                return imagePath;
            }
            else
            {
                if (!imagePath.StartsWith("/") && !imagePath.StartsWith("\\"))
                {
                    imagePath = "/" + imagePath;
                }
                return "https://images.pricemestatic.com" + imagePath.Replace("\\", "/");
            }
        }

        private static Dictionary<int, int> GetGoogleCategoryMapDic(string googleCategoryMapFile, List<int> categoriesList)
        {
            Dictionary<int, int> dic = new Dictionary<int, int>();
            using (StreamReader sr = new StreamReader(googleCategoryMapFile))
            {
                string line = sr.ReadLine();

                while(!string.IsNullOrEmpty(line))
                {
                    string[] infos = line.Split(new string[] { "->" }, StringSplitOptions.RemoveEmptyEntries);
                    if(infos.Length > 1)
                    {
                        int cId = int.Parse(infos[0].Trim());
                        if (categoriesList.Contains(cId))
                        {
                            dic.Add(cId, int.Parse(infos[1].Trim()));
                        }
                    }

                    line = sr.ReadLine();
                }
            }

            return dic;
        }

        private static List<int> GetCategoriesList(string categoryConfig)
        {
            string[] cs = categoryConfig.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            List<int> list = new List<int>();
            foreach(string str in cs)
            {
                list.Add(int.Parse(str));
            }
            return list;
        }
    }
}
