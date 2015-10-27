using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GhostInspectorTool.GhostInspector.Test
{
    public class List : API
    {
        public List()
        {
            this.method = "GET";
            this.adress += "tests/";
        }
    }
}
