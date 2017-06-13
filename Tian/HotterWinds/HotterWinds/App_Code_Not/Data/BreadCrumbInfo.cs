using PriceMeCommon.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PriceMe
{
    public class BreadCrumbInfo
    {
        string _currentPageName;

        public List<LinkInfo> linkInfoList;

        public string CurrentPageName
        {
            get { return _currentPageName; }
            set { _currentPageName = value; }
        }

        public string CurrentPageId { get; set; }
        public string CurrentPageKey { get; set; }
        public decimal ProductBestPrice { get; set; }
    }
}