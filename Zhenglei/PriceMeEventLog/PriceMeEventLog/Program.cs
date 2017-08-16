using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using LogWriter;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace PriceMeEventLog
{
    class Program
    {
        static string AccessKeyId_Static = ConfigurationManager.AppSettings["AWSAccessKey"];
        static string SecretAccessKeyId_Static = ConfigurationManager.AppSettings["AWSSecretKey"];

        private static void Main(string[] args)
        {
            int eventID = int.Parse(ConfigurationManager.AppSettings["EventID"]);
            int hours = int.Parse(ConfigurationManager.AppSettings["Hours"]);
            List<WebEventInfo> webEventInfoList = SystemEventController.GetWebEventInfoList(eventID, hours);
            WriteEventInfoToLog(webEventInfoList);
        }

        private static void WriteEventInfoToLog(List<WebEventInfo> webEventInfoList)
        {
            string filePath = Environment.CurrentDirectory + "\\log\\" + DateTime.Now.ToString("yyyy-MM-dd HH-mm") + ".txt";
            FileLogWriter.WriteLine(filePath, "Count : " + webEventInfoList.Count);
            FileLogWriter.WriteLine(filePath, "---------------------------------------------------------------------");
            List<string> list = new List<string>();
            int num = 1;
            using (List<WebEventInfo>.Enumerator enumerator = webEventInfoList.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    WebEventInfo current = enumerator.Current;
                    FileLogWriter.WriteLine(filePath, "No : " + num);
                    FileLogWriter.WriteLine(filePath, "Request URL : " + current.RequestURL);
                    FileLogWriter.WriteLine(filePath, "Application Virtual Path : " + current.ApplicationVirtualPath);
                    FileLogWriter.WriteLine(filePath, "Exception Type : " + current.ExceptionType);
                    FileLogWriter.WriteLine(filePath, "Exception Message : " + current.ExceptionMessage);
                    FileLogWriter.WriteLine(filePath, "Event Count : " + current.EventCount);
                    string text = "";
                    using (List<DateTime>.Enumerator enumerator2 = current.EventDateTimes.GetEnumerator())
                    {
                        while (enumerator2.MoveNext())
                        {
                            DateTime current2 = enumerator2.Current;
                            text = text + current2.ToString("yyyy-MM-dd HH:mm:ss") + ", ";
                        }
                    }
                    FileLogWriter.WriteLine(filePath, "Exception DateTimes : " + text);
                    FileLogWriter.WriteLine(filePath, "Stack Trace : " + current.StackTrace);
                    FileLogWriter.WriteLine(filePath, "---------------------------------------------------------------------");
                    string text2 = current.Ip.Trim();
                    if (!list.Contains(text2))
                    {
                        list.Add(text2);
                    }
                    num++;
                }
            }
            string text3 = "";
            using (List<string>.Enumerator enumerator3 = list.GetEnumerator())
            {
                while (enumerator3.MoveNext())
                {
                    string text2 = enumerator3.Current;
                    text3 = text3 + text2 + ", ";
                }
            }
            FileLogWriter.WriteLine(filePath, "All ip : " + text3);

            //filePath = @"E:\2017_08_02 17_08.csv";
            FileInfo fileInfo = new FileInfo(filePath);
            long len = fileInfo.Length / 1024;
            int maxFileSize = int.Parse(ConfigurationManager.AppSettings["MaxFileSize"]);
            
            if (len >= maxFileSize)
            {
                string emailAddress = ConfigurationManager.AppSettings["EmailAddress"];
                List<string> emailList = new List<string>();
                emailList.Add(emailAddress);
                DoSendEmail(emailList, null, "超过" + maxFileSize + "Kb了", "system event log", "");
            }
        }

        public static void DoSendEmail(List<string> toAddress, string replytoAddress, string eBody, string subject, string source)
        {
            if (string.IsNullOrEmpty(source))
            {
                source = "PriceMe <support@priceme.co.nz>";
            }

            AmazonSimpleEmailServiceClient ses = new AmazonSimpleEmailServiceClient(AccessKeyId_Static, SecretAccessKeyId_Static);

            Destination det = new Destination();
            det.ToAddresses = toAddress;

            Message mes = new Message();
            Amazon.SimpleEmail.Model.Content con = new Amazon.SimpleEmail.Model.Content();
            con.Data = subject;
            mes.Subject = con;
            string data = eBody;
            Body body = new Body();
            Amazon.SimpleEmail.Model.Content conHtml = new Amazon.SimpleEmail.Model.Content();
            conHtml.Data = data;
            body.Text = conHtml;
            body.Html = conHtml;
            mes.Body = body;

            var sendEmailRequest = new SendEmailRequest(source, det, mes);
            if (!string.IsNullOrEmpty(replytoAddress))
            {
                List<string> replyToAddressesList = new List<string>();
                replyToAddressesList.Add(replytoAddress);
                sendEmailRequest.ReplyToAddresses = replyToAddressesList;
            }

            ses.SendEmail(sendEmailRequest);
        }
    }
}