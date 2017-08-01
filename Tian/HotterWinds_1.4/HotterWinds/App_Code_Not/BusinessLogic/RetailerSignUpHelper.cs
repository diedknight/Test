using PriceMe;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Text;

/// <summary>
/// RetailerSignUpHelper
/// </summary>
public class RetailerSignUpHelper
{
    static string connectionStr = PriceMeCommon.BusinessLogic.MultiCountryController.CommonConnectionStringSettings_Static.ConnectionString;
    
    #region read

    /// <summary>
    /// get retailer signup info 
    /// by organisation id
    /// or email
    /// </summary>
    /// <param name="orgId"></param>
    /// <param name="email"></param>
    /// <returns></returns>
    public static int ReadRetailerLead(int orgId, string email)
    {
        #region

        int leadID = 0;
        var sql = string.Format("SELECT RetailerLeadID FROM [dbo].[CSK_Store_RetailerLeadSignUp] WHERE CapsuleOrgId = {0} OR ContactEmail = '{1}'", orgId, email);
        using (SqlConnection conn = new SqlConnection(connectionStr))
        using (SqlCommand cmd = new SqlCommand(sql, conn))
        {
            cmd.CommandType = CommandType.Text;
            cmd.CommandTimeout = 0;
            conn.Open();
            var obj = cmd.ExecuteScalar();
            leadID = int.Parse(obj.ToString());
            conn.Close();
        }

        #endregion

        return leadID;
    }

    #endregion

    #region new
    
