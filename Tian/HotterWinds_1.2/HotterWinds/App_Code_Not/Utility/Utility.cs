using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using PriceMeCommon.Data;
using PriceMeDBA;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Data;
using System.IO;
using System.Text.RegularExpressions;
using PriceMeCache;
using System.Threading;
using PriceMeCommon.Extend;
using System.Security.Cryptography;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using System.Configuration;
using PriceMeCommon;

namespace PriceMe
{
    public class Utility
    {
        public static readonly string SSN_PRODUCT_REVIEW = "SSN_CAPTCHA_PRODUCT_REVIEW";
        public static IFormatProvider CurrentCulture = new System.Globalization.CultureInfo(Resources.Resource.TextString_Culture);
        private static string accessKeyID = ConfigurationManager.AppSettings["AWSAccessKey"];
        private static string secretAccessKeyID = ConfigurationManager.AppSettings["AWSSecretKey"];

        public static void PageNotFound()
        {
            HttpContext.Current.Response.Status = "404 Not Found";
            HttpContext.Current.Response.Redirect("/404.aspx", true);
        }

        public static string MembershipCreateStatus(string status)
        {
            Regex reg = new Regex("[A-Z][a-z]*");
            string returnStr = "";
            MatchCollection matches = reg.Matches(status);
            foreach (Match match in matches)
            {
                returnStr += match.Groups[0].ToString() + " ";
            }
            return returnStr.Trim();
        }

        public static string GetClientIPAddress(HttpContext context)
        {
            string result = String.Empty;
            if (context == null)
            {
                return "000.000.000.000";
            }

            result = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(result))
            {
                result = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }

            if (string.IsNullOrEmpty(result))
            {
                result = HttpContext.Current.Request.UserHostAddress;
            }

            return result;
        }

        public static string UrlParaEncode(string str)
        {
            return System.Web.HttpUtility.UrlEncode(str);
        }

        public static string UrlEncode(string url)
        {
            return url.Replace("'", System.Web.HttpUtility.UrlEncode(","))
                    .Replace("\"", System.Web.HttpUtility.UrlEncode("\""))
                    .Replace("<", System.Web.HttpUtility.UrlEncode("<"))
                    .Replace("(", System.Web.HttpUtility.UrlEncode("("))
                    .Replace(")", System.Web.HttpUtility.UrlEncode(")"))
                    .Replace(">", System.Web.HttpUtility.UrlEncode(">"));
        }

        //public static string GetStarImage(double score)
        //{
        //    if (score > 5.0d)
        //    {
        //        score = 5.0d;
        //    }
        //    string ScoreName = "";
        //    int stars = 0;
        //    score -= 1d;
        //    while (score > 0d)
        //    {
        //        stars++;
        //        score -= 1d;
        //    }
        //    score += 1d;

        //    bool isHaft = false;
        //    if (score >= 0.3d && score < 0.8d)
        //    {
        //        isHaft = true;
        //    }
        //    else if (score >= 0.8d)
        //    {
        //        stars++;
        //    }

        //    switch (stars)
        //    {
        //        case 0:
        //            if (isHaft)
        //            {
        //                ScoreName = "HaftScore";
        //            }
        //            else
        //            {
        //                ScoreName = "NoScore";
        //            }
        //            break;
        //        case 1:
        //            if (isHaft)
        //            {
        //                ScoreName = "OneAndHaftScore";
        //            }
        //            else
        //            {
        //                ScoreName = "OneScore";
        //            }
        //            break;
        //        case 2:
        //            if (isHaft)
        //            {
        //                ScoreName = "TwoAndHaftScore";
        //            }
        //            else
        //            {
        //                ScoreName = "TwoScore";
        //            }
        //            break;
        //        case 3:
        //            if (isHaft)
        //            {
        //                ScoreName = "ThreeAndHaftScore";
        //            }
        //            else
        //            {
        //                ScoreName = "ThreeScore";
        //            }
        //            break;
        //        case 4:
        //            if (isHaft)
        //            {
        //                ScoreName = "FourAndHaftScore";
        //            }
        //            else
        //            {
        //                ScoreName = "FourScore";
        //            }
        //            break;
        //        case 5:
        //            ScoreName = "FiveScore";
        //            break;
        //    }

        //    return ScoreName;
        //}

        public static string GetParameter(string sParam)
        {
            if (System.Web.HttpContext.Current.Request.QueryString[sParam] != null)
            {
                return System.Web.HttpContext.Current.Request.QueryString[sParam];
            }
            if (System.Web.HttpContext.Current.Request.Params[sParam] != null)
            {
                return System.Web.HttpContext.Current.Request.Params[sParam];
            }
            else
            {
                return "";
            }
        }

        public static int GetIntParameter(string sParam)
        {
            int iOut = 0;

            string sOut = GetParameter(sParam);
            if (!String.IsNullOrEmpty(sOut))
            {
                if (sOut.Contains(','))
                {
                    int.TryParse(sOut.Split(',')[0], out iOut);
                }
                else
                {
                    int.TryParse(sOut, out iOut);
                }
            }

            return iOut;
        }

        public static BreadCrumbInfo GetBreadCrumbInfo(string currentPageName)
        {
            return GetBreadCrumbInfo(null, currentPageName);
        }

        public static BreadCrumbInfo GetBreadCrumbInfo(List<LinkInfo> linkInfoList, string currentPageName)
        {
            BreadCrumbInfo breadCrumbInfo = new BreadCrumbInfo();

            List<LinkInfo> _linkInfoList = new List<LinkInfo>();

            LinkInfo linkInfo = new LinkInfo();
            linkInfo.LinkText = Resources.Resource.Global_HomePageName;
            linkInfo.LinkURL = Resources.Resource.Global_HomePageUrl;
            _linkInfoList.Add(linkInfo);

            if (linkInfoList != null && linkInfoList.Count > 0)
            {
                _linkInfoList.AddRange(linkInfoList);
            }
            if (linkInfoList != null && linkInfoList.Count > 1)
            {
                if (_linkInfoList[0].LinkText == _linkInfoList[1].LinkText)
                    _linkInfoList.Remove(_linkInfoList[0]);
            }

            breadCrumbInfo.linkInfoList = _linkInfoList;
            breadCrumbInfo.CurrentPageName = currentPageName;

            return breadCrumbInfo;
        }

        public static BreadCrumbInfo GetCatalogBreadCrumbInfo(PriceMeCache.CategoryCache category, ManufacturerInfo manufacturer, bool endWithLink, string currentPageName)
        {
            BreadCrumbInfo breadCrumbInfo = new BreadCrumbInfo();

            if (category == null || category.CategoryID == 0) return breadCrumbInfo;

            List<LinkInfo> linkInfoList = new List<LinkInfo>();

            LinkInfo linkInfo = new LinkInfo();
            linkInfo.LinkText = Resources.Resource.Global_HomePageName;
            linkInfo.LinkURL = Resources.Resource.Global_HomePageUrl;
            linkInfoList.Add(linkInfo);

            if (category != null && category.CategoryID != 0)
            {
                List<PriceMeCache.CategoryCache> categoryList = PriceMeCommon.BusinessLogic.CategoryController.GetBreadCrumbCategoryList(category, WebConfig.CountryId);

                for (int i = categoryList.Count - 1; i > 0; i--)
                {
                    linkInfo = new LinkInfo();
                    linkInfo.LinkText = categoryList[i].CategoryName;
                    linkInfo.LinkURL = UrlController.GetCatalogUrl(categoryList[i].CategoryID);
                    if (i == categoryList.Count - 1)
                    {
                        linkInfo.Value = categoryList[i].CategoryID.ToString();
                    }
                    linkInfoList.Add(linkInfo);
                }
            }

            if (manufacturer == null || manufacturer.ManufacturerID == 0 || manufacturer.ManufacturerID == -1 || PriceMeCommon.BusinessLogic.CategoryController.IsHiddenBrandsCategoryID(category.CategoryID, WebConfig.CountryId))
            {
                if (endWithLink)
                {
                    linkInfo = new LinkInfo();
                    linkInfo.LinkText = category.CategoryName;
                    linkInfo.LinkURL = UrlController.GetCatalogUrl(category.CategoryID);
                    linkInfoList.Add(linkInfo);
                }
                else
                {
                    if (category == null || category.CategoryID == 0)
                    {
                        breadCrumbInfo.CurrentPageName = currentPageName;
                    }
                    else
                    {
                        breadCrumbInfo.CurrentPageName = category.CategoryName;
                    }
                }
            }
            else
            {
                linkInfo = new LinkInfo();
                linkInfo.LinkText = category.CategoryName;
                linkInfo.LinkURL = UrlController.GetCatalogUrl(category.CategoryID);
                linkInfoList.Add(linkInfo);

                if (endWithLink)
                {
                    linkInfo = new LinkInfo();
                    linkInfo.LinkText = manufacturer.ManufacturerName;
                    Dictionary<string, string> ps = new Dictionary<string, string>();
                    ps.Add("c", category.CategoryID.ToString());
                    ps.Add("m", manufacturer.ManufacturerID.ToString());
                    linkInfo.LinkURL = UrlController.GetRewriterUrl(PageName.Catalog, ps);
                    linkInfoList.Add(linkInfo);
                }
                else if (string.IsNullOrEmpty(currentPageName))
                {
                    breadCrumbInfo.CurrentPageName = manufacturer.ManufacturerName;
                }
            }

            breadCrumbInfo.linkInfoList = linkInfoList;

            return breadCrumbInfo;
        }


