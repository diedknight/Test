using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HotterWinds.Modules
{
    public partial class ResultMessage : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void ShowSuccess(string message)
        {
            tblResult.Visible = true;
            trSuccess.Visible = true;
            trInfo.Visible = false;
            trFail.Visible = false;
            lblSuccess.Text = message;
        }

        public void ShowFail(string message)
        {
            tblResult.Visible = true;
            trSuccess.Visible = false;
            trInfo.Visible = false;
            trFail.Visible = true;
            lblFail.Text = message;
        }

        public void ShowInfo(string message)
        {
            tblResult.Visible = true;
            trSuccess.Visible = false;
            trFail.Visible = false;
            trInfo.Visible = true;
            lblInfo.Text = message;

        }

        protected string GetPath()
        {
            string sPath = Request.ApplicationPath;
            if (sPath == "/")
            {
                sPath = "";
            }
            return sPath;
        }

        public void NotShow()
        {
            tblResult.Visible = false;
            trSuccess.Visible = false;
            trFail.Visible = false;
            trInfo.Visible = false;
            lblInfo.Text = "";
        }
    }
}