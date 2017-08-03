using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MT.Extend
{
    public class Config
    {
        public static string GetAppSetting(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }

        public static string GetConnection(string name)
        {
            return ConfigurationManager.ConnectionStrings[name].ConnectionString;
        }

        public static string RabbitMQ_Host
        {
            get { return GetAppSetting("RabbitMQ_Host"); }
        }

        public static string RabbitMQ_UserName
        {
            get { return GetAppSetting("RabbitMQ_UserName"); }
        }

        public static string RabbitMQ_Password
        {
            get { return GetAppSetting("RabbitMQ_Password"); }
        }

        public static string RabbitMQ_QueueName
        {
            get { return GetAppSetting("RabbitMQ_QueueName"); }
        }

        public static ushort RabbitMQ_Prefetch
        {
            get { return string.IsNullOrEmpty(Config.GetAppSetting("RabbitMQ_Prefetch")) ? (ushort)1 : Convert.ToUInt16(Config.GetAppSetting("RabbitMQ_Prefetch")); }
        }

    }
}
