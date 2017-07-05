using System;
using System.Collections.Generic;

namespace PriceMeCache
{
    public class ExpertReview
    {
        private int _reviewID;

        public int ReviewID
        {
            get { return _reviewID; }
            set { _reviewID = value; }
        }

        private int _productID;

        public int ProductID
        {
            get { return _productID; }
            set { _productID = value; }
        }

        private string _title;

        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }

        private string _description;

        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        private string _pros;

        public string Pros
        {
            get { return _pros; }
            set { _pros = value; }
        }

        private string _cons;

        public string Cons
        {
            get { return _cons; }
            set { _cons = value; }
        }

        private string _verdict;

        public string Verdict
        {
            get { return _verdict; }
            set { _verdict = value; }
        }

        private int _sourceID;
        public int SourceID
        {
            get { return _sourceID; }
            set { _sourceID = value; }
        }

        private string _reviewURL;

        public string ReviewURL
        {
            get { return _reviewURL; }
            set { _reviewURL = value; }
        }

        private string _reviewBy;

        public string ReviewBy
        {
            get { return _reviewBy; }
            set { _reviewBy = value; }
        }

        private DateTime _reviewDate;

        public DateTime ReviewDate
        {
            get { return _reviewDate; }
            set { _reviewDate = value; }
        }

        private float _priceMeScore;

        public float PriceMeScore
        {
            get { return _priceMeScore; }
            set { _priceMeScore = value; }
        }

        private string _webSiteName;

        public string WebSiteName
        {
            get { return _webSiteName; }
            set { _webSiteName = value; }
        }

        private string _logoFile;

        public string LogoFile
        {
            get { return _logoFile; }
            set { _logoFile = value; }
        }

        private bool _IsExpertReview;

        public bool IsExpertReview
        {
            get { return _IsExpertReview; }
            set { _IsExpertReview = value; }
        }
    }

}
