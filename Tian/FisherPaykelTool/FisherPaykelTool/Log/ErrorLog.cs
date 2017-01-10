using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FisherPaykelTool.Log
{
    public class ErrorLog : XbaiLog
    {
        private string _logPath = System.Configuration.ConfigurationManager.AppSettings["LogPath"];

        public ErrorLog(int countryId)
        {
            string fileName = Path.Combine(this._logPath, "error_log", DateTime.Now.ToString("yyyy MM dd"), "error_" + countryId + ".txt");

            this.Init(fileName);
        }        

    }
}
