<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminNew.Master" AutoEventWireup="true" CodeBehind="menu.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Shop.menu" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="/admin/css/weibo.css">
    <script type="text/javascript" src="../js/weiboHelper.js"></script>
      <link rel="stylesheet" href="/Admin/shop/Public/css/dist/component-min.css">
    <link rel="stylesheet" href="/Admin/shop/Public/plugins/jbox/jbox-min.css">
    <!-- diy css start-->
    <link rel="stylesheet" href="/Admin/shop/PublicMob/css/style.css">
    <link rel="stylesheet" href="/Admin/shop/Public/plugins/uploadify/uploadify-min.css">
    <script src="/Admin/shop/Public/js/dist/underscore.js"></script>
    <script src="/Admin/shop/Public/plugins/jbox/jquery.jBox-2.3.min.js"></script>
    <script src="/Admin/shop/Public/plugins/zclip/jquery.zclip-min.js"></script>
    <script src="/Admin/shop/Public/plugins/uploadify/jquery.uploadify.min.js?ver2016"></script>
    <script src="/Admin/shop/Public/js/jquery-ui/jquery-ui.min.js"></script>
    <script src="/Admin/shop/Public/js/config.js"></script>
    <script src="/Admin/shop/Public/plugins/colorpicker/colorpicker.js"></script>
    <script src="/Admin/js/HiShopComPlugin.js"></script>
    <script src="/Admin/shop/Public/js/dist/componentadmin-min.js?v1023"></script>
    <style type="text/css">
        .subtitlespan {
            display: inline-block;
            width: 220px;
            overflow: hidden;
        }
        .jbox-container {
            overflow-x: hidden;
        }
        .gagp-goodslist .twoclass ul li {
            padding: 0;
            border-bottom: none;
        }
        /*.gagp-goodslist{
            min-height:300px !important;
        }*/
        .modalshopclasslist .oneclass p:first-child i.down {
            background: url("../images/iconfont-14052230.png");
        }</style>
    <script>

        $(function () {
           

            CreateDropdown($("#txtContent"), $("#myModal  .form-horizontal").eq(0), { createType: 1, showTitle: true, txtContinuity: false, reWriteSpan: false, style: "margin-left: 35px;" });

            $("#dropdow-menu-link  li[data-val='20']").remove();
            $("#dropdow-menu-link .dropdown-toggle").eq(0).css({ "padding-left": "15px" });
            InitTextCounter(32, "#txtMessageContent", "#iLeftWords");

            $("#Homepage").click(function () {
                $("#txtContent").val(($(this).attr("gotourl")));
                $("#txtshowContent").val(($(this).attr("gotourl")));
                messagetype(1);
            });
            $('#GoodsAndType').click(function () {
                var Rand = Math.random();
                $("#MyGoodsAndTypeIframe").attr("src", "Goods.aspx?Rand=" + Rand);
                $('#myIframeModal').modal('toggle').children().css({
                    width: '800px',
                    height: '700px'
                })
                $("#myIframeModal").modal({ show: true });
                $("#txtContent").val('');
                $("#txtshowContent").val('');
                messagetype(1);
            });
            loadmenu();
            
            

        })
        var bishi;
        var setmenuid="";
        function onemenu(menuid) {
            setmenuid = menuid;
            bishi = "1";
        }
        // 加载菜单列表
        function loadmenu() {
            $.getJSON("../../API/WeiboProcess.ashx?action=gettopmenus").done(function (d) {
                
                if (d.status == "0") {
                    var menuhtml = "";
                    $('#ulmenu li').remove();
                    $("#content").html("");
                    $('#tabpane').remove();
                    var b = 0;
                    var menuid = 0;
                    for (var i in d.data) {
                        var menudata = d.data[i];
                        var active = "";
                        if (setmenuid == "menu_" + menudata.menuid)
                            active = "class=\"active\"";
                        if (i == 0) {
                            if (i == d.data.length - 1)
                            {
                                active = "class=\"active\"";
                                b = 1;
                            }
                            if (setmenuid == "" && bishi != "0") {
                                active = "class=\"active\"";
                                b = 1;
                            }
                            menuhtml = "  <li " + active + " id=\"" + menudata.menuid + "\"><a href=\"#\" id=\"menu_" + menudata.menuid + "\" onclick=\"onemenu('menu_" + menudata.menuid + "')\">" + menudata.name + "</a></li>";
                            
                        }
                        else {
                            b = 0;
                           
                            if (bishi == "0" && (d.data.length - 1) == i) {
                                menuhtml = "  <li class=\"active\" id=\"" + menudata.menuid + "\"><a href=\"#\" id=\"menu_" + menudata.menuid + "\"  onclick=\"onemenu('menu_" + menudata.menuid + "')\">" + menudata.name + "</a></li>";
                               
                                menuid = menudata.menuid;
                                $("#" + setmenuid.split('_')[1]).removeClass('active');
                                //onemenu("menu_" + menudata.menuid + "");
                            }
                            else
                                menuhtml = "  <li " + active + " id=\"" + menudata.menuid + "\"><a href=\"#\" id=\"menu_" + menudata.menuid + "\"  onclick=\"onemenu('menu_" + menudata.menuid + "')\">" + menudata.name + "</a></li>";
                        }
                        var childmenuhtml = "";
                        var js=0;
                        for (var j in menudata.childdata) {

                            var childmenudata = menudata.childdata[j];
                            childmenuhtml += "<div class='list'><span  >" + childmenudata.name + "</span><span  class='edit' ><span style=\"cursor: pointer;\" onclick=\"addandeditmenu('1','" + childmenudata.menuid + "','" + childmenudata.parentmenuid + "','two')\">编辑</span>&nbsp;|<span style=\"cursor: pointer;\" onclick=\"delmenu('" + childmenudata.menuid + "','1')\">删除</span></span></div>";
                        }


                        $("#addmenu").before(menuhtml);//添加父菜单的Tab选项卡
                        var tabcontent = $($("#tabcontent").html());
                        tabcontent.find('#fltitle').text(menudata.name);
                        tabcontent.find('#EditMenu').attr("onclick", "addandeditmenu('1','" + menudata.menuid + "','','one')");
                        tabcontent.find('#DelMenu').attr("onclick", "delmenu('" + menudata.menuid + "','0')");
                        tabcontent.find('#addtwomenu').attr("onclick", "addandeditmenu('0','','" + menudata.menuid + "','two')");
                        tabcontent.find("#childmenu").append(childmenuhtml);//添加子菜单
                       
                        $("#content").append(tabcontent);//添加父菜单
                        tabcontent.find("#tabpane").attr("id", "tabmenu_" + menudata.menuid + "");
                        tabcontent.find("#tabmenu_" + menudata.menuid).parent('#panediv').attr("id", "toptabmenu_" + menudata.menuid);
                        if (setmenuid == "" && b == 1) {
                        
                            $("#toptabmenu_" + menudata.menuid).addClass('active');
                        }
                        else {
                              
                            if (bishi == "0") {
                                $("#toptabmenu_" + setmenuid.split('_')[1]).removeClass('active');
                                $("#toptabmenu_" + menuid).addClass('active');
                              
                            }
                            else {
                                $("#toptabmenu_" + setmenuid.split('_')[1]).addClass('active');
                            }

                        }
                          
                        if (menudata.childdata.length == 0) {
                            tabcontent.find("#spanhid").text("");
                        }
                    }

                    setload();
                }
                else
                {
                    
                   
                }
            });

        }
        //加载Tab选项卡
        function setload() {
            $('#mytabl > ul li').click(function () {
                $('#mytabl > ul li').removeClass('active');
                $(this).addClass('active');
                $(this).parent().next().children().removeClass('active');
                $(this).parent().next().children().eq($(this).index()).addClass('active');
            })
        }
        var andedit;
        var editid;
        var parentid;
        //打开窗口
        function addandeditmenu(type, id, parentmenuid, oneortwo) {
            andedit = type;
            if (oneortwo == "two")
                if (parentmenuid == "")
                    bishi = "0";

            if (oneortwo == "one")
                if (type == "0") {
                    bishi = "0";
                }
                else {
                    if (parentmenuid == "" && bishi != "0")
                        bishi = "1";
                }

            editid = id;
            parentid = parentmenuid
            $("#txttitle").val('');
            $("#txtContent").val('');
            $("#txtshowContent").val('');
            if (type == 0) {

                $("#modaltitle").text('添加菜单');
            }
            else {
                $("#modaltitle").text('修改菜单');
                var url = "&MenuId=" + id;
                $.getJSON("../../API/WeiboProcess.ashx?action=getmenu" + url).done(function (d) {
                    if (d.status == "0") {

                        var data = d.data[0];
                        $("#txttitle").val(data.name);
                        $("#txtContent").val(data.content);
                        $("#txtshowContent").val(data.content);
                    }
                    else {
                        HiTipsShow("查询菜单失败！", 'fail');
                    }
                });
            }
            $('#myModal').modal('toggle').children().css({
                width: '600px',
                height: '500px'
            })
            $("#myModal").modal({ show: true });
        }
        //添加和修改菜单
        function submitaddandedit() {
            if ($.trim($("#txttitle").val()) == "") {
                HiTipsShow("请输入标题！", 'warning');
                return;
            }
            if ($.trim($("#txtContent").val()) == "") {
                HiTipsShow("请填写输入链接！", 'warning');
                return;
            }
             
            if (parentid == "") {
                if ($.trim($("#txttitle").val()).length > 5) {
                    HiTipsShow("一级菜单标题不能大于5个字！", 'warning');
                    return;
                }
            }
            else
            {
                if ($.trim($("#txttitle").val()).length > 13) {
                    HiTipsShow("二级菜单标题不能大于13个字！", 'warning');
                    return;
                }
            }


            var Type = 'click';
            //if (mestype != 0)
            Type = 'view';
            var urlContent = $("#txtContent").val();
            if (!isSimpleURL(urlContent)) {
                HiTipsShow("请输入正确的链接地址！", 'warning');
                return;
            }
            if (andedit == 0) {//添加一级和二级菜单
                var url = "&ParentMenuId=" + parentid + "&Name=" + encodeURIComponent($("#txttitle").val()) + "&Content=" + encodeURIComponent($("#txtContent").val()) + "&Type=" + Type + "";
                $.getJSON("../../API/WeiboProcess.ashx?action=addmenu" + url).done(function (d) {
                    if (d.status == "0") {
                        ShowMsg("保存成功!", true);
                        loadmenu();
                        $('#myModal').modal('hide')
                    }
                    else {
                        if (d.status == "1") {
                            HiTipsShow("添加菜单失败！", 'fail');
                        }
                        else {
                            HiTipsShow("一级菜单最多只能添加三个,二级菜单最多只能添加五个！", 'fail');
                        }
                    }
                });
            }
            if (andedit == 1)//修改一级和二级菜单
            {
                var url = "&ParentMenuId=" + parentid + "&MenuId=" + editid + "&Name=" + encodeURIComponent($("#txttitle").val()) + "&Content=" + encodeURIComponent($("#txtContent").val()) + "&Type=" + Type + "";
                $.getJSON("../../API/WeiboProcess.ashx?action=editmenu" + url).done(function (d) {
                    if (d.status == "0") {
                        ShowMsg("保存成功!", true);
                        loadmenu();
                        $('#myModal').modal('hide')
                    }
                    else {
                        HiTipsShow("修改菜单失败！", 'fail');
                    }
                });
            }
        }
        function isSimpleURL(str_url) {// 验证url
            var strRegex = "^(https|http)://(.*)$";
            var re = new RegExp(strRegex);
            return re.test(str_url);
        }
        function delmenu(id,type) {
            if (confirm("确定要删除数据吗？")) {
                var url = "&MenuId=" + id;
                $.getJSON("../../API/WeiboProcess.ashx?action=delmenu" + url).done(function (d) {
                    if (d.status == "0") {
                        if (type == 0) {
                            setmenuid = "";

                        }

                        $('#ulmenu li').remove();
                        $("#content").html("");
                        $('#tabpane').remove();
                        loadmenu();
                        HiTipsShow("删除成功！", 'success');

                    }
                    if (d.status == "2") {
                        HiTipsShow("请先删除二级菜单！", 'fail');
                    }
                });
            }
        }
        function showmessage() {

            $('#myMessageModal').modal('toggle').children().css({
                width: '600px',
                height: '500px'
            })
            $("#myMessageModal").modal({ show: true });

            if (mestype == 0)
                $("#txtMessageContent").val($("#txtContent").val());
            else
                $("#txtMessageContent").val('');
        }
        function okmessage() {
            $("#txtContent").val($("#txtMessageContent").val());
            $("#txtshowContent").val($("#txtMessageContent").val());
            $("#myMessageModal").modal('hide');
        }
        function jsopenemotion() {
            var EmotionFace = weiboHelper.options.Emotions;
            if ($(".emotion-wrapper").is(":visible")) {
                $(".emotion-wrapper").hide()
            } else {
                var emotionHtml = "";
                for (var i = 0; i < EmotionFace.length; i++) {
                    emotionHtml += '<li><img src="http://img.t.sinajs.cn/t4/appstyle/expression/ext/normal/' + EmotionFace[i][0] + '" alt="[' + EmotionFace[i][1] + ']" title="[' + EmotionFace[i][1] + ']"></li>';
                }
                $(".emotion-container").html(emotionHtml);
                $(".emotion-wrapper").show("slow", function () {
                    $(".emotion-container img").click(function () {
                        $("#txtMessageContent").val($("#txtMessageContent").val() + ($(this).attr("alt"))).keyup();
                        $(".emotion-wrapper").hide()
                    })
                });
            }
        }
        function contentchange() {
            $("#txtshowContent").val($("#txtContent").val());
        }
        var mestype = 0;
        function messagetype(type) {
            mestype = type;
        }
        function okhttp() {
            var content = "http://";
            if ($.trim($("#txthttp").val()) == "") {
                HiTipsShow("请输入链接地址！", 'warning');
                return;
            }

            $("#txtContent").val('');
            $("#txtshowContent").val('');
            $("#txtContent").val($("#txthttp").val());
            $("#txtshowContent").val($("#txthttp").val());
            if ($("#txthttp").val().indexOf('http://') == -1) {
                $("#txtContent").val(content + $("#txthttp").val());
                $("#txtshowContent").val(content + $("#txthttp").val());
            }
            $("#myOutHttpModal").modal('hide');
        }
        function showhttp(type) {
            $("#txthttp").val('');
            mestype = type;
            $('#myOutHttpModal').modal('toggle').children().css({
                width: '500px',
                height: '100px'
            })
            $("#myOutHttpModal").modal({ show: true });

        }
        function savemenu()
        {
            $.getJSON("../../API/WeiboProcess.ashx?action=savemenu").done(function (d) {
                if(d.result)
                {
                    HiTipsShow("成功保存到微博！", 'success');
                    return;
                }
                else
                {
                    HiTipsShow("保存到微博失败，请确认是否授权或者是否开通自定义菜单！", 'fail');
                    return;
                }
            });
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <div class="page-header">
                    <h2>自定义菜单</h2>
                    <small></small>
    </div>
    <script type="text/template" id="tabcontent">
        <div id="panediv" class="tab-pane navgation">
        <div class="tab-pane navgation" id="tabpane">
            <p class="nav-one">一级菜单</p>
            <div class="shop-index clearfix"><span class="fl" id="fltitle">店铺主页</span><span class="fr"><span style="cursor: pointer;" id="EditMenu">编辑</span>&nbsp;|<span style="cursor: pointer;" id="DelMenu">删除</span>&nbsp;<span id="spanhid">|&nbsp;设置二级菜单以后，主链接已失效。</span></span></div>
            <p class="nav-two">二级菜单</p>
            <div class="nav-two-list" id="childmenu">
            </div>
            <div class="add-navgation" id="addtwomenu">
                ＋添加二级菜单
            </div>
        </div>
            </div>
    </script>

    <div id="mytabl">
        <!-- Nav tabs -->
        <ul class="nav nav-tabs margin" id="ulmenu">

            <div id="addmenu" class="addmenu-one" onclick="addandeditmenu(0,'','','one');">＋添加一级菜单</div>
        </ul>
        <!-- Tab panes -->
        <div class="tab-content" id="content">
        </div>
    </div>
    <br />
    <div style="text-align: center">

        <button type="button" class="btn btn-primary" onclick="savemenu();">
        保存到微博
                    
                </button>
    </div>
    <div class="modal fade" id="myModal">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h4 class="modal-title" id="modaltitle"></h4>
                </div>
                <div class="modal-body">


                    <form class="form-horizontal">
                        <div class="form-group">
                            <label for="title" class="col-xs-2 control-label">标题：</label>
                            <div class="col-xs-3">
                                <input type="text" class="form-control" id="txttitle" placeholder="标题">
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="content" class="col-xs-2 control-label">输入链接：</label>
                            <div class="col-xs-5">
                                <textarea id="txtshowContent" cols="30" rows="3" style="width: 400px;display: none" disabled class="form-control" placeholder=""></textarea>
                                <%--<textarea id="txtContent" cols="30" rows="3" style="width: 400px; " onchange="contentchange();" class="form-control" placeholder="回复内容"></textarea>--%>
                                <input type="text" id="txtContent" style="width:400px;"  class="form-control" placeholder="请输入链接或选择下面的链接" />
                            </div>
                        </div>
                            <div class="form-group" >
                                </div>

                        <div class="form-group" style="display:none;">
                            <div class="menu-list">
                                <ul class="clearfix">
          <%--                          <li><a data-link-type="Goods" id="message" class="js-open-shortlink" href="javascript:;" onclick="showmessage();">一般信息</a></li>--%>
                                    <li><a data-link-type="Goods" class="js-open-shortlink" href="javascript:;" onclick="showhttp('1');">自定义外链</a></li>
                                    <li><a data-link-type="Goods" id="GoodsAndType" class="js-open-shortlink" href="javascript:;">商品及分类</a></li>
                                    <li><a data-link-type="Homepage" id="Homepage" class="js-open-shortlink" href="javascript:;" gotourl=' http://<%= Request.Url.Host+"/Default.aspx" %> '>店铺主页</a></li>
                                </ul>
                            </div>
                        </div>
                    </form>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">关闭</button>
                    <button type="button" class="btn btn-primary" onclick="submitaddandedit();">提交</button>
                </div>
            </div>
            <!-- /.modal-content -->
        </div>
        <!-- /.modal-dialog -->
    </div>

    <div class="modal fade" id="myIframeModal">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h4 class="modal-title">商品及分类</h4>
                </div>
                <div class="modal-body">
                    <iframe src="" id="MyGoodsAndTypeIframe" width="750" height="370"></iframe>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">关闭</button>

                </div>
            </div>
            <!-- /.modal-content -->
        </div>
        <!-- /.modal-dialog -->
    </div>
    <div class="modal fade" id="myMessageModal">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h4 class="modal-title">一般信息</h4>
                </div>
                <div class="modal-body">

                    <div class="app-init-container">
                        <div class="sinaweibo-letter-wrap">


                            <div class="wb-sender">
                                <div class="wb-sender__inner">
                                    <div class="wb-sender__input js-editor-wrap">
                                        <div class="misc top clearfix">
                                            <div class="content-actions clearfix">

                                                <div class="editor-module insert-emotion">
                                                    <a href="javascript:;" id="jsopenemotion" onclick="jsopenemotion();">表情</a>
                                                    <div class="emotion-wrapper">
                                                        <ul class="emotion-container clearfix">
                                                        </ul>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="content-wrapper">
                                            <textarea id="txtMessageContent" class="js-txta" cols="50" rows="4"></textarea>
                                            <div class="js-picture-container picture-container"></div>
                                            <div class="complex-backdrop">
                                                <div class="js-complex-content complex-content" id="picback"></div>
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
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">关闭</button>
                    <button type="button" class="btn btn-primary" onclick="okmessage();">确定</button>
                </div>
            </div>
            <!-- /.modal-content -->
        </div>
        <!-- /.modal-dialog -->
    </div>

    <div class="modal fade" id="myOutHttpModal">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h4 class="modal-title">自定义外链</h4>
                </div>
                <div class="modal-body">
                    <form class="form-horizontal">
                        <div class="form-group">
                            <label for="title" class="col-xs-3 control-label">链接地址</label>
                            <div class="col-xs-6">
                                <input type="text" style="width: 300px;" class="form-control" id="txthttp" placeholder="链接地址：http://example.com">
                            </div>
                        </div>

                    </form>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">关闭</button>
                    <button type="button" class="btn btn-primary" onclick="okhttp();">确定</button>
                </div>
            </div>
            <!-- /.modal-content -->
        </div>
        <!-- /.modal-dialog -->
    </div>
</asp:Content>
