<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<div class="my-apply well" <asp:Literal ID="litStyle" runat="server"></asp:Literal> >
    <div class="title">
        订单编号：<em>
            <%# Eval("OrderId")%></em> <span>¥<%# Eval("OrderTotal","{0:F2}")%></span></div>
    <div class="my-apply-content">
        
            <asp:Repeater ID="Repeater1" runat="server" DataSource='<%# Eval("OrderItems") %>'>
                <ItemTemplate>
                    <Hi:ListImage runat="server" DataField="ThumbnailsUrl" />
                    <div class="info">
                        <p>
                            <%# Eval("ItemDescription")%></p>
                    <p>
                        <span class="specification" style="float: left; ">      
                              <input type="hidden" value="<%# Eval("SkuContent")%>" />
                        </span>
                    </p>
                        <p>
                              
                            数量：<i><%# Eval("Quantity")%></i> 
                           
                                <%# int.Parse(Eval("OrderItemsStatus").ToString().Trim()) == (int)Hidistro.Entities.Orders.OrderStatus.BuyerAlreadyPaid ? "<button class=\"btn btn-danger\" onclick=\"urllink('" + Eval("SkuID") + "','" + Eval("OrderId") + "')\">申请退款</button>" : int.Parse(Eval("OrderItemsStatus").ToString().Trim()) == (int)Hidistro.Entities.Orders.OrderStatus.SellerAlreadySent ? "<button class=\"btn btn-danger\" onclick=\"urllink('" + Eval("SkuID") + "','" + Eval("OrderId") + "')\">申请退货</button>" : int.Parse(Eval("OrderItemsStatus").ToString().Trim()) == (int)Hidistro.Entities.Orders.OrderStatus.ApplyForRefund ? "<button class=\"btn btn-danger\" >已申请退款</button>" : int.Parse(Eval("OrderItemsStatus").ToString().Trim()) == (int)Hidistro.Entities.Orders.OrderStatus.Refunded ? "<button class=\"btn btn-danger\" >已退款</button>" : int.Parse(Eval("OrderItemsStatus").ToString().Trim()) == (int)Hidistro.Entities.Orders.OrderStatus.Returned ? "<button class=\"btn btn-danger\" >已退货</button>" :int.Parse(Eval("OrderItemsStatus").ToString().Trim()) == (int)Hidistro.Entities.Orders.OrderStatus.ApplyForReturns?"<button class=\"btn btn-info btn-xs\" >已申请退货</button>":""%>
                        </p>
                         
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        
        <Hi:OrderStatusLabel ID="OrderStatusLabel1" Visible="false" OrderStatusCode='<%# Eval("OrderStatus") %>'
            runat="server" />
    </div>
    </div>
    <script language="javascript">
        function urllink(skuid, orderid) {
            location.href = "RequestReturn.aspx?orderId=" + orderid + "&skuid=" + skuid + "";
        }
        $(function () {
            var skuInputs = $('.specification input');
            $.each(skuInputs, function (j, input) {
                var text = '';
                var sku = $(input).val().split(';');
                var changsku = '';
                for (var i = sku.length - 2; i >= 0; i--) {
                    changsku += sku[i] + ';';
                }
                $.each(changsku.split(';'), function (i, sku) {
                    if ($.trim(sku))
                        text += '<span class="property" style="color:black;">' + sku.split('：')[1] + '</span>';
                });
                $(input).parent().html(text);


            });

        });
    </script>