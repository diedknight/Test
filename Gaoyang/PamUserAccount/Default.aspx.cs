using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using PriceMeDBA;
using SubSonic.Schema;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Transactions;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Xml;
using System.Linq;


namespace PamAccountGenerator
{
    public partial class Default : System.Web.UI.Page
    {
        Dictionary<string, string> ForeignCurrencys = new Dictionary<string, string>();
        public bool isAdmin = true;
        bool needToSendEmail = true;//是否发邮件
        bool isLocalHost = true;
        private string LogInUser = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            bool.TryParse(ConfigurationManager.AppSettings["IsLocalHost"], out isLocalHost);
            LogInUser = HttpContext.Current.User.Identity.Name;
            if (string.IsNullOrEmpty(LogInUser)
                || !HttpContext.Current.Request.IsAuthenticated)//可能登录超时了
            {
                try
                {
                    Response.Redirect("/Login.aspx?url=/", false);
                }
                catch (Exception ex)
                {
                    RecordException(ex, "PamAccount - Redirect");
                }
            }

            if (ConfigurationManager.AppSettings["User"].Contains(LogInUser.ToLower()))
                isAdmin = false;

            if (!IsPostBack)
            {
                #region init
                var sql = @"SELECT DISTINCT [countryID] ,[country] FROM [DBO].[CSK_Store_Retailer] r
  LEFT JOIN [CSK_Util_Country] c ON r.RetailerCountry = c.countryID WHERE c.countryID > 0 AND r.RetailerStatus = 1";
                StoredProcedure sp = new StoredProcedure("");
                sp.Command.CommandType = CommandType.Text;
                sp.Command.CommandSql = sql;
                var ds = sp.ExecuteDataSet();

                ddlCountry.Items.Clear();
                ddlCountry.DataTextField = "country";
                ddlCountry.DataValueField = "countryID";
                ddlCountry.SelectedValue = "3";
                ddlCountry.DataSource = ds;
                ddlCountry.DataBind();


                sql = "SELECT countryID, country FROM [DBO].[CSK_Util_Country] ORDER BY country";
                sp = new StoredProcedure("");
                sp.Command.CommandType = CommandType.Text;
                sp.Command.CommandSql = sql;
                ds = sp.ExecuteDataSet();

                ddlBillingCountry.Items.Clear();
                ddlBillingCountry.DataTextField = "country";
                ddlBillingCountry.DataValueField = "countryID";
                ddlBillingCountry.SelectedValue = "3";
                ddlBillingCountry.DataSource = ds;
                ddlBillingCountry.DataBind();

                sql = "select AdminName,AdminID from CSK_Store_AdminInformation where Status=1";
                sp = new StoredProcedure("");
                sp.Command.CommandType = CommandType.Text;
                sp.Command.CommandSql = sql;
                ds = sp.ExecuteDataSet();

                string adminID = "";
                sql = "select AdminID from CSK_Store_AdminInformation where AdminName='" + LogInUser.Trim() + "'";
                sp = new StoredProcedure("");
                sp.Command.CommandType = CommandType.Text;
                sp.Command.CommandSql = sql;
                IDataReader reader = sp.ExecuteReader();
                while (reader.Read())
                {
                    adminID = reader["AdminID"].ToString().Trim();
                }

                ddlAdmin.DataTextField = "AdminName";
                ddlAdmin.DataValueField = "AdminID";
                ddlAdmin.DataSource = ds;
                ddlAdmin.DataBind();
                if (isAdmin && !string.IsNullOrEmpty(adminID))
                    ddlAdmin.SelectedValue = adminID;
                #endregion
                string rid = Request.QueryString["rid"];
                if (!string.IsNullOrEmpty(rid))
                    getRetailerInfoByRid(rid);
                else {
                    rdbNoNeedPsd.Checked = true;
                    rdo1.Checked = true;
                    paymentNo.Checked = true;
                }

                #region read CSK_Store_RetailerLeadSignUp

                var lid = Utility.GetIntParameter("lid");
                if (lid > 0)
                {
                    var sign = RetailerLeadHelper.ReadRetailerLead(lid);
                    OrganisationID.Value = sign.CapsuleOrgId.ToString();
                    RetailerLeadID.Value = sign.RetailerLeadID.ToString();
                    txtRName.Text = sign.RetailerName;
                    txtWUrl.Text = sign.WebsiteURL;
                    if (sign.StoreType > 0)
                        ddlStoreType.SelectedValue = sign.StoreType.ToString();
                    ddlBillingCountry.SelectedValue = ddlCountry.SelectedValue = sign.SiteCountryID.ToString();
                    txtFUrl.Text = sign.FeedURL;
                    txtFirstName.Text = sign.ContactFirstName;
                    txtLastName.Text = sign.ContactLastName;
                    txtEAddress.Text = sign.ContactEmail;
                    txtTellNum.Text = sign.Phone;
                    txtGSTNumber.Text = sign.GSTNumber;
                }

                #endregion
            }

            txtRName.Focus();
            //string action = Request.Form["action"],rid=Request.Form["rid"];
            //if (!string.IsNullOrEmpty(action)) {
            //    getRetailerInfoByRid(int.Parse(rid));
            //    Response.End();
            //}
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            if (ddlCountry.SelectedValue == "-1")
            {
                lblMsg.Text = "Please select a Retailer Country";
                return;
            }

            //Response.Write(ddlPPC.SelectedValue+"============");
            //return;
            #region check name

            var rName = txtRName.Text.Trim();
            var uName = rName.Replace(" ", "").ToLower().Trim();
            var pwd = GetRndString(8, true, true, false, false, "");//uName + new Random().Next(1000, 10000); ;
            var userName = "";
            var r = CSK_Store_Retailer.SingleOrDefault(p => p.RetailerName.ToLower() == rName.ToLower());
            //if (r != null && r.RetailerId > 0)
            //{
            //    lblMsg.Text = "The retailer name has existed, please change the retailer name.";
            //    return;
            //}

            #endregion


            if (isAdmin && rdbTCSNo.Checked)
                needToSendEmail = false;
            

            #region new User

            MembershipCreateStatus membershipCreateStatus;
            
            string userKey = string.Empty, processMsg = string.Empty;
            bool Done = false;
            int retailerID = 0;
            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    #region retailer
                  
