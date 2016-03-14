using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace RetailerDataReview.DataModel
{
    public abstract class BaseModel
    {
        protected abstract string ConnectionNameString();

        protected DbDataReader ExecuteReader(string str)
        {
            string conStr = ConfigAppString.GetConnection(this.ConnectionNameString());

            SqlConnection con = new SqlConnection(conStr);

            con.Open();

            SqlCommand cmd = new SqlCommand(str, con);
            cmd.CommandType = CommandType.Text;

            return cmd.ExecuteReader();
        }

        protected object ExecuteScalar(string str)
        {
            string conStr = ConfigAppString.GetConnection(this.ConnectionNameString());

            using (SqlConnection con = new SqlConnection(conStr))
            {
                con.Open();

                SqlCommand cmd = new SqlCommand(str, con);
                cmd.CommandType = CommandType.Text;

                return cmd.ExecuteScalar();
            }
        }

    }
}
