using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PriceMeDBA;

namespace PriceMeCommon.BusinessLogic {
    public class ConsumerController {
        private static List<int> _all = new List<int>();

        public static List<int> All {
            get { return ConsumerController._all; }
        }
        
        static ConsumerController() {
            _all = CSK_Store_ConsumerPriceMeMapping.All().Select(c => c.ProductId).ToList();
        }
    }
}
