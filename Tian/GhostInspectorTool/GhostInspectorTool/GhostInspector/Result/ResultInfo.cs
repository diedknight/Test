using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace GhostInspectorTool.GhostInspector.Result
{
    public class ResultInfo<T> 
    {

        public string code { get; set; }

        public T data { get; set; }

        //public Dictionary<string, object> result { get; set; }

        //public T GetValue<T>(string key, T defValue)
        //{
        //    if (result == null) return defValue;
        //    if (!result.ContainsKey(key)) return defValue;

        //    return (T)result[key];
        //}

    }
}
