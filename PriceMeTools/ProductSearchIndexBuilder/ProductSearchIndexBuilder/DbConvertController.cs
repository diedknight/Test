using System;
using System.Linq;

namespace ProductSearchIndexBuilder
{
    public static class DbConvertController<T_target> where T_target : class, new()
    {
        public static T_target ReadDataFromDataReader(System.Data.Common.DbDataReader sqlDR)
        {
            var columns = Enumerable.Range(0, sqlDR.FieldCount).Select(sqlDR.GetName).ToList();
            Type type = typeof(T_target);

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
                            string name = targetpropertyInfo.Name;
                            if (columns.Contains(name))
                            {
                                object value = sqlDR[name];
                                if (value != null && !string.IsNullOrEmpty(value.ToString()))
                                {
                                    if (targetpropertyInfo.PropertyType.IsGenericType && targetpropertyInfo.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                                    {
                                        value = Convert.ChangeType(value, Nullable.GetUnderlyingType(targetpropertyInfo.PropertyType));
                                    }
                                    else
                                    {
                                        value = Convert.ChangeType(value, targetpropertyInfo.PropertyType);
                                    }


                                    targetpropertyInfo.SetValue(result, value, null);
                                }
                            }
                        }
                        return result;
                    }
                }
            }

            return null;
        }
    }
}