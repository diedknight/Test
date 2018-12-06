using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChristmasSite.Code
{
    public static class PageExtension
    {
        public static Pagination CreatePagination(string pageUrl, int pageIndex, int pageSize = 60)
        {
            return new Pagination(pageUrl, pageIndex, pageSize);
        }
    }
}
