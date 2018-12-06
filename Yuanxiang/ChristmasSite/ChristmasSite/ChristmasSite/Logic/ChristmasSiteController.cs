using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}
