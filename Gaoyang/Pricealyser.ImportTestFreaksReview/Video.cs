using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pricealyser.ImportTestFreaksReview
{
    public class Video
    {
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
