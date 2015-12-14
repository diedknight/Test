<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Success.aspx.cs" Inherits="PamAccountGenerator.Success" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>PAM User Account Generator - Success</title>
    <meta http-equiv="Content-Type" content="text/html;charset=ISO-8859-1" />
    <link href="http://images.pricemestatic.com/pricemeico.ico" rel="shortcut icon" />
</head>
<body>
    <form id="form1" runat="server">
    <div class="layoutDiv" id="mainDiv">
            <div class="contentDiv">
                <table style="width:100%">
                    <tr style="vertical-align:top;">
                        <td style="width:20%">
                            <div>
                                <a href="http://www.priceme.co.nz" title="Online Shopping & Price Comparison">
                                    <img height="67" width="220" src="http://images.pricemestatic.com/Images/PriceMeNewDesign/priceme_nn.png?ver=20130103" alt="PriceMe - Compare prices and buy online" /></a>
                            </div>
                        </td>
                        <td style="width: 60%;  text-align:center">
                            <div>
                                <h1>
                                    <img style="vertical-align: middle" src="http://images.pricemestatic.com/images/retailers.gif" width="24" height="24" alt="Retailer Sign Up" />
                                    PAM Account Generator</h1>
                                <table class="signUpTable" width="100%">
                                    <tbody>
                                        <tr style="display:table-row">
                                            <td colspan="2">
                                                <h1><asp:Label runat="server" ID="lblMsg" Font-Size="Large" 
                                                        Font-Bold="true" ForeColor="Green" Text="Retailer Henrik_Test_228 created successfully" /></h1>
                                                
                                                <asp:Label runat="server" ID="lblNewUser" Font-Size="Large" />
                                                <br />
                                                <br />
                                                <a href="Default.aspx">Want to add another retailer?</a>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                        </td>

                        <td style="width:20%; text-align:right;">
                            <asp:Button runat="server" ID="btnLogOut" Text="Log Out" OnClick="btnLogOut_Click" /></td>
                    </tr>
               
                </table>
            </div>
        </div>
    </form>
</body>
</html>
