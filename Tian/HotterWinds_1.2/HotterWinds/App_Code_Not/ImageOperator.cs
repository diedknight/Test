using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

public class ImageOperator
{
    public System.Drawing.Image AddLogo(System.Drawing.Image image, string drawString, Font font, Brush brush, PointF drawPoint)
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

    public System.Drawing.Image ResizeImage(System.Drawing.Image image, Size outputSize, bool scale)
    {
        if (scale)
        {
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

    public System.Drawing.Image AddBackground(System.Drawing.Image image, Size size)
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
}

