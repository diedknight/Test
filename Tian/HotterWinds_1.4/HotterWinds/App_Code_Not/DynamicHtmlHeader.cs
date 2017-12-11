using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;

/// <summary>
/// Summary description for DynamicHtmlHeader
/// </summary>
public static class DynamicHtmlHeader
{
    public static void SetHtmlHeader(string keyword, string description,Page page)
    {
        if (page.Request.RawUrl.ToLower().Contains("catalog") || page.Request.RawUrl.Contains("p-") || page.Request.RawUrl.ToLower().Contains("full-"))
        {
            if (!page.Request.RawUrl.Contains("p-"))
                keyword = ConfigurationManager.AppSettings["metaKeywords"].ToString() + ", " + keyword.Replace(",", ", ");
            //else
            //{
            //    HtmlLink link = new HtmlLink();
            //    link.Attributes.Add("rel", "microsummary");
            //    link.Href = Resources.Resource.Global_HomePageUrl + "/ProductMicrosummary.xml";
            //    page.Header.Controls.Add(link);
            //}
        }

        if (page.Request.RawUrl.Contains("EmailFriend") || page.Request.RawUrl.ToLower().Contains("login") || page.Request.RawUrl.ToLower().Contains("retailerproductalert") || page.Request.RawUrl.ToLower().Contains("/410.")
            || page.Request.RawUrl.ToLower().Contains("productnotfound") || page.Request.RawUrl.ToLower().Contains("contactus") || page.Request.RawUrl.ToLower().Contains("/404.")
            || page.Request.RawUrl.ToLower().Contains("register.aspx") || page.Request.RawUrl.ToLower().Contains("register2.aspx") || page.Request.RawUrl.ToLower().Contains("reportlocation.aspx") || page.Request.RawUrl.ToLower().Contains("dealpostback.aspx")
           || page.Request.RawUrl.ToLower().Contains("feed-file-specs") )
        {
            HtmlMeta mate = new HtmlMeta();
            mate.Name = "robots";
            mate.Content = "noindex, nofollow";
            page.Header.Controls.Add(mate);
        }
        if (page.Request.RawUrl.ToLower().Contains("/pe-") || page.Request.RawUrl.Contains("404_1"))
        {
            HtmlMeta mate = new HtmlMeta();
            mate.Name = "robots";
            mate.Content = "noindex";
            page.Header.Controls.Add(mate);
        }
        
        //Encode/Content type
        if (!ClearMeta(page, "Content-Type"))
        {
            HtmlMeta encode = new HtmlMeta();
            encode.HttpEquiv = "Content-Type";
            encode.Content = "text/html;charset=utf-8";
            page.Header.Controls.Add(encode);
        }

        //Language
        //if (!ClearMeta(page, "Content-Language"))
        //{
        //    HtmlMeta lang = new HtmlMeta();
        //    lang.HttpEquiv = "Content-Language";
        //    lang.Content = Resources.Resource.TextString_Culture;
        //    page.Header.Controls.Add(lang);
        //}

        //Description
        if (!ClearMeta(page, "description"))
        {
            HtmlMeta desc = new HtmlMeta();
            desc.Name = "Description";
            desc.Content = description.Replace("<b>", "").Replace("</b>", "");
            desc.Content = description.Replace("PriceMe", "hotterwinds");
            desc.Content = description.Replace("priceme", "hotterwinds");
            desc.Content = description.Replace("Price Me", "hotterwinds");
            page.Header.Controls.Add(desc);
        }

        //Keyword
        if (!ClearMeta(page, "keywords"))
        {
            HtmlMeta keywords = new HtmlMeta();
            keywords.Name = "keywords";
            keywords.Content = keyword.ToLower();
            page.Header.Controls.Add(keywords);
        }

        //<meta name="apple-itunes-app" content="app-id=311507490, affiliate-data=partnerId=30&siteID=k1CkFsOh4nQ"/>
        //if (ConfigurationManager.AppSettings["appid"] != null && !ClearMeta(page, "apple-itunes-app"))
        //{
        //    HtmlMeta app = new HtmlMeta();
        //    app.Name = "apple-itunes-app";
        //    app.Content = "app-id=" + ConfigurationManager.AppSettings["appid"];
        //    page.Header.Controls.Add(app);
        //}
    }

    public static bool ClearMeta(Page page, string content)
    {
        bool isContain = false;
        foreach (Control hm1 in page.Header.Controls)
        {
            if (hm1 is HtmlMeta && (((HtmlMeta)hm1).Name.ToLower() == content || ((HtmlMeta)hm1).HttpEquiv == content))
            {
                //page.Header.Controls.Remove(hm1);
                isContain = true;
                break;
            }
        }

        return isContain;
    }

