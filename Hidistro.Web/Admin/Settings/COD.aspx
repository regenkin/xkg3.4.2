<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="COD.aspx.cs" MasterPageFile="~/Admin/AdminNew.Master"
    Inherits="Hidistro.UI.Web.Admin.Settings.COD" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>

<asp:content id="Content1" contentplaceholderid="head" runat="server">
    <style type="text/css">
        .lineCss{background-color:green;height:2px;width:auto; margin-left:5px; margin-right:5px;}
    </style>
    <script type="text/javascript">

        function setEnable(obj) {

            var type = "-1";

            var ob = $("#" + obj.id);
            var cls = ob.attr("class");
            var enable = "false";
            if (cls == "switch-btn") {

                ob.empty();
                ob.append("已关闭 <i></i>")
                ob.removeClass();
                ob.addClass("switch-btn off");
                enable = "false";

            }
            else {
                ob.empty();
                ob.append("已开启 <i></i>")
                ob.removeClass();
                ob.addClass("switch-btn");
                enable = "true";
            }
            $.ajax({
                type: "post",
                url: "PayConfigHandler.ashx",
                data: { type: type, enable: enable },
                dataType: "text",
                success: function (data) {
                    if (enable == 'true') {
                        msg('货到付款已开启！');
                    }
                    else {
                        msg('货到付款已关闭！');
                    }
                }
            });
        }

        function msg(msg) {
            HiTipsShow(msg, 'success');
        }
    </script>
</asp:content>
<asp:content id="Content2" contentplaceholderid="ContentPlaceHolder1" runat="server">
    <form id="thisForm" runat="server" class="form-horizontal">
        <div class="page-header">
            <h2>设置收款账号</h2>
        </div>
        <div>
        <ul class="nav nav-tabs" role="tablist">
            <li role="presentation"><a href="WeixinPay.aspx">微信支付</a></li>
            <li role="presentation"><a href="Alipay.aspx">支付宝</a></li>
            <%--<li role="presentation" ><a href="ChinaBank.aspx">网银在线</a></li>--%>
            <li role="presentation"><a href="ShengPay.aspx">盛付通</a></li>
            <li role="presentation"><a href="OfflinePay.aspx">线下支付</a></li>
            <li role="presentation" class="active"><a href="COD.aspx">货到付款</a></li>
        </ul>
        <div>          
            <div class="set-switch" style="margin-top:5px;">
                <strong>货到付款设置</strong>
                <p>启用后买家可选择货到付款下单，您需自行通过合作快递安排配送和收款。</p>
                <div id="PodEnable" class="<%=_enable?"switch-btn":"switch-btn off" %>" onclick="setEnable(this)">
                    <%=_enable?"已开启":"已关闭"%>
                    <i></i>
                </div>
            </div>
        </div>
    </div>
  </form>
</asp:content>
