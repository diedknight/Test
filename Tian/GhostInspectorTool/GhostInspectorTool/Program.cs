using GhostInspectorTool.GhostInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Suite = GhostInspectorTool.GhostInspector.Suite;
using Result = GhostInspectorTool.GhostInspector.Result;
using System.Configuration;

namespace GhostInspectorTool
{
    class Program
    {

        static void Main(string[] args)
        {
            var api = APIFactory.Create<Suite.List>(Config.GetAppSetting("apiKey"));
            var list = api.Request<List<Result.Suite.ListResult>>();

            list.data.ForEach(item =>
            {
                var api1 = APIFactory.Create<Suite.Execute>(Config.GetAppSetting("apiKey"), item._id);
                string startUrl = Config.GetAppSetting(item.organization.name);
                if (startUrl == null) return;

                api1.startUrl = startUrl;
                api1.Request<List<Result.Suite.ExecuteResult>>();
            });
        }
    }
}
