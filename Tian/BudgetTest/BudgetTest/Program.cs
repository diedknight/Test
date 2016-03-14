using BudgetTest.DataModel;
using BudgetTest.Email;
using BudgetTest.EmailTemplate;
using PriceMeDBA;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetTest
{
    class Program
    {
        static void Main(string[] args)
        {
            List<BudgetPerMonth> budgetPerMonthList = BudgetPerMonth.GetList();

            ExceedBudgetEmail.SendEmail(budgetPerMonthList);

            //if (!ConfigAppString.SendBudgetEmail) return;

            int days = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(1).AddDays(-1).Day;

            budgetPerMonthList.ForEach(budgetPerMonth =>
            {
                var memberShipInfo = aspnet_MembershipInfo.SingleOrDefault(item => item.RetailerID == budgetPerMonth.RetailerId);
                if (memberShipInfo == null) return;

                var userId = Guid.Parse(memberShipInfo.UserID);
                var membership = aspnet_Membership.SingleOrDefault(item => item.UserId == userId);
                if (membership == null) return;

                string html = Template.Load("MerchantEmail.html");

                html = html.Replace("[first name]", memberShipInfo.FirstName);
                html = html.Replace("[old-budget]", (budgetPerMonth.Budget / days).ToString("0.00"));
                html = html.Replace("[new-budget]", (budgetPerMonth.Budget / days + Math.Abs(budgetPerMonth.Balance) / days).ToString("0.00"));

                string emailStr = ConfigurationManager.AppSettings["Email"];
                string wSAccessKey = ConfigurationManager.AppSettings["WSAccessKey"];
                string wSSecretKey = ConfigurationManager.AppSettings["WSSecretKey"];

                string subject = ConfigAppString.Subject;
                //string to = ConfigAppString.BudgetReportEmail;

                IEmail email = new AmazonEmail(emailStr, wSAccessKey, wSSecretKey);

                if (ConfigAppString.SendBudgetEmail)
                    email.Send(subject, html, ConfigAppString.BudgetReportEmail, membership.Email);
                else
                    email.Send(subject, html, ConfigAppString.BudgetReportEmail);


            });

        }

    }
}
