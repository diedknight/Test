using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PriceMeCommon.Data
{
    public class PagerInfo
    {
        int _currentPageIndex;
        int _pageCount;
        int _pageSize;
        int _startIndex;
        int _endIndex;
        private int _resultsCount;

        public int EndIndex
        {
            get { return _endIndex; }
        }

        public int StartIndex
        {
            get { return _startIndex; }
        }

        public int PageSize
        {
            get { return _pageSize; }
        }

        public int CurrentPageIndex
        {
            get { return _currentPageIndex; }
        }

        public int PageCount
        {
            get { return _pageCount; }
        }

        public int ResultsCount
        {
            get { return _resultsCount; }
        }

        public PagerInfo(int pageIndex, int pageSize, int resultsCount)
        {
            if (pageIndex < 1)
            {
                pageIndex = 1;
            }

            _resultsCount = resultsCount;

            int startIndex = (pageIndex - 1) * pageSize;
            int endIndex = startIndex + pageSize;
            if (endIndex > resultsCount)
            {
                endIndex = resultsCount;
                if (startIndex > endIndex)
                {
                    startIndex = endIndex - pageSize;
                }
                if (startIndex < 0)
                {
                    startIndex = 0;
                }
            }

            int pageCount = resultsCount / pageSize;
            if (resultsCount % pageSize > 0)
            {
                pageCount++;
            }

            this._currentPageIndex = pageIndex;
            this._pageSize = pageSize;
            this._pageCount = pageCount;
            this._startIndex = startIndex;
            this._endIndex = endIndex;
        }
    }
}