using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceMeCache
{
    public class CommonAllList
    {
        public string ListName { get; set; }
        public string ListDesc { get; set; }
        public string UserName { get; set; }
        public string FirstImageUrl { get; set; }
        public string UserID { get; set; }
        public int ListID { get; set; }

        public DateTime? ModifyOn { get; set; }

        public int ListProCount { get; set; }
    }
}
