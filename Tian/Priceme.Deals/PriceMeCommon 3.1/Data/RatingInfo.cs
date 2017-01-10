using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PriceMeCommon.Data
{
    public class RatingInfo
    {
        string ratingTitle = "";
        public string RatingTitle
        {
            get { return ratingTitle; }
            set { ratingTitle = value; }
        }

        string ratingImageUrl = "";
        public string RatingImageUrl
        {
            get { return ratingImageUrl; }
            set { ratingImageUrl = value; }
        }

        string ratingCss = "";
        public string RatingCss
        {
            get { return ratingCss; }
            set { ratingCss = value; }
        }
    }
}