    public static void SetFaceBookHeader(string title, string type, string imageURL, string url, string des, string amount, string currency, Page page)
    {
        HtmlMeta mateFBtitle = new HtmlMeta();
        mateFBtitle.Attributes.Add("property", "og:title");
        mateFBtitle.Content = title;
        page.Header.Controls.Add(mateFBtitle);

        HtmlMeta mateFBtype = new HtmlMeta();
        mateFBtype.Attributes.Add("property", "og:type");
        mateFBtype.Content = type;
        page.Header.Controls.Add(mateFBtype);

        HtmlMeta mateFBimageURL = new HtmlMeta();
        mateFBimageURL.Attributes.Add("property", "og:image");
        mateFBimageURL.Content = imageURL;
        page.Header.Controls.Add(mateFBimageURL);

        HtmlMeta mateFBimageW = new HtmlMeta();
        mateFBimageW.Attributes.Add("property", "og:image:width");
        mateFBimageW.Content = "200";
        page.Header.Controls.Add(mateFBimageW);

        HtmlMeta mateFBimageH = new HtmlMeta();
        mateFBimageH.Attributes.Add("property", "og:image:height");
        mateFBimageH.Content = "200";
        page.Header.Controls.Add(mateFBimageH);

        HtmlMeta mateFBurl = new HtmlMeta();
        mateFBurl.Attributes.Add("property", "og:url");
        mateFBurl.Content = url.Replace("www.priceme", "hotterwinds");
        page.Header.Controls.Add(mateFBurl);

        //HtmlMeta mateFBsiteName = new HtmlMeta();
        //mateFBsiteName.Attributes.Add("property", "og:site_name");
        //mateFBsiteName.Content = "PriceMe " + Resources.Resource.Country;
        //page.Header.Controls.Add(mateFBsiteName);

        if (!string.IsNullOrEmpty(des))
        {
            HtmlMeta mateDes = new HtmlMeta();
            mateDes.Attributes.Add("property", "og:description");
            mateDes.Content = des;
            page.Header.Controls.Add(mateDes);
        }

        if (!string.IsNullOrEmpty(amount))
        {
            HtmlMeta mateAmount = new HtmlMeta();
            mateAmount.Attributes.Add("property", "og:price:amount");
            mateAmount.Content = amount;
            page.Header.Controls.Add(mateAmount);
        }

        if (!string.IsNullOrEmpty(currency))
        {
            HtmlMeta mateCurrency = new HtmlMeta();
            mateCurrency.Attributes.Add("property", "og:price:currency");
            mateCurrency.Content = currency;
            page.Header.Controls.Add(mateCurrency);
        }

        HtmlMeta mateFBadmin = new HtmlMeta();
        mateFBadmin.Attributes.Add("property", "fb:app_id");
        mateFBadmin.Content = System.Configuration.ConfigurationManager.AppSettings["FacebookAppID"];
        page.Header.Controls.Add(mateFBadmin);
    }

    public static void SetFaceBookHeaderForVideo(string pid, string imageUrl, Page page)
    {
        HtmlMeta mateFBvideo = new HtmlMeta();
        mateFBvideo.Attributes.Add("property", "og:video");
        mateFBvideo.Content = "https://api.treepodia.com/video/compact.swf?video_url=http://api.treepodia.com/video/get/"+PriceMe.WebConfig.UUID +"/"+pid;
        page.Header.Controls.Add(mateFBvideo);
        
        HtmlMeta mateFBvideoType = new HtmlMeta();
        mateFBvideoType.Attributes.Add("property", "og:video:type");
        mateFBvideoType.Content = "application/x-shockwave-flash";
        page.Header.Controls.Add(mateFBvideoType);

        HtmlMeta mateFBvideoWidth = new HtmlMeta();
        mateFBvideoWidth.Attributes.Add("property", "og:video:width");
        mateFBvideoWidth.Content = "640";
        page.Header.Controls.Add(mateFBvideoWidth);

        HtmlMeta mateFBvideoHeight = new HtmlMeta();
        mateFBvideoHeight.Attributes.Add("property", "og:video:height");
        mateFBvideoHeight.Content = "360";
        page.Header.Controls.Add(mateFBvideoHeight);

        HtmlMeta mateFBmedium = new HtmlMeta();
        mateFBmedium.Attributes.Add("name", "medium");
        mateFBmedium.Content = "video";
        page.Header.Controls.Add(mateFBmedium);

        HtmlMeta mateFBvideo_height = new HtmlMeta();
        mateFBvideo_height.Attributes.Add("name", "video_height");
        mateFBvideo_height.Content = "640";
        page.Header.Controls.Add(mateFBvideo_height);

        HtmlMeta mateFBvideo_width = new HtmlMeta();
        mateFBvideo_width.Attributes.Add("name", "video_width");
        mateFBvideo_width.Content = "360";
        page.Header.Controls.Add(mateFBvideo_width);

        HtmlMeta mateFBvideo_type = new HtmlMeta();
        mateFBvideo_type.Attributes.Add("name", "video_type");
        mateFBvideo_type.Content = "application/x-shockwave-flash";
        page.Header.Controls.Add(mateFBvideo_type);

    }

