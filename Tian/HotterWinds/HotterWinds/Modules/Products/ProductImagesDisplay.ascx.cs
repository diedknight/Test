using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PriceMeCommon.Extend;
using PriceMeDBA;

namespace HotterWinds.Modules.Products
{
    public partial class ProductImagesDisplay : System.Web.UI.UserControl
    {
        public PriceMeDBA.CSK_Store_ProductNew Product = null;
        public int VideoCount = 0;

        protected string ImageAlt;
        protected string ImageUrl;

        protected List<string> listImages;

        protected void Page_Load(object sender, EventArgs e)
        {
            string noImageAvailable = "/images/no_image_available.gif";

            string orgimage = Product.DefaultImage;
            ImageAlt = Product.ProductName.SafeString();
            if (string.IsNullOrEmpty(ImageUrl) || !ImageUrl.Contains("."))
            {
                ImageUrl = noImageAvailable;
                orgimage = string.Empty;
                ImageAlt = "no image";
            }

            //ImageUrl = PriceMe.Utility.GetImage(Product.DefaultImage, "_ms");
            ImageUrl = PriceMe.Utility.GetLargeImage1(Product.DefaultImage);

            listImages = new List<string>();
            List<CSK_Store_Image> images = PriceMeDBStatic.PriceMeDB.CSK_Store_Images.Where(i => i.ProductID == Product.ProductID).ToList();
            if (images != null && images.Count > 0)
            {
                foreach (CSK_Store_Image img in images)
                {
                    if (!img.ImageFile.Contains("."))
                        continue;

                    string imgL = PriceMe.Utility.GetLargeImage(img.ImageFile);

                    listImages.Add(imgL);
                }
            }
            else if (!string.IsNullOrEmpty(orgimage))
            {
                string orgimageurl = orgimage;
                if (!orgimageurl.Contains("http://") && !orgimageurl.Contains("https://"))
                    orgimageurl = orgimage.FixUrl(Resources.Resource.ImageWebsite);
                listImages.Add(orgimageurl);
            }
        }
    }
}