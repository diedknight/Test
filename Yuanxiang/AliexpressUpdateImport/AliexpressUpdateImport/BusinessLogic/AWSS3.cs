using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace AliexpressImport.BusinessLogic
{
    public class AWSS3
    {
        private static string WSAccessKey = ConfigurationManager.AppSettings["WSAccessKey"];
        private static string WSSecretKey = ConfigurationManager.AppSettings["WSSecretKey"];

        public static void UploadImageExecute(string filePath)
        {
            string fileName = Path.GetFileName(filePath);

            string key = "images/thumbs/" + fileName;

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
                    BucketName = "cdn.shop.co.nz",
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
