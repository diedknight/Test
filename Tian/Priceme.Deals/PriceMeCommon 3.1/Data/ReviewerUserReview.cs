using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PriceMeCommon.Data
{
    public class ReviewerUserReview
    {
        private int _productID;

        public int ProductID
        {
            get { return _productID; }
            set { _productID = value; }
        }
        private int _categoryID;

        public int CategoryID
        {
            get { return _categoryID; }
            set { _categoryID = value; }
        }
        private int _rating;

        public int Rating
        {
            get { return _rating; }
            set { _rating = value; }
        }
        private string _body;

        public string Body
        {
            get { return _body; }
            set { _body = value; }
        }
        private string _authorName;

        public string AuthorName
        {
            get { return _authorName; }
            set { _authorName = value; }
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
        private string _postDate;

        public string PostDate
        {
            get { return _postDate; }
            set { _postDate = value; }
        }

        private string _title;

        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }

        private int _manufacturerID;

        public int ManufacturerID
        {
            get { return _manufacturerID; }
            set { _manufacturerID = value; }
        }

        private string _userName;

        public string UserName
        {
            get { return _userName; }
            set { _userName = value; }
        }
        private DateTime _postDate2;

        public DateTime PostDate2
        {
            get { return _postDate2; }
            set { _postDate2 = value; }
        }
    }
}