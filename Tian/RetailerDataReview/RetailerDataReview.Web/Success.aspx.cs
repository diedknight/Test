using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RetailerDataReview.Web
{
    public partial class Success : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int rid = this.Query("rid", -1);
            string token = this.Query("token");

            if (token.ToLower() != MD5(rid.ToString()).ToLower()) return;

            PriceMeDBA.RetailerDataReview info = PriceMeDBA.RetailerDataReview.SingleOrDefault(item => item.RetailerId == rid);
            if (info != null) return;

            info = new PriceMeDBA.RetailerDataReview();
            info.RetailerId = rid;
            info.IsCorrect = true;
            info.Save();
        }


        private int Query(string name, int defVal = 0)
        {
            int rid = defVal;

            string ridStr = Request.QueryString[name];
            if (ridStr == null || ridStr.Length == 0) return rid;

            int.TryParse(ridStr, out rid);

            return rid;
        }

        private string Query(string name)
        {
            string str = Request.QueryString[name];
            if (str == null) return "";

            return str;
        }

        private string MD5(string str)
        {
            using (MD5 md5 = new MD5CryptoServiceProvider())
            {
                byte[] result = Encoding.UTF8.GetBytes(str);
                byte[] output = md5.ComputeHash(result);
                return BitConverter.ToString(output).Replace("-", "");
            }
        }

    }
}