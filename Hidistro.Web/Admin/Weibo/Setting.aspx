<%@ Page Title="绑定微博帐号" Language="C#" MasterPageFile="~/Admin/AdminNew.Master" AutoEventWireup="true" CodeBehind="Setting.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Shop.Settings" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>

<%@ Import Namespace="Hidistro.Core" %>
<%@ Import Namespace="System.Web.UI.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="/admin/css/weibo.css">
    <%-- <link rel="stylesheet" href="http://b.yzcdn.cn/v2/build_css/stylesheets/www/pages/pc/setting_all_8324c733e1.css" onerror="_cdnFallback(this)">--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script>
        $(function () {
            if ($.trim($("#ctl00_ContentPlaceHolder1_txtAccess_Token").val()) != "")
                loaduserinfo();

        });

        function loaduserinfo() {
            $.getJSON("../../API/WeiBoAPI.ashx?action=userinfo").done(function (d) {
                if (typeof (d.IsAuthorized) != "undefined") {
                    if (d.IsAuthorized == "0") {
                        HiTipsShow('授权失败！', 'fail');
                        return;
                    }
                }
                if (typeof (d.error) != "undefined") {

                    HiTipsShow(d.error, 'fail');
                    return
                }
                $("#screen_name").text(d.screen_name);
                if ($.trim(d.id) != "") {
                    $("#authorization").text('已授权');
                    $("#ctl00_ContentPlaceHolder1_cancel").css("display", "");
                }
            });
        }

    </script>
    <form class="form-horizontal" runat="server">
        <div class="page-header">
            <h2>绑定微博帐号</h2>
            <small></small>
        </div>
        <fieldset>
            <div class="control-group">
                <label class="control-label">微博昵称：</label>
                <div class="controls"><span class="sink" id="screen_name">未绑定</span></div>
            </div>
            <div class="control-group">
                <label class="control-label">授权情况：</label>
                <div class="controls"><span class="sink" id="authorization">未授权</span></div>

                <Hi:ImageLinkButton ID="cancel" Style="margin-left: 10px; display: none" DeleteMsg="是否确定解除新浪微博授权?" CssClass="js-show-cancel-modal" OnClick="cancel_Click" runat="server" Text="解除授权" IsShow="true" />

            </div>

            <ol class="serial-list">
                <li>
                    <div class="serial-title">
                        <span class="serial">1</span>
                        <p class="serial-title-content">
                            登录新浪微博，进入个人页面，点击菜单管理中心 -&gt; 粉丝服务。
                        </p>
                    </div>
                    <div class="setting-weibo-image setting-weibo-image-1"></div>
                    <div class="setting-weibo-image setting-weibo-image-prevent-menu"></div>
                    <div class="setting-weibo-image setting-weibo-image-prevent-auto"></div>
                    <p><span style="color: #f00">关闭</span>自定义菜单和自动回复。</p>
                </li>
                <li>
                    <div class="serial-title">
                        <span class="serial">2</span>
                        <p class="serial-title-content">
                            开启开发模式，配置服务器信息
                        </p>
                    </div>
                    <div class="setting-weibo-image setting-weibo-image-2"></div>
                    <div style="font-size: larger; color: blue; margin-bottom:10px; margin-top:10px;">
                        微博服务器配置：
                    </div>
                    <p>
                        1. 选中消息推送服务
                    </p>
                    <p>
                        2. 把下列 URL 和 APPKEY 填写到微博服务器配置信息的输入框（可以点击[ 复制 ]按钮复制）；如果以前填写过URL和Token，请点击[ 修改 ] ，再填写以下信息
                    </p>


                    <div class="form-horizontal">
                        <div class="form-group">
                            <label for="inputEmail3" class="col-xs-1 control-label">URL:</label>
                            <div class="col-xs-5">

                                <input style="width: 400px;" class="form-control" type="text" id="url" placeholder="" name="url" value="" disabled="">
                            </div>
                            <samp>
                                <button type="button" class="btn btn-primary" data-clipboard-target="url" id="d_clip_button">复制</button></samp>
                        </div>
                        <div class="form-group">
                            <label for="inputEmail3" class="col-xs-1 control-label">Appkey:</label>
                            <div class="col-xs-3">

                                <input class="form-control" type="text" id="appid" placeholder="" name="appid" value="4281069216" disabled="" style="width: 200px;">
                            </div>
                            <samp>
                                <button type="button" class="btn btn-primary" data-clipboard-target="appid" id="appkey_clip">复制</button></samp>
                        </div>
                    </div>

                    <p>
                        3. 点击[ 保存 ]已启用服务器配置
                    </p>
                    <div class="auth-wx-image wx-image-4-0711"></div>
                </li>
                <li>
                    <div class="serial-title">
                        <span class="serial">3</span>
                        <p class="serial-title-content">
                            保存之后会出现如下信息。
                        </p>
                    </div>

                    <div class="setting-weibo-image setting-weibo-image-3"></div>
                     <div style="font-size: larger; color: blue; margin-bottom:10px; margin-top:10px;">
                        系统access_token配置信息：
                    </div>
                    <p>将 access_token 填写到下面。如果设置过access_token，请点击右侧[ 修改 ]重新设置</p>
                    <div class="form-horizontal">
                        <div class="form-group">
                            <label for="inputEmail3" class="col-xs-2 control-label"><em>*</em>access_token:</label>
                            <div class="col-xs-3">
                                <asp:TextBox ID="txtAccess_Token" CssClass="form-control" runat="server" Width="300" />

                            </div>
                        </div>
                    </div>

                </li>
            </ol>

        </fieldset>
        <div class="form-actions" style="padding-left: 150px; margin-bottom: 20px;">

            <asp:Button runat="server" ID="btnAdd" Text="授  权" OnClick="btnOK_Click" CssClass="btn btn-primary js-submit-btn" />
        </div>
    </form>
    <script src="../js/ZeroClipboard.min.js"></script>
    <script>
        $(function () {
            $("#url").val("http://"+window.location.host + "/API/wb.ashx");
            $('#aspnetForm').formvalidation({
                'ctl00$ContentPlaceHolder1$txtAccess_Token': {
                    validators: {
                        notEmpty: {
                            message: "access_token名称不能为空"
                        }, stringLength: {
                            min: 1,
                            max: 200,
                            message: ''
                        }
                    }
                }
            })
        })
        var copy = new ZeroClipboard(document.getElementById("appkey_clip"), {
            moviePath: "../js/ZeroClipboard.swf"
        });
        copy.on('complete', function (client, args) {
            HiTipsShow("复制成功，复制内容为：" + args.text, 'success');
        });
        var copy = new ZeroClipboard(document.getElementById("d_clip_button"), {
            moviePath: "../js/ZeroClipboard.swf"
        });
        copy.on('complete', function (client, args) {
            HiTipsShow("复制成功，复制内容为：" + args.text, 'success');
        });
    </script>
</asp:Content>
