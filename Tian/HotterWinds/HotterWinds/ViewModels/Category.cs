using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HotterWinds.ViewModels
{
    public class CategoryV
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int ParentId { get; set; }
    }
}