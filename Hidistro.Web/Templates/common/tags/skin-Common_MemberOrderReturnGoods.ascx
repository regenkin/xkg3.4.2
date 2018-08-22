<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
    	<hr style="margin:0 0px 0 0px;">
    	<div class="choose_goods_content">
	    	 <a href='<%# "/Vshop/ProductReview.aspx?OrderId="+Request["OrderId"]+"&ProductId=" + Eval("ProductId") %>&skuid=<%#Eval("ID") %>'> <Hi:ListImage ID="ListImage1" runat="server" DataField="ThumbnailsUrl" /></a>
	    	<div class="info">
	    		<p> <%# Eval("ItemDescription")%></p>
	    		<div>数量： <%# Eval("Quantity")%>
                                <%# int.Parse(Eval("OrderItemsStatus").ToString().Trim()) == (int)Hidistro.Entities.Orders.OrderStatus.BuyerAlreadyPaid ? "<button class=\"btn btn-danger\" onclick=\"urllink('" + Eval("SkuID") + "','" + Eval("OrderId") + "')\">申请退款</button>" : int.Parse(Eval("OrderItemsStatus").ToString().Trim()) == (int)Hidistro.Entities.Orders.OrderStatus.SellerAlreadySent ? "<button class=\"btn btn-danger\" onclick=\"urllink('" + Eval("SkuID") + "','" + Eval("OrderId") + "')\">申请退货</button>" : int.Parse(Eval("OrderItemsStatus").ToString().Trim()) == (int)Hidistro.Entities.Orders.OrderStatus.ApplyForRefund ? "<button class=\"btn btn-danger\" >已申请退款</button>" : "<button class=\"btn btn-danger\" >已申请退货</button>"%></div>
	    	</div>
    	</div>
<script type="text/javascript">
    $(function () {
        var skuInputs = $('.specification input');
        $.each(skuInputs, function (j, input) {
            var text = '';
            $.each($(input).val().split(';'), function (i, sku) {
                if ($.trim(sku))
                    text += '<span class="badge-h">' + sku.split('：')[1] + '</span>';
            });
            $(input).parent().html(text);


        });

    });

    function urllink(productid) {
        var orderid = $("#orderid").text();
        location.href = "RequestReturn.aspx?orderId="+orderid+"&ProductId="+productid+"";
    } 
</script>
