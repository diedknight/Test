using SubSonic.Schema;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetailerDataReview.DataModel
{
    public class RetailerPaymentOption : BaseModel
    {
        public int RetailerId { get; set; }
        public int PaymentOptionId { get; set; }

        protected override string ConnectionNameString()
        {
            return "CommerceTemplate";
        }

        public static List<RetailerPaymentOption> GetList(int retailerId)
        {
            List<RetailerPaymentOption> list = new List<RetailerPaymentOption>();

            string sql = "select RetailerId,PaymentOptionId from CSK_Store_RetailerPaymentOption where RetailerId=" + retailerId;

            var sp = new StoredProcedure("");
            sp.Command.CommandSql = sql;
            sp.Command.CommandTimeout = 0;
            sp.Command.CommandType = CommandType.Text;
            var dr = sp.ExecuteReader();

            while (dr.Read())
            {
                RetailerPaymentOption info = new RetailerPaymentOption();
                info.RetailerId = dr["RetailerId"] == DBNull.Value ? 0 : Convert.ToInt32(dr["RetailerId"]);
                info.PaymentOptionId = dr["PaymentOptionId"] == DBNull.Value ? 0 : Convert.ToInt32(dr["PaymentOptionId"]);

                list.Add(info);
            }

            dr.Close();

            return list;
        }

    }
}
