using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ImportAttrs
{
    public class CmdNoneQueryWorker
    {
        private static readonly object obj = new object();
        private static CmdNoneQueryWorker _instance = null;

        private CmdNoneQueryWorker()
        {
            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    try
                    {
                        Thread.Sleep(60 * 1000);

                        var queue = CmdQueue.Instance;
                        if (queue.Count != 0)
                        {
                            using (var con = ImportController.GetDBConnection())
                            {
                                con.Open();
                                queue.Loop(con);
                            }
                        }
                    }
                    catch(Exception ex)
                    {
                        DateTime nowTime = DateTime.Now;
                        string importAttrLogDir = System.Configuration.ConfigurationManager.AppSettings["ImportAttrLogDir"];
                        string unMatchLogPath = System.IO.Path.Combine(importAttrLogDir, "UnMatch_" + nowTime.ToString("yyyy-MM-dd_HH_mm") + ".txt");

                        XbaiLog.WriteLog(unMatchLogPath, "Exception... CmdNoneQueryWorker : ");
                        XbaiLog.WriteLog(unMatchLogPath, ex.Message + " \t " + ex.StackTrace);
                    }
                }
            });            
        }

        public static CmdNoneQueryWorker Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (obj)
                    {
                        if (_instance == null)
                        {
                            _instance = new CmdNoneQueryWorker();
                        }
                    }
                }

                return _instance;
            }
        }


    }
}
