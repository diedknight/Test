using PriceMeChannel.App_Code_Not.Ajax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dapper;

namespace HotterWinds.App_Code_Not.Ajax.AjaxController
{
    public class AjaxDefaultController : IAjaxable
    {
        public string SigningUp(AjaxContext context)
        {
            try
            {
                HotterWindsDBA.HW_Newsletter_Email newsletter = new HotterWindsDBA.HW_Newsletter_Email();
                newsletter.EmailAddress = context.Parameter["email"].ToString();
                newsletter.Save();

                return "1";
            }
            catch
            {
                return "0";
            }
        }

        public string AddReview(AjaxContext context)
        {
            try
            {
                string comment = context.GetParameter("comment", "");
                int rating = context.GetParameter("rating", 0);
                int pid = context.GetParameter("pid", 0);
                string name = context.GetParameter("name", "");
                string yourname = context.GetParameter("yourName", "");

                HotterWindsDBA.CSK_Store_ProductReview review = new HotterWindsDBA.CSK_Store_ProductReview();
                review.Body = comment;
                review.createdBy = name;
                review.createdOn = DateTime.Now;
                review.IsApproved = true;
                review.modifiedBy = name;
                review.modifiedOn = DateTime.Now;
                review.PostDate = DateTime.Now;
                review.ProductID = pid;
                review.Rating = rating;
                review.RetailerCountry = 3;
                review.UserName = yourname;
                review.Title = "";
                review.AuthorName = name;

                review.Save();

                return review.PostDate.ToString("MMMM dd , yyyy");
            }
            catch(Exception ex)
            {
                return "0";
            }
        }

    }
}