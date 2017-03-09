using Priceme.Infrastructure.Email;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Topshelf;

namespace MonitorPriceMeApi
{
    public class TownCrier
    {
        readonly Timer _timer;
        public TownCrier()
        {
            _timer = new Timer(1000 * 60 * 5) { AutoReset = true };
            _timer.Elapsed += (sender, eventArgs) =>
            {
                IEmail email = null;
                string address = "";
                string WSAccessKey = "";
                string WSSecretKey = "";
                string to = "";

                try
                {
                    address = System.Configuration.ConfigurationManager.AppSettings["Email"];
                    WSAccessKey = System.Configuration.ConfigurationManager.AppSettings["WSAccessKey"];
                    WSSecretKey = System.Configuration.ConfigurationManager.AppSettings["WSSecretKey"];
                    to = System.Configuration.ConfigurationManager.AppSettings["to"];

                    email = Factory.Create(address, WSAccessKey, WSSecretKey);

                    nz.co.priceme.api.PriceMeService ps = new nz.co.priceme.api.PriceMeService();
                    var a = ps.C_GetAllRootCategory();
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                    e += "   " + ex.StackTrace;

                    email.Send("Monitor priceme api", e, to);
                }
            };
        }

        public void Start() { _timer.Start(); }
        public void Stop() { _timer.Stop(); }
    }

    class Program
    {
        static void Main(string[] args)
        {
            HostFactory.Run(x =>
            {
                x.Service<TownCrier>(s =>
                {
                    s.ConstructUsing(name => new TownCrier());
                    s.WhenStarted(tc => tc.Start());
                    s.WhenStopped(tc => tc.Stop());
                });
                x.RunAsLocalSystem();

                x.SetDescription("Xbai");
                x.SetDisplayName("MonitorPriceMeApi");
                x.SetServiceName("MonitorPriceMeApi");
            });
        }
    }
}
