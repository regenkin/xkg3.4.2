<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Admin/AdminNew.Master"  CodeBehind="RecycleStationDetail.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Trade.RecycleStationDetail" %>

 
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        #profile p {
            line-height: 30px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form runat="server">
        <asp:HiddenField ID="hdfOrderID" runat="server" />
        <asp:HiddenField ID="hdProductID" runat="server" />
        <asp:HiddenField ID="hdSkuID" runat="server" />
        <asp:HiddenField ID="hdReturnsId" runat="server" />
        <asp:HiddenField ID="hdHasNewKey" runat="server" />
        <asp:HiddenField ID="hdExpressUrl" runat="server" />
        <asp:HiddenField ID="hdCompanyCode" runat="server" />
        
        <input type="hidden" id="otherdiscountprice" value="<%=otherDiscountPrice %>" />
        <div class="page-header">
            <h2>
                <ul class="clearfix">
                    <li class="active">订单详情</li>
                </ul>
            </h2>
        </div>
        <div class="orderProcess" id="divOrderProcess" runat="server">
            <ul class="clearfix">
                <li class="clearfix ok">
                    <h5 class="fl">1.买家下单</h5>
                    <span class="triangle fl"></span>
                </li>
                <li class="clearfix <%=ProcessClass2 %>">
                    <h5 class="fl">2.买家付款</h5>
                    <span class="triangle fl"></span>
                    <span class="triangle-topright"></span>
                    <span class="triangle-bottomright"></span>
                </li>
                <li class="clearfix <%=ProcessClass3 %>">
                    <h5 class="fl">3.发货</h5>
                    <span class="triangle fl"></span>
                    <span class="triangle-topright"></span>
                    <span class="triangle-bottomright"></span>
                </li>
                <li class="clearfix nomar <%=ProcessClass4 %>">
                    <h5>4.买家确认收货</h5>
                    <span class="triangle-topright"></span>
                    <span class="triangle-bottomright"></span>
                </li>
            </ul>
            <ul class="datatime clearfix mb10">
                <li>
                    <asp:Literal ID="litOrderDate" runat="server"></asp:Literal></li>
                <li>
                    <asp:Literal ID="litPayDate" runat="server"></asp:Literal></li>
                <li>
                    <asp:Literal ID="litShippingDate" runat="server"></asp:Literal></li>
                <li>
                    <asp:Literal ID="litFinishDate" runat="server"></asp:Literal></li>
            </ul>
            <span class="datatime"></span>
        </div>
        <div class="set-switch resetBorder">
            <h3><strong>当前订单状态：<Hi:OrderStatusLabel ID="lblOrderStatus" runat="server" />
                <asp:Label runat="server" ID="lbCloseReason" Text="关闭原因：" ForeColor="#777777" Font-Size="10" style="font-weight:normal;">
                    <asp:Label runat="server" ID="lbReason"></asp:Label></asp:Label></strong></h3>
            <p class="mb10">订单号：<asp:Literal runat="server" ID="litOrderId" /></p>
           <%-- <div class="orderRemarks mb20" id="divRemarkShow" runat="server">
                <p>备注：<Hi:OrderRemarkImage runat="server" DataField="ManagerMark" ID="OrderRemarkImageLink" /></p>
                <p>
                    <asp:Literal ID="litManagerRemark" runat="server"></asp:Literal>
                </p>
            </div>--%>
        <%--    <button type="button" class="btn btn-primary btn-sm inputw100" onclick="ShowRemarkOrder()">备注</button>
            <input type="button" id="btnModifyAddr" runat="server" class="btn btn-primary btn-sm inputw100" value="修改收货地址" visible="false" />
            <input type="button" id="btnModifyPrice" runat="server" class="btn btn-primary btn-sm inputw100" value="修改价格" />
            <asp:Button ID="btnConfirmPay" runat="server" CssClass="btn btn-warning btn-sm inputw100" Text="确认付款" OnClientClick="return HiConform('<strong>订单将变为已付款状态，确认客户已付款？</strong><p>如果客户已经通过其他途径支付了订单款项，您可以使用此操作修改订单状态</p>',this);" Visible="false" OnClick="btnConfirmPay_Click" />
            <input type="button" id="btnSendGoods" runat="server" class="btn btn-success btn-sm inputw100" value="发货" onclick="ShowSend()" />
            <input type="button" id="btnClocsOrder" runat="server" class="btn btn-danger btn-sm inputw100" value="关闭订单" onclick="ShowCloseOrder()" />
            <input type="button" id="btnConfirmOrder" runat="server" class="btn btn-danger btn-sm inputw100" value="确认收货" visible="false" />
            <input type="button" id="btnViewLogistic" runat="server" class="btn btn-info btn-sm inputw100" value="查看物流" visible="false" onclick="ShowLogistic()" />--%>
             <asp:Button ID="btnRestoreCheck" runat="server" Text="还原" CssClass="btn btn-primary resetSize"  OnClientClick="return HiConform('<strong>还原订单</strong><p>订单还原后，可在所有订单中查看订单信息。</p>',this)" />
             <input type="button" runat="server" id="btnDeleteCheck" value="彻底删除" class="btn btn-danger resetSize" onclick="ShowDeleteOrder()" />
        </div>
        <div>
            <ul class="nav nav-tabs" role="tablist" id="myTab">
                <li role="presentation" class="active"><a href="#home" aria-controls="home" role="tab" data-toggle="tab">订单信息</a></li>
                <li role="presentation"><a href="#profile" aria-controls="profile" role="tab" data-toggle="tab">收货和物流信息</a></li>
            </ul>
            <div class="tab-content">
                <div role="tabpanel" class="tab-pane active" id="home">
                    <div class="set-switch mt20">
                        <h5 class="mb10"><strong>买家信息</strong></h5>
                        <ul class="clearfix buyerInfo">
                            <li style="width:375px;">
                                <p class="mb10"><span>姓名：</span><asp:Literal runat="server" ID="litRealName" /></p>
                                <p class="mb10"><span>所在地区：</span><asp:Literal runat="server" ID="litShippingRegion"></asp:Literal></p>
                                <p><span>买家留言：</span><asp:Literal ID="litRemark" runat="server"></asp:Literal></p>
                            </li>
                            <li>
                                <p class="mb10"><span>手机号：</span><asp:Literal runat="server" ID="litUserTel" /></p>
                                <p><span>付款方式：</span><asp:Literal ID="litPayType" runat="server"></asp:Literal></p>
                                <p class="mt10"><span>收货时段：</span><asp:Literal ID="litShipToDate" runat="server"></asp:Literal></p>
                            </li>
                            <li>
                                <p class="mb10"><span>用户名：</span><asp:Literal runat="server" ID="litUserName" /></p>
                                <p><span>微信昵称：</span><asp:Literal runat="server" ID="litWeiXinNickName" /></p>
                            </li>
                        </ul>
                    </div>
                    <div class="ordertable">
                        <table class="table" width="100%">
                            <thead>
                                <tr>
                                    <th class="nocenter" width="250">商品信息</th>
                                    <th width="100">单价</th>
                                    <th width="100">数量</th>
                                    <th width="100">售后</th>
                                    <th width="100">涨价或优惠</th>
                                    <th width="100">小计</th>
                                    <th width="100">订单来源</th>
                                    <th width="170">优惠</th>
                                </tr>
                            </thead>
                        </table>
                        <table class="mb10">
                            <tbody>
                                <tr>
                                    <td width="750">
                                        <asp:Repeater ID="rptItemList" runat="server">
                                            <ItemTemplate>
                                                <table class="tbProductList" style="margin-bottom: 0; margin-top: -1px;">
                                                    <tbody>
                                                        <tr>
                                                            <td width="450">
                                                                <div class="listInfoLeft">
                                                                    <div class="orderInfolist clearfix">
                                                                        <div class="orderImg fl clearfix">
                                                                            <div class="img fl">
                                                                                <input type="hidden" name="itemtotalprice" value="<%#Eval("Type").ToString()=="0"?((decimal.Parse(Eval("ItemListPrice").ToString())*(int.Parse(Eval("Quantity").ToString()))).ToString("")):"0" %>" />
                                                                                
                                                                                <input type="hidden" name="adjustedcommssion" value="<%#Eval("Type").ToString()=="0"?(decimal.Parse(Eval("ItemAdjustedCommssion").ToString())*(-1)).ToString(""):"0" %>" />
                                                                                <input type="hidden" name="discountAverage" value="<%#Eval("Type").ToString()=="0"?(decimal.Parse(Eval("discountAverage").ToString())).ToString("F2"):"0" %>" />
                                                                                <Hi:ListImage runat="server" DataField="ThumbnailsUrl" Width="60" Height="60" />
                                                                            </div>
                                                                            <div class="imgInfo fl">
                                                                                <p class="setColor"><a href="/ProductDetails.aspx?productId=<%#Eval("ProductId") %>" target="_blank"><%#Eval("ItemDescription") %></a></p>
                                                                                <p><%#Eval("SKUContent") %></p>
                                                                            </div>
                                                                        </div>
                                                                        <p class="fl orderP"><%#Eval("Type").ToString()=="0"? "￥"+Eval("ItemListPrice","{0:f2}"):Eval("PointNumber")+"积分" %></p>
                                                                        <p class="fl orderP"><%# Eval("Quantity") %></p>
                                                                    </div>
                                                                </div>
                                                            </td>
                                                            <td width="100"><span style="color: red"><%# Eval("OrderItemsStatus").ToString().Trim() == Hidistro.Entities.Orders.OrderStatus.ApplyForRefund.ToString() ? "申请退款" : Eval("OrderItemsStatus").ToString().Trim() == Hidistro.Entities.Orders.OrderStatus.ApplyForReturns.ToString() ? "申请退货" : Eval("OrderItemsStatus").ToString().Trim() == Hidistro.Entities.Orders.OrderStatus.Refunded.ToString() ? "已退款" : Eval("OrderItemsStatus").ToString().Trim() == Hidistro.Entities.Orders.OrderStatus.Returned.ToString() ? "已退货" : ""%></span></td>
                                                            <td width="100"><%#(decimal.Parse(Eval("ItemAdjustedCommssion").ToString())>0?"-":"")+"￥"+(decimal.Parse(Eval("ItemAdjustedCommssion").ToString())).ToString("F2").Trim('-')%></td>
                                                            <td width="100" id="showitemprice"></td>
                                                    </tbody>
                                                </table>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </td>
                                    <td width="270" class="border">
                                        <div class="listInfoRight clearfix" style="padding: 0;">
                                            <div class="shopSource fl">
                                                <p>
                                                    <asp:Literal ID="litSiteName" runat="server"></asp:Literal>
                                                </p>
                                            </div>
                                            <div class="operation fr">
                                                <%--<p>积分抵现：-￥1.00</p>
                                                <p>2周年专享优惠券：-￥3.00</p>
                                                <p>满30减1元：-￥1.00</p>
                                                <p>满2件包邮</p>--%>
                                                <asp:Literal ID="litActivityShow" runat="server"></asp:Literal>
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                        <table width="100%">
                            <tr>
                                <td width="250"></td>
                                <td width="100"></td>
                                <td width="100"></td>
                                <td width="100"></td>
                                <td width="100" class="aligin">实收款：</td>
                                <td width="100" class="color">￥<Hi:FormatedMoneyLabel
                                    ID="lblorderTotalForRemark" runat="server" /><br />
                                    含运费￥<asp:Literal ID="litFreight" runat="server" /><%--<br />
                                    2000积分--%></td>
                                <td width="100"></td>
                                <td width="170"></td>
                            </tr>
                        </table>
                    </div>

                    <asp:Literal ID="litCommissionInfo" runat="server"></asp:Literal>
                    <%--                    <div class="commissionInfo mb20">
                        <h3>佣金信息</h3>
                        <div class="commissionInfoInner">
                            <p class="mb5"><span>上二级分销商：</span>-</p>
                            <p class="mb5"><span>上一级分销商：</span>pen的店<i>￥0.00</i></p>
                            <div class="clearfix">
                                <p><span>成交店铺：</span>依依小铺<i>￥1.00</i></p>
                                <p><span>店铺总额：</span><i>￥1.00</i></p>
                            </div>
                        </div>
                    </div>--%>
                    <asp:Repeater ID="rptRefundList" runat="server" OnItemDataBound="rptRefundList_ItemDataBound">
                        <HeaderTemplate>
                            <div class="commissionInfo mb20">
                                <h3>退款信息</h3><a id="returnInfo" name="returnInfo"> </a>
                                <div>
                                    <table style="width: 100%;">
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr>
                                <td style="vertical-align: middle;">

                                    <div class="orderInfolist clearfix">
                                        <div class="orderImg fl clearfix" style="padding-left: 8px">
                                            <div class="img fl">
                                                <Hi:ListImage runat="server" DataField="ThumbnailsUrl" Width="60" Height="60" />
                                            </div>
                                            <div class="imgInfo fl">
                                                <p class="setColor"><a href="/ProductDetails.aspx?productId=<%#Eval("ProductId") %>" target="_blank"><%#Eval("ItemDescription") %></a></p>
                                                <p><%#Eval("SKUContent") %></p>
                                            </div>
                                        </div>
                                    </div>
                                </td>
                                <td style="padding: 15px; width: 30%; vertical-align: middle;">
                                    <p class="mb5">
                                        <span>退款帐号：</span>
                                        <%#Server.HtmlEncode(Eval("Account").ToString()) %>
                                    </p>
                                    <p class="mb5">
                                        <span>退款理由：</span><asp:Literal ID="litComments" runat="server"></asp:Literal>
                                        <%#Server.HtmlEncode(Eval("Comments").ToString()) %>
                                    </p>
                                    <p class="mb5"><span>退款金额：</span><span class="cssModify"><i style="color: red; font-weight: bold; font-style: normal;">￥<%#Eval("RefundMoney","{0:F2}") %></i><%--<a href="javascript:;" class="ml5 cssLinkModify" style="display:none;" id="linkModify" runat="server" title="修改退款金额">修改</a>--%></span>
                                        <span class="cssCancelModify" style="display:none"><input type="text" value="<%#Eval("RefundMoney","{0:F2}") %>" name="txtRefundMoney" class="form-control resetSize inputw100 inl" maxlength="8" /><input type="button" value="确定" class="ml5 btn btn-primary btn-xs cssModifyRefundMoney" pid="<%#Eval("ProductId") %>" /><input type="button" value="取消" class="cssbtnCancelModify ml5 btn btn-default btn-xs" /></span>
                                    </p>
                                </td>
                                <td style="border-left: 1px solid #ccc; padding: 15px; vertical-align: middle; width: 395px;">
                                    <p><span>如果未发货，可以点击同意来退款给买家。</span></p>
                                    <p><span>如果已发货，请与买家联系确认退货事宜，达成一致后再视情况处理。</span></p>
                                    <p>
                                      <%--  <input type="button" class="btn btn-success btn-sm inputw100" value="同意退款" id="btnAgree" runat="server" returnsid='<%#Eval("ReturnsId") %>' skuid='<%#Eval("skuid") %>' productid='<%#Eval("productid") %>' money='<%#Eval("RefundMoney","{0:F2}") %>' onclick="AgreeReturnFun($(this).attr('productid'), $(this).attr('skuid'), $(this).attr('money'), $(this).attr('returnsid'))" />
                                        <input type="button" class="btn btn-danger btn-sm inputw100" value="拒绝退款" id="btnRefuce" runat="server" returnsid='<%#Eval("ReturnsId") %>' skuid='<%#Eval("skuid") %>' productid='<%#Eval("productid") %>' onclick="RefuseReturnFun($(this).attr('productid'),$(this).attr('skuid'),$(this).attr('returnsid'))" />--%>
                                        <asp:Label ID="lblIsAgree" runat="server" CssClass="btn-danger" Text="已同意退款" Visible="false"></asp:Label>
                                        <%#Eval("AdminRemark").ToString().Trim().Length>0?"["+Server.HtmlEncode(Eval("AdminRemark").ToString().Trim())+"]":"" %>
                                    </p>
                                </td>
                            </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                            </table>
                        </div>
                            </div>
                        </FooterTemplate>
                    </asp:Repeater>

                </div>
                <div role="tabpanel" class="tab-pane" id="profile">
                    <div class="set-switch">
                        <p class="row">
                            <span class="col-xs-2 alignR">收货地址:</span>
                            <span class="col-xs-10">
                                <asp:Literal ID="lblOriAddress" runat="server"></asp:Literal></span>
                        </p>
                        <p class="row" id="pNewAddress" runat="server">
                            <span class="col-xs-2 alignR">新收货地址:</span>
                            <span class="col-xs-10">
                                <asp:Literal ID="litAddress" runat="server"></asp:Literal></span>
                        </p>
                        <p class="row">
                            <span class="col-xs-2 alignR">配送方式:</span>
                            <span class="col-xs-10">
                                <asp:Literal ID="litModeName" runat="server" Text="-"></asp:Literal></span>
                        </p>
                        <p class="row">

                            <span class="col-xs-2 alignR">物流公司:</span>
                            <span class="col-xs-10">
                                <asp:Literal ID="litCompanyName" runat="server" Text="-"></asp:Literal></span>
                        </p>
                        <p class="row">
                            <span class="col-xs-2 alignR">快递单号:</span>
                            <span class="col-xs-10" id="numberId">
                                <asp:Literal ID="litShipOrderNumber" runat="server" Text="-"></asp:Literal></span>
                        </p>
                        <div id="pLoginsticInfo" runat="server" class="row" visible="false">
                            <span class="col-xs-2 alignR">快递单物流信息:</span>
                            <span id="expressInfo" class="col-xs-10" style="color:#C60">正在加载中....<br /> <a href="http://www.kuaidi100.com" target="_blank"  style="color:Red;">此物流信息由快递100提供</a></span>
                        </div>
                    </div>
                </div>
            </div>
        </div>


        <div class="modal fade" id="AgreeReturn">
            <div class="modal-dialog">
                <div class="modal-content form-horizontal">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">同意退款</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-horizontal">
                            <div class="form-group">
                                <label for="inputPassword3" class="col-xs-3 control-label">退款金额：</label>
                                <asp:TextBox ID="txtMoney" CssClass="form-control inputw200 inl" runat="server"></asp:TextBox> 元
                            </div>
                            <div class="form-group">
                                <label for="inputPassword3" class="col-xs-3 control-label">备注：</label>
                                <asp:TextBox ID="txtMemo" runat="server" TextMode="MultiLine" Rows="2" Columns="20" CssClass="form-control" Height="90" Width="320"></asp:TextBox>
                                <label for="inputPassword3" class="col-xs-3 control-label"></label>
                                <small>同意退款以后不可恢复，是否继续？</small>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <asp:Button ID="btnAgreeConfirm" runat="server" CssClass="btn btn-success" Text="确定" OnClick="btnAgreeConfirm_Click" OnClientClick="return HiConform('<strong>您正在执行确认退款操作！</strong><p>同意退款以后不可恢复，是否继续？</p>',this);" />
                            <button type="button" class="btn btn-default" data-dismiss="modal">取消</button>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="modal fade" id="RefuseReturn">
            <div class="modal-dialog">
                <div class="modal-content form-horizontal">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">拒绝退款</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-horizontal">
                            <div class="form-group">
                                <label for="inputPassword3" class="col-xs-3 control-label">原因：</label>
                                <asp:TextBox ID="txtAdminMemo" runat="server" TextMode="MultiLine" Rows="2" Columns="20" CssClass="form-control" Height="90" Width="320"></asp:TextBox>
                                <label for="inputPassword3" class="col-xs-3 control-label"></label>
                                <small>拒绝退款以后不可恢复，是否继续？</small>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <asp:Button ID="btnRefuseConfirm" runat="server" CssClass="btn btn-success" Text="确定" OnClick="btnRefuseConfirm_Click" OnClientClick="return RefuseConfirm(this);" />
                            <button type="button" class="btn btn-default" data-dismiss="modal">取消</button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <!--编辑备注信息-->


        <div class="modal fade" id="RemarkOrder">
            <div class="modal-dialog">
                <div class="modal-content form-horizontal">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">修改备注</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group isordersbatch">
                            <label class="col-xs-3 control-label">订单编号：</label><div class="col-xs-8 exitpa">
                                <asp:Literal ID="spanOrderId" runat="server" />
                            </div>
                        </div>
                        <div class="form-group isordersbatch">
                            <label class="col-xs-3 control-label">成交时间：</label><div class="col-xs-8 exitpa">
                                <Hi:FormatedTimeLabel runat="server" ID="lblorderDateForRemark" />
                            </div>
                        </div>
                        <div class="form-group isordersbatch">
                            <label class="col-xs-3 control-label">订单实收款(元)：</label><div class="col-xs-8 exitpa">
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-xs-3 control-label" style="padding-top: 0px; line-height: 22px"><em>*</em>标志：</label>
                            <Hi:OrderRemarkImageRadioButtonList
                                runat="server" ID="orderRemarkImageForRemark" Width="238" />
                        </div>
                        <div class="form-group">
                            <label class="col-xs-3 control-label">备注：</label><asp:TextBox ID="txtRemark" TextMode="MultiLine" runat="server" Width="300" Height="50" />
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



        <!--删除确认-->
        <div class="modal fade" id="deleteConfirm">
            <div class="modal-dialog">
                <div class="modal-content form-horizontal">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">删除确认</h4>
                    </div>
                    <div class="modal-body">
                           注意：彻底删除订单后，为了订单相关统计数据的准确性，你可以<br />
                           &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;选择<span style="color: #c9302c">“删除并更新”</span>，系统删除订单后自动更新当天的统计数据<br />
                           &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;选择<span style="color: #c9302c">“直接删除”</span>，系统直接删除订单但不会更新已有统计数据
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="btnDeleteAndUpdateData" runat="server" CssClass="btn btn-success" BackColor="#c9302c" BorderColor="#c9302c"  Text="删除并更新" OnClientClick="return HiConform('<strong>订单删除</strong><p>彻底删除后的订单将不能恢复，请确认是否继续？</p>',this)" />
                        <asp:Button ID="btnDelete" runat="server" CssClass="btn btn-success" BackColor="#c9302c" BorderColor="#c9302c" Text="直接删除" OnClientClick="return HiConform('<strong>订单删除</strong><p>彻底删除后的订单将不能恢复，请确认是否继续？</p>',this)" />
                        <button type="button" class="btn btn-default" data-dismiss="modal">取消</button>
                    </div>
                </div>
            </div>
        </div>

        <!--修改配送方式-->
        <div class="modal fade" id="setShippingMode">
            <div class="modal-dialog">
                <div class="modal-content form-horizontal">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">修改配送方式</h4>
                    </div>
                    <div class="modal-body">
                        <p><em>*</em>请选择新的配送方式:<%--<Hi:ShippingModeDropDownList runat="server" ID="ddlshippingMode" CssClass="form-control inl autow ml5" />--%></p>
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="btnMondifyShip" runat="server" CssClass="btn btn-success" Text="修改配送方式" />
                        <button type="button" class="btn btn-default" data-dismiss="modal">关闭</button>
                    </div>
                </div>
            </div>
        </div>

        <!--修改支付方式-->
        <div id="setPaymentMode" style="display: none;">
            <div class="frame-content">
                <p><span class="frame-span frame-input130">请选择新的支付方式:<em>*</em></span><Hi:PaymentDropDownList runat="server" ID="ddlpayment" /></p>
            </div>
        </div>


        <div style="display: none">
            <asp:Button ID="btnMondifyPay" runat="server" CssClass="submit_DAqueding" Text="修改支付方式" />
        </div>
    </form>

    <script type="text/javascript">
        $(document).ready(function(){
            $(".tbProductList").each(function () {
                var itemval = parseFloat($(this).find("input[name='itemtotalprice']").val());
                var itemadd = parseFloat($(this).find("input[name='adjustedcommssion']").val());
                var discountAverage= parseFloat($(this).find("input[name='discountAverage']").val());
                var html = (itemval + itemadd-discountAverage).toFixed(2);
                $(this).find("td[id='showitemprice']").html("￥"+html);
            });

            <%if(Hidistro.Core.Globals.RequestQueryStr("type")=="showlogistic"){%>
            ShowLogistic();
            <%}%>
            /*修改价格事件*/
            $(".cssLinkModify,.cssbtnCancelModify").click(function(){
                var obj=this;
                if($(obj).parent().parent().find(".cssModify").is(":hidden")){
                    $(obj).parent().parent().find(".cssCancelModify").hide();
                    $(obj).parent().parent().find(".cssModify").show();
                }else{
                    $(obj).parent().parent().find(".cssCancelModify").show();
                    $(obj).parent().parent().find(".cssModify").hide();
                }
            })
            $(".cssLinkModify").show();
            $("input[name='txtRefundMoney']").blur(function () {
                var temp = $(this).val();
                var t = temp.charAt(0);
                if (temp == "") {
                    temp = "0";
                } else {
                    if ('' != temp.replace(/\d{1,}\.{0,1}\d{0,}/, '')) {
                        temp = temp.match(/\d{1,}\.{0,1}\d{0,}/) == null ? '' : temp.match(/\d{1,}\.{0,1}\d{0,}/);
                    }
                    if (temp == "") {
                        temp = "0";
                    }
                }
                $(this).val(Number(temp).toFixed(2));
            })
            $(".cssModifyRefundMoney").click(function(){
                var price=$(this).parent().find('input[name="txtRefundMoney"]').val();
                var oid=$("#ctl00_ContentPlaceHolder1_hdfOrderID").val();
                var pid=$(this).attr("pid");
                var data="posttype=modifyRefundMondy&price="+price+"&oid="+oid+"&pid="+pid;
                $.ajax({
                    type: "post",
                    url: "orderdetails.aspx",
                    data: data,
                    dataType: "json",
                    success: function (json) {
                        if(json.type=="1"){
                            ShowMsgAndReUrl("价格修改成功！",true,"OrderDetails.aspx?OrderId="+oid);
                        }else{
                            ShowMsg(json.tips,false);
                        }
                    }
                });
            })
            
            var orderId = "<%=orderId%>";//"201509069413702";// 
            var hasNewKey = $("#ctl00_ContentPlaceHolder1_hdHasNewKey").val();
            if (orderId && document.getElementById("ctl00_ContentPlaceHolder1_pLoginsticInfo")) {
                var expressData = getExpressData(orderId);
                var numberId = $("#numberId").html().trim();
                var html = '<table>';
                var expressUrl = $("#ctl00_ContentPlaceHolder1_hdExpressUrl").val();
                var companyCode = $("#ctl00_ContentPlaceHolder1_hdCompanyCode").val();
                var url = encodeURIComponent(window.location.href);
                if (expressData.message == "ok") {
                    if (hasNewKey == "1") {//付费接口通过订阅获取本地信息
                        var result = expressData.type;
                        if (result == "1") {//区分类型是为了两个接口返回的json数据结构不一样
                            var data = expressData.content.lastResult.data;
                            for (var i = 0; i < data.length; i++) {
                                html += '<tr><td style=\"width:180px\">' + data[i].time + '</td>\
                             <td>' + data[i].context + '</td>';
                                html += '</tr>';
                            }
                        }
                        else {//付费接口通过api获取信息
                            var message = expressData.content.message;
                            if (message == "ok") {
                                var data = expressData.content.data;
                                for (var i = 0; i < data.length; i++) {
                                    html += '<tr><td style=\"width:180px\">' + data[i].time + '</td>\
                             <td>' + data[i].context + '</td>';
                                    html += '</tr>';
                                }
                            }
                            else {
                                expressUrl = expressUrl.replace("{numberId}", numberId).replace("{companyCode}", companyCode).replace("{callbackurl}", url);
                                html += '<tr><td><a href="' + expressUrl + '" target="_blank">查看物流</a></td></tr>';
                            }
                        }
                    }
                    else {//免费接口
                        var message = expressData.content.message;
                        if (message == "ok") {
                            var data = expressData.content.data;
                            for (var i = 0; i < data.length; i++) {
                                html += '<tr><td style=\"width:180px\">' + data[i].time + '</td>\
                             <td>' + data[i].context + '</td>';
                                html += '</tr>';
                            }
                        }
                        else {
                            expressUrl = expressUrl.replace("{numberId}", numberId).replace("{companyCode}", companyCode).replace("{callbackurl}", url);
                            html += '<tr><td><a href="' + expressUrl + '" target="_blank">查看物流</a></td></tr>';
                        }
                    }
                }
                html += '<tr><td ><a href="http://www.kuaidi100.com" target="_blank" id="power" runat="server" visible="false" style="color:Red;">此物流信息由快递100提供</a></td></tr> '
                html += '</table>';
                $('#expressInfo').html(html);
            }
        })
        
        function getExpressData(orderId) {
            var url = '/API/VshopProcess.ashx';
            var expressData;
            $.ajax({
                type: "get",
                url: url,
                data: { action: 'Logistic', orderId: orderId },
                dataType: "json",
                async: false,
                success: function (data) {
                    expressData = data;
                }
            });
            return expressData;
        }
        function RefuseConfirm(obj){
            var adminRemark=$.trim($("#ctl00_ContentPlaceHolder1_txtAdminMemo").val());
            if(adminRemark==""){
                ShowMsg("请填写拒绝原因！",false);
                return false;
            }else{
                return HiConform('<strong>您正在执行拒绝退款操作！</strong><p>拒绝退款以后不可恢复，是否继续？</p>',obj);
            }
        }
        var formtype = "";
        function ValidationCloseReason() {
            var reason = document.getElementById("ctl00_contentHolder_ddlCloseReason").value;
            if (reason == "请选择关闭的理由") {
                ShowMsg("请选择关闭的理由",false);
                return false;
            }
            setArryText("ctl00_contentHolder_ddlCloseReason", reason);
            return true;
        }
        function ModifyMemo() {            
            $radioId = $("input[type='radio'][name='ctl00$ContentPlaceHolder1$orderRemarkImageForRemark']:checked")[0];
            if ($radioId == null || $radioId == "undefined") {
                ShowMsg('请选择标志图标', false);
                return false;
            }
            return true;
        }
        function ValidationPayment() {
            var payment = document.getElementById("ctl00_contentHolder_ddlpayment").value;
            if (payment == "") {
                ShowMsg("请选择支付方式",false);
                return false;
            }
            setArryText("ctl00_contentHolder_ddlpayment", payment);
            return true;
        }
         
        function ValidationShippingMode() {
            var shipmode = document.getElementById("ctl00_contentHolder_ddlshippingMode").value;
            if (shipmode == "") {
                ShowMsg("请选择配送方式",false);
                return false;
            }
            setArryText("ctl00_contentHolder_ddlshippingMode", shipmode);
            return true;
        }

        //备注弹出框
        function ShowRemarkOrder() {
            arrytext = null;
            formtype = "remark";
            
            $(".isordersbatch").hide();
            //DialogShow("订单备注", 'orderrmark', 'RemarkOrder', 'ctl00_contentHolder_btnRemark');
            $('#RemarkOrder').modal('toggle').children().css({
                width: '520px',
                height: '180px'
            })
            $("#RemarkOrder").modal({ show: true });
        }
        /*查看物流*/
        function ShowLogistic(){
            $('#myTab li:eq(1) a').tab('show') 
        }
        //删除订单
        function ShowDeleteOrder() {
            $('#deleteConfirm').modal('toggle').children().css({
                width: '520px',
                height: '260px'
            })
            $("#deleteConfirm").modal({ show: true });
        }

        //发货
        function ShowSend() {
            var orderId = "<%=orderId %>";
            DialogFrame("SendOrderGoods.aspx?OrderId=" + orderId, '订单发货', 750, 220)
        }
        function checkClose(){
            var objSel=$("#ctl00_ContentPlaceHolder1_ddlCloseReason").val();
            if("请选择关闭的理由"==objSel){
                ShowMsg(objSel,false);
                return false;
            }
            return true;
        }

        //修改支付方式
        function UpdatePaymentMode() {
            arrytext = null;
            formtype = "paytype";
            DialogShow("修改支付方式", 'paytype', 'setPaymentMode', 'ctl00_contentHolder_btnMondifyPay');
        }


        //修改配送方式
        function UpdateShippingMode() {
            arrytext = null;
            formtype = "shipptype";
            //DialogShow("修改配送方式", 'updateship', 'setShippingMode', 'ctl00_contentHolder_btnMondifyShip');
            $('#setShippingMode').modal('toggle').children().css({
                width: '520px',
                height: '180px'
            })
            $("#setShippingMode").modal({ show: true });
        }

        function validatorForm() {
            switch (formtype) {
                case "remark":
                    $radioId = $("input[type='radio'][name='ctl00$contentHolder$orderRemarkImageForRemark']:checked")[0];
                    if ($radioId == null || $radioId == "undefined") {
                        ShowMsg('请先标记备注',false);
                        return false;
                    }
                    setArryText($radioId.id, "true");
                    setArryText("ctl00_contentHolder_txtRemark", $("#ctl00_contentHolder_txtRemark").val());
                    break;
                case "shipptype":
                    return ValidationShippingMode();
                    break;
                case "closeorder":
                    return ValidationCloseReason();
                    break;
                case "paytype":
                    return ValidationPayment();
                    break;
                case "changeorder":
                    if ($("#ctl00_contentHolder_shippingAddress_txtpost").val().replace(/\s/g, "") == "") {
                        ShowMsg("发货单号不允许为空！",false);
                        return false;
                    }
                    setArryText("ctl00_contentHolder_shippingAddress_txtpost", $("#ctl00_contentHolder_shippingAddress_txtpost").val());
                    break;
            };
            return true;
        }
        function CloseFrameWindow() {
            var win = art.dialog.open.origin;
            win.location.reload();
        }
        /*同意退款*/
        function AgreeReturnFun(productid,skuid,money,returnsid){
            $("#ctl00_ContentPlaceHolder1_txtMoney").val(money)
            $("#ctl00_ContentPlaceHolder1_hdProductID").val(productid);
            $("#ctl00_ContentPlaceHolder1_hdSkuID").val(skuid);
            $("#ctl00_ContentPlaceHolder1_hdReturnsId").val(returnsid);
            $('#AgreeReturn').modal('toggle').children().css({
                width: '520px',
                height: '260px'
            })
            $("#AgreeReturn").modal({ show: true });
        }
        /*拒绝退款*/
        function RefuseReturnFun(productid, skuid, returnsid) {
            $("#ctl00_ContentPlaceHolder1_hdProductID").val(productid);
            $("#ctl00_ContentPlaceHolder1_hdSkuID").val(skuid);
            $("#ctl00_ContentPlaceHolder1_hdReturnsId").val(returnsid);
            $('#RefuseReturn').modal('toggle').children().css({
                width: '520px',
                height: '260px'
            })
            $("#RefuseReturn").modal({ show: true });
        }
    </script>
</asp:Content>
