using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for DFP_AdsChontroller
/// </summary>
public class DFP_AdsChontroller
{
    static readonly string Static_CommonConnectionString = PriceMeCommon.BusinessLogic.MultiCountryController.CommonConnectionStringSettings_Static.ConnectionString;
    static List<int> Static_RootCategory_DFP_MapList = new List<int>();

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