                    CSK_Store_Retailer retailer = new CSK_Store_Retailer();
                    if (r != null && r.RetailerId > 0) {
                        retailer = r;
                        if (retailer.RetailerStatus == 99) {
                            //如果选择的这个网店还是inactive的话，除了上面的这些外，你还需要把这个网店改成active才行，并且把这个网店的所有产品放为inactive
                            var updateRp = CSK_Store_RetailerProduct.Find(s => s.RetailerId == r.RetailerId && s.RetailerProductStatus);
                            foreach (var up in updateRp) {
                                up.RetailerProductStatus = false;
                                up.ModifiedBy = "Generator";
                                up.ModifiedOn = DateTime.Now;
                                up.Save();
                            }
                           
                            sendEmail("Email from Merchant Generator", "Someone chose an existing retailer (Retailerid is " + r.RetailerId + "), and set all products to inactive, please check it. ");
                        }


                        //如果Accept T&Cs:选择的是Yes， 点Sign up按钮后，你需要判断retailer表中的列IsSetupComplete 是否为0，如果为0，就不需要做什么。如果为1，你还需要把PPCmember表的列IsNoLinkToPPC改为1。
                        //if (rdbTCSYes.Checked)
                        //{
                        //    if (retailer.IsSetupComplete ?? false)
                        //    {
                        //        var ppcs = CSK_Store_PPCMember.SingleOrDefault(s => s.RetailerId == r.RetailerId);
                        //        ppcs.IsNoLinkToPPC = true;
                        //        ppcs.Save();
                        //    }
                        //}
                    }else
                        retailer.IsSetupComplete = false;
                    #region retailerInfo
                    //retailer.SetIsNew(true);
                    retailer.RetailerName = rName;
                    retailer.RetailerURL = txtWUrl.Text.Trim();
                    retailer.RetailerCountry = int.Parse(ddlCountry.SelectedValue);
                    retailer.RetailerContactName = txtFirstName.Text.Trim();
                    retailer.ContactLasName = string.IsNullOrEmpty(txtLastName.Text.Trim()) ? "" : txtLastName.Text.Trim();
                    retailer.ContactEmail = txtEAddress.Text.Trim();
                    retailer.ContactFirstName = "";
                    retailer.AdminComments = txtComment.Text.Trim();

                    if (rdo2.Checked)
                        retailer.AdminComments = retailer.AdminComments + " The customer chose Fetcher.";
                    else if(rdo4.Checked)
                        retailer.AdminComments = retailer.AdminComments + " The customer chose Api2Cart.";

                    int oid = 0;
                    int.TryParse(OrganisationID.Value, out oid);
                    retailer.CapsuleOrgId = oid;

                    //default value
                    retailer.RetailerStatus = 1;
                    
                    //retailer.SetInactiveReason = null;
                    retailer.StoreType = byte.Parse(ddlStoreType.SelectedValue);
                    retailer.RetailerRatingSum = 0;
                    retailer.RetailerTotalRatingVotes = 0;
                    retailer.IncludeGST = true;
                    retailer.IsCreditcard = false;
                    retailer.CCFee = -1;
                    retailer.IsCertificated = false;
                    retailer.IsParallelImported = false;
                    retailer.TechnicalAdminID = int.Parse(ddlAdmin.SelectedValue);
                    retailer.isCPA = false;
                    retailer.IsPickUp = false;
                    retailer.SecurePayment = false;
                    retailer.PhoneSupport = false;
                    retailer.ReturnsAccepted = false;

                    //var ForeignCurrency = string.Empty;
                    retailer.ForeignCurrency = "";//非空
                    retailer.RetailerCrawlerInfo = 0;//非空

                    retailer.TradingName = "";
                    retailer.YearEstablished = "0";
                    retailer.ContactTelephone = "";
                    retailer.BusinessMobile = "";
                    retailer.RetailerPhone = "";
                    retailer.RetailerFax = "";
                    retailer.RetailerCity = retailer.RetailerCountry == 3 ? 2 : 0;
                    retailer.RetailerCityDistrict = "";
                    retailer.VideoCode = "";
                    retailer.Videourl = "";
                    retailer.PriceDescriptor = "";
                    retailer.RetailerShortDescription = "";
                    retailer.RetailerMessage = "";
                    retailer.RetailerImage = 0;
                    retailer.Location = "";
                    retailer.LocationCategoryId = 0;
                    retailer.RetailerPaymentType = 0;
                    retailer.Availability = "";
                    //retailer.AdminComments = "";
                    retailer.CurrencyCode = "";
                    retailer.RetailerAffiliates = "";
                    retailer.ReturnPolicy = "";
                    retailer.DeliveryInfo = "";
                    retailer.AustraliaDelivery = false;
                    retailer.Discounts = "";
                    retailer.LogoFile = "";
                    retailer.Postcode = "";
                    retailer.FrequencytypeID = 0;
                    retailer.CompanyEmail = "";
                    retailer.Building = "";
                    retailer.BusinessTollfreeNumber = "";
                    retailer.BusinessMobile = "";
                    retailer.Level = "";
                    retailer.Street = "";
                    retailer.Suburb = "";
                    retailer.District = "";
                    retailer.Region = "";
                    retailer.ProductsServices = "";
                    retailer.BrandsCarried = "";
                    retailer.NumberOfEmployees = "";
                    retailer.DeliveryTime = "";
                    retailer.GSTNumber = txtGSTNumber.Text;
                    retailer.CompanyRegNumber = "";
                    retailer.FullcompanyName = txtFullCompanyName.Text.Trim();
                    retailer.Keyword = "";
                    retailer.ISCUpdateTime = DateTime.Parse("1900-01-01 00:00:00.000");
                    retailer.RetailerTypeID = 1;
                    retailer.CheckSetupEmailBy = "";
                    retailer.RegistrationNumber = "";
                    retailer.Ref = "";
                    retailer.FinishLlater = 0;
                    retailer.ShoppingRetailerID = 0;
                    retailer.CurrencySymbol = "";
                    retailer.CompanyRegNumber = txtCompanyRegNum.Text.Trim();
                    retailer.RetailerPhone = txtTellNum.Text.Trim();
                    if (!String.IsNullOrEmpty(retailer.RetailerPhone))
                        retailer.PhoneSupport = true;

                    retailer.CreatedOn = DateTime.Now;
                    retailer.ModifiedOn = DateTime.Now;
                    retailer.CreatedBy = LogInUser;
                    retailer.ModifiedBy = LogInUser;
                    if (isAdmin)
                    {
                        retailer.CreatedBy = ddlAdmin.SelectedItem.Text;
                        retailer.ModifiedBy = ddlAdmin.SelectedItem.Text;
                    }

                    retailer.Save();
                    if (retailer.RetailerId <= 0)
                    {
                        processMsg += string.Format("Create retailer [{0}] fail.<br/>", retailer.RetailerName);
                    }

                    #endregion

                    #endregion

                    #region retailCrawlerInfo

                    CSK_Store_RetailerCrawlerInfo retailerCrawlerInfo = new CSK_Store_RetailerCrawlerInfo();
                    if (r != null && r.RetailerId > 0)
                    {
                        retailerCrawlerInfo = CSK_Store_RetailerCrawlerInfo.SingleOrDefault(s => s.RetailerId == r.RetailerId);
                        if (retailerCrawlerInfo == null)
                            retailerCrawlerInfo = new CSK_Store_RetailerCrawlerInfo();
                    }
                    else {
                        retailerCrawlerInfo.CrawlClassType = 3;

                        retailerCrawlerInfo.CrawlFetcherName =
                        (retailer.RetailerName.Length > 23
                        ? retailer.RetailerName.Substring(0, 23)
                        : retailer.RetailerName).Replace(" ", "")
                        + "Fetcher";
                    }
                    //retailerCrawlerInfo.SetIsNew(true);
                    retailerCrawlerInfo.CrawlName =
                        (retailer.RetailerName.Length > 50
                        ? retailer.RetailerName.Substring(0, 50)
                        : retailer.RetailerName).Replace(" ", "");
                    
