<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminNew.Master" AutoEventWireup="true" CodeBehind="CustomDistributorEdit.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Member.CustomDistributorEdit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="page-header">
        <h2>会员分组设置</h2>
    </div>
    <form id="thisForm" runat="server" class="form-horizontal">

        <div class="form-group">
            <label for="inputEmail3" class="col-xs-2 control-label"><span style="color: red">*</span>分组名称：</label>
            <div class="col-xs-3">
                <asp:TextBox ID="txtGroupName" CssClass="form-control" runat="server" />
            </div>
        </div>
        <div class="form-group">
            <label for="inputPassword3" class="col-xs-2 control-label">分组介绍：</label>
            <div class="col-xs-4">
                <asp:TextBox ID="txtShopIntroduction" CssClass="form-control" Width="300px" Height="100px" TextMode="MultiLine" runat="server" />
                <%--<small class="help-block">微信分享店铺给好友时会显示这里的文案</small>--%>
            </div>
        </div>
        <div class="form-group">
            <label for="inputPassword3" class="col-xs-2 control-label">选择会员：</label>
            <div class="col-xs-3">
                <button type="button" class="btn btn-default">选择</button>
            </div>
        </div>
        <div class="footer-btn navbar-fixed-bottom">
            <button type="button" class="btn btn-success">保存</button>
        </div>
    </form>
</asp:Content>
