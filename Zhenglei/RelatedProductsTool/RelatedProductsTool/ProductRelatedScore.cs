using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RelatedProductsTool
{
    public class ProductRelatedScore
    {
        public int MainProductId { get; set; }
        public int RelatedProductId { get; set; }
        public float SuccessorScore { get; set; }
        public float BrandScore { get; set; }
        public float BestSellerScore { get; set; }
        public float PPCCountScore { get; set; }
        public float PriceScore { get; set; }
        public int CategoryId { get; set; }
        public float TotalScore
        {
            get
            {
                return SuccessorScore + BrandScore + BestSellerScore + PPCCountScore + PriceScore;
            }
        }

        public override string ToString()
        {
            string toStrFormat = "MainProductId : {0}, RelatedProductId: {1}, SuccessorScore: {2}, BrandScore: {3}, BestSellerScore: {4}, PPCCountScore: {5}, PriceScore: {6}";
            return string.Format(toStrFormat, MainProductId, RelatedProductId, SuccessorScore, BrandScore, BestSellerScore, PPCCountScore, PriceScore);
        }

        public static string ToCSVHeaderString()
        {
            return "\"MainProductId\",\"RelatedProductId\",\"SuccessorScore\",\"BrandScore\",\"BestSellerScore\",\"PPCCountScore\",\"PriceScore\",\"TotalScore\"";
        }

        public string ToCSVString()
        {
            string toStrFormat = "{0},{1},{2},{3},{4},{5},{6},{7}";
            return string.Format(toStrFormat, MainProductId, RelatedProductId, SuccessorScore, BrandScore, BestSellerScore, PPCCountScore, PriceScore, TotalScore);
        }



        //([ProductId]
        //,[CountryId]
        //,[RelatedProductId]
        //,[Score]
        //,[CreatedOn]
        //,[ModifiedOn])
        public string ToSqlString(DateTime dt, int countryId)
        {
            string format = "select {0}, {1}, {2}, {3}, '{4}', '{5}', {6}";
            return string.Format(format, MainProductId, countryId, RelatedProductId, TotalScore.ToString("0.0"), dt.ToString("yyyy-MM-dd HH:mm:ss"), dt.ToString("yyyy-MM-dd HH:mm:ss"), CategoryId);
        }
    }
}