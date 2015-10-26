using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Data
{
    public class LogInfo
    {
        LogType logType;
        public LogType LogType
        {
            get { return logType; }
        }

        string logMsg;
        public string LogMsg
        {
            get { return logMsg; }
        }

        string otherInfo;

        public string OtherInfo
        {
            get { return otherInfo; }
        }

        public LogInfo(LogType logType, string logMsg, string otherInfo)
        {
            this.logType = logType;
            this.logMsg = logMsg;
            this.otherInfo = otherInfo;
        }

        public override string ToString()
        {
            return logType.ToString() + " --- \nlogMsg : " + logMsg + "\notherInfo : " + otherInfo;
        }
    }
}