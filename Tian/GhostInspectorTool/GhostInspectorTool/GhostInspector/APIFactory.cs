using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GhostInspectorTool.GhostInspector
{
    public class APIFactory
    {
        public static T Create<T>(string apiKey, params object[] args) where T : API, new()
        {
            var obj = (T)Activator.CreateInstance(typeof(T), args);
            obj.apiKey = apiKey;

            return obj;
        }
    }
}
