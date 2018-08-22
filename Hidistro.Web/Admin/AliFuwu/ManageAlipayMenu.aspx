<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminNew.Master" AutoEventWireup="true" CodeBehind="ManageAlipayMenu.aspx.cs" Inherits="Hidistro.UI.Web.Admin.AliFuwu.ManageAlipayMenu" %>


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
        }
    </style>

    <script>

        $(function () {
            CreateDropdown($("#txtContent"), $("#myModal  .form-horizontal").eq(0), { createType: 1, showTitle: true, txtContinuity: false, reWriteSpan: true, style: "margin-left: 35px;" });

            $("#dropdow-menu-link  li[data-val='20']").remove();
            $("#dropdow-menu-link .dropdown-toggle").eq(0).css({ "padding-left": "15px" });


            loadmenu();
        })
        var setmenuid = "";
        function onemenu(menuid) {
            setmenuid = menuid;
            bishi = "1";
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
            $.getJSON("../../API/AlipayFWMenuProcess.ashx?action=gettopmenus").done(function (d) {
                if (d.status == "0") {
                    showmenu(d);
                    var menuhtml = "";
                    $('#ulmenu li').remove();
                    $("#content").html("");
                    $('#tabpane').remove();
                    var b = 0;
                    var menuid = 0;
                    if (d.data.length == 3) {
                        $("#addmenu").css("display", "none");
                    }
                    else {
                        $("#addmenu").css("display", "");
                    }
                    for (var i in d.data) {
                        var menudata = d.data[i];
                        var active = "";
                        if (setmenuid == "menu_" + menudata.menuid)
                            active = "class=\"active\"";
                        if (i == 0) {
                            if (i == d.data.length - 1) {
                                active = "class=\"active\"";
                                b = 1;
                            }

                            if (setmenuid == "" && bishi != "0") {
                                active = "class=\"active\"";
                                b = 1;
                            }
                            menuhtml = "  <li " + active + " id=\"" + menudata.menuid + "\"><a  id=\"menu_" + menudata.menuid + "\" onclick=\"onemenu('menu_" + menudata.menuid + "')\">" + menudata.name + "<i class=\"glyphicon glyphicon-remove\"   onclick=\"delmenu('" + menudata.menuid + "','0')\"></i></a> </li>";

                        }
                        else {
                            b = 0;

                            if (bishi == "0" && (d.data.length - 1) == i) {
                                menuhtml = "  <li class=\"active\" id=\"" + menudata.menuid + "\"><a id=\"menu_" + menudata.menuid + "\"  onclick=\"onemenu('menu_" + menudata.menuid + "')\">" + menudata.name + "<i class=\"glyphicon glyphicon-remove\"   onclick=\"delmenu('" + menudata.menuid + "','0')\"></i></a></li>";
                                menuid = menudata.menuid;
                                $("#" + setmenuid.split('_')[1]).removeClass('active');
                            }
                            else
                                menuhtml = "  <li " + active + " id=\"" + menudata.menuid + "\"><a id=\"menu_" + menudata.menuid + "\"  onclick=\"onemenu('menu_" + menudata.menuid + "')\">" + menudata.name + "<i class=\"glyphicon glyphicon-remove\"   onclick=\"delmenu('" + menudata.menuid + "','0')\"></i></a></li>";
                        }
                        var childmenuhtml = "";
                        var js = 0;
                        for (var j in menudata.childdata) {
                            //&nbsp;|<span style=\"cursor: pointer;\" onclick=\"delmenu('" + childmenudata.menuid + "','1')\">删除</span>
                            var childmenudata = menudata.childdata[j];
                            childmenuhtml += "<div class='list'><span><span class=\"subtitlespan\" id=\"spantitlename" + childmenudata.menuid + "\">" + childmenudata.name + "</span><span style=\"display:none;\" id=\"spanedittile" + childmenudata.menuid + "\"><input type=\"text\" onkeypress=\"EnterPress(event,'" + childmenudata.menuid + "');\" class=\"form-control\" id=\"txtedittitle" + childmenudata.menuid + "\"  maxlenth=\"14\" onkeyup=\"$(this).val(getStrbylen($(this).val(), 14))\" onpaste=\"$(this).val(getStrbylen($(this).val(), 14))\" style=\"width:120px;\" value=\"" + childmenudata.name + "\"></span></span><span  class='edit' ><span id=\"btnedit" + childmenudata.menuid + "\" style=\"cursor: pointer;\" onclick=\"edittitle('" + childmenudata.menuid + "')\" title=\"修改菜单名称\">编辑</span><span style=\"display:none;\" id=\"submitedit" + childmenudata.menuid + "\"><input id=\"saveedit\" type=\"button\" value=\"保存\" onclick=\"updatename('" + childmenudata.menuid + "')\" class=\"btn btn-primary\"/>&nbsp;<input id=\"conseledit\" class=\"btn btn-danger\" onclick=\"conseleditwin('" + childmenudata.menuid + "')\" type=\"button\" value=\"取消\" /></span>&nbsp;|&nbsp;<span style=\"cursor: pointer;color:blue\" onclick=\"addandeditmenu('1','" + childmenudata.menuid + "','" + childmenudata.parentmenuid + "','two')\">链接</span>&nbsp;|&nbsp;<span style=\"cursor: pointer;\" onclick=\"delmenu('" + childmenudata.menuid + "','1')\">删除</span></span></div>";

                        }
                        $("#addmenu").before(menuhtml);//添加父菜单的Tab选项卡
                        var tabcontent = $($("#tabcontent").html());
                        tabcontent.find('#fltitle').text(menudata.name);
                        tabcontent.find('#EditMenu').attr("onclick", "addandeditmenu('1','" + menudata.menuid + "','','one')");

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
            if (byteLength($.trim(name)) > 14) {
                HiTipsShow("二级菜单标题不能大于14个字符！", 'warning');
                return;
            }
            $.getJSON("../../API/AlipayFWMenuProcess.ashx?action=updatename" + url).done(function (d) {
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
            if (id == "") {
                $("#txttitle").val("");
                $("#txtContent").val("");
                $("#txtshowContent").val("");
                $("#spLinkTitle").html("选择链接");
            }
            editid = id;
            parentid = parentmenuid
            var targetUrl = "editmenu.aspx";
            if (type == 0) {
                $("#modaltitle").text('添加菜单');
                if (parentmenuid > 0) {
                    targetUrl += "?pid=" + parentmenuid;
                }
            }
            else {
                $("#modaltitle").text('修改菜单');
                targetUrl += "?MenuId=" + id;
                var url = "&MenuId=" + id;
                $.getJSON("../../API/AlipayFWMenuProcess.ashx?action=getmenu" + url).done(function (d) {
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

            $("#ifmMobile").attr("src", targetUrl);
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
                    HiTipsShow("一级菜单标题最多4个字！", 'warning');
                    return;
                }
            }
            else {
                if ($.trim($("#txttitle").val()).length > 7) {
                    HiTipsShow("二级菜单标题不能大于7个字！", 'warning');
                    return;
                }
                if ($.trim($("#txtContent").val()) == "") {
                    HiTipsShow("链接内容不能为空！", 'warning');
                    return;
                }
            }
            //if (mestype != 0)
            Type = 'view';
            var urlContent = $("#txtContent").val();
            if (!isSimpleURL(urlContent)) {
                HiTipsShow("请输入正确的链接地址！", 'warning');
                return;
            }
            if (andedit == 0) {//添加一级和二级菜单
                var pic = "";
                if ($.trim($("#uploader1_preview").html()) != "") {
                    pic = $("#uploader1_image").attr('src');
                }
                var url = "&ParentMenuId=" + parentid + "&Name=" +encodeURIComponent($("#txttitle").val()) + "&Content=" + encodeURIComponent($("#txtContent").val()) + "&Type=" + Type + "&ShopMenuPic=" + encodeURIComponent(pic);

                $.getJSON("../../API/AlipayFWMenuProcess.ashx?action=addmenu" + url).done(function (d) {
                    if (d.status == "0") {
                        HiTipsShow("添加成功！", 'success');
                        loadmenu();
                        $('#myModal').modal('hide')
                    }
                    else {
                        if (d.status == "1") {
                            HiTipsShow("添加菜单失败！", 'fail');
                        }
                        else {
                            HiTipsShow("一级菜单最多只能添加3个,二级菜单最多只能添加5个！", 'fail');
                        }
                    }
                });
            }
            if (andedit == 1)//修改一级和二级菜单
            {
                var pic = "";
                if ($.trim($("#uploader1_preview").html()) != "") {
                    pic = $("#uploader1_image").attr('src');
                }
                var url = "&ParentMenuId=" + parentid + "&MenuId=" + editid + "&Name=" + encodeURIComponent($("#txttitle").val()) + "&Content=" + encodeURIComponent($("#txtContent").val()) + "&Type=" + Type + "&ShopMenuPic=" + encodeURIComponent(pic);
                $.getJSON("../../API/AlipayFWMenuProcess.ashx?action=editmenu" + url).done(function (d) {
                    if (d.status == "0") {
                        HiTipsShow("修改成功！", 'success');
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
        function contentchange() {
            $("#txtshowContent").val($("#txtContent").val());
        }
        function delmenu(id, type) {
            if (confirm("确定要删除该菜单吗？")) {
                var url = "&MenuId=" + id;
                $.getJSON("../../API/AlipayFWMenuProcess.ashx?action=delmenu" + url).done(function (d) {
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
     <div class="page-header">
        <h2>自定义服务窗菜单</h2>
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
                <script type="text/template" id="tabcontent">
                    <div id="panediv" class="tab-pane navgation">
                        <div class="tab-pane navgation" id="tabpane">
                            <p class="nav-one">一级菜单</p>
                            <div class="shop-index clearfix"><span class="fl" id="fltitle">店铺主页</span><span class="fr"><span style="cursor: pointer;" id="EditMenu">编辑</span>&nbsp;<span id="spanhid">|&nbsp;设置二级菜单以后，主链接已失效。</span></span></div>
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
            </div>
        </div>
        <br />
    
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
   <%--                             <textarea  cols="30" rows="3" style="width: 400px; " onchange="contentchange();"></textarea>--%>
                                <input type="text" id="txtContent" style="width:400px;"  class="form-control" placeholder="请输入链接或选择下面的链接" />
                            </div>
                        </div>

                        <div class="form-group" style="display:none;">
                            <div class="menu-list">
                                <ul class="clearfix">
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
     <form runat="server">
        <div style="text-align: center; margin-bottom: 20px;">
            <asp:Button ID="BtnSave" class="btn btn-primary" runat="server" Text="保存到服务窗" />
        </div>
    </form>
</asp:Content>
