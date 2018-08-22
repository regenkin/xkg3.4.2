<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminNew.Master" AutoEventWireup="true" CodeBehind="AliFuwuConfig.aspx.cs" Inherits="Hidistro.UI.Web.Admin.AliFuwu.AliFuwuConfig" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../js/ZeroClipboard.min.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            var copy = new ZeroClipboard(document.getElementById("copyurl"), {
                moviePath: "../js/ZeroClipboard.swf"
            });
            copy.on('complete', function (client, args) {
                HiTipsShow("复制内容为：" + args.text, 'success');
            });

            var copy2 = new ZeroClipboard(document.getElementById("copytoken"), {
                moviePath: "../js/ZeroClipboard.swf"
            });
            copy2.on('complete', function (client, args) {
                HiTipsShow("复制内容为：" + args.text, 'success');
            });

            var copy3 = new ZeroClipboard(document.getElementById("copyRSAPublic"), {
                moviePath: "../js/ZeroClipboard.swf"
            });
            copy3.on('complete', function (client, args) {
                HiTipsShow("复制内容为：" + args.text, 'success');
            });
        })

        function copyurl(obj) {
            var copy = new ZeroClipboard(document.getElementById(obj), {
                moviePath: "../js/ZeroClipboard.swf"
            });
        }

        function RsaCreat() {
            var submit = "<%=RSACreat.ClientID%>"; //提交btn
            HiTipsShow("<strong>确定生成新的RSA密钥对？</strong><p style='color:red;'>生成后，请在支付宝服务窗开发者中心更新应用公钥，否则当前系统服务窗将不能正常使用！</p>", "confirm", submit);
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div class="page-header">
                    <h2>绑定支付宝服务窗</h2>
</div>
     <div class="set-switch">
                    <div class="iconimg"><i class="glyphicon glyphicon-info-sign"></i></div>
                    <div class="info">
                        <strong>重要提示</strong>
                        <p class="color" style="color:red; line-height:25px;">服务窗口绑定后，如果再次更换服务窗APPID，原用户OPENID将丢失，只有用户再次登入才能重新获取，此期间你无法向用户发送信息！</p>
                        <a href="http://doc.open.alipay.com/doc2/detail?treeId=53&articleId=103415&docType=1" target="_blank">服务窗开通指南</a>
                    </div>
                </div>

    <form runat="server">
                <div class="titileBorderBox">
                    <h3><strong>第一步：</strong></h3>
                <div class="contentBox">
                        <p class="mb10">在 <a target="_blank" href="https://fuwu.alipay.com">服务窗平台</a>开发者->开发者页面的AppId复制到下方对应的输入框，并保存！</p>
                        <div class="form-inline">
                            <div class="form-group">
                                <label>
                                    <span>AppId：</span><asp:TextBox ID="txtAliAppId" CssClass="form-control resetSize"  Width="300px" runat="server" /><br />
                                    <span></span><small class="inl">服务窗身份的唯一标识</small>
                                </label>
                            </div>
                        </div>
                        <div class="form-inline">
                            <div class="form-group">
                                <label>
                                    <span>关注消息：</span><asp:TextBox ID="txtAliFollowTitle" CssClass="form-control resetSize" runat="server" TextMode="MultiLine" Height="60px" Width="300px" /><br />
                                    <span></span><small class="inl">用户关注服务窗后的第一条信息</small>
                                </label>
                            </div>
                        </div>
                  
                     <div class="form-inline">
                      <div class="form-group">
                        <label style="padding-left:80px"><asp:Button ID="btnSave" runat="server" Text="保存" CssClass="btn btn-success float inputw100" OnClick="btnSave_Click"  />

                        </label>
                   </div>
                         </div>
                    </div>
                  
                    <h3><strong>第二步：</strong></h3>
                        <div class="contentBox">
                        <p class="mb10">复制以下信息至 <a target="_blank" href="https://fuwu.alipay.com">服务窗平台</a> ->开发者->修改网关及密钥！</p>
                        <div class="form-inline">
                            <div class="form-group">
                                <label style="color:#07D"><span style="color:#333">回调地址：</span><asp:Literal runat="server" ID="txtUrl"></asp:Literal></label>
                            </div>
                            <asp:HiddenField ID="hdfCopyUrl" runat="server" />
                            <button type="button" class="btn resetSize btn-default" id="copyurl" data-clipboard-target="<%=hdfCopyUrl.ClientID %>" onclick="copyurl('copyurl');">复制</button>
                        </div>
                        <div class="form-inline">
                            <div class="form-group">
                                <label  style="color:#07D"><span style="color:#333">应用网关：</span><asp:Literal runat="server" ID="txtToken"></asp:Literal></label>
                            </div>
                            <asp:HiddenField ID="hdfCopyToken" runat="server"  />
                            <button type="button" class="btn resetSize btn-default" id="copytoken" data-clipboard-target="<%=hdfCopyToken.ClientID %>" onclick="copyurl('copytoken');">复制</button>
                        </div>
                        <div class="form-inline">
                            <div class="form-group">
                                <label><span>RSA公钥　</span><button type="button" class="btn resetSize btn-danger" onclick="RsaCreat()" >生成新公钥</button>
                                    <asp:Button ID="RSACreat" runat="server" Text="生成新公钥" OnClick="RsaCreat_Click" CssClass="btn btn-success float inputw100" style="display:none"  /> 
                           </label>
                           </div> 
                            <button type="button" class="btn resetSize btn-default" id="copyRSAPublic" data-clipboard-target="<%=RSAPublic.ClientID %>" onclick="copyurl('copyRSAPublic');">复制</button>
                            <div style="padding:10px;color:#07D;width:460px;margin-top:5px;border:1px solid #ddd">
                                    <asp:Literal runat="server" ID="txtRSA"></asp:Literal></div>

                             </div>
                            <asp:HiddenField ID="RSAPublic" runat="server" Value=""  />
                            
                       
                    </div>
                    
                </div>

                

    </form>
</asp:Content>
