using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PriceMeCommon.Data
{
    public class SearchResult
    {
        List<ProductCatalog> _productCatalogList = new List<ProductCatalog>();
        int _currentPageIndex;
        int _pageCount;
        int _productCount;

        public int ProductCount
        {
            get { return _productCount; }
            set { _productCount = value; }
        }

        public int PageCount
        {
            get { return _pageCount; }
            set { _pageCount = value; }
        }

        public int CurrentPageIndex
        {
            get { return _currentPageIndex; }
            set { _currentPageIndex = value; }
        }

        public List<ProductCatalog> ProductCatalogList
        {
            get { return _productCatalogList; }
            set { _productCatalogList = value; }
        }
    }

    public class SearchResult<T>
    {
        List<T> _resultList = new List<T>();
        int _currentPageIndex;
        int _pageCount;
        int _productCount;

        public int ProductCount
        {
            get { return _productCount; }
            set { _productCount = value; }
        }

        public int PageCount
        {
            get { return _pageCount; }
            set { _pageCount = value; }
        }

        public int CurrentPageIndex
        {
            get { return _currentPageIndex; }
            set { _currentPageIndex = value; }
        }

        public List<T> ResultList
        {
            get { return _resultList; }
            set { _resultList = value; }
        }
    }
}