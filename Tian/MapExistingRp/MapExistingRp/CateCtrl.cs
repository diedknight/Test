using MapExistingRp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace MapExistingRp
{
    public class CateCtrl
    {
        public static List<CategoryData> Get()
        {
            List<CategoryData> list = new List<CategoryData>();

            string sqlStr = AppConfig.CIdSql;

            using (var con = DB.DBConnection)
            {
                list = con.Query<CategoryData>(sqlStr).ToList();
            }

            return list;
        }
    }
}
