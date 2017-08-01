using PriceMeCommon.Data;
using PriceMeDBA;
using SubSonic.Schema;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceMeCommon.BusinessLogic
{
    public static class UserController
    {
        public static DataSet GetListDataSet(string userProviderUserKey, bool isCurrentUser, int countryId)
        {
            using (SubSonic.DataProviders.SharedDbConnectionScope sdbs = new SubSonic.DataProviders.SharedDbConnectionScope(MultiCountryController.GetDBProvider(countryId)))
            {
                var sql = string.Format("SELECT * FROM CSK_Store_List WHERE UserID = '{0}' AND IsPublic = 1 AND ListID IN(SELECT DISTINCT ListID FROM CSK_Store_ProductList)", userProviderUserKey);
                if (isCurrentUser)
                    sql = string.Format("SELECT * FROM CSK_Store_List WHERE UserID = '{0}' AND ListID IN(SELECT DISTINCT ListID FROM CSK_Store_ProductList)", userProviderUserKey);
                StoredProcedure sp = new StoredProcedure("");
                sp.Command.CommandType = CommandType.Text;
                sp.Command.CommandSql = sql;
                DataSet ds = sp.ExecuteDataSet();
                return ds;
            }
        }

        public static DataSet GetListDataSetByParseId(string parseId, bool isCurrentUser, int countryId)
        {
            using (SubSonic.DataProviders.SharedDbConnectionScope sdbs = new SubSonic.DataProviders.SharedDbConnectionScope(MultiCountryController.GetDBProvider(countryId)))
            {
                var sql = string.Format("SELECT * FROM CSK_Store_List WHERE ParseID = '{0}' AND IsPublic = 1 AND ListID IN(SELECT DISTINCT ListID FROM CSK_Store_ProductList)", parseId);
                if (isCurrentUser)
                    sql = string.Format("SELECT * FROM CSK_Store_List WHERE ParseID = '{0}' AND ListID IN(SELECT DISTINCT ListID FROM CSK_Store_ProductList)", parseId);
                StoredProcedure sp = new StoredProcedure("");
                sp.Command.CommandType = CommandType.Text;
                sp.Command.CommandSql = sql;
                DataSet ds = sp.ExecuteDataSet();
                return ds;
            }
        }

        public static Dictionary<int, PriceMeCache.CommonAllList> GetAllList(int countryId)
        {
            var sql = @"SELECT [CSK_Store_List].ListID
                  ,[ListName]
                  ,[ParseID]
	              ,[CSK_Store_List].Description
                  ,ModifiedOn
	              ,[CSK_Store_ProductList].ProductID
	              ,[CSK_Store_Product].DefaultImage
                  FROM [dbo].[CSK_Store_List]
                  inner join [dbo].[CSK_Store_ProductList]
                  on [CSK_Store_List].ListID = [CSK_Store_ProductList].ListID
                  inner join [CSK_Store_Product]
                  on [CSK_Store_Product].ProductID = [CSK_Store_ProductList].ProductID
                  where [CSK_Store_List].IsPublic=0";

            Dictionary<int, PriceMeCache.CommonAllList> listDic = new Dictionary<int, PriceMeCache.CommonAllList>();

            using (SubSonic.DataProviders.SharedDbConnectionScope sdbs = new SubSonic.DataProviders.SharedDbConnectionScope(MultiCountryController.GetDBProvider(countryId)))
            {
                var sp = new StoredProcedure("");
                sp.Command.CommandSql = sql.ToString();
                sp.Command.CommandTimeout = 0;
                sp.Command.CommandType = CommandType.Text;
                var dr = sp.ExecuteReader();
                while (dr.Read())
                {
                    int listID = dr.GetInt32(0);
                    string imageFile = dr.IsDBNull(6) ? "" : dr.GetString(6);

                    if (listDic.ContainsKey(listID))
                    {
                        listDic[listID].FirstImageUrl = imageFile;
                        listDic[listID].ListProCount++;
                    }
                    else
                    {
                        PriceMeCache.CommonAllList cal = new PriceMeCache.CommonAllList();
                        cal.ListID = listID;
                        cal.FirstImageUrl = imageFile;
                        cal.ListName = dr.GetString(1);
                        cal.UserID = dr.GetString(2);
                        cal.ListDesc = dr.IsDBNull(3) ? "" : dr.GetString(3);
                        cal.ModifyOn = dr.GetDateTime(4);
                        cal.ListProCount = 1;

                        listDic.Add(listID, cal);
                    }
                }
            }

            return listDic;
        }

        public static Dictionary<int, List<ProductCatalog>> GetListProducts(string userId, int countryId)
        {
            Dictionary<int, List<ProductCatalog>> dic = new Dictionary<int, List<ProductCatalog>>();

            using (SubSonic.DataProviders.SharedDbConnectionScope sdbs = new SubSonic.DataProviders.SharedDbConnectionScope(MultiCountryController.GetDBProvider(countryId)))
            {
                var sql0 = string.Format("SELECT * FROM [dbo].[CSK_Store_ProductList] WHERE ListID IN (SELECT ListID FROM CSK_Store_List WHERE ParseID = '{0}')", userId);
                StoredProcedure sp0 = new StoredProcedure("");
                sp0.Command.CommandSql = sql0;
                sp0.Command.CommandType = CommandType.Text;
                sp0.Command.CommandTimeout = 0;
                var reader = sp0.ExecuteTypedList<CSK_Store_ProductList>();
                foreach (var item in reader)
                {
                    ProductCatalog pdc = SearchController.SearchProduct(item.ProductID.ToString(), countryId);
                    if (pdc != null)
                    {
                        if (dic.ContainsKey(item.ListID))
                        {
                            dic[item.ListID].Add(pdc);
                        }
                        else
                        {
                            var list = new List<ProductCatalog>();
                            list.Add(pdc);
                            dic.Add(item.ListID, list);
                        }
                    }
                }
            }

            return dic;
        }

        public static bool DeleteList(int listId, int countryId)
        {
            try
            {
                using (SubSonic.DataProviders.SharedDbConnectionScope sdbs = new SubSonic.DataProviders.SharedDbConnectionScope(MultiCountryController.GetDBProvider(countryId)))
                {
                    var sql = "delete from [CSK_Store_List] where ListID=@0";
                    var sp = new SubSonic.Schema.StoredProcedure("");
                    sp.Command.CommandType = CommandType.Text;
                    sp.Command.CommandSql = sql;
                    sp.Command.Parameters.Add("@0", listId);
                    sp.Execute();

                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public static bool ChangePublic(int listId, string isPublic, int countryId)
        {
            try
            {
                using (SubSonic.DataProviders.SharedDbConnectionScope sdbs = new SubSonic.DataProviders.SharedDbConnectionScope(MultiCountryController.GetDBProvider(countryId)))
                {
                    var sql = "update [CSK_Store_List] set IsPublic=@0  where ListID=@1";
                    var sp = new StoredProcedure("");
                    sp.Command.CommandType = CommandType.Text;
                    sp.Command.CommandSql = sql;
                    sp.Command.Parameters.Add("@0", isPublic == "1" ? "0" : "1");
                    sp.Command.Parameters.Add("@1", listId);
                    sp.Execute();

                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public static bool AlertUpdate(string price, int typeId, string ex_rids, string alertId, int countryId)
        {
            try
            {
                using (SubSonic.DataProviders.SharedDbConnectionScope sdbs = new SubSonic.DataProviders.SharedDbConnectionScope(MultiCountryController.GetDBProvider(countryId)))
                {
                    string sql = "update [CSK_Store_ProductAlert] set [ProductPrice]=@0,[AlertType]=@1,[ExcludedRetailers]=@2 where AlertId=@3";
                    StoredProcedure sp = new StoredProcedure("");
                    sp.Command.CommandSql = sql;
                    sp.Command.CommandTimeout = 0;
                    sp.Command.CommandType = CommandType.Text;
                    sp.Command.Parameters.Add("@0", price);
                    sp.Command.Parameters.Add("@1", typeId);
                    sp.Command.Parameters.Add("@2", ex_rids);
                    sp.Command.Parameters.Add("@3", alertId);
                    sp.Execute();

                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}