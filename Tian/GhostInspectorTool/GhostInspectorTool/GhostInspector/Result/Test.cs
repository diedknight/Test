using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GhostInspectorTool.GhostInspector.Result
{
    public class Test : IEntity
    {
        public string _id { get; set; }
        public string name { get; set; }
        public string suite { get; set; }
        public string organization { get; set; }
    }
}
