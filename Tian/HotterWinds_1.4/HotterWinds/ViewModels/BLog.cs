using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HotterWinds.ViewModels
{
    public class BLog
    {
        public string Title { get; set; }
        public string Link { get; set; }
        public DateTime PubDate { get; set; }
        public string Creator { get; set; }
        public string Description { get; set; }
        public int Comments { get; set; }
        public string ImgUrl { get; set; }
    }
}