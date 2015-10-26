using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Data;

namespace Fetcher
{
    public interface IFetcher
    {
        List<MobilePlanInfo> GetMobilePlanInfoList();

        string ProviderName { get; }
        int ProviderID { get; }
        int Priority { get; }
    }
}
