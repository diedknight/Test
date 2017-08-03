
//using FSuite;
using MassTransit;
using MassTransit.Logging;
using MT.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MT.Extend
{
    public abstract class BaseConsumer : IConsumer
    { }

    public abstract class BaseConsumer<TContract> : BaseConsumer, IConsumer<TContract> where TContract : class, IContract
    {
        public async Task Consume(ConsumeContext<TContract> context)
        {
            await Task.Factory.StartNew(() =>
            {
                try
                {
                    var simpleContext = new SimpleConsumeContext<TContract>(context);

                    this.ConumeHandleBefore(simpleContext);
                    this.ConsumeHandle(simpleContext);
                    this.ConumeHandleAfter(simpleContext);
                }
                catch (Exception ex)
                {
                    //XbaiLog.WriteLog("error", ex.Message);
                    //XbaiLog.WriteLog("error", ex.StackTrace);
                }
            });
        }

        protected virtual void ConumeHandleBefore(SimpleConsumeContext<TContract> context)
        { }

        protected virtual void ConumeHandleAfter(SimpleConsumeContext<TContract> context)
        { }

        protected abstract void ConsumeHandle(SimpleConsumeContext<TContract> context);
    }
}
