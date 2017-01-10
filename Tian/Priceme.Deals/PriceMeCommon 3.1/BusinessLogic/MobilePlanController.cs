using PriceMeCache;
using PriceMeCommon.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Linq;

namespace PriceMeCommon
{
    public static class MobilePlanController
    {
        public static Dictionary<int, int> phoneandplanmaps;
        public static Dictionary<int, int> PhoneAndPlanMaps
        {
            get
            {
                return phoneandplanmaps;
            }
        }
        public static Dictionary<int, int> phoneandminplanmaps;
        public static Dictionary<int, int> PhoneAndMinPlanMaps
        {
            get
            {
                return phoneandminplanmaps;
            }
        }
        public static List<MobilePlanInfo> _mobilePlanList = new List<MobilePlanInfo>();
        public static List<MobilePlanInfo> MobilePlanList
        {
            get
            {
                return _mobilePlanList;
            }
        }
        public static List<MobilePlanCarrier> _mobilePlanCarriers;
        public static List<MobilePlanCarrier> MobilePlanCarriers
        {
            get
            {
                if (_mobilePlanCarriers == null || _mobilePlanCarriers.Count == 0)
                    GetMobilePlanCarriers();
                return _mobilePlanCarriers;
            }
        }

        static Dictionary<int, int> pricemeMobilePlanProductIDMaps;
        public static Dictionary<int, int> PriceMeMobilePlanProductIDMaps
        {
            get
            {
                return pricemeMobilePlanProductIDMaps;
            }
        }

        public static Dictionary<string, List<MobilePlanInfo>> DicMobilePlanAndPhones =
            new Dictionary<string, List<MobilePlanInfo>>();
        public static Dictionary<int, MPProfile> profiles = new Dictionary<int, MPProfile>();
        
        public static void Load()
        {
            Load(null);
        }

        public static void Load(Timer.DKTimer dkTimer)
        {
            if (dkTimer != null)
            {
                dkTimer.Set("MobilePlanController.Load() --- Start");
            }
            GetPhoneAndPlanMaps();
            LogWriter.WriteLineToFile(PriceMeCommon.ConfigAppString.LogPath, "MobilePhoneAndPlanMaps Count : " + phoneandplanmaps.Count);
            
            GetMobilePlanCarriers();
            GetAllMobilePlans();
            GetProfiles();
            LogWriter.WriteLineToFile(PriceMeCommon.ConfigAppString.LogPath, "MobilePlanList Count : " + MobilePlanList.Count);

            LoadPriceMeMobilePlanProductIDMaps();

            if (dkTimer != null)
            {
                dkTimer.Set("MobilePlanController.Load() --- End");
            }
        }

        private static void LoadPriceMeMobilePlanProductIDMaps()
        {
            pricemeMobilePlanProductIDMaps = new Dictionary<int, int>();
            try
            {
                var connectionStr = System.Configuration.ConfigurationManager.ConnectionStrings["CommerceTemplate_Plan"].ConnectionString;
                string sql = "select id, pid from [dbo].[CSK_Store_MobilePhone]";
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
                        string pidString = idr["pid"].ToString();
                        if (int.TryParse(pidString, out pid))
                        {
                            if (!pricemeMobilePlanProductIDMaps.ContainsKey(pid))
                            {
                                pricemeMobilePlanProductIDMaps.Add(pid, int.Parse(idr["id"].ToString()));
                            }
                        }
                    }
                    conn.Close();
                }
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// 从mobile plan 获取priceme phone 和 mobile plan 的对应数量
        /// ado.net
        /// </summary>
        public static Dictionary<int, int> GetPhoneAndPlanMaps()
        {
            phoneandplanmaps = new Dictionary<int, int>();
            phoneandminplanmaps = new Dictionary<int, int>();
            try
            {
                var connectionStr = System.Configuration.ConfigurationManager.ConnectionStrings["CommerceTemplate_Plan"].ConnectionString;
                using (SqlConnection conn = new SqlConnection(connectionStr))
                {
                    conn.Open();
                    string sql = "SELECT PhoneProductID,COUNT(DISTINCT MobilePlanID) AS 'PlanCount',min(Price) AS 'MinPrice' FROM vw_MobilePlans "
                        + "WHERE PhoneProductID > 0  AND Status = 1 AND mStatus = 1 GROUP BY PhoneProductID HAVING COUNT(MobilePlanID) > 0 ORDER BY PhoneProductID";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandTimeout = 500;

                    IDataReader idr = cmd.ExecuteReader();
                    while (idr.Read())
                    {
                        int pid = int.Parse(idr["PhoneProductID"].ToString());
                        phoneandplanmaps.Add(pid,int.Parse(idr["PlanCount"].ToString()));
                        phoneandminplanmaps.Add(pid,(int)decimal.Parse(idr["MinPrice"].ToString()));
                    }
                    conn.Close();
                }
            }
            catch (Exception ex)
            {

            }
            return phoneandplanmaps;
        }

