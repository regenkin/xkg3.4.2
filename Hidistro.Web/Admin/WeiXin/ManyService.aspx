<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminNew.Master" AutoEventWireup="true" CodeBehind="ManyService.aspx.cs" Inherits="Hidistro.UI.Web.Admin.WeiXin.ManyService" %>

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
                url: "manyservice.aspx",
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

                        ShowMsg("多客服设置" + opname + '！', true);
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
        <h2>多客服设置</h2>
    </div>
    <div class="set-switch">
        <p>微信多客服系统能够全面帮助您在微信上与用户沟通,及时了解用户咨询信息,不错过任何一个客户。</p>
        <p style="color: red">(仅认证服务号可在公众平台开通多客服，客服账号在微信公众平台添加和管理）。</p>
        <p style="color: red">关闭多客服以后，无匹配自动回复自动生效，用户微信咨询信息不会提醒客服人员</p>
        <p style="color: red">开启多客服以后，无匹配自动回复自动失效，用户微信咨询信息轮流分配并提醒客服人员（不论客服是否登录都会分配）</p>
        <div id="guidepageEnable" class="<%=enableManyService?"switch-btn":"switch-btn off" %>" onclick="setEnable(this)">
            <%=enableManyService?"已开启":"已关闭"%>
            <i></i>
        </div>
    </div>
</asp:Content>
