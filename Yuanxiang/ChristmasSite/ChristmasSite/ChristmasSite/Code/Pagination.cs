﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChristmasSite.Code
{
    public class Pagination
    {
        public UrlParam UrlParam { get; private set; }

        public int CurPage { get; private set; }
        public int PageSize { get; private set; }
        public int PageCount { get; private set; }

        public int Amount { get; private set; }

        public Pagination(string pageUrl, int pageIndex, int pageSize)
        {
            this.UrlParam = new UrlParam(pageUrl);

            this.PageSize = pageSize;
            if (this.PageSize <= 0) this.PageSize = 20;

            this.CurPage = pageIndex;
            if (this.CurPage <= 0) this.CurPage = 1;
        }

        public void Init(int amount)
        {
            this.Amount = amount;

            this.PageCount = this.Amount / this.PageSize;
            if (this.Amount % this.PageSize > 0) this.PageCount += 1;

            if (this.PageCount == 0) this.PageCount = 1;
            if (this.CurPage > this.PageCount) this.CurPage = this.PageCount;
        }

        public string Render()
        {
            return new PageHtmlRender(this).Render();
        }

    }
}
