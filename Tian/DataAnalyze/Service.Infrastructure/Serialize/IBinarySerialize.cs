using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Infrastructure.Serialize
{
    public interface IBinarySerialize
    {
        byte[] Serialize<T>(string fileName, T obj) where T : class;
        byte[] Serialize<T>(T obj) where T : class;
        T Deserialize<T>(string fileName) where T : class;
        T Deserialize<T>(byte[] bs) where T : class;
    }
}
