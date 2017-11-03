using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClearImage.Log
{
    public class SuccessLog : XbaiLog
    {
        public SuccessLog()
        {
            string dir = System.Configuration.ConfigurationManager.AppSettings["LogPath"];
            if (string.IsNullOrEmpty(dir)) dir = AppDomain.CurrentDomain.BaseDirectory;

            this.Init(Path.Combine(dir, "Success", DateTime.Now.ToString("yyyy-MM-dd") + ".txt"), "table name", "id", "origin path", "new path", "NoImageonDrive");
        }

        public void WriteLine(string tableName, int Id, string originPath, string newPath, bool NoImageonDrive)
        {
            base.WriteLine(tableName, Id.ToString(), originPath, newPath, NoImageonDrive.ToString());
        }

    }
}
