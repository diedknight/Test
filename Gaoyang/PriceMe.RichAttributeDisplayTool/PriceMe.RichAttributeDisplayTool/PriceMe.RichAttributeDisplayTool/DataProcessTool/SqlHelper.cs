using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Reflection;
using System.Security;
using PriceMe.RichAttributeDisplayTool.RichClass;




namespace PriceMe.RichAttributeDisplayTool.DataProcessTool
{
    public class SqlHelper
    {
        public static string connStr = ConfigurationManager.ConnectionStrings["PriceMe"].ConnectionString;

        

        public SqlHelper() { 
            
        }

        public SqlHelper(string sqlconn)
        {
            connStr = sqlconn;
        }

        /// <summary>
        /// 获取所有要跑的分类
        /// </summary>
        /// <returns></returns>
        public static List<T> sqlReader<T>(string sql) where T:IClass,new()
        {
            
            #region sql
                List<T> commonlist = new List<T>();

                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    if (conn.State == ConnectionState.Closed)
                        conn.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.CommandTimeout = 3600;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            while (sdr.Read())
                            {
                                T list = new T();

                                list.GetPropertyInfoArray<T>().ToList().ForEach(f =>
                                {
                                    //if (!string.IsNullOrEmpty(sdr[f.Name].ToString()))
                                    f.SetValue(list, Utility.ChangeType(sdr[f.Name],f.PropertyType),null);
                                });
                                
                                commonlist.Add(list);
                            }
                            conn.Close();
                        }
                    }
                }

            #endregion

            return commonlist;
        }


        /// <summary>
        /// 测试SQL语句的大批量添加(无条数限制)
        /// </summary>
        /// <param name="sql">Sql语句</param>
        /// <returns>返回受影响的行数</returns>
        public static string InsertToTables(List<AttributeCategoryComparisons> list) 
        {
            string err_msg = "";

            if (list.Count <= 0) return "error";

            var dt = new DataTable("AttributeCategoryComparison");
            dt.Columns.Add("Aid", typeof(Int32));
            dt.Columns.Add("IsHigherBetter", typeof(Boolean));
            dt.Columns.Add("Top10", typeof(String));
            dt.Columns.Add("Top20", typeof(String));
            dt.Columns.Add("Top30", typeof(String));
            dt.Columns.Add("Average", typeof(String));
            dt.Columns.Add("Bottom30", typeof(String));
            dt.Columns.Add("Bottom20", typeof(String));
            dt.Columns.Add("Bottom10", typeof(String));
            dt.Columns.Add("Createdon", typeof(DateTime));
            dt.Columns.Add("Modifiedon",typeof(DateTime));

            foreach (var r in list)
            {
                var dr = dt.NewRow();

                dr["Aid"] = r.Aid;
                dr["IsHigherBetter"] = r.IsHigherBetter;
                dr["Top10"] = r.Top10;
                dr["Top20"] = r.Top20;
                dr["Top30"] = r.Top30;
                dr["Bottom10"] = r.Bottom10;
                dr["Bottom20"] = r.Bottom20;
                dr["Bottom30"] = r.Bottom30;
                dr["Average"] = r.Average;
                dr["Createdon"] = r.Createon??DateTime.Now;
                dr["Modifiedon"] = r.Modifiedon??DateTime.Now;

                dt.Rows.Add(dr);
            }

            
            using (var conn = new SqlConnection(connStr))
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                //开始事务
                using (var tran = conn.BeginTransaction())
                {
                    using (var sbc = new SqlBulkCopy(conn, SqlBulkCopyOptions.Default, tran))
                    {
                        sbc.BulkCopyTimeout = 100;//超时之前操作完成所允许的秒数
                        sbc.BatchSize = dt.Rows.Count;//每一批次中的行数
                        sbc.DestinationTableName = dt.TableName;//服务器上目标表的名称
                        try
                        {
                            //映射定义数据源中的列和目标表中的列之间的关系
                            foreach (DataColumn col in dt.Columns)
                                sbc.ColumnMappings.Add(col.ColumnName, col.ColumnName);
                            sbc.WriteToServer(dt);
                            tran.Commit();
                            err_msg = "";
                        }
                        catch (Exception ex)
                        {
                            err_msg = ex.Message;
                            tran.Rollback();
                            throw ex;
                        }
                        finally
                        {
                            conn.Close();
                        }
                    }

                }
            }
            return err_msg;

        }

        /// <summary>
        /// 获取AttributeCategoryComparison对象
        /// </summary>
        /// <returns></returns>
        public static AttributeCategoryComparison getACC(int aid) {
            var single =new LinqBllBase<AttributeCategoryComparison>();
            return single.Single(s => s.Aid == aid);
        }


    }
}
