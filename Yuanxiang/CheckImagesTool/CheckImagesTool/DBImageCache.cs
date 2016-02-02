using SubSonic.Schema;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckImagesTool
{
    public class DBImageCache
    {
        private class TableInfo
        {
            public string TableName { get; set; }
            public string ColumnName { get; set; }
            public bool IsInit { get; set; }
            public string PrimaryKey { get; set; }
        }
        
        private static List<TableInfo> _list = null;
        private static DBImageCache _obj = null;
        private static bool _isInit = false;        

        private DBImageCache()
        {
            _list = new List<TableInfo>();            
        }

        public static DBImageCache Instance
        {
            get
            {
                if (_obj == null)
                {
                    _obj = new DBImageCache();
                }

                return _obj;
            }
        }

        public void Add(string tableName, string columnName)
        {
            if (_list.FirstOrDefault(item => item.TableName == tableName && item.ColumnName == columnName) != null) return;

            string primaryKey = "";

            switch (tableName.ToLower())
            {
                case "csk_store_product": primaryKey = "ProductID"; break;
                case "csk_store_retailerproduct": primaryKey = "RetailerProductId"; break;
                case "csk_store_image": primaryKey = "ImageID"; break;
                case "csk_store_manufacturer": primaryKey = "ManufacturerID"; break;
                case "csk_store_productismerged_temp": primaryKey = "MID"; break;
                case "csk_store_category": primaryKey = "CategoryID"; break;
                case "csk_util_country": primaryKey = "countryID"; break;
            }

            if (primaryKey == "") throw new Exception("table " + tableName + " is not configure primary key! the primary key is hard code");

            _list.Add(new TableInfo() { TableName = tableName, ColumnName = columnName, IsInit = false, PrimaryKey = primaryKey });
            _isInit = false;
        }

        public bool Compare(string fileName)
        {
            try
            {
                if (string.IsNullOrEmpty(fileName)) return false;

                string tempStr = Path.GetFileName(fileName);

                if (string.IsNullOrEmpty(tempStr)) return false;
                if (string.IsNullOrEmpty(Path.GetExtension(tempStr))) return false;

                return IndexController.Compare(tempStr);
            }
            catch (Exception ex)
            {
                Console.WriteLine(fileName);
                Console.WriteLine("error:" + ex.Message + " " + ex.StackTrace);

                return false;
            }
        }

        public void Init()
        {
            if (_isInit) return;

            this.RunTable(50000, item =>
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(item)) return;

                    string fileName = Path.GetFileName(item);

                    if (string.IsNullOrEmpty(fileName)) return;
                    if (string.IsNullOrEmpty(Path.GetExtension(fileName))) return;

                    IndexController.Add(fileName);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(item);
                    Console.WriteLine("error:" + ex.Message + " " + ex.StackTrace);                    
                }
            });

            _isInit = true;
        }

        private void RunTable(int pageSize, Action<string> action)
        {
            _list.ForEach(item =>
            {
                if (item.IsInit) return;

                int pageIndex = 1;
                while (true)
                {
                    bool isOver = true;
                    string sql = " select " + item.ColumnName + " from";
                    sql += " (";
                    sql += " select ROW_NUMBER() over(ORDER BY " + item.PrimaryKey + " asc) as xbai_num," + item.ColumnName + " from " + item.TableName;
                    sql += " ) as a";
                    sql += " where xbai_num BETWEEN " + ((pageIndex - 1) * pageSize + 1) + " AND " + pageIndex * pageSize;

                    StoredProcedure sp = new StoredProcedure("");
                    sp.Command.CommandSql = sql;
                    sp.Command.CommandTimeout = 0;
                    sp.Command.CommandType = CommandType.Text;
                    IDataReader dr = sp.ExecuteReader();
                    while (dr.Read())
                    {
                        isOver = false;

                        string val = dr[item.ColumnName].ToString();
                        if (item.TableName.ToLower() == "csk_store_category")
                        {
                            val = val.Replace("_s.", ".");
                            val = val.Replace("_ms.", ".");
                        }

                        action(val);
                    }
                    dr.Close();

                    pageIndex++;
                    if (isOver) break;
                }
            
                item.IsInit = true;
            });
        }

    }
}
