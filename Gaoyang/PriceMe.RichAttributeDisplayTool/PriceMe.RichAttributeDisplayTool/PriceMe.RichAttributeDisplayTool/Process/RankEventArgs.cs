using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceMe.RichAttributeDisplayTool.Process
{
    class RankEventArgs:EventArgs
    {
        public List<double> perList;

        public RankEventArgs(List<double> val)
        {
            this.perList = val;
        }
    }
}
