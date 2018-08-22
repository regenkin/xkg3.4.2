<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminNew.Master" AutoEventWireup="true" CodeBehind="ReplyEdit.aspx.cs" Inherits="Hidistro.UI.Web.Admin.AliFuwu.ReplyEdit" %>
<%@ Register Src="~/hieditor/ueditor/controls/ucUeditor.ascx" TagName="KindeditorControl" TagPrefix="Kindeditor" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .exit-shop-info {
            min-height: 300px;
            /*max-height: 390px;*/
            padding: 10px;
            line-height: 18px;
            color: #666;
            border: 1px solid #E7E7EB;
            font-size: 14px;
            overflow-y:visible;
            max-height:2000px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form id="aspnetForm" runat="server">
        <div class="page-header">
            <h2><%=htmlTitle %></h2>
            <small>用户关注商户支付宝服务窗后，系统自动发送给用户的消息。若回复内容为空，则用户关注该商户支付宝服务窗后，不会收到任何回复消息。</small>
        </div>
        <div class="shop-navigation clearfix">
            <div class="fl">
                <div class="mobile-border">
                    <div class="mobile-d">
                        <div class="mobile-header">
                            <i></i>
                            <div class="mobile-title">店铺主页</div>
                        </div>
                        <div class="mobile mate-list"><asp:Literal ID="litInfo" runat="server"></asp:Literal>
                            
                        </div>
                        <div class="mobile-footer"></div>
                    </div>
                </div>
            </div>
            <div class="fl frwidth">
                <div class="set-switch resetBorder" style="display:<%=(type=="subscribe")?"none":"block"%>">
                    <p class="mb10"><strong>关键词：</strong></p>
                    <asp:TextBox ID="txtKeys" runat="server" MaxLength="200" CssClass="form-control mb5"></asp:TextBox>
                    <small>多个关键词可以使用逗号“,”分隔，无关键词回复请将关键词设为"*"。</small>
                    <p class="mb10 mt10"><strong>匹配类型：</strong></p>
                    <div class="resetradio mb10">
                        <asp:RadioButtonList ID="rbtlMatchType" runat="server" RepeatDirection="Horizontal" Width="200">
                            <asp:ListItem Text="精确匹配" Value="2"></asp:ListItem>
                            <asp:ListItem Text="模糊匹配" Value="1"></asp:ListItem>
                        </asp:RadioButtonList>
                    </div>
                    <small>精确匹配是对话完全一致才能触发，模糊匹配是包含就触发。</small>
                </div>
                <div class="set-switch resetBorder">
                    <p class="mb10"><strong>类型：</strong></p>
                    <div class="typeList">
                        <ul class="clearfix">
                            <li messagetype="1">
                                <asp:HiddenField ID="hdfMessageType" runat="server" Value="1" />
                                <asp:HiddenField ID="hdfArticleID" runat="server" Value="0" />
                                <asp:HiddenField ID="hdfIsOldArticle" runat="server" Value="0" />
                                <p class="mb5"><i class="glyphicon glyphicon-list-alt"></i></p>
                                <p>文本内容</p>
                            </li>
                            <li messagetype="2">
                                <p class="mb5"><i class="glyphicon glyphicon-list-alt"></i></p>
                                <p>单条图文</p>
                            </li>
                            <li messagetype="4">
                                <p class="mb5"><i class="glyphicon glyphicon-list-alt"></i></p>
                                <p>多条图文</p>
                            </li>
                        </ul>
                    </div>
                </div>




                <div id="divContent">
                    <p class="mb10"><strong>文本内容：</strong><small style="display:inline">(支持文字链接，不支持图片或其他html标签)</small></p>
                    <div>
                        <Kindeditor:KindeditorControl ID="fkContent" runat="server" Width="638" Height="200" ShowType="2" />
                    </div>
                </div>
                <p class="mt10 mb10">
                    <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" OnClientClick="return  Verification();" Text="保存" CssClass="btn btn-success inputw100" />
                </p>

            </div>
        </div>


        <%--        <div class="footer-btn navbar-fixed-bottom">
            
        </div>--%> <%--class="active"--%>
    </form>



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
    <script type="text/javascript">
        function Verification(){
   
            if ($("#<%= txtKeys.ClientID%>").val().length > 50){
                ShowMsg("关键词必须少于50个字！", false);
                return false;
            }
            um.setContent(um.getPlainTxt());/*清除span,等等格式*/
            if ($("#<%= fkContent.ClientID%>").val().length > 1000){
                ShowMsg("回复内容必须1000字以内！", false);
                return false;
            }
            return true;
        }
        $(document).ready(function () {
            var messagetypevalue = $("#<%=hdfMessageType.ClientID%>").val();
            var selName = ".typeList li[messagetype='" + messagetypevalue + "']";
            $(selName).attr("class", "active");
            $(".typeList li").click(function () {

                //$(".mobile.mate-list").html("<div class=\"exit-shop-info\"></div>");

                var typeval = $(this).attr("messagetype");


                switch (typeval) {
                    case "2":
                    case "4":
                        $("#divContent").hide();

                        var Rand = Math.random();
                        $("#myPictureIframeModal").attr("src", "../weixin/ArticeSelect.aspx?type=" + typeval + "&Rand=" + Rand);
                        $('#MyPictureIframe').modal('toggle').children().css({
                            width: '800px',
                            height: '700px'
                        })
                        $("#MyPictureIframe").modal({ show: true });
                        break;
                    default:
                        
                        $(".typeList li").removeClass("active");
                        $("li[messagetype='1']").addClass("active");
                        $("#<%=hdfMessageType.ClientID%>").val(1);                        
                        $("#<%=hdfArticleID.ClientID%>").val(0);
                        $(".mobile.mate-list").html("<div class=\"exit-shop-info\">" + um.getContent() + "</div>");
                        $("#divContent").show();
                        break;
                }
            });
            var msgtype = $("#<%=hdfMessageType.ClientID%>").val();
            switch (msgtype) {
                case "2":
                case "4":
                    closeModal("MyPictureIframe",<%=hdfArticleID.Value%>);
                    $("#divContent").hide();
                    break;
                default:
                    break;
            }
            /*编辑器事件*/
            um.addListener('ready', function (editor) {
                $(".exit-shop-info").html(um.getContent());
            });
            um.addListener('selectionchange', function () {
                $(".exit-shop-info").html(um.getContent());
            });
        })

        function closeModal(modalid,articleId) {
            $("#<%=hdfArticleID.ClientID%>").val(articleId);
            $.ajax({
                url: "replyedit.aspx?type=getarticleinfo",
                type: "post",
                data: "articleid=" + articleId,
                datatype: "json",
                success: function (json) {
                    if (json.type == "1") {
                            var tips = "";
                            switch (json.articletype) {
                                case 2:
                                    tips = '<div class="mate-inner">' +
                                        '<h3 id="singelTitle">' + json.title + '</h3>' +
                                        '<span>' + json.date + '</span>' +
                                        '<div class="mate-img">' +
                                        '<img src="' + json.imgurl + '" class="img-responsive">' +
                                        '</div>' +
                                        '<div class="mate-info">' + json.memo + '</div>' +
                                        '<div class="red-all clearfix">' +
                                        '<strong class="fl">查看全文</strong>' +
                                        '<em class="fr">&gt;</em>' +
                                        '</div>' +
                                        '</div>';
                                    break;
                                case 4:
                                    var temp = "";
                                    var arr = json.items;
                                    for (var i = 0; i < arr.length; i++) {
                                        temp += '             <div class="mate-inner">' +
                                                   '                 <div class="child-mate">' +
                                                   '                     <div class="child-mate-title clearfix">' +
                                                   '                         <div class="title">' + arr[i].title + '</div>' +
                                                   '                         <div class="img">' +
                                                   '                             <img id="img2" src="' + arr[i].imgurl + '" class="img-responsive">' +
                                                   '                         </div>' +
                                                   '                     </div>' +
                                                   '                 </div>' +
                                                   '             </div>';
                                    }
                                    tips = '<div class="mate-inner top">' +
                                   '                 <div class="mate-img">' +
                                   '                     <img id="img1" src="' + json.imgurl + '" class="img-responsive">' +
                                   '                     <div class="title">' + json.title + '</div>' +
                                   '                 </div>' +
                                   '             </div>' + temp;
                                    break;
                                default:
                                    tips = '<div class="exit-shop-info">' + json.memo + '</div>';
                                    break;
                            }

                            $(".typeList li").removeClass("active");
                            $("li[messagetype='"+json.articletype+"']").addClass("active");
                            $("#<%=hdfMessageType.ClientID%>").val(json.articletype);


                            $(".mobile.mate-list").html(tips);
                            $('#' + modalid).modal('hide');
                      
                    } else {
                        //HiTipsShow(json.tips, "error");
                    }
                }
            })
            
        }

    </script>
</asp:Content>