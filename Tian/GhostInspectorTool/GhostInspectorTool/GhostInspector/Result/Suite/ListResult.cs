using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GhostInspectorTool.GhostInspector.Result.Suite
{
    public class ListResult : IEntity
    {
        public string _id { get; set; }
        public Organization organization { get; set; }
        public DateTime dateCreated { get; set; }
        public int testCount { get; set; }
        public string name { get; set; }
    }
}
