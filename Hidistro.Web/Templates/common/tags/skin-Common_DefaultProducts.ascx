<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<div class="col-xs-6">
	<%
	    string plink=Globals.GetCurrentDistributorId()>0?"&&ReferralId="+Globals.GetCurrentDistributorId():"";
	  %>
	<a href='<%# Globals.ApplicationPath + "ProductDetails.aspx?ProductId=" + Eval("ProductId")%><%=plink %>'>
		<div class="index-content well">
		
            <img data-original="<%#Eval("ThumbnailUrl310").ToString().Length>5?Eval("ThumbnailUrl310").ToString():"/utility/pics/none.gif" %>" src="/Utility/pics/lazy-ico.gif"/>
		    <div class="info">
		        <div class="title">
		            <%# Eval("ProductName") %>
		        </div>
		        <div class="price text-danger">
		            ¥<%# Eval("SalePrice", "{0:F2}") %><span class="text-muted">已售<%# Eval("ShowSaleCounts")%>件</span>
		        </div>
		    </div>
            <asp:Literal ID="litpromotion" runat="server"></asp:Literal> 
		</div>
	</a>
</div>