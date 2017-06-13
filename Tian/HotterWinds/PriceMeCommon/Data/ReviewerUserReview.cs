using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PriceMeCommon.Data
{
    public class ReviewerUserReview
    {

        public int ProductID { get; set; }
        
        public int CategoryID { get; set; }

        public int Rating { get; set; }

        public string Body { get; set; }

        public string AuthorName { get; set; }

        public string ProductName { get; set; }

        public string DefaultImage { get; set; }

        public DateTime PostDate { get; set; }

        public string Title { get; set; }

        public int ManufacturerID { get; set; }

        public string UserName { get; set; }

        public DateTime PostDate2 { get; set; }
    }
}