using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SubSonic;
using System.IO;
using PriceMeCommon;

namespace Priceme.Deals.Code.Ajax
{
    public class AjaxController : IAjaxable
    {
        public string LoadCategory(AjaxContext context)
        {
            string html = "";
            string str = context.GetParameter("input", "").Trim().ToLower();
            if (str == "") return "";

            var allCategories = CategoryController.CategoryOrderByName.Where(item => CategoryController.GetCategoryProductCount(item.CategoryID) > 0 && item.IsAccessories == false).ToList();

            //allCategories = allCategories.Where(item =>
            // {
            //     string text = item.CategoryName.Trim().ToLower();
            //     for (var i = 0; i < str.Length; i++)
            //     {
            //         if (text.Length <= i) return false;
            //         if (text[i] != str[i]) return false;
            //     }

            //     return true;
            // }).ToList();

            allCategories = allCategories.Where(item => item.CategoryName.ToLower().Contains(str)).ToList();

            allCategories.ForEach(item => {

                string url = context.HttpContext.Request.Url.GetBaseUrl() + "/?cid=" + item.CategoryID;
                url = UrlRoute.Encode(url);

                html += $"<li value=\"{url}\">{item.CategoryName}</li>";
            });

            return html;
        }

    }
}
