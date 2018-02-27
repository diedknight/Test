using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimilarityMatchTool
{
    public class DB
    {
        public static SqlConnection Connection
        {
            get
            {
                return new SqlConnection(AppConfig.DBStr);
            }
        }
    }
}
