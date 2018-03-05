using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using ExtensionWebsite.Data;

namespace ExtensionWebsite.Code
{
    public static class SiteConfig
    {
        public static Dictionary<int, string> dicConnection;
        public static List<Retailer> ListRetailer;
        public static List<int> ListOverseasRetailer;
        public static List<RetailerExtension> ListRetailerExtension;
        public static string Countrys;

        public static void Load()
        {
            Countrys = System.Configuration.ConfigurationManager.AppSettings["Countrys"].ToString();
            BindConnection();
            BindRetailerProductUrls();
            BindOverseasRetailers();
            BindRetailerExtension();
            BindRetailerProductExtension();
        }

        private static void BindConnection()
        {
            dicConnection = new Dictionary<int, string>();
            string connection = System.Configuration.ConfigurationManager.ConnectionStrings["CommerceTemplate"].ConnectionString;
            dicConnection.Add(0, connection);

            connection = System.Configuration.ConfigurationManager.ConnectionStrings["AUConnectionString"].ConnectionString;
            dicConnection.Add(1, connection);

            connection = System.Configuration.ConfigurationManager.ConnectionStrings["NZConnectionString"].ConnectionString;
            dicConnection.Add(3, connection);

            connection = System.Configuration.ConfigurationManager.ConnectionStrings["PHConnectionString"].ConnectionString;
            dicConnection.Add(28, connection);

            connection = System.Configuration.ConfigurationManager.ConnectionStrings["SGConnectionString"].ConnectionString;
            dicConnection.Add(36, connection);

            connection = System.Configuration.ConfigurationManager.ConnectionStrings["HKConnectionString"].ConnectionString;
            dicConnection.Add(41, connection);

            connection = System.Configuration.ConfigurationManager.ConnectionStrings["MYConnectionString"].ConnectionString;
            dicConnection.Add(45, connection);

            connection = System.Configuration.ConfigurationManager.ConnectionStrings["IDConnectionString"].ConnectionString;
            dicConnection.Add(51, connection);

            connection = System.Configuration.ConfigurationManager.ConnectionStrings["THConnectionString"].ConnectionString;
            dicConnection.Add(55, connection);

            connection = System.Configuration.ConfigurationManager.ConnectionStrings["VNConnectionString"].ConnectionString;
            dicConnection.Add(56, connection);
        }

        private static void BindRetailerProductUrls()
        {
            ListRetailer = new List<Retailer>();
            string sql = "Select r.RetailerId, RetailerName, RetailerURL, LogoFile, r.RetailerCountry, p.PPCMemberTypeID From csk_store_retailer r "
                       + "inner join CSK_Store_PPCMember p on r.RetailerId = p.RetailerId Where RetailerStatus = 1 And IsSetupComplete = 1 And r.RetailerCountry in (" + Countrys + ")";
            using (SqlConnection sqlConn = new SqlConnection(dicConnection[0]))
            {
                sqlConn.Open();
                using (System.Data.SqlClient.SqlCommand sqlCMD = new System.Data.SqlClient.SqlCommand())
                {
                    sqlCMD.CommandText = sql;
                    sqlCMD.CommandTimeout = 0;
                    sqlCMD.CommandType = CommandType.Text;
                    sqlCMD.Connection = sqlConn;

                    IDataReader dr = sqlCMD.ExecuteReader();
                    while (dr.Read())
                    {
                        int rid, countryid = 0;
                        int.TryParse(dr["RetailerId"].ToString(), out rid);
                        int.TryParse(dr["RetailerCountry"].ToString(), out countryid);
                        string rname = dr["RetailerName"].ToString();
                        string url = dr["RetailerURL"].ToString();
                        if (url.Contains("//"))
                            url = url.Split(new string[] { "//" }, StringSplitOptions.None)[1];

                        string logoFile = dr["LogoFile"].ToString();
                        logoFile = "https://images2.pricemestatic.com" + Utility.GetSpecialSize(logoFile, "s");
                        int type = 0;
                        int.TryParse(dr["PPCMemberTypeID"].ToString(), out type);

                        Retailer r = new Retailer();
                        r.RetailerId = rid;
                        r.RetailerName = rname;
                        r.RetailerUrl = url;
                        r.RetailerLog = logoFile;
                        r.IsNolink = (type == 2 ? false : true);
                        r.RetailerCountry = countryid;

                        ListRetailer.Add(r);
                    }
                    dr.Close();
                }
                sqlConn.Close();
            }
        }

