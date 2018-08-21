using System;
using System.Collections.Generic;
using System.Text;

namespace ExpertReviewIndex.Data
{
    public class ProductVideo
    {
        private int productID;

        public int ProductID
        {
            get { return productID; }
            set { productID = value; }
        }
        private string url;

        public string Url
        {
            get { return url; }
            set { url = value; }
        }
        private string thumbnail;

        public string Thumbnail
        {
            get { return thumbnail; }
            set { thumbnail = value; }
        }

    }
}
