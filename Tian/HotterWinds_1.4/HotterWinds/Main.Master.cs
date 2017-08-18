using PriceMe;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace HotterWinds
{
    public partial class Main : System.Web.UI.MasterPage
    {
        int CategoryID = 0;
        List<int> BrandIDs = null;
        public string ATagTitle = "";
        public int pageID = -1;
        public int SPACE_ID = 0;

        public bool IsSearchPage = false;
        public bool IsNoIndexPage = false;

        string PageViewLink;

        public string ShareImage = "";
        public string ShareUrl = "";
        public string SharePrice = "";
        public string ShareName = "Christmas Wish list";

        public string GoogleAnalytis_require = System.Configuration.ConfigurationManager.AppSettings["GoogleAnalytis_require"].ToString();

        string iosAppArgument = "";

        public void SetIosAppArgument(string iaa)
        {
            iosAppArgument = iaa;
        }

        public void SetPageViewLink(string url)
        {
            PageViewLink = url;
        }

        public void SetBreadCrumb(BreadCrumbInfo breadCrumbInfo)
        {
            //this.PrettyBreadCrumb1.breadCrumbInfo = breadCrumbInfo;
        }

        public void SetBreadCrumb(BreadCrumbInfo breadCrumbInfo, string reportType)
        {
            //this.PrettyBreadCrumb1.breadCrumbInfo = breadCrumbInfo;
        }

        public void SetBreadCrumb(BreadCrumbInfo breadCrumbInfo, int id)
        {
            //this.PrettyBreadCrumb1.breadCrumbInfo = breadCrumbInfo;
        }

        public void SetBreadCrumb(BreadCrumbInfo breadCrumbInfo, int id, string reportType)
        {
            //this.PrettyBreadCrumb1.breadCrumbInfo = breadCrumbInfo;
        }

        public void DisplayCategoryMenu(bool isDisplay)
        {
            this.NewHeaderControl.DisplayCategoryMenu = isDisplay;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //AddMeta();
            AddLink();
            AddCssAndJs();
        }

        public void AddCanonical(string canonicalUrl)
        {
            HtmlLink canonicalLink = new HtmlLink();
            canonicalLink.Href = Resources.Resource.Global_HomePageUrl + canonicalUrl;
            canonicalLink.Href = canonicalLink.Href.TrimEnd('/');
            canonicalLink.Attributes.Add("rel", "canonical");
            this.Page.Header.Controls.Add(canonicalLink);
        }

        public void AddAlternate(string alternate)
        {
            if (WebConfig.CountryId == 3)
            {
                HtmlLink alternateLink = new HtmlLink();
                alternateLink.Href = alternate;
                alternateLink.Attributes.Add("rel", "alternate");
                this.Page.Header.Controls.Add(alternateLink);
            }
        }

        public void SetBrandIDs(List<int> brandIDs)
        {
            BrandIDs = brandIDs;
        }

        public void SetBrandID(int brandID)
        {
            List<int> bids = new List<int>();
            bids.Add(brandID);
            BrandIDs = bids;
        }

        public void SetCategory(int categoryID)
        {
            CategoryID = categoryID;
            this.NewHeaderControl.CategoryID = categoryID;
        }

        int GetMainCategoryID()
        {
            int cid = Utility.GetIntParameter("cid");
            if (cid <= 0)
            {
                cid = Utility.GetIntParameter("c");
            }
            return cid;
        }

        public void InitATag(string _title)
        {
            ATagTitle = _title;
        }

        void AddMeta()
        {
            HtmlMeta meta = new HtmlMeta();
            meta.Name = "viewport";
            meta.Content = "width=device-width,initial-scale=1.0, maximum-scale=1.0, user-scalable=0";
            this.Page.Header.Controls.Add(meta);

            if (IsNoIndexPage)
            {
                HtmlMeta noIndexMeta = new HtmlMeta();
                noIndexMeta.Name = "robots";
                noIndexMeta.Content = "noindex";
                this.Page.Header.Controls.Add(noIndexMeta);
            }

            //<meta name="apple-itunes-app" content="app-id=391388653,  app-argument=myURL"> 
            if (string.IsNullOrEmpty(iosAppArgument))
            {
                iosAppArgument = Request.Url.Scheme + "://" + Request.Url.Host + Request.RawUrl.ToString();
            }
            //HtmlMeta iosMeta = new HtmlMeta();
            //iosMeta.Name = "apple-itunes-app";
            //iosMeta.Content = "app-id=391388653,  app-argument=" + iosAppArgument;
            //this.Page.Header.Controls.Add(iosMeta);

            HtmlMeta googleplayMeta = new HtmlMeta();
            googleplayMeta.Name = "google-play-app";
            googleplayMeta.Content = "app-id=PriceMe.Android";
            this.Page.Header.Controls.Add(googleplayMeta);
        }

        public void AddNoindexMeta()
        {
            HtmlMeta meta = new HtmlMeta();
            meta.Name = "viewport";
            meta.Content = "width=device-width,initial-scale=1.0, maximum-scale=1.0, user-scalable=0";
            this.Page.Header.Controls.Add(meta);

            HtmlMeta noIndexMeta = new HtmlMeta();
            noIndexMeta.Name = "robots";
            noIndexMeta.Content = "noindex";
            this.Page.Header.Controls.Add(noIndexMeta);
        }

        void AddLink()
        {
            HtmlLink cssLink = new HtmlLink();
            cssLink.Href = "https://s3.pricemestatic.com/Images/HotterWindsVersion/Hotter-Winds-icon.ico";
            cssLink.Attributes.Add("rel", "shortcut icon");
            this.Page.Header.Controls.Add(cssLink);

            cssLink = new HtmlLink();
            cssLink.Href = "/apple-touch-icon.png";
            cssLink.Attributes.Add("rel", "apple-touch-icon");
            this.Page.Header.Controls.Add(cssLink);

            cssLink = new HtmlLink();
            cssLink.Href = "//fonts.googleapis.com/css?family=Open+Sans:300,400,600,700,800";
            cssLink.Attributes.Add("rel", "stylesheet");
            cssLink.Attributes.Add("type", "text/css");
            this.Page.Header.Controls.Add(cssLink);
        }

        void AddCssAndJs()
        {
            //css
            //HtmlLink bootstrapCssLink2 = new HtmlLink();
            //bootstrapCssLink2.Href = "https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css";
            //bootstrapCssLink2.Attributes.Add("rel", "stylesheet");
            //bootstrapCssLink2.Attributes.Add("type", "text/css");
            //bootstrapCssLink2.Attributes.Add("integrity", "sha384-BVYiiSIFeK1dGmJRAkycuHAHRg32OmUcww7on3RYdg4Va+PmSTsz/K68vbdEjh4u");
            //bootstrapCssLink2.Attributes.Add("crossorigin", "anonymous");
            //this.Page.Header.Controls.Add(bootstrapCssLink2);

            //https://maxcdn.bootstrapcdn.com/font-awesome/4.6.3/css/font-awesome.min.css
            HtmlLink awesomeLink = new HtmlLink();
            awesomeLink.Href = "https://maxcdn.bootstrapcdn.com/font-awesome/4.6.3/css/font-awesome.min.css";
            awesomeLink.Attributes.Add("rel", "stylesheet");
            awesomeLink.Attributes.Add("type", "text/css");
            this.Page.Header.Controls.Add(awesomeLink);

            HtmlLink cssLink = new HtmlLink();
            cssLink.Href = WebConfig.CssJsPath + "/Content/new_common.css?ver=" + PriceMe.WebConfig.WEB_cssVersion;
            cssLink.Attributes.Add("rel", "stylesheet");
            cssLink.Attributes.Add("type", "text/css");

            this.Page.Header.Controls.Add(cssLink);

            if (pageID == (int)CssFile.Home)
            {
                AddCssFile("home.min");

            }
            else if (pageID == (int)CssFile.Catalog)
            {
                AddCssFile("new_catalog");
                //this.Page.Header.Controls.Add(cssLink);
            }
            else if (pageID == (int)CssFile.Product)
            {
                AddCssFile("new_product");
                //this.Page.Header.Controls.Add(cssLink);
            }
            else if (pageID == (int)CssFile.Retailer)
            {
                AddCssFile("new_retailer");
                AddCssFile("new_other");
            }
            else if (pageID == (int)CssFile.Brand)
            {
                AddCssFile("new_catalog");
                AddCssFile("new_other");
            }
            else if (pageID == (int)CssFile.Compare)
            {
                AddCssFile("new_product");
                AddCssFile("new_other");
            }
            else if (pageID == (int)CssFile.About && (WebConfig.CountryId == 3 || WebConfig.CountryId == 28))
            {
                AddCssFile("js_composer.min");
                AddCssFile("new_other");
            }
            else
            {
                AddCssFile("new_other");
            }

            //js
            //HtmlGenericControl scriptHtmlControl = new HtmlGenericControl("script");
            //scriptHtmlControl.Attributes.Add("type", "text/javascript");
            //scriptHtmlControl.Attributes.Add("src", "//ajax.googleapis.com/ajax/libs/jquery/3.1.1/jquery.min.js");
            //scriptHtmlControl.Attributes.Add("crossorigin", "anonymous");
            //this.Page.Header.Controls.Add(scriptHtmlControl);

            //HtmlGenericControl fallbackJQControl = new HtmlGenericControl("script");
            //fallbackJQControl.Attributes.Add("type", "text/javascript");
            //fallbackJQControl.InnerHtml = "window.jQuery || document.write('<script src=\"//images.pricemestatic.com/Images/css/jquery-3.1.0.min.js\"><\\/script>')";
            //this.Page.Header.Controls.Add(fallbackJQControl);

            //PriceMe.IE8JqueryTag IE8JqueryTag = new IE8JqueryTag();
            //this.Page.Header.Controls.Add(IE8JqueryTag);

            PriceMe.Html5shivJSTag Html5shivJSTag = new Html5shivJSTag();
            this.Page.Header.Controls.Add(Html5shivJSTag);

            PriceMe.RespondJSTag RespondJSTag = new RespondJSTag();
            this.Page.Header.Controls.Add(RespondJSTag);

            HtmlGenericControl commonScriptControl = new HtmlGenericControl("script");
            commonScriptControl.Attributes.Add("type", "text/javascript");
            commonScriptControl.Attributes.Add("src", WebConfig.CssJsPath + "/Scripts/new_common.js?ver=" + WebConfig.WEB_cssVersion);
            this.Page.Header.Controls.Add(commonScriptControl);

            //HtmlGenericControl bootstrapScriptControl = new HtmlGenericControl("script");
            //bootstrapScriptControl.Attributes.Add("type", "text/javascript");
            //bootstrapScriptControl.Attributes.Add("src", "https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js");
            //bootstrapScriptControl.Attributes.Add("integrity", "sha384-Tc5IQib027qvyjSMfHjOMaLkfuWVxZxUPnCJA7l2mCWNIpG9mGCD8wGNIcPD7Txa");
            //bootstrapScriptControl.Attributes.Add("crossorigin", "anonymous");
            //this.Page.Header.Controls.Add(bootstrapScriptControl);

            HtmlGenericControl lotameScriptControl = GetLotameScriptControl(WebConfig.CountryId);
            this.Page.Header.Controls.Add(lotameScriptControl);

            if (pageID == 2)
            {
                AddJsFile("new_product");
            }
            else if (pageID == (int)CssFile.Home)
            {
                AddJsFile("new_home");
            }

            if (WebConfig.Environment == "prod")
            {
                HtmlGenericControl googleScriptHtmlControl = new HtmlGenericControl("script");
                googleScriptHtmlControl.Attributes.Add("type", "text/javascript");

                googleScriptHtmlControl.InnerHtml = @"(function(i,s,o,g,r,a,m){i['GoogleAnalyticsObject']=r;i[r]=i[r]||function(){ (i[r].q=i[r].q||[]).push(arguments)},i[r].l=1*new Date();a=s.createElement(o), m=s.getElementsByTagName(o)[0];a.async=1;a.src=g;m.parentNode.insertBefore(a,m) })(window,document,'script','//www.google-analytics.com/analytics.js','ga');
            ga('create', '" + ConfigurationManager.AppSettings["GoogleAnalytis"] + @"', 'auto'); ga('require', '" + ConfigurationManager.AppSettings["GoogleAnalytis_require"] + @"');";
                if (CategoryID > 0)
                {
                    googleScriptHtmlControl.InnerHtml += "ga('set', 'contentGroup1', '" + CategoryID + "');";
                }
                if (BrandIDs != null && BrandIDs.Count > 0)
                {
                    googleScriptHtmlControl.InnerHtml += "ga('set', 'contentGroup2', '" + BrandIDs[0] + "');";
                }

                if (!string.IsNullOrEmpty(HttpContext.Current.User.Identity.Name))
                {
                    googleScriptHtmlControl.InnerHtml += "ga('set', 'contentGroup3', '" + HttpContext.Current.User.Identity.Name + "');";
                }

                if (string.IsNullOrEmpty(PageViewLink))
                {
                    googleScriptHtmlControl.InnerHtml += " ga('send', 'pageview');";
                }
                else
                {
                    googleScriptHtmlControl.InnerHtml += " ga('send', 'pageview', '" + PageViewLink + "');";
                }
                //this.Page.Header.Controls.Add(googleScriptHtmlControl);

                HtmlGenericControl googleScriptHtml = new HtmlGenericControl("script");
                googleScriptHtml.Attributes.Add("type", "text/javascript");

                googleScriptHtml.InnerHtml = @"(function(w,d,s,l,i){w[l]=w[l]||[];w[l].push({'gtm.start':"
                                           + "new Date().getTime(),event:'gtm.js'});var f=d.getElementsByTagName(s)[0],"
                                           + "j=d.createElement(s),dl=l!='dataLayer'?'&l='+l:'';j.async=true;j.src="
                                           + "'https://www.googletagmanager.com/gtm.js?id='+i+dl;f.parentNode.insertBefore(j,f);"
                                           + "})(window,document,'script','dataLayer','" + WebConfig.GoogTagKey + "');";
                //this.Page.Header.Controls.Add(googleScriptHtml);
            }

            if (IsSearchPage && WebConfig.Environment == "prod")
            {
                HtmlGenericControl googleAdsHtmlControl = new HtmlGenericControl("script");
                googleAdsHtmlControl.Attributes.Add("type", "text/javascript");
                googleAdsHtmlControl.Attributes.Add("charset", "utf-8");
                googleAdsHtmlControl.InnerHtml = @"(function(G,o,O,g,L,e){G[g]=G[g]||function(){(G[g]['q']=G[g]['q']||[]).push(
            arguments)},G[g]['t']=1*new Date;L=o.createElement(O),e=o.getElementsByTagName(
            O)[0];L.async=1;L.src='//www.google.com/adsense/search/async-ads.js';
            e.parentNode.insertBefore(L,e)})(window,document,'script','_googCsa');";

                this.Page.Header.Controls.Add(googleAdsHtmlControl);
            }

            if (pageID == (int)CssFile.Product)
            {
                //测试Visual Webiste
                HtmlGenericControl visualWebisteScriptControl = GetVisualWebisteScriptControl();
                this.Page.Header.Controls.Add(visualWebisteScriptControl);
            }

            //https://developers.google.com/structured-data/site-name
            string websiteName = "PriceMe " + Resources.Resource.ShortCountryName;
            string alternateWebsiteName = websiteName + " " + WebConfig.CountryId;

            if (WebConfig.Environment == "prod" && pageID == 0)
            {
                HtmlGenericControl googleWebSiteSearchHtmlControl = new HtmlGenericControl("script");
                googleWebSiteSearchHtmlControl.Attributes.Add("type", "application/ld+json");
                googleWebSiteSearchHtmlControl.InnerHtml = "{" +
                    "\"@context\": \"https://schema.org\"," +
                    "\"@type\": \"WebSite\"," +
                    "\"url\": \"" + Resources.Resource.Global_HomePageUrl + "\"," +
                    "\"name\" : \"" + websiteName + "\"," +
                    "\"alternateName\" : \"" + alternateWebsiteName + "\"," +
                    "\"potentialAction\": {" +
                    "\"@type\": \"SearchAction\"," +
                    "\"target\": \"" + Resources.Resource.Global_HomePageUrl + "/search.aspx?&q={search_keywords}\"," +
                    "\"query-input\": \"required name=search_keywords\"" +
                    "}" +
                    "}";

                this.Page.Header.Controls.Add(googleWebSiteSearchHtmlControl);
            }

            if (pageID != 0)
            {
                HtmlGenericControl webisteStructuredDataScriptControl = GetWebisteStructuredDataScriptControl("https://hotterwinds.co.nz/", "hotterwinds", "hotterwinds 25");
                this.Page.Header.Controls.Add(webisteStructuredDataScriptControl);
            }

            HtmlGenericControl script1 = new HtmlGenericControl("script");
            script1.Attributes.Add("type", "text/javascript");

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<!-- Google Tag Manager -->");
            sb.AppendLine("<script>(function (w, d, s, l, i) {");
            sb.AppendLine("w[l] = w[l] || []; w[l].push({");
            sb.AppendLine(" 'gtm.start':");
            sb.AppendLine("new Date().getTime(), event: 'gtm.js'");
            sb.AppendLine("}); var f = d.getElementsByTagName(s)[0],");
            sb.AppendLine("j = d.createElement(s), dl = l != 'dataLayer' ? '&l=' + l : ''; j.async = true; j.src =");
            sb.AppendLine("'https://www.googletagmanager.com/gtm.js?id=' + i + dl; f.parentNode.insertBefore(j, f);");
            sb.AppendLine("})(window, document, 'script', 'dataLayer', '" + GoogleAnalytis_require + "');</script>");
            sb.AppendLine("<!-- End Google Tag Manager -->");

            script1.InnerHtml = sb.ToString();

            this.Page.Header.Controls.Add(script1);
        }

        private HtmlGenericControl GetLotameScriptControl(int countryId)
        {
            //New Zealand: priceme.co.nz 3
            //< script src = "https://tags.crwdcntrl.net/c/10665/cc_af.js" ></ script >

            // Philippines: priceme.com.ph 28
            // < script src = "https://tags.crwdcntrl.net/c/10666/cc_af.js" ></ script >

            //  Malaysia: priceme.com.my 45
            //  < script src = "https://tags.crwdcntrl.net/c/10667/cc_af.js" ></ script >

            //   Singapore: priceme.com.sg 36
            //   < script src = "https://tags.crwdcntrl.net/c/10668/cc_af.js" ></ script >

            //    Australia: priceme.com.au 1
            //    < script src = "https://tags.crwdcntrl.net/c/10669/cc_af.js" ></ script >

            //     Hong Kong: priceme.com.hk 41
            //     < script src = "https://tags.crwdcntrl.net/c/10670/cc_af.js" ></ script >

            //      Deals: deals.priceme.co.nz
            //      < script src = "https://tags.crwdcntrl.net/c/10671/cc_af.js" ></ script >

            string scriptSrc = "";
            switch (countryId)
            {
                case 1:
                    scriptSrc = "https://tags.crwdcntrl.net/c/10668/cc_af.js";
                    break;
                case 28:
                    scriptSrc = "https://tags.crwdcntrl.net/c/10666/cc_af.js";
                    break;
                case 36:
                    scriptSrc = "https://tags.crwdcntrl.net/c/10668/cc_af.js";
                    break;
                case 41:
                    scriptSrc = "https://tags.crwdcntrl.net/c/10670/cc_af.js";
                    break;
                case 45:
                    scriptSrc = "https://tags.crwdcntrl.net/c/10667/cc_af.js";
                    break;
                default:
                    scriptSrc = "https://tags.crwdcntrl.net/c/10665/cc_af.js";
                    break;
            }

            HtmlGenericControl lotameScriptControl = new HtmlGenericControl("script");
            lotameScriptControl.Attributes.Add("type", "text/javascript");
            lotameScriptControl.Attributes.Add("src", scriptSrc);

            return lotameScriptControl;
        }

        /// <summary>
        /// 加载DFP广告脚本
        /// </summary>
        /// <param name="rootCategoryID">大类</param>
        /// <param name="ccid">大类的下一层分类</param>
        /// <param name="prodp">价格范围ID</param>
        public void LoadDFPAds(int rootCategoryID, int ccid, int prodp)
        {
            if (!DFP_AdsChontroller.HasDFPAds(rootCategoryID)) return;

            HtmlGenericControl _DFPHeadScriptControl = GetDFPHeadScriptControl();
            this.Page.Header.Controls.Add(_DFPHeadScriptControl);

            HtmlGenericControl _DFPInfoScriptControl = GetDFPInfoScriptControl(rootCategoryID, ccid, WebConfig.CountryId, prodp, "div-gpt-ad-1433735994482-2", "div-gpt-ad-1433735994482-1", "div-gpt-ad-1433735994482-0");
            this.Page.Header.Controls.Add(_DFPInfoScriptControl);
        }

        private HtmlGenericControl GetDFPInfoScriptControl(int rootCategoryID, int subcat, int countryID, int prodp, string mediumRectangleSlot, string horisontalMiddleSlot, string horisontalLowerSlot)
        {
            string prodpScript = "";
            if (prodp > 0)
            {
                prodpScript = "googletag.pubads().setTargeting('prodp', ['" + prodp + @"']);";
            }
            HtmlGenericControl scriptControl = new HtmlGenericControl("script");
            scriptControl.Attributes.Add("type", "text/javascript");
            scriptControl.InnerHtml = @"
        googletag.cmd.push(function() {
        googletag.defineSlot('/1053498/horisontal_lower_banner', [728, 90], '" + horisontalLowerSlot + @"').addService(googletag.pubads());
        googletag.defineSlot('/1053498/horisontal_middle_banner', [728, 90], '" + horisontalMiddleSlot + @"').addService(googletag.pubads());
        googletag.defineSlot('/1053498/pm_prod_mrec', [300, 250], '" + mediumRectangleSlot + @"').addService(googletag.pubads());
        googletag.pubads().enableSingleRequest();
        googletag.pubads().setTargeting('cid', ['" + rootCategoryID + @"']);
        googletag.pubads().setTargeting('subcat', ['" + subcat + @"']);
        googletag.pubads().setTargeting('ccid', ['" + countryID + @"']);"
            + prodpScript +
            @"googletag.enableServices();
        });";
            return scriptControl;
        }

        private HtmlGenericControl GetDFPHeadScriptControl()
        {
            HtmlGenericControl scriptControl = new HtmlGenericControl("script");
            scriptControl.Attributes.Add("type", "text/javascript");
            scriptControl.InnerHtml = @"
            var googletag = googletag || {};
            googletag.cmd = googletag.cmd || [];
            (function() {
            var gads = document.createElement('script');
            gads.async = true;
            gads.type = 'text/javascript';
            var useSSL = 'https:' == document.location.protocol;
            gads.src = (useSSL ? 'https:' : 'http:') + '//www.googletagservices.com/tag/js/gpt.js';
            var node = document.getElementsByTagName('script')[0];
            node.parentNode.insertBefore(gads, node);
            })();";
            return scriptControl;
        }

        private HtmlGenericControl GetWebisteStructuredDataScriptControl(string siteUrl, string websiteName, string alternateWebsiteName)
        {
            HtmlGenericControl scriptControl = new HtmlGenericControl("script");
            scriptControl.Attributes.Add("type", "application/ld+json");
            scriptControl.InnerHtml =
                "{  \"@context\" : \"https://schema.org\"," +
                "   \"@type\" : \"WebSite\"," +
                "   \"name\" : \"" + websiteName + "\"," +
                "   \"alternateName\" : \"" + alternateWebsiteName + "\"," +
                "   \"url\" : \"" + siteUrl + "\"" +
                "}";

            return scriptControl;
        }

        private HtmlGenericControl GetVisualWebisteScriptControl()
        {
            HtmlGenericControl visualWebisteScriptControl = new HtmlGenericControl("script");
            visualWebisteScriptControl.Attributes.Add("type", "text/javascript");
            visualWebisteScriptControl.InnerHtml =
                @"var _vwo_code=(function(){
            var account_id=147419,
            settings_tolerance=2000,
            library_tolerance=2500,
            use_existing_jquery=false,
            // DO NOT EDIT BELOW THIS LINE
            f=false,d=document;
            return{use_existing_jquery:function(){return use_existing_jquery;},
            library_tolerance:function(){return library_tolerance;},
            finish:function(){if(!f){f=true;var a=d.getElementById('_vis_opt_path_hides');
            if(a)a.parentNode.removeChild(a);}},finished:function(){return f;},
            load:function(a){var b=d.createElement('script');b.src=a;b.type='text/javascript';
            b.innerText;b.onerror=function(){_vwo_code.finish();};
            d.getElementsByTagName('head')[0].appendChild(b);},
            init:function(){settings_timer=setTimeout('_vwo_code.finish()',settings_tolerance);
            var a=d.createElement('style'),b='body{opacity:0 !important;filter:alpha(opacity=0) !important;background:none !important;}',
            h=d.getElementsByTagName('head')[0];a.setAttribute('id','_vis_opt_path_hides');
            a.setAttribute('type','text/css');if(a.styleSheet)a.styleSheet.cssText=b;
            else a.appendChild(d.createTextNode(b));h.appendChild(a);
            this.load('//dev.visualwebsiteoptimizer.com/j.php?a='+account_id+'&u='+encodeURIComponent(d.URL)+'&r='+Math.random());
            return settings_timer;}};}());_vwo_settings_timer=_vwo_code.init();</script>";

            return visualWebisteScriptControl;
        }

        public void AddCssFile(string page)
        {
            HtmlLink cssLink = new HtmlLink();
            cssLink.Href = WebConfig.CssJsPath + "/content/" + page + ".css?ver=" + WebConfig.WEB_cssVersion;
            cssLink.Attributes.Add("rel", "stylesheet");
            cssLink.Attributes.Add("type", "text/css");
            this.Page.Header.Controls.Add(cssLink);
        }

        public void AddJsFile(string page)
        {
            HtmlGenericControl scriptControl = new HtmlGenericControl("script");
            scriptControl.Attributes.Add("type", "text/javascript");
            scriptControl.Attributes.Add("src", WebConfig.CssJsPath + "/Scripts/" + page + ".js?ver=" + WebConfig.WEB_cssVersion);
            this.Page.Header.Controls.Add(scriptControl);
        }

        public void AddProductJs()
        {
            if (!string.IsNullOrEmpty(WebConfig.ABTestingKey))
            {
                HtmlGenericControl googleScriptHtmlControl = new HtmlGenericControl("script");
                googleScriptHtmlControl.InnerHtml = "function utmx_section(){}function utmx(){}(function(){var k='" + WebConfig.ABTestingKey + "',d=document,l=d.location,c=d.cookie; if(l.search.indexOf('utm_expid='+k)>0)return; function f(n){if(c){var i=c.indexOf(n+'=');if(i>-1){var j=c.indexOf(';',i);return escape(c.substring(i+n.length+1,j<0?c.length:j))}}}var x=f('__utmx'),xx=f('__utmxx'),h=l.hash;d.write('<sc'+'ript src=\"'+'http'+(l.protocol=='https:'?'s://ssl':'://www')+'.google-analytics.com/ga_exp.js?'+'utmxkey='+k+'&utmx='+(x?x:'')+'&utmxx='+(xx?xx:'')+'&utmxtime='+new Date().valueOf()+(h?'&utmxhash='+escape(h.substr(1)):'')+'\" type=\"text/javascript\" charset=\"utf-8\"><\\/sc'+'ript>')})();</script><script>utmx('url','A/B');";
                this.Page.Header.Controls.Add(googleScriptHtmlControl);
            }
        }

        public void SetSpaceID(int spaceID)
        {
            SPACE_ID = spaceID;
        }

        //protected void footerSubmit_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        HotterWindsDBA.HW_Newsletter_Email newsletter = new HotterWindsDBA.HW_Newsletter_Email();
        //        newsletter.EmailAddress = this.Request.Form["email"];
        //        newsletter.Save();

        //        this.Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "temp", "<script>alert(\"Successfully signed up to the newsletter\");</script>");
        //    }
        //    catch
        //    {
        //        this.Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "temp", "<script>alert(\"Sign up failed\");</script>");
        //    }
        //}
    }
}