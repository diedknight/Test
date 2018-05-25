using PriceMe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HotterWinds
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this.txtUserName.Text))
            {
                this.ltlErrorMsg.Text = "<ul class=\"woocommerce-error\"><li><strong>Error:</strong> Username is required.</li></ul>";
                return;
            }

            if (string.IsNullOrWhiteSpace(this.txtPassword.Text))
            {
                this.ltlErrorMsg.Text = "<ul class=\"woocommerce-error\"><li><strong>Error:</strong> Password is required.</li></ul>";
                return;
            }

            var user = HotterWindsDBA.aspnet_User.SingleOrDefault(item => item.UserName == this.txtUserName.Text);
            var userId = user == null ? Guid.NewGuid() : user.UserId;
            HotterWindsDBA.aspnet_Membership ship = null;

            if (user != null)
            {
                string password = PriceMe.Utility.EncryptStr(this.txtPassword.Text);
                ship = HotterWindsDBA.aspnet_Membership.SingleOrDefault(item => item.UserId == user.UserId && item.Password == password);
            }

            if (user == null || ship == null)
            {
                this.ltlErrorMsg.Text = "<ul class=\"woocommerce-error\"><li><strong>Error:</strong> The password you entered for the username " + this.txtUserName.Text + " is incorrect.</li></ul>";
                return;
            }
            else
            {
                var json = JSONHelper.ObjectToJSON(new UserData()
                {
                    name = user.UserName,
                    sex = true,
                    isapproveemail = true,
                    countryid = 3,
                    email = ship.Email,
                    parseid = user.ParseID,
                    userid = user.UserId.ToString(),
                    createon = ship.CreateDate,
                    pass = ship.Password,
                    firstname = "",
                    lastname = "",
                    logintype = 0
                });
                json = Utility.EncryptStr(json).ToLower();

                HttpCookie c = new HttpCookie("our_custom_session_cookienew_xxxx", json);
                c.Path = "/";
                //c.Domain = "priceme.co.nz";
                //c.Domain = "192.168.1.109";
                c.Secure = false;
                c.HttpOnly = true;

                if (this.checkRememberMe.Checked)
                {
                    c.Expires = DateTime.Now.AddMonths(1);
                }

                Response.SetCookie(c);

                if (Session["returnUrl"] == null)
                {
                    Response.Redirect("/default.aspx");
                }
                else
                {
                    Response.Redirect(Session["returnUrl"].ToString());
                }
            }

        }

        protected void btnRegister_Click(object sender, EventArgs e)
        {
            var user = HotterWindsDBA.aspnet_User.SingleOrDefault(item => item.UserName == this.txtRegEmail.Text);

            if (user != null)
            {
                this.ltlErrorMsg.Text = "<ul class=\"woocommerce-error\"><li><strong>Error:</strong> An account is already registered with your email address. Please login.</li></ul>";
                return;
            }

            var appId = HotterWindsDBA.aspnet_Application.All().First().ApplicationId;

            user = new HotterWindsDBA.aspnet_User();
            user.ApplicationId = appId;
            user.UserId = Guid.NewGuid();
            user.UserName = this.txtRegEmail.Text;
            user.LoweredUserName = this.txtRegEmail.Text.ToLower();
            user.IsAnonymous = false;
            user.LastActivityDate = DateTime.Now;

            user.Save();

            string password = PriceMe.Utility.EncryptStr(this.txtRegPassword.Text);

            var ship = new HotterWindsDBA.aspnet_Membership();
            ship.ApplicationId = appId;
            ship.UserId = user.UserId;
            ship.Password = password;
            ship.PasswordFormat = 1;
            ship.PasswordSalt = password;
            ship.IsApproved = true;
            ship.IsLockedOut = false;
            ship.CreateDate = DateTime.Now;
            ship.LastLoginDate = DateTime.Now;
            ship.LastPasswordChangedDate = DateTime.Now;
            ship.LastLockoutDate = DateTime.Now;
            ship.FailedPasswordAttemptCount = 0;
            ship.FailedPasswordAttemptWindowStart = DateTime.Now;
            ship.FailedPasswordAnswerAttemptCount = 0;
            ship.FailedPasswordAnswerAttemptWindowStart = DateTime.Now;

            ship.Save();


            var json = JSONHelper.ObjectToJSON(new UserData()
            {
                name = user.UserName,
                sex = true,
                isapproveemail = true,
                countryid = 3,
                email = ship.Email,
                parseid = user.ParseID,
                userid = user.UserId.ToString(),
                createon = ship.CreateDate,
                pass = ship.Password,
                firstname = "",
                lastname = "",
                logintype = 0
            });
            json = Utility.EncryptStr(json).ToLower();

            HttpCookie c = new HttpCookie("our_custom_session_cookienew_xxxx", json);
            c.Path = "/";
            //c.Domain = "priceme.co.nz";
            //c.Domain = "192.168.1.109";
            c.Secure = false;
            c.HttpOnly = true;

            if (this.checkRememberMe.Checked)
            {
                c.Expires = DateTime.Now.AddMonths(1);
            }

            Response.SetCookie(c);

            if (Session["returnUrl"] == null)
            {
                Response.Redirect("/default.aspx");
            }
            else
            {
                Response.Redirect(Session["returnUrl"].ToString());
            }

        }
    }
}