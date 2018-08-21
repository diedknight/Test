using System;
using System.Collections.Generic;
using System.Text;

namespace ExpertReviewIndex.Data
{
    public class ReviewAverage
    {
        private int _productID;
        public int ProductID
        {
            get { return _productID; }
            set { _productID = value; }
        }

        private int _userReviewCount;
        public int UserReviewCount
        {
            get { return _userReviewCount; }
            set { _userReviewCount = value; }
        }

        private int _expertReviewCount;
        public int ExpertReviewCount
        {
            get { return _expertReviewCount; }
            set { _expertReviewCount = value; }
        }

        private double _productRating;
        public double ProductRating
        {
            get { return _productRating; }
            set { _productRating = value; }
        }

        private string _FeatureScore;

        public string FeatureScore
        {
            get { return _FeatureScore; }
            set { _FeatureScore = value; }
        }

        private double _TFEAverageRating;
        public double TFEAverageRating
        {
            get { return _TFEAverageRating; }
            set { _TFEAverageRating = value; }
        }

        private double _TFUAverageRating;
        public double TFUAverageRating
        {
            get { return _TFUAverageRating; }
            set { _TFUAverageRating = value; }
        }
    }
}
