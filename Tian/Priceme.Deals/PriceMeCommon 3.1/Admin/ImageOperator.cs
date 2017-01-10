using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Net;

namespace PriceMeCommon.Admin
{
    public static class ImageOperator
    {
        public static System.Drawing.Image AddLogo(System.Drawing.Image image, string drawString, Font font, Brush brush, PointF drawPoint)
        {
            Graphics graphic;
            try
            {
                graphic = Graphics.FromImage(image);
            }
            catch
            {
                Bitmap newBmp = new Bitmap(image.Width, image.Height);
                graphic = System.Drawing.Graphics.FromImage(newBmp);
                graphic.DrawImage(image,
                                   new Rectangle(0, 0, newBmp.Width, newBmp.Height),
                                   new Rectangle(0, 0, image.Width, image.Height),
                                   GraphicsUnit.Pixel);
                image = newBmp;

            }
            graphic.DrawString(drawString, font, brush, drawPoint);
            graphic.Dispose();

            return image;
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
            graphic.DrawImage(image, new RectangleF(p.X, p.Y, image.Width, image.Height));
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
            System.Drawing.Image img = GetPircemeImage(imagePath, new Size(200, 200));
            string mPath = imagePath.Insert(imagePath.LastIndexOf("."), "_m");
            img.Save(mPath, ImageFormat.Jpeg);
        }

        public static void ResizeImageToMS(string imagePath)
        {
            if (!imagePath.Contains("."))
            {
                Console.WriteLine("this image path is invaild: " + imagePath);
                return;
            }
            System.Drawing.Image img = GetPircemeImage(imagePath, new Size(150, 150));
            string mPath = imagePath.Insert(imagePath.LastIndexOf("."), "_ms");
            img.Save(mPath, ImageFormat.Jpeg);
        }

        public static void ResizeImageToS(string imagePath)
        {
            if (!imagePath.Contains("."))
            {
                Console.WriteLine("this image path is invaild: " + imagePath);
                return;
            }
            System.Drawing.Image img = GetPircemeImage(imagePath, new Size(90, 90));
            string mPath = imagePath.Insert(imagePath.LastIndexOf("."), "_s");
            img.Save(mPath, ImageFormat.Jpeg);
        }

        public static void ResizeImageToL(string imagePath, string tagetPath)
        {
            if (!imagePath.Contains("."))
            {
                Console.WriteLine("this image path is invaild: " + imagePath);
                return;
            }
            FileInfo fileInfo = new FileInfo(tagetPath);
            if (!fileInfo.Directory.Exists)
            {
                fileInfo.Directory.Create();
            }
            System.Drawing.Image img = GetPircemeImage(imagePath, new Size(600, 600));
            img.Save(tagetPath, ImageFormat.Jpeg);
        }

        public static void ResizeImageToL(string imagePath)
        {
            if (!imagePath.Contains("."))
            {
                Console.WriteLine("this image path is invaild: " + imagePath);
                return;
            }
            System.Drawing.Image img = GetPircemeImage(imagePath, new Size(600, 600));
            string lPath = imagePath.Insert(imagePath.LastIndexOf("."), "_l");
            img.Save(lPath, ImageFormat.Jpeg);
        }

        public static void ResizeImageToAll(string imagePath)
        {
            ResizeImageToAll(imagePath, false);
        }

        public static void ResizeImageToAll(string imagePath, bool includeL)
        {
            ResizeImageToM(imagePath);
            ResizeImageToMS(imagePath);
            ResizeImageToS(imagePath);
            if (includeL)
            {
                ResizeImageToL(imagePath);
            }
        }

        public static bool DownAndResizeImage(string srcURL, string filePath)
        {
            return DownAndResizeImage(srcURL, filePath, false);
        }

        public static bool DownAndResizeImage(string srcURL, string filePath, bool includeL)
        {
            if (!File.Exists(filePath))
            {
                try
                {
                    if (DownImage(srcURL, filePath))
                    {
                        ResizeImageToAll(filePath, includeL);
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    LogWriter.WriteLineToFile(ConfigAppString.ExceptionLogPath, ex.Message + "\t" + ex.StackTrace);
                }
            }
            return false;
        }

        public static string DownAndResizeImage(string srcURL, string dir, string imageName, bool includeL)
        {
            try
            {
                string imageFileName = DownImage(srcURL, dir, imageName);
                string imageFilePath = dir + "\\" + imageFileName;
                if (!string.IsNullOrEmpty(imageFilePath))
                {
                    ResizeImageToAll(imageFilePath, includeL);
                    return imageFileName;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                LogWriter.WriteLineToFile(ConfigAppString.ExceptionLogPath, ex.Message + "\t" + ex.StackTrace);
            }
            return "";
        }

        private static HttpWebRequest GetHttpWebRequest(string srcURL)
        {
            HttpWebRequest httpWebRequest = HttpWebRequest.Create(srcURL) as HttpWebRequest;

            httpWebRequest.Method = "GET";
            httpWebRequest.UserAgent = "Mozilla/5.0 (Windows NT 6.2; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/27.0.1453.94 Safari/537.36";

            return httpWebRequest;
        }

        public static bool DownImage(string srcURL, string filePath)
        {
            if (!File.Exists(filePath))
            {
                try
                {
                    string dirPath = filePath.Substring(0, filePath.LastIndexOf('\\'));
                    CreateDir(dirPath);

                    HttpWebRequest httpWebRequest = GetHttpWebRequest(srcURL);
                    HttpWebResponse webResponse = httpWebRequest.GetResponse() as HttpWebResponse;

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

                    return true;
                }
                catch (Exception ex)
                {
                    LogWriter.WriteLineToFile(ConfigAppString.ExceptionLogPath, ex.Message + "\t" + ex.StackTrace);
                    Console.WriteLine(ex.Message);
                }
            }
            return false;
        }

        public static string DownImage(string srcURL, string dir, string fileName)
        {
            try
            {
                HttpWebRequest httpWebRequest = GetHttpWebRequest(srcURL);
                HttpWebResponse webResponse = httpWebRequest.GetResponse() as HttpWebResponse;

                CreateDir(dir);

                string ext;
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
            catch (Exception ex)
            {
                LogWriter.WriteLineToFile(ConfigAppString.ExceptionLogPath, ex.Message + "\t" + ex.StackTrace);
                Console.WriteLine(ex.Message);
            }

            return "";
        }

        public static void CreateDir(string path)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }
    }
}