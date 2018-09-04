using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PurgeCloudflareCacheService
{
    public static class DBController
    {
        public static DbConnection CreateDBConnection(DbInfo dbInfo)
        {
            DbProviderFactory factory = DbProviderFactories.GetFactory(dbInfo.ProviderName);

            DbConnection dbConnection = factory.CreateConnection();
            dbConnection.ConnectionString = dbInfo.ConnectionString;

            return dbConnection;
        }

        public static DbDataAdapter CreateDataAdapter(DbInfo dbInfo)
        {
            DbProviderFactory factory = DbProviderFactories.GetFactory(dbInfo.ProviderName);
            var dbDataAdapter = factory.CreateDataAdapter();

            if (dbDataAdapter == null)
            {
                if ("MySql.Data.MySqlClient" == dbInfo.ProviderName)
                {
                    dbDataAdapter = (DbDataAdapter)System.Reflection.Assembly.Load("MySql.Data").CreateInstance("MySql.Data.MySqlClient.MySqlDataAdapter");
                }
            }

            return dbDataAdapter;
        }

        public static DbCommand CreateDbCommand(string sqlString, DbConnection dbConnection)
        {
            DbCommand dbCommand = dbConnection.CreateCommand();
            dbCommand.CommandText = sqlString;
            dbCommand.CommandTimeout = 0;
            return dbCommand;
        }
    }
}