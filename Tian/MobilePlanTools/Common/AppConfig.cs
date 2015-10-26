using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Common
{
    public static class AppConfig
    {
        public readonly static string FetcherFileRootPath = ConfigurationManager.AppSettings["FetcherFileRootPath"];
        public readonly static string CrawlLogFileRootPath = ConfigurationManager.AppSettings["CrawlLogFileRootPath"];
        public readonly static string ImportCarrierRootPath = ConfigurationManager.AppSettings["ImportProviderRootPath"];
        public readonly static string ImportCrawlRootPath = ConfigurationManager.AppSettings["ImportCrawlRootPath"];
        public readonly static bool IsUpdate = Convert.ToBoolean(ConfigurationManager.AppSettings["IsUpdate"]);
        /// <summary>
        /// 每次更新后,把InActiveHour小时内没有更新的设置为inactive
        /// </summary>
        public readonly static string InActiveHour = ConfigurationManager.AppSettings["InActiveHour"];
        /// <summary>
        /// 是否要设置inactive的比率
        /// 把产品设置inactive时先计算 modifiedon为当天产品数量在这个provider的比例
        /// 如果超过,则可以放inactive
        /// </summary>
        public readonly static decimal InActiveRate = decimal.Parse(ConfigurationManager.AppSettings["InActiveRate"]);
        /// <summary>
        /// 是否要设置inactive的天数
        /// 如果小于InActiveRate 则看没有更新的产品的modifiedon时间距当天是否有InActiveDay. 
        /// 如果小于InActiveDay 则不修改数据库，只发邮件。 
        /// 如果大于=InActiveDay 则放这些没有更新的产品为inactive
        /// </summary>
        public readonly static int InActiveDay = int.Parse(ConfigurationManager.AppSettings["InActiveDay"]);
        /// <summary>
        /// 星期几发送CrawReport文件到设置的邮箱
        /// </summary>
        public readonly static int InActiveWeekDay = int.Parse(ConfigurationManager.AppSettings["InActiveWeekDay"]);
        /// <summary>
        /// 发送CrawReport文件的邮箱
        /// </summary>
        public readonly static string InfoEmail = ConfigurationManager.AppSettings["InfoEmail"];
        /// <summary>
        /// 接收CrawReport文件的邮箱
        /// </summary>
        public readonly static string EmailTo = ConfigurationManager.AppSettings["EmailTo"];
    }
}
