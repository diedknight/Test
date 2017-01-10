using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FisherPaykelTool.Log
{
    public class EmailLog : XbaiLog
    {
        private string _logPath = System.Configuration.ConfigurationManager.AppSettings["LogPath"];

        public EmailLog(int countryId)
        {
            string fileName = Path.Combine(this._logPath, "email_log", DateTime.Now.ToString("yyyy MM dd"), "log_" + countryId + ".txt");

            this.Init(fileName, "PID", "Email");
        }
    }
}
