<%@ Page Language="C#" MasterPageFile="~/Admin/AdminNew.Master" AutoEventWireup="true" CodeBehind="ImportFromTB.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Goods.ImportFromTB" Title="无标题页" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .table_title {
            background: #f2f2f2;
        }

        input[type="checkbox"], input[type="radio"] {
            margin: 0;
        }

        .set-switch {
            padding: 6px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form runat="server">
        <div class="page-header">
            <h2>数据包导入</h2>
            <small></small>
        </div>
        <div id="mytabl">
            <!-- Nav tabs -->
            <ul class="nav nav-tabs">
                <li class="active"><a href="#home">从淘宝数据包导入</a></li>
                <li><a href="ImportFromPP.aspx">从拍拍数据包导入</a></li>

            </ul>
            <!-- Tab panes -->
            <div class="tab-content">
                <div class="tab-pane active">
                    <div class="set-switch">
                        <strong>数据包信息</strong>
                        <p>淘宝数据包的来源：1.系统本身导出的淘宝数据包；2.淘宝或商品助理导出的文件，把CSV文件和图片文件夹都命名为products然后选中两个打成zip的压缩包</p>
                    </div>
                    <div class="form-horizontal">
                        <div class="form-group">
                            <label for="" class="col-xs-2 control-label"><em>*</em>导入插件版本</label>
                            <div class="col-xs-3">
                                <asp:DropDownList runat="server" ID="dropImportVersions" Width="200" CssClass="form-control"></asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group" style="padding-left: 92px;">
                            导入之前需要先将数据包文件上传到服务器上：
                        </div>
                        <div class="form-group">
                            <label for="" class="col-xs-2 control-label">导入的数据包文件</label>
                            <div class="col-xs-4">
                                <asp:FileUpload runat="server" ID="fileUploader" CssClass="form-control" />
                            </div>
                            <asp:Button CssClass="btn btn-primary" runat="server" ID="btnUpload" Text="上传" />
                        </div>
                        <div style="margin-left: 176px">
                            <small class="help-block">请上传 *.zip 格式的数据包，上传数据包须小于40M，否则可能上传失败，<br />
                                您还可以使用FTP工具先将数据包上传到网站的/storage/data/taobao目录以后，再重新打开此页面操作。</small>
                        </div>
                        <div class="form-group">
                            <label for="" class="col-xs-2 control-label"><em>*</em>导入的数据包文件</label>
                            <div class="col-xs-4">
                                <asp:DropDownList runat="server" ID="dropFiles" Width="320" CssClass="form-control"></asp:DropDownList>
                                <small class="help-block pt10">如果上面的下拉框中没有您要导入的数据包文件,请先上传。</small>
                            </div>
                        </div>
                    </div>
                    <div class="set-switch">
                        <strong>导入选项</strong>
                    </div>
                    <div class="form-horizontal">
                        <div class="form-group">
                            <label for="" class="col-xs-2 control-label"><em>*</em>商品分类</label>
                            <div class="col-xs-3">
                                <Hi:ProductCategoriesDropDownList ID="dropCategories" CssClass="form-control" runat="server" NullToDisplay="-请选择商品分类-" Width="200" />
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="" class="col-xs-2 control-label"><em>*</em>商品品牌</label>
                            <div class="col-xs-3">
                                <Hi:BrandCategoriesDropDownList runat="server" ID="dropBrandList" NullToDisplay="--请选择品牌--" CssClass="form-control" Width="200"></Hi:BrandCategoriesDropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="" class="col-xs-2 control-label">商品导入状态</label>
                            <div class="col-xs-4">

                                <div class="checkbox setradio">
                                    <label>
                                        <asp:RadioButton runat="server" ID="radOnSales" GroupName="SaleStatus" Checked="true"></asp:RadioButton>
                                        出售中
                                    </label>
                                    <label>
                                        <asp:RadioButton runat="server" ID="radUnSales" GroupName="SaleStatus" Text="下架区" Visible="false"></asp:RadioButton>

                                    </label>
                                    <label>
                                        <asp:RadioButton runat="server" ID="radInStock" GroupName="SaleStatus"></asp:RadioButton>
                                        仓库中
                                    </label>
                                </div>

                            </div>
                        </div>
                    </div>

                    <div class="form-group">
                        <div class="col-xs-offset-3 col-xs-10" style="margin-bottom: 20px;">

                            <asp:Button ID="btnImport" runat="server" OnClientClick="return doImport();" CssClass="btn inputw100 btn-primary" Text="导 入" />
                        </div>
                    </div>
                </div>
                <div class="tab-pane"></div>

            </div>
        </div>


    </form>
</asp:Content>

