using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Dapper;


namespace ImportAttrToExcel
{
    public class CategoryCtrl
    {
        private static Dictionary<int, string> _dic = new Dictionary<int, string>();


        public static string GetCategoryName(int Id)
        {
            string name = "";

            if (_dic.ContainsKey(Id))
            {
                name = _dic[Id];
            }
            else
            {
                using (var con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["PriceMe_DB"].ConnectionString))
                {
                    string sql = "select top 1 CategoryName from CSK_Store_Category where CategoryID=@CId";

                    name = con.ExecuteScalar<string>(sql, new { CID = Id });
                }

                _dic.Add(Id, name);
            }

            return name;
        }

    }
}
