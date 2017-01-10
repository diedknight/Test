using System;
using System.Data;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PriceMeDBA;
using PriceMeCommon;
using PriceMeCommon.Data;
using SubSonic;
using SubSonic.Schema;
using SubSonic.DataProviders;

namespace PriceMeCommon
{
    /// <summary>
    /// Controller for business logic relating to Products
    /// </summary>
    public static class ProductRatingController
    {
        /// <summary>
        /// Adds a rating to a product for a given user and resets the product's rating stats
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="productID"></param>
        /// <param name="rating"></param>
        public static void AddUserRating(string userName, int productID, int rating)
        {
            PriceMeDBDB db = PriceMeStatic.PriceMeDB;
            StoredProcedure sp = db.CSK_Store_Product_AddRating();
            sp.Command.AddParameter("@productID", productID, DbType.Int32);
            sp.Command.AddParameter("@rating", rating, DbType.Int32);
            sp.Command.AddParameter("@userName", userName, DbType.String);
            sp.Execute();
        }

        /// <summary>
        /// Gets the user's rating for a products
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="productID"></param>
        /// <returns></returns>
        public static int GetUserRating(string userName, int productID)
        {
            PriceMeDBDB db = PriceMeStatic.PriceMeDB;
            int iOut = -1;
            
            var rat = from r in db.CSK_Store_ProductRatings where r.UserName == userName && r.ProductID == productID select r.Rating;
            if (rat.Count() > 0 && rat != null)
                iOut = rat.SingleOrDefault();

            return iOut;
        }        
    }
}