using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PriceMeCommon.Data
{
    public class NarrowByInfo
    {
        public int ID { get; set; }
        public bool IsBool { get; set; }
        public List<int> ProductCountListWithoutP { get; set; }

        public NarrowByInfo()
        {
            IsBool = false;
            NarrowItemGroupDic = new Dictionary<int, NarrowItemGroup>();
            NarrowItemList = new List<NarrowItem>();
            Name = "";
            Title = "";
        }

        public Dictionary<int, NarrowItemGroup> NarrowItemGroupDic { get; set; }

        public List<NarrowItem> NarrowItemList { get; set; }

        public int ListOrder { get; set; }

        public string Name { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public bool IsSlider { get; set; }

        public string SelectedValue { get; set; }

        public object CategoryAttributeTitleMap { get; set; }
    }
}