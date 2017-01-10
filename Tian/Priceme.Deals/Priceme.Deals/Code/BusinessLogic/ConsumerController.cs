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
    static string connectionStr = System.Configuration.ConfigurationManager.ConnectionStrings["CommerceTemplate_Common"].ConnectionString;

    public static void Load()
    {
        dicCategoryMap = new Dictionary<int, List<string>>();
        BindConsumerCategoryMap();
    }

    private static void BindConsumerCategoryMap()
    {
        string sql = "Select * From dbo.CSK_Store_ConsumerCategoryMap Where CountryId = " + PriceMeCommon.ConfigAppString.CountryID;
        using (SqlConnection conn = new SqlConnection(connectionStr))
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
}