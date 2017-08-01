using PriceMeCommon.Data;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceMeCommon.BusinessLogic
{
    public static class ManufacturerController
    {
        static Dictionary<int, Dictionary<int, ManufacturerInfo>> MultiCountryManufacturerDic_Static;
        static Dictionary<int, List<string>> MultiCountryManufacturerLetter_Static;
        static Dictionary<int, Dictionary<int, string>> MultiCountryManufacturerProductURLDic_Static;

        public static void Load(Timer.DKTimer dkTimer)
        {
            MultiCountryManufacturerDic_Static = GetManufacturerDic();

            MultiCountryManufacturerLetter_Static = GetMultiCountryManufacturerLetter(MultiCountryManufacturerDic_Static);

            MultiCountryManufacturerProductURLDic_Static = GetMultiCountryManufacturerProductURLDic();
        }

        public static void LoadForBuildIndex()
        {
            MultiCountryManufacturerDic_Static = GetManufacturerDic();
        }

        private static Dictionary<int, Dictionary<int, string>> GetMultiCountryManufacturerProductURLDic()
        {
            Dictionary<int, Dictionary<int, string>> multiDic = new Dictionary<int, Dictionary<int, string>>();

            var connectionStr = MultiCountryController.CommonConnectionStringSettings_Static.ConnectionString;
            string sql = "SELECT [PID],[URL],COUNTRYID FROM [CSK_Store_ManufacturerProduct] WHERE Status = 1 And COUNTRYID in (" + string.Join(",", MultiCountryController.CountryIdList) + ")";
            using (SqlConnection conn = new SqlConnection(connectionStr))
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.CommandTimeout = 0;
                conn.Open();
                using (var idr = cmd.ExecuteReader())
                {
                    while (idr.Read())
                    {
                        int pid = idr.GetInt32(0);
                        string mUrl = idr.GetString(1);
                        int countryId = idr.GetInt32(2);

                        if (multiDic.ContainsKey(countryId))
                        {
                            multiDic[countryId].Add(pid, mUrl);
                        }
                        else
                        {
                            Dictionary<int, string> dic = new Dictionary<int, string>();
                            dic.Add(pid, mUrl);
                            multiDic.Add(countryId, dic);
                        }
                    }
                }
            }

            return multiDic;
        }

        private static Dictionary<int, List<string>> GetMultiCountryManufacturerLetter(Dictionary<int, Dictionary<int, ManufacturerInfo>> multiCountryManufacturerDic)
        {
            Dictionary<int, List<string>> multiDic = new Dictionary<int, List<string>>();

            foreach(int countryId in multiCountryManufacturerDic.Keys)
            {
                List<string> manufacturerLetter = new List<string>();

                List<int> allBrandsId = GetAllBrandsIdList(countryId);

                foreach (var manufacturer in multiCountryManufacturerDic[countryId].Values)
                {
                    //判断manufactrure是否显示（当前country的就可以显示）
                    if (allBrandsId.Contains(manufacturer.ManufacturerID) && manufacturer.ManufacturerID != -1)// 
                    {
                        manufacturer.IsHidden = false;
                    }
                    else
                    {
                        manufacturer.IsHidden = true;
                    }

                    //获取firstletter并暂存
                    var firstLetter = manufacturer.ManufacturerName.Substring(0, 1).ToUpper();

                    if (!manufacturerLetter.Contains(firstLetter) && !manufacturer.IsHidden)
                    {
                        manufacturerLetter.Add(firstLetter);
                    }
                }

                //去掉数字开头
                for (int i = 0; i < manufacturerLetter.Count;)
                {
                    string letter = manufacturerLetter[i];
                    int lNum = 0;
                    if (int.TryParse(letter, out lNum))
                    {
                        manufacturerLetter.RemoveAt(i);
                    }
                    else
                    {
                        i++;
                    }
                }

                manufacturerLetter = manufacturerLetter.OrderBy(m => m).ToList();

                multiDic.Add(countryId, manufacturerLetter);
            }

            return multiDic;
        }

        private static Dictionary<int, Dictionary<int, ManufacturerInfo>> GetManufacturerDic()
        {
            Dictionary<int, Dictionary<int, ManufacturerInfo>> multiDic = new Dictionary<int, Dictionary<int, ManufacturerInfo>>();

            Dictionary<int, Dictionary<int, string>> multiCountryManufacturerDescs = new Dictionary<int, Dictionary<int, string>>();
            Dictionary<int, Dictionary<int, string>> multiCountryManufacturerUrls = new Dictionary<int, Dictionary<int, string>>();

            string sql = "SELECT * FROM [CSK_Store_ManufacturerDetails] WHERE [CountryID] in (" + string.Join(",", MultiCountryController.CountryIdList) + ")";
            using (SqlConnection conn = new SqlConnection(MultiCountryController.CommonConnectionStringSettings_Static.ConnectionString))
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.CommandTimeout = 0;
                conn.Open();
                using (var idr = cmd.ExecuteReader())
                {
                    while (idr.Read())
                    {
                        int mid = int.Parse(idr["MID"].ToString());
                        int countryId = int.Parse(idr["CountryID"].ToString());
                        string desc = idr["Description"].ToString();
                        string localURL = idr["LocalURL"].ToString();

                        if (!string.IsNullOrEmpty(desc))
                        {
                            if (multiCountryManufacturerDescs.ContainsKey(countryId))
                            {
                                multiCountryManufacturerDescs[countryId].Add(mid, desc);
                            }
                            else
                            {
                                Dictionary<int, string> descDic = new Dictionary<int, string>();
                                descDic.Add(mid, desc);
                                multiCountryManufacturerDescs.Add(countryId, descDic);
                            }
                        }

                        if (!string.IsNullOrEmpty(localURL))
                        {
                            if (multiCountryManufacturerUrls.ContainsKey(countryId))
                            {
                                multiCountryManufacturerUrls[countryId].Add(mid, localURL);
                            }
                            else
                            {
                                Dictionary<int, string> urlDic = new Dictionary<int, string>();
                                urlDic.Add(mid, localURL);
                                multiCountryManufacturerUrls.Add(countryId, urlDic);
                            }
                        }
                    }
                }
            }

            List<ManufacturerInfo> manufacturerInfoList = new List<ManufacturerInfo>();
            //每个数据库Manufacturer表的数据是一样的
            string sqlGetAllManufacturer = "GetAllManufacturer";
            using (SqlConnection conn = new SqlConnection(MultiCountryController.GetDBConnectionString(MultiCountryController.CountryIdList[0])))
            using (SqlCommand cmd = new SqlCommand(sqlGetAllManufacturer, conn))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;
                conn.Open();
                using (var idr = cmd.ExecuteReader())
                {
                    while (idr.Read())
                    {
                        ManufacturerInfo manufacturer = new ManufacturerInfo();
                        manufacturer.ManufacturerID = int.Parse(idr["ManufacturerID"].ToString());
                        manufacturer.ManufacturerName = idr["ManufacturerName"].ToString();
                        manufacturer.IsPopular = bool.Parse(idr["IsPopular"].ToString());
                        manufacturer.ImagePath = idr["ImagePath"].ToString();
                        manufacturer.BrandsURL = idr["BrandsURL"].ToString();
                        manufacturer.IsHidden = false;
                        manufacturerInfoList.Add(manufacturer);
                    }
                }
            }

            foreach(int countryId in MultiCountryController.CountryIdList)
            {
                Dictionary<int, ManufacturerInfo> dic = new Dictionary<int, ManufacturerInfo>();

                Dictionary<int, string> manufacturerDescs = new Dictionary<int, string>();
                Dictionary<int, string> manufacturerUrls = new Dictionary<int, string>();
                if (multiCountryManufacturerDescs.ContainsKey(countryId))
                {
                    manufacturerDescs = multiCountryManufacturerDescs[countryId];
                }

                if (multiCountryManufacturerUrls.ContainsKey(countryId))
                {
                    manufacturerUrls = multiCountryManufacturerUrls[countryId];
                }

                foreach (ManufacturerInfo mi in manufacturerInfoList)
                {
                    ManufacturerInfo newMi = new ManufacturerInfo(mi);
                    if (manufacturerDescs.ContainsKey(newMi.ManufacturerID))
                    {
                        newMi.Description = manufacturerDescs[newMi.ManufacturerID];
                    }

                    if (manufacturerUrls.ContainsKey(newMi.ManufacturerID))
                    {
                        newMi.URL = manufacturerUrls[newMi.ManufacturerID];
                    }
                    else
                    {
                        newMi.URL = "";
                    }

                    dic.Add(newMi.ManufacturerID, newMi);
                }

                multiDic.Add(countryId, dic);
            }

            return multiDic;
        }

        private static List<int> GetAllBrandsIdList(int countryId)
        {
            Lucene.Net.Search.IndexSearcher allBrandsIndexSearcher = MultiCountryController.GetAllBrandsLuceneSearcher(countryId);

            List<int> allBrandsId = new List<int>();
            if (allBrandsIndexSearcher != null)
            {
                for (int i = 0; i < allBrandsIndexSearcher.IndexReader.MaxDoc; i++)
                {
                    string mid = allBrandsIndexSearcher.Doc(i).Get("ManufacturerID");
                    allBrandsId.Add(int.Parse(mid));
                }
            }

            return allBrandsId;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="manufacturerID"></param>
        /// <returns></returns>
        public static ManufacturerInfo GetManufacturerByID(int manufacturerId, int countryId)
        {
            if (MultiCountryManufacturerDic_Static.ContainsKey(countryId) && MultiCountryManufacturerDic_Static[countryId].ContainsKey(manufacturerId))
            {
                return MultiCountryManufacturerDic_Static[countryId][manufacturerId];
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<ManufacturerInfo> GetPopularManufacturersOrderByName(int countryId)
        {
            if (MultiCountryManufacturerDic_Static.ContainsKey(countryId))
            {
                return MultiCountryManufacturerDic_Static[countryId].Values.Where(manu => manu.IsPopular == true
                    && manu.IsHidden == false).OrderBy(manu => manu.ManufacturerName);
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="letter"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="pagerInfo"></param>
        /// <returns></returns>
        public static IEnumerable<ManufacturerInfo> GetAllBrandsByLetter(string letter, int pageIndex, int pageSize, out PagerInfo pagerInfo, int countryId)
        {
            if (MultiCountryManufacturerDic_Static.ContainsKey(countryId))
            {
                var manufacturerDictionary = MultiCountryManufacturerDic_Static[countryId];

                IEnumerable<ManufacturerInfo> result = null;
                if (string.IsNullOrEmpty(letter))
                {
                    result = manufacturerDictionary.Values.Where(manu => manu.IsHidden == false);
                }
                else
                {
                    if (letter.Equals("n"))
                    {
                        var result_ = manufacturerDictionary.Values.Where(manu => manu.IsHidden == false);
                        IList<ManufacturerInfo> result0 = new List<ManufacturerInfo>();
                        foreach (var item in result_)
                        {
                            bool isNum = false;
                            int num = 0;
                            isNum = int.TryParse(item.ManufacturerName.Substring(0, 1), out num);
                            if (isNum) { result0.Add(item); }
                        }
                        pagerInfo = new PagerInfo(pageIndex, pageSize, result0.Count());
                        return result0.Skip(pagerInfo.StartIndex).Take(pageSize);
                    }
                    else
                        result = manufacturerDictionary.Values.Where(manu => manu.ManufacturerName.StartsWith(letter) && manu.IsHidden == false);
                }
                pagerInfo = new PagerInfo(pageIndex, pageSize, result.Count());
                return result.Skip(pagerInfo.StartIndex).Take(pageSize);
            }

            pagerInfo = new PagerInfo(pageIndex, pageSize, 0);
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="countryId"></param>
        /// <returns></returns>
        public static string GetManufacturerProductURL(int productId, int countryId)
        {
            if(MultiCountryManufacturerProductURLDic_Static.ContainsKey(countryId) && MultiCountryManufacturerProductURLDic_Static[countryId].ContainsKey(productId))
            {
                return MultiCountryManufacturerProductURLDic_Static[countryId][productId];
            }

            return string.Empty;
        }

        public static List<string> GetManufacturerLetter(int countryId)
        {
            if(MultiCountryManufacturerLetter_Static.ContainsKey(countryId))
            {
                return MultiCountryManufacturerLetter_Static[countryId];
            }

            return null;
        }
    }
}