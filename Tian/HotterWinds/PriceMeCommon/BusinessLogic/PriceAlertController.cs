using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PriceMeDBA;
using PriceMeCommon.Data;

namespace PriceMeCommon.BusinessLogic
{
    public class PriceAlertController
    {        
        /// <summary>
        /// 根据user 获取price alert
        /// </summary>
        /// <param name="user"></param>
        /// <returns>
        /// Price Alert
        /// Price Alert 相关的产品
        /// </returns>
        public static List<PriceMeDBA.LinqEntity.CSK_Store_ProductAlert> GetUserPriceAlert(string user, out Dictionary<int, ProductCatalog> relatedProductsOut, out int count, int countryId)
        {
            List<PriceMeDBA.LinqEntity.CSK_Store_ProductAlert> list = null;
            Dictionary<int, ProductCatalog> relatedProducts = new Dictionary<int, ProductCatalog>();
            int cc = 0;
            try
            {
                using (SubSonic.DataProviders.SharedDbConnectionScope sdbs = new SubSonic.DataProviders.SharedDbConnectionScope(MultiCountryController.GetDBProvider(countryId)))
                {
                    var lists = new PriceMe.Bll.LinqBllBase<PriceMeDBA.LinqEntity.CSK_Store_ProductAlert>();
                    list = lists.Query(q => q.ParseID == user);
                    if (list != null)
                    {
                        foreach (var item in list)
                        {
                            ProductCatalog pc = SearchController.SearchProduct(item.ProductId.ToString(), countryId);
                            if (pc != null)
                            {
                                if (!relatedProducts.ContainsKey(item.ProductId))
                                    relatedProducts.Add(item.ProductId, pc);
                                cc++;
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
            count = cc;
            relatedProductsOut = relatedProducts;
            return list;
        }

        public static CSK_Store_ProductAlert GetUserPriceAlert(int aId, int countryId)
        {
            CSK_Store_ProductAlert obj= null;
            try
            {
                using (SubSonic.DataProviders.SharedDbConnectionScope sdbs = new SubSonic.DataProviders.SharedDbConnectionScope(MultiCountryController.GetDBProvider(countryId)))
                {
                    var list = CSK_Store_ProductAlert.Find(p => p.AlertId == aId).ToList();
                    if (list != null || list.Count > 0)
                        obj = list[0];
                }
            }
            catch (Exception)
            {
            }
            return obj;
        }

        public static CSK_Store_ProductAlert GetProductAlert(int countryId, int productId, string email)
        {
            string emailLower = email.ToLower();
            using (SubSonic.DataProviders.SharedDbConnectionScope sdbs = new SubSonic.DataProviders.SharedDbConnectionScope(MultiCountryController.GetDBProvider(countryId)))
            {
                return CSK_Store_ProductAlert.SingleOrDefault(p => p.ProductId == productId && p.Email.ToLower() == emailLower);
            }
        }

        public static void AddProductAlert(int countryId, int productId, string userID, string email, decimal productPrice, int v2, int v3, string alertType, string excludedRetailers, string priceType, string priceEach, decimal productPrice2)
        {
            string sql = string.Format("Insert CSK_Store_ProductAlert (AlertGUID, ProductId, ParseID, Email, ProductPrice, Status, IsActive, AlertType, ExcludedRetailers, PriceType, PriceEach,[OriginalPrice]) "
                            + "Values ('{0}', {1}, '{2}', '{3}', {4}, {5}, {6},{7},'{8}',{9},{10},{11})",
                            Guid.NewGuid().ToString(), productId, userID, email, productPrice, v2, v3, alertType, excludedRetailers, priceType, priceEach, productPrice);

            using (SubSonic.DataProviders.SharedDbConnectionScope sdbs = new SubSonic.DataProviders.SharedDbConnectionScope(MultiCountryController.GetDBProvider(countryId)))
            {
                SubSonic.Schema.StoredProcedure sp = new SubSonic.Schema.StoredProcedure("");
                sp.Command.CommandSql = sql;
                sp.Command.CommandType = System.Data.CommandType.Text;
                sp.Command.CommandTimeout = 0;
                sp.Execute();
            }
        }
    }
}
