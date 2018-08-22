<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminNew.master" AutoEventWireup="true" CodeBehind="CommissionsList.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Fenxiao.CommissionsList" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register Src="../Ascx/ucDateTimePicker.ascx" TagName="DateTimePicker" TagPrefix="Hi" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .suminfos {
            line-height: 12px;
            padding: 10px;
            margin-top: 10px;
        }

            .suminfos span {
                margin-right: 20px;
                color: #fb4310;
            }

        .red-all {
            color: #fb4310;
        }
    </style>
    <script>

        $(function () {

            ////添加第一列信息
            //var tabText = $(".nav-tabs .active").text().trim().replace("佣金","");
            //var $commisTb = $(".td_bg");
            //if ($commisTb.length>0) {
            //    $commisTb.eq(0).prepend("<td rowspan=" + $commisTb.length + " valign='middle'>" + tabText + "</td>");
            //}

        });

        function ShowDetail(ReferralUserId,storeName){
            var userid=<%=userid%>;
            var StartTime='<%=StartTime%>';
            var EndTime='<%=EndTime%>';
            DialogFrame("SubStoreCommissions.aspx?EndTime="+EndTime+"&StartTime="+StartTime +"&UserId="+userid+"&ReferralUserId="+ReferralUserId, "下级分销商贡献金额-当前【"+storeName+"】", 700, 480, function(){
            
            });
           
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form runat="server">
        <div class="page-header" style="margin-bottom: 5px">
            <h2>分销商佣金明细</h2>
        </div>
        <!--搜索-->
        <div style="padding: 0px 5px; margin-top: 0px; line-height: 40px">当前分销商：<%=CurrentStoreName %></div>

        <div class="set-switch">
            <div class="form-horizontal clearfix">

                <div class="form-group">
                    <label class="col-xs-1 pad control-label resetSize" for="setdate">时间范围：</label>
                    <div class="form-inline journal-query">
                        <div class="form-group">
                            <Hi:DateTimePicker CalendarType="StartDate" ID="calendarStartDate" runat="server" CssClass="form-control resetSize inputw150" />
                            &nbsp;&nbsp;至&nbsp;
                                   <Hi:DateTimePicker ID="calendarEndDate" runat="server" CalendarType="EndDate" CssClass="form-control resetSize inputw150" />
                        </div>
                        <asp:Button ID="btnQueryLogs" runat="server" class="btn resetSize btn-primary" Text="查询" OnClick="btnQueryLogs_Click" />&nbsp;&nbsp;
                                <div class="form-group">
                                    <label for="exampleInputName2">快速查看</label>
                                    <asp:Button ID="Button1" runat="server" class="btn resetSize btn-default" Text="最近7天" OnClick="Button1_Click1" />
                                    <asp:Button ID="Button4" runat="server" class="btn resetSize btn-default" Text="最近一个月" OnClick="Button4_Click1" />
                                </div>
                    </div>
                </div>
            </div>
            <div class="suminfos">佣金总额：<span>￥<%=CurrentTotal.ToString("f2") %></span>店铺销售佣金：<span>￥<asp:Literal ID="storeSum" Text="0.00" runat="server" /></span>下一级销售佣金：<span>￥<asp:Literal ID="fristSum" Text="0.00" runat="server" /></span>下二级销售佣金：<span>￥<asp:Literal ID="secondSum" Text="0.00" runat="server" /></span>其它佣金：<span>￥<asp:Literal ID="OtherSum" Text="0.00" runat="server" /></span></div>
        </div>

        <div class="play-tabs" style="margin-bottom: -10px;">
            <ul class="nav nav-tabs" role="tablist">
                <li role="presentation" class="<%=StoreDisplay %>">
                    <asp:LinkButton ID="Store" Text="店铺销售佣金" runat="server" OnClick="Store_Click"></asp:LinkButton></li>
                <li role="presentation" class="<%=FristDisplay %>">
                    <asp:LinkButton ID="Frist" Text="下一级销售佣金" runat="server" OnClick="Frist_Click"></asp:LinkButton></li>
                <li role="presentation" class="<%=SecondDisplay %>">
                    <asp:LinkButton ID="Second" Text="下二级销售佣金" runat="server" OnClick="Second_Click"></asp:LinkButton></li>
                <li role="presentation" class="<%=OtherDisplay %>">
                    <asp:LinkButton ID="Other" Text="其他佣金" runat="server" OnClick="Other_Click"></asp:LinkButton></li>
            </ul>
            <div style="font-size: 1px; line-height: 1px"></div>
        </div>
        <!--数据列表-->
        <asp:Repeater ID="reCommissions" runat="server">
            <HeaderTemplate>
                <div>
                    <table class="table table-hover mar table-bordered" style="table-layout: fixed" id="commisTb">
                        <thead>
                            <tr>
                                <th width="100">成交店铺</th>
                                <th width="120">分佣时间</th>
                                <th width="120">订单号</th>
                                <th width="100">订单金额</th>
                                <th width="100">佣金</th>

                            </tr>
                        </thead>
                        <tbody>
            </HeaderTemplate>
            <ItemTemplate>
                <tr class="td_bg">
                    <td>&nbsp;<%# ((int)Eval("CommType")==4  || (int)Eval("CommType")==3)?"<span style='color:#FF6600'>升级奖励</span>":Eval("fromStoreName")%></td>
                    <td width="200">&nbsp;<%# Eval("TradeTime", "{0:yyyy-MM-dd HH:mm:ss}")%></td>
                    <td>&nbsp;<a href="/Admin/trade/OrderDetails.aspx?OrderId=<%# Eval("OrderId")%>" target="_blank"><%# Eval("OrderId")%></a></td>

                    <td>&nbsp;￥<%# Eval("OrderTotal", "{0:F2}")%></td>
                    <td class="red-all">&nbsp;￥<%# Eval("CommTotal","{0:F2}")%></td>


                </tr>
            </ItemTemplate>
            <FooterTemplate>
                </tbody>
     </table>
     </div>
            </FooterTemplate>
        </asp:Repeater>


        <asp:Repeater ID="SubCommissions" runat="server">
            <HeaderTemplate>
                <div>
                    <table class="table table-hover mar table-bordered" style="table-layout: fixed" id="commisTb">
                        <thead>
                            <tr>
                                <th width="100">成交店铺</th>
                                <th width="120">订单数量</th>
                                <th width="120">订单总金额</th>
                                <th width="100">佣金总金额</th>
                                <th width="100">查看明细</th>

                            </tr>
                        </thead>
                        <tbody>
            </HeaderTemplate>
            <ItemTemplate>
                <tr class="td_bg">
                    <td>&nbsp;<%# Eval("StoreName")%></td>
                    <td width="200">&nbsp;<%# Eval("Ordernums")%></td>
                    <td>&nbsp;￥<%# Eval("OrderTotalSum", "{0:F2}")%></td>
                    <td class="red-all">&nbsp;￥<%# Eval("CommTotalSum","{0:F2}")%></td>
                    <td>&nbsp;<input type="button" class="btn btn-primary btn-xs" value="查看佣金明细" onclick="ShowDetail(<%# Eval("UserId")%>,'<%# Eval("StoreName")%>    ')" /></td>




                </tr>
            </ItemTemplate>
            <FooterTemplate>
                </tbody>
     </table>
     </div>
            </FooterTemplate>
        </asp:Repeater>
        <!-- --其他-->
        <asp:Repeater ID="otherCommissions" runat="server">
            <HeaderTemplate>
                <div>
                    <table class="table table-hover mar table-bordered" style="table-layout: fixed" id="commisTb">
                        <thead>
                            <tr>
                                <th width="100">佣金来源</th>
                                <th width="120">佣金说明</th>
                                <th width="200">分佣时间</th>
                                <th width="100">佣金</th>

                            </tr>
                        </thead>
                        <tbody>
            </HeaderTemplate>
            <ItemTemplate>
                <tr class="td_bg">
                    <td>&nbsp;<%#(int)Eval("CommType")==5 ?"<span style='color:#FF6600'>后台调整</span>": ((int)Eval("CommType")==4  || (int)Eval("CommType")==3)?"<span style='color:#FF6600'>升级奖励</span>":Eval("fromStoreName")%></td>
                    <td>&nbsp;<%# Eval("CommRemark")%></td>
                    <td width="200">&nbsp;<%# Eval("TradeTime", "{0:yyyy-MM-dd HH:mm:ss}")%></td>
                    
                    <td class="red-all">&nbsp;￥<%# Eval("CommTotal","{0:F2}")%></td>


                </tr>
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
            </div>
            <div class="page fr">
                <div class="pageNumber">
                    <div class="pagination" style="margin: 0px">
                        <UI:Pager runat="server" ShowTotalPages="true" ID="pager" />
                    </div>
                </div>
            </div>
        </div>

        <div class="clearfix" style="height: 30px"></div>

    </form>
</asp:Content>
