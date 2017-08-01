using System;
using System.Collections;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Web.Configuration;
using System.Collections.Generic;
using Parse;
using PriceMeCommon.BusinessLogic;

public static class SiteConfig
{
    static string WebVersion_Static = "9.2";
    public static void Load()
    {
        try
        { 
            Timer.DKTimer dkTimer = new Timer.DKTimer();
            dkTimer.Start();

            dkTimer.Set(" ------ Website " + WebVersion_Static + " restart. ------ ");

            if (!string.IsNullOrEmpty(PriceMe.WebConfig.ParseServer))
            {
                ParseClient.Initialize(new ParseClient.Configuration
                {
                    ApplicationId = PriceMe.WebConfig.ParseAPPID,
                    Server = PriceMe.WebConfig.ParseServer
                });
            }
            else
            {
                ParseClient.Initialize(PriceMe.WebConfig.ParseAPPID, PriceMe.WebConfig.ParseNETSDK);
            }

            dkTimer.Set("Start load Cache");

            MultiCountryController.LoadWithCheckIndexPath();

            dkTimer.Set("Befor load Category Cache");
            CategoryController.Load(dkTimer);

            dkTimer.Set("Befor load Product Cache");
            ProductController.Load(dkTimer);

            dkTimer.Set("Befor load Retailer Cache");
            RetailerController.Load(dkTimer);

            dkTimer.Set("Befor load Review Cache");
            ReviewController.Load(dkTimer);

            dkTimer.Set("Befor load BuyingGuide Cache");
            BuyingGuideController.Load(dkTimer);

            dkTimer.Set("Befor load ResourcesInfo Cache");
            PriceMe.ResourcesInfoController.Load(PriceMe.WebConfig.CountryId);

            dkTimer.Set("Befor load Manufacturer Cache"); 
            ManufacturerController.Load(dkTimer);

            if (PriceMe.WebConfig.CountryId == 3)
            {
                dkTimer.Set("Befor load MobilePlan Cache");
                MobilePlanController.Load(dkTimer);
            }
            dkTimer.Set("Befor load Attribute Cache");
            AttributesController.Load(dkTimer);

            DFP_AdsChontroller.Load();

            ConsumerController.Load();

            WebSiteController.Load(PriceMe.WebConfig.CountryId);

            dkTimer.Set("End load Cache");

            dkTimer.Set(" ------ Website restart end. ------ ");

            dkTimer.Stop();

            MultiCountryController.OnIndexChanged += MultiCountryController_OnIndexChanged;

            LogController.WriteLog("------------\r\n" + dkTimer.OutputText() + "\r\n------------ at : " + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "\r\n");
        }
        catch (Exception ex)
        {
            string msg = ex.Message + "-----------" + ex.StackTrace;
            LogController.WriteException("------------\n" + msg);
            if (ex.InnerException != null)
            {
                LogController.WriteException("\t--------\nInnerException : " + ex.InnerException.Message + "-----------" + ex.InnerException.StackTrace + "\n");
            }

            PriceMeDBA.CSK_Store_ExceptionCollect exception = new PriceMeDBA.CSK_Store_ExceptionCollect();
            exception.ExceptionInfo = msg;
            exception.ExceptionType = "onStart";
            exception.ExceptionAppName = "PriceMe";
            exception.errorPagePath = "onStart";
            exception.Level = 0;
            exception.Save();
        }
    }

    private static void MultiCountryController_OnIndexChanged(int countryId, string newLuceneIndexPath)
    {
        ReLoad();
    }

    public static bool ReLoad()
    {
        try
        {
            Timer.DKTimer dkTimer = new Timer.DKTimer();
            dkTimer.Start();

            dkTimer.Set(" ------ Lucene index path changed. ------ ");

            dkTimer.Set("Start " + WebVersion_Static + " Reload Cache");

            dkTimer.Set("Befor load Category Cache");
            CategoryController.Load(dkTimer);

            dkTimer.Set("Befor load Product Cache");
            ProductController.Load(dkTimer);

            dkTimer.Set("Befor load Retailer Cache");
            RetailerController.Load(dkTimer);

            dkTimer.Set("Befor load Review Cache");
            ReviewController.Load(dkTimer);

            dkTimer.Set("Befor load BuyingGuide Cache");
            BuyingGuideController.Load(dkTimer);

            dkTimer.Set("Befor load ResourcesInfo Cache");
            PriceMe.ResourcesInfoController.Load(PriceMe.WebConfig.CountryId);

            dkTimer.Set("Befor load Manufacturer Cache");
            ManufacturerController.Load(dkTimer);

            dkTimer.Set("Befor load Attribute Cache");
            AttributesController.Load(dkTimer);

            DFP_AdsChontroller.Load();

            ConsumerController.Load();

            WebSiteController.Load(PriceMe.WebConfig.CountryId);

            dkTimer.Set("End load Cache");

            dkTimer.Set(" ------ Lucene index path changed end. ------ ");

            dkTimer.Stop();

            LogController.WriteLog("------------\r\n" + dkTimer.OutputText() + "\r\n------------ at : " + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "\r\n");
        }
        catch (Exception ex)
        {
            string msg = ex.Message + "-----------" + ex.StackTrace;
            LogController.WriteException("------------\n" + msg);
            if (ex.InnerException != null)
            {
                LogController.WriteException("\t--------\nInnerException : " + ex.InnerException.Message + "-----------" + ex.InnerException.StackTrace + "\n");
            }

            PriceMeDBA.CSK_Store_ExceptionCollect exception = new PriceMeDBA.CSK_Store_ExceptionCollect();
            exception.ExceptionInfo = ex.Message + "-----------" + ex.StackTrace;
            exception.ExceptionType = "onStart";
            exception.ExceptionAppName = "PriceMe";
            exception.errorPagePath = "onStart";
            exception.Level = 0;
            exception.Save();
            return false;
        }
        return true;
    }
}