<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Order_ShippingAddress.ascx.cs" Inherits="Hidistro.UI.Web.Admin.Order_ShippingAddress" %>

<h1>物流信息</h1>
<div class="Settlement">
    <table width="100%" border="0" cellspacing="0">
        <tr id="tr_company" runat="server" visible="false">
            <td align="right">物流公司：</td>
            <td colspan="2" width="85%">
                <asp:Literal ID="litCompanyName" runat="server" /></td>
        </tr>
        <tr>
            <td width="15%" align="right">收货地址：</td>
            <td width="60%">
                <asp:Literal ID="lblShipAddress" runat="server" /></td>
            <td width="25%"><span class="Name">
                <asp:Label ID="lkBtnEditShippingAddress" runat="server">
                <a href="javascript:DialogFrame('../trade/ShipAddress.aspx?action=update&OrderId=<%=Page.Request.QueryString["OrderId"] %>','修改收货地址',620,410);" visible="false">修改收货地址</a>
                </asp:Label></td>
        </tr>
        <tr>
            <td align="right">送货上门时间：</td>
            <td colspan="2" width="85%">
                <asp:Literal ID="litShipToDate" runat="server" /></td>
        </tr>
        <tr>
            <td align="right">配送方式：</td>
            <td colspan="2" width="85%">
                <asp:Literal ID="litModeName" runat="server" /><%=edit %></td>
        </tr>
        <tr>
            <td align="right" nowrap="nowrap">买家留言：</td>
            <td colspan="2">
                <asp:Label ID="litRemark" runat="server" Style="word-wrap: break-word; word-break: break-all;" /></td>
        </tr>
    </table>
</div>
<div id="updatetag_div" style="display: none;">
    <div class="frame-content">
        <p><span class="frame-span frame-input90">发货单号：<em>*</em> </span>
            <asp:TextBox ID="txtpost" CssClass="forminput" runat="server" /></p>
    </div>
</div>
<input type="hidden" id="OrderId" runat="server" clientidmode="Static" />
<div style="display: none">
    <asp:Button ID="btnupdatepost" runat="server" Text="修 改" CssClass="submit_DAqueding" />
    <input type="hidden" id="hdtagId" runat="server" />
</div>
<script>
    function ShowPurchaseOrder() {
        formtype = "changeorder";
        arrytext = null;
        DialogShow("修改发货单号", 'changepurcharorder', 'updatetag_div', 'ctl00_ContentPlaceHolder1_shippingAddress_btnupdatepost');
    }
</script>
<!--物流信息-->
<asp:Panel ID="plExpress" runat="server" Visible="false" Style="width: 730px; margin-bottom: 10px;">
    <h1>快递单物流信息</h1>
    <div id="expressInfo">正在加载中....</div>


    <script type="text/javascript">
        $(function () {
            var orderId = $('#OrderId').val();
            if (orderId) {
                var expressData = getExpressData(orderId);

                var html = '<table>';
                var data = expressData.lastResult.data;
                if (expressData.lastResult.message != "ok")
                    html += '<tr><td>该单号暂无物流进展，请稍后再试，或检查公司和单号是否有误。</td></tr>';
                else {
                    for (var i = 0; i < data.length; i++) {
                        html += '<tr><td>' + data[i].time + '</td>\
                             <td>' + data[i].context + '</td>';
                        html += '</tr>';
                    }
                }
                html += '<tr><td><a href="http://www.kuaidi100.com/?refer=hishop" target="_blank" id="power" runat="server" visible="false" style="color:Red;">此物流信息由快递100提供</a></td></tr>';
                html += '</table>';
                $('#expressInfo').html(html);
            }
        });

        function getExpressData(orderId) {
            var url = '/API/VshopProcess.ashx';
            var expressData;
            $.ajax({
                type: "get",
                url: url,
                data: { action: 'Logistic', orderId: orderId },
                dataType: "json",
                async: false,
                success: function (data) {
                    expressData = data;
                }
            });
            return expressData;
        }
    </script>
</asp:Panel>
