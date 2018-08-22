<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminNew.Master" AutoEventWireup="true" CodeBehind="ValidationService.aspx.cs" Inherits="Hidistro.UI.Web.Admin.WeiXin.ValidationService" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function setEnable(obj, type) {
            var action = "setenable";
            var tipspre = "会员公众号登录授权";
            if (type == 1) {
                action = "setautologin";
                tipspre = "微信端一进入店铺就要登录";
            }
            var ob = $("#" + obj.id);
            var cls = ob.attr("class");
            var enable = 0;
            if (cls != "switch-btn") {
                enable = 1;
            }
            $.ajax({
                type: "post",
                url: "validationservice.aspx",
                data: { enable: enable, action: action },
                dataType: "json",
                success: function (json) {
                    if (json.type == 1) {
                        var opname = "已关闭";
                        if (cls == "switch-btn") {
                            ob.empty();
                            ob.append(opname + " <i></i>")
                            ob.removeClass();
                            ob.addClass("switch-btn off");
                            /*授权登录关闭后，下面的自动登录也要关闭*/
                            ob = $("#guideAutoLogin");
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

                        ShowMsg(tipspre + opname + '！', true);
                    }
                    else {
                        ShowMsg(json.tips, false);
                    }
                }
            });
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="page-header">
        <h2>会员登录授权</h2>
    </div>
    <div class="set-switch">
        <p>开启以后，会员可在微信端通过微信授权快速登录系统。</p>
        <p class="red">(仅认证服务号可用）</p>
        <div id="guidepageEnable" class="<%=enableValidationService?"switch-btn":"switch-btn off" %>" onclick="setEnable(this,0)">
            <%=enableValidationService?"已开启":"已关闭"%>
            <i></i>
        </div>
    </div>

    
    <div class="set-switch">
        <p>开启以后，首次访问微信端商城任何页面的用户，都将被要求先进行微信授权登录<span class="red">（需要系统先开启微信授权登录）。</span></p>
        <p>&nbsp;</p>
        <div id="guideAutoLogin" class="<%=enableIsAutoToLogin?"switch-btn":"switch-btn off" %>" onclick="setEnable(this,1)">
            <%=enableIsAutoToLogin?"已开启":"已关闭"%>
            <i></i>
        </div>
    </div>
</asp:Content>
