using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace AliexpressImport.BusinessLogic
{
    public static class ImageOperator
    {
        public static string DownImage(string srcURL, string dir, string fileName, out string ext)
        {
            HttpWebRequest httpWebRequest = GetHttpWebRequest(srcURL);
            HttpWebResponse webResponse = httpWebRequest.GetResponse() as HttpWebResponse;

            CreateDir(dir);

            ext = string.Empty;
            if (webResponse.ContentType.Equals("image/jpeg"))
            {
                ext = ".jpg";
            }
            else if (webResponse.ContentType.Equals("image/gif"))
            {
                ext = ".gif";
            }
            else if (webResponse.ContentType.Equals("image/bmp"))
            {
                ext = ".bmp";
            }
            else
            {
                ext = ".png";
            }

            string filePath = dir + "\\" + fileName + ext;
            Stream responseStream = webResponse.GetResponseStream();
            using (FileStream fs = File.Open(filePath, FileMode.Create))
            {

                byte[] downloadBytes = new byte[2048];
                int downloadBytesCount = 0;
                while ((downloadBytesCount = responseStream.Read(downloadBytes, 0, downloadBytes.Length)) > 0)
                {
                    fs.Write(downloadBytes, 0, downloadBytesCount);
                }
                fs.Flush();
            }
            return fileName + ext;
        }

        private static HttpWebRequest GetHttpWebRequest(string srcURL)
        {
            HttpWebRequest httpWebRequest = HttpWebRequest.Create(srcURL) as HttpWebRequest;

            httpWebRequest.Method = "GET";
            httpWebRequest.UserAgent = "Mozilla/5.0 (Windows NT 6.2; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/27.0.1453.94 Safari/537.36";

            return httpWebRequest;
        }

        public static void CreateDir(string path)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }

        public static void CopyImageFTP(string imagePath, string imageName)
        {
            CopyFile.NetWorkCopy.CopyFile(ConfigAppString.TargetIP, ConfigAppString.TargetPath, ConfigAppString.UserID, ConfigAppString.Password, imagePath, imageName);
        }

        public static string GetRandomString(int length)
        {
            string rs = "00";

            Random random = new Random();
            int r = 0;
            for (int i = 0; i < length; i++)
            {
                if (i == 0)
                {
                    r = random.Next(1, 10);
                }
                else
                {
                    r = random.Next(10);
                }
                rs += r;
            }

            return rs;
        }

        public static System.Drawing.Image ResizeImage(System.Drawing.Image image, Size outputSize, bool scale)
        {
            if (scale)
            {
                if (image.Width < outputSize.Width && image.Height < outputSize.Height) return image;

                float inHW = (float)image.Height / (float)image.Width;
                float outHW = (float)outputSize.Height / (float)outputSize.Width;
                if (inHW > outHW)
                {
                    float a = outputSize.Height / inHW;
                    outputSize.Width = (int)a;
                }
                else
                {
                    float a = outputSize.Width * inHW;
                    outputSize.Height = (int)a;
                }
            }
            return new Bitmap(image, outputSize);
        }

        public static System.Drawing.Image ResizeImage(string imagePath, Size outputSize, bool scale)
        {
            System.Drawing.Image image = System.Drawing.Image.FromFile(imagePath);
            System.Drawing.Image _image = ResizeImage(image, outputSize, scale);
            image.Dispose();
            return _image;
        }

        public static System.Drawing.Image AddBackground(System.Drawing.Image image, Size size)
        {
            Graphics graphic;
            Bitmap newBmp = new Bitmap(size.Width, size.Height);
            graphic = System.Drawing.Graphics.FromImage(newBmp);
            graphic.FillRectangle(Brushes.White, new Rectangle(0, 0, size.Width, size.Height));
            Point p = new Point();
            p.X = (size.Width - image.Width) / 2;
            p.Y = (size.Height - image.Height) / 2;
            graphic.DrawImage(image, p);
            image = newBmp;

            graphic.Dispose();

            return image;
        }

        public static System.Drawing.Image AddBackground(string imagePath, Size size)
        {
            System.Drawing.Image image = System.Drawing.Image.FromFile(imagePath);
            System.Drawing.Image _image = AddBackground(image, size);
            image.Dispose();
            return _image;
        }

        public static System.Drawing.Image GetPircemeImage(System.Drawing.Image image, Size outputSize)
        {
            System.Drawing.Image _image = ResizeImage(image, outputSize, true);
            _image = AddBackground(_image, outputSize);
            return _image;
        }

        public static System.Drawing.Image GetPircemeImage(string imagePath, Size outputSize)
        {
            System.Drawing.Image image = System.Drawing.Image.FromFile(imagePath);
            System.Drawing.Image _image = GetPircemeImage(image, outputSize);
            image.Dispose();
            return _image;
        }

        public static void ResizeImageToM(string imagePath)
        {
            if (!imagePath.Contains("."))
            {
                Console.WriteLine("this image path is invaild: " + imagePath);
                return;
            }
            System.Drawing.Image img = GetPircemeImage(imagePath, new Size(550, 550));
            string mPath = imagePath.Insert(imagePath.LastIndexOf("."), "_550");
            img.Save(mPath, ImageFormat.Jpeg);
        }

        public static void ResizeImageToMS(string imagePath)
        {
            if (!imagePath.Contains("."))
            {
                Console.WriteLine("this image path is invaild: " + imagePath);
                return;
            }
            System.Drawing.Image img = GetPircemeImage(imagePath, new Size(415, 415));
            string mPath = imagePath.Insert(imagePath.LastIndexOf("."), "_415");
            img.Save(mPath, ImageFormat.Jpeg);
        }

        public static void ResizeImageToS(string imagePath)
        {
            if (!imagePath.Contains("."))
            {
                Console.WriteLine("this image path is invaild: " + imagePath);
                return;
            }
            System.Drawing.Image img = GetPircemeImage(imagePath, new Size(100, 100));
            string mPath = imagePath.Insert(imagePath.LastIndexOf("."), "_100");
            img.Save(mPath, ImageFormat.Jpeg);
        }

        public static void ResizeImageToSS(string imagePath)
        {
            if (!imagePath.Contains("."))
            {
                Console.WriteLine("this image path is invaild: " + imagePath);
                return;
            }
            System.Drawing.Image img = GetPircemeImage(imagePath, new Size(64, 64));
            string mPath = imagePath.Insert(imagePath.LastIndexOf("."), "_64");
            img.Save(mPath, ImageFormat.Jpeg);
        }

        public static void ResizeImageToMSS(string imagePath)
        {
            if (!imagePath.Contains("."))
            {
                Console.WriteLine("this image path is invaild: " + imagePath);
                return;
            }
            System.Drawing.Image img = GetPircemeImage(imagePath, new Size(75, 75));
            string mPath = imagePath.Insert(imagePath.LastIndexOf("."), "_75");
            img.Save(mPath, ImageFormat.Jpeg);
        }
    }
}
