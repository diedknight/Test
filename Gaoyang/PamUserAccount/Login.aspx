<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>PAM User Account Generator - Login</title>
    <link href="http://images.pricemestatic.com/pricemeico.ico" rel="shortcut icon" />
</head>
<body>
    <form id="form1" runat="server">
        <div id="header" class="layoutDiv">
            <div id="logo">
                <a href="http://www.priceme.co.nz" title="Online Shopping & Price Comparison">
                    <img height="67" width="220" src="http://images.pricemestatic.com/Images/PriceMeNewDesign/priceme_nn.png?ver=20130103" alt="PriceMe - Compare prices and buy online" /></a>
            </div>
        </div>
        <div class="layoutDiv" id="mainDiv">
            <div class="contentDiv">
                <table>
                    <tr>
                        <td style="height: 288px"></td>

                        <td align="center" width="100%" style="height: 288px; padding-top: 20px;" class="hookline">
                            <table border="0">
                                <tr>
                                    <td align="left" style="padding: 0 0 0 5px">Please Log In</td>
                                </tr>
                                <tr>
                                    <td>
                                        <table align="center" style="text-align: center; color: Black; height: 216px; background-color: #EBECED; border-collapse: collapse; border-color: #ABB6BD;" border="1" width="500">

                                            <tr>
                                                <td style="text-align: center; vertical-align: middle; width: 500px">

                                                    <asp:Login ID="Login1" runat="server"
                                                        FailureText="*Login failed,please try again"
                                                        LoginButtonText="Login"
                                                        PasswordLabelText="Password:"
                                                        PasswordRequiredErrorMessage="Please enter password"
                                                        RememberMeText="Save Password?"
                                                        UserNameLabelText="Username:"
                                                        UserNameRequiredErrorMessage="Please enter username" BackColor="#EBECED" BorderColor="Transparent" BorderPadding="4" BorderStyle="Solid" BorderWidth="0px" Font-Names="Verdana" Font-Size="0.8em" ForeColor="#333333" TextLayout="TextOnTop" Width="500px" Height="45px">
                                                        <TitleTextStyle BackColor="#990000" Font-Bold="True" Font-Size="0.9em" ForeColor="White" />
                                                        <InstructionTextStyle Font-Italic="True" ForeColor="Black" />
                                                        <TextBoxStyle Font-Bold="True" Font-Size="0.8em" />
                                                        <LoginButtonStyle BackColor="White" BorderColor="#CC9966" BorderStyle="Solid" BorderWidth="1px"
                                                            Font-Names="Verdana" Font-Size="0.8em" ForeColor="#990000" />
                                                        <LayoutTemplate>
                                                            <table width="500">
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName" Font-Bold="True" Font-Size="12px">User name: </asp:Label>
                                                                        <asp:TextBox ID="UserName" runat="server" Font-Bold="False" Font-Size="15px" Width="200px"></asp:TextBox>
                                                                        <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="UserName"
                                                                            ErrorMessage="input username" ToolTip="input username" ValidationGroup="Login1">*</asp:RequiredFieldValidator>
                                                                    </td>
                                                                </tr>

                                                                <tr>
                                                                    <td style="height: 26px">
                                                                        <asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="Password" Font-Bold="True" Font-Size="12px">&nbsp;Password: </asp:Label>
                                                                        <asp:TextBox ID="Password" runat="server" Font-Bold="False" Font-Size="15px" TextMode="Password" Width="200px"></asp:TextBox>
                                                                        <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="Password"
                                                                            ErrorMessage="Fill in Password" ToolTip="Fill in Password" ValidationGroup="Login1">*</asp:RequiredFieldValidator></td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left">
                                                                        <asp:CheckBox ID="RememberMe" runat="server" Style="padding-left: 200px;" Text="Remember Me"/>
                                                                    </td>
                                                                </tr>

                                                                <tr>
                                                                    <td align="center">
                                                                        <hr style="width: 98%" />
                                                                    </td>
                                                                </tr>

                                                                <tr>
                                                                    <td>
                                                                        <asp:Literal ID="FailureText" runat="server" EnableViewState="False"></asp:Literal></td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:ImageButton ID="LoginButton" ImageUrl="http://images.pricemestatic.com/images/css/LogIn.bmp" runat="server" BorderColor="#EBECED"
                                                                            BorderStyle="Solid" BorderWidth="0px" CommandName="Login" ForeColor="#990000" ValidationGroup="Login1" Height="30px" Width="95px" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </LayoutTemplate>
                                                    </asp:Login>
                                                </td>

                                            </tr>

                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </form>
</body>
</html>
