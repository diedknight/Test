//#undef NoDebug
#define NoDebug

using Priceme.Deals.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace Priceme.Deals
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
#if (NoDebug)
            PriceMeCommon.CategoryController.Load();
            PriceMeCommon.RetailerController.Load();

            PriceMeCommon.BusinessLogic.IndexModifyController.StartCheckModify();

            PriceMeCommon.BusinessLogic.IndexModifyController.IndexModifyEvent += (s, t) =>
            {
                PriceMeCommon.CategoryController.Load();
                PriceMeCommon.RetailerController.Load();
            };
#endif
        }

        protected void Application_BeginRequest(Object sender, EventArgs e)
        {
            HttpContext.Current.RewritePath(UrlRoute.Decode(this.Request.Url));
        }

    }
}