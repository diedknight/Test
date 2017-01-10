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
        public static List<PriceMeDBA.LinqEntity.CSK_Store_ProductAlert> GetUserPriceAlert(string user,
            out Dictionary<int, ProductCatalog> relatedProductsOut, out int count)
        {
            List<PriceMeDBA.LinqEntity.CSK_Store_ProductAlert> list = null;
            Dictionary<int, ProductCatalog> relatedProducts = new Dictionary<int, ProductCatalog>();
            int cc = 0;
            try
            {

                var lists = new PriceMe.Bll.LinqBllBase<PriceMeDBA.LinqEntity.CSK_Store_ProductAlert>();
                list=lists.Query(q => q.ParseID == user);
               // list = CSK_Store_ProductAlert.Find(p => p.UserId == user).ToList();
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        ProductCatalog pc = PriceMeCommon.ProductSearcher.GetProductByProductID(item.ProductId);
                        if (pc != null)
                        {
                            if (!relatedProducts.ContainsKey(item.ProductId))
                                relatedProducts.Add(item.ProductId, pc);
                            cc++;
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

        public static CSK_Store_ProductAlert GetUserPriceAlert(int aid)
        {
            CSK_Store_ProductAlert obj= null;
            try
            {
                var list = CSK_Store_ProductAlert.Find(p => p.AlertId == aid).ToList();
                if(list!=null || list.Count > 0)
                    obj = list[0];
            }
            catch (Exception)
            {
            }
            return obj;
        }
    }
}
