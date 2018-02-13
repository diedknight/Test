using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PushSharp.Apple;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;

namespace IOSPush
{
    public class AppleApns
    {
        public static void Push(AppleApns.ProductInfo productInfo, params string[] tokens)
        {            
            Assembly asm = Assembly.GetAssembly(typeof(AppleApns));
            Stream strm = asm.GetManifestResourceStream("IOSPush.apns_push_cer.p12");

            byte[] bt = new byte[strm.Length];
            strm.Read(bt, 0, Convert.ToInt32(strm.Length));
            
            // Configuration (NOTE: .pfx can also be used here)
            var config = new ApnsConfiguration(ApnsConfiguration.ApnsServerEnvironment.Sandbox,
                bt, "");

            // Create a new broker
            var apnsBroker = new ApnsServiceBroker(config);

            // Wire up events
            apnsBroker.OnNotificationFailed += (notification, aggregateEx) =>
            {

                aggregateEx.Handle(ex =>
                {

                    // See what kind of exception it was to further diagnose
                    if (ex is ApnsNotificationException)
                    {
                        var notificationException = (ApnsNotificationException)ex;

                        // Deal with the failed notification
                        var apnsNotification = notificationException.Notification;
                        var statusCode = notificationException.ErrorStatusCode;

                        Console.WriteLine($"Apple Notification Failed: ID={apnsNotification.Identifier}, Code={statusCode}");

                    }
                    else
                    {
                        // Inner exception might hold more useful information like an ApnsConnectionException			
                        Console.WriteLine($"Apple Notification Failed for some unknown reason : {ex.InnerException}");
                    }

                    // Mark it as handled
                    return true;
                });
            };

            apnsBroker.OnNotificationSucceeded += (notification) =>
            {
                Console.WriteLine("Apple Notification Sent!");
            };

            // Start the broker
            apnsBroker.Start();

            string jsonStr = JsonConvert.SerializeObject(productInfo, Formatting.Indented);

            tokens.ToList().ForEach(token =>
            {
                apnsBroker.QueueNotification(new ApnsNotification
                {
                    DeviceToken = token,
                    Payload = JObject.Parse(jsonStr)
                });
            });

            // Stop the broker, wait for it to finish   
            // This isn't done after every message, but after you're
            // done with the broker
            apnsBroker.Stop();
        }

        public class ProductInfo
        {
            public ProductInfo()
            {
                this.aps = new APS();
                this.ProductId = 0;
                this.ProductName = "";
                this.ProductBestPrice = 0m;
                this.AlertMsg = "";
                this.ProductImage = "";
                this.CountryId = 0;
            }

            [JsonProperty("aps")]
            private APS aps { get; set; }

            public int ProductId { get; set; }
            public string ProductName { get; set; }
            public decimal ProductBestPrice { get; set; }

            public string AlertMsg
            {
                get { return this.aps.alert.body; }
                set { this.aps.alert.body = value; }
            }

            public string ProductImage { get; set; }
            public int CountryId { get; set; }


            private class APS
            {
                public APS()
                {
                    this.alert = new Alert();
                    this.badge = 1;
                    this.sound = "default";
                    this.mutable_content = "1";
                }

                public Alert alert { get; set; }
                public int badge { get; set; }
                public string sound { get; set; }

                [JsonProperty("mutable-content")]
                public string mutable_content { get; set; }
            }

            private class Alert
            {
                public Alert()
                {
                    this.action_loc_key = "Open";
                    this.title = "Price Alert";
                    this.body = "";                    
                }

                public string title { get; set; }
                public string body { get; set; }

                [JsonProperty("action-loc-key")]
                public string action_loc_key { get; set; }            
            }


        }
    }
}
