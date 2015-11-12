using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace PriceMe.RichAttributeDisplayTool
{
    class Task
    {
        public string SendEmail = ConfigurationManager.AppSettings["IsSendEmail"];//是否发邮件
        public static string logPath = ConfigurationManager.AppSettings["RichAttributeLog"]; //日志路径

        public static bool isSendEmal = true;

        public Task()
        {
            if (!string.IsNullOrEmpty(SendEmail))
                isSendEmal = SendEmail == "1" ? true : false;
        }

        /// <summary>
        /// 开始工作
        /// </summary>
        public void start(){
            try
            {
                Process.Work.StartWork();
            }
            catch(Exception ex){
                if (isSendEmal)
                    Utility.sendEmail("PriceMe.RichAttributeDisplayTool", ex.Message);

                Utility.writeFailLog(ex.Message);
            }
        }
    }
}
