<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminNew.Master" 
    AutoEventWireup="true" CodeBehind="WXConfigBindOK.aspx.cs" 
    Inherits="Hidistro.UI.Web.Admin.WeiXin.WXConfigBindOK" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../js/ZeroClipboard.min.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {

            $("#ChangeBindUrl").click(function () {
                $.ajax({
                    type: 'get',
                    dataType: 'json',
                    url: 'GetWeixinProcessor.ashx?action=getcanchangebind',
                    success: function (data) {
                        console.log(data);
                        if (data.status == '1')//不能换
                        {
                            location.href = "WXConfigChangeBind.aspx"

                        } else if (data.status == '2')//可以换绑,也没有用户绑定openId的
                        {
                            location.href = "WXConfig.aspx?reset=1";
                        } else if (data.status == '3')//可以换绑,但有用户绑定openId的
                        {
                                $('#myModal2').modal('toggle').children().css({
                                    width: '530px'
                                })
                        } else {
                            ShowMsg(data.msg, false);
                        }
                    },
                    error: function () {
                        ShowMsg("请求出错了,请与管理员联系！", false);
                    }
                });
            });
        });

        function ModifyMemo2()
        {
            location.href = "WXConfig.aspx?reset=1";
        }

        function ModifyMemo()
        { }
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
    <style>
        .divcontent div {color:#FF8000; line-height:25px; width:94%;}
        .divcontent p {color:red; line-height:25px; width:100%; text-align:center; font-size:16px; margin-top:15px;}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    
 <form runat="server">
                <div class="page-header">
                    <h2>绑定微信公众号</h2>
                </div>
                <div class="set-switch">
                    <div class="iconimg"><i class="glyphicon color glyphicon-ok-circle"></i></div>
                    <div class="info">
                        <strong class="color">已成功绑定微信公众号</strong>
                        <p class="mt5">店铺正式运营期间请勿随意<a href="javascript:void(0);" id="ChangeBindUrl">更换微信公众号</a>，以免造成不必要的麻烦，如果网站出现绑定公众号错误，您可以手动<asp:Button ID="btnClearToken" runat="server" Text="刷新令牌" CssClass="btnLink" OnClick="btnClearToken_Click" />。</p>
                    </div>
                </div>
                <div class="public-number">
                    <table class="table">
                        <thead>
                            <tr>
                                <th>配置项</th>
                                <th width="460">配置说明</th>
                                <th>状态</th>
                                <th>操作</th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <td>URL/Token</td>
                                <td>绑定到微信公众平台以后，可通过系统后台管理您的微信公众号</td>
                                <td>已绑定</td>
                                <td><input type="button" class="btn btn-success btn-sm y-btnclick1" value="<%=BindSetDesc %>" /></td>
                            </tr>
                            <tr>
                                <td>AppID/AppSecret</td>
                                <td>保存到系统后台以后，可在系统中使用更多微信公众号的功能</td>
                                <td>已设置</td>
                                <td>
                                    <input type="button" class="btn btn-success btn-sm y-btnclick2" value="重新设置" />
                                  </td>
                            </tr>
                        </tbody>
                    </table>
                </div>



    <div class="modal fade" role="dialog" aria-labelledby="mySmallModalLabel" id="myModal">
        <div class="modal-dialog modal-sm">
            <div class="modal-content">
                <div class="w-modalbox">
                    <h5>微信公众号绑定详情</h5>
                    <div class="titileBorderBox borderSolidB">
                        <div class="contentBox pl20">
                            <p class="mb10">复制以下信息至 <a target="_blank" href="http://mp.weixin.qq.com">微信公众平台</a> -&gt;开发者中心-&gt;服务器配置中，并提交！</p>
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
                    </div>
                    <div class="y-ikown pt10 pb10">
                        <input type="submit" name="ctl00$ContentPlaceHolder1$btnRemark" value="我知道了" onclick="return ModifyMemo();" id="ctl00_ContentPlaceHolder1_btnRemark" class="btn btn-success inputw100" data-dismiss="modal">
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="modal fade" role="dialog" aria-labelledby="mySmallModalLabel" id="myModal1">
        <div class="modal-dialog modal-sm">
            <div class="modal-content">
                <div class="w-modalbox">
                    <h5>重新设置</h5>
                    <div class="titileBorderBox borderSolidB">
                        <div class="contentBox pl20">
                            <p class="mb10">如果您微信公众号的AppSecret已经变更，请将新的AppSecret配置在下方</p>
                            <div class="form-inline pl50" style="margin-bottom:0;">
                                <div class="form-group">
                                    <label><span>AppId：</span>
                                        <asp:Label ID="lbAppId" runat="server" style="width:200px; text-align:left;"></asp:Label>
                                    </label>
                                </div>
                            </div>
                            <div class="form-inline pl50" style="margin-bottom:15px;">
                                <div class="form-group">
                                    <label style="color:#999999;"><span></span>微信公众号身份唯一标识</label>
                                </div>
                            </div>
                            <div class="form-inline pl50" style="margin-bottom:0;">
                                <div class="form-group">
                                    <label><span>AppSecret：</span> <asp:TextBox ID="txtAppSecret" CssClass="form-control resetSize" runat="server" style=" width:250px;" /></label>
                                </div>
                            </div>
                            <div class="form-inline pl50" style="margin-bottom:15px;">
                                <div class="form-group">
                                    <label style="color:#999999;"><span></span>审核后在公众平台开启开发模式后可查看</label>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="y-ikown pt10 pb10">
                        <%--<input type="submit" name="ctl00$ContentPlaceHolder1$btnRemark" value="我知道了" onclick="return ModifyMemo();" id="ctl00_ContentPlaceHolder1_btnRemark" class="btn btn-success inputw100" data-dismiss="modal">--%>
                        <asp:Button ID="btnSave" runat="server" Text="保存"  OnClick="btnSave_Click"   CssClass="btn btn-success inputw100" data-dismiss="modal" />
                    
                        <input type="submit" name="ctl00$ContentPlaceHolder1$btnRemark" value="取消" onclick="return ModifyMemo();" id="ctl00_ContentPlaceHolder1_btnRemark" class="btn btn-primary inputw100" data-dismiss="modal">
                    </div>
                </div>
            </div>
        </div>
    </div>

   <div class="modal fade" role="dialog" aria-labelledby="mySmallModalLabel" id="myModal2">
         <div class="modal-dialog modal-sm">
             <div class="modal-content">
                 <div class="w-modalbox">
                     <h5>更换微信公众号</h5>
                     <div class="titileBorderBox borderSolidB">
                         <div class="contentBox pl20 divcontent">
                             <div> 更换微信公众号以后，会员通过微信授权登录店铺时，需要再次通过绑定已有用户名才能找回以前的会员数据，请谨慎操作！
                             </div>
                             <p>确定更换吗？</p>
                         </div>
                     </div>
                     <div class="y-ikown pt10 pb10">

                         <input type="submit" value="确定更换" onclick="return ModifyMemo2();" class="btn btn-primary inputw100"
                             data-dismiss="modal">

                         <input type="submit"  value="再考虑一下"  class="btn btn-success inputw100" style="margin-left:15px;" data-dismiss="modal">
                     </div>
                 </div>
             </div>
         </div>
     </div>
 </form>

<script type="text/javascript">
    $(function () {
        $('.y-btnclick1').click(function () {
            $('#myModal').modal('toggle').children().css({
                width: '530px'
            })
        })
        $('.y-btnclick2').click(function () {
            $('#myModal1').modal('toggle').children().css({
                width: '530px'
            })
        })
    })
</script>
           
</asp:Content>
