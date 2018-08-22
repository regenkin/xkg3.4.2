<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<div class="panel panel-default">
    <div class="panel-heading order-shopcart">
        <h3 class="panel-title">订单商品
        </h3>
        <a id="orderProductsChange" href="/vshop/shoppingCart.aspx">修改</a>
    </div>
    <asp:Repeater ID="rptCartProductItems" runat="server" DataSource='<%# Eval("LineItems") %>'>
        <ItemTemplate>
             
          
            <div class="panel-body goods-list-p">
                <a href="<%# "/ProductDetails.aspx?productId=" + Eval("ProductId")%>" class="detailLink">
                    <div class="box">
                        <div class="left">
                            <Hi:ListImage runat="server" DataField="ThumbnailUrl60" />
                        </div>
                        <div class="right">
                            <div class="bcolor name">
                                <%#Hidistro.SaleSystem.Vshop.VshopBrowser.GetLimitedTimeDiscountNameStr(Globals.ToNum(Eval("LimitedTimeDiscountId"))) %> <%# Eval("Name")%>
                            </div>
                           
                            <div class="specification">
                                <input type="hidden" value="<%# Eval("SkuContent")%>" />
                            </div>
                            <div class="price text-danger">
                                <%# Eval("Type").ToString()=="0"?"":"积分兑换商品"%>
                                <%# Eval("Type").ToString()=="0"?"￥"+Eval("AdjustedPrice", "{0:F2}"):Eval("PointNumber")+"积分"%>
                                <span class="bcolor"> x
                    <%# Eval("Quantity")%></span>  
                            </div>
                             
                        </div>
                    </div>
                </a>
            </div>

        </ItemTemplate>
    </asp:Repeater>
</div>


<div class="btn-group selectShippingType">
    <button type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown" id='dropdown<%# Eval("TemplateId") %>'>请选择配送方式<span class="caret"></span></button>
    <ul id='shippingTypeUl<%# Eval("TemplateId") %>' class="dropdown-menu" role="menu">
    </ul>
    <input type="hidden" id='selectShippingType<%# Eval("TemplateId") %>' value="<%# Eval("TemplateId")%>" class="Template" />
    <input type="hidden" id='selectShippingTypeValue<%# Eval("TemplateId") %>' class="selectShippingTypeValue" />
</div>

<asp:Literal runat="server" ID="LitCoupon"></asp:Literal>

<textarea id='remark<%# Eval("TemplateId") %>' class="form-control" rows="3" placeholder="订单备注（选填）"></textarea>
<div class="panel panel-default">
<!--积分相关开始-->
<div class="selectBox" <%#SettingsManager.GetMasterSettings(false).PonitToCash_Enable==true?"":"style=\"display:none\""%>>
     <div class="selectcheckbox" id="UsePointNumber<%# Eval("TemplateId") %>" onclick="IsUsePointNumber(this, <%# Eval("TemplateId") %>)">
                    <span class="glyphicon glyphicon-ok"></span>
     </div>
    <span >使用积分(可用<span class="MemberPointNumber"   id="MemberPointNumber<%# Eval("TemplateId") %>"><%#Eval("MemberPointNumber") %></span>点)</span>

    <span style="display:none" id="spanPointNumber<%# Eval("TemplateId") %>"><input class="txtPointNumber" value="0"  onblur ="setvalue(this,<%# Eval("TemplateId") %>)" style="width:50px;" onkeyup="if(this.value.length==1){this.value=this.value.replace(/[^1-9]/g,'');$('#IntegralOffset<%# Eval("TemplateId") %>').html(0);}else{this.value=this.value.replace(/\D/g,'');}" onafterpaste="if(this.value.length==1){this.value=this.value.replace(/[^1-9]/g,'')}else{this.value=this.value.replace(/\D/g,'')}" id="txtPointNumber<%# Eval("TemplateId") %>" type="text" />点,抵应付金额<span style="color:red">￥</span><span style="color:red" class="IntegralOffset" id="IntegralOffset<%# Eval("TemplateId") %>">0.00</span></span>
</div>
<span style="display:none"  id="OldMemberPointNumber"><%#Eval("MemberPointNumber") %></span>
<span style="display:none"  id="NewMemberPointNumber"><%#Eval("MemberPointNumber") %></span>
<span style="display:none"  id="PointToCashRate"><%#SettingsManager.GetMasterSettings(false).PointToCashRate%></span>
<span style="display:none"  id="PonitToCash_MaxAmount"><%#SettingsManager.GetMasterSettings(false).PonitToCash_MaxAmount%></span>
<!--积分相关结束-->

<div style="  text-align: right; ">
    <div class="last">

        <p class="">
             <span>商品金额：</span>¥<span id='goodsmenoy<%# Eval("TemplateId") %>' class="Exemption"><%#Eval("Total", "{0:F2}") %> </span>
        </p>
        <p class="">
            <span>优惠减免：</span>¥<span id='Exemption<%# Eval("TemplateId") %>' class="Exemption"><asp:Literal runat="server" ID="litExemption"></asp:Literal></span><span style="display: none" id='oldExemption<%# Eval("TemplateId") %>'><asp:Literal runat="server" ID="litoldExemption"></asp:Literal></span>
            <span style="display: none">是否包邮<span id='bFreeShipping<%# Eval("TemplateId") %>' class="bFreeShipping"><asp:Literal runat="server" ID="litbFreeShipping"></asp:Literal></span></span>
        </p>
        <p class=" shippingTypes">
            运费金额：<span>¥<label style="font-weight: normal; margin-bottom: 0;" id='shipcost<%# Eval("TemplateId") %>'><%#Eval("ShipCost", "{0:F2}") %></label></span>
        </p>
        <p class="">
            应付总额：<span><strong class="text-danger">¥<label class="sumtotal"  id='total<%# Eval("TemplateId") %>' style="margin-bottom: 0;"><asp:Literal runat="server" ID="litTotal"></asp:Literal></label><label style="display: none" id='oldtotal<%# Eval("TemplateId") %>'><asp:Literal runat="server" ID="litoldTotal"></asp:Literal></label></strong></span>
        </p>
        <p <%# decimal.Parse( Eval("GetPointNumber").ToString())==0?"style=\"display:none\"":"" %>  class="">
            兑换商品扣除积分：<span  ><%#Eval("GetPointNumber") %></span>
        </p>
    </div>

</div>
    </div>

