using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HotterWinds.HWUtility
{
    public class Utility
    {
        public static string FixImagePath(string imagePath, string size)
        {
            if (string.IsNullOrEmpty(imagePath)) return "";
            if (imagePath.IndexOf(".") == -1) return "";

            if (imagePath.ToLower().Contains("https") || imagePath.ToLower().Contains("http"))
            {
                return imagePath.Insert(imagePath.LastIndexOf('.'), size);
            }
            else
            {
                string imageUrl = "https://images.pricemestatic.com" + imagePath.Insert(imagePath.LastIndexOf('.'), size).Replace("\\", "/");

                return imageUrl;
            }
        }
    }
}