<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Order_ChargesList.ascx.cs" Inherits="Hidistro.UI.Web.Admin.Order_ChargesList" %>
  
  
          <div class="Settlement">
          <table width="100%" border="0" cellspacing="0">
            <tr>
              <td style="width:100px;" align="right">运费(元)： </td>
              <td><asp:Literal ID="litFreight" runat="server" />&nbsp;(<asp:Literal ID="litShippingMode" runat="server"></asp:Literal>)&nbsp;<asp:HyperLink ID="hlkFreightFreePromotion" runat="server" Target="_blank" /></td>
              <td valign="middle"><span class="Name">&nbsp
              <asp:Label ID="lkBtnEditshipingMode" Visible="false" runat="server">
                <a href="javascript:UpdateShippingMode()">修改配送方式</a>
              </asp:Label></span></td>
            </tr>
            <tr>
              <td align="right" style="display:none">支付手续费(元)：</td>
              <td style="display:none"><asp:Literal ID="litPayCharge" runat="server" />&nbsp;(<asp:Literal ID="litPayMode" runat="server"></asp:Literal>)</td>
              <td  ><span class="Name">&nbsp<asp:LinkButton runat="server" ID="lkBtnEditPayMode"  Text="修改支付方式" OnClientClick="javascript:UpdatePaymentMode();return false;" Visible="false" /></span></td>
            </tr>
            <tr>
              <td align="right">红包抵扣(元)：</td>
              <td colspan="2" ><span class="colorB"><asp:Literal ID="litRedPager" runat="server" /></span></td>
            </tr>
            <tr style="display:none">
              <td align="right">优惠券折扣(元)：</td>
              <td colspan="2" ><span class="colorB"><asp:Literal ID="litCouponValue" runat="server" Visible="false" /><asp:Literal ID="litCoupon" runat="server" /></span></td>
            </tr>
            <tr style="display:none">
              <td align="right">涨价或减价(元)：</td>
              <td><span class="colorB"><asp:Literal ID="litDiscount" runat="server" /></span></td>
              <td>为负代表折扣，为正代表涨价 </td>
            </tr>
            <asp:Literal ID="litTax" runat="server" />
            <asp:Literal ID="litInvoiceTitle" runat="server" />
            <tr style="display:none">
              <td align="right">订单可得积分：</td>
              <td colspan="2" class="colorA"><asp:Literal ID="litPoints" runat="server" />&nbsp;<asp:HyperLink ID="hlkSentTimesPointPromotion" runat="server" Target="_blank" /></td>
            </tr>
             <tr>
              <td align="right">优惠减免(元)：</td>
              <td><span class="colorB"><asp:Literal ID="litExmition" runat="server" /></span></td>
              <td> </td>
            </tr>
            <tr class="bg">
              <td align="right" class="colorG">订单实收款(元)：</td>
              <td colspan="2"><strong class="colorG fonts"><asp:Literal ID="litTotalPrice" runat="server" /></strong></td>
            </tr>
          </table>
  </div>
  
  