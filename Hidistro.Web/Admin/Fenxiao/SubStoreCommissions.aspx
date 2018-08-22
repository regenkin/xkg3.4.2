<%@ Page Language="C#" MasterPageFile="~/Admin/SimplePage.Master" AutoEventWireup="true" CodeBehind="SubStoreCommissions.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Fenxiao.SubStoreCommissions" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script>

        $(function () {
    

            if ($(".td_bg").length == 0) {
                $("#tbodyHtml").html("无分佣数据！");
                $("#tbodyHtml").css({"text-align":"center","line-height":"40px"})
            };
    

        });

    </script>
   

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <form runat="server" id="thisForm">
        <asp:Repeater ID="reCommissions" runat="server">

            <HeaderTemplate>
                <div>
                    <table class="table table-hover mar table-bordered" style="table-layout: fixed;" id="commisTb">
                        <thead>
                            <tr>
                                <th width="100">成交店铺</th>
                                <th width="120">分佣时间</th>
                                <th width="120">订单号</th>

                                <th width="100">订单金额</th>
                                <th width="100">佣金</th>

                            </tr>
                        </thead>
                        <tbody id="tbodyHtml">
            </HeaderTemplate>
            <ItemTemplate>
                <tr class="td_bg">
                    <td>&nbsp;<%# Eval("fromStoreName")%></td>
                    <td width="200">&nbsp;<%# Eval("TradeTime", "{0:yyyy-MM-dd HH:mm:ss}")%></td>
                    <td>&nbsp;<a href="/Admin/trade/OrderDetails.aspx?OrderId=<%# Eval("OrderId")%>" target="_blank"><%# Eval("OrderId")%></a></td>

                    <td>&nbsp;￥<%# Eval("OrderTotal", "{0:F2}")%></td>
                    <td class="red-all">&nbsp;￥<%# Eval("CommTotal","{0:F2}")%></td>


                </tr>
            </ItemTemplate>
            <FooterTemplate>
                </tbody>
     </table>
     </div>
            </FooterTemplate>
        </asp:Repeater>

 <!--数据列表底部功能区域-->
  <br />
        <div class="select-page clearfix">
                    <div class="form-horizontal fl">
                       
                    </div>
                    <div  class="page fr">
                         <div class="pageNumber">
                        <div class="pagination" style="margin:0px">
                        <UI:Pager runat="server" ShowTotalPages="true" ID="pager" />　
                       </div>
                      </div>
                    </div>
                </div>

        <div class="clearfix" style="height:30px"></div>
    </form>
</asp:Content>