using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PriceMeCache
{
    public class NewExpertReview
    {
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

        private string _summary;

        public string Summary
        {
            get { return _summary; }
            set { _summary = value; }
        }

        private string _verdict;

        public string Verdict
        {
            get { return _verdict; }
            set { _verdict = value; }
        }

        private float _priceMeScore;

        public float PriceMeScore
        {
            get { return _priceMeScore; }
            set { _priceMeScore = value; }
        }

        private int _alascore;

        public int Alascore
        {
            get { return _alascore; }
            set { _alascore = value; }
        }

        private string _productName;

        public string ProductName
        {
            get { return _productName; }
            set { _productName = value; }
        }

        private string _defaultImage;

        public string DefaultImage
        {
            get { return _defaultImage; }
            set { _defaultImage = value; }
        }

        private int _reviewCount;

        public int ReviewCount
        {
            get { return _reviewCount; }
            set { _reviewCount = value; }
        }
    }
}
