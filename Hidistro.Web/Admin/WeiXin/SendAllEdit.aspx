<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminNew.Master" AutoEventWireup="true" CodeBehind="SendAllEdit.aspx.cs" Inherits="Hidistro.UI.Web.Admin.WeiXin.SendAllEdit" %>
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
            <h2>微信群发</h2>
            <small>设置会员在微信上收到的消息及内容。群发接口发送，会员每月只能接收4条群发消息；客服接口发送，使用该接口没有接收次数限制，且点击图文消息能直接进行链接跳转，但是会员如果没有主动发消息给公众号（包括发送信息、点击自定义菜单、扫描二维码事件、支付成功事件、用户维权等），将不会收到相关消息。</small>
        </div>
        <div class="shop-navigation clearfix">
            <div class="fl">
                <div class="mobile-border">
                    <div class="mobile-d">
                        <div class="mobile-header">
                            <i></i>
                            <div class="mobile-title">店铺主页</div>
                        </div>
                        <div class="mobile mate-list">
                            <asp:Literal ID="litInfo" runat="server"></asp:Literal>

                        </div>
                        <div class="mobile-footer"></div>
                    </div>
                </div>
            </div>
            <div class="fl frwidth">
                <div class="set-switch resetBorder">
                    <p class="mb10"><strong>标题：</strong></p>
                    <asp:TextBox ID="txtTitle" runat="server" MaxLength="200" CssClass="form-control mb5"></asp:TextBox>

                </div>
                <div class="set-switch resetBorder">
                    <p class="mb10"><strong>类型：</strong></p>
                    <div class="typeList">
                        <ul class="clearfix">
                            <li messagetype="1">
                                <asp:HiddenField ID="hdfSendID" runat="server" Value="0" />
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
                    <input type="button" id="btnWeiXinSend" value="群发接口发送" class="btn btn-success"/>
                    <input type="button" id="btnKefuSend" value="客服接口发送" class="btn btn-warning"/>
                </p>
            </div>
        </div>
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
        function removeHTMLTag() {
            //um.setContent(um.getPlainTxt());/*清除span,等等格式*/
            var str=um.getContent();
            str = str.replace(/<\/?[^>^a^p]*>/g,''); //去除HTML tag
            str=str.replace(/<img[^>]*>/g,'');
            return str;
        }
        $(document).ready(function () {
            var messagetypevalue = $("#<%=hdfMessageType.ClientID%>").val();
            var selName = ".typeList li[messagetype='" + messagetypevalue + "']";
            $(selName).attr("class", "active");
            $(".typeList li").click(function () {
                var typeval = $(this).attr("messagetype");
                switch (typeval) {
                    case "2":
                    case "4":
                        $("#divContent").hide();

                        var Rand = Math.random();
                        $("#myPictureIframeModal").attr("src", "ArticeSelect.aspx?type=" + typeval + "&Rand=" + Rand);
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
                        $(".mobile.mate-list").html("<div class=\"exit-shop-info\">" +removeHTMLTag() + "</div>");
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
                $(".exit-shop-info").html(removeHTMLTag());
            });
            um.addListener('selectionchange', function () {
                $(".exit-shop-info").html(removeHTMLTag());
            });
            $("#btnWeiXinSend,#btnKefuSend").click(function(){
                um.setContent(um.getPlainTxt());/*清除span,等等格式*/
                var btnVal=$(this).val();
                var sendtype=0;
                var apiName="客服接口";
                var msgTips="<p>使用该接口没有接收次数限制，且点击图文消息能直接进行链接跳转，但是只有48小时内与公众号有过交互的会员才能收到，是否立即发送？</p>";
                if(btnVal=="群发接口发送"){
                    sendtype=1;
                    apiName="群发接口";
                    msgTips="<strong>确定要使用微信群发接口发送吗？</strong><p>会员每月只能接收4条群发消息，是否立即发送？</p>";
                }
                var obj=this;
                if(!isconfirmOK){
                    return HiConform(msgTips,obj);
                }

                $(obj).attr("disabled","disabled").attr("value","发送中...");
                var sendid=$("#ctl00_ContentPlaceHolder1_hdfSendID").val();
                var msgtype=$("#ctl00_ContentPlaceHolder1_hdfMessageType").val();
                var articleid=$("#ctl00_ContentPlaceHolder1_hdfArticleID").val();
                var isoldarticle=$("#ctl00_ContentPlaceHolder1_hdfIsOldArticle").val();
                var title=encodeURIComponent($("#ctl00_ContentPlaceHolder1_txtTitle").val());
                um.setContent(um.getPlainTxt());/*清除span,等等格式*/
                var content=removeHTMLTag();

                var data="sendtype="+sendtype+"&sendid=" + sendid+"&msgtype="+msgtype+"&articleid="+articleid+"&title="+title+"&isoldarticle="+isoldarticle+"&content="+content;
                //alert(data);
                //return false;
                $.ajax({
                    url: "sendalledit.aspx?type=postdata",
                    type: "post",
                    data: data,
                    datatype: "json",
                    success: function (json) {
                        if(json.type=="1"){
                            if(sendid>0){
                               ShowMsgAndReUrl("微信群发记录修改成功！", true, "sendalllist.aspx");
                            }else{
                               ShowMsgAndReUrl("微信群发记录添加成功！", true, "sendalllist.aspx");
                            }
                        }else if(json.type=="2"){
                            ShowMsg("微信群发记录已保存！发送失败，原因是："+json.tips, false);
                            $(obj).attr("value",apiName+"发送失败");
                        }else{
                            $(obj).removeAttr("disabled").attr("value",apiName+"发送");
                            ShowMsg(json.tips, false);
                        }
                    }
                });
            })
        })

        function closeModal(modalid,articleId) {
            $("#<%=hdfArticleID.ClientID%>").val(articleId);
            $.ajax({
                url: "sendalledit.aspx?type=getarticleinfo",
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
    <script type="text/javascript" src="../weixin/GetImagesMsgId.aspx"></script>
</asp:Content>
