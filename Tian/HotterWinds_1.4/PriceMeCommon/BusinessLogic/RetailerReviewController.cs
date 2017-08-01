using PriceMeDBA;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceMeCommon.BusinessLogic
{
    public static class RetailerReviewController
    {
        static Dictionary<int, int> RetailerPPcRatingDic_Static;

        public static void Load()
        {
            RetailerPPcRatingDic_Static = GetRetailerPPcRatingDic();
        }

        private static Dictionary<int, int> GetRetailerPPcRatingDic()
        {
            Dictionary<int, int> dic = new Dictionary<int, int>();
            string sql = "Select RetailerId, ReviewRating From CSK_Store_PPCMember";
            using (SqlConnection conn = new SqlConnection(MultiCountryController.CommonConnectionStringSettings_Static.ConnectionString))
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 0;

                conn.Open();
                using (IDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        int rid = 0, rating = 0;
                        int.TryParse(dr["RetailerId"].ToString(), out rid);
                        int.TryParse(dr["ReviewRating"].ToString(), out rating);
                        if (!dic.ContainsKey(rid))
                            dic.Add(rid, rating);
                    }
                }
            }

            return dic;
        }

        public static int GetRetailerPPcRating(int retailerId)
        {
            if(RetailerPPcRatingDic_Static.ContainsKey(retailerId))
            {
                return RetailerPPcRatingDic_Static[retailerId];
            }

            return -1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rid"></param>
        /// <param name="rating"></param>
        /// <param name="countryId"></param>
        public static void SaveRetailerVotesSum(int rid, int rating, int countryId)
        {
            using (SubSonic.DataProviders.SharedDbConnectionScope sdbs = new SubSonic.DataProviders.SharedDbConnectionScope(MultiCountryController.GetDBProvider(countryId)))
            {
                CSK_Store_RetailerVotesSum votes = CSK_Store_RetailerVotesSum.SingleOrDefault(r => r.RetailerID == rid);
                if (votes == null)
                {
                    votes = new CSK_Store_RetailerVotesSum();
                    votes.RetailerID = rid;
                    votes.RetailerRatingSum = rating + 3;
                    votes.RetailerTotalRatingVotes = 2;
                    votes.Save();
                }
                else
                {
                    votes.RetailerTotalRatingVotes = votes.RetailerTotalRatingVotes + 1;
                    votes.RetailerRatingSum = votes.RetailerRatingSum + rating;
                    votes.Save();
                }
            }
        }

        public static void RetailerReviewInsert(CSK_Store_RetailerReview rr, int reviewstatus, int basicReview, string email)
        {
            string sql = string.Format("INSERT INTO [dbo].[Merchant_Reviews] "
                + "([ReviewID],[RetailerId],[ReviewStatus],[DeliveryRating],[ServiceRating],[EaseOfPurchaseRating],[OverallRating],"
                + "[ProductInfoRating],[Title],[Descriptive],[Email],[CreatedOn],[CreatedBy],[BasicReview])"
                + "VALUES(@ReviewID, @RetailerId, " + reviewstatus + ",@DeliveryRating,@ServiceRating,@EaseOfPurchaseRating,@OverallRating,"
                + "@ProductInfoRating,@Title,@Descriptive,@Email,@CreatedOn,@CreatedBy," + basicReview + ")");

            SqlParameter p_id = new SqlParameter("@ReviewID", SqlDbType.Int);
            p_id.Direction = ParameterDirection.Input;
            p_id.Value = rr.RetailerReviewId;
            SqlParameter p_rid = new SqlParameter("@RetailerId", SqlDbType.Int);
            p_rid.Direction = ParameterDirection.Input;
            p_rid.Value = rr.RetailerId;
            SqlParameter p_Dr = new SqlParameter("@DeliveryRating", SqlDbType.Int);
            p_Dr.Direction = ParameterDirection.Input;
            p_Dr.Value = rr.OnTimeDelivery;
            SqlParameter p_Sr = new SqlParameter("@ServiceRating", SqlDbType.Int);
            p_Sr.Direction = ParameterDirection.Input;
            p_Sr.Value = rr.CustomerCare;
            SqlParameter p_Er = new SqlParameter("@EaseOfPurchaseRating", SqlDbType.Int);
            p_Er.Direction = ParameterDirection.Input;
            p_Er.Value = rr.EasyOfOrdering;
            SqlParameter p_Or = new SqlParameter("@OverallRating", SqlDbType.Int);
            p_Or.Direction = ParameterDirection.Input;
            p_Or.Value = rr.OverallStoreRating;
            SqlParameter p_Pr = new SqlParameter("@ProductInfoRating", SqlDbType.Int);
            p_Pr.Direction = ParameterDirection.Input;
            p_Pr.Value = rr.Availability;
            SqlParameter p_Title = new SqlParameter("@Title", SqlDbType.NVarChar);
            p_Title.Direction = ParameterDirection.Input;
            p_Title.Value = rr.Title;
            SqlParameter p_Des = new SqlParameter("@Descriptive", SqlDbType.NVarChar);
            p_Des.Direction = ParameterDirection.Input;
            p_Des.Value = rr.Body;
            SqlParameter p_Email = new SqlParameter("@Email", SqlDbType.NVarChar);
            p_Email.Direction = ParameterDirection.Input;
            p_Email.Value = email;
            SqlParameter p_Con = new SqlParameter("@CreatedOn", SqlDbType.DateTime);
            p_Con.Direction = ParameterDirection.Input;
            p_Con.Value = rr.CreatedOn;
            SqlParameter p_Cby = new SqlParameter("@CreatedBy", SqlDbType.NVarChar);
            p_Cby.Direction = ParameterDirection.Input;
            p_Cby.Value = rr.CreatedBy;

            using (SqlConnection conn = new SqlConnection(MultiCountryController.CommonConnectionStringSettings_Static.ConnectionString))
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.CommandTimeout = 0;
                cmd.Parameters.Add(p_id);
                cmd.Parameters.Add(p_rid);
                cmd.Parameters.Add(p_Dr);
                cmd.Parameters.Add(p_Sr);
                cmd.Parameters.Add(p_Er);
                cmd.Parameters.Add(p_Or);
                cmd.Parameters.Add(p_Pr);
                cmd.Parameters.Add(p_Title);
                cmd.Parameters.Add(p_Des);
                cmd.Parameters.Add(p_Email);
                cmd.Parameters.Add(p_Con);
                cmd.Parameters.Add(p_Cby);

                conn.Open();
                cmd.ExecuteScalar();
            }
        }

        public static void GetRetailerEmail(int rid, out string email, out string firstname)
        {
            email = string.Empty;
            firstname = string.Empty;

            string sql = "Select s.Email, i.FirstName From aspnet_Membership s inner join aspnet_MembershipInfo i On s.UserId = i.UserID "
                        + "Where s.UserId in (Select UserId From CSK_Store_PPCMember Where RetailerId = " + rid + ")";
            using (SqlConnection conn = new SqlConnection(MultiCountryController.CommonConnectionStringSettings_Static.ConnectionString))
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 0;

                conn.Open();
                using (IDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        email = dr["Email"].ToString();
                        firstname = dr["FirstName"].ToString();
                    }
                }
            }
        }
    }
}