using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for DFP_AdsChontroller
/// </summary>
public class DFP_AdsChontroller
{
    static readonly string Static_CommonConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["CommerceTemplate_Common"].ConnectionString;
    static List<int> Static_RootCategory_DFP_MapList = new List<int>();
    /*static Dictionary<int, Dictionary<string, string>> Static_RootCategory_DFP_MapDic = new Dictionary<int, Dictionary<string, string>>();

    public static void Load()
    {
        string querySql = @"SELECT [RootCategoryID]
                            ,[AdsType]
                            ,[AdsSlotID]
                            FROM [DFP_RootCategory_Map]";

        Static_RootCategory_DFP_MapDic = new Dictionary<int, Dictionary<string, string>>();
        using (System.Data.SqlClient.SqlConnection sqlConn = new System.Data.SqlClient.SqlConnection(Static_CommonConnectionString))
        {
            sqlConn.Open();
            using (System.Data.SqlClient.SqlCommand sqlCmd = new System.Data.SqlClient.SqlCommand(querySql, sqlConn))
            {
                sqlCmd.CommandTimeout = 0;
                sqlCmd.Connection = sqlConn;

                System.Data.SqlClient.SqlDataReader sdr = sqlCmd.ExecuteReader();
                while (sdr.Read())
                {
                    int rootCid = sdr.GetInt32(0);
                    int adsType = sdr.GetInt32(1);
                    string adsSlotID = sdr.GetString(2);

                    string key = rootCid + "_" + adsType;
                    if(Static_RootCategory_DFP_MapDic.ContainsKey(rootCid))
                    {
                        Static_RootCategory_DFP_MapDic[rootCid].Add(key, adsSlotID);
                    }
                    else
                    {
                        Dictionary<string, string> dic = new Dictionary<string, string>();
                        dic.Add(key, adsSlotID);
                        Static_RootCategory_DFP_MapDic.Add(rootCid, dic);
                    }
                }
            }
        }
    }

    public static Dictionary<string, string> GetDFP_AdsDic(int rootCategoryID)
    {
        if(Static_RootCategory_DFP_MapDic.ContainsKey(rootCategoryID))
        {
            return Static_RootCategory_DFP_MapDic[rootCategoryID];
        }
        else
        {
            return null;
        }
    }*/

    public static void Load()
    {
        string querySql = @"SELECT [RootCategoryID] FROM [DFP_RootCategory_Map]";

        Static_RootCategory_DFP_MapList = new List<int>();
        using (System.Data.SqlClient.SqlConnection sqlConn = new System.Data.SqlClient.SqlConnection(Static_CommonConnectionString))
        {
            sqlConn.Open();
            using (System.Data.SqlClient.SqlCommand sqlCmd = new System.Data.SqlClient.SqlCommand(querySql, sqlConn))
            {
                sqlCmd.CommandTimeout = 0;
                sqlCmd.Connection = sqlConn;

                System.Data.SqlClient.SqlDataReader sdr = sqlCmd.ExecuteReader();
                while (sdr.Read())
                {
                    int rootCid = sdr.GetInt32(0);

                    Static_RootCategory_DFP_MapList.Add(rootCid);
                }
            }
        }
    }

    public static bool HasDFPAds(int rootCategoryID)
    {
        return Static_RootCategory_DFP_MapList.Contains(rootCategoryID);
    }
}