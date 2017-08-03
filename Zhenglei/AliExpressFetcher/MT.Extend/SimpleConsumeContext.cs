using MassTransit;
using MT.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MT.Extend
{
    public class SimpleConsumeContext<TContract> where TContract : class, IContract
    {
        private ConsumeContext<TContract> _context = null;

        public TContract Message { get; private set; }

        public SimpleConsumeContext(ConsumeContext<TContract> context)
        {
            this._context = context;
            this.Message = context.Message;
        }

        public void Publish<T>(T message) where T : class, IContract
        {
            if (!typeof(T).IsInterface) throw new Exception("TContract type must be interface");

            this._context.Send<T>(this.GetAdress(typeof(T)), message);
        }

        private Uri GetAdress(Type type) 
        {
            string host = this._context.DestinationAddress.Scheme + "://" + this._context.DestinationAddress.Host + "/";
            string address = host + type.Namespace + ":" + type.Name;

            return new Uri(address);
        }
    }
}
