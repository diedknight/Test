using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Infrastructure.Serialize
{
    public class JsonSerialize : ISerialize
    {
        public string Serialize<T>(T obj) where T : class
        {
            return JsonConvert.SerializeObject(obj);
        }
        public T Deserialize<T>(string str) where T : class
        {
            return JsonConvert.DeserializeObject<T>(str);
        }
        public object Deserialize(string str, Type type)
        {
            return JsonConvert.DeserializeObject(str, type);
        }
    }
}
