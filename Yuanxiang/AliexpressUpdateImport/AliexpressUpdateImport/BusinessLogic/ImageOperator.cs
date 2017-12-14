using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

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
    }
}
