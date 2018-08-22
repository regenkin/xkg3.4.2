<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ShengPay.aspx.cs" MasterPageFile="~/Admin/AdminNew.Master"
    Inherits="Hidistro.UI.Web.Admin.Settings.ShengPay" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <Hi:Script ID="Script4" runat="server" Src="/admin/js/jquery.formvalidation.js" />
    <style type="text/css">
        .lineCss{background-color:green;height:2px;width:auto;}
        .msgDiv{background-color:#D3FBFE; border:1px solid; border-color:#0c9e0e;  margin-top:10px;margin-bottom:10px;}
    </style>
    <script type="text/javascript">
        function setEnable(obj) {
            var type = "-3";
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
                        msg('盛付通支付已开启！');
                        $('#maindiv').css('display', '');
                    }
                    else {
                        msg('盛付通支付已关闭！');
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
            var key = $('#<%=txt_key.ClientID%>').val();
            if (mid == "") {
                errAlert("请输入商户号!");
                $('#<%=txt_mid.ClientID%>').focus();
                return false;
            }
            if (key == "") {
                errAlert("请输入商家密钥（Key）!");
                $('#<%=txt_key.ClientID%>').focus();
                return false;
            }          
            
            return true;
        }

        function errAlert(msg) {
            HiTipsShow(msg, 'error');
        }

        $(function () {
            $('#aspnetForm').formvalidation({

                'ctl00$ContentPlaceHolder1$txt_mid': {
                    validators: {
                        notEmpty: {
                            message: '请输入商户号'
                        },
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
                            message: '请输入商家密钥（Key）'
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
                <li role="presentation" ><a href="Alipay.aspx">支付宝</a></li>
               <%-- <li role="presentation"><a href="ChinaBank.aspx">网银在线</a></li>--%>
                <li role="presentation" class="active"><a href="ShengPay.aspx">盛付通</a></li>
                <li role="presentation" ><a href="OfflinePay.aspx">线下支付</a></li>
                <li role="presentation"><a href="COD.aspx">货到付款</a></li>
            </ul>
            <div class="set-switch">
                <strong>盛付通收款设置</strong>
                <p>还没开通盛付通支付？<a target="_blank" href="https://zhuanye.shengpay.com/SP/Business/quicklygather.aspx">免费申请开通盛付通支付接口</a></p>
                <div id="offlineEnable" class="<%=_enable?"switch-btn":"switch-btn off" %>" onclick="setEnable(this)">
                    <%=_enable?"已开启":"已关闭"%>
                    <i></i>
                </div>

            </div>
            <div id="maindiv" style="<%=_enable?"": "display:none" %>">
            <div class="msgDiv" style="display:none;">
                <div style="font-weight: bold; margin-left: 5px;">提示</div>
                <div style="margin-left: 5px;">商家通过线上提交申请后，等待商家信息审核通过，完成签约即能为商家开通权限。请复制盛付通提供的商户号和商家密钥，填写保存后即成功完成收款设置。</div>

            </div>
            <%--<div class="write-form form-horizontal" style="width: 800px; margin-top:5px;">--%>
                
                <div class="form-group">
                    <label for="inputEmail2" class="col-xs-2 control-label">商户号：</label>
                    <div class="col-xs-4">
                        <asp:TextBox runat="server" class="form-control" ID="txt_mid"></asp:TextBox>            
                    </div>
                </div>
                <div class="form-group">
                    <label for="inputEmail3" class="col-xs-2 control-label">商家密钥（Key）：</label>
                    <div class="col-xs-4">
                        <asp:TextBox runat="server" class="form-control" ID="txt_key"></asp:TextBox>
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


        <%--</div>--%>
    </form>

</asp:Content>