<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Admin/AdminNew.Master"
    CodeBehind="ChinaBank.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Settings.ChinaBank" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <Hi:Script ID="Script4" runat="server" Src="/admin/js/jquery.formvalidation.js" />
    <style type="text/css">
        .lineCss{background-color:green;height:2px;width:auto;}
        .msgDiv{background-color:#D3FBFE; border:1px solid; border-color:#0c9e0e; height:50px; margin-top:10px;margin-bottom:10px;}
    </style>
    <script type="text/javascript">
        function setEnable(obj) {
            var type = "-6";
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
                        msg('网银在线付款已开启！');
                        $('#maindiv').css('display', '');
                    }
                    else {
                        msg('网银在线付款已关闭！');
                        $('#maindiv').css('display', 'none');
                    }
                }
            });
        }

        function msg(msg) {
            HiTipsShow(msg, 'success');
        }

        function beforeSaveData(obj) {

            var mid = $('#<%=txt_mid.ClientID%>').val();
            var md5 = $('#<%=txt_md5.ClientID%>').val();
            var des = $('#<%=txt_des.ClientID%>').val();
            if (mid == "") {
                errAlert("请输入商户号!");
                $('#<%=txt_mid.ClientID%>').focus();
                return false;
            }
            if (md5 == "") {
                errAlert("请输入MD5私钥!");
                $('#<%=txt_md5.ClientID%>').focus();
                return false;
            }
            if (des == "") {
                errAlert("请输入DES密钥!");
                $('#<%=txt_des.ClientID%>').focus();
                return false;
            }
            return true;
        }

        $(function () {
            $('#aspnetForm').formvalidation({
                'ctl00$ContentPlaceHolder1$txt_md5': {
                    validators: {
                        notEmpty: {
                            message: "网银支付MD5私钥。请登录网银在线商户管理后台，在【安全中心-网银+密钥修改】中设置。"
                        }
                        ,
                        stringLength: {
                            min: 1,
                            max: 100,
                            message: ''
                        }
                    }
                },
                'ctl00$ContentPlaceHolder1$txt_mid': {
                    validators: {
                        notEmpty: {
                            message: '请输入商户号'
                        }
                        ,
                        stringLength: {
                            min: 1,
                            max: 100,
                            message: ''
                        }
                    }
                },

                'ctl00$ContentPlaceHolder1$txt_des': {
                    validators: {
                        notEmpty: {
                            message: '网银支付DES密钥。请登录网银在线商户管理后台，在【安全中心-网银+密钥修改】中生成。'
                        }
                    }
                }
            });

        });

        function errAlert(msg) {
            HiTipsShow(msg, 'error');
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form id="thisForm" runat="server" class="form-horizontal">
        <div class="page-header">
            <h2>设置收款账号</h2>
        </div>
        <div>
        <ul class="nav nav-tabs" role="tablist">
            <li role="presentation"><a href="WeixinPay.aspx">微信支付</a></li>
            <li role="presentation"><a href="Alipay.aspx">支付宝</a></li>
            <li role="presentation" class="active"><a href="ChinaBank.aspx">网银在线</a></li>
            <li role="presentation"><a href="ShengPay.aspx">盛付通</a></li>
            <li role="presentation"><a href="OfflinePay.aspx">线下支付</a></li>
            <li role="presentation"><a href="COD.aspx">货到付款</a></li>
        </ul>
        <div class="set-switch">
            <strong>网银在线收款设置</strong>
            <p>设置网银在线收款账号后买家付款资金将会直接打入您的网银在线账号。</p>
            <div id="offlineEnable" class="<%=_enable?"switch-btn":"switch-btn off" %>" onclick="setEnable(this)">
                <%=_enable?"已开启":"已关闭"%>
                <i></i>
            </div>
            
        </div>
        <%--<div class="write-form form-horizontal" style="width: 800px;">--%>
            <div id="maindiv" style="<%=_enable?"": "display:none" %>">
                <div class="form-group" style="display:none;">
                    <label for="inputEmail1" class="col-xs-2 control-label">快速签约：</label>
                    <div class="col-xs-4">
                        <button type="button" class="btn btn-success bigsize" onclick='location.href="http://www.chinabank.com.cn/index.jsp"'>去网银在线</button>
                    </div>
                </div>
                <div class="form-group">
                    <label for="inputEmail2" class="col-xs-2 control-label">MD5私钥：</label>
                    <div class="col-xs-4">
                        <asp:TextBox runat="server" class="form-control" ID="txt_md5"></asp:TextBox>
                        <small class="help-block">网银支付MD5私钥。请登录网银在线商户管理后台，在【安全中心-网银+密钥修改】中设置。</small>
                    </div>
                </div>
                <div class="form-group">
                    <label for="inputEmail3" class="col-xs-2 control-label">商户号：</label>
                    <div class="col-xs-4">
                        <asp:TextBox runat="server" class="form-control" ID="txt_mid"></asp:TextBox>                    
                    </div>
                </div>
                <div class="form-group">
                    <label for="inputEmail4" class="col-xs-2 control-label">DES密钥：</label>
                    <div class="col-xs-4">
                        <asp:TextBox runat="server" class="form-control" ID="txt_des"></asp:TextBox>
                        <small class="help-block">网银支付DES密钥。请登录网银在线商户管理后台，在【安全中心-网银+密钥修改】中生成。</small>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-xs-offset-2 col-xs-10 marginl">
                        <asp:Button runat="server" OnClick="Unnamed_Click" class="btn btn-success inputw100"
                            OnClientClick="return beforeSaveData(this)" Text="保存" />
                    </div>
                </div>
             </div>
        </div>

    </form>
</asp:Content>

