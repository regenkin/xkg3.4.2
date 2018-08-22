<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminNew.Master" 
    AutoEventWireup="true" CodeBehind="OrderStatisticsDetail.aspx.cs" 
    Inherits="Hidistro.UI.Web.Admin.Sales.OrderStatisticsDetail" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register Src="~/Admin/Ascx/ucDateTimePicker.ascx" TagPrefix="uc1" TagName="ucDateTimePicker" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form id="form1" runat="server">

        <a href="OrderStatistics.aspx" class="fr" >返回</a>
        <br />

                <div class="set-switch">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <label class="col-xs-1 pad resetSize control-label" for="setdate">操作时间：</label>
                            <div class="form-inline journal-query">
                                <div class="form-group">
                                    <%--<input type="text" id="setdate" placeholder="创建日期">--%>
                                    <%--<UI:WebCalendar CalendarType="StartDate" ID="txtBeginDate" runat="server"  CssClass="form-control resetSize" />--%>
                                    <uc1:ucDateTimePicker ID="txtBeginDate" runat="server" CssClass="form-control resetSize" />

                                    &nbsp;&nbsp;至&nbsp;
                                    <%--<input type="text" class="form-control resetSize" placeholder="创建日期">--%>
                                   <%-- <UI:WebCalendar CalendarType="EndDate" ID="txtEndDate" runat="server"  CssClass="form-control resetSize" />--%>
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


                <div class="data-shop">
                    <div class="datashop-top">
                        <ul class="nav nav-tabs">
                            <li class="active"><a href="javascript:void(0)">店铺销售数据</a></li>
                        </ul>
                    </div>
                    <div class="data-list clearfix">
                        <div class="data-box fl">
                            <h4>销售额</h4>
                            <p>￥<%= FXSaleAmountFee %></p>
                        </div>
                        <div class="data-box fl">
                            <h4>订单数</h4>
                            <p><%=FXOrderNumber %></p>
                        </div>
                        <div class="data-box fl">
                            <h4>成交用户数</h4>
                            <p><%=BuyerNumber %></p>
                        </div>
                        <div class="data-box fl">
                            <h4>客单价</h4>
                            <p>￥<%= FXBuyAvgPrice %></p>
                        </div>
                         <div class="data-box fl">
                            <h4>佣金收入</h4>
                            <p>￥<%=FXCommissionFee %></p>
                        </div>
                    </div>
                </div>


               

                <div>
                    <ul class="nav nav-tabs" role="tablist">
                        <li role="presentation" class="active"><a href='OrderStatisticsDetail.aspx?UserId=<%= UserId %>'>
                            一级分店</a></li>
                        <li role="presentation"><a href='OrderStatisticsDetail_L2.aspx?UserId=<%= UserId %>' >
                            二级分店</a></li>
                    </ul>
                    <div class="tab-content">
                        <div role="tabpanel" class="tab-pane active" id="home">
                             <!--选项卡1-->

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
                                                  <%--  <p><a href="DistributorStatisticsDetails.aspx?UserId=1">查看详情</a></p>--%>
                                                </td>
                                            </tr>
            
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </tbody>
                            </table>
                        </div>
 
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
                            
                        </div>

                        <div role="tabpanel" class="tab-pane" id="profile">

                            <!--选项卡2-->
                        </div>
                    </div>

                </div>

                

<hr />
          
 

    </form>
</asp:Content>
