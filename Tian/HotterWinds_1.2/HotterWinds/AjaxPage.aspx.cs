using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using PriceMeChannel.App_Code_Not.Ajax;

namespace HotterWinds
{
    public partial class AjaxPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                
                string className = PriceMe.Utility.GetParameter("controller");
                string methodName = PriceMe.Utility.GetParameter("action");
                string param = PriceMe.Utility.GetParameter("param");
                string fromUrl = PriceMe.Utility.GetParameter("url");

                string userId = "";
                //var user = this.CurrentUser;
                //if (user != null && user.ProviderUserKey != null)
                //{
                //    userId = user.ProviderUserKey.ToString();
                //}

                if (string.IsNullOrEmpty(className) || string.IsNullOrEmpty(methodName))
                {
                    Response.Write("0");
                }
                else
                {
                    var tempParam = JsonConvert.DeserializeObject<Dictionary<string, object>>(param);

                    object obj = AjaxBuilder.Ajax(HttpContext.Current, className, methodName, tempParam, fromUrl, userId);

                    Response.Write(obj);
                }
            }
            catch
            {
                Response.Write("0");
            }
            finally
            {
                Response.End();
            }
        }
    }
}