    public static void SetFaceBookHeaderForVideoDes(string pid,string pName,string pDescription,string imageUrl ,string type,Page page)
    {
        HtmlMeta mateFBtitle = new HtmlMeta();
        mateFBtitle.Attributes.Add("itemprop", "name");
        mateFBtitle.Content = pName;
        page.Header.Controls.Add(mateFBtitle);

        HtmlMeta mateFBdes = new HtmlMeta();
        mateFBdes.Attributes.Add("itemprop", "description");
        mateFBdes.Content = pDescription;
        page.Header.Controls.Add(mateFBdes);

        HtmlMeta mateFBthumbnailUrl = new HtmlMeta();
        mateFBthumbnailUrl.Attributes.Add("itemprop", "thumbnailUrl");
        mateFBthumbnailUrl.Content = imageUrl;
        page.Header.Controls.Add(mateFBthumbnailUrl);

        HtmlMeta mateFBcontentURL = new HtmlMeta();
        mateFBcontentURL.Attributes.Add("itemprop", "contentURL");
        mateFBcontentURL.Content = "https://api.treepodia.com/video/get/" + PriceMe.WebConfig.UUID + "/" + pid;
        page.Header.Controls.Add(mateFBcontentURL);

        HtmlMeta mateFBembedURL = new HtmlMeta();
        mateFBembedURL.Attributes.Add("itemprop", "embedURL");
        mateFBembedURL.Content = "https://api.treepodia.com/video/compact.swf?video_url=http://api.treepodia.com/video/get/" + PriceMe.WebConfig.UUID + "/" + pid;
        page.Header.Controls.Add(mateFBembedURL);

        HtmlMeta mateFBvideo = new HtmlMeta();
        mateFBvideo.Attributes.Add("property", "og:video");
        mateFBvideo.Content = "https://api.treepodia.com/video/compact.swf?video_url=http://api.treepodia.com/video/get/" + PriceMe.WebConfig.UUID + "/" + pid;
        page.Header.Controls.Add(mateFBvideo);

        HtmlMeta mateFBvideoType = new HtmlMeta();
        mateFBvideoType.Attributes.Add("property", "og:video:type");
        mateFBvideoType.Content = "application/x-shockwave-flash";
        page.Header.Controls.Add(mateFBvideoType);

        HtmlMeta mateFBvideoWidth = new HtmlMeta();
        mateFBvideoWidth.Attributes.Add("property", "og:video:width");
        mateFBvideoWidth.Content = "640";
        page.Header.Controls.Add(mateFBvideoWidth);

        HtmlMeta mateFBvideoHeight = new HtmlMeta();
        mateFBvideoHeight.Attributes.Add("property", "og:video:height");
        mateFBvideoHeight.Content = "360";
        page.Header.Controls.Add(mateFBvideoHeight);

        HtmlMeta mateFBtype = new HtmlMeta();
        mateFBtype.Attributes.Add("property", "og:type");
        mateFBtype.Content = type;
        page.Header.Controls.Add(mateFBtype);
    }

    public static void SetPaginationHeader(string prevURL, string nextURL, Page page)
    {
        if (page.Header == null) return;
        if (!string.IsNullOrEmpty(prevURL))
        {
            HtmlLink link = new HtmlLink();
            link.Attributes.Add("rel", "prev");
            link.Href = prevURL;
            page.Header.Controls.Add(link);
        }

        if (!string.IsNullOrEmpty(nextURL))
        {
            HtmlLink link = new HtmlLink();
            link.Attributes.Add("rel", "next");
            link.Href = nextURL;
            page.Header.Controls.Add(link);
        }
    }
}