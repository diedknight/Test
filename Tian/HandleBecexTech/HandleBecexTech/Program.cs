using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace HandleBecexTech
{
    class Program
    {
        static void Main(string[] args)
        {
            using (WebClient wc = new WebClient())
            {
                var bts = wc.DownloadData(AppConfig.FeedFileURL);

                using (MemoryStream ms = new MemoryStream(bts))
                {

                    XmlDocument doc = new XmlDocument();
                    doc.Load(ms);
                    var rootNode = doc.SelectSingleNode("Feed");
                    var nodes = doc.SelectNodes("Feed/Product");

                    foreach (XmlNode node in nodes)
                    {
                        try
                        {
                            var priceNode = node.SelectSingleNode("Price");
                            var price = Convert.ToDecimal(priceNode.InnerText);

                            if (price >= 400)
                            {
                                rootNode.RemoveChild(node);
                            }
                        }
                        catch (Exception ex)
                        {
                            string logFile = Path.Combine(AppConfig.NewFeedPath, "log" + DateTime.Now.ToString("yyyy_MM_dd") + ".txt");

                            XbaiLog.WriteLog(logFile, ex.Message);
                            XbaiLog.WriteLog(logFile, ex.StackTrace);
                        }
                    }

                    string fileName = Path.Combine(AppConfig.NewFeedPath, AppConfig.NewFeedName);
                    doc.Save(fileName);
                }

            }

        }
    }
}
