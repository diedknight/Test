using com.alibaba.openapi.client.policy;
using com.alibaba.openapi.client.primitive;
using com.alibaba.openapi.client.util;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace com.alibaba.openapi.client.serialize
{
    class Param2RequestSerializer : Serializer
    {
        public String supportedContentType()
        {
            return Protocol.param2;
        }

        public Dictionary<String, Object> serialize(Object serializer)
        {
            Dictionary<String, Object> result = serializeNest(serializer);
            return result;
        }

        private Dictionary<String, Object> serializeNest(Object serializer)
        {
            Dictionary<String, Object> result = new Dictionary<String, Object>();
            if (serializer == null)
            {
                return result;
            }
            Type type = serializer.GetType();

            IEnumerable<FieldInfo> fis = type.GetRuntimeFields();
            TextInfo tInfo = Thread.CurrentThread.CurrentCulture.TextInfo;

            foreach (FieldInfo fi in fis)
            {
                Type fieldType = fi.FieldType;

                String piName = fi.Name;
                String firstCharacter = piName.Substring(0, 1);
                String upperFirstCharacter = firstCharacter.ToUpper(Thread.CurrentThread.CurrentCulture);
                String tempName = upperFirstCharacter + piName.Substring(1);

                MethodInfo mi = type.GetMethod("get" + tempName);
                object value = mi.Invoke(serializer, null);
                if (value != null)
                {
                    object trueValue = null;
                    if (fieldType.IsAssignableFrom(typeof(bool?))
                        ||fieldType.IsAssignableFrom(typeof(byte?))
                        ||fieldType.IsAssignableFrom(typeof(char?))
                        ||fieldType.IsAssignableFrom(typeof(double?))
                        ||fieldType.IsAssignableFrom(typeof(float?))
                        ||fieldType.IsAssignableFrom(typeof(int?))
                        ||fieldType.IsAssignableFrom(typeof(long?)))
                    {
                        trueValue = value;
                    }
                    else if (fieldType.IsAssignableFrom(typeof(String)))
                    {
                        if (value.GetType().IsAssignableFrom(typeof(DateTime)))
                        {
                            DateTime dateTime = (DateTime)value;
                            trueValue = DateUtil.formatForOcean(dateTime);
                        }
                        else
                        {
                            trueValue = value;
                        }
                    }
                    else if (fieldType.IsAssignableFrom(typeof(DateTime?)))
                    {
                        DateTime dateTime = (DateTime)value;
                        trueValue = DateUtil.format(dateTime);
                    }
                    else if (fieldType.IsAssignableFrom(typeof(Byte[])) || fieldType.IsAssignableFrom(typeof(byte[])))
                    {
                        trueValue = Convert.ToBase64String((byte[])value);
                    }
                    else
                    {
                        DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(fieldType);
                        MemoryStream stream = new MemoryStream();
                        dataContractJsonSerializer.WriteObject(stream, value);
                        byte[] dataBytes = new byte[stream.Length];
                        stream.Position = 0;
                        stream.Read(dataBytes, 0, (int)stream.Length);

                        string dataString = Encoding.UTF8.GetString(dataBytes);
                        trueValue = dataString;
                    }
                    result.Add(piName, trueValue);
                }
            }
            return result;
        }

    }
}
