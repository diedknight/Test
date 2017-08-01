<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AboutUsNavigation.ascx.cs" Inherits="HotterWinds.Modules.AboutUsNavigation" %>

<%
    string url = Request.Url.ToString().ToLower();
    string other = "";
%>
<div style="padding:20px 20px 20px 15px;">
<ul class="nav nav-tabs"  role="tablist">
    <%other = url.Contains("aboutus.aspx") ? "active" : "";%><li class="<%=other %>"><a  href="<%=Page.ResolveUrl("~/About/AboutUs.aspx")%>" ><%=Resources.Resource.TextString_AboutUs %></a></li>
      <%--<%other = url.Contains("contactus.aspx") ? "active" : "";%><li class="<%=other %>"><a  href="<%=Page.ResolveUrl("~/About/ContactUs.aspx")%>" ><%=Resources.Resource.TextString_ContactUs %></a></li>--%>
  <%--<%other = url.Contains("howthisworks.aspx") ? "other" : "";%><li><a class="<%=other %>" href="<%=Page.ResolveUrl("~/About/HowThisWorks.aspx")%>"><%=Resources.Resource.TextString_HowThisWorks %></a></li>--%>
    <%--<%other = url.Contains("howtousepriceme.aspx") ? "active" : "";%><li class="<%=other %>"><a  href="<%=Page.ResolveUrl("~/About/HowToUsePriceMe.aspx")%>"><%=Resources.Resource.TextString_HowToUsePriceMe %></a></li>--%>
    <%other = url.Contains("faqs.aspx") ? "active" : "";%><li  class="<%=other %>" ><a href="<%=Page.ResolveUrl("~/About/FAQs.aspx")%>"><%=Resources.Resource.TextString_FAQs %></a></li>
    <%--<%other = url.Contains("newsletter.aspx") ? "active" : "";%><li class="<%=other %>" ><a  href="<%=Page.ResolveUrl("~/NewsLetter.aspx") %>" ><%=Resources.Resource.TextString_Newsletter %></a></li>--%>
   <%-- <%other = url.Contains("advancedfeatures.aspx") ? "active" : "";%><li class="<%=other %>" ><a href="<%=Page.ResolveUrl("~/About/AdvancedFeatures.aspx")%>" ><%=Resources.Resource.TextString_AdvancedFeatures %></a></li>--%>
    <%--<%other = url.Contains("privacypolicy.aspx") ? "active" : "";%><li class="<%=other %>" ><a href="<%=Page.ResolveUrl("~/About/PrivacyPolicy.aspx")%>" ><%=Resources.Resource.TextString_PrivacyPolicy %></a></li>--%>

    
    <%--<%other = url.Contains("previousnewsletters.aspx") ? "other" : "";%><li><a class="<%=other %>" href="<%=Page.ResolveUrl("~/PreviousNewsletters.aspx")%>"><%=Resources.Resource.TextString_PreviousNewsletters %></a></li>
    <%if (!url.Contains("aboutus.aspx"))
      {%>
    <%other = url.Contains("contactus.aspx") ? "other" : "";%><li><a class="<%=other %>" href="<%=Page.ResolveUrl("~/About/ContactUs.aspx")%>" rel="nofollow"><%=Resources.Resource.TextString_Contact %></a></li>
    <%} %>
    <%other = url.Contains("mediacenter.aspx") ? "other" : "";%><li><a class="<%=other %>" href="<%=Page.ResolveUrl("~/About/MediaCenter.aspx")%>"><%=Resources.Resource.TextString_MediaCentre %></a></li>
    <%other = url.Contains("newsarticles.aspx") ? "other" : "";%><li><a class="<%=other %>" href="<%=Page.ResolveUrl("~/About/NewsArticles.aspx")%>"><%=Resources.Resource.TextString_NewsArticles %></a></li>
    <%if (!url.Contains("aboutus.aspx"))
      {%>
    <%other = url.Contains("pricemeblog.aspx") ? "other" : "";%><li><a class="<%=other %>" href="<%=Resources.Resource.Blog_URL %>"><%=Resources.Resource.TextString_PriceMeBlog %></a></li>
    <%} %>
    <%if (WebConfig.CountryId == 3)
      {%>
    <%other = url.Contains("featuredbrand.aspx") ? "other" : "";%><li><a class="<%=other %>" href="<%=Page.ResolveUrl("~/About/FeaturedBrand.aspx")%>" ><%=Resources.Resource.TextString_FeaturedBrand %></a></li>
    <%} %>--%>
</ul>
</div>