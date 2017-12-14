using AliexpressImport.Data;
using SubSonic.Schema;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AliexpressImport.BusinessLogic
{
    public static class CategoryContronller
    {
        public static List<CategoryData> listCategory;

        static CategoryContronller()
        {
            Init();
        }

        private static void Init()
        {
            LoadCategorys();
        }

        private static void LoadCategorys()
        {
            listCategory = new List<CategoryData>();
            string sql = "Select Id, Name From Category";
            StoredProcedure sp = new StoredProcedure("");
            sp.Command.CommandSql = sql;
            sp.Command.CommandTimeout = 0;
            sp.Command.CommandType = CommandType.Text;
            IDataReader dr = sp.ExecuteReader();
            while (dr.Read())
            {
                int id = 0;
                int.TryParse(dr["Id"].ToString(), out id);

                CategoryData c = new CategoryData();
                c.CategoryId = id;
                c.CategoryName = dr["Name"].ToString();
                listCategory.Add(c);
            }
            dr.Close();
        }
    }
}
