using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using ClearImage.Config;
using ClearImage.Log;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SS_Image
{
    class Program
    {
        static void Main(string[] args)
        {

            XmlConfig config = new XmlConfig();
            config.ForEach(db =>
            {
                db.ForEach((data, tableName) =>
                {
                    try
                    {
                        //data.Image = "https://s3.pricemestatic.com/Images/RetailerProductImages/StRetailer1292/0092243958_ms.jpg";
                        Uri uri = new Uri(data.Image);
                        string key = uri.AbsolutePath;
                        key = RemovePostfix(key);
                        key = key.Replace(".", "_ss.");
                        if (key.IndexOf("/") == 0) key = key.Substring(1);

                        string fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ImageChache", "temp.jpg");
                        string dir = Path.GetDirectoryName(fileName);

                        if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

                        using (WebClient wc = new WebClient())
                        {
                            wc.DownloadFile(data.Image, fileName);
                            ImageOper.Resize_ss(fileName);
                            ImageOper.Resize_l(fileName);

                            fileName = AddPostfix(fileName, "_ss");

                            Upload(fileName, key);

                            //Large image
                            key = uri.AbsolutePath;
                            key = RemovePostfix(key);
                            key = key.Replace(".", "_l.");
                            if (key.IndexOf("/") == 0) key = key.Substring(1);
                            key = "Large/" + key;

                            fileName = RemovePostfix(fileName);
                            fileName = AddPostfix(fileName, "_l");
                            Upload(fileName, key);

                            db.Update(data.Id, uri.ToString());
                        }
                    }
                    catch (Exception ex)
                    {
                        new ErrorLog().WriteLine(db.ConfigInfo.Name, data.Id, data.Image);

                        XbaiLog.WriteLog(ex.Message);
                        XbaiLog.WriteLog(ex.StackTrace);
                    }
                });
            });

        }

        private static string RemovePostfix(string filePath)
        {
            filePath = filePath.Replace("_m.", ".");
            filePath = filePath.Replace("_ms.", ".");
            filePath = filePath.Replace("_s.", ".");
            filePath = filePath.Replace("_l.", ".");
            filePath = filePath.Replace("_ss.", ".");

            return filePath;
        }

        private static string AddPostfix(string filePath, string postfix)
        {
            string dir = Path.GetDirectoryName(filePath);
            string name = Path.GetFileNameWithoutExtension(filePath) + postfix;
            string extension = Path.GetExtension(filePath);

            return Path.Combine(dir, name + extension);
        }

        private static void Upload(string filePath, string key)
        {

            if (string.IsNullOrEmpty(filePath)) return;
            if (!File.Exists(filePath)) return;

            string WSAccessKey = System.Configuration.ConfigurationManager.AppSettings["WSAccessKey"];
            string WSSecretKey = System.Configuration.ConfigurationManager.AppSettings["WSSecretKey"];

            string contenttype = "";

            switch (Path.GetExtension(filePath).ToLower())
            {
                case ".png": contenttype = "image/png"; break;
                case ".jpeg": contenttype = "image/jpeg"; break;
                case ".gif": contenttype = "image/gif"; break;
                case ".bmp": contenttype = "image/bmp"; break;
                case ".tiff": contenttype = "image/tiff"; break;
                default: contenttype = "image/png"; break;
            }

            AmazonS3Config S3Config = new AmazonS3Config()
            {
                ServiceURL = "s3-ap-southeast-1.amazonaws.com",
                CommunicationProtocol = Amazon.S3.Model.Protocol.HTTPS,
            };

            using (var client = AWSClientFactory.CreateAmazonS3Client(WSAccessKey, WSSecretKey, S3Config))
            {
                PutObjectRequest putRequest1 = new PutObjectRequest
                {
                    BucketName = "s3.pricemestatic.com",
                    Key = key,
                    ContentType = contenttype,
                    FilePath = filePath,
                    CannedACL = S3CannedACL.PublicRead
                };

                using (PutObjectResponse response1 = client.PutObject(putRequest1))
                { }
            }
        }

    }
}
