using PriceMeDBA;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using SubSonic.Schema;
using PriceMeCommon.Data;

/// <summary>
/// Summary description for RetailerReviewController
/// </summary>
public class RetailerReviewController
{
    static string connectionStr = System.Configuration.ConfigurationManager.ConnectionStrings["CommerceTemplate_Common"].ConnectionString;
    public static Dictionary<int, int> retailerReviewEmail;

	public RetailerReviewController()
	{
        
	}

    public static Dictionary<int, int> GetRetailerReviewEamil()
    {
        if (retailerReviewEmail == null)
            BindRetailerReviewEmail();

        return retailerReviewEmail;
    }

    private static void BindRetailerReviewEmail()
    {
        retailerReviewEmail = new Dictionary<int, int>();
        string sql = "Select RetailerId, ReviewRating From CSK_Store_PPCMember";
        using (SqlConnection conn = new SqlConnection(connectionStr))
        using (SqlCommand cmd = new SqlCommand(sql, conn))
        {
            cmd.CommandType = CommandType.Text;
            cmd.CommandTimeout = 0;

            conn.Open();
            IDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                int rid = 0, rating = 0;
                int.TryParse(dr["RetailerId"].ToString(), out rid);
                int.TryParse(dr["ReviewRating"].ToString(), out rating);
                if (!retailerReviewEmail.ContainsKey(rid))
                    retailerReviewEmail.Add(rid, rating);
            }
            dr.Close();
            conn.Close();
        }
    }

    public static List<int> RetailerReviewChallenged(int rid)
    {
        List<int> listReviews = new List<int>();

        string sql = "select ReviewID from dbo.Merchant_Reviews where RetailerId = " + rid + " And ReviewStatus in (2, 3)";
        using (SqlConnection conn = new SqlConnection(connectionStr))
        using (SqlCommand cmd = new SqlCommand(sql, conn))
        {
            cmd.CommandType = CommandType.Text;
            cmd.CommandTimeout = 0;

            conn.Open();
            IDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                int id = 0;
                int.TryParse(dr["ReviewID"].ToString(), out id);
                listReviews.Add(id);
            }
            dr.Close();
            conn.Close();
        }

        return listReviews;
    }

    public static List<int> RetailerReviewPublic(int rid, out Dictionary<int, int> dicMReviews)
    {
        List<int> listReviews = new List<int>();
        dicMReviews = new Dictionary<int, int>();

        string sql = "select MerchantReviewID, ReviewID from dbo.Merchant_Reviews where RetailerId = " + rid + " And ReviewStatus in (4, 5)";
        using (SqlConnection conn = new SqlConnection(connectionStr))
        using (SqlCommand cmd = new SqlCommand(sql, conn))
        {
            cmd.CommandType = CommandType.Text;
            cmd.CommandTimeout = 0;

            conn.Open();
            IDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                int id = 0, mid = 0;
                int.TryParse(dr["MerchantReviewID"].ToString(), out mid);
                int.TryParse(dr["ReviewID"].ToString(), out id);
                listReviews.Add(id);
                if (!dicMReviews.ContainsKey(mid))
                    dicMReviews.Add(mid, id);
            }
            dr.Close();
            conn.Close();
        }

        return listReviews;
    }

    public static List<ReviewCommunicationData> GetReviewCommunication(Dictionary<int, int> dicMReviews)
    {
        List<ReviewCommunicationData> datas = new List<ReviewCommunicationData>();

        if (dicMReviews.Count == 0)
            return datas;

        string stringmid = string.Empty;
        foreach (KeyValuePair<int, int> pair in dicMReviews)
        {
            stringmid += pair.Key + ",";
        }
        if (!string.IsNullOrEmpty(stringmid))
            stringmid = stringmid.Substring(0, stringmid.LastIndexOf(','));

        string sql = "Select * From dbo.CSK_Store_ReviewCommunicationPublic where Sender = 1 And merchantreviewid in (" + stringmid + ")";
        using (SqlConnection conn = new SqlConnection(connectionStr))
        using (SqlCommand cmd = new SqlCommand(sql, conn))
        {
            cmd.CommandType = CommandType.Text;
            cmd.CommandTimeout = 0;

            conn.Open();
            IDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                int id = 0, mid = 0;
                int.TryParse(dr["ID"].ToString(), out id);
                int.TryParse(dr["merchantreviewid"].ToString(), out mid);
                bool Sender = false;
                bool.TryParse(dr["Sender"].ToString(), out Sender);
                string Content = dr["Content"].ToString();
                DateTime Datatime = DateTime.Now;
                DateTime.TryParse(dr["Datatime"].ToString(), out Datatime);
                string Attachment = dr["Attachment"].ToString();

                int reviewid = 0;
                if (dicMReviews.ContainsKey(mid))
                    reviewid = dicMReviews[mid];

                ReviewCommunicationData data = new ReviewCommunicationData();
                data.Id = id;
                data.ReviewId = reviewid;
                data.Sender = Sender;
                data.Content = Content;
                data.CreatedOn = Datatime;
                data.Attachment = Attachment;
                datas.Add(data);
            }
            dr.Close();
            conn.Close();
        }

        return datas;
    }

    public static List<ReviewCommunicationData> GetReviewCommunication(List<ReviewCommunicationData> datas, int reviewid)
    {
        List<ReviewCommunicationData> coms = datas.Where(c => c.ReviewId == reviewid).OrderByDescending(c => c.CreatedOn).ToList();
        
        return coms;
    }

    public static void RetailerReviewInsert(CSK_Store_RetailerReview rr, int reviewstatus, int basicReview, string email)
    {
        try
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

            using (SqlConnection conn = new SqlConnection(connectionStr))
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.CommandType = CommandType.Text;
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
                conn.Close();
            }
        }
        catch (Exception ex)
        {
            LogWriter.FileLogWriter.WriteLine(PriceMeCommon.ConfigAppString.LogPath, "Retailer review error: " + ex.Message + ex.StackTrace);
        }
    }

    public static void SaveRetailerVotesSum(int rid, int rating)
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

    public static void GetRetailerEmail(int rid, out string email, out string firstname)
    {
        email = string.Empty;
        firstname = string.Empty;

        string sql = "Select s.Email, i.FirstName From aspnet_Membership s inner join aspnet_MembershipInfo i On s.UserId = i.UserID "
                    + "Where s.UserId in (Select UserId From CSK_Store_PPCMember Where RetailerId = " + rid + ")";
        using (SqlConnection conn = new SqlConnection(connectionStr))
        using (SqlCommand cmd = new SqlCommand(sql, conn))
        {
            cmd.CommandType = CommandType.Text;
            cmd.CommandTimeout = 0;

            conn.Open();
            IDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                email = dr["Email"].ToString();
                firstname = dr["FirstName"].ToString();
            }
            dr.Close();
            conn.Close();
        }
    }
}