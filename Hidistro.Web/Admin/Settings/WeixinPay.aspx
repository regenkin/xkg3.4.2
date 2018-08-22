<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Admin/AdminNew.Master"
    CodeBehind="WeixinPay.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Settings.WeixinPay" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <Hi:Style ID="Style1"  runat="server" Href="/admin/css/bootstrapSwitch.css" />
    <Hi:Script ID="Script1" runat="server" Src="/admin/js/bootstrapSwitch.js" />
    <style type="text/css">
        .lineCss{background-color:green;height:2px;width:auto;}
        .msgDiv{background-color:#D3FBFE; border:1px solid; border-color:#0c9e0e; height:50px; margin-top:10px;margin-bottom:10px;}
        .spcss{display:none}
    </style>
    <script type="text/javascript">
        function setEnable(obj) {

         

            var type = "-5";
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
                        msg('微信支付已开启！');
                        $('#maindiv').css('display', '');
                    }
                    else {
                        msg('微信支付已关闭！');
                        $('#maindiv').css('display', 'none');
                    }
                }
            });
        }

        function msg(msg) {
            HiTipsShow(msg, 'success');
        }

        function errAlert(msg) {
            HiTipsShow(msg, 'error');
        }

        function beforeSaveData(obj) {
     
            var mch_id = $('#<%=txt_mch_id.ClientID%>').val();
            var key = $('#<%=txt_key.ClientID%>').val();
           <%-- if (appid == "") {
                errAlert("请输入appid!");
                $('#<%=txt_appid.ClientID%>').focus();
                return false;
            }
            if (appsecret == "") {
                errAlert("请输入appsecret!");
                $('#<%=txt_appsecret.ClientID%>').focus();
                return false;
            }--%>
            if (mch_id == "") {
                errAlert("请输入mch_id!");
                $('#<%=txt_mch_id.ClientID%>').focus();
                return false;
            }
            if (key == "") {
                errAlert("请输入Key!");
                $('#<%=txt_key.ClientID%>').focus();
                return false;
            }
            return true;
        }

        $(function () {
            $('#aspnetForm').formvalidation({
                 'ctl00$ContentPlaceHolder1$txt_mch_id': {
                    validators: {
                        notEmpty: {
                            message: '微信支付商户号，审核通过后，在微信发送的邮件中查看。'
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
                            message: '商户支付密钥Key。请登录<a target="_blank" href="https://pay.weixin.qq.com/index.php/home/login?return_url=%2F"> 微信支付商户平台</a> ，在【账户设置-安全设置-API安全】中设置。'
                        }
                    }
                }
            });

            if ($("#ctl00_ContentPlaceHolder1_EnableSP").prop("checked")) {
                $(".spcss").show();
                $(".keycss").hide();
            }

      
            $('#mySwitch').on('switch-change', function (e, data) {
              
                if (data.value) {
                    $(".spcss").show();
                    $(".keycss").hide();
                    $('html, body, .content').animate({ scrollTop: $(document).height() }, 300)
                } else {
                    $(".spcss").hide();
                    $(".keycss").show();
                };

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
                <li role="presentation" class="active"><a href="WeixinPay.aspx">微信支付</a></li>
                <li role="presentation" ><a href="Alipay.aspx">支付宝</a></li>
                <%--<li role="presentation"><a href="ChinaBank.aspx">网银在线</a></li>--%>
                <li role="presentation" ><a href="ShengPay.aspx">盛付通</a></li>
                <li role="presentation" ><a href="OfflinePay.aspx">线下支付</a></li>
                <li role="presentation"><a href="COD.aspx">货到付款</a></li>
            </ul>
            <div class="set-switch">
                <strong>微信支付收款设置</strong>
                <p>还没开通微信支付？<a target="_blank" href="https://mp.weixin.qq.com/cgi-bin/readtemplate?t=news/open-app-apply-guide_tmpl&lang=zh_CN">立即免费申请开通微信支付接口</a></p>
                <p>微信支付只支持会员在微信客户端购买付款，如需支持客户在其他浏览器上购买，请设置其他收款方式。</p>
                <div id="offlineEnable" class="<%=_enable?"switch-btn":"switch-btn off" %>" onclick="setEnable(this)">
                    <%=_enable?"已开启":"已关闭"%>
                    <i></i>
                </div>

            </div>            
    
            <%--<div class="write-form form-horizontal" style="width: 800px; margin-top:10px;">--%>
            <div id="maindiv" style="<%=_enable?"": "display:none" %>">
                <div class="form-group">
                    <label for="inputEmail2" class="col-xs-2 control-label">公众号AppId：</label>
                    <div class="col-xs-4">
                        <asp:Label CssClass="form-control" ID="lblAppId" runat="server"></asp:Label>
                        <small class="help-block">微信公众号身份的唯一标识</small>
                    </div>
                </div>
                <div class="form-group">
                    <label for="inputEmail3" class="col-xs-2 control-label">公众号AppSecret：</label>
                    <div class="col-xs-4">
                        <asp:Label CssClass="form-control" ID="lblAppSecret" runat="server"></asp:Label>
                        <small class="help-block">审核后在公众平台开启开发模式后可查看</small>
                    </div>
                </div>
                <div class="form-group" >
                    <label for="inputEmail3" class="col-xs-2 control-label">
                        商户号MCH_ID：</label>
                    <div class="col-xs-4">
                        <asp:TextBox runat="server" class="form-control" ID="txt_mch_id"></asp:TextBox>
                        <small class="help-block">微信支付商户号，审核通过后，在微信发送的邮件中查看。</small>
                    </div>
                </div>
                <div class="form-group keycss">
                    <label for="inputEmail3" class="col-xs-2 control-label">Key：</label>
                    <div class="col-xs-4">
                        <asp:TextBox runat="server" class="form-control" ID="txt_key"></asp:TextBox>
                        <small class="help-block">商户支付密钥Key<%--，服务商模式下无需填写--%>。</small>
                    </div>
                </div>
                  <div class="form-group" id="alipaypanel" runat="server" style="display:none">
                        <label for="inputEmail3" class="col-xs-2 control-label"><em>*</em>服务商模式：</label>
                        <div class="col-xs-7">
                            <div class="switch" id="mySwitch">
                                <input id="EnableSP" type="checkbox"  runat="server"/>
                            </div>
                            <small>如果您的微信支付是服务商代为开通的，请开启，并向服务商索取相关参数</small>
                        </div>
                    </div>
                 <div class="form-group  spcss">
                    <label for="inputEmail3" class="col-xs-2 control-label">
                        服务商AppID：</label>
                    <div class="col-xs-4">
                        <asp:TextBox runat="server" class="form-control" ID="Main_AppId"></asp:TextBox>
                        <small class="help-block">服务商AppID，由服务商提供。</small>
                    </div>
                </div>
                 <div class="form-group  spcss">
                    <label for="inputEmail3" class="col-xs-2 control-label">
                        服务商商户ID：</label>
                    <div class="col-xs-4">
                        <asp:TextBox runat="server" class="form-control" ID="Main_Mch_ID"></asp:TextBox>
                        <small class="help-block">服务商商户号，由服务商提供。</small>
                    </div>
                </div>
                <div class="form-group  spcss">
                    <label for="inputEmail3" class="col-xs-2 control-label">
                        服务商KEY：</label>
                    <div class="col-xs-4">
                        <asp:TextBox runat="server" class="form-control" ID="Main_PayKey"></asp:TextBox>
                        <small class="help-block">服务商签名KEY，由服务商提供。</small>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-xs-offset-2 col-xs-10 marginl">
                        <asp:Button ID="btnSave" runat="server" OnClick="Unnamed_Click" class="btn btn-success inputw100"
                            OnClientClick="return beforeSaveData(this)" Text="保存" />
                    </div>
                </div>
             </div>
            </div>

        <%--</div>--%>
    </form>

</asp:Content>