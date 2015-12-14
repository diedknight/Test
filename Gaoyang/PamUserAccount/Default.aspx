<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="PamAccountGenerator.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>PAM User Account Generator</title>
    <meta http-equiv="Content-Type" content="text/html;charset=ISO-8859-1" />
    <link href="http://images.pricemestatic.com/pricemeico.ico" rel="shortcut icon" />
    <script src="http://code.jquery.com/jquery.js"></script>
    <style type="text/css">
        tr {
            vertical-align: top;
        }

        .signUpTable td {
            padding: 3px 0;
        }

        td .rightText {
            text-align: right;
            padding-right: 8px!important;
        }

        input[type="text"], textarea {
            border-radius: 3px;
            box-shadow: inset 0 1px 3px rgba(83,79,51,0.1);
            border: 1px solid #b7b5a2;
            width: 250px;
        }

        select {
            width: 254px;
        }

        .validSum {
            border: 1px #d2d2d2 solid;
            text-align: center;
        }

        #a {
            display: block;
            top: 199px;
            left: 412.5px;
        }
        .divButton {
            background-color: red;
	        display: inline-block;
	        color: #fff;
	        text-decoration: none;
	        -moz-border-radius: 6px;
	        -webkit-border-radius: 6px;
	        -moz-box-shadow: 0 1px 3px rgba(0,0,0,0.6);
	        -webkit-box-shadow: 0 1px 3px rgba(0,0,0,0.6);
	        text-shadow: 0 -1px 1px rgba(0,0,0,0.25);
	        border-bottom: 1px solid rgba(0,0,0,0.25);
	        position: relative;
	        cursor: pointer;
            font-size:12px;
        }

        .fontColor {color:red;}
    </style>
    <script type="text/javascript">
        //var check1 = document.getElementById("rdo1").checked;
        //var check3 = document.getElementById("rdo3").checked;
 
        function showFeedURL(show) {
            showNeedPassword(show);
            if (show) {
                document.getElementById("trFeedUri").style.display = "table-row";
                document.getElementById("txtFUrl").value = "http://";//  
                document.getElementById("trNeedPsd").style.display = "table-row";
                var check = document.getElementById("rdbNoNeedPsd").checked;
                if(check==true)
                    showNeedPassword(false);
                //document.getElementById("rdbNeedPsd").checked = false;
                //document.getElementById("rdbNoNeedPsd").checked = true;
            }
            else {
                document.getElementById("trFeedUri").style.display = "none";
                //document.getElementById("txtFUrl").value = "http://*";
                document.getElementById("trNeedPsd").style.display = "none";
            }
     
            //showNeedPassword(show);
        }
        function showTxtFeedBox(show) {
            var check = document.getElementById("rdbRetailerFeed").checked;
            if (show) {
                //document.getElementById("trFeedUri").style.display = "table-row";
                document.getElementById("trNeedPsd").style.display = "table-row"; 
                document.getElementById("trRetailerFeed").style.display = "table-row";
                if (check) {
                    document.getElementById("trFeedUri").style.display = "table-row";

                    if (document.getElementById("txtFUrl").value == "http://*") {
                        document.getElementById("txtFUrl").value = "http://";//  
                    }

                }
                if (document.getElementById("rdbNoNeedPsd").checked == true)
                    showNeedPassword(false);
                else
                    showNeedPassword(true);
            }
            else {
                document.getElementById("trFeedUri").style.display = "none";

                if (document.getElementById("txtFUrl").value == "http://") {
                    document.getElementById("txtFUrl").value = "http://*";
                }

               document.getElementById("trNeedPsd").style.display = "none";
               document.getElementById("trRetailerFeed").style.display = "none";
               showNeedPassword(false);
            }
        }
        function showNeedPassword(show) {
            if (show) {
                document.getElementById("userNameTR").style.display = "table-row";
                document.getElementById("psdTR").style.display = "table-row";
                //document.getElementById("txtUname").value = "";
                //document.getElementById("txtPsd").value = "";
            }
            else {
                document.getElementById("userNameTR").style.display = "none";
                document.getElementById("psdTR").style.display = "none";
                //document.getElementById("txtUname").value = "&;";
                //document.getElementById("txtPsd").value = "&;";
            }
        }
        function showPPC() {
            var val = document.getElementById("<%= ddlCountry.ClientID%>").value;
            document.getElementById("ddlBillingCountry").value = val;
            if (val == 3) {
                //document.getElementById("trPPC").style.display = "table-row";
                document.getElementById("trGSTNumber").style.display = "table-row";
            }
            else {
                //document.getElementById("trPPC").style.display = "none";
                document.getElementById("trGSTNumber").style.display = "none";
            }
            showCurrency();
        }
        //载入JS
        window.onload = function () {
            var rid = "<%=Request["rid"]%>";
            if(rid=="")
                showCurrency();
        }
        function showCurrency() {
            var val = document.getElementById("<%= ddlCountry.ClientID%>").value;
            var curr;
            //Rp	ID
            //HK	HK
            //sg 36 my 45 au 1 nz 3 ph 28 hk 41 id 51
            if (val == 3 || val == 36 || val == 1)
                curr = "($)";
            if (val == 36)
                document.getElementById("ddlPPC").value = 0.2;
            if (val == 3)
                document.getElementById("ddlPPC").value = 0.3;

            if (val == 28)
                curr = "(P)";
            else if (val == 45)
                curr = "(RM)";
            else if (val == 51)
                curr = "(Rp)";
            else if (val == 41)
                curr = "(HK)";
            else if (val == 55)
                curr = "(฿)";
            else if (val == 56)
                curr = "(Đ)";

            document.getElementById("lblCurrency").innerText = curr;
            document.getElementById("lblBudgetCurrency").innerText = curr;
        }

        function selectPPC(sel) {
            document.getElementById("ddlPPC").value = sel;
        }

        function onkeyup(){
            if (event.keyCode == 116) {
                window.location.href = "/Default.aspx";
            }
        }
       
        function common_ajax() {
            this.ajax = function (options) {
                var settings = {
                    url: "?",
                    type: "post",
                    cache: false,
                    dataType: "json",
                    data: null,
                    success: function (data) { },
                    error: function (req) { },
                    beforeSend: function () { },
                    complete: function () {}
                }
                $.extend(settings, options);
                $.ajax(settings);
            }
        }


        //根据网店自动绑定数据
        function userIt(rid) {
            //var tar=this;
            //var act = {
            //    data:{rid:rid,action:"action"},
            //    action: function () {
            //        var req = new common_ajax();
            //        req.ajax({
            //            data:this.data,
            //            success: function (data) {
            //                console.info(data);
            //            }
            //        })
            //    }
            //}
            //act.action();
            //return false;

            location.href = "Default.aspx?rid=" + rid;
        }


    </script>
