﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonitoringManufacturerProductLinks
{
    public class Keywords
    {
        private static List<string> _words = new List<string>();

        static Keywords()
        {
            _words = System.Configuration.ConfigurationManager.AppSettings["Keywords"].ToString().Split(';').ToList();
        }

        public static string Find(string str)
        {
            string result = "";
            str = str.ToLower().Trim();

            _words.ForEach(item =>
            {
                if (string.IsNullOrEmpty(item)) return;
                if (!string.IsNullOrEmpty(result)) return;

                if (str.Contains(item.ToLower().Trim())) result = item;
            });

            return result;
        }

        public static bool Exist(string str)
        {
            bool result = false;
            str = str.ToLower().Trim();
            
            _words.ForEach(item =>
            {
                if (string.IsNullOrEmpty(item)) return;
                if (result) return;

                result = str.Contains(item.ToLower().Trim());
            });

            return result;
        }

    }
}
