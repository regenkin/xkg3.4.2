<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MemberCouponList.aspx.cs"
    MasterPageFile="~/Admin/AdminNew.Master" Inherits="Hidistro.UI.Web.Admin.promotion.MemberCouponList" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Import Namespace="System.Web.UI.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        #ctl00_ContentPlaceHolder1_grdCoupondsList th {margin:0px;border-left:0px;border-right:0px;background-color:#F7F7F7;text-align:center; vertical-align:middle;}
        #ctl00_ContentPlaceHolder1_grdCoupondsList td {margin:0px;border-left:0px;border-right:0px;text-align:center;vertical-align:middle;}
        #searchDiv input {margin-right:20px;}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form id="thisForm" runat="server" class="form-horizontal">
        <div class="page-header">
            <h2>优惠券</h2>
            <%--<small>优惠券保存后，只能对发放总量和领券限制做编辑修改，请谨慎操作。</small>--%>
        </div>
        <div class="blank">
            <a href="NewCoupon.aspx" class="btn btn-primary">新增优惠券</a>
        </div>
        <div id="allProductsDiv">
            <div class="play-tabs">
                <div class="table-page">
                <ul class="nav nav-tabs" role="tablist">
                    <li id="tabHeader_couponds" role="presentation">
                        <a href="CouponsList.aspx?bFininshed=false">优惠券</a>
                    </li>
                    <li id="tabHeader_Finished" role="presentation">
                        <a href="CouponsList.aspx?bFininshed=true">已结束优惠券</a>
                    </li> 
                    <li id="tabHeader_memberCouponds" role="presentation" class="active">
                        <a href="MemberCouponList.aspx">领用/使用记录</a>
                    </li>                                       
                </ul>
                 <div class="page-box" style="margin-right:15px;">
                        <div class="page fr">
                            <div class="form-group">
                                <label for="exampleInputName2">每页显示数量：</label>
                                <UI:PageSize runat="server" ID="hrefPageSize" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="tab-pane active">
                     <div style="margin-top:5px;">
                        <div class="set-switch">
                        <div class="form-inline" id="searchDiv">                            
                            <asp:TextBox runat="server" CssClass="form-control resetSize" ID="txt_name" placeholder="优惠券名称" Width="110px"></asp:TextBox>   

                            <asp:TextBox runat="server" CssClass="form-control resetSize" ID="txt_orderNo" placeholder="订单编号" Width="110px"></asp:TextBox>
                           
                            <asp:Button CssClass="btn btn-primary resetSize" ID="btnSeach" runat="server" Text="查询" />

                        </div>
                        </div>

<%--                        <div class="select-page clearfix" style="margin-top: 20px;">
                        </div>--%>
                        <UI:Grid ID="grdCoupondsList" runat="server" ShowHeader="true" AutoGenerateColumns="false"
                            DataKeyNames="Id" HeaderStyle-CssClass="table_title" CssClass="table table-hover mar table-bordered"  GridLines="None" Width="100%">
                                            <Columns>                                                
                                                <asp:TemplateField HeaderText="优惠券名称" SortExpression="CouponName" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <%# Eval("CouponName")%>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="面值" HeaderStyle-HorizontalAlign="Center" ShowHeader="true">
                                                    <ItemTemplate>
                                                       ￥<%# Eval("CouponValue")%>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="使用条件" HeaderStyle-HorizontalAlign="Center" ShowHeader="true">
                                                    <ItemTemplate>
                                                        <%# Eval("useConditon")%>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="领取人" HeaderStyle-HorizontalAlign="Center" ShowHeader="true">
                                                    <ItemTemplate>
                                                        <%# Eval("userName")%>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="领取时间" HeaderStyle-HorizontalAlign="Center" ShowHeader="true">
                                                    <ItemTemplate>
                                                        <%# Eval("ReceiveDate")%>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="使用时间"  HeaderStyle-HorizontalAlign="Center" ShowHeader="true" >
                                                    <ItemTemplate>
                                                        <%# Eval("UsedDate")%>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="订单编号" HeaderStyle-HorizontalAlign="Center" ShowHeader="true">
                                                    <ItemTemplate>
                                                        <%# Eval("OrderNo")%>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="状态" HeaderStyle-HorizontalAlign="Center" ShowHeader="true">
                                                    <ItemTemplate>
                                                        <%# Eval("sStatus")%>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </ui:grid>
                        <div class="bottomPageNumber clearfix">
                            <div class="pageNumber">
                                <div class="pagination" style="width: auto">
                                    <UI:Pager runat="server" ShowTotalPages="true" ID="pager1"  />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>
</asp:Content>