</head>
<body onkeyup="onkeyup()">
    <form id="form1" runat="server">
        <div class="layoutDiv" id="mainDiv">
            <div class="contentDiv">
                <table style="width:100%">
                    <tr>
                        <td style="width:18%">
                            <div>
                                <a href="http://www.priceme.co.nz" title="Online Shopping & Price Comparison">
                                    <img height="67" width="220" src="http://images.pricemestatic.com/Images/PriceMeNewDesign/priceme_nn.png?ver=20130103" alt="PriceMe - Compare prices and buy online" /></a>
                            </div>
                        </td>
                           <td style="width: 64%; " align="center">
                            <div>
                                <div style="font-size: 2em;font-weight: bold;">
                                    <img style="vertical-align: middle" src="http://images.pricemestatic.com/images/retailers.gif" width="24" height="24" alt="Retailer Sign Up" />
         
                                    PAM Account Generator</div>
                                <table class="signUpTable">
                                    <tbody>
                                        <tr style="display:table-row">
                                            <td colspan="2">
                                                <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="vgEmail" ForeColor="Red"
                                                    CssClass="validSum" />
                                                <asp:Label runat="server" ID="lblMsg" Font-Size="Large" Font-Bold="true" ForeColor="Red" /><br/>
                                                <asp:Label runat="server" ID="lblNewUser" Font-Size="Large" />
                                            </td>
                                        </tr>
                                        <tr><td colspan="2">General Info<hr /></td></tr>
                                        <tr>
                                            <td class="rightText"><span><span style="color: #f03">*</span><b>Retailer Name:</b></span></td>
                                            <td>
                                                <asp:TextBox ID="txtRName" runat="server" ValidationGroup="vgEmail" />
                                                <asp:RequiredFieldValidator ID="rfvName" runat="server" ControlToValidate="txtRName"
                                                    Display="None" ErrorMessage="Please enter Retailer name" ValidationGroup="vgEmail" Text="" />
                                                 <asp:Button runat="server" ID="btnSearch" Text="Search" OnClick="btnSearch_Click" /><br />
                                                   <asp:Label runat="server" ID="lblSearchResult" ForeColor="Red" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="rightText" colspan="2">
                                                <asp:Repeater runat="server" ID="rptRetailers">
                                                    <HeaderTemplate>
                                                        <table style="border:1px solid #ccc; float:right;width:460px;">
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <tr><td><div class="divButton" onclick="userIt(<%#Eval("RetailerId") %>)">Use it</div></td><td><%#Eval("RetailerName") %>&nbsp;</td><td><%# Eval("IsSetupComplete").ToString()=="True"&&Eval("RetailerStatus").ToString()=="1"? GetRetailerUrlLink(Eval("RetailerId").ToString(),Eval("RetailerName").ToString(),Eval("RetailerCountry").ToString()):getRetailerCountName(Eval("RetailerCountry").ToString()) %></td></tr>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        </table>
                                                    </FooterTemplate>
                                                </asp:Repeater>
                                                <div style="clear:both;"></div>
                                                <asp:Repeater runat="server" ID="rptRetailers2">
                                                    <HeaderTemplate>
                                                        <table style="border:1px solid #ccc;width:460px;float:right;">
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <tr><td><%# GetRetailerUrlLink2(Eval("RetailerLeadID").ToString(),Eval("RetailerName").ToString(),Eval("SiteCountryID").ToString()) %></td></tr>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        </table>
                                                    </FooterTemplate>
                                                </asp:Repeater>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="rightText"><span>&nbsp;&nbsp;<b>Full company name:</b></span></td>
                                            <td>
                                                <asp:TextBox ID="txtFullCompanyName" runat="server"  ValidationGroup="vgEmail"/>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="rightText"><span>&nbsp;&nbsp;<b>Telephone Number:</b></span></td>
                                            <td>
                                                <asp:TextBox ID="txtTellNum" runat="server"  ValidationGroup="vgEmail"/>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="rightText"><span>&nbsp;&nbsp;<b>Company Reg Number:</b></span></td>
                                            <td>
                                                <asp:TextBox ID="txtCompanyRegNum" runat="server"  ValidationGroup="vgEmail"/>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="rightText"><span><span style="color: #f03">*</span><b>Retailer URL:</b></span></td>
                                            <td>
                                                <asp:TextBox ID="txtWUrl" runat="server" Text="http://" ValidationGroup="vgEmail" />
                                                <asp:RequiredFieldValidator ID="rfvUrl" runat="server" ControlToValidate="txtWUrl" InitialValue="http://"
                                                    Display="None" ErrorMessage="Please enter your Web Site URL" ValidationGroup="vgEmail" /></td>
                                        </tr>
                                           <tr>
                                            <td class="rightText"><span><span style="color: #f03"></span><b>Store type:</b></span></td>
                                            <td>
                                                <asp:DropDownList ID="ddlStoreType" runat="server" AppendDataBoundItems="true">
                                                    <asp:ListItem value="0" selected="True" >Please select</asp:ListItem>
		                                            <asp:ListItem value="1">Online and Retail Store</asp:ListItem>
		                                            <asp:ListItem value="2">Online no pickup</asp:ListItem>
		                                            <asp:ListItem value="3">Online with pickup</asp:ListItem>
		                                            <asp:ListItem value="4">Physical Retail Store only</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="rightText"><span><span style="color: #f03">*</span><b>Retailer Country:</b></span></td>
                                            <td>
                                                <asp:DropDownList ID="ddlCountry" runat="server" AppendDataBoundItems="true" onclick="javascript:showPPC();">
                                                    <asp:ListItem Text="Please select a country" Value="-1" />
                                                </asp:DropDownList>
                                                &nbsp;<asp:CompareValidator runat="server" ID="cvRtc" ControlToValidate="ddlCountry"
                                                     Display="None" ErrorMessage="Please select a Retailer Country" Operator="GreaterThanEqual"
                                                     ValueToCompare="1" ValidationGroup="vgEmail" />
                                                    </td>
                                        </tr>
                                        <tr><td colspan="2">Billing<hr /></td></tr>
                                        <%if(isAdmin){ %>
                                         <tr>
                                            <td class="rightText"><span>&nbsp;&nbsp;<b>PPC Type:</b></span></td>
                                            <td>
                                                <asp:RadioButton runat="server" ID="rdbPPC" GroupName="isPPC" Text="PPC" Checked="true" />
                                                <asp:RadioButton runat="server" ID="rdbNolink" GroupName="isPPC" Text="Nolink" />
                                            </td>
                                        </tr>
                                        <%} %>
                                        <tr id="trPPC">
                                            <td class="rightText"><span><span style="color: #f03">*</span><b>PPC <asp:Label runat="server" ID="lblCurrency" Text="($)"></asp:Label>:</b></span></td>
                                            <td>
                                                <%--<asp:TextBox ID="txtPPC" runat="server" ValidationGroup="vgEmail" />
                                                    0.10 ,0.15,0.16 0.17 ... 0.25 0.30 (0.00&#26684;&#24335;) please select --%>
                                                <asp:DropDownList ID="ddlPPC" runat="server" Width="100"  AppendDataBoundItems="true">
		                                            <asp:ListItem value="-1">Select ... </asp:ListItem>
		                                            <asp:ListItem value="0.1">0.10</asp:ListItem>
		                                            <asp:ListItem value="0.15">0.15</asp:ListItem>
		                                            <asp:ListItem value="0.16">0.16</asp:ListItem>
		                                            <asp:ListItem value="0.17">0.17</asp:ListItem>
		                                            <asp:ListItem value="0.18">0.18</asp:ListItem>
		                                            <asp:ListItem value="0.19">0.19</asp:ListItem>
		                                            <asp:ListItem value="0.2">0.20</asp:ListItem>
		                                            <asp:ListItem value="0.21">0.21</asp:ListItem>
		                                            <asp:ListItem value="0.22">0.22</asp:ListItem>
		                                            <asp:ListItem value="0.23">0.23</asp:ListItem>
		                                            <asp:ListItem value="0.24">0.24</asp:ListItem>
		                                            <asp:ListItem value="0.25">0.25</asp:ListItem>
		                                            <asp:ListItem value="0.3">0.30</asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:CompareValidator runat="server" ID="cvddlPPC" ControlToValidate="ddlPPC" 
                                                     Display="None" ErrorMessage="Please select PPC" Operator="GreaterThanEqual"
                                                     ValueToCompare="0" ValidationGroup="vgEmail" />
                                                &nbsp;<a href="javascript:void(0)" onclick="selectPPC(0.17)">0.17</a>
                                                &nbsp;<a href="javascript:void(0)" onclick="selectPPC(0.2)">0.20</a>
                                                &nbsp;<a href="javascript:void(0)" onclick="selectPPC(0.25)">0.25</a>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="rightText"><span>&nbsp;&nbsp;<b>Daily Budget <asp:Label runat="server" ID="lblBudgetCurrency" Text="($)"></asp:Label>:</b></span></td>
                                            <td>
                                                <%--<asp:TextBox ID="txtDailyBudget" runat="server" ValidationGroup="vgEmail" />
                                                    2,3,4,..,15 20(selected) 25  30 33 35 40 ,...,100,150--%>
                                                 <asp:DropDownList ID="ddlDailyBudget" runat="server" Width="100" AppendDataBoundItems="true">
		                                            <asp:ListItem value="2">2</asp:ListItem>
		                                            <asp:ListItem value="3">3</asp:ListItem>
		                                            <asp:ListItem value="4">4</asp:ListItem>
		                                            <asp:ListItem value="5">5</asp:ListItem>
		                                            <asp:ListItem value="6">6</asp:ListItem>
		                                            <asp:ListItem value="7">7</asp:ListItem>
		                                            <asp:ListItem value="8">8</asp:ListItem>
		                                            <asp:ListItem value="9">9</asp:ListItem>
		                                            <asp:ListItem value="10">10</asp:ListItem>
		                                            <asp:ListItem value="11">11</asp:ListItem>
		                                            <asp:ListItem value="12">12</asp:ListItem>
		                                            <asp:ListItem value="13">13</asp:ListItem>
			                                        <asp:ListItem value="14">14</asp:ListItem>
		                                            <asp:ListItem value="15">15</asp:ListItem>
		                                            <asp:ListItem value="20" Selected="True">20</asp:ListItem>
		                                            <asp:ListItem value="25">25</asp:ListItem>
		                                            <asp:ListItem value="30">30</asp:ListItem>
		                                            <asp:ListItem value="33">33</asp:ListItem>
		                                            <asp:ListItem value="35">35</asp:ListItem>
		                                            <asp:ListItem value="40">40</asp:ListItem>
		                                            <asp:ListItem value="45">45</asp:ListItem>
		                                            <asp:ListItem value="50">50</asp:ListItem>
		                                            <asp:ListItem value="55">55</asp:ListItem>
		                                            <asp:ListItem value="60">60</asp:ListItem>
		                                            <asp:ListItem value="65">65</asp:ListItem>
		                                            <asp:ListItem value="70">70</asp:ListItem>
		                                            <asp:ListItem value="75">75</asp:ListItem>
		                                            <asp:ListItem value="80">80</asp:ListItem>
		                                            <asp:ListItem value="85">85</asp:ListItem>
		                                            <asp:ListItem value="90">90</asp:ListItem>
		                                            <asp:ListItem value="95">95</asp:ListItem>
		                                            <asp:ListItem value="100">100</asp:ListItem>
		                                            <asp:ListItem value="150">150</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                       
                                        <tr id="trGSTNum5ber">
                                            <td class="rightText"><span><b>Support Direct Payment:</b></span></td>
                                            <td>
                                                <asp:RadioButton runat="server" ID="paymentYes" GroupName="IsCreditCardPaymentOnly" Text="Yes" />
                                                <asp:RadioButton runat="server" ID="paymentNo" GroupName="IsCreditCardPaymentOnly" Text="No"   />
                                            </td>
                                        </tr>
                                       
                                        <tr id="trGSTNumber">
                                            <td class="rightText"><span>&nbsp;&nbsp;<b>GST number:</b></span></td>
                                            <td>
                                                <asp:TextBox ID="txtGSTNumber" runat="server" ValidationGroup="vgEmail" /></td>
                                        </tr>
                                        <tr>
                                            <td class="rightText"><span><span style="color: #f03">*</span><b>Billing Country:</b></span></td>
                                            <td>
                                                <asp:DropDownList ID="ddlBillingCountry" runat="server" AppendDataBoundItems="true">
                                                    <asp:ListItem Text="Please select a country" Value="-1" />
                                                </asp:DropDownList>
                                                &nbsp;<asp:CompareValidator runat="server" ID="cvbc" ControlToValidate="ddlBillingCountry"
                                                     Display="None" ErrorMessage="Please select a Billing Country" Operator="GreaterThanEqual"
                                                     ValueToCompare="0" ValidationGroup="vgEmail" />
                                            </td>
                                        </tr>
                                        <tr><td colspan="2">Data<hr /></td></tr>
                                        <tr><td></td><td>
                                            <asp:Label ID="statusMsg" runat="server" CssClass="fontColor" Text="" Visible="false"></asp:Label></td></tr>
                                        <tr>
                                            <td class="rightText"><span>&nbsp;&nbsp;<b>Feed/WebHarvy:</b></span></td>
                                            <td>
                                                <asp:RadioButton runat="server" ID="rdo2" GroupName="isFeed" Visible="false" Text="Fetcher" onclick="javascript:showTxtFeedBox(false);" />
                                                <asp:RadioButton runat="server" ID="rdo1" GroupName="isFeed" Text="Feed" onclick="javascript:showTxtFeedBox(true);"  />
                                                <asp:RadioButton runat="server" ID="rdo3" GroupName="isFeed" Text="WebHarvy" onclick="javascript:showTxtFeedBox(false);" />
                                                <asp:RadioButton runat="server" ID="rdo4" GroupName="isFeed" Visible="false" Text="Api2Cart" onclick="javascript:showTxtFeedBox(false);" />
                                            </td>
                                        </tr>
                                        <tr id="trRetailerFeed" runat="server">
                                            <td class="rightText"><span>&nbsp;&nbsp;<b>Feed:</b></span></td>
                                            <td>
                                                <asp:RadioButton runat="server" ID="rdbRetailerFeed" GroupName="isRetailerFeed" Text="Retailer Feed" onclick="javascript:showFeedURL(true);"  Checked="true"/>
                                                <asp:RadioButton runat="server" ID="rdbFTPFeed" GroupName="isRetailerFeed" Text="Priceme FTP Feed" onclick="javascript:showFeedURL(false);"/>
                                            </td>
                                        </tr>




                                        <tr id="trFeedUri" runat="server">
                                            <td class="rightText"><span><span style="color: #f03">*</span><b>Feed URL:</b></span></td>
                                            <td>
                                                <asp:TextBox ID="txtFUrl" runat="server" Text="http://" ValidationGroup="vgEmail" />
                                                <asp:RequiredFieldValidator ID="rfvFeedURL" runat="server" ControlToValidate="txtFUrl" InitialValue="http://"
                                                    Display="None" ErrorMessage="Please enter feed url"  ValidationGroup="vgEmail" />
                                            </td>
                                        </tr>
                                        <tr id="trNeedPsd" runat="server">
                                            <td class="rightText"><span>&nbsp;&nbsp;<b>Need Password:</b></span></td>
                                            <td>
                                                <asp:RadioButton runat="server" ID="rdbNeedPsd" GroupName="isNeed" Text="Yes" onclick="javascript:showNeedPassword(true);"/>
                                                <asp:RadioButton runat="server" ID="rdbNoNeedPsd" GroupName="isNeed" Text="No" onclick="javascript:showNeedPassword(false);"  />
                                            </td>
                                        </tr>
                                        <tr id="userNameTR"  style="display:none;" runat="server">
                                            <td class="rightText"><span><span style="color: #f03">*</span><b>Username:</b></span></td>
                                            <td>
                                                <asp:TextBox ID="txtUname" runat="server"  ValidationGroup="vgEmail" Text=""/>
                                                <%--<asp:RequiredFieldValidator ID="rfvUname" ControlToValidate="txtUname" Display="None" ErrorMessage="Please enter username"  ValidationGroup="vgEmail"  runat="server"  />--%>
                                            </td>
                                        </tr>
                                        <tr id="psdTR"  style="display:none;" runat="server">
                                            <td class="rightText"><span><span style="color: #f03">*</span><b>Password:</b></span></td>
                                            <td>
                                                <asp:TextBox ID="txtPsd" runat="server"  ValidationGroup="vgEmail" Text=""/>
                                                <%--<asp:RequiredFieldValidator ID="rfvPsd"  ControlToValidate="txtPsd" Display="None" ErrorMessage="Please enter password"  ValidationGroup="vgEmail"  runat="server"/>--%>
                                            </td>
                                        </tr>
                                        <tr><td colspan="2">Contact<hr /></td></tr>
                                        <tr>
                                            <td class="rightText"><span><span style="color: #f03">*</span><b>Contact First Name:</b></span></td>
                                            <td>
                                                <asp:TextBox ID="txtFirstName" runat="server" ValidationGroup="vgEmail" />
                                                <asp:RequiredFieldValidator ID="rfvFirstName" runat="server" ControlToValidate="txtFirstName"
                                                    Display="None" ErrorMessage="Please enter first name" ValidationGroup="vgEmail" /></td>
                                        </tr>
                                        <tr>
                                            <td class="rightText"><span><span style="color: #f03">&nbsp;&nbsp;</span><b>Contact Last Name:</b></span></td>
                                            <td>
                                                <asp:TextBox ID="txtLastName" runat="server" ValidationGroup="vgEmail" />
                                                <%--<asp:RequiredFieldValidator ID="rfvLastName" runat="server" ControlToValidate="txtLastName"
                                                    Display="None" ErrorMessage="Please enter last name" ValidationGroup="vgEmail" />--%></td>
                                        </tr>
                                        <tr>
                                            <td class="rightText"><span><span style="color: #f03">*</span><b>Email Address:</b></span></td>
                                            <td>
                                                <asp:TextBox ID="txtEAddress" runat="server" ValidationGroup="vgEmail" />
                                                <asp:RequiredFieldValidator ID="rfvEAddress" runat="server" ControlToValidate="txtEAddress"
                                                    Display="None" ErrorMessage="Please enter Email Address" ValidationGroup="vgEmail" /></td>
                                        </tr>
                                        <%if(isAdmin){ %>
                                        <tr><td colspan="2">T&Cs<hr /></td></tr>
                                          <tr>
                                            <td class="rightText"><span>&nbsp;&nbsp;<b>Accept T&Cs:</b></span></td>
                                            <td>
                                                <asp:RadioButton runat="server" ID="rdbTCSYes" GroupName="isTCS" Text="Yes" />
                                                <asp:RadioButton runat="server" ID="rdbTCSNo" GroupName="isTCS" Text="No"  Checked="true"/>
                                            </td>
                                        </tr>
                                             <tr>
                                            <td class="rightText"><span><span style="color: #f03"></span><b>Admin:</b></span></td>
                                            <td>
                                                <asp:DropDownList ID="ddlAdmin" runat="server" AppendDataBoundItems="true">
                                                    <asp:ListItem value="-1">Please select ... </asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <%} %>
                                            <tr>
                                            <td class="rightText"><span><span style="color: #f03"></span><b>Comment:</b></span></td>
                                            <td>
                                                <asp:TextBox ID="txtComment" runat="server" TextMode="MultiLine" Height="50px" ValidationGroup="vgEmail" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" style="text-align: center;">
                                                <asp:ImageButton ID="btnSend" runat="server" ImageUrl="http://images.pricemestatic.com/images/sign_up.gif"
                                                    OnClick="btnAdd_Click" ValidationGroup="vgEmail" /></td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                        </td>
                        <td style="width:18%;text-align:right;">
                            <asp:Button runat="server" ID="btnLogOut" Text="Log Out" OnClick="btnLogOut_Click" /></td>
                    </tr>
                </table>
                <asp:HiddenField runat="server" ID="OrganisationID" />
                <asp:HiddenField runat="server" ID="RetailerLeadID" />
            </div>
        </div>
        <script type="text/javascript">

        </script>
    </form>
</body>
</html>
