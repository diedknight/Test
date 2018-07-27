using PriceMeDBA;
using RetailerProductsIndexController;
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
            //try
            //{
            //    IndexController.Load(1);
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //    Console.WriteLine(ex.StackTrace);
            //}

            //var list = Term2.GetScore("GeoVision GV-SD010-18X Indoor IP Speed Dome - 1/4\" Exview CCD, 18x Optical Zoom, 12x Digital Zoom, H.264, MPEG4, MJPEG, 2 Way Audio, RJ45 Connector - W Price_1575.75");

            //return;

            if (Config.CreateModel)
            {

                Data.EachInput2((rpName, pId) =>
                {
                    Term2.Add(rpName, pId);

                    //if (Term.WordCount >= 300000) Term.Save();
                });

                Term2.Save();
            }

            var bwList = Entity.BlackWhiteList.GetList();

            IndexController.Load();
            IndexController.LoopIndex(rp =>
            {

                if (!Config.Categories.Exists(item => item.CategoryID == Convert.ToInt32(rp.CategoryID)))
                {
                    return;
                }

                if (Convert.ToBoolean(rp.IsMerge))
                {
                    return;
                }

                string retailerProductName = rp.RetailerProductName;
                int productId = Convert.ToInt32(rp.ProductID);
                decimal price = Convert.ToDecimal(rp.RetailerPrice);
                string productModel = GetProductModel(retailerProductName, Convert.ToInt32(rp.CategoryID));

                var scoreList = Term2.GetScore(retailerProductName, productId, price, 100);
                var tempScoreList = scoreList;

                //minus 0.2 scores
                if (productModel != "")
                {
                    scoreList.ForEach(scoreItem =>
                    {
                        if (!scoreItem.ProductName.Split(' ').ToList().Exists(str => str.ToLower() == productModel.ToLower()))
                        {
                            scoreItem.Score -= 0.2;
                        }
                    });
                }


                //black white list
                bwList.ForEach(bw =>
                {
                    if (bw.BlackList.Exists(b => b == Convert.ToInt32(rp.CategoryID)))
                    {
                        tempScoreList = tempScoreList.Where(s => s.CId != bw.Target).ToList();
                    }

                    if (bw.WhiteList.Exists(w => w == Convert.ToInt32(rp.CategoryID)))
                    {
                        tempScoreList = tempScoreList.Where(s => s.CId == bw.Target).ToList();
                    }
                });

                var scoreInfo = tempScoreList.OrderByDescending(t => t.Score).FirstOrDefault();
                if (scoreInfo == null)
                {
                    return;
                }

                scoreList = new List<WordScore>() { scoreInfo };


                scoreList.ForEach(item =>
                {
                    if (item.Score < Config.Score)
                    {
                        return;
                    }

                    var avgPrice = GetproductAvgPrice(item.PId);
                    double rate = Math.Abs(Convert.ToDouble(price) - avgPrice) / Convert.ToDouble(price);

                    if (rate > Config.PriceRate)
                    {
                        return;
                    }

                    if (item.ManId == -1 || string.IsNullOrEmpty(item.Img))
                    {
                        return;
                    }

                    if (rp.ManufacturerID != item.ManId.ToString())
                    {
                        return;
                    }





                    SimilarityMatchReport report = SimilarityMatchReport.SingleOrDefault(p => p.PID == productId && p.ToPID == item.PId);
                    if (report == null) report = new SimilarityMatchReport();

                    //var res_p = CSK_Store_Product.SingleOrDefault(obj => obj.ProductID == item.PId);
                    //string res_productName = res_p == null ? "" : res_p.ProductName;

                    //var sam_p = CSK_Store_Product.SingleOrDefault(obj => obj.ProductID == productId);
                    //int sam_cid = sam_p == null ? 0 : (sam_p.CategoryID ?? 0);

                    report.PID = productId;
                    report.PName = retailerProductName;
                    report.Price = price;

                    report.CID = item.CId;
                    report.ToPID = item.PId;
                    report.ToPName = item.ProductName;
                    report.Score = item.Score;

                    report.Status = true;
                    report.Createdon = DateTime.Now;
                    report.CreatedBy = "analysis_tool";

                    report.Save();

                });
            });


            //string cIds = string.Join(",", Config.Categories.Select(item => item.CategoryID).ToList());
            //string sql = " select ProductId,RetailerProductName,RetailerPrice ";
            //sql += " from CSK_Store_RetailerProduct";
            //sql += " where RetailerProductStatus = 1";
            //sql += " and IsDeleted = 0";
            //sql += " and RetailerId in (select RetailerId from CSK_Store_Retailer where RetailerStatus = 1 and retailercountry <> 56) ";
            //sql += " and ProductId in (select productid from CSK_Store_Product where IsMerge = 0 and CategoryID in (" + cIds + "))";


            //StoredProcedure sp = new StoredProcedure("");
            //sp.Command.CommandTimeout = 0;
            //sp.Command.CommandType = CommandType.Text;

            //sp.Command.CommandSql = sql;
            //IDataReader dr = sp.ExecuteReader();
            //while (dr.Read())
            //{
            //    string retailerProductName = dr["RetailerProductName"].ToString();
            //    int productId = Convert.ToInt32(dr["ProductId"]);
            //    decimal price = Convert.ToDecimal(dr["RetailerPrice"]);

            //    Term2.GetScore(retailerProductName, productId, price, 1).ForEach(item =>
            //    {
            //        if (item.Score < Config.Score)
            //        {
            //            return;
            //        }

            //        var avgPrice = GetproductAvgPrice(item.PId);
            //        double rate = Math.Abs(Convert.ToDouble(price) - avgPrice) / Convert.ToDouble(price);

            //        if (rate > Config.PriceRate)
            //        {
            //            return;
            //        }



            //        SimilarityMatchReport report = SimilarityMatchReport.SingleOrDefault(p => p.PID == productId && p.ToPID == item.PId);
            //        if (report == null) report = new SimilarityMatchReport();


            //        var res_p = CSK_Store_Product.SingleOrDefault(obj => obj.ProductID == item.PId);
            //        string res_productName = res_p == null ? "" : res_p.ProductName;

            //        var sam_p = CSK_Store_Product.SingleOrDefault(obj => obj.ProductID == productId);
            //        int sam_cid = sam_p == null ? 0 : (sam_p.CategoryID ?? 0);

            //        report.PID = productId;
            //        report.PName = retailerProductName;
            //        report.Price = price;

            //        report.CID = res_p.CategoryID ?? 0;
            //        report.ToPID = item.PId;
            //        report.ToPName = res_productName;
            //        report.Score = item.Score;

            //        report.Status = true;
            //        report.Createdon = DateTime.Now;
            //        report.CreatedBy = "analysis_tool";

            //        report.Save();

            //        //TempDataClassificationResult temp = new TempDataClassificationResult();


            //        //temp.Sample_ProductId = productId;
            //        //temp.Sample_RetailerProductName = retailerProductName;
            //        //temp.Sample_Price = price;
            //        //temp.Sample_CategoryId = sam_cid;

            //        //temp.Result_ProductId = item.PId;
            //        //temp.Result_ProductName = res_productName;
            //        //temp.Score = item.Score;

            //        //temp.Save();

            //    });
            //}

            //dr.Close();



        }

        public static double GetproductAvgPrice(int pid)
        {
            string sql = "select isnull(avg(RetailerPrice*1.0),0) from csk_store_retailerproduct where ProductId=" + pid;

            StoredProcedure sp = new StoredProcedure("");
            sp.Command.CommandTimeout = 0;
            sp.Command.CommandType = CommandType.Text;

            sp.Command.CommandSql = sql;
            var price = sp.ExecuteScalar<double>();

            return price;
        }

        public static string GetProductModel(string productName, int cId)
        {
            string productModel = "";
            List<string> strList = productName.Split(',').ToList();
            var setting = CSK_Store_AutomaticMergingOptionSetting.SingleOrDefault(item => item.Categoryid == cId);
            if (setting == null)
            {
                setting = new CSK_Store_AutomaticMergingOptionSetting();
                setting.FirstCharacterIsLetter = Config.FirstCharacterIsLetter;
                setting.IncludeCharacterAndLetter = Config.IncludeCharacterAndLetter;
                setting.LengthModel = Config.LengthModel;
            }

            foreach (var str in strList)
            {
                if (string.IsNullOrEmpty(str)) continue;

                bool result = true;
                int letterCount = 0;
                int digitCount = 0;

                foreach (char c in str)
                {
                    if (char.IsLetter(c)) letterCount++;
                    if (char.IsDigit(c)) digitCount++;
                }

                if ((setting.FirstCharacterIsLetter ?? false))
                {
                    result = result && char.IsLetter(str.First());
                }
                else
                {
                    result = result && char.IsLetterOrDigit(str.First());
                }

                if ((setting.IncludeCharacterAndLetter ?? false))
                {
                    result = result && (letterCount != 0 && digitCount != 0 && letterCount + digitCount == str.Length);
                }
                else
                {
                    result = result && (letterCount + digitCount == str.Length);
                }

                result = result && str.Length >= (setting.LengthModel ?? 0);


                if (result == true)
                {
                    productModel = str;
                    break;
                }
            }


            return productModel;
        }


    }
}
