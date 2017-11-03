using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ClearImage.Config
{
    public class XmlConfig
    {
        private List<DBInfo> _list = new List<DBInfo>();

        public XmlConfig()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Config", "config.xml"));

            var root = doc.SelectSingleNode("xbai");

            var nodes = root.SelectNodes("table");
            foreach (XmlNode node in nodes)
            {
                ConfigInfo info = new ConfigInfo();
                info.IdCol = node.SelectSingleNode("id_col").InnerText;
                info.ImageCol = node.SelectSingleNode("image_col").InnerText;
                info.Name = node.SelectSingleNode("name").InnerText;
                info.Where = node.SelectSingleNode("where").InnerText;

                var topNode = node.SelectSingleNode("top");
                if (topNode != null && topNode.InnerText.Trim() != "0")
                {
                    info.Top = Convert.ToInt32(topNode.InnerText.Trim());
                }

                this._list.Add(new DBInfo(info));
            }
        }

        public void ForEach(Action<DBInfo> action)
        {
            this._list.ForEach(item => { action(item); });
        }

    }

}
