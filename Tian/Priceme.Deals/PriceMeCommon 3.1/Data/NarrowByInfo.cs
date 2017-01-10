using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PriceMeCommon.Data
{
    public class NarrowByInfo
    {
        string _title;
        string _name;
        string _discription;
        int _listOrder;
        public int ID { get; set; }
        public bool IsBool { get; set; }
        public List<int> ProductCountListWithoutP { get; set; }

        public NarrowByInfo()
        {
            IsBool = false;
            NarrowItemGroupDic = new Dictionary<int, NarrowItemGroup>();
            NarrowItemList = new List<NarrowItem>();
            _name = "";
            _title = "";
        }

        public Dictionary<int, NarrowItemGroup> NarrowItemGroupDic { get; set; }

        public List<NarrowItem> NarrowItemList { get; set; }

        public int ListOrder
        {
            get { return _listOrder; }
            set { _listOrder = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }

        public string Discription
        {
            get { return _discription; }
            set { _discription = value; }
        }

        public bool IsSlider
        {
            get;
            set;
        }

        public string SelectedValue
        {
            get;
            set;
        }

        public object CategoryAttributeTitleMap
        {
            get;
            set;
        }
    }
}