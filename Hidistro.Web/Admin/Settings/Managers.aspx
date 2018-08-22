<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminNew.Master" AutoEventWireup="true" CodeBehind="Managers.aspx.cs" Inherits="Hidistro.UI.Web.Admin.settings.Managers" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>

<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
  <link rel="stylesheet" href="/utility/skins/blue.css" type="text/css" media="screen" />
    <Hi:Script ID="Script5" runat="server" Src="/utility/jquery.artDialog.js" />
       <Hi:Script ID="Script7" runat="server" Src="/utility/iframeTools.js" />
      <Hi:Script ID="Script6" runat="server" Src="/utility/Window.js" />
      <Hi:Script ID="Script4" runat="server" Src="/admin/js/jquery.formvalidation.js" />
      <Hi:Script ID="Script9" runat="server" Src="/utility/globals.js"/> <%--//调用到快速翻页函数--%>
  
    <style>
        .table_title{background:#f2f2f2}
       table td,th{text-align:center}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="page-header">
                    <h2>人员管理</h2>
    </div>

  
    
    <form runat="server" id="thisForm">

         <!--搜索-->
     
          <div  class="form-horizontal" style="margin:20px 0px">        
            <div class="form-inline">用户名：
            <asp:TextBox ID="txtSearchText" runat="server"  CssClass="form-control resetSize input-ssm start_datetime1"   />
            <Hi:RoleDropDownList ID="dropRolesList" runat="server"　CssClass="form-control resetSize"  SystemAdmin="true" NullToDisplay="全部"   />
            <asp:Button ID="btnSearchButton" runat="server" Text="查询" CssClass="btn resetSize btn-primary"/> 
            </div>                            
          </div>


         <!--分页功能-->
           <div class="select-page clearfix">
                    <div class="form-horizontal fl">
                        <a href="AddManager.aspx" class="btn btn-success">添加管理员</a>
                    </div>

                    <div  class="page fr">
                         <div class="pageNumber">
                        <div class="pagination" style="margin:0px">
                        <UI:Pager runat="server"  ShowTotalPages="true" ID="pager" />
                      </div>
                    </div>
                    </div>
        </div>


       <%-- class="table table-hover mar table-bordered" style="table-layout:fixed"--%>
         <!--数据绑定-->
        <UI:Grid ID="grdManager" runat="server" AutoGenerateColumns="False"  ShowHeader="true" DataKeyNames="UserId" GridLines="None"
                   HeaderStyle-CssClass="table_title" CssClass="table table-hover mar table-bordered"  SortOrderBy="CreateDate" SortOrder="ASC" Width="100%">
                    <Columns>
                       
                        <asp:TemplateField HeaderText="用户名" SortExpression="UserName" ItemStyle-Width="280px" HeaderStyle-CssClass="td_right td_left">
                            <ItemTemplate>
	                                <asp:Label ID="lblUserName" runat="server" Text='<%# Bind("UserName") %>'></asp:Label>      
                            </ItemTemplate>
                        </asp:TemplateField>
                         <asp:BoundField HeaderText="邮件地址" SortExpression="Email" DataField="Email" HeaderStyle-CssClass="td_right td_left" />
                        <asp:TemplateField HeaderText="创建时间" SortExpression="CreateDate" HeaderStyle-CssClass="td_right td_left">
                            <ItemTemplate>
                                <Hi:FormatedTimeLabel ID="lblCreateDate" Time='<%# Bind("CreateDate") %>' runat="server"></Hi:FormatedTimeLabel>
                            </ItemTemplate>
                        </asp:TemplateField>
                         <asp:TemplateField HeaderText="操作" ItemStyle-Width="20%" HeaderStyle-CssClass="td_left td_right_fff">
                         <ItemStyle CssClass="spanD spanN" />
                             <ItemTemplate>
                                 <span class="submit_bianji"><a href='<%# Globals.GetAdminAbsolutePath("/Settings/EditManager.aspx?UserId=" + Eval("UserId"))%> '>编辑</a></span>
		                         <span class="submit_shanchu">
                                     <%--<Hi:ImageLinkButton runat="server" ID="Delete" Text="删除" CommandName="Delete" IsShow="true" />--%>
                                      <asp:Button ID="Delete" runat="server" Text="删除"    Class="btnLink pad"  CommandName="Delete" IsShow="true"    OnClientClick="return HiConform('<strong>确定要删除选择的人员吗？</strong><p>删除后不可恢复！</p>',this)" ToolTip="" /> 
		                         </span>
                             </ItemTemplate>
                         </asp:TemplateField>  
                    </Columns>
                </UI:Grid>   

    </form>

</asp:Content>
