<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<a href="<%# Globals.ApplicationPath + "/Vshop/GroupBuyProductDetails.aspx?groupbuyId=" + Eval("GroupBuyId") %>">
    <div class="well goods-box">
    <Hi:ListImage ID="ListImage1" runat="server" DataField="ThumbnailUrl60" />
        <div class="info">
            <div class="name font-xl">
                <%# Eval("ProductName") %></div>
            <div class="intro font-m text-muted">
                <%# Eval("ShortDescription")%></div>
            <div class="price text-danger">
                ¥<%# Eval("Price", "{0:F2}") %><del class="font-s text-muted">¥<%# Eval("SalePrice", "{0:F2}") %></del><span
                    class="sales font-s text-muted">已团<%# Eval("SoldCount")%>件</span></div>
        </div>
    </div>
</a>