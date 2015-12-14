using PriceMeDBA;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Text;

namespace PamAccountGenerator
{
    /// <summary>
    /// RetailerLeadHelper
    /// </summary>
    public class RetailerLeadHelper
    {
        static string connectionStr = ConfigurationManager.ConnectionStrings["CommerceTemplate_Common"].ConnectionString;

        #region read

        /// <summary>
        /// get retailer signup info by leadid
        /// </summary>
        /// <param name="lid"></param>
        /// <returns></returns>
        public static CSK_Store_RetailerLeadSignUp ReadRetailerLead(int lid)
        {
            #region

            CSK_Store_RetailerLeadSignUp signUp = null;
            var sql = string.Format("SELECT * FROM [dbo].[CSK_Store_RetailerLeadSignUp] WHERE RetailerLeadID = '{0}'", lid);
            using (SqlConnection conn = new SqlConnection(connectionStr))
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 0;
                conn.Open();
                var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    signUp = new CSK_Store_RetailerLeadSignUp();
                    signUp.RetailerLeadID = int.Parse(reader["RetailerLeadID"].ToString());
                    signUp.SiteCountryID = int.Parse(reader["SiteCountryID"].ToString());
                    signUp.CapsuleOrgId = int.Parse(reader["CapsuleOrgId"].ToString());
                    signUp.RetailerName = reader["RetailerName"].ToString();
                    signUp.WebsiteURL = reader["WebsiteURL"].ToString();
                    signUp.StoreType = int.Parse(reader["StoreType"].ToString());
                    signUp.ContactFirstName = reader["ContactFirstName"].ToString();
                    signUp.ContactLastName = reader["ContactLastName"].ToString();
                    signUp.ContactEmail = reader["ContactEmail"].ToString();
                    signUp.JobFunction = int.Parse(reader["JobFunction"].ToString());
                    signUp.GSTNumber = reader["GSTNumber"].ToString();
                    signUp.FeedURL = reader["FeedURL"].ToString();
                    if (!string.IsNullOrEmpty(reader["ECommercePlatform"].ToString()))
                    {
                        int platForm = 0;
                        int.TryParse(reader["ECommercePlatform"].ToString(), out platForm);
                        signUp.ECommercePlatform = platForm;
                    }
                }
                conn.Close();
            }

            #endregion

