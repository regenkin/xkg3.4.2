<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminNew.Master" 
    AutoEventWireup="true" CodeBehind="OrderStatistics.aspx.cs" 
    Inherits="Hidistro.UI.Web.Admin.Sales.OrderStatistics" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register Src="~/Admin/Ascx/ucDateTimePicker.ascx" TagPrefix="uc1" TagName="ucDateTimePicker" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style type="text/css">        .templateTitle select {
   height:auto;     }
    </style>
    <form id="form1" runat="server">
                <div class="page-header">
                    <h2>订单统计</h2>
                </div>
                <div class="set-switch">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <label class="col-xs-1 pad resetSize control-label" for="setdate">操作时间：</label>
                            <div class="form-inline journal-query">
                                <div class="form-group">
                                    <%--<input type="text" id="setdate" placeholder="创建日期">--%>                     
                                    <uc1:ucDateTimePicker ID="txtBeginDate" runat="server" CssClass="form-control resetSize" />
                                    &nbsp;&nbsp;至&nbsp;
                                    <%--<input type="text" class="form-control resetSize" placeholder="创建日期">--%>
                                    <uc1:ucDateTimePicker ID="txtEndDate" runat="server" CssClass="form-control resetSize" />
                                   
                                </div>
                                <asp:Button ID="btnSearch" runat="server"  CssClass="btn resetSize btn-primary" Text="查询" />
<%--                                <button type="submit" class="btn resetSize btn-primary">查询</button>--%>
                                <div class="form-group">
                                    <label for="exampleInputName2">查询日期</label>
                                    <asp:Button ID="btnWeekView" runat="server" class="btn resetSize btn-default" Text="最近7天" OnClick="btnWeekView_Click"   />
                                    <asp:Button ID="btnMonthView" runat="server" class="btn resetSize btn-default" Text="最近一个月" OnClick="btnMonthView_Click" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <h3 class="templateTitle">所有订单</h3>
                <div class="orderList mb20">
                    <ul class="clearfix">
                        <li class="noborder">
                            <div class="number"><%=OrderNumber%></div>
                            <p>订单总数</p>
                        </li>
                        <li>
                            <div class="number">￥<%=SaleAmountFee%></div>
                            <p>订单总金额</p>
                        </li>
                    </ul>
                </div>
                <h3 class="templateTitle">分销订单</h3>
                <div class="orderList mb20">
                    <ul class="clearfix">
                        <li class="noborder">
                            <div class="number"><%=FXOrderNumber%></div>
                            <p>分销订单数</p>
                        </li>
                        <li>
                            <div class="number">￥<%=FXSaleAmountFee%></div>
                            <p>分销订单金额</p>
                        </li>
                        <li>
                            <div class="number"><%=FXResultPercent%>%</div>
                            <p>分销业绩占比</p>
                        </li>
                        <li>
                            <div class="number">￥<%=FXCommissionFee%></div>
                            <p>产生佣金</p>
                        </li>
                    </ul>
                </div>
                <h3 class="templateTitle">
                    <span>分销业绩</span>
                    <asp:DropDownList ID="ddlTop" runat="server" AutoPostBack="True"
                         OnSelectedIndexChanged="ddlTop_SelectedIndexChanged" Visible="true" CssClass="form-control resetSize autow inl inputw150">
                        <asp:ListItem Selected="True" Value="10">TOP10</asp:ListItem>
                        <asp:ListItem Value="20">TOP20</asp:ListItem>
                        <asp:ListItem Value="50">TOP50</asp:ListItem>
                        <asp:ListItem Value="100">TOP100</asp:ListItem>
                        <asp:ListItem Value="200">TOP200</asp:ListItem>
                        <asp:ListItem Value="500">TOP500</asp:ListItem>
                    </asp:DropDownList>
                </h3>
                <div class="topListTable">
                    <table class="table">
                        <thead>
                            <tr>
                                <th width="3%"></th>
                                <th width="10%" style="text-align:left">微信头像</th>
                                <th width="11%" style="text-align:left">微信昵称/手机</th>
                                <th width="10%">用户名</th>
                                <th width="10%" style="text-align:left">店名/联系人</th>
                                <th width="10%">销售额</th>
                                <th width="10%">订单数</th>
                                <th width="10%">成交用户</th>
                                <th width="8%">客户单价</th>
                                 <th width="8%">佣金收入</th>
                                <th width="14%"></th>
                            </tr>
                        </thead>
                        <tbody>
                            <asp:Repeater ID="rptList" runat="server">
                                <ItemTemplate>
                                    <tr>
                                        <td>
                                            <img src="../images/0001.gif" runat="server" id="rank1" visible='<%# Eval("RowNumber").ToString()=="1"%>' >
                                            <img src="../images/0002.gif" runat="server" id="rank2" visible='<%# Eval("RowNumber").ToString()=="2"%>' >
                                            <img src="../images/0003.gif" runat="server" id="rank3" visible='<%# Eval("RowNumber").ToString()=="3"%>' >
                                            <asp:Literal runat="server" ID="lbRank" Text='<%# Eval("RowNumber")%>'   Visible='<%#  Convert.ToInt32( Eval("RowNumber").ToString()) >3 %>' />
                                        </td>
                                        <td style="text-align:left">
                                            <div class="img fl mr10">
                                                <%--<img src="http://fpoimg.com/60x60">--%>
                                                <Hi:ListImage ID="ListImage1" runat="server" DataField="UserHead"  Width="60" Height="60"/>
                                            </div>
                                        </td>
                                        <td style="text-align:left"><%# Eval("RealName").ToString()==""?"未设置":Eval("RealName") %><br />
                                            <%# Eval("CellPhone").ToString()==""?"未绑定":Eval("CellPhone") %>
                                        </td>
                                        <td><em><%# Eval("UserName").ToString()==""?"未设置":Eval("UserName") %></em></td>
                                        <td style="text-align:left">
                                            <p><%# Eval("StoreName").ToString()==""?"未设置":Eval("StoreName") %></p>
                                            <p><%# Eval("UserName").ToString()==""?"未设置":Eval("UserName") %></p>
                                        </td>
                                        <td><em>￥<%# Convert.ToDecimal( Eval("SaleAmountFee").ToString()).ToString("N2") %></em></td>
                                        <td><em><%# Eval("OrderNumber") %></em></td>
                                        <td><em><%# Eval("BuyerNumber") %></em></td>
                                        <td><em>￥<%# Convert.ToDecimal( Eval("BuyerAvgPrice").ToString()).ToString("N2") %></em></td>
                                        <td><em>￥<%# Convert.ToDecimal( Eval("CommissionAmountFee").ToString()).ToString("N2") %></em></td>
                                        <td>
                                            <p><a href='OrderStatisticsDetail.aspx?UserId=<%# Eval("AgentId") %>'>查看详情</a></p>
                                        </td>
                                    </tr>
            
                                </ItemTemplate>
                            </asp:Repeater>
                        </tbody>
                    </table>
                </div>

<hr />
          
 
    <!--数据列表-->
        <br />
        
        
        <br />

        <div class="select-page clearfix">
                    <div class="form-horizontal fl">
                       
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
