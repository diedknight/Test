<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProductDescription.ascx.cs" Inherits="HotterWinds.Modules.Products.ProductDescription" %>
<%@ Import Namespace="PriceMeCommon.BusinessLogic" %>

<style type="text/css">
    #specs {
        border: none;
    }

        #specs .rich-right-attr {
            font-style:oblique;   
        }
</style>

<script type="text/javascript">

    $(function () {
        $('[data-toggle="popover"]').popover({ html: true });
        $('[data-toggle="tooltip"]').tooltip();
        setTableWidthPD();
    })
    $(window).resize(function () {
        setTableWidthPD();
    });
</script>
<%
    int videoCount = 0;

    var Videos = ProductController.GetProductVideos(product.ProductID, PriceMe.WebConfig.CountryId);
    if (Videos != null)
        videoCount = Videos.Count;

    if (isDisplay)
    { %>

<div id="specs" class="panel panel-default">
    <a id="pspecs"></a>
<%--    <div class="panel-heading">
        <h3><span class="glyphicon glyphicon-info-sign"></span><%=Resources.Resource.Product_Specifications%></h3>
    </div>--%>
    <div class="panel-body fontlarger">
        <%
            var adt = AttributesController.AllAttributeDisplayType_Static;
            var acc = AttributesController.AllAttributeCategoryComparison_Static;

            var desc = ProductController.GetDescriptionAndAttribute(product.ProductID);

            int i = 30;
            foreach (PriceMeCache.AttributeGroup pair in pdas)
            {
        %>
        <div class="divAttGroup">
            <div class="divAttGroupName">
                <span class="desSpan"><%=pair.AttributeGroupName %>
                </span>
            </div>
            <div class="divAttGroupContent">
                <ul>
                    <%
                        System.Collections.Generic.Dictionary<string, string> pms = new System.Collections.Generic.Dictionary<string, string>();
                        foreach (PriceMeCache.AttributeGroupList pd in pair.AttributeGroupList)
                        {
                            string title = pd.AttributeName == null ? "" : pd.AttributeName;
                            string translationTilte = AttributesController.GetAttNameTranslations(title, PriceMe.WebConfig.CountryId);
                            if (!string.IsNullOrEmpty(translationTilte))
                                title = translationTilte;

                            string sdesc = pd.ShortDescription == null ? "" : pd.ShortDescription;
                            string value = pd.Value == null ? "" : pd.Value;
                            if (pd.AttributeTypeID == 2)
                            {
                                if (value.ToLower() == "yes")
                                    value = "<span class=\"glyphicon glyphicon-ok colorPros\"></span>";
                                else if (value.ToLower() == "no")
                                    value = "<span class=\"glyphicon glyphicon-remove colorCros\"></span>";
                            }

                            if (string.IsNullOrEmpty(value))
                                continue;

                            string unit = pd.Unit == null ? "" : pd.Unit;
                            int t = pd.T;
                            int avs = pd.Avs;
                            int cid = product.CategoryID ?? 0;

                            int tt = 0;
                            PriceMeCache.ProductDescAndAttr dnt = null;
                            var dnts = desc.Where(s => s.AttributeTitleID == pd.AttributeId).ToList();
                            if (dnts != null && dnts.Count > 0)
                            {
                                dnt = new PriceMeCache.ProductDescAndAttr();
                                dnt = dnts[0];
                            }
                            if (dnt != null)
                                tt = dnt.T;

                            string url = "";
                            if (t == 1)
                            {
                                pms.Clear();
                                pms.Add("c", cid.ToString());
                                pms.Add("avs", avs.ToString());
                                url = PriceMe.UrlController.GetRewriterUrl(PriceMe.PageName.Catalog, pms);//Utility.GetRewriterUrl("catalog", pms);
                            }
                    %>
                    <li class="attLi">

                        <% if (pair.AttributeGroupName == "Energy Star")
                            {%>

                        <div class="rich-left-attr"><span class="attTitle">Energy star</span></div>

                        <div class="rich-right-attr">
                            <img src="<%=Resources.Resource.ImageWebsite%>/Images/RetailerImages/energystar-logo-full225.png" width="20" height="20" alt="Energy Star" />
                            <%= value%><%= unit%></div>


                        <%}
                            else
                            {%>

                        <%if (title.ToLower() == "energy star")
                            { %>
                        <div class="rich-left-attr">
                        </div>
                        <div class="rich-right-attr">
                            <div class="desEnergystar">
                                <div class="desEsImg">
                                    <img src="<%=Resources.Resource.ImageWebsite%>/Images/RetailerImages/energystar-logo-full225.png" width="20" height="20" alt="Energy Star" />
                                </div>
                                <div class="desEsValue"><%= value%></div>
                            </div>
                        </div>
                        <%}
                        else
                        { %>
                        <div class="rich-left-attr">
                            <%--<span class="attTitle" aid="<%=pd.AttributeId %>" com="<%=tt %>" ><%= title%></span>--%>
                            <span class="attTitle"><%= title%></span>
                            <% if (!string.IsNullOrEmpty(sdesc))
                                {
                                    i--;%>
                            <div class="helpTopicDiv bg1" data-placement="top" data-original-title="<%= sdesc.Replace("\"", " ")%>" data-toggle="tooltip" style="vertical-align: middle; z-index: <%=i%>;">
                            </div>
                            <% } %>
                        </div>

                        <div class="rich-right-attr">
                            <%

                                bool isCom = tt == 1 ? true : false;
                                var listrich = adt.Where(s => s.AttributeID == pd.AttributeId && s.IsCompareAttribute == isCom).ToList();
                                bool isRich = false;
                                if (listrich != null && listrich.Count > 0)
                                    isRich = true;

                                string rich_val = value + "&nbsp;" + unit;

                                if (isRich)
                                {
                                    var rich = listrich[0];
                                    var richs = acc.Where(s => s.Aid == rich.AttributeID && s.IsCompareAttribute == isCom).ToList();
                                    PriceMeCache.AttributeCategoryComparison rich_result = null;
                                    if (richs != null && richs.Count > 0)
                                        rich_result = richs[0];

                                    if (rich_result == null)
                                    {
                            %>
                        </div>
                        <div style="clear: both;"></div>
                    </li>

                    <%
                            continue;
                        }

                        double cur_val = 0;

                        double.TryParse(value, out cur_val); double av_val = double.Parse(rich_result.Average);

                        var isHigherOrLower = ((cur_val - av_val) * 1.0) / av_val;

                        var isLower = isHigherOrLower <= 0 ? rich.DisplayAdjectiveWorse : rich.DisplayAdjectiveBetter;
                        if (!rich_result.IsHigherBetter)
                            isLower = isHigherOrLower <= 0 ? rich.DisplayAdjectiveBetter : rich.DisplayAdjectiveWorse;

                        string rich_desc = "";

                        isHigherOrLower = Math.Ceiling(isHigherOrLower * 100) < 0 ? -Math.Ceiling(isHigherOrLower * 100) : Math.Ceiling(isHigherOrLower * 100);

                        var istop = getRankTxt(rich_result, cur_val);


                        rich_desc = getRankDesc(title, isHigherOrLower, isLower, istop);

                        double tiaoWidth;
                        if (rich.TypeID == 1)
                        {
                            tiaoWidth = (cur_val * 1.0 / double.Parse(rich_result.Bottom10)) * 100;
                            if (!rich_result.IsHigherBetter)
                            {
                                if (tiaoWidth == 100)
                                    tiaoWidth = 10;
                                else
                                    tiaoWidth = tiaoWidth > 100 ? (tiaoWidth - 100) : tiaoWidth;
                            }
                            tiaoWidth = tiaoWidth >= 100 ? 98 : tiaoWidth;
                        }
                        else if (rich.TypeID != 4 && rich.TypeID != 5)
                        {
                            tiaoWidth = (cur_val * 1.0 / double.Parse(rich_result.Top10)) * 100;
                            tiaoWidth = tiaoWidth >= 100 ? 93 : tiaoWidth;
                        }
                        else
                        {
                            double t10 = double.Parse(rich_result.Top10);
                            if (cur_val >= t10)
                            {
                                tiaoWidth = 90.00d;
                            }
                            else
                            {
                                tiaoWidth = cur_val / t10 * 100;
                            }
                            tiaoWidth = tiaoWidth >= 100 ? 98 : tiaoWidth;
                        }
                    %>
                    <%=PriceMe.Utility.switchRichDisplay(rich.TypeID,rich_val,tiaoWidth,cur_val, rich_result.IsHigherBetter) %>

                    <% if (isHigherOrLower > 0 && !string.IsNullOrEmpty(istop))
                        {
                            string richtop = string.Empty;
                            if (rich.TypeID == 1)
                            {
                                richtop = "top: 0px;";
                            }
                            else if (rich.TypeID == 7)
                                richtop = "top: -40px;";
                            else if (rich.TypeID == 8)
                                richtop = "top: -90px;";%>
                    <div data-container="body" data-toggle="popover" title="<%=title+" comparison" %>" style="position: relative; line-height: 23px; width: 110px; font-size: 13px; <%=richtop %>" data-placement="top" data-content="<%= rich_desc%>" class="rich-button tempAdd-button"><%=istop %></div>
                    <%} %>

                    <%}
                    else
                    {%>

                    <%= value%>&nbsp;<%= unit%>

                    <%} %>
            </div>
            <%}%>
            <%} %>

            <div style="clear: both;"></div>
            </li>
        <%} %>
                </ul>
        </div>
        <div class="clr"></div>
    </div>
    <%} %>

    <%
        string phy = GetPhysical();
        if (phy != "")
        {
    %>
    <div class="divAttGroup">
        <div class="divAttGroupName"><span class="desSpan"><%=Resources.Resource.TextString_PhysicalSpecs%></span></div>
        <div class="divAttGroupContent">
            <ul>
                <%=phy %>
            </ul>
        </div>
        <div class="clr"></div>
    </div>
    <%} %>

    <div class="divAttGroup">
        <div class="divAttGroupName"><span class="desSpan">Product info</span></div>
        <div class="divAttGroupContent">
            <ul>

                <%if (ManufacturerId > 0 && !string.IsNullOrEmpty(ManufacturerProductUrl))
                    { %>
                <li style="margin-bottom: 6px;">
                    <div class="rich-left-attr">
                        <span class="attTitle">
                            <%=Resources.Resource.TextString_Manufacturer %></span>
                    </div>
                    <div class="rich-right-attr">
                        <a href="<%=ManufacturerProductUrl %>" target="_blank">
                            <%=ManufacturerName%> website</a> <span class="glyphicon glyphicon-new-window iconGray"></span>
                    </div>

                    <div style="clear: both;"></div>
                </li>
                <%} %>



                <%if (videoCount > 0)
                    { %>

                <li style="margin-bottom: 6px;">
                    <div class="rich-left-attr">
                        <span class="attTitle">Product videos</span>
                    </div>
                    <div class="rich-right-attr">
                        <span><a target="_blank" onclick="javascript:ShowProductImageVideos(<%=product.ProductID %>,'iv','video');" data-toggle="modal" data-target="#popUpimgModal" href="#">
                            <span style="color: gray; position: relative; top: 3px;" class="glyphicon glyphicon-facetime-video"></span>&nbsp;Product videos </a><span style="color: gray;">(<%=videoCount %>)</span>
                        </span>

                    </div>

                    <div style="clear: both;"></div>
                </li>


                <%} %>


                <li>
                    <div class="rich-left-attr">
                        <span class="attTitle">List date PriceMe </span>
                    </div>
                    <div class="rich-right-attr">
                        <%=GetProductDate()%>
                    </div>

                    <div style="clear: both;"></div>
                </li>


            </ul>
        </div>
        <div class="clr"></div>
    </div>




