using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PriceMeCommon.Data;
using PriceMeDBA;
using PriceMeCommon.DBTableInfo;
using System.Data.SqlClient;
using System.Data;

/// <summary>
/// Business logic for the ads
/// </summary>

namespace PriceMeCommon
{
    public static class ManufacturerController
    {
        /// <summary>
        /// Quick add a Manufacturer only with Manufacturer Name.
        /// </summary>
        /// <param name="ManufacturerName"></param>
        static Dictionary<int, ManufacturerInfo> manufacturerDictionary;

        private static List<string> manufacturerLetter = null;

        public static Dictionary<int, ManufacturerInfo> ManufacturerDictionary
        {
            get { return ManufacturerController.manufacturerDictionary; }
            set { ManufacturerController.manufacturerDictionary = value; }
        }

        static Dictionary<string, int> relatedCategoryInfoDictionary;

        public static Dictionary<string, int> RelatedCategoryInfoDictionary
        {
            get { return relatedCategoryInfoDictionary; }
        }

        static Dictionary<int, string> manufacturerProductURLDictionary;

        public static Dictionary<int, string> ManufacturerProductURLDictionary
        {
            get { return manufacturerProductURLDictionary; }
        }

        public static List<string> ManufacturerLetter
        {
            get { return manufacturerLetter; }
        }

        static ManufacturerController()
        {
            //Load();
        }

        static Dictionary<int, string> ManufacturerDescs;
        static Dictionary<int, string> ManufacturerUrls;
        public static void LoadForBuildIndex()
        {
            List<string> _manufacturerLetter = new List<string>();
            Dictionary<int, ManufacturerInfo> _manufacturerDictionary = new Dictionary<int, ManufacturerInfo>();

            #region CSK_Store_ManufacturerDetails

            ManufacturerDescs = new Dictionary<int, string>();
            ManufacturerUrls = new Dictionary<int, string>();

            try
            {
                string sql = "SELECT * FROM [CSK_Store_ManufacturerDetails] WHERE [CountryID] = " + ConfigAppString.CountryID;
                using (SqlConnection conn = new SqlConnection(ConfigAppString.CommerceTemplateCommon))
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandTimeout = 0;
                    conn.Open();
                    IDataReader idr = cmd.ExecuteReader();
                    while (idr.Read())
                    {
                        int mid = int.Parse(idr["MID"].ToString());
                        if (!ManufacturerDescs.ContainsKey(mid))
                            ManufacturerDescs.Add(mid, idr["Description"].ToString());
                        else
                            ManufacturerDescs[mid] = idr["Description"].ToString();

                        if (!ManufacturerUrls.ContainsKey(mid))
                            ManufacturerUrls.Add(mid, idr["LocalURL"].ToString());
                        else
                            ManufacturerUrls[mid] = idr["LocalURL"].ToString();
                    }
                    conn.Close();
                }
            }
            catch (Exception ex)
            {

            }
            #endregion

            #region GetAllManufacturers()
            using (System.Data.IDataReader idr = PriceMeCommon.PriceMeStatic.PriceMeDB.GetAllManufacturer().ExecuteReader())
            {
                while (idr.Read())
                {
                    ManufacturerInfo manufacturer = new ManufacturerInfo();
                    manufacturer.ManufacturerID = int.Parse(idr["ManufacturerID"].ToString());
                    manufacturer.ManufacturerName = idr["ManufacturerName"].ToString();
                    //manufacturer.Description = idr["Description"].ToString();
                    manufacturer.IsPopular = bool.Parse(idr["IsPopular"].ToString());
                    manufacturer.ImagePath = idr["ImagePath"].ToString();
                    //manufacturer.Location = idr["Location"].ToString();

                    manufacturer.BrandsURL = idr["BrandsURL"].ToString();

                    if (ManufacturerDescs != null && ManufacturerDescs.ContainsKey(manufacturer.ManufacturerID))
                    {
                        manufacturer.Description = ManufacturerDescs[manufacturer.ManufacturerID];
                    }

                    if (ManufacturerUrls != null && ManufacturerUrls.ContainsKey(manufacturer.ManufacturerID))
                    {
                        manufacturer.URL = ManufacturerUrls[manufacturer.ManufacturerID];
                    }
                    else
                    {
                        manufacturer.URL = "";
                    }

                    if (string.IsNullOrEmpty(manufacturer.ManufacturerName))
                        continue;

                    manufacturer.IsHidden = false;

                    _manufacturerDictionary.Add(manufacturer.ManufacturerID, manufacturer);
                }
                LogWriter.WriteLineToFile(PriceMeCommon.ConfigAppString.LogPath, "_manufacturerDictionary count:  " + _manufacturerDictionary.Count);
                LogWriter.WriteLineToFile(PriceMeCommon.ConfigAppString.LogPath, "_manufacturerLetter count:  " + _manufacturerLetter.Count);
            }
            #endregion

