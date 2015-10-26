using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GhostInspectorTool.GhostInspector.Suite
{
    public class List : API
    {
        public List()
        {
            this.method = "GET";
            this.adress += "suites/";
        }
    }
}
