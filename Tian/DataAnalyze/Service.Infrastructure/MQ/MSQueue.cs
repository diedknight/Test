using Service.Infrastructure.Log;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace Service.Infrastructure.MQ
{
    public class MSQueue : IQueue
    {
        private static readonly object obj = new object();
        private string key = "";

        public MSQueue(string key = "MSMQ")
        {
            this.key = key;
        }

        public void Send<T>(params T[] items)
        {
            lock (MSQueue.obj)
            {
                MessageQueue messageQueue = new MessageQueue(ConfigurationManager.AppSettings[this.key]);
                messageQueue.Formatter = new BinaryMessageFormatter();

                using (MessageQueueTransaction messageQueueTransaction = new MessageQueueTransaction())
                {
                    try
                    {
                        messageQueueTransaction.Begin();
                        for (int i = 0; i < items.Length; i++)
                        {
                            T t = items[i];
                            Message message = new Message(t, new BinaryMessageFormatter());
                            messageQueue.Send(message, messageQueueTransaction);
                        }
                        messageQueueTransaction.Commit();
                        
                    }
                    catch (Exception ex)
                    {
                        messageQueueTransaction.Abort();
                        XbaiLog.WriteLog("QueueLog", ex.Message);
                    }
                }
                messageQueue.Close();
            }
        }

        public T Receive<T>()
        {
            T result;
            lock (MSQueue.obj)
            {
                T t = default(T);
                MessageQueue messageQueue = new MessageQueue(ConfigurationManager.AppSettings[this.key]);
                messageQueue.Formatter = new BinaryMessageFormatter();
                if (!messageQueue.CanRead)
                {
                    XbaiLog.WriteLog("QueueLog", "CDCLog Can't Read!");
                    result = default(T);
                }
                else
                {
                    using (MessageQueueTransaction messageQueueTransaction = new MessageQueueTransaction())
                    {
                        try
                        {
                            messageQueueTransaction.Begin();
                            Message message = messageQueue.Receive(TimeSpan.FromSeconds(3.0), messageQueueTransaction);
                            message.Formatter = new BinaryMessageFormatter();
                            messageQueueTransaction.Commit();
                            t = (T)((object)message.Body);
                        }
                        catch(Exception ex)
                        {
                            //XbaiLog.WriteLog("QueueLog", ex.Message);
                            messageQueueTransaction.Abort();
                            result = default(T);
                            return result;
                        }
                    }
                    messageQueue.Close();
                    result = t;
                }
            }
            return result;
        }
    }
}
