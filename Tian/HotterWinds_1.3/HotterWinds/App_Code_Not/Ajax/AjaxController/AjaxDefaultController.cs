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
    }
}