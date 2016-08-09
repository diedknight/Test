using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonitoringManufacturerProductLinks
{
    public class TableInfo
    {
        public string TableName { get; set; }
        public string UrlColumn { get; set; }
        public string StatusColumn { get; set; }

        public string IdColumn { get; set; }

        public TableInfo()
        {
            this.TableName = "";
            this.UrlColumn = "";
            this.StatusColumn = "";
            this.IdColumn = "";
        }

        public static List<TableInfo> GetInfos()
        {
            List<TableInfo> list = new List<TableInfo>();

            string str = System.Configuration.ConfigurationManager.AppSettings["TargetTableAndColumn"];
            if (string.IsNullOrEmpty(str)) return list;

            str.Split(';').ToList().ForEach(item => {
                var arr = item.Split(',');

                TableInfo info = new TableInfo();
                info.TableName = arr.First().Trim();
                info.StatusColumn = arr.Last().Trim();

                if (arr.Length > 1) info.UrlColumn = arr[1].Trim();

                list.Add(info);
            });

            return list;
        }
    }
}
