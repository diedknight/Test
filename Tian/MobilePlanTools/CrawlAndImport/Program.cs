using System;
using System.IO;
using Common;
using Crawl;
using Import;
using SubSonic;
using SubSonic.Schema;
using System.Data;
using System.Text;
using System.Net.Mail;

//using SubSonic.Query;
//using SubSonic.Schema;
//using PriceMePlansDBA;

namespace CrawlAndImport
{
    class Program
    {
        static CrawlReportLog crlog;
        static void Main(string[] args)
        {
            CrawlReport();

            //Fetcher.BaseFetcher fetcher = new Fetcher.TelecomFetcher();
            //fetcher.GetMobilePlanInfoList();


            FetcherCrawler fetcherCrawler = new FetcherCrawler();
            fetcherCrawler.CrawlFinishedEvent += new CrawlFinishedEventArgs.OnCrawlFinished(fetcherCrawler_CrawlFinishedEvent);
            fetcherCrawler.CrawlAll(1, System.Threading.ThreadPriority.Normal);
            Console.WriteLine("Prepare send email...........................................");
            NewMobilePhoneNoticeEmail();
            InactiveNoticeEmail();
        }

        static void fetcherCrawler_CrawlFinishedEvent(object sender, CrawlFinishedEventArgs crawlFinishedEvent)
        {
            CrawlImport crawl = new CrawlImport(crawlFinishedEvent.EventCrawlResults.Fetcher.ProviderName, crlog);
            crawl.Import(crawlFinishedEvent);

            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine("ProviderName : " + crawlFinishedEvent.EventCrawlResults.Fetcher.ProviderName);
            Console.WriteLine("MobilePlanInfoFilePath : " + crawlFinishedEvent.EventCrawlResults.MobilePlanInfoFilePath);
            Console.WriteLine("MobilePlanInfoList count : " + crawlFinishedEvent.EventCrawlResults.MobilePlanInfoList.Count);
        }

        private static void CrawlReport()
        {
            string date = DateTime.Now.ToString("yyyyMMdd_HH");
            string path = AppConfig.ImportCrawlRootPath + @"\" + date;
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            crlog = new CrawlReportLog(path + @"\CrawlReport.txt", System.IO.FileMode.Create);
            crlog.WriteLine("Provider Name\t\tMatchCount\tNoMatchCount\tMobilePlanInfoList.Count");
        }

        /// <summary>
        /// 当有新的mobilephone,发送邮件
        /// </summary>
        private static void NewMobilePhoneNoticeEmail()
        {
            int weekDay = (int)DateTime.Today.DayOfWeek;
            Console.WriteLine(string.Format("AppConfig.InActiveWeekDay:\t{0}\r\nToday:{1}", AppConfig.InActiveWeekDay, weekDay));
            if (AppConfig.InActiveWeekDay != weekDay) { Console.WriteLine("Today not send New MobilePhone Report Email ..."); return; }

            var emailBody = string.Empty;
            try
            {
                //检查有没有邮件内容,没有则不发送邮件
                emailBody = GetPidNullPhones();
                if (string.IsNullOrEmpty(emailBody)) return;

                string emTo = AppConfig.EmailTo;
                string emFrom = AppConfig.InfoEmail;
                if (emFrom.Contains(";"))
                {
                    string[] emFroms = emFrom.Split(';');
                    emFrom = emFroms[0];
                }

                MailMessage emailMessage = new MailMessage();
                emailMessage.From = new MailAddress(emFrom, "New product from Mobile Plan Remind");
                string[] emTos = emTo.Split(';');
                foreach (var item in emTos)
                {
                    emailMessage.To.Add(new MailAddress(item));
                }
                emailMessage.ReplyTo = (new MailAddress(emFrom));
                emailMessage.IsBodyHtml = true;
                emailMessage.Subject = "New product from Mobile Plan Remind";
                emailMessage.Body = emailBody;
                SmtpClient smtpClient = new SmtpClient();
                smtpClient.Send(emailMessage);
                Console.WriteLine("New Mobile Phone Notice Email sent.");
            }
            catch (Exception ex)
            {
                crlog.WriteLine("");
                crlog.WriteLine("发邮件出错了:\t" + ex.Message);
                crlog.WriteLine(ex.StackTrace);
                crlog.WriteLine("");
                crlog.WriteLine("");
                crlog.WriteLine("New product from Mobile Plan Remind:");
                crlog.WriteLine(emailBody);
            }
        }

