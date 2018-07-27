using SubSonic.Schema;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace DataAnalyze
{
    public class Data
    {
        private static List<Tuple<int, int, string, int, string>> GetIsMergeProducts()
        {
            List<Tuple<int, int, string, int, string>> list = new List<Tuple<int, int, string, int, string>>();

            string cids = string.Join(",", Config.Categories.Select(item => item.CategoryID));

            string sql = "select ProductId,ManufacturerID,DefaultImage,CategoryID,ProductName from CSK_Store_Product where IsMerge=1 and CategoryID in (" + cids + ")";
            StoredProcedure sp = new StoredProcedure("");
            sp.Command.CommandSql = sql;
            sp.Command.CommandTimeout = 0;
            sp.Command.CommandType = CommandType.Text;
            IDataReader dr = sp.ExecuteReader();

            while (dr.Read())
            {
                int pId = dr["ProductId"] == DBNull.Value ? 0 : Convert.ToInt32(dr["ProductId"]);
                int manId = dr["ManufacturerID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["ManufacturerID"]);
                string img = dr["DefaultImage"] == DBNull.Value ? "" : dr["DefaultImage"].ToString();
                int CategoryID = dr["CategoryID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["CategoryID"]);
                string ProductName = dr["ProductName"] == DBNull.Value ? "" : dr["ProductName"].ToString();

                if (pId == 0) continue;

                list.Add(new Tuple<int, int, string, int, string>(pId, manId, img, CategoryID, ProductName));
            }

            dr.Close();

            //DataSet ds = sp.ExecuteDataSet();
            //foreach (DataRow row in ds.Tables[0].Rows)
            //{
            //    int pId = row["ProductId"] == DBNull.Value ? 0 : Convert.ToInt32(row["ProductId"]);
            //    if (pId == 0) continue;

            //    list.Add(pId);
            //}

            return list;
        }

        private static HashSet<int> GetNZRetailers()
        {
            HashSet<int> set = new HashSet<int>();

            string sql = "select RetailerId from CSK_Store_Retailer where RetailerCountry=3";
            StoredProcedure sp = new StoredProcedure("");
            sp.Command.CommandSql = sql;
            sp.Command.CommandTimeout = 0;
            sp.Command.CommandType = CommandType.Text;
            IDataReader dr = sp.ExecuteReader();

            while (dr.Read())
            {
                int pId = dr["RetailerId"] == DBNull.Value ? 0 : Convert.ToInt32(dr["RetailerId"]);
                if (pId == 0) continue;

                set.Add(pId);
            }

            dr.Close();

            return set;
        }

        //public static void EachInput(Action<string, int> action)
        //{
        //    var PIdList = GetIsMergeProducts();
        //    //var rIdSet = GetNZRetailers();

        //    StoredProcedure sp = new StoredProcedure("");
        //    sp.Command.CommandTimeout = 0;
        //    sp.Command.CommandType = CommandType.Text;

        //    PIdList.ForEach(pId =>
        //    {
        //        sp.Command.CommandSql = "select RetailerProductName,RetailerPrice from CSK_Store_RetailerProduct where ProductId=" + pId;
        //        IDataReader dr = sp.ExecuteReader();

        //        while (dr.Read())
        //        {
        //            string retailerProductName = dr["RetailerProductName"].ToString();
        //            decimal price = dr["RetailerPrice"] == DBNull.Value ? 0 : Convert.ToDecimal(dr["RetailerPrice"]);
        //            string val = string.Format("{0} Price_{1}", retailerProductName, price.ToString("0.00"));

        //            action(val, pId);
        //        }

        //        dr.Close();
        //    });


        //    //int pageIndex = 1;
        //    //int pageSize = 50000;
        //    //while (true)
        //    //{
        //    //    bool isOver = true;
        //    //    string sql = " select RetailerProductName,ProductId,RetailerPrice from";
        //    //    sql += " (";
        //    //    sql += " select ROW_NUMBER() over(ORDER BY RetailerProductId asc) as xbai_num, a.RetailerProductName, a.ProductId, a.RetailerPrice from CSK_Store_RetailerProduct as a";
        //    //    sql += " ) as x";
        //    //    sql += " where xbai_num BETWEEN " + ((pageIndex - 1) * pageSize + 1) + " AND " + pageIndex * pageSize;

        //    //    sp.Command.CommandSql = sql;
        //    //    IDataReader dr = sp.ExecuteReader();

        //    //    while (dr.Read())
        //    //    {
        //    //        isOver = false;

        //    //        int pId = dr["ProductId"] == DBNull.Value ? 0 : Convert.ToInt32(dr["ProductId"]);
        //    //        if (pId == 0) continue;

        //    //        string retailerProductName = dr["RetailerProductName"].ToString();
        //    //        decimal price = dr["RetailerPrice"] == DBNull.Value ? 0 : Convert.ToDecimal(dr["RetailerPrice"]);
        //    //        string val = string.Format("{0} Price_{1}", retailerProductName, price.ToString("0.00"));

        //    //        action(retailerProductName, pId);
        //    //    }

        //    //    dr.Close();

        //    //    pageIndex++;
        //    //    if (isOver) break;
        //    //}
        //}

        public static void EachInput2(Action<string, Tuple<int, int, string, int, string>> action)
        {
            if (Config.UseProduct)
            {
                var PIdList = GetIsMergeProducts();

                PIdList.ForEach(pItem =>
                {

                    string name = "";

                    string conStr = System.Configuration.ConfigurationManager.ConnectionStrings["CommerceTemplate"].ConnectionString;
                    using (var con = new SqlConnection(conStr))
                    {
                        string sql = "select ProductName from CSK_Store_Product where ProductId=" + pItem.Item1;
                        name = con.ExecuteScalar<string>(sql);
                    }

                    action(name, pItem);
                });
            }
            else
            {
                var PIdList = GetIsMergeProducts();
                //var rIdSet = GetNZRetailers();

                StoredProcedure sp = new StoredProcedure("");
                sp.Command.CommandTimeout = 0;
                sp.Command.CommandType = CommandType.Text;

                PIdList.ForEach(pId =>
                {
                    sp.Command.CommandSql = "select RetailerProductName,RetailerPrice from CSK_Store_RetailerProduct where ProductId=" + pId.Item1;
                    IDataReader dr = sp.ExecuteReader();

                    StringBuilder sb = new StringBuilder();

                    while (dr.Read())
                    {
                        string retailerProductName = dr["RetailerProductName"].ToString();
                        decimal price = dr["RetailerPrice"] == DBNull.Value ? 0 : Convert.ToDecimal(dr["RetailerPrice"]);
                        string val = string.Format("{0} Price_{1} ", retailerProductName, price.ToString("0.00"));

                        sb.Append(val);
                    }

                    action(sb.ToString(), pId);

                    dr.Close();
                });
            }
        }

        //public static void EachOutputRPNameCount(Action<int, int> action)
        //{
        //    int pageIndex = 1;
        //    int pageSize = 50000;

        //    StoredProcedure sp = new StoredProcedure("");
        //    sp.Command.CommandTimeout = 0;
        //    sp.Command.CommandType = CommandType.Text;

        //    while (true)
        //    {
        //        bool isOver = true;
        //        string sql = " select TotalWord,PId from";
        //        sql += " (";
        //        sql += " select ROW_NUMBER() over(ORDER BY Id asc) as xbai_num, TotalWord,PId from RPNameWordCount";
        //        sql += " ) as a";
        //        sql += " where xbai_num BETWEEN " + ((pageIndex - 1) * pageSize + 1) + " AND " + pageIndex * pageSize;

        //        sp.Command.CommandSql = sql;                
        //        IDataReader dr = sp.ExecuteReader();

        //        while (dr.Read())
        //        {
        //            isOver = false;
        //            int TotalWord = dr["TotalWord"] == DBNull.Value ? 0 : Convert.ToInt32(dr["TotalWord"]);
        //            int pId = dr["PId"] == DBNull.Value ? 0 : Convert.ToInt32(dr["PId"]);
        //            if (pId == 0) continue;

        //            action(TotalWord, pId);
        //        }
        //        dr.Close();

        //        pageIndex++;
        //        if (isOver) break;
        //    }
        //}

        //public static void EachOutputWordInfo(Action<string, double, byte[]> action)
        //{
        //    int pageIndex = 1;
        //    int pageSize = 50000;

        //    StoredProcedure sp = new StoredProcedure("");
        //    sp.Command.CommandTimeout = 0;
        //    sp.Command.CommandType = CommandType.Text;

        //    while (true)
        //    {
        //        bool isOver = true;
        //        string sql = " select Word,Weight,HitPIdCountJson from";
        //        sql += " (";
        //        sql += " select ROW_NUMBER() over(ORDER BY Id asc) as xbai_num, Word,Weight,HitPIdCountJson from RPNameWord";
        //        sql += " ) as a";
        //        sql += " where xbai_num BETWEEN " + ((pageIndex - 1) * pageSize + 1) + " AND " + pageIndex * pageSize;


        //        sp.Command.CommandSql = sql;                
        //        IDataReader dr = sp.ExecuteReader();

        //        while (dr.Read())
        //        {
        //            isOver = false;

        //            string word = dr["Word"].ToString();
        //            double weight = dr["Weight"] == DBNull.Value ? 0d : Convert.ToDouble(dr["Weight"]);
        //            byte[] bt = dr["HitPIdCountJson"] == DBNull.Value ? new byte[0] : (byte[])dr["HitPIdCountJson"];

        //            action(word, weight, bt);
        //        }
        //        dr.Close();

        //        pageIndex++;
        //        if (isOver) break;
        //    }
        //}

    }
}
