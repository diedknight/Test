using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GhostInspectorTool.GhostInspector.Result.Test
{
    public class ListResult : IEntity
    {
        public string _id { get; set; }
        public Organization organization { get; set; }
        public SuiteEntity suite { get; set; }
        public string name { get; set; }
        public string startUrl { get; set; }

    }
}
