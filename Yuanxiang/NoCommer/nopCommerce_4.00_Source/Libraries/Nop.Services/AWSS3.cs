using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;

namespace Nop.Services
{
    public class AWSS3
    {
        private static string WSAccessKey = "0ZY4PSJK9RC3FMPQVB02";
        private static string WSSecretKey = "rRK5GHwelS25Yk5XOLDYLuN8W9I0czKys5n+Tu1D";

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
