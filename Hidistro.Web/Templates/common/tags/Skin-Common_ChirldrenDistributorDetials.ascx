<%@ Control Language="C#" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<div class="well member-orders-nav">
    <div class="nav-title clearfix">
       <div class="nav-title-left">
	        <p><span class="text-right">订单编号 :</span><span><%# Eval("OrderId")%></span></p>	
	        <p><%#Eval("FinishDate")==DBNull.Value?"<span class='text-right'>下单日期 :</span><span>"+Eval("OrderDate","{0:yyyy-MM-dd HH:mm}")+"</span>":"<span class='text-right'>完成日期 :</span><span>"+Eval("FinishDate","{0:yyyy-MM-dd HH:mm}")+"</span>" %></p>
       </div>
       <div class="nav-title-middle" >
		    <p class="text-right"><span class="text-danger"><Hi:OrderStatusLabel ID="OrderStatusLabel1" OrderStatusCode='<%# Eval("OrderStatus") %>' runat="server" /></span></p>
            <p><span class="text-right">成交金额 :</span><span><%# Eval("OrderTotal","{0:F2}")%></span></p>
	    </div>
    </div>
<hr style="margin:0 -10px 0 -10px;">
<asp:Repeater ID="rporderitems" runat="server" DataSource='<%# Eval("OrderItems") %>'>
    <ItemTemplate>
        <div class="member-orders-content">
           <Hi:ListImage ID="ListImage1" runat="server" DataField="ThumbnailsUrl" />
            <div class="info">
                <div class="name bcolor"><%# Eval("ItemDescription")%>
                    <%#Eval("OrderItemsStatus").ToString()=="9"?("<span class='text-danger'>(已退款，金额￥"+decimal.Parse( Eval("ReturnMoney").ToString()).ToString("F2"))+")</span>":"" %>
                    <%#Eval("OrderItemsStatus").ToString()=="10"?("<span class='text-danger'>(已退货，金额￥"+decimal.Parse( Eval("ReturnMoney").ToString()).ToString("F2"))+")</span>":"" %>
                </div>
                <p class="text-muted update-price">数量 :<%# Eval("Quantity")%>
                   <%-- 佣金:<span class="text-color"><%#(!(Eval("OrderItemsStatus").ToString().Equals("9")||Eval("OrderItemsStatus").ToString().Equals("10")))?(Eval("IsAdminModify").ToString()=="0"? (decimal.Parse(Eval("ItemsCommission", "{0:F2}"))-decimal.Parse(Eval("ItemAdjustedCommssion","{0:F2}"))).ToString(): decimal.Parse(Eval("ItemsCommission", "{0:F2}")).ToString()):"0" %>元</span><%# (Eval("OrderItemsStatus").ToString().Equals("1")&&Eval("Type").ToString().Equals("0")) ? "<a onclick=\"UpdatePrice(" + Eval("ItemsCommission", "{0:F2}") + ",'" + Eval("OrderId") + "','"+Eval("SkuId")+"')\">改价</a>" : ""%>--%>
               </p>
            </div>
         </div>
         <hr style="margin:0 -10px 0 0px;">
    </ItemTemplate>
</asp:Repeater>
 <p class="text-right" style="padding:10px 0;"><span class="text-right">佣金 :</span><span><asp:Literal ID="litCommission" runat="server"></asp:Literal></span></p>		
</div>
