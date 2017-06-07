<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="pricemeextension.aspx.cs" Inherits="ExtensionWebsite.pricemeextension" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="//fonts.googleapis.com/css?family=Open+Sans:300,400,600,700,800" rel="stylesheet" type="text/css"/>
    <style type="text/css">
        .pc_toolbar {
    background-color: #f5f5f5;
    font-size: 12px;
    font-weight: 400;
    position: fixed;
    left: 0;
    top: 0;
    z-index:9999999;
    width: 100%;
    min-width: 450px;
    height: 30px;
    box-sizing: inherit !important;
    font-family: 'Open Sans',sans-serif !important;
}

#pc_toolbar * {
    -webkit-box-sizing: inherit !important;
    box-sizing: inherit !important;
    font-family: 'Open Sans',sans-serif !important;
}

#pc_toolbar *, :after, :before {
    -webkit-box-sizing: inherit !important;
    box-sizing: inherit !important;
}

.pc_toolbar .bg {
    background: url(https://images.pricemestatic.com/images/PriceMeNewDesign/extension.png?v=20170418) no-repeat;
    opacity:initial;
}

.pc_toolbar_visible {
    margin-top:30px !important;
}

.pc_toolbar::after {
    content:"";
    clear:both;
    display:block;
}

.pc_toolbar .pc_log {
    float:left;
    width:115px;
    margin-left:5px;
    padding-right:10px;
    position:relative;
    border-right: 1px solid #fff;
}

.pc_toolbar .pc_log::before {
    content: "";
    display: block;
    position: absolute;
    border: solid;
    top: 10px;
    border-width: 5px 0 5px 7px;
    z-index: 2;
    right: -8px;
    border-color: transparent transparent transparent #fff;
}

.pc_toolbar .pc_log::after {
    content: "";
    display: block;
    position: absolute;
    border: solid;
    top: 10px;
    border-width: 5px 0 5px 7px;
    z-index: 2;
    right: -6px;
    border-color: transparent transparent transparent #fff;
}

.pc_toolbar .pc_log .pc_log_img {
    background-position: 0px 1px;
    width:115px;
    height:30px;
}

.pc_toolbar .pc_best {
    float:left;
    width: auto;
    color:white;
    font-weight: bold;
    position:relative;
    border-right: 1px solid #3498db;
}

.pc_toolbar .pc_best .pc_best_content {
    background:#3498db;
    height:30px;
}

.pc_toolbar .pc_best::before {
    content: "";
    display: block;
    position: absolute;
    border: solid;
    top: 10px;
    border-width: 5px 0 5px 7px;
    z-index: 2;
    right: -8px;
    border-color: transparent transparent transparent #3498db;
}

.pc_toolbar .pc_best::after {
    content: "";
    display: block;
    position: absolute;
    border: solid;
    top: 10px;
    border-width: 5px 0 5px 7px;
    z-index: 2;
    right: -6px;
    border-color: transparent transparent transparent #3498db;
}

.pc_toolbar .pc_best a {
    color:white;
    font-weight: bold;
    text-decoration: initial;
    font-size:15px !important;
    line-height:16px !important;
    height:16px !important;
    padding: 7px 15px;
    display:block;
}

.pc_toolbar .pc_compare {
    float:left;
    width:auto;
    line-height:16px;
    padding: 7px 40px 7px 20px;
    cursor:pointer;
    position:relative;
    background-color: #F2F2F2;
    border-radius: 3px;
    background-position: 0px 300px;
}

/*.pc_toolbar .pc_compare::before {
    content: "";
    display: block;
    position: absolute;
    border: solid;
    top: 10px;
    border-width: 5px 0 5px 7px;
    z-index: 2;
    right: -8px;
    border-color: transparent transparent transparent #ccc;
}

.pc_toolbar .pc_compare::after {
    content: "";
    display: block;
    position: absolute;
    border: solid;
    top: 10px;
    border-width: 5px 0 5px 7px;
    z-index: 2;
    right: -6px;
    border-color: transparent transparent transparent #f9f9f9;
}*/

.pc_toolbar .pc_compare:hover {
    background-color:#eee;
    background-position: 169px -106px;
}

.pc_toolbar .pc_close {
    float:right;
    width: 30px;
    font-size: 23px;
    font-weight: bold;
    cursor: pointer;
    color: #aaa;
    margin-right: 10px;
}

.pc_toolbar .pc_close:hover {
    color:#666;
}

.pc_toolbar .pc_compare .compare_title {
    font-size: 15px !important;
    line-height:16px !important;
    color: #000222 !important;
}

.pc_toolbar .pc_compare:hover .compare_conten {
    display:block;
}

    .pc_toolbar .pc_compare .compare_conten {
        display:none;
        box-shadow: 0 5px 5px #666;
        -moz-box-shadow: 0 5px 5px #666;
        -webkit-box-shadow: 0 5px 5px #666;
        position: absolute;
        z-index: 9999;
        background-color: #f8f8f8;
        visibility: visible;
        top:28px;
        left:0px;
        max-height: 400px;
        overflow: hidden;
        overflow-y: auto;
        padding: 5px;
        width: 415px;
    }
    
    .pc_toolbar .pc_compare .conten_l {
        border-bottom: solid 1px #CCC;
        cursor:pointer;
        margin: 5px 0;
        height: 35px;
        background-position: 0px 300px;
    }

    .pc_toolbar .pc_compare .linecolor {
        background-color:#F4F4F4;
    }

    .pc_toolbar .pc_compare .conten_l:hover {
        background-position:0px -179px;
        background-repeat:repeat-x;
    }
    .pc_toolbar .pc_compare .conten_l::after {
        content:"";
        clear:both;
        display:block;
    }

    .pc_toolbar .pc_compare .conten_l .conten_img{
        float:left;
        width:95px;
        text-align:left;
        padding-top:2px;
        height:35px;
        overflow:hidden;
    }

    .pc_toolbar .pc_compare .conten_l .conten_name{
        float:left;
        width:120px;
        text-align: left !important;
        margin-right: 10px;
        height:35px;
        display:table;
    }
    
    .pc_toolbar .pc_compare .conten_l .conten_nolinkcolor {
        color: #999 !important;
        font-size:13px;
    }

    .pc_toolbar .pc_compare .conten_l .conten_name .conten_name_con {
        max-height:30px; 
        overflow:hidden;
        line-height: 14px !important;
        font-weight:bold;
        font-size: 12px !important;
        color: #3366D5 !important;
    }

    .pc_toolbar .pc_compare .conten_l .conten_price{
        float:left;
        width: 79px;
        margin-right: 10px;
        text-align:left;
        color: #ea5504 !important;
        font-weight:bold;
        font-size:17px;
        height:35px;
        display:table;
        text-align:right;
    }

    .pc_toolbar .pc_compare .conten_l .conten_price .decimal {
        position: relative;
        top: -0.2em;
        font-size: 0.8em;
        font-weight: normal;
    }

    .pc_toolbar .pc_compare .conten_l .conten_price .symbol {
        font-size: 12px;
    font-weight: normal;
    }

    .pc_toolbar .pc_compare .conten_l .conten_Shipping{
        float:left;
        width:70px;
        text-align:left;
        color:#999 !important;
        height:35px;
        margin-right:4px;
        text-align:right;
        font-size:12px !important;
        background-position: 0px 300px;
    }

    .pc_toolbar .pc_compare .conten_l .Shipping {
        background-position: 52px -60px;
    }

    .pc_toolbar .pc_compare .conten_l .vertical {
        display: table-cell;
        vertical-align: middle;
        border:none !important;
    }

    .pc_toolbar .pc_compare .conten_l .vertical::after {
        background:none !important;
    }

    .pc_toolbar .pc_compare .conten_l .conten_price .vertical {
        color: #ea5504 !important;
        font-size: 17px !important;
        border: none !important;
    }

    



    </style>
</head>
<body>
    <form id="form1" runat="server">
    <%if(isReturn && datas.Count > 0){ %>
    <div id="pc_toolbar" class="pc_toolbar">
        <div class="pc_log">
            <a href="<%=homeurl %>">
                <div class="bg pc_log_img">&nbsp;</div></a></div>
        <div class="pc_best">
            <div class="pc_best_content">
                <a href="<%=rpurl %>"><%=stringsave %></a>
            </div>
        </div>
        <div class="bg pc_compare">
            <div class="compare_title">Compare <%=datas.Count %> prices</div>
            <div class="compare_conten">
                <%
          int i = 0;
                    foreach(ExtensionWebsite.Data.RetailerProduct rp in datas){
                        string linecolor = string.Empty;
                        if (i % 2 != 0)
                            linecolor = " linecolor";
                        i++;
                      string scriptFormat = "on_clickOut('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}')";
                      string VSOnclickScript = string.Format(scriptFormat, Guid.NewGuid(), rp.ProductId, rp.RetailerId, rp.RetailerProductId,
                          rp.RetailerProductName.Replace("'", " ").Replace("\"", "").Replace("\r\n", " ").Replace("\r", " ").Replace("\n", " "),
                          0, rp.RetailerPrice.ToString("0.00"), "&t=ext", countryid, track);
                      string DeliveryInfo = GetDelivery(rp.Freight);
                      string shoppingcss = " Shipping";
                      if (DeliveryInfo == "&nbsp;")
                          shoppingcss = "";
                      string name = rp.RetailerProductName.Length > 35 ? rp.RetailerProductName.Substring(0, 35) + "..." : rp.RetailerProductName;
                      %>
                <div class="bg conten_l<%=linecolor %>"<%if (!rp.IsNolink){ %> onclick="<%=VSOnclickScript %>"<%}else{ %> style="cursor:default;"<%} %>>
                    <div class="conten_img">
                        <%if(rp.IsNolink){ %>
                        <span class="conten_nolinkcolor"><%=rp.RetailerName %></span>
                        <%}else{ %>
                        <img src="<%=rp.RetailerLogo %>" alt="<%=rp.RetailerName %>" width="85" height="28" /><%} %></div>
                    <div class="conten_name">
                        <div class="vertical">
                            <div class="conten_name_con"<%if (rp.IsNolink){ %> style="color:#999 !important; font-weight:500 !important;" <%} %>><%=name %></div></div></div>
                    <div class="conten_price"<%if (islongprice){ %> style="width:140px;"<%} %>>
                        <div class="vertical"<%if (rp.IsNolink){ %> style="color:#999 !important; font-weight:500 !important;" <%} %>><%=ExtensionWebsite.Code.Utility.ProductListPrice(rp.RetailerPrice, countryid) %></div></div>
                    <%if (!islongprice){ %>
                    <div class="bg conten_Shipping<%=shoppingcss %>">
                        <div style="padding: 10px 20px 0 0; font-size:12px !important;"><%=DeliveryInfo %></div></div><%} %>
                </div>
                <%} %>
            </div>
        </div>
        <div class="pc_close" onclick="pctoolbarclose();">×</div>
    </div>
    <%} %>
    </form>
</body>
</html>
