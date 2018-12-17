using PricemeResource.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PricemeResource.Logic
{
    public static class RetailerController
    {
        static TimeSpan CacheTimeSpan_Static = new TimeSpan(12, 0, 0);
        
        public static void Init()
        {
            GetRetailerDeep(0);
            IsPPcRetailer(0);
        }

        public static List<RetailerCache> LoadRetailer()
        {
            List<RetailerCache> listRetailers = new List<RetailerCache>();

            string sql = "select * from CSK_Store_Retailer where IsSetupComplete = 1 and RetailerStatus <> 99 order by retailername";
            using (var sqlConn = DBController.CreateDBConnection(WebSiteConfig.PamUserDbInfo))
            {
                using (var sqlCMD = DBController.CreateDbCommand(sql, sqlConn))
                {
                    sqlConn.Open();
                    using (var sqlDR = sqlCMD.ExecuteReader())
                    {
                        while (sqlDR.Read())
                        {
                            var rc = DbConvertController<RetailerCache>.ReadDataFromDataReader(sqlDR);
                            if (rc != null)
                            {
                                listRetailers.Add(rc);
                            }
                        }
                    }
                }
            }

            return listRetailers;
        }

        public static List<int> LoadPPCRetailer()
        {
            List<int> listPPCRetailers = new List<int>();

            string sql = "select r.RetailerId from CSK_Store_Retailer r inner join CSK_Store_PPCMember p On r.RetailerId = p.RetailerId where r.IsSetupComplete = 1 and r.RetailerStatus <> 99 and p.PPCMemberTypeID = 2";
            using (var sqlConn = DBController.CreateDBConnection(WebSiteConfig.PamUserDbInfo))
            {
                using (var sqlCMD = DBController.CreateDbCommand(sql, sqlConn))
                {
                    sqlConn.Open();
                    using (var sqlDR = sqlCMD.ExecuteReader())
                    {
                        while (sqlDR.Read())
                        {
                            int rid = 0;
                            int.TryParse(sqlDR["RetailerId"].ToString(), out rid);
                            listPPCRetailers.Add(rid);
                        }
                    }
                }
            }

            return listPPCRetailers;
        }

        public static RetailerCache GetRetailerDeep(int retailerId)
        {
            List<RetailerCache> listRetailers = null;

            string key = "ppcretailerlist";

            List<RetailerCache> cmd = MemoryCacheController.Get<List<RetailerCache>>(key);
            if (cmd != null)
            {
                listRetailers = cmd;
            }
            else
            {
                listRetailers = LoadRetailer();

                MemoryCacheController.Set<List<RetailerCache>>(key, listRetailers, CacheTimeSpan_Static);
            }

            RetailerCache retailer = listRetailers.SingleOrDefault(r => r.RetailerId == retailerId);
            
            if (retailer != null && string.IsNullOrEmpty(retailer.LogoFile))
                retailer.LogoFile = @"\images\retailerimages\no_retailer_image.png";

            return retailer;
        }

        public static bool IsPPcRetailer(int rid)
        {
            List<int> listPPCRetailers = null;
            string key = "retailerlist";

            List<int> cmd = MemoryCacheController.Get<List<int>>(key);
            if (cmd != null)
            {
                listPPCRetailers = cmd;
            }
            else
            {
                listPPCRetailers = LoadPPCRetailer();

                MemoryCacheController.Set<List<int>>(key, listPPCRetailers, CacheTimeSpan_Static);
            }

            if (listPPCRetailers.Contains(rid))
                return true;

            return false;
        }

        public static List<StoreGLatLng> GetRetailerGLatLng(int retailerId, DbInfo dbInfo)
        {
            List<StoreGLatLng> listSgl = new List<StoreGLatLng>();

            string sql = "";

            using (var sqlConn = DBController.CreateDBConnection(dbInfo))
            {
                if (sqlConn is MySql.Data.MySqlClient.MySqlConnection)
                {
                    sql = "SELECT * FROM Store_GLatLng where Retailerid = " + retailerId + " and LENGTH(GLat) >0 and LENGTH(Glng) > 0 and GLat <> '0' and Glng <> '0'";
                }
                else
                {
                    sql = "SELECT * FROM Store_GLatLng where Retailerid = " + retailerId + " and len(GLat) >0 and len(Glng) > 0 and GLat <> '0' and Glng <> '0'";
                }

                using (var sqlCMD = DBController.CreateDbCommand(sql, sqlConn))
                {
                    sqlConn.Open();
                    using (var sqlDR = sqlCMD.ExecuteReader())
                    {
                        while (sqlDR.Read())
                        {
                            var storeGLatLng = DbConvertController<StoreGLatLng>.ReadDataFromDataReader(sqlDR);
                            if (storeGLatLng != null)
                            {
                                listSgl.Add(storeGLatLng);
                            }
                        }
                    }
                }
            }

            return listSgl;
        }
    }
}
