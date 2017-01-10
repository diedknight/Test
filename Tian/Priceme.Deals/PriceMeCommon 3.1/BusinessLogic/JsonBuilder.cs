using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PriceMeCommon
{
    public class JsonBuilder
    {
        static string keyValueFormat = "\"{0}\" : \"{1}\"";

        public static string GetJson(object obj)
        {
            if (obj == null)
            {
                return "";
            }

            string json = "{";
            Type type = obj.GetType();
            System.Reflection.PropertyInfo[] publicInstancePropertyInfos = type.GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);

            foreach (System.Reflection.PropertyInfo propertyInfo in publicInstancePropertyInfos)
            {
                string key = propertyInfo.Name;
                object objectValue = propertyInfo.GetValue(obj, null);
                if (objectValue == null)
                    continue;
                Type valueType = objectValue.GetType();
                if (valueType.IsValueType || valueType.Name.Equals("String"))
                {
                    json += string.Format(keyValueFormat, key, objectValue.ToString().Replace("\"", "").Replace("'", "").Replace("\r\n", " ").Replace("{", " ").Replace("}", " ")) + ",";
                }
                else
                {
                    System.Collections.IEnumerable iEnumerable = objectValue as System.Collections.IEnumerable;
                    if (iEnumerable != null)
                    {
                        json += "\"" + key + "\" : [";
                        foreach (object objValue in iEnumerable)
                        {
                            Type objType = objValue.GetType();
                            if (objType.IsValueType || objType.Name.Equals("String"))
                            {
                                json += "{" + string.Format(keyValueFormat, key, objValue.ToString().Replace("\"", "").Replace("'", "").Replace("\r\n", " ").Replace("{", " ").Replace("}", " ")) + "},";
                            }
                            else
                            {
                                string jsonString = GetJson(objValue);
                                json += jsonString + ",";
                            }
                        }
                        json = json.TrimEnd(',') + "],";
                    }
                }
            }

            json = json.TrimEnd(',') + "}";
            return json;
        }
    }
}