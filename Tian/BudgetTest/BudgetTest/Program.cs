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

            int dayCount = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(1).AddDays(-1).Day;
            int curday = DateTime.Now.Day;

            budgetPerMonthList.ForEach(budgetPerMonth =>
            {
                var memberShipInfo = aspnet_MembershipInfo.SingleOrDefault(item => item.RetailerID == budgetPerMonth.RetailerId);
                if (memberShipInfo == null) return;

                var userId = Guid.Parse(memberShipInfo.UserID);
                var membership = aspnet_Membership.SingleOrDefault(item => item.UserId == userId);
                if (membership == null) return;

                string html = Template.Load("MerchantEmail.html");

                int oldBudget = Convert.ToInt32(budgetPerMonth.Budget / dayCount);
                int increase = Convert.ToInt32(budgetPerMonth.Cost / curday) - oldBudget;

                if (increase < 2) increase = 2;
                else if (increase < oldBudget) increase = oldBudget;
                else increase += oldBudget;


                html = html.Replace("[first name]", memberShipInfo.FirstName);
                html = html.Replace("[old-budget]", oldBudget.ToString());
                html = html.Replace("[new-budget]", (oldBudget + increase).ToString());

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
