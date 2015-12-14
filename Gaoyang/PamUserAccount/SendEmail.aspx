<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SendEmail.aspx.cs" Inherits="PamAccountGenerator.SendEmail" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>PAM User Account Generator - Send Email</title>
    <meta http-equiv="Content-Type" content="text/html;charset=ISO-8859-1" />
    <link href="http://images.pricemestatic.com/pricemeico.ico" rel="shortcut icon" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="layoutDiv" id="mainDiv">
            <div class="contentDiv">
                <table style="width: 100%">
                    <tr style="vertical-align: top;">
                        <td style="width: 20%">
                            <div id="header" class="layoutDiv">
                                <div id="logo">
                                    <a href="http://www.priceme.co.nz" title="Online Shopping & Price Comparison">
                                        <img height="67" width="220" src="http://images.pricemestatic.com/Images/PriceMeNewDesign/priceme_nn.png?ver=20130103" alt="PriceMe - Compare prices and buy online" /></a>
                                </div>
                            </div>
                        </td>
                        <td style="width: 60%; text-align: center">
                            <h1>
                                <img style="vertical-align: middle" src="http://images.pricemestatic.com/images/retailers.gif" width="24" height="24" alt="Retailer Sign Up" />
                                PAM Account Generator</h1>
                            <div style="text-align: center;">
                                <h1>
                                    <asp:Label runat="server" ID="lblMsg" Font-Size="Large"
                                        Font-Bold="true" ForeColor="Green" Text="Retailer Henrik_Test_228 created successfully" /></h1>
                                <asp:Label runat="server" ID="lblNewUser" Font-Size="Large" />
                            </div>
                            <div style="padding: 10px;">
                                <div class="contentDiv" style="text-align: center">
                                    <h4>Email Content </h4>
                                    <div id="emailDiv">
                                        <asp:Label runat="server" ID="lblContent" Width="600" Style="text-align: left; background-color: #E5EBF3; padding: 8px;" Text="Retailer Henrik_Test_228 created successfully" /></div>
                                    <br />
                                    <asp:Button ID="btnCancel" Text="don't send" OnClick="btnCancel_Click" runat="server" />
                                    &nbsp;&nbsp;&nbsp;&nbsp;<asp:Button Text="send email" ID="btnSend" OnClick="btnSend_Click" runat="server" />
                                </div>
                            </div>

                        </td>
                        <td style="width: 20%; text-align: right;">
                            <asp:Button runat="server" ID="btnLogOut" Text="Log Out" OnClick="btnLogOut_Click" /></td>
                    </tr>
                </table>
            </div>
        </div>

    </form>
</body>
</html>