        public static bool RecoverPasswordEmail(string firstname, string youremail, string username, string password)
        {
            try
            {
                string stringSubject = "PriceMe Password Reminder!";
                string stringBody = "<br/>Dear " + firstname + ",<br/><br/>Please find your user name and password below.<br><br/>User name: " + username + "<br/>Password: " + password + "<br/><br/>Thanks for using PriceMe. <br/><br/>The PriceMe Team<br/><a href='" + Resources.Resource.Global_HomePageUrl + "'>" + Resources.Resource.Global_HomePageUrl + "/</a>";

                AmazonEmail(stringBody, stringSubject, youremail, System.Configuration.ConfigurationManager.AppSettings["InfoEmail"]);

                return true;
            }
            catch (Exception ex)
            {
                PriceMeCommon.BusinessLogic.LogController.WriteException("Set RecoverPasswordEmail Email error : " + ex.Message + "\t" + ex.StackTrace);
                return false;
            }
        }

        public static bool RegisterEmail(string firstname, string youremail, string username, string password, string body)
        {
            try
            {
                string stringSubject = "Welcome to PriceMe and thanks for signing up!";

                AmazonEmail(body, stringSubject, youremail, PriceMeCommon.ConfigAppString.EmailAddress);
                return true;
            }
            catch (Exception ex)
            {
                PriceMeCommon.BusinessLogic.LogController.WriteException("Set Register Email error : " + ex.Message + "\t" + ex.StackTrace);
                PriceMeCommon.BusinessLogic.LogController.WriteException("Email body : " + body);
                return false;
            }
        }

        public static void UserReviewEmail(CSK_Store_ProductReview review, string userName, string name, string categoryName, string productName, string productUrl, bool isOther, string email)
        {
            string em = PriceMeCommon.ConfigAppString.EmailAddress;

            string stringBody = "<br/>Dear " + userName + ",<br/><br/>Thanks for your " + categoryName + " product review.<br/>" +
                                        "It has been approved and is now displayed online:<br/>" + Resources.Resource.Global_HomePageUrl + productUrl +
                                        "<br/><br/>You can now update your member profile online:<br/>" + Resources.Resource.Global_HomePageUrl + "/Community/Review-MyPriceMe.aspx<br/><br/>First login and then click 'My Account'.<br/><br/>Thanks again for your review.<br/><br/>" +
                                        "Best regards,<br/>The PriceMe Team";
            string stringSource = "Your PriceMe product review for: " + productName;

            try
            {
                AmazonEmail(stringBody, stringSource, email, em);
            }
            catch (Exception ex)
            {
                PriceMeCommon.BusinessLogic.LogController.WriteException("toemail:" + email + "   email:" + em + "  " + ex.Message + ex.StackTrace);
            }
            //notify admin new product review
            NotifyAdminNewProductReviewEmail(review, productName, userName);
        }

        public static void NotifyAdminNewRetailerReviewEmail(string retailer, string author, string reviewContent)
        {
            try
            {
                string emTo = System.Configuration.ConfigurationManager.AppSettings["RetaileReviewEmail"];
                string emFrom = System.Configuration.ConfigurationManager.AppSettings["InfoEmail"];

                string stringSubject = "New Retailer review for " + retailer;
                string stringBody = string.Format("Hello!<br/><br/>There's a new retailer review to approve for PriceMe {0}." +
                    "<br/>Please check.<br/>Here the review content from member {1}:<br/>{2}<br/><br/>Thanks,<br/>PriceMe System",
                    Resources.Resource.Country, author, reviewContent);

                AmazonEmail(stringBody, stringSubject, emTo, emFrom);
            }
            catch (Exception ex)
            {
                LogWriter.FileLogWriter.WriteLine("NewRetailerReviewEmail", ex.Message);
            }
        }

        /// <summary>
        /// notify admin new product review
        /// </summary>
        /// <param name="review"></param>
        /// <param name="product"></param>
        /// <param name="author"></param>
        public static void NotifyAdminNewProductReviewEmail(CSK_Store_ProductReview review, string product, string author)
        {
            try
            {
                string emTo = System.Configuration.ConfigurationManager.AppSettings["ProductReviewEmail"];
                string emFrom = System.Configuration.ConfigurationManager.AppSettings["InfoEmail"];

                string stringSubject = "New proudct review for: " + product;
                string stringBody = string.Format("Hello!<br/><br/>There's a new product review to approve for PriceMe {0}." +
                    "<br/>Please check.<br/>Here the review content from user {1}:<br/>{2}<br/>{3}<br/><br/>Thanks,<br/>PriceMe System",
                    Resources.Resource.Country, author, review.Title, review.Body);

                AmazonEmail(stringBody, stringSubject, emTo, emFrom);
            }
            catch (Exception ex)
            {
                LogWriter.FileLogWriter.WriteLine("NewProductReviewEmail", ex.Message);
            }
        }

        private static string GetUserEmail(int id)
        {
            string email = string.Empty;
            SubSonic.Schema.StoredProcedure sp = PriceMeDBStatic.PriceMeDB.aspnet_Users_GetEmail();
            sp.Command.AddParameter("@reviewId", id, DbType.Int32);
            IDataReader dr = sp.ExecuteReader();
            while (dr.Read())
            {
                email = dr["Email"].ToString();
            }
            dr.Close();

            return email;
        }

        private static string GetOtherUserEmail(int id)
        {
            string email = string.Empty;
            SubSonic.Schema.StoredProcedure sp = PriceMeDBStatic.PriceMeDB.aspnet_OtherUsers_GetEmail();
            sp.Command.AddParameter("@reviewId", id, DbType.Int32);
            IDataReader dr = sp.ExecuteReader();
            while (dr.Read())
            {
                email = dr["Email"].ToString();
            }
            dr.Close();

            return email;
        }

        public static bool ReportAbuseCommentEmail(string youremail, string subject, string body)
        {
            try
            {
                AmazonEmail(body, subject, youremail, System.Configuration.ConfigurationManager.AppSettings["InfoEmail"]);
                return true;
            }
            catch (Exception ex)
            {
                LogWriter.FileLogWriter.WriteLine("ReportAbuseCommentEmail", ex.Message);
                return false;
            }
        }

        public static bool NewsLetterEmail(string youremail, string subject, string body)
        {
            try
            {
                AmazonEmail(body, subject, youremail, System.Configuration.ConfigurationManager.AppSettings["InfoEmail"]);
                return true;
            }
            catch (Exception ex)
            {
                LogWriter.FileLogWriter.WriteLine("NewsLetterEmail", ex.Message);
                return false;
            }
        }

        public static bool FriendEmail(string username, string youremail, string friendemail, string productname, int productID)
        {
            string link = UrlController.GetProductUrl(productID, productname);//Utility.GetRewriterUrl("product", productID.ToString(), "");
            try
            {
                username = username.Substring(0, 1).ToUpper() + username.Substring(1, username.Length - 1);

                string subject = username + " recommends: " + productname;
                string body = "<br/>Please visit the following product page recommended to you by your friend " + username + " (" + youremail +
                                              "): <br/><br/>" + productname + "<br/><br/><a href='" + link + "'>" + link + "</a><br/><br/><br/> Regards <br/><a href='" + Resources.Resource.Global_HomePageUrl + "'>" + Resources.Resource.Global_HomePageUrl + "/</a><br/>Smarter Shopping";

                AmazonEmail(body, subject, friendemail, System.Configuration.ConfigurationManager.AppSettings["InfoEmail"]);
                return true;
            }
            catch (Exception ex)
            {
                LogWriter.FileLogWriter.WriteLine("FriendEmail", ex.Message);
                return false;
            }
        }


        public static bool ReportProductEmail(string emailTo, string subject, string body)
        {
            try
            {
                AmazonEmail(body, subject, emailTo, System.Configuration.ConfigurationManager.AppSettings["InfoEmail"]);
                return true;
            }
            catch (Exception ex)
            {
                LogWriter.PriceMeDataBaseExceptionWriter.WriteException(ex.Message + "--- " + emailTo + "---" + ex.StackTrace, "SendEmail", "RetailerReport", "PriceMe", 1);
                return false;
            }
        }

        public static void RetailerReviewEmail(string stringBody, string OverallStoreRating, string retaileremail)
        {
            string stringSubject = "New " + OverallStoreRating + " Star Review Notification - PriceMe";

            AmazonEmail(stringBody, stringSubject, retaileremail, PriceMeCommon.ConfigAppString.EmailAddress);
        }

        public static bool AmazonEmail(string stringBody, string stringSubject, string stringEmail, string stringSource)
        {
            AmazonSimpleEmailServiceClient ses = new AmazonSimpleEmailServiceClient(accessKeyID, secretAccessKeyID);

            string from = "PriceMe Admin <" + stringSource + ">";
            SendEmailRequest seReq = new SendEmailRequest();
            seReq.Source = from;

            string[] emails = stringEmail.Split(',');
            List<string> list = new List<string>();
            for (int i = 0; i < emails.Length; i++)
            {
                list.Add(emails[i]);
            }

            Destination det = new Destination();
            det.ToAddresses = list;

            seReq.Destination = det;

            Message mes = new Message();
            Content con = new Content();
            con.Data = stringSubject;
            mes.Subject = con;

            Body body = new Body();
            Content conHtml = new Content();
            conHtml.Data = stringBody;
            body.Text = conHtml;
            body.Html = conHtml;
            mes.Body = body;

            seReq.Message = mes;

            list = new List<string>() { from };

            seReq.ReplyToAddresses = list;

            ses.SendEmail(seReq);
            return true;
        }

