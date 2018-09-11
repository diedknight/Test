using Newtonsoft.Json;
using ProductSearchIndexBuilder.Data;
using StackExchange.Redis;
using System;
using System.Collections.Generic;

namespace ProductSearchIndexBuilder
{
    public class RedisManager : IDisposable
    {
        private JsonSerializerSettings _jsonConfig = new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore, NullValueHandling = NullValueHandling.Ignore };
        private ConnectionMultiplexer _redis;
        private IDatabase _db;
        private string myInstanceName;

        public RedisManager(string configuration = "localhost", string instanceName = "priceme")
        {
            this._redis = ConnectionMultiplexer.Connect(configuration);
            this._db = this._redis.GetDatabase();
            myInstanceName = instanceName;
        }

        ~RedisManager()
        {
            this.Dispose();
        }

        public void Dispose()
        {
            this._redis.Dispose();
        }

        public void Set(string key, string value)
        {
            this._db.StringSet(myInstanceName + "_" + key, value);
        }

        public void Set<T>(string key, T data)
        {
            string json = JsonConvert.SerializeObject(data, this._jsonConfig);

            this.Set(key, json);
        }

        public void Remove(string key)
        {
            this._db.KeyDelete(myInstanceName + "_" + key);
        }

        public T Get<T>(string key) where T : class
        {
            string json = this._db.StringGet(myInstanceName + "_" + key);
            if (json != null)
            {
                return JsonConvert.DeserializeObject<T>(json);
            }
            else
            {
                return null;
            }
        }
    }
}