                    retailerCrawlerInfo.RetailerId = retailer.RetailerId;
                    retailerCrawlerInfo.FeedType = 0;
                    if (rdo1.Checked)
                        retailerCrawlerInfo.FeedType = 6;
                    //else if (rdo2.Checked||rdo4.Checked)
                    //    retailerCrawlerInfo.CrawlClassType = 1;
                    else
                        retailerCrawlerInfo.FeedType = 8;

                    if (rdbFTPFeed.Checked)
                        retailerCrawlerInfo.IsFTP = true;
                    else
                        retailerCrawlerInfo.IsFTP = false;

                    if (rdo3.Checked)
                    {
                        retailerCrawlerInfo.IsWebHarvy = true;
                        retailerCrawlerInfo.IsFTP = false;
                    }
                    else retailerCrawlerInfo.IsWebHarvy = false;

                    

                    retailerCrawlerInfo.FrequencytypeID = 1;
                    retailerCrawlerInfo.CrawlerPriority = 1;
                    retailerCrawlerInfo.AutoImported = true;
                    retailerCrawlerInfo.IsCrawlerAtNoon = false;
                    retailerCrawlerInfo.IsHeader = true;
                    retailerCrawlerInfo.Isdailydeals = false;
                    retailerCrawlerInfo.Isdisplayoutofstock = true;
                    retailerCrawlerInfo.NeedDeliverManufacturer = false;
                    retailerCrawlerInfo.IsFeedExcludeCategory = false;
                    retailerCrawlerInfo.IsAddBrandToProduct = false;
                    retailerCrawlerInfo.IsPassword = false;
                    retailerCrawlerInfo.IsNewOnly = true;
                    retailerCrawlerInfo.IsLocalDirectory = false;
                    retailerCrawlerInfo.IsNoImage = false;
                    retailerCrawlerInfo.IsCategoryList = false;
                    retailerCrawlerInfo.IsRemoveQuotes = false;
                    retailerCrawlerInfo.IsPagesize = false;
                    retailerCrawlerInfo.IsDupliateProducts = false;
                    retailerCrawlerInfo.IsFreeShipping = false;
                    retailerCrawlerInfo.IsReplaceWithOtherKeyword = false;
                    retailerCrawlerInfo.IsMatchByUrl = true;
                    if (retailerCrawlerInfo.IsWebHarvy.Value)
                        retailerCrawlerInfo.IsDupliateProducts = true;

                    retailerCrawlerInfo.CategoryID = 0;
                    retailerCrawlerInfo.ProductInfoCategory = 0;
                    retailerCrawlerInfo.CrawlDateTime = DateTime.Parse("1900-01-01 00:00:00.000");
                    retailerCrawlerInfo.ImportDateTime = DateTime.Parse("1900-01-01 00:00:00.000");
                    retailerCrawlerInfo.ImportProductsLoaded = 0;
                    retailerCrawlerInfo.AdminComments = "";
                    retailerCrawlerInfo.FeedFormatid = 0;
                    retailerCrawlerInfo.FeedsType = "";

                    
                        retailerCrawlerInfo.FeedPassWord = txtPsd.Text.Replace("&;", "").Trim();
                        retailerCrawlerInfo.FeedUserName = txtUname.Text.Replace("&;", "").Trim();

                        if (txtFUrl.Text.Trim() == "http://*") retailerCrawlerInfo.CrawlStartURL = "";
                        else retailerCrawlerInfo.CrawlStartURL = txtFUrl.Text.Trim();

                        if (!String.IsNullOrEmpty(retailerCrawlerInfo.FeedUserName) && !String.IsNullOrEmpty(retailerCrawlerInfo.FeedPassWord))
                            retailerCrawlerInfo.IsPassword = true;
                    


                    retailerCrawlerInfo.FreeShippingKeyWord = "";
                    retailerCrawlerInfo.LocalDirectory = "";
                    retailerCrawlerInfo.NoImageKeyWord = "";
                    retailerCrawlerInfo.CrawlTime = "";
                    retailerCrawlerInfo.SeparatedCategory = "";
                    retailerCrawlerInfo.IsImagesDownload = false;
                    retailerCrawlerInfo.ImageUrl = "";
                    retailerCrawlerInfo.CategoryListUrl = "";
                    retailerCrawlerInfo.ReplaceKeyword = "";
                    retailerCrawlerInfo.KeywordTobeReplaced = "";
                    retailerCrawlerInfo.XlsSheetName = "";
                    retailerCrawlerInfo.IsSameColumnMatch = false;

                    retailerCrawlerInfo.CreatedOn = DateTime.Now;
                    retailerCrawlerInfo.CreatedBy = LogInUser;
                    retailerCrawlerInfo.ModifiedOn = DateTime.Now;
                    retailerCrawlerInfo.ModifiedBy = LogInUser;

                    retailerCrawlerInfo.Save();
                    if (retailerCrawlerInfo.RetailerCrawlerInfoID <= 0)
                    {
                        processMsg += string.Format("Create retailerCrawlerInfo of [{0}] fail.<br/>", retailer.RetailerName);
                    }

                    var sql1 = string.Format("UPDATE CSK_Store_Retailer SET RetailerCrawlerInfo = {0} WHERE RetailerId = {1}",
                        retailerCrawlerInfo.RetailerCrawlerInfoID, retailerCrawlerInfo.RetailerId.Value);
                    StoredProcedure sp1 = new StoredProcedure("");
                    sp1.Command.CommandSql = sql1;
                    sp1.Command.CommandType = CommandType.Text;
                    sp1.Execute();

                    #endregion

                    #region new user

                    decimal ppcRate = 0;
                    string gst = string.Empty;

                    

