<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminNew.Master" AutoEventWireup="true" CodeBehind="letter.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Shop.Letter" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="/admin/css/weibo.css">
    <script src="../js/weiboHelper.js"></script>
    <script>
        $(function () {
            InitTextCounter(300, "#txtContent", "#iLeftWords");
            $('#Picture').click(function () {
                var Rand = Math.random();
                $("#myPictureIframeModal").attr("src", "../shop/SelectArtice.aspx?Rand=" + Rand);
                $('#MyPictureIframe').modal('toggle').children().css({
                    width: '800px',
                    height: '700px'
                })
                $("#MyPictureIframe").modal({ show: true });
            });
            <%=htmlJs%>
        })
        var msgtype = "text";
        var displayname="";
        var summary="";
        var image="";
        var url = "";
        var ArticleId = "";
        function savedata() {
            if (msgtype == "text") {
                if ($.trim($("#txtContent").val()) == "") {
                    HiTipsShow("说点什么吧！", 'warning');
                    return;
                }
            }
            else
            {
                if(displayname=="")
                {
                    HiTipsShow("说点什么吧！", 'warning');
                    return;
                }
            }
            var params = "&ArticleId=" + ArticleId + "&msgtype=" + msgtype + "&displayname=" + displayname + "&url=" + url + "&summary=" + summary + "&image=" + image + "&Content=" + $("#txtContent").val();
            $("#send").button('loading');
            $.getJSON("../../API/WeiBoAPI.ashx?action=sendtouidmessage" + params).done(function (d) {
                $("#send").button('reset');
                if (d.result)
                {

                    HiTipsShow("发送成功！", 'success');
                    cleardata();
                    return
                }
                else
                {
                    HiTipsShow("调用接口失败！", 'fail');
                    return
                }
              
            });

        }
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
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
       <div class="page-header">
                    <h2>私信群发</h2>
                    <small></small>
    </div>
        <div class="app-init-container">
            <div class="sinaweibo-letter-wrap">
                <p class="letter-top-info">发送给：<span class="info-label">所有人</span></p>
                <hr>
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

                            <div class="misc clearfix">
                                <div class="content-actions clearfix">
                                    <div class="word-counter pull-right">还能输入 <i id="iLeftWords">300</i> 个字</div>
                                    <button class="btn btn-primary" onclick="savedata();" id="send" data-loading-text='正在提交...' >发送</button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="notify-bar js-notify animated hinge hide">
        </div>



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
 
</asp:Content>
