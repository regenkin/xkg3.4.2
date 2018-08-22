<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminNew.Master" AutoEventWireup="true" CodeBehind="EditTemplateId.aspx.cs" Inherits="Hidistro.UI.Web.Admin.WeiXin.EditTemplateId" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="page-header">
        <h2>编辑微信模板消息Id</h2>
        <small>模板为Id是在微信公众平台添加对应的模板后随机生成的一串字符，系统根据此Id来确定所使用的模板。</small>
    </div>
    <form runat="server" class="form-horizontal">
        <div class="titileBorderBox">
            <div class="contentBox">
                <div class="form-inline">
                    <div class="form-group">
                        <label>
                            模板Id:
                        <asp:TextBox ID="txtTemplateId" runat="server" ClientIDMode="Static" Width="300px" CssClass="form-control inputw200"></asp:TextBox></label>
                    </div>
                    <a href="weixinSettings.html#step5" target="_blank" class="">点击查看帮助</a>
                </div>
            </div>
        </div>
        <div class="form-group">
            <div class="col-xs-10 col-xs-offset-2">
                <asp:Button ID="btnSaveEmailTemplet" runat="server" Text="保存" CssClass="btn btn-success float inputw100" />
                <input value="返回" onclick="location.href = 'messagetemplets.aspx'" class="btn btn-primary inputw100" type="button">
            </div>
        </div>
    </form>
</asp:Content>
