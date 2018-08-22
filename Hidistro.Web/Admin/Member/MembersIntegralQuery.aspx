<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MembersIntegralQuery.aspx.cs"
    MasterPageFile="~/Admin/AdminNew.Master" Inherits="Hidistro.UI.Web.Admin.member.MembersIntegralQuery" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="Hidistro.Core" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="/utility/skins/blue.css" type="text/css" media="screen" />
 
    <style type="text/css">
        /*.selectthis {border-color:red; color:red; border:1px solid;}*/
        .tdClass{text-align:center;}
        .labelClass{margin-right:10px;}
        .thCss{text-align:center;}
        .selectthis{border:1px solid;border-color:#999999; color:#c93027;margin-right:2px;}
        .selectthis:hover {border:1px solid;border-color:#999999; color:#c93027;margin-right:2px;}
        .aClass{border:1px solid;border-color:#999999; color:#999999;margin-right:2px;}
        .aClass:hover{border:1px solid;border-color:#999999; color:#999999;margin-right:2px;}
    </style>
   
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <form id="thisForm" runat="server" class="form-horizontal">
                      

        <div>
            <div class="page-header">
                <h2>会员积分查询</h2>
            </div>
        <!--搜索-->

        <!--数据列表区域-->
        <div  >
          
            <div class="form-inline mb10">
                 <div class="set-switch">
                    <div class="form-inline  mb10">
                    <div class="form-group mr20">
                        <label for="ctl00_ContentPlaceHolder1_txtSearchText" class="ml10">昵称：</label>
                            <asp:TextBox ID="txtSearchText" CssClass="form-control resetSize"  runat="server" /> 
                    </div>
                       <div class="form-group mr20" style ="margin-left :30px;">
                       <label for="sellshop1">手机号码：</label>
                            <asp:TextBox ID="txtPhone" CssClass="form-control resetSize" runat="server" />
                        </div>
                           <div class="form-group mr20" style ="margin-left :30px;">
                  <a class="bl mb5" href="/admin/Member/ManageMembers.aspx" style="cursor: pointer">清除条件</a>
                      </div>  
                      </div>  
                      <div class="form-inline ">
                          <div class="form-group">
                                <label class="ml10">姓名：</label>
                      
                             <asp:TextBox ID="txtRealName" CssClass="form-control resetSize" runat="server" />
                            </div>
                       <div class="form-group mr20" style ="margin-left :65px;">
                        <label>会员等级：</label>                 
                            <Hi:MemberGradeDropDownList ID="rankList" Width="170" runat="server" CssClass="form-control resetSize"
                                AllowNull="true" NullToDisplay="全部" />
                            </div>   
                         <div class="form-group" style ="margin-left :30px;">
                        
                        <asp:Button ID="btnSearchButton" runat="server" CssClass="btn resetSize btn-primary" Text="搜索" />
                             </div>
                                   
                    </div>   
                </div>          
             </div>
 

            <div style="margin-bottom:5px;  margin-top:10px;">
        
                <div class="form-inline" id="pagesizeDiv" style="float: left; width:100%; margin-bottom:5px;">
                 
                </div>
  
                  <div class="page-box clearfix">
                    <div class="page fr">
                        <div class="form-group" style="margin-right:0px;margin-left:0px;">
                            <label for="exampleInputName2">每页显示数量：</label>
                       <UI:PageSize runat="server"  ID="hrefPageSize" />
                        </div>
                    </div>
                </div>
                <div class="pageNumber" style="float: right;  height:29px; margin-bottom:5px; display:none;" >
                    <label>每页显示数量：</label>
                    <div class="pagination" style="display:none;">
                        <UI:Pager runat="server" ShowTotalPages="false" ID="pager" />
                    </div>
                </div>
                
             
                <!--结束-->                           
            </div>
         
            <UI:Grid ID="grdMemberList" runat="server" ShowHeader="true" AutoGenerateColumns="false"
                DataKeyNames="UserId" HeaderStyle-CssClass="table_title" CssClass="table table-hover mar table-bordered"
                GridLines="None" Width="100%" >
                <Columns>
                   
                      <asp:TemplateField HeaderText="昵称" ShowHeader="true">
                        <ItemTemplate>
                            <div></div>
                            &nbsp;<%# Eval("UserName")%></ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="姓名"  SortExpression="RealName"  HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <p><asp:Literal ID="lblUserName" runat="server" Text='<%# Eval("RealName").ToString()==""?"未设置":Eval("RealName") %>' /></p>
                            
                           
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="微信OPenID" HeaderStyle-HorizontalAlign="Center" ShowHeader="true">
                        <ItemTemplate>
                            <div></div>
                            &nbsp;<%# Eval("OpenID").ToString()==""?"未绑定":Eval("OpenID")%></ItemTemplate>
                    </asp:TemplateField>
                   <asp:TemplateField HeaderText="手机" ShowHeader="true">
                        <ItemTemplate>
                            <div></div>
                            &nbsp; <asp:Literal ID="lbCellPhone" runat="server" Text='<%# Eval("CellPhone") %>' /></ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="会员等级" ShowHeader="true">
                        <ItemTemplate>
                            <div></div>
                            &nbsp; <asp:Literal ID="lblGradeName" runat="server" Text='<%# Eval("GradeName") %>' /></ItemTemplate>
                    </asp:TemplateField>
                     
                    <asp:TemplateField HeaderText="会员可用积分" >
                        <ItemTemplate>
                            &nbsp;
                             <ItemTemplate>&nbsp;<%# Eval("Points")%></ItemTemplate>
                        </ItemTemplate>
                    </asp:TemplateField>

               
                   
                    <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="border_top border_bottom"
                        HeaderStyle-Width="95">
                        <ItemStyle CssClass="spanD spanN" />
                        <ItemTemplate>
                            <p><a href='<%# Globals.GetAdminAbsolutePath(string.Format("/member/MembersIntegralDetail.aspx?userId={0}", Eval("UserId")))%>'>
                                积分明细</a>
                          
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </UI:Grid>   

           
        </div>
        <!--数据列表底部功能区域-->
    
        <div class="bottomPageNumber clearfix">
            <div class="pageNumber">
                <div class="pagination" style="width: auto">
                    <UI:Pager runat="server" ShowTotalPages="true" ID="pager1" />
                </div>
            </div>
        </div>
       <!--会员短信群发-->
 
        </div>
 


      
         <!-- dialog end-->

        </form>
   
</asp:Content>
