using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IndexBuildCommon.Data
{
    public class BulidIndexSpeedInfo
    {
        public DateTime StartReadDBTime;
        public DateTime EndReadDBTime;
        public DateTime StartWriteIndexTime;
        public DateTime EndWriteIndexTime;
    }
}
