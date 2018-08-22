<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<div class="orderlist">
    <div class="orderinfo">
        <p>订单编号：<%#Eval("OrderId") %></p>
        <p>订单日期：<%# Eval("OrderDate","{0:d}")%></p>
        <p>订单状态：<Hi:OrderStatusLabel ID="OrderStatusLabel1" IsShowToUser="true" Gateway='<%#Eval("Gateway") %>' OrderStatusCode='<%# Eval("OrderStatus") %>' runat="server" /></p>
        <span class="price">￥<em><%# Eval("OrderTotal","{0:F2}")%></em></span>
    </div>
    <asp:Repeater ID="rporderitems" runat="server" DataSource='<%# Eval("OrderItems") %>'>
        <ItemTemplate>
            <div class="orderimg">
                <Hi:ListImage ID="ListImage1" runat="server" DataField="ThumbnailsUrl" />
                <div class="orderimginfo">
                    <a href="<%# Globals.ApplicationPath + "/Vshop/MemberOrderDetails.aspx?OrderId=" + Eval("OrderId") %>">
                        <div class="name bcolor">
                            <%#Hidistro.SaleSystem.Vshop.VshopBrowser.GetLimitedTimeDiscountNameStr(Globals.ToNum(Eval("LimitedTimeDiscountId"))) %> <%# Eval("ItemDescription")%>
                            <%#Eval("OrderItemsStatus").ToString()=="9"?("<span class='text-danger'>(已退款，金额￥"+decimal.Parse( Eval("ReturnMoney").ToString()).ToString("F2"))+")</span>":"" %>
                            <%#Eval("OrderItemsStatus").ToString()=="10"?("<span class='text-danger'>(已退货，金额￥"+decimal.Parse( Eval("ReturnMoney").ToString()).ToString("F2"))+")</span>":"" %>
                        </div>
                    </a>
                    <div class="specification">
                        <input type="hidden" value="<%# Eval("SkuContent")%>" />
                    </div>
                        <div class="orderreturn">
                            数量<%#Eval("OrderItemsStatus") %>：<i><%# Eval("Quantity")%></i> &nbsp;&nbsp;单价：<i><%#Eval("Type").ToString()=="0"? Eval("ItemAdjustedPrice","{0:F2}").ToString()+"元":Eval("PointNumber").ToString()+"积分"%></i>
                                <%# int.Parse(Eval("OrderItemsStatus").ToString().Trim()) == (int)Hidistro.Entities.Orders.OrderStatus.BuyerAlreadyPaid ? "<button class=\"btn btn-default btn-xs \" orderid=\""+Eval("OrderID")+"\" skuid=\""+Eval("SkuID")+"\" onclick=\"urllink('" + Eval("ID") + "','" + Eval("OrderId") + "',"+Eval("Type")+")\" typeid='"+Eval("Type")+"'>申请退款</button>" : int.Parse(Eval("OrderItemsStatus").ToString().Trim()) == (int)Hidistro.Entities.Orders.OrderStatus.SellerAlreadySent ? "<button class=\"btn btn-default btn-xs waittochangestatus\"  orderid=\""+Eval("OrderID")+"\" skuid=\""+Eval("SkuID")+"\" orderitemid=\""+Eval("ID")+"\" onclick=\"urllink('" + Eval("ID") + "','" + Eval("OrderId") + "',"+Eval("Type")+")\" typeid='"+Eval("Type")+"'>申请退货</button>" : int.Parse(Eval("OrderItemsStatus").ToString().Trim()) == (int)Hidistro.Entities.Orders.OrderStatus.ApplyForRefund ? "<button class=\"btn btn-info btn-xs\" >已申请退款</button>" : int.Parse(Eval("OrderItemsStatus").ToString().Trim()) == (int)Hidistro.Entities.Orders.OrderStatus.ApplyForReturns?"<button class=\"btn btn-info btn-xs\" >已申请退货</button>":""%>
                        </div>
                </div>
            </div>
        </ItemTemplate>
    </asp:Repeater>
    <div class="linkbtn" style="height: 36px;"><span class="fl"><label class="fwnormal batchoperator"><input name="cbox" type="checkbox" class="alignSub" value="<%#Eval("OrderID") %>" /> 选中订单</label></span>
        <%# ((int)Eval("OrderStatus") == 3 || (int)Eval("OrderStatus") == 5) ? "<a href='"+Globals.ApplicationPath + "/Vshop/MyLogistics.aspx?OrderId=" + Eval("OrderId")+"' class='btn btn-info btn-xs'>查看物流</a>" : ""%>
        <%# (int)Eval("OrderStatus") == 1&&(int)Eval("PaymentTypeId")!=99&&(int)Eval("PaymentTypeId")!=0&&(string)Eval("GateWay")!="hishop.plugins.payment.bankrequest"&&(string)Eval("GateWay")!="hishop.plugins.payment.podrequest"? "<a href='"+ Globals.ApplicationPath + "/Vshop/FinishOrder.aspx?PaymentType=1&OrderId=" + Eval("OrderMarking")+"' class='btn btn-danger btn-xs'>去付款</a> <a class='btn btn-warning btn-xs' onclick='CancelOrder(\""+Eval("OrderId")+"\")'>取消订单</a>" : ""%>
        <%# (int)Eval("OrderStatus") == 3 ? "<a href='javascript:void(0)' onclick=\"FinishOrder('"+Eval("OrderId")+"')\" class='btn btn-danger btn-xs'>确认收货</a>" : ""%>
        <%# (int)Eval("PaymentTypeId")==99&&(int)Eval("OrderStatus")==1 ? "<a href='"+Globals.ApplicationPath + "/Vshop/FinishOrder.aspx?OrderId=" + Eval("OrderMarking")+"&onlyHelp=true' class='btn btn-danger btn-xs '>线下支付帮助</a> <a class='btn btn-warning btn-xs' onclick='CancelOrder(\""+Eval("OrderId")+"\")'>取消订单</a>" : ""%>
        <%#(Eval("HasRedPage")).ToString()=="1"?"<a href='/Vshop/GetRedShare.aspx?orderid="+Eval("OrderId")+"' class='btn btn-warning btn-xs btn-danger'>发钱咯</a>":"" %>
    </div>
</div>