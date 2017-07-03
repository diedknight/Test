using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceMeCommon.Data
{
    public class NarrowItemGroup
    {
        public NarrowItemGroup()
        {
            GroupItems = new List<NarrowItem>();
        }

        public int GroupID { get; set; }
        public string GroupName { get; set; }
        public string GroupUrl { get; set; }
        public bool IsSelected { get; set; }
        public List<NarrowItem> GroupItems { get; set; }
    }
}
