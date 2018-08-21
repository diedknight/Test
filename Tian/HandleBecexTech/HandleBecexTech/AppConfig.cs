using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HandleBecexTech
{
    public class AppConfig
    {
        public static string FeedFileURL { get; set; }
        public static string NewFeedPath { get; set; }
        public static string NewFeedName { get; set; }
        public static string LogPath { get; set; }

        static AppConfig()
        {
            FeedFileURL = ConfigurationManager.AppSettings["FeedFileURL"];
            NewFeedPath = ConfigurationManager.AppSettings["NewFeedPath"];
            NewFeedName = ConfigurationManager.AppSettings["NewFeedName"];
            LogPath = ConfigurationManager.AppSettings["LogPath"];
        }


    }
}
