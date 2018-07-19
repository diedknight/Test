using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IndexBuildCommon.Data
{
    class ClickData : IComparable<ClickData>
    {
        public int Id;
        public int Clicks;

        public int CompareTo(ClickData other)
        {
            return other.Clicks.CompareTo(this.Clicks);
        }
    }
}