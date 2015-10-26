using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GhostInspectorTool.GhostInspector.Result.Suite
{
    public class ExecuteResult : IEntity
    {
        public string _id { get; set; }
        public DateTime dateExecutionFinished { get; set; }
        public DateTime dateExecutionStarted { get; set; }
        public string startUrl { get; set; }
        public string endUrl { get; set; }
        public string test { get; set; }


    }
}
