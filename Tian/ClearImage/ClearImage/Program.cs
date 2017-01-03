using ClearImage.Config;
using ClearImage.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClearImage
{
    class Program
    {
        static void Main(string[] args)
        {
            XmlConfig config = new XmlConfig();
            config.ForEach(db => {
                db.ForEach(data => {
                    var localImage = new LocalImage(data);

                    try
                    {
                        string url = localImage.Upload();
                        db.Update(data.Id, url);

                        new SuccessLog().WriteLine(db.ConfigInfo.Name, data.Id, localImage.FilePath, url, localImage.NoImageOnDrive);

                        localImage.CopyLocalImage();
                        localImage.DeleteLocalImage();                        
                    }
                    catch (Exception ex)
                    {
                        new ErrorLog().WriteLine(db.ConfigInfo.Name, data.Id, localImage.FilePath);

                        XbaiLog.WriteLog(ex.Message);
                        XbaiLog.WriteLog(ex.StackTrace);
                    }
                });
            });

        }
    }
}
