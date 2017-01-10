using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PriceMeDBA;

namespace PriceMeCommon.BusinessLogic {
    public class PopularBrandsController {
        public static List<CSK_Store_PopularBrand> Load() {
            return CSK_Store_PopularBrand.All().OrderBy(b => b.ListOrder).ToList();
        }
    }
}
