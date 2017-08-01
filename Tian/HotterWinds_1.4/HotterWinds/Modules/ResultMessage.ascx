<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ResultMessage.ascx.cs" Inherits="HotterWinds.Modules.ResultMessage" %>

<table class="full-width"   id="tblResult" runat="server" visible="false" enableviewstate="false">
	<tr id="trSuccess" runat="server">
		<td>
			<table>
                <tr><td>
                    <div class="alert alert-success" role="alert" style="margin-top:8px;">
                        <asp:Label id="lblSuccess" runat="server"></asp:Label>
                        </div>
                    </td></tr>
				<%--<tr>
					<td rowspan="2"><img src="<%=Resources.Resource.ImageWebsite %>/images/icons/icon_check.gif" align="absMiddle">
					</td>
				</tr>
				<tr>
					<td>
						<b>
							<asp:Label id="lblSuccess" runat="server"></asp:Label>
						</b>
					</td>
				</tr>--%>
			</table>
		</td>
	</tr>
	<tr id="trFail" runat="server">
		<td>
			<table class="failInfoTable">
				<tr>
					<td class="tdImage">
                        <%--<img src="<%=Resources.Resource.ImageWebsite %>/images/icons/icon_error.gif" align="absMiddle">
					</td>
					<td class="tdMsg">
					<b>--%>
                        <div class="alert alert-danger" role="alert"><asp:Label id="lblFail" runat="server" ForeColor="red"></asp:Label></div><%--</b>--%>
					</td>
				</tr>
			</table>
		</td>
	</tr>
	<tr id="trInfo" runat="server">
		<td>
			<table>
                 <tr><td>
                    <div class="alert alert-warning" role="alert">
                        <asp:Label id="lblInfo" runat="server"></asp:Label>
                        </div>
                    </td></tr>
				<%--<tr>
					<td rowspan="2"><img src="<%=Resources.Resource.ImageWebsite %>/images/icons/info.gif" align="absMiddle">
					</td>
				</tr>
				<tr>
					<td >
					<b><asp:Label id="lblInfo" runat="server"></asp:Label></b>
					</td>
				</tr>--%>
			</table>
		</td>
	</tr>
</table>