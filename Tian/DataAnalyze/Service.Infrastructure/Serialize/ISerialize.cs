using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Infrastructure.Serialize
{
    public interface ISerialize
    {
        string Serialize<T>(T obj) where T : class;
        T Deserialize<T>(string str) where T : class;
        object Deserialize(string str, Type type);

    }
}
