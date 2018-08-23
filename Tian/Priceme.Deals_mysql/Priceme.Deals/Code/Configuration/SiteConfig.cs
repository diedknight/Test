using System;
using System.Data;
using System.Collections;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Web.Configuration;
using PriceMeCommon;
using PriceMeCommon.Data;
using System.Collections.Generic;
using PriceMeCache;

public static class SiteConfig
{
    public static void Load()
    {
        try
        {
            int countryID = PriceMeCommon.ConfigAppString.CountryID;
            Timer.DKTimer dkTimer = new Timer.DKTimer();
            dkTimer.Start();

            dkTimer.Set(" ------ Website restart. ------ ");

            dkTimer.Set("Start load Cache");

            dkTimer.Set("Befor load Category Cache");
            if(ConfigAppString.StartDebug)
                PriceMeCommon.CategoryController.Load(dkTimer);
            else
                PriceMeCommon.CategoryController.Load();
            
            dkTimer.Set("Befor load Product Cache");
            if (ConfigAppString.StartDebug)
                PriceMeCommon.BusinessLogic.ProductController.Load(dkTimer);
            else
                PriceMeCommon.BusinessLogic.ProductController.Load();

            dkTimer.Set("Befor load Retailer Cache");
            PriceMeCommon.RetailerController.Load(dkTimer);

            dkTimer.Set("Befor load Review Cache");
            PriceMeCommon.ReviewController.Load();

            dkTimer.Set("Befor load BuyingGuide Cache");
            PriceMeCommon.BuyingGuideController.LoadBuyingGuideAndMaps();

            dkTimer.Set("Befor load ResourcesInfo Cache");
            PriceMeCommon.ResourcesInfoController.Load();

            dkTimer.Set("Befor load Manufacturer Cache"); 
            if (ConfigAppString.StartDebug)
                PriceMeCommon.ManufacturerController.Load(dkTimer);
            else
                PriceMeCommon.ManufacturerController.Load();

            if (ConfigAppString.CountryID == 3)
            {
                dkTimer.Set("Befor load MobilePlan Cache");
                if (ConfigAppString.StartDebug)
                    PriceMeCommon.MobilePlanController.Load(dkTimer);
                else
                    PriceMeCommon.MobilePlanController.Load();
            }
            dkTimer.Set("Befor load Attribute Cache");
            PriceMeCommon.AttributesController.Load(dkTimer);//dkTimer
            PriceMeCommon.ProductDescriptorController.Load(dkTimer);//dkTimer

            dkTimer.Set("Befor load SpaceIDsController Cache");
            PriceMe.SpaceIDsController.Init();

            dkTimer.Set("Befor load allExpertReviewSources");
            Dictionary<int, ExpertReviewSource> allExpertReviewSources = VelocityController.GetCache<Dictionary<int, ExpertReviewSource>>(VelocityCacheKey.ExpertReviewSource);
            if (allExpertReviewSources == null)
            {
                LogWriter.FileLogWriter.WriteLine(PriceMeCommon.ConfigAppString.LogPath, "AllExpertReviewSources no velocity");
            }
            else if (allExpertReviewSources.Count == 0)
                LogWriter.FileLogWriter.WriteLine(PriceMeCommon.ConfigAppString.LogPath, "AllExpertReviewSources count 0.");

            //BannerController.SaveToDB();

            DFP_AdsChontroller.Load();

            ConsumerController.Load();

            dkTimer.Set("End load Cache");

            dkTimer.Set(" ------ Website restart end. ------ ");

            dkTimer.Stop();

            LogWriter.FileLogWriter.WriteLine(PriceMeCommon.ConfigAppString.LogPath, "------------\r\n" + dkTimer.OutputText() + "\r\n------------ at : " + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "\r\n");

            PriceMeCommon.BusinessLogic.IndexModifyController.StartCheckModify();

            PriceMeCommon.BusinessLogic.IndexModifyController.IndexModifyEvent += new EventHandler(IndexModifyController_IndexModifyEvent);
        }
        catch (Exception ex)
        {
            string msg = ex.Message + "-----------" + ex.StackTrace;
            LogWriter.FileLogWriter.WriteLine(PriceMeCommon.ConfigAppString.ExceptionLogPath, "------------\n" + msg);
            if (ex.InnerException != null)
            {
                LogWriter.FileLogWriter.WriteLine(PriceMeCommon.ConfigAppString.ExceptionLogPath, "\t--------\nInnerException : " + ex.InnerException.Message + "-----------" + ex.InnerException.StackTrace + "\n");
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

    static void IndexModifyController_IndexModifyEvent(object sender, EventArgs e)
    {
        ReLoad();
    }

    public static bool ReLoad()
    {
        try
        {
            string indexPath = ConfigAppString.GetRealTimeIndexPath(ConfigAppString.CountryID);
            Timer.DKTimer dkTimer = new Timer.DKTimer();
            dkTimer.Start();

            dkTimer.Set(" ------ Lucene index path changed. ------ ");

            dkTimer.Set("Start Reload Cache");

            dkTimer.Set("Index Path : " + indexPath);

            dkTimer.Set("Befor load Category Cache");
            PriceMeCommon.CategoryController.Load();

            dkTimer.Set("Befor load Product Cache");
            PriceMeCommon.BusinessLogic.ProductController.Load();

            dkTimer.Set("Befor load Retailer Cache");
            PriceMeCommon.RetailerController.Load();

            dkTimer.Set("Befor load Review Cache");
            PriceMeCommon.ReviewController.Load();

            dkTimer.Set("Befor load BuyingGuide Cache");
            PriceMeCommon.BuyingGuideController.LoadBuyingGuideAndMaps();

            dkTimer.Set("Befor load ResourcesInfo Cache");
            PriceMeCommon.ResourcesInfoController.Load();

            dkTimer.Set("Befor load Manufacturer Cache");
            if (ConfigAppString.StartDebug)
                PriceMeCommon.ManufacturerController.Load(dkTimer);
            else
                PriceMeCommon.ManufacturerController.Load();

            dkTimer.Set("Befor load Attribute Cache");
            PriceMeCommon.AttributesController.Load(dkTimer);
            PriceMeCommon.ProductDescriptorController.Load(dkTimer);

            dkTimer.Set("Befor load allExpertReviewSources");
            Dictionary<int, ExpertReviewSource> allExpertReviewSources = VelocityController.GetCache<Dictionary<int, ExpertReviewSource>>(VelocityCacheKey.ExpertReviewSource);
            if (allExpertReviewSources == null)
            {
                LogWriter.FileLogWriter.WriteLine(PriceMeCommon.ConfigAppString.LogPath, "AllExpertReviewSources no velocity");
            }
            else if (allExpertReviewSources.Count == 0)
                LogWriter.FileLogWriter.WriteLine(PriceMeCommon.ConfigAppString.LogPath, "AllExpertReviewSources count 0.");

            //Dictionary<int, List<ExpertReview>> expretReviews = VelocityController.GetCache<Dictionary<int, List<ExpertReview>>>(VelocityCacheKey.AllExpertReviews);
            //if (expretReviews == null)
            //{
            //    LogWriter.FileLogWriter.WriteLine(PriceMeCommon.ConfigAppString.LogPath, "AllExpertReviews no velocity");
            //}

            //BannerController.SaveToDB();
            //BannerController.Init();

            DFP_AdsChontroller.Load();

            ConsumerController.Load();

            dkTimer.Set("End load Cache");

            dkTimer.Set(" ------ Lucene index path changed end. ------ ");

            dkTimer.Stop();

            LogWriter.FileLogWriter.WriteLine(PriceMeCommon.ConfigAppString.LogPath, "------------\r\n" + dkTimer.OutputText() + "\r\n------------ at : " + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "\r\n");
        }
        catch (Exception ex)
        {
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