                    if (r != null && r.RetailerId > 0)
                    {
                        #region 修改已经存在的网店

                       


                        var ppcMember = CSK_Store_PPCMember.SingleOrDefault(s => s.RetailerId == r.RetailerId);
                        var user = Membership.GetUser(Guid.Parse(ppcMember.UserId));
                        if (user != null)
                        {
                            //var oldPassword = user.ResetPassword();
                            //user.ChangePassword(oldPassword, pwd);

                            var item = aspnet_MembershipInfo.SingleOrDefault(s => s.UserID == ppcMember.UserId);
                           // item.SetIsNew(true);
                            item.UserID = user.ProviderUserKey.ToString();
                            item.FirstName = retailer.RetailerContactName;
                            item.LastName = retailer.ContactLasName;
                            item.RetailerID = retailer.RetailerId;

                            item.Sex = "";
                            item.Postcode = "";
                            item.UserPhoto = "";
                            item.Address = "";
                            item.Save();

                        }

                        CSK_Store_PPCMember ppc = ppcMember;
                        //ppc.SetIsNew(true);
                        ppc.RetailerId = retailer.RetailerId;
                        ppc.RetailerCountry = int.Parse(ddlBillingCountry.SelectedValue);
                        ppc.InvoiceCountry = ppc.RetailerCountry;
                        bool flag = true;
                        if (retailer.RetailerCountry != 3) flag = false;

                        var FixedCPCRate = 0.25m;
                        //if (ddlCountry.SelectedValue == "3")
                        //{
                            var cpcRate = ddlPPC.SelectedValue;
                            decimal.TryParse(cpcRate, out FixedCPCRate);
                            if (FixedCPCRate == 0) FixedCPCRate = 0.25m;
                        //}

                        ppc.FixedCPCRate = decimal.Parse(FixedCPCRate.ToString("f2"));
                        ppc.PPCTime = 24;
                        ppc.Categories = "";//非空
                        ppc.Discount = 1;//非空
                        if (rdbNolink.Checked)
                            ppc.PPCMemberTypeID = 5;
                        else
                            ppc.PPCMemberTypeID = 2;//非空
                        ppc.PPCIndex = 0.2m;//非空
                        ppc.IsPPCtoNolink = false;//非空
                        ppc.isPAMPowerUser = false;//非空
                        //ppc.IsNoLinkToPPC = false;//非空
                        ppc.Startdate = DateTime.Now;//非空

                        ppc.IsCreditCardPaymentOnly = flag;
                        ppc.IsInvoiceReminder = flag;
                        ppc.IsAutomatedInvoice = flag;
                        ppc.IsPayMethod = flag;
                        ppc.IsAddGSTAmount = ppc.InvoiceCountry == 3 ? true : false;

                        ppc.PPCPlanID = 1;
                        ppc.MinimumDailyBudget = 10;
                        ppc.DailyCap = decimal.Parse(ddlDailyBudget.SelectedValue);
                        ppc.IsRestricted = false;
                        ppc.PPCDropOff = false;
                        ppc.IsFixedCPCRate = true;
                        ppc.IsMinimumCharge = ddlCountry.SelectedValue == "3" ? true : false;
                        if (paymentYes.Checked)
                            ppc.IsCreditCardPaymentOnly = false;
                        else if (paymentNo.Checked)
                            ppc.IsCreditCardPaymentOnly = true;
                        ppc.IsUploadRetailer = false;
                        ppc.PPCPaymentOptionId = 0;
                        ppc.PPCDropOnTime = 0;
                        ppc.IsSendEmail = false;
                        ppc.PPCRateDeals = 0m;
                        ppc.UserId = user.ProviderUserKey.ToString().ToLower();

                        //ppc.PPCAccountID = ppcAccount.id;
                        ppc.CreatedOn = DateTime.Now;
                        ppc.CreatedBy = LogInUser;
                        ppc.ModifiedOn = DateTime.Now;
                        ppc.ModifiedBy = LogInUser;

                        ppc.Save();

                        userName = user.UserName;

                        ppcRate = ppc.FixedCPCRate.Value;
                        #endregion
                    }
                    else
                    {
                        #region 新增新的网店
                        userName = retailer.RetailerName;

                        MembershipUser mu = Membership.CreateUser(uName, pwd, retailer.ContactEmail, "12rmb", "12rmb", true, out membershipCreateStatus);
                        
                        if (membershipCreateStatus.Equals(MembershipCreateStatus.Success))
                        {
                            bool userInRole = Roles.IsUserInRole(mu.UserName, "Retailer");
                            if (!userInRole)
                            {
                                Roles.AddUserToRoles(mu.UserName, new string[] { "Retailer" });
                            }
                            userKey = mu.ProviderUserKey.ToString();

                            if (!string.IsNullOrEmpty(retailer.RetailerContactName))
                            {
                                aspnet_MembershipInfo item = new aspnet_MembershipInfo();
                                item.SetIsNew(true);
                                item.UserID = userKey;
                                item.FirstName = retailer.RetailerContactName;
                                item.LastName = retailer.ContactLasName;
                                item.RetailerID = retailer.RetailerId;

                                item.Sex = "";
                                item.Postcode = "";
                                item.UserPhoto = "";
                                item.Address = "";
                                //item.MembershipType = 0;
                                //item.OtherEmail = "";
                                //item.RegisterIP = "";
                                //item.Birthday = null;

                                item.Save();

                                var ii = aspnet_MembershipInfo.SingleOrDefault(p => p.UserID == userKey);
                                if (ii == null)
                                {
                                    processMsg += string.Format("Create MembershipInfo [{0}] fail.<br />", uName);
                                }
                            }
                            else
                            {
                                processMsg += string.Format("retailer[{0}].RetailerContactName is null, MembershipInfo will not create.<br/>", retailer.RetailerName);
                            }


                            #region CSK_Store_PPCAccount
                            CSK_Store_PPCAccount ppcAccount = new CSK_Store_PPCAccount();

                            ppcAccount.SetIsNew(true);
                            ppcAccount.AmountDue = 0;
                            ppcAccount.TotalBalance = 0;
                            ppcAccount.Createdon = DateTime.Now;
                            ppcAccount.Createdby = LogInUser;
                            ppcAccount.Modifiedon = DateTime.Now;
                            ppcAccount.Modifiedby = LogInUser;

                            ppcAccount.Save();
                            if (ppcAccount.id <= 0)
                            {
                                processMsg += string.Format("Fail to create PPCAccount of Retailer:[{0}].<br />", retailer.RetailerName);
                            }

                            #endregion

                            #region ppc info

                            CSK_Store_PPCMember ppc = new CSK_Store_PPCMember();
                            ppc.SetIsNew(true);
                            ppc.RetailerId = retailer.RetailerId;
                            ppc.RetailerCountry = int.Parse(ddlBillingCountry.SelectedValue);
                            ppc.InvoiceCountry = ppc.RetailerCountry;
                            bool flag = true;
                            if (retailer.RetailerCountry != 3) flag = false;

                            var FixedCPCRate = 0.25m;
                            //if (ddlCountry.SelectedValue == "3")
                            //{
                                var cpcRate = ddlPPC.SelectedValue;
                                decimal.TryParse(cpcRate, out FixedCPCRate);
                                if (FixedCPCRate == 0) FixedCPCRate = 0.25m;
                            //}

                            ppc.FixedCPCRate = decimal.Parse(FixedCPCRate.ToString("f2"));
                            ppc.PPCTime = 24;
                            ppc.Categories = "";//非空
                            ppc.Discount = 1;//非空
                            if (rdbNolink.Checked)
                                ppc.PPCMemberTypeID = 5;
                            else
                                ppc.PPCMemberTypeID = 2;//非空
                            ppc.PPCIndex = 0.2m;//非空
                            ppc.IsPPCtoNolink = false;//非空
                            ppc.isPAMPowerUser = false;//非空
                            ppc.IsNoLinkToPPC = false;//非空
                            ppc.Startdate = DateTime.Now;//非空

                            ppc.IsCreditCardPaymentOnly = flag;
                            ppc.IsInvoiceReminder = flag;
                            ppc.IsAutomatedInvoice = flag;
                            ppc.IsPayMethod = flag;
                            ppc.IsAddGSTAmount = ppc.InvoiceCountry == 3 ? true : false;

                            ppc.PPCPlanID = 1;
                            ppc.MinimumDailyBudget = 10;
                            ppc.DailyCap = decimal.Parse(ddlDailyBudget.SelectedValue);
                            ppc.IsRestricted = false;
                            ppc.PPCDropOff = false;
                            ppc.IsFixedCPCRate = true;
                            ppc.IsMinimumCharge = ddlCountry.SelectedValue == "3" ? true : false;
                            if (paymentYes.Checked)
                                ppc.IsCreditCardPaymentOnly = false;
                            else if (paymentNo.Checked)
                                ppc.IsCreditCardPaymentOnly = true;
                            ppc.IsUploadRetailer = false;
                            ppc.PPCPaymentOptionId = 0;
                            ppc.PPCDropOnTime = 0;
                            ppc.IsSendEmail = false;
                            ppc.PPCRateDeals = 0m;


                            ppc.UserId = userKey.ToLower();

                            ppc.PPCAccountID = ppcAccount.id;

                            ppc.CreatedOn = DateTime.Now;
                            ppc.CreatedBy = LogInUser;
                            ppc.ModifiedOn = DateTime.Now;
                            ppc.ModifiedBy = LogInUser;

                            ppc.Save();
                            if (ppc.PPCMemberId <= 0)
                            {
                                processMsg += string.Format("Fail to create PPCMember of Retailer:[{0}].<br />", retailer.RetailerName);
                            }

                            ppcRate = ppc.FixedCPCRate.Value;
                            gst = retailer.RetailerCountry == ppc.InvoiceCountry ? " + gst" : " ";

                            #endregion

                            #region PPCBudgetHistory

                            CSK_Store_PPCBudgetHistory PPCBudgetHistory = new CSK_Store_PPCBudgetHistory();
                            PPCBudgetHistory.SetIsNew(true);
                            PPCBudgetHistory.PPCMemberId = ppc.PPCMemberId;
                            PPCBudgetHistory.PPC = ppc.FixedCPCRate.Value;
                            PPCBudgetHistory.DailyBudget = ppc.DailyCap.Value;
                            PPCBudgetHistory.CreatedOn = DateTime.Now;
                            PPCBudgetHistory.CreatedBy = LogInUser;
                            PPCBudgetHistory.ModifiedOn = DateTime.Now;

                            PPCBudgetHistory.Save();
                            if (PPCBudgetHistory.Id <= 0)
                            {
                                processMsg += string.Format("Fail to create PPCBudgetHistory of Retailer:[{0}].<br/>", retailer.RetailerName);
                            }

                            #endregion
                        }
                        else
                        {
                            lblMsg.Text = string.Format("Failed to create new user: {0}, Result:" + membershipCreateStatus);
                            processMsg += string.Format("Failed to create new user: {0}, Reason: {1}. And MembershipInfo,PPCAccount,PPCMember,PPCBudgetHistory will not create.<br />", uName, membershipCreateStatus);
                        }
                        #endregion
                    }

