using ExpertReviewIndex.Data;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace ExpertReviewIndex.MySqlController
{
    public static class DBController
    {
        public static void GetUserReviewData(out Dictionary<int, ProductVotesSum> pvsDic, out List<int> pvsList)
        {
            pvsDic = new Dictionary<int, ProductVotesSum>();
            pvsList = new List<int>();

            string sql = "Select ProductID, ProductRatingVotes, ProductRatingSum From CSK_Store_ProductVotesSum";
            using (var idr = ExecuteDataReader(sql, null))
            {
                while (idr.Read())
                {
                    ProductVotesSum er = new ProductVotesSum();
                    er.ProductID = int.Parse(idr["ProductID"].ToString());
                    er.ProductRatingVotes = int.Parse(idr["ProductRatingVotes"].ToString());
                    er.ProductRatingSum = int.Parse(idr["ProductRatingSum"].ToString());

                    if (!pvsDic.ContainsKey(er.ProductID))
                        pvsDic.Add(er.ProductID, er);

                    if (!pvsList.Contains(er.ProductID))
                        pvsList.Add(er.ProductID);
                }
            }
        }

        public static IDataReader ExecuteDataReader(string sqltext, MySqlParameter[] param)
        {
            MySqlConnection conn = new MySqlConnection(SiteConfig.ConnectionStrings("conn_mysql"));

            conn.Open();
            MySqlCommand comm = new MySqlCommand(sqltext, conn);
            if (param != null && param.Length > 0)
                comm.Parameters.AddRange(param);

            IDataReader idr = comm.ExecuteReader();

            return idr;
        }
    }
}
