using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpcomingProductAlert.Email
{
    public class AWSEmail
    {
        private string _aWSAccessKey = "";
        private string _aWSSecretKey = "";

        private List<string> _toList = new List<string>();
        private List<string> _replyToList = new List<string>();
        private List<string> _ccList = new List<string>();
        private List<string> _bccList = new List<string>();

        public string Subject { get; set; }
        public string Body { get; set; }

        public string FromEmail { get; private set; }
        public string FromName { get;private set; }

        public AWSEmail(string fromEmail, string fromName)
        {
            this._aWSAccessKey = System.Configuration.ConfigurationManager.AppSettings["WSAccessKey"];
            this._aWSSecretKey = System.Configuration.ConfigurationManager.AppSettings["WSSecretKey"];

            this.FromEmail = fromEmail;
            this.FromName = fromName;
        }

        public void AddToAddress(string address)
        {
            if (string.IsNullOrEmpty(address)) return;

            this._toList.Add(address);
        }

        public void AddReplyAddress(string address)
        {
            if (string.IsNullOrEmpty(address)) return;

            this._replyToList.Add(address);
        }

        public void AddCCAddress(string address)
        {
            if (string.IsNullOrEmpty(address)) return;

            this._ccList.Add(address);
        }

        public void AddBCCAddress(string address)
        {
            if (string.IsNullOrEmpty(address)) return;

            this._bccList.Add(address);
        }

        public void Send()
        {
            AmazonSimpleEmailServiceClient ses = new AmazonSimpleEmailServiceClient(this._aWSAccessKey, this._aWSSecretKey);

            SendEmailRequest seReq = new SendEmailRequest();
            seReq.Source = this.FromEmail;
            if (!string.IsNullOrEmpty(this.FromName)) seReq.Source = this.FromName + " <" + this.FromEmail + ">";
            

            Destination det = new Destination();
            det.ToAddresses = this._toList;
            det.CcAddresses = this._ccList;
            det.BccAddresses = this._bccList;

            seReq.Destination = det;

            Message mes = new Message();
            Content con = new Content();
            con.Data = this.Subject;
            mes.Subject = con;

            Body bodyObj = new Body();
            Content conHtml = new Content();
            conHtml.Data = this.Body;
            bodyObj.Text = conHtml;
            bodyObj.Html = conHtml;
            mes.Body = bodyObj;
            
            seReq.Message = mes;

            seReq.ReplyToAddresses = this._replyToList;

            //list = new List<string>();
            //list.Add(ConfigAppString.ReplyToEmail);
            //seReq.ReplyToAddresses = list;            

            ses.SendEmail(seReq);
        }

    }
}
