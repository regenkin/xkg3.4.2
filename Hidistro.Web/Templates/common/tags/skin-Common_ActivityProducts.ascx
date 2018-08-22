<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<div class="col-xs-6">
<%
    string plink=Globals.GetCurrentDistributorId()>0?"&&ReferralId="+Globals.GetCurrentDistributorId():"";
  %>
<a href='<%# Globals.ApplicationPath + "ProductDetails.aspx?ProductId=" + Eval("ProductId")%><%=plink %>'>
<div class="index-content well">
    <Hi:ListImage runat="server" DataField="ThumbnailUrl310" />
<div class="info">
        <div class="title">
            <%# Eval("ProductName") %>
        </div>
        <div class="price text-danger">
            ¥<%# Eval("SalePrice", "{0:F2}") %><span class="text-muted">已售<%# Eval("ShowSaleCounts")%>件</span></div>
</div>
</div>
</a>
</div>