        public static List<MobilePlanInfo> GetMobilePlansByPhoneID(string phone, int minu,
            int data, int text, int contract, int spend, string carrier, string sort, bool isDesc)
        {
            List<MobilePlanInfo>  list = new List<MobilePlanInfo>();
            try
            {
                //先检查cache，没有的话再从数据库获取
                var cacheKey = string.Format("{0}_{1}_{2}_{3}_{4}_{5}_{6}_{7}_{8}",
                    phone, minu, data, text, contract, spend, carrier.Replace(",", ""), sort, isDesc);
                if (DicMobilePlanAndPhones.ContainsKey(cacheKey))
                {
                     list = DicMobilePlanAndPhones[cacheKey];
                     return list;
                }

                #region filter list from MobilePlanList

                if (phone == "0")
                    list = MobilePlanList;
                else list = MobilePlanList.FindAll(p => p.PhoneProductID.ToString() == phone);

                //minutes
                if (minu > 0)
                    list = list.FindAll(p => p.Minutes > minu || p.Minutes == -1);
                else if (minu == -1)
                    list = list.FindAll(p => p.Minutes == -1);

                //data
                if (data > 0)
                    list = list.FindAll(p => p.DataMB > data || p.DataMB == -1);
                else if (data == -1)
                    list = list.FindAll(p => p.DataMB == -1);
                //price
                if (spend > 0)
                    list = list.FindAll(p => p.Price <= spend || p.Price == -1);
                else if (spend == -1)
                    list = list.FindAll(p => p.Price == -1);
                //text
                if (text > 0)
                    list = list.FindAll(p => p.Texts > text || p.Texts == -1);
                else if (text == -1)
                    list = list.FindAll(p => p.Texts == -1);
                carrier = carrier.TrimEnd(',');
                if (!string.IsNullOrEmpty(carrier) && !carrier.Equals("-1") && !carrier.Equals("0"))
                {
                    var _list = new List<MobilePlanInfo>();
                    var carriers = carrier.Split(',');
                    foreach (var item in carriers)
                    {
                        var _tmp = list.FindAll(p => p.CarrierID == item);
                        _list.AddRange(_tmp);
                    }
                    list = _list;
                }
                if (contract > 0)
                {
                    list = list.FindAll(p => p.ContractTypeID == contract);
                }
                var tmp = list;
                list = new List<MobilePlanInfo>();
                foreach (var item in tmp)
                {
                    var aa = list.Find(p=>p.MobilePlanID == item.MobilePlanID);
                    if (aa == null)
                        list.Add(item);
                }

                #endregion

                #region sorting
                if (!string.IsNullOrEmpty(sort))//
                {
                    switch (sort)
                    {
                        case "carrier":
                            if (isDesc)
                                list = list.OrderByDescending(p => p.CarrierName).ToList();
                            else
                                list = list.OrderBy(p => p.CarrierName).ToList();
                            break;
                        case "plan":
                            if (isDesc)
                                list = list.OrderByDescending(p => p.MobilePlanName).ToList();
                            else
                                list = list.OrderBy(p => p.MobilePlanName).ToList();
                            break;
                        case "contract":
                            if (isDesc)
                                list = list.OrderByDescending(p => p.ContractType).ToList();
                            else
                                list = list.OrderBy(p => p.ContractType).ToList();
                            break;
                        case "minutes":
                            if (isDesc)
                                list = list.OrderByDescending(p => p.Minutes).ToList();
                            else
                                list = list.OrderBy(p => p.Minutes).ToList();
                            break;
                        case "data":
                            if (isDesc)
                                list = list.OrderByDescending(p => p.DataMB).ToList();
                            else
                                list = list.OrderBy(p => p.DataMB).ToList();
                            break;
                        case "upfront":
                            if (isDesc)
                                list = list.OrderByDescending(p => p.UpfrontPrice).ToList();
                            else
                                list = list.OrderBy(p => p.UpfrontPrice).ToList();
                            break;
                        default:
                            if (isDesc)
                                list = list.OrderByDescending(p => p.Price).ToList();
                            else
                                list = list.OrderBy(p => p.Price).ToList();
                            break;
                    }
                }
                #endregion

                DicMobilePlanAndPhones.Add(cacheKey, list);
            }
            catch (Exception ex)
            {
                list = new List<MobilePlanInfo>();
            }
            return list;
        }

