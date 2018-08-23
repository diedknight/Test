using Newtonsoft.Json;
using Priceme.Deals.Code.Ajax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Priceme.Deals
{
    public partial class AjaxPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string className = this.GetParameter("controller");
                string methodName = this.GetParameter("action");
                string param = this.GetParameter("param");
                string fromUrl = this.GetParameter("url");

                if (string.IsNullOrEmpty(className) || string.IsNullOrEmpty(methodName))
                {
                    Response.Write("0");
                }
                else
                {
                    var tempParam = JsonConvert.DeserializeObject<Dictionary<string, object>>(param);

                    object obj = AjaxBuilder.Ajax(HttpContext.Current, className, methodName, tempParam, fromUrl);

                    Response.Write(obj);
                }
            }
            catch (Exception ex)
            {
                Response.Write("0");
            }
            finally
            {
                Response.End();
            }
        }

        public string GetParameter(string sParam)
        {
            if (System.Web.HttpContext.Current.Request[sParam] != null)
            {
                return System.Web.HttpContext.Current.Request[sParam];
            }
            else
            {
                return "";
            }
        }

    }
}