<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<div class="well goods-box">
    <a href="/ProductDetails.aspx?ProductId=<%# Eval("ProductId") %>"
        target="_blank">
        <Hi:ListImage runat="server" DataField="ThumbnailUrl60" />
        <div class="info">
            <div class="name font-l bcolor" style="height:16px;">
                <%# Eval("ProductName")%></div>
            <div class="intro font-m text-muted text-overflow">
                <%# Eval("ShortDescription")%></div>
            <div class="price text-danger">
                ¥<%# Eval("SalePrice", "{0:F2}")%>
            </div>
        </div>
 </a><a href="javascript:void(0)" onclick="Submit('<%# Eval("FavoriteId")%>')"><span
        class="glyphicon glyphicon-remove move"></span></a>
</div>
