using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Data;

namespace Common
{
    public class LogEventArgs : EventArgs
    {
        LogInfo logInfo;

        public LogInfo LogInfo
        {
            get { return logInfo; }
        }

        public LogEventArgs(LogType logType, string logMsg, string otherInfo)
        {
            logInfo = new LogInfo(logType, logMsg, otherInfo);
        }
    }
}
