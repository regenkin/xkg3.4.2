<%@ Page Title="好友的微博" Language="C#" MasterPageFile="~/Admin/AdminNew.Master" AutoEventWireup="true" CodeBehind="timeline.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Shop.Timeline" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="/admin/css/weibo.css">
    <script src="../js/weiboHelper.js"></script>
    <script>

        $(function () {

            InitTextCounter(300, "#txtContent", "#iLeftWords");
            //加载用户最新微博
            loaduserinfo();
            loadStatuses(1);
            $("#Homepage").click(function () {
                $("#txtContent").val($("#txtContent").val() + ($(this).attr("gotourl"))).keyup();
            });

            $('#GoodsAndType').click(function () {
                var Rand = Math.random();
                $("#MyIframe").attr("src", "Goods.aspx?Rand=" + Rand);
                $('#myIframeModal').modal('toggle').children().css({
                    width: '800px',
                    height: '700px'
                })
                $("#myIframeModal").modal({ show: true });
            });
        })
        var set = 1;
        function gotopage() {
            $("#loading").css("display", "block");
            set++;
            loadStatuses(set);

        }
        function loaduserinfo() {
            $.getJSON("../../API/WeiBoAPI.ashx?action=userinfo").done(function (d) {
                ErrorAndIsAuthorized(d);
                $("#friends_count").text(d.friends_count);
                $("#followers_count").text(d.followers_count);
            });
        }
        function loadStatuses(page) {
            $.getJSON("../../API/WeiBoAPI.ashx?action=friends_timeline&page=" + page).done(function (d) {

                if (parseInt(d.statuses.length) == 20) {
                    $("#pagediv").css("display", "");
                }
                if (d.statuses) {
                    for (var i in d.statuses) {
                        //$("#pic_urls").html('');
                        var status = d.statuses[i];
                        var tpl = $($("#statusTpl").html());

                        tpl.find('#lstimg').attr('src', status.user.profile_image_url);

                        tpl.find('#wbname').text(status.user.screen_name);
                        var userhomeurl = "http://weibo.com/" + status.user.id;
                        if ($.trim(userhomeurl) != "") {
                            tpl.find('#wbname').attr('href', userhomeurl);

                        }

                        if (typeof (status.retweeted_status) != "undefined") {
                            var zuserhomeurl = "http://weibo.com/" + status.retweeted_status.user.id;
                            if ($.trim(zuserhomeurl) != "") {
                                tpl.find('#zwbname').attr('href', userhomeurl);

                            }
                            if (status.retweeted_status.pic_urls != "") {
                                for (var o in status.retweeted_status.pic_urls) {
                                    tpl.find("#zpic_urls").append("<li class=\"bigcursor\"  \><img tid=\"" + status.id + "\" src=\"" + status.retweeted_status.pic_urls[o].thumbnail_pic.replace(/\/thumbnail\//g, "/square/") + "\"/></li>");
                                }
                            }

                            if (status.retweeted_status.pic_urls.length < 6) {
                                tpl.find("#zpic_urls").css("width", "auto");
                            }
                            tpl.find('#zwbmedia').css("display", "");
                            tpl.find('#zcomments_count').attr('title', status.id);
                            var dd = new Date(status.retweeted_status.created_at);
                            tpl.find('#zsource').html(status.retweeted_status.source.replace(/<a /, "<a target='_blank' "));
                            tpl.find('#zcreated_at').text(dd.toLocaleString());
                            // var txtlink = "<a class=\"wb-shortlink\" target=\"_blank\"  href=\"\">sssssssss</a>";
                            tpl.find('#zwbtext').html(weiboHelper.FilterEmotionHtml(status.retweeted_status.text));
                            tpl.find('#zreposts_count').text("转发(" + status.retweeted_status.reposts_count + ")");
                            tpl.find('#zcomments_count').text("评论(" + status.retweeted_status.comments_count + ")");
                            tpl.find('#zreposts_count').attr('title', status.retweeted_status.id);
                            tpl.find('#zcomments_count').attr('title', status.retweeted_status.id);
                        }
                        else {

                            tpl.find('#zwbmedia').css("display", "none");
                        }


                        tpl.find('#comments_count').attr('title', status.id);
                        var dd = new Date(status.created_at);
                        tpl.find('#source').html(status.source.replace(/<a /, "<a target='_blank' "));
                        tpl.find('#created_at').text(dd.toLocaleString());
                        tpl.find('#wbtext').html(weiboHelper.FilterEmotionHtml(status.text));
                        tpl.find('#reposts_count').text("转发(" + status.reposts_count + ")");
                        tpl.find('#comments_count').text("评论(" + status.comments_count + ")");
                        tpl.find('#reposts_count').attr('title', status.id);
                        tpl.find('#comments_count').attr('title', status.id);

                        tpl.find('.wb-detail').attr("ID", "detail" + status.id)
                        if (status.pic_urls != "") {
                            for (var o in status.pic_urls) {
                                tpl.find("#pic_urls").append("<li class=\"bigcursor\"  \><img tid=\"" + status.id + "\" src=\"" + status.pic_urls[o].thumbnail_pic.replace(/\/thumbnail\//g, "/square/") + "\"/></li>");
                            }
                        }
                        if (status.pic_urls.length < 6) {
                            tpl.find("#pic_urls").css("width", "auto");
                        }
                        $("#listdata tbody").append(tpl);
                    }

                    $(".wb-media .bigcursor img").click(function () {
                        var tid = $(this).attr("tid");
                        var obj = $("#detail" + tid);
                        var bigimgsrc = $(this).attr("src").replace(/\/square\//g, "/bmiddle/");
                        var imgarr = obj.find(".bigcursor img");
                        var imgnum = imgarr.length;
                        var bigimglist = '';
                        if (imgnum == 1) {
                            bigimglist = '<div node-type="imagesBox" class="WB_expand_media S_bg1"><div class="tab_feed_a clearfix">' +
                                '<div class="tab"><ul class="clearfix"><li><span class="line S_line1"><a action-type="feed_list_media_toSmall" href="javascript:;" class="S_txt1" onclick="upordownpiclist(' + tid + ')"><i class="W_ficon ficon_arrow_fold S_ficon">k</i>收起</a></span></li></ul></div>' +
                                 '</div><div class="WB_media_view"><div class="media_show_box"><ul style="background-image: url(&quot;about:blank&quot;);" class="clearfix" node-type="picShow"><li node-type="imgBox" class="clearfix leftcursor"><div class="artwork_box"><div node-type="imgSpanBox"><img  tid="' + tid + '" node-type="imgShow" src="' + bigimgsrc + '"></div></div><i node-type="loading" style="display: none;" class="W_loading"></i></li></ul>' +
                                '<div style="top:0px;left:0px;display:none;" node-type="recLayer" class="W_layer"></div></div>' +
                                '</div>';

                        } else {
                            var smallimglist = '';
                            for (var i = 0; i < imgnum; i++) {
                                smallimglist += '<li><a href="javascript:;" action-type="thumbItem"><img tid="' + tid + '" class="S_line2" src="' + $(imgarr[i]).attr("src").replace(/\/thumbnail\//g, "/square/") + '" alt=""></a></li>';//http://ww2.sinaimg.cn/square/80891114gw1eubdexr4g0j20c80gctcj.jpg
                            }
                            bigimglist = '<div node-type="imagesBox" class="WB_expand_media S_bg1"><div class="tab_feed_a clearfix">' +
                                '<div class="tab"><ul class="clearfix"><li><span class="line S_line1"><a action-type="feed_list_media_toSmall" href="javascript:;" class="S_txt1" onclick="upordownpiclist(' + tid + ')"><i class="W_ficon ficon_arrow_fold S_ficon">k</i>收起</a></span></li></ul></div>' +
                                 '</div><div class="WB_media_view"><div class="media_show_box"><ul style="background-image: url(&quot;about:blank&quot;);" class="clearfix" node-type="picShow"><li node-type="imgBox" class="clearfix leftcursor"><div class="artwork_box"><div node-type="imgSpanBox"><img tid="' + tid + '" node-type="imgShow" src="' + bigimgsrc + '"></div></div><i node-type="loading" style="display: none;" class="W_loading"></i></li></ul>' +
                                '<div style="top:0px;left:0px;display:none;" node-type="recLayer" class="W_layer"></div></div>' +
                                '<div class="pic_choose_box clearfix"><a href="javascript:;" node-type="prev" action-type="prev" class="arrow_left_small arrow_dis S_bg2" title="上一页" tid="' + tid + '"><i class="W_ficon ficon_arrow_left S_ficon">b</i></a><div class="stage_box"><ul style="margin-left: 0px;" node-type="picChoose" class="choose_box clearfix">' +

                               smallimglist +

                                '</ul></div><a href="javascript:;" node-type="next" action-type="next" class="arrow_right_small S_bg2" title="下一页" tid="' + tid + '"><i class="W_ficon ficon_arrow_right S_ficon">a</i></a></div> </div>' +
                                '</div>';
                            showpictrue(bigimgsrc, tid);
                        }
                        if ($(obj.find(".wb-media")).is(":visible")) {
                            if (bigimglist.length > 0) {
                                $(obj.find(".wb-image--expand")).html(bigimglist).show();
                                $(obj.find(".wb-media")).hide();
                            }
                        }
                        $("#detail" + tid).find('img[node-type="imgShow"]').mousemove(function (event) {
                            imageonmousemove(event, this, imgarr);
                        });
                        $("#detail" + tid).find('img[node-type="imgShow"]').click(function () {
                            var tid = $(this).attr("tid");
                            var liClassName = $("#detail" + tid).find('li[node-type="imgBox"]').attr("class");// $($(this).parent().parent().parent()).attr("class");
                            var imglistnum = imgarr.length;
                            if (liClassName.indexOf("rightcursor") != -1) {
                                for (var i = 0; i < imglistnum; i++) {
                                    if ($(imgarr[i]).attr("src").replace(/\/square\//g, "/bmiddle/") == $(this).attr("src")) {
                                        if (i < imglistnum - 1) {
                                            showpictrue($(imgarr[i + 1]).attr("src").replace(/\/square\//g, "/bmiddle/"), tid);
                                            break;
                                        }
                                    }
                                }
                            } else if (liClassName.indexOf("leftcursor") != -1) {
                                for (var i = 0; i < imglistnum; i++) {
                                    if ($(imgarr[i]).attr("src").replace(/\/square\//g, "/bmiddle/") == $(this).attr("src")) {
                                        if (i > 0) {
                                            showpictrue($(imgarr[i - 1]).attr("src").replace(/\/square\//g, "/bmiddle/"), tid);
                                            break;
                                        }
                                    }
                                }
                            } else {
                                upordownpiclist(tid);
                            }
                        });
                        $("#detail" + tid).find(".choose_box li img").click(function () {
                            var tid = $(this).attr("tid");
                            $($(this).parent().parent().parent()).find("a").removeClass("current");
                            $(this).parent().addClass("current");
                            showpictrue($(this).attr("src").replace(/\/square\//g, "/bmiddle/"), tid);
                        });
                        $("#detail" + tid).find("a[action-type='turnLeft']").click(function () {
                            var tid = $(this).attr("tid");
                            rotate($('#detail' + tid).find('img[node-type="imgShow"]'), 'left');
                        });
                        $("#detail" + tid).find("a[action-type='turnRight']").click(function () {
                            var tid = $(this).attr("tid");
                            rotate($('#detail' + tid).find('img[node-type="imgShow"]'), 'right');
                        });
                        $("#detail" + tid).find("a[action-type='next']").click(function () {
                            var tid = $(this).attr("tid");
                            var imglistobj = $('#detail' + tid).find('.choose_box img');
                            if (imglistobj.length > 7) {
                                $('#detail' + tid).find("ul[node-type='picChoose']").animate({ marginLeft: "-" + (imglistobj.length - 7) * 56 + "px" });
                            }
                        });
                        $("#detail" + tid).find("a[action-type='prev']").click(function () {
                            var tid = $(this).attr("tid");
                            $('#detail' + tid).find("ul[node-type='picChoose']").animate({ marginLeft: "0px" });

                        })
                    })

                    $("#loading").css("display", "none");
                }
                else {
                    HiTipsShow("获取数据失败！", 'fail');
                }

            });
        }
        var commentid;
        var subtype;
        function openshow(obj, type) {
            subtype = type;
            if (type == 0)
                $("#myModalLabel").text('转发');
            else
                $("#myModalLabel").text('评论');
            commentid = obj;

            $("#txtContent").val('');
            $("#myModal").modal({ show: true });


        }


        function createcomments() {

            var actiontype = 'action=commentscreate';

            if (subtype == 0)
                actiontype = 'action=repost';
            else {
                if ($.trim($("#txtContent").val()) == "") {
                    HiTipsShow("说点什么吧！", 'warning');
                    return;
                }
            }

            var url = actiontype + "&id=" + commentid + "&comment=" + encodeURI($("#txtContent").val());
            $.getJSON("../../API/WeiBoAPI.ashx?" + url).done(function (d) {
                if ($.trim(d.created_at) != "") {
                    if (subtype == 1)
                        HiTipsShow("评论成功！", 'success');

                    else
                        HiTipsShow("转发成功！", 'success');
                    InitTextCounter(300, "#txtContent", "#iLeftWords");
                    $("#txtContent").val('');
                    $('#myModal').modal('hide');
                    $("#listdata tbody").html('');
                    loadStatuses(1);
                }
                else {
                    if (subtype == 1)
                        HiTipsShow("评论失败！", 'fail');
                    else
                        HiTipsShow("转发失败！", 'fail');
                }
            });
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="page-header">
        <h2>好友的微博</h2>
        <small></small>
    </div>


    <script type="text/template" id="statusTpl">
        <tr>
            <td>
                <div class="td-cont">
                    <a href="javascript:;" class="avatar wb-face">
                        <img src="" id="lstimg" alt=""></a>
                </div>
            </td>
            <td colspan="2">
                <div class="td-cont">
                    <div class="wb-detail">
                        <div class="wb-info">
                            <a id="wbname" target="_blank" class="wb-name new-window" href="javascript:void(0);"></a>

                        </div>
                        <div class="wb-text large" id="wbtext">
                        </div>

                        <div class="wb-media">
                            <ul class="wb-media-list clearfix" id="pic_urls">
                            </ul>
                        </div>
                        <div class="wb-image--expand">
                        </div>



                        <%--  转发开始--%>
                        <div class="wb-media--expand" id="zwbmedia">
                            <div class="wb-forward-content">
                                <div class="wb-info">
                                    <a target="_blank" href="javascript:void(0);" id="zwbname" class="wb-name new-window"></a>

                                </div>
                                <div class="wb-text" id="zwbtext">
                                </div>
                                <div class="wb-media">
                                    <ul class="wb-media-list clearfix" id="zpic_urls">
                                    </ul>
                                </div>
                                <div class="wb-func clearfix">
                                    <div class="wb-handle pull-right">
                                        <a href="javascript:;" id="zreposts_count" class="js-wb-action js-wb-repost--inner" title="" onclick="openshow(this.title,'0')">转发</a> | <a href="javascript:;" class="js-wb-action js-wb-comment--inner" id="zcomments_count" title="" onclick="openshow(this.title,'1')">评论</a>
                                    </div>
                                    <div class="wb-from">
                                        <a target="_blank" href="javascript:void(0);" id="zcreated_at" class="wb-time"></a><em>来自</em> <span class="js-new-window"><a id="zsource" class="new-window">微博 weibo.com</a></span>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <%--  转发结束--%>


                        <div class="memo"></div>
                        <div class="wb-func clearfix">

                            <div class="wb-handle outer pull-right">
                                <a href="javascript:;" class="js-wb-action js-wb-repost" id="reposts_count" title="" onclick="openshow(this.title,'0')">转发</a> | <a class="js-wb-action js-wb-comment"
                                    id="comments_count" href="javascript:;" title="" onclick="openshow(this.title,'1')">评论</a>
                            </div>

                            <div class="wb-from">
                                <a target="_parent" href="javascript:void(0);" id="created_at" class="wb-time"></a><em>来自</em> <span class="js-new-window"><span id="source" class="new-window">微博 weibo.com</span></span>
                            </div>
                        </div>

                    </div>
                </div>
            </td>
        </tr>
    </script>

    <div class="app-init-container">
        <div class="app-init">



            <div class="app__content">
                <div class="sinaweibo-timeline">

                    <div id="js-timeline-container" class="timeline-container">
                        <table class="table" id="listdata">
                            <colgroup>
                                <col class="th-avatar">
                                <col class="th-content">
                                <col class="th-filter">
                            </colgroup>
                            <thead>
                                <tr class="weibo-feed">
                                    <th>
                                        <div class="td-cont">头像</div>
                                    </th>
                                    <th>
                                        <div class="td-cont">信息</div>
                                    </th>

                                    <th class="th-filter"></th>

                                </tr>
                            </thead>
                            <tbody id="js-status-container">
                            </tbody>
                        </table>
                        <div class="pagenavi" id="loading" style="width: 100%; text-align: center; padding-bottom: 20px;">

                            <image src="../images/loading.gif"></image>
                        </div>
                        <div class="pagenavi" id="pagediv" style="display: none; width: 100%; text-align: center; padding-bottom: 20px;">
                            <!-- 第一页，非最后一页 -->
                            <a class="next" style="cursor: pointer" onclick="gotopage();">加载更多</a>

                        </div>
                    </div>
                    <div class="timeline-sidebar">
                        <div class="js-wb-profile wb-profile">
                            <div class="wb-profile__inner">
                                <!-- <div class="wb-profile"> -->

                                <div class="dash-well">


                                    <div class="info-group">
                                        <div class="info-group__inner">
                                            <a href="#" id="friends_count"></a>
                                            <span>关注数</span>
                                        </div>
                                    </div>
                                    <div class="info-group">
                                        <div class="info-group__inner">
                                            <a href="#" id="followers_count">117</a>
                                            <span>粉丝数</span>
                                        </div>
                                    </div>


                                </div>
                                <!-- </div> -->
                            </div>
                        </div>

                    </div>
                </div>
            </div>


        </div>
    </div>
    <div class="notify-bar js-notify animated hinge hide">
    </div>

    <div class="modal fade" id="myModal" tabindex="-1" role="dialog"
        aria-labelledby="myModalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close"
                        data-dismiss="modal" aria-hidden="true">
                        &times;
                    </button>
                    <h4 class="modal-title" id="myModalLabel">评论
                    </h4>
                </div>
                <div class="modal-body">
                    <div class="app clf">
                        <div class="app-inner clmargin clearfix">
                            <div class="app-init-container">
                                <div class="sinaweibo-letter-wrap">

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
                                                        <div class="editor-module insert-shortlink">
                                                            <div class="js-reply-menu dropup hover dropdown--right add-reply-menu">
                                                                <a href="javascript:;" class="dropdown-toggle" data-toggle="dropdown">
                                                                    <span class="dropdown-current">插入链接</span>
                                                                    <i class="caret"></i>
                                                                </a>
                                                                <ul class="dropdown-menu">
                                                                    <li><a data-link-type="Goods" id="GoodsAndType" class="js-open-shortlink" href="javascript:;">商品及分类</a></li>
                                                                    <li><a data-link-type="Homepage" id="Homepage" class="js-open-shortlink" href="javascript:;" gotourl=' http://<%= Request.Url.Host+"/Default.aspx" %> '>店铺主页</a></li>
                                                                </ul>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="content-wrapper">
                                                    <textarea id="txtContent" class="js-txta" cols="30" rows="2"></textarea>
                                                    <div class="js-picture-container picture-container"></div>
                                                    <div class="complex-backdrop" style="display: none;">
                                                        <div class="js-complex-content complex-content"></div>
                                                    </div>
                                                </div>

                                                <div class="misc clearfix">
                                                    <div class="content-actions clearfix">
                                                        <div class="word-counter pull-right">还能输入 <i id="iLeftWords">300</i> 个字</div>

                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="notify-bar js-notify animated hinge hide">
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default"
                        data-dismiss="modal">
                        关闭
                    </button>
                    <button type="button" class="btn btn-primary" onclick="createcomments();">
                        提交
                    </button>
                </div>
            </div>
            <!-- /.modal-content -->
        </div>
    </div>
    <div class="modal fade" id="myIframeModal">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h4 class="modal-title">商品及分类</h4>
                </div>
                <div class="modal-body">
                    <iframe src="" id="MyIframe" width="750" height="370"></iframe>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">关闭</button>

                </div>
            </div>
            <!-- /.modal-content -->
        </div>
        <!-- /.modal-dialog -->
    </div>
</asp:Content>
