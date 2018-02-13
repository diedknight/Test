using PriceMeCrawlerTask.Common.Log;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CheckInvalidImageUrl
{
    class Program
    {
        static void Main(string[] args)
        {
            Priceme.Infrastructure.Excel.ExcelSimpleHelper _helper = new Priceme.Infrastructure.Excel.ExcelSimpleHelper();
            _helper.Read(AppConfig.file);
            _helper.CurIndex = AppConfig.startRowIndex;

            string imgFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Cache", "temp");
            string dir = Path.GetDirectoryName(imgFile);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            while (true)
            {
                //remove existed file
                File.Delete(imgFile);

                var line = _helper.ReadLine();
                if (line == null) break;

                ImgState imgState = new ImgState();
                string url = line[AppConfig.columnIndex];

                //downlaod img
                DownlaodFile(url, imgFile);

                if (File.Exists(imgFile))
                {
                    imgState.StateStr = "valid image";
                    imgState.State = true;

                    try
                    {
                        using (Bitmap bitmap = new Bitmap(imgFile))
                        {
                            var border = GetBorder1(bitmap);
                            imgState.BorderWidth = border[0];
                            imgState.BorderHeight = border[1];

                            imgState.Width = bitmap.Width - (imgState.BorderWidth * 2);
                            imgState.Height = bitmap.Height - (imgState.BorderHeight * 2);
                        }
                    }
                    catch (Exception ex)
                    {
                        WriteLog(ex.Message);
                        WriteLog(ex.StackTrace);
                    }
                }
                else
                {
                    imgState.StateStr = "invalid image";
                    imgState.State = false;
                }

                //write to excel
                if (!string.IsNullOrEmpty(imgState.StateStr))
                {
                    _helper.CurIndex--;

                    if (imgState.State)
                    {
                        _helper.UpdateCell(line.Count, imgState.StateStr);
                        _helper.UpdateCell(line.Count + 1, imgState.Width + "x" + imgState.Height);
                    }
                    else
                    {
                        _helper.UpdateCell(false, true, line.Count, imgState.StateStr);
                    }

                    _helper.CurIndex++;
                }
            }

            //save
            _helper.Save(AppConfig.file);

        }

        public static void DownlaodFile(string url, string imgFile)
        {
            try
            {
                using (WebClient w = new WebClient())
                {
                    w.DownloadFile(url, imgFile);
                }
            }
            catch (Exception ex)
            {
                WriteLog("url_exception:" + url);
                WriteLog(ex.Message);
                WriteLog(ex.StackTrace);
            }
        }

        public static List<int> GetBorder1(Bitmap bitmap)
        {
            List<int> border = new List<int>();
            border.Add(0);
            border.Add(0);

            string colorName = "ffffffff";

            for (int hIndex = 0; hIndex < bitmap.Height; hIndex++)
            {
                for (int wIndex = 0; wIndex < bitmap.Width; wIndex++)
                {
                    var color = bitmap.GetPixel(wIndex, hIndex);
                    if (color.Name != colorName)
                    {
                        border[1] = hIndex;
                        break;
                    }
                }
                if (border[1] != 0) break;
            }

            for (int wIndex = 0; wIndex < bitmap.Width; wIndex++)
            {
                for (int hIndex = 0; hIndex < bitmap.Height; hIndex++)
                {
                    var color = bitmap.GetPixel(wIndex, hIndex);
                    if (color.Name != colorName)
                    {
                        border[0] = wIndex;
                        break;
                    }
                }

                if (border[0] != 0) break;
            }

            return border;
        }

        public static List<int> GetBorder(Bitmap bitmap)
        {
            List<int> border = new List<int>();
            border.Add(0);
            border.Add(0);

            int q1 = 0;
            int q2 = 0;
            int q3 = 0;

            string colorName = "ffffffff";
            colorName += "ffffffff";
            colorName += "ffffffff";

            //height
            q2 = bitmap.Width / 2;  //中点
            q1 = q2 / 2;    //1分位
            q3 = q2 + (q2 - q1);    //3分位

            for (int i = 0; i < bitmap.Height; i++)
            {
                var color1 = bitmap.GetPixel(q1, i);
                var color2 = bitmap.GetPixel(q2, i);
                var color3 = bitmap.GetPixel(q3, i);

                if (color1.Name + color2.Name + color3.Name != colorName)
                {
                    border[1] = i;
                    break;
                }
            }

            //width
            q2 = bitmap.Height / 2;  //中点
            q1 = q2 / 2;    //1分位
            q3 = q2 + (q2 - q1);    //3分位

            for (int i = 0; i < bitmap.Width; i++)
            {
                var color1 = bitmap.GetPixel(i, q1);
                var color2 = bitmap.GetPixel(i, q2);
                var color3 = bitmap.GetPixel(i, q3);

                if (color1.Name + color2.Name + color3.Name != colorName)
                {
                    border[0] = i;
                    break;
                }
            }

            return border;
        }

        public static void WriteLog(string msg)
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Log", "log.txt");

            Console.WriteLine(msg);
            XbaiLog.WriteLog(path, msg);
        }
    }
}
