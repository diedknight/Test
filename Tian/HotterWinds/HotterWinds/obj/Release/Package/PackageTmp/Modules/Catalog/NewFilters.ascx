<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NewFilters.ascx.cs" Inherits="HotterWinds.Modules.Catalog.NewFilters" %>

<aside>

<div id="newfilter">

    <input type="hidden" id="url_Raw" value="<%= Request.RawUrl %>" />
    <input type="hidden" id="swi_pre" value="<%=SearchWithIn %>" />

    <input type="hidden" class="ppr" name="ps" value="<%=PageSize %>" />
    <input type="hidden" class="ppr" name="pg" value="<%=PagePosition %>" />
    <input type="hidden" class="ppr" name="v" value="<%=View %>" />
    <input type="hidden" class="ppr" name="q" value="<%=QueryKeywords %>" />
    <input type="hidden" class="ppr" name="pt" value="<%=PageToName %>" />
    <input type="hidden" class="ppr" name="c" value="<%=CategoryID %>" />
    <input type="hidden" class="ppr" name="dv" value="<%=DefaultView %>" />
    <input type="hidden" class="ppr" name="dsb" value="<%=DefaultSortBy %>" />


    <%  int position = 0;
        int collapseCount = 7;
    %>

    <span class="glyphicon glyphicon-filter"></span><h3 class="filterTitle"><%=Resources.Resource.TextString_Filters %></h3>
    <div id="filterHeader">
        <span id="productCountInfo"><label><%=CurrentProductCount.ToString("N0") %></label> / <%=TotalProductCount.ToString("N0") %> <%=Resources.Resource.TextString_Products %></span>

        <%
            string text = Selections.Count.ToString();
            if (Selections != null && Selections.Count > 0) {
                if(Selections.Count > 1)
                {
                    text += " " + Resources.Resource.TextString_Filters;
                }
                else
                {
                    text += " " + Resources.Resource.TextString_Filter;
                }
                %>
        <%} %>
        <a id="fCountATag" href="<%=UrlNoSelection %>" class="btn btn-primary btn-xs"><span id="fCount"><%=text %></span> <span class="glyphicon glyphicon-remove-circle"></span></a>
    </div>

    <div id="filters">

        <%foreach(var nb in NarrowByInfoList) {
                position++;
                string collapse = "0";
                if(collapseCount < position)
                {
                    collapse = "1";
                }
                %>
        <div class="nbDiv">
            <div class="nbTitle">
                <h4><%=nb.Title %></h4>
                <%if(nb.IsSlider && !nb.Name.Contains("PriceRange") && !nb.Name.Contains("Days")) {%>
                <span class="unit">(<%=nb.NarrowItemList[0].OtherInfo %>)</span>
                <%} %>
                <%if (!string.IsNullOrEmpty(nb.Description))
                                                    {%>
                <div class="helpTopicDiv bg1" data-placement="top" data-original-title="<%= nb.Description.Replace("\"", " ")%>" data-toggle="tooltip" style="vertical-align: middle;">
                </div>
                <%}%>
                <span class="glyphicon glyphicon-minus" data-collapse="<%=collapse %>"></span>
            </div>
            <div class="nbValues">
            <%if (nb.Title.Equals("Categories"))
              {%>
                <div id="filterCategoriesDiv">
                <%
                    var topItems = nb.NarrowItemList.Take(MaxValueCount);
                    foreach(var ti in topItems)
                    {%>
                    
                <div>
                    <a class="nameATag" href="<%=ti.Url %>"><%=ti.DisplayName %></a>
                    <span class="count">(<%=ti.ProductCount %>)</span>
                </div>

                    <%}

                    if (nb.NarrowItemList.Count > MaxValueCount)
                    {%>
                <div class="moreValues">
                    <%
                        for (int i = MaxValueCount; i < nb.NarrowItemList.Count; i++)
                        {%>

                    <div>
                        <a class="nameATag" href="<%=nb.NarrowItemList[i].Url %>"><%=nb.NarrowItemList[i].DisplayName %></a>
                        <span class="count">(<%=nb.NarrowItemList[i].ProductCount %>)</span>
                    </div>

                        <%}
                        %>
                </div>

                <div class="moreButton">
                    <div class="more" data-more="<%=Resources.Resource.TextString_ShowMore %>" data-less="<%=Resources.Resource.TextString_ShowLess %>"><%=Resources.Resource.TextString_ShowLess %></div>
                </div>
                <%
                    }%>
                    </div><%
                        } %>
            <%else if (nb.Name.Contains("PriceRange"))
                {
                    long min = 0;
                    long max = long.Parse(nb.NarrowItemList.Last().Value);
                    long fromIndex = min;
                    long toIndex = max;
                    string sValue = "";

                    List<int> countList = nb.NarrowItemList.Select(p => p.ProductCount).ToList();

                    double av = nb.ProductCountListWithoutP.Average();
                    int maxCountWithoutP = nb.ProductCountListWithoutP.Max();
                    List<int> heightList = new List<int>();
                    foreach (int c in countList)
                    {
                        int h = GetIconHeight(CategoryID, 0, c, av, maxCountWithoutP);
                        heightList.Add(h);
                    }

                    if (MyPriceRange != null)
                    {%>
            <span class="selected"></span>
                    <%
                            fromIndex = (long)MyPriceRange.MinPrice;
                            toIndex = (long)MyPriceRange.MaxPrice;
                            sValue = fromIndex + "-" + toIndex;
                        }
                    %>
            <div id="slider_Price"></div>
            <input type="hidden" class="ppr" id="pr" name="pr" value="<%=sValue %>" />
            <%if (!IsAjax) { %>
            <script>
                createFilterSlider("#slider_Price", "#pr", "int", <%=min%>, <%=max%>, <%=fromIndex%>, <%=toIndex%>, "<%=Resources.Resource.TextString_PriceSymbol%>", 1);

                createHeightBar('slider_Price', new Array(<%=string.Join(",", heightList)%>));
            </script>
            <%}
                else
                { %>

                <%=CreateHeightBarHtml("slider_Price", heightList)%>

            <%} %>
                <%} else if (nb.Name.Equals("Days", StringComparison.InvariantCultureIgnoreCase))
                    {
                        int minValue = int.Parse(nb.NarrowItemList[0].Value);
                        int maxValue = int.Parse(nb.NarrowItemList.Last().Value);
                        int fromIndex = 1;
                        int toIndex = maxValue;
                        string sValue = "";

                        List<int> countList = PriceMe.Utility.GetDaysProductCountList(nb);

                        double av = nb.ProductCountListWithoutP.Average();
                        int maxCountWithoutP = nb.ProductCountListWithoutP.Max();
                        List<int> heightList = new List<int>();
                        foreach (int c in countList)
                        {
                            int h = GetIconHeight(CategoryID, 0, c, av, maxCountWithoutP);
                            heightList.Add(h);
                        }

                        if (MyDaysRange != null && MyDaysRange.MaxDays > 0)
                        {%>
            <span class="selected"></span>
                        <%
                                fromIndex = MyDaysRange.MinDays;
                                toIndex = MyDaysRange.MaxDays;
                                sValue = fromIndex + "-" + toIndex;
                            }
                    %>
            <div id="slider_days"></div>
            <input type="hidden" class="ppr" id="dr" name="dr" value="<%=sValue %>" />
                <%if (!IsAjax)
                    { %>
            <script>
                createFilterSlider("#slider_days", "#dr", "int", <%=minValue%>, <%=maxValue%>, <%=fromIndex%>, <%=toIndex%>, "", 2);

                createHeightBar('slider_days', new Array(<%=string.Join(",", heightList)%>));
            </script>
            <%}
                else
                { %>

                <%=CreateHeightBarHtml("slider_days", heightList)%>

                <%}  %>
            <%} else if (nb.Title == Resources.Resource.TextString_ProductName)
                {%>
                <div class="valueDiv input-group">
                    <input type="search" class="form-control" id="searchWithIn" value="<%=SearchWithIn %>"
                    onkeydown="if(event.keyCode==13){event.keyCode = 9;event.returnValue = false;return loadFilterProducts();}" />
                    <span class="input-group-btn">
                        <button type="button" class="btn btn-sm btn-info btnGray" onclick="return loadFilterProducts();">
                            <span class="glyphicon glyphicon-search"></span>
                        </button>
                    </span>
                </div>
                
                <%if(!string.IsNullOrEmpty(SearchWithIn)){ %>
                <span class="selected"></span>
                <%} %>

                <%}
                    else if(nb.IsSlider)
                    {
                        List<float> valueArray = nb.NarrowItemList.Select(ni => ni.FloatValue).ToList();
                        valueArray.Sort();

                        float minValue = valueArray.First();
                        float maxValue = valueArray.Last();
                        float fromIndex = minValue;
                        float toIndex = maxValue;

                        var map = (PriceMeCache.CategoryAttributeTitleMapCache)nb.CategoryAttributeTitleMap;
                        int titleID = map.AttributeTitleID;

                        string sValue = "";

                        if (!string.IsNullOrEmpty(nb.SelectedValue))
                        {
                            var arr = nb.SelectedValue.Split('|');
                            string[] selectedIndex = arr[0].Split('-');
                            if (selectedIndex.Length > 1)
                            {
                                fromIndex = float.Parse(selectedIndex[0]);
                                toIndex = float.Parse(selectedIndex[1]);
                            }
                        }

                        
                        List<int> countList = PriceMe.Utility.GetAttributeSliderProductCountList_New(nb);
                        double av = nb.ProductCountListWithoutP.Average();
                        int maxCountWithoutP = nb.ProductCountListWithoutP.Max();
                        List<int> heightList = new List<int>();
                        foreach(int c in countList)
                        {
                            int h = GetIconHeight(CategoryID, titleID, c, av, maxCountWithoutP);
                            heightList.Add(h);
                        }
                    %>

                <%if (minValue != fromIndex || maxValue != toIndex)
                    {
                        sValue = titleID + "_" + fromIndex + "-" + toIndex;
                        %>
                <span class="selected"></span>
                <%} %>

                <div id="slider_<%=titleID%>"></div>
                <input type="hidden" class="ppr" id="avsr_<%=titleID%>" name="avsr_<%=titleID%>" value="<%=sValue %>" />
                <%if (!IsAjax)
                    {
                        string sliderStep = PriceMe.Utility.GetSliderStep(nb).ToString("0.0");
                        string sliderStepType = "float";
                        if(sliderStep == "1.0")
                        {
                            sliderStepType = "int";
                        }
                        %>
                <script>
                    createFilterAttributeSlider(<%=titleID%>, "#slider_<%=titleID%>", "#avsr_<%=titleID%>", "<%=sliderStepType%>", <%=minValue%>, <%=maxValue%>, <%=fromIndex%>, <%=toIndex%>, "", <%=sliderStep%>);

                    createHeightBar('slider_<%=titleID%>', new Array(<%=string.Join(",", heightList)%>));
                </script>
                <%}
                else
                { %>
                <%=CreateHeightBarHtml("slider_" + titleID, heightList)%>
                <%} %>
                <%}
                else
                { %>
        
                <%foreach (var nig in nb.NarrowItemGroupDic.Values)
                    {
                        string gIsChecked = "checked";
                        if (!nig.IsSelected)
                        {
                            gIsChecked = "";
                        }
                        %>
                <div class="group">
                    <div class="groupTitle">
                        <input type="checkbox" <%=gIsChecked%> data-url="<%=nig.GroupUrl %>" /><i class="<%=gIsChecked %> checkSpan glyphicon glyphicon-ok"></i><label class="name"><%=nig.GroupName %></label><span class="glyphicon glyphicon-chevron-up"></span><span id="g_<%=nig.GroupID %>" class="count ajRefreshText">(<%=nig.GroupItems.Sum(gi => gi.ProductCount) %>)</span>
                    </div>
                    <ul class="groupValues">
                    <%foreach (var nigi in nig.GroupItems)
                        { 
                            string isChecked = "checked";
                            string url = GetRemoveUrl(nb.Name, nigi.Value);
                            if(string.IsNullOrEmpty(url))
                            {
                                url = nigi.Url;
                                isChecked = "";
                            }
                            %>
                        <li>
                            <input type="checkbox" <%=isChecked%> name="<%=nb.Name %>" value="<%=nigi.Value %>" /><i class="<%=isChecked %> checkSpan glyphicon glyphicon-ok"></i><label class="name"><%=nigi.DisplayName %></label><span id="gv_<%=nigi.Value %>" class="count ajRefreshText">(<%=nigi.ProductCount %>)</span>
                        </li>
                    <%} %>
                    </ul>
                </div>
                <%} %>

                <%
                    int topCount = MaxValueCount - nb.NarrowItemGroupDic.Count;
                    if (topCount > 0)
                    {
                        var top = nb.NarrowItemList.Take(topCount);%>
                    <%foreach (var ni in top)
                    {
                        string isChecked = "checked";
                        string url = GetRemoveUrl(nb.Name, ni.Value);
                        if (string.IsNullOrEmpty(url))
                        {
                            url = ni.Url;
                            isChecked = "";
                    }%>
                <div>
                    <input type="checkbox" <%=isChecked %> name="<%=nb.Name %>" value="<%=ni.Value %>" /><i class="<%=isChecked %> checkSpan glyphicon glyphicon-ok"></i>
                    <%if (ni.DisplayName.Equals("yes", StringComparison.InvariantCultureIgnoreCase))
                        { %>
                        <span class="glyphicon glyphicon-ok colorPros" style="color:green;"></span>
                    <%}
                    else if (ni.DisplayName.Equals("no", StringComparison.InvariantCultureIgnoreCase))
                    { %>
                        <span class="glyphicon glyphicon-remove" style="color:red;"></span>
                    <%}
                    else
                    { %>
                    <label class="name"><%=ni.DisplayName %></label>
                    <%} %>
                    <span id="av_<%=ni.Value %>" class="count ajRefreshText">(<%=ni.ProductCount %>)</span>
                </div>
                    <%}
                }
                else {
                    topCount = 0;
                }%>

                <%if (nb.NarrowItemList.Count > topCount)
                { %>

                <div class="moreValues">
                    <%for (int i = topCount; i < nb.NarrowItemList.Count; i++)
                            {
                                var ni = nb.NarrowItemList[i];
                                string isChecked = "checked";
                                string url = GetRemoveUrl(nb.Name, ni.Value);
                                if (string.IsNullOrEmpty(url))
                                {
                                    url = ni.Url;
                                    isChecked = "";
                                }
                           %>
                    <div>
                        <input type="checkbox" <%=isChecked %> data-url="<%=url%>" name="<%=nb.Name %>" value="<%=ni.Value %>" /><i class="<%=isChecked %> checkSpan glyphicon glyphicon-ok"></i>
                        <%if (ni.DisplayName.Equals("yes", StringComparison.InvariantCultureIgnoreCase))
                        { %>
                        <span class="glyphicon glyphicon-ok colorPros" style="color:red;"></span>
                        <%}
                        else if (ni.DisplayName.Equals("no", StringComparison.InvariantCultureIgnoreCase))
                        { %>
                        <span class="glyphicon glyphicon-remove" style="color:green;"></span>
                        <%}
                        else
                        { %>
                        <label class="name"><%=ni.DisplayName %></label>
                        <%} %>
                        <span id="av_<%=ni.Value %>" class="count ajRefreshText">(<%=ni.ProductCount %>)</span>
                    </div>
                    <%} %>
                </div>

                <div class="moreButton">
                    <div class="more" data-more="<%=Resources.Resource.TextString_ShowMore %>" data-less="<%=Resources.Resource.TextString_ShowLess %>"><%=Resources.Resource.TextString_ShowLess %></div>
                </div>

                <%} %>
            <%} %>
            </div>
        </div>
        <%} %>

    </div>
</div>

</aside>