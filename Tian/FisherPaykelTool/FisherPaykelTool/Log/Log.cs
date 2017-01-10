using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FisherPaykelTool.Log
{
    public class Log : XbaiLog
    {
        private string _logPath = System.Configuration.ConfigurationManager.AppSettings["logPath"];

        public Log()
        {
            string fileName = Path.Combine(this._logPath, DateTime.Now.ToString("yyyy MM dd"), "log.txt");

            this.Init(fileName);
        }

        public override void WriteLine(params string[] str)
        {
            Console.WriteLine(string.Join(",", str));
            base.WriteLine(str);
        }

    }
}
