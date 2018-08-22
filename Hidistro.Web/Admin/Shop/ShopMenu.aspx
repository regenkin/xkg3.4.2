<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminNew.Master" AutoEventWireup="true" CodeBehind="ShopMenu.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Shop.ShopMenu" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="/admin/css/weibo.css">
    <link rel="stylesheet" href="/Admin/shop/Public/css/dist/component-min.css">
    <link rel="stylesheet" href="/Admin/shop/Public/plugins/jbox/jbox-min.css">

    <!-- diy css start-->
    <link rel="stylesheet" href="/Admin/shop/PublicMob/css/style.css">

    <link rel="stylesheet" href="/Admin/shop/Public/plugins/uploadify/uploadify-min.css">
    <script src="/Admin/shop/Public/js/dist/underscore.js"></script>
    <script src="/Admin/shop/Public/plugins/jbox/jquery.jBox-2.3.min.js"></script>
    <script src="/Admin/shop/Public/plugins/zclip/jquery.zclip-min.js"></script>
    <script src="/Admin/shop/Public/plugins/zclip/jquery.cookie.js"></script>
    <script src="/Admin/shop/Public/plugins/uploadify/jquery.uploadify.min.js?ver2016"></script>
    <script src="/Admin/shop/Public/js/jquery-ui/jquery-ui.min.js"></script>
    <script src="/Admin/shop/Public/js/config.js"></script>

    <script src="/Admin/shop/Public/plugins/colorpicker/colorpicker.js"></script>

    <script src="/Admin/js/HiShopComPlugin.js"></script>
    <script src="/Admin/shop/Public/js/dist/componentadmin-min.js?v1023"></script>
    <script type="text/javascript" src="../js/weiboHelper.js"></script>
    <link rel="stylesheet" href="../css/common.css" />
    <script type="text/javascript" src="../js/Region.js"></script>
    <style type="text/css">        .form-group {
    padding:2px    }
        .gagp-goodslist .twoclass ul li {
        padding:0;border-bottom:none;
        }
        .jbox-container {
        overflow-x:hidden;
        }
        .gagp-goodslist {
            min-height:200px;
        }
        .modalshopclasslist .oneclass p:first-child i.down{
            background:url("../images/iconfont-14052230.png");
        }
        .preview {
    display: table-cell;
    width: 88px;
    height: 88px;
    border: 1px solid #CFCFCF;
    vertical-align: middle;
    text-align: center;
}
       #albums {
    font-size: 12px;
}
    </style>

    <script>
        function callBackDrpMenu() {
            HiShop.popbox.dplPickerColletion({
                linkType: $(this).data("val"),
                callback: function (item, type) {
                    //ele.show();
                    var link = item.link;
                    //if (link.indexOf('http') > -1) {
                    //    link = item.link;
                    //} else {
                    //    link = "http://" + window.location.host + item.link;
                    //}
                    $("#txtshowContent").val(link);
                    $("#txtContent").val(link);
                }
            });
        }

        $(function () {
            InitTextCounter(32, "#txtMessageContent", "#iLeftWords");
            CreateDropdown($("#txtContent"), $("#uploaderpic"), { createType: 3, showTitle: true, style: "margin-left: 50px;", txtContinuity: false, reWriteSpan: false, callback: callBackDrpMenu });

            $("#dropdow-menu-link  li[data-val='20']").remove();
            
            loadmenu();

            $('#Picture,#uploader_preview').click(function () {
                HiShop.popbox.ImgPicker(function (obj) {
                    SaveImg(obj);
                });
            });

        })
        var setmenuid = "";
        var active = "";
        var itemsactive = "";
        function onemenu(menuid) {
            setmenuid = menuid;
        }
        function SaveImg(obj) {
            var str = obj + "";
            //str=obj+"";
            //alert(obj)
            $("#uploader_preview").html("<img style='width:60px;height:60px;background-color: rgb(255, 255, 255);' id='picsrc' src='" + str.split(",")[0] + "'>");
        }
        function edittitle(id) {

            $("#txtedittitle" + id).focus();
            $("#spantitlename" + id).css("display", "none");
            $("#spanedittile" + id).css("display", "");
            $("#btnedit" + id).css("display", "none");
            $("#submitedit" + id).css("display", "");
        }
        function conseleditwin(id) {
            $("#spantitlename" + id).css("display", "");
            $("#spanedittile" + id).css("display", "none");
            $("#btnedit" + id).css("display", "");
            $("#submitedit" + id).css("display", "none");
        }
        //菜单显示
        function showmenu(data) {

            $("#showmenuul").html("");
            $("#showtextul").html("");
            if (data.enableshopmenu == "True") {
                var html = "";
                if (data.shopmenustyle == "1") {
                    $("#textmenu").remove();
                    for (var i in data.data) {
                        var menudata = data.data[i];

                        html += "<li class=\"child\">";

                        html += "<div class=\"img\">";
                        html += " <img src=\"" + menudata.shopmenupic + "\"/>";
                        html += "</div>";

                        html += "<p>" + menudata.name + "</p>";

                        if (menudata.childdata.length > 0) {
                            html += " <div class=\"inner-nav\"><ul>";
                            for (var j in menudata.childdata) {
                                var childmenudata = menudata.childdata[j];

                                html += " <li>" + childmenudata.name + "</li>";

                            }
                            html += " </ul></div>";
                        }
                        html += "</li>";

                    }
                    $("#showmenuul").append(html);
                }
                else {

                    $("#picmenu").remove();
                    for (var i in data.data) {
                        var menudata = data.data[i];
                        if (i == 0)
                            html += "<li class=\"noborder\">";
                        else
                            html += "<li>";
                        html += "<p>" + menudata.name + "</p>";
                        if (menudata.childdata.length > 0) {
                            html += " <div class=\"inner-nav\"><ul>";
                            for (var j in menudata.childdata) {
                                var childmenudata = menudata.childdata[j];

                                html += " <li>" + childmenudata.name + "</li>";

                            }
                            html += " </ul></div>";
                        }
                        html += "</li>";

                    }
                    $("#showtextul").append(html);
                }



                $('.mobile-nav ul li').not('.mobile-nav ul li li').css('width', $('.mobile-nav').width() / $('.mobile-nav ul li').not('.mobile-nav ul li li').length).hover(function () {
                    $(this).find('.inner-nav').show().css({
                        'top': -$(this).find('.inner-nav').height() - 20,
                        'left': '50%',
                        'marginLeft': -$(this).find('.inner-nav').width() / 2
                    });
                }, function () {
                    $(this).find('.inner-nav').hide();
                })
            }

        }
        function EnterPress(e, id) {
            var e = e || window.event;
            if (e.keyCode == 13) {
                updatename(id);
            }
        }
        // 加载菜单列表
        function loadmenu() {
            $.getJSON("../../API/MenuProcess.ashx?action=gettopmenus").done(function (d) {

                if (d.status == "0") {
                    showmenu(d);
                    var menuhtml = "";
                    $('#ulmenu li').remove();
                    $("#content").html("");
                    $('#tabpane').remove();

                    if (d.data.length > 4) {
                        $("#addmenu").css("display", "none");
                    }
                    else {
                        $("#addmenu").css("display", "");
                    }
                    for (var i in d.data) {
                        
                        active = "";
                        itemsactive = "";
                        var menudata = d.data[i];
                         //设置选中选项开始
                        if (setmenuid == "")
                            if (i == 0) {
                                active = "class=\"active\"";
                                itemsactive = "active";
                            }
                            else {
                                active = "";
                                itemsactive = "";
                            }
                        else {
                            if(setmenuid.split('_')[1]== menudata.menuid )
                            {
                                active = "class=\"active\"";
                                itemsactive = "active";
                            }
                        }
                        //设置选中选项结束
                        menuhtml = "  <li " + active + " id=\"" + menudata.menuid + "\"><a id=\"menu_" + menudata.menuid + "\"  onclick=\"onemenu('menu_" + menudata.menuid + "')\">" + menudata.name + "<i class=\"glyphicon glyphicon-remove\"   onclick=\"delmenu('" + menudata.menuid + "','0')\"></i></a></li>";
                        var childmenuhtml = "";
                        var js = 0;
                        for (var j in menudata.childdata) {
                            //&nbsp;|<span style=\"cursor: pointer;\" onclick=\"delmenu('" + childmenudata.menuid + "','1')\">删除</span>
                            var childmenudata = menudata.childdata[j];
                            childmenuhtml += "<div class='list'><span><span id=\"spantitlename" + childmenudata.menuid + "\">" + childmenudata.name + "</span><span style=\"display:none;\" id=\"spanedittile" + childmenudata.menuid + "\"><input type=\"text\" onkeypress=\"EnterPress(event,'" + childmenudata.menuid + "');\" class=\"form-control\" id=\"txtedittitle" + childmenudata.menuid + "\"   style=\"width:120px;\" value=\"" + childmenudata.name + "\"></span></span><span  class='edit' ><span id=\"btnedit" + childmenudata.menuid + "\" style=\"cursor: pointer;\" onclick=\"edittitle('" + childmenudata.menuid + "')\">编辑</span><span style=\"display:none;\" id=\"submitedit" + childmenudata.menuid + "\"><input id=\"saveedit\" type=\"button\" value=\"保存\" onclick=\"updatename('" + childmenudata.menuid + "')\" class=\"btn btn-primary\"/>&nbsp;<input id=\"conseledit\" class=\"btn btn-danger\" onclick=\"conseleditwin('" + childmenudata.menuid + "')\" type=\"button\" value=\"取消\" /></span>&nbsp;|&nbsp;<span style=\"cursor: pointer;\" onclick=\"delmenu('" + childmenudata.menuid + "','1')\">删除</span>&nbsp;|&nbsp;<span style=\"cursor: pointer;color:blue\" onclick=\"addandeditmenu('1','" + childmenudata.menuid + "','" + childmenudata.parentmenuid + "','two')\">链接</span></span></div>";
                        }


                        $("#addmenu").before(menuhtml);//添加父菜单的Tab选项卡
                        var tabcontent = $($("#tabcontent").html());
                        tabcontent.find('#fltitle').text(menudata.name);
                        tabcontent.find('#EditMenu').attr("onclick", "addandeditmenu('1','" + menudata.menuid + "','','one')");

                        tabcontent.find('#addtwomenu').attr("onclick", "addandeditmenu('0','','" + menudata.menuid + "','two')");
                        tabcontent.find("#childmenu").append(childmenuhtml);//添加子菜单

                        $("#content").append(tabcontent);//添加父菜单
                        tabcontent.find("#tabpane").attr("id", "tabmenu_" + menudata.menuid + "");
                        tabcontent.find("#tabmenu_" + menudata.menuid).parent('#panediv').attr("id", "toptabmenu_" + menudata.menuid);//设置子菜单DIv的ID
                        $("#toptabmenu_" + menudata.menuid).addClass(itemsactive);

                        if (menudata.childdata.length == 0) {
                            tabcontent.find("#spanhid").text("");
                        }
                        
                    }

                    setload();
                }
                else {


                }
            });

        }
        function updatename(id) {

            var name = $("#txtedittitle" + id).val();


            var url = "&MenuId=" + id + "&Name=" + name;
            if ($.trim(name) == "") {
                HiTipsShow("请填写标题！", 'warning');
                return;
            }
            if ($.trim(name).length > 7) {
                HiTipsShow("二级菜单标题不能大于7个字！", 'warning');
                return;
            }
            $.getJSON("../../API/MenuProcess.ashx?action=updatename" + url).done(function (d) {
                if (d.status == "0") {
                    loadmenu();
                    conseleditwin(id);
                    $("#spantitlename" + id).text(name);
                    HiTipsShow("修改成功！", 'success');
                }
                else {
                    HiTipsShow("修改失败！", 'fail');
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
        var bishi;
        var editid;
        var parentid;
        //打开窗口
        function addandeditmenu(type, id, parentmenuid, oneortwo) {

            andedit = type;
            if (oneortwo == "two")
                $("#titlemessage").html("标题不能为空，长度在7个字以内");

            if (oneortwo == "one")
                $("#titlemessage").html("标题不能为空，长度在4个字以内");

            if (parentmenuid == "") {
                $("#uploaderpic").css("display", "");
            } else {

                //$("#uploader1_image").remove();
                $("#uploaderpic").css("display", "none");
                //$("#uploader1_upload").css("display", "");
                //$("#uploader1_delete").css("display", "none");
            }
            if (oneortwo == "one") {
                $("#linkem").css('display', 'none')
            }
            else {
                $("#linkem").css('display', '')
            }
            editid = id;
            parentid = parentmenuid
            $("#txttitle").val('');
            $("#txtContent").val('');
            $("#txtshowContent").val('');
            if (type == 0) {
                //$("#uploader1_preview").html('');
                //$("#uploader1_upload").css("display", "");
                //$("#uploader1_delete").css("display", "none");
                $("#modaltitle").text('添加导航');
            }
            else {
                $("#modaltitle").text('修改导航');
                var url = "&MenuId=" + id;
                $.getJSON("../../API/MenuProcess.ashx?action=getmenu" + url).done(function (d) {
                    if (d.status == "0") {
                        var data = d.data[0];
                        $("#txttitle").val(data.name);
                        $("#txtContent").val(data.content);
                        $("#txtshowContent").val(data.content);
                        if ($.trim(data.shopmenupic) != "") {
                            $("#uploader_preview").html("<img style='width:60px;height:60px;background-color: rgb(255, 255, 255);' id='picsrc' src='" + data.shopmenupic + "'>");
                        }
                        else {
                            $("#uploader_preview").html("");
                        }
                    }
                    else {
                        HiTipsShow("查询导航失败！", 'fail');
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
                HiTipsShow("请填写标题！", 'warning');
                return;
            }


            if (parentid == "") {

                if ($.trim($("#txttitle").val()).length > 4) {
                    HiTipsShow("一级导航标题最多只能添加4个字！", 'warning');
                    return;
                }
            }
            else {


                if ($.trim($("#txttitle").val()).length > 7) {
                    HiTipsShow("二级导航标题不能大于7个字！", 'warning');
                    return;
                }
                if ($.trim($("#txtContent").val()) == "") {
                    HiTipsShow("链接内容不能为空！", 'warning');
                    return;
                }
            }


            var Type = 'click';
            if (mestype != 0)
                Type = 'view';
            if (andedit == 0) {//添加一级和二级菜单
                var pic = "";
                if ($.trim($("#uploader_preview").html()) != "") {
                    pic = $("#picsrc").attr("src");
                }
                if (pic == undefined) {
                    pic = "";
                }
                var url = "&ParentMenuId=" + parentid + "&Name=" + encodeURIComponent($("#txttitle").val()) + "&Content=" + encodeURIComponent($("#txtContent").val()) + "&Type=" + Type + "&ShopMenuPic=" + encodeURIComponent(pic);
                $.getJSON("../../API/MenuProcess.ashx?action=addmenu" + url).done(function (d) {
                    if (d.status == "0") {
                        HiTipsShow("添加成功！", 'success');
                        if (parentid == "") {
                            setmenuid = "menu_" + d.menuid;
                        }
                        loadmenu();
                        $('#myModal').modal('hide')
                    }
                    else {
                        if (d.status == "1") {
                            HiTipsShow("添加导航失败！", 'fail');
                        }
                        else {
                            HiTipsShow("一级导航最多只能添加4个,二级菜单最多只能添加5个！", 'fail');
                        }
                    }
                });
            }
            if (andedit == 1)//修改一级和二级菜单
            {
                var pic = "";
                if ($("#uploader_preview").html() != "") {
                    pic = $("#picsrc").attr("src");
                }
                if (pic == undefined) {
                    pic = "";
                }
                var url = "&ParentMenuId=" + parentid + "&MenuId=" + editid + "&Name=" + encodeURIComponent($("#txttitle").val()) + "&Content=" + encodeURIComponent($("#txtContent").val()) + "&Type=" + Type + "&ShopMenuPic=" + encodeURIComponent(pic);
                $.getJSON("../../API/MenuProcess.ashx?action=editmenu" + url).done(function (d) {
                    if (d.status == "0") {
                        HiTipsShow("修改成功！", 'success');
                        loadmenu();
                        $('#myModal').modal('hide')
                    }
                    else {
                        HiTipsShow("修改导航失败！", 'fail');
                    }
                });
            }
        }
        function delmenu(id, type) {
            if (confirm("确定要删除数据吗？")) {
                var url = "&MenuId=" + id;
                $.getJSON("../../API/MenuProcess.ashx?action=delmenu" + url).done(function (d) {
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

        function setEnable(obj) {
            var ob = $("#" + obj.id);
            var cls = ob.attr("class");
            var enable = "false";
            if (cls == "switch-btn") {

                ob.empty();
                ob.append("已关闭 <i></i>")
                ob.removeClass();
                ob.addClass("switch-btn off");
                enable = "false";

            }
            else {
                ob.empty();
                ob.append("已开启 <i></i>")
                ob.removeClass();
                ob.addClass("switch-btn");
                enable = "true";
            }
            $.ajax({
                type: "post",
                url: "../../API/MenuProcess.ashx",
                data: { type: "1", enable: enable, action: "setenable" },
                dataType: "text",
                success: function (data) {
                    if (enable == 'true') {
                        msg('已开启！');

                    }
                    else {
                        msg('已关闭！');

                    }
                    loadmenu();
                }
            });
        }
        function msg(msg) {
            HiTipsShow(msg, 'success');
        }

        function errAlert(msg) {
            HiTipsShow(msg, 'error');
        }
        $(function () {
            $('body').on('mouseover', '#mytabl ul li', function () {
                $(this).find('i').show();
            });
            $('body').on('mouseout', '#mytabl ul li', function () {
                $(this).find('i').hide();
            });
        })
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form runat="server">
        <div class="page-header">
            <h2>店铺导航</h2>
            <small></small>
        </div>
        <div class="set-switch">
            <p class="mb5"><strong>店铺导航</strong></p>
            <p>店铺导航是一组快捷菜单，您可以自定义每个导航的链接，并决定在哪些页面显示店铺导航</p>
            <div id="offlineEnable" class="hide <%=_enable?"switch-btn":"switch-btn off" %>" onclick="setEnable(this)">
                <%=_enable?"已开启":"已关闭"%>
                <i></i>
            </div>
        </div>
        <div class="shop-navigation clearfix">
            <div class="fl">
                <div class="mobile-border">
                    <div class="mobile-d">
                        <div class="mobile-header">
                            <i></i>
                            <div class="mobile-title">店铺主页</div>
                        </div>
                        <div class="set-overflow">
                            <div class="white-box"></div>
                            <div class="mobile-nav" id="picmenu">
                                <ul class="clearfix" id="showmenuul">
                                </ul>
                            </div>
                            <div class="mobile-nav mobile-nav-text" id="textmenu">
                                <ul class="clearfix" id="showtextul">
                                </ul>
                            </div>
                        </div>
                    </div>
                    <div class="clear-line">
                        <div class="mobile-footer"></div>
                    </div>
                </div>
            </div>
            <div class="fl frwidth">
                <div class="set-switch">
                    <p class="mb5"><strong>在以下页面显示店铺导航:</strong></p>
                    <p style="line-height: 22px">
                        <asp:CheckBox ID="ShopDefault" runat="server" Text="店铺主页" />&nbsp;&nbsp;
                         <asp:CheckBox ID="ActivityMenu" runat="server" Text="店铺活动" />&nbsp;&nbsp;
                    <asp:CheckBox ID="MemberDefault" runat="server" Text="会员中心" />&nbsp;&nbsp;
                        <asp:CheckBox ID="DistributorsMenu" runat="server" Text="分销中心" />
                        <br />
                        <asp:CheckBox ID="GoodsListMenu" runat="server" Text="所有商品" />&nbsp;&nbsp;
                              
                         <asp:CheckBox ID="GoodsCheck" runat="server" Text="搜索结果" />&nbsp;&nbsp;
                    <asp:CheckBox ID="GoodsType" runat="server" Text="商品分类" />&nbsp;&nbsp;
                   
                    <asp:CheckBox ID="BrandMenu" runat="server" Text="品牌专题" />

                    </p>
                </div>
                <div class="set-switch">
                    <p class="mb5"><strong>店铺导航显示风格:</strong></p>
                    <p>
                        <asp:RadioButtonList ID="RadioType" runat="server" RepeatDirection="Horizontal" CellPadding="-1" CellSpacing="-1" Width="200">
                            <asp:ListItem Text="纯文字" Value="0" Selected="True">纯文字</asp:ListItem>
                            <asp:ListItem Text="小图标加文字" Value="1">小图标加文字</asp:ListItem>
                        </asp:RadioButtonList>

                    </p>
                </div>

                <script type="text/template" id="tabcontent">
                    <div id="panediv" class="tab-pane navgation">
                        <div class="tab-pane navgation" id="tabpane">
                            <p class="nav-one">一级导航</p>
                            <div class="shop-index clearfix"><span class="fl" id="fltitle">店铺主页</span><span class="fr"><span style="cursor: pointer;" id="EditMenu">编辑</span>&nbsp;<span id="spanhid">|&nbsp;<small class="inl">设置二级导航以后，主链接已失效。</small></span></span></div>
                            <p class="nav-two">二级导航</p>
                            <div class="nav-two-list" id="childmenu">
                            </div>
                            <div class="add-navgation" id="addtwomenu">
                                ＋添加二级导航
                            </div>
                        </div>
                    </div>
                </script>

                <div id="mytabl">
                    <!-- Nav tabs -->
                    <ul class="nav nav-tabs margin" id="ulmenu">
                        <div id="addmenu" class="addmenu-one" onclick="addandeditmenu(0,'','','one');">＋添加一级导航</div>
                    </ul>
                    <!-- Tab panes -->
                    <div class="tab-content" id="content">
                    </div>
                </div>
            </div>
        </div>
        <br />

        <div style="text-align: center; margin-bottom: 20px;">
            <asp:Button ID="BtnSave" class="btn btn-success inputw100" runat="server" Text="保存" />
        </div>
        <div class="modal fade" id="myModal">
            <div class="modal-dialog">
                <div class="modal-content">
                    <from id="myModealFrom">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title" id="modaltitle"></h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-horizontal">
                            <div class="form-group">
                                <label for="title" class="col-xs-2 control-label"><em>*</em>标题</label>
                                <div class="col-xs-5">
                                    <input type="text" class="form-control" id="txttitle" placeholder="标题" name="txttitle">
                                     <small class="help-block" id="titlemessage"></small>
                                </div>
                            </div>
                            <div class="form-group" id="uploaderpic">
                                <label for="title" class="col-xs-2 control-label">图标</label>
                                <div class="col-xs-6">                                
                                <div id="uploader_preview" class="preview" style="" type="1"></div><br />
                                    <a class="js-open-articles mt5"  id="Picture" href="javascript:;" type="1">选择图片</a><small class="help-block">建议尺寸：60 * 60像素，文件大小不超过10K</small>
                                    </div>
                            </div>
                            
                               <div class="form-group">
                                <label for="content" class="col-xs-2 control-label"><em id="linkem">*</em>链接地址</label>
                                <div class="col-xs-5">
                                 
                                    <textarea id="txtshowContent" cols="30" rows="3" style="width: 400px;" disabled class="form-control" placeholder=""></textarea>
                                    <textarea id="txtContent" cols="30" rows="3" style="width: 400px; display: none" onchange="contentchange();" class="form-control" placeholder="回复内容"></textarea>
                                     </div>
                            </div>
                        </div>
                    </div>

                    <div class="modal-footer">
                         <button type="button" class="btn btn-primary" onclick="submitaddandedit();">确定</button>
                        <button type="button" class="btn btn-default" data-dismiss="modal">关闭</button>
                       
                    </div>
                    </from>
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
    </form>
</asp:Content>
