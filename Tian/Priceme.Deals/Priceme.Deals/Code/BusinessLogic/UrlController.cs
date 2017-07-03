using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
using PriceMeDBA;
using PriceMeCommon;
using PriceMeCommon.Data;
using PriceMeCommon.Extend;
using PriceMeCommon.BusinessLogic;
using PriceMeCache;

namespace PriceMe
{
    public static class UrlController
    {

        //不能用[^\w-]+ 因为 \w 包括字母，数字，下划线，汉字等其它非符号
        private static Regex illegalReg = new Regex(@"[^a-z0-9-+]+", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static Regex illegalReg1 = new Regex(@"[^a-z0-9-]+", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static Regex illegalReg2 = new Regex("-+", RegexOptions.Compiled);
        private static Regex illegalReg3 = new Regex("^-+|-+$", RegexOptions.Compiled);

        public static string GetRewriterUrl(PageName pageName, Dictionary<string, string> queryParameters)
        {
            string url = "";
            //if (queryParameters == null || queryParameters.Count == 0)
            //{
            //    throw new ParameterException("queryParameters is null");
            //}

            switch (pageName)
            {
                case PageName.Catalog:
                    url = CatalogUrl(queryParameters);
                    break;
                case PageName.Search:
                    url = SearchUrl(queryParameters);
                    break;
                case PageName.RetailerInfo:
                    url = RetailerInfoUrl(queryParameters);
                    break;
                case PageName.RetailerList:
                    url = RetailerListUrl(queryParameters);
                    break;
                case PageName.RetailerCategory:
                    url = GetRetailerCategoryUrl(queryParameters);
                    break;
                case PageName.RetailerReview:
                    url = RetailerReviewUrl(queryParameters);
                    break;
                case PageName.Brand:
                    url = BrandUrl(queryParameters);
                    break;
                case PageName.BuyingGuide:
                    url = GetCategoryBuyingGuidesUrl(queryParameters);
                    break;
                case PageName.AllBrands:
                    url = GetAllBrandsUrl(queryParameters);
                    break;
                case PageName.AllList:
                    url = GetAllListsUrl(queryParameters);
                    break;
                case PageName.ExpertReview:
                    url = ExpertReviewUrl(queryParameters);
                    break;
                case PageName.all_lists:
                    url = get_all_lists(queryParameters);
                    break;
                case PageName.AllRrecentPriceDrop:
                    url = GetAllRrecentPriceDropUrl(queryParameters);
                    break;
            }

            return url;
        }

        private static string GetAllRrecentPriceDropUrl(Dictionary<string, string> queryParameters)
        {
            string url = "/Community/AllRrecentPriceDrop.aspx";
            if (queryParameters.ContainsKey("pg") && queryParameters["pg"] != "1")
            {
                url = url + "?pg=" + queryParameters["pg"];
            }
            return url;
        }

        private static string get_all_lists(Dictionary<string, string> queryParameters)
        {
            if (queryParameters.Count == 0 || (queryParameters.Count == 1 && queryParameters.ContainsKey("pg") && queryParameters["pg"] == "1"))
            {
                return "/all_lists.aspx";
            }

            string url = "/all_lists.aspx?";

            if (queryParameters.ContainsKey("pg") && queryParameters["pg"] != "1")
            {
                url = url + "pg=" + queryParameters["pg"];
            }
            return url.TrimEnd('&');
        }

        private static string ExpertReviewUrl(Dictionary<string, string> queryParameters)
        {
            if (!queryParameters.ContainsKey("pid"))
            {
                throw new ParameterException("ExpertReviewUrl Parameter Error! queryParameters not contains 'pid'");
            }

            int pid = int.Parse(queryParameters["pid"]);
            string name = queryParameters["name"];

            string sOut = GetProductExperReviewUrl(pid, name);

            if (queryParameters.ContainsKey("pg") && queryParameters["pg"] != "1")
                sOut += "&pg=" + queryParameters["pg"];
            if (queryParameters.ContainsKey("stars"))
                sOut += "&stars=" + queryParameters["stars"];
            if (queryParameters.ContainsKey("sb"))
                sOut += "&sb=" + queryParameters["sb"];

            if (sOut.Contains(".aspx&")) sOut = sOut.Replace(".aspx&", ".aspx?");
            return sOut;
        }

        public static string GetAllListsUrl(Dictionary<string, string> queryParameters)
        {
            if (queryParameters.Count == 0 || (queryParameters.Count == 1 && queryParameters.ContainsKey("pg") && queryParameters["pg"] == "1"))
            {
                return "/lists.aspx";
            }

            string url = "/lists/";
            if (queryParameters.ContainsKey("pg") && queryParameters["pg"] != "1")
            {
                url = url + "pg-" + queryParameters["pg"] + ",";
            }
            url = url.TrimEnd(',') + ".aspx";
            return url;
        }

        public static string GetAllBrandsUrl(Dictionary<string, string> queryParameters)
        {
            if (queryParameters.Count == 0 || (queryParameters.Count == 1 && queryParameters.ContainsKey("pg") && queryParameters["pg"] == "1"))
            {
                return "/AllBrands.aspx";
            }

            string url = "/AllBrands.aspx?";
            if (queryParameters.ContainsKey("sortletter"))
            {
                url = url + "sortletter=" + queryParameters["sortletter"] + "&";
            }
            if (queryParameters.ContainsKey("pg") && queryParameters["pg"] != "1")
            {
                url = url + "pg=" + queryParameters["pg"];
            }
            return url.TrimEnd('&');
        }

        public static string GetMostPopularUrl(int categoryID)
        {
            string url = "/MostPopular/p_cid-" + categoryID + ".aspx ";
            return url;
        }

        public static string GetCategoryBuyingGuidesUrl(Dictionary<string, string> queryParameters)
        {
            if (queryParameters.ContainsKey("bg"))
            {
                int bgid = queryParameters["bg"].ToInt(0);
                var bg = BuyingGuideController.GetBuyingGuideByBGID(bgid, PriceMe.WebConfig.CountryId);
                //int cid = queryParameters["c"].ToInt(0);
                //PriceMeCache.CategoryCache cat = CategoryController.GetCategoryByCategoryID(cid);

                string url = "";
                if (bg != null)
                {
                    if (PriceMe.WebConfig.CountryId == 1 || PriceMe.WebConfig.CountryId == 3 || PriceMe.WebConfig.CountryId == 28 || PriceMe.WebConfig.CountryId == 36 || PriceMe.WebConfig.CountryId == 45)
                    {
                        url = "/" + FilterInvalidUrlPathChar(bg.BGName) + "-Buying-Guides/bg-" + bg.BGID + ".aspx";
                    }
                    else
                    {
                        if (queryParameters.ContainsKey("cName"))
                        {
                            string cName = queryParameters["cName"];
                            url = "/" + FilterInvalidUrlPathChar(cName) + "-Buying-Guides/bg-" + bg.BGID + ".aspx";
                        }
                        else
                        {
                            url = "/Buying-Guides/bg-" + bg.BGID + ".aspx";
                        }
                    }
                }
                return url;
            }
            //if (queryParameters.ContainsKey("bg") && queryParameters.ContainsKey("c"))
            //{
            //    int bgid = queryParameters["bg"].ToInt(0);
            //    CSK_Store_BuyingGuide bg = BuyingGuideController.GetBuyingGuideByBGID(bgid);
            //    int cid = queryParameters["c"].ToInt(0);
            //    PriceMeCache.CategoryCache cat = CategoryController.GetCategoryByCategoryID(cid);

            //    string url ="";
            //    if (bg != null) url = "/" + FilterInvalidUrlPathChar(bg.BGName) + "-Buying-Guides/bg-" + bg.BGID+-cat.CategoryID + ".aspx";
            //    return url;
            //}
            if (queryParameters.ContainsKey("c"))
            {
                int cid = queryParameters["c"].ToInt(0);
                PriceMeCache.CategoryCache cat = CategoryController.GetCategoryByCategoryID(cid, PriceMe.WebConfig.CountryId);
                return GetCategoryBuyingGuidesUrl(cat);
            }

            return "/BuyingGuides.aspx";
        }

        //public static string GetBuyingGuidesUrl(CSK_Store_BuyingGuide buyingGuide)
        //{
        //    string url = "/" + FilterInvalidUrlPathChar(buyingGuide.BGName) + "-Buying-Guides/bg-" + buyingGuide.BGID + ".aspx";
        //    return url;
        //}

        public static string GetCategoryBuyingGuidesUrl(PriceMeCache.CategoryCache category)
        {
            string url = "/" + FilterInvalidUrlPathChar(category.CategoryNameEN) + "-Buying-Guides/bg-" + category.CategoryID + ".aspx";
            return url;
        }

        private static string BrandUrl(Dictionary<string, string> queryParameters)
        {
            string url = "";
            int mid = int.Parse(queryParameters["mid"]);
            ManufacturerInfo manu = ManufacturerController.GetManufacturerByID(mid, PriceMe.WebConfig.CountryId);
            if (manu != null)
            {
                string pString = "brand_" + mid;
                url = "/" + FilterInvalidUrlPathChar(manu.ManufacturerName).Replace("-", "_") + "/" + pString + ".aspx";
                url = url.Replace("/Money/", "/Money-/").Replace("/money/", "/money-/");
            }
            return url;
        }

        public static string GetBrandPageUrl(int manufacturerID)
        {
            string url = "";
            ManufacturerInfo manu = ManufacturerController.GetManufacturerByID(manufacturerID, PriceMe.WebConfig.CountryId);
            if (manu != null)
            {
                string pString = "brand_" + manufacturerID;
                url = "/" + FilterInvalidUrlPathChar(manu.ManufacturerName) + "/" + pString + ".aspx";
            }
            return url;
        }

        #region ruangang
        public static string GetProductUrl(int productID, string productName)
        {
            return GetProductUrl(productID.ToString(), productName);
        }

        public static string GetProductUrl(string productID, string productName)
        {
            if (PriceMe.WebConfig.CountryId == 41)
                return string.Format("/p-{0}.aspx", productID);
            return string.Format("/{0}/p-{1}.aspx", FilterInvalidNameChar(productName), productID);
        }

        public static string GetAuthorUrl(string authorName)
        {
            return string.Format("/Community/{0}.aspx", HttpUtility.UrlEncode(authorName.Replace("-", "#8254;")));
        }

        public static string GetReviewAuthorUrl(string authorName)
        {
            return string.Format("/Community/{0}.aspx", HttpUtility.UrlEncode(authorName.Replace("-", "#8254;")) + "-rtreview");
        }

        public static string GetProductExperReviewUrl(int productID, string productName)
        {
            if (PriceMe.WebConfig.CountryId == 41)
                return string.Format("/pe-{0}.aspx", productID);
            return string.Format("/{0}-review/pe-{1}.aspx", FilterInvalidNameChar(productName), productID);
        }

        public static string GetProductUserReviewUrl(int productID, string productName)
        {
            if (PriceMe.WebConfig.CountryId == 41)
                return string.Format("/pu-{0}.aspx", productID);
            return string.Format("/{0}-review/pu-{1}.aspx", FilterInvalidUrlPathChar(productName), productID);
        }

        public static string GetProductDescriptionUrl(int productID, string productName)
        {
            if (PriceMe.WebConfig.CountryId == 41)
                return string.Format("/pd-{0}.aspx", productID);
            return string.Format("/{0}-specs/pd-{1}.aspx", FilterInvalidUrlPathChar(productName), productID);
        }

        public static string GetProductMapUrl(int productID, string productName)
        {
            if (PriceMe.WebConfig.CountryId == 41)
                return string.Format("/pm-{0}.aspx", productID);
            return string.Format("/{0}-map/pm-{1}.aspx", FilterInvalidNameChar(productName), productID);
        }

        public static string GetRetailerProductUrl(int rpid, string rpname)
        {
            if (PriceMe.WebConfig.CountryId == 41)
                return string.Format("/rp-{0}.aspx", rpid);

            if (rpname.Length > 65)
                rpname = rpname.Substring(0, 65);

            return string.Format("/{0}/rp-{1}.aspx", FilterInvalidUrlPathChar(rpname), rpid);
        }

        private static string GetRetailerCategoryUrl(Dictionary<string, string> q)
        {
            string rid = string.Empty;
            string rname = string.Empty;
            string cid = string.Empty;
            string cname = string.Empty;
            string pg = string.Empty;

            if (!q.ContainsKey("rid") || !q.ContainsKey("rname") || !q.ContainsKey("cid") || !q.ContainsKey("cname"))
                throw new ParameterException("RetailerInfoParameterError! queryParameters not contains key");
            rid = q["rid"];
            rname = q["rname"];
            cid = q["cid"];
            cname = q["cname"];

            if (q.ContainsKey("pg"))
                pg = q["pg"];

            string url = string.Empty;
            if (pg != string.Empty && pg != "1")
                url = string.Format("/{0}/rc-{1}_{2},pg-{3}.aspx", FilterInvalidUrlPathChar(rname + "-" + cname), rid, cid, pg);
            else
                url = string.Format("/{0}/rc-{1}_{2}.aspx", FilterInvalidUrlPathChar(rname + "-" + cname), rid, cid);

            return url;
        }

        public static string GetRetailerCategoryUrl(int rid, string rname, int cid, string cname)
        {
            return string.Format("/{0}/rc-{1}_{2}.aspx", FilterInvalidUrlPathChar(rname + "-" + cname), rid, cid);
        }

        public static string GetReviewerUrl(int cid, string cname)
        {
            return string.Format("/{0}-Reviews/rec-{1}.aspx", FilterInvalidUrlPathChar(cname), cid);
        }
        #endregion

        public static string GetCatalogUrl(int categoryID)
        {
            if (categoryID == 1283 && PriceMe.WebConfig.CountryId == 3)
            {
                return Resources.Resource.Global_HomePageUrl + "/plans/mobile-plans";
            }
            else if (categoryID == 358 && PriceMe.WebConfig.CountryId == 3)
            {
                return Resources.Resource.Global_HomePageUrl + "/plans/broadband-plans";
            }
            else if (PriceMe.WebConfig.CountryId == 3 && categoryID == 359)
            {
                return Resources.Resource.Global_HomePageUrl + "/plans/all-broadband-plans?type=1";
            }
            else if (PriceMe.WebConfig.CountryId == 3 && categoryID == 360)
            {
                return Resources.Resource.Global_HomePageUrl + "/plans/all-broadband-plans?type=5";
            }
            else if (PriceMe.WebConfig.CountryId == 3 && categoryID == 436)
            {
                return Resources.Resource.Global_HomePageUrl + "/plans/all-broadband-plans?type=4";
            }
            else if (PriceMe.WebConfig.CountryId == 3 && categoryID == 361)
            {
                return Resources.Resource.Global_HomePageUrl + "/plans/all-broadband-plans";
            }
            else if (categoryID > 0)
            {
                Dictionary<string, string> queryParameters = new Dictionary<string, string>();
                queryParameters.Add("c", categoryID.ToString());
                return CatalogUrl(queryParameters);
            }
            else
            {
                if (categoryID == -99)
                {
                    return Resources.Resource.Global_HomePageUrl + "/money/";
                }
                else if (categoryID == -991)
                {
                    return Resources.Resource.Global_HomePageUrl + "/money/credit-cards";
                }
                else if (categoryID == -992)
                {
                    return Resources.Resource.Global_HomePageUrl + "/money/home-loans";
                }
                else if (categoryID == -993)
                {
                    return Resources.Resource.Global_HomePageUrl + "/money/kiwisaver";
                }
                else if (categoryID == -994)
                {
                    return Resources.Resource.Global_HomePageUrl + "/money/savings-accounts";
                }
                else if (categoryID == -995)
                {
                    return Resources.Resource.Global_HomePageUrl + "/money/term-deposits";
                }
                else
                {
                    return Resources.Resource.Global_HomePageUrl;
                }
            }
        }

        private static string SearchUrl(Dictionary<string, string> queryParameters)
        {
            if (!queryParameters.ContainsKey("q"))
            {
                throw new ParameterException("searchParameterError! queryParameters not contains 'q'");
            }

            string url = "/search.aspx?";

            foreach (string key in queryParameters.Keys)
            {
                if (key == "pg" && queryParameters[key] == "1") continue;
                url += key + "=" + HttpUtility.UrlEncode(queryParameters[key]) + "&";
            }

            return url.TrimEnd('&');
        }

        private static string RetailerInfoUrl(Dictionary<string, string> queryParameters)
        {
            if (!queryParameters.ContainsKey("retailerId"))
                throw new ParameterException("RetailerInfoParameterError! queryParameters not contains 'retailerId'");

            int retailerId = int.Parse(queryParameters["retailerId"]);
            string sOut = RetailerInfoUrl(retailerId) + "?";
            foreach (string key in queryParameters.Keys)
            {
                if (key == "pg" && queryParameters[key] == "1") continue;
                if (key == "sortBy" && queryParameters[key] == "0") continue;
                if (key == "tab" || key == "retailerId") continue;
                sOut += (key + "=" + queryParameters[key] + "&");
            }

            sOut = sOut.TrimEnd('&').TrimEnd('?');
            if (queryParameters.ContainsKey("tab")) sOut += ("#" + queryParameters["tab"]);

            return sOut;
        }

        public static string RetailerInfoUrl(int retailerId)
        {
            RetailerCache retailer = RetailerController.GetRetailerDeep(retailerId, PriceMe.WebConfig.CountryId);

            if (retailer == null)
                return string.Empty;

            return RetailerInfoUrl(retailerId, retailer.RetailerName);
        }

        public static string RetailerInfoUrl(int retailerId, string retailerName)
        {
            string typeName = FilterInvalidUrlPathChar(retailerName);

            typeName = typeName.Length > 80 ? typeName.Substring(0, 80) : typeName;
            typeName.Replace("?", "");

            string sOut = "/" + typeName + "/r-" + retailerId + ".aspx";

            return sOut;
        }

        /// <summary>
        /// 从Community/xxx.aspx 页面点击 retailer review 跳转到 retaileInfo 页面时，
        /// 直接显示 StoreRevivws tab
        /// </summary>
        /// <param name="retailerId"></param>
        /// <param name="retailerName"></param>
        /// <param name="tab">要显示的Tab</param>
        /// <returns></returns>
        public static string RetailerInfoUrl(int retailerId, string retailerName, string tab)
        {
            string sOut = RetailerInfoUrl(retailerId, retailerName) + "#" + tab;

            return sOut;
        }

        private static string RetailerReviewUrl(Dictionary<string, string> queryParameters)
        {
            if (!queryParameters.ContainsKey("retailerId"))
                throw new ParameterException("RetailerInfoParameterError! queryParameters not contains 'retailerId'");

            int retailerId = int.Parse(queryParameters["retailerId"]);
            RetailerCache retailer = RetailerController.GetRetailerDeep(retailerId, PriceMe.WebConfig.CountryId);

            string typeName = FilterInvalidUrlPathChar(retailer.RetailerName);
            typeName = typeName.Length > 80 ? typeName.Substring(0, 80) : typeName;
            typeName.Replace("?", "");
            if (typeName.Length > 0)
                typeName += "/";

            string sOut = "RetailerReview/" + typeName + retailerId + ".aspx";

            return sOut;
        }

        private static string RetailerListUrl(Dictionary<string, string> queryParameters)
        {
            if (!queryParameters.ContainsKey("rcId") && !queryParameters.ContainsKey("pg"))
                return "/RetailerList.aspx";

            if (queryParameters.ContainsKey("rcId"))
            {
                int rcId = int.Parse(queryParameters["rcId"]);

                string retailerCategoryName = RetailerController.GetRetailerCategoryName(rcId, WebConfig.CountryId);

                string typeName = FilterInvalidUrlPathChar(retailerCategoryName);
                typeName = typeName.Length > 80 ? typeName.Substring(0, 80) : typeName;
                typeName.Replace("?", "");

                string sOut = string.Empty;

                if (string.IsNullOrEmpty(typeName))
                    sOut = "/RetailerList.aspx";
                else
                {
                    string sortletter = string.Empty;
                    if (queryParameters.ContainsKey("sortletter"))
                        sortletter = queryParameters["sortletter"];
                    if (string.IsNullOrEmpty(sortletter))
                        sOut = "/" + typeName + "-Retailers/rtCatID-" + rcId + ".aspx";
                    else
                        sOut = "/" + typeName + "-Retailers/rtCatID-" + rcId + ",sortletter_" + sortletter + ".aspx";
                    if (queryParameters.ContainsKey("pg") && queryParameters["pg"] != "1")
                    {
                        sOut += "?pg=" + queryParameters["pg"];
                    }
                }
                return sOut;
            }
            else
            {
                string pageinfo = queryParameters["pg"];
                if (pageinfo == "1")
                {
                    return "/RetailerList.aspx";
                }
                else
                {
                    string sOut = "/RetailerList.aspx";
                    if (queryParameters.ContainsKey("sortletter"))
                        sOut += "&sortletter=" + queryParameters["sortletter"];
                    sOut += "&pg=" + pageinfo;
                    return sOut.Replace(".aspx&", ".aspx?");
                }
            }
        }

        public static string GetPrimaryProfileUrl(string value)
        {
            if (value != string.Empty)
                return "/Community/" + value + "-MyPriceMe.aspx";

            return "/Community/MyPriceMe.aspx";
        }

        public static string GetUserProfile(string userProfile)
        {
            userProfile = FilterInvalidUrlPathChar(userProfile);
            string sOut = "/Community/" + userProfile + ".aspx";
            return sOut;
        }

        public static string GetUserProfile(string userProfile, string op)
        {
            userProfile = FilterInvalidUrlPathChar(userProfile);
            string sOut = "/Community/" + userProfile + "-" + op + ".aspx";
            return sOut;
        }

        static Regex duplicateAspxRegex = new Regex("(\\.aspx){2,}$");
        public static string Fixed301Url(string originalUrl)
        {
            System.Text.RegularExpressions.Match match = duplicateAspxRegex.Match(originalUrl);
            if (match.Success)
            {
                return duplicateAspxRegex.Replace(originalUrl, ".aspx").Replace("404.aspx?", "").Replace("error500.aspx?", "");
            }
            return originalUrl;
        }

        public static string GetRealUrl(string originalUrl, bool redirectToMobileTouch)
        {
            string specialUrl = originalUrl.Substring(originalUrl.LastIndexOf('/'));
            if (originalUrl.IndexOf("ThankYou.aspx?", StringComparison.InvariantCultureIgnoreCase) > -1)
            {
                return originalUrl;
            }
            else if (originalUrl.IndexOf("login.aspx?", StringComparison.InvariantCultureIgnoreCase) > -1 || originalUrl.IndexOf("Register2.aspx?", StringComparison.InvariantCultureIgnoreCase) > -1)
            {
                return originalUrl;
            }
            else if (originalUrl.IndexOf("changepassword.aspx?", StringComparison.InvariantCultureIgnoreCase) > -1 || originalUrl.IndexOf("Register2.aspx?", StringComparison.InvariantCultureIgnoreCase) > -1)
            {
                return originalUrl;
            }
            else if (originalUrl.IndexOf("/error500.aspx?", StringComparison.InvariantCultureIgnoreCase) > -1)
            {
                return originalUrl;
            }
            else if (originalUrl.IndexOf("/404.aspx?", StringComparison.InvariantCultureIgnoreCase) > -1)
            {
                return originalUrl;
            }
            else if (originalUrl.IndexOf("/Register.aspx?", StringComparison.InvariantCultureIgnoreCase) > -1)
            {
                return originalUrl;
            }
            else if (originalUrl.IndexOf("/Register2.aspx?", StringComparison.InvariantCultureIgnoreCase) > -1)
            {
                return originalUrl;
            }
            else if (originalUrl.IndexOf("/logout.aspx?", StringComparison.InvariantCultureIgnoreCase) > -1)
            {
                return originalUrl;
            }
            else if (originalUrl.IndexOf("ProductReviewSuccess.aspx?", StringComparison.InvariantCultureIgnoreCase) > -1)
                return originalUrl;
            else if (specialUrl.IndexOf("/rp-", StringComparison.InvariantCultureIgnoreCase) > -1)
            {
                return GetRetailerProductUrl(originalUrl, redirectToMobileTouch);
            }
            else if (specialUrl.IndexOf("/p-", StringComparison.InvariantCultureIgnoreCase) > -1)
            {
                return GetProductUrl(originalUrl, redirectToMobileTouch);
            }
            else if (originalUrl.IndexOf("_c-", StringComparison.InvariantCultureIgnoreCase) > -1 || originalUrl.IndexOf(",c-", StringComparison.InvariantCultureIgnoreCase) > -1)
            {
                return GetCatalogPageUrl(originalUrl);
            }
            else if (specialUrl.IndexOf("/c-", StringComparison.InvariantCultureIgnoreCase) > -1)
            {
                Regex regex = new Regex(@"/c-(?<cid>\d+)");
                Match match = regex.Match(originalUrl);
                if (match.Success)
                {
                    if (redirectToMobileTouch)
                    {
                        return Resources.Resource.MobileTouchSiteURL + "/NewCatalog.aspx?c=" + match.Groups["cid"];
                    }
                    else
                    {
                        string newQueryString = "/NewCatalog.aspx?c=" + match.Groups["cid"];
                        int index = originalUrl.IndexOf("?");
                        if (index > 0)
                            newQueryString += ("&" + originalUrl.Substring(index + 1));
                        index = originalUrl.IndexOf("#");
                        if (index > 0)
                            newQueryString += ("#" + originalUrl.Substring(index + 1));
                        return newQueryString;
                    }
                }
            }
            else if (originalUrl.IndexOf("/search/", StringComparison.InvariantCultureIgnoreCase) > -1)
            {
                return GetSearchPageUrl(originalUrl);
            }
            else if (originalUrl.IndexOf("/pe-", StringComparison.InvariantCultureIgnoreCase) > -1)
            {
                return GetExperReviewPageUrl(originalUrl, redirectToMobileTouch);
            }
            //else if (originalUrl.IndexOf("-review/pe-", StringComparison.InvariantCultureIgnoreCase) > -1)
            //{
            //    return GetExperReviewPageUrl(originalUrl, redirectToMobileTouch);
            //}
            else if (specialUrl.IndexOf("/pu-", StringComparison.InvariantCultureIgnoreCase) > -1)
            {
                Regex regex = new Regex(@"/(?<name>[^/]*)?-review/pu-(?<pid>\d+).aspx", RegexOptions.IgnoreCase);
                if (PriceMe.WebConfig.CountryId == 41)
                    regex = new Regex(@"/pu-(?<pid>\d+).aspx", RegexOptions.IgnoreCase);

                Match match = regex.Match(originalUrl);

                if (match.Success)
                {
                    string pid = match.Groups["pid"].ToString();
                    string name = match.Groups["name"].ToString();

                    if (redirectToMobileTouch)
                    {
                        return Resources.Resource.MobileTouchSiteURL + "/ProductReview.aspx?pid=" + pid;
                    }
                    else
                    {
                        return "/ProductReview.aspx?pid=" + pid + "&name=" + name;
                    }
                }
            }
            else if (specialUrl.IndexOf("/pm-", StringComparison.InvariantCultureIgnoreCase) > -1)
            {
                Regex regex = new Regex(@"/(?<name>[^/]*)?-map/pm-(?<pid>\d+).aspx", RegexOptions.IgnoreCase);
                if (PriceMe.WebConfig.CountryId == 41)
                    regex = new Regex(@"/pm-(?<pid>\d+).aspx", RegexOptions.IgnoreCase);
                Match match = regex.Match(originalUrl);

                if (match.Success)
                {
                    string pid = match.Groups["pid"].ToString();
                    string name = match.Groups["name"].ToString();

                    if (redirectToMobileTouch)
                    {
                        return Resources.Resource.MobileTouchSiteURL + "/ProductMap.aspx?pid=" + pid;
                    }
                    else
                    {
                        return "/ProductMap.aspx?pid=" + pid + "&name=" + name;
                    }
                }
            }
            else if (specialUrl.IndexOf("/pd-", StringComparison.InvariantCultureIgnoreCase) > -1)
            {
                Regex regex = new Regex(@"/(?<name>[^/]*)?-specs/pd-(?<pid>\d+).aspx", RegexOptions.IgnoreCase);
                if (PriceMe.WebConfig.CountryId == 41)
                    regex = new Regex(@"/pd-(?<pid>\d+).aspx", RegexOptions.IgnoreCase);
                Match match = regex.Match(originalUrl);

                if (match.Success)
                {
                    string pid = match.Groups["pid"].ToString();
                    string name = match.Groups["name"].ToString();

                    if (redirectToMobileTouch)
                    {
                        return Resources.Resource.MobileTouchSiteURL + "/ProductDescription.aspx?pid=" + pid;
                    }
                    else
                    {
                        return "/ProductDescription.aspx?pid=" + pid + "&name=" + name;
                    }
                }
            }
            else if (originalUrl.IndexOf("-Reviews/rec-", StringComparison.InvariantCultureIgnoreCase) > -1)
            {
                Regex regex = new Regex(@"(/(?<name>[^/]*))?-Reviews/rec-(?<cid>\d+)", RegexOptions.IgnoreCase);
                Match match = regex.Match(originalUrl);

                if (match.Success)
                {
                    string cid = match.Groups["cid"].ToString();
                    string name = match.Groups["name"].ToString();

                    return "/Reviews.aspx?cid=" + cid + "&name=" + name;
                }
            }
            else if (specialUrl.IndexOf("/rc-", StringComparison.InvariantCultureIgnoreCase) > -1)
            {
                return GetRetailerCategoryPageUrl(originalUrl);
            }
            else if (specialUrl.IndexOf("/r-", StringComparison.InvariantCultureIgnoreCase) > -1)
            {
                Regex regex = new Regex(@"(/(?<name>[^/]*))?/r-(?<rid>\d+)", RegexOptions.IgnoreCase);

                Match match = regex.Match(originalUrl);

                if (match.Success)
                {
                    string rid = match.Groups["rid"].ToString();
                    string name = string.Empty;
                    string url = string.Empty;
                    name = match.Groups["name"].ToString();
                    url = string.Format("/retailerinfo.aspx?rid={0}&name={1}", rid, name);

                    regex = new Regex(@"pg=(?<pg>\d+)", RegexOptions.IgnoreCase);
                    match = regex.Match(originalUrl);

                    if (match.Success)
                    {
                        string pg = match.Groups["pg"].ToString();
                        url += string.Format("&pg={0}", pg);
                    }
                    regex = new Regex(@"sortBy=(?<sortBy>\d+)", RegexOptions.IgnoreCase);
                    match = regex.Match(originalUrl);

                    if (match.Success)
                    {
                        string pg = match.Groups["sortBy"].ToString();
                        url += string.Format("&sortBy={0}", pg);
                    }

                    regex = new Regex(@"rvt=(?<rvt>[\s\S]*)", RegexOptions.IgnoreCase);
                    match = regex.Match(originalUrl);

                    if (match.Success)
                    {
                        string rvt = match.Groups["rvt"].ToString();
                        if (rvt.Contains("&"))
                            rvt = rvt.Split('&')[0];
                        url += string.Format("&rvt={0}", rvt);
                    }

                    regex = new Regex(@"tab=(?<tab>[\s\S]*)", RegexOptions.IgnoreCase);
                    match = regex.Match(originalUrl);

                    if (match.Success)
                    {
                        string tab = match.Groups["tab"].ToString();
                        url += string.Format("&tab={0}", tab);
                    }
                    return url;
                }
                else return "/RetailerList.aspx";
            }
            else if (originalUrl.IndexOf("/brand_", StringComparison.InvariantCultureIgnoreCase) > -1)
            {
                return GetBrandPageUrl(originalUrl);
            }
            else if (originalUrl.IndexOf("/MyWishList/", StringComparison.InvariantCultureIgnoreCase) > -1)
            {
                string url = string.Empty;

                if (originalUrl.Contains("n_"))
                {
                    Regex regex = new Regex(@"/MyWishList/n_(?<name>[^/]*)?,s_(?<sid>[^/]*)?.aspx", RegexOptions.IgnoreCase);
                    Match match = regex.Match(originalUrl);
                    if (match.Success)
                    {
                        string name = match.Groups["name"].ToString();
                        string sid = match.Groups["sid"].ToString();

                        url = "/MyWishList.aspx?sid=" + sid + "&name=" + name;
                    }
                }
                else
                {
                    Regex regex = new Regex(@"/MyWishList/s_(?<sid>[^/]*)?.aspx", RegexOptions.IgnoreCase);
                    Match match = regex.Match(originalUrl);
                    if (match.Success)
                    {
                        string sid = match.Groups["sid"].ToString();
                        url = "/MyWishList.aspx?sid=" + sid;
                    }
                }

                return url;
            }
            else if (originalUrl.IndexOf("/forum-", StringComparison.InvariantCultureIgnoreCase) > -1)
            {
                return GetForumTopicListUrl(originalUrl);
            }
            else if (originalUrl.IndexOf("/topic-", StringComparison.InvariantCultureIgnoreCase) > -1)
            {
                return GetForumTopicUrl(originalUrl);
            }
            else if (originalUrl.IndexOf("/newtopic-", StringComparison.InvariantCultureIgnoreCase) > -1)
            {
                return GetForumNewTopicUrl(originalUrl);
            }
            else if (originalUrl.IndexOf("/forum/report", StringComparison.InvariantCultureIgnoreCase) > -1)
            {
                return GetForumReportUrl(originalUrl);
            }
            else if (originalUrl.IndexOf("/forum", StringComparison.InvariantCultureIgnoreCase) > -1)
            {
                return GetForumUrl(originalUrl);
            }
            else if (originalUrl.IndexOf("/retailerinfo/", StringComparison.InvariantCultureIgnoreCase) > -1)
            {
                Regex regex = new Regex(@"/retailerinfo/(?<name>[^/]*)?/(?<rid>\d+).", RegexOptions.IgnoreCase);
                Match match = regex.Match(originalUrl);
                if (match.Success)
                {
                    string rid = match.Groups["rid"].ToString();
                    string name = match.Groups["name"].ToString();

                    return "/RetailerInfo.aspx?rid=" + rid + "&name=" + name + "&type=old";
                }
            }
            else if (originalUrl.IndexOf("/RetailerList/pg_", StringComparison.InvariantCultureIgnoreCase) > -1)
            {
                string paramValue = System.IO.Path.GetFileNameWithoutExtension(originalUrl);
                if (paramValue.Contains("sortletter"))
                {
                    string[] infos = paramValue.Split(',');
                    string pg = infos[0].Replace("pg_", "");
                    string sort = infos[1].Replace("sortletter_", "");

                    return "/RetailerList.aspx?pg=" + pg + "&sortletter=" + sort;
                }
                else
                    paramValue = paramValue.Replace("pg_", "");
                return "/RetailerList.aspx?pg=" + paramValue;
            }
            else if (originalUrl.IndexOf("retailers/rtcatid", StringComparison.InvariantCultureIgnoreCase) > -1)
            {
                string paramValue = System.IO.Path.GetFileNameWithoutExtension(originalUrl);
                if (paramValue.Contains("pg"))
                {
                    string[] infos = paramValue.Split(',');
                    string cid = infos[0].Replace("rtCatID-", "");
                    System.Text.StringBuilder url = new System.Text.StringBuilder("/RetailerList.aspx?rtCatID=" + cid);
                    if (infos.Length > 2)
                    {
                        string sort = infos[1].Replace("sortletter_", "");
                        string pg = infos[2].Replace("pg_", "");
                        url.Append("&sortletter=" + sort + "&pg=" + pg);
                    }
                    else
                    {
                        string pg = infos[1].Replace("pg_", "");
                        url.Append("&pg=" + pg);
                    }
                    return url.ToString();
                }
                else if (paramValue.Contains("sortletter"))
                {
                    string[] infos = paramValue.Split(',');
                    string cid = infos[0].Replace("rtCatID-", "");
                    string sort = infos[1].Replace("sortletter_", "");

                    return "/RetailerList.aspx?rtCatID=" + cid + "&sortletter=" + sort;
                }
                paramValue = paramValue.Replace("rtCatID-", "");
                return "/RetailerList.aspx?rtCatID=" + paramValue;
            }
            else if (originalUrl.IndexOf("reportretailer.aspx", StringComparison.InvariantCultureIgnoreCase) > -1)
                return originalUrl;
            else if (originalUrl.IndexOf("retailer.aspx", StringComparison.InvariantCultureIgnoreCase) > -1)
            {
                return "/RetailerList.aspx";
            }
            else if (originalUrl.IndexOf("community/allrrecentpricedrop.aspx", StringComparison.InvariantCultureIgnoreCase) > -1)
            {
                return originalUrl;
            }
            else if (originalUrl.IndexOf("community/", StringComparison.InvariantCultureIgnoreCase) > -1)
            {
                if (!originalUrl.Contains("?") || HttpContext.Current.Request.HttpMethod.Equals("post", StringComparison.InvariantCultureIgnoreCase))
                {
                    originalUrl = originalUrl.ToLower();
                    string realPageTo = string.Empty;
                    if (originalUrl.Contains("mypriceme"))
                        realPageTo = "/Community/PrimaryProfile.aspx";
                    else
                        realPageTo = "/Community/UserProfile.aspx";

                    string paramValue = System.IO.Path.GetFileNameWithoutExtension(originalUrl).ToLower();

                    if (paramValue == "mypriceme")
                        return realPageTo;
                    else if (paramValue == "list" || paramValue == "aboutme" || paramValue == "personaldetails"
                        || paramValue == "changeemail" || paramValue == "changepassword"
                        || paramValue == "uploadpicture" || paramValue == "preview" || paramValue == "myaccount")
                        return originalUrl;
                    else if (paramValue.Contains("-"))
                    {
                        string[] paramInfo = paramValue.Split('-');
                        if (paramInfo[0].ToLower() == "favourites")
                        {
                            return "/Community/favourites.aspx";
                        }
                        else
                        {
                            string url = realPageTo;
                            if (originalUrl.Contains("mypriceme") && paramInfo.Length == 2)
                                url += "?op=" + paramInfo[0] + "&author=" + paramInfo[1].Replace("#8254;", "-").Replace("+", " ");
                            else if (paramInfo.Length == 2)
                                url += "?op=" + paramInfo[1] + "&author=" + paramInfo[0].Replace("#8254;", "-").Replace("+", " ");
                            else if (paramInfo.Length == 3)
                                url += "?op=" + paramInfo[1] + "&author=" + paramInfo[0].Replace("#8254;", "-").Replace("+", " ") + "&reviewid=" + paramInfo[2];
                            return url;
                        }
                    }
                    else
                    {
                        return "/Community/UserProfile.aspx?author=" + paramValue.Replace("#8254;", "-").Replace("+", " ");
                    }
                }
            }
            else if (originalUrl.IndexOf("/retailerreview/", StringComparison.InvariantCultureIgnoreCase) > -1)
            {
                string paramValue = System.IO.Path.GetFileNameWithoutExtension(originalUrl);
                return "/members/retailerreview.aspx?retailerid=" + paramValue;
            }
            else if (originalUrl.IndexOf("/retailerfullreview/", StringComparison.InvariantCultureIgnoreCase) > -1)
            {
                string paramValue = System.IO.Path.GetFileNameWithoutExtension(originalUrl);
                return "members/retailerfullreview.aspx?rtfreviewid=" + paramValue;
            }
            else if (originalUrl.IndexOf("/MostPopular/p_cid-", StringComparison.InvariantCultureIgnoreCase) > -1)
            {
                Regex regex = new Regex(@"/p_cid-(?<cid>\d+)");
                Match match = regex.Match(originalUrl);
                if (match.Success)
                {
                    return "/MostPopular.aspx?cid=" + match.Groups["cid"];
                }
            }
            else if (originalUrl.IndexOf("/bg-", StringComparison.InvariantCultureIgnoreCase) > -1 || originalUrl.IndexOf(",wn-", StringComparison.InvariantCultureIgnoreCase) > -1)
            {
                string paramValue = System.IO.Path.GetFileNameWithoutExtension(originalUrl);
                Regex regex = new Regex(@"bg-(?<bgid>\d+)");//Regex regex = new Regex(@"bg-(?<bgid>\d+)-(?<cid>\d+)");
                Match match = regex.Match(paramValue);
                if (match.Success)
                {
                    //string cid = match.Groups["cid"].Value;
                    string bgid = match.Groups["bgid"].Value;
                    return "/BuyingGuides.aspx?bgid=" + bgid;
                    //return "/BuyingGuides.aspx?cid=" + cid+"&bgid="+bgid;
                }
            }
            else if (originalUrl.IndexOf("/allbrands/", StringComparison.InvariantCultureIgnoreCase) > -1 || originalUrl.IndexOf(",wn-", StringComparison.InvariantCultureIgnoreCase) > -1)
            {
                Regex slRegex = new Regex(@"/p_sl-(?<sl>[0-9a-zA-Z]),?");
                Regex pgRegex = new Regex(@",?pg-(?<pg>\d+)");

                Match slMatch = slRegex.Match(originalUrl);
                Match pgMatch = pgRegex.Match(originalUrl);
                string rUrl = "/AllBrands.aspx?";

                if (slMatch.Success)
                {
                    rUrl += "sortletter=" + slMatch.Groups["sl"].Value;
                }
                if (slMatch.Success && pgMatch.Success)
                {
                    rUrl += "&pg=" + pgMatch.Groups["pg"].Value;
                }
                else if (pgMatch.Success)
                {
                    rUrl += "pg=" + pgMatch.Groups["pg"].Value;
                }

                return rUrl;
            }
            else if (originalUrl.IndexOf("/lists/", StringComparison.InvariantCultureIgnoreCase) > -1 || originalUrl.IndexOf(",wn-", StringComparison.InvariantCultureIgnoreCase) > -1)
            {
                Regex pgRegex = new Regex(@",?pg-(?<pg>\d+)");

                Match pgMatch = pgRegex.Match(originalUrl);
                string rUrl = "/lists.aspx?";
                if (pgMatch.Success)
                {
                    rUrl += "pg=" + pgMatch.Groups["pg"].Value;
                }

                return rUrl;
            }
            else if (originalUrl.IndexOf("/productnotfound/", StringComparison.InvariantCultureIgnoreCase) > -1)
            {
                HttpContext.Current.Response.Status = "404 not found";
                HttpContext.Current.Response.Redirect("/404.aspx", true);
                HttpContext.Current.Response.End();
                return originalUrl;
            }
            else if (originalUrl == "/")
            {
                originalUrl = "/default.aspx";
            }
            return originalUrl;
        }

        private static string GetSearchPageUrl(string originalUrl)
        {
            Regex regex = new Regex(@"q\-(?<kw>[^\\.^,]*)");
            Match match = regex.Match(originalUrl);
            if (match.Success)
            {
                string kw = match.Groups["kw"].Value;
                string url = "/search.aspx?q=" + kw;
                return url;
            }
            return originalUrl;
        }

        private static string GetBrandPageUrl(string originalUrl)
        {
            Regex reg = new Regex(@"/brand_(?<mid>\d+)\.aspx", RegexOptions.IgnoreCase);
            Match match = reg.Match(originalUrl);
            if (match.Success)
            {
                string brandURL = "/brand.aspx?mid=" + match.Groups["mid"].Value + "&cachefix=1";
                return brandURL;
            }
            return originalUrl;
        }

        private static string GetCatalogPageUrl(string originalUrl)
        {
            Regex regex = new Regex(@"/p_(?<queryString>[^\.]+)\.aspx");
            Match match = regex.Match(originalUrl);
            if (match.Success)
            {
                string queryString = match.Groups["queryString"].Value;
                string newQueryString = GetQueryString(queryString);
                int index = originalUrl.IndexOf("?");
                if (index > 0)
                    newQueryString += ("&" + originalUrl.Substring(index + 1));
                string url = "/catalog.aspx?" + newQueryString.Replace("&rid=", "&pcsid=");

                index = originalUrl.IndexOf("#");
                if (index > 0)
                    newQueryString += ("#" + originalUrl.Substring(index + 1));

                return url;
            }

            return originalUrl;
        }

        private static string GetRetailerCategoryPageUrl(string originalUrl)
        {
            Regex regex = new Regex(@"/(?<name>[^/]*)?/rc-(?<rid>\d+)_(?<cid>\d+),pg-(?<pg>\d+)\.aspx", RegexOptions.IgnoreCase);
            Match match = regex.Match(originalUrl);
            if (match.Success)
            {
                string rid = match.Groups["rid"].Value;
                string cid = match.Groups["cid"].Value;
                string name = match.Groups["name"].Value;
                string pg = match.Groups["pg"].Value;
                string url = "/RetailerCategory.aspx?rid=" + rid + "&cid=" + cid + "&name=" + name + "&pg=" + pg;
                return url;
            }
            else
            {
                regex = new Regex(@"/(?<name>[^/]*)?/rc-(?<rid>\d+)_(?<cid>\d+)\.aspx", RegexOptions.IgnoreCase);
                match = regex.Match(originalUrl);
                if (match.Success)
                {
                    string rid = match.Groups["rid"].Value;
                    string cid = match.Groups["cid"].Value;
                    string name = match.Groups["name"].Value;
                    string url = "/RetailerCategory.aspx?rid=" + rid + "&cid=" + cid + "&name=" + name;
                    return url;
                }
            }

            return originalUrl;
        }

        private static string GetExperReviewPageUrl(string originalUrl, bool redirectToMobileTouch)
        {
            Regex regex = new Regex(@"/(?<name>[^/]*)?-review/pe-(?<pid>\d+).aspx", RegexOptions.IgnoreCase);
            if (PriceMe.WebConfig.CountryId == 41)
                regex = new Regex(@"/pe-(?<pid>\d+).aspx", RegexOptions.IgnoreCase);
            Match match = regex.Match(originalUrl);

            if (match.Success)
            {
                string pid = match.Groups["pid"].Value;
                string name = match.Groups["name"].ToString();

                string pg = "", sortType = "", stars = "";
                if (originalUrl.Contains("?"))
                {
                    string paramStr = originalUrl.Substring(originalUrl.IndexOf("?") + 1);
                    string[] paras = paramStr.Split('&');
                    foreach (string p in paras)
                    {
                        if (p.Contains("pg=")) pg = p.Substring(p.IndexOf("=") + 1);
                        if (p.Contains("stars=")) stars = p.Substring(p.IndexOf("=") + 1);
                        if (p.Contains("sb=")) sortType = p.Substring(p.IndexOf("=") + 1);
                    }
                }
                string url = "";
                if (redirectToMobileTouch)
                {
                    url = Resources.Resource.MobileTouchSiteURL + "/ProductReview.aspx?pid=" + pid;
                }
                else
                {
                    url = "/ExpertReview.aspx?pid=" + pid + "&name=" + name;
                }
                if (pg != "") url += "&pg=" + pg;
                if (stars != "") url += "&stars=" + stars;
                if (sortType != "") url += "&sb=" + sortType;
                return url;
            }

            return originalUrl;
        }

        private static string GetUserReviewPageUrl(string originalUrl)
        {
            Regex regex = new Regex(@"/(?<name>[^/]*)?-review/pu-(?<pid>\d+).aspx", RegexOptions.IgnoreCase);
            Match match = regex.Match(originalUrl);

            if (match.Success)
            {
                string pid = match.Groups["pid"].Value;
                string name = match.Groups["name"].Value;
                string url = "/ExpertReview.aspx?pid=" + pid + "&name=" + name;
                return url;
            }

            return originalUrl;
        }

        //private static string GetProductDescriptionPageUrl(string originalUrl)
        //{
        //    Regex regex = new Regex(@"/(?<name>[^/]*)?-review/pe-(?<pid>\d+).aspx", RegexOptions.IgnoreCase);
        //    Match match = regex.Match(originalUrl);

        //    if (match.Success)
        //    {
        //        string pid = match.Groups["pid"].Value;
        //        string name = match.Groups["name"].Value;
        //        string url = "/ExpertReview.aspx?pid=" + pid + "&name=" + name;
        //        return url;
        //    }

        //    return originalUrl;
        //}

        private static string GetQueryString(string queryString)
        {
            string[] sPs = queryString.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            string sParams = "";
            foreach (string sp in sPs)
            {
                if (sp.IndexOf('-') > -1)
                {
                    string rps = sp.Substring(0, sp.IndexOf('-'));
                    sParams += sp.Replace(rps + "-", rps + "=") + "&";
                }
            }

            return sParams.TrimEnd('&');
        }

        private static string CatalogUrl(Dictionary<string, string> queryParameters)
        {
            if (!queryParameters.ContainsKey("c"))
            {
                throw new ParameterException("catalogParameterError! queryParameters not contains 'c'");
            }

            int cid = int.Parse(queryParameters["c"]);
            PriceMeCache.CategoryCache category = CategoryController.GetCategoryByCategoryID(cid, PriceMe.WebConfig.CountryId);

            string url = "";
            if (category != null)
            {
                url = GetSEOUrl(queryParameters, category);
            }
            //if (queryParameters.ContainsKey("pcsid"))
            //{
            //    url += "?pcsid=" + queryParameters["pcsid"];
            //}
            return url;
        }

        private static string GetSEOUrl(Dictionary<string, string> queryParameters, CategoryCache category)
        {
            string url;
            if (queryParameters.Count == 1 || (category.IsSiteMap) || (category.IsSiteMapDetail))
            {
                url = "/" + FilterInvalidNameChar(category.CategoryNameEN) + "/c-" + category.CategoryID + ".aspx";
            }
            else
            {
                string queryString = GetNewUrlParameterString(queryParameters);
                if (!string.IsNullOrEmpty(queryString))
                {
                    queryString = "?" + queryString;
                }

                url = "/" + FilterInvalidNameChar(category.CategoryNameEN) + "/c-" + category.CategoryID + ".aspx" + queryString;
            }
            return url;
        }

        private static string GetNewUrlParameterString(Dictionary<string, string> queryParameters)
        {
            Dictionary<string, string> oldParameters = new Dictionary<string, string>(queryParameters);
            Dictionary<string, string> newParameters = new Dictionary<string, string>();

            if (queryParameters.ContainsKey("c"))
            {
                newParameters.Add("c", queryParameters["c"]);
                oldParameters.Remove("c");
            }

            if (queryParameters.ContainsKey("m"))
            {
                newParameters.Add("m", queryParameters["m"]);
                oldParameters.Remove("m");
            }

            if (queryParameters.ContainsKey("pr"))
            {
                newParameters.Add("pr", queryParameters["pr"]);
                oldParameters.Remove("pr");
            }

            if (queryParameters.ContainsKey("avs"))
            {
                newParameters.Add("avs", queryParameters["avs"]);
                oldParameters.Remove("avs");
            }

            if (queryParameters.ContainsKey("avsr"))
            {
                newParameters.Add("avsr", queryParameters["avsr"]);
                oldParameters.Remove("avsr");
            }

            if (queryParameters.ContainsKey("rid"))
            {
                newParameters.Add("rid", queryParameters["rid"]);
                oldParameters.Remove("rid");
            }

            if (queryParameters.ContainsKey("samp"))
            {
                newParameters.Add("samp", queryParameters["samp"]);
                oldParameters.Remove("samp");
            }

            if (oldParameters.Count > 0)
            {
                List<string> otherParameter = new List<string>();
                foreach (string key in oldParameters.Keys)
                {
                    otherParameter.Add(key);
                }
                otherParameter.Sort();
                foreach (string key in otherParameter)
                {
                    newParameters.Add(key, queryParameters[key]);
                }
            }

            string queryString = "";
            foreach (string pName in newParameters.Keys)
            {
                if (pName == "pg" && (queryParameters[pName] == "1" || queryParameters[pName] == "0"))
                {
                    continue;
                }

                if (pName == "c")
                {
                    continue;
                }

                //if (pName == "v" && queryParameters[pName] == "Grid")
                //{
                //    continue;
                //}

                if (pName != "samp" && pName != "avsr")
                {
                    queryString += pName + "=" + FilterInvalidUrlPathChar(queryParameters[pName]) + "&";
                }
                else
                {
                    queryString += pName + "=" + queryParameters[pName] + "&";
                }
            }
            return queryString.TrimEnd('&');
        }

        public static AttributeParameterCollection GetAttributeParameterCollectionAndSortAttributesParamters(Dictionary<string, string> queryParameters, PriceMeCache.CategoryCache category)
        {
            AttributeParameterCollection attributeParameterCollection = new AttributeParameterCollection();
            if (queryParameters.ContainsKey("avs"))
            {
                string attributeValuesIDString = queryParameters["avs"];
                string[] avids = attributeValuesIDString.Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
                int aid;

                List<string> avidsString = new List<string>();
                avidsString.AddRange(avids);
                avidsString.Sort(new AttributeStringSort());

                queryParameters["avs"] = GetAttributeValueString(avidsString);

                foreach (string avid in avids)
                {
                    string avidString = avid;
                    if (avidString.ToLower().EndsWith("r"))
                    {
                        avidString = avid.Substring(0, avidString.Length - 1);
                        int.TryParse(avidString, out aid);
                        if (aid != 0)
                        {
                            AttributeParameter attributeParameter = GetAttributeParameterByAttributeRangeIDAndCategoryID(aid, category.CategoryID);
                            if (attributeParameter != null)
                            {
                                attributeParameterCollection.Add(attributeParameter);
                            }
                        }
                    }
                    else
                    {
                        int.TryParse(avidString, out aid);
                        if (aid != 0)
                        {
                            AttributeParameter attributeParameter = GetAttributeParameterByAttributeValueIDAndCategoryID(aid, category.CategoryID);
                            if (attributeParameter != null)
                            {
                                attributeParameterCollection.Add(attributeParameter);
                            }
                        }
                    }
                }
            }
            return attributeParameterCollection;
        }

        private static string GetAttributeValueString(List<string> avidsString)
        {
            string str = "";
            foreach (string adav in avidsString)
            {
                str += adav + "-";
            }
            return str.TrimEnd('-');
        }

        private static string GetCatalogUrlDescriptionString(Dictionary<string, string> queryParameters, AttributeParameterCollection urlAttributeParameterList, PriceMeCache.CategoryCache category)
        {
            string catalogUrlDescriptionString = "";

            if (queryParameters.ContainsKey("m") && queryParameters["m"].Length > 0)
            {
                int mid = int.Parse(queryParameters["m"]);
                ManufacturerInfo manufacturer = ManufacturerController.GetManufacturerByID(mid, PriceMe.WebConfig.CountryId);
                if (manufacturer != null)
                {
                    //throw new Exception("catalogParameterError! mid : " + mid + " not exist!");
                    catalogUrlDescriptionString = manufacturer.ManufacturerName + " ";
                }
            }

            catalogUrlDescriptionString += category.CategoryNameEN + " ";

            if (urlAttributeParameterList.Count > 0)
            {
                catalogUrlDescriptionString += urlAttributeParameterList.ToURLString();
            }

            catalogUrlDescriptionString = FilterInvalidUrlPathChar(catalogUrlDescriptionString.TrimEnd(' '));

            if (catalogUrlDescriptionString.Length > 100)
            {
                catalogUrlDescriptionString = catalogUrlDescriptionString.Substring(0, 100);
            }
            return catalogUrlDescriptionString;
        }

        public static AttributeParameter GetAttributeParameterByAttributeValueIDAndCategoryID(int aid, int cid)
        {
            AttributeTitleCache productDescriptorTitle = AttributesController.GetAttributeTitleByVauleID(aid);
            if (productDescriptorTitle != null)
            {
                AttributeValueCache attributeValue = AttributesController.GetAttributeValueByID(aid);
                if (attributeValue != null)
                {
                    string key = cid + "," + productDescriptorTitle.TypeID;
                    CategoryAttributeTitleMapCache categoryAttributeTitleMap = AttributesController.GetCategoryAttributeTitleMapByKey(key);
                    if (categoryAttributeTitleMap != null)
                    {
                        AttributeParameter attributeParameter = new AttributeParameter();
                        attributeParameter.ListOrder = categoryAttributeTitleMap.AttributeOrder;
                        attributeParameter.AttributeName = productDescriptorTitle.Title;
                        attributeParameter.AttributeValue = attributeValue.Value + (productDescriptorTitle.Unit == null ? "" : productDescriptorTitle.Unit.Trim());
                        return attributeParameter;
                    }
                }
            }
            return null;
        }

        public static AttributeParameter GetAttributeParameterByAttributeRangeIDAndCategoryID(int arid, int cid)
        {
            var attributeValueRange = AttributesController.GetAttributeValueRangeByID(arid);
            if (attributeValueRange != null)
            {
                AttributeTitleCache productDescriptorTitle = AttributesController.GetAttributeTitleByID(attributeValueRange.AttributeTitleID);
                string key = cid + "," + productDescriptorTitle.TypeID;

                CategoryAttributeTitleMapCache categoryAttributeTitleMap = AttributesController.GetCategoryAttributeTitleMapByKey(key);
                if (categoryAttributeTitleMap != null)
                {
                    AttributeParameter attributeParameter = new AttributeParameter();
                    attributeParameter.ListOrder = categoryAttributeTitleMap.AttributeOrder;
                    attributeParameter.AttributeName = productDescriptorTitle.Title;
                    attributeParameter.AttributeValue = AttributesController.GetAttributeValueString(attributeValueRange, productDescriptorTitle.Unit);
                    return attributeParameter;
                }
            }
            return null;
        }

        /// <summary>
        /// 得到把参数排序后的结果
        /// </summary>
        /// <param name="queryParameters"></param>
        /// <returns></returns>
        private static string GetUrlParameterString(Dictionary<string, string> queryParameters)
        {
            Dictionary<string, string> oldParameters = new Dictionary<string, string>(queryParameters);
            Dictionary<string, string> newParameters = new Dictionary<string, string>();

            if (queryParameters.ContainsKey("c"))
            {
                newParameters.Add("c", queryParameters["c"]);
                oldParameters.Remove("c");
            }

            if (queryParameters.ContainsKey("m"))
            {
                newParameters.Add("m", queryParameters["m"]);
                oldParameters.Remove("m");
            }

            if (queryParameters.ContainsKey("pr"))
            {
                newParameters.Add("pr", queryParameters["pr"]);
                oldParameters.Remove("pr");
            }

            if (queryParameters.ContainsKey("avs"))
            {
                newParameters.Add("avs", queryParameters["avs"]);
                oldParameters.Remove("avs");
            }

            if (queryParameters.ContainsKey("rid"))
            {
                newParameters.Add("rid", queryParameters["rid"]);
                oldParameters.Remove("rid");
            }

            if (queryParameters.ContainsKey("samp"))
            {
                newParameters.Add("samp", queryParameters["samp"]);
                oldParameters.Remove("samp");
            }

            if (oldParameters.Count > 0)
            {
                List<string> otherParameter = new List<string>();
                foreach (string key in oldParameters.Keys)
                {
                    otherParameter.Add(key);
                }
                otherParameter.Sort();
                foreach (string key in otherParameter)
                {
                    newParameters.Add(key, queryParameters[key]);
                }
            }

            string queryString = "";
            foreach (string pName in newParameters.Keys)
            {
                if (pName == "pg" && (queryParameters[pName] == "1" || queryParameters[pName] == "0"))
                {
                    continue;
                }
                if (pName == "pcsid")
                {
                    continue;
                }

                if (pName != "samp")
                {
                    queryString += pName + "-" + FilterInvalidUrlPathChar(queryParameters[pName]) + ",";
                }
                else
                {
                    queryString += pName + "-" + queryParameters[pName] + ",";
                }
            }
            return queryString.TrimEnd(',');
        }

        public static string FilterInvalidUrlPathChar(string sourceString)
        {

            sourceString = illegalReg.Replace(sourceString, "-");
            sourceString = illegalReg2.Replace(sourceString, "-");
            //sourceString = illegalReg3.Replace(sourceString, "");

            return HttpUtility.UrlEncode(sourceString);
        }

        public static string FilterInvalidNameChar(string name)
        {
            name = illegalReg1.Replace(name, "-");
            name = illegalReg2.Replace(name, "-");
            name = illegalReg3.Replace(name, "");

            return name;
        }

        public static Dictionary<string, string> GetCatalogParameters(int categoryID, List<int> manufeaturerIDs, string priceRange, List<int> selectedAttributeIDs, List<int> selectedAttributeRangeIDs, string sortBy, List<int> retailerIDs, string view, string searchWithin, string displayAllManufeaturerIDString,
            Dictionary<int, string> selectedAttrRangeValues, List<int> selectedAttributeGroupIDs, string daysRangeString, string onsaleonly)
        {
            Dictionary<string, string> ps = GetCatalogParameters(categoryID, manufeaturerIDs, priceRange, selectedAttributeIDs, selectedAttributeRangeIDs, sortBy, retailerIDs, view, selectedAttrRangeValues);

            if (!string.IsNullOrEmpty(searchWithin))
            {
                ps.Add("swi", searchWithin);
            }

            if (!string.IsNullOrEmpty(displayAllManufeaturerIDString))
            {
                ps.Add("samp", displayAllManufeaturerIDString);
            }

            if (selectedAttributeGroupIDs != null && selectedAttributeGroupIDs.Count > 0)
            {
                foreach (int avID in selectedAttributeIDs)
                {
                    int avgID = AttributesController.GetCatalogAttributeGroupID(avID);
                    selectedAttributeGroupIDs.Remove(avgID);
                }
                string sagIDs = string.Join("-", selectedAttributeGroupIDs);
                ps.Add("avg", sagIDs);
            }

            if (!string.IsNullOrEmpty(daysRangeString))
            {
                ps.Add("dr", daysRangeString);
            }

            if (onsaleonly.Equals("1"))
            {
                ps.Add("os", onsaleonly);
            }

            return ps;
        }

        public static Dictionary<string, string> GetCatalogParameters(int categoryID, List<int> manufeaturerIDs, string priceRange, List<int> selectedAttributeIDs, List<int> selectedAttributeRangeIDs, string sortBy, List<int> retailerIDs, string view,
            Dictionary<int, string> selectedAttrRangeValues)
        {
            Dictionary<string, string> ps = new Dictionary<string, string>();

            ps.Add("c", categoryID.ToString());

            if (manufeaturerIDs != null && manufeaturerIDs.Count > 0)
            {
                ps.Add("m", string.Join("-", manufeaturerIDs));
            }

            string avsString = "";
            if (selectedAttributeIDs != null)
            {
                foreach (int avid in selectedAttributeIDs)
                {
                    avsString += avid + "-";
                }
            }
            if (selectedAttributeRangeIDs != null)
            {
                foreach (int avrid in selectedAttributeRangeIDs)
                {
                    avsString += avrid + "r-";
                }
            }
            if (!string.IsNullOrEmpty(avsString))
            {
                ps.Add("avs", avsString.TrimEnd('-'));
            }

            //已经选择的Attribute value 
            var avsrStr = string.Empty;
            if (selectedAttrRangeValues != null && selectedAttrRangeValues.Count > 0)
            {
                foreach (int avid in selectedAttrRangeValues.Keys)
                {
                    avsrStr += avid + "_" + selectedAttrRangeValues[avid] + ",";
                }
            }
            if (!string.IsNullOrEmpty(avsrStr))
            {
                ps.Add("avsr", avsrStr.TrimEnd(','));
            }

            if (!string.IsNullOrEmpty(sortBy))
            {
                ps.Add("sb", sortBy);
            }

            if (retailerIDs != null && retailerIDs.Count > 0)
            {
                ps.Add("pcsid", string.Join("-", retailerIDs));
            }

            if (!string.IsNullOrEmpty(view))
            {
                ps.Add("v", view);
            }

            if (!string.IsNullOrEmpty(priceRange))
            {
                ps.Add("pr", priceRange);
            }

            return ps;
        }
        //public static Dictionary<string, string> GetCatalogParameters(int categoryID, int manufeaturerID, int priceRangeNumber, List<int> selectedAttributeIDs, List<int> selectedAttributeRangeIDs, string sortBy, int retailerID, string view)
        //{
        //    Dictionary<string, string> ps = new Dictionary<string, string>();

        //    ps.Add("c", categoryID.ToString());

        //    if (manufeaturerID != 0)
        //    {
        //        ps.Add("m", manufeaturerID.ToString());
        //    }

        //    if (priceRangeNumber > 0)
        //    {
        //        ps.Add("pr", priceRangeNumber.ToString());
        //    }

        //    string avsString = "";
        //    if (selectedAttributeIDs != null)
        //    {
        //        foreach (int avid in selectedAttributeIDs)
        //        {
        //            avsString += avid + "-";
        //        }
        //    }
        //    if (selectedAttributeRangeIDs != null)
        //    {
        //        foreach (int avrid in selectedAttributeRangeIDs)
        //        {
        //            avsString += avrid + "r-";
        //        }
        //    }
        //    if (!string.IsNullOrEmpty(avsString))
        //    {
        //        ps.Add("avs", avsString.TrimEnd('-'));
        //    }

        //    if (!string.IsNullOrEmpty(sortBy))
        //    {
        //        ps.Add("sb", sortBy);
        //    }

        //    if (retailerID != 0)
        //    {
        //        ps.Add("pcsid", retailerID.ToString());
        //    }

        //    if (!string.IsNullOrEmpty(view))
        //    {
        //        ps.Add("v", view);
        //    }

        //    return ps;
        //}

        public static void SetNarrowByInfoUrl(List<NarrowByInfo> narrowByInfoList, PageName pageName, Dictionary<string, string> currentParameters, List<int> selectedAttributeGroupIDs)
        {
            foreach (NarrowByInfo narrowByInfo in narrowByInfoList)
            {
                SetNarrowByInfoUrl(narrowByInfo, pageName, currentParameters, selectedAttributeGroupIDs);
            }
        }

        public static void SetNarrowByInfoUrl(NarrowByInfo narrowByInfo, PageName pageName, Dictionary<string, string> currentParameters, List<int> selectedAttributeGroupIDs)
        {
            if (string.IsNullOrEmpty(narrowByInfo.Name))
            {
                return;
            }
            Dictionary<string, string> newPS = new Dictionary<string, string>(currentParameters);
            if (newPS.ContainsKey("pg"))
            {
                newPS.Remove("pg");
            }

            if (narrowByInfo.Name.Equals("Search Category"))
            {
                string pName = "c";
                newPS.Remove(pName);

                foreach (NarrowItem narrowItem in narrowByInfo.NarrowItemList)
                {
                    Dictionary<string, string> ps = new Dictionary<string, string>(newPS);
                    string pValue = narrowItem.Value;
                    ps.Add(pName, pValue);
                    narrowItem.Url = GetRewriterUrl(pageName, ps);
                }
            }
            else if (narrowByInfo.Name.Equals("Catalog Category"))
            {
                foreach (NarrowItem narrowItem in narrowByInfo.NarrowItemList)
                {
                    Dictionary<string, string> ps = new Dictionary<string, string>();
                    ps.Add("c", narrowItem.Value);
                    narrowItem.Url = GetRewriterUrl(PageName.Catalog, ps);
                }
            }
            else if (narrowByInfo.Name.Equals("AttributeRange") || narrowByInfo.Name.Equals("Attribute"))
            {
                string pName = "avs";
                string pValue = "";
                if (currentParameters.ContainsKey(pName))
                {
                    pValue = currentParameters[pName] + "-";
                }
                newPS.Remove(pName);

                List<int> avgIDs = null;
                if (selectedAttributeGroupIDs != null)
                {
                    avgIDs = new List<int>(selectedAttributeGroupIDs);
                }
                else
                {
                    avgIDs = new List<int>();
                }
                foreach (NarrowItem narrowItem in narrowByInfo.NarrowItemList)
                {
                    string pv = "";
                    Dictionary<string, string> ps = new Dictionary<string, string>(newPS);
                    if (narrowByInfo.Name.Equals("AttributeRange"))
                    {
                        pv = pValue + narrowItem.Value + "r";
                    }
                    else
                    {
                        pv = pValue + narrowItem.Value;
                        int avID = int.Parse(narrowItem.Value);
                        int avgID = AttributesController.GetCatalogAttributeGroupID(avID);
                        List<int> _avgIDs = new List<int>(avgIDs);
                        if (_avgIDs.Contains(avgID))
                        {
                            ps.Remove("avg");
                            _avgIDs.Remove(avgID);
                            if (_avgIDs.Count > 0)
                            {
                                string _pv = string.Join("-", _avgIDs);
                                ps.Add("avg", _pv);
                            }
                        }
                    }
                    ps.Add(pName, pv);
                    narrowItem.Url = GetRewriterUrl(pageName, ps);
                }
            }
            else if (narrowByInfo.Name.Equals("Retailer"))
            {
                string pName = "pcsid";
                if (newPS.ContainsKey(pName))
                {
                    string pValue = newPS[pName];
                    newPS.Remove(pName);

                    foreach (NarrowItem narrowItem in narrowByInfo.NarrowItemList)
                    {
                        Dictionary<string, string> ps = new Dictionary<string, string>(newPS);
                        string newValue = pValue + "-" + narrowItem.Value;
                        ps.Add(pName, newValue);
                        narrowItem.Url = GetRewriterUrl(pageName, ps);
                    }
                }
                else
                {
                    foreach (NarrowItem narrowItem in narrowByInfo.NarrowItemList)
                    {
                        Dictionary<string, string> ps = new Dictionary<string, string>(newPS);
                        string pValue = narrowItem.Value;
                        ps.Add(pName, pValue);
                        narrowItem.Url = GetRewriterUrl(pageName, ps);
                    }
                }
            }
            else if (narrowByInfo.Name.Equals("Manufacturer") || narrowByInfo.Name.Equals("TopManufacturer"))
            {
                string pName = "m";
                if (newPS.ContainsKey(pName))
                {
                    string pValue = newPS[pName];
                    newPS.Remove(pName);

                    foreach (NarrowItem narrowItem in narrowByInfo.NarrowItemList)
                    {
                        Dictionary<string, string> ps = new Dictionary<string, string>(newPS);
                        string newValue = pValue + "-" + narrowItem.Value;
                        ps.Add(pName, newValue);
                        narrowItem.Url = GetRewriterUrl(pageName, ps);
                    }
                }
                else
                {
                    foreach (NarrowItem narrowItem in narrowByInfo.NarrowItemList)
                    {
                        Dictionary<string, string> ps = new Dictionary<string, string>(newPS);
                        string pValue = narrowItem.Value;
                        ps.Add(pName, pValue);
                        narrowItem.Url = GetRewriterUrl(pageName, ps);
                    }
                }
            }
            else if (narrowByInfo.Name.Equals("CatalogPriceRange") || narrowByInfo.Name.Equals("SearchPriceRange"))
            {
                string pName = "pr";
                newPS.Remove(pName);

                foreach (NarrowItem narrowItem in narrowByInfo.NarrowItemList)
                {
                    Dictionary<string, string> ps = new Dictionary<string, string>(newPS);
                    string pValue = narrowItem.Value;
                    ps.Add(pName, pValue);
                    narrowItem.Url = GetRewriterUrl(pageName, ps);
                }
            }
        }

        #region ruangang
        public static string GetForumRewriteUrl(ForumPageName pageTo, string seoString, string id)
        {
            return GetForumRewriteUrl(pageTo, seoString, id, "");
        }

        public static string GetForumRewriteUrl(ForumPageName pageTo, string seoString, string id, string page)
        {

            seoString = FilterInvalidUrlPathChar(seoString);
            page = page.Trim();

            string url = "";
            switch (pageTo)
            {
                case ForumPageName.TopicList:
                    if (id == "") id = "0";
                    if (seoString == "") seoString = "Others";
                    if (seoString != "")
                    {
                        if (page.Trim() != "" || page == "1")
                            url = "/" + seoString + "/Forum-" + id + "," + page;
                        else
                            url = "/" + seoString + "/Forum-" + id;
                    }
                    else
                    {
                        if (page.Trim() != "")
                            url = "/Forum-" + id + "," + page;
                        else
                            url = "/Forum-" + id;
                    }
                    break;
                case ForumPageName.Topic:
                    if (page == "" || page == "1")
                        url = "/" + seoString + "/Topic-" + id;
                    else
                    {
                        url = "/" + seoString + "/Topic-" + id + "," + page;
                    }
                    break;
                case ForumPageName.NewTopic:
                    // http://www.priceme.co.nz/NewTopic-c616.aspx
                    // http://www.priceme.co.nz/Huggies-240-Unscented-Baby-Wipes/NewTopic-p882286239.aspx
                    // c 和 p 是夹在 id 中传过来的。
                    url = "/" + seoString + "/NewTopic-" + id;
                    break;
                case ForumPageName.ForumAll:
                    if (page == "1")
                        url = "/" + seoString + "/Forum";
                    else
                        url = "/" + seoString + "/Forum," + page;
                    break;
                case ForumPageName.Edit:
                    url = "/Forum/Edit-" + id;
                    break;
            }

            return (url + ".aspx").FixUrl();
        }
        #endregion

        public static Dictionary<string, string> GetSearchPageParameters(string queryKeywords, string searchWithIn, int categoryID, List<int> manufeaturerIDs, PriceRange priceRange, List<int> selectedAttributeIDs, List<int> selectedAttributeRangeIDs, string sortBy, List<int> retailerIDs, string view,
            Dictionary<int, string> selectedAttrRangeValues, List<int> selectedAttributeGroupIDs)
        {
            if (priceRange != null)
                return GetSearchPageParameters(queryKeywords, searchWithIn, categoryID, manufeaturerIDs, priceRange.ToUrlString(), selectedAttributeIDs, selectedAttributeRangeIDs, sortBy, retailerIDs, view, selectedAttrRangeValues, selectedAttributeGroupIDs);
            else
                return GetSearchPageParameters(queryKeywords, searchWithIn, categoryID, manufeaturerIDs, "", selectedAttributeIDs, selectedAttributeRangeIDs, sortBy, retailerIDs, view, selectedAttrRangeValues, selectedAttributeGroupIDs);
        }

        public static Dictionary<string, string> GetSearchPageParameters(string queryKeywords, string searchWithIn, int categoryID, List<int> manufeaturerIDs, string priceRangeString, List<int> selectedAttributeIDs, List<int> selectedAttributeRangeIDs, string sortBy, List<int> retailerIDs, string view,
            Dictionary<int, string> selectedAttrRangeValues, List<int> selectedAttributeGroupIDs)
        {
            Dictionary<string, string> ps = new Dictionary<string, string>();

            if (categoryID != 0)
            {
                ps.Add("c", categoryID.ToString());
            }

            ps.Add("q", queryKeywords);

            if (!string.IsNullOrEmpty(searchWithIn))
            {
                ps.Add("swi", searchWithIn);
            }

            if (!string.IsNullOrEmpty(priceRangeString) && priceRangeString != "0-Max")
            {
                ps.Add("pr", priceRangeString);
            }

            if (manufeaturerIDs != null && manufeaturerIDs.Count > 0)
            {
                ps.Add("m", string.Join("-", manufeaturerIDs));
            }

            string avsString = "";
            if (selectedAttributeIDs != null)
            {
                foreach (int avid in selectedAttributeIDs)
                {
                    avsString += avid + "-";
                }
            }
            if (selectedAttributeRangeIDs != null)
            {
                foreach (int avrid in selectedAttributeRangeIDs)
                {
                    avsString += avrid + "r-";
                }
            }
            if (!string.IsNullOrEmpty(avsString))
            {
                ps.Add("avs", avsString.TrimEnd('-'));
            }

            if (selectedAttributeGroupIDs != null && selectedAttributeGroupIDs.Count > 0)
            {
                foreach (int avID in selectedAttributeIDs)
                {
                    int avgID = AttributesController.GetCatalogAttributeGroupID(avID);
                    selectedAttributeGroupIDs.Remove(avgID);
                }
                string sagIDs = string.Join("-", selectedAttributeGroupIDs);
                ps.Add("avg", sagIDs);
            }

            //已经选择的Attribute value 
            var avsrStr = string.Empty;
            if (selectedAttrRangeValues != null && selectedAttrRangeValues.Count > 0)
            {
                foreach (int avid in selectedAttrRangeValues.Keys)
                {
                    avsrStr += avid + "_" + selectedAttrRangeValues[avid] + "_";
                }
            }
            if (!string.IsNullOrEmpty(avsrStr))
            {
                ps.Add("avsr", avsrStr.TrimEnd('_'));
            }

            if (!string.IsNullOrEmpty(sortBy))
            {
                ps.Add("sb", sortBy);
            }

            if (retailerIDs != null && retailerIDs.Count > 0)
            {
                ps.Add("pcsid", string.Join("-", manufeaturerIDs));
            }

            if (!string.IsNullOrEmpty(view))
            {
                ps.Add("v", view);
            }

            return ps;
        }

        #region ruangang
        private static string GetProductUrl(string orgUrl, bool redirectToMobileTouch)
        {
            orgUrl = orgUrl.Replace(" ", "");
            Match ma = Regex.Match(orgUrl, @"(/(?<name>[^/]*))?/p-(?<pid>\d+)\.aspx", RegexOptions.IgnoreCase);

            //Match ma = Regex.Match(orgUrl, @"/p-(?<pid>\d+)", RegexOptions.IgnoreCase);
            if (ma.Success)
            {
                string newUrl = "";

                if (!redirectToMobileTouch)
                {
                    newUrl = "/product.aspx?pid=" + ma.Groups["pid"].Value + "&name=" + ma.Groups["name"].Value;
                    if (orgUrl.EndsWith("?aid=5", StringComparison.InvariantCultureIgnoreCase))
                        newUrl += "&aid=5";
                }
                else
                {
                    newUrl = Resources.Resource.MobileTouchSiteURL + "/product.aspx?pid=" + ma.Groups["pid"].Value;
                }

                return newUrl;
            }

            return orgUrl;
        }

        private static string GetRetailerProductUrl(string orgUrl, bool redirectToMobileTouch)
        {
            //string paramValue = System.IO.Path.GetFileNameWithoutExtension(orgUrl);
            //Match ma = Regex.Match(orgUrl, @"/rp-(?<rpid>\d+)", RegexOptions.IgnoreCase);

            Match ma = Regex.Match(orgUrl, @"(/(?<name>[^/]*))?/rp-(?<rpid>\d+)\.aspx", RegexOptions.IgnoreCase);

            if (ma.Success)
            {
                if (redirectToMobileTouch)
                {
                    int rpid = int.Parse(ma.Groups["rpid"].Value);
                    var retailerProduct = ProductController.GetRetailerProduct(rpid, PriceMe.WebConfig.CountryId);
                    if (retailerProduct != null)
                    {
                        return Resources.Resource.MobileTouchSiteURL + "/product.aspx?pid=" + retailerProduct.ProductId;
                    }
                }
                else
                {
                    return "/retailerproduct.aspx?rpid=" + ma.Groups["rpid"].Value + "&rpname=" + ma.Groups["name"].Value;
                }
            }

            return orgUrl;
        }

        private static string GetForumTopicListUrl(string orgUrl)
        {
            //string paramValue = System.IO.Path.GetFileNameWithoutExtension(orgUrl);
            Regex reg = new Regex(@"forum-(?<fid>\d+)(,(?<page>\d+))?", RegexOptions.IgnoreCase);
            Match ma = reg.Match(orgUrl);
            if (ma.Success)
            {
                return "/Forum/TopicList.aspx?cid=" + ma.Groups["fid"].Value + "&page=" + ma.Groups["page"].Value;
            }
            else
            {
                return orgUrl;
            }
        }

        private static string GetForumTopicUrl(string orgUrl)
        {
            //string paramValue = System.IO.Path.GetFileNameWithoutExtension(orgUrl);
            Regex reg = new Regex(@"topic-(?<tid>\d+)(,(?<page>\d+))?", RegexOptions.IgnoreCase);
            Match ma = reg.Match(orgUrl);
            if (ma.Success)
            {
                return "/Forum/Topic.aspx?tid=" + ma.Groups["tid"].Value + "&page=" + ma.Groups["page"].Value;
            }
            else
            {
                return orgUrl;
            }
        }

        private static string GetForumNewTopicUrl(string orgUrl)
        {
            //string paramValue = System.IO.Path.GetFileNameWithoutExtension(orgUrl);
            Regex reg = new Regex(@"newtopic-(?<type>[cp]?)(?<pid>\d+)", RegexOptions.IgnoreCase);
            Match ma = reg.Match(orgUrl);
            if (ma.Success)
            {
                string cID = "0", pID = "0";
                if (ma.Groups["type"].Value.ToUpper() == "C")
                {
                    cID = ma.Groups["pid"].Value;
                }
                else
                {
                    pID = ma.Groups["pid"].Value;
                }
                return "/Forum/NewTopic.aspx?cid=" + cID + "&pid=" + pID;
            }
            else
                return orgUrl;
        }

        private static string GetForumReportUrl(string orgUrl)
        {
            Regex reg = new Regex(@"report-(?<qid>\d+)", RegexOptions.IgnoreCase);
            Match ma = reg.Match(orgUrl);
            if (ma.Success)
            {
                string qID = ma.Groups["qid"].Value;
                return "/Forum/report.aspx?qid=" + qID;
            }
            else
                return orgUrl;
        }

        private static string GetForumUrl(string orgUrl)
        {
            Regex reg = new Regex(@"forum(,(?<page>\d+))?", RegexOptions.IgnoreCase);
            Match ma = reg.Match(orgUrl);
            if (ma.Success)
            {
                return "/Forum/forum.aspx?page=" + ma.Groups["page"].Value;
            }
            else
                return orgUrl;
        }

        #endregion

        private static string GetProductEditionV1Url(string orgUrl)
        {
            Match ma = Regex.Match(orgUrl, @"(/(?<name>[^/]*))?/pv1-(?<pid>\d+)\.aspx", RegexOptions.IgnoreCase);

            if (ma.Success)
            {
                string newUrl = "/productV1.aspx?pid=" + ma.Groups["pid"].Value + "&name=" + ma.Groups["name"].Value;
                if (orgUrl.EndsWith("?aid=5", StringComparison.InvariantCultureIgnoreCase))
                    newUrl += "&aid=5";

                return newUrl;
            }

            return orgUrl;
        }

        private static string GetProductEditionV2Url(string orgUrl)
        {
            Match ma = Regex.Match(orgUrl, @"(/(?<name>[^/]*))?/pv2-(?<pid>\d+)\.aspx", RegexOptions.IgnoreCase);

            if (ma.Success)
            {
                string newUrl = "/productV2.aspx?pid=" + ma.Groups["pid"].Value + "&name=" + ma.Groups["name"].Value;
                if (orgUrl.EndsWith("?aid=5", StringComparison.InvariantCultureIgnoreCase))
                    newUrl += "&aid=5";

                return newUrl;
            }

            return orgUrl;
        }
    }
}