        private static void BindRetailerExtension()
        {
            string sql = "select RetailerId, RetailerURLExtension from RetailerExtension Where TypeId = 1";
            using (SqlConnection sqlConn = new SqlConnection(dicConnection[0]))
            {
                sqlConn.Open();
                using (System.Data.SqlClient.SqlCommand sqlCMD = new System.Data.SqlClient.SqlCommand())
                {
                    sqlCMD.CommandText = sql;
                    sqlCMD.CommandTimeout = 0;
                    sqlCMD.CommandType = CommandType.Text;
                    sqlCMD.Connection = sqlConn;

                    IDataReader dr = sqlCMD.ExecuteReader();
                    while (dr.Read())
                    {
                        int rid;
                        int.TryParse(dr["RetailerId"].ToString(), out rid);
                        string url = dr["RetailerURLExtension"].ToString();

                        Retailer retailer = ListRetailer.SingleOrDefault(r => r.RetailerId == rid);
                        if (retailer != null)
                        {
                            retailer.RetailerUrl = url;

                            ListRetailer.Add(retailer);
                        }
                    }
                    dr.Close();
                }
                sqlConn.Close();
            }
        }

        private static void BindOverseasRetailers()
        {
            ListOverseasRetailer = new List<int>();
            string sql = "Select r.RetailerId, r.RetailerCountry, p.RetailerCountry From CSK_Store_Retailer r "
                       + "inner join CSK_Store_PPCMember p On r.RetailerId = p.RetailerId And r.RetailerCountry != p.RetailerCountry";
            using (SqlConnection sqlConn = new SqlConnection(dicConnection[0]))
            {
                sqlConn.Open();
                using (System.Data.SqlClient.SqlCommand sqlCMD = new System.Data.SqlClient.SqlCommand())
                {
                    sqlCMD.CommandText = sql;
                    sqlCMD.CommandTimeout = 0;
                    sqlCMD.CommandType = CommandType.Text;
                    sqlCMD.Connection = sqlConn;

                    IDataReader dr = sqlCMD.ExecuteReader();
                    while (dr.Read())
                    {
                        int rid = 0;
                        int.TryParse(dr["RetailerId"].ToString(), out rid);
                        ListOverseasRetailer.Add(rid);
                    }
                    dr.Close();
                }
                sqlConn.Close();
            }
        }

        private static void BindRetailerProductExtension()
        {
            ListRetailerExtension = new List<RetailerExtension>();
            string sql = "select RetailerId, TypeId, ConvertTxt from RetailerExtension Where TypeId != 1";
            using (SqlConnection sqlConn = new SqlConnection(dicConnection[0]))
            {
                sqlConn.Open();
                using (System.Data.SqlClient.SqlCommand sqlCMD = new System.Data.SqlClient.SqlCommand())
                {
                    sqlCMD.CommandText = sql;
                    sqlCMD.CommandTimeout = 0;
                    sqlCMD.CommandType = CommandType.Text;
                    sqlCMD.Connection = sqlConn;

                    IDataReader dr = sqlCMD.ExecuteReader();
                    while (dr.Read())
                    {
                        int rid, typeid;
                        int.TryParse(dr["RetailerId"].ToString(), out rid);
                        int.TryParse(dr["TypeId"].ToString(), out typeid);
                        string convertTxt = dr["ConvertTxt"].ToString();

                        RetailerExtension re = new RetailerExtension();
                        re.RetailerId = rid;
                        re.TypeId = typeid;
                        re.ConvertTxt = convertTxt;

                        ListRetailerExtension.Add(re);
                    }
                    dr.Close();
                }
                sqlConn.Close();
            }
        }
    }
}