        public static List<MobilePlanInfo> GetAllMobilePlans()
        {
            try
            {
                var connectionStr = System.Configuration.ConfigurationManager.ConnectionStrings["CommerceTemplate_Plan"].ConnectionString;
                using (SqlConnection conn = new SqlConnection(connectionStr))
                {
                    #region read db
                    conn.Open();
                    StringBuilder sql = new StringBuilder("SELECT MobilePlanID,MobilePlanName,Price,DataMB,[Minutes],Texts,CarrierName,CarrierLogo,"
                        + "ContractTypeID,ContractType,PhoneProductID,PhoneId,MapID,UpfrontPrice,CarrierID FROM vw_MobilePlans "
                        + "WHERE PhoneProductID > 0 AND Status = 1 AND mStatus = 1");
                    SqlCommand cmd = new SqlCommand(sql.ToString(), conn);
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandTimeout = 500;
                    IDataReader dr = cmd.ExecuteReader();
                    #endregion

                    #region get record
                    List<string> mpIDs = new List<string>();
                    while (dr.Read())
                    {
                        var mpid = dr["MobilePlanID"].ToString();
                        //if (mpIDs.Contains(mpid)) continue;
                        //mpIDs.Add(mpid);
                        MobilePlanInfo row = new MobilePlanInfo();

                        //Carrier
                        row.CarrierID = dr["CarrierID"].ToString();
                        row.CarrierName = dr["CarrierName"].ToString();
                        row.CarrierLogo = dr["CarrierLogo"].ToString();
                        //row.CarrierURl = dr["CarrierURl"].ToString();
                        //MobilePlan
                        row.MobilePlanID = mpid;
                        //row.PhoneCount = phoneCount[row.MobilePlanID];
                        row.MobilePlanName = dr["MobilePlanName"].ToString();
                        //row.MobilePlanURL = dr["MobilePlanURL"].ToString();
                        //row.PlanDescription = dr["PlanDescription"].ToString();
                        row.Price = string.IsNullOrEmpty(dr["Price"].ToString()) ? 0 :
                            System.Convert.ToDecimal(dr["Price"].ToString());
                        row.DataMB = string.IsNullOrEmpty(dr["DataMB"].ToString()) ? 0 :
                            System.Convert.ToInt32(dr["DataMB"].ToString());
                        row.Minutes = string.IsNullOrEmpty(dr["Minutes"].ToString()) ? 0 :
                            System.Convert.ToInt32(dr["Minutes"].ToString());
                        row.Texts = string.IsNullOrEmpty(dr["Texts"].ToString()) ? 0 :
                            System.Convert.ToInt32(dr["Texts"].ToString());
                        //ContractType
                        row.ContractTypeID = string.IsNullOrEmpty(dr["ContractTypeID"].ToString()) ? 0 :
                            System.Convert.ToInt32(dr["ContractTypeID"].ToString());
                        row.ContractType = dr["ContractType"].ToString();
                        //MobilePhone
                        row.PhoneId = string.IsNullOrEmpty(dr["PhoneId"].ToString()) ? 0 :
                            System.Convert.ToInt32(dr["PhoneId"].ToString());
                        row.PhoneProductID = string.IsNullOrEmpty(dr["PhoneProductID"].ToString()) ? 0 :
                            System.Convert.ToInt32(dr["PhoneProductID"].ToString());
                        //CSK_Store_MobilePlanPhoneMap.UpfrontPrice
                        row.MapID = string.IsNullOrEmpty(dr["MapID"].ToString()) ? 0 :
                            System.Convert.ToInt32(dr["MapID"].ToString());
                        row.UpfrontPrice = string.IsNullOrEmpty(dr["UpfrontPrice"].ToString()) ? 0 :
                            System.Convert.ToDecimal(dr["UpfrontPrice"].ToString());
                        if (row.UpfrontPrice < 0) row.UpfrontPrice = 0;

                        _mobilePlanList.Add(row);
                    }
                    #endregion
                    conn.Close();
                }
            }
            catch (Exception ex)
            {

            }
            return _mobilePlanList;
        }

