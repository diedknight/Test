using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PriceMeDBA;
using PriceMeCommon;
using System.Data.SqlClient;

namespace PriceMeCommon
{
    public static class ProductDescriptorController
    {
        static Dictionary<int, string> productDescription = new Dictionary<int,string>();

        public static Dictionary<int, string> ProductDescription
        {
            get { return productDescription; }
        }

        public static void Load(Timer.DKTimer dkTimer)
        {
            if (dkTimer != null)
                dkTimer.Set("ProductDescriptorController.Load() --- Befor GetProductDescription");
            GetProductDescription();

            if (dkTimer != null)
                dkTimer.Set("ProductDescriptorController.Load() --- End GetProductDescription");
        }

        public static void Load()
        {
            Load(null);
        }

        private static PriceMeDBDB db = PriceMeStatic.PriceMeDB;

        public static List<CSK_Store_ProductDescriptor> GetProductDescriptorByProductId(int productId)
        {
            return (from pd in db.CSK_Store_ProductDescriptors where pd.ProductID == productId select pd).ToList();
        }

        public static CSK_Store_ProductDescriptorTitle GetProductDescriptorTitleByTypeId(int typeId)
        {
            return CSK_Store_ProductDescriptorTitle.SingleOrDefault(pdt => pdt.TypeID == typeId);
        }

        public static List<Store_Compare_Attribute> GetCompareAttributesByCategoryId(int categoryId)
        {
            return (from ca in db.Store_Compare_Attributes where ca.CategoryID == categoryId select ca).ToList();
        }

        public static Store_Compare_Attribute GetCompareAttributesById(int Id)
        {
            return Store_Compare_Attribute.SingleOrDefault(ca => ca.ID == Id);
        }

        public static List<Store_Compare_Attribute_Map> GetCompareAttributesMapByProductIdAndAId(int productId, int aId)
        {
            return (from cam in db.Store_Compare_Attribute_Maps where cam.ProductID == productId && cam.CompareAttributeID == aId select cam).ToList();
        }

        public static List<CSK_Store_ProductReview> GetRecentReviews()
        {
            return (from pr in db.CSK_Store_ProductReviews.Take(5) where pr.IsApproved == true orderby pr.PostDate descending select pr).ToList();
        }

        //public static int GetAllProductReviewsCountByAuthorName(string author)
        //{
        //    int count = 0;
        //    SubSonic.Schema.StoredProcedure sp = db.GetAllProductReviewCountByAuthorName();
        //    sp.Command.AddParameter("@author", author, DbType.String);
        //    IDataReader dr = sp.ExecuteReader();
        //    while (dr.Read())
        //        count = int.Parse(dr[0].ToString());
        //    dr.Close();

        //    return count;
        //}

        public static DataSet GetProductReviewByReviewID(int reviewID)
        {
            SubSonic.Schema.StoredProcedure sp = db.GetProductReviewByReviewID();
            sp.Command.AddParameter("reviewID", reviewID, DbType.Int32);

            return sp.ExecuteDataSet();
        }
        /// <summary>
        /// Get product description from common table: [dbo].[PM_ProductDescription ] of db pam_user
        /// </summary>
        private static void GetProductDescription()
        {
            productDescription = new Dictionary<int, string>();
            try
            {
                var connectionStr = System.Configuration.ConfigurationManager.ConnectionStrings["CommerceTemplate_Common"].ConnectionString;
                string sql = string.Format("SELECT [PID],[Description] FROM [CSK_Store_PM_ProductDescription] WHERE COUNTRYID = {0}", ConfigAppString.CountryID);
                using (SqlConnection conn = new SqlConnection(connectionStr))
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandTimeout = 0;
                    conn.Open();
                    IDataReader idr = cmd.ExecuteReader();
                    while (idr.Read())
                    {
                        int pid = 0;
                        if (int.TryParse(idr[0].ToString(), out pid))
                        {
                            productDescription.Add(pid, idr[1].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogWriter.WriteLineToFile(PriceMeCommon.ConfigAppString.ExceptionLogPath, "GetProductDescription() errer : " + ex.Message + ex.StackTrace);
            }
        }
    }
}
