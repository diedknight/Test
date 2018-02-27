using SimilarityMatchTool.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace SimilarityMatchTool
{
    public class ProductCtrl
    {
        public static List<ProductData> GetCache1()
        {
            List<ProductData> list = new List<ProductData>();

            using (var con = DB.Connection)
            {
                var Ids = CateCtrl.GetConfigCategoryList().Select(item => item.CategoryId).ToList();

                list = con.Query<ProductData>(SqlConfig.cache1, new { CIds = Ids }).ToList();
            }

            return list;
        }

        public static List<ProductData> GetCache2()
        {
            List<ProductData> list = new List<ProductData>();

            using (var con = DB.Connection)
            {
                var Ids = CateCtrl.GetConfigCategoryList().Select(item => item.CategoryId).ToList();

                list = con.Query<ProductData>(SqlConfig.cache2, new { CIds = Ids }).ToList();
            }

            return list;
        }


    }
}
