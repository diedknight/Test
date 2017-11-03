using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Service.Infrastructure
{
    public class Utils
    {
        public static string GetTimeStamp()
        {
            return Convert.ToInt64((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalMilliseconds).ToString();
        }
        public static string MD5(string Input)
        {
            return Utils.GetMD5(Input).ToLower();
        }
        public static string GetMD5(string source)
        {
            string result;
            try
            {
                MD5 mD = new MD5CryptoServiceProvider();
                byte[] value = mD.ComputeHash(Encoding.UTF8.GetBytes(source));
                string text = BitConverter.ToString(value).Replace("-", "");
                result = text;
            }
            catch (Exception)
            {
                result = "";
            }
            return result;
        }
        public static double GetTenByNumber(int number, string s)
        {
            string[] array = new string[]
			{
				"0",
				"1",
				"2",
				"3",
				"4",
				"5",
				"6",
				"7",
				"8",
				"9",
				"A",
				"B",
				"C",
				"D",
				"E",
				"F",
				"G",
				"H",
				"I",
				"J",
				"K",
				"L",
				"M",
				"N",
				"O",
				"P",
				"Q",
				"R",
				"S",
				"T",
				"U",
				"V",
				"W",
				"X",
				"Y",
				"Z"
			};
            int num = 0;
            double num2 = 0.0;
            for (int i = s.Length - 1; i >= 0; i--)
            {
                string b = s.Substring(i, 1);
                for (int j = 0; j < array.Length; j++)
                {
                    if (array[j] == b)
                    {
                        num = j;
                        break;
                    }
                }
                num2 += (double)num * Math.Pow((double)number, (double)(s.Length - 1 - i));
            }
            return num2;
        }
        public static string GetNumberByTen(int number, double value)
        {
            string text = "";
            string result;
            if (number >= 36)
            {
                result = "";
            }
            else
            {
                string[] array = new string[]
				{
					"0",
					"1",
					"2",
					"3",
					"4",
					"5",
					"6",
					"7",
					"8",
					"9",
					"A",
					"B",
					"C",
					"D",
					"E",
					"F",
					"G",
					"H",
					"I",
					"J",
					"K",
					"L",
					"M",
					"N",
					"O",
					"P",
					"Q",
					"R",
					"S",
					"T",
					"U",
					"V",
					"W",
					"X",
					"Y",
					"Z"
				};
                while (Math.Floor(value) > 0.0)
                {
                    int num = Convert.ToInt32(value % (double)number);
                    text = array[num] + text;
                    value = Math.Floor(value / (double)number);
                }
                result = text;
            }
            return result;
        }
        public T Changedynamic<T>(IDictionary<string, object> dic) where T : class, new()
        {
            T info = Activator.CreateInstance<T>();
            info.GetType().GetProperties().ToList<PropertyInfo>().ForEach(delegate(PropertyInfo infoPro)
            {
                infoPro.SetValue(info, dic[infoPro.Name], null);
            });
            return info;
        }
    }
}
