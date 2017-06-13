using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;

namespace HotterWinds
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            SiteConfig.Load();

        }

        void Application_End(Object sender, EventArgs e)
        {

        }

        void Application_Error(Object sender, EventArgs e)
        {
            // Code that runs when an unhandled error occurs
            Exception ex = Server.GetLastError();

            if (ex is HttpRequestValidationException)
            {
                Response.ClearContent();
                Response.Write("<h3>HTML tags are not allowed. Please only use text.</h3>");
                Response.Write("<br /><input type='button' onclick='javascript:history.go(-1);' value='Go back'>");
                Server.ClearError();
            }
            else if (ex is ArgumentException)
            {
                Response.ClearContent();
                Response.Write("<h3>Invalid postback or callback argument. </h3>");
                Response.Write("<br /><input type='button' onclick='javascript:history.go(-1);' value='Go back'>");
                Server.ClearError();
            }

            //Writer Datebase
            //String exceptionType = ex.Message;
            //String exceptionInfo = ex.GetBaseException().ToString();
            //String errorPagePath = Request.RawUrl;
            //LogWriter.PriceMeDataBaseExceptionWriter.WriteException(exceptionInfo, exceptionType, errorPagePath, "PriceMe Website", 0);
            //Commerce.Common.ExceptionCollect.Insert(exceptionInfo, exceptionType, errorPagePath);

            //if (Server.GetLastError() is HttpUnhandledException)
            //    Server.Transfer("ErrorPage.aspx");     
            //if ((Context != null) && (Context.User.IsInRole("Administrator")))
            //{
            //    Response.Clear();
            //    while (ex.InnerException != null)
            //    {
            //        Response.Write("<h4>" + ex.InnerException.Message + "</h4>");
            //        Response.Write("<pre>" + ex.InnerException.ToString() + "</pre>");
            //        ex = ex.InnerException;
            //    }
            //    Server.ClearError(); //Leave this here - if you do it last, 
            //    it will cancel out the web.config customErrors section
            //}
        }

        void Session_Start(Object sender, EventArgs e)
        {
            // Code that runs when a new session is started

        }

        void Session_End(Object sender, EventArgs e)
        {
            // Code that runs when a session ends. 
            // Note: The Session_End event is raised only when the sessionstate mode
            // is set to InProc in the Web.config file. If session mode is set to StateServer 
            // or SQLServer, the event is not raised.

        }

        string GetAppRoot(string pagePath)
        {
            string appRoot = "";
            appRoot = pagePath;
            //strip out the page
            appRoot = appRoot.Replace(System.IO.Path.GetFileName(appRoot), "");

            //strip the "content" directory
            return appRoot;
        }
        string GetAppRoot()
        {
            return Request.ApplicationPath + "/";
        }

        protected void Application_BeginRequest(Object sender, EventArgs e)
        {            
            string[] need301Redir = new string[] {  "https://priceme.co.nz",
                                                "https://priceme.co.nz/",
                                                "https://priceme.co.nz/default.aspx"};
            foreach (string nr in need301Redir)
            {
                if (nr == Request.Url.AbsoluteUri.ToLower())
                {
                    string pUrl = Request.ServerVariables["HTTP_REFERER"];
                    if (!string.IsNullOrEmpty(pUrl) && nr == pUrl.ToLower())
                    {
                        break;
                    }
                    Context.Response.Status = "301 Moved Permanently";
                    Context.Response.AddHeader("Location", Resources.Resource.Global_HomePageUrl);
                    Context.Response.End();
                    return;
                }
            }

            if (Request.Path.Equals("/RetailerCenter.aspx", StringComparison.InvariantCultureIgnoreCase))
            {
                Context.Response.Status = "301 Moved Permanently";
                Context.Response.AddHeader("Location", "/RetailerCenter/RetailerCenter.aspx");
                Context.Response.End();
                return;
            }

            if (Request.Path.ToLower().Contains("/newsletter/"))
            {
                Context.Response.Status = "301 Moved Permanently";
                Context.Response.AddHeader("Location", "/About/AboutUs.aspx");
                Context.Response.End();
                return;
            }

            string currentUrl = HttpContext.Current.Request.RawUrl;

            string fixedUrl = PriceMe.UrlController.Fixed301Url(currentUrl);
            if (currentUrl != fixedUrl)
            {
                Context.Response.Status = "301 Moved Permanently";
                Context.Response.AddHeader("Location", fixedUrl);
                Context.Response.End();
                return;
            }

            string url = PriceMe.UrlController.GetRealUrl(currentUrl, false);
            HttpContext.Current.RewritePath(url, false);
        }

        public override string GetVaryByCustomString(HttpContext context, string arg)
        {
            if (arg == "AllCacheFlag")
            {
                Object _flag = context.Cache.Get("AllCacheFlag");
                if (_flag == null)
                {
                    _flag = DateTime.Now.Ticks.ToString();
                    context.Cache.Add("AllCacheFlag", _flag, null, System.Web.Caching.Cache.NoAbsoluteExpiration, new TimeSpan(48, 0, 0), CacheItemPriority.High, null);
                }
                return _flag.ToString();
            }
            return base.GetVaryByCustomString(context, arg);
        }

    }
}