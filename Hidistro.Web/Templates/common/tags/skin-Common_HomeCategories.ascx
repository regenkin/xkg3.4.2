<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<asp:Literal ID="litStart" runat="server" Text="<li>" Visible="false"></asp:Literal>
	<a name="False" value="<%# Eval("BrandId") %>"  onclick="goUrl('/BrandDetail.aspx?BrandId=<%# Eval("BrandId") %>');event.cancelBubble = true;">
		<asp:Literal ID="litpromotion" runat="server" Text='<%# Eval("Logo") %>'></asp:Literal>
	</a>
<asp:Literal ID="litEnd" runat="server" Text="</li>" Visible="false"></asp:Literal>