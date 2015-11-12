using SubSonic.Schema;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckCategoryTool
{
    public class CategoryTool
    {
        StreamWriter _sw;
        public StreamWriter SW
        {
            get { return _sw; }
            set { _sw = value; }
        }

        List<int> listCates;
        List<int> listCountry;
        int CountryId;
        decimal PriceRateJudge;

        public void Tools()
        {
            Write("Begin......" + DateTime.Now);

            decimal.TryParse(ConfigurationManager.AppSettings["CategoryPriceJudge"].ToString().Replace("%", ""), out PriceRateJudge);
            PriceRateJudge = decimal.Round(PriceRateJudge / 100, 2);

            string stringCates = ConfigurationManager.AppSettings["CategoryId"].ToString();
            listCates = new List<int>();
            if (stringCates == "0")
                GetCategorys();
            else
            {
                string[] temps = stringCates.Split(',');
                foreach (string temp in temps)
                {
                    int cateid = 0;
                    int.TryParse(temp, out cateid);
                    if (cateid != 0)
                        listCates.Add(cateid);
                }
            }
            Write("Get " + listCates.Count + " category......" + DateTime.Now);

            listCountry = new List<int>();
            string stringCountry = ConfigurationManager.AppSettings["CountryId"].ToString();
            string[] countrys = stringCountry.Split(',');
            foreach (string temp in countrys)
            {
                int cid = 0;
                int.TryParse(temp, out cid);
                listCountry.Add(cid);
            }

            foreach (int countryid in listCountry)
            {
                CountryId = countryid;
                Write("Get " + countryid + " country......" + DateTime.Now);

                foreach (int cid in listCates)
                {
                    Write("Categoryid: " + cid + " ......" + DateTime.Now);
                    bool isNew = false;
                    decimal maxprice = 0;
                    decimal minprice = 0;
                    bool isMaxError = false;
                    bool isMinError = false;
                    GetCategoryPriceHistory(cid, out maxprice, out minprice, out isMaxError, out isMinError);
                    if (maxprice == 0)
                        isNew = true;

                    decimal newMaxPrice, newMinPrice;
                    int maxRpid, maxPid, minRpid, minPid;
                    GetRetailerPrice(cid, "desc", out newMaxPrice, out maxRpid, out maxPid);
                    GetRetailerPrice(cid, "asc", out newMinPrice, out minRpid, out minPid);

                    if (newMaxPrice == 0) continue;

                    if (isNew)
                        InsertCategoryPriceHistory(cid, newMaxPrice, newMinPrice, maxRpid, maxPid, minRpid, minPid, 0, 0);
                    else
                    {
                        Write("History Maxprice: " + maxprice + "    Minprice: " + minprice);
                        isMaxError = false;
                        isMinError = false;
                        if (maxprice != newMaxPrice || minprice != newMinPrice)
                        {
                            decimal rateMax = decimal.Round(Math.Abs(newMaxPrice - maxprice) / maxprice, 2);
                            if ((newMaxPrice > maxprice) && rateMax > PriceRateJudge)
                                isMaxError = true;
                            decimal rateMin = decimal.Round(Math.Abs(newMinPrice - minprice) / minprice, 2);
                            if ((newMinPrice < minprice) && rateMin > PriceRateJudge)
                                isMinError = true;

                            Write("Current Maxprice: " + newMaxPrice + "    Minprice: " + newMinPrice);
                            Write("Current Maxprice rate: " + rateMax + "    Minprice rate: " + rateMin);
                            InsertCategoryPriceHistory(cid, newMaxPrice, newMinPrice, maxRpid, maxPid, minRpid, minPid, (isMaxError ? 1 : 0), (isMinError ? 1 : 0));
                        }
                    }
                }
            }

            Write("End......" + DateTime.Now);
        }

        private void InsertCategoryPriceHistory(int cid, decimal maxPrice, decimal minPrice, int maxRpid, int maxPid, int minRpid, int minPid, int isErrorMax, int isErrorMin)
        {
            string sql = "Insert CSK_Store_CategoryPriceHistory Values (" + maxPid + ", " + maxRpid + ", " + minPid + ", " + minRpid + ", "
                        + "" + cid + ", " + CountryId + ", " + maxPrice + ", " + minPrice + ", " + isErrorMax + ", " + isErrorMin + ", "
                        + "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', 0)";
            try
            {
                StoredProcedure sp = new StoredProcedure("");
                sp.Command.CommandSql = sql;
                sp.Command.CommandTimeout = 0;
                sp.Command.CommandType = CommandType.Text;
                sp.Execute();
            }
            catch (Exception ex) { Write("insert error sql: " + sql + ex.Message + ex.StackTrace); }
        }

        private void UpdateCategoryPriceHistory(int cid, decimal maxPrice, decimal minPrice, int maxRpid, int maxPid, int minRpid, int minPid, bool isErrorMax, bool isErrorMin)
        {
            int intIsErrorMax = isErrorMax ? 1 : 0;
            int intIsErrorMin = isErrorMin ? 1 : 0;

            string sql = "Update CSK_Store_CategoryPriceHistory Set MaxPID = " + maxPid + ", MaxRPID = " + maxRpid + ", MinPID = " + minPid + ", "
                        + "MinRPID = " + minRpid + ", MaxPrice = " + maxPrice + ", MInPrice = " + minPrice + ", isErrorMax = " + intIsErrorMax + ", "
                        + "isErrorMin = " + intIsErrorMin + " Where CategoryID = " + cid + " And CountryID = " + CountryId;
            try
            {
                StoredProcedure sp = new StoredProcedure("");
                sp.Command.CommandSql = sql;
                sp.Command.CommandTimeout = 0;
                sp.Command.CommandType = CommandType.Text;
                sp.Execute();
            }
            catch (Exception ex) { Write("update error sql: " + sql + ex.Message + ex.StackTrace); }
        }

        private void GetCategoryPriceHistory(int cid, out decimal maxprice, out decimal minprice, out bool isMaxError, out bool isMinError)
        {
            maxprice = 0;
            minprice = 0;
            isMaxError = false;
            isMinError = false;
            string sql = "Select top 1 MaxPrice, MinPrice, isErrorMax, isErrorMin From CSK_Store_CategoryPriceHistory Where CategoryID = "
                        + cid + " And CountryID = " + CountryId + " Order by CreatedOn Desc";
            try
            {
                StoredProcedure sp = new StoredProcedure("");
                sp.Command.CommandSql = sql;
                sp.Command.CommandTimeout = 0;
                sp.Command.CommandType = CommandType.Text;
                IDataReader dr = sp.ExecuteReader();
                while (dr.Read())
                {
                    decimal.TryParse(dr["MaxPrice"].ToString(), out maxprice);
                    decimal.TryParse(dr["MinPrice"].ToString(), out minprice);
                    bool.TryParse(dr["isErrorMax"].ToString(), out isMaxError);
                    bool.TryParse(dr["isErrorMin"].ToString(), out isMinError);
                }
                dr.Close();
            }
            catch (Exception ex) { Write("Get CategoryPriceHistory error sql: " + sql + ex.Message + ex.StackTrace); }
        }

        private void GetCategorys()
        {
            string sql = "select CategoryID from CSK_Store_Category where IsDisplayIsMerged = 0";
            StoredProcedure sp = new StoredProcedure("");
            sp.Command.CommandSql = sql;
            sp.Command.CommandTimeout = 0;
            sp.Command.CommandType = CommandType.Text;
            IDataReader dr = sp.ExecuteReader();
            while (dr.Read())
            {
                int cid = 0;
                int.TryParse(dr["CategoryID"].ToString(), out cid);
                listCates.Add(cid);
            }
            dr.Close();
        }

        private void GetRetailerPrice(int cid, string sort, out decimal price, out int rpid, out int pid)
        {
            price = 0;
            rpid = 0;
            pid = 0;
            string sql = "Select top 1 rp.RetailerPrice, rp.RetailerProductId, rp.ProductId From CSK_Store_RetailerProduct rp inner join CSK_Store_Product p "
                        + "On rp.ProductId = p.ProductID Where p.CategoryID = " + cid + " And rp.RetailerProductStatus = 1 And "
                        + "rp.IsDeleted = 0 And p.IsMerge = 1 And RetailerId in (Select RetailerId From CSK_Store_Retailer "
                        + "Where RetailerStatus = 1 And RetailerCountry = " + CountryId + ") order by rp.RetailerPrice " + sort;
            try
            {
                StoredProcedure sp = new StoredProcedure("");
                sp.Command.CommandSql = sql;
                sp.Command.CommandTimeout = 0;
                sp.Command.CommandType = CommandType.Text;
                IDataReader dr = sp.ExecuteReader();
                while (dr.Read())
                {
                    decimal.TryParse(dr["RetailerPrice"].ToString(), out price);
                    int.TryParse(dr["RetailerProductId"].ToString(), out rpid);
                    int.TryParse(dr["ProductId"].ToString(), out pid);
                }
                dr.Close();
            }
            catch (Exception ex) { Write("Get RetailerPrice error sql: " + sql + ex.Message + ex.StackTrace); }
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
