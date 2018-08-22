<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminNew.Master" AutoEventWireup="true" CodeBehind="RedpackSetting.aspx.cs" Inherits="Hidistro.UI.Web.Admin.WeiXin.RedpackSetting" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function setEnable(obj) {
            var ob = $("#" + obj.id);
            var cls = ob.attr("class");
            var enable = 0;
            if (cls != "switch-btn") {
                enable = 1;
            }
            $.ajax({
                type: "post",
                url: "redpacksetting.aspx",
                data: { enable: enable, action: "setenable" },
                dataType: "json",
                success: function (json) {
                    if (json.type == 1) {
                        var opname = "已关闭";
                        if (cls == "switch-btn") {
                            ob.empty();
                            ob.append(opname + " <i></i>")
                            ob.removeClass();
                            ob.addClass("switch-btn off");
                        }
                        else {
                            opname = "已开启";
                            ob.empty();
                            ob.append(opname + " <i></i>")
                            ob.removeClass();
                            ob.addClass("switch-btn");
                        }

                        ShowMsg("微信证书设置" + opname + '！', true);
                    }
                    else {
                        ShowMsg('操作失败！', false);
                    }
                }
            });
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="page-header">
        <h2>微信证书设置</h2>
    </div>
    <div class="set-switch">
        <strong>提示</strong>
        <h3 style="font-size: 14px">还没开通微信支付？ <a target="_blank" href="https://mp.weixin.qq.com/cgi-bin/readtemplate?t=news/open-app-apply-guide_tmpl&lang=zh_CN">立即免费申请开通微信支付接口</a></h3>
        <p>设置好微信证书以后，才能使用微信红包和微信批量放款相关功能。</p>
        <div id="guidepageEnable" class="<%=enableWXRequest?"switch-btn":"switch-btn off" %>" onclick="setEnable(this)">
            <%=enableWXRequest?"已开启":"已关闭"%>
            <i></i>
        </div>

    </div>
    <div>
        <p><a href="../Settings/WeixinPay.aspx" class="inlineBlockBg">微信支付收款设置</a></p>
        <p>如已成功设置微信支付收款，请忽略以上提示，直接设置微信证书。</p></div>
    <form runat="server" class="form-horizontal">
        <div class="form-group">
            <label class="col-xs-2 control-label"><em></em>微信证书：</label>
            <div class="col-xs-6">
                 <asp:FileUpload runat="server" ID="fileUploader" CssClass="forminput" /><span style="color:red"><label runat="server" id="labfilename"></label>&nbsp;</span>
                <small>上传格式为*****.p12,如：apiclient_cert.p12</small>
            </div>
        </div>
         <div class="form-group">
            <label class="col-xs-2 control-label"><em></em>证书密码：</label>
            <div class="col-xs-6">
                <asp:TextBox ID="txtCertPassword" CssClass="form-control inputw200" runat="server" />
            </div>
        </div>
        <div class="form-group">
                        <div class="col-xs-10 col-xs-offset-2">
                            <asp:Button ID="btnOK" runat="server" Text="保 存" CssClass="btn btn-success float inputw100"
                        OnClientClick="return PageIsValid();" OnClick="btnOK_Click" />
               </div>
                    </div>
    </form>
</asp:Content>
