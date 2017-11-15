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
        public static List<SourcsData> listProduct;
        public static List<SourcsData> listImage;
        public static List<SourcsData> listRetailerproduct;
        public static string key = System.Configuration.ConfigurationManager.AppSettings["DataSourcs"].ToString().ToLower();

        static void Main(string[] args)
        {
            BindData();
            XmlConfig config = new XmlConfig();
            config.ForEach(db => {
                db.ForEach(data => {
                    string filePath = string.Empty;
                    try
                    {
                        var localImage = new LocalImage(data);
                        filePath = localImage.FilePath;

                        string url = localImage.Upload();

                        DataSourcsController.UpdataSourcsData(data.Image.ToLower(), url);

                        db.Update(data.Id, url);

                        new SuccessLog().WriteLine(db.ConfigInfo.Name, data.Id, localImage.FilePath, url, localImage.NoImageOnDrive);

                        localImage.CopyLocalImage();
                        localImage.DeleteLocalImage();                        
                    }
                    catch (Exception ex)
                    {
                        new ErrorLog().WriteLine(db.ConfigInfo.Name, data.Id, filePath);

                        XbaiLog.WriteLog(ex.Message);
                        XbaiLog.WriteLog(ex.StackTrace);
                    }
                });
            });

        }

        private static void BindData()
        {
            string psql = "select ProductId as Id, DefaultImage from CSK_Store_Product where LEN(DefaultImage)>0 and DefaultImage not like '%https://s3.pricemestatic.com/%'";
            string rpsql = "select RetailerProductId as Id, DefaultImage from CSK_Store_retailerProduct where LEN(DefaultImage)>0 and DefaultImage not like '%https://s3.pricemestatic.com/%'";
            string isql = "select ImageId as Id, ImageFile as DefaultImage from CSK_Store_Image where LEN(ImageFile)>0 and ImageFile not like '%https://s3.pricemestatic.com/%'";

            if (key == "product")
            {
                listImage = DataSourcsController.GetSourcsData(isql);
                listRetailerproduct = DataSourcsController.GetSourcsData(rpsql);
            }
            else if (key == "retailerproduct")
            {
                listProduct = DataSourcsController.GetSourcsData(psql);
                listImage = DataSourcsController.GetSourcsData(isql);
            }
            else if (key == "image")
            {
                listProduct = DataSourcsController.GetSourcsData(psql);
                listRetailerproduct = DataSourcsController.GetSourcsData(rpsql);
            }
        }
    }
}
