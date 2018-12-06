using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChristmasSite.Logic;

namespace ChristmasSite.Code.Ajax
{
    public class AjaxController : IAjaxable
    {
        public string LoadCategory(AjaxContext context)
        {
            string html = "";
            string str = context.GetParameter("input", "").Trim().ToLower();
            if (str == "") return "";

            var allCategories = DBController.listCates;
            
            allCategories = allCategories.Where(item => item.CategoryName.ToLower().Contains(str)).ToList();

            allCategories.ForEach(item => {

                string url = SiteConfig.BlackFridayUrl + "?cid=" + item.CategoryId;

                html += $"<li value=\"{url}\">{item.CategoryName}</li>";
            });

            return html;
        }

    }
}
