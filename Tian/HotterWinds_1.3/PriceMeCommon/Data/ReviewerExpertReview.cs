using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PriceMeCommon.Data
{
    public class ReviewerExpertReview
    {
        private int _productID;

        public int ProductID
        {
            get { return _productID; }
            set { _productID = value; }
        }
        private double _priceMeScore;

        public double PriceMeScore
        {
            get { return _priceMeScore; }
            set { _priceMeScore = value; }
        }
        private int _retailerCountry;

        public int RetailerCountry
        {
            get { return _retailerCountry; }
            set { _retailerCountry = value; }
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
        private string _Description;

        public string Description
        {
            get { return _Description; }
            set { _Description = value; }
        }

        private string _Title;

        public string Title
        {
            get { return _Title; }
            set { _Title = value; }
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
        private string _sourceID;

        public string SourceID
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
        private string _reviewDate;

        public string ReviewDate
        {
            get { return _reviewDate; }
            set { _reviewDate = value; }
        }
        private int _categoryID;

        public int CategoryID
        {
            get { return _categoryID; }
            set { _categoryID = value; }
        }

        private int _manufacturerID;

        public int ManufacturerID
        {
            get { return _manufacturerID; }
            set { _manufacturerID = value; }
        }
    }
}
