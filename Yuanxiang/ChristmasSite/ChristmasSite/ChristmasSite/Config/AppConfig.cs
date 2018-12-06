using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChristmasSite.Config
{
    public class AppConfig
    {
        public DbInfo PricemeDBConnection { get; private set; }

        public AppConfig(IConfiguration configuration)
        {
            this.PricemeDBConnection = configuration.GetSection("PricemeDbInfo").Get<DbInfo>();
        }
    }
}
