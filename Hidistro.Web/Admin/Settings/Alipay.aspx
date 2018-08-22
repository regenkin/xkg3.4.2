<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Admin/AdminNew.Master"
    CodeBehind="Alipay.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Settings.Alipay" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <Hi:Script ID="Script4" runat="server" Src="/admin/js/jquery.formvalidation.js" />
    <style type="text/css">
        .lineCss {
            background-color: green;
            height: 2px;
            width: auto;
            margin-left: 5px;
            margin-right: 5px;
        }
    </style>
    <script type="text/javascript">
        function setEnable(obj) {
            var type = "-4";
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
                        msg('支付宝付款已开启！');
                        $('#maindiv').css('display', '');
                    }
                    else {
                        msg('支付宝付款已关闭！');
                        $('#maindiv').css('display', 'none');
                    }
                }
            });
        }


        function beforeSaveData(obj) {
            debugger
            var mid = $('#<%=txt_mid.ClientID%>').val();
            var name = $('#<%=txt_mName.ClientID%>').val();
            var pid = $('#<%=txt_pid.ClientID%>').val();
            var key = $('#<%=txt_key.ClientID%>').val();
            if (mid == "") {
                errAlert("请输入支付宝帐号!");
                $('#<%=txt_mid.ClientID%>').focus();
                return false;
            }
            if (name == "") {
                errAlert("请输入支付宝帐号姓名!");
                $('#<%=txt_mName.ClientID%>').focus();
                return false;
            }
            if (pid == "") {
                errAlert("请输入合作者身份（PID）!");
                $('#<%=txt_pid.ClientID%>').focus();
                return false;
            }
            if (key == "") {
                errAlert("请输入安全校验码（Key）!");
                $('#<%=txt_key.ClientID%>').focus();
                return false;
            }
            //$('#aspnetForm').attr('action', 'save');
            return true;
        }

        function errAlert(msg) {
            HiTipsShow(msg, 'error');
        }
        function msg(msg) {
            HiTipsShow(msg, 'success');
        }

        $(function () {
            $('#aspnetForm').formvalidation({
                'ctl00$ContentPlaceHolder1$txt_mid': {
                    validators: {
                        notEmpty: {
                            message: "请输入支付宝帐号"
                        }
                        ,
                        stringLength: {
                            min: 1,
                            max: 100,
                            message: ''
                        }
                    }
                },
                'ctl00$ContentPlaceHolder1$txt_mName': {
                    validators: {
                        notEmpty: {
                            message: '请输入支付宝账号姓名'
                        }
                        ,
                        stringLength: {
                            min: 1,
                            max: 100,
                            message: ''
                        }
                    }
                },

                'ctl00$ContentPlaceHolder1$txt_pid': {
                    validators: {
                        notEmpty: {
                            message: '成功申请支付宝接口后获取到的PID'
                        }
                        ,
                        stringLength: {
                            min: 1,
                            max: 100,
                            message: ''
                        }
                    }
                },
                'ctl00$ContentPlaceHolder1$txt_key': {
                    validators: {
                        notEmpty: {
                            message: '成功申请支付宝接口后获取到的Key'
                        }
                        ,
                        stringLength: {
                            min: 1,
                            max: 100,
                            message: ''
                        }
                    }
                }
            });

        });


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
                <li role="presentation" class="active"><a href="Alipay.aspx">支付宝</a></li>
               <%-- <li role="presentation"><a href="ChinaBank.aspx">网银在线</a></li>--%>
                <li role="presentation"><a href="ShengPay.aspx">盛付通</a></li>
                <li role="presentation"><a href="OfflinePay.aspx">线下支付</a></li>
                <li role="presentation"><a href="COD.aspx">货到付款</a></li>
            </ul>
            <div class="set-switch">
                <strong>支付宝收款设置</strong>
                <p>还没有开通支付宝支付？<a target="_blank" href="https://b.alipay.com/order/productDetail.htm?productId=2014110308142133">在线申请</a></p>
                <p>目前微信屏蔽了支付宝的链接，使用支付宝支付需要在浏览器中打开支付链接。</p>
                <div id="offlineEnable" class="<%=_enable?"switch-btn":"switch-btn off" %>" onclick="setEnable(this)">
                    <%=_enable?"已开启":"已关闭"%>
                    <i></i>
                </div>
            </div>

            <%--<div class="lineCss" style="margin-top: -18px; margin-bottom:5px;"></div>--%>

            <%--<div class="write-form form-horizontal" style="width: 800px;">--%>
            <div id="maindiv" style="<%=_enable?"": "display:none" %>">
                <div class="form-group">
                    <label for="inputEmail1" class="col-xs-2 control-label">支付宝账号：</label>
                    <div class="col-xs-4">
                        <asp:TextBox runat="server" class="form-control" ID="txt_mid"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <label for="inputEmail2" class="col-xs-2 control-label">支付宝账号姓名：</label>
                    <div class="col-xs-4">
                        <asp:TextBox runat="server" class="form-control" ID="txt_mName"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <label for="inputEmail3" class="col-xs-2 control-label">合作者身份(PID)：</label>
                    <div class="col-xs-4">
                        <asp:TextBox runat="server" class="form-control" ID="txt_pid"></asp:TextBox>
                        <small class="help-block">成功申请支付宝接口后获取到的PID</small>
                    </div>
                </div>
                <div class="form-group">
                    <label for="inputEmail4" class="col-xs-2 control-label">安全校验码(Key)：</label>
                    <div class="col-xs-4">
                        <asp:TextBox runat="server" class="form-control" ID="txt_key"></asp:TextBox>
                        <small class="help-block">成功申请支付宝接口后获取到的Key</small>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-xs-offset-2 marginl">
                        <asp:Button runat="server" OnClick="Unnamed_Click" class="btn btn-success bigsize"
                            OnClientClick="return beforeSaveData(this)" Text="保存" />
                    </div>
                </div>
            </div>
        </div>
    </form>
</asp:Content>
