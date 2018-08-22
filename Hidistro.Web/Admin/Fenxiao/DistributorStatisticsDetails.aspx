<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminNew.Master" AutoEventWireup="true" CodeBehind="DistributorStatisticsDetails.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Fenxiao.DistributorStatisticsDetails" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register src="../Ascx/ucDateTimePicker.ascx" tagname="DateTimePicker" tagprefix="Hi" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <style>

        .yejiItem{text-align:center;line-height:30px;float:left;margin:5px;padding:10px 40px;border-left:1px solid #b9caca}
            .yejiItem:first-child {border-left:0px
            }
            .yejiItem .money{color:#125acb;font-weight:bold;font-size:18px}
             .yejiItem .yejitxt{color:#444451;font-weight:bold;font-size:18px}
             .infodiv{float:left;}
             .infosdetail{width:250px;margin-left:10px;line-height:23px}
             .infostitle{width:90px; text-align:center;}
             .infosdetail ul label{width:90px;text-align:right;margin-right:10px;font-weight:normal;}
            .infosdetailLong{width:500px;}
            .table>tbody>tr>td{vertical-align: middle}
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

      <div class="page-header">
            <div class="infodiv">
                <div class="qrCode">
                            <Hi:HiImage  ID="ListImage1"   runat="server" Width="90" Height="90" />
                </div>
                  <div class="infostitle" style="font-weight:bold" id="txtStoreName" runat="server" >--</div>
              </div>
           <a onclick="javascript:history.go(-1)" style=";float:right;margin:60px 0px 0px 800px" class="btn btn-primary">返回</a>
          <div class="clearfix"></div>
         </div>

    <form runat="server">

          <!--搜索-->
           <div class="set-switch">
                    <div class="form-horizontal clearfix">
                      
                        <div class="form-group">
                            <label class="col-xs-1 pad control-label resetSize" for="setdate">时间范围：</label>
                            <div class="form-inline journal-query">
                                <div class="form-group">
                                   <Hi:DateTimePicker CalendarType="StartDate" ID="calendarStartDate" runat="server" CssClass="form-control resetSize inputw150" />&nbsp;&nbsp;至&nbsp;
                                   <Hi:DateTimePicker ID="calendarEndDate" runat="server" CalendarType="EndDate" CssClass="form-control resetSize inputw150" />
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




         <!--店铺销售数据-->
                 <div class="data-shop">
                    <div class="datashop-top">
                        <ul class="nav nav-tabs">
                            <li class="active"><a href="javascript:void(0)">店铺实时数据</a></li>
                        </ul>
                    </div>
                    <div class="data-list clearfix">
                        <div class="data-box fl">
                            <h4>销售额</h4>
                            <p   id="OrdersTotal" runat="server">￥0</p>
                        </div>
                        <div class="data-box fl">
                            <h4>订单数</h4>
                            <p  id="ReferralOrders" runat="server">0</p>
                        </div>
                        <div class="data-box fl">
                            <h4>成交用户数</h4>
                             <p id="BuyUsernums"  runat="server">-</p>
                        </div>
                        <div class="data-box fl">
                            <h4>客户单价</h4>
                            <p id="BuyPrice"  runat="server">￥-</p>
                        </div>

                         <div class="data-box fl">
                            <h4>佣金收入</h4>
                            <p   id="TotalReferral" runat="server">￥0</p>
                        </div>
                        
                    </div>
                </div>
        
     <!--数据tab-->
 <div class="play-tabs">
                    <ul class="nav nav-tabs" role="tablist">
                        <li role="presentation" class="<%=FristDisplay %>">
                            <asp:LinkButton ID="Frist" Text="一级分店(0)" runat="server" OnClick="Frist_Click" ></asp:LinkButton></li>
                        <li role="presentation"  class="<%=SecondDisplay %>"><asp:LinkButton ID="Second" Text="二级分店(0)" runat="server"   OnClick="Second_Click" ></asp:LinkButton></li>
                    </ul>
                　　<div style="font-size:3px;line-height:5px"></div>
 </div>
              

     <!--数据列表-->
             <div>
             <table class="table" style="table-layout:fixed">
                        <thead>
                           <tr>
                                        <th width="3%"></th>
                                        <th width="10%" style="text-align:left">微信头像</th>
                                        <th width="10%"  style="text-align:left">微信昵称/手机</th>
                                        <th width="10%">用户名</th>
                                        <th width="10%"  style="text-align:left">店名/联系人</th>
                                        <th width="10%">销售额</th>
                                        <th width="10%">订单数</th>
                                        <th width="10%">成交用户</th>
                                        <th width="8%">客户单价</th>
                                         <th width="8%">佣金收入</th>
                                    </tr>
                        </thead>
                        <tbody>

        <asp:Repeater ID="reDistributor" runat="server">
                                        <ItemTemplate>
                                            <tr>
                                                <td width="3%">
                                                    <asp:Literal runat="server" ID="litph"  Text="0"/>
                                                   </td>
                                                <td width="10%" style="text-align:left">
                                                    <div class="img fl mr10">
                                                        <Hi:ListImage ID="ListImage1" runat="server" DataField="UserHead"  Width="60" Height="60"/>
                                                    </div>
                                                </td>
                                                <td width="10%" style="text-align:left">
                                                    <p><span><%# Eval("UserName")%></span></p>
                                                    <p><span><%# Eval("CellPhone")%></span></p>
                                                </td>
                                                <td width="10%"><%# Eval("UserName")%></td>
                                                <td width="10%"  style="text-align:left">
                                                   <p><%# Eval("StoreName")%></p>
                                                    <p><%# Eval("RealName")%></p>
                                                </td>
                                                <td width="10%"><em>￥<%# Math.Round((decimal)Eval("OrderTotalSum"),2) %></em></td>
                                                <td width="10%"><%#Eval("Ordernums") %></td>
                                                <td width="10%"><%# int.Parse(Eval("Ordernums").ToString())==0?"0":Eval("BuyUserIds") %></td>
                                                <td width="8%">￥<%# Math.Round(decimal.Parse(Eval("OrderTotalSum").ToString())/ decimal.Parse(Eval("BuyUserIds").ToString()),2) %></td>
                                                <td width="8%"><em>￥<%# decimal.Parse( Eval("CommTotalSum", "{0:F2}")) + decimal.Parse( Eval("ReferralRequestBalance", "{0:F2}"))%></em></td>
                                              
                                            </tr>
                                        </ItemTemplate>
                                    </asp:Repeater>
         </tbody>
     </table>
     </div>

    

         <!--数据列表底部功能区域-->
  <br />
        <div class="select-page clearfix">
                    <div class="form-horizontal fl">
                       <a onclick="javascript:history.go(-1)" class="btn btn-primary">返回</a>
                    </div>
                    <div  class="page fr">
                         <div class="pageNumber">
                        <div class="pagination" style="margin:0px">
                        <UI:Pager runat="server" ShowTotalPages="true" DefaultPageSize="5" ID="pager" />　
                       </div>
                      </div>
                    </div>
                </div>

        <div class="clearfix" style="height:30px"></div>

        
    </form>



</asp:Content>
