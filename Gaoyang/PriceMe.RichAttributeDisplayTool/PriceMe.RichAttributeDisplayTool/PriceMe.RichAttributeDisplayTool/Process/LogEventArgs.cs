using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceMe.RichAttributeDisplayTool.Process
{
    class LogEventArgs
    {
        public List<RichClass.AttributeCategoryComparisons> accList;
        public Dictionary<int, int> updateSucc;

        public LogEventArgs(List<RichClass.AttributeCategoryComparisons> acc, Dictionary<int, int> update) {
            this.accList = acc;
            this.updateSucc = update;
        }
    }
}
