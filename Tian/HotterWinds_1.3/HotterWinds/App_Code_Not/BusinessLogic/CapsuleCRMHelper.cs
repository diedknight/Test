using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;

namespace PriceMe
{
    /// <summary>
    /// Summary description for CapsuleCRMHelper
    /// api doc: http://developer.capsulecrm.com/v1/writing/
    /// </summary>
    public class CapsuleCRMHelper
    {
        /// <summary>
        /// Create new organisation and person
        /// </summary>
        /// <param name="organisation"></param>
        /// <param name="website"></param>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="email"></param>
        /// <param name="gstNumber"></param>
        /// <param name="pid">return new person id</param>
        /// <returns>new organisation id</returns>
        public static string NewOrganisationAndPerson(string organisation, string website, string firstName, string lastName, string email, string gstNumber, out string pid, out string hid)
        {
            var oid = NewOrganisation(organisation, website, out hid);
            if (!string.IsNullOrEmpty(oid))
            {
                pid = NewPersonToOrganisation(oid, firstName, lastName, email);
                NewOpportunityToOrganisation(oid, organisation);
                SetOrganisationGSTNumber(oid, gstNumber);
                var admin = "Jasseff";
                var adminEmail = "jasseff@priceme.com";
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["ReceiveCRMMailAdmin"]))
                    admin = ConfigurationManager.AppSettings["ReceiveCRMMailAdmin"];
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["ReceiveCRMMailAdminEAddress"]))
                    adminEmail = ConfigurationManager.AppSettings["ReceiveCRMMailAdminEAddress"];
                var person = (firstName+" "+lastName).Trim();
                SendCRMNotifierEmail(admin, adminEmail, organisation, email, person, firstName, lastName, website, Resources.Resource.Country, oid);
            }
            else pid = string.Empty;

            return oid;
        }

        /// <summary>
        /// Update person jobTitle, update history note, feedURL to organisation
        /// </summary>
        /// <param name="hid"></param>
        /// <param name="pid"></param>
        /// <param name="note"></param>
        /// <param name="jobTitle"></param>
        /// <param name="feedURL"></param>
        /// <returns></returns>
        public static string UpdatePersonJobTitleAndHistoryNote(int oid, int hid, int pid, string note, string jobTitle, string feedURL)
        {
            try
            {
                if (hid > 0 && !string.IsNullOrEmpty(note))
                {
                    UpdateHistoryNote(hid, note);
                }
                if (pid > 0 && !string.IsNullOrEmpty(jobTitle))
                {
                    UpdateJobTitleToPerson(pid, jobTitle);
                }
                if (oid > 0)
                {
                    SetOrganisationFeedURL(oid, feedURL);
                }
            }
            catch (Exception ex)
            {

            }
            return pid.ToString();
        }

        /// <summary>
        /// Create a new Organisation to crm
        /// </summary>
        /// <param name="name"></param>
        /// <param name="website"></param>
        /// <returns></returns>
        public static string NewOrganisation(string name, string website, out string hid)
        {
            var id = string.Empty;

            try
            {
                var postData = string.Format("<organisation><contacts><address><type>Office</type><country>{1}</country></address></contacts><name>{0}</name></organisation>",
                    name, Resources.Resource.Country);
                var output = RestClient.Request(HttpVerb.POST, postData, "organisation");

                if (!string.IsNullOrEmpty(output))
                {
                    var index = "party/";
                    id = output.Substring(output.IndexOf(index) + index.Length);
                    SetOrganisationWebSite(id, website);
                    hid = NewOrganisationHistoryNote(int.Parse(id), "New");
                }
                else hid = string.Empty;
            }
            catch (Exception ex)
            {
                hid = string.Empty;
                Console.WriteLine(ex.Message + ex.StackTrace);
            }

            return id;
        }

        /// <summary>
        /// record custom field of organisation
        /// Custom Field[Web Site URL]
        /// Custom Field[Validity]
        /// </summary>
        /// <param name="id"></param>
        /// <param name="website"></param>
        /// <param name="valid"></param>
        /// <returns></returns>
        public static string SetOrganisationWebSite(string oid, string website)
        {
            try
            {
                var postData = string.Format("<customFields><customField><label>{0}</label><text>{1}</text></customField><customField><label>{2}</label><boolean>{3}</boolean></customField></customFields>",
                    "Web Site URL", website, "Validity", WebConfig.Environment == "dev" ? false : true);
                var paramter = string.Format("party/{0}/customfields", oid);
                var output = RestClient.Request(HttpVerb.POST, postData, paramter);

                return output;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ex.StackTrace);
                return string.Empty;
            }
        }

        /// <summary>
        /// record custom field of organisation
        /// Custom Field[GstNumber]
        /// </summary>
        /// <param name="oid"></param>
        /// <param name="gstNumber"></param>
        /// <returns></returns>
        public static string SetOrganisationGSTNumber(string oid, string gstNumber)
        {
            try
            {
                int oid_ = 0;
                int.TryParse(oid,out oid_);
                if (oid_ > 0 && !string.IsNullOrEmpty(gstNumber))
                {
                    var postData = new StringBuilder();
                    postData.AppendFormat("<customField><label>{0}</label><text>{1}</text></customField>",
                    "GstNumber", gstNumber);
                    var postData_ = string.Format("<customFields>{0}</customFields>", postData);
                    var paramter = string.Format("party/{0}/customfields", oid);
                    var output = RestClient.Request(HttpVerb.POST, postData_, paramter);

                    return output;
                }
                else
                    return string.Empty;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ex.StackTrace);
                return string.Empty;
            }
        }

        /// <summary>
        /// record custom field of organisation
        /// Custom Field[FeedURL]
        /// </summary>
        /// <param name="oid"></param>
        /// <param name="feedURL"></param>
        /// <returns></returns>
        public static string SetOrganisationFeedURL(int oid, string feedURL)
        {
            try
            {
                var postData = new StringBuilder();
                if (!string.IsNullOrEmpty(feedURL))
                {
                    postData.AppendFormat("<customField><label>{0}</label><text>{1}</text></customField>",
                    "FeedURL", feedURL);
                }
                if (postData.Length > 0)
                {
                    var postData_ = string.Format("<customFields>{0}</customFields>", postData);
                    var paramter = string.Format("party/{0}/customfields", oid);
                    var output = RestClient.Request(HttpVerb.POST, postData_, paramter);

                    return output;
                }
                else
                    return string.Empty;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ex.StackTrace);
                return string.Empty;
            }
        }

        /// <summary>
        /// Create a new person to organisation
        /// </summary>
        /// <param name="orgId"></param>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        public static string NewPersonToOrganisation(string orgId, string firstName, string lastName, string email)
        {
            var pid = string.Empty;
            try
            {
                var postData = string.Format("<person><contacts><email><type>Work</type><emailAddress>{1}</emailAddress></email></contacts>" +
                    "<firstName>{2}</firstName><lastName>{3}</lastName><organisationId>{0}</organisationId></person>",
                    orgId, email, firstName, lastName);
                var output = RestClient.Request(HttpVerb.POST, postData, "person");

                if (!string.IsNullOrEmpty(output))
                {
                    var index = "party/";
                    pid = output.Substring(output.IndexOf(index) + index.Length);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ex.StackTrace);
            }

            return pid;
        }

        /// <summary>
        /// Create a history note to organisation
        /// </summary>
        /// <param name="oid"></param>
        /// <param name="note"></param>
        /// <returns></returns>
        public static string NewOrganisationHistoryNote(int oid, string note)
        {
            var hid = string.Empty;

            try
            {
                var postData = string.Format("<historyItem><note>{0}</note></historyItem>", note);
                var parameter = string.Format("party/{0}/history", oid);
                var output = RestClient.Request(HttpVerb.POST, postData, parameter);

                if (!string.IsNullOrEmpty(output))
                {
                    var index = "history/";
                    hid = output.Substring(output.IndexOf(index) + index.Length);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ex.StackTrace);
            }

            return hid;
        }

        /// <summary>
        /// Update a history note to organisation
        /// </summary>
        /// <param name="hid"></param>
        /// <param name="note"></param>
        /// <returns></returns>
        public static string UpdateHistoryNote(int hid, string note)
        {
            try
            {
                var postData = string.Format("<historyItem><note>{0}</note></historyItem>", note);
                var parameter = string.Format("history/{0}", hid);
                var output = RestClient.Request(HttpVerb.PUT, postData, parameter);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ex.StackTrace);
            }

            return hid.ToString();
        }

        /// <summary>
        /// Update jobTitle of a person
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="jobTitle"></param>
        /// <returns></returns>
        public static string UpdateJobTitleToPerson(int pid, string jobTitle)
        {
            try
            {
                var postData = string.Format("<person><jobTitle>{0}</jobTitle></person>", jobTitle);
                var parameter = string.Format("person/{0}", pid);
                var output = RestClient.Request(HttpVerb.PUT, postData, parameter);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ex.StackTrace);
            }

            return pid.ToString();
        }

        /// <summary>
        /// Create an Opportunity to organisation
        /// </summary>
        /// <param name="oid"></param>
        /// <param name="organisation"></param>
        /// <returns></returns>
        public static string NewOpportunityToOrganisation(string oid, string organisation)
        {
            var opId = string.Empty;

            try
            {
                var postData = string.Format("<opportunity><name>NewOpportunity-{0}</name><milestone>{1}</milestone></opportunity>",
                    organisation, "Opportunity");
                var parameter = string.Format("party/{0}/opportunity", oid);
                var output = RestClient.Request(HttpVerb.POST, postData, parameter);

                if (!string.IsNullOrEmpty(output))
                {
                    var index = "opportunity/";
                    opId = output.Substring(output.IndexOf(index) + index.Length);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ex.StackTrace);
            }

            return opId;
        }

        /// <summary>
        /// Get opportunity id of organisation
        /// </summary>
        /// <param name="ogId"></param>
        /// <returns></returns>
        public static string GetOpportunityOfOrganisation(int ogId)
        {
            var opId = string.Empty;
            try
            {
                var parameter = string.Format("party/{0}/opportunity", ogId);
                var output = RestClient.Request(HttpVerb.GET, "", parameter);

                if (!string.IsNullOrEmpty(output))
                {
                    var index = "<id>";
                    opId = output.Substring(output.IndexOf(index) + index.Length);
                    opId = opId.Substring(0, opId.IndexOf("<"));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ex.StackTrace);
            }

            return opId;
        }

        /// <summary>
        /// Update milestone of a opportunity
        /// </summary>
        /// <param name="ogId"></param>
        /// <param name="milestone"></param>
        /// <returns></returns>
        public static string UpdateOpportunityToOrganisation(int ogId, string milestone)
        {
            var opId = string.Empty;
            try
            {
                opId = GetOpportunityOfOrganisation(ogId);
                if (!string.IsNullOrEmpty(opId))
                {
                    var postData = string.Format("<opportunity><milestone>{0}</milestone></opportunity>",
                        milestone);
                    var parameter = string.Format("opportunity/{0}", opId);
                    var output = RestClient.Request(HttpVerb.PUT, postData, parameter);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ex.StackTrace);
            }

            return opId;
        }
        
        /// <summary>
        /// Create an Task to organisation
        /// </summary>
        /// <param name="oid"></param>
        /// <param name="organisation"></param>
        /// <returns></returns>
        public static string NewTask(string oid, string organisation)
        {
            var taskId = string.Empty;

            try
            {
                var postData = string.Format("<task><description>{0} has just been added.</description><dueDateTime>{1}</dueDateTime></task>",
                    organisation, DateTime.Now.AddDays(1).ToString("yyyy-MM-ddTHH:mm:ssZ"));
                var parameter = string.Format("party/{0}/task", oid);
                var output = RestClient.Request(HttpVerb.POST, postData, parameter);

                if (!string.IsNullOrEmpty(output))
                {
                    var index = "task/";
                    taskId = output.Substring(output.IndexOf(index) + index.Length);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ex.StackTrace);
            }

            return taskId;
        }

        /// <summary>
        /// Delete crm organisation, person, opportunity
        /// by email
        /// </summary>
        /// <param name="email"></param>
        public static void DeleteOrganisation(string email)
        {
            var oid = string.Empty;
            var pid = GetPersonByEmail(email, out oid);

            #region delete opportunity

            DeleteOpportunityOfOrganisation(oid);

            #endregion

            #region delete organisation
            int oid_ = 0;
            int.TryParse(oid, out oid_);
            if (oid_ > 0)
            {
                try
                {
                    var parameter = string.Format("party/{0}", oid);
                    var output = RestClient.Request(HttpVerb.DELETE, "", parameter);

                    if (output != "OK")//error
                    {

                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine(ex.StackTrace);
                }
            }
            #endregion

            #region delete person
            int pid_ = 0;
            int.TryParse(pid, out pid_);
            if (pid_ > 0)
            {
                try
                {
                    var parameter = string.Format("party/{0}", pid);
                    var output = RestClient.Request(HttpVerb.DELETE, "", parameter);

                    if (output != "OK")//error
                    {

                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine(ex.StackTrace);
                }
            }
            #endregion
        }

        /// <summary>
        /// get organistaion id and person id by person email
        /// </summary>
        /// <param name="email"></param>
        /// <param name="oid_"></param>
        /// <returns></returns>
        private static string GetPersonByEmail(string email, out string oid_)
        {
            var pID = string.Empty;

            try
            {
                var parameter = string.Format("person?email={0}", email);
                var output = RestClient.Request(HttpVerb.GET, "", parameter);

                if (!string.IsNullOrEmpty(output))
                {
                    var index = "<id>";
                    pID = output.Substring(output.IndexOf(index) + index.Length);
                    pID = pID.Substring(0, pID.IndexOf("<"));

                    index = "<organisationId>";
                    oid_ = output.Substring(output.IndexOf(index) + index.Length);
                    oid_ = oid_.Substring(0, oid_.IndexOf("<"));
                }
                else
                    oid_ = string.Empty;
            }
            catch (Exception ex)
            {
                oid_ = string.Empty;
                Console.WriteLine(ex.Message + ex.StackTrace);
            }

            return pID;
        }

        /// <summary>
        /// delete crm opportunity of organisation
        /// </summary>
        /// <param name="ogId"></param>
        /// <returns></returns>
        private static string DeleteOpportunityOfOrganisation(string ogId)
        {
            var result = string.Empty;
            int ogId_ = 0;
            int.TryParse(ogId, out ogId_);
            if (ogId_ > 0)
            {
                var opp = GetOpportunityOfOrganisation(ogId_);
                int opp_ = 0;
                int.TryParse(opp, out opp_);
                try
                {
                    if (opp_ > 0)
                    {
                        var parameter = string.Format("opportunity/{0}", opp_);
                        result = RestClient.Request(HttpVerb.DELETE, "", parameter);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message + ex.StackTrace);
                }
            }

            return result;
        }

        /// <summary>
        /// send notifier email to admin
        /// </summary>
        /// <param name="admin"></param>
        /// <param name="adminEmail"></param>
        /// <param name="organisation"></param>
        /// <param name="email"></param>
        /// <param name="person"></param>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="website"></param>
        /// <param name="country"></param>
        /// <param name="oid"></param>
        private static void SendCRMNotifierEmail(string admin, string adminEmail, string organisation, string email, string person, string firstName, string lastName, string website, string country, string oid)
        {
            try
            {
                var admins = admin.Split(';').ToList();
                var adminEmails = adminEmail.Split(';');
                for (int i = 0; i < adminEmails.Length; i++)
                {
                    var ebody = GetCRMNotifierEmailBody(admins[i], organisation, email, person, firstName, lastName, website, country, oid);
                    var subject = string.Format("[Capsule] New Contact Added: {0}", organisation);
                    var source = string.Format("{0} <{1}>", "Capsule Notifier", "support@priceme.co.nz");
                    DoSendEmail(adminEmails[i].Split(',').ToList(), email, ebody, subject, source);
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + e.StackTrace);
            }
        }
        
        private static string GetCRMNotifierEmailBody(string admin, string organisation, string email, string person, string firstName, string lastName, string website, string country, string oid)
        {
            var body = string.Format(@"<p>Hello {0},</p>" +
"<p>{1} has just been added as a contact using your web form.<br>REPLY BY EMAIL: <a href=\"mailto:{2}\" target=\"_blank\">{2}</a></p>" +
"<p>They entered the following details:</p>" +
"<ul>" +
    "<li>PERSON_NAME: {3} </li>" +
    "<li>FIRST_NAME: {4}</li>" +
    "<li>LAST_NAME: {5}</li>" +
    "<li>ORGANISATION_NAME: {1}</li>" +
    "<li>EMAIL: <a href=\"mailto:{2}\" target=\"_blank\">{2}</a></li>" +
    "<li>CUSTOMFIELD[Web Site URL]: <a href=\"{6}\" target=\"_blank\">{6}</a></li>" +
    "<li>COUNTRY[OFFICE]: {7}</li>" +
"</ul>" +
"<p>Open contact in Capsule:  <a href=\"https://priceme.capsulecrm.com/party/{8}\" target=\"_blank\">https://priceme.capsulecrm.<wbr>com/party/{8}</a></p>" +
"<p>The Capsule Team</p>", admin, organisation, email, person, firstName, lastName, website, country, oid);
            return body;
        }

        /// <summary>
        /// 执行发送邮件
        /// </summary>
        /// <param name="toAddress"></param>
        /// <param name="replytoAddress"></param>
        /// <param name="eBody"></param>
        /// <param name="subject"></param>
        /// <param name="source"></param>
        public static void DoSendEmail(List<string> toAddress, string replytoAddress, string eBody, string subject, string source)
        {
            if (replytoAddress.Contains("SeleniumTest")) return;//SeleniumTest 不发送邮件
            if (toAddress.Contains("SeleniumTest@priceme.com")) return;//SeleniumTest 不发送邮件

            string accessKeyID = ConfigurationManager.AppSettings["AWSAccessKey"];
            string secretAccessKeyID = ConfigurationManager.AppSettings["AWSSecretKey"];
            AmazonSimpleEmailServiceClient ses = new AmazonSimpleEmailServiceClient(accessKeyID, secretAccessKeyID);

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
            List<string> replyToAddressesList = new List<string>();
            replyToAddressesList.Add(replytoAddress);
            sendEmailRequest.ReplyToAddresses = replyToAddressesList;

            ses.SendEmail(sendEmailRequest);
        }
    }


    public class RestClient
    {
        public static string EndPoint { get { return "https://priceme.capsulecrm.com/api/"; } }
        public static HttpVerb Method { get; set; }
        public static string ContentType { get; set; }
        public static string PostData { get; set; }
        private static string APIToken { get { return WebConfig.CRMToken; } }

        #region Constructor

        public RestClient()
        {
            Method = HttpVerb.GET;
            ContentType = "application/xml";
        }

        public RestClient(HttpVerb method)
        {
            Method = method;
            ContentType = "application/xml";
        }

        public RestClient(HttpVerb method, string postData)
        {
            Method = method;
            ContentType = "application/xml";
            PostData = postData;
        }

        #endregion

        public static string Request(HttpVerb method, string postData, string parameters)
        {
            ContentType = "application/xml";
            return Request(method, ContentType, postData, parameters);
        }

        public static string Request(HttpVerb method, string contentType, string postData, string parameters)
        {
            Method = method;
            ContentType = contentType;
            PostData = postData;

            var url = EndPoint + parameters;
            var request = (HttpWebRequest)WebRequest.Create(url);

            request.Method = Method.ToString();
            request.ContentLength = 0;
            request.ContentType = ContentType;

            #region Authentication
            string username = APIToken;
            string password = "x";
            string usernamePassword = username + ":" + password;
            CredentialCache mycache = new CredentialCache();
            mycache.Add(new Uri(url), "Basic", new NetworkCredential(username, password));
            request.Credentials = mycache;
            request.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(new ASCIIEncoding().GetBytes(usernamePassword)));
            #endregion

            if (!string.IsNullOrEmpty(PostData) && (Method == HttpVerb.POST || Method == HttpVerb.PUT))
            {
                var bytes = Encoding.GetEncoding("utf-8").GetBytes(PostData);
                request.ContentLength = bytes.Length;

                using (var writeStream = request.GetRequestStream())
                {
                    writeStream.Write(bytes, 0, bytes.Length);
                    //using (var sw = new System.IO.StreamWriter(writeStream))
                    //{
                    //    sw.Write(PostData);
                    //}
                }
            }

            using (var response = (HttpWebResponse)request.GetResponse())
            {
                var responseValue = string.Empty;

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    var message = String.Format("Request result: Received HTTP {0}-{0:d}", response.StatusCode);
                    //throw new ApplicationException(message);
                    Console.WriteLine(message);
                }


                if (Method == HttpVerb.POST)
                    responseValue = response.GetResponseHeader("Location");
                else if (Method == HttpVerb.PUT || Method == HttpVerb.DELETE)
                    responseValue = response.StatusCode.ToString();
                else
                {
                    // grab the response
                    using (var responseStream = response.GetResponseStream())
                    {
                        if (responseStream != null)
                        {
                            using (var reader = new StreamReader(responseStream))
                            {
                                responseValue = reader.ReadToEnd();
                            }
                        }
                    }
                }

                return responseValue;
            }
        }
    }

    public enum HttpVerb
    {
        GET,
        POST,
        PUT,
        DELETE
    }
}