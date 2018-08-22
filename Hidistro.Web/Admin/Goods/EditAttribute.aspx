<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminNew.Master" AutoEventWireup="true" CodeBehind="EditAttribute.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Goods.EditAttribute" %>
<%@ Register TagPrefix="cc1" TagName="AttributeView" Src="~/Admin/Goods/ascx/AttributeView.ascx" %>
<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <form runat="server">
     <div class="page-header">
            <h2>添加新的商品类型</h2>
            <small>商品类型是一系属性的组合，可以用来向顾客展示某些商品具有的特有的属性，一个商品类型下可添加多种属性.一种是供客户查看的扩展属性,如图书类型商品的作者，出版社等，一种是供客户可选的规格,如服装类型商品的颜色、尺码。</small>
        </div>
        <div id="mytabl">
            <!-- Nav tabs -->
            <ul class="nav nav-tabs">
                <li ><a href='<%= Globals.GetAdminAbsolutePath("/Goods/EditProductType.aspx?typeId=" + Page.Request.QueryString["typeId"])%>'>基本设置</a></li>
                <li class="active"><a href='<%= Globals.GetAdminAbsolutePath("/Goods/EditAttribute.aspx?typeId=" + Page.Request.QueryString["typeId"])%>'>扩展属性</a></li>
                <li><a href='<%= Globals.GetAdminAbsolutePath("/Goods/EditSpecification.aspx?typeId=" + Page.Request.QueryString["typeId"])%>'>规 格</a></li>

            </ul>
            <!-- Tab panes -->
            <div class="tab-content">
                <div class="tab-pane active">
                      <cc1:AttributeView runat="server" ID="attributeView" />
                    </div>
                </div>
            </div>
 </form>
</asp:Content>
 
