using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using SubSonic.Schema;

namespace PriceMeDBA {
    public partial class CSK_Store_Retailer: IActiveRecord{
        private decimal _avRating = 0;
        private string _reviewString = "";
        private string _retailerLogoSmall = "";
        private List<CSK_Store_RetailerReview> _reviews = null;
        private int _reviewsCount = 0;
        //private CSK_Store_RetailerVotesSum _vote = null;
        private decimal rating = 0m;
        private string storeTypeName = string.Empty;
        private int clicks = 0;

        public int Clicks
        {
            get { return clicks; }
            set { clicks = value; }
        }

        //public decimal AvRating
        //{
        //    get
        //    {
        //        if (Vote.RetailerTotalRatingVotes > 1)
        //        {
        //            _avRating = decimal.Round((Vote.RetailerRatingSum - 3m) / (Vote.RetailerTotalRatingVotes - 1m), 1);
        //            _avRating = _avRating.ToString().Length > 3 ? decimal.Parse(_avRating.ToString().Substring(0, 3)) : _avRating;
        //        }
        //        return _avRating;
        //    }
        //}
        public decimal AvRating
        {
            get
            {
                if (this.RetailerTotalRatingVotes > 1)
                {
                    _avRating = decimal.Round((((RetailerRatingSum ?? 0) - 3m) / ((RetailerTotalRatingVotes ?? 2) - 1m)), 1);
                    _avRating = _avRating.ToString().Length > 3 ? decimal.Parse(_avRating.ToString().Substring(0, 3)) : _avRating;
                }
                return _avRating;
            }
        }


        public string ReviewString {
            get {
                if (this.RetailerTotalRatingVotes > 1)
                {
                    //int totalReviews = Vote.RetailerTotalRatingVotes - 1;
                    int totalReviews = (this.RetailerTotalRatingVotes ?? 0) - 1;

                    return string.Format("{0} review{1}", totalReviews, totalReviews > 1 ? "s" : "");
                }

                return string.Empty;
            }  
        }

        public string StoreTypeName
        {
            get { return storeTypeName; }
            set { storeTypeName = value; }
        }

        //public string RetailerLogoSmall {
        //    get {
        //        if (string.IsNullOrEmpty(this.LogoFile))
        //            return "";
        //        else {
        //            return Regex.Replace(this.LogoFile, @"([\s\S]+?)(.(jpg|gif|png))$", "$1_ms$2", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        //        }
        //    }
        //}

        public List<CSK_Store_RetailerReview> Reviews
        {
            get {
                return PriceMeDBStatic.PriceMeDB.CSK_Store_RetailerReviews.Where(rr => rr.RetailerId == this.RetailerId && rr.IsApproved == true).ToList();
            }
            set { _reviews = value; }
        }

        public int ReviewsCount
        {
            get
            {
                return Reviews.Count;
            }
            set { _reviewsCount = value; }
        }

        //public CSK_Store_RetailerVotesSum Vote
        //{
        //    get
        //    {
        //        CSK_Store_RetailerVotesSum vote = null;
        //        var vote_ = PriceMeDBStatic.PriceMeDB.CSK_Store_RetailerVotesSums.FirstOrDefault(v => v.RetailerID == this.RetailerId);
        //        if (vote_ != null)
        //        {
        //            vote = vote_;
        //            //vote.RetailerRatingSum -= 3;
        //            //vote.RetailerTotalRatingVotes =
        //            //    vote.RetailerTotalRatingVotes > 1 ? vote.RetailerTotalRatingVotes -= 1 : 1;
        //        }
        //        else
        //        {
        //            vote = new CSK_Store_RetailerVotesSum();
        //            vote.RetailerID = this.RetailerId;
        //            vote.RetailerRatingSum = 3;
        //            vote.RetailerTotalRatingVotes = 2;
        //        }
        //        return vote;
        //    }
        //    set
        //    {
        //        _vote = value;
        //    }
        //}

        public decimal Rating
        {
            get
            {
                //System.Data.IDataReader idr = null;
                //SubSonic.Schema.StoredProcedure sp = PriceMeDBStatic.PriceMeDB.CSK_Store_12RMB_GetRetailerRating();
                //sp.Command.AddParameter("@retailerID", this.RetailerId, System.Data.DbType.Int32);
                //idr = sp.ExecuteReader();
                //if (idr.Read())
                //{
                //    rating = string.IsNullOrEmpty(idr["OverallStoreRating"].ToString()) ? 0 : decimal.Parse(idr["OverallStoreRating"].ToString());
                //}
                //idr.Close();
                //return rating;
                
                //by huangrilng
                return AvRating;
            }
        }
    }
}