    /// <summary>
    /// 保存 retailer signup 信息到CSK_Store_RetailerLeadSignUp
    /// </summary>
    /// <param name="retailerName"></param>
    /// <param name="webSiteURL"></param>
    /// <param name="firstName"></param>
    /// <param name="lastName"></param>
    /// <param name="emailAddress"></param>
    /// <param name="gstNumber"></param>
    /// <param name="oid"></param>
    /// <returns></returns>
    public static int NewRetailerLead(string retailerName, string webSiteURL, string firstName, string lastName, string emailAddress, string gstNumber, int oid)
    {
        int newLeadID = 0;
        try
        {
            string sql = string.Format("INSERT INTO [dbo].[CSK_Store_RetailerLeadSignUp] " +
                "([SiteCountryID],[RetailerName],[WebsiteURL],[StoreType],[ContactFirstName],[ContactLastName],[ContactEmail],[JobFunction],[GSTNumber],[CapsuleOrgId],[ValidLead])" +
                "VALUES(@ID, @RetailerName, @WebsiteURL,0,@FirstName,@LastName,@Email,0,@GST,@OrgId,@isValid);SELECT @@IDENTITY;");

            SqlParameter p_ID = new SqlParameter("@ID", SqlDbType.Int);
            p_ID.Direction = ParameterDirection.Input;
            p_ID.Value = WebConfig.CountryId;

            SqlParameter p_RetailerName = new SqlParameter("@RetailerName", SqlDbType.NVarChar);
            p_RetailerName.Direction = ParameterDirection.Input;
            p_RetailerName.Value = retailerName;

            SqlParameter p_WebsiteURL = new SqlParameter("@WebsiteURL", SqlDbType.NVarChar);
            p_WebsiteURL.Direction = ParameterDirection.Input;
            p_WebsiteURL.Value = webSiteURL;

            SqlParameter p_FirstName = new SqlParameter("@FirstName", SqlDbType.NVarChar);
            p_FirstName.Direction = ParameterDirection.Input;
            p_FirstName.Value = firstName;

            SqlParameter p_LastName = new SqlParameter("@LastName", SqlDbType.NVarChar);
            p_LastName.Direction = ParameterDirection.Input;
            p_LastName.Value = lastName;

            SqlParameter p_Email = new SqlParameter("@Email", SqlDbType.NVarChar);
            p_Email.Direction = ParameterDirection.Input;
            p_Email.Value = emailAddress;

            SqlParameter p_GST = new SqlParameter("@GST", SqlDbType.NVarChar);
            p_GST.Direction = ParameterDirection.Input;
            p_GST.Value = gstNumber;

            SqlParameter p_OrgId = new SqlParameter("@OrgId", SqlDbType.Int);
            p_OrgId.Direction = ParameterDirection.Input;
            p_OrgId.Value = oid;

            SqlParameter p_isValid = new SqlParameter("@isValid", SqlDbType.Bit);
            p_isValid.Direction = ParameterDirection.Input;
            p_isValid.Value = PriceMe.WebConfig.Environment == "prod";

            using (SqlConnection conn = new SqlConnection(connectionStr))
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 0;
                cmd.Parameters.Add(p_ID);
                cmd.Parameters.Add(p_RetailerName);
                cmd.Parameters.Add(p_WebsiteURL);
                cmd.Parameters.Add(p_FirstName);
                cmd.Parameters.Add(p_LastName);
                cmd.Parameters.Add(p_Email);
                cmd.Parameters.Add(p_GST);
                cmd.Parameters.Add(p_OrgId);
                cmd.Parameters.Add(p_isValid);

                conn.Open();
                var obj = cmd.ExecuteScalar();
                conn.Close();
                int.TryParse(obj.ToString(), out newLeadID);
            }
            if (newLeadID <= 0)
                newLeadID = ReadRetailerLead(oid, emailAddress);
            NewRetailerLeadTracking(newLeadID);
        }
        catch (Exception ex)
        {

        }
        return newLeadID;
    }

    /// <summary>
    /// 记录 Retailer SignUpTime to CSK_Store_RetailerLeadTracking
    /// </summary>
    /// <param name="newLeadID"></param>
    /// <param name="newTrackID_"></param>
    /// <param name="sql"></param>
    private static int NewRetailerLeadTracking(int newLeadID)
    {
        int newTrackID = 0;
        if (newLeadID > 0)
        {
            var sql = string.Format("INSERT INTO [dbo].[CSK_Store_RetailerLeadTracking] " +
            "([RetailerLeadId] ,[RetailerID] ,[SignUpTime]) VALUES({0} ,0 ,GETDATE());SELECT @@IDENTITY;", newLeadID);
            using (SqlConnection conn = new SqlConnection(connectionStr))
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 0;
                conn.Open();
                var obj = cmd.ExecuteScalar();
                conn.Close();
                int.TryParse(obj.ToString(), out newTrackID);
            }
        }
        return newTrackID;
    }

    #endregion

    #region update

    /// <summary>
    /// 保存 retailer signup 信息到CSK_Store_RetailerLeadSignUp
    /// </summary>
    /// <param name="retailerName"></param>
    /// <param name="webSiteURL"></param>
    /// <param name="firstName"></param>
    /// <param name="lastName"></param>
    /// <param name="emailAddress"></param>
    /// <param name="oid"></param>
    /// <param name="gstNumber"></param>
    /// <param name="feedURL"></param>
    /// <param name="jobFunction"></param>
    /// <param name="platForm"></param>
    /// <returns></returns>
    public static int UpdateRetailerLead(int lid, string retailerName, string webSiteURL, string firstName, string lastName, string emailAddress, int oid, string gstNumber, string feedURL, int jobFunction, int platForm)
    {
        int newLeadID = 0;
        try
        {
            if (lid > 0)
            {
                System.Collections.Generic.List<SqlParameter> list = new System.Collections.Generic.List<SqlParameter>();
                var update = new StringBuilder();
                if (!string.IsNullOrEmpty(gstNumber))
                {
                    update.Append("[GSTNumber] = @GST,");

                    SqlParameter p = new SqlParameter("@GST", SqlDbType.NVarChar);
                    p.Direction = ParameterDirection.Input;
                    p.Value = gstNumber;
                    list.Add(p);
                }
                if (!string.IsNullOrEmpty(feedURL))
                {
                    update.Append("[FeedURL] = @FeedURL,");

                    SqlParameter p = new SqlParameter("@FeedURL", SqlDbType.NVarChar);
                    p.Direction = ParameterDirection.Input;
                    p.Value = feedURL;
                    list.Add(p);
                }
                if (jobFunction > 0)
                {
                    update.Append("[JobFunction] = @Job,");

                    SqlParameter p = new SqlParameter("@Job", SqlDbType.Int);
                    p.Direction = ParameterDirection.Input;
                    p.Value = jobFunction;
                    list.Add(p);
                }
                if (platForm > 0)
                {
                    update.Append("[ECommercePlatform] = @Platform,");

                    SqlParameter p = new SqlParameter("@Platform", SqlDbType.Int);
                    p.Direction = ParameterDirection.Input;
                    p.Value = platForm;
                    list.Add(p);
                }
                if (update.Length > 0)
                {
                    string sql = string.Format("UPDATE [dbo].[CSK_Store_RetailerLeadSignUp] SET {0} WHERE RetailerLeadID = {1}",
                        update.ToString().TrimEnd(','), lid);
                    using (SqlConnection conn = new SqlConnection(connectionStr))
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandTimeout = 0;
                        cmd.Parameters.AddRange(list.ToArray());
                        conn.Open();
                        var obj = cmd.ExecuteScalar();
                        conn.Close();
                    }
                }
            }
            else
            {
                NewRetailerLead(retailerName, webSiteURL, firstName, lastName, emailAddress, gstNumber, oid);
            }
        }
        catch (Exception ex)
        {

        }
        return newLeadID;
    }

    #endregion
}