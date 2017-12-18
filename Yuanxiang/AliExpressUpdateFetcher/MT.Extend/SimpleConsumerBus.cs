using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MT.Extend
{
    public class SimpleConsumerBus<TConsumer> where TConsumer :BaseConsumer, new()
    {
        private IBusControl _bus = null;

        public SimpleConsumerBus()
            : this(
            Config.RabbitMQ_Host,
            Config.RabbitMQ_UserName,
            Config.RabbitMQ_Password,
            Config.RabbitMQ_QueueName,
            Config.RabbitMQ_Prefetch)
        { }

        public SimpleConsumerBus(string hostAddress, string username, string password, string queueName, ushort prefetchCount = 1)
        {
            if (string.IsNullOrEmpty(queueName)) throw new Exception("queue name is null");
            if (string.IsNullOrEmpty(hostAddress)) throw new Exception("host address is null");
            if (string.IsNullOrEmpty(username)) throw new Exception("username is null");
            if (string.IsNullOrEmpty(password)) throw new Exception("password is null");

            this._bus = Bus.Factory.CreateUsingRabbitMq(x =>
            {
                var host = x.Host(new Uri(hostAddress), h =>
                {
                    h.Username(username);
                    h.Password(password);
                });

                x.ReceiveEndpoint(host, queueName, e =>
                {
                    e.PrefetchCount = prefetchCount;    //并发数concurrent
                    e.Consumer<TConsumer>();
                });
            });
        }

        public void Start()
        {
            this._bus.Start();
        }

        public void Stop()
        {
            this._bus.Stop();
        }

    }
}
