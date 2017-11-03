using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Infrastructure.Request
{
    internal class PartManage
    {
        private List<Part> _partList = new List<Part>();
        public List<Part> PartList
        {
            get
            {
                return this._partList;
            }
            set
            {
                this._partList = value;
            }
        }
        public long GetByteLength(string boundary)
        {
            long num = 0L;
            foreach (Part current in this._partList)
            {
                num += (long)Encoding.UTF8.GetByteCount("--" + boundary + "\r\n");
                num += current.GetByteLength();
            }
            num += (long)Encoding.UTF8.GetByteCount("--" + boundary + "--");
            return num;
        }
        public long GetByteLength()
        {
            string text = "";
            foreach (Part current in this._partList)
            {
                text += ((current.Read() == "") ? "" : (current.Read() + "&"));
            }
            long result;
            if (text != "")
            {
                text = text.Substring(0, text.Length - 1);
                result = (long)Encoding.UTF8.GetByteCount(text);
            }
            else
            {
                result = 0L;
            }
            return result;
        }
        public void Write(StreamWriter writer, string boundary)
        {
            foreach (Part current in this._partList)
            {
                writer.WriteLine("--" + boundary);
                current.Write(writer);
            }
            writer.Write("--" + boundary + "--");
            writer.Flush();
        }
        public string Read(bool isPost)
        {
            string text = "";
            foreach (Part current in this._partList)
            {
                text += ((current.Read() == "") ? "" : (current.Read() + "&"));
            }
            string result;
            if (text != "")
            {
                text = text.Substring(0, text.Length - 1);
                if (!isPost)
                {
                    text = "?" + text;
                }
                result = text;
            }
            else
            {
                result = "";
            }
            return result;
        }
    }
}
