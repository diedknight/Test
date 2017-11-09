using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImportAttrs.Data
{
    public class AttributeTitleInfo
    {
        public int TitleId { get; set; }
        public string Title { get; set; }
        public bool IsNumeric { get; set; } = false;
        public string Unit { get; set; }
        public int AttributeTypeID { get; set; }
        public bool CatelogAttributes { get; set; } = false;
    }
}
