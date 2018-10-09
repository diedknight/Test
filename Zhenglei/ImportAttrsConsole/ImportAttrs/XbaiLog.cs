using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ImportAttrs
{
    public class XbaiLog
    {
        private static readonly object obj = new object();

        private List<string> _headStrList = null;
        private string _fullFilePath = null;

        protected void Init(string fullFilePath, params string[] headStr)
        {
            this._fullFilePath = fullFilePath;
            this._headStrList = headStr.ToList();

            InitHeader();
        }

        public virtual void WriteLine(params string[] str)
        {
            string temp = "";

            str.ToList().ForEach(item => temp += (item + "\t"));

            if (temp.Length != 0) temp = temp.Substring(0, temp.Length - 1);

            XbaiLog.WriteLog(this._fullFilePath, temp);
        }

        public virtual void OverWrite(params string[] str)
        {
            string temp = "";

            str.ToList().ForEach(item => temp += (item + "\t"));

            if (temp.Length != 0) temp = temp.Substring(0, temp.Length - 1);

            XbaiLog.OverWrite(this._fullFilePath, temp);
        }

        private void InitHeader()
        {
            if (this._headStrList.Count == 0) return;

            lock (obj)
            {                
                var dir = Path.GetDirectoryName(_fullFilePath);
                if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

                if (File.Exists(_fullFilePath)) return;
            }

            string str = "";
            this._headStrList.ForEach(item => str += (item + "\t"));

            if (str.Length != 0) str = str.Substring(0, str.Length - 1);

            XbaiLog.WriteLog(_fullFilePath, str);
        }

        public static void WriteLog(string content)
        {

            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log", DateTime.Now.ToString("yyyy-MM-dd") + ".txt");

            WriteLog(path, DateTime.Now + "： " + content);
        }

        public static void WriteLog(string fullFilePath, string content)
        {
            lock (obj)
            {
                var dir = Path.GetDirectoryName(fullFilePath);
                if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

                FileStream fileStream = null;

                if (!File.Exists(fullFilePath))
                    fileStream = File.Create(fullFilePath);
                else
                    fileStream = File.Open(fullFilePath, FileMode.Append);

                using (var streamWriter = new StreamWriter(fileStream))
                {
                    streamWriter.WriteLine(content);
                }
            }
        }

        public static void OverWrite(string fullFilePath, string content)
        {
            lock (obj)
            {
                var dir = Path.GetDirectoryName(fullFilePath);
                if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

                FileStream fileStream = null;

                if (!File.Exists(fullFilePath))
                    fileStream = File.Create(fullFilePath);
                else
                    fileStream = File.Open(fullFilePath, FileMode.Truncate);

                using (var streamWriter = new StreamWriter(fileStream))
                {
                    streamWriter.WriteLine(content);
                }
            }
        }

    }
}
