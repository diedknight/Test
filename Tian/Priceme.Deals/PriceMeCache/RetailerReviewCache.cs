using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PriceMeDBA;

namespace PriceMeCache
{
    [Serializable]
    public class RetailerReviewCache
    {
        #region

        public int RetailerReviewId { get; set; }

        public int RetailerId { get; set; }

        public int EasyOfOrdering { get; set; }

        public int OnTimeDelivery { get; set; }

        public int CustomerCare { get; set; }

        public int Availability { get; set; }

        public int OverallStoreRating { get; set; }

        public string Goods { get; set; }

        public string Title { get; set; }

        public string Body { get; set; }

        public bool IsApproved { get; set; }

        public string AdminComments { get; set; }

        public string UserIP { get; set; }

        public int TotalComment { get; set; }

        public DateTime CreatedOn { get; set; }

        public string CreatedBy { get; set; }

        //
        public int ReviewID { get; set; }
        public int RetailerID { get; set; }
        public float Delivery { get; set; }
        public float Service { get; set; }
        public float EaseOfPurchase { get; set; }
        public float OverallRating { get; set; }
        public float ProductInfo { get; set; }
        public bool PurchaseAgain { get; set; }
        public string Email { get; set; }
        public string Descriptive { get; set; }

        public string SourceType { get; set; }
        #endregion
    }
}
