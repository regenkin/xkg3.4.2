<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminNew.Master" AutoEventWireup="true" CodeBehind="EditProductType.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Goods.EditProductType" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" language="javascript">
        $(function () {
            $('#aspnetForm').formvalidation({

                'ctl00$ContentPlaceHolder1$txtTypeName': {
                    validators: {
                        notEmpty: {
                            message: "类型名称不能为空，在30个字符以内"
                        },
                        stringLength: {
                            min: 1,
                            max: 30,
                            message: '至少输入1个且不能超过30个字符'
                        }
                    }
                },

                'ctl00$ContentPlaceHolder1$txtRemark': {
                    validators: {

                        stringLength: {
                            min: 0,
                            max: 100,
                            message: '备注的长度限制在0-100个字符之间'
                        }
                    }
                }
            })
        })
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form runat="server">
        <div class="page-header">
            <h2>编辑商品类型</h2>
            <small>商品类型是一系属性的组合，可以用来向顾客展示某些商品具有的特有的属性，一个商品类型下可添加多种属性.一种是供客户查看的扩展属性,如图书类型商品的作者，出版社等，一种是供客户可选的规格,如服装类型商品的颜色、尺码。</small>
        </div>
        <div id="mytabl">
            <!-- Nav tabs -->
            <ul class="nav nav-tabs">
                <li class="active"><a href="#home">基本设置</a></li>
                <li><a href='<%= Globals.GetAdminAbsolutePath("/Goods/EditAttribute.aspx?typeId=" + Page.Request.QueryString["typeId"])%>'>扩展属性</a></li>
                <li><a href='<%= Globals.GetAdminAbsolutePath("/Goods/EditSpecification.aspx?typeId=" + Page.Request.QueryString["typeId"])%>'>规 格</a></li>

            </ul>
            <!-- Tab panes -->
            <div class="tab-content">
                <div class="tab-pane active">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <label for="inputEmail3" class="col-xs-2 control-label"><em>*</em>商品类型名称：</label>
                            <div class="col-xs-3">
                                <asp:TextBox ID="txtTypeName" CssClass="form-control" runat="server" />
                                <small>类型名称不能为空，在30个字符以内</small>
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="inputEmail3" class="col-xs-2 control-label">关联品牌：</label>
                            <div class="col-xs-6">
                                <Hi:BrandCategoriesCheckBoxList runat="server" ID="chlistBrand" />
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="inputEmail3" class="col-xs-2 control-label">备注：</label>
                            <div class="col-xs-3">
                                <asp:TextBox ID="txtRemark" TextMode="MultiLine" CssClass="form-control" Width="320" Height="90" runat="server"></asp:TextBox>
                                <small>备注的长度限制在0-100个字符之间</small>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-xs-offset-2 col-xs-10">

                                <asp:Button ID="btnEditProductType" runat="server" Text="保 存" CssClass="btn btn-primary" />
                            </div>
                        </div>
                    </div>
                </div>

            </div>
        </div>

    </form>
</asp:Content>

