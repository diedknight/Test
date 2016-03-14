using SubSonic.Schema;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetailerDataReview.DataModel
{
    public class Retailer:BaseModel
    {
        public int RetailerId { get; set; }
        public string RetailerName { get; set; }
        public string RetailerURL { get; set; }
        public string GSTNumber { get; set; }
        public string CompanyRegNumber { get; set; }
        public decimal CCFee { get; set; }

        public int RetailerCountry { get; set; }

        public static List<Retailer> GetList(int[] Ids = null)
        {
            List<Retailer> list = new List<Retailer>();

            string sql = " select RetailerId,RetailerName,RetailerURL,GSTNumber,CompanyRegNumber,CCFee,RetailerCountry from Csk_Store_Retailer";
            sql += " where RetailerStatus = 1 and IsSetupComplete = 1 and RetailerCountry = " + ConfigAppString.CountryId + " and RetailerId in (select retailerid from CSK_Store_PPCMember where PPCMemberTypeID = 2)";
            if (Ids != null && Ids.Length != 0)
            {
                string tempIds = "";

                foreach (int id in Ids)
                {
                    tempIds += id + ",";
                }
                tempIds = tempIds.Length == 0 ? "" : tempIds.Substring(0, tempIds.Length - 1);

                sql += " and RetailerId in (" + tempIds + ")";
            }

            var sp = new StoredProcedure("", SubSonic.DataProviders.ProviderFactory.GetProvider("CommerceTemplate"));
            sp.Command.CommandSql = sql;
            sp.Command.CommandTimeout = 0;
            sp.Command.CommandType = CommandType.Text;
            var dr = sp.ExecuteReader();

            while (dr.Read())
            {
                Retailer retailer = new Retailer();
                retailer.RetailerId = dr["RetailerId"] == DBNull.Value ? 0 : Convert.ToInt32(dr["RetailerId"]);
                retailer.RetailerName = dr["RetailerName"].ToString();
                retailer.RetailerURL = dr["RetailerURL"].ToString();
                retailer.GSTNumber = dr["GSTNumber"].ToString();
                retailer.CompanyRegNumber = dr["CompanyRegNumber"].ToString();
                retailer.CCFee = dr["CCFee"] == DBNull.Value ? 0 : Convert.ToDecimal(dr["CCFee"]);
                retailer.RetailerCountry = dr["RetailerCountry"] == DBNull.Value ? 0 : Convert.ToInt32(dr["RetailerCountry"]);

                list.Add(retailer);
            }

            dr.Close();

            return list;
        }

        protected override string ConnectionNameString()
        {
            return "CommerceTemplate";
        }
    }
}
