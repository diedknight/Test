using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PricemeResource.Logic;

namespace PricemeResource.Pages
{
    public class MarkerImagesModel : PageModel
    {
        public void OnGet()
        {
            string page = Utility.GetParameter("page", this.Request);
            bool isNoLink = false;
            string nolink = Utility.GetParameter("isNoLink", this.Request);
            bool.TryParse(nolink, out isNoLink);
            int RetailerId = Utility.GetIntParameter("RetailerId", this.Request);
            string RetailerName = Utility.GetParameter("RetailerName", this.Request);
            string retailerLogoPath = Utility.GetParameter("retailerLogoPath", this.Request);
            string minPrice = Utility.GetParameter("minPrice", this.Request);

            System.IO.MemoryStream memoryStream = new System.IO.MemoryStream();

            string bgImagePath = WebSiteConfig.ImageWebsite + "/images/css/map-sbg-norma.png";
            System.Net.WebRequest request = System.Net.HttpWebRequest.Create(bgImagePath);
            System.Net.WebResponse response = request.GetResponse();

            if (page == "product")
            {
                if (!isNoLink && !string.IsNullOrEmpty(retailerLogoPath) && retailerLogoPath.Contains("."))
                {
                    retailerLogoPath = retailerLogoPath.Insert(retailerLogoPath.LastIndexOf('.'), "_s");
                    retailerLogoPath = retailerLogoPath.Replace("/images/RetailerImages/", "");
                    retailerLogoPath = WebSiteConfig.ImageWebsite + "/images/RetailerImages/" + retailerLogoPath;

                    System.Net.WebRequest request1;
                    System.Net.WebResponse response1;
                    try
                    {
                        request1 = System.Net.HttpWebRequest.Create(retailerLogoPath);
                        response1 = request1.GetResponse();
                    }
                    catch (Exception ex)
                    {
                        request1 = System.Net.HttpWebRequest.Create(WebSiteConfig.ImageWebsite + "/images/no_image_available.gif");
                        response1 = request1.GetResponse();
                    }

                    using (System.Drawing.Image bgImage = System.Drawing.Image.FromStream(response.GetResponseStream()))
                    {
                        System.Drawing.Image retailerLogoImage = System.Drawing.Image.FromStream(response1.GetResponseStream());

                        System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(bgImage);
                        graphics.DrawImage(retailerLogoImage, 5, 19, 63, 21);

                        System.Drawing.Pen pen = new System.Drawing.Pen(System.Drawing.Color.FromArgb(1, 103, 204));
                        graphics.DrawString(minPrice, new System.Drawing.Font("arial", 10f, System.Drawing.FontStyle.Bold), pen.Brush, new System.Drawing.PointF(4f, 4f));

                        bgImage.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
                        byte[] bytes = new byte[memoryStream.Length];

                        memoryStream.Seek(0, SeekOrigin.Begin);
                        memoryStream.Read(bytes, 0, bytes.Length);
                        
                        Response.ContentType = "image/png";
                        Response.ContentLength = bytes.Length;
                        
                        Response.Body.Write(bytes, 0, bytes.Length);
                    }
                }
                else
                {
                    using (System.Drawing.Image bgImage = System.Drawing.Image.FromStream(response.GetResponseStream()))
                    {
                        string retaiName = RetailerName;
                        if (RetailerName.Length > 8)
                            retaiName = RetailerName.Substring(0, 8) + ".. ";

                        System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(bgImage);
                        System.Drawing.Pen pen = new System.Drawing.Pen(System.Drawing.Color.FromArgb(0, 0, 0));
                        graphics.DrawString(retaiName, new System.Drawing.Font("arial", 8f), pen.Brush, new System.Drawing.PointF(4f, 19f));

                        pen = new System.Drawing.Pen(System.Drawing.Color.FromArgb(1, 103, 204));
                        graphics.DrawString(minPrice, new System.Drawing.Font("arial", 10f, System.Drawing.FontStyle.Bold), pen.Brush, new System.Drawing.PointF(4f, 4f));

                        bgImage.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
                        byte[] bytes = new byte[memoryStream.Length];

                        memoryStream.Seek(0, SeekOrigin.Begin);
                        memoryStream.Read(bytes, 0, bytes.Length);

                        Response.ContentType = "image/png";
                        Response.ContentLength = bytes.Length;

                        Response.Body.Write(bytes, 0, bytes.Length);
                    }
                }
            }
            else if (page == "retailer")
            {
                if (!isNoLink && !string.IsNullOrEmpty(retailerLogoPath) && retailerLogoPath.Contains("."))
                {
                    retailerLogoPath = retailerLogoPath.Insert(retailerLogoPath.LastIndexOf('.'), "_s");
                    retailerLogoPath = retailerLogoPath.Replace("/images/RetailerImages/", "");
                    retailerLogoPath = WebSiteConfig.ImageWebsite + "/images/RetailerImages/" + retailerLogoPath;

                    System.Net.WebRequest request1;
                    System.Net.WebResponse response1;
                    try
                    {
                        request1 = System.Net.HttpWebRequest.Create(retailerLogoPath);
                        response1 = request1.GetResponse();
                    }
                    catch (Exception ex)
                    {
                        request1 = System.Net.HttpWebRequest.Create(WebSiteConfig.ImageWebsite + "/images/no_image_available.gif");
                        response1 = request1.GetResponse();
                    }

                    using (System.Drawing.Image bgImage = System.Drawing.Image.FromStream(response.GetResponseStream()))
                    {
                        System.Drawing.Image retailerLogoImage = System.Drawing.Image.FromStream(response1.GetResponseStream());

                        System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(bgImage);
                        graphics.DrawImage(retailerLogoImage, 5, 10, 63, 21);
                        
                        bgImage.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
                        byte[] bytes = new byte[memoryStream.Length];

                        memoryStream.Seek(0, SeekOrigin.Begin);
                        memoryStream.Read(bytes, 0, bytes.Length);

                        Response.ContentType = "image/png";
                        Response.ContentLength = bytes.Length;

                        Response.Body.Write(bytes, 0, bytes.Length);
                    }
                }
                else
                {
                    using (System.Drawing.Image bgImage = System.Drawing.Image.FromStream(response.GetResponseStream()))
                    {
                        System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(bgImage);
                        System.Drawing.Pen pen = new System.Drawing.Pen(System.Drawing.Color.FromArgb(0, 0, 0));

                        string retaiName = RetailerName;
                        if (RetailerName.Length > 16)
                        {
                            retaiName = RetailerName.Substring(0, 16) + ".. ";
                            graphics.DrawString(retaiName, new System.Drawing.Font("arial", 8f), pen.Brush, new System.Drawing.PointF(4f, 10f));
                        }
                        else if (RetailerName.Length > 8 && RetailerName.Length <= 16)
                            graphics.DrawString(retaiName, new System.Drawing.Font("arial", 8f), pen.Brush, new System.Drawing.PointF(4f, 10f));
                        else
                            graphics.DrawString(retaiName, new System.Drawing.Font("arial", 10f), pen.Brush, new System.Drawing.PointF(4f, 10f));

                        bgImage.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
                        byte[] bytes = new byte[memoryStream.Length];

                        memoryStream.Seek(0, SeekOrigin.Begin);
                        memoryStream.Read(bytes, 0, bytes.Length);

                        Response.ContentType = "image/png";
                        Response.ContentLength = bytes.Length;

                        Response.Body.Write(bytes, 0, bytes.Length);
                    }
                }
            }
        }
    }
}