<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminNew.Master" AutoEventWireup="true" CodeBehind="DistributorUpdateList.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Fenxiao.DistributorUpdateList" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register src="../Ascx/ucDateTimePicker.ascx" tagname="DateTimePicker" tagprefix="Hi" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form runat="server">
    <div class="page-header">
        <h2>分销商升级奖励设置</h2>
        <small>分销商获得佣金自动升级，会产生一定的现金奖励，并计入分销商的佣金总额（系统调整佣金影响的等级变化，在下一次消费时生效）。<%--分销商完成购物自动升级，会额外给予一定的现金奖励，并计入分销商的佣金总额（手动调整的等级变化不参与该奖励,一次升多级只计最终等级的奖励）。--%></small>
    </div>
    <div class="table-page">
        <ul class="nav nav-tabs">
            <li>
                <a href="distributorupdateset.aspx"><span>奖励设置</span></a></li>
            <li class="active"><a href="distributorupdatelist.aspx"><span>升级佣金明细</span></a></li>
        </ul>
    </div>
        <div class="tab-pane active">
                <div class="set-switch">
                    <div class="form-inline">
                        <div class="form-group mr20">
                            <label for="ctl00_ContentPlaceHolder1_txtKey">分销商名称：</label>
                            <asp:TextBox ID="txtKey" runat="server" CssClass="form-control resetSize inputw150"></asp:TextBox>&nbsp;&nbsp;
                            <label for="ctl00_ContentPlaceHolder1_calendarStartDate_txtDateTimePicker">奖励时间：</label>
                            <Hi:DateTimePicker CalendarType="StartDate" ID="calendarStartDate" runat="server" CssClass="form-control resetSize inputw100" />
                            至
                                <Hi:DateTimePicker ID="calendarEndDate" runat="server" CalendarType="EndDate" CssClass="form-control resetSize inputw100" />
                           &nbsp;&nbsp;
                            <asp:Button ID="btnSearch" runat="server" Text="查询" CssClass="btn resetSize btn-primary" OnClick="btnSearch_Click" />
                        </div>
                    </div>
                </div>
            </div>


        <div class="datalist mt5">
            <asp:Repeater ID="rptList" runat="server">
                <HeaderTemplate>
                    <table class="table table-bordered table-hover">
                        <thead>
                            <tr>
                                <th>分销商名称</th>
                                <th>奖励金额</th>
                                <th>奖励时间</th>
                                <th>奖励原因</th>
                            </tr>
                        </thead>
                        <tbody>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td>
                            <%#Eval("StoreName") %>
                        </td>
                        <td>
                            <%#Eval("Commission","{0:F2}") %>元
                        </td>
                        <td><%#Eval("PubTime","{0:yyyy-MM-dd HH:mm:ss}") %></td>
                        <td><%#Eval("Memo") %></td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    </tbody>
	</table>
                </FooterTemplate>
            </asp:Repeater>
            <div class="page">
                <div class="bottomPageNumber clearfix">
                    <div class="pageNumber">
                        <div class="pagination">
                            <UI:Pager runat="server" ShowTotalPages="true" ID="pager" DefaultPageSize="5" />
                        </div>
                    </div>
                </div>
            </div>

            </div>
        </form>
</asp:Content>
