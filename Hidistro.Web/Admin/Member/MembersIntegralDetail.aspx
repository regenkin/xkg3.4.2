<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MembersIntegralDetail.aspx.cs"
    MasterPageFile="~/Admin/AdminNew.Master" Inherits="Hidistro.UI.Web.Admin.member.MembersIntegralDetail" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register src="../Ascx/ucDateTimePicker.ascx" tagname="DateTimePicker" tagprefix="Hi" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="/utility/skins/blue.css" type="text/css" media="screen" />
    <Hi:Script ID="Script5" runat="server" Src="/utility/jquery.artDialog.js" />
    <Hi:Script ID="Script6" runat="server" Src="/utility/Window.js" />
    <Hi:Script ID="Script4" runat="server" Src="/admin/js/jquery.formvalidation.js" />
  
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
                      

        
            <div class="page-header">
                <h2>会员积分明细</h2>
            </div>
        <!--搜索-->

        <!--数据列表区域-->
        <div  >
          
            <div class="form-inline mb10">
                 <div class="set-switch">
                     
                    <div class="form-group mr20">
                        <label for="sellshop1">积分来源/用途：</label>
                        <asp:DropDownList ID="drIntegralStatus" runat="server">
                            <asp:ListItem Value="-1">全部</asp:ListItem>
                             <asp:ListItem Value="1">购物送积分</asp:ListItem>
                             <asp:ListItem Value="2">签到送积分</asp:ListItem>
                             <asp:ListItem Value="3">抽奖送积分</asp:ListItem>
                          <%--   <asp:ListItem Value="4">订单退换货送积分</asp:ListItem>--%>
                             <asp:ListItem Value="5">积分抵现</asp:ListItem>
                             <asp:ListItem Value="6">抽奖消耗</asp:ListItem>
                             <asp:ListItem Value="7">积分兑换</asp:ListItem>
                             <asp:ListItem Value="0">其他</asp:ListItem>
                        </asp:DropDownList>  
                    </div>
                       <div class="form-group mr20" style ="margin-left :30px;">
                     
                              <div class="form-group mr20">
                                <label for="sellshop5">创建时间：</label>
                                <Hi:DateTimePicker CalendarType="StartDate" ID="calendarStartDate" runat="server" CssClass="form-control resetSize inputw150" />- <Hi:DateTimePicker ID="calendarEndDate" runat="server" CalendarType="EndDate" CssClass="form-control resetSize inputw150" />
                            </div>&nbsp;&nbsp;&nbsp;&nbsp;
                              <asp:Button ID="btnSearchButton" runat="server" CssClass="btn resetSize btn-primary" Text="搜索" />
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
            <div id="datalist"  >
            <UI:Grid ID="grdMemberList" runat="server" ShowHeader="true" AutoGenerateColumns="false"
                DataKeyNames="UserId" HeaderStyle-CssClass="table_title" CssClass="table table-hover mar table-bordered"
                GridLines="None" Width="100%" >
                <Columns>
                      <asp:TemplateField HeaderText="积分来源" ShowHeader="true">
                        <ItemTemplate>
                          <div></div>
                               &nbsp;
                             <%#Eval("IntegralSource") %>

                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="日期"   >
                        <ItemTemplate>
                           <div></div>
                             <%# Eval("TrateTime")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="积分变化" HeaderStyle-HorizontalAlign="Center" ShowHeader="true">
                        <ItemTemplate>
                            <div></div>
                            &nbsp;
                            <%#Convert.ToDecimal( Eval("IntegralChange"))>0?"<em style=\"color:red\">+"+Eval("IntegralChange","{0:f0}")+"</em>":"<em style=\"color:#008000\">"+Eval("IntegralChange","{0:f0}")+"</em>" %>

                        </ItemTemplate>
                    </asp:TemplateField>
                   <asp:TemplateField HeaderText="备注" ShowHeader="true">
                        <ItemTemplate>
                            <div></div>
                            &nbsp; <asp:Literal ID="Remark" runat="server" Text='<%# Eval("Remark") %>' /></ItemTemplate>
                    </asp:TemplateField>
                    
                </Columns>
            </UI:Grid>   

            </div>      
        </div>
        <!--数据列表底部功能区域-->
    
        <div class="bottomPageNumber clearfix">
            <div class="pageNumber">
                <div class="pagination" style="width: auto">
                    <UI:Pager runat="server" ShowTotalPages="true" ID="pager1" />
                </div>
            </div>
        </div>
      

         <!-- dialog end-->

        </form>
    <script type="text/javascript">
         
    </script>
</asp:Content>
