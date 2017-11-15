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
    background: url(data:image/png;base64,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) no-repeat;
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
