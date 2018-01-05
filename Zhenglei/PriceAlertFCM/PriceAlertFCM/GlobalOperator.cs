using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Web.Script.Serialization;
using System.Configuration;

namespace PriceAlertFCM
{
    public class GlobalOperator
    {
        static readonly string ApplicationId_Static = ConfigurationManager.AppSettings["ApplicationId"].ToString();
        static readonly string SenderId_Static = ConfigurationManager.AppSettings["SenderId"].ToString();

        public static string SendPushNotification(PriceAlertInfo pai, string token)
        {
            string rs = string.Empty;
            try
            {
                string dataJson = Newtonsoft.Json.JsonConvert.SerializeObject(pai);

                string deviceId = token;

                WebRequest tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
                tRequest.Method = "post";
                tRequest.ContentType = "application/json";
                var data = new
                {
                    to = deviceId,
                    notification = new
                    {
                        body = "1:" + dataJson,
                        sound = "Enabled"
                    }
                };
                var serializer = new JavaScriptSerializer();
                var json = serializer.Serialize(data);
                Byte[] byteArray = Encoding.UTF8.GetBytes(json);
                tRequest.Headers.Add(string.Format("Authorization: key={0}", ApplicationId_Static));
                tRequest.Headers.Add(string.Format("Sender: id={0}", SenderId_Static));
                tRequest.ContentLength = byteArray.Length;
                using (Stream dataStream = tRequest.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                    using (WebResponse tResponse = tRequest.GetResponse())
                    {
                        using (Stream dataStreamResponse = tResponse.GetResponseStream())
                        {
                            using (StreamReader tReader = new StreamReader(dataStreamResponse))
                            {
                                string sResponseFromServer = tReader.ReadToEnd();
                                rs = sResponseFromServer;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                rs = ex.Message;
            }

            return rs;
        }

        public static string FixImagePath(string path)
        {
            string str = path.Replace("\\", "/");
            if (!str.StartsWith("/") && !str.StartsWith("http:") && !str.StartsWith("https:"))
            {
                str = "/" + str;
            }
            return str;
        }
    }
}