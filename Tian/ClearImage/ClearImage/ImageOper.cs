using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;

namespace ClearImage
{
    public class ImageOper
    {
        public static void Resize_M(string imagePath)
        {
            if (!imagePath.Contains("."))
            {
                Console.WriteLine("this image path is invaild: " + imagePath);
            }
            else
            {
                Image pircemeImage = ImageOper.GetPircemeImage(imagePath, new Size(200, 200));
                string filename = imagePath.Insert(imagePath.LastIndexOf("."), "_m");
                pircemeImage.Save(filename, ImageFormat.Jpeg);
            }
        }
        public static void Resize_Ms(string imagePath)
        {
            if (!imagePath.Contains("."))
            {
                Console.WriteLine("this image path is invaild: " + imagePath);
            }
            else
            {
                Image pircemeImage = ImageOper.GetPircemeImage(imagePath, new Size(150, 150));
                string filename = imagePath.Insert(imagePath.LastIndexOf("."), "_ms");
                pircemeImage.Save(filename, ImageFormat.Jpeg);
            }
        }
        public static void Resize_s(string imagePath)
        {
            if (!imagePath.Contains("."))
            {
                Console.WriteLine("this image path is invaild: " + imagePath);
            }
            else
            {
                Image pircemeImage = ImageOper.GetPircemeImage(imagePath, new Size(90, 90));
                string filename = imagePath.Insert(imagePath.LastIndexOf("."), "_s");
                pircemeImage.Save(filename, ImageFormat.Jpeg);
            }
        }
        public static void Resize_ss(string imagePath)
        {
            if (!imagePath.Contains("."))
            {
                Console.WriteLine("this image path is invaild: " + imagePath);
            }
            else
            {
                Image pircemeImage = ImageOper.GetPircemeImage(imagePath, new Size(35, 35));
                string filename = imagePath.Insert(imagePath.LastIndexOf("."), "_ss");
                pircemeImage.Save(filename, ImageFormat.Jpeg);
            }
        }

        public static void Resize_l(string imagePath)
        {
            if (!imagePath.Contains("."))
            {
                Console.WriteLine("this image path is invaild: " + imagePath);
            }
            else
            {
                Image pircemeImage = ImageOper.GetPircemeImage(imagePath, new Size(600, 600));
                string filename = imagePath.Insert(imagePath.LastIndexOf("."), "_l");
                pircemeImage.Save(filename, ImageFormat.Jpeg);
            }
        }

        public static Image ResizeImage(Image image, Size outputSize, bool scale)
        {
            Image result;
            if (scale)
            {
                if (image.Width < outputSize.Width && image.Height < outputSize.Height)
                {
                    result = image;
                    return result;
                }
                float num = (float)image.Height / (float)image.Width;
                float num2 = (float)outputSize.Height / (float)outputSize.Width;
                if (num > num2)
                {
                    float num3 = (float)outputSize.Height / num;
                    outputSize.Width = (int)num3;
                }
                else
                {
                    float num3 = (float)outputSize.Width * num;
                    outputSize.Height = (int)num3;
                }
            }
            result = new Bitmap(image, outputSize);
            return result;
        }

        public static Image ResizeImage(string imagePath, Size outputSize, bool scale)
        {
            Image image = Image.FromFile(imagePath);
            Image result = ImageOper.ResizeImage(image, outputSize, scale);
            image.Dispose();
            return result;
        }

        public static Image AddBackground(Image image, Size size)
        {
            Bitmap bitmap = new Bitmap(size.Width, size.Height);
            Graphics graphics = Graphics.FromImage(bitmap);
            graphics.FillRectangle(Brushes.White, new Rectangle(0, 0, size.Width, size.Height));
            graphics.DrawImage(image, new Rectangle(new Point
            {
                X = (size.Width - image.Width) / 2,
                Y = (size.Height - image.Height) / 2
            }, image.Size));
            image = bitmap;
            graphics.Dispose();
            return image;
        }

        public static Image AddBackground(string imagePath, Size size)
        {
            Image image = Image.FromFile(imagePath);
            Image result = ImageOper.AddBackground(image, size);
            image.Dispose();
            return result;
        }

        public static Image GetPircemeImage(Image image, Size outputSize)
        {
            Image image2 = ImageOper.ResizeImage(image, outputSize, true);
            return ImageOper.AddBackground(image2, outputSize);
        }

        public static Image GetPircemeImage(string imagePath, Size outputSize)
        {
            Image image = Image.FromFile(imagePath);
            Image pircemeImage = ImageOper.GetPircemeImage(image, outputSize);
            image.Dispose();
            return pircemeImage;
        }

    }
}