        public static List<MobilePlanCarrier> GetMobilePlanCarriers()
        {
            _mobilePlanCarriers = new List<MobilePlanCarrier>();

            try
            {
                var connectionStr = System.Configuration.ConfigurationManager.ConnectionStrings["CommerceTemplate_Plan"].ConnectionString;
                using (SqlConnection conn = new SqlConnection(connectionStr))
                {
                    #region read db
                    conn.Open();
                    string sql = "SELECT [Id],[Name] FROM [dbo].[CSK_Store_Carrier]";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandTimeout = 500;
                    IDataReader dr = cmd.ExecuteReader();
                    #endregion

                    #region get record
                    while (dr.Read())
                    {
                        MobilePlanCarrier row = new MobilePlanCarrier();

                        row.ID = dr["Id"].ToString();
                        row.CarrierName = dr["Name"].ToString();

                        _mobilePlanCarriers.Add(row);
                    }
                    #endregion
                    conn.Close();
                }
            }
            catch (Exception ex)
            {

            }
            return _mobilePlanCarriers;
        }

        /// <summary>
        /// 获取mobile plan 各个profile的范围
        /// <add key="MobilePlanProfiles" value="0-300,0-300;200-999,200-1536;1000+,300-1536;0-999,1536+;1000+,2560+"/>
        /// </summary>
        private static Dictionary<int, MPProfile> GetProfiles()
        {
            var config = System.Configuration.ConfigurationManager.AppSettings["MobilePlanProfiles"];
            if (string.IsNullOrEmpty(config)) return null;
            var pfs = config.Split(';');            
            for (int i = 0; i < pfs.Length; i++)
            {
                if (string.IsNullOrEmpty(pfs[i])) continue;
                MPProfile p = new MPProfile();
                var pf_ = pfs[i].Split(',');
                if (pf_[0].Contains("+"))
                {
                    p.UnlimitedMinutes = true;
                    p.MinMinutes = int.Parse(pf_[0].Replace("+", ""));
                }
                else
                {
                    var pf_minute = pf_[0].Split('-');
                    p.MinMinutes = int.Parse(pf_minute[0]);
                    p.MaxMinutes = int.Parse(pf_minute[1]);
                }

                if (pf_[1].Contains("+"))
                {
                    p.UnlimitedDatas = true;
                    p.MinDatas = int.Parse(pf_[1].Replace("+", ""));
                }
                else
                {
                    var pf_data = pf_[1].Split('-');
                    p.MinDatas = int.Parse(pf_data[0]);
                    p.MaxDatas = int.Parse(pf_data[1]);
                }
                profiles.Add(i + 1, p);
            }
            return profiles;
        }

        #region Minutes/mth

        /// <summary>
        /// Minutes/mth 可选值
        /// </summary>
        public static int[] MinuMthValueArray = new int[] { 50, 100, 200, 300, 500, 600, 800, 1000, -1 };
        /// <summary>
        /// Minutes/mth 可选值组成的字符串（50, 100, ..., Unlimited）
        /// </summary>
        public static string MinuMthFilterString
        {
            get
            {
                StringBuilder str = new StringBuilder();
                foreach (var v in MinuMthValueArray)
                {
                    if (v == -1)
                        str.Append("'Unlimited'");
                    else
                        str.Append("'" + ConvertInt(v) + "',");
                }
                return str.ToString();
            }
        }

        /// <summary>
        /// 通过索引得到 Minutes/mth 值
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public static int GetMinuMthValue(int index)
        {
            if (index < 0 || index > MinuMthValueArray.Length)
                return 0;

            return MinuMthValueArray[index];
        }

        /// <summary>
        /// 通过 Minutes/mth 值得到 索引
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int GetMinuMthValueIndex(int value)
        {
            for (int i = 0; i < MinuMthValueArray.Length; i++)
            {
                if (value == MinuMthValueArray[i]) { return i; }
            }

            return MinuMthValueArray.Length - 1;
        }

