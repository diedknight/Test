using SimilarityMatchTool.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace SimilarityMatchTool
{
    public class CateCtrl
    {
        private static List<CategoryData> _configCategoryList = null;

        public static List<CategoryData> GetConfigCategoryList()
        {
            if (_configCategoryList == null)
            {

                _configCategoryList = new List<CategoryData>();

                using (var con = DB.Connection)
                {
                    _configCategoryList = con.Query<CategoryData>(AppConfig.CID).ToList();
                }
            }

            return _configCategoryList;
        }
    }
}
