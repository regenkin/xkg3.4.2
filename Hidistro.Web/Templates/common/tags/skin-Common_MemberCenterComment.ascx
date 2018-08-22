<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<div class="orderlist">
    <div class="orderinfo">
        <p>订单编号：<%#Eval("OrderId") %></p>
      <%--  <p>订单日期：dd</p>--%>
        <span class="price">￥<em><%# Eval("ItemAdjustedPrice","{0:F2}")%></em></span>
    </div>
            <div class="orderimg">
                <Hi:ListImage ID="ListImage1" runat="server" DataField="ThumbnailsUrl" />
                <div class="orderimginfo">
                    <a href="ProductReview.aspx?OrderId=<%#Eval("OrderId") %>&ProductId=<%#Eval("ProductID") %>&SkuId=<%#Eval("SkuId") %>&itemid=<%#Eval("ID") %>">
                        <div class="name bcolor">
                            <%# Eval("ItemDescription")%>
                        </div>
                    </a>
                    <div class="specification">
                        <input type="hidden" value="<%# Eval("SkuContent")%>" />
                    </div>
                        <div class="orderreturn">
                            数量：<i><%# Eval("Quantity")%></i>
                                <button class="btn btn-danger btn-xs" onclick="location.href='ProductReview.aspx?OrderId=<%#Eval("OrderId") %>&ProductId=<%#Eval("ProductID") %>&SkuId=<%#Eval("SkuId") %>&itemid=<%#Eval("ID") %>'">发表评价</button>
                        </div>
                </div>
            </div>
</div>