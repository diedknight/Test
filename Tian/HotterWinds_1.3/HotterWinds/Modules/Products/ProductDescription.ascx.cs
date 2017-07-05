using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PriceMeCache;
using PriceMeCommon;
using PriceMeCommon.BusinessLogic;
using PriceMeDBA;

namespace HotterWinds.Modules.Products
{
    public partial class ProductDescription : System.Web.UI.UserControl
    {
        public CSK_Store_ProductNew product = null;
        public List<AttributeGroup> pdas;
        public bool isDisplay = false;
        public int ManufacturerId;
        public string ManufacturerName;
        public string ManufacturerProductUrl;

        public string categoryName = "";

        const string rich_up = "<span class='glyphicon glyphicon-thumbs-up' aria-hidden='true'></span>";
        const string rich_down = "<span class='glyphicon glyphicon-thumbs-down' aria-hidden='true'></span>";
         
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected string GetPhysical()
        {
            decimal length = product.Length.Value, width = product.Width.Value, height = product.Height.Value, weight = product.Weight.Value;
            string munit = string.Empty;
            if (!string.IsNullOrEmpty(product.UnitOfMeasure))
                munit = product.UnitOfMeasure;

            List<string> ls = new List<string>();
            string info = "N02";
            if (munit.ToLower() == "mm")
                info = "N00";

            if (height > 0)
                ls.Add(string.Format("<li class=\"attLi\"><div class=\"rich-left-attr\"><span class=\"attTitle\">" + Resources.Resource.TextString_Height + "</span></div><div class=\"rich-right-attr\">{0} {1}</div><div style=\"clear:both;\"></div></li>", height.ToString(info), munit));
            if (width > 0)
                ls.Add(string.Format("<li class=\"attLi\"><div class=\"rich-left-attr\"><span class=\"attTitle\">" + Resources.Resource.TextString_Width + "</span></div><div class=\"rich-right-attr\">{0} {1}</div><div style=\"clear:both;\"></div></li>", width.ToString(info), munit));
            if (length > 0)
                ls.Add(string.Format("<li class=\"attLi\"><div class=\"rich-left-attr\"><span class=\"attTitle\">" + Resources.Resource.TestString_Depth + "</span></div><div class=\"rich-right-attr\">{0} {1}</div><div style=\"clear:both;\"></div></li>", length.ToString(info), munit));
            if (weight > 0)
            {
                if (CategoryController.IsNoWeightUnitCategory(product.CategoryID ?? 0, PriceMe.WebConfig.CountryId))
                    ls.Add(string.Format("<li class=\"attLi\"><div class=\"rich-left-attr\"><span class=\"attTitle\">" + Resources.Resource.TextString_Weight + "</span></div><div class=\"rich-right-attr\">{0} {1}</div></li>", (weight * 1000).ToString("0"), "g"));
                else
                    ls.Add(string.Format("<li class=\"attLi\"><div class=\"rich-left-attr\"><span class=\"attTitle\">" + Resources.Resource.TextString_Weight + "</span></div><div class=\"rich-right-attr\">{0} {1}</div></li>", weight.ToString("N02"), "kg"));
            }
            return string.Join("", ls.ToArray());
        }

        protected string GetProductDate()
        {
            var culture = System.Threading.Thread.CurrentThread.CurrentCulture;
            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture(Resources.Resource.TextString_Culture);
            System.Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat.Calendar = new System.Globalization.GregorianCalendar();

            string date = (product.CreatedOn ?? DateTime.Now).ToString("dd MMMM yyyy");
            if (PriceMe.WebConfig.CountryId == 41)
                date = (product.CreatedOn ?? DateTime.Now).ToString("yyyy MMMM dd");

            System.Threading.Thread.CurrentThread.CurrentCulture = culture;
            return date;
        }


        protected string getRankTxt(PriceMeCache.AttributeCategoryComparison rich_result, double cur_val)
        {
            string istop = "";


            double top10 = double.Parse(rich_result.Top10);
            double top20 = double.Parse(rich_result.Top20);
            double top30 = double.Parse(rich_result.Top30);
            double bottom30 = double.Parse(rich_result.Bottom30);
            double bottom20 = double.Parse(rich_result.Bottom20);
            double bottom10 = double.Parse(rich_result.Bottom10);

            if (rich_result.IsHigherBetter)
            {
                if (cur_val >= top10)
                    istop = rich_up + " Top 10%";
                else if (cur_val < top10 && cur_val >= top20)
                    istop = rich_up + " Top 20%";
                else if (cur_val < top20 && cur_val >= top30)
                    istop = rich_up + " Top 30%";

                else if (cur_val <= bottom30 && cur_val >= bottom20)
                    istop = rich_down + " Bottom 30%";
                else if (cur_val < bottom20 && cur_val >= bottom10)
                    istop = rich_down + " Bottom 20%";
                else if (cur_val < bottom10)
                    istop = rich_down + " Bottom 10%";
            }
            else
            {
                if (cur_val <= bottom10)
                    istop = rich_up + " Top 10%";
                else if (cur_val > bottom10 && cur_val <= bottom20)
                    istop = rich_up + " Top 20%";
                else if (cur_val > bottom20 && cur_val <= bottom30)
                    istop = rich_up + " Top 30%";


                else if (cur_val >= top10)
                    istop = rich_down + " Bottom 10%";
                else if (cur_val >= top20)
                    istop = rich_down + " Bottom 20%";
                else if (cur_val >= top30)
                    istop = rich_down + " Bottom 30%";
            }

            return istop;
        }


        protected string getRankDesc(string title, double isHigherOrLower, string isLower, string istop)
        {
            var desc = new System.Text.StringBuilder();
            desc.Append("The ");
            desc.Append(title.ToLower());
            desc.Append(" is ");
            desc.Append("<span style='font-weight:bolder;'>" + isHigherOrLower + "% " + isLower + "</span>");
            desc.Append(" than the average, and ranks in the <span style='font-weight:bolder;'>" + istop.ToLower().Replace(rich_up, "").Replace(rich_down, "") + "</span>");
            desc.Append(" among ");
            desc.Append(categoryName + ".");
            return desc.ToString();
        }
    }
}