using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MT.Contract
{
    public interface IFeedInfo : IContract
    {
        int ProductCount { get; set; }
        int RetailerId { get; set; }
        string FilePath { get; set; }
        string DownLoadMode { get; set; }
    }
}
