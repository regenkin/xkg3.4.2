<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminNew.Master" AutoEventWireup="true" CodeBehind="BalanceDrawRequestList.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Fenxiao.BalanceDrawRequestList" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register src="../Ascx/ucDateTimePicker.ascx" tagname="DateTimePicker" tagprefix="Hi" %>
<%@ Import Namespace="Hidistro.ControlPanel.Store" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <div class="page-header">
            <h2>提现记录</h2>
         </div>
    <form runat="server">

         <div class="set-switch">
                    <div class="form-horizontal clearfix">
                      

                        <div class="form-inline  mr20">
                            
                            
                            <div class="form-inline journal-query">
                            <div class="form-group">
                                <label　 for="sellshop1">　店铺名：</label>
                                <asp:TextBox ID="txtStoreName" CssClass="form-control resetSize inputw100" runat="server" />
                             </div>
                                
                                <div class="form-group" style="padding-left:4px">
                                    <label  for="setdate">支付日期：</label>
                                   <Hi:DateTimePicker CalendarType="StartDate" ID="txtRequestStartTime" runat="server" CssClass="form-control resetSize inputw100" />&nbsp;至&nbsp;
                                   <Hi:DateTimePicker ID="txtRequestEndTime" runat="server" CalendarType="EndDate" CssClass="form-control resetSize inputw100" />&nbsp;&nbsp;&nbsp;
                                </div>
                                <asp:Button ID="btnSearchButton" runat="server" class="btn resetSize btn-primary" Text="查询"/>&nbsp;&nbsp; <%--OnClick="btnQueryLogs_Click"--%> 
                                <div class="form-group">
                                    <label for="exampleInputName2">快速查看</label>
                                    <asp:Button ID="Button1" runat="server" class="btn resetSize btn-default" Text="最近7天" OnClick="Button1_Click1"  />
                                    <asp:Button ID="Button4" runat="server" class="btn resetSize btn-default" Text="最近一个月" OnClick="Button4_Click1" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>


        <!--数据列表-->
             <div>
             <table class="table table-hover mar table-bordered" style="table-layout:fixed">
                        <thead>
                            <tr>
                                <td width="80">分销商店铺</td>
                                 <td  width="120">
                            申请日期
                        </td>
                        <td  width="100">
                         提现金额
                        </td>
                       
                       
                         <td  width="100">
                          帐号类型
                        </td>
                         <td  width="100">
                          帐号
                        </td>
                       <td  width="80">
                          收款人
                        </td>
                                 <td  width="120">
                          支付日期
                        </td>
                            </tr>
                        </thead>
                        <tbody>

        <asp:Repeater ID="reBalanceDrawRequest"  runat="server"  >
     <ItemTemplate>
      <tr  class="td_bg">
                              <td>
                                   &nbsp; <%# Eval("StoreName")%>
                                </td>
                              <td >
                                 &nbsp; <%# Eval("RequestTime", "{0:yyyy-MM-dd HH:mm:ss}")%></td>
                                 <td>
                                     &nbsp;￥<%# Eval("Amount", "{0:F2}")%></td>
                                  <td>
                                       &nbsp;<%# VShopHelper.GetCommissionPayType(Eval("RequestType").ToString()) %>
                                   </td>
                                <td>
                                    
                                    &nbsp;<%# Eval("MerchantCode") %><br />
                                    <%# Eval("bankName") %>
                                </td>
                                <td>
                                    &nbsp;<%# Eval("AccountName") %></td>
                                <td>
                                    &nbsp;<%# Eval("CheckTime", "{0:yyyy-MM-dd HH:mm:ss}")%></td>
        </tr>
     </ItemTemplate>
 </asp:Repeater>
         </tbody>
     </table>
     </div>

    

         <!--数据列表底部功能区域-->
  <br />
        <div class="select-page clearfix">
                    
                    <div  class="page fr">
                         <div class="pageNumber">
                        <div class="pagination" style="margin:0px">
                        <UI:Pager runat="server" ShowTotalPages="true" DefaultPageSize="10" ID="pager" />　
                       </div>
                      </div>
                    </div>
                </div>

        <div class="clearfix" style="height:30px"></div>



    </form>

</asp:Content>
