using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace ImportAttrs
{
    public class CmdQueue
    {
        private static readonly object _obj = new object();
        private static CmdQueue _curObj = null;

        private Queue<SqlCommand> _queue = null;
        private CmdNoneQueryWorker _worker = null;

        public static CmdQueue Instance
        {
            get
            {
                if (_curObj == null)
                {
                    lock (_obj)
                    {
                        if (_curObj == null)
                        {
                            _curObj = new CmdQueue();
                        }
                    }
                }

                return _curObj;
            }
        }

        private CmdQueue()
        {
            this._queue = new Queue<SqlCommand>();
            this._worker = CmdNoneQueryWorker.Instance;
        }

        public int Count
        {
            get
            {
                lock (_obj)
                {
                    return this._queue.Count;
                }
            }
        }

        public void Enqueue(SqlCommand cmd)
        {
            lock (_obj)
            {
                this._queue.Enqueue(cmd);
            }
        }

        public SqlCommand Dequeue()
        {
            lock (_obj)
            {
                if (this._queue.Count != 0)
                {
                    return this._queue.Dequeue();

                }
                else
                {
                    return null;
                }
            }
        }

        public void Loop(SqlConnection con)
        {
            var cmd = this.Dequeue();

            while (cmd != null)
            {
                try
                {
                    using (cmd)
                    {
                        cmd.Connection = con;
                        cmd.ExecuteNonQuery();
                    }                    
                }
                catch (Exception ex)
                {
                    DateTime nowTime = DateTime.Now;
                    string importAttrLogDir = System.Configuration.ConfigurationManager.AppSettings["ImportAttrLogDir"];
                    string unMatchLogPath = System.IO.Path.Combine(importAttrLogDir, "UnMatch_" + nowTime.ToString("yyyy-MM-dd_HH_mm") + ".txt");
                    XbaiLog.WriteLog(unMatchLogPath, "Exception... CmdQueue : ");
                    XbaiLog.WriteLog(unMatchLogPath, "Exception... CmdQueue : " + cmd.CommandText);
                    XbaiLog.WriteLog(unMatchLogPath, ex.Message + " \t " + ex.StackTrace);
                }

                cmd = this.Dequeue(); //next
            }
        }


    }
}
