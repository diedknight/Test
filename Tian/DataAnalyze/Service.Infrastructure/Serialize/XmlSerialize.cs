using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Service.Infrastructure.Serialize
{
    public class XmlSerialize : ISerialize
    {
        public string Serialize<T>(T obj) where T : class
        {
            string result;
            if (obj == null)
            {
                result = "";
            }
            else
            {
                string text = "";
                XmlSerializer xmlSerializer = new XmlSerializer(obj.GetType());
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    XmlSerializerNamespaces xmlSerializerNamespaces = new XmlSerializerNamespaces();
                    xmlSerializerNamespaces.Add("", "");
                    using (XmlWriter xmlWriter = XmlWriter.Create(memoryStream, new XmlWriterSettings
                    {
                        Encoding = new UTF8Encoding(false)
                    }))
                    {
                        xmlSerializer.Serialize(xmlWriter, obj, xmlSerializerNamespaces);
                    }
                    text = Encoding.UTF8.GetString(memoryStream.ToArray());
                }
                result = text;
            }
            return result;
        }

        public T Deserialize<T>(string str) where T : class
        {
            T result;
            using (StringReader stringReader = new StringReader(str))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
                result = (T)((object)xmlSerializer.Deserialize(stringReader));
            }
            return result;
        }

        public object Deserialize(string str, Type type)
        {
            using (StringReader stringReader = new StringReader(str))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(type);
                return xmlSerializer.Deserialize(stringReader);
            }
        }

    }
}
