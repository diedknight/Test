using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using PriceMeCommon.Data;

namespace HotterWinds.Modules.Catalog
{
    public partial class NewFilters : System.Web.UI.UserControl
    {
        protected int MaxValueCount = 5;

        public PriceRange MyPriceRange;
        public string UrlNoSelection;
        public List<NarrowByInfo> NarrowByInfoList;
        public int CurrentProductCount;
        public int TotalProductCount;
        public Dictionary<string, string> RemoveUrlDic;
        public string SearchWithIn;
        public DaysRange MyDaysRange;
        public List<LinkInfo> Selections;

        public int PageSize;
        public int PagePosition;
        public string View;
        public string SortBy;
        public string QueryKeywords;
        public string PageToName;
        public int CategoryID;
        public string DefaultView;
        public string DefaultSortBy;

        public bool IsAjax = false;

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public int GetIconHeight(int cateID, int typeID, int count, double av, int maxCount)
        {
            if (av == 0d || count == 0)
            {
                return 0;
            }

            if (maxCount < 31)
            {
                return count;
            }

            int max = 31;
            int min = 2;

            var hh = (int)(decimal.Round((decimal)(count * max / maxCount)));

            if (hh > max)
            {
                return max;
            }
            else if (hh < min)
            {
                return min;
            }
            else
            {
                return hh;
            }
        }

        public string GetRemoveUrl(string filterName, string filterValue)
        {
            string removeUrl = "";
            if (RemoveUrlDic == null)
                return removeUrl;

            if (filterName == "Manufacturer")
            {
                RemoveUrlDic.TryGetValue("m-" + filterValue, out removeUrl);
            }
            else if (filterName == "Attribute")
            {
                RemoveUrlDic.TryGetValue("av-" + filterValue, out removeUrl);
            }
            else if (filterName == "AttributeRange")
            {
                RemoveUrlDic.TryGetValue("avr-" + filterValue, out removeUrl);
            }
            else if (filterName == "Retailer")
            {
                RemoveUrlDic.TryGetValue("r-" + filterValue, out removeUrl);
            }
            else if (filterName == "On sale")
            {
                RemoveUrlDic.TryGetValue("os-" + filterValue, out removeUrl);
            }
            return removeUrl;
        }

        protected string CreateHeightBarHtml(string barId, List<int> countList)
        {
            if (countList.Count == 0)
                return "";

            System.Text.StringBuilder htmlSB = new System.Text.StringBuilder("<ul id='heightBar_" + barId + "' class='heightBar ajRefreshUL'>");

            float withP = 90f / countList.Count;
            for (int i = 0; i < countList.Count; i++)
            {
                int height = countList[i];
                htmlSB.Append("<li id='" + barId + '_' + i + "' class='ajRefreshLI' style='" + "height:" + height + "px" + ";" + "width:" + withP + "%" + "'></li>");
            }

            htmlSB.Append("</ul>");

            return htmlSB.ToString();
        }
    }
}