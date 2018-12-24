﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChristmasSite.Code
{
    public class PageHtmlRender
    {
        private Pagination _pagination = null;
        private LinkRange _linkRange = null;
        private NumRange _numRange = null;

        public PageHtmlRender(Pagination pagination)
        {
            this._pagination = pagination;
            this._linkRange = new LinkRange(this._pagination.CurPage, this._pagination.PageCount, 5);
            this._numRange = new NumRange(this._pagination.CurPage, this._pagination.PageSize, this._pagination.PageCount, this._pagination.Amount);
        }

        public string Render()
        {
            if (this._pagination.Amount == 0) return "";

            string resultNode = $"<li class=\"showingLI\">Showing {this._numRange.Start} - {this._numRange.End} of {this._numRange.Amount} results</li> ";

            string firstNode = $"<li><a href=\"{this.GetUrl(1)}\" rel=\"nofollow\" class=\"pagerAtag pagerAHidden\">First</a></li>";
            string prevNode = $"<li><a href=\"{this.GetUrl(this._linkRange.Prev)}\" class=\"pagerAtag\">&lt; Prev</a></li>";

            string nextNode = $"<li><a href=\"{this.GetUrl(this._linkRange.Next)}\" rel=\"nofollow\" class=\"pagerAtag\">Next &gt;</a></li>";
            string lastNode = $"<li><a href=\"{this.GetUrl(this._pagination.PageCount)}\" rel=\"nofollow\" class=\"pagerAtag pagerAHidden\">Last</a></li>";

            string pageNodes = "";
            for (int i = this._linkRange.First; i <= this._linkRange.Last; i++)
            {
                if (i == this._linkRange.Current)
                {
                    pageNodes += $"<li class=\"active\"><a>{i}</a></li>";
                    continue;
                }

                pageNodes += $"<li><a class=\"pagerAHidden\" href=\"{this.GetUrl(i)}\">{i}</a></li>";
            }

            if (this._pagination.CurPage == 1)
            {
                firstNode = "";
                prevNode = "";
            }

            if (this._pagination.CurPage == this._pagination.PageCount)
            {
                nextNode = "";
                lastNode = "";
            }

            if (this._pagination.PageCount == 1)
            {
                firstNode = "";
                prevNode = "";

                pageNodes = "";

                nextNode = "";
                lastNode = "";
            }

            string wrapNode = $"<ul class=\"pagination PrettyPagerDiv\">{firstNode + prevNode + pageNodes + nextNode + lastNode + resultNode}</ul>";

            return wrapNode;
        }

        private string GetUrl(int pageNum)
        {
            this._pagination.UrlParam.SetParam("pg", pageNum.ToString());

            if (pageNum <= 1) this._pagination.UrlParam.RemoveParam("pg");

            return this._pagination.UrlParam.GetUrl();
        }

        //class

        private class NumRange
        {
            public int Start { get; private set; }
            public int End { get; private set; }
            public int Amount { get; private set; }

            public NumRange(int curPage, int pageSize, int pageCount, int amount)
            {
                this.Amount = amount;

                if (curPage == 1)
                {
                    this.Start = 1;
                    this.End = amount > pageSize ? pageSize : amount;
                    return;
                }

                if (curPage == pageCount)
                {
                    this.Start = (curPage - 1) * pageSize + 1;
                    this.End = amount;
                    return;
                }

                this.Start = (curPage - 1) * pageSize + 1;
                this.End = curPage * pageSize;
            }

        }

        private class LinkRange
        {
            public int First { get; private set; }

            public int Prev { get; private set; }
            public int Current { get; private set; }
            public int Next { get; private set; }
            public int Last { get; private set; }

            public int ShowRange { get; private set; }
            public int PageCount { get; private set; }

            public LinkRange(int curPage, int pageCount, int showRange)
            {
                this.Current = curPage > 0 ? curPage : 1;
                this.ShowRange = showRange > 0 ? showRange : 1;
                this.PageCount = pageCount > 0 ? pageCount : 1;

                this.Prev = this.Current - 1;
                if (this.Prev < 1) this.Prev = 1;

                this.Next = this.Current + 1;
                if (this.Next > this.PageCount) this.Next = this.PageCount;

                if (this.ShowRange == 1 || this.PageCount == 1)
                {
                    this.First = this.Current;
                    this.Last = this.Current;
                    return;
                }

                if (this.Current < this.ShowRange)
                {
                    this.First = 1;
                    this.Last = this.PageCount > this.ShowRange ? this.ShowRange : this.PageCount;
                    return;
                }

                int tempRange = this.ShowRange;
                tempRange = (tempRange % 2 != 0) ? tempRange - 1 : tempRange;

                this.First = this.Current;
                this.Last = this.Current;

                while (tempRange > 0)
                {
                    if (this.Last < this.PageCount)
                    {
                        this.Last += 1;
                        tempRange -= 1;
                    }

                    if (this.First > 1)
                    {
                        this.First -= 1;
                        tempRange -= 1;
                    }

                }

            }
        }

    }
}