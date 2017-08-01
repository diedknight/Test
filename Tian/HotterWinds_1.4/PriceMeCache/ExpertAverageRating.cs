using System;
using System.Collections.Generic;

namespace PriceMeCache
{
    [Serializable]
    public class ExpertAverageRating
    {
        private int _expertAverageRatingID;

        public int ExpertAverageRatingID
        {
            get { return _expertAverageRatingID; }
            set { _expertAverageRatingID = value; }
        }
        private int _productID;

        public int ProductID
        {
            get { return _productID; }
            set { _productID = value; }
        }
        private float _averageRating;

        public float AverageRating
        {
            get { return _averageRating; }
            set { _averageRating = value; }
        }
        private int _countryID;

        public int CountryID
        {
            get { return _countryID; }
            set { _countryID = value; }
        }
        private int _votes;

        public int Votes
        {
            get { return _votes; }
            set { _votes = value; }
        }
        private int _votesHasScore;

        public int VotesHasScore
        {
            get { return _votesHasScore; }
            set { _votesHasScore = value; }
        }
    }
}