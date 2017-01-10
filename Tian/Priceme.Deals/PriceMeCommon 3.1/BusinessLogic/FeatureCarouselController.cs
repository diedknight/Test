using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PriceMeDBA;

namespace PriceMeCommon.BusinessLogic {
    public class FeatureCarouselController {
        public static List<CSK_Store_FeatureCarousel> Load() {
            PriceMeDBDB db = PriceMeStatic.PriceMeDB;
            return (from fc in db.CSK_Store_FeatureCarousels where fc.ENABLED == true orderby fc.ListOrder select fc).ToList<CSK_Store_FeatureCarousel>();
        }
    }
}
