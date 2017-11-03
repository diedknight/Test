using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Service.Infrastructure.Email
{
    public class XbaiEmail : IEmail
    {
        private List<Attachment> _attachments = new List<Attachment>();
        private MailMessage _mail = null;
        private SmtpClient _smtpClient = null;

        public XbaiEmail(string adress, string password, string displayName, string host)
        {
            this._mail = new MailMessage();
            this._mail.From = new MailAddress(adress, displayName, Encoding.UTF8);
            this._smtpClient = new SmtpClient();
            this._smtpClient.Credentials = new NetworkCredential(adress, password);
            this._smtpClient.Host = host;
        }

        public void Send(string subject, string body, params string[] to)
        {
            try
            {
                to.ToList<string>().ForEach(delegate(string item) { this._mail.To.Add(new MailAddress(item)); });

                this._mail.SubjectEncoding = Encoding.UTF8;
                this._mail.Subject = subject;
                this._mail.BodyEncoding = Encoding.UTF8;
                this._mail.Body = body;
                this._mail.IsBodyHtml = true;
                this._mail.Priority = MailPriority.High;
                
                this._attachments.ForEach(delegate(Attachment item) { this._mail.Attachments.Add(item); });
                this._smtpClient.Send(this._mail);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void addAttachment(Stream s, string name)
        {
            this._attachments.Add(new Attachment(s, name));
        }
    }
}
