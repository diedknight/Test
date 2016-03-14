using SubSonic.Schema;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetailerDataReview.DataModel
{
    public class PaymentOption:BaseModel
    {
        public int PaymentOptionId { get; set; }
        public string Name { get; set; }

        public int Order { get; set; }

        protected override string ConnectionNameString()
        {
            return "CommerceTemplate";
        }

        public static List<PaymentOption> GetList(int countryId)
        {
            List<PaymentOption> list = new List<PaymentOption>();

            string sql = " select PaymentId as PaymentOptionId,Name from CSK_Store_PaymentOption";
            sql += " where PaymentId in (";
            sql += "    select paymentoptionid from CSK_Store_RetailerPaymentOption";
            sql += "    where RetailerId in (select RetailerId from CSK_Store_Retailer where RetailerCountry = " + countryId + ")";
            sql += " )";

            var sp = new StoredProcedure("");
            sp.Command.CommandSql = sql;
            sp.Command.CommandTimeout = 0;
            sp.Command.CommandType = CommandType.Text;
            var dr = sp.ExecuteReader();

            while (dr.Read())
            {
                PaymentOption info = new PaymentOption();

                info.PaymentOptionId = dr["PaymentOptionId"] == DBNull.Value ? 0 : Convert.ToInt32(dr["PaymentOptionId"]);
                info.Name = dr["Name"].ToString();

                list.Add(info);
            }

            dr.Close();

            return list;
        }


    }
}
