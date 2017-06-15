using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RelatedProductsTool
{
    public static class Utility
    {
        public static List<int> GetIntList(string value, string v)
        {
            List<int> list = new List<int>();
            if (string.IsNullOrEmpty(value))
            {
                return list;
            }

            string[] strs = value.Split(new string[] { v }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string str in strs)
            {
                int id;
                if (int.TryParse(str, out id))
                {
                    list.Add(id);
                }
            }
            return list;
        }
    }
}