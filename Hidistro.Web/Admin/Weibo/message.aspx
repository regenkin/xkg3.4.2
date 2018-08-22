<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminNew.Master" AutoEventWireup="true" CodeBehind="message.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Shop.Message" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>

<%@ Import Namespace="Hidistro.Core" %>
<%@ Import Namespace="System.Web.UI.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form runat="server">
        <input id="hidstatus" type="hidden" runat="server" value="-1" />
        <link rel="stylesheet" href="/admin/css/weibo.css">
        <script src="../js/weiboHelper.js"></script>
        <script>
            $(function () {

                $('#Picture').click(function () {
                    var Rand = Math.random();
                    $("#myPictureIframeModal").attr("src", "../shop/SelectArtice.aspx?Rand=" + Rand);
                    $('#MyPictureIframe').modal('toggle').children().css({
                        width: '800px',
                        height: '700px'
                    })
                    $("#MyPictureIframe").modal({ show: true });
                });
            })
            function ReceiverMessage(messageid, senderid) {
                cleardata();
                InitTextCounter(300, "#txtContent", "#iLeftWords");
                $("#myMessageModal").modal({ show: true });
                MessageId = messageid;
                SenderId = senderid;
            }
            var msgtype = "text";
            var displayname = "";
            var summary = "";
            var image = "";
            var url = "";
            var ArticleId = "";
            var MessageId = "";
            var SenderId = "";
            function closeModal(modalid, txtContentid, value, title, ShortDescription, ThumbnailUrl40, articleId) {
                ArticleId = articleId;
                if (value.indexOf("http://") < 0)
                    value = "http://" + value;
                msgtype = "articles";
                $("#txtContent").css("display", "none");
                $("#txtContent").val('');
                $("#showdiv").css("display", "block");
                $("#showurl").attr("href", value);
                $("#showgotourl").attr("href", value);
                $("#showmessage").text(title);
                url = value;
                displayname = title;
                summary = ShortDescription;
                image = ThumbnailUrl40;
                $('#' + modalid).modal('hide');
                InitTextCounter(300, "#txtContent", "#iLeftWords");

            }
            function cleardata() {
                msgtype = "text";
                displayname = '';
                summary = '';
                image = '';
                url = '';
                ArticleId = "";
                $("#txtContent").css("display", "block");
                $("#txtContent").val('');
                $("#showdiv").css("display", "none");
            }
            function savedata() {

                if (msgtype == "text") {
                    if ($.trim($("#txtContent").val()) == "") {
                        HiTipsShow("说点什么吧！", 'warning');
                        return;
                    }
                }
                else {
                    if (displayname == "") {
                        HiTipsShow("说点什么吧！", 'warning');
                        return;
                    }
                }
                var params = "&MessageId=" + MessageId + "&SenderId=" + SenderId + "&ArticleId=" + ArticleId + "&msgtype=" + msgtype + "&displayname=" + displayname + "&url=" + url + "&summary=" + summary + "&image=" + image + "&Content=" + $("#txtContent").val();
           
                $("#send").button('loading');
                $.getJSON("../../API/WeiBoAPI.ashx?action=sendmessage" + params).done(function (d) {
                    $("#send").button('reset');
                    if (d.result) {
                        $("#myMessageModal").modal('hide');
                        HiTipsShow("发送成功！", 'success');
                        cleardata();
                        location.reload();
                        return
                    }
                    else {
                        HiTipsShow("调用接口失败,原因48小时内未回复或者您发送给对方信息超过99条！", 'fail');
                        return
                    }

                });

            }
            function DetailData(MessageId) {
                var params = "&MessageId=" + MessageId;
                $.getJSON("../../API/WeiboProcess.ashx?action=getmessageinfo" + params).done(function (d) {
                    if (d.status == "1") {

                        $("#myDetailMessageModal").modal({ show: true });
                        if (d.DisplayName == "") {
                            $("#DetailMessage").html(weiboHelper.FilterEmotionHtml(d.SenderMessage));
                            $("#Detailshowdiv").css('display', "none");
                            $("#DetailMessage").css('display', '');
                        }
                        else {
                            $("#DetailMessage").css('display', 'none');
                            $("#Detailshowdiv").css('display', "");
                            $("#Detailshowurl").attr('href', d.Url);
                            $("#Detailshowmessage").text(d.DisplayName);
                            $("#Detailshowgotourl").attr('href', d.Url);
                        }
                    }
                    else {
                        HiTipsShow("查询失败！", 'fail');
                        return
                    }

                });

            }
        </script>

        <div class="page-header">
            <h2>微博消息</h2>
            <small>注意事项：消息接收方给消息发送方主动发送过消息，则在48小时内，消息发送方具备针对于消息接收方的主动配额99条；
            <a href="https://secure-appldnld.apple.com/QuickTime/091-8571.20140218.8MjJw/QuickTimeInstaller.exe">语音下载地址</a>
            </small>
        </div>
        <div class="app-init-container">
            <div class="sinaweibo-messages">
                <nav class="ui-nav">
                    <ul>
                        <li <%=alldata %>><a class="js-nav-all" href="message.aspx?Status=-1">我的所有私信</a></li>
                        <li <%=nodata %>><a href="message.aspx?Status=0">未回复</a></li>
                    </ul>
                </nav>
                <div class="app__content">
                    <table class="table js-tb-header" id="js-messages-container" style="padding: 0px; vertical-align: middle; display: table;">
                        <colgroup>
                            <col class="" style="width: 70px;">
                            <col class="" style="width: 110px;">
                            <col class="" style="width: 440px;">
                            <col class="" style="width: 80px;">
                            <col class="">
                        </colgroup>
                        <thead class="tableFloatingHeaderOriginal">
                            <tr>
                                <th class="pic">
                                    <div class="td-cont">
                                        <span>粉丝</span>
                                    </div>
                                </th>
                                <th class="username">
                                    <div class="td-cont">
                                    </div>
                                </th>
                                <th class="message">
                                    <div class="td-cont">
                                        <span>信息（</span><span class="autoreply-ck"><label class="checkbox inline"><asp:CheckBox ID="replycheck" OnCheckedChanged="CheckBox1_CheckedChanged" AutoPostBack="true" CssClass="js-chk-autoreply" runat="server" />
                                            显示自动回复的</label>）</span>
                                    </div>
                                </th>
                                <th class="time">
                                    <div class="td-cont">时间</div>
                                </th>
                                <th class="time">回复状态</th>
                                <th class="opts">
                                    <div class="td-cont">
                                        <span>操作</span>
                                    </div>
                                </th>
                            </tr>
                        </thead>
                        <asp:Repeater runat="server" ID="RepMessage">
                            <ItemTemplate>
                                <tr>
                                    <td class="pic" style="vertical-align: middle;">
                                        <div class="td-cont">
                                            <span>
                                                <asp:Image runat="server" ID="Pic" />
                                            </span>
                                        </div>
                                    </td>
                                    <td class="username" style="vertical-align: middle;">
                                        <div class="td-cont">
                                            <asp:Literal runat="server" ID="LitUserName"></asp:Literal>
                                        </div>
                                    </td>

                                    <td class="message" style="vertical-align: middle;">
                                        <a></a>
                                        <div class="td-cont">
                                            <span>

                                                <%#Eval("Type").ToString()=="image"?"<a target=\"_blank\" href="+"https://upload.api.weibo.com/2/mss/msget?access_token="+SettingsManager.GetMasterSettings(false).Access_Token+"&fid="+Eval("Vfid")+"><img width='50' height='50'  title='点击下载图片' alt='点击下载图片' src="+"https://upload.api.weibo.com/2/mss/msget?access_token="+SettingsManager.GetMasterSettings(false).Access_Token+"&fid="+Eval("Vfid")+"></img></a>":Eval("Type").ToString()=="voice"?"<embed src="+"https://upload.api.weibo.com/2/mss/msget?access_token="+SettingsManager.GetMasterSettings(false).Access_Token+"&fid="+Eval("Vfid")+" autostart=\"false\" width=\"200\" height=\"20\" controller=\"true\" align=\"middle\" bgcolor=\"black\" target=\"myself\" type=\"video/quicktime\" pluginspage='http://www.apple.com/quicktime/download/index.html' />":Eval("Text") %>


                                            </span>
                                        </div>
                                    </td>
                                    <td class="time" style="vertical-align: middle;">
                                        <div class="td-cont"><%#Eval("Created_at") %></div>
                                    </td>
                                    <td class="time" style="vertical-align: middle;">
                                        <%#Eval("Status").ToString().Trim().Equals("0")?"未回复":"<span style=\"color:blue\">已回复</span>" %>
                                    </td>
                                    <td class="opts" style="vertical-align: middle;">
                                        <div class="td-cont">
                                            <%#Eval("Status").ToString().Trim().Equals("0")?"<span style=\"cursor:pointer\" onclick=\"ReceiverMessage('"+Eval("MessageId")+"','"+Eval("Sender_id")+"')\">回复</span>":"<span style='cursor:pointer' onclick=\"DetailData('"+Eval("MessageId")+"')\">查看回复</span>" %>
                                        </div>
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </table>
                    <div class="page">
                        <div class="bottomPageNumber clearfix">
                            <div class="pageNumber">
                                <div class="pagination">
                                    <UI:Pager runat="server" ShowTotalPages="true" ID="pager" DefaultPageSize="5" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>
    <!-- 模态框（Modal） -->
    <div class="modal fade" id="myMessageModal" tabindex="-1" role="dialog"
        aria-labelledby="myMessageModal" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close"
                        data-dismiss="modal" aria-hidden="true">
                        &times;
                    </button>
                    <h4 class="modal-title" id="myMessageModal1">回复
                    </h4>
                </div>
                <div class="modal-body">
                    <div class="wb-sender">
                        <div class="wb-sender__inner">
                            <div class="wb-sender__input js-editor-wrap">
                                <div class="misc top clearfix">
                                    <div class="content-actions clearfix">

                                        <div class="editor-module insert-emotion">
                                            <a class="js-open-emotion" data-action-type="emotion" href="javascript:;">表情</a>
                                            <div class="emotion-wrapper">
                                                <ul class="emotion-container clearfix">
                                                </ul>
                                            </div>
                                        </div>
                                        <div class="editor-module insert-article">
                                            <a id="Picture" href="javascript:;">选择图文</a>
                                        </div>
                                    </div>
                                </div>
                                <div class="content-wrapper">
                                    <textarea id="txtContent" class="js-txta" cols="50" rows="4"></textarea>
                                    <div class="js-picture-container picture-container"></div>
                                    <div class="complex-backdrop">
                                        <div class="js-complex-content complex-content" id="picback"></div>
                                    </div>
                                    <div class="complex-backdrop" id="showdiv" style="display: none;">
                                        <div class="js-complex-content complex-content">

                                            <div class="ng ng-single">
                                                <a href="javascript:;" class="close--circle js-delete-complex" onclick="cleardata();">×</a>
                                                <div class="ng-item">
                                                    <a id="showurl" href="" target="_blank" class="new-window"><span class="label label-success" id="showmessage"></span></a>
                                                </div>
                                                <div class="ng-item view-more">
                                                    <a id="showgotourl" href="" target="_blank" class="clearfix new-window">
                                                        <span class="pull-left">阅读全文</span>
                                                        <span class="pull-right">&gt;</span>
                                                    </a>
                                                </div>
                                            </div>

                                        </div>
                                    </div>


                                </div>

                            </div>
                        </div>
                    </div>
                    <div class="misc clearfix">

                        <div class="word-counter pull-right">还能输入 <i id="iLeftWords">300</i> 个字</div>

                    </div>
                </div>


                <div class="modal-footer">

                    <button class="btn btn-primary" onclick="savedata();" id="send" data-loading-text='正在提交...'>回复</button>
                    <button type="button" class="btn btn-default"
                        data-dismiss="modal">
                        关闭
                    </button>

                </div>
            </div>
            <!-- /.modal-content -->
        </div>
        <!-- /.modal -->
    </div>
    <!-- /.modal -->
    <div class="modal fade" id="MyPictureIframe">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h4 class="modal-title">图文</h4>
                </div>
                <div class="modal-body">
                    <iframe src="" id="myPictureIframeModal" width="750" height="370"></iframe>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">关闭</button>

                </div>
            </div>
            <!-- /.modal-content -->
        </div>
        <!-- /.modal-dialog -->
    </div>

    <!-- 模态框（Modal） -->
    <div class="modal fade" id="myModal" tabindex="-1" role="dialog"
        aria-labelledby="myModalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close"
                        data-dismiss="modal" aria-hidden="true">
                        &times;
                    </button>
                    <h4 class="modal-title" id="myModalLabel">图片
                    </h4>
                </div>
                <div class="modal-body">
                    <iframe src="" id="MyIframe" width="750" height="370"></iframe>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default"
                        data-dismiss="modal">
                        关闭
                    </button>

                </div>
            </div>
            <!-- /.modal-content -->
        </div>
        <!-- /.modal -->
    </div>
    <!-- /.modal -->

    <div class="modal fade" id="myDetailMessageModal" tabindex="-1" role="dialog"
        aria-labelledby="myDetailMessageModal" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close"
                        data-dismiss="modal" aria-hidden="true">
                        &times;
                    </button>
                    <h4 class="modal-title" id="myDetailMessageModal1">查看回复
                    </h4>
                </div>
                <div class="modal-body">
                    <div class="wb-sender">
                        <div class="wb-sender__inner">
                            <div class="wb-sender__input js-editor-wrap">
                                <div class="content-wrapper">

                                    <div class="js-txta" style="height: 100px;" id="DetailMessage"></div>
                                    <div class="complex-backdrop" id="Detailshowdiv" style="display: none;">
                                        <div class="js-complex-content complex-content">
                                            <div class="ng ng-single">
                                                <div class="ng-item">
                                                    <a id="Detailshowurl" href="" target="_blank" class="new-window"><span class="label label-success" id="Detailshowmessage"></span></a>
                                                </div>
                                                <div class="ng-item view-more">
                                                    <a id="Detailshowgotourl" href="" target="_blank" class="clearfix new-window">
                                                        <span class="pull-left">阅读全文</span>
                                                        <span class="pull-right">&gt;</span>
                                                    </a>
                                                </div>
                                            </div>

                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default"
                        data-dismiss="modal">
                        关闭
                    </button>

                </div>
            </div>
            <!-- /.modal-content -->
        </div>
        <!-- /.modal -->
    </div>

</asp:Content>