            manufacturerDictionary = _manufacturerDictionary;

            relatedCategoryInfoDictionary = GetAllRelatedCategoryDictionary();

            //log
            var IsHiddenFalse = ManufacturerDictionary.Values.Where(manu => manu.IsHidden == false).Count();
            var IsHiddenTrue = ManufacturerDictionary.Values.Where(manu => manu.IsHidden == true).Count();
            LogWriter.WriteLineToFile(PriceMeCommon.ConfigAppString.LogPath, "ManufacturerController.Load() --- ManufacturerDictionary Count: " + ManufacturerDictionary.Count);
            LogWriter.WriteLineToFile(PriceMeCommon.ConfigAppString.LogPath, "ManufacturerController.Load() --- ManufacturerDictionary IsHiddenFalse Count: " + IsHiddenFalse);
            LogWriter.WriteLineToFile(PriceMeCommon.ConfigAppString.LogPath, "ManufacturerController.Load() --- ManufacturerDictionary IsHiddenTrue Count: " + IsHiddenTrue);
            
            if (relatedCategoryInfoDictionary == null)
                LogWriter.WriteLineToFile(PriceMeCommon.ConfigAppString.LogPath, "ManufacturerController.Load() --- ManufacturerRelatedCategoryClicks Count: null");
            else
                LogWriter.WriteLineToFile(PriceMeCommon.ConfigAppString.LogPath, "ManufacturerController.Load() --- ManufacturerRelatedCategoryClicks Count: " + relatedCategoryInfoDictionary.Count);
        }

        public static void Load(Timer.DKTimer dkTimer)
        {
            ManufacturerDictionary = VelocityController.GetCache<Dictionary<int, ManufacturerInfo>>(VelocityCacheKey.AllManufacturer);
            if (ManufacturerDictionary == null)
            {
                LogWriter.WriteLineToFile(PriceMeCommon.ConfigAppString.LogPath, "ManufacturerController.Load() --- ManufacturerDictionary velocity is null ");

                LoadForBuildIndex();                
            }
            else
            {
                LogWriter.WriteLineToFile(PriceMeCommon.ConfigAppString.LogPath, "ManufacturerController.Load() --- ManufacturerDictionary  velocity is not null ");
                relatedCategoryInfoDictionary = VelocityController.GetCache<Dictionary<string, int>>(VelocityCacheKey.ManufacturerRelatedCategoryClicks);
            }

            if (dkTimer != null)
            {
                dkTimer.Set("ManufacturerController.Load() --- Befor GetAllManufacturerLetters()");
            }
            GetAllManufacturerLetters();//获取所有Manufacturer的firstletter
            GetManufacturerProductURL();
        }

        public static void Load()
        {
            Load(null);
        }

        /// <summary>
        /// 获取所有Manufacturer的firstletter
        /// </summary>
        private static void GetAllManufacturerLetters()
        {
            //manufacturerDictionary 已经从velocity或在上一步从db里读取出来，
            if (manufacturerDictionary == null || manufacturerDictionary.Values == null) return;

            List<string> _manufacturerLetter = new List<string>();
            //获取当前country的BrandsIdList
            List<int> allBrandsId = ManufacturerSearcher.GetAllBrandsIdList();
            LogWriter.WriteLineToFile(PriceMeCommon.ConfigAppString.LogPath, "allBrandsId count:  " + allBrandsId.Count);
            foreach (var manufacturer in manufacturerDictionary.Values)
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

                if (!_manufacturerLetter.Contains(firstLetter) && !manufacturer.IsHidden)
                {
                    _manufacturerLetter.Add(firstLetter);
                }
            }

            if (_manufacturerLetter.Count > 0)
            {
                manufacturerLetter = _manufacturerLetter.OrderBy(m => m).ToList();
            }

