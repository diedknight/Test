using System;
using System.Configuration;

namespace PriceMeCommon.BusinessLogic
{
    public static class LogController
    {
        static string ExceptionLogPath_Static;
        static string LogPath_Static;

        static LogController()
        {
            Init();
        }

        public static void Init()
        {
            string logDirectory = ConfigurationManager.AppSettings["LogDirectory"];

            ExceptionLogPath_Static = logDirectory + "Ex\\Excetpion" + DateTime.Now.ToString("yyyy-MM-dd_HH") + ".txt";
            LogPath_Static = logDirectory + "Log\\Log" + DateTime.Now.ToString("yyyy-MM-dd_HH") + ".txt";
        }

        public static void WriteLog(string log)
        {
            LogWriter.FileLogWriter.WriteLine(LogPath_Static, log);
            Console.WriteLine(log);
        }

        public static void WriteException(string ex)
        {
            LogWriter.FileLogWriter.WriteLine(ExceptionLogPath_Static, ex);
            Console.WriteLine(ex);
        }
    }
}
