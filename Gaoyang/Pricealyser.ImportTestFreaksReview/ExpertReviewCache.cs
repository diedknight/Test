using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SubSonic.Schema;
using System.Data;

namespace Pricealyser.ImportTestFreaksReview
{
     class ExpertReviewCache
    {

         public ExpertReviewCache()
         {
             LoadFeatureScoreCache();
             LoadExpertReviewCache();
         }

         List<int> _FeatureScoreList;

        public  List<int> FeatureScoreList
        {
            get { return this._FeatureScoreList; }
            set { this._FeatureScoreList = value; }
        }

        List<string> _ExpertReviewList;

        public  List<string> ExpertReviewList
        {
            get { return this._ExpertReviewList; }
            set { this._ExpertReviewList = value; }
        }
         string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["CommerceTemplate"].ConnectionString;

       

        public  void LoadFeatureScoreCache()
        {
            _FeatureScoreList = new List<int>();
            string sql = "Select distinct(ProductID) From CSK_Store_ExpertReviewFeatureScore";
            using (System.Data.SqlClient.SqlConnection sqlConn = new System.Data.SqlClient.SqlConnection(connectionString))
            {
                sqlConn.Open();
                using (System.Data.SqlClient.SqlCommand sqlCMD = new System.Data.SqlClient.SqlCommand())
                {
                    sqlCMD.CommandText = sql;
                    sqlCMD.CommandTimeout = 0;
                    sqlCMD.CommandType = System.Data.CommandType.Text;
                    sqlCMD.Connection = sqlConn;

                    using (IDataReader dr = sqlCMD.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            int pid = 0;
                            int.TryParse(dr["ProductID"].ToString(), out pid);
                            _FeatureScoreList.Add(pid);
                        }
                    }
                }
            }
            //StoredProcedure sp = new StoredProcedure("");
            //sp.Command.CommandSql = sql;
            //sp.Command.CommandType = System.Data.CommandType.Text;
            //sp.Command.CommandTimeout = 0;
            //IDataReader dr = sp.ExecuteReader();
            //while (dr.Read())
            //{
            //    int pid = 0;
            //    int.TryParse(dr["ProductID"].ToString(), out pid);
            //    _FeatureScoreList.Add(pid);
            //}
            //dr.Close();
        }

        public  void LoadExpertReviewCache()
        {
            _ExpertReviewList = new List<string>();
            string sql = "Select ProductID, SourceID From CSK_Store_ExpertReviewAU";
            using (System.Data.SqlClient.SqlConnection sqlConn = new System.Data.SqlClient.SqlConnection(connectionString))
            {
                sqlConn.Open();
                using (System.Data.SqlClient.SqlCommand sqlCMD = new System.Data.SqlClient.SqlCommand())
                {
                    sqlCMD.CommandText = sql;
                    sqlCMD.CommandTimeout = 0;
                    sqlCMD.CommandType = System.Data.CommandType.Text;
                    sqlCMD.Connection = sqlConn;

                    using (IDataReader dr = sqlCMD.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            string pid = dr["ProductID"].ToString();
                            string sid = dr["SourceID"].ToString();
                            //string revUrl = dr["ReviewURL"].ToString();+ "|" + revUrl
                            string key = pid + "|" + sid ;
                            if (!_ExpertReviewList.Contains(key))
                                _ExpertReviewList.Add(key);
                        }
                    }
                }
                sqlConn.Close();
            }

            //StoredProcedure sp = new StoredProcedure("");
            //sp.Command.CommandSql = sql;
            //sp.Command.CommandType = System.Data.CommandType.Text;
            //sp.Command.CommandTimeout = 0;
            //IDataReader dr = sp.ExecuteReader();
            //while (dr.Read())
            //{
            //    string pid = dr["ProductID"].ToString();
            //    string sid = dr["SourceID"].ToString();
            //    string key = pid + "|" + sid;
            //    if (!_ExpertReviewList.Contains(key))
            //        _ExpertReviewList.Add(key);
            //}
            //dr.Close();
        }
    }
}
