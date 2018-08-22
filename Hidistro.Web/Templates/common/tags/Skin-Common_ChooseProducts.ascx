<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<div class="col-xs-6">
<div class="index-content well">
    <Hi:ListImage runat="server" DataField="ThumbnailUrl310" />
    <div class="content-right">
        <div>
            <a href='<%# Globals.ApplicationPath + "/ProductDetails.aspx?ProductId=" + Eval("ProductId") %>'>
                <%# Eval("ProductName") %></a></div>

        <div class="price text-danger">
            ¥<%# Eval("SalePrice", "{0:F2}") %><span class="text-muted">已售<%# Eval("SaleCounts")%>件</span></div>
     
    </div>
</div>
 <div class="right"><input type="checkbox" name="CheckGroup" id='CheckGroup<%#Eval("ProductId") %>' value='<%# Eval("ProductId") %>' /></div>

</div>
