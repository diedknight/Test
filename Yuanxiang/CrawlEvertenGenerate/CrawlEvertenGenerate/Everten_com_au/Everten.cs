using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CopyFile;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;

namespace CrawlEvertenGenerate.Everten_com_au
{
    public static class Everten
    {
        static string msg;
        static string file;
        static decimal filelength;

        public static void EvertenLoad()
        {
            string stringSubject = string.Empty;

            msg += "begin......" + DateTime.Now + "<br/>";
            System.Console.WriteLine("begin......" + DateTime.Now);

            msg += "run EvertenFetcher......" + DateTime.Now + "<br/>";
            System.Console.WriteLine("run EvertenFetcher......" + DateTime.Now);
            //Run EvertenFetcher
            List<ProductItem> ps = new List<ProductItem>();
            try
            {
                EvertenFetcher ef = new EvertenFetcher();
                ps = ef.GetProducts();

                file = ef.InfoFile;

                if (File.Exists(file))
                {
                    long lSize = new FileInfo(file).Length;
                    CountSize(lSize);
                }

                msg += "get " + ps.Count + " EvertenFetcher......" + DateTime.Now + "<br/>";
                System.Console.WriteLine("get " + ps.Count + " EvertenFetcher......" + DateTime.Now);

                //msg += "Create csv......" + DateTime.Now + "<br/>";
                //System.Console.WriteLine("Create csv......" + DateTime.Now);
                //Create csv
                //CreateCsv(ps);

                msg += "Create "+ filelength + "KB csv successful......" + DateTime.Now + "<br/>";
                System.Console.WriteLine("Create " + filelength + "KB csv successful......" + DateTime.Now);

                //Send FTP
                msg += "Send FTP......" + DateTime.Now + "<br/>";
                System.Console.WriteLine("Send FTP......" + DateTime.Now);
                decimal FileKB = 0;
                decimal.TryParse(System.Configuration.ConfigurationManager.AppSettings["FileKB"].ToString(), out FileKB);
                if (filelength > FileKB)
                    SendFTP();

                stringSubject = "Crawl Everten and generate CSV file ---------- Success<br/>";
            }
            catch (Exception ex)
            {
                msg += ex.Message + "</br>" + ex.StackTrace + "</br>";
                stringSubject = "Crawl Everten and generate CSV file ---------- Error<br/>";
            }

            msg += "Send Email......" + DateTime.Now + "<br/>";
            System.Console.WriteLine("Send Email......" + DateTime.Now);
            //Send Email            
            AmazonEmail(msg, stringSubject);

            System.Console.WriteLine("End......" + DateTime.Now);
        }

        private static void CountSize(long Size)
        {
            string m_strSize = "";
            long FactSize = 0;
            FactSize = Size;
            m_strSize = (FactSize / 1024.00).ToString("F2");
            decimal.TryParse(m_strSize, out filelength);
        }

        private static void SendFTP()
        {
            string userID = System.Configuration.ConfigurationManager.AppSettings["userid_FTP"];
            string password = System.Configuration.ConfigurationManager.AppSettings["password_FTP"];
            string targetIP = System.Configuration.ConfigurationManager.AppSettings["targetIP_FTP"];
            string targetPath = System.Configuration.ConfigurationManager.AppSettings["targetPath_FTP"];
            CopyFile.FtpCopy.UploadFileSmall(file, targetPath, targetIP, userID, password);
        }

        private static bool AmazonEmail(string stringBody, string stringSubject)
        {
            string AWSAccessKey = System.Configuration.ConfigurationManager.AppSettings["AWSAccessKey"].ToString();
            string AWSSecretKey = System.Configuration.ConfigurationManager.AppSettings["AWSSecretKey"].ToString();
            string stringEmail = System.Configuration.ConfigurationManager.AppSettings["AdminEmail"].ToString();

            AmazonSimpleEmailServiceClient ses = new AmazonSimpleEmailServiceClient(AWSAccessKey, AWSSecretKey);

            string from = "PriceMe Admin <info@priceme.co.nz>";
            SendEmailRequest seReq = new SendEmailRequest();
            seReq.Source = from;

            string[] emails = stringEmail.Split(',');
            List<string> list = new List<string>();
            for (int i = 0; i < emails.Length; i++)
            {
                list.Add(emails[i]);
            }

            Destination det = new Destination();
            det.ToAddresses = list;

            seReq.Destination = det;

            Message mes = new Message();
            Content con = new Content();
            con.Data = stringSubject;
            mes.Subject = con;

            Body body = new Body();
            Content conHtml = new Content();
            conHtml.Data = stringBody;
            body.Text = conHtml;
            body.Html = conHtml;
            mes.Body = body;

            seReq.Message = mes;

            list = new List<string>() { from };

            seReq.ReplyToAddresses = list;

            ses.SendEmail(seReq);
            return true;
        }
    }
}
