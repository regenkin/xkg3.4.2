<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminNew.Master" AutoEventWireup="true" CodeBehind="BuyerAlreadyPaid.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Trade.BuyerAlreadyPaid" %>

<%@ Import Namespace="Hidistro.Core" %>
<%@ Import Namespace="Hidistro.Entities.Sales" %>
<%@ Register Src="../Ascx/ucDateTimePicker.ascx" TagName="DateTimePicker" TagPrefix="Hi" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style type="text/css">
        #RemarkOrder .modal-body input.form-control {
            display: inline-block;
        }

        #liOper2 em {
            font-style: normal;
            color: red;
        }
    </style>
    <script type="text/javascript">
        var goUrl = "<%=Reurl%>&t=" + (new Date().getTime());
    </script>
    <form runat="server">
        <div class="page-header">
            <h2>等待发货</h2>
        </div>
        <div id="mytabl">
            <!-- Nav tabs -->
            <div class="table-page">
                <ul class="nav nav-tabs" style="display: none">

                    <li id="liOper0">
                        <asp:HyperLink runat="server" NavigateUrl="BuyerAlreadyPaid.aspx"><span>所有待发货</span></asp:HyperLink></li>
                    <li id="liOper1">
                        <asp:HyperLink runat="server" NavigateUrl="BuyerAlreadyPaid.aspx?stype=1"><span>货到付款订单</span></asp:HyperLink></li>
                    <li id="liOper2">
                        <asp:HyperLink runat="server" NavigateUrl="BuyerAlreadyPaid.aspx?stype=2"><span>退款中</span></asp:HyperLink></li>
                    <li id="liOper3">
                        <asp:HyperLink runat="server" NavigateUrl="BuyerAlreadyPaid.aspx?stype=3"><span>未打印快递单</span></asp:HyperLink></li>
                    <li id="liOper4">
                        <asp:HyperLink runat="server" NavigateUrl="BuyerAlreadyPaid.aspx?stype=4"><span>已打印快递单</span></asp:HyperLink></li>
                </ul>
            </div>
            <div class="tab-content">
                <div class="tab-pane active">
                    <div class="set-switch">
                        <div class="form-inline mb10">
                            <div class="form-group mr20">
                                <label for="sellshop1">　订单号：</label>
                                <asp:TextBox ID="txtOrderId" runat="server" CssClass="form-control resetSize inputw150" /><asp:Label ID="lblStatus"
                                    runat="server" Style="display: none;"></asp:Label>
                            </div>
                            <div class="form-group mr20">
                                <label for="sellshop1">　成交时间：</label>
                                <Hi:DateTimePicker CalendarType="StartDate" ID="calendarStartDate" runat="server" CssClass="form-control resetSize inputw150" />
                                至
                                <Hi:DateTimePicker ID="calendarEndDate" runat="server" CalendarType="EndDate" CssClass="form-control resetSize inputw150" />

                            </div>
                        </div>
                        <div class="form-inline mb10">
                            <div class="form-group mr20">
                                <label for="sellshop1">　收货人：</label>
                                <asp:TextBox ID="txtShopTo" runat="server" placeholder="姓名/手机号" CssClass="form-control resetSize inputw150" />
                            </div>
                            <div class="form-group">
                                <label for="sellshop1">收货人地区：</label>
                                <Hi:RegionSelector runat="server" ID="dropRegion" />
                            </div>
                        </div>
                        <div class="form-inline">
                            <div class="form-group mr20">
                                <label for="sellshop1">商品名称：</label>
                                <asp:TextBox ID="txtProductName" runat="server" CssClass="form-control resetSize inputw150" />

                                <asp:DropDownList ID="OrderFromList" runat="server" Width="107" Visible="false">
                                    <asp:ListItem Text="所有" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="普通订单" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="团购订单" Value="2"></asp:ListItem>
                                </asp:DropDownList><label for="ctl00_ContentPlaceHolder1_txtShopName">&nbsp; 分销店铺名称：</label>
                                <asp:TextBox ID="txtShopName" runat="server" CssClass="form-control resetSize inputw150" />
                                <label for="ctl00_ContentPlaceHolder1_txtUserName">　　买家昵称：</label>
                                <asp:TextBox ID="txtUserName" runat="server" CssClass="form-control resetSize inputw150" />
                                <asp:Button ID="btnSearchButton" runat="server" class="btn resetSize btn-primary" Text="查询" />
                                <a class=" mb5" onclick="resetform();" style="cursor: pointer">清除条件</a>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>





        <div>
            <div class="orderManagement">
                <div class="tableHeader mb10">
                    <table width="100%">
                        <thead>
                            <tr>
                                <th width="270">商品信息</th>
                                <th width="91">单价</th>
                                <th width="91">数量</th>
                                <th width="91">售后</th>
                                <th width="119">实付金额</th>
                                <th width="119">收货人</th>
                                <th width="119">订单来源</th>
                                <th width="120">操作</th>
                            </tr>
                        </thead>
                    </table>
                </div>
                <div class="select-page nomarg clearfix">
                    <div class="form-horizontal fl">
                        <div class="form-group mar forced">
                            <div class="checkbox">
                                <label>
                                    <input type="checkbox" id="selAll" class="allselect">全选</label>
                                &nbsp;&nbsp;&nbsp;&nbsp;
                            <input type="button" class="btn btn-info resetSize" value="指定物流" onclick="SetLogisticsForOrders()" />
                                <input type="button" class="btn btn-primary resetSize" value="打印快递单" onclick="printPosts()" />
                                <input type="button" class="btn btn-primary resetSize" value="打印发货单" onclick="printGoods()" />
                                <input type="button" class="btn btn-success resetSize" value="批量发货" onclick="SetLogisticsOrSendGoods(1)" />&nbsp;︱
                            <input type="button" class="btn btn-primary resetSize" value="批量备注" onclick="RemarkOrderFun('0', '', '', '', '')" />
                                <asp:Button ID="btnDeleteCheck" runat="server" Text="批量删除" CssClass="btn btn-danger resetSize" OnClientClick="return HiConform('<strong>订单删除后将进入订单回收站！</strong><p>确定要批量删除所选择的订单吗？</p>',this)" />
                                <asp:Button ID="btnExport" runat="server" Text="导出Excel" CssClass="btn btn-primary resetSize" OnClick="btnExport_Click" OnClientClick="return checkExport()" />
                            </div>
                        </div>
                    </div>
                    <div class="page fr">
                        <div class="form-group">
                            <label>每页显示数量：</label>
                            <UI:PageSize runat="server" ID="hrefPageSize" />
                            <input type="hidden" id="hidOrderId" runat="server" />
                        </div>
                    </div>
                </div>
            </div>


            <div class="orderManagementList orderManagementListOther">
                <ul>

                    <asp:Repeater ID="rptList" runat="server" OnItemCommand="rptList_ItemCommand" OnItemDataBound="rptList_ItemDataBound">


                        <ItemTemplate>
                            <li>
                                <div class="listTitle">
                                    <label>
                                        <input name="CheckBoxGroup" type="checkbox" value='<%#Eval("OrderId") %>' />
                                        订单编号：<%#Eval("OrderId") %><%#Globals.GetBarginShow(Eval("BargainDetialId")) %><asp:Literal
                                            ID="group" runat="server" Text='<%#  Eval("GroupBuyId")!=DBNull.Value&& Convert.ToInt32(Eval("GroupBuyId"))>0?"(团)":"" %>' /></label>&nbsp;<span><Hi:FormatedTimeLabel ID="lblStartTimes" Time='<%#Eval("OrderDate") %>' ShopTime="true"
                                                runat="server"></Hi:FormatedTimeLabel></span>
                                     <span style="margin-left: 40px;">
                                      <asp:Literal ID="WeiXinNickName" runat="server"></asp:Literal>
                                        <%--<Hi:OrderStatusLabel ID="lblOrderStatus" OrderStatusCode='<%# Eval("OrderStatus") %>'runat="server" />--%>
                                     </span><span class="ml10 logistics" tid="<%#Eval("ExpressCompanyAbb") %>" oid="<%#Eval("OrderID") %>" shipnum="<%#Eval("ShipOrderNumber") %>"><%--物流公司：未指定--%></span>
                                    <div style="float: right; padding-right: 10px; width: 26px" data-toggle="tooltip" data-placement="left" title="<%#Server.HtmlEncode(Eval("ManagerRemark").ToString()) %>" onclick="RemarkOrderFun('<%#Eval("OrderId") %>','<%#Eval("OrderDate") %>','<%#Eval("OrderTotal") %>','<%#Server.HtmlEncode(Eval("ManagerMark").ToString()) %>','<%#Server.HtmlEncode(Eval("ManagerRemark").ToString()) %>')">
                                        <Hi:OrderRemarkImage runat="server" DataField="ManagerMark" ID="OrderRemarkImageLink" />
                                    </div>
                                </div>
                                <table>
                                    <tbody>
                                        <tr>
                                            <td width="550">
                                                <div class="listInfoLeft">
                                                    <asp:Repeater ID="rptSubList" runat="server" OnItemDataBound="rptSubList_ItemDataBound">
                                                        <ItemTemplate>
                                                            <div class="orderInfolist clearfix">
                                                                <div class="orderImg fl clearfix">
                                                                    <div class="img fl">
                                                                        <Hi:ListImage runat="server" DataField="ThumbnailsUrl" Width="60" Height="60" />
                                                                    </div>
                                                                    <div class="imgInfo fl">
                                                                        <p class="setColor"><a href="/ProductDetails.aspx?productId=<%#Eval("ProductId") %>" target="_blank"><%# Eval("ItemDescription")%></a></p>
                                                                        <p><%#Eval("SKUContent") %></p>
                                                                    </div>
                                                                </div>
                                                                <p class="fl orderP"><%#Eval("Type").ToString()=="0"? "￥"+Eval("ItemListPrice","{0:f2}"):Eval("PointNumber")+"积分" %></p>
                                                                <p class="fl orderP"><%#Eval("Quantity") %></p>
                                                                <p class="fl orderP ">
                                                                   <%-- <Hi:OrderItemStatusLabel ID="lblOrderItemStatus" OrderStatusCode='<%# Eval("OrderItemsStatus") %>'
                                                                        runat="server" />--%>
                                                                    <span  style="color:red; ">
                                                                        <hi:OrderCombineStatus id="lbOrderCombineStatus" OrderStatusCode='<%# Eval("OrderStatus") %>' OrderItemsStatusCode='<%# Eval("OrderItemsStatus") %>' runat="server"></hi:OrderCombineStatus>
                                                                    </span>
                                                                </p>
                                                            </div>
                                                        </ItemTemplate>
                                                    </asp:Repeater>
                                                </div>
                                            </td>
                                            <td width="470">
                                                <div class="listInfoRight clearfix">
                                                    <div class="paymentAmount fl">
                                                        <p>
                                                            <asp:Literal ID="litOtherInfo" runat="server"></asp:Literal>
                                                        </p>
                                                    </div>
                                                    <div class="username fl">
                                                        <p>
                                                            <%#Eval("ShipTo") %><Hi:WangWangConversations runat="server" ID="WangWangConversations" WangWangAccounts='<%#Eval("Wangwang") %>' />
                                                            <br>
                                                            <%#Eval("CellPhone") %>
                                                        </p>
                                                        <%-- <span class="setColor glyphicon glyphicon-comment"></span>--%>
                                                        <span data-toggle="tooltip" title="<%#Eval("Remark") %>" style="<%#Eval("Remark").ToString().Trim()==""?"display:none": ""%>">
                                                            <img src="../images/xi.gif" border="0"></span>
                                                    </div>
                                                    <div class="shopSource fl">
                                                        <p><%#Eval("ReferralUserId").ToString()=="" ? "主站" : Eval("ReferralUserId").ToString()=="0" ? "主站" : "" + Eval("StoreName")%></p>
                                                    </div>
                                                    <div class="operation fr">
                                                        <a href="OrderDetails.aspx?OrderId=<%#Eval("OrderID") %>" target="_blank" class="btn btn-info resetSize inputw100 btnvieworders bl mb5">查看详情</a>
                                                        <%#GetSpitLink(Eval("SplitState") ,Eval("OrderStatus"),Eval("OrderID")) %>
                                                        <input type="button" id="btnModifyPrice" visible="false" runat="server" class="btn btn-primary resetSize inputw100" value="一键改价" />
                                                        <asp:Button ID="btnPayOrder" runat="server" CssClass="btn btn-warning resetSize inputw100" Text="确认付款" CommandArgument='<%# Eval("OrderId") %>' CommandName="CONFIRM_PAY" OnClientClick="return HiConform('<strong>订单将变为已付款状态，确认客户已付款？</strong><p>如果客户已经通过其他途径支付了订单款项，您可以使用此操作修改订单状态</p>',this);" Visible="false" />
                                                        <input type="button" class="btn btn-danger resetSize inputw100" id="btnCloseOrderClient" runat="server" visible="false" value="关闭订单" />
                                                        <input type="button" id="btnSendGoods" visible="false" runat="server" class="btn btn-success resetSize inputw100" value="发货" />
                                                        <asp:Button ID="btnConfirmOrder" CssClass="btn btn-warning resetSize inputw100" runat="server" Text="确认收货" CommandArgument='<%# Eval("OrderId") %>' CommandName="FINISH_TRADE" DeleteMsg="确认要完成该订单吗？"
                                                            Visible="false" OnClientClick="return HiConform('<p>订单将变为完成状态，确认该订单已收货吗？</p>',this)" />
                                                        <a href="javascript:;" onclick="return CheckRefund(this.title)"
                                                            runat="server" id="lkbtnCheckRefund" visible="false" title='<%# Eval("OrderId") %>'>确认退款</a>
                                                        <a href="javascript:void(0)" onclick="return CheckReturn(this.title)" runat="server"
                                                            id="lkbtnCheckReturn" visible="false" title='<%# Eval("OrderId") %>'>确认退货</a>
                                                        <a href="javascript:void(0)" onclick="return CheckReplace(this.title)" runat="server"
                                                            id="lkbtnCheckReplace" visible="false" title='<%# Eval("OrderId") %>'>确认换货</a>&nbsp;
                                                    </div>
                                                </div>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </li>
                        </ItemTemplate>

                    </asp:Repeater>


                </ul>
            </div>
        </div>







        <!--选项卡-->
        <div class="dataarea mainwidth">



            <!--结束-->
            <div class="functionHandleArea clearfix m_none" style="display: none">
                <div class="blank8 clearfix">
                </div>
                <div class="batchHandleArea">
                    <ul>
                        <li class="batchHandleButton"><span class="signicon"></span>
                            <span class="downproduct">
                                <a href="javascript:downOrder()">下载配货单</a></span> <span class="sendproducts"><a href="javascript:batchSend()"
                                    onclick="">批量发货</a></span> </li>
                    </ul>
                </div>
            </div>



            <!--数据列表底部功能区域-->
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


        <!--关闭订单--->
        <div class="modal fade" id="closeOrder">
            <div class="modal-dialog">
                <div class="modal-content form-horizontal">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">关闭订单</h4>
                    </div>
                    <div class="modal-body">
                        <p style="color: red">
                            关闭交易?请您确认已经通知买家,并已达成一致意见,您单方面关闭交易,将可能导致交易纠纷
                        </p>
                        <p class="pt5">
                            <span class="frame-span frame-input110">关闭该订单的理由:</span>
                            <Hi:CloseTranReasonDropDownList runat="server" ID="ddlCloseReason" CssClass="form-control inputw200 inl" />
                        </p>
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="btnCloseOrder" runat="server" CssClass="btn btn-success" Text="关闭订单" />
                        <button type="button" class="btn btn-default" data-dismiss="modal">关闭</button>
                    </div>
                </div>
            </div>
        </div>

        <div class="modal fade" id="RemarkOrder">
            <div class="modal-dialog">
                <div class="modal-content form-horizontal">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">修改备注</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group isordersbatch">
                            <label class="col-xs-3 control-label">订单号：</label><div class="col-xs-8 exitpa" id="spanOrderId"></div>
                        </div>
                        <div class="form-group isordersbatch">
                            <label class="col-xs-3 control-label">提交时间：</label><div class="col-xs-8 exitpa" id="lblOrderDateForRemark"></div>
                        </div>
                        <div class="form-group isordersbatch">
                            <label class="col-xs-3 control-label">订单实收款(元)：</label><div class="col-xs-8 exitpa" style="color: red; font-weight: bold;">
                                <Hi:FormatedMoneyLabel
                                    ID="lblOrderTotalForRemark" runat="server" />
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-xs-3 control-label" style="padding-top: 0px; line-height: 22px"><em>*</em>标志：</label>
                            <Hi:OrderRemarkImageRadioButtonList
                                runat="server" ID="orderRemarkImageForRemark" Width="238" />
                        </div>

                        <div class="form-group">
                            <label class="col-xs-3 control-label">备注：</label><asp:TextBox ID="txtRemark" TextMode="MultiLine" runat="server"
                                Width="300" Height="50" />
                            <label class="col-xs-3 control-label"></label>
                            <small>(备注信息仅后台可见)</small>
                        </div>

                        <div class="modal-footer">
                            <input type="text" id="txtcategoryId" runat="server" class="hide" />
                            <asp:Button ID="btnRemark" runat="server" Text="保存" CssClass="btn btn-success" OnClientClick="return ModifyMemo()" />
                            <button type="button" class="btn btn-default" data-dismiss="modal">关闭</button>
                        </div>
                    </div>
                </div>
            </div>
        </div>



        <div id="DownOrder" style="display: none;">
            <div class="frame-content" style="text-align: center;">
                <input type="button" id="btnorderph" onclick="javascript: Setordergoods();" class="submit_DAqueding"
                    value="订单配货表" />
                &nbsp;
            <input type="button" id="Button1" onclick="javascript: Setproductgoods();" class="submit_DAqueding"
                value="商品配货表" />
                <p>
                    导出内容只包括等待发货状态的订单
                </p>
                <p>
                    订单配货表不会合并相同的商品,商品配货表则会合并相同的商品。
                </p>
            </div>
        </div>
        <!--确认退款--->
        <div id="CheckRefund" style="display: none;">
            <div class="frame-content">
                <p>
                    <em>执行本操作前确保：<br />
                        1.买家已付款完成，并确认无误； 2.确认买家的申请退款方式。</em>
                </p>
                <table cellpadding="0" cellspacing="0" width="100%" border="0" class="fram-retreat">
                    <tr>
                        <td align="right" width="30%">订单号:
                        </td>
                        <td align="left" class="bd_td">&nbsp;<asp:Label ID="lblOrderId" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">订单金额:
                        </td>
                        <td align="left" class="bd_td">&nbsp;<asp:Label ID="lblOrderTotal" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td align="right">买家退款方式:
                        </td>
                        <td align="left" class="bd_td">&nbsp;<asp:Label ID="lblRefundType" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">退款原因:
                        </td>
                        <td align="left" class="bd_td">&nbsp;<asp:Label ID="lblRefundRemark" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">联系人:
                        </td>
                        <td align="left" class="bd_td">&nbsp;<asp:Label ID="lblContacts" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">电子邮件:
                        </td>
                        <td align="left" class="bd_td">&nbsp;<asp:Label ID="lblEmail" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">联系电话:
                        </td>
                        <td align="left" class="bd_td">&nbsp;<asp:Label ID="lblTelephone" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">联系地址:
                        </td>
                        <td align="left" class="bd_td">&nbsp;<asp:Label ID="lblAddress" runat="server"></asp:Label>
                        </td>
                    </tr>
                </table>
                <p>
                    <span class="frame-span frame-input100" style="margin-right: 10px;">管理员备注:</span>
                    <span>
                        <asp:TextBox ID="txtAdminRemark" runat="server" CssClass="form-control resetSize inputw150" Width="243" /></span>
                </p>
                <br />
                <div style="text-align: center;">
                    <input type="button" id="Button2" onclick="javascript: acceptRefund();" class="submit_DAqueding"
                        value="确认退款" />
                    &nbsp;
                <input type="button" id="Button3" onclick="javascript: refuseRefund();" class="submit_DAqueding"
                    value="拒绝退款" />
                </div>
            </div>
        </div>
        <!--确认退货--->
        <div id="CheckReturn" style="display: none;">
            <div class="frame-content">
                <p>
                    <em>执行本操作前确保：<br />
                        1.已收到买家寄换回来的货品，并确认无误； 2.确认买家的申请退款方式。</em>
                </p>
                <table cellpadding="0" cellspacing="0" width="100%" border="0" class="fram-retreat">
                    <tr>
                        <td align="right" width="30%">订单号:
                        </td>
                        <td align="left" class="bd_td">&nbsp;<asp:Label ID="return_lblOrderId" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">订单金额:
                        </td>
                        <td align="left" class="bd_td">&nbsp;<asp:Label ID="return_lblOrderTotal" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td align="right">买家退款方式:
                        </td>
                        <td align="left" class="bd_td">&nbsp;<asp:Label ID="return_lblRefundType" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">退货原因:
                        </td>
                        <td align="left" class="bd_td">&nbsp;<asp:Label ID="return_lblReturnRemark" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">联系人:
                        </td>
                        <td align="left" class="bd_td">&nbsp;<asp:Label ID="return_lblContacts" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">电子邮件:
                        </td>
                        <td align="left" class="bd_td">&nbsp;<asp:Label ID="return_lblEmail" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">联系电话:
                        </td>
                        <td align="left" class="bd_td">&nbsp;<asp:Label ID="return_lblTelephone" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">联系地址:
                        </td>
                        <td align="left" class="bd_td">&nbsp;<asp:Label ID="return_lblAddress" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">退款金额:
                        </td>
                        <td align="left" class="bd_td">&nbsp;<asp:TextBox ID="return_txtRefundMoney" runat="server" />
                        </td>
                    </tr>
                </table>
                <p>
                    <span class="frame-span frame-input100" style="margin-right: 10px;">管理员备注:</span>
                    <span>
                        <asp:TextBox ID="return_txtAdminRemark" runat="server" CssClass="form-control resetSize inputw150" Width="243" /></span>
                </p>
                <br />
                <div style="text-align: center;">
                    <input type="button" id="Button4" onclick="javascript: acceptReturn();" class="submit_DAqueding"
                        value="确认退货" />
                    &nbsp;
                <input type="button" id="Button5" onclick="javascript: refuseReturn();" class="submit_DAqueding"
                    value="拒绝退货" />
                </div>
            </div>
        </div>
        <!--确认换货--->
        <div id="CheckReplace" style="display: none;">
            <div class="frame-content">
                <p>
                    <em>执行本操作前确保：<br />
                        1.已收到买家寄还回来的货品，并确认无误； </em>
                </p>
                <table cellpadding="0" cellspacing="0" width="100%" border="0" class="fram-retreat">
                    <tr>
                        <td align="right" width="30%">订单号:
                        </td>
                        <td align="left" class="bd_td">&nbsp;<asp:Label ID="replace_lblOrderId" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">订单金额:
                        </td>
                        <td align="left" class="bd_td">&nbsp;<asp:Label ID="replace_lblOrderTotal" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td align="right">换货备注:
                        </td>
                        <td align="left" class="bd_td">&nbsp;<asp:Label ID="replace_lblComments" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">联系人:
                        </td>
                        <td align="left" class="bd_td">&nbsp;<asp:Label ID="replace_lblContacts" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">电子邮件:
                        </td>
                        <td align="left" class="bd_td">&nbsp;<asp:Label ID="replace_lblEmail" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">联系电话:
                        </td>
                        <td align="left" class="bd_td">&nbsp;<asp:Label ID="replace_lblTelephone" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">联系地址:
                        </td>
                        <td align="left" class="bd_td">&nbsp;<asp:Label ID="replace_lblAddress" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">邮政编码:
                        </td>
                        <td align="left" class="bd_td">&nbsp;<asp:Label ID="replace_lblPostCode" runat="server"></asp:Label>
                        </td>
                    </tr>
                </table>
                <p>
                    <span class="frame-span frame-input100" style="margin-right: 10px;">管理员备注:</span>
                    <span>
                        <asp:TextBox ID="replace_txtAdminRemark" runat="server" CssClass="form-control resetSize inputw150" Width="243" /></span>
                </p>
                <br />
                <div style="text-align: center;">
                    <input type="button" id="Button6" onclick="javascript: acceptReplace();" class="submit_DAqueding"
                        value="确认换货" />
                    &nbsp;
                <input type="button" id="Button7" onclick="javascript: refuseReplace();" class="submit_DAqueding"
                    value="拒绝换货" />
                </div>
            </div>
        </div>
        <div style="display: none">
            <input type="hidden" id="hidOrderTotal" runat="server" />
            <input type="hidden" id="hidRefundType" runat="server" />
            <input type="hidden" id="hidRefundMoney" runat="server" />
            <input type="hidden" id="hidAdminRemark" runat="server" />
            <asp:Button ID="btnAcceptRefund" runat="server" CssClass="submit_DAqueding" Text="确认退款" />
            <asp:Button ID="btnRefuseRefund" runat="server" CssClass="submit_DAqueding" Text="拒绝退款" />
            <asp:Button ID="btnAcceptReturn" runat="server" CssClass="submit_DAqueding" Text="确认退货" />
            <asp:Button ID="btnRefuseReturn" runat="server" CssClass="submit_DAqueding" Text="拒绝退货" />
            <asp:Button ID="btnAcceptReplace" runat="server" CssClass="submit_DAqueding" Text="确认换货" />
            <asp:Button ID="btnRefuseReplace" runat="server" CssClass="submit_DAqueding" Text="拒绝换货" />
            <asp:Button ID="btnOrderGoods" runat="server" CssClass="submit_DAqueding" Text="订单配货表" />&nbsp;
        <asp:Button runat="server" ID="btnProductGoods" Text="商品配货表" CssClass="submit_DAqueding" />
        </div>
    </form>
    <script src="order.helper.js" type="text/javascript"></script>
    <script type="text/javascript">
        var formtype = "";
        function resetform() {
            //document.getElementById("aspnetForm").reset();
            window.location.href = "BuyerAlreadyPaid.aspx?stype=<%=stype%>";
        }
        function ShowOrderState() {
            var status = "<%=stype%>";// $("#ctl00_ContentPlaceHolder1_lblStatus").html();
            $("#liOper" + status).attr("class", "active");
        }
        $(document).ready(function () {
            ShowOrderState();
            $.ajax({
                url: "BuyerAlreadyPaid.aspx",
                type: "post",
                data: "isCallback=GetStype",
                datatype: "json",
                success: function (json) {
                    if (json.type == "1") {
                        if (json.buyalreadypaidcount > 0) {
                            $("#liOper0").find("span").html($("#liOper0").find("span").html() + "(" + json.buyalreadypaidcount + ")");
                            if (json.count1 > 0) {
                                $("#liOper1").find("span").html($("#liOper1").find("span").html() + "(" + json.count1 + ")");
                            }
                            if (json.count2 > 0) {
                                $("#liOper2").find("span").html($("#liOper2").find("span").html() + "<em>(" + json.count2 + ")</em>");
                            }
                            if (json.count3 > 0) {
                                $("#liOper3").find("span").html($("#liOper3").find("span").html() + "(" + json.count3 + ")");
                            }
                            if (json.count4 > 0) {
                                var text = $("#liOper4").find("span").text();
                                $("#liOper4").find("span").html($("#liOper4").find("span").html() + "(" + json.count4 + ")");
                            }
                        }
                    }
                    $(".nav.nav-tabs").show();
                }
            });


            $("[data-toggle='tooltip']").tooltip({ html: false });
        });
        //function getlinksitem(itemid,itemcount,name,type){
        //    var temp="";
        //    if(itemcount>0){
        //        temp="<em>("+itemcount+")</em>";
        //    }
        //}
        //备注信息
        function RemarkOrderFun(OrderId, OrderDate, OrderTotal, managerMark, managerRemark) {
            if (OrderId != "0") {
                $(".isordersbatch").hide();
                arrytext = null;
                formtype = "remark";
                $("#ctl00_ContentPlaceHolder1_lblOrderTotalForRemark").html(OrderTotal);
                $("#ctl00_ContentPlaceHolder1_hidOrderId").val(OrderId);
                $("#spanOrderId").html(OrderId);
                $("#lblOrderDateForRemark").html(OrderDate);
                for (var i = 0; i <= 5; i++) {
                    if (document.getElementById("ctl00_ContentPlaceHolder1_orderRemarkImageForRemark_" + i).value == managerMark) {
                        setArryText("ctl00_ContentPlaceHolder1_orderRemarkImageForRemark_" + i, "true");
                        $("#ctl00_ContentPlaceHolder1_orderRemarkImageForRemark_" + i).click();//.attr("checked","checked");
                    }
                    else {
                        $("#ctl00_ContentPlaceHolder1_orderRemarkImageForRemark_" + i).attr("checked", false);
                    }
                }
                setArryText("ctl00_ContentPlaceHolder1_txtRemark", managerRemark);
                $("#ctl00_ContentPlaceHolder1_txtRemark").val(managerRemark);
                //DialogShow("修改备注", 'updateremark', 'RemarkOrder', 'ctl00_ContentPlaceHolder1_btnRemark');

                $('#RemarkOrder').modal('toggle').children().css({
                    width: '520px',
                    height: '180px'
                })
                $("#RemarkOrder").modal({ show: true });
            } else {
                //批量
                var IsTrue = false;
                var obj = document.getElementsByName("CheckBoxGroup");

                for (var i = 0; i < obj.length; i++) {
                    if (obj[i].checked == true) {
                        IsTrue = true;
                        break;
                    }
                }
                if (!IsTrue) {
                    ShowMsg('请选择您要操作的选项!', false);
                    return false;
                }
                $("#ctl00_ContentPlaceHolder1_hidOrderId").val(OrderId);
                $(".isordersbatch").hide();
                $('#RemarkOrder').modal('toggle').children().css({
                    width: '520px',
                    height: '180px'
                })
                $("#RemarkOrder").modal({ show: true });
            }
        }
        /*关闭订单*/
        function CloseOrderFun(orderId) {
            arrytext = null;
            formtype = "close";
            $("#ctl00_ContentPlaceHolder1_hidOrderId").val(orderId);
            //DialogShow("关闭订单", 'closeframe', 'closeOrder', 'ctl00_ContentPlaceHolder1_btnCloseOrder');
            $('#closeOrder').modal('toggle').children().css({
                width: '520px',
                height: '260px'
            })
            $("#closeOrder").modal({ show: true });
        }

        function checkExport() {
            var IsTrue = false;
            var obj = document.getElementsByName("CheckBoxGroup");
            for (var i = 0; i < obj.length; i++) {
                if (obj[i].checked == true) {
                    IsTrue = true;
                    break;
                }
            }
            if (!IsTrue) {
                ShowMsg('请选择您要导出到Excel文档的项目!', false);
                return false;
            } else {
                return true;
            }
        }
        function ValidationCloseReason() {
            var reason = document.getElementById("ctl00_ContentPlaceHolder1_ddlCloseReason").value;
            if (reason == "请选择关闭的理由") {
                ShowMsg("请选择关闭的理由", false);
                return false;
            }
            setArryText("ctl00_ContentPlaceHolder1_ddlCloseReason", reason);
            return true;
        }
        function SetLogisticsOrSendGoods(typeid) {
            var type = "saveorders";
            var typename = "指定物流";
            if (typeid == 1) {
                type = "sendorders";
                typename = "发货";
            }
            var orderIds = "";
            $("input:checked[name='CheckBoxGroup']").each(function () {
                orderIds += $(this).val() + ",";
            });
            if (orderIds == "") {
                ShowMsg("请选要批量" + typename + "的订单", false);
            }
            else {
                DialogFrame('SendOrderGoods.aspx?type=' + type + '&OrderId=' + orderIds + '&reurl=' + encodeURIComponent(goUrl), '批量' + typename, 750, 220)
            }
        }
        function SetLogisticsForOrders() {
            var type = "saveorders";
            var typename = "指定物流";
            var orderIds = "";
            $("input:checked[name='CheckBoxGroup']").each(function () {
                orderIds += $(this).val() + ",";
            });
            if (orderIds == "") {
                ShowMsg("请选要批量" + typename + "的订单", false);
            }
            else {
                DialogFrame('SetLogistics.aspx?type=' + type + '&OrderId=' + orderIds + '&reurl=' + encodeURIComponent(goUrl), '批量' + typename, 390, 135)
            }
        }
        ////批量发货
        //function batchSend() {
        //    var orderIds = "";
        //    $("input:checked[name='CheckBoxGroup']").each(function () {
        //        orderIds += $(this).val() + ",";
        //    });
        //    if (orderIds == "") {
        //        ShowMsg("请选要发货的订单", false);
        //    }
        //    else {
        //        DialogFrame("../sales/BatchSendOrderGoods.aspx?OrderIds=" + orderIds, "批量发货", null, null, function () { location.reload(); });
        //    }
        //}
        /*订单配送表*/
        function Setordergoods() {
            $("#ctl00_ContentPlaceHolder1_btnOrderGoods").trigger("click");
        }
        /*商品配货表*/
        function Setproductgoods() {
            $("#ctl00_ContentPlaceHolder1_btnProductGoods").trigger("click");
        }
        // 批量打印发货单
        function printGoods() {
            var orderIds = "";
            $("input:checked[name='CheckBoxGroup']").each(function () {
                orderIds += $(this).val() + ",";
            });
            if (orderIds == "") {
                ShowMsg("请选要打印的订单", false);
            }
            else {
                var url = "BatchPrintSendOrderGoods.aspx?OrderIds=" + orderIds;
                //alert(url)///Admin/sales/BatchPrintSendOrderGoods.aspx?OrderIds=201509091156365,201509098685213,201509092394383,201509091543245,201509010022093,
                window.open(url, "批量打印发货单", "width=800, top=0, left=0, toolbar=no, menubar=no, scrollbars=yes, resizable=no,location=no, status=no");
            }
        }

        /*批量打印快递单*/
        function printPosts() {
            var orderIds = "";
            $("input:checked[name='CheckBoxGroup']").each(function () {
                orderIds += $(this).val() + ",";
            });
            if (orderIds == "") {
                ShowMsg("请选要打印的订单", false);
            }
            else {
                var url = "BatchPrintData.aspx?OrderIds=" + orderIds;
                DialogFrame(url, "批量打印快递单", 550, 430);
            }
        }
        var arrytempstr = null;
        function setArryText(keyname, keyvalue) {
            var temptex = "\"" + keyname + "\":\"" + escape(keyvalue) + "\"";
            if (arrytempstr != null) {
                arrytempstr = arrytempstr.substr(0, arrytempstr.length - 1);
                arrytempstr += "," + temptex + "}";

            } else {
                arrytempstr = "{" + temptex + "}"
            }
            arrytext = $.parseJSON(arrytempstr);
        }

        function getArrayString(keyname, keyvalue) {
            var temptex = "\"" + keyname + "\":\"" + keyvalue + "\"";
            return temptex;
        }

        function ModifyMemo() {
            switch (formtype) {
                case "remark":
                    arrytext = null;
                    $radioId = $("input[type='radio'][name='ctl00$ContentPlaceHolder1$orderRemarkImageForRemark']:checked")[0];
                    if ($radioId == null || $radioId == "undefined") {
                        ShowMsg('请选择标志图标', false);
                        return false;
                    }
                    setArryText($radioId.id, "true");
                    setArryText("ctl00_ContentPlaceHolder1_txtRemark", $("#ctl00_ContentPlaceHolder1_txtRemark").val());
                    break;
                case "shipptype":
                    return ValidationShippingMode();
                    break;
                case "close":
                    return ValidationCloseReason();
                    break;
            };
            return true;
        }
        // 下载配货单
        function downOrder() {
            var orderIds = "";
            $("input:checked[name='CheckBoxGroup']").each(function () {
                orderIds += $(this).val() + ",";
            }
             );
            if (orderIds == "") {
                ShowMsg("请选要下载配货单的订单", false);
            }
            else {
                ShowMessageDialog("下载配货批次表", "downorder", "DownOrder");
            }
        }
        $('#selAll').click(function () {
            if ($(this).prop("checked")) {
                $('.orderManagementList input[type="checkbox"]').prop('checked', true);
            } else {
                $('.orderManagementList input[type="checkbox"]').prop('checked', false);
            }
        });
        $(function () {
            $(".datalist img[src$='tui.gif']").each(function (item, i) {
                $parent_link = $(this).parent();
                $parent_link.attr("href", "javascript:DialogFrame('sales/" + $parent_link.attr("href") + "','退款详细信息',null,null);");
            });


            $('#ctl00_ContentPlaceHolder1_dropRegion').find('select').each(function () {
                $(this).removeClass();
                $(this).addClass('form-control inl autow mr5 resetSize');
            });
        });
    </script>
</asp:Content>

