<%@ Page Language="C#" MasterPageFile="~/Admin/SimplePage.Master" AutoEventWireup="true" CodeBehind="EditBaseInfo.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Goods.EditBaseInfo" Title="无标题页" %>

<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form runat="server" style="width:650px;">
        <div class="page-header">
            <h2>批量修改商品基本信息</h2>
            <small>手工输入您想要修改的信息后在页底处保存设置即可</small>
        </div>
        <div class="set-switch">
            <div class="form-inline mb10">
                <div class="form-group mr20">
                    <label for="sellshop1">　商品名称： 增加前缀：</label>
                    <asp:TextBox ID="txtPrefix" runat="server" Width="80px" MaxLength="20" CssClass="form-control mr5" />增加后缀     
                                  <asp:TextBox ID="txtSuffix" runat="server" Width="80px" MaxLength="20" CssClass="form-control" />
                </div>

                <div class="form-group">
                    <label for="sellshop3"></label>
                    <asp:Button ID="btnAddOK" runat="server" Text="确定" CssClass="btn btn-success" />
                </div>
            </div>
            <div class="form-inline">
                <div class="form-group mr20">
                    <label for="sellshop4" style="padding-left: 74px;">查找字符串：</label>
                    <asp:TextBox ID="txtOleWord" runat="server" Width="80px" CssClass="form-control mr5" />替换成为<asp:TextBox ID="txtNewWord" runat="server" Width="80px" CssClass="form-control ml5" />
                </div>
                
                <div class="form-group">
                    <label for="sellshop3"></label>
                 <asp:Button ID="btnReplaceOK" runat="server" Text="查询" CssClass="btn btn-success" />
                </div>
            </div>
            
        </div>
        <UI:Grid ID="grdSelectedProducts" DataKeyNames="ProductId" runat="server" ShowHeader="true" AutoGenerateColumns="false" HeaderStyle-CssClass="table_title" GridLines="None" CssClass="table table-bordered table-hover" Width="100%">
            <Columns>
                <asp:TemplateField HeaderText="商品图片" ItemStyle-Width="10%" HeaderStyle-CssClass="td_right td_left">
                    <ItemTemplate>
                        <a href='<%#"/ProductDetails.aspx?productId="+Eval("ProductId")%>' target="_blank">
                            <Hi:ListImage ID="ListImage1" runat="server" DataField="ThumbnailUrl40" />
                        </a>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="商品名称" ItemStyle-Width="15%" HeaderStyle-CssClass="td_right td_left">
                    <ItemTemplate>
                        <asp:TextBox ID="txtProductName" runat="server" Width="280px" Text='<%#Eval("ProductName") %>'></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="商家编码" ItemStyle-Width="10%" HeaderStyle-CssClass="td_right td_left">
                    <ItemTemplate>
                        <asp:TextBox ID="txtProductCode" runat="server" Width="80px" Text='<%#Eval("ProductCode") %>'></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="原价" HeaderStyle-Width="10%" HeaderStyle-CssClass="td_right td_left">
                    <ItemTemplate>
                        <asp:TextBox ID="txtMarketPrice" runat="server" Width="80px" Text='<%#Eval("MarketPrice", "{0:F2}") %>'></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </UI:Grid>

        <div class="form-group">
            <div class="col-xs-offset-4 col-xs-10">
                   <button type="button" class="btn btn-default"  onclick="CloseModal();">关闭</button>&nbsp;&nbsp;&nbsp;
                <asp:Button ID="btnSaveInfo" runat="server"   Text="保存设置" CssClass="btn btn-success" />

            </div>
        </div>

    </form>
    <script>

        function CloseModal() {
            window.parent.closeModal("divEditBaseInfo");
        }


        //return false;
    </script>
</asp:Content>
