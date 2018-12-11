using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ChristmasSite.Logic
{
    public static class ChristmasSiteController
    {
        private static string ImageWebsite_Static = "https://images.pricemestatic.com";

        public static string GetImage(string imageurl, string size)
        {
            string img = string.Empty;
            if (string.IsNullOrEmpty(imageurl))
                img = ImageWebsite_Static + "/images/no_image_available.gif";
            else
            {
                if (!string.IsNullOrEmpty(size))
                    img = FixImagePath(imageurl.Insert(imageurl.LastIndexOf('.'), "_" + size));
                else
                    img = imageurl;

                if (!img.Contains("https://") && !img.Contains("http://"))
                    img = ImageWebsite_Static + img;
                if (img.Contains("http://"))
                    img = img.Replace("http://", "https://");

                if (img.StartsWith("/"))
                    img = img.Substring(1, img.Length - 1);
            }

            return img;
        }

        public static string FixImagePath(string path)
        {
            string str = path.Replace("\\", "/");
            if (!str.StartsWith("/") && !str.StartsWith("http:") && !str.StartsWith("https:"))
            {
                str = "/" + str;
            }
            return str;
        }

        public static int GetIconHeight(int count, double av, int maxCount)
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

        public static string CreateHeightBarHtml(string barId, List<int> countList)
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
