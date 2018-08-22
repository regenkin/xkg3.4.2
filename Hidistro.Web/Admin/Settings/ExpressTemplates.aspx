<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminNew.Master" AutoEventWireup="true" CodeBehind="ExpressTemplates.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Settings.ExpressTemplates" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>

<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>

        .table_title{background:#f2f2f2}
       table td,th{text-align:center}
       .td_left{text-align:left;padding-left:30px;width:20%}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <div class="page-header">
                    <h2>快递单模板</h2>
    </div>

    
         <!--分页功能-->
           <div class="select-page clearfix">
                    <div class="form-horizontal fl">
                        <a href="AddExpressTemplate.aspx" class="btn btn-success">新快递单模板</a>
                    </div>

                    <div  class="page fr">
                         <div class="pageNumber">
                        <div class="pagination" style="margin:0px">
                        <UI:Pager runat="server"  ShowTotalPages="true" ID="pager" />
                      </div>
                    </div>
                    </div>
        </div>

      <form runat="server" id="thisForm">
    <!--数据列表区域-->
	  <div class="datalist">
     <UI:Grid ID="grdExpressTemplates" runat="server" ShowHeader="true" AutoGenerateColumns="false" 
         DataKeyNames="ExpressId" HeaderStyle-CssClass="table_title" CssClass="table table-hover mar table-bordered"  GridLines="None" >
            <Columns>   
                <asp:TemplateField HeaderText="快递单模板名称" ItemStyle-CssClass="td_left"  >
                   <ItemTemplate>
                       <input name="CheckBoxGroup" type="checkbox" value='<%#Eval("ExpressId") %>'>
                      <asp:Literal id="lblExpressName" runat="server" Text='<%#Eval("ExpressName") %>'></asp:Literal>
                      <asp:Literal ID="litXmlFile" runat="server" Text='<%#Eval("XmlFile") %>' Visible="false"></asp:Literal>
                   </ItemTemplate>
                </asp:TemplateField>                   
                <asp:TemplateField HeaderText="物流公司"  HeaderStyle-CssClass="td_right">
                   <ItemTemplate>
                      <asp:Literal id="ENM" runat="server" Text='<%#Eval("ExpressName").ToString().Replace("快递","") %>'></asp:Literal>
                   </ItemTemplate>
                </asp:TemplateField>
                <UI:YesNoImageColumn DataField="IsUse" HeaderText="是否启用" HeaderStyle-Width="16%" ItemStyle-CssClass="btn-xs" HeaderStyle-CssClass="td_right" />   
                <UI:YesNoImageColumn DataField="IsDefault" HeaderText="默认模板" CommandName="IsDefault"  ItemStyle-CssClass="btn-xs" HeaderStyle-Width="16%" HeaderStyle-CssClass="td_right" />                                                                 
                <asp:TemplateField HeaderText="操作" ItemStyle-Width="25%" HeaderStyle-CssClass="td_right_fff">
                    <ItemTemplate>
                    <span><a class="btn btn-info btn-xs" href='<%# "EditExpressTemplate.aspx?ExpressId=" + Eval("ExpressId") + "&ExpressName=" + Globals.UrlEncode((string)Eval("ExpressName")) + "&XmlFile=" + Eval("XmlFile")%>'>编辑</a></span>  　
                    <span><Hi:ImageLinkButton ID="lkDelete" Text="删除" IsShow="true" CssClass="btn btn-danger btn-xs"  CommandName="DeleteRow" runat="server" /></span>   
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </UI:Grid>


           <!--数据列表底部功能区域-->
  
        <div class="select-page clearfix" style="margin-top:10px">
                    <div class="form-horizontal fl">
                        <div class="form-group mar forced">
                            <div class="checkbox">
                                <label><input type="checkbox" name="selall"   onclick="javascript: SelectAllNew(this);">全选</label>
                                &nbsp;&nbsp;&nbsp;&nbsp;
                                <Hi:ImageLinkButton ID="lkbDeleteCheck" class="btn resetSize btn-danger" runat="server" Text="批量删除" IsShow="true"/>
                            </div>
                        </div>
                    </div>
                    <div  class="page fr">
                         <div class="pageNumber">
                        <div class="pagination" style="margin:0px">
                        <UI:Pager runat="server" ShowTotalPages="true" ID="pager1" />
                       </div>
                      </div>
                    </div>
                </div>

      <div class="blank5 clearfix"></div>
	  </div>

    </form>
    <script>

        function SelectAllNew(obj) {
            $("[name=CheckBoxGroup]").prop("checked", $(obj).get(0).checked);
            //.attr("checked", $(obj).get(0).checked);//这种方式会有异常，只能执行一次，
        }
   </script>
</asp:Content>
