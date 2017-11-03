using PriceMeDBA;
using Service.Infrastructure.Log;
using SubSonic.Schema;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAnalyze
{
    class Program
    {
        static void Main(string[] args)
        {
            //var list = Term2.GetScore("GeoVision GV-SD010-18X Indoor IP Speed Dome - 1/4\" Exview CCD, 18x Optical Zoom, 12x Digital Zoom, H.264, MPEG4, MJPEG, 2 Way Audio, RJ45 Connector - W Price_1575.75");
            
            //return;

            //Data.EachInput2((rpName, pId) =>
            //{
            //    Term2.Add(rpName, pId);

            //    //if (Term.WordCount >= 300000) Term.Save();
            //});

            //Term2.Save();

            string sql = "select ProductId,RetailerProductName,RetailerPrice from CSK_Store_RetailerProduct where RetailerProductStatus=1 and IsDeleted=0 and RetailerId in (select RetailerId from CSK_Store_Retailer where RetailerStatus=1) and ProductId in (select productid from CSK_Store_Product where IsMerge=0 and CategoryID in (select Categoryid from CSK_Store_HelpTopCategory))";            

            StoredProcedure sp = new StoredProcedure("");
            sp.Command.CommandTimeout = 0;
            sp.Command.CommandType = CommandType.Text;

            sp.Command.CommandSql = sql;
            IDataReader dr = sp.ExecuteReader();
            while (dr.Read())
            {
                string retailerProductName = dr["RetailerProductName"].ToString();
                int productId = Convert.ToInt32(dr["ProductId"]);
                decimal price = Convert.ToDecimal(dr["RetailerPrice"]);

                Term2.GetScore(retailerProductName, productId, price, 1).ForEach(item =>
                {
                    var res_p = CSK_Store_Product.SingleOrDefault(obj => obj.ProductID == item.PId);
                    string res_productName = res_p == null ? "" : res_p.ProductName;

                    var sam_p = CSK_Store_Product.SingleOrDefault(obj => obj.ProductID == productId);
                    int sam_cid = sam_p == null ? 0 : (sam_p.CategoryID ?? 0);

                    TempDataClassificationResult temp = new TempDataClassificationResult();

                    temp.Sample_ProductId = productId;
                    temp.Sample_RetailerProductName = retailerProductName;
                    temp.Sample_Price = price;
                    temp.Sample_CategoryId = sam_cid;

                    temp.Result_ProductId = item.PId;
                    temp.Result_ProductName = res_productName;
                    temp.Score = item.Score;

                    temp.Save();

                });
            }

            dr.Close();

        }
    }
}
