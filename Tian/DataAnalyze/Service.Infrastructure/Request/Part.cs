using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Infrastructure.Request
{
    internal abstract class Part
    {
        protected abstract long WriteHeader(StreamWriter writer);
        protected abstract long WriteBody(StreamWriter writer);
        public abstract long GetByteLength();
        public abstract string Read();
        public long Write(StreamWriter writer)
        {
            long num = this.WriteHeader(writer);
            return num + this.WriteBody(writer);
        }
    }
}
