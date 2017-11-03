using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Infrastructure.Request
{
    internal class FilePart : Part
    {
        public string FileName = "";
        private string header = "";
        private long bodyLength = 0L;
        public string Name
        {
            get;
            set;
        }
        public byte[] data
        {
            get;
            set;
        }
        public FilePart()
        {
        }
        public FilePart(string name, string fileName, byte[] data)
        {
            this.Name = name;
            this.FileName = fileName;
            this.data = data;
        }
        protected override long WriteHeader(StreamWriter writer)
        {
            this.header = string.Format("Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: application/octet-stream\r\n", this.Name, this.FileName);
            writer.Write(this.header);
            return (long)Encoding.UTF8.GetByteCount(this.header);
        }
        protected override long WriteBody(StreamWriter writer)
        {
            int num = 0;
            writer.WriteLine();
            if (this.data != null)
            {
                writer.Flush();
                writer.BaseStream.Write(this.data, 0, this.data.Length);
                num = this.data.Length;
            }
            writer.WriteLine();
            this.bodyLength = (long)(num + Encoding.UTF8.GetByteCount("\r\n\r\n"));
            return this.bodyLength;
        }
        public override long GetByteLength()
        {
            this.header = string.Format("Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: application/octet-stream\r\n", this.Name, this.FileName);
            long num = (long)((this.data == null) ? 0 : this.data.Length);
            this.bodyLength = num + (long)Encoding.UTF8.GetByteCount("\r\n\r\n");
            return (long)Encoding.UTF8.GetByteCount(this.header) + this.bodyLength;
        }
        public override string Read()
        {
            return "";
        }
    }
}
