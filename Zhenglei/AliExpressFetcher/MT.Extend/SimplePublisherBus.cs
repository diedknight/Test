using MassTransit;
using MT.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MT.Extend
{
    public class SimplePublisherBus
    {
        private IBusControl _bus = null;
        private string _host = "";

        public SimplePublisherBus()
            : this(
            Config.RabbitMQ_Host,
            Config.RabbitMQ_UserName,
            Config.RabbitMQ_Password)
        { }

        public SimplePublisherBus(string hostAddress, string username, string password)
        {
            if (string.IsNullOrEmpty(hostAddress)) throw new Exception("host address is null");
            if (string.IsNullOrEmpty(username)) throw new Exception("username is null");
            if (string.IsNullOrEmpty(password)) throw new Exception("password is null");

            this._host = hostAddress;
            this._bus = Bus.Factory.CreateUsingRabbitMq(sbc =>
            {
                var host = sbc.Host(new Uri(hostAddress), h =>
                {
                    h.Username(username);
                    h.Password(password);
                });
            });
        }

        //public SimplePublisherBus(IBusControl bus)
        //{
        //    this._bus = bus;
        //    this._host = bus.Address.Scheme + "://" + bus.Address.Host + "/";
        //}

        public void Start()
        {
            this._bus.Start();
        }

        public void Stop()
        {
            this._bus.Stop();            
        }

        public void Publish<TContract>(DateTime scheduledTime, TContract message) where TContract : class, IContract
        {
            var contractType = typeof(TContract);

            if (!contractType.IsInterface) throw new Exception("TContract type must be interface");

            string destinationAddress = this._host + contractType.Namespace + ":" + contractType.Name;

            this._bus.Publish<TContract>(message);

            //this._bus.ScheduleMessage<TContract>(new Uri(destinationAddress), scheduledTime, message, null);
        }

        public void Publish<TContract>(TContract message) where TContract : class, IContract
        {
            if (!typeof(TContract).IsInterface) throw new Exception("TContract type must be interface");

            this.Publish<TContract>(DateTime.Now, message);            
        }
    }
}
