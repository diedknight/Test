using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PriceMeCommon.Data;

namespace PriceMeCommon.BusinessLogic
{
    public static class IndexModifyController
    {
        public static event EventHandler IndexModifyEvent;
        static System.Threading.Thread checkModifyThread;

        public static void StartCheckModify()
        {
            checkModifyThread = new System.Threading.Thread(new System.Threading.ThreadStart(CheckModify));
            checkModifyThread.Start();
        }

        public static void EndCheck()
        {
            if (checkModifyThread != null && checkModifyThread.IsAlive)
                checkModifyThread.Abort();
        }

        public static void ReloadLuceneIndex()
        {
            LuceneController.LoadAllIndexSearcher();
        }

        static void CheckModify()
        {
            int interval = ConfigAppString.Interval;
            if (ConfigAppString.CountryID != 0)
            {
                CountryInfo currentCountryInfo = ConfigAppString.GetCountryInfo(ConfigAppString.CountryID);
                while (true)
                {
                    try
                    {
                        string indexPath = ConfigAppString.GetRealTimeIndexPath(ConfigAppString.CountryID);
                        if (!string.IsNullOrEmpty(indexPath) && indexPath != currentCountryInfo.LuceneIndexPath)
                        {
                            ConfigAppString.Init();
                            if (!LuceneController.LoadAllIndexSearcher())
                            {
                                LuceneController.LoadAllIndexSearcher();
                            }
                            currentCountryInfo = ConfigAppString.GetCountryInfo(ConfigAppString.CountryID);
                            if (IndexModifyEvent != null)
                                IndexModifyEvent(typeof(LuceneController), new EventArgs());
                        }
                    }
                    catch (Exception ex)
                    {
                        LogWriter.WriteLineToFile(ConfigAppString.ExceptionLogPath, ex.Message + "\t" + ex.StackTrace + Environment.NewLine + "------------------------------------");
                    }
                    System.Threading.Thread.CurrentThread.Join(interval);
                }
            }
        }
    }
}