            while (true)
            {
                //去掉数字，以a,b,c开头
                if (manufacturerLetter == null || manufacturerLetter.Count == 0)
                    break;
                string letter = manufacturerLetter[0];
                int lNum = 0;
                if (int.TryParse(letter, out lNum))
                {
                    manufacturerLetter.Add(letter);
                    manufacturerLetter.RemoveAt(0);
                }
                else
                {
                    break;
                }
            }
        }

        private static Dictionary<string, int> GetAllRelatedCategoryDictionary()
        {
            Dictionary<string, int> relatedCategoryDictionary = new Dictionary<string, int>();
            using (System.Data.IDataReader idr = PriceMeCommon.PriceMeStatic.PriceMeDB.GetAllRelatedCategories().ExecuteReader())
            {
                while (idr.Read())
                {
                    string relatedCategoriesKey = idr["RelatedCategoriesKey"].ToString();
                    int clicks = int.Parse(idr["Clicks"].ToString());
                    relatedCategoryDictionary.Add(relatedCategoriesKey, clicks);
                }
            }
            return relatedCategoryDictionary;
        }

        public static int GetRelatedCategoryClicks(string key)
        {
            if (relatedCategoryInfoDictionary == null || !relatedCategoryInfoDictionary.ContainsKey(key))
            {
                return 0;
            }
            else
            {
                return relatedCategoryInfoDictionary[key];
            }
        }

        public static ManufacturerInfo GetManufacturerByID(int manufacturerID)
        {
            if (manufacturerDictionary.ContainsKey(manufacturerID))
            {
                return manufacturerDictionary[manufacturerID];
            }
            CSK_Store_Manufacturer manufacturer = CSK_Store_Manufacturer.SingleOrDefault(man => man.ManufacturerID == manufacturerID);
            if( manufacturer != null)
            {
                ManufacturerInfo manufacturerInfo = new DBTableInfo.ManufacturerInfo();
                manufacturerInfo.ManufacturerID = manufacturer.ManufacturerID;
                manufacturerInfo.ManufacturerName = manufacturer.ManufacturerName;
                manufacturerInfo.IsPopular = manufacturer.IsPopular ?? false;
                manufacturerInfo.ImagePath = manufacturer.ImagePath;

                if (ManufacturerUrls != null && ManufacturerUrls.ContainsKey(manufacturer.ManufacturerID))
                {
                    manufacturerInfo.URL = ManufacturerUrls[manufacturer.ManufacturerID];
                }
                else
                {
                    manufacturerInfo.URL = "";
                }

                if (ManufacturerDescs != null && ManufacturerDescs.ContainsKey(manufacturer.ManufacturerID))
                {
                    manufacturerInfo.Description = ManufacturerDescs[manufacturer.ManufacturerID];
                }
                else
                {
                    manufacturerInfo.Description = "";
                }

                return manufacturerInfo;
            }
            return null;
        }

        public static IEnumerable<ManufacturerInfo> GetPopularManufacturersOrderByName()
        {
            return manufacturerDictionary.Values.Where(manu => manu.IsPopular == true
                && manu.IsHidden == false).OrderBy(manu => manu.ManufacturerName);
        }

        public static IEnumerable<ManufacturerInfo> GetManufacturersByLetter(string letter, int pageIndex, int pageSize, out PagerInfo pagerInfo)
        {
            IEnumerable<ManufacturerInfo> result = null;
            if(string.IsNullOrEmpty(letter))
            {
                result = manufacturerDictionary.Values;
            }
            else
            {
                result = manufacturerDictionary.Values.Where(manu => manu.ManufacturerName.StartsWith(letter));
            }
            pagerInfo = new PagerInfo(pageIndex, pageSize, result.Count());

            return result.Skip(pagerInfo.StartIndex).Take(pageSize);
        }

        public static IEnumerable<ManufacturerInfo> GetAllBrandsByLetter(string letter, int pageIndex, int pageSize, out PagerInfo pagerInfo)//, out string log
        {
            //log = string.Format("letter:{0};;;pageIndex:{1};;;;Value count:", letter, pageIndex);
            try
            {

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
                        //log += ";;;;;result count:" + result0.Count();
                        return result0.Skip(pagerInfo.StartIndex).Take(pageSize);
                    }
                    else
                        result = manufacturerDictionary.Values.Where(manu => manu.ManufacturerName.StartsWith(letter) && manu.IsHidden == false);
                }
                pagerInfo = new PagerInfo(pageIndex, pageSize, result.Count());
                //log += ";;;;;result count:" + result.Count();
                return result.Skip(pagerInfo.StartIndex).Take(pageSize);
            }
            catch (Exception ex)
            {
                pagerInfo = new PagerInfo(pageIndex, pageSize, 0);
                //log += ex.Message;
                
                return null;
            }
        }

        /// <summary>
        /// Get product manufacturer url from common table: [dbo].[CSK_Store_ManufacturerProduct] of db pam_user
        /// </summary>
        private static void GetManufacturerProductURL()
        {
            manufacturerProductURLDictionary = new Dictionary<int,string>();
            try
            {
                var connectionStr = System.Configuration.ConfigurationManager.ConnectionStrings["CommerceTemplate_Common"].ConnectionString;
                string sql = string.Format("SELECT [PID],[URL] FROM [CSK_Store_ManufacturerProduct] WHERE COUNTRYID = {0}", ConfigAppString.CountryID);
                using (SqlConnection conn = new SqlConnection(connectionStr))
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandTimeout = 0;
                    conn.Open();
                    IDataReader idr = cmd.ExecuteReader();
                    while (idr.Read())
                    {
                        int pid = 0;
                        if (int.TryParse(idr[0].ToString(), out pid))
                        {
                            manufacturerProductURLDictionary.Add(pid, idr[1].ToString());
                        }
                    }
                    conn.Close();
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}