using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceMe.RichAttributeDisplayTool.Process
{
    class LogTxt
    {


        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="accList"></param>
        /// <param name="updateRecord"></param>
        /// <param name="logType"></param>
        public static void LogWrite(Object sender,LogEventArgs e)
        {
            var process = (ProcessData)sender;

            var info = new StringBuilder();

            info.AppendLine("Add Succ " + e.accList.Count + " record.");

            e.accList.ForEach(f =>
            {
                info.AppendLine("     Aid:" + f.Aid);
            });

            info.AppendLine("Update Succ " + e.updateSucc.Count + " record.");

            e.updateSucc.ToList().ForEach(f =>
            {
                info.AppendLine("     Aid:" + f.Value);
            });

            info.AppendLine("Total Succ " + (e.accList.Count + e.updateSucc.Count) + " record.");
            info.AppendLine("Exe Datetime " + DateTime.Now);

            Utility.writeSuccLog(info.ToString());

            if (Task.isSendEmal)
                Utility.sendEmail("PriceMe.RichAttributeDisplayTool", info.ToString());

        }



    }
}
