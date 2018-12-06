using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ChristmasSite.Logic;
using ChristmasSite.Code.Ajax;
using Newtonsoft.Json;

namespace ChristmasSite.Pages
{
    public class AjaxPageModel : PageModel
    {
        public void OnGet()
        {
            try
            {
                string className = Utility.GetParameter("controller", this.Request);
                string methodName = Utility.GetParameter("action", this.Request);
                string param = Utility.GetParameter("param", this.Request);
                string fromUrl = Utility.GetParameter("url", this.Request);

                if (string.IsNullOrEmpty(className) || string.IsNullOrEmpty(methodName))
                {
                    byte[] bytes = System.Text.Encoding.ASCII.GetBytes("0");
                    Response.ContentType = "image/png";
                    Response.ContentLength = bytes.Length;

                    Response.Body.Write(bytes, 0, bytes.Length);
                }
                else
                {
                    var tempParam = JsonConvert.DeserializeObject<Dictionary<string, object>>(param);

                    object obj = AjaxBuilder.Ajax(HttpContext, className, methodName, tempParam, fromUrl);

                    byte[] bytes = System.Text.Encoding.ASCII.GetBytes(obj.ToString());
                    Response.ContentType = "image/png";
                    Response.ContentLength = bytes.Length;

                    Response.Body.Write(bytes, 0, bytes.Length);
                }
            }
            catch (Exception ex)
            {
                byte[] bytes = System.Text.Encoding.ASCII.GetBytes("0");
                Response.ContentType = "image/png";
                Response.ContentLength = bytes.Length;

                Response.Body.Write(bytes, 0, bytes.Length);
            }
        }
    }
}