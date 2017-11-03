using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Service.Infrastructure.Serialize
{
    public class BinarySerialize : IBinarySerialize
    {
        private readonly string _basePath = AppDomain.CurrentDomain.BaseDirectory + "\\Data";
        public byte[] Serialize<T>(string fileName, T obj) where T : class
        {
            if (!Directory.Exists(this._basePath))
            {
                Directory.CreateDirectory(this._basePath);
            }
            byte[] array = null;
            string path = Path.Combine(this._basePath, fileName);
            IFormatter formatter = new BinaryFormatter();
            using (Stream stream = new FileStream(path, FileMode.Create, FileAccess.ReadWrite, FileShare.None))
            {
                formatter.Serialize(stream, obj);
                array = new byte[stream.Length];
                stream.Seek(0L, SeekOrigin.Begin);
                stream.Read(array, 0, array.Length);
            }
            return array;
        }
        public byte[] Serialize<T>(T obj) where T : class
        {
            byte[] array = null;
            IFormatter formatter = new BinaryFormatter();
            using (Stream stream = new MemoryStream())
            {
                formatter.Serialize(stream, obj);
                array = new byte[stream.Length];
                stream.Seek(0L, SeekOrigin.Begin);
                stream.Read(array, 0, array.Length);
            }
            return array;
        }
        public T Deserialize<T>(string fileName) where T : class
        {
            string path = Path.Combine(this._basePath, fileName);
            T result;
            if (!Directory.Exists(this._basePath))
            {
                result = default(T);
            }
            else
            {
                if (!File.Exists(path))
                {
                    result = default(T);
                }
                else
                {
                    IFormatter formatter = new BinaryFormatter();
                    using (Stream stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        result = (T)((object)formatter.Deserialize(stream));
                    }
                }
            }
            return result;
        }
        public T Deserialize<T>(byte[] bs) where T : class
        {
            IFormatter formatter = new BinaryFormatter();
            T result;
            using (Stream stream = new MemoryStream(bs))
            {
                result = (T)((object)formatter.Deserialize(stream));
            }
            return result;
        }
    }
}
