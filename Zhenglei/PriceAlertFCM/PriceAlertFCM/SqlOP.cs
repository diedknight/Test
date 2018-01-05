using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceAlertFCM
{
    public static class SqlOP
    {
        static string DBConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["CommerceTemplate"].ConnectionString;

        public static SqlConnection CreateSqlConnection()
        {
            SqlConnection sqlCon = new SqlConnection(DBConnectionString);
            return sqlCon;
        }

        public static SqlCommand CreateTextSqlCommand(string sql)
        {
            SqlCommand sqlCmd = new SqlCommand(sql);
            sqlCmd.CommandTimeout = 0;
            return sqlCmd;
        }

        public static SqlCommand CreateStoredProcedureSqlCommand(string spName)
        {
            SqlCommand sqlCmd = new SqlCommand(spName);
            sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
            sqlCmd.CommandTimeout = 0;
            return sqlCmd;
        }
    }
}
