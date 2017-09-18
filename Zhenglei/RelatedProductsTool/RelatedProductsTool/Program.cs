using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RelatedProductsTool
{
    class Program
    {
        static string LogDir_Static;
        static LogWriter LogWriter_Static;

        static int MaxCount_Static = int.Parse(ConfigurationManager.AppSettings["MaxCount"]);
        static int Interval_Static = int.Parse(ConfigurationManager.AppSettings["Interval"]);

        static void Main(string[] args)
        {
            System.Diagnostics.Process.GetCurrentProcess().PriorityClass = System.Diagnostics.ProcessPriorityClass.Idle;
            int retedCount = int.Parse(ConfigurationManager.AppSettings["RetedCount"]);
            string logDir = ConfigurationManager.AppSettings["LogDir"];
            LogDir_Static = System.IO.Path.Combine(logDir, DateTime.Now.ToString("yyyy-MM-dd"));
            System.IO.DirectoryInfo dirInfo = new System.IO.DirectoryInfo(LogDir_Static);
            if (!dirInfo.Exists)
            {
                dirInfo.Create();
            }

            bool detailLog = bool.Parse(ConfigurationManager.AppSettings["DetailLog"]);
            string logFilePath = System.IO.Path.Combine(LogDir_Static, "Log_" + DateTime.Now.ToString("HH_mm") + ".txt");
            LogWriter_Static = new LogWriter(logFilePath);

            LogWriter_Static.WriteLine("Load countries section");

            CountriesNodeInfo mCountriesNodeInfo = null;
            try
            {
                mCountriesNodeInfo = (CountriesNodeInfo)ConfigurationManager.GetSection("countries");

                RelatedProductsController.Init();
            }
            catch (Exception ex)
            {
                LogWriter_Static.WriteLine("-------------------------");
                LogWriter_Static.WriteLine(ex.Message + "\t" + ex.StackTrace);
                LogWriter_Static.WriteLine("-------------------------");
                return;
            }

            foreach (int key in mCountriesNodeInfo.CountryInfoListDic.Keys)
            {
                try
                {
                    CountryInfo ci = mCountriesNodeInfo.CountryInfoListDic[key];
                    LogWriter_Static.WriteLine("Start country : " + ci.ToString());
                    //如果配置了ProductIds，则只计算配置的ProductIds，不管配置的CategoryIds
                    if (ci.ProductIds.Count > 0)
                    {
                        List<ProductInfo> pList = RelatedProductsController.GetProductInfosByProductIds(ci.ProductIds, ci.MyConnectionStringSettings.ConnectionString);
                        List<int> cidsList = pList.Select(p => p.CategoryId).ToList();
                        Dictionary<int, List<ProductInfo>> cProductDic = RelatedProductsController.GetProductInfosByCategoryIds(cidsList, ci.MyConnectionStringSettings.ConnectionString, ci.CountryId);

                        WriteDicLog(cProductDic);

                        LogWriter_Static.WriteLine("Start process related product score.");
                        var list = GetRelatedProductScore(pList, cProductDic);
                        string filePath = System.IO.Path.Combine(LogDir_Static, DateTime.Now.ToString("HH_mm") + ".csv");

                        LogWriter_Static.WriteLine("Start write detail log.");
                        WriteToFile(list, filePath);
                    }
                    else
                    {
                        Dictionary<int, List<ProductInfo>> cProductDic = RelatedProductsController.GetProductInfosByCategoryIds(ci.CategoryIds, ci.MyConnectionStringSettings.ConnectionString, ci.CountryId);
                        WriteDicLog(cProductDic);
                        LogWriter_Static.WriteLine("Start process related product score.");
                        FindAndWriteRelatedProductScore(cProductDic, retedCount, detailLog, ci.CountryId);

                        //LogWriter_Static.WriteLine("Start write to db.");
                        //RelatedProductsController.WriteToDB(dic, ci.CountryId, ConfigurationManager.ConnectionStrings["PriceMe_PM"].ConnectionString);
                    }
                    LogWriter_Static.WriteLine("CountryId : " + ci.CountryId + " finished.");
                }
                catch(Exception ex)
                {
                    LogWriter_Static.WriteLine("-------------------------");
                    LogWriter_Static.WriteLine(ex.Message + "\t" + ex.StackTrace);
                    LogWriter_Static.WriteLine("-------------------------");
                }
            }
        }

        private static void WriteDicLog(Dictionary<int, List<ProductInfo>> cProductDic)
        {
            foreach(int key in cProductDic.Keys)
            {
                LogWriter_Static.WriteLine("CategoryId : " + key + "\tProduct count: " + cProductDic[key].Count);
            }
        }

        private static void WriteAllToFile(Dictionary<int, List<ProductRelatedScore>> logDic, int countryId)
        {
            foreach (int key in logDic.Keys)
            {
                string filePath = System.IO.Path.Combine(LogDir_Static, countryId + "_CID_" + key + ".csv");
                WriteToFile(logDic[key], filePath);
            }
        }

        private static void WriteAllToFile(Dictionary<int, List<ProductRelatedScore>> dic, string mLogDir)
        {
            System.IO.DirectoryInfo dirInfo = new System.IO.DirectoryInfo(mLogDir);
            if (!dirInfo.Exists)
            {
                dirInfo.Create();
            }

            foreach(int key in dic.Keys)
            {
                string filePath = System.IO.Path.Combine(mLogDir, "cid_" + key + ".csv");
                WriteToFile(dic[key], filePath);
            }
        }

        private static void WriteToFile(List<ProductRelatedScore> list, string filePath)
        {
            list = list.Where(p => p.TotalScore > 0).ToList();
            using (System.IO.StreamWriter sw = new System.IO.StreamWriter(filePath, false))
            {
                sw.WriteLine(ProductRelatedScore.ToCSVHeaderString());
                foreach(var prs in list)
                {
                    sw.WriteLine(prs.ToCSVString());
                }
            }

        }

        private static List<ProductRelatedScore> GetRelatedProductScore(List<ProductInfo> ptList, Dictionary<int, List<ProductInfo>> cProductDic)
        {
            List<ProductRelatedScore> list = new List<ProductRelatedScore>();
            
            foreach (int key in cProductDic.Keys)
            {
                List<ProductInfo> productList = cProductDic[key];
                SortProductInfosByClicks(productList);
                foreach (ProductInfo mainPI in ptList)
                {
                    List<ProductRelatedScore> prsList = new List<ProductRelatedScore>();
                    foreach (ProductInfo pi in productList)
                    {
                        if (pi.ProductId != mainPI.ProductId)
                        {
                            ProductRelatedScore prs = RelatedProductsController.GetRelatedScore(mainPI, pi);
                            //Console.WriteLine(prs.ToString());

                            prsList.Add(prs);
                        }
                    }

                    if (prsList.Count == 0)
                        continue;

                    prsList = prsList.OrderByDescending(p => p.TotalScore).ToList();
                    
                    list.AddRange(prsList);
                }
            }

            return list;
        }

        private static void FindAndWriteRelatedProductScore(Dictionary<int, List<ProductInfo>> cProductDic, int retedCount, bool detailLog, int countryId)
        {
            RelatedProductsOperator relatedProductsOperator = new RelatedProductsOperator(MaxCount_Static, Interval_Static, countryId);

            foreach (int key in cProductDic.Keys)
            {
                List<ProductRelatedScore> logList = new List<ProductRelatedScore>();
                List<ProductInfo> pList = cProductDic[key];
                SortProductInfosByClicks(pList);
                foreach(ProductInfo mainPI in pList)
                {
                    List<ProductRelatedScore> prsList = new List<ProductRelatedScore>();
                    foreach(ProductInfo pi in pList)
                    {
                        if(pi.ProductId != mainPI.ProductId)
                        {
                            ProductRelatedScore prs = RelatedProductsController.GetRelatedScore(mainPI, pi);
                            prs.CategoryId = key;
                            prsList.Add(prs);
                        }
                    }

                    if(prsList.Count == 0)
                        continue;

                    prsList = prsList.OrderByDescending(p => p.TotalScore).ToList();

                    if(detailLog)
                    {
                        logList.AddRange(prsList);
                    }

                    prsList = prsList.Take(retedCount).ToList();
                    relatedProductsOperator.Add(prsList, mainPI.ProductId);

                    //try
                    //{
                    //    //这里还有优化的空间
                    //    prsList = prsList.Take(retedCount).ToList();
                    //    //RelatedProductsController.WriteToDBByProductId(prsList, mainPI.ProductId, countryId, DateTime.Now, ConfigurationManager.ConnectionStrings["PriceMe_PM"].ConnectionString);
                    //}
                    //catch(Exception ex)
                    //{
                    //    LogWriter_Static.WriteLine("Main product id : " + mainPI.ProductId + " ex:");
                    //    LogWriter_Static.WriteLine(ex.Message + "\t" + ex.StackTrace);
                    //}
                }

                if (detailLog)
                {
                    string filePath = System.IO.Path.Combine(LogDir_Static, countryId + "_CID_" + key + ".csv");
                    LogWriter_Static.WriteLine("Start write detail log cid : " + key + ".");
                    WriteToFile(logList, filePath);
                }
            }

            relatedProductsOperator.Finish();
        }

        private static void SortProductInfosByClicks(List<ProductInfo> pList)
        {
            pList = pList.OrderByDescending(p => p.Clicks).ToList();
            for(int i = 0; i < pList.Count; i++)
            {
                pList[i].CategoryClickIndex = i;
            }
        }
    }
}