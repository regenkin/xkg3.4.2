<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminNew.Master" AutoEventWireup="true" CodeBehind="CommissionsAllList.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Fenxiao.CommissionsAllList" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register src="../Ascx/ucDateTimePicker.ascx" tagname="DateTimePicker" tagprefix="Hi" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script>

        $(function () {
            //合并行
            var $temrow = null;
            var tempOrderId = "";


            var $commisTb = $(".td_bg");
           
            if ($commisTb.length > 0) {
                $commisTb.each(function () {
                    var $tds=$(this).find("td");
                    var thisOrderId = $tds.eq(0).text().trim();

                    if (thisOrderId != tempOrderId) {
                        tempOrderId = thisOrderId;
                        $tds.eq(0).css("border-width", "1px 1px 0px 1px");
                        $tds.eq(1).css("border-width", "1px 1px 0px 1px");
                        $tds.eq(2).css("border-width", "1px 1px 0px 1px");
                    } else {
                        $tds.eq(0).text("").css("border-width", "0px 1px 0px 0px");
                        $tds.eq(1).text("").css("border-width", "0px 1px 0px 0px");
                        $tds.eq(2).text("").css("border-width", "0px 1px 0px 0px");
                    }

                });
            }




        });

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
          <div class="page-header">
            <h2>佣金明细</h2>
         </div>
    <form runat="server">
    <div class="set-switch">
                    <div class="form-horizontal clearfix">
                       <div class="form-inline mb10">
                            <div class="form-group">
                                <label　 for="sellshop1" style="margin-left:10px">店铺名称：</label>
                                <asp:TextBox ID="txtStoreName" CssClass="form-control resetSize inputw150" runat="server" />
                            </div>
                            <div class="form-group" style="padding-left:1px">
                                <label for="sellshop2">　订单号：</label>
                                <asp:TextBox ID="txtOrderId"  CssClass="form-control  resetSize  inputw150" runat="server"  Width="150" />
                            </div>
                        </div>

                        <div class="form-inline  mb10">
                            <label class="col-xs-1 pad control-label resetSize" style="font-weight:500" for="setdate">时间范围：</label>
                            <div class="form-inline journal-query">
                                <div class="form-group" style="padding-left:4px">
                                   <Hi:DateTimePicker CalendarType="StartDate" ID="calendarStartDate" runat="server" CssClass="form-control resetSize inputw150" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;至&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                   <Hi:DateTimePicker ID="calendarEndDate" runat="server" CalendarType="EndDate" CssClass="form-control resetSize inputw150" />&nbsp;&nbsp;&nbsp;
                                </div>
                                <asp:Button ID="btnQueryLogs" runat="server" class="btn resetSize btn-primary" Text="查询" OnClick="btnQueryLogs_Click" />&nbsp;&nbsp;
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
     <asp:Repeater ID="reCommissions"  runat="server" OnItemDataBound="rptypelist_ItemDataBound" >
         <HeaderTemplate>
             <div>
             <table class="table table-hover mar table-bordered" style="table-layout:fixed">
                        <thead>
                            <tr>
                                <th  width="120"><%--订单号--%>流水号</th>
                                 <th  width="100">买家昵称</th>
                                <th width="120"><%--分佣--%>时间</th>
                                 <th  width="100"><%--与成交店铺关系--%>类型</th>  
                                 <th  width="100">分佣店铺</th>
                                <th  width="100">佣金</th>  
                            </tr>
                        </thead>
                        <tbody>
         </HeaderTemplate>
     <ItemTemplate>
      <tr  class="td_bg">
                                 <td>
                                    &nbsp;<%#Eval("UserName")==DBNull.Value?"<span style='color:#ccc'>"+Eval("OrderId").ToString()+"</span>":"<a href='/Admin/trade/OrderDetails.aspx?OrderId="+Eval("OrderId")+"' target='_blank'>"+Eval("OrderId")+"</a>"%></td>
                                  <td>
                                    &nbsp;<%# (int)Eval("CommType")==4?"<span style='color:#ccc'>升级奖励</span>":(int)Eval("CommType")==5?"<span style='color:#ccc'>后台调整</span>":Eval("UserName")==DBNull.Value?"<span style='color:#ccc'>订单已删除</span>":Eval("UserName").ToString()%></td>
           <td width="200">
                                 &nbsp;<%# Eval("TradeTime", "{0:yyyy-MM-dd HH:mm:ss}")%></td>
          <td>&nbsp;<%# (int)Eval("CommType")==4?"<span style='color:#FF6600'>升级奖励</span>":(int)Eval("CommType")==5?"<span style='color:#FF6600'>后台调整</span>":getNextName(Eval("UserId").ToString(),Eval("ReferralUserId").ToString(),Eval("ReferralPath").ToString())%></td>
          <td>&nbsp;<%# Eval("StoreName")%></td>
                                <td>
                                    &nbsp;￥<%# Eval("CommTotal","{0:F2}")%></td>
       
           </tr>
         <asp:Repeater ID="reCommissionsChild"  runat="server" >
              <ItemTemplate>
        <tr  class="td_bg">
                                 <td>
                                    &nbsp;<a href="/Admin/trade/OrderDetails.aspx?OrderId=<%# Eval("OrderId")%>" target="_blank"><%# Eval("OrderId")%></a></td>
                                  <td>
                                    &nbsp;<%# Eval("UserName")%></td>
           <td width="200">
                                 &nbsp;<%# Eval("TradeTime", "{0:yyyy-MM-dd HH:mm:ss}")%></td>
          <td>&nbsp;<%# (int)Eval("CommType")==3?"<span style='color:#FF6600'>升级奖励</span>":getNextName(Eval("UserId").ToString(),Eval("ReferralUserId").ToString(),Eval("ReferralPath").ToString())%></td>
          <td>&nbsp;<%# Eval("StoreName")%></td>
                                <td>
                                    &nbsp;￥<%# Eval("CommTotal","{0:F2}")%></td>
       
           </tr>
          </ItemTemplate>
         </asp:Repeater>
     </ItemTemplate>
     <FooterTemplate>
         </tbody>
     </table>
     </div>
     </FooterTemplate>
     </asp:Repeater>

         <!--数据列表底部功能区域-->
  <br />
        <div class="select-page clearfix">
                    <div class="form-horizontal fl">
                       &nbsp;佣金总额：￥<%=Math.Round(CurrentTotal,2) %>
                    </div>
                    <div  class="page fr">
                         <div class="pageNumber">
                        <div class="pagination" style="margin:0px">
                        <UI:Pager runat="server" ShowTotalPages="true" ID="pager" />　
                       </div>
                      </div>
                    </div>
                </div>

        <div class="clearfix" style="height:30px"></div>
        </form>
</asp:Content>
