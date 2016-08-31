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

                var countryDetail = CountryDetail.GetCountryDetail(alert.CountryID);
                var content = CSK_Content.Get();

                string url = PriceMeUrl.GetProductUrl(countryDetail, product.ProductId, product.ProductName);
                string aTag = "<a href=\"" + url + "\">View Product</a>";

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
                if (!string.IsNullOrEmpty(content.CC)) content.BCC.Split(';').ToList().ForEach(bcc => email.AddBCCAddress(bcc));

                try
                {
                    email.Send();
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