        public static bool AmazonEmailOutside(string stringBody, string stringSubject, string stringEmail, string stringSource, string yourname, string youremail)
        {
            AmazonSimpleEmailServiceClient ses = new AmazonSimpleEmailServiceClient(accessKeyID, secretAccessKeyID);

            string from = yourname + " <" + stringSource + ">";
            SendEmailRequest seReq = new SendEmailRequest();
            seReq.Source = from;

            string[] emails = stringEmail.Split(',');
            List<string> list = new List<string>();
            for (int i = 0; i < emails.Length; i++)
            {
                list.Add(emails[i]);
            }

            Destination det = new Destination();
            det.ToAddresses = list;

            seReq.Destination = det;

            Message mes = new Message();
            Content con = new Content();
            con.Data = stringSubject;
            mes.Subject = con;

            Body body = new Body();
            Content conHtml = new Content();
            conHtml.Data = stringBody;
            body.Text = conHtml;
            body.Html = conHtml;
            mes.Body = body;

            seReq.Message = mes;

            list = new List<string>() { yourname + " <" + youremail + ">" };

            seReq.ReplyToAddresses = list;

            ses.SendEmail(seReq);
            return true;
        }

        public static bool IsAdmin()
        {
            return HttpContext.Current.User.IsInRole("administrator") ? true : false;
        }

