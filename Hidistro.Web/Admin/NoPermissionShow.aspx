<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/SimplePage.Master" AutoEventWireup="true" CodeBehind="NoPermissionShow.aspx.cs" Inherits="Hidistro.UI.Web.Admin.NoPermissionShow" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <header class="ui-header navbar-fixed-top">
        <div class="ui-header-inner clearfix">
            <h1 class="ui-header-logo">
                <a href="/admin/" title="管理后台">
                    <span class="version"><i></i></span>
                </a>
            </h1>
            <nav class="ui-header-nav">
                <asp:Literal runat="server" ID="topMenu"></asp:Literal>
            </nav>
            <div class="ui-header-user ">
                <div class="customer-wrap">
                </div>
                <div class="dropdown hover dropdown-right">
                    <div class="dropdown hover dropdown-right">
                        <a href="javascript:;" class="dropdown-toggle" data-toggle="dropdown">
                            <span class="txt">
                                <span class="ellipsis team_name">
                                    <asp:Literal runat="server" ID="litSitename"></asp:Literal></span><span class="dash"> - </span><span>
                                        <asp:Literal runat="server" ID="litUsername"></asp:Literal></span>
                            </span>
                            <i class="caret"></i>
                        </a>
                        <ul class="dropdown-menu">
                            <li><a href="/admin/Settings/EditManager.aspx?UserID=<%=CurrentUserId %>"><span class="glyphicon glyphicon-user"></span>个人信息</a></li>
                            <li><a href="/admin/Settings/EditManagerPassword.aspx?UserID=<%=CurrentUserId %>"><span class="glyphicon glyphicon-list-alt"></span>修改密码</a></li>
                            <li class="divide"></li>
                            <li><a href="/Admin/LoginExit.aspx"><span class="glyphicon glyphicon-indent-right"></span>退出</a></li>
                        </ul>
                    </div>
                </div>
            </div>
        </div>
    </header>
    <div style="" class="container">
        <div style="" class="app">
            <div style="" class="app-inner clearfix">
                <div style="padding-top:150px;text-align:center;font-size:16px;">
                    <img src="../images/noadmin.png" style="width:190px" />
                    <p style="margin-top: 20px; font-size: 14px;">您没有权限访问该页面，请点击<a href="/admin/" style="color:#07D">返回首页</a></p>
                </div>
            </div>
        </div>
        <aside class="ui-sidebar sidebar" style="min-height: 533px;">
            <nav class="well" id="menu_left">
                <asp:Literal runat="server" ID="leftMenu"></asp:Literal>
            </nav>
        </aside>
    </div>
    <div class="mt20"></div>

    <div class="edui-default" style="position: fixed; left: 0px; top: 0px; width: 0px; height: 0px;" id="edui_fixedlayer">
        <div style="display: none;" id="edui104" class="edui-popup  edui-bubble edui-default" onmousedown="return false;">
            <div id="edui104_body" class="edui-popup-body edui-default">
                <iframe class="edui-default" style="position: absolute; z-index: -1; left: 0; top: 0; background-color: transparent;" src="about:blank" frameborder="0" height="100%" width="100%"></iframe>
                <div class="edui-shadow edui-default"></div>
                <div id="edui104_content" class="edui-popup-content edui-default"></div>
            </div>
        </div>
    </div>
</asp:Content>
