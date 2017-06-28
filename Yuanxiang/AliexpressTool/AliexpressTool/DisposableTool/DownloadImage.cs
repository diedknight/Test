using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AliexpressTool.DisposableTool
{
    public class DownloadImage
    {
        StreamWriter outWriterIo;
        string ImagePath = System.Configuration.ConfigurationManager.AppSettings["ImagePath"].ToString();

        public void Image()
        {
            string FilePath = System.Configuration.ConfigurationManager.AppSettings["FilePath"].ToString();

            string newfilepath = FilePath.Replace(".txt", "_new.txt");
            FileStream outWriterFile = File.Open(newfilepath, FileMode.Create, FileAccess.Write, FileShare.Read);
            outWriterIo = new StreamWriter(outWriterFile, System.Text.Encoding.Default);

            CreateDir(ImagePath);

            using (StreamReader streamReader = File.OpenText(FilePath))
            {
                string productLine;
                int i = 0;
                while ((productLine = streamReader.ReadLine()) != null)
                {
                    productLine = productLine.Replace("&amp;", "&");
                    string[] productInfoPart = productLine.Split('\t');

                    string newproductLine = productLine;
                    if (i == 0)
                    {
                        newproductLine += "\t" + "Picturename1" + "\t" + "Picturename2" + "\t" + "Picturename3" + "\t" + "Picturename4" + "\t" + "Picturename5" + "\t" + "Picturename6";
                        OutWriter(newproductLine);
                    }
                    else
                    {
                        string imagename = string.Empty;
                        if (!string.IsNullOrEmpty(productInfoPart[15].ToString()))
                        {
                            imagename = DownloadImages(productInfoPart[15].ToString());
                            newproductLine += "\t" + imagename;
                        }
                        else
                            newproductLine += "\t" + string.Empty;

                        if (!string.IsNullOrEmpty(productInfoPart[16].ToString()))
                        {
                            imagename = DownloadImages(productInfoPart[16].ToString());
                            newproductLine += "\t" + imagename;
                        }
                        else
                            newproductLine += "\t" + string.Empty;

                        if (!string.IsNullOrEmpty(productInfoPart[17].ToString()))
                        {
                            imagename = DownloadImages(productInfoPart[17].ToString());
                            newproductLine += "\t" + imagename;
                        }
                        else
                            newproductLine += "\t" + string.Empty;

                        if (!string.IsNullOrEmpty(productInfoPart[18].ToString()))
                        {
                            imagename = DownloadImages(productInfoPart[18].ToString());
                            newproductLine += "\t" + imagename;
                        }
                        else
                            newproductLine += "\t" + string.Empty;

                        if (!string.IsNullOrEmpty(productInfoPart[19].ToString()))
                        {
                            imagename = DownloadImages(productInfoPart[19].ToString());
                            newproductLine += "\t" + imagename;
                        }
                        else
                            newproductLine += "\t" + string.Empty;

                        if (!string.IsNullOrEmpty(productInfoPart[20].ToString()))
                        {
                            imagename = DownloadImages(productInfoPart[20].ToString());
                            newproductLine += "\t" + imagename;
                        }
                        else
                            newproductLine += "\t" + string.Empty;

                        OutWriter(newproductLine);
                    }

                    i++;
                }
            }
        }

        private string DownloadImages(string url)
        {
            string imagename = GetRandomString(9);
            imagename = DownImage(url, imagename);

            return imagename;
        }

        public string DownImage(string srcURL, string fileName)
        {
            HttpWebRequest httpWebRequest = GetHttpWebRequest(srcURL);
            HttpWebResponse webResponse = httpWebRequest.GetResponse() as HttpWebResponse;

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

            string filePath = ImagePath + "\\" + fileName + ext;
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
            return filePath;
        }

        private HttpWebRequest GetHttpWebRequest(string srcURL)
        {
            HttpWebRequest httpWebRequest = HttpWebRequest.Create(srcURL) as HttpWebRequest;

            httpWebRequest.Method = "GET";
            httpWebRequest.UserAgent = "Mozilla/5.0 (Windows NT 6.2; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/27.0.1453.94 Safari/537.36";

            return httpWebRequest;
        }

        public void CreateDir(string path)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }

        private void OutWriter(string info)
        {
            outWriterIo.WriteLine(info);
            outWriterIo.Flush();
        }

        private string GetRandomString(int length)
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
