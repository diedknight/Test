using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace PopualerSearchIndexBuilder
{
    public static class LogController
    {
        static LogWriter LogWriter_Static;
        static LogWriter ExceptionWriter_Static;

        public static void WriteLog(string log)
        {
            LogWriter_Static.WriteLine(log);
            Console.WriteLine(log);
        }

        public static void WriteException(string ex)
        {
            ExceptionWriter_Static.WriteLine(ex);
            Console.WriteLine(ex);
        }

        internal static void Init(IConfigurationRoot configuration)
        {
            string logDirectory = configuration["LogDirectory"];

            var exceptionLogPath = System.IO.Path.Combine(logDirectory, "Ex\\Excetpion" + DateTime.Now.ToString("yyyy-MM-dd_HH") + ".txt");
            var logPath = System.IO.Path.Combine(logDirectory, "Log\\Log" + DateTime.Now.ToString("yyyy-MM-dd_HH") + ".txt");

            LogWriter_Static = new LogWriter(logPath);
            ExceptionWriter_Static = new LogWriter(exceptionLogPath);
        }
    }
}