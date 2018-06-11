using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace IpAnalytics.Config
{
    public class JobConfig
    {
        private static string _filename = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Config", "JobConfig.xml");
        private static XmlDocument _doc = new XmlDocument();
        private static XmlNode _section = null;


        public static void Load(string sectionName)
        {
            if (!File.Exists(_filename)) return;

            _doc.Load(_filename);

            _section = _doc.SelectSingleNode("config");
            _section = _section.SelectSingleNode(sectionName);
        }

        public static string GetValue(string name)
        {
            foreach (XmlNode node in _section.SelectNodes("key"))
            {
                if (node.Attributes["name"].Value == name)
                {
                    return node.Attributes["value"].Value;
                }
            }

            return "";
        }

        public static void SetValue(string name, string value)
        {
            foreach (XmlNode node in _section.SelectNodes("key"))
            {
                if (node.Attributes["name"].Value == name)
                {
                    node.Attributes["value"].Value = value;
                    _doc.Save(_filename);                    
                }
            }

            return;
        }

    }
}
