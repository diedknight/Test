using com.alibaba.openapi.client.policy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.alibaba.openapi.client.serialize
{
    class SerializerProvider
    {
        private static SerializerProvider instance;
        private static Object lockObject=new Object();
        public static SerializerProvider getInstance()
        {
            if (instance == null)
            {
                lock (lockObject)
                {
                    if (instance == null)
                    {
                        instance = new SerializerProvider();
                        instance.initial();
                    }
                }
            }
            return instance;
        }

        private SerializerProvider()
        {

        }

        private void initial()
        {
            serializerStore.Add(Protocol.param2, new Param2RequestSerializer());
            deSerializerStore.Add(Protocol.param2, new Json2Deserializer());
            deSerializerStore.Add(Protocol.json2, new Json2Deserializer());
        }
    
        private  Dictionary<String, Serializer> serializerStore = new Dictionary<String, Serializer>();

        public  Serializer getSerializer(String contentType)
        {
            return serializerStore[contentType];
        }

        private  Dictionary<String, DeSerializer> deSerializerStore = new Dictionary<String, DeSerializer>();

        public  DeSerializer getDeSerializer(String contentType)
        {
            return deSerializerStore[contentType];
        }

        


    }
}
