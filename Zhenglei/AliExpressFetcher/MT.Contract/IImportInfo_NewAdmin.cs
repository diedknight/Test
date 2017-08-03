using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MT.Contract
{
    public interface IImportInfo_NewAdmin : IContract
    {
        string Label { get; set; }
        string Body { get; set; }
        bool Recoverable { get; set; }
    }

}