        #endregion

        #region Data/mth

        /// <summary>
        /// Data/mth 可选值
        /// </summary>
        public static int[] MthDataValueArray = new int[] { 0, 50, 100, 150, 200, 250, 300, 500, 800, 1100, 2100, -1 };
        /// <summary>
        /// Data/mth 可选值组成的字符串（50, 100, ..., Unlimited）
        /// </summary>
        public static string MthDataFilterString
        {
            get
            {
                return "'Any','50MB','100MB','150MB','200MB','250MB','300MB','500MB','800MB','1G','2G','Unlimited'";
            }
        }

        /// <summary>
        /// 通过索引得到 Data/mth 值
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public static int GetMthDataValue(int index)
        {
            if (index < 0 || index > MthDataValueArray.Length)
                return 0;

            return MthDataValueArray[index];
        }

        /// <summary>
        /// 通过 Data/mth 值得到 索引
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int GetMthDataValueIndex(int value)
        {
            for (int i = 0; i < MthDataValueArray.Length; i++)
            {
                if (value == MthDataValueArray[i]) { return i; }
            }

            return MthDataValueArray.Length - 1;
        }

        #endregion

        #region Spend per month

        /// <summary>
        /// Spend per month 可选值
        /// </summary>
        public static int[] MthSpendValueArray = new int[] { 20, 25, 30, 35, 50, 75, 100, 125, 150, 175, 200, 250, 500, 0 };
        /// <summary>
        /// Spend per month 可选值组成的字符串（50, 100, ..., Don't care）
        /// </summary>
        public static string MthSpendFilterString
        {
            get
            {
                StringBuilder str = new StringBuilder();
                foreach (var v in MthSpendValueArray)
                {
                    if (v == 0)
                        str.Append("'Any'");
                    else
                        str.Append("'" + v + "',");
                }
                return str.ToString();
            }
        }

        /// <summary>
        /// 通过索引得到 Spend per month 值
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public static int GetMthSpendValue(int index)
        {
            if (index < 0 || index > MthSpendValueArray.Length)
                return 0;

            return MthSpendValueArray[index];
        }

        /// <summary>
        /// 通过 Spend per month 值得到 索引
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int GetMthSpendValueIndex(int value)
        {
            for (int i = 0; i < MthSpendValueArray.Length; i++)
            {
                if (value == MthSpendValueArray[i]) { return i; }
            }

            return MthSpendValueArray.Length - 1;
        }

        #endregion

        #region Text per month

        /// <summary>
        /// Text per month 可选值
        /// </summary>
        public static int[] MthTextValueArray = new int[] { 0, 100, 200, 500, 800, 1000, 1200, 1500, 2000, -1 };
        /// <summary>
        /// Text per month 可选值组成的字符串（50, 100, ..., Don't care）
        /// </summary>
        public static string MthTextFilterString
        {
            get
            {
                StringBuilder str = new StringBuilder();
                foreach (var v in MthTextValueArray)
                {
                    if (v == 0)
                        str.Append("'Any',");
                    else if (v == -1)
                        str.Append("'Unlimited'");
                    else
                        str.Append("'" + ConvertInt(v) + "',");
                }
                return str.ToString();
            }
        }

        /// <summary>
        /// 通过索引得到 Text per month 值
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public static int GetMthTextValue(int index)
        {
            if (index < 0 || index > MthTextValueArray.Length)
                return 0;

            return MthTextValueArray[index];
        }

        /// <summary>
        /// 通过 Text per month 值得到 索引
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int GetMthTextValueIndex(int value)
        {
            for (int i = 0; i < MthTextValueArray.Length; i++)
            {
                if (value == MthTextValueArray[i]) { return i; }
            }

            return MthTextValueArray.Length - 1;
        }

        #endregion
        
        public static string ConvertInt(int src)
        {
            if (src == 0)
                return "0";
            return String.Format("{0:0,0}", src);
        }
    }
    public class MPProfile
    {
        public int MinMinutes { get; set; }
        public int MaxMinutes { get; set; }
        public bool UnlimitedMinutes { get; set; }
        public int MinDatas { get; set; }
        public int MaxDatas { get; set; }
        public bool UnlimitedDatas { get; set; }
    }
}
