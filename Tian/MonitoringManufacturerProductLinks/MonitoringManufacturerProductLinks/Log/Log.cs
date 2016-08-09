using PaymentToosCVS.Email;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentToosCVS.Log
{
    public class Log : XbaiLog
    {
        private List<LogEntity> _list = null;

        public Log(string fileName)
        {
            string logPath = System.Configuration.ConfigurationManager.AppSettings["Log"];

            this._list = new List<LogEntity>();
            this.Init(Path.Combine(logPath, fileName));
        }

        public void Add(string url)
        {
            this._list.Add(new LogEntity() { Url = url });
        }

        public void Write()
        {
            this._list.ForEach(item => {
                this.WriteLine(item.Url);
            });
        }

        public void SendEmail()
        {
            string email_adress = "nacy@priceme.com";
            string email_password = "";
            string email_displayName = "Admin";
            string email_host = "127.0.0.1";
            string email_subject = "Monitoring Manufacturer Product Links";
            string[] email_to = System.Configuration.ConfigurationManager.AppSettings["Email"].Split(',');

            XbaiEmail email = new XbaiEmail(email_adress, email_password, email_displayName, email_host);

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<!DOCTYPE html>");
            sb.AppendLine("<html xmlns=\"http://www.w3.org/1999/xhtml\">");
            sb.AppendLine("<body>");
            sb.AppendLine("<table>");

            this._list.ForEach(item => {
                sb.AppendLine("<tr>");
                sb.AppendLine("<td>" + item.Url + "</td>");
                sb.AppendLine("</tr>");
            });

            sb.AppendLine("</table>");
            sb.AppendLine("</body>");
            sb.AppendLine("</html>");

            email.Send(email_subject, sb.ToString(), email_to);
        }

        private class LogEntity
        {
            public string Url { get; set; }            
        }

    }
}
