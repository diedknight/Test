using PriceMeCrawlerTask.Common.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImportAttrs
{
    public class Log: XbaiLog
    {       
        public Log(string fullPath)
        {            
            Init(fullPath);
        }
    }
}
