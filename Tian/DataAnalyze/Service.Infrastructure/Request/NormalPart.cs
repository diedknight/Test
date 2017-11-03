using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Infrastructure.Request
{
    internal class NormalPart : Part
    {
        private string header = "";
        private string body = "";
        public string Name
        {
            get;
            set;
        }
        public string Value
        {
            get;
            set;
        }
        public NormalPart()
        {
        }
        public NormalPart(string name, string value)
        {
            this.Name = name;
            this.Value = value;
        }
        protected override long WriteHeader(StreamWriter writer)
        {
            this.header = string.Format("Content-Disposition: form-data; name=\"{0}\"\r\n", this.Name);
            writer.Write(this.header);
            return (long)Encoding.UTF8.GetByteCount(this.header);
        }
        protected override long WriteBody(StreamWriter writer)
        {
            this.body = string.Format("\r\n{0}\r\n", this.Value);
            writer.Write(this.body);
            return (long)Encoding.UTF8.GetByteCount(this.body);
        }
        public override long GetByteLength()
        {
            this.header = string.Format("Content-Disposition: form-data; name=\"{0}\"\r\n", this.Name);
            this.body = string.Format("\r\n{0}\r\n", this.Value);
            return (long)Encoding.UTF8.GetByteCount(this.header + this.body);
        }
        public override string Read()
        {
            return "";
        }
    }
}
