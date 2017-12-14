using AliexpressImport.BusinessLogic;
using AliexpressImport.Data;
using MT.Contract;
using MT.Extend;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AliexpressDBA;

namespace AliexpressImport.CustomConsumer
{
    public class Consumer : BaseConsumer<IShopContract>
    {
        protected override void ConsumeHandle(SimpleConsumeContext<IShopContract> context)
        {
            Category category = new Category();
            CrawlerParameter parameter = new CrawlerParameter();
            
            try
            {
                var msg = context.Message;
                string temp = msg.Label;
                string filePath = msg.Label;
                
                parameter.FilePath = filePath;
                
                ImportProduct ip = new ImportProduct();
                ip.ProcessFeed((object)parameter);
            }
            catch (Exception ex)
            {
                OutManagerContronller.WriterInfo(TraceEventType.Error, "consume handle... " + ex.Message + " " + ex.StackTrace);
            }
        }
    }
}
