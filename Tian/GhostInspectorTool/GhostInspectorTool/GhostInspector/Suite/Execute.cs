using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GhostInspectorTool.GhostInspector.Suite
{
    public class Execute : API
    {
        public string startUrl { get; set; }

        public Execute() : this("") { }

        public Execute(string suiteId)
        {
            this.method = "GET";
            this.adress += string.Format("suites/{0}/execute/", suiteId);
        }
    }
}
