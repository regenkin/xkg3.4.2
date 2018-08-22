<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Admin/AdminNew.Master"
    CodeBehind="WeixinRed.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Settings.WeixinRed" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <Hi:Script ID="Script1" runat="server" Src="/admin/js/jquery.formvalidation.js" />
    <style type="text/css">
        .lineCss{background-color:green;height:2px;width:auto;}
        .msgDiv{background-color:#D3FBFE; border:1px solid; border-color:#0c9e0e; height:50px; margin-top:10px;margin-bottom:10px;}
        .divBtn{background-color:#009900;border:none;width:130px;height:30px; line-height:30px; color:white; text-align:center;cursor:pointer;}
    </style>
    <Hi:Script ID="Script5" runat="server" Src="/utility/jquery.artDialog.js" />
    <Hi:Script ID="Script7" runat="server" Src="/utility/iframeTools.js" />
    <Hi:Script ID="Script6" runat="server" Src="/utility/Window.js" />
    <Hi:Script ID="Script4" runat="server" Src="/admin/js/jquery.formvalidation.js" />
    <script type="text/javascript">
        function setEnable(obj) {
            var type = "-7";
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
                        msg('微信红包已开启！');
                        $('#maindiv').css('display', '');
                    }
                    else {
                        msg('微信红包已关闭！');
                        $('#maindiv').css('display', 'none');
                    }
                }
            });
        }

        function msg(msg) {
            HiTipsShow(msg, 'success');
        }
        
        function Check()
        {            
            if ($('#<%=txt_key.ClientID%>').val() == "")
            {
                errAlert("请输入证书密码!");
                //setTimeout(function () { HiTipsShow("请输入证书密码", 'warning') }, 300);
                return false;
            }
            return true;
        }

        function errAlert(msg) {
            HiTipsShow(msg, 'error');
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form id="thisForm" runat="server" class="form-horizontal">

        <div>
            <div class="set-switch">
                <strong>微信红包设置</strong>
                <p>开通了微信支付的商户可以通过此接口向自己的用户发放红包来进行促销活动。</p>
                <p>还没开通微信支付？<a href="https://mp.weixin.qq.com/cgi-bin/readtemplate?t=news/open-app-apply-guide_tmpl&lang=zh_CN">立即免费申请开通微信支付接口</a>
                </p>
                <div id="offlineEnable" class="<%=_enable?"switch-btn":"switch-btn off" %>" onclick="setEnable(this)">
                    <%=_enable?"已开启":"已关闭"%>
                    <i></i>
                </div>

            </div>
            <div class="lineCss" style="margin-top: -18px;"></div>

            <div id="maindiv" style="<%=_enable?"": "display:none" %>">
            <div class="msgDiv">
                <div style="font-weight: bold; margin-left: 5px;">提示</div>
                <div style="margin-left: 5px;">
                    开通微信红包设置的前提是必须先开通微信支付，并在微信支付收款设置设置成功后方能使用！证书和证书密码用于管理员微信发红包。
                </div>
            </div>
            
            <div>
                <table>
                    <tr>
                        <td>
                            <div class="divBtn" onclick="location.href='WeixinPay.aspx'">
                                微信支付收款设置 
                            </div>
                        </td>
                        <td >
                            <small class="help-block" style="margin-left: 5px;">微信公众号身份的唯一标识</small>
                        </td>
                    </tr>
                </table>                
            </div>
                      

            <%--<div class="write-form form-horizontal" style="width: 800px; margin-top: 10px;">--%>

                <div class="form-group">
                    <label for="inputEmail2" class="col-xs-2 control-label">
                        微信证书：</label>
                    <div class="col-xs-4">
                        <asp:FileUpload runat="server" ID="fileUploader" CssClass="form-control"  />
                        <span style="color: red">
                            <label runat="server" id="labfilename"></label>
                            &nbsp;
                        </span>
                        <small class="help-block">上传格式为*****.p12,如：apiclient_cert.p12</small>
                    </div>
                </div>
                <div class="form-group">
                    <label for="inputEmail3" class="col-xs-2 control-label">
                        证书密码：</label>
                    <div class="col-xs-4">
                        <asp:TextBox runat="server" class="form-control" ID="txt_key"></asp:TextBox>
                        <small class="help-block">证书密码就是微信支付商户号（即MCH_ID）</small>

                    </div>
                </div>                
                <div class="form-group">
                    <div class="col-xs-offset-2 marginl">
                       
                        <asp:Button runat="server" CssClass="btn btn-success bigsize" Text="保存" OnClientClick="return Check();" OnClick="Unnamed_Click" />
                    </div>
                </div>
            </div>
            </div>

        <%--</div>--%>
        </form>
    <script>

        $(function () {
            $('#aspnetForm').formvalidation({

                'ctl00$ContentPlaceHolder1$txt_key': {
                    validators: {
                        notEmpty: {
                            message: '证书密码就是微信支付商户号（即MCH_ID）'
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