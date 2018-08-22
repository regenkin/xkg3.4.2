<%@ Page Language="C#" MasterPageFile="~/Admin/AdminNew.Master" AutoEventWireup="true" CodeBehind="BrandCategories.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Goods.BrandCategories" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>

<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content2" ContentPlaceHolderID="head" Runat="Server">
        
      <style>
        .table_title{background:#f2f2f2}
       table td,th{text-align:center}
    </style>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <form runat="server">
     <div class="page-header">
            <h2>品牌管理</h2>
            <small>管理商品所属的各个品牌，如果在上架商品时给商品指定了品牌分类，则商品可以按品牌分类浏览</small>
        </div>

    
        <div class="form-inline">
            <div class="form-group">
                <label for="exampleInputName2">品牌名称</label>
                <asp:TextBox ID="txtSearchText" runat="server" CssClass="form-control resetSize" placeholder="" />
                <asp:Button ID="btnSearchButton" runat="server" Text="查询" class="btn resetSize btn-primary" />
              
            </div>
        </div>
        <div class="form-horizontal" style="margin-top: 10px; margin-bottom:10px;">
                        <a href="AddBrandCategory.aspx" class="btn btn-success">添加新商品品牌</a>

              <asp:LinkButton ID="btnorder" class="btn btn-primary" runat="server">批量保存排序</asp:LinkButton> 
                    </div> 
 <UI:Grid ID="grdBrandCategriesList" runat="server" AutoGenerateColumns="false" ShowHeader="true" CssClass="table table-hover mar table-bordered"  DataKeyNames="BrandId" GridLines="None" Width="100%" HeaderStyle-CssClass="table_title">
              <Columns>
                  <asp:TemplateField HeaderText="品牌Logo" ItemStyle-Width="14%" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                            <a id="A1" href='<%# Eval("CompanyUrl") %>' runat="server" target="_blank"><Hi:HiImage ID="HiImage1" runat="server" DataField="Logo"  Width="100" Height="50" CssClass="Img100_30"/></a>
                        </ItemTemplate>
                  </asp:TemplateField>
                  <asp:TemplateField HeaderText="品牌名称" ItemStyle-Width="14%" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                            <asp:Literal ID="litName" runat="server" Text='<%# Bind("BrandName") %>'></asp:Literal>
                        </ItemTemplate>
                  </asp:TemplateField>
               
                  <UI:SortImageColumn Visible="false" HeaderText="排序" ReadOnly="true" ItemStyle-Width="14%" HeaderStyle-CssClass="td_right td_left"/>
                    <asp:TemplateField HeaderText="显示顺序" ItemStyle-Width="70px"  HeaderStyle-CssClass="td_right td_left">
                   <ItemTemplate>
                      <input id="Text1" type="text" runat="server" value='<%# Eval("DisplaySequence") %>' style="width:60px;" onkeyup="this.value=this.value.replace(/\D/g,'')" onafterpaste="this.value=this.value.replace(/\D/g,'')" />
                   </ItemTemplate>
                   </asp:TemplateField>
                  <asp:TemplateField HeaderText="操作" HeaderStyle-Width="20%" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                             <span class="submit_bianji"><asp:HyperLink ID="lkEdit" runat="server" Text="编辑" NavigateUrl='<%# "EditBrandCategory.aspx?brandId="+Eval("BrandId")%>' /></span> 
                             <span class="submit_shanchu"><%--<Hi:ImageLinkButton runat="server" ID="lkbtnDelete" CommandName="Delete" IsShow="true" Text="删除" />--%>
                                             <asp:Button ID="lkbtnDelete" CssClass="btnLink"  runat="server" Text="删除" CommandName="Delete"  OnClientClick="return HiConform('<strong>确定要删除选择的商品品牌吗？</strong><p>删除商品品牌不可恢复！</p>',this)" />
                             </span>
                        </ItemTemplate>
                  </asp:TemplateField>
              </Columns>
    
            </UI:Grid></form>
</asp:Content>

