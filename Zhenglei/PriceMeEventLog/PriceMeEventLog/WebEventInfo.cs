using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceMeEventLog
{
    public class WebEventInfo
    {
        public string Ip
        {
            get;set;
        }

        public string ApplicationVirtualPath
        {
            get; set;
        }

        public List<DateTime> EventDateTimes
        {
            get; set;
        } = new List<DateTime>();

        public string RequestURL
        {
            get; set;
        }

        public string ExceptionMessage
        {
            get; set;
        }

        public string ExceptionType
        {
            get; set;
        }

        public string StackTrace
        {
            get; set;
        }

        public int EventCount
        {
            get; set;
        } = 0;
    }
}
