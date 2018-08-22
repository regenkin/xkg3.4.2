<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminNew.Master" AutoEventWireup="true" CodeBehind="ConcernUrl.aspx.cs" Inherits="Hidistro.UI.Web.Admin.WeiXin.ConcernUrl" %>

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
                url: "concernurl.aspx",
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

                        ShowMsg(opname + '！', true);
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
        <h2>一键关注</h2>
    </div>

    <div class="set-switch">
        <strong>提示</strong>
        <p>开启以后，会员可以通过首页面板的关注链接来关注您的微信公众号。<a href="../help/yijianguanzhu.html" target="_blank" class="ml20">开启指引</a></p>
        <div id="guidepageEnable" class="<%=enableGuidePageSet?"switch-btn":"switch-btn off" %>" onclick="setEnable(this)">
            <%=enableGuidePageSet?"已开启":"已关闭"%>
            <i></i>
        </div>
    </div>

    <form id="form1" runat="server" class="form-horizontal">
        <div class="form-group">
            <label class="col-xs-2 control-label resetSize"><strong>微信引导页面地址：</strong></label>
            <div class="col-xs-5">
                <asp:TextBox ID="txtGuidePageSet" runat="server" CssClass="form-control resetSize" MaxLength="500"></asp:TextBox>
                <small class="mt10">未关注公众号时，引导至此页面 <a href="../help/yijianguanzhu.html" target="_blank" class="fr">如何制作引导页面</a></small>
            </div>
        </div>
        <div class="form-group">
            <div class="col-xs-10 col-xs-offset-2">
                <asp:Button ID="btnSave" runat="server" Text="保存" CssClass="btn btn-success inputw100" OnClick="btnSave_Click" />
            </div>
        </div>

    </form>
</asp:Content>
