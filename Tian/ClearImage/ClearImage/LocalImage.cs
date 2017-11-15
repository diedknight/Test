using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using ClearImage.Config;
using ClearImage.Log;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClearImage
{
    public class LocalImage
    {
        private string _filePath = "";
        private string _filePath_m = "";
        private string _filePath_ms = "";
        private string _filePath_s = "";
        private string _filePath_ss = "";
        private string _filePath_l = "";

        private DataInfo _dataInfo = null;

        public string FilePath { get { return this._filePath; } }
        public bool NoImageOnDrive { get; private set; } = false;

        public LocalImage(DataInfo dataInfo)
        {
            this._dataInfo = dataInfo;

            string tempImgPath = Path.Combine(Path.GetDirectoryName(dataInfo.Image), Path.GetFileName(dataInfo.Image));

            string rootDir = this.FixDir(System.Configuration.ConfigurationManager.AppSettings["CheckImagePathDriver"]);
            string rootDir2 = this.FixDir(System.Configuration.ConfigurationManager.AppSettings["CheckImagePathDriver2"]);
            string rootDir3 = this.FixDir(System.Configuration.ConfigurationManager.AppSettings["LargeImagePatch"]);
            
            string filePath = (rootDir + tempImgPath).Replace(@"\\", @"\");
            string largeFilePath = (rootDir3 + tempImgPath).Replace(@"\\", @"\");

            if (!File.Exists(filePath)) filePath = (rootDir2 + tempImgPath).Replace(@"\\", @"\");
            //if (!File.Exists(filePath)) return;
            if (!File.Exists(filePath)) this.NoImageOnDrive = true;

            if (Program.key.ToLower() == "category")
            {
                filePath = this.RemovePostfix(filePath);
                largeFilePath = this.RemovePostfix(largeFilePath);
            }

            this._filePath = filePath;
            this._filePath_m = this.AddPostfix(filePath, "_m");
            this._filePath_ms = this.AddPostfix(filePath, "_ms");
            this._filePath_s = this.AddPostfix(filePath, "_s");
            this._filePath_ss = this.AddPostfix(filePath, "_ss");
            this._filePath_l = this.AddPostfix(largeFilePath, "_l");

            if (!File.Exists(this._filePath_m))
            {
                ImageOper.Resize_M(this._filePath);
            }
            if (!File.Exists(this._filePath_ms))
            {
                ImageOper.Resize_Ms(this._filePath);
            }
            if (!File.Exists(this._filePath_s))
            {
                ImageOper.Resize_s(this._filePath);
            }
            if (!File.Exists(this._filePath_ss))
            {
                ImageOper.Resize_ss(this._filePath);
            }
            if (!File.Exists(this._filePath_l))
            {
                ImageOper.Resize_l(this._filePath);
                this._filePath_l = this._filePath.Insert(this._filePath.LastIndexOf("."), "_l");
            }
        }

        public string Upload()
        {
            string url = this.Upload(this._filePath);

            this.Upload(this._filePath_m);
            this.Upload(this._filePath_ms);
            this.Upload(this._filePath_s);
            this.Upload(this._filePath_ss);
            this.Upload(this._filePath_l);

            return url;
        }

        private string GetS3Key(string filePath)
        {
            string fileName = Path.GetFileName(filePath);
            string testdir = System.Configuration.ConfigurationManager.AppSettings["S3File"];
            List<string> dir = Path.GetDirectoryName(this._dataInfo.Image).Split('\\').Where(item => item != "").ToList();

            if (filePath == this._filePath_l) dir.Insert(0, "Large");
            if (!string.IsNullOrEmpty(testdir)) dir.Insert(0, testdir);

            string key = string.Join("\\", dir.ToArray());
            key = Path.Combine(key, fileName);
            key = key.Replace(" ", "").Replace("\\", "/");
            key = key.ToLower();
            if (filePath == this._filePath_l)
                key = key.Replace("large/", "Large/");
            if (key.IndexOf("/") == 0) key = key.Substring(1);

            return key;
        }

        private string Upload(string filePath)
        {
            //string key = System.Configuration.ConfigurationManager.AppSettings["S3File"];
            //if (string.IsNullOrEmpty(key)) key = Path.GetDirectoryName(this._dataInfo.Image);

            //string key = key = Path.GetDirectoryName(this._dataInfo.Image);
            //if (key.IndexOf("\\") == 0)
            //    key = System.Configuration.ConfigurationManager.AppSettings["S3File"] + key;
            //else
            //    key = System.Configuration.ConfigurationManager.AppSettings["S3File"] + "\\" + key;

            //key = Path.Combine(key, Path.GetFileName(filePath)).Replace(" ", "").Replace("\\", "/");
            //if (key.IndexOf("/") == 0) key = key.Substring(1);

            string key = this.GetS3Key(filePath);
            string url = "https://s3.pricemestatic.com/" + key;

            if (string.IsNullOrEmpty(filePath)) return url;
            if (!File.Exists(filePath)) return url;

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
            
            return url;
        }

        public void CopyLocalImage()
        {
            this.CopyLocalImage(this._filePath);
            this.CopyLocalImage(this._filePath_m);
            this.CopyLocalImage(this._filePath_ms);
            this.CopyLocalImage(this._filePath_s);
            this.CopyLocalImage(this._filePath_ss);
            this.CopyLocalImage(this._filePath_l);
        }

        private void CopyLocalImage(string filePath)
        {
            if (string.IsNullOrEmpty(filePath)) return;
            if (!File.Exists(filePath)) return;

            string sourcePathRoot = Path.GetPathRoot(filePath);
            //string destPathRoot = FixDir(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ImageCache"));

            string destPathRoot = FixDir(System.Configuration.ConfigurationManager.AppSettings["copyPath"]);
            if(string.IsNullOrEmpty(destPathRoot))
                destPathRoot = FixDir(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ImageCache"));

            string destFilePath = filePath.Replace(sourcePathRoot, destPathRoot);
            string destDir = Path.GetDirectoryName(destFilePath);

            if (!Directory.Exists(destDir)) Directory.CreateDirectory(destDir);

            File.Copy(filePath, destFilePath, true);
        }

        public void DeleteLocalImage()
        {
            this.DeleteLocalImage(this._filePath);
            this.DeleteLocalImage(this._filePath_m);
            this.DeleteLocalImage(this._filePath_ms);
            this.DeleteLocalImage(this._filePath_s);
            this.DeleteLocalImage(this._filePath_ss);
            this.DeleteLocalImage(this._filePath_l);
        }

        private void DeleteLocalImage(string filePath)
        {
            if (string.IsNullOrEmpty(filePath)) return;
            if (!File.Exists(filePath)) return;

            File.Delete(filePath);
        }

        private string FixDir(string dir)
        {
            if (string.IsNullOrEmpty(dir)) return dir;
            if (dir.Substring(dir.Length - 1) == "\\") return dir;

            return dir + "\\";
        }

        private string AddPostfix(string filePath, string postfix)
        {
            string dir = Path.GetDirectoryName(filePath);
            string name = Path.GetFileNameWithoutExtension(filePath) + postfix;
            string extension = Path.GetExtension(filePath);

            return Path.Combine(dir, name + extension);
        }

        private string RemovePostfix(string filePath)
        {
            filePath = filePath.Replace("_m.", ".");
            filePath = filePath.Replace("_ms.", ".");
            filePath = filePath.Replace("_s.", ".");
            filePath = filePath.Replace("_l.", ".");
            filePath = filePath.Replace("_ss.", ".");

            return filePath;
        }

    }
}