                    #endregion

                    #region CSK_Store_RetailerNewsletterSet

                    CSK_Store_RetailerNewsletterSet set = new CSK_Store_RetailerNewsletterSet();
                    set.RetailerID = retailer.RetailerId;
                    set.IsMonthlyEmail = true;
                    //set.IsWeeklyEmail = true;
                    set.CreatedOn = set.ModifiedOn = DateTime.Now;
                    set.CreatedBy = set.ModifiedBy = LogInUser;
                    set.Save();
                    if (set.ID <= 0)
                    {
                        processMsg += string.Format("Fail to create RetailerNewsletterSet of Retailer:[{0}].<br />", retailer.RetailerName);
                    }

                    #endregion

                    UpdateConfig("");

                    scope.Complete();
                 
                    #region 添加Cookie
                        var cookieDic = new Dictionary<string, string>();
                        cookieDic.Add("NewRetailerName", userName);
                        cookieDic.Add("NewUPwd", pwd);
                        cookieDic.Add("NewRetailerCountry", retailer.RetailerCountry.ToString());
                        cookieDic.Add("NewContactEmail", retailer.ContactEmail.ToString());
                        cookieDic.Add("NewRetailerContactName", retailer.RetailerContactName.ToString());
                        cookieDic.Add("NewRetailerId", retailer.RetailerId.ToString());
                        cookieDic.Add("NewNeedSendEmail", needToSendEmail.ToString());
                        cookieDic.Add("cookiePPCRate", ppcRate.ToString());
                        cookieDic.Add("cookieGST", gst);
                        batchAddCookies(cookieDic);
                    #endregion

