using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Service.Infrastructure.Request
{
    internal class TextPart : Part
    {
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
        public TextPart()
        {
        }
        public TextPart(string name, string value)
        {
            this.Name = name;
            this.Value = value;
        }
        protected override long WriteHeader(StreamWriter writer)
        {
            return 0L;
        }
        protected override long WriteBody(StreamWriter writer)
        {
            return 0L;
        }
        public override long GetByteLength()
        {
            long result;
            if (string.IsNullOrEmpty(this.Name))
            {
                result = 0L;
            }
            else
            {
                result = (long)Encoding.UTF8.GetByteCount(this.Name.Trim() + "=" + HttpUtility.UrlEncode(this.Value.Trim()));
            }
            return result;
        }
        public override string Read()
        {
            string result;
            if (string.IsNullOrEmpty(this.Name))
            {
                result = "";
            }
            else
            {
                result = string.Format("{0}={1}", this.Name.Trim(), HttpUtility.UrlEncode(this.Value.Trim()));
            }
            return result;
        }
    }
}
