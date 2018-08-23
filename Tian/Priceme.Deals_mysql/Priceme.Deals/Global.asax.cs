//#undef NoDebug
#define NoDebug

using Priceme.Deals.Code;
using PriceMeCommon.BusinessLogic;
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

            MultiCountryController.LoadWithCheckIndexPath();
#if (NoDebug)
            CategoryController.Load(null);
            RetailerController.Load(null);
#endif
            MultiCountryController.OnIndexChanged += MultiCountryController_OnIndexChanged;
        }

        private void MultiCountryController_OnIndexChanged(int countryId, string newLuceneIndexPath)
        {
            CategoryController.Load(null);
            RetailerController.Load(null);
        }

        protected void Application_BeginRequest(Object sender, EventArgs e)
        {
            HttpContext.Current.RewritePath(UrlRoute.Decode(this.Request.Url));
        }

    }
}