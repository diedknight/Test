using PriceMe;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for ConsumerController
/// </summary>
public static class ConsumerController
{
    public static Dictionary<int, List<string>> dicCategoryMap;
    public static Dictionary<int, List<string>> dicBrandMap;

    public static void Load()
    {
        dicCategoryMap = new Dictionary<int, List<string>>();
        BindConsumerCategoryMap();

        dicBrandMap = new Dictionary<int, List<string>>();
        BindConsumerBrandMap();
    }

    private static void BindConsumerCategoryMap()
    {
        string sql = "Select * From dbo.CSK_Store_ConsumerCategoryMap Where CountryId = " + WebConfig.CountryId;
        using (SqlConnection conn = new SqlConnection(PriceMeCommon.BusinessLogic.MultiCountryController.CommonConnectionStringSettings_Static.ConnectionString))
        using (SqlCommand cmd = new SqlCommand(sql, conn))
        {
            cmd.CommandType = CommandType.Text;
            cmd.CommandTimeout = 0;

            conn.Open();
            IDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                int cid = 0;
                int.TryParse(dr["PriceMeCategoryId"].ToString(), out cid);
                string categoryname = dr["ConsumerCategory"].ToString().ToLower();
                if (dicCategoryMap.ContainsKey(cid))
                {
                    List<string> listCate = dicCategoryMap[cid];
                    listCate.Add(categoryname);
                    dicCategoryMap[cid] = listCate;
                }
                else
                {
                    List<string> listCate = new List<string>();
                    listCate.Add(categoryname);
                    dicCategoryMap.Add(cid, listCate);
                }
            }
            dr.Close();
            conn.Close();
        }
    }

    private static void BindConsumerBrandMap()
    {
        string sql = "Select * From dbo.CSK_Store_ConsumerTag Where CountryId = " + WebConfig.CountryId;
        using (SqlConnection conn = new SqlConnection(PriceMeCommon.BusinessLogic.MultiCountryController.CommonConnectionStringSettings_Static.ConnectionString))
        using (SqlCommand cmd = new SqlCommand(sql, conn))
        {
            cmd.CommandType = CommandType.Text;
            cmd.CommandTimeout = 0;

            conn.Open();
            IDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                int mid = 0;
                int.TryParse(dr["ManufacturerId"].ToString(), out mid);
                string tag = dr["Tag"].ToString().ToLower();

                if (dicBrandMap.ContainsKey(mid))
                {
                    List<string> listTag = dicBrandMap[mid];
                    listTag.Add(tag);
                    dicBrandMap[mid] = listTag;
                }
                else
                {
                    List<string> listTag = new List<string>();
                    listTag.Add(tag);
                    dicBrandMap.Add(mid, listTag);
                }
            }
            dr.Close();
            conn.Close();
        }
    }
}