            return signUp;
        }

        /// <summary>
        /// search same name retailer by input retailer name
        /// from CSK_Store_RetailerLeadSignUp
        /// </summary>
        public static List<CSK_Store_RetailerLeadSignUp> SearchRetailer(string retailer)
        {
            List<CSK_Store_RetailerLeadSignUp> list = new List<CSK_Store_RetailerLeadSignUp>();

            var sql = string.Format("SELECT * FROM [dbo].[CSK_Store_RetailerLeadSignUp] WHERE RetailerName LIKE '{0}' and RetailerLeadID in(select distinct RetailerLeadId from CSK_Store_RetailerLeadtracking where isnull(RetailerId,0)=0)", retailer);
            //var sql = string.Format("SELECT * FROM [dbo].[CSK_Store_RetailerLeadSignUp] WHERE RetailerName LIKE '{0}' ", retailer);
            using (SqlConnection conn = new SqlConnection(connectionStr))
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 0;
                conn.Open();
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    CSK_Store_RetailerLeadSignUp sign = new CSK_Store_RetailerLeadSignUp();
                    sign.RetailerLeadID = int.Parse(reader["RetailerLeadID"].ToString());
                    sign.RetailerName = reader["RetailerName"].ToString();
                    sign.SiteCountryID = int.Parse(reader["SiteCountryID"].ToString());

                    list.Add(sign);
                }
                conn.Close();
            }

            return list;
        }

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
        /// <param name="oid"></param>
        /// <returns></returns>
        public static int NewRetailerLead(string retailerCountry, string retailerName, string webSiteURL, string firstName, string lastName, string emailAddress, int oid)
        {
            int newLeadID = 0;
            try
            {
                string sql = string.Format("INSERT INTO [dbo].[CSK_Store_RetailerLeadSignUp] " +
                    "([SiteCountryID],[RetailerName],[WebsiteURL],[StoreType],[ContactFirstName],[ContactLastName],[ContactEmail],[JobFunction],[CapsuleOrgId],[ValidLead])" +
                    "VALUES({0},'{1}','{2}',0,'{3}','{4}','{5}',0,'{6}','true');SELECT @@IDENTITY;",
                    retailerCountry, retailerName, webSiteURL, firstName, lastName, emailAddress, oid);
                using (SqlConnection conn = new SqlConnection(connectionStr))
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandTimeout = 0;
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
        /// <param name="siteCountryID"></param>
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
        public static int UpdateRetailerLead(int lid, string retailerCountry, string retailerName, string webSiteURL, string firstName, string lastName, string emailAddress, int oid, string gstNumber, string feedURL, int jobFunction, int platForm)
        {
            int newLeadID = 0;
            try
            {
                if (lid > 0)
                {
                    var update = new StringBuilder();
                    if (!string.IsNullOrEmpty(gstNumber))
                    {
                        update.AppendFormat("[GSTNumber] = '{0}',", gstNumber);
                    }
                    if (!string.IsNullOrEmpty(feedURL))
                    {
                        update.AppendFormat("[FeedURL] = '{0}',", feedURL);
                    }
                    if (jobFunction > 0)
                    {
                        update.AppendFormat("[JobFunction] = {0},", jobFunction);
                    }
                    if (platForm > 0)
                    {
                        update.AppendFormat("[ECommercePlatform] = {0},", platForm);
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
                            conn.Open();
                            var obj = cmd.ExecuteScalar();
                            conn.Close();
                        }
                    }
                }
                else
                {
                    NewRetailerLead(retailerCountry, retailerName, webSiteURL, firstName, lastName, emailAddress, oid);
                }
            }
            catch (Exception ex)
            {

            }
            return newLeadID;
        }

        /// <summary>
        /// Save table RetailerLeadTracking column
        /// RetailerID
        /// RetailerCreatedTime
        /// </summary>
        /// <param name="lid"></param>
        /// <param name="oid"></param>
        /// <param name="retailer"></param>
        public static void SaveRetailerLeadTracking(string lid, string oid, int retailer)
        {
            #region

            try
            {
                string sql_update = string.Format("UPDATE [dbo].[CSK_Store_RetailerLeadTracking] SET RetailerID = '{0}', RetailerCreatedTime = GETDATE() WHERE RetailerLeadId = {1};",
                 retailer, lid);
                using (SqlConnection conn = new SqlConnection(connectionStr))
                using (SqlCommand cmd = new SqlCommand(sql_update, conn))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandTimeout = 0;
                    conn.Open();
                    var rows = cmd.ExecuteNonQuery();
                    if (rows <= 0)
                    {
                        #region new CSK_Store_RetailerLeadTrack

                        string sql_add = string.Format("INSERT INTO [dbo].[CSK_Store_RetailerLeadTracking] ([RetailerLeadId],[RetailerID],[SignUpTime],[RetailerCreatedTime]) VALUES ({0} ,{1} ,GETDATE(),GETDATE());",
                            lid, retailer);
                        using (SqlCommand cmd2 = new SqlCommand(sql_add, conn))
                        {
                            cmd2.CommandType = CommandType.Text;
                            cmd2.CommandTimeout = 0;
                            cmd2.ExecuteNonQuery();
                        }

                        #endregion
                    }

                    CapsuleCRMHelper.UpdateOpportunityToOrganisation(oid, "Proposal");

                    conn.Close();
                }
            }
            catch (Exception ex)
            {

            }

            #endregion
        }
        
        /// <summary>
        /// Add value to column CapsuleOrgId of table CSK_Store_Retailer
        /// </summary>
        /// <param name="retailer"></param>
        /// <param name="oid"></param>
        public static void SaveRetailerCapsuleOrgId(int retailer, int oid)
        {
            #region

            try
            {
                if (oid > 0)
                {
                    CSK_Store_Retailer Retailer = CSK_Store_Retailer.SingleOrDefault(p => p.RetailerId == retailer);
                    if (Retailer == null || Retailer.RetailerId != retailer)
                    {
                        #region dbo
                        string connectionStr_ = ConfigurationManager.ConnectionStrings["CommerceTemplate"].ConnectionString;
                        string sql_update = string.Format("UPDATE [dbo].[CSK_Store_Retailer] SET CapsuleOrgId = {0} WHERE RetailerId = {1}", oid, retailer);
                        using (SqlConnection conn = new SqlConnection(connectionStr_))
                        using (SqlCommand cmd = new SqlCommand(sql_update, conn))
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandTimeout = 0;
                            conn.Open();
                            var rows = cmd.ExecuteNonQuery();

                            conn.Close();
                        }
                        #endregion
                    }
                    else 
                    {
                        Retailer.CapsuleOrgId = oid;
                        Retailer.Save();
                    }
                }
            }
            catch (Exception ex)
            {

            }

            #endregion
        }

        #endregion
    }
}