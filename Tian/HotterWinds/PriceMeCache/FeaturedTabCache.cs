using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PriceMeCache
{
    public class FeaturedTabCache
    {
        private int _CategoryID;

        public int CategoryID
        {
            get { return _CategoryID; }
            set { _CategoryID = value; }
        }
        private string _Label;

        public string Label
        {
            get { return _Label; }
            set { _Label = value; }
        }
        private string _Title;

        public string Title
        {
            get { return _Title; }
            set { _Title = value; }
        }
        private int _ListOrder;

        public int ListOrder
        {
            get { return _ListOrder; }
            set { _ListOrder = value; }
        }

        private List<FeaturedProduct> _FeaturedProductList;

        public List<FeaturedProduct> FeaturedProductList
        {
            get { return _FeaturedProductList; }
            set { _FeaturedProductList = value; }
        }
    }

    public class FeaturedProduct
    {
        private int _ProductID;

        public int ProductID
        {
            get { return _ProductID; }
            set { _ProductID = value; }
        }
        private string _ProductName;

        public string ProductName
        {
            get { return _ProductName; }
            set { _ProductName = value; }
        }
        private string _DefaultImage;

        public string DefaultImage
        {
            get { return _DefaultImage; }
            set { _DefaultImage = value; }
        }
        private int _CategoryID;

        public int CategoryID
        {
            get { return _CategoryID; }
            set { _CategoryID = value; }
        }
        private int _RootID = 0;

        public int RootID
        {
            get { return _RootID; }
            set { _RootID = value; }
        }
        private double _MinPrice;

        public double MinPrice
        {
            get { return _MinPrice; }
            set { _MinPrice = value; }
        }
        private string _ProductGUID;

        public string ProductGUID
        {
            get { return _ProductGUID; }
            set { _ProductGUID = value; }
        }
        private double _MaxPrice;

        public double MaxPrice
        {
            get { return _MaxPrice; }
            set { _MaxPrice = value; }
        }
    }
}
