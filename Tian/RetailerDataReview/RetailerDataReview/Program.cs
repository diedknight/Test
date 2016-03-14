using Channelyser.Job.EmailTemplate;
using PriceMeDBA;
using RetailerDataReview.DataModel;
using RetailerDataReview.Email;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RetailerDataReview
{
    class Program
    {
        static void Main(string[] args)
        {
            var retailerList = Retailer.GetList(ConfigAppString.RetailerIds);
            var paymentOptionList = PaymentOption.GetList(ConfigAppString.CountryId);

            retailerList.ForEach(retailer => {
                bool isFirstItem = true;

                var memberShipInfo = aspnet_MembershipInfo.SingleOrDefault(item => item.RetailerID == retailer.RetailerId);
                var userId = Guid.Parse(memberShipInfo.UserID);
                var membership = aspnet_Membership.SingleOrDefault(item => item.UserId == userId);
                var contactList = RetailerUserEmail.GetList(retailer.RetailerId);
                var numberStores = GLatLng.GetCount(retailer.RetailerId);
                var retailerPaymentOptionList = RetailerPaymentOption.GetList(retailer.RetailerId);


                string html = Template.Load("email-activation.html");

                html = html.Replace("{FirstName}", memberShipInfo.FirstName);
                html = html.Replace("{LastName}", memberShipInfo.LastName);
                html = html.Replace("{Email}", membership.Email);

                html = html.Replace("{RetailerName}", retailer.RetailerName);
                html = html.Replace("{RetailerUrl}", retailer.RetailerURL);
                html = html.Replace("{GSTNumber}", retailer.GSTNumber);
                html = html.Replace("{CompanyRegNumber}", retailer.CompanyRegNumber);

                //html = html.Replace("{ContactFirstName}", contactList.Count == 0 ? "" : contactList[0].ContactFirstName);
                //html = html.Replace("{ContactLastName}", contactList.Count == 0 ? "" : contactList[0].ContactLastName);
                //html = html.Replace("{ContactEmail}", contactList.Count == 0 ? "" : contactList[0].Email);

                string contactHtml = "";
                contactList.ForEach(contact => {
                    contactHtml += "<tr style=\"line-height:35px; height:35px;\">";
                    contactHtml += "<td style=\"width:150px;\">" + (isFirstItem ? "Accounting contact:" : "") + "</td>";
                    contactHtml += "<td>" + contact.ContactFirstName + " " + contact.ContactLastName + "</td>";
                    contactHtml += "</tr>";

                    contactHtml += "<tr style=\"line-height:35px;height:35px;\">";
                    contactHtml += "<td></td>";
                    contactHtml += "<td>" + contact.Email + "</td>";
                    contactHtml += "</tr>";

                    isFirstItem = false;
                });

                html = html.Replace("{ContactList}", contactHtml);

                //if (contactList.Count < 2)
                //{
                //    html = html.Replace("{ContactList}", "");
                //}
                //else
                //{
                    
                //    for (int i = 1; i < contactList.Count; i++)
                //    {
                //        var contact = contactList[i];

                //        contactHtml += "<tr style=\"line-height:35px; height:35px;\">";
                //        contactHtml += "<td style=\"width:150px;\"></td>";
                //        contactHtml += "<td>" + contact.ContactFirstName + " " + contact.ContactLastName + "</td>";
                //        contactHtml += "</tr>";

                //        contactHtml += "<tr style=\"line-height:35px;height:35px;\">";
                //        contactHtml += "<td></td>";
                //        contactHtml += "<td>" + contact.Email + "</td>";
                //        contactHtml += "</tr>";
                //    }

                //    html = html.Replace("{ContactList}", contactHtml);
                //}

                if (numberStores == 0)
                {
                    html = html.Replace("{NumberStores}", "None");
                    html = html.Replace("{MapUrlATag}", "");
                }
                else
                {
                    html = html.Replace("{NumberStores}", numberStores <= 1 ? (numberStores + " store") : (numberStores + " stores"));
                    html = html.Replace("{MapUrlATag}", "<a target=\"_blank\" href=\"" + new MapUrl().GetUrl(retailer.RetailerId, retailer.RetailerCountry, retailer.RetailerName) + "\">check locations on map</a>");
                }

                string optionHtmlStr = "";

                paymentOptionList.ForEach(paymentOption => {
                    bool isSupport = retailerPaymentOptionList.Exists(item => item.PaymentOptionId == paymentOption.PaymentOptionId);

                    if (isSupport) paymentOption.Order = 1;
                });

                paymentOptionList = paymentOptionList.OrderByDescending(paymentOption => paymentOption.Order).ToList();

                paymentOptionList.ForEach(paymentOption =>
                {
                    paymentOption.Order = 0;

                    StringBuilder sb = new StringBuilder();
                    bool isSupport = retailerPaymentOptionList.Exists(item => item.PaymentOptionId == paymentOption.PaymentOptionId);
                    string surcharge = "";
                    if (retailer.CCFee < 0) surcharge = "";
                    else if (retailer.CCFee == 0) surcharge = "no surcharge";
                    else surcharge = "(" + (retailer.CCFee * 100).ToString("0.00") + "% surcharge)";

                    sb.AppendLine("<tr style=\"line-height:50px;height:50px;\">");
                    sb.AppendLine("<td style=\"width:150px;\">" + paymentOption.Name + ":</td>");
                    sb.AppendLine("<td style=\"width:100px;\">" + (isSupport ? "<span style=\"color:#66CD00;font-size:20px;\">√</span>" : "<span style=\"color:#CD3700;font-size:20px;\">×</span>") + "</td>");

                    if (paymentOption.Name == "Visa" && isSupport)
                        sb.AppendLine("<td>" + surcharge + "</td>");
                    else
                        sb.AppendLine("<td></td>");
                        
                    sb.AppendLine("</tr>");

                    optionHtmlStr += sb.ToString();
                });

                html = html.Replace("{PaymentOptionList}", optionHtmlStr);

                html = html.Replace("{CorrectBtnUrl}", ConfigAppString.SuccessUrl + "?rid=" + retailer.RetailerId + "&token=" + MD5(retailer.RetailerId.ToString()));
                html = html.Replace("{NeedToUpdateBtnUrl}", "https://merchant.priceme.com");

                html = html.Replace("cash", "Cash");

                //发送邮件
                string emailStr = ConfigurationManager.AppSettings["Email"];
                string wSAccessKey = ConfigurationManager.AppSettings["WSAccessKey"];
                string wSSecretKey = ConfigurationManager.AppSettings["WSSecretKey"];

                string subject = ConfigAppString.Subject;
                string to = membership.Email;

                if (ConfigAppString.DebugEmail != "") to = ConfigAppString.DebugEmail;

                IEmail email = new AmazonEmail(emailStr, wSAccessKey, wSSecretKey);
                email.Send(subject, html, to);
            });
        }

        private static string ReplaceSpeicalRetailerName(string retalerName)
        {
            retalerName = retalerName.Replace("~", "-");
            retalerName = retalerName.Replace(".", "-");
            retalerName = retalerName.Replace("&", "-");
            retalerName = retalerName.Replace("'", "-");
            retalerName = retalerName.Replace("%", "-");
            retalerName = retalerName.Replace("(", "-");
            retalerName = retalerName.Replace(")", "-");
            retalerName = retalerName.Trim();
            retalerName = retalerName.Replace(" ", "-");

            return retalerName;
        }

        private static string MD5(string str)
        {
            using (MD5 md5 = new MD5CryptoServiceProvider())
            {
                byte[] result = Encoding.UTF8.GetBytes(str);
                byte[] output = md5.ComputeHash(result);
                return BitConverter.ToString(output).Replace("-", "");
            }
        }

    }
}
