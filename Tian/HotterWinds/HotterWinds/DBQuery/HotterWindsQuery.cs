using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dapper;
using System.Data.SqlClient;

namespace HotterWinds.DBQuery
{
    public class HotterWindsQuery
    {
        protected static SqlConnection GetConnection()
        {
            string conStr = System.Configuration.ConfigurationManager.ConnectionStrings["HotterWinds"].ConnectionString;

            return new SqlConnection(conStr);
        }
    }
}