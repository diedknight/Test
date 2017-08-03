using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MT.Contract
{
    public class ImportInfo_Test : IImportInfo_Test
    {

        public string Label { get; set; }

        public string Body { get; set; }

        public bool Recoverable { get; set; }
    }
}
