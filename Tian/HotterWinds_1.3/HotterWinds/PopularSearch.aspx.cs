using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PriceMe;
using PriceMeCommon.BusinessLogic;
using PriceMeCommon.Extend;
using PriceMeCommon.Data;

namespace HotterWinds
{
    public partial class PopularSearch : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string input = Utility.GetParameter("q").Replace("-", " ").Trim();
            string type = Utility.GetParameter("t").Trim();
            string reqType = Utility.GetParameter("type");
            string pids = Utility.GetParameter("pids");
            //input = Regex.Replace(input, @"\p{P}+", " ");//删除所有符号

            if (input.Length <= 1)
            {
                Response.End();
                return;
            }

            List<string> ks = new List<string>();
            List<string> ks2 = new List<string>();
            List<string> ks3 = new List<string>();
            List<string> ks4 = new List<string>();
            List<string> ksR = new List<string>();

            string trackUrlFormat = "/SearchSuggest/type={0}";
            try
            {
                Dictionary<string, LinkInfo> ps, cs, bs, bac, rs;
                List<LinkInfo> otherInfos;
                PopularSearcherController.GetSuggestKeywords(null, input.ToLower(), 15, 5, 5, false, out ps, out cs, out bs, out bac, out rs, out otherInfos, pids, PriceMe.WebConfig.CountryId);
                foreach (string c in cs.Keys)
                {
                    int cid = 0;
                    int.TryParse(c, out cid);
                    if (PriceMeCommon.BusinessLogic.CategoryController.IsSearchOnly(cid, PriceMe.WebConfig.CountryId))
                        continue;

                    string trackUrl = string.Format(trackUrlFormat, "category&cid=" + c);
                    ks2.Add("[\"" + cs[c].LinkText.SafeString() + "\",\"" + FixPopularSearchUrl(UrlController.GetCatalogUrl(int.Parse(c))).SafeString() + "\",\"" + cs[c].LinkText.SafeString() + "\",\"" + trackUrl + "\"]");
                }

                foreach (LinkInfo li in otherInfos)
                {
                    string trackUrl = string.Format(trackUrlFormat, "others");
                    ks2.Add("[\"" + li.LinkText.SafeString() + "\",\"" + FixPopularSearchUrl(li.LinkURL).SafeString() + "\",\"" + li.LinkText.SafeString() + "\",\"" + trackUrl + "\"]");
                }

                Dictionary<string, string> tmp;
                tmp = new Dictionary<string, string>();
                tmp.Add("mid", "");
                foreach (string b in bs.Keys)
                {
                    tmp["mid"] = b;

                    string trackUrl = string.Format(trackUrlFormat, "brand&mid=" + b);
                    ks3.Add("[\"" + bs[b].LinkText.SafeString() + "\",\"" + FixPopularSearchUrl(UrlController.GetRewriterUrl(PageName.Brand, tmp)).SafeString() + "\",\"" + bs[b].LinkText.SafeString() + "\",\"" + trackUrl + "\"]");
                }

                string str = "";
                foreach (string p in ps.Keys)
                {
                    str = ps[p].LinkText;
                    string trackUrl = string.Format(trackUrlFormat, "product&pid=" + p);
                    string imageUrl = Utility.GetImage(ps[p].ImageUrl, "_s");


                    if (reqType == "")
                    {
                        string url = FixPopularSearchUrl(UrlController.GetProductUrl(int.Parse(p), ps[p].LinkText)).SafeString();
                        if (CategoryController.IsSearchOnly(ps[p].CategoryID, PriceMe.WebConfig.CountryId) && ps[p].PPC_RetailerID > 0)
                        {
                            url = FixPopularSearchUrl(Utility.GetRootUrl("/ResponseRedirect.aspx?pid=" + p + "&rid=" + ps[p].PPC_RetailerID + "&rpid=" + ps[p].PPC_RetailerProductID + "&countryID=" + WebConfig.CountryId + "&cid=" + ps[p].CategoryID + "&t=s", WebConfig.CountryId)).SafeString();
                            string uuid = Guid.NewGuid().ToString();
                            url += "&uuid=" + uuid;
                        }

                        ks.Add("[\"" + str.SafeString() + "\",\"" + url + "\",\"" + ps[p].Value.SafeString() + "\",\"" + trackUrl + "\",\"" + ps[p].Title + "\",\"" + imageUrl.SafeString() + "\"]");
                    }
                    else
                    {
                        string proids = string.Empty;
                        var threePid = pids.Split(',');

                        List<string> pidList = new List<string>();
                        string pidStr = "";
                        foreach (var pid in threePid)
                            if (!string.IsNullOrEmpty(pid))
                            {
                                pidList.Add(pid);
                                pidStr += pid + ",";
                            }

                        threePid = pidList.ToArray();

                        if (threePid.Length >= 3)
                        {
                            threePid[2] = p;
                            for (int i = 0; i < 3; i++)
                            {
                                proids += threePid[i] + ",";
                            }
                        }
                        else
                            proids += pidStr.TrimEnd(',') + "," + p + ",";

                        proids = proids.TrimEnd(',');

                        ks.Add("[\"" + str.SafeString() + "\",\"/Compare.aspx?t=js&pids=" + proids + "\",\"" + ps[p].Value.SafeString() + "\",\"" + trackUrl + "\",\"" + ps[p].Title + "\",\"" + imageUrl.SafeString() + "\"]");
                    }

                }

                tmp = new Dictionary<string, string>();
                tmp.Add("m", "");
                tmp.Add("c", "");
                foreach (string bc in bac.Keys)
                {
                    //tmp["m"] = bc;
                    string[] cInfos = bc.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                    if (cInfos.Length > 1)
                    {
                        tmp["m"] = cInfos[0];
                        tmp["c"] = cInfos[1];

                        int cid = 0;
                        int.TryParse(cInfos[1], out cid);
                        if (CategoryController.IsSearchOnly(cid, PriceMe.WebConfig.CountryId))
                            continue;

                        string trackUrl = string.Format(trackUrlFormat, "brandAndCategory&mid=" + cInfos[0] + "&cid=" + cInfos[1]);
                        ks4.Add("[\"" + bac[bc].LinkText.SafeString() + "\",\"" + FixPopularSearchUrl(UrlController.GetRewriterUrl(PageName.Catalog, tmp)).SafeString() + "\",\"" + bac[bc].Value + "\",\"" + trackUrl + "\"]");
                    }
                }

                foreach (string r in rs.Keys)
                {
                    int rid = int.Parse(r);
                    string trackUrl = string.Format(trackUrlFormat, "retaielr&rid=" + r);
                    rs[r].Value = GetNewRetailerRatingValue(rs[r].Value);
                    ksR.Add("[\"" + rs[r].LinkText.SafeString() + "\",\"" + FixPopularSearchUrl(UrlController.RetailerInfoUrl(rid)).SafeString() + "\",\"" + rs[r].Value.SafeString() + "\",\"" + trackUrl + "\"]");
                }
            }
            catch (Exception ex)
            {
                PriceMeCommon.BusinessLogic.LogController.WriteException("Popularsearch: " + ex.Message + ex.StackTrace);
            }

