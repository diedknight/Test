using PriceMeCommon.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Priceme.Deals
{
    public partial class Recommend : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ((Main)this.Master).Breadcrumb = "Recommend";
        }

        protected void btnSend_Click(object sender, EventArgs e)
        {
            string code = this.txtCode.Text.Trim();
            string des = this.txtDes.Text.Trim();            
            string email = this.txtEmail.Text.Trim();
            //string storeNmae = this.txtStoreName.Text.Trim();
            //DateTime until = this.txtUntil.Text.Trim() != "" ? Convert.ToDateTime(this.txtUntil.Text.Trim()) : DateTime.MinValue;
            string value = this.txtValue.Text.Trim();
            string voucherName = this.txtVoucherName.Text.Trim();
            string voucherUrl = this.txtVoucherUrl.Text.Trim();

            DealsVoucher.Insert(code, des, email, "", Convert.ToDateTime("1900-1-1"), value, voucherName, voucherUrl);

            Response.Redirect("/voucher");
        }
    }
}