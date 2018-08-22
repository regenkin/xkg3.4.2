<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminNew.Master" AutoEventWireup="true" CodeBehind="DistributorStatistics.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Fenxiao.DistributorStatistics" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Import Namespace="System.Web.UI.WebControls" %>
<%@ Register src="../Ascx/ucDateTimePicker.ascx" tagname="DateTimePicker" tagprefix="Hi" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
      <style>
        em{color:#FF6600}
    </style>
    <script>

        $(function () {
            var tableTitle = $('.title-table').offset().top - 58;
            $(window).scroll(function () {
                if ($(document).scrollTop() >= tableTitle) {
                    $('.title-table').css({
                        position: 'fixed',
                        top: '58px'
                    })
                }
                //console.log($(document).scrollTop() + $('.title-table').height() + "||" + tableTitle)
                if ($(document).scrollTop() + $('.title-table').height() - 38 <= tableTitle) {
                    $('.title-table').removeAttr('style');
                }
            });

        });

    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <div class="page-header">
            <h2>分销商销售排行榜</h2>
   </div>
    <form runat="server">

          <div class="tab-pane active">

                    

           <%--                    <div class="select-page clearfix" style="margin-top: 20px;">
                    </div>--%>
                    <div class="sell-table">
                        <div class="title-table">
                            <table class="table">
                                <thead>
                                    <tr><td colspan="11" style="background:#f7f7f7;">

                                        <!--查询区-->
                   <div style="margin:15px 0px 0px 20px">
                    <div class="form-horizontal clearfix">
                      
                        <div class="form-group">
                            <label class="col-xs-1 pad control-label resetSize" for="setdate">时间范围：&nbsp;&nbsp;&nbsp;</label>
                            <div class="form-inline journal-query">
                                <div class="form-group">
                                   <Hi:DateTimePicker CalendarType="StartDate" ID="calendarStartDate" runat="server" CssClass="form-control resetSize inputw150" />&nbsp;&nbsp;至&nbsp;&nbsp;
                                   <Hi:DateTimePicker ID="calendarEndDate"  runat="server" CalendarType="EndDate" CssClass="form-control resetSize inputw150" />&nbsp;&nbsp;&nbsp;
                                </div>
                                <asp:Button ID="btnQueryLogs" runat="server" class="btn resetSize btn-primary" Text="查询" OnClick="btnQueryLogs_Click"  />&nbsp;&nbsp;
                                <div class="form-group">
                                    <label for="exampleInputName2">&nbsp;&nbsp;&nbsp;快速查看</label>
                                    <asp:Button ID="Button1" runat="server" class="btn resetSize btn-default"  OnClick="Button1_Click1" Text="最近7天"   />
                                    <asp:Button ID="Button4" runat="server" class="btn resetSize btn-default"   OnClick="Button4_Click1"  Text="最近一个月"  />
                                </div>
                            </div>
                        </div>
                    </div>
                   </div> 
                                        </td></tr>
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
                                        <th width="15%"></th>
                                    </tr>
                                </thead>
                            </table>
                        </div>
                        <div class="content-table">
                            <table class="table">
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
                                                <td width="10%"><%# Eval("BuyUserIds") %></td>
                                                <td width="8%">￥<%# Eval("BuyUserIds").ToString()=="0"?"0.00":Math.Round(decimal.Parse(Eval("OrderTotalSum").ToString())/ decimal.Parse(Eval("BuyUserIds").ToString()),2).ToString() %></td>
                                                <td width="8%"><em>￥<%# decimal.Parse( Eval("CommTotalSum", "{0:F2}"))%></em></td>
                                               <td width="15%">
                                                    <p>
                                                        <a href="DistributorStatisticsDetails.aspx?UserId=<%#Eval("UserId") %>">查看详情</a>
                                                   
                                                    </p>
                                                   
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:Repeater>

                                </tbody>
                            </table>
                        </div>
                    </div>
                    <div class="page">
                        <div class="bottomPageNumber clearfix">
                            <div class="pageNumber">
                                <div class="pagination">
                                    <UI:Pager runat="server" ShowTotalPages="true" ID="pager" />
                                </div>
                            </div>
                        </div>
                    </div>
                
              
                </div>

    </form>
</asp:Content>