        /// <summary>
        /// 获取新的mobilephone
        /// </summary>
        /// <returns></returns>
        private static string GetPidNullPhones()
        {
            var sql = "SELECT ID,Name FROM CSK_Store_MobilePhone WHERE Pid IS NULL";
            Console.WriteLine("search new mobilephone:\t" + sql);
            StoredProcedure sp = new StoredProcedure("");
            sp.Command.CommandType = CommandType.Text;
            sp.Command.CommandSql = sql;
            sp.Command.CommandTimeout = 0;
            var ds = sp.ExecuteDataSet();
            StringBuilder str = new StringBuilder();
            if (ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows != null && ds.Tables[0].Rows.Count > 0)
            {
                Console.WriteLine("Got Pid is NULL  Phones:\t" + ds.Tables[0].Rows.Count);
                str.AppendLine("Pid is NULL Phones:<br/>ID&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Name<br/>");
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    str.AppendLine(string.Format("{0}&nbsp;&nbsp;&nbsp;{1}<br/>", row["ID"], row["Name"]));
                }
                return str.ToString();
            }
            else Console.WriteLine("Pid is NULL Phones:NULL; not send email.");
            return "";
        }

        /// <summary>
        /// 如果小于AppConfig.InActiveRate 则看下没有更新的产品的modifiedon时间距当天是否有AppConfig.InActiveDay. 
        /// 如果小于AppConfig.InActiveDay 则不修改数据库，只发邮件。 
        /// </summary>
        private static void InactiveNoticeEmail()
        {
            var emailBody = string.Empty;
            try
            {
                //检查有没有邮件内容,没有则不发送邮件
                emailBody += crlog.PlanEmailBody.ToString();
                var emailBody_ = crlog.MapEmailBody.ToString();
                if (!string.IsNullOrEmpty(emailBody_))
                    emailBody_ = "<br /><br /><br />" + emailBody_;
                emailBody += emailBody_;
                if (string.IsNullOrEmpty(emailBody)) 
                {
                    Console.WriteLine("no inactive record email content, not send email.");
                    return; 
                }

                string emTo = AppConfig.EmailTo;
                string emFrom = AppConfig.InfoEmail;
                if (emFrom.Contains(";"))
                {
                    string[] emFroms = emFrom.Split(';');
                    emFrom = emFroms[0];
                }

                MailMessage emailMessage = new MailMessage();
                emailMessage.From = new MailAddress(emFrom, "No Update 70% MobilePlan & MobilePlanPhoneMap");
                string[] emTos = emTo.Split(';');
                foreach (var item in emTos)
                {
                    emailMessage.To.Add(new MailAddress(item));
                }
                emailMessage.ReplyTo = (new MailAddress(emFrom));
                emailMessage.IsBodyHtml = true;
                emailMessage.Subject = "No Update 70% MobilePlan & MobilePlanPhoneMap";
                emailMessage.Body = emailBody;
                SmtpClient smtpClient = new SmtpClient();
                smtpClient.Send(emailMessage);

                Console.WriteLine("Inactive Email sent.");
            }
            catch (Exception ex)
            {
                crlog.WriteLine("");
                crlog.WriteLine("发邮件出错了:\t" + ex.Message);
                crlog.WriteLine(ex.StackTrace);
                crlog.WriteLine("");
                crlog.WriteLine("");
                crlog.WriteLine("No Update 70% MobilePlan & MobilePlanPhoneMap");
                crlog.WriteLine(emailBody);
            }
        }
    }
}