            if (type == "p")
            {//plugin
                Response.Write(string.Format("[\"{0}\",{1}]", input, GetOpenSearchSuggest(ks)));
            }
            else
            {
                if (reqType == "")
                    Response.Write("window.SuggestHelper.BuildSuggest(\"" + input.SafeString() + "\",{p:[");
                else
                    Response.Write("window.SuggestHelper2.BuildSuggest(\"" + input.SafeString() + "\",{p:[");


                Response.Write(string.Join(",", ks.ToArray()));
                Response.Write("],c:[");
                Response.Write(string.Join(",", ks2.ToArray()));
                Response.Write("],b:[");
                Response.Write(string.Join(",", ks3.ToArray()));
                Response.Write("],bac:[");
                Response.Write(string.Join(",", ks4.ToArray()));
                Response.Write("],r:[");
                Response.Write(string.Join(",", ksR.ToArray()));
                Response.Write("]})");
            }
            //Response.End();
        }

        private string GetNewRetailerRatingValue(string value)
        {
            if (!value.Contains("star_"))
                return "0";
            string nv = value.Split(new string[] { "star_" }, StringSplitOptions.RemoveEmptyEntries)[1].Replace(".gif", "");
            double rating = 0;
            if (nv.Contains("h"))
            {
                rating = 0.5;
            }
            rating = double.Parse(nv.Replace("h", "")) + rating;
            return (Utility.GetStarRatingPercent(rating) * 100 - 1).ToString("0.00");
        }

        private string FixPopularSearchUrl(string url)
        {
            if (!url.Contains("/plans/"))
            {
                if (url.Contains("?"))
                {
                    url += "&fp=ps";
                }
                else
                {
                    url += "?fp=ps";
                }
            }

            return url;
        }

        private string GetOpenSearchSuggest(List<string> stringList)
        {
            string suggestString = "[{0}],[{1}],[{2}]";
            string searchTerms = "";
            string descriptions = "";
            string queryURLs = "";

            foreach (string str in stringList)
            {
                string[] info = str.Split(',');
                searchTerms += info[0].Substring(1) + ",";
                descriptions += info[2] + ",";
                queryURLs += info[1].Insert(1, Resources.Resource.Global_HomePageUrl) + ",";
            }

            suggestString = string.Format(suggestString, searchTerms.TrimEnd(','), descriptions.TrimEnd(','), queryURLs.TrimEnd(','));
            return suggestString;
        }
    }
}