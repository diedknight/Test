using PriceMeCommon.Data;
using System;
using System.Collections.Generic;

namespace PriceMeCommon.BusinessLogic
{
    public static class ConvertController<T_target, T_useToConvert>
        where T_target : class, new()
        where T_useToConvert : class, new()
    {
        public static T_target ConvertData(T_useToConvert dataToConvert, ConvertMap convertMap)
        {
            Type type = typeof(T_target);
            Type typeToConvert = dataToConvert.GetType();

            System.Reflection.PropertyInfo[] targetpropertyInfos = type.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
            System.Reflection.ConstructorInfo[] constructorInfos = type.GetConstructors(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);

            if (constructorInfos.Length > 0)
            {
                foreach (System.Reflection.ConstructorInfo constructorInfo in constructorInfos)
                {
                    if (constructorInfo.GetParameters().Length == 0)
                    {
                        T_target result = (T_target)constructorInfo.Invoke(null);
                        foreach (System.Reflection.PropertyInfo targetpropertyInfo in targetpropertyInfos)
                        {
                            System.Reflection.PropertyInfo propertyInfo = GetConvertProperty(typeToConvert, targetpropertyInfo.Name, convertMap);
                            if (propertyInfo != null)
                            {
                                object value = propertyInfo.GetValue(dataToConvert, null);
                                if (value != null)
                                {
                                    if (propertyInfo.PropertyType != targetpropertyInfo.PropertyType &&
                                        !targetpropertyInfo.PropertyType.Name.Contains("Nullable") &&
                                        !propertyInfo.PropertyType.Name.Contains("Nullable"))
                                    {
                                        value = System.Convert.ChangeType(value, targetpropertyInfo.PropertyType);
                                    }
                                    targetpropertyInfo.SetValue(result, value, null);
                                }
                            }
                            else
                            {

                            }
                        }
                        return result;
                    }
                }
            }

            return null;
        }

        private static System.Reflection.PropertyInfo GetConvertProperty(Type typeToConvert, string propertyName, ConvertMap convertMap)
        {
            System.Reflection.PropertyInfo propertyInfo = typeToConvert.GetProperty(propertyName);
            if (propertyInfo == null && convertMap != null)
            {
                string map = convertMap.GetMap(propertyName);
                if (!string.IsNullOrEmpty(map))
                {
                    propertyInfo = typeToConvert.GetProperty(map);
                }
            }
            return propertyInfo;
        }

        public static T_target ConvertData(T_useToConvert dataToConvert)
        {
            return ConvertData(dataToConvert, null);
        }

        public static List<T_target> ConvertData(IEnumerable<T_useToConvert> dataListToConvert, ConvertMap convertMap)
        {
            List<T_target> tList = new List<T_target>();

            foreach (T_useToConvert tToConvert in dataListToConvert)
            {
                T_target t = ConvertData(tToConvert, convertMap);
                if (t != null)
                {
                    tList.Add(t);
                }
            }

            return tList;
        }

        public static List<T_target> ConvertData(IEnumerable<T_useToConvert> dataListToConvert)
        {
            return ConvertData(dataListToConvert, null);
        }

        public static IEnumerable<T_target> ConvertEnumerableData(IEnumerable<T_useToConvert> dataListToConvert, ConvertMap convertMap)
        {
            IList<T_target> tList = new List<T_target>();

            foreach (T_useToConvert tToConvert in dataListToConvert)
            {
                T_target t = ConvertData(tToConvert, convertMap);
                if (t != null)
                {
                    tList.Add(t);
                }
            }

            return tList;
        }

        public static IEnumerable<T_target> ConvertEnumerableData(IEnumerable<T_useToConvert> dataListToConvert)
        {
            return ConvertEnumerableData(dataListToConvert, null);
        }
    }
}