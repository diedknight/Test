using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PaymentToosCVS.Email
{
    public class AmazonEmail:IEmail
    {
        private string _aWSAccessKey = "";
        private string _aWSSecretKey = "";
        private string _address="";

        private List<Attachment> _attachments = new List<Attachment>();

        public AmazonEmail(string adress, string aWSAccessKey, string aWSSecretKey)
        {
            this._address = adress;
            this._aWSAccessKey = aWSAccessKey;
            this._aWSSecretKey = aWSSecretKey;
        }

        public void Send(string subject, string body, params string[] to)
        {
            if (this._attachments.Count != 0)
            {
                this.SendRawMail(subject, body, to);
                return;
            }

            AmazonSimpleEmailServiceClient ses = new AmazonSimpleEmailServiceClient(this._aWSAccessKey, this._aWSSecretKey);            

            SendEmailRequest seReq = new SendEmailRequest();
            seReq.Source = this._address;

            Destination det = new Destination();
            det.ToAddresses = to.ToList();

            seReq.Destination = det;

            Message mes = new Message();
            Content con = new Content();
            con.Data = subject;
            mes.Subject = con;            

            Body bodyObj = new Body();                      
            Content conHtml = new Content();                
            conHtml.Data = body;
            bodyObj.Text = conHtml;
            bodyObj.Html = conHtml;
            mes.Body = bodyObj;

            seReq.Message = mes;

            //list = new List<string>();
            //list.Add(ConfigAppString.ReplyToEmail);
            //seReq.ReplyToAddresses = list;
            
            seReq.ReplyToAddresses = new List<string>() { this._address };            

            ses.SendEmail(seReq);
        }


        private void SendRawMail(string subject, string body, params string[] to)
        {
            AlternateView htmlView = AlternateView.CreateAlternateViewFromString(body, Encoding.UTF8, "text/html");
            MailMessage mailMessage = new MailMessage();
            
            mailMessage.From = new MailAddress(this._address);
            to.ToList().ForEach(item => mailMessage.To.Add(new MailAddress(item)));

            mailMessage.Subject = subject;
            mailMessage.SubjectEncoding = Encoding.UTF8;

            mailMessage.AlternateViews.Add(htmlView);

            this._attachments.ForEach(item => mailMessage.Attachments.Add(item));

            RawMessage rawMessage = new RawMessage();

            using (MemoryStream memoryStream = this.ConvertMailMessageToMemoryStream(mailMessage))
            {
                rawMessage.WithData(memoryStream);
            }

            SendRawEmailRequest request = new SendRawEmailRequest();
            request.WithRawMessage(rawMessage);

            request.WithDestinations(to.ToList());
            request.WithSource(this._address);

            AmazonSimpleEmailServiceClient ses = new AmazonSimpleEmailServiceClient(this._aWSAccessKey, this._aWSSecretKey);

            ses.SendRawEmail(request);
        }

        public void addAttachment(System.IO.Stream s, string name)
        {
            this._attachments.Add(new Attachment(s, name));
        }


        private MemoryStream ConvertMailMessageToMemoryStream(MailMessage message)
        {
            Assembly assembly = typeof(SmtpClient).Assembly;
            Type mailWriterType = assembly.GetType("System.Net.Mail.MailWriter");
            MemoryStream fileStream = new MemoryStream();
            ConstructorInfo mailWriterContructor = mailWriterType.GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic, null, new[] { typeof(Stream) }, null);
            object mailWriter = mailWriterContructor.Invoke(new object[] { fileStream });
            MethodInfo sendMethod = typeof(MailMessage).GetMethod("Send", BindingFlags.Instance | BindingFlags.NonPublic);
            sendMethod.Invoke(message, BindingFlags.Instance | BindingFlags.NonPublic, null, new[] { mailWriter, true, true }, null);
            MethodInfo closeMethod = mailWriter.GetType().GetMethod("Close", BindingFlags.Instance | BindingFlags.NonPublic);
            closeMethod.Invoke(mailWriter, BindingFlags.Instance | BindingFlags.NonPublic, null, new object[] { }, null);
            return fileStream;
        }

    }
}
