using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GhostInspectorTool.GhostInspector.Test
{
    public class Execute : API
    {
        public string startUrl { get; set; }

        public Execute() : this("") { }

        public Execute(string testId)
        {
            this.method = "GET";
            this.adress += string.Format("tests/{0}/execute/", testId);
        }
    }
}
