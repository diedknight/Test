using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UpcomingProductAlert.DB;
using UpcomingProductAlert.Email;
using UpcomingProductAlert.Log;

namespace UpcomingProductAlert
{
    class Program
    {
        private static string _debug = System.Configuration.ConfigurationManager.AppSettings["Debug"];
        private static string _debugEmail = System.Configuration.ConfigurationManager.AppSettings["DebugEmail"];

        static void Main(string[] args)
        {
            UpcomingProductAlter.GetAll().ForEach(alert =>
            {                
                var product = Product.Get(alert.UpcomingProductID, alert.CountryID);
                if (product == null) return;

                Console.WriteLine("alertId:" + alert.UpcomingProductAlterID);

                var countryDetail = CountryDetail.GetCountryDetail(alert.CountryID);
                var content = CSK_Content.Get();
                
                string url = PriceMeUrl.GetProductUrl(countryDetail, product.ProductId, product.ProductName);
                string aTag = "<table class=\"button Budget-button\" style=\"border-spacing: 0; border-collapse: collapse; vertical-align: top; text-align: left; width: 200px; overflow: hidden; padding: 0;\">"
                + "<tr style=\"vertical-align: top; text-align: left; padding: 0;\" align=\"left\">"
                + "<td style=\"word-break: break-word; -webkit-hyphens: auto; -moz-hyphens: auto; hyphens: auto; border-collapse: collapse !important; vertical-align: top; text-align: center; color: #ffffff; font-family: 'Helvetica Neue', 'Arial', sans-serif; font-weight: normal; line-height: 19px; font-size: 14px; display: block; width: auto !important; border-radius: 4px; background: #48A4CF; margin: 0; padding: 13px 0 12px;\" align=\"center\" bgcolor=\"#48A4CF\" valign=\"top\">"
                + "<a href=\"" + url + "\" style=\"color: #ffffff; text-decoration: none; font-weight: bold; font-family: Helvetica, Arial, sans-serif; font-size: 15px;\">View Product</a>"
                + "</td>"
                + "</tr>"
                + "</table>";

                content.TitleReplace("[product_name]", product.ProductName);
                content.CtxReplace("[product_name]", product.ProductName);
                content.CtxReplace("[country_name]", countryDetail.CountryName);
                content.CtxReplace("[View product]", aTag);

                AWSEmail email = new AWSEmail(content.FromEmail, content.FromName);                                    

                email.Subject = content.Title;
                email.Body = content.Ctx;

                if (_debug != null && _debug.ToLower() == "true")
                    email.AddToAddress(_debugEmail);
                else
                    email.AddToAddress(alert.email);


                email.AddReplyAddress(content.ReplyTo);

                if (!string.IsNullOrEmpty(content.CC)) content.CC.Split(';').ToList().ForEach(cc => email.AddCCAddress(cc));
                if (!string.IsNullOrEmpty(content.BCC)) content.BCC.Split(';').ToList().ForEach(bcc => email.AddBCCAddress(bcc));

                try
                {
                    email.Send();
                    alert.UpdateStatus(true);

                    var log = new EmailLog(countryDetail.CountryId);
                    log.WriteLine(alert.UpcomingProductID.ToString(), alert.email);
                }
                catch (Exception ex)
                {
                    var log = new ErrorLog(countryDetail.CountryId);
                    log.WriteLine("Error UpcomingProductAlterID:" + alert.UpcomingProductAlterID);
                    log.WriteLine(ex.Message);
                    log.WriteLine(ex.StackTrace);
                    log.WriteLine();                    
                }                
            });
        }



    }
}