        public static string GetUserName()
        {
            string sUserName = "";
            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                sUserName = HttpContext.Current.User.Identity.Name;
            }
            else
            {
                if (HttpContext.Current.Request.Cookies["userreview"] != null)
                {
                    if (HttpContext.Current.Request.Cookies["userreview"].Value.Length <= 18)
                        sUserName = HttpContext.Current.Request.Cookies["userreview"].Value;
                }
            }
            HttpContext.Current.Response.Cookies["userreview"].Value = sUserName;
            HttpContext.Current.Response.Cookies["userreview"].Expires = DateTime.Today.AddMinutes(30);
            return sUserName;
        }

        public static string GetOtherUserName(out string userName, out bool isOther)
        {
            //string sUserName = "";
            isOther = true;
            userName = string.Empty;

            if (!string.IsNullOrEmpty(HttpContext.Current.User.Identity.Name))
            {
                //sUserName = HttpContext.Current.User.Identity.Name;
                //userName = sUserName;

                //aspnet_User user = aspnet_User.SingleOrDefault(u => u.UserName == sUserName);
                //aspnet_MembershipInfo info = aspnet_MembershipInfo.SingleOrDefault(m => m.UserID == user.UserId.ToString());
                //if ((info.MembershipType ?? 0) != 0)
                //{
                //    sUserName = (info.FirstName + " " + info.LastName).Trim();
                //    isOther = true;
                //}

                var user = Utility.GetUserInfoFromCookie();
                userName = user.name;
            }

            return userName;
        }

        public static aspnet_MembershipInfo GetUserMembershipInfo()
        {
            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                string sUserName = HttpContext.Current.User.Identity.Name;

                return GetUserMembershipInfo(sUserName);
            }

            return null;
        }

        public static aspnet_MembershipInfo GetUserMembershipInfo(string userName)
        {
            aspnet_User user = aspnet_User.SingleOrDefault(u => u.UserName == userName);
            aspnet_MembershipInfo info = aspnet_MembershipInfo.SingleOrDefault(m => m.UserID == user.UserId.ToString());
            return info;
        }

        public static string GetStarImage(decimal score)
        {
            return GetStarImage((double)score);
        }

        public static string GetStarImage(double score)
        {
            if (score > 5.0d)
            {
                score = 5.0d;
            }
            string ScoreName = "";

            int stars = (int)score;
            score = score - stars;

            bool isHaft = false;
            if (score >= 0.25d && score < 0.75d)//因为前端显示为ToString("0.0")，为避免都显示0.3，星星数量却不同，判断由0.3,0.8改为0.25，0.75
            {
                isHaft = true;
            }
            else if (score >= 0.75d)
            {
                stars++;
            }

            switch (stars)
            {
                case 0:
                    ScoreName = isHaft ? "HaftScore" : "NoScore";
                    break;
                case 1:
                    ScoreName = isHaft ? "OneAndHaftScore" : "OneScore";
                    break;
                case 2:
                    ScoreName = isHaft ? "TwoAndHaftScore" : "TwoScore";
                    break;
                case 3:
                    ScoreName = isHaft ? "ThreeAndHaftScore" : "ThreeScore";
                    break;
                case 4:
                    ScoreName = isHaft ? "FourAndHaftScore" : "FourScore";
                    break;
                case 5:
                    ScoreName = "FiveScore";
                    break;
            }
            return ScoreName;
        }

        public static string GetStarImageUrl(Page page, double score)
        {
            if (score > 5.0d)
            {
                score = 5.0d;
            }
            string url = Resources.Resource.ImageWebsite + "/images/rating/";
            string starFileName = "";

            int stars = (int)score;
            score = score - stars;

            starFileName = stars.ToString();
            if (score >= 0.3d && score < 0.8d)
                starFileName += "h";
            else if (score >= 0.8d)
                starFileName = (stars + 1) + "";

            starFileName = "star_" + starFileName + ".gif";
            url += starFileName;
            return page.ResolveClientUrl(url);
        }

        public static string GetLStarImageUrl(Page page, double score)
        {
            if (score > 5.0d)
            {
                score = 5.0d;
            }
            string url = Resources.Resource.ImageWebsite + "/images/rating/";
            string starFileName = "";

            int stars = (int)score;
            score = score - stars;

            starFileName = stars.ToString();
            if (score >= 0.3d && score < 0.8d)
                starFileName += "h";
            else if (score >= 0.8d)
                starFileName = (stars + 1) + "";

            starFileName = "l_star_" + starFileName + ".gif";
            url += starFileName;
            return page.ResolveClientUrl(url);
        }

        public static string GetRatingImageFileName(double score)
        {
            if (score > 5.0d)
            {
                score = 5.0d;
            }
            string starFileName = "";
            int stars = (int)score;
            score = score - stars;

            starFileName = stars.ToString();
            if (score >= 0.3d && score < 0.8d)
            {
                starFileName += "h";
            }
            else if (score >= 0.8d)
            {
                starFileName = (stars + 1) + "";
            }
            starFileName = "star_" + starFileName + ".gif";
            return starFileName;
        }

        public static string GetShareWishList(int sid, string pwd)
        {
            string param = string.Format("{0}", sid);
            byte[] bytes = Encoding.UTF8.GetBytes(param);

            MemoryStream msm = new MemoryStream(bytes);
            Stream st = MyCrypto.MyRijndael.EncryptStream(msm, string.Format(System.Configuration.ConfigurationManager.AppSettings.Get("ShareWishListSecurityKey"), pwd));
            bytes = new byte[st.Length];
            st.Read(bytes, 0, bytes.Length);
            st.Close();
            msm.Close();

            string key = Convert.ToBase64String(bytes).Replace('+', '_');

            return string.Format("/MyWishList/s-{0}.aspx", key);
        }

        public static int DecodeShareWishList(string param, string pwd)
        {
            int sid = 0;
            try
            {
                byte[] bytes = Convert.FromBase64String(param.Replace('_', '+'));
                MemoryStream msm = new MemoryStream(bytes);

                Stream sm = MyCrypto.MyRijndael.DecryptStream(msm, string.Format(System.Configuration.ConfigurationManager.AppSettings.Get("ShareWishListSecurityKey"), pwd));

                StreamReader sr = new StreamReader(sm, Encoding.UTF8);
                param = sr.ReadToEnd();
                int.TryParse(param, out sid);

                sr.Close();
                sm.Close();
                msm.Close();
            }
            catch { }

            return sid;
        }

        public static string GetProductViewTracking(int pid, List<string> rids)
        {
            long dt = DateTime.Now.Ticks;
            // {Now.Ticks}:{rid1}:{rid2}:{...}:{ridN}
            string param = string.Format("{0}:{1}", dt, string.Join(":", rids.ToArray()));
            byte[] bytes = Encoding.UTF8.GetBytes(param);

            //MemoryStream msm = new MemoryStream(bytes);
            //Stream st = MyCrypto.MyRijndael.EncryptStream(msm, string.Format(ConfigurationManager.AppSettings.Get("ViewTrackingSecurityKey"), pid));
            //bytes = new byte[st.Length];
            //st.Read(bytes, 0, bytes.Length);
            //st.Close();
            //msm.Close();
            //return string.Format("/ViewTracking.aspx?pid={0}&param={1}", pid, Convert.ToBase64String(bytes).Replace('+', '_'));

            param = Convert.ToBase64String(bytes);
            return string.Format("/ViewTracking.aspx?pid={0}&param={1}", pid, param.Replace('+', '_'));
        }

        public static void DecodeProductViewTracking(int pid, string param, out long ticks, out List<string> rids)
        {
            ticks = 0L;
            rids = new List<string>();
            try
            {
                byte[] bytes = Convert.FromBase64String(param.Replace('_', '+'));
                //MemoryStream msm = new MemoryStream(bytes);

                //Stream sm = MyCrypto.MyRijndael.DecryptStream(msm, string.Format(ConfigurationManager.AppSettings.Get("ViewTrackingSecurityKey"), pid));

                //StreamReader sr = new StreamReader(sm, Encoding.UTF8);
                //param = sr.ReadToEnd();
                // {Now.Ticks}:{rid1}:{rid2}:{...}:{ridN}
                param = Encoding.UTF8.GetString(bytes);
                string[] tmp = param.Split(new char[] { ':' });
                long.TryParse(tmp[0], out ticks);
                for (int i = 1; i < tmp.Length; i++)
                    rids.Add(tmp[i]);

                //sr.Close();
                //sm.Close();
                //msm.Close();
            }
            catch
            {
                //
            }
        }

        public static string GetSiteRoot()
        {
            //return "/";

            string Port = System.Web.HttpContext.Current.Request.ServerVariables["SERVER_PORT"];
            if (Port == null || Port == "80" || Port == "443")
                Port = "";
            else
                Port = ":" + Port;

            string Protocol = System.Web.HttpContext.Current.Request.ServerVariables["SERVER_PORT_SECURE"];
            if (Protocol == null || Protocol == "0")
                Protocol = "http://";
            else
                Protocol = "https://";

            string appPath = System.Web.HttpContext.Current.Request.ApplicationPath;
            if (appPath == "/")
                appPath = "";

            string sOut = Protocol + System.Web.HttpContext.Current.Request.ServerVariables["SERVER_NAME"] + Port + appPath;
            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(@"^(?<SiteRoot>https?://(w{3}\.)?priceme\.((com)|(net\.au)|(info)|(co\.nz)))");
            System.Text.RegularExpressions.Match match = regex.Match(sOut);
            if (match.Success)
            {
                sOut = sOut.Replace(match.Groups["SiteRoot"].Value, Resources.Resource.Global_HomePageUrl);
            }
            //HomePageUrl_sOut  HomePageUrl_sOutR
            if (sOut.Contains(Resources.Resource.HomePageUrl_sOut))
                sOut = sOut.Replace(Resources.Resource.HomePageUrl_sOut, Resources.Resource.HomePageUrl_sOutR);

            //if (sOut.Contains(".nz.nz"))
            //    sOut = sOut.Replace(".nz.nz", ".nz");

            //if (sOut.Contains(".au.au"))
            //    sOut = sOut.Replace(".au.au", ".au");

            //if (sOut.Contains(".ph.ph"))
            //    sOut = sOut.Replace(".ph.ph", ".ph");

            //if (sOut.Contains(".hk.hk"))
            //    sOut = sOut.Replace(".hk.hk", ".hk");

            //if(sOut.Contains(".sg.sg"))
            //    sOut = sOut.Replace(".sg.sg", ".sg");
            //if (sOut.Contains(".my.my"))
            //    sOut = sOut.Replace(".my.my", ".my");
            //if (sOut.Contains(".id.id"))
            //    sOut = sOut.Replace(".id.id", ".id");
            return sOut;
        }

        public static string GetPriceRange(string bestPrice, string maxPrice)
        {
            string returnStr = "";
            if (bestPrice == maxPrice)
            {
                returnStr = bestPrice;
            }
            else
            {
                returnStr = bestPrice + " - " + maxPrice;
            }
            return returnStr;
        }

        public static string GetSpecialSizeImage(string bigImagePath, string ImageSize)
        {
            if (bigImagePath.Trim().Length > 0)
            {
                if (bigImagePath.LastIndexOf(".") >= 0)
                {
                    string s1 = bigImagePath.Substring(0, bigImagePath.LastIndexOf("."));
                    string s2 = bigImagePath.Substring(bigImagePath.LastIndexOf("."));
                    s2 = "_" + ImageSize + s2;
                    return s1 + s2;
                }
                else
                    return "";
            }
            else
                return "";
        }

        #region StripHTML

        public static string StripHTML(string htmlString)
        {
            return StripHTML(htmlString, "", true);
        }

        public static string StripHTML(string htmlString, string htmlPlaceHolder)
        {
            return StripHTML(htmlString, htmlPlaceHolder, true);
        }

        public static string StripHTML(string htmlString, string htmlPlaceHolder, bool stripExcessSpaces)
        {
            string pattern = @"<(.|\n)*?>";
            string sOut = System.Text.RegularExpressions.Regex.Replace(htmlString, pattern, htmlPlaceHolder);
            sOut = sOut.Replace("&nbsp;", "");
            sOut = sOut.Replace("&amp;", "&");

            if (stripExcessSpaces)
            {
                // If there is excess whitespace, this will remove
                // like "THE      WORD".
                char[] delim = { ' ' };
                string[] lines = sOut.Split(delim, StringSplitOptions.RemoveEmptyEntries);

                sOut = "";
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                foreach (string s in lines)
                {
                    sb.Append(s);
                    sb.Append(" ");
                }
                return sb.ToString().Trim();
            }
            else
            {
                return sOut;
            }

        }

        #endregion

        public static bool DecodeSolidShopLocationUpdParam(string param, int retailerId, out int year, out int month, out string retailerName)
        {
            string pwd = string.Format(System.Configuration.ConfigurationManager.AppSettings.Get("ClickStatisticsSecurityKey"), retailerId);
            year = 0;
            month = 0;
            retailerName = "";

            try
            {
                byte[] bytes = Convert.FromBase64String(param.Replace('_', '+').Replace('-', '/'));
                MemoryStream msm = new MemoryStream(bytes);

                Stream sm = MyCrypto.MyRijndael.DecryptStream(msm, pwd);

                StreamReader sr = new StreamReader(sm, Encoding.UTF8);
                param = sr.ReadToEnd();
                //param = {year}:{month}:{retailerId}:{retailerName};
                string[] tmp = param.Split(new char[] { ':' });
                int.TryParse(tmp[0], out year);
                int.TryParse(tmp[1], out month);
                retailerName = tmp[2];
                sr.Close();
                sm.Close();
                msm.Close();
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }

        public static string ConvertInt(double src)
        {
            string result = "";
            if (Math.Abs(src) < 1000)
                result = src.ToString();
            else
            {
                result = src.ToString().Substring(src.ToString().Length - 3, 3);
                long quotient = Convert.ToInt64(src) / 1000;
                if (Math.Abs(quotient) > 0)
                    result = ConvertInt(quotient) + "," + result;
            }

            return result;
        }

        //public static string PriceIntCultureString(double price)
        //{
        //    return PriceMeCommon.PriceMeStatic.PriceIntCultureString(price, CurrentCulture, Resources.Resource.TextString_PriceSymbol);
        //}

        //public static string PriceCultureString(double price)
        //{
        //    return PriceMeCommon.PriceMeStatic.PriceCultureString(price, CurrentCulture, Resources.Resource.TextString_PriceSymbol);
        //}

        //public static string PriceCultureStringWithoutPriceSymbol(double price)
        //{
        //    return PriceMeCommon.PriceMeStatic.PriceCultureString(price, CurrentCulture, "");
        //}

        public static string GetEmailContent(string userName, string pwd, string emailAddress, string checkURL)
        {
            string emailContent = "<br/>Dear " + userName + ",<br/><br/>Welcome to PriceMe and thanks for signing up!"
                                 + "<br/><br/>" + "Please click the link below to confirm your email address and complete your PriceMe registration.<br /><br /><a href=\'" + checkURL + "'>" + checkURL + "</a>"
                                 + "<br/><br/>Your personal <user name> profile page is located here:<br />"
                                 + "<a href='" + Resources.Resource.Global_HomePageUrl + "/Community/MyPriceMe.aspx'>"
                                 + Resources.Resource.Global_HomePageUrl + "/Community/MyPriceMe.aspx</a><br/>"
                                 + "<br/><br/>Thanks again for choosing PriceMe and get ready for 'Smarter Shopping'. <br/><br/>The PriceMe Team <br/><a href='"
                                 + Resources.Resource.Global_HomePageUrl + "'>" + Resources.Resource.Global_HomePageUrl + "</a>";

            return emailContent;
        }



        public static bool SendCheckEmail(string youremail, string checkURL)
        {
            try
            {
                string body = "Please click the link below to confirm your email address and complete your PriceMe registration.<br /><br /><a href=\'" + checkURL + "'>" + checkURL + "</a>";

                AmazonEmail(body, "Check Email!", youremail, PriceMeCommon.ConfigAppString.EmailAddress);
                return true;
            }
            catch (Exception ex)
            {
                PriceMeCommon.BusinessLogic.LogController.WriteException("Set Register Email error : " + ex.Message + "\t" + ex.StackTrace);
                PriceMeCommon.BusinessLogic.LogController.WriteException("Email body : " + checkURL);
                return false;
            }
        }

        public static string GetChristmasString(DateTime LastChristmasDevDate)
        {
            string christmasString = string.Format(Resources.Resource.TextString_ChristmasDelivery, LastChristmasDevDate.ToString("t").Replace("a.m.", "AM").Replace("p.m.", "PM").Replace("00:00:00", "00") + " " + LastChristmasDevDate.ToString("dd MMMM")).Replace(" PM", "PM");
            christmasString = christmasString.Replace(christmasString.Substring(christmasString.IndexOf(":"), 3), "");
            return christmasString;
        }

        public static string GetVisitShopButtonClass()
        {
            var cls = string.Empty;
            switch (WebConfig.CountryId)
            {
                case 41://Hong Kong
                    cls = "-hk"; break;
                case 51://Indonesia
                    cls = "-id"; break;
                case 55://Thailand
                    cls = "-th"; break;
                case 56://Vietnam
                    cls = "-vn";
                    break;
                default: break;
            }
            return cls;
        }

        public static string GetRootUrl(string url, int countryID)
        {
            var rootUrl = "";
            if (countryID == 3 || countryID == 25)
                rootUrl = "https://track.priceme.co.nz";
            else if (countryID == 1)
            {
                rootUrl = "https://track.priceme.com.au";
            }
            else if (countryID == 28)
            {
                rootUrl = "https://track.priceme.com.ph";
            }
            else if (countryID == 41)
            {
                rootUrl = "https://track.priceme.com.hk";
            }
            else if (countryID == 36)
            {
                rootUrl = "https://track.priceme.com.sg";
            }
            else if (countryID == 45)
            {
                rootUrl = "https://track.priceme.com.my";
            }
            else if (countryID == 51)
            {
                rootUrl = "https://track.priceme.co.id";
            }
            else if (countryID == 55)
            {
                rootUrl = "https://track.priceme.com";
            }
            else if (countryID == 56)
            {
                rootUrl = "https://track.priceme.com.vn";
            }
            url = rootUrl + url;

            //char[] Pattern = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
            //       string result = "";
            //       int n = Pattern.Length;
            //       System.Random random = new Random(~unchecked((int)DateTime.Now.Ticks));
            //       for (int i = 0; i < 5; i++)
            //       {
            //           int rnd = random.Next(0,n);
            //           result += Pattern[rnd];
            //       }

            return url;
        }

        public static string GetLoginControl(HttpContext context)
        {
            var oUrl = context.Request.RawUrl.ToLower();
            var loginUrl = string.Empty;
            if (!oUrl.Contains("login.aspx") && !oUrl.Contains("default.aspx"))
            {
                loginUrl = "/Login.aspx" + "?url=" + UrlParaEncode(context.Request.RawUrl);
            }
            else
            {
                loginUrl = "/Login.aspx";
            }

            if (string.IsNullOrEmpty(context.User.Identity.Name))
            {

                StringBuilder sb = new StringBuilder("<a class=\"loginTag\" href=\"");
                sb.Append(loginUrl);
                sb.Append("\"><span class=\"glyphicon glyphicon-user\"></span> ");
                sb.Append(Resources.Resource.TextString_LogIn);
                sb.Append("</a>");
                return sb.ToString();
            }
            else
            {
                StringBuilder sb = new StringBuilder("<ul id=\"loginUL\" class=\"nav navbar-nav\">");
                sb.Append("<li class=\"dropdown\">");
                sb.Append("<a href=\"#\" class=\"dropdown-toggle\" data-toggle=\"dropdown\">");
                sb.Append(HttpContext.Current.User.Identity.Name.CutOut(8));
                sb.Append(" <b class=\"caret\"></b></a>");
                sb.Append("<ul class=\"dropdown-menu\">");
                sb.Append("<li class=\"bg1\" id=\"UserLoginAccount\"><a href=\"");
                sb.Append(UrlController.GetPrimaryProfileUrl(""));
                sb.Append("\"><span class=\"glyphicon glyphicon-cog iconGray\"></span> " + Resources.Resource.TextString_MyAccount + "</a></li>");
                sb.Append("<li class=\"bg1\" id=\"UserLoginLists\"><a href=\"");
                sb.Append(UrlController.GetPrimaryProfileUrl("PricemeList"));
                sb.Append("\"><span class=\"glyphicon glyphicon-list-alt iconGray\"></span> " + Resources.Resource.TextString_MyLists + "</a></li>");
                sb.Append("<li class=\"bg1\" id=\"UserLoginFavourites\"><a href=\"");
                sb.Append(UrlController.GetPrimaryProfileUrl("Favourites"));
                sb.Append("\"><span class=\"glyphicon glyphicon-heart iconGray\"></span> Favourites</a></li>");
                sb.Append("<li class=\"bg1\" id=\"UserLoginAlerts\"><a href=\"");
                sb.Append(UrlController.GetPrimaryProfileUrl("PriceAlert"));
                sb.Append("\"><span class=\"glyphicon glyphicon-bell iconGray\"></span> " + Resources.Resource.TextString_MyAlert + "</a></li>");
                sb.Append("<li class=\"bg1\" id=\"UserLoginReviews\"><a href=\"");
                sb.Append(UrlController.GetPrimaryProfileUrl("Review"));
                sb.Append("\"><span class=\"glyphicon glyphicon-star iconGray\"></span> " + Resources.Resource.TextString_MyReviews + "</a></li>");
                //if(WebConfig.CountryId == 3)
                //{
                //sb.Append("<li class=\"bg1\" id=\"UserLoginPosts\"><a href=\"");
                //sb.Append(UrlController.GetPrimaryProfileUrl("PricemeForum"));
                //sb.Append("\"><span class=\"glyphicon glyphicon-comment iconGray\"></span> My Forum Posts</a></li>");
                //}
                sb.Append("<li class=\"bg1\" id=\"UserLogout\"><a onclick=\"FBLogout();\" href=\"/logout.aspx");
                sb.Append("\"><span class=\"glyphicon glyphicon-log-out iconGray\"></span> ");
                sb.Append(Resources.Resource.TextString_LogOut);
                sb.Append("</a></li>");
                sb.Append("</ul>");
                sb.Append("</li>");
                sb.Append("</ul>");

                return sb.ToString();
            }
        }

        public static string GetDateTime(DateTime dt)
        {
            string date = string.Empty;

            date = dt.ToString("d MMM yyyy", System.Globalization.CultureInfo.CreateSpecificCulture("en-US"));

            return date;
        }

        public static string GetAgoDateTime(DateTime dt)
        {
            string stringDate = string.Empty;

            TimeSpan span = DateTime.Now.Subtract(dt);
            int days = (int)span.TotalDays;
            if (days == 0)
                stringDate = "Today";
            else if (days == 1)
                stringDate = "Yesterday";
            else if (days <= 6)
                stringDate = days + " days ago";
            else if (days < 30)
            {
                int week = days / 7;
                string stringWeek = "week";
                if (week > 1)
                    stringWeek = "weeks";
                stringDate = week + " " + stringWeek + " ago";
            }
            else if (days < 365)
            {
                int month = days / 30;
                string stringMonth = "month";
                if (month > 1)
                    stringMonth = "months";
                stringDate = month + " " + stringMonth + " ago";
            }
            else
            {
                int year = days / 365;
                string stringYear = "year";
                if (year > 1)
                    stringYear = "years";
                stringDate = year + " " + stringYear + " ago";
            }

            return stringDate;
        }

        //
        static System.Text.RegularExpressions.Regex PriceRegex = new System.Text.RegularExpressions.Regex("(?<tag1>[^\\d]*)(?<price>[\\d,\\.]+)(?<tag2>[^\\d]*)");

        static string NewPriceCultureString_Double(double price)
        {
            string priceInfo = price.ToString("C", CurrentCulture);

            priceInfo = FixPriceFormat(priceInfo);

            return priceInfo;
        }

        static string NewPriceCultureString_Int(double price)
        {
            string priceInfo = price.ToString("C0", CurrentCulture);

            priceInfo = FixPriceFormat(priceInfo);

            return priceInfo;
        }

        static string FixPriceFormat(string priceString)
        {
            if (WebConfig.CountryId == 28)
            {
                priceString = priceString.Replace("Php", Resources.Resource.TextString_PriceSymbol);
            }
            else if (WebConfig.CountryId == 56)
            {
                priceString = priceString.Replace("₫", Resources.Resource.TextString_PriceSymbol);
            }
            else if (WebConfig.CountryId == 36)
            {
                priceString = priceString.Replace("$", Resources.Resource.TextString_PriceSymbol);
            }

            System.Text.RegularExpressions.Match match = PriceRegex.Match(priceString);
            if (match.Success)
            {
                string priceInfo = match.Groups["price"].Value;

                string priceTag1 = match.Groups["tag1"].Value.Trim();

                string priceTag2 = match.Groups["tag2"].Value.Trim();

                if (!string.IsNullOrEmpty(priceTag1))
                {
                    priceInfo = "<span class='priceSymbol'>" + priceTag1 + "</span><span class='priceSpan'>" + priceInfo + "</span>";

                }
                else
                {
                    priceInfo = "<span class='priceSpan'>" + priceInfo + "</span><span class='priceSymbol'>" + priceTag2 + "</span>";
                }
                return priceInfo;
            }
            else
            {
                return priceString;
            }
        }
        static string FixPriceFormatForPriceFilter(string priceString)
        {
            if (WebConfig.CountryId == 28)
            {
                priceString = priceString.Replace("Php", Resources.Resource.TextString_PriceSymbol);
            }
            else if (WebConfig.CountryId == 56)
            {
                priceString = priceString.Replace("₫", Resources.Resource.TextString_PriceSymbol);
            }
            else if (WebConfig.CountryId == 36)
            {
                priceString = priceString.Replace("$", Resources.Resource.TextString_PriceSymbol);
            }

            return priceString;
        }

        public static string FormatPrice(double price)
        {
            if (WebConfig.CountryId == 28
                || WebConfig.CountryId == 51
                || WebConfig.CountryId == 41
                || WebConfig.CountryId == 55
                || WebConfig.CountryId == 45
                || WebConfig.CountryId == 56)
            {
                return NewPriceCultureString_Int(price);
            }
            else
            {
                return NewPriceCultureString_Double(price);
            }
        }
        public static string FormatPriceForPriceFilter(string price)
        {
            var price_ = 0d;
            double.TryParse(price, out price_);
            return FormatPriceForPriceFilter(price_);
        }
        public static string FormatPriceForPriceFilter(double price)
        {
            string priceInfo = NewPriceCultureString_Int(price).Replace("span", "label");

            return priceInfo;
        }

        public static string FormatPrice(string priceString)
        {
            double price = double.Parse(priceString, System.Globalization.CultureInfo.CreateSpecificCulture("en-US"));

            return FormatPrice(price);
        }

        public static string FormatPriceNotPriceSymbol(string priceString)
        {
            double price = double.Parse(priceString, System.Globalization.CultureInfo.CreateSpecificCulture("en-US"));

            return FormatPriceNotPriceSymbol(price);
        }

        public static string FormatPriceNotPriceSymbol(double price)
        {
            string priceString = FormatPrice(price);

            System.Text.RegularExpressions.Match match = PriceRegex.Match(priceString);
            if (match.Success)
            {
                string priceInfo = match.Groups["price"].Value;

                return priceInfo;
            }

            return priceString;
        }
        public static string FormatNumeric(string num)
        {
            double nn;
            if (double.TryParse(num, out nn))
                return FormatNumeric(nn);
            else
                return "0";
        }
        public static string FormatNumeric(double num)
        {
            return string.Format(CurrentCulture, "{0:N0}", num);
        }

        public static string GetShippingOriginHTML(int countryID)
        {
            //<img width="16" height="11" alt="" src="<%=Resources.Resource.ImageWebsite + countryInfo.CountryFlag %>" /></div></a>

            var uc = PriceMeCommon.BusinessLogic.RetailerController.GetUtilCountry(countryID);
            if (uc == null) return "";

            string imageHTML = "<img width='16' height='11' alt='" + uc.CountryID + "' src='" + Resources.Resource.ImageWebsite + uc.CountryFlag + "' />";

            string html = imageHTML + "&nbsp;<span>" + uc.CountryID + "</span>";

            return html;
        }

        public static string switchRichDisplay(int typeid, string val, double top1, double cur_val, bool IsHigherBetter)
        {
            string result = "";
            switch (typeid)
            {
                case 1:
                    result = getHorisontalBar(val, top1, IsHigherBetter);
                    break;
                case 2:
                    result = getVerticalBar(val, top1, IsHigherBetter);
                    break;
                case 3:
                    //result = getCpu(val);
                    result = getHorisontalBar(val, top1, IsHigherBetter);
                    break;
                case 4:
                    result = GetBattery(val, top1);
                    break;
                case 5:
                    result = getDensity(val, top1);
                    break;
                case 6:
                    result = getStar(val, cur_val);
                    break;
                case 7:
                    result = getCpu(val);
                    break;
                case 8:
                    result = getSize(val);
                    break;
            }

            return result;
        }

        private static string getSize(string val)
        {
            var size = new System.Text.StringBuilder();
            size.Append("<div class='viz-temp' style='display:inline-block;height:82px;width:90px;'>");
            size.Append("<div class='viz-wrap area-arrows' style='display:inline-block;width:90px;height:90px;'>");
            size.Append("<svg class='viz area-viz'>");
            size.Append("<path stroke-linecap='round' stroke-width='2' stroke='#bbb' d='M 4 86 l 0 -9 M 4 86 l 9 0 M 4 86 L 27 63'></path>");
            size.Append("<path stroke-linecap='round' stroke-width='2' stroke='#bbb' d='M 86 4 l 0 9 M 86 4 l -9 0 M 86 4 L 63 27'></path>");
            size.Append("</svg>");
            size.Append("<div class='val' style='line-height:90px;font-size:21px;top: 0px;'>{0}</div>");
            size.Append("</div>");
            size.Append("<span class='size-viz-suffix'> inch</span>");
            size.Append("</div>");

            val = val.ToLower().Replace("inch", "").Replace("in", "").Trim();
            var html = string.Format(size.ToString(), val);
            return html;
        }

        private static string getCpu(string val)
        {
            val = val.ToLower().Replace("ghz", "").Replace("&nbsp;", "").Trim();
            decimal decv = 0;
            decimal.TryParse(val, out decv);
            string valx = "43.990697090391";
            string valy = "0.4539726579197";
            if (decv > 4m)
            {
                valx = "82.360679774998";
                valy = "63.511410091699";
            }
            else if (decv > 3.5m)
            {
                valx = "87.360679774998";
                valy = "53.511410091699";
            }
            else if (decv > 3.0m)
            {
                valx = "89.360679774998";
                valy = "40.511410091699";
            }
            else if (decv > 2.5m)
            {
                valx = "80.360679774998";
                valy = "30.511410091699";
            }
            else if (decv > 2.0m)
            {
                valx = "80.807164557508";
                valy = "20.290578510099";
            }
            else if (decv > 1.5m)
            {
                valx = "70.807164557508";
                valy = "6.290578510099";
            }
            else
            {
                valx = "43.990697090391";
                valy = "0.4539726579197";
            }

            var cpu = new System.Text.StringBuilder();
            cpu.Append("<div class='viz-temp' style='display:inline-block;height:82px;width:90px;'>");
            cpu.Append("<div class='viz-wrap speedometer' style='width:60px;height:60px;'>");
            cpu.Append("<svg class='viz speed-viz' width='60' height='60'>");
            cpu.Append("<g stroke='#bbb' stroke-width='2' transform='scale(0.6)'>");
            cpu.Append("<circle cx='50' cy='40' r='4' stroke-width='0' fill='#b2d764'></circle>");
            cpu.Append("<line x1='24.920473174376628' y1='58.22134282106667' x2='20.066371208126945' y2='61.74805433482151'></line>");
            cpu.Append("<line x1='20.51724799485024' y1='49.579526825623375' x2='14.810908897079322' y2='51.43362879187306'></line>");
            cpu.Append("<line x1='19' y1='40.00000000000001' x2='13' y2='40.00000000000001'></line>");
            cpu.Append("<line x1='20.517247994850237' y1='30.42047317437663' x2='14.810908897079315' y2='28.566371208126952'></line>");
            cpu.Append("<line x1='24.920473174376628' y1='21.778657178933337' x2='20.066371208126945' y2='18.251945665178496'></line>");
            cpu.Append("<line x1='31.77865717893333' y1='14.920473174376632' x2='28.25194566517849' y2='10.066371208126949'></line>");
            cpu.Append("<line x1='40.420473174376625' y1='10.51724799485024' x2='38.56637120812694' y2='4.810908897079322'></line>");
            cpu.Append("<line x1='49.99999999999999' y1='9' x2='49.99999999999999' y2='3'></line>");
            cpu.Append("<line x1='59.57952682562336' y1='10.517247994850237' x2='61.43362879187305' y2='4.810908897079315'></line>");
            cpu.Append("<line x1='68.22134282106666' y1='14.920473174376625' x2='71.74805433482149' y2='10.066371208126942'></line>");
            cpu.Append("<line x1='75.07952682562336' y1='21.778657178933326' x2='79.93362879187305' y2='18.251945665178486'></line>");
            cpu.Append("<line x1='79.48275200514976' y1='30.420473174376625' x2='85.18909110292068' y2='28.566371208126938'></line>");
            cpu.Append("<line x1='81' y1='39.99999999999999' x2='87' y2='39.99999999999999'></line>");
            cpu.Append("<line x1='79.48275200514976' y1='49.57952682562336' x2='85.18909110292068' y2='51.43362879187305'></line>");
            cpu.Append("<line x1='75.07952682562338' y1='58.221342821066656' x2='79.93362879187306' y2='61.7480543348215'></line>");
            cpu.Append("<line x1='50' y1='40' x2='" + valx + "' y2='" + valy + "' stroke-width='3' stroke='#b2d764'></line>");
            cpu.Append("</g>");
            cpu.Append("</svg>");
            cpu.Append("<div class='val' style='line-height:60px;font-size:17px;top: -5px;'>{0}</div>");
            cpu.Append("</div>");
            cpu.Append("<span class='cpu-viz-suffix'> GHz</span>");
            cpu.Append("</div>");

            var html = string.Format(cpu.ToString(), val);
            return html;
        }

        private static string getDensity(string val, double width)
        {
            string color = "";
            if (width >= 45)
                color = "#b2d764";
            else if (width < 30)
                color = "red";
            else
                color = "#ffaa00";

            int count = (int)(width / 4);
            if (count > 25)
                count = 25;

            var den = new System.Text.StringBuilder();
            den.Append("<div class='viz-temp' style='height: auto;display:inline-block;width:90px'>");
            den.Append("<div class='ratio-square'>");
            den.Append("<div class='ratio-viz'>");
            for (int i = 0; i < count; i++)
            {
                int row = i / 5;

                int k = i;
                if (row > 0)
                    k = i - (row * 5);
                int clu = 3 + (k * 8);

                int rv = 40 + (row * 8);

                den.Append("<div style='position:absolute;left:" + clu + "px;top:" + rv + "px;width:7px;height:7px;background-color:" + color + ";'></div>");
            }
            den.Append("</div>");
            den.Append("<span style='font-size:14px;position:relative; bottom:-125%'>{0}</span>");
            den.Append("</div>");
            den.Append("</div>");

            var html = string.Format(den.ToString(), val);
            return html;
        }

        private static string GetBattery(string val, double width)
        {
            string color = "";
            if (width >= 45)
                color = "#b2d764";
            else if (width < 30)
                color = "red";
            else
                color = "#ffaa00";

            var battery = new System.Text.StringBuilder();
            battery.Append(" <div class='battery-viz-temp' style='height: auto;display:inline-block;width:90px'>");
            battery.Append("<div class='battery-viz-wrap'>");
            battery.Append("<div class='battery-viz'>");
            battery.Append("<div class='battery-cap'></div>");
            battery.Append("<div class='battery-color' style='width:{1}%;background-color:{2};'></div>");
            battery.Append("</div>");
            battery.Append("<span style='font-size:14px;position:relative; bottom:-80%'>{0}</span>");
            battery.Append("</div>");
            //battery.Append("<span class='battery-viz-suffix'> mAh</span>");
            battery.Append("</div>");

            var html = string.Format(battery.ToString(), val, width.ToString("0.00"), color);
            return html;
        }

        private static string getStar(string val, double cur_val)
        {

            double star = cur_val;
            // double.TryParse(cur_val, out star);

            string posi = "444";

            if (star >= 5 || star >= 4.5)
                posi = "954";
            else if (star >= 4 || star >= 3.5)
                posi = "848";
            else if (star >= 3 || star >= 2.5)
                posi = "742";
            else if (star >= 2 || star >= 1.5)
                posi = "637";
            else if (star >= 1 || star >= 0.5)
                posi = "535";
            else
                posi = "444";

            var stars = new System.Text.StringBuilder();
            stars.Append("<div class='star-clr' style='height: auto;display:inline-block;width:150px'>");
            stars.Append("<div class='star-float-left1'>");
            stars.Append("<div class='star-FourAndHaftScoreP' style='background-position: 0px -{0}px;' title=''></div></div>");
            stars.Append("<div class='star-float-left2' >&nbsp;{1}</div>");
            stars.Append("</div>");

            var html = string.Format(stars.ToString(), posi, val);
            return html;
        }

        private static string getVerticalBar(string val, double top1, bool IsHigherBetter)
        {

            var bar = new System.Text.StringBuilder();
            bar.Append("<div class='vertical-bar-viz-temp' style='height: auto;display:inline-block;width:90px'>");
            bar.Append("<div class='vertical-bar-viz-wrap' >");
            bar.Append("<div class='vertical-bar-viz'>");
            if (IsHigherBetter)
            {
                if (top1 >= 45)
                    bar.Append("<div class='vertical-bar-color' style='height:{0};background-color:#b2d764;'></div>");
                else if (top1 < 45 && top1 >= 30)
                    bar.Append("<div class='vertical-bar-color' style='height:{0};background-color:#ffaa00;'></div>");
                else
                    bar.Append("<div class='vertical-bar-color' style='height:{0};background-color:red;'></div>");
            }
            else
            {
                if (top1 < 30)
                    bar.Append("<div class='vertical-bar-color' style='height:{0};background-color:#b2d764;'></div>");
                else if (top1 < 45 && top1 >= 30)
                    bar.Append("<div class='vertical-bar-color' style='height:{0};background-color:#ffaa00;'></div>");
                else
                    bar.Append("<div class='vertical-bar-color' style='height:{0};background-color:red;'></div>");
            }
            bar.Append("</div>");
            bar.Append("<div class='vertical-bar-val'>{1}</div>");
            bar.Append("</div>");
            bar.Append("<span class='vertical-bar-viz-suffix'> </span>");
            bar.Append("</div>");

            var html = string.Format(bar.ToString(), top1.ToString("0.00") + "%", val);
            return html;
        }

        private static string getHorisontalBar(string val, double top1, bool IsHigherBetter)
        {
            //background-color:red

            var bar = new System.Text.StringBuilder();
            bar.Append("<div class='vertical-bar-viz-temp' style='height: auto;display:inline-block;width:90px'>");
            bar.Append("<div class='vertical-bar-viz-wrap vertical-bar-viz-short'>");
            bar.Append("<div class='vertical-bar-viz' style='width: 48px; height: 12px;'>");
            if (IsHigherBetter)
            {
                if (top1 >= 45)
                    bar.Append("<div class='vertical-bar-color' style='width: {0}; height: 12px;background-color:#b2d764;'>");
                else if (top1 < 45 && top1 >= 30)
                    bar.Append("<div class='vertical-bar-color' style='width: {0}; height: 12px;background-color:#ffaa00;'>");
                else
                    bar.Append("<div class='vertical-bar-color' style='width: {0}; height: 12px;background-color:red;'>");
            }
            else
            {
                if (top1 < 30)
                    bar.Append("<div class='vertical-bar-color' style='width: {0}; height: 12px;background-color:#b2d764;'>");
                else if (top1 < 45 && top1 >= 30)
                    bar.Append("<div class='vertical-bar-color' style='width: {0}; height: 12px;background-color:#ffaa00;'>");
                else
                    bar.Append("<div class='vertical-bar-color' style='width: {0}; height: 12px;background-color:red;'>");
            }

            bar.Append("</div>");
            bar.Append("</div>");
            bar.Append("<div style='right:13px;' class='vertical-bar-val'>{1}</div>");
            bar.Append("</div></div>");

            var html = string.Format(bar.ToString(), top1.ToString("0.00") + "%", val);
            return html;
        }

        /// <summary>
        /// 将用户信息写入Cookies
        /// </summary>
        /// <param name="user">用户对象</param>
        /// <returns></returns>
        public static bool SaveUserInfoToCookie(UserData user)
        {

            var json = JSONHelper.ObjectToJSON(user);
            json = EncryptStr(json).ToLower();
            HttpCookie c = new HttpCookie("our_custom_session_cookienew_xxxx", json);
            c.Path = "/";
            //c.Domain = "priceme.co.nz";
            //c.Domain = "192.168.1.109";
            c.Secure = false;
            c.HttpOnly = true;
            c.Expires = DateTime.Now.AddMonths(1);
            HttpContext.Current.Response.Cookies.Add(c);

            return true;

        }

        /// <summary>
        /// 从Cookie里获取用户信息
        /// </summary>
        /// <returns></returns>
        public static UserData GetUserInfoFromCookie()
        {
            var userCookie = HttpContext.Current.Request.Cookies["our_custom_session_cookienew_xxxx"];
            if (userCookie != null)
            {
                var json = DecryptStr(userCookie.Value.ToUpper());
                return JSONHelper.JSONToObject<UserData>(json);
            }

            return null;
        }

        //URL传输参数加密Key这个key可以自己设置支持8位这个东西很重要的,密钥
        static string _QueryStringKey = "PriceMe8";

        /// <summary>
        /// 加密算法
        /// </summary>
        public static string EncryptStr(string QueryString)
        {
            return Encrypt(QueryString, _QueryStringKey);
        }

        /// <summary>
        /// 解密算法
        /// </summary>
        public static string DecryptStr(string QueryString)
        {
            return Decrypt(QueryString, _QueryStringKey);
        }

        public static string Encrypt(string originalString, string sKey)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();

            // 把字符串放到byte数组中
            byte[] inputByteArray = Encoding.Default.GetBytes(originalString);

            des.Key = ASCIIEncoding.ASCII.GetBytes(sKey); //建立加密对象的密钥和偏移量
            des.IV = ASCIIEncoding.ASCII.GetBytes(sKey);  //原文使用ASCIIEncoding.ASCII方法的

            //GetBytes方法
            MemoryStream ms = new MemoryStream();         //使得输入密码必须输入英文文本
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);

            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            StringBuilder ret = new StringBuilder();

            foreach (byte b in ms.ToArray())
            {
                ret.AppendFormat("{0:X2}", b);
            }
            ret.ToString();
            return ret.ToString();
        }

        public static string Decrypt(string originalString, string sKey)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();

            byte[] inputByteArray = new byte[originalString.Length / 2];
            for (int x = 0; x < originalString.Length / 2; x++)
            {
                int i = (Convert.ToInt32(originalString.Substring(x * 2, 2), 16));
                inputByteArray[x] = (byte)i;
            }

            //建立加密对象的密钥和偏移量，此值重要，不能修改

            des.Key = ASCIIEncoding.ASCII.GetBytes(sKey);

            des.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);

            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();

            //建立StringBuild对象，CreateDecrypt使用的是流对象，必须把解密后的文本变成流对象
            StringBuilder ret = new StringBuilder();

            return System.Text.Encoding.Default.GetString(ms.ToArray());
        }

        public static string GetIPAddress()
        {
            string userIP;
            // HttpRequest Request = HttpContext.Current.Request;  
            HttpRequest Request = HttpContext.Current.Request; // ForumContext.Current.Context.Request;  
            // 如果使用代理，获取真实IP  
            if (string.IsNullOrEmpty(Request.ServerVariables["HTTP_X_FORWARDED_FOR"]))
                userIP = Request.ServerVariables["REMOTE_ADDR"];
            else
                userIP = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (string.IsNullOrEmpty(userIP))
                userIP = Request.UserHostAddress;

            return userIP;
        }

        public static void FixProductCatalogList(List<PriceMeCommon.Data.ProductCatalog> productCatalogList, string view)
        {
            foreach (var pc in productCatalogList)
            {
                if (string.IsNullOrEmpty(pc.DefaultImage))
                {
                    pc.ImageAlt = "No Image";
                }
                else
                {
                    pc.ImageAlt = HttpUtility.HtmlEncode(pc.ProductName);
                }

                string imgFixString;
                if (view.Equals("list", StringComparison.InvariantCultureIgnoreCase))
                {
                    imgFixString = "_s";
                }
                else
                {
                    imgFixString = "_ms";
                }

                string imageSRC = !pc.DefaultImage.Contains(".") ? (Resources.Resource.ImageWebsite + "/images/no_image_available.gif") : (GlobalOperator.FixImagePath(pc.DefaultImage.Insert(pc.DefaultImage.LastIndexOf('.'), imgFixString)));
                if (!imageSRC.StartsWith("http:") && !imageSRC.StartsWith("https:"))
                {
                    imageSRC = Resources.Resource.ImageWebsite + imageSRC;
                }
                if (imageSRC.Contains("http://"))
                    imageSRC = imageSRC.Replace("http://", "https://");
                pc.DefaultImage = imageSRC;
                string clickOutUrl = Utility.GetRootUrl("/ResponseRedirect.aspx?aid=40&pid=" + pc.ProductID + "&rid=" + pc.BestPPCRetailerID + "&rpid=" + pc.BestPPCRetailerProductID + "&countryID=" + WebConfig.CountryId + "&cid=" + pc.CategoryID + "&t=c", WebConfig.CountryId).Replace("&", "&amp;");
                string uuid = Guid.NewGuid().ToString();
                clickOutUrl += "&uuid=" + uuid;

                pc.ClickOutUrl = clickOutUrl;
                string linkUrl = UrlController.GetProductUrl(int.Parse(pc.ProductID), pc.ProductName);
                pc.LinkUrl = linkUrl;

                double score;
                double avR = 0;
                if (double.TryParse(pc.AvRating, out avR))
                {
                    score = double.Parse(pc.AvRating, PriceMeCommon.PriceMeStatic.Provider);
                }
                else
                {
                    score = 0d;
                }
                //pc.StarsImage = Utility.GetStarImage(score);
                pc.RatingPercent = Utility.GetStarRatingPercent(score);
                if (score.ToString("0.0") != "0.0")
                {
                    pc.StarsImageAlt = string.Format(Resources.Resource.TextString_OutOfRating, score.ToString("0.0")).Replace(",", ".");
                }
                else
                {
                    pc.StarsImageAlt = Resources.Resource.TextString_NoRating;
                }

                if (WebConfig.CountryId != 41)
                {
                    if (WebConfig.CountryId == 51)
                    {
                        pc.ComparePriceString = Resources.Resource.TextString_Compare;
                    }
                    else
                    {
                        pc.ComparePriceString = Resources.Resource.TextString_Compare + " " + Resources.Resource.TextString_PriceCount;
                    }
                }
                else
                {
                    pc.ComparePriceString = Resources.Resource.TextString_Compare + Resources.Resource.TextString_PriceCount;
                }

                if (PriceMeCommon.BusinessLogic.CategoryController.IsSearchOnly(pc.CategoryID, WebConfig.CountryId))
                {
                    pc.IsSearchOnly = true;
                }
                else
                {
                    pc.IsSearchOnly = false;
                }
            }
        }

        public static float GetStarRatingPercent(double score)
        {
            if (score <= 0)
                return 0.01f;

            if (score > 5.0d)
            {
                score = 5.0d;
            }

            float p = (float)score / 5f;
            return p;
        }

        public static string GetPriceRangeString(PriceRange priceRange)
        {
            return FormatPriceForPriceFilter(priceRange.MinPrice) + "-" + FormatPriceForPriceFilter(priceRange.MaxPrice);
        }

        public static List<int> GetDaysProductCountList(NarrowByInfo daysNarrowByInfo)
        {
            int maxValue = int.Parse(daysNarrowByInfo.NarrowItemList.Last().Value);
            int step = 10;
            int stepDays = maxValue / step;
            if (maxValue % step > 5)
            {
                stepDays++;
            }

            List<int> countList = new List<int>();
            int cc = 0;
            int ss = stepDays;
            for (int i = 0; i < daysNarrowByInfo.NarrowItemList.Count; i++)
            {
                if (i < ss)
                {
                    cc += daysNarrowByInfo.NarrowItemList[i].ProductCount;
                }
                else
                {
                    countList.Add(cc);

                    ss += stepDays;
                    if (daysNarrowByInfo.NarrowItemList.Count - ss <= step)
                    {
                        ss = daysNarrowByInfo.NarrowItemList.Count;
                    }
                    cc = daysNarrowByInfo.NarrowItemList[i].ProductCount;
                }
            }
            countList.Add(cc);

            return countList;
        }

        public static List<int> GetAttributeSliderProductCountList_New(NarrowByInfo attributeSliderNarrowByInfo)
        {
            List<float> valueArray = attributeSliderNarrowByInfo.NarrowItemList.Select(ni => ni.FloatValue).ToList();
            valueArray.Sort();
            float minValue = valueArray.First();
            float maxValue = valueArray.Last();

            float step = GetHeightCountStep(attributeSliderNarrowByInfo);

            List<int> countList = new List<int>();
            float nsc = minValue + step;
            int sumCC = 0;

            do
            {
                int cc = attributeSliderNarrowByInfo.NarrowItemList.Where(ni => ni.FloatValue <= nsc).Sum(ni => ni.ProductCount);
                cc -= sumCC;
                sumCC += cc;

                countList.Add(cc);
                nsc += step;
            }
            while (nsc <= maxValue);

            return countList;
        }

        public static float GetSliderStep(NarrowByInfo attributeSliderNarrowByInfo)
        {
            List<float> valueArray = attributeSliderNarrowByInfo.NarrowItemList.Select(ni => ni.FloatValue).ToList();
            valueArray.Sort();
            float minValue = valueArray.First();
            float maxValue = valueArray.Last();

            var map = (PriceMeCache.CategoryAttributeTitleMapCache)attributeSliderNarrowByInfo.CategoryAttributeTitleMap;
            int dbStep3 = map.Step3;

            float range = maxValue - minValue;

            float step = 0;

            if (range >= dbStep3)
            {
                step = 1;
            }
            else
            {
                step = (float)range / (float)dbStep3;
            }

            if (step < 0.1f)
            {
                step = 0.1f;
            }

            return step;
        }

        public static float GetHeightCountStep(NarrowByInfo attributeSliderNarrowByInfo)
        {
            List<float> valueArray = attributeSliderNarrowByInfo.NarrowItemList.Select(ni => ni.FloatValue).ToList();
            valueArray.Sort();
            float minValue = valueArray.First();
            float maxValue = valueArray.Last();

            var map = (PriceMeCache.CategoryAttributeTitleMapCache)attributeSliderNarrowByInfo.CategoryAttributeTitleMap;
            int dbStep3 = map.Step3;

            float range = maxValue - minValue;

            float step = range / (float)dbStep3;

            if (step < 0.1)
            {
                step = 0.1f;
            }

            return step;
        }

        public static List<int> GetAttributeSliderProductCountList(NarrowByInfo attributeSliderNarrowByInfo)
        {
            List<int> valueArray = attributeSliderNarrowByInfo.NarrowItemList.Select(ni => ni.ListOrder).ToList();
            valueArray.Sort();
            int minValue = valueArray.First();
            int maxValue = valueArray.Last();

            var map = (PriceMeCache.CategoryAttributeTitleMapCache)attributeSliderNarrowByInfo.CategoryAttributeTitleMap;

            int stepPart = 4;

            int s = (int)map.Step;
            if (s > 0)
            {
                stepPart = (int)(maxValue - minValue) / s;
            }

            if (stepPart == 0)
            {
                stepPart = 4;
            }

            var sc = attributeSliderNarrowByInfo.NarrowItemList.Count / stepPart;
            if (attributeSliderNarrowByInfo.NarrowItemList.Count % stepPart > 0)
            {
                stepPart++;
            }

            List<int> countList = new List<int>();
            int cc = 0;
            int nsc = sc;
            for (int i = 0; i < attributeSliderNarrowByInfo.NarrowItemList.Count; i++)
            {
                if (i < nsc)
                {
                    cc += attributeSliderNarrowByInfo.NarrowItemList[i].ProductCount;
                }
                else
                {
                    nsc += sc;
                    countList.Add(cc);
                    cc = attributeSliderNarrowByInfo.NarrowItemList[i].ProductCount;
                }
            }

            if (cc > 0)
            {
                countList.Add(cc);
            }

            return countList;
        }

        public static string GetStarRatingNew(double rating)
        {
            string stringrating = (PriceMe.Utility.GetStarRatingPercent(rating) * 100 - 1).ToString("0.00");
            if (stringrating == "-1.00")
                stringrating = "0.00";

            return stringrating;
        }

        public static bool IsMyList(int pid)
        {
            string pidCollection = string.Empty;
            if (HttpContext.Current.Request.Cookies["compareprice"] != null && HttpContext.Current.Request.Cookies["compareprice"].Value != null)
                pidCollection = HttpContext.Current.Request.Cookies["compareprice"].Value;

            string[] pids = pidCollection.Split('_');
            for (int i = 0; i < pids.Length; i++)
            {
                int productid = 0;
                int.TryParse(pids[i], out productid);

                if (pid == productid)
                    return true;
            }

            return false;
        }

        public static string GetImage(string imageurl, string size)
        {
            string img = string.Empty;
            if (string.IsNullOrEmpty(imageurl))
                img = Resources.Resource.ImageWebsite + "/images/no_image_available.gif";
            else
            {
                img = GlobalOperator.FixImagePath(imageurl.Insert(imageurl.LastIndexOf('.'), size));
                if (!img.Contains("https://") && !img.Contains("http://"))
                    img = Resources.Resource.ImageWebsite + img;
                if (img.Contains("http://"))
                    img = img.Replace("http://", "https://");

                if (img.StartsWith("/"))
                    img = img.Substring(1, img.Length - 1);
            }

            return img;
        }

        public static string GetLargeImage(string imageurl)
        {
            string size = "_l";
            string img = string.Empty;
            if (string.IsNullOrEmpty(imageurl))
                img = Resources.Resource.ImageWebsite + "/images/no_image_available.gif";
            else
                img = Resources.Resource.ImageWebsite + "/Large" + GlobalOperator.FixImagePath(imageurl.Insert(imageurl.LastIndexOf('.'), size));

            return img;
        }

        public static string GetLargeImage1(string imageurl)
        {
            string size = "_l";
            string img = string.Empty;
            if (string.IsNullOrEmpty(imageurl))
                img = Resources.Resource.ImageWebsite + "/images/no_image_available.gif";
            else if (!imageurl.ToLower().Contains("https://"))
                img = Resources.Resource.ImageWebsite + "/Large" + GlobalOperator.FixImagePath(imageurl.Insert(imageurl.LastIndexOf('.'), size));
            else
                img = GlobalOperator.FixImagePath(imageurl.Insert(imageurl.LastIndexOf('.'), size)).Replace("/Images/", "/Large/Images/");
            return img;
        }

    }
}