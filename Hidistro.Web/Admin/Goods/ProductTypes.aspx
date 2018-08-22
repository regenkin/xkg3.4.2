<%@ Page Language="C#" AutoEventWireup="true" Inherits="Hidistro.UI.Web.Admin.Goods.ProductTypes" MasterPageFile="~/Admin/AdminNew.Master" MaintainScrollPositionOnPostback="true" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    <script>
 
    </script>
      <style>
        .table_title{background:#f2f2f2}
       table td,th{text-align:center}
    </style>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form runat="server">
        <div class="page-header">
            <h2>商品类型</h2>
            <small>商品类型是一系属性的组合，可以用来向顾客展示某些商品具有的特有的属性，比如服装类型的颜色，尺码；图书类型的作者，出版社等</small>
        </div>

        <div class="form-inline">
            <div class="form-group">
                <label for="exampleInputName2">商品类型名称</label>
                <asp:TextBox ID="txtSearchText" runat="server" CssClass="form-control resetSize" placeholder="" />
                <asp:Button ID="btnSearchButton" runat="server" Text="查询" class="btn resetSize btn-primary" />
            </div>
        </div>
         
            
              <div class="form-horizontal" style="margin-top: 10px; margin-bottom:10px;">
                        <a href="AddProductType.aspx" class="btn btn-success">添加新商品类型</a>
                    </div> 
         <div>
             <div class="datalist mt5">
        <UI:Grid ID="grdProductTypes" runat="server" ShowHeader="true" AutoGenerateColumns="false" DataKeyNames="TypeId"  HeaderStyle-CssClass="table_title" CssClass="table table-hover mar table-bordered" GridLines="None" Width="100%"  >
            <Columns>
                <asp:TemplateField HeaderText="商品类型名称" ItemStyle-Width="20%" HeaderStyle-CssClass="td_right td_left">
                    <ItemTemplate>
                        <asp:Label ID="lblTypeName" runat="server" Text='<%# Eval("TypeName") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtTypeName" runat="server" Text='<%# Eval("TypeName") %>'></asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateField>  
                <asp:TemplateField HeaderText="关联品牌" ItemStyle-Width="30%" HeaderStyle-CssClass="td_right td_left">
                    <ItemTemplate>
                        <asp:Label ID="lbbrand" runat="server"  ></asp:Label>
                    </ItemTemplate>
                    
                </asp:TemplateField>
                <asp:TemplateField HeaderText="备注" ItemStyle-Width="20%" HeaderStyle-CssClass="td_right td_left">
                    <ItemTemplate>
                        <asp:Label ID="lblRemark" runat="server" Text='<%# Eval("Remark") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtRemark" runat="server" Text='<%# Eval("Remark") %>'></asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="操作" HeaderStyle-CssClass="td_right td_left">
                    <ItemStyle CssClass="spanD spanN" />
                    <ItemTemplate>
                        <span class="submit_bianji">
                            <asp:HyperLink ID="lkbViewAttribute" runat="server" Text="编辑" NavigateUrl='<%# "EditProductType.aspx?TypeId="+Eval("TypeId")%>'></asp:HyperLink></span>
                        <span class="submit_shanchu">
                          <%--  <Hi:ImageLinkButton ID="lkbDelete" IsShow="true" runat="server" CommandName="Delete" Text="删除" />--%>
                            <asp:Button ID="lkbDelete" CssClass="btnLink"  runat="server" Text="删除" CommandName="Delete"  OnClientClick="return HiConform('<strong>确定要删除选择的商品类型吗？</strong><p>删除商品类型不可恢复！</p>',this)" />
                        </span>
                    </ItemTemplate>
                </asp:TemplateField>

            </Columns>
        </UI:Grid>
                 </div>
             </div>
        <div class="page">
            <div class="bottomPageNumber clearfix">
                <div class="pageNumber">

                    <div class="pagination">
                        <UI:Pager runat="server" ShowTotalPages="true" ID="pager" />
                    </div>
                </div>
            </div>

        </div>
    </form>
</asp:Content>


