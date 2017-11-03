using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Infrastructure.MQ
{
    public interface IQueue
    {
        void Send<T>(params T[] obj);
        T Receive<T>();
    }
}
