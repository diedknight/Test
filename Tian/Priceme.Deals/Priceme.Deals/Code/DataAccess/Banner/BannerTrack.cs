using System;
using System.Collections.Generic;
using System.Web;

namespace Commerce.Common.Banner
{
    /// <summary>
    ///BannerTrack 的摘要说明
    /// </summary>
    public class BannerTrack
    {
        public BannerTrack()
        {
            //
            //TODO: 在此处添加构造函数逻辑
            //
        }

        public int BannerTrackID;
        public int BannerOrderID;
        public string UserIP;
        public DateTime CreatedOn;
    }
}