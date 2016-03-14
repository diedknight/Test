using BudgetTest.DataModel;
using BudgetTest.Email;
using BudgetTest.EmailTemplate;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetTest
{
    public class ExceedBudgetEmail
    {
        public static void SendEmail(List<BudgetPerMonth> list)
        {
            if (list == null || list.Count == 0) return;

            string html = Template.Load("Email.html");

            string tableHtml = "";

            tableHtml += "<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"width:100%;padding-bottom:30px;\">";
            tableHtml += "<thead>";
            tableHtml += "<tr style=\"background-color:#CCCCCC;border:solid 1px #ddd; line-height:50px; height:50px; text-align:left;\">";
            tableHtml += "<th style=\"padding:0 10px;\">RetailerID</th>";
            tableHtml += "<th style=\"padding:0 10px;\">RetailerName</th>";
            tableHtml += "<th style=\"padding:0 10px;\">OverBudget</th>";
            tableHtml += "</tr>";
            tableHtml += "</thead>";
            tableHtml += "<tbody>";

            list.ForEach(item =>
            {
                tableHtml += "<tr style=\"line-height:30px;height:30px;\">";
                tableHtml += "<td>" + item.RetailerId + "</td>";
                tableHtml += "<td>" + item.RetailerName + "</td>";
                tableHtml += "<td>" + Math.Abs(item.Balance) + "</td>";
                tableHtml += "</tr>";
            });

            tableHtml += "</tbody>";
            tableHtml += "</table>";

            html = html.Replace("{Table}", tableHtml);

            string emailStr = ConfigurationManager.AppSettings["Email"];
            string wSAccessKey = ConfigurationManager.AppSettings["WSAccessKey"];
            string wSSecretKey = ConfigurationManager.AppSettings["WSSecretKey"];

            string subject = "Budget report";
            string to = ConfigAppString.BudgetReportEmail;

            IEmail email = new AmazonEmail(emailStr, wSAccessKey, wSSecretKey);
            email.Send(subject, html, to);
        }
    }
}
