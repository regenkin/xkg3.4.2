<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EmailSettings.aspx.cs"
    MasterPageFile="~/Admin/AdminNew.Master" Inherits="Hidistro.UI.Web.Admin.promotion.EmailSettings" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <Hi:Script ID="Script1" runat="server" Src="/utility/plugin.js" />
    <script type="text/javascript">
        $(document).ready(function () {
            pluginContainer = $("#pluginContainer");
            templateRow = $(pluginContainer).find("[rowType=attributeTemplate]");
            dropPlugins = $("#ddlEmails");
            selectedNameCtl = $("#<%=txtSelectedName.ClientID %>");
            configDataCtl = $("#<%=txtConfigData.ClientID %>");

            // 绑定邮件类型列表
            $(dropPlugins).append($("<option value=\"\">-请选择发送方式-</option>"));
            $.ajax({
                url: "PluginHandler.aspx?type=EmailSender&action=getlist",
                type: 'GET',
                async: false,
                dataType: 'json',
                timeout: 10000,
                success: function (resultData) {
                    if (resultData.qty == 0)
                        return;

                    $.each(resultData.items, function (i, item) {
                        if (item.FullName == $(selectedNameCtl).val())
                            $(dropPlugins).append($(String.format("<option value=\"{0}\" selected=\"selected\">{1}</option>", item.FullName, item.DisplayName)));
                        else
                            $(dropPlugins).append($(String.format("<option value=\"{0}\">{1}</option>", item.FullName, item.DisplayName)));
                    });
                }
            });

            $(dropPlugins).bind("change", function () { SelectPlugin("EmailSender"); });

            if ($(selectedNameCtl).val().length > 0) {
                SelectPlugin("EmailSender");
            }
        });

        function TestCheck() {
            if ($(dropPlugins).val() == "") {
                alert("请先选择发送方式并填写配置信息");
                return false;
            }
            return true;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form id="thisForm" runat="server" class="form-horizontal">

    <div>
        <div class="page-header">
            <h2>邮件设置管理</h2>
            <small>邮件设置提供修改邮件连接信息</small>
        </div>
        <div>
            <div class="formitem">
                <div class="emailhead">
                    <p>选择ASP.NET邮件组件方式进行正确设置，用以开通自动向用户发送如注册、订单付款等邮件</p>
                    <p>
                        如果您需要经常面向大量用户邮箱进行邮件群发，建议你开通更高品质的<font style="color: Red">EDM邮件营销服务。</font><a href="http://zzfw.pufang.net/edm/
"
                            target="_blank">点此开通</a>
                    </p>
                </div>
                <ul id="pluginContainer" style="margin: 10px 0px;">
                    <li style="margin-bottom: 0px;"><span class="formitemtitle Pw_140">发送方式：</span>
                        <span class="formselect">
                            <select id="ddlEmails" name="ddlEmails"></select></span>
                    </li>
                    <li rowtype="attributeTemplate" style="display: none;"><span class="formitemtitle Pw_140">
                        $Name$：</span>
                        $Input$
                    </li>
                </ul>
                <ul class="btntf Pa_140" style="margin: 5px 0px; height: 30px;">
                    <asp:Button ID="btnChangeEmailSettings" runat="server" Text="保 存" CssClass="submit_DAqueding float">
                    </asp:Button>
                </ul>
            </div>
            <div class="formitem">
                <ul style="margin: 5px 0px; height: 30px;">
                    <li><span class="formitemtitle Pw_140">测试邮箱：</span>
                        <asp:TextBox runat="server" ID="txtTestEmail" CssClass="forminput" />
                    </li>
                </ul>

                <ul class="btntf Pa_140 clear" style="margin: 10px 0px; height: 30px;">
                    <asp:Button ID="btnTestEmailSettings" runat="server" OnClientClick="return TestCheck();"
                        Text="发送测试邮件" CssClass="submit_DAqueding inbnt"></asp:Button>
                </ul>
            </div>
        </div>
    </div>

    <asp:HiddenField runat="server" ID="txtSelectedName" />
    <asp:HiddenField runat="server" ID="txtConfigData" />

    </form>


</asp:Content>

