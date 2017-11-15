using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClearImage.Log
{
    public class ErrorLog:XbaiLog
    {
        public ErrorLog()
        {
            string dir = System.Configuration.ConfigurationManager.AppSettings["LogPath"];
            if (string.IsNullOrEmpty(dir)) dir = AppDomain.CurrentDomain.BaseDirectory;

            this.Init(Path.Combine(dir, "Failure", DateTime.Now.ToString("yyyy-MM-dd") + ".txt"), "table name", "id", "path");
        }

        public void WriteLine(string tableName, int Id, string path)
        {
            base.WriteLine(tableName, Id.ToString(), path);
        }

        public void WriteLine(string info)
        {
            base.WriteLine(info);
        }
    }
}
