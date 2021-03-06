﻿using ExpertReviewIndex.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace ExpertReviewIndex.SqlServerController
{
    public static class DBController
    {
        public static void GetExpertReviewData(out Dictionary<int, ExpertReview> _expertReviewDic, out List<int> erPidList)
        {
            _expertReviewDic = new Dictionary<int, ExpertReview>();
            erPidList = new List<int>();

            string sql = string.Format("Select e.ProductID, e.AverageRating, e.Votes, e.VotesHasScore, e.UserVotes, e.UserAverageRating From PriceMe_ExpertAverageRatingTF e "
                                     + "Where e.ProductID in (Select ProductID from CSK_Store_RetailerProduct where RetailerId in (select RetailerId from CSK_Store_Retailer "
                                     + "where RetailerCountry = @CountryID))");

            List<System.Data.SqlClient.SqlParameter> listPars = new List<System.Data.SqlClient.SqlParameter>();
            System.Data.SqlClient.SqlParameter par = new System.Data.SqlClient.SqlParameter();
            par.DbType = System.Data.DbType.Int32;
            par.ParameterName = "@CountryID";
            par.Value = SiteConfig.AppSettings("CountryID");
            listPars.Add(par);

            using (SqlConnection conn = new SqlConnection(SiteConfig.ConnectionStrings("conn_sqlserver")))
            {
                conn.Open();
                using (SqlCommand comm = new SqlCommand(sql, conn))
                {
                    foreach (SqlParameter spar in listPars)
                        comm.Parameters.Add(spar);

                    using (IDataReader idr = comm.ExecuteReader())
                    {
                        while (idr.Read())
                        {
                            ExpertReview er = new ExpertReview();
                            er.ProductID = int.Parse(idr["ProductID"].ToString());
                            er.PriceMeScore = double.Parse(idr["AverageRating"].ToString());
                            er.Votes = int.Parse(idr["Votes"].ToString());
                            er.VotesHasScore = int.Parse(idr["VotesHasScore"].ToString());
                            er.UserVotes = int.Parse(idr["UserVotes"].ToString());
                            er.UserAverageRating = float.Parse(idr["UserAverageRating"].ToString());

                            if (!_expertReviewDic.ContainsKey(er.ProductID))
                                _expertReviewDic.Add(er.ProductID, er);

                            if (!erPidList.Contains(er.ProductID))
                                erPidList.Add(er.ProductID);
                        }
                    }
                }
            }
        }

        public static void GetFeatureScore(out Dictionary<int, string> fsDic)
        {
            fsDic = new Dictionary<int, string>();
            string sql = "Select * From CSK_Store_ExpertReviewFeatureScore";

            using (SqlConnection conn = new SqlConnection(SiteConfig.ConnectionStrings("conn_sqlserver")))
            {
                conn.Open();
                using (SqlCommand comm = new SqlCommand(sql, conn))
                {
                    using (IDataReader dr = comm.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            int pid = 0;
                            int.TryParse(dr["ProductID"].ToString(), out pid);
                            string temp = string.Empty;
                            decimal Ease_of_use = 0;
                            decimal.TryParse(dr["Ease_of_use"].ToString(), out Ease_of_use);
                            if (Ease_of_use > 0)
                            {
                                Ease_of_use = decimal.Round((Ease_of_use / 2), 1);
                                temp += "Usability:" + Ease_of_use + ";";
                            }
                            decimal Value_for_money = 0;
                            decimal.TryParse(dr["Value_for_money"].ToString(), out Value_for_money);
                            if (Value_for_money > 0)
                            {
                                Value_for_money = decimal.Round((Value_for_money / 2), 1);
                                temp += "Value_for_money:" + Value_for_money + ";";
                            }
                            decimal Features = 0;
                            decimal.TryParse(dr["Features"].ToString(), out Features);
                            if (Features > 0)
                            {
                                Features = decimal.Round((Features / 2), 1);
                                temp += "Features:" + Features + ";";
                            }
                            decimal Design = 0;
                            decimal.TryParse(dr["Design"].ToString(), out Design);
                            if (Design > 0)
                            {
                                Design = decimal.Round((Design / 2), 1);
                                temp += "Product design:" + Design + ";";
                            }
                            decimal Sound_quality = 0;
                            decimal.TryParse(dr["Sound_quality"].ToString(), out Sound_quality);
                            if (Sound_quality > 0)
                            {
                                Sound_quality = decimal.Round((Sound_quality / 2), 1);
                                temp += "Sound:" + Sound_quality + ";";
                            }
                            decimal Picture_quality = 0;
                            decimal.TryParse(dr["Picture_quality"].ToString(), out Picture_quality);
                            if (Picture_quality > 0)
                            {
                                Picture_quality = decimal.Round((Picture_quality / 2), 1);
                                temp += "Image_quality:" + Picture_quality + ";";
                            }
                            decimal Durability = 0;
                            decimal.TryParse(dr["Durability"].ToString(), out Durability);
                            if (Durability > 0)
                            {
                                Durability = decimal.Round((Durability / 2), 1);
                                temp += "Durability:" + Durability + ";";
                            }
                            decimal Overall_quality = 0;
                            decimal.TryParse(dr["Overall_quality"].ToString(), out Overall_quality);
                            if (Overall_quality > 0)
                            {
                                Overall_quality = decimal.Round((Overall_quality / 2), 1);
                                temp += "Quality:" + Overall_quality + ";";
                            }
                            decimal Performance = 0;
                            decimal.TryParse(dr["Performance"].ToString(), out Performance);
                            if (Performance > 0)
                            {
                                Performance = decimal.Round((Performance / 2), 1);
                                temp += "Performance:" + Performance + ";";
                            }
                            decimal Reliability = 0;
                            decimal.TryParse(dr["Reliability"].ToString(), out Reliability);
                            if (Reliability > 0)
                            {
                                Reliability = decimal.Round((Reliability / 2), 1);
                                temp += "Reliability:" + Reliability + ";";
                            }

                            if (!string.IsNullOrEmpty(temp))
                            {
                                temp = temp.Substring(0, temp.Length - 1).Replace("_", " ");
                                fsDic.Add(pid, temp);
                            }
                        }
                    }
                }
            }
        }

        public static List<ExpertReview> GetAllExpertReviewTF()
        {
            List<ExpertReview> datas = new List<ExpertReview>();

            string sql = "Select e.ProductID, e.Title, e.Description, e.Pros, e.Cons, e.Verdict, e.IsExpertReview, e.PriceMeScore, e.SourceID, e.ReviewURL, CONVERT(varchar(12) , e.ReviewDate, 23) as ReviewDate, p.CategoryID, p.ManufacturerID,e.DisplayLinkStatus "
                        + "from CSK_Store_ExpertReviewAU e inner join CSK_Store_Product p on e.ProductID = p.ProductID "
                        + "Where p.ProductID in (Select ProductID From CSK_Store_RetailerProduct "
                        + "Where RetailerId in (Select RetailerId From CSK_Store_Retailer Where RetailerCountry = " + SiteConfig.AppSettings("CountryID") + ")) order by e.ReviewDate desc ";

            using (SqlConnection conn = new SqlConnection(SiteConfig.ConnectionStrings("conn_sqlserver")))
            {
                conn.Open();
                using (SqlCommand comm = new SqlCommand(sql, conn))
                {
                    using (IDataReader dr = comm.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            ExpertReview er = new ExpertReview();
                            er.ProductID = int.Parse(dr["ProductID"].ToString());
                            er.CategoryID = int.Parse(dr["CategoryID"].ToString());
                            er.PriceMeScore = double.Parse(dr["PriceMeScore"].ToString());
                            er.Description = dr["Description"].ToString();
                            er.Title = dr["Title"].ToString();
                            er.Pros = dr["Pros"].ToString();
                            er.Cons = dr["Cons"].ToString();
                            er.Verdict = dr["Verdict"].ToString();
                            er.SourceID = dr["SourceID"].ToString();
                            er.ReviewURL = dr["ReviewURL"].ToString();
                            er.ReviewDate = dr["ReviewDate"].ToString();
                            er.ManufacturerID = int.Parse(dr["ManufacturerID"].ToString());
                            er.IsExpertReview = bool.Parse(dr["IsExpertReview"].ToString());

                            if (er.SourceID == "156")
                            {
                                if (!string.IsNullOrEmpty(er.Pros))
                                {
                                    string stringPros = string.Empty;
                                    string[] temps = er.Pros.Split(',');
                                    for (int k = (temps.Length - 1); k >= 0; k--)
                                    {
                                        stringPros += "<div class=\"pc\"><span class=\"glyphicon glyphicon-plus colorPros\"></span> " + temps[k].Trim() + "</div>";
                                    }
                                    er.Pros = stringPros;
                                }

                                if (!string.IsNullOrEmpty(er.Cons))
                                {
                                    string stringCons = string.Empty;
                                    string[] temps = er.Cons.Split(',');
                                    for (int k = (temps.Length - 1); k >= 0; k--)
                                    {
                                        stringCons += "<div class=\"pc\"><span class=\"glyphicon glyphicon-minus colorCros\"></span> " + temps[k].Trim() + "</div>";
                                    }
                                    er.Cons = stringCons;
                                }
                            }

                            datas.Add(er);
                        }
                    }
                }
            }

            return datas;
        }

        public static List<ProductVideo> GetProductVideo()
        {
            List<ProductVideo> datas = new List<ProductVideo>();

            string sql = "Select e.ProductID, e.Url,e.Thumbnail from CSK_Store_ProductVideo e inner join CSK_Store_Product p "
                        + "on e.ProductID = p.ProductID Where Url is not null and p.ProductID in (Select ProductID From CSK_Store_RetailerProduct "
                        + "Where RetailerProductStatus = 1 And RetailerId in (Select RetailerId From CSK_Store_Retailer Where RetailerStatus = 1))"
                        + "order by e.CreatedOn desc";

            using (SqlConnection conn = new SqlConnection(SiteConfig.ConnectionStrings("conn_sqlserver")))
            {
                conn.Open();
                using (SqlCommand comm = new SqlCommand(sql, conn))
                {
                    using (IDataReader dr = comm.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            int pid = 0;
                            int.TryParse(dr["ProductID"].ToString(), out pid);

                            ProductVideo video = new ProductVideo();
                            video.ProductID = pid;
                            video.Url = dr["Url"].ToString();
                            video.Thumbnail = dr["Thumbnail"].ToString();

                            datas.Add(video);
                        }
                    }
                }
            }

            return datas;
        }

        public static bool IsExpertReviewNull(int pid)
        {
            bool isEr = false;
            string sql = "Select * From  CSK_Store_ExpertReviewAU Where ProductID = " + pid;
            using (SqlConnection conn = new SqlConnection(SiteConfig.ConnectionStrings("conn_sqlserver")))
            {
                conn.Open();
                using (SqlCommand comm = new SqlCommand(sql, conn))
                {
                    using (IDataReader idr = comm.ExecuteReader())
                    {
                        while (idr.Read())
                        {
                            if (string.IsNullOrEmpty(idr["Title"].ToString()) && string.IsNullOrEmpty(idr["Pros"].ToString()) && string.IsNullOrEmpty(idr["Cons"].ToString()))
                            {
                                isEr = true;
                                break;
                            }
                        }
                    }
                }
            }

            return isEr;
        }

        public static bool IsExpertReviewBySource(int pid, int sourceid)
        {
            bool isEr = false;
            string sql = "Select * From  CSK_Store_ExpertReviewAU Where ProductID = " + pid + " And SourceID = " + sourceid;
            using (SqlConnection conn = new SqlConnection(SiteConfig.ConnectionStrings("conn_sqlserver")))
            {
                conn.Open();
                using (SqlCommand comm = new SqlCommand(sql, conn))
                {
                    using (IDataReader idr = comm.ExecuteReader())
                    {
                        while (idr.Read())
                        {
                            isEr = true;
                            break;
                        }
                    }
                }
            }

            return isEr;
        }
    }
}
