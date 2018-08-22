<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<div id="cartProducts" class="well shopcart">
<asp:Literal ID="litpromotion" runat="server"></asp:Literal> 

<asp:Repeater ID="rptCartProduct" runat="server" DataSource='<%# Eval("LineItems") %>' >
    <ItemTemplate>
    <hr style="margin:0 0px 0 0px;">
        <div class="goods-box goods-box-shopcart">
          
                <Hi:ListImage runat="server" DataField="ThumbnailUrl60" />
                <div class="info">
                     <a href="<%#"/ProductDetails.aspx?productId=" + Eval("ProductId")%>"> <div class="name bcolor">
                        <%# Eval("Name")%></div></a>
                    <div class="specification">
                        <input id="skucontent" type="hidden" value="<%# Eval("SkuContent")%>" />
                    </div>
                    <div class="price text-danger">
                        ¥<%# Eval("AdjustedPrice", "{0:F2}")%> <%#Hidistro.SaleSystem.Vshop.VshopBrowser.GetLimitedTimeDiscountNameStr(Globals.ToNum(Eval("LimitedTimeDiscountId"))) %>
                    </div>
                    <div class="goods-num">
		                <div name="spSub" class="shopcart-minus">
		                    - 
		                </div>
		                <input type="tel" class="ui_textinput" name="buyNum" value='<%# Eval("Quantity")%>'
		                    skuid='<%# Eval("SkuId")%>' limitedTimeDiscountId='<%#Eval("LimitedTimeDiscountId") %>' />
		                <div name="spAdd" class="shopcart-add">
		                    + 
		                </div>
		            </div>
                </div>
            <a href="javascript:void(0)" name="iDelete" skuid='<%# Eval("SkuId")%>' limitedTimeDiscountId='<%#Eval("LimitedTimeDiscountId") %>'><span
                class="glyphicon glyphicon-remove close_span"></span></a>
            <!--<div class="goods-num">
                <div name="spSub" class="shopcart-minus">
                    - 
                </div>
                <input type="tel" class="ui_textinput" name="buyNum" value='<%# Eval("Quantity")%>'
                    skuid='<%# Eval("SkuId")%>' />
                <div name="spAdd" class="shopcart-add">
                    + 
                </div>
            </div>-->
        </div>
    </ItemTemplate>
    
</asp:Repeater>
<div><asp:Literal ID="litline" runat="server"></asp:Literal> </div>

</div>
