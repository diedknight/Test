using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AliexpressImport.BusinessLogic;
using MT.Extend;
using AliexpressImport.CustomConsumer;
using System.Diagnostics;
using AliexpressImport.Data;

namespace AliexpressImport
{
    class Program
    {
        static DateTime startTime;
        static void Main(string[] args)
        {
            try
            {
                startTime = DateTime.Now;

                //创建Log报告
                OutManagerContronller.Load();

                OutManagerContronller.WriterInfo(TraceEventType.Verbose, "Start Import...");

                //测试
                string filepath = System.Configuration.ConfigurationManager.AppSettings["TestFeedPath"].ToString();
                CrawlerParameter parameter = new CrawlerParameter();
                parameter.FilePath = filepath;

                ImportProduct ip = new ImportProduct();
                ip.ProcessFeed((object)parameter);

                //CheckRabbitMQ();

                OutManagerContronller.WriterInfo(TraceEventType.Verbose, "End...");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }

            OutManagerContronller.Close();
        }

        static void CheckRabbitMQ()
        {
            ////总共运行时间
            float time = ConfigAppString.EndTaskTime * 60;
            int lastTime = int.Parse(time.ToString());
            lastTime = 60 * 1000 * lastTime;

            SimpleConsumerBus bus = new SimpleConsumerBus<Consumer>();
            
            try
            {
                bus.Start();

                OutManagerContronller.WriterInfo(TraceEventType.Verbose, "Start Read Messages...");

                while (true)
                {
                    string exit = Console.ReadLine();
                    if (exit == "exit") break;
                }

                bus.Stop();
            }
            catch (Exception ex) { OutManagerContronller.WriterInfo(TraceEventType.Error, "Check Message... " + ex.Message + " " + ex.StackTrace); }

            OutManagerContronller.WriterInfo(TraceEventType.Verbose, "Start deal with expired products...");
        }
    }
}