</div>
</div>
<%}
    else
    {%>
<div id="specs" class="panel panel-default">
    <a id="pspecs"></a>
    <div class="panel-body">
        <div class="divAttGroup">
            <div class="divAttGroupName"><span class="desSpan">Product info</span></div>
            <div class="divAttGroupContent">
                <ul>

                    <%if (ManufacturerId > 0 && !string.IsNullOrEmpty(ManufacturerProductUrl))
                        { %>
                    <li style="margin-bottom: 6px;">
                        <div class="rich-left-attr">
                            <span class="attTitle">
                                <%=Resources.Resource.TextString_Manufacturer %></span>
                        </div>
                        <div class="rich-right-attr">
                            <a href="<%=ManufacturerProductUrl %>" target="_blank">
                                <%=ManufacturerName%> website</a> <span class="glyphicon glyphicon-new-window iconGray"></span>
                        </div>

                        <div style="clear: both;"></div>
                    </li>
                    <%} %>



                    <%if (videoCount > 0)
                        { %>

                    <li style="margin-bottom: 6px;">
                        <div class="rich-left-attr">
                            <span class="attTitle">Product videos</span>
                        </div>
                        <div class="rich-right-attr">
                            <span><a target="_blank" onclick="javascript:ShowProductImageVideos(<%=product.ProductID %>,'iv','video');" data-toggle="modal" data-target="#popUpimgModal" href="#">
                                <span style="color: gray; position: relative; top: 3px;" class="glyphicon glyphicon-facetime-video"></span>&nbsp;Product videos </a><span style="color: gray;">(<%=videoCount %>)</span>
                            </span>

                        </div>

                        <div style="clear: both;"></div>
                    </li>


                    <%} %>


                    <li>
                        <div class="rich-left-attr">
                            <span class="attTitle">List date PriceMe</span>
                        </div>
                        <div class="rich-right-attr">
                            <%=GetProductDate()%>
                        </div>

                        <div style="clear: both;"></div>
                    </li>


                </ul>
            </div>
            <div class="clr"></div>
        </div>
    </div>
</div>
<%} %>
