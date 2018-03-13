using SubSonic.Schema;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckRetailerReviewEmail
{
    public class RetailerReviews
    {
        StreamWriter _sw;
        public StreamWriter SW
        {
            get { return _sw; }
            set { _sw = value; }
        }

        private List<ReviewsData> listDatas;

        public void Check()
        {
            Write("Begin......" + DateTime.Now);
            GetDatas();
            Write("Get "+listDatas.Count+" reviews email......" + DateTime.Now);

            foreach (ReviewsData data in listDatas)
            {
                if (data.ModifiedOn.AddDays(14) < DateTime.Today)
                {
                    UpdateReviewStatus(data.Id);
                    Write("MerchantReviewID " + data.Id + " update review status......" + DateTime.Now);
                }
            }
            
            Write("End......" + DateTime.Now);
        }

        private void UpdateReviewStatus(int mid)
        {
            string sql = "update Merchant_Reviews set ReviewStatus = 7, ModifiedOn = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' "
                        + "Where MerchantReviewID = " + mid;
            StoredProcedure sp = new StoredProcedure("");
            sp.Command.CommandSql = sql;
            sp.Command.CommandTimeout = 0;
            sp.Command.CommandType = CommandType.Text;
            sp.Execute();
        }

        private void GetDatas()
        {
            listDatas = new List<ReviewsData>();
            string sql = "select MerchantReviewID, ModifiedOn from Merchant_Reviews where ReviewStatus = 8";
            StoredProcedure sp = new StoredProcedure("");
            sp.Command.CommandSql = sql;
            sp.Command.CommandTimeout = 0;
            sp.Command.CommandType = CommandType.Text;
            IDataReader dr = sp.ExecuteReader();
            while (dr.Read())
            {
                int mid = 0;
                int.TryParse(dr["MerchantReviewID"].ToString(), out mid);
                DateTime modifiedon = DateTime.Now;
                DateTime.TryParse(dr["ModifiedOn"].ToString(), out modifiedon);

                ReviewsData data = new ReviewsData();
                data.Id = mid;
                data.ModifiedOn = modifiedon;

                listDatas.Add(data);
            }
            dr.Close();
        }

        private void Write(string info)
        {
            System.Console.WriteLine(info);

            _sw.WriteLine(info);
            _sw.WriteLine(_sw.NewLine);
            _sw.Flush();
        }
    }
}
