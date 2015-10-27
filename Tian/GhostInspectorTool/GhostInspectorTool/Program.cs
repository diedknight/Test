using GhostInspectorTool.GhostInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Test = GhostInspectorTool.GhostInspector.Test;
using Suite = GhostInspectorTool.GhostInspector.Suite;
using Result = GhostInspectorTool.GhostInspector.Result;
using System.Configuration;

namespace GhostInspectorTool
{
    class Program
    {

        static void Main(string[] args)
        {
            var api = APIFactory.Create<Test.List>(Config.GetAppSetting("apiKey"));
            var list = api.Request<List<Result.Test.ListResult>>();

            list.data.ForEach(item =>
            {
                var api1 = APIFactory.Create<Test.Execute>(Config.GetAppSetting("apiKey"), item._id);
                string newHost = Config.GetAppSetting(item.organization.name);
                if (newHost == null) return;

                Uri newUri = new Uri(newHost);
                Uri startUri = new Uri(item.startUrl);

                api1.startUrl = item.startUrl.Replace(startUri.Host, newUri.Host);
                api1.Request<object>();
            });
        }
    }
}
