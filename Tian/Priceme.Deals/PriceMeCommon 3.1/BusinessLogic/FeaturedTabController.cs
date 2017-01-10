using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using SubSonic;
using SubSonic.Schema;
using PriceMeDBA;

namespace PriceMeCommon.BusinessLogic {
    public class FeaturedTabController {

        private static PriceMeDBDB db = new PriceMeDBDB();

        public static List<CSK_Store_FeaturedTab> GetAll(){
            return CSK_Store_FeaturedTab.All().OrderBy(f => f.ListOrder).ToList();
        }

        public static IDataReader GetFeaturedProducts( int catID ) {
            //StoredProcedure sp = new StoredProcedure("CSK_Store_FeatureProducts");
            StoredProcedure sp = db.CSK_Store_FeatureProducts();
            sp.Command.AddParameter("@CATID",  catID, DbType.Int32 );
            return sp.ExecuteReader();
        }
    }
}
