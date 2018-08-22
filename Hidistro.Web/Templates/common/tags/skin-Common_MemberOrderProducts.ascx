<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%--<a href="<%# Globals.ApplicationPath + "ProductDetails.aspx?productId=" + Eval("ProductId")%>">--%>
<a href='<%# "/Vshop/ProductReview.aspx?OrderId="+Request["OrderId"]+"&ProductId=" + Eval("ProductId")+"&skuid="+Eval("skuid") %>&itemid=<%#Eval("ID") %>'>
    <div class="box">
        <div class="left">
            <Hi:ListImage runat="server" DataField="ThumbnailsUrl" />
        </div>
        <div class="right">
            <div class="name bcolor text-overflow">
                <%#Hidistro.SaleSystem.Vshop.VshopBrowser.GetLimitedTimeDiscountNameStr(Globals.ToNum(Eval("LimitedTimeDiscountId"))) %> <%# Eval("ItemDescription")%>
                <%# Eval("OrderItemsStatus").ToString() == "ApplyForRefund" ? "(已申请退款)" :(Eval("OrderItemsStatus").ToString() == "ApplyForReturns" ? "(已申请退货)" :"")%>
                <%# Eval("OrderItemsStatus").ToString() == "Refunded" ? "(已退款)" :(Eval("OrderItemsStatus").ToString() == "Returned" ? "(已退货)" :"")%></div>
            <div class="specification">
                <input type="hidden" value="<%# Eval("SkuContent")%>" />
            </div>
            <div class="price text-danger">
                <%#Eval("Type").ToString()=="0"? Eval("ItemAdjustedPrice","{0:F2}").ToString()+"元":Eval("PointNumber").ToString()+"积分"%><span class="bcolor"> x
                    <%# Eval("Quantity")%></span></div>
        </div>
    </div>
</a>

<script type="text/javascript">
    $(function () {
        var skuInputs = $('.specification input');
        $.each(skuInputs, function (j, input) {
            var text = '';
            var sku = $(input).val().split(';');
            var changsku='';
            for (var i = sku.length - 2; i >= 0; i--) {
                changsku += sku[i]+';';
            }
            $.each(changsku.split(';'), function (i, sku) {
                if ($.trim(sku))
                    text += '<span class="property">' + sku.split('：')[1] + '</span>';
            });
            $(input).parent().html(text);


        });

    });
        
        
</script>