                    Done = true;
                    retailerID = retailer.RetailerId;
                    if (string.IsNullOrEmpty(processMsg))
                    {
                        try
                        {
                            if (needToSendEmail)
                                Response.Redirect("SendEmail.aspx", false);
                            else
                                Response.Redirect("Success.aspx", false);
                        }
                        catch (Exception ex)
                        {
                            RecordException(ex, "PamAccount - Redirect");
                        }
                    }
                    else
                    {
                        RecordException(processMsg);
                    }

                }
                catch (TimeoutException toex)
                {
                    scope.Dispose();//roll back

                    SendErrorEmail(toex);

                    lblMsg.Text = "The server is busy, please wait and try again later.";
                }
                catch (Exception ex)
                {
                    scope.Dispose();//roll back

                    SendErrorEmail(ex);
                    
                    //lblMsg.Text = "We have noted this error and it will be reviewed by our technical staff. We will fix this without you needing to do anything further.";
                    lblMsg.Text = ex.Message+"===>"+ex.StackTrace;
                }
               
            }
            
            #region CSK_Store_RetailerLead && capsule
            if (Done)
            {
                RetailerLeadHelper.SaveRetailerLeadTracking(RetailerLeadID.Value, OrganisationID.Value, retailerID);
                var lid = 0;
                int.TryParse(RetailerLeadID.Value, out lid);
                if (lid <= 0)//RetailerLeadSingUp 表中没有这个retailer, 那么crm 中也没有相应记录, 添加到crm
                {
                    string pid = string.Empty, hid = string.Empty;
                    var oid = CapsuleCRMHelper.NewOrganisationAndPerson(rName, txtWUrl.Text.Trim(),
                        txtFirstName.Text.Trim(), txtLastName.Text.Trim(), txtEAddress.Text.Trim(),
                        ddlCountry.SelectedItem.Text, out pid, out hid);
                    var oid_ = 0;
                    int.TryParse(oid, out oid_);
                    RetailerLeadHelper.SaveRetailerCapsuleOrgId(retailerID, oid_);
                    //Session["NewLeadID"] = RetailerLeadHelper.NewRetailerLead(ddlCountry.SelectedValue,
                    //    rName, txtWUrl.Text.Trim(), txtFirstName.Text.Trim(), txtLastName.Text.Trim(), 
                    //    txtEAddress.Text.Trim(), oid_);
                }
            }

            #endregion

            #endregion

        }

        /// <summary>
        ///（获取Retailer信息通过RID）
        /// </summary>
        /// <param name="rid"></param>
        protected void getRetailerInfoByRid(string id) {
            int rid=int.Parse(id);
            var csr = CSK_Store_Retailer.SingleOrDefault(s => s.RetailerId ==rid);
            //General Info模块
            txtRName.Text = csr.RetailerName;
            txtFullCompanyName.Text = csr.FullcompanyName;
            txtTellNum.Text = csr.RetailerPhone;
            txtCompanyRegNum.Text = csr.CompanyRegNumber;
            txtWUrl.Text = csr.RetailerURL;
            ddlStoreType.SelectedValue = csr.StoreType.ToString();
            ddlCountry.SelectedValue = csr.RetailerCountry.ToString();

            var ppc = CSK_Store_PPCMember.SingleOrDefault(s => s.RetailerId == rid);
            //Billing模块
            if (ppc.PPCMemberTypeID == 5)
                rdbNolink.Checked = true;
            else
                rdbPPC.Checked = true;

            ddlPPC.SelectedValue = ppc.FixedCPCRate.ToString();
            if (ppc.FixedCPCRate.ToString() == "0.20")
                ddlPPC.SelectedValue = "0.2";
            else if(ppc.FixedCPCRate.ToString() == "0.30")
                ddlPPC.SelectedValue = "0.3";
            else if (ppc.FixedCPCRate.ToString() == "0.10") 
                ddlPPC.SelectedValue = "0.1";

            if (ppc.IsCreditCardPaymentOnly??false)
                paymentNo.Checked = true;
            else
                paymentYes.Checked = true;
            txtGSTNumber.Text = csr.GSTNumber;
            ddlDailyBudget.SelectedValue = ppc.DailyCap.ToString();
            ddlBillingCountry.SelectedValue = ppc.RetailerCountry.ToString();

            var csrci = CSK_Store_RetailerCrawlerInfo.SingleOrDefault(s => s.RetailerId == rid);
            //Data模块   
            
            statusMsg.Visible = true;
            if (csr.RetailerStatus == 1){
                rdo2.Visible = true;
                rdo4.Visible=true;
                statusMsg.Text = "This retailer currently is active.";
            }
            else if (csr.RetailerStatus == 99) {
                statusMsg.Text = "Please note that this retailer currently is inactive.";
                rdo2.Visible = false;
                rdo4.Visible = false;       
            }

            //Response.Write(csrci.CrawlStartURL+"<br/>");
            //Response.Write(csrci.FeedUserName + "<br/>");
            //Response.Write(csrci.FeedPassWord + "<br/>");

            txtFUrl.Text = csrci.CrawlStartURL;
            if (csrci.IsPassword ?? false){
                
                userNameTR.Attributes.Add("style", "display:non3e");
                psdTR.Attributes.Add("style", "display:non3e");

                rdbNeedPsd.Checked = true;
                txtUname.Text = csrci.FeedUserName;
                txtPsd.Text = csrci.FeedPassWord;
            }
            else
                rdbNoNeedPsd.Checked = true;
            if (csrci.CrawlClassType == 1)
            {
                if (csrci.CrawlFetcherName.ToLower() != "api2cartfetcher")
                    rdo2.Checked = true;
                else
                    rdo4.Checked = true;
                trRetailerFeed.Attributes.Add("hidden", "hidden");
                trFeedUri.Attributes.Add("hidden", "hidden");
                trNeedPsd.Attributes.Add("hidden", "hidden");
                userNameTR.Attributes.Add("hidden", "hidden");
                psdTR.Attributes.Add("hidden", "hidden");
            }
            else if (csrci.CrawlClassType > 1) {
                if (!csrci.CrawlStartURL.ToLower().Contains("webharvy"))
                {
                    rdo1.Checked = true;
                    if (csr.RetailerStatus != 99) {
                        if (csrci.CrawlStartURL.ToLower().Contains("priceme.co.nz") || csrci.CrawlStartURL.ToLower().Contains("shoppingcomretailerfeed") || string.IsNullOrEmpty(csrci.CrawlStartURL))
                        {
                            trRetailerFeed.Attributes.Add("hidden", "hidden");
                            trFeedUri.Attributes.Add("hidden", "hidden");
                            trNeedPsd.Attributes.Add("hidden", "hidden");

                            userNameTR.Attributes.Add("hidden", "hidden");
                            psdTR.Attributes.Add("hidden", "hidden");
                        }
                    }
                    
                }
                else {
                    rdo3.Checked = true;
                    trRetailerFeed.Attributes.Add("hidden", "hidden");
                    trFeedUri.Attributes.Add("hidden", "hidden");
                    trNeedPsd.Attributes.Add("hidden", "hidden");
                    userNameTR.Attributes.Add("hidden", "hidden");
                    psdTR.Attributes.Add("hidden", "hidden");
                }
                   


            }
            //Contact模块
            txtFirstName.Text = csr.RetailerContactName;
            txtLastName.Text = csr.ContactLasName;
            txtEAddress.Text = csr.ContactEmail;
        }


        protected string getRetailerCountName(string countryID) {
            var sql = @"SELECT DISTINCT [countryID] ,[country] FROM [DBO].[CSK_Store_Retailer] r
  LEFT JOIN [CSK_Util_Country] c ON r.RetailerCountry = c.countryID WHERE c.countryID =" + countryID + " AND r.RetailerStatus = 1";
            StoredProcedure sp = new StoredProcedure("");
            sp.Command.CommandType = CommandType.Text;
            sp.Command.CommandSql = sql;
            var ds = sp.ExecuteDataSet();
            if (ds != null && ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0]["country"].ToString();
            else
                return "==";
        }

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="from">发件人</param>
        /// <param name="subject">标题</param>
        /// <param name="body">内容</param>
        /// <param name="to">收件人</param>
        public static void sendEmail(string subject, string body)
        {
            string _aWSAccessKey = System.Configuration.ConfigurationManager.AppSettings["AWSAccessKey"];
            string _aWSSecretKey = System.Configuration.ConfigurationManager.AppSettings["AWSSecretKey"];
            string AdminEmail = ConfigurationManager.AppSettings["AdminEmail"];
            AmazonSimpleEmailServiceClient ses = new AmazonSimpleEmailServiceClient(_aWSAccessKey, _aWSSecretKey);

            SendEmailRequest seReq = new SendEmailRequest();
            seReq.Source = "PamAccountGenerator <info@channelyser.com>";

            Destination det = new Destination();
            det.ToAddresses = AdminEmail.Split(',').ToList();

            seReq.Destination = det;

            Message mes = new Message();
            Content con = new Content();
            con.Data = subject;
            mes.Subject = con;

            Body bodyObj = new Body();
            Content conHtml = new Content();
            conHtml.Data = body;
            bodyObj.Text = conHtml;
            bodyObj.Html = conHtml;
            mes.Body = bodyObj;

            seReq.Message = mes;

            //list = new List<string>();
            //list.Add(ConfigAppString.ReplyToEmail);
            //seReq.ReplyToAddresses = list;

            seReq.ReplyToAddresses = new List<string>() { "PamAccountGenerator <info@channelyser.com>" };

            ses.SendEmail(seReq);
        }



        protected void btnLogOut_Click(object sender, EventArgs e)
        {
            try
            {
                System.Web.Security.FormsAuthentication.SignOut();
                Response.Redirect("/Login.aspx?url=/", false);
            }
            catch (Exception ex)
            {
                RecordException(ex, "PamAccount - Redirect");
            }
        }

        /// <summary>
        /// 批量添加Cookies
        /// </summary>
        /// <returns></returns>
        protected void batchAddCookies(Dictionary<string, string> cookieDic)
        {
            foreach (var cookie in cookieDic) {
                HttpCookie hc = new HttpCookie(cookie.Key, cookie.Value);
                hc.Expires = DateTime.Now.AddHours(1);
                Response.Cookies.Add(hc);
            }
        }


        /// <summary>
        /// 生成随机字符串
        /// </summary>
        /// <param name="length">目标字符串的长度</param>
        /// <param name="useNum">是否包含数字，1=包含，默认为包含</param>
        /// <param name="useLow">是否包含小写字母，1=包含，默认为包含</param>
        /// <param name="useUpp">是否包含大写字母，1=包含，默认为包含</param>
        /// <param name="useSpe">是否包含特殊字符，1=包含，默认为不包含</param>
        /// <param name="custom">要包含的自定义字符，直接输入要包含的字符列表</param>
        /// <returns>指定长度的随机字符串</returns>
        private string GetRndString(int length, bool useNum, bool useLow, bool useUpp, bool useSpe, string custom)
        {
            byte[] b = new byte[4];
            new System.Security.Cryptography.RNGCryptoServiceProvider().GetBytes(b);
            Random rand = new Random(BitConverter.ToInt32(b, 0));
            StringBuilder s = new StringBuilder();
            StringBuilder str = new StringBuilder(custom);

            if (useNum == true) { str.Append("0123456789"); }
            if (useLow == true) { str.Append("abcdefghijkmnopqrstuvwxyz"); }//l和1不容易分清楚，所以去除 
            if (useUpp == true) { str.Append("ABCDEFGHIJKLMNOPQRSTUVWXYZ"); }
            if (useSpe == true) { str.Append("!\"#$%&'()*+,-./:;<=>?@[\\]^_`{|}~"); }

            var str2 = str.ToString();
            for (int i = 0; i < length; i++)
            {
                s.Append(str2.Substring(rand.Next(0, str.Length - 1), 1));
            }

            return s.ToString();
        }

        #region check the retailer name has exsit

        /// <summary>
        /// check the retailer name has exsit
        /// </summary>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                var retailer = txtRName.Text.Trim();
                if (string.IsNullOrEmpty(retailer))
                    lblSearchResult.Text = "Please enter Retailer name";
                else
                {
                    retailer = RemoveKeyword(retailer);
                    var ds = SearchRetailer(retailer);
                    if (ds != null && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                    {
                        lblSearchResult.ForeColor = System.Drawing.Color.Red;
                        lblSearchResult.Text = "The retailer might exist on PriceMe:";
                    }
                    else
                    {
                        lblSearchResult.ForeColor = System.Drawing.Color.Black;
                        lblSearchResult.Text = "No similar retailer on PriceMe. <br/>Please change another keyword to search or add a new retailer.";
                    }



                    rptRetailers.DataSource = ds;
                    rptRetailers.DataBind();

                    var ds2 = RetailerLeadHelper.SearchRetailer(retailer);
                    if (ds2.Count > 0)
                    {
                        lblSearchResult.ForeColor = System.Drawing.Color.Red;
                        lblSearchResult.Text = "";
                    }
                    rptRetailers2.DataSource = ds2;
                    rptRetailers2.DataBind();
                }
            }
            catch (Exception ex)
            {
                RecordException(ex, "PamAccount - Search Retailer");
            }
        }

        /// <summary>
        /// 去掉所有介词和定冠词
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private string RemoveKeyword(string input)
        {
            var keywords = new List<string>();
            string words = ConfigurationManager.AppSettings["IgnoreWord"];
            keywords.AddRange(words.Split(','));
            foreach (var str in keywords)
            {
                if (input.StartsWith(str + " "))
                    input = input.Substring((str + " ").Length);
                if (input.EndsWith(" " + str))
                    input = input.Substring(0, input.Length - (" " + str).Length);
                input = input.Replace(" " + str + " ", " ");
            }

            return "%" + input.Replace(" ", "%").Trim() + "%";
        }

        /// <summary>
        /// search same name retailer by input retailer name
        /// from CSK_Store_Retailer
        /// </summary>
        private DataSet SearchRetailer(string retailer)
        {
            string sql = string.Format("SELECT RetailerId,RetailerName,IsSetupComplete,RetailerStatus,RetailerCountry FROM CSK_Store_Retailer WHERE RetailerName LIKE '{0}' OR RetailerURL LIKE '{0}' ORDER BY RetailerCountry", retailer);
            StoredProcedure sp = new StoredProcedure("");
            sp.Command.CommandType = CommandType.Text;
            sp.Command.CommandSql = sql;
            sp.Command.CommandTimeout = 0;
            return sp.ExecuteDataSet();
        }

        /// <summary>
        /// get retailer link tag to priceme site
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="name"></param>
        /// <param name="country"></param>
        /// <returns></returns>
        public string GetRetailerUrlLink(string Id, string name, string country)
        {
            string aTag = "<a href=\"{0}\" target=\"_blank\" title=\"{1}\">{0}</a>";
            string tagUri = GetRetailerUrl(Id, name, country);
            return string.Format(aTag, tagUri, name);
        }

        /// <summary>
        /// get retailer link tag to priceme site
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="name"></param>
        /// <param name="country"></param>
        /// <returns></returns>
        public string GetRetailerUrlLink2(string Id, string name, string country)
        {
            string aTag = string.Format("<a href=\"{2}/Default.aspx?lid={0}\" title=\"{1}\">{1} exists in RetailerLead, please click here to add the retailer.</a>",
                Id, name, "");
            return aTag;
        }

        /// <summary>
        /// get retailer link url on priceme site
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="name"></param>
        /// <param name="country"></param>
        /// <returns></returns>
        private string GetRetailerUrl(string Id, string name, string country)
        {
            string siteRootUrl = GetSiteRootUrl(country);
            string retailerUri = GetRetailerUrl(Id, name);
            return siteRootUrl + retailerUri;
        }

        /// <summary>
        /// get priceme root site url of retailer country
        /// </summary>
        /// <returns></returns>
        private static string GetSiteRootUrl(string country)
        {
            string siteRootUrl = "http://www.priceme.co.nz";

            switch (country)
            {
                case "1":
                    siteRootUrl = "http://www.priceme.net.au";
                    break;
                case "3":
                    break;
                case "28":
                    siteRootUrl = "http://www.priceme.com.ph";
                    break;
                case "36":
                    siteRootUrl = "http://www.priceme.com.sg";
                    break;
                case "41":
                    siteRootUrl = "http://www.priceme.com.hk";
                    break;
                case "45":
                    siteRootUrl = "http://www.priceme.com.my";
                    break;
                case "51":
                    siteRootUrl = "http://www.priceme.co.id";
                    break;
                case "55":
                    siteRootUrl = "http://www.priceme.com.vn/";
                    break;
                case "56":
                    siteRootUrl = "http://www.priceme.com/";
                    break;
                default:
                    break;
            }
            return siteRootUrl;
        }

        /// <summary>
        /// get retailer uri
        /// </summary>
        /// <param name="retailerId"></param>
        /// <param name="retailerName"></param>
        /// <returns></returns>
        private static string GetRetailerUrl(string retailerId, string retailerName)
        {
            retailerName = FilterInvalidUrlPathChar(retailerName);

            retailerName = retailerName.Length > 80 ? retailerName.Substring(0, 80) : retailerName;
            retailerName.Replace("?", "");

            string sOut = string.Format("/{0}/r-{1}.aspx", retailerName, retailerId);

            return sOut;
        }

        //不能用[^\w-]+ 因为 \w 包括字母，数字，下划线，汉字等其它非符号
        private static Regex illegalReg = new Regex(@"[^a-z0-9-]+", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static Regex illegalReg2 = new Regex("-+", RegexOptions.Compiled);
        private static Regex illegalReg3 = new Regex("^-+|-+$", RegexOptions.Compiled);
        public static string FilterInvalidUrlPathChar(string sourceString)
        {
            sourceString = illegalReg.Replace(sourceString, "-");
            sourceString = illegalReg2.Replace(sourceString, "-");
            sourceString = illegalReg3.Replace(sourceString, "");
            return sourceString;
        }

        #endregion

        #region send email

        private string GetErrorEmailBody(Exception ex)
        {
            string body = "<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"border:1px solid #ccc;\">"
                   + "<tbody><tr><td>Time:</td><td>" + DateTime.Now.ToString()
                   + "</td></tr><tr><td>User:</td><td>" + LogInUser
                   + "</td></tr><tr><td>Error:</td><td>"
                   + "</td></tr><tr><td>ex.Message:</td><td>" + ex.Message
                   + "</td></tr><tr><td>ex.StackTrace:</td><td>" + ex.StackTrace
                   + "</td></tr></tbody></table>";
            return body;
        }

        private void RecordException(Exception ex, string type)
        {
            CSK_Store_ExceptionCollect exc = new CSK_Store_ExceptionCollect();
            exc.ExceptionAppName = "PamAccountGenerator";
            exc.ExceptionInfo = ex.Message + "\r\n\r\n" + ex.StackTrace;
            exc.ExceptionType = type;
            exc.errorPagePath = Request.Url.AbsoluteUri;
            exc.Save();
        }

        private void SendErrorEmail(Exception ex)
        {
            try
            {
                RecordException(ex, "Retailer SignUp");

                var AdminEmail = ConfigurationManager.AppSettings["AdminEmail"];
                if (string.IsNullOrEmpty(AdminEmail))
                {
                    return;
                }

                string emailBody = GetErrorEmailBody(ex);
                MailMessage emailMessage = new MailMessage();
                emailMessage.From = new MailAddress("info@priceme.co.nz", "PamAccountGenerator");
                var toEmails = AdminEmail.Split(';');
                foreach (var email in toEmails)
                {
                    emailMessage.To.Add(new MailAddress(email));
                }

                emailMessage.ReplyToList.Add(new MailAddress("info@priceme.co.nz"));
                emailMessage.IsBodyHtml = true;
                emailMessage.Subject = "Error from PamAccountGenerator";
                emailMessage.Body = emailBody;

                SmtpClient smtpClient = new SmtpClient();
                smtpClient.Send(emailMessage);

            }
            catch (Exception e)
            {
                CSK_Store_ExceptionCollect exc = new CSK_Store_ExceptionCollect();
                exc.ExceptionAppName = "PamAccountGenerator - SendEmail";
                exc.ExceptionInfo = e.Message + "\r\n\r\n" + e.StackTrace;
                exc.ExceptionType = "Retailer SignUp - SendEmail";
                exc.errorPagePath = Request.Url.AbsoluteUri;
                exc.Save();
            }
        }

        private void RecordException(string info)
        {
            CSK_Store_ExceptionCollect exc = new CSK_Store_ExceptionCollect();
            exc.ExceptionAppName = "PamAccountGenerator";
            exc.ExceptionInfo = info;
            exc.ExceptionType = "Create User";
            exc.errorPagePath = Request.Url.AbsoluteUri;
            exc.Save();
        }

        #endregion

        #region update config

        /// <summary>
        /// 修改"UpdateConfig"里面的所有文件夹里面的web.config 里面的 'update' key,
        /// 如果update这个key 是0，就改成1，反之就改成0。
        /// 目的是为了重启pam加载新的retailer
        /// </summary>
        private void UpdateConfig(string folder)
        {
            var root = folder;
            if (string.IsNullOrEmpty(root))
                root = ConfigurationManager.AppSettings["UpdateConfig"];
            if (string.IsNullOrEmpty(root)) return;

            if (Directory.Exists(root))
            {
                var direcs = Directory.GetDirectories(root);
                foreach (var direc in direcs)
                {
                    UpdateConfig(direc);
                }

                UpdateConfig(Directory.GetFiles(root));
            }
        }

        /// <summary>
        /// 修改 web.config 里面的 'update' key,
        /// 如果update这个key 是0，就改成1，反之就改成0。
        /// </summary>
        private void UpdateConfig(string[] files)
        {
            foreach (var file in files)
            {
                if (file.ToLower().Contains("web.config"))
                {
                    XmlDocument doc = new XmlDocument();
                    doc.Load(file);
                    XmlNodeList nodes = doc.SelectNodes(@"//appSettings//add");
                    foreach (XmlNode node in nodes)
                    {
                        var key = node.Attributes["key"];
                        if (key != null && key.Value != null)
                        {
                            if (key.Value.ToLower() == "update")
                            {
                                XmlElement xmlEle = (XmlElement)node;
                                string v = xmlEle.GetAttribute("value");
                                if (v == "0")
                                    v = "1";
                                else v = "0";
                                xmlEle.SetAttribute("value", v);

                                doc.Save(file);
                            }
                        }
                    }
                }
            }
        }

        #endregion
    }
}