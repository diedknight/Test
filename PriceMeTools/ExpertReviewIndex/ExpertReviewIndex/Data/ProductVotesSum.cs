using System;
using System.Collections.Generic;
using System.Text;

namespace ExpertReviewIndex.Data
{
    public class ProductVotesSum
    {
        int _id;
        public int Id
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
            }
        }

        int _productID;
        public int ProductID
        {
            get
            {
                return _productID;
            }
            set
            {
                _productID = value;
            }
        }

        int _productRatingSum;
        public int ProductRatingSum
        {
            get
            {
                return _productRatingSum;
            }
            set
            {
                _productRatingSum = value;
            }
        }

        int _productRatingVotes;
        public int ProductRatingVotes
        {
            get
            {
                return _productRatingVotes;
            }
            set
            {
                _productRatingVotes = value;
            }
        }
    }
}
