<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MemberRanking_1.aspx.cs"
    MasterPageFile="~/Admin/AdminNew.Master" Inherits="Hidistro.UI.Web.Admin.Member.MemberRanking_1" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .selectthis{border:1px solid;border-color:#999999; color:#c93027;margin-right:2px;}
        .selectthis:hover {border:1px solid;border-color:#999999; color:#c93027;margin-right:2px;}
        .aClass{border:1px solid;border-color:#999999; color:#999999;margin-right:2px;}
        .aClass:hover{border:1px solid;border-color:#999999; color:#999999;margin-right:2px;}
    </style>
    <script type="text/javascript">
        $(document).ready(function () {
            $('#pagesizeDiv').find('a').each(function () {
                if ($(this).attr("class") != "selectthis") {
                    $(this).removeClass();
                    $(this).addClass('aClass');
                }
            });
        });
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form id="thisForm" runat="server" class="form-horizontal">
    <div class="page-header">
        <h2>会员消费排行</h2>
        <small>查询有成交记录的会员的订单数和购物金额,并按购物金额从高到低排行</small>
    </div>
    <!--数据列表区域-->
    <div class="datalist">
        <!--搜索-->
        <!--结束-->
        <div class="searcharea clearfix ">
            <div class="form-inline">
                <label>起始日期：</label>
                <UI:WebCalendar CalendarType="StartDate" ID="calendarStartDate" 
                    runat="server" class="form-control" />
               <label>终止日期：</label>
                <UI:WebCalendar ID="calendarEndDate" runat="server" CalendarType="EndDate"
                   class="form-control" />
                <label>排行方式：</label>
                <asp:DropDownList runat="server" ID="ddlSort" CssClass="form-control"
                    Style="width: 150px;" />
                <asp:Button ID="btnSearchButton" runat="server" CssClass="btn btn-primary" Text="查询" />                 
                <asp:LinkButton ID="btnCreateReport" runat="server" Text="生成报告" />
                

            </div>
        </div>
        <div class="functionHandleArea clearfix" style="margin-top:5px;">
            <!--分页功能-->

            <div class="form-inline" id="pagesizeDiv" style="float: left; width: 100%;">
                <label>每页显示数量：</label><UI:PageSize runat="server" ID="hrefPageSize" />
            </div>
            <div class="pageNumber">
                <div class="pagination">
                    <UI:Pager runat="server" ShowTotalPages="false" ID="pager" />
                </div>
            </div>
            <!--结束-->
        </div>
        <UI:Grid ID="grdProductSaleStatistics" runat="server" ShowHeader="true" AutoGenerateColumns="false"
            HeaderStyle-CssClass="table_title" CssClass="table table-hover mar table-bordered"
            GridLines="None" Width="100%">
            <Columns>
                <asp:TemplateField HeaderText="排行" HeaderStyle-CssClass="td_right td_left">
                    <ItemTemplate>
                        <asp:Image ID="Image1" runat="server" ImageUrl='<%# Convert.ToInt32(Eval("IndexId"))==1?"../images/0001.gif":Convert.ToInt32(Eval("IndexId"))==2?"../images/0002.gif":"../images/0003.gif" %>'
                            Visible='<%#Convert.ToInt32(Eval("IndexId"))<4 && Convert.ToDecimal(Eval("SaleTotals"))>0 %>' />
                        <strong>
                            <asp:Literal ID="Label1" runat="Server" Text='<%#Eval("IndexId")%>' Visible='<%# Convert.ToDecimal(Eval("SaleTotals"))>0 %>' /></strong>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="会员" HeaderStyle-CssClass="td_right td_left">
                    <ItemTemplate>
                        <asp:Label ID="Label2" runat="Server" Text='<%# Eval("UserName") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="订单数" HeaderStyle-CssClass="td_right td_left">
                    <ItemTemplate>
                        <asp:Label ID="Label3" runat="Server" Text='<%# Eval("OrderCount") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="消费金额" HeaderStyle-CssClass="table_title">
                    <ItemTemplate>
                        <Hi:FormatedMoneyLabel ID="FormatedMoneyLabelForAdmin1" Money='<%#Eval("SaleTotals") %>'
                            runat="server"></Hi:FormatedMoneyLabel>
                    </ItemTemplate>
                </asp:TemplateField>

            </Columns>
        </UI:Grid>


        <div class="blank12 clearfix"></div>

    </div>
    <!--数据列表底部功能区域-->
    <div class="bottomPageNumber clearfix">
        <div class="pageNumber">
            <div class="pagination" style="width: auto">
                <UI:Pager runat="server" ShowTotalPages="true" ID="pager1" />
            </div>
        </div>
    </div>

</form>
</asp:Content>
