using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Configuration;
using System.Text.RegularExpressions;
using PriceMeDBA;
using System.Net.Mail;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;

namespace Pricealyser.PerformaceReport
{
    public class PerformaceReport
    {
        private Regex illegalReg = new Regex(@"[^a-z0-9-]+", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private Regex illegalReg2 = new Regex("-+", RegexOptions.Compiled);
        private Regex illegalReg3 = new Regex("^-+|-+$", RegexOptions.Compiled);

        StreamWriter sw;
        string stringRids = ConfigurationManager.AppSettings["RetailerId"];
        string cycleKey = ConfigurationManager.AppSettings["CycleKey"];
        string productCount = ConfigurationManager.AppSettings["ProductCount"];
        string bccEmail = ConfigurationManager.AppSettings["BccEmail"];
        string accessKeyID = ConfigurationManager.AppSettings["AWSAccessKey"];
        string secretAccessKeyID = ConfigurationManager.AppSettings["AWSSecretKey"];

        string listData;
        string imgUrl;
        string cycleString;
        int totalRef = 0;

        public void Performace()
        {
            string logPath = ConfigurationManager.AppSettings["LogPath"];
            if (!Directory.Exists(logPath))
                Directory.CreateDirectory(logPath);
            sw = new StreamWriter(logPath + DateTime.Today.ToString("yyyyMMdd") + ".txt");
            WriterLog("Begin......");

            //List<int> listRids = GetSendRetailer();
            //var rids = stringRids.Split(',').Select(s=>int.Parse(s)).ToList();

            var rids = GetSendRetailer();

            foreach (int rid in rids)
            {
                CSK_Store_Retailer retailer = CSK_Store_Retailer.SingleOrDefault(r => r.RetailerId == rid);

                //Get data
                WriterLog("Get data......");
                GetData(rid);

                //Create Html Code
                WriterLog("Create Html Code......");
                string htmlString = CreateHtmlCode(retailer.RetailerContactName, rid);

                if (totalRef <= 10) continue;

                //Send Email
                WriterLog("Send Email......");
                AmazonSimpleEmailServiceClient ses = new AmazonSimpleEmailServiceClient(accessKeyID, secretAccessKeyID);
                SendEmailRequest seReq = new SendEmailRequest();
                seReq.Source = "info@priceme.co.nz";
                List<string> list = new List<string>();
                Destination det = new Destination();
                list.Add(retailer.ContactEmail);
                det.ToAddresses = list;
                list = new List<string>();
                list.Add(bccEmail);
                det.BccAddresses = list;
                seReq.Destination = det;
                list = new List<string>();
                list.Add("info@priceme.co.nz");
                seReq.ReplyToAddresses = list;
                
                Message mes = new Message();
                Content con = new Content();
                con.Data = "Your Top Products Last " + cycleKey + " on PriceMe";
                mes.Subject = con;

                Body body = new Body();
                Content conHtml = new Content();
                conHtml.Data = htmlString;
                body.Text = conHtml;
                body.Html = conHtml;
                mes.Body = body;
                seReq.Message = mes;
                ses.SendEmail(seReq);

                //MailMessage mess = new MailMessage();
                //mess.From = new MailAddress("info@priceme.co.nz", "PriceMe");
                //mess.To.Add(new MailAddress(retailer.ContactEmail));
                //mess.ReplyTo = new MailAddress("info@priceme.co.nz");
                //mess.IsBodyHtml = true;
                //mess.Subject = "Your Top Products Last " + cycleKey + " on PriceMe";
                //mess.Body = htmlString;
                //SmtpClient smtp = new SmtpClient();
                //smtp.Send(mess);
            }
        }

        private List<int> GetSendRetailer()
        {
            List<int> listRids = new List<int>();
            string stringType = string.Empty;
            if (cycleKey.ToLower() == "month")
                stringType = "IsMonthlyEmail = 1";
            else
                stringType = "IsWeeklyEmail = 1";

            string sql = "Select RetailerID From CSK_Store_RetailerNewsletterSet Where " + stringType;
            SubSonic.Schema.StoredProcedure sp = new SubSonic.Schema.StoredProcedure("");
            sp.Command.CommandSql = sql;
            sp.Command.CommandTimeout = 0;
            sp.Command.CommandType = System.Data.CommandType.Text;
            System.Data.IDataReader dr = sp.ExecuteReader();
            while (dr.Read())
            {
                int rid = 0;
                int.TryParse(dr["RetailerID"].ToString(), out rid);
                if (!listRids.Contains(rid))
                    listRids.Add(rid);
            }
            dr.Close();

            return listRids;
        }

        private void GetData(int rid)
        {
            totalRef = 0;
            listData = string.Empty;
            string strDate = string.Empty;
            string endDate = DateTime.Today.ToString("yyyy-MM-dd");

            if (cycleKey == "Week")
            {
                string dateString = string.Empty;
                strDate = DateTime.Today.AddDays(-7).ToString("yyyy-MM-dd");

                DateTime strString = DateTime.Parse(strDate);
                DateTime endString = DateTime.Parse(endDate).AddDays(-1);

                if (strString.ToString("MMMM yyyy") == endString.ToString("MMMM yyyy"))
                    dateString = strString.ToString("d MMMM yyyy").Split(' ')[0] + "-" + endString.ToString("d MMMM yyyy");
                else
                    dateString = strString.ToString("d MMMM") + "-" + endString.ToString("d MMMM yyyy");

                cycleString = "Here’s a list of your top products that PriceMe shoppers clicked on last week (period " + dateString + ").";
                imgUrl = "http://images.pricemestatic.com/Images/NewsletterImages/invoicestatsemail/weekly.png";
            }
            else if (cycleKey == "Month")
            {
                cycleString = "Here are you most referred products last month on PriceMe.";
                imgUrl = "http://images.pricemestatic.com/Images/NewsletterImages/invoicestatsemail/Monthly.png";
                strDate = DateTime.Today.AddMonths(-1).ToString("yyyy-MM-dd");
            }

            string sql = "Select top " + productCount + " R.RetailerProductName, P.ProductName, P.ProductID, COUNT(R.RetailerProductname) as counts from CSK_Store_RetailerTracker T inner join CSK_Store_RetailerProduct R "
                       + "on T.RetailerProductID = R.RetailerProductId inner join CSK_Store_Product P on P.ProductID=R.ProductId "
                        + "where T.CreatedOn > '" + strDate + "' and T.CreatedOn < '" + endDate + "' "
                        + "and UserIP not in (select IPAddress from CSK_Store_IP_Blacklist)and R.RetailerId = " + rid + " group by R.RetailerProductName, P.ProductName, P.ProductID order by counts desc";
            WriterLog("sql: " + sql);
            int sqlCount = 0;
            SubSonic.Schema.StoredProcedure sp = new SubSonic.Schema.StoredProcedure("");
            sp.Command.CommandSql = sql;
            sp.Command.CommandTimeout = 0;
            sp.Command.CommandType = System.Data.CommandType.Text;
            System.Data.IDataReader dr = sp.ExecuteReader();
            while (dr.Read())
            {
                string stringData = string.Empty;
                string count = dr["counts"].ToString();
                string name = dr["RetailerProductName"].ToString();
                string pname = dr["ProductName"].ToString();
                string pid = dr["ProductID"].ToString();
                string url = CreateProductUrl(pname, pid);

                stringData = "<div style=\"font-family:'Trebuchet MS';font-size:13px;font-family:tahoma,geneva,sans-serif;color:rgb(102,102,102);padding-top: 3px;clear: both;\">"
                            + "<div style=\"float:left;width:100px;\"><strong>" + count + "</strong>&nbsp; referrals&nbsp; -&nbsp;</div><div style=\"float:left;width:500px;\"><a href=\"" + url + "\" target=\"_blank\">" + name + "</a></div></div>";
                listData += stringData;
                sqlCount++;

                totalRef += Convert.ToInt32(count);
            }
            dr.Close();
            productCount = sqlCount.ToString();
            //WriterLog(listData);
        }

        private string CreateHtmlCode(string firstName, int rid)
        {
            string stencilPath = ConfigurationManager.AppSettings["StencilPath"];
            string htmlString = File.ReadAllText(stencilPath);
            htmlString = htmlString.Replace("[string_FirstName]", firstName);
            htmlString = htmlString.Replace("[string_CycleTitle]", cycleString);
            htmlString = htmlString.Replace("[string_Cycle]", cycleKey.ToLower());
            htmlString = htmlString.Replace("[string_ImageUrl]", imgUrl);
            htmlString = htmlString.Replace("[string_ProductContent]", listData);
            htmlString = htmlString.Replace("[string_ProductCount]", productCount);
            htmlString = htmlString.Replace("[string_Year]", DateTime.Today.Year.ToString());
            htmlString = htmlString.Replace("[string_Rid]", rid.ToString());
            htmlString = htmlString.Replace("\r\n", "");

            return htmlString;
        }

        private string CreateProductUrl(string name, string pid)
        {
            string url = "http://www.priceme.co.nz" + string.Format("/{0}/p-{1}.aspx", FilterInvalidUrlPathChar(name), pid);

            return url;
        }

        private string FilterInvalidUrlPathChar(string sourceString)
        {

            sourceString = illegalReg.Replace(sourceString, "-");
            sourceString = illegalReg2.Replace(sourceString, "-");
            sourceString = illegalReg3.Replace(sourceString, "");
            return sourceString;
        }

        private void WriterLog(string info)
        {
            Console.WriteLine(info);
            sw.WriteLine(info);
            sw.Flush();
        }
    }
}
