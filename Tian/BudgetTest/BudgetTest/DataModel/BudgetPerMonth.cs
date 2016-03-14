using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetTest.DataModel
{
    public class BudgetPerMonth:BaseModel
    {
        public int RetailerId { get; set; }
        public string RetailerName { get; set; }
        public decimal Cost { get; set; }
        public decimal Balance { get; set; }
        public decimal Budget { get; set; }

        protected override string ConnectionNameString()
        {
            return "CommerceTemplate";
        }

        public static List<BudgetPerMonth> GetList()
        {
            List<BudgetPerMonth> list = new List<BudgetPerMonth>();

            BudgetPerMonth db = new BudgetPerMonth();

            DateTime currentMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            int days = currentMonth.AddMonths(1).AddDays(-1).Day;

            string sql = @"select T.retailerid,R.retailername,count(*)*fixedcpcrate cost,DailyCap*{0}-count(*)*fixedcpcrate balance,DailyCap*{0} budget from CSK_Store_RetailerTracker T inner join csk_store_retailer R on T.retailerid=R.retailerid inner join csk_store_ppcmember P on R.retailerid=P.retailerid where T.CreatedOn>'{1}'  and UserIP not in
                            (select ipaddress from CSK_Store_IP_Blacklist)   and ppcmembertypeid=2 and P.IsAutomatedInvoice=1 
                            group by T.retailerid,R.retailername,DailyCap*{0},fixedcpcrate
                            order by balance";

            sql = string.Format(sql, days, currentMonth.ToString("yyyy-MM-dd"));

            var dr = db.ExecuteReader(sql);

            while (dr.Read())
            {
                BudgetPerMonth info = new BudgetPerMonth();
                info.RetailerId = dr["retailerid"] == DBNull.Value ? 0 : Convert.ToInt32(dr["retailerid"]);
                info.RetailerName = dr["retailername"].ToString();
                info.Cost = dr["cost"] == DBNull.Value ? 0m : Convert.ToDecimal(dr["cost"]);
                info.Balance = dr["balance"] == DBNull.Value ? 0m : Convert.ToDecimal(dr["balance"]);
                info.Budget = dr["budget"] == DBNull.Value ? 0m : Convert.ToDecimal(dr["budget"]);

                list.Add(info);
            }

            dr.Close();

            list = list.Where(item => item.Balance < 0m).ToList();

            return list;
        }
    }
}
