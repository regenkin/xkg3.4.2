<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminNew.Master" AutoEventWireup="true" CodeBehind="WXConfig.aspx.cs" Inherits="Hidistro.UI.Web.Admin.WeiXin.WXConfig" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../js/ZeroClipboard.min.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            var copy = new ZeroClipboard(document.getElementById("copyurl"), {
                moviePath: "../js/ZeroClipboard.swf"
            });
            copy.on('complete', function (client, args) {
                HiTipsShow("复制成功，复制内容为：" + args.text, 'success');
            });

            var copy2 = new ZeroClipboard(document.getElementById("copytoken"), {
                moviePath: "../js/ZeroClipboard.swf"
            });
            copy2.on('complete', function (client, args) {
                HiTipsShow("复制成功，复制内容为：" + args.text, 'success');

                window.location.href = "WXConfigBinkOK.aspx";
            });
        })

        function copyurl(obj) {
            var copy = new ZeroClipboard(document.getElementById(obj), {
                moviePath: "../js/ZeroClipboard.swf"
            });
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

            <div class="page-header">
                    <h2>绑定微信公众号</h2>
                </div>
                <div class="set-switch">
                    <div class="iconimg"><i class="glyphicon glyphicon-info-sign"></i></div>
                    <div class="info">
                        <strong>重要提示</strong>
                        <p class="color" style="color:red; line-height:25px;">后期只有在所有使用微信授权登录的会员关联绑定系统用户名以后，您才能更换绑定其他微信公众号，请谨慎操作！</p>
                        <a href="../help/zhiyin.html" target="_blank">查看操作指南</a>
                    </div>
                </div>
    <form runat="server">
                <div class="titileBorderBox">
                    <h3><strong>第一步：</strong></h3>
                    <div class="contentBox">
                        <p class="mb10">复制以下信息至 <a target="_blank" href="http://mp.weixin.qq.com">微信公众平台</a> ->开发者中心->服务器配置中，并提交！</p>
                        <div class="form-inline">
                            <div class="form-group">
                                <label><span>Url：</span><asp:Literal runat="server" ID="txtUrl"></asp:Literal></label>
                            </div>
                            <asp:HiddenField ID="hdfCopyUrl" runat="server" />
                            <button type="button" class="btn resetSize btn-default" id="copyurl" data-clipboard-target="<%=hdfCopyUrl.ClientID %>" onclick="copyurl('copyurl');">复制</button>
                        </div>
                        <div class="form-inline">
                            <div class="form-group">
                                <label><span>Token：</span><asp:Literal runat="server" ID="txtToken"></asp:Literal></label>
                            </div>
                            <asp:HiddenField ID="hdfCopyToken" runat="server" />
                            <button type="button" class="btn resetSize btn-default" id="copytoken" data-clipboard-target="<%=hdfCopyToken.ClientID %>" onclick="copyurl('copytoken');">复制</button>
                        </div>
                    </div>
                    <h3><strong>第二步：</strong></h3>
                    <div class="contentBox">
                        <p class="mb10">在 <a target="_blank" href="http://mp.weixin.qq.com">微信公众平台</a>开启自定义菜单，将开发者中心->开发者ID中的AppId和Appsecret复制到下方对应的输入框，并保存！</p>
                        <div class="form-inline">
                            <div class="form-group">
                                <label>
                                    <span>AppId：</span><asp:TextBox ID="txtAppId" CssClass="form-control resetSize" runat="server" /><br />
                                    <span></span><small class="inl">微信公众号身份的唯一标识</small>
                                </label>
                            </div>
                        </div>
                        <div class="form-inline">
                            <div class="form-group">
                                <label>
                                    <span>AppSecret：</span>
                                    <asp:TextBox ID="txtAppSecret" CssClass="form-control resetSize" runat="server" /><br />
                                    <span></span><small class="inl">审核后在公众平台开启开发模式后可查看</small>
                                </label>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="form-group">
                    <div class="col-xs-10 col-xs-offset-2 setmargin">
                        <asp:Button ID="btnSave" runat="server" Text="保存" CssClass="btn btn-success float inputw100" OnClick="btnSave_Click" />
                    </div>
                </div>

    </form>
<%--    <div class="page-header">
        <h2>微信公众号</h2>
    </div>
    <div class="set-switch">
        <strong>提示</strong>
        <p>开通微信营销需要绑定微信公众帐号，不知道怎么绑定请参考。<a href="../help/zhiyin.html" class="ml20" target="_blank">开通指引</a></p>
    </div>
    <form runat="server">
        <div class="titileBorderBox">
            <h3><strong>基本通讯配置</strong></h3>
            <div class="contentBox">
                <p class="mb10">请将URL与Token配置到 <a target="_blank" href="http://mp.weixin.qq.com">微信公众平台</a> 下</p>
                <div class="form-inline">
                    <div class="form-group">
                        <label><span>Url：</span><asp:Literal runat="server" ID="txtUrl"></asp:Literal></label>
                    </div>
                    <asp:HiddenField ID="hdfCopyUrl" runat="server" />
                    <button type="button" class="btn resetSize btn-default" id="copyurl" data-clipboard-target="<%=hdfCopyUrl.ClientID %>" onclick="copyurl('copyurl');">复制</button>
                </div>
                <div class="form-inline">
                    <div class="form-group">
                        <label><span>Token：</span><asp:Literal runat="server" ID="txtToken"></asp:Literal></label>
                    </div>
                    <asp:HiddenField ID="hdfCopyToken" runat="server" />
                    <button type="button" class="btn resetSize btn-default" id="copytoken" data-clipboard-target="<%=hdfCopyToken.ClientID %>" onclick="copyurl('copytoken');">复制</button>
                </div>
            </div>
            <h3><strong>自定义菜单权限配置</strong></h3>
            <div class="contentBox">
                <p class="mb10">如果您开通了自定义菜单，请将 微信公众平台 下的AppId与AppSecret配置在下方。</p>
                <div class="form-inline">
                    <div class="form-group">
                        <label>
                            <span>AppId：</span><asp:TextBox ID="txtAppId" CssClass="form-control resetSize" runat="server" /><br />
                            <span></span><small class="inl">微信公众号身份的唯一标识</small>
                        </label>
                    </div>
                </div>
                <div class="form-inline">
                    <div class="form-group">
                        <label>
                            <span>AppSecret：</span>
                            <asp:TextBox ID="txtAppSecret" CssClass="form-control resetSize" runat="server" /><br />
                            <span></span><small class="inl">审核后在公众平台开启开发模式后可查看</small>
                        </label>
                    </div>
                </div>
            </div>
        </div>
        <div class="form-group">
            <div class="col-xs-10 col-xs-offset-2 setmargin">
                <asp:Button ID="btnSave" runat="server" Text="保存" CssClass="btn btn-success float inputw100" OnClick="btnSave_Click" />
            </div>
        </div>
    </form>--%>